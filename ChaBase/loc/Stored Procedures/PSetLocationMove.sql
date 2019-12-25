create PROCEDURE [loc].[PSetLocationMove](@rootId int, @nodeToMove int , @newParentNode int)
AS
BEGIN
	declare @maxLevelSpace tinyint, @levelCount tinyint, @nodeToMoveLevel int, @levelIncrement int, @maxLevelTmp int 
	declare @maxLevel tinyint = (select MaxLvl from loc.LocationMaster where LId = @rootId);
	declare @parentLevel int = (select Lvl from Location where LId = @newParentNode);
	declare @locationName nvarchar(255) = (select LocationName from Location where LId = @nodeToMove);
	

	declare @lastChild table(LId int, Lvl tinyint);
	--declare @customerLocation table(LId int);
	declare @Tmp table(LId int, LocationName nvarchar(255), PLId int, Lvl tinyint, IsGroup bit);
	with LocationCTE(LId, LocationName, PLId, Lvl, IsGroup) 
	as
	(
		select LId, LocationName, PLId, Lvl, IsGroup from Location where LId = @nodeToMove
		union all
		select a.LId, a.LocationName, a.PLId, a.Lvl, a.IsGroup
		from Location a
		inner join
		LocationCTE b
		on a.PLId = b.LId
	) 

	insert into @Tmp
	select LId, LocationName, PLId, Lvl, IsGroup from LocationCTE
	
	insert into @lastChild
	select LId, Lvl from @Tmp where Lvl = @maxLevel

	--insert into @customerLocation
	--select LocationId from CustCompany where LocationId in (select LId from @lastChild);
	
	--insert into @customerLocation
	--select LocationId from CustIndividual where LocationId in (select LId from @lastChild);

	set @maxLevelSpace = @maxLevel - @parentLevel
	set @levelCount = (select Count(distinct(Lvl)) from @Tmp);
	set @nodeToMoveLevel = (select Lvl from @Tmp where LId = @nodeToMove);
	set @levelIncrement = @parentLevel - @nodeToMoveLevel;
	set @maxLevelTmp = (select distinct(max(Lvl)) from @Tmp)
	
	declare @error nvarchar(max) = 'Cannot move ' ;
	
	if @maxLevelSpace < @levelCount
	begin
		set @error = @error + cast((select LocationName from loc.Location where LId = @nodeToMove) as nvarchar(50)) + ' because it exceeds the maximum level allowed.';
		raiserror(@error,16,1); 
		return 
	end

	--if @nodeToMoveLevel > @parentLevel and @maxLevelTmp = @maxLevel and exists (select * from @customerLocation)
	--begin
	--	set @error = @error + cast((select LocationName from Location where LId = @nodeToMove) as nvarchar(50))+ ' because it or one of its child is being referenced by a customer.';
	--	raiserror(@error,16,1);
	--	return
	--end

	declare @xLID int, @xPLID int, @xLvl tinyint
	declare CurX cursor for
	select LId, PLId, Lvl from loc.Location where LId = @newParentNode
	open CurX
	fetch from CurX into @xLId, @xPLId, @xLvl 
	while @@FETCH_STATUS = 0
	begin 
		update Location set PLId = @xLId, Lvl = @xLvl+1 where LId = @nodeToMove
		declare @yLID int
		declare CurY cursor for
		select LId from @Tmp where LId <> @nodeToMove
		open CurY
		fetch from CurY into @yLID 
		while @@FETCH_STATUS = 0
		begin
			update Location set Lvl = Lvl + @levelIncrement + 1 where LId = @yLID

			if (select Lvl from Location where LId = @yLID) = @maxLevel
				update Location set IsGroup = 0 where LId = @yLID;

			--else if not exists (select * from @customerLocation)
			--	update Location set IsGroup = 1 where LId in (select LId from @lastChild);

			fetch next from CurY into @yLID
		end
		close CurY
		deallocate CurY

		fetch next from CurX into @xLId, @xPLId, @xLvl
	end
	close CurX
	deallocate CurX

	--declare @message varchar(200);
	--if @@ERROR = 1 
	--begin
	--	set @message = 'ERROR! Location ' + (select LocationName from Location where LId = @nodeToMove) + ' Could Not Move.';
	--	raiserror(@message,16,1);
	--end
	--else
	--begin
	--	set @message = (select LocationName from Location where LId = @nodeToMove) + ' Moved Successfully!';
	--	raiserror(@message,16,1);
	--end
END
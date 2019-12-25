create PROCEDURE [loc].[PSetCreateLocationInSelectedLvl] (@CurrentRoot int, @LvlToCreate int, @LocationName nvarchar(255))
As
BEGIN
	if @LvlToCreate = 1
		begin
			raiserror ('ERROR!!! Cannot Create Location Because Root Level Cannot be Preceeded.', 16, 1)
			return
		end
	
	declare @MaxLvl tinyint
	set @MaxLvl = (select MaxLvl from loc.LocationMaster where LId = @CurrentRoot)

	declare @Tmp table (LId int, LocationName nvarchar(255), PLId int, Lvl tinyint, IsGroup bit);
	
	with LocationCTE (LId, LocationName, PLId, Lvl, IsGroup)
	as
	(	
		select LId, LocationName, PLId, Lvl , IsGroup from loc.[Location] where LId = @CurrentRoot
		union all	
		select a.LId, a.LocationName, a.PLId, a.Lvl , a.IsGroup 
		from [Location] a
		inner join LocationCTE b on a.PLId = b.LId
	)

	insert into @Tmp
	select Lid, LocationName, PLId, Lvl, IsGroup from LocationCTE 
	
	declare @yLId int, @yLvl tinyint
	
	declare CurY cursor for
	select LId, Lvl from @Tmp where Lvl > @LvlToCreate
	open CurY 
	fetch from CurY into @yLId, @yLvl
	while @@FETCH_STATUS = 0
	begin
		update loc.[Location] set Lvl = @yLvl + 1 where LId = @yLId

		fetch next from CurY into @yLId, @yLvl
	end
	close CurY
	deallocate CurY

	declare @xLId int, @xPLId int, @xLvl tinyint

	declare CurX cursor for
	select LId, PLId, Lvl from @Tmp where Lvl = @LvlToCreate order by PLId
	open CurX
	fetch from CurX into @xLId, @xPLId, @xLvl
	while @@FETCH_STATUS = 0
	begin
		declare @Id int		
		insert into loc.[Location] values (@LocationName, @xPLId, @xLvl, 1)
		set @Id = @@IDENTITY
		
		update loc.Location set PLId = @Id, Lvl = @xLvl + 1 where LId = @xLId
		fetch next from CurX into @xLId, @xPLId, @xLvl
	end		 
	close CurX
	deallocate CurX
		
	update loc.LocationMaster set MaxLvl = @MaxLvl + 1 where LId = @CurrentRoot
END
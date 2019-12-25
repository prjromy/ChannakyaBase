CREATE PROCEDURE [loc].[PSetRemoveLocationByLvl] (@CurrentRoot int, @LvlToRemove int)
As 
Begin
	declare @MaxLvl tinyint
	set @MaxLvl = (select MaxLvl from loc.LocationMaster where LId = @CurrentRoot)

	if @LvlToRemove = 1 or @LvlToRemove = @MaxLvl
	Begin
		raiserror ('Cannot Remove Root or Last Level Nodes.',16,1)
		return
	end

	declare @Tmp table (LId int, LocationName nvarchar(255), PLId int, Lvl tinyint, IsGroup bit);
	
	with LocationCTE (LId, LocationName, PLId, Lvl, IsGroup)
	as
	(	
		select LId, LocationName, PLId, Lvl , IsGroup from loc.[Location] where LId = @CurrentRoot
		union all	
		select a.LId, a.LocationName, a.PLId, a.Lvl , a.IsGroup 
		from loc.[Location] a
		inner join LocationCTE b on a.PLId = b.LId
	)

	insert into @Tmp
	select Lid, LocationName, PLId, Lvl, IsGroup from LocationCTE 

	declare @xLId int, @xPLId int, @xLvl tinyint

	declare CurX cursor for
	select LId, PLId, Lvl from @Tmp where Lvl = @LvlToRemove order by PLId
	open CurX
	fetch from CurX into @xLId, @xPLId, @xLvl 
	while @@FETCH_STATUS = 0
	begin
		delete from loc.[Location] where Lid = @xLId

		declare @yLId int
		declare CurY cursor for 
		select LId from @Tmp where PLId = @xLId
		open CurY
		fetch from CurY into @yLId 
		while @@FETCH_STATUS = 0
		begin			
			update loc.[Location] set PLId = @xPLId, Lvl = @xLvl where LId = @yLid
			fetch next from CurY into @yLId
		end
		close CurY
		deallocate CurY

		fetch next from CurX into @xLId, @xPLId, @xLvl 
	end
	close CurX
	deallocate CurX

	declare @zLId int
	declare CurZ cursor for
	select LId from @Tmp where Lvl > @LvlToRemove+1
	open CurZ
	fetch from CurZ into @zLid
	while @@FETCH_STATUS = 0
	begin
		update loc.[Location] set Lvl = Lvl-1 where LId = @zLId
		fetch next from CurZ into @zLId
	end
	close CurZ
	deallocate CurZ

	update loc.LocationMaster set MaxLvl = MaxLvl - 1 where LId = @CurrentRoot
END
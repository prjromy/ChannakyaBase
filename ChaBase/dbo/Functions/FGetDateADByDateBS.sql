Create Function [dbo].[FGetDateADByDateBS](@DateBS varchar(10))
returns smalldatetime
as
begin
Declare @NYear int
Declare @NMonth int
Declare @NDay int

set @NYear = dbo.fgetBSYear(@DateBS) -- convert(int, left(@Datebs,4))
set @NMonth = dbo.FGetBSMonth(@DateBS) --convert(int, substring(@DateBs,6,2))
set @NDay = dbo.FGetBSDay(@DateBS) --convert(int, right(@DateBs, 2))
	if not exists(select nyear from ndated where nyear = @nyear)
		goto lblExit
	if @NMonth not between 1 and 12
		goto lblexit 

	if (select dbo.FGetDaysInMonth(@NYear, @NMonth)) < @NDay
		goto lblexit

	Declare @StartDate smalldatetime
	Declare @TotDay int
	
	select @StartDate = startdate from BSADCal where NYear = @NYear
	select @TotDay = isnull(max(days),0) from dbo.fgetnmonthcal(@NYear) where mnth < @NMonth
	set @TotDay = @TotDay + @NDay -1
	return @StartDate + @Totday
lblExit:
	return -1
	
end
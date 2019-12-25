Create Function [dbo].[FGetDateAD](@NYear int, @NMonth int, @NDay int)
returns smalldatetime
as
begin
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
	set @TotDay = @TotDay + @NDay-1
	return @StartDate + @Totday
lblExit:
	return -1
	
end
CREATE Function [dbo].[FGetDateBS](@EngDate  date)
returns varchar(10)
as 
begin
	Declare @StartDateYear smalldatetime
	Declare @NYear as int
	Declare @NMonth as int
	Declare @NDay as int
	Declare @DaysInMonth int
	Declare @DF int

	set @DF = cast( isnull((select PValue from lg.ParamValue where PId=35),0) as int)

	Declare @TotDays int


	Select @NYear = Nyear, @StartDateYear = Startdate from bsadcal where @Engdate between startdate and enddate
	--select @Nyear

	set @TotDays =  datediff(day,@StartDateYear, @EngDate) + 1
	--Select @Totdays

	select @NMonth = min(mnth), @DaysInMonth  = isnull(min(Days),0) from dbo.fgetnmonthcal(@NYear) where days >=@Totdays --group by days
	--select @NMonth, @DaysInMonth


	--select @NDay = @TotDays - isnull(max(days),0) , @DaysInMonth = @DaysInMonth - isnull(max(Days),0) from dbo.fgetnmonthcal(@NYear) where days < @Totdays --group by days
	select @NDay = @TotDays - isnull(max(days),0)  from dbo.fgetnmonthcal(@NYear) where days < @Totdays --group by days
	--select @NDay, @DaysInmonth
	
	return dbo.FGetFormatedDate(@NYear, @NMonth, @NDay, @DF)
end
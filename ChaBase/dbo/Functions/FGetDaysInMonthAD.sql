CREATE Function [dbo].[FGetDaysInMonthAD](@year int, @month int)
returns
int
as
begin
	Declare @DIM int
	Declare @Date Date  = cast( CAST(@Year AS varchar) + '-' + CAST(@Month AS varchar) + '-' + CAST(1 AS varchar) as DateTime)
	
	set @DIM = datediff(day, dateadd(day, 1-day(@date), @date),
              dateadd(month, 1, dateadd(day, 1-day(@date), @date)))
    return @DIM
end
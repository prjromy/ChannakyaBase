
CREATE Function [dbo].[FGetDaysInMonth](@Year Int, @Month Int)
returns int
as 
begin
	Declare @DaysInMonth int
	if @Month = 1
		select  @DaysInMonth = M1 from NDateD where nyear = @Year
	else if @Month = 2
		select  @DaysInMonth = M2 from NDateD where nyear = @Year
	else if @Month = 3
		select  @DaysInMonth = M3 from NDateD where nyear = @Year
	else if @Month =4
		select  @DaysInMonth = M4 from NDateD where nyear = @Year
	else if @Month =5
		select  @DaysInMonth = M5 from NDateD where nyear = @Year
	else if @Month =6
		select  @DaysInMonth = M6 from NDateD where nyear = @Year
	else if @Month =7
		select  @DaysInMonth = M7 from NDateD where nyear = @Year
	else if @Month = 8
		select  @DaysInMonth = M8 from NDateD where nyear = @Year
	else if @Month = 9
		select  @DaysInMonth = M9 from NDateD where nyear = @Year
	else if @Month = 10
		select  @DaysInMonth = M10 from NDateD where nyear = @Year
	else if @Month = 11
		select  @DaysInMonth = M11 from NDateD where nyear = @Year
	else if @Month = 12
		select  @DaysInMonth = M12  from NDateD where nyear = @Year
	else 
		select @DaysInMonth = 0
		
	return  @DaysInMonth


end
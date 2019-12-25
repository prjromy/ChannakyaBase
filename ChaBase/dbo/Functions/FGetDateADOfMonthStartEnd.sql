create Function [dbo].[FGetDateADOfMonthStartEnd](@NDate varchar(10), @IsStart bit)
returns date
as
begin
	Declare @BSYear int set @BSYear = dbo.FGetBSYear(@NDate)
	Declare @BSMonth int set @BSMonth = dbo.FGetBSMonth(@NDate)
	Declare @DateAD date
	if (@IsStart = 1)
	begin
		set @DateAD = dbo.FGetDateAD(@BSYear, @BSMonth, 1)
	end
	else
	begin
		Declare @DaysInMonth int set @DaysInMonth = dbo.FGetDaysInMonth(@BSyear, @BSMonth)
			set @DateAD = dbo.FGetDateAD(@BSYear, @BSMonth, @DaysInMonth)
	end
	return @DateAD
	
end
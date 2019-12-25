CREATE FUNCTION [dbo].[FGetNMonthCal](@Year int)
RETURNS @Temp TABLE (Mnth int, Days Int)

begin
	Declare @M1 int
	Declare @M2 int
	Declare @M3 int
	Declare @M4 int
	Declare @M5 int
	Declare @M6 int
	Declare @M7 int
	Declare @M8 int
	Declare @M9 int
	Declare @M10 int
	Declare @M11 int
	Declare @M12 int
	Declare @Days int
	set @Days = 0
	 DECLARE CURR cursor LOCAL FOR
	SELECT   m1,m2,m3,m4,m5,m6,m7,m8,m9,m10,m11,m12 from ndated where nyear = @Year
	open curr
	FETCH  CURR INTO @m1 , @M2, @m3, @m4, @m5, @m6, @m7, @m8, @m9, @m10, @m11, @m12
	
		WHILE @@FETCH_STATUS=0
			BEGIN
				set @Days = @Days + @M1
				insert @temp select 1, @Days

				set @Days = @Days + @M2
				insert @temp select 2, @Days
				
				set @Days = @Days + @M3
				insert @temp select 3, @Days

				set @Days = @Days + @M4
				insert @temp select 4, @Days

				set @Days = @Days + @M5
				insert @temp select 5, @Days

				set @Days = @Days + @M6
				insert @temp select 6, @Days

				set @Days = @Days + @M7
				insert @temp select 7, @Days

				set @Days = @Days + @M8
				insert @temp select 8, @Days

				set @Days = @Days + @M9
				insert @temp select 9, @Days

				set @Days = @Days + @M10
				insert @temp select 10, @Days

				set @Days = @Days + @M11
				insert @temp select 11, @Days

				set @Days = @Days + @M12
				insert @temp select 12, @Days

				FETCH  NEXT FROM CURR INTO @m1 , @M2, @m3, @m4, @m5, @m6, @m7, @m8, @m9, @m10, @m11, @m12
			END
	CLOSE  CURR
	DEALLOCATE  CURR
	return  
end
CREATE FUNCTION [dbo].[FGetDateInMonthDays](@startdate DATETIME, @enddate DATETIME)
RETURNS @daysandmonth table(Month int,  Days int)
AS

BEGIN
 
DECLARE @tempdate DATETIME, @years INT, @months INT, @days INT
SELECT @tempdate = @startdate

SELECT @years = DATEDIFF(YEAR, @tempdate, @enddate) - CASE WHEN (MONTH(@startdate) > MONTH(@enddate)) OR (MONTH(@startdate) = MONTH(@enddate) AND DAY(@startdate) > DAY(@enddate)) THEN 1 ELSE 0 END
SELECT @tempdate = DATEADD(YEAR, @years, @tempdate)

SELECT @months = DATEDIFF(MONTH, @tempdate, @enddate) - CASE WHEN DAY(@startdate) > DAY(@enddate) THEN 1 ELSE 0 END

SELECT @tempdate = DATEADD(MONTH, @months, @tempdate)

SELECT @days = DATEDIFF(DAY, @tempdate, @enddate)


insert into @daysandmonth values((@months+12* @years ) ,@days )
 
return
end
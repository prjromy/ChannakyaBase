CREATE FUNCTION [dbo].[FGETNDAYS](@THISYEAR INT)
RETURNS INT
---MARKED ENCRYPTION 
as 
begin
Declare @TotalDays int
 select @TotalDays =   isnull(SUM(M1) + SUM(M2) + SUM(M3) + SUM(M4) + SUM(M5) + SUM(M6) + SUM(M7) + SUM(M8) + SUM(M9) + SUM(M10) + SUM(M11) + SUM(M12) ,0)
                     FROM         NDateD where nYear < @ThisYear  

return  @TotalDays
end
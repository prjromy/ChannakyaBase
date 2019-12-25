create FUNCTION [fin].[GetAcNo]
	(@IACCNO  INT)
	 
RETURNS NVARCHAR(20)
---MARKED ENCRYPTION 
AS
begin
	RETURN (Select accno from fin.adetail where iaccno=@iaccno)
end
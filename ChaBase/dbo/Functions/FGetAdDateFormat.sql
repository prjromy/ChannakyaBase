
-- =============================================
cREATE FUNCTION  [dbo].[FGetAdDateFormat]()

RETURNS varchar(10)
AS
BEGIN
Declare @Result varchar(10)
	set @Result=(select PValue from lg.ParamValue where pid=34)
	
	RETURN @Result
END
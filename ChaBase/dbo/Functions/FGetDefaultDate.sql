
-- =============================================
create FUNCTION  [dbo].[FGetDefaultDate]()

RETURNS varchar(10)
AS
BEGIN
Declare @Result varchar(10)
	set @Result=(select PValue from lg.ParamValue where pid=6011)
	
	RETURN @Result
END
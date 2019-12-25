
-- =============================================
CREATE FUNCTION  [dbo].[FGetBsDateFormat]()

RETURNS varchar(10)
AS
BEGIN
Declare @Result varchar(10)
	set @Result=(select PValue from lg.ParamValue where pid=35)
	
	RETURN @Result
END
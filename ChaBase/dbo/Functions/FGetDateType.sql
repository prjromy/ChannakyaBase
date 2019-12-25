-- =============================================
CREATE FUNCTION  [dbo].[FGetDateType]()

RETURNS varchar(10)
AS
BEGIN
Declare @Result varchar(10)
	set @Result=(select PValue from lg.ParamValue where pid=36)
	
	RETURN @Result
END
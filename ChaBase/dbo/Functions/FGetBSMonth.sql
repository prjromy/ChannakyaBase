
CREATE Function [dbo].[FGetBSMonth](@NDate varchar(10))
returns tinyint
AS
BEGIN
	Declare @Result tinyint
	Declare @DTFormat tinyint
	--set @DTFormat = 2 --ISNULL((Select Number from FGetParamValueTB() where ParamId = 4),1)
	set @DTFormat = cast( isnull((select PValue from lg.ParamValue where pid=35),0) as int)
	IF @DTFormat =2  --ddMMyyyy
		set @Result = cast(substring(@NDate,4,2) as tinyint)
	else if @DTFormat = 1 --MMddyyyy
		set @Result = cast(left(@NDate,2) as tinyint)
	else if @DTFormat = 4 --yyyyddMM
		set @Result = cast(Right(@NDate, 2) as tinyint)
	else if @DTFormat = 3 --YYYYMMdd
		set @Result = cast(substring(@NDate,6,2) as tinyint)

	RETURN @Result
END
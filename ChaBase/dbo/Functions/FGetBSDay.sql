CREATE Function [dbo].[FGetBSDay](@NDate varchar(10))
returns tinyint
AS
BEGIN
	Declare @Result tinyint
	Declare @DTFormat tinyint
	set @DTFormat = cast( isnull((select PValue from LG.ParamValue where PId=35),0) as int)
	--set @DTFormat = 2 --ISNULL((Select Number from FGetParamValueTB() where ParamId = 4),1)
	IF @DTFormat = 2  --ddMMyyyy
		set @Result = cast(left(@NDate,2) as tinyint)
	else if @DTFormat = 1 --MMddyyyy
		set @Result = cast(substring(@NDate,4,2) as tinyint)
	else if @DTFormat = 4 --yyyyddMM
		set @Result = cast(substring(@NDate, 6,2) as tinyint) --Right(@NDate, 2)
	else if @DTFormat = 3 --YYYYMMdd
		set @Result = cast (right(@NDate,2) as tinyint)
	

	RETURN @Result
END


CREATE Function [dbo].[FGetBSYear](@NDate varchar(10))
returns int
AS
BEGIN
	Declare @Result int
	Declare @DTFormat tinyint
	--set @DTFormat = 2 -- ISNULL((Select Number from FGetParamValueTB() where ParamId = 4),1)
	set @DTFormat = cast( isnull((select PValue from LG.ParamValue where PId=35),0) as int)
	IF @DTFormat = 2    --ddMMyyyy
		set @Result = cast(right(@Ndate, 4) as int)
	else if @DTFormat = 1 --MMddyyyy
		set @Result = cast(right(@Ndate, 4) as int)
	else if @DTFormat = 4 --yyyyddMM
		set @Result = cast(Left(@Ndate, 4) as int)
	else if @DTFormat = 3 --YYYYMMdd
		set @Result = cast(Left(@Ndate, 4) as int)
	

	RETURN @Result
END
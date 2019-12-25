

CREATE Function [dbo].[FGetFormatedDate](@Year smallint, @Month tinyint, @Day tinyint, @DF tinyint)
returns varchar(10)
as
begin
	--Declare @DF as tinyint
	--set @DF = 1 --ISNULL((Select Number from FGetParamValueTB() where ParamId = 4),1)

	Declare @Result varchar(10)

	Declare @CMnth varchar(2)
	declare @CDay varchar(2)	
	Declare @CYr varchar(4)


	if @Month <10
		begin
		set @CMnth = '0'+ ltrim(str(@Month))
		end
	else
		begin
		set @CMnth = ltrim(str(@Month))
		end

	if @Day < 10 
		begin
		set @CDay = '0' + ltrim(str(@Day))
		end
	else
		begin
		set @CDay = ltrim(str(@Day))
		end

	set @cYr = ltrim(str(@Year))

	
	if @DF  = 2 --dd-mm-yyyy
		set @Result =  @CDay + '-' + @CMnth+ '-' + @cYr	 
	else if @DF = 1 --mm-dd-yyyy
		set @Result = @CMnth  +'-'+ @CDay + '-' +  @cYr
	else if @DF = 4 --yyyy-dd-mm
		set @Result  =  @cYr +'-'+  @CDay + '-' +@CMnth
	else if @DF = 3 --yyyy-mm-dd	
		set @Result  = @cYr +'-'+ @CMnth + '-' +@CDay
	
	return  isnull(@Result,'')
end
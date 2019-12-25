CREATE FUNCTION [fin].[FGetGETAVLBALANCE]

	(

		@IACCNO AS INT,

		@TEDATE AS DATETIME,

		@STYPE AS INT --@STYPE = 0 FOR DEPOSIT, @STYPE = 1 FOR LOAN 

	)



RETURNS MONEY  

---MARKED ENCRYPTION  

AS

BEGIN

	declare @A AS money    --good balance

	declare @B as money	 

	declare @C as money	--freeze amount

 
	declare @X as money 	--limit amount

	declare @avlbal as money

	declare @isovdacc as bit

	 


 

	select @A=isnull(Fin.GETBALANCE(@iaccno,@SType),0) 

	 
set @B=isnull((Select TOP 1 Bal from fin.ABal WHERE IAccno=@IACCNO AND Flag=4),0)-isnull((Select TOP 1 Bal from fin.ABal WHERE IAccno=@IACCNO AND Flag=5),0)

	 
	if @stype = 1

		set @C = isnull((select appAmt from fin.adrLimit where iaccno = @iaccno),0) - @A 

	select @isovdacc=P.HASOVERDRAW FROM fin.PRODUCTDETAIL AS P INNER JOIN fin.ADETAIL AS A  ON A.PID=P.PID WHERE A.IACCNO=@iaccno

	

	if @isovdacc=1

	begin

		select @X=APPAMT FROM fin.ADRLIMIT AS AD INNER JOIN fin.Adur AS A ON AD.IACCNO=A.IACCNO WHERE (AD.IACCNO=@iaccno)  AND (A.MATDAT >=@tedate)

		select @X=isnull(@X,0)
		select @avlbal = (@A +@X) - @B

	end

	else
		select  @avlbal = @A -@B
	if @stype=1
		return @c
	RETURN @avlbal 

END
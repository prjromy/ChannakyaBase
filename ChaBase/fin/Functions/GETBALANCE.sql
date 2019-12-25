CREATE FUNCTION [fin].[GETBALANCE]

	(@IACCNO INT,@stype int) 

RETURNS MONEY

---MARKED ENCRYPTION  

AS

BEGIN

	DECLARE @balamt AS money

	declare @tdate smalldatetime
	if(@stype=1)

			Select @BalAmt = Bal From fin.ADetail Where IAccNo = @IAccNo
			
	else
	set @BalAmt =(Select TOP 1 Bal from fin.ABal WHERE IAccno=@IACCNO AND Flag=2) + (Select TOP 1 Bal from fin.ABal WHERE IAccno=@IACCNO AND Flag=3)-(Select TOP 1 Bal from fin.ABal WHERE IAccno=@IACCNO AND Flag=1)

	RETURN isnull(@balamt,0)

END
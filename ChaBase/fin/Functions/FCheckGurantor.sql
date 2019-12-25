CREATE FUNCTION [fin].[FCheckGurantor] (@IACCNO INT)











RETURNS @CHKGUARANTOR TABLE (AccountNumber VARCHAR(25),BlockedAmt MONEY,DisplayMsg BIT,IsLoanee BIT,IsPercent bit)



---MARKED ENCRYPTION 



as



Begin


	Insert Into @CHKGUARANTOR (AccountNumber,BlockedAmt,DisplayMsg,IsLoanee,IsPercent)

	SELECT     adetail.accno as AccountNumber, IsNull(fin.Guarantor.BlockedAmt,0) as BlockedAmt , DisplayMsg, 0 as IsLoanee,IsPercent

	FROM         fin.Guarantor INNER JOIN
	                    fin.ADetail ON fin.Guarantor.LIaccno = fin.ADetail.IAccno

	WHERE     (fin.Guarantor.GIaccno = @IACCNO) and adetail.accstate = 1 and Status=1

 
Return



End
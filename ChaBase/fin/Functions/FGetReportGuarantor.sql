CREATE FUNCTION	[fin].[FGetReportGuarantor](@BRCHID INT=null)

RETURNS TABLE

---MARKED ENCRYPTION 

As

Return

(

SELECT ADetail.Accno AS LoanNo, ADetail.Aname AS LoaneeName, ADetail_1.Accno AS GuarantorAccNo,ADetail_1.Aname AS GuarantorName, 

	Guarantor.BlockedAmt,ADrLimit.AppAmt LoanApproved, ADetail.Bal AS LoanBalance, ADetail_1.Bal AS AccBalance

FROM  fin.Guarantor 

INNER JOIN fin.ADetail ON Guarantor.LIaccno = ADetail.IAccno 

INNER JOIN fin.ADetail ADetail_1 ON Guarantor.GIaccno = ADetail_1.IAccno

INNER JOIN fin.ADrLimit On ADetail.IAccNo = ADrLimit.IAccNo

Where ADetail.Brchid =coalesce(@BRCHID,ADetail.Brchid))
CREATE FUNCTION Fin.FGetReportOverdraftBalance (
	@TDATE SMALLDATETIME
	,@BRCHID INT
	)
RETURNS TABLE
	---MARKED ENCRYPTION   
AS
RETURN

SELECT ProductDetail.PName
	,ADetail.Accno
	,ADetail.Aname
	,ADetail.Bal
	,'ODInterest' = IsNull((
			SELECT IsNull(Sum(IntLog.IntCal), 0)
			FROM fin.IntLog
			WHERE (IntLog.IAccNo = AODSetUp.IAccNo)
				AND (IntLog.Valued = 10)
			), 0) + IsNull((
			SELECT IsNull((Sum(Amt)), 0)
			FROM fin.Aadjustmnt
			WHERE Aadjustmnt.PID = 10
				AND Aadjustmnt.IAccNo = AODSetUp.IAccNo
			), 0) + IsNull((
			SELECT IsNull((Sum(CrAmt)), 0) - IsNull((Sum(DrAmt)), 0)
			FROM fin.AODIntReceivable
			WHERE AODIntReceivable.IAccNo = AODSetUp.IAccNo
			), 0)
	,ADrLimit.AppAmt
	,ADur.ValDat
	,ADur.Month
	,ADur.Days
	,ADur.MatDat
	,Company.TDate AS 'Transaction Date'
FROM fin.AODSetUp
INNER JOIN fin.ADetail ON AODSetUp.IAccno = ADetail.IAccno
INNER JOIN fin.ProductDetail ON ADetail.PID = ProductDetail.PID
INNER JOIN fin.ADrLimit ON ADetail.IAccno = ADrLimit.IAccNo
INNER JOIN fin.SchmDetails ON ProductDetail.SDID = SchmDetails.SDID
INNER JOIN fin.ADur ON ADetail.IAccno = ADur.IAccno
INNER JOIN lg.Company ON ADetail.BrchID = Company.CompanyId
WHERE (ADetail.BrchID = @BrchID)
	AND (ADur.IsOD = 1)
	AND (
		(ADur.MatDat >= @TDate)
		OR (ADetail.Bal < 0)
		)
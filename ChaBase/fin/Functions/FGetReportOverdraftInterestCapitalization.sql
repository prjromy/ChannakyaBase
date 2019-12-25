CREATE FUNCTION fin.FGetReportOverdraftInterestCapitalization (
	@fromDate SMALLDATETIME
	,@toDate SMALLDATETIME
	,@branchId INT
	)
RETURNS TABLE
AS
RETURN

SELECT ADetail.Accno
	,ADetail.Aname
	,AODIntReceivable.Tdate
	,AODIntReceivable.DRAmt CapAmt
	,ADetail_1.Accno AS TransferedTo
	,AMTrns.tno
	,AODIntReceivable.vno
FROM fin.AODIntReceivable
INNER JOIN fin.ADetail ON AODIntReceivable.Iaccno = ADetail.IAccno
INNER JOIN trans.AMTrns ON AODIntReceivable.TNo = AMTrns.tno
INNER JOIN fin.ADetail ADetail_1 ON AMTrns.IAccno = ADetail_1.IAccno
WHERE (AODIntReceivable.DRAmt > 0)
	AND ADetail.Brchid = @branchId
	AND AODIntReceivable.Tdate BETWEEN @fromDate
		AND @toDate
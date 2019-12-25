--select * from  fin.FGetReportLoanBalanceTillDate('2017/01/01',null)
CREATE FUNCTION [fin].[FGetReportLoanBalanceTillDate] (
	@TDATE SMALLDATETIME
	,@BRANCHID INT = NULL
	)
	---MARKED ENCRYPTION  
RETURNS TABLE
AS
RETURN

SELECT TT.SDName
	,TT.PName
	,TT.PID
	,Sum(TT.PrinOut) 'PrinOut'
	,Sum(TT.IntOut) 'IntOut'
	,Sum(TT.POnPrOut) 'POnPrOut'
	,Sum(TT.POnIOut) 'POnIOut'
	,Sum(TT.IOnIOut) 'IOnIOut'
FROM (
	SELECT SDName
		,PName
		,ADetail.PID
		,T.*
	FROM fin.SchmDetails
	INNER JOIN fin.ProductDetail ON SchmDetails.SDID = ProductDetail.SDID
	INNER JOIN fin.ADetail ON ProductDetail.PID = ADetail.PID
	INNER JOIN (
		SELECT IAccNo
			,PrinOut
			,IntOut
			,POnPrOut
			,POnIOut
			,IOnIOut
		FROM fin.FGetReportAllTotalLoanBalanceByTransDate(@TDATE)
		) T ON ADetail.IAccNo = T.IAccNo
	WHERE fin.SchmDetails.SType = 1
		AND ADetail.BrchId = coalesce(@branchId, ADetail.BrchID)
	) TT
GROUP BY TT.SDName
	,TT.PName,TT.PID
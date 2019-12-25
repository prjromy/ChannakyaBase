CREATE FUNCTION [fin].[FGetReportDepositBalanceTillDate] (
	@TDATE SMALLDATETIME
	,@BRCHID INT
	
	)
RETURNS TABLE
	---MARKED ENCRYPTION 
AS
RETURN

SELECT T.IAccNo
	,IsNull(Sum(T.IntToExp), 0) 'IntToExp'
	,IsNull(Sum(T.IntToCap), 0) 'IntToCap'
	,IsNull(Sum(T.Bal), 0)+IsNull(Sum(T.ShadowCr), 0)-IsNull(Sum(T.ShadowDr), 0) 'Bal'
	,ADetail.accno as AccountNumber
	,ADetail.Aname as AccountName
	
FROM (
	SELECT X.IAccNo
		,Sum(X.IntCal + X.Amt - X.IntExpenses) 'IntToExp'
		,Sum(0) 'IntToCap'
		,Sum(0) 'Bal'
		,Sum(0) ShadowCr
		,Sum(0) ShadowDr
	FROM (
		SELECT IAccNo
			,Sum(IntCal) 'IntCal'
			,Sum(0) 'Amt'
			,Sum(0) 'IntExpenses'
			,Sum(0) ShadowCr
		,Sum(0) ShadowDr
		FROM fin.IntLog
		WHERE 
		TDate <= @TDate AND 
		IntLog.Valued IN (
				0
				,5
				)
		GROUP BY IAccNo
		
		UNION ALL
		
		SELECT IAccNo
			,Sum(0) 'IntCal'
			,Sum(Amt) 'Amt'
			,Sum(0) 'IntExpenses'
			,Sum(0) ShadowCr
		,Sum(0) ShadowDr
		FROM fin.AAdjustmnt
		WHERE
		TDate <= @TDate AND 
		AAdjustmnt.pid IN (
				0
				,5
				)
		GROUP BY IAccNo
		
		UNION ALL
		
		SELECT IAccNo
			,Sum(0) 'IntCal'
			,Sum(0) 'Amt'
			,Sum(IntAmt) 'IntExpenses'
			,Sum(0) ShadowCr
		,Sum(0) ShadowDr
		FROM fin.AIntExpenses
		WHERE TDate <= @TDate
		GROUP BY IAccNo
		) X
	GROUP BY X.IAccNo
	union all
	SELECT Y.IAccNo
		,Sum(0) 'IntToExp'
		,Sum(Y.IntExpenses - Y.IntAmt - Y.IntAmtPybl) 'IntToCap'
		,Sum(0) 'Bal'
		,Sum(0) ShadowCr
		,Sum(0) ShadowDr
	FROM (
		SELECT IAccNo
			,Sum(IntAmt) 'IntExpenses'
			,Sum(0) 'IntAmt'
			,Sum(0) 'IntAmtPybl'
			,Sum(0) ShadowCr
		,Sum(0) ShadowDr
		FROM fin.AIntExpenses
	WHERE TDate <= @TDate
		GROUP BY IAccNo
		
		UNION ALL
		
		SELECT 'IAccNo' = CASE 
				WHEN FIAccNo IS NOT NULL
					THEN FIAccNo
				ELSE TIAccNo
				END
			,Sum(0) 'IntExpenses'
			,Sum(IntAmt) 'IntAmt'
			,Sum(0) 'IntAmtPybl'
			,Sum(0) ShadowCr
		,Sum(0) ShadowDr
		FROM fin.AIntCap
	  WHERE VDate <= @TDate
		GROUP BY FIAccNo
			,TIAccNo
		
		UNION ALL
		
		SELECT IAccNo
			,Sum(0) 'IntExpenses'
			,Sum(0) 'IntAmt'
			,Sum(IntAmt) 'IntAmtPybl'
			,Sum(0) ShadowCr
		   ,Sum(0) ShadowDr
		FROM fin.AIntPayable
		WHERE TDate <= @TDate
		GROUP BY IAccNo
		) Y
	GROUP BY Y.IAccNo
	union all
	select IAccno, sum(0) as IntExpenses,SUM(0) as IntAmt,bal ,Sum(0) ShadowCr
		,Sum(0) ShadowDr  from fin.ABal where Flag=3 group by IAccno,bal
			union all
		select IAccno, sum(0) as IntExpenses,SUM(0) as IntAmt,Sum(0) as balance ,bal as ShadowCr
		,Sum(0) ShadowDr  from fin.ABal where Flag=2 group by IAccno,bal
		union all
		select IAccno, sum(0) as IntExpenses,SUM(0) as IntAmt,Sum(0) as balance ,Sum(0) as ShadowCr
		,Bal ShadowDr  from fin.ABal where Flag=1 group by IAccno,bal
	)T 

	INNER JOIN fin.ADetail ON ADetail.IAccNo = T.IAccNo
	inner join fin.ProductDetail pd on pd.PID=ADetail.PID
	inner join fin.SchmDetails sd on pd.SDID=pd.SDID
    WHERE ADetail.Brchid=coalesce(@BRCHID,ADetail.Brchid) and sd.SType=0
	
					
GROUP BY T.IAccNo,ADetail.accno,ADetail.Aname
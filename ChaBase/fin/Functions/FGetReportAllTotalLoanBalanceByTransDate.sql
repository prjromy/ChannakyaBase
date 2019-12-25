CREATE FUNCTION [fin].[FGetReportAllTotalLoanBalanceByTransDate] (@TDATE SMALLDATETIME)
RETURNS TABLE
	---MARKED ENCRYPTION   
AS
RETURN

SELECT Z.IAccNo
	,Sum(PrinOut) 'PrinOut'
	,Sum(Interest - InterestP) 'IntOut'
	,Sum(POnPr - POnPrP) 'POnPrOut'
	,Sum(POnI - POnIP) 'POnIOut'
	,Sum(IOnI - IOnIP) 'IOnIOut'
FROM (
	SELECT IAccNo
		,Sum(PrinOut) 'PrinOut'
		,Sum(INT) 'Interest'
		,Sum(PonPr) 'POnPr'
		,Sum(PonI) 'POnI'
		,Sum(IonI) 'IOnI'
		,Sum(InterestP) 'InterestP'
		,Sum(POnPrP) 'POnprP'
		,Sum(POnIP) 'POnIP'
		,Sum(IOnIP) 'IOnIP'
	FROM (
		SELECT IAccNo
			,0 'PrinOut'
			,'Int' = Sum(CASE 
					WHEN Valued = 0
						THEN IsNull(IntCal, 0)
					ELSE 0
					END + CASE 
					WHEN Valued = 5
						THEN IsNull(IntCal, 0)
					ELSE 0
					END)
			,'PonPr' = Sum(CASE 
					WHEN Valued = 3
						THEN IsNull(IntCal, 0)
					ELSE 0
					END + CASE 
					WHEN Valued = 8
						THEN IsNull(IntCal, 0)
					ELSE 0
					END)
			,'PonI' = Sum(CASE 
					WHEN Valued = 4
						THEN IsNull(IntCal, 0)
					ELSE 0
					END + CASE 
					WHEN Valued = 9
						THEN IsNull(IntCal, 0)
					ELSE 0
					END)
			,'IonI' = Sum(CASE 
					WHEN Valued = 1
						THEN IsNull(IntCal, 0)
					ELSE 0
					END + CASE 
					WHEN Valued = 6
						THEN IsNull(IntCal, 0)
					ELSE 0
					END)
			,0 'InterestP'
			,0 'POnPrP'
			,0 'POnIP'
			,0 'IOnIP'
		FROM fin.IntLog
		WHERE Iaccno IN (
				SELECT IAccNo
				FROM fin.FGetAccType(1)
				)
			AND TDate <= @TDate
		GROUP BY IAccNo
		
		UNION ALL
		
		SELECT IAccNo
			,0 'PrinOut'
			,'Int' = Sum(CASE 
					WHEN PID = 0
						THEN IsNull(Amt, 0)
					ELSE 0
					END + CASE 
					WHEN PID = 5
						THEN IsNull(Amt, 0)
					ELSE 0
					END)
			,'PonPr' = Sum(CASE 
					WHEN PID = 3
						THEN IsNull(Amt, 0)
					ELSE 0
					END + CASE 
					WHEN PID = 8
						THEN IsNull(Amt, 0)
					ELSE 0
					END)
			,'PonI' = Sum(CASE 
					WHEN PID = 4
						THEN IsNull(Amt, 0)
					ELSE 0
					END + CASE 
					WHEN PID = 9
						THEN IsNull(Amt, 0)
					ELSE 0
					END)
			,'IonI' = Sum(CASE 
					WHEN PID = 1
						THEN IsNull(Amt, 0)
					ELSE 0
					END + CASE 
					WHEN PID = 6
						THEN IsNull(Amt, 0)
					ELSE 0
					END)
			,0 'InterestP'
			,0 'POnPrP'
			,0 'POnIP'
			,0 'IOnIP'
		FROM fin.Aadjustmnt
		WHERE IAccNo IN (
				SELECT IAccNo
				FROM fin.FGetAccType(1)
				)
			AND TDate <= @TDate
		GROUP BY IAccNo
		
		UNION ALL
		
		SELECT IAccNo
			,'PrinOut' = Sum(CASE 
					WHEN PID = 10
						THEN IsNull(Amt, 0)
					ELSE 0
					END - CASE 
					WHEN PID = 13
						THEN IsNull(Amt, 0)
					ELSE 0
					END)
			,0 'Int'
			,0 'PonPr'
			,0 'PonI'
			,0 'IonI'
			,'InterestP' = Sum(CASE 
					WHEN PID = 0
						THEN IsNull(Amt, 0)
					ELSE 0
					END + CASE 
					WHEN PID = 5
						THEN IsNull(Amt, 0)
					ELSE 0
					END)
			,'POnPrP' = Sum(CASE 
					WHEN PID = 3
						THEN IsNull(Amt, 0)
					ELSE 0
					END + CASE 
					WHEN PID = 8
						THEN IsNull(Amt, 0)
					ELSE 0
					END)
			,'POnIP' = Sum(CASE 
					WHEN PID = 4
						THEN IsNull(Amt, 0)
					ELSE 0
					END + CASE 
					WHEN PID = 9
						THEN IsNull(Amt, 0)
					ELSE 0
					END)
			,'IOnIP' = Sum(CASE 
					WHEN PID = 1
						THEN IsNull(Amt, 0)
					ELSE 0
					END + CASE 
					WHEN PID = 6
						THEN IsNull(Amt, 0)
					ELSE 0
					END)
		FROM trans.AMTrns
		INNER JOIN fin.AmTransLoan ON AmTrns.TNo = AmTransLoan.TNo
		WHERE VDate <= @TDate
		GROUP BY IAccNo
		) X
	GROUP BY IAccNo
	) Z
INNER JOIN fin.ADetail ad ON ad.iaccno = z.iaccno
GROUP BY Z.IAccNo
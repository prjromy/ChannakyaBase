CREATE FUNCTION [fin].[FGetReportLoanAccruedInterestListing] (
	@BRCHID INT
	,@TDATE SMALLDATETIME
	)
RETURNS TABLE
	---MARKED ENCRYPTION 
AS
RETURN (
		SELECT *
		FROM (
			SELECT X.IAccNo
				,ADetail.AccNo
				,ADetail.AName
				,Sum(X.MAccured) AS MAccured
				,Sum(X.UMAccured) AS UMAccured
			FROM (
				SELECT IAccNo
					,Sum(IsNull(CalcInt, 0)) - Sum(IsNull(PdInt, 0)) AS MAccured
					,0 AS UMAccured
				FROM fin.ALSch
				WHERE (fin.ALSch.SchDate <= @TDate)
				GROUP BY IAccNo
				
				UNION
				
				SELECT IAccNo
					,0 AS MAccured
					,Sum(IsNull(CalcInt, 0)) - Sum(IsNull(PdInt, 0)) AS UMAccured
				FROM fin.ALSch
				WHERE (
						SchDate > (
							SELECT Max(S1.SchDate)
							FROM fin.ALSch S1
							WHERE (S1.SchDate <= @TDate)
							)
						)
					AND (
						Schdate <= (
							SELECT Max(S2.SchDate)
							FROM fin.ALSch S2
							WHERE (S2.SchDate > @TDate)
							)
						)
				GROUP BY IAccNo
				) X
			INNER JOIN fin.ADetail ON x.IAccNo = ADetail.IAccNo
			WHERE ADetail.AccState <> 2
				AND brchid = @BrchID
			GROUP BY x.IAccNo
				,ADetail.AccNo
				,ADetail.AName
			) Final
		)
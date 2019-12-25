CREATE FUNCTION [fin].[FGetReportInterestLog] (
	@FDATE SMALLDATETIME
	,@TDATE SMALLDATETIME
	,@BRANCHID INT
	,@ISLOAN BIT
	,@ISINT BIT
	)
RETURNS @T1 TABLE (
	TDATE SMALLDATETIME
	,PNAME VARCHAR(100)
	,ACCNO VARCHAR(50)
	,BALANCE DECIMAL(18, 2)
	,RATE DECIMAL(18, 2)
	,FDAY TINYINT
	,INTCAL FLOAT
	,REMARK VARCHAR(100)
	,BRCHID INT
	,IACCNO INT
	)
	---MARKED ENCRYPTION  
AS
BEGIN
	IF @IsLoan = 0 --Deposit------------------------
		INSERT INTO @T1
		SELECT IntLog.Tdate
			,productdetail.pname
			,ADetail.Accno
			,IntLog.Balance
			,IntLog.Rate
			,IntLog.Fday
			,IntLog.Intcal
			,'Remark' = CASE 
				WHEN IntLog.Valued IN (
						0
						,5
						)
					THEN 'Interest'
				ELSE CASE 
						WHEN IntLog.Valued IN (
								1
								,6
								)
							THEN 'Interest on Interest'
						ELSE CASE 
								WHEN IntLog.Valued IN (
										2
										,7
										)
									THEN 'Interest on Other'
								ELSE CASE 
										WHEN IntLog.Valued IN (
												3
												,8
												)
											THEN 'Penalty on Principal'
										ELSE CASE 
												WHEN IntLog.Valued IN (
														4
														,9
														)
													THEN 'Penalty on Interest'
												ELSE cast(IntLog.Valued AS NVARCHAR(25))
												END
										END
								END
						END
				END
			,ADetail.BrchID
			,IntLog.iaccno
		FROM fin.IntLog
		INNER JOIN fin.ADetail ON IntLog.IAccno = ADetail.IAccno
		INNER JOIN fin.ProductDetail ON ADetail.PID = ProductDetail.PID
		INNER JOIN fin.SchmDetails ON ProductDetail.SDID = SchmDetails.SDID
		WHERE (
				IntLog.Tdate BETWEEN @FDate
					AND @TDate
				)
			AND (ADetail.BrchID = @BranchId)
			AND SchmDetails.SType = 0
			AND IntLog.Valued IN (
				0
				,5
				)
		ORDER BY IntLog.Valued
			,SchmDetails.SType
			,ProductDetail.PID
			,ADetail.Accno
			,IntLog.Tdate
	ELSE --LOAn----------------
	IF @IsInt = 1 -----------incase of LOAN INTEREST
		INSERT INTO @T1
		SELECT IntLog.Tdate
			,productdetail.pname
			,ADetail.Accno
			,IntLog.Balance
			,IntLog.Rate
			,IntLog.Fday
			,IntLog.Intcal
			,'Remark' = CASE 
				WHEN IntLog.Valued IN (
						0
						,5
						)
					THEN 'Interest'
				ELSE CASE 
						WHEN IntLog.Valued IN (
								1
								,6
								)
							THEN 'Interest on Interest'
						ELSE CASE 
								WHEN IntLog.Valued IN (
										2
										,7
										)
									THEN 'Interest on Other'
								ELSE CASE 
										WHEN IntLog.Valued IN (
												3
												,8
												)
											THEN 'Penalty on Principal'
										ELSE CASE 
												WHEN IntLog.Valued IN (
														4
														,9
														)
													THEN 'Penalty on Interest'
												ELSE cast(IntLog.Valued AS NVARCHAR(25))
												END
										END
								END
						END
				END
			,ADetail.BrchID
			,IntLog.iaccno
		FROM fin.IntLog
		INNER JOIN fin.ADetail ON IntLog.IAccno = ADetail.IAccno
		INNER JOIN fin.ProductDetail ON ADetail.PID = ProductDetail.PID
		INNER JOIN fin.SchmDetails ON ProductDetail.SDID = SchmDetails.SDID
		WHERE (
				IntLog.Tdate BETWEEN @FDate
					AND @TDate
				)
			AND (ADetail.BrchID = @BranchId)
			AND SchmDetails.SType = 1
			AND IntLog.Valued IN (
				0
				,5
				)
		ORDER BY IntLog.Valued
			,SchmDetails.SType
			,ProductDetail.PID
			,ADetail.Accno
	ELSE ---------incase of LOAN PENALTY
		INSERT INTO @T1
		SELECT IntLog.Tdate
			,productdetail.pname
			,ADetail.Accno
			,IntLog.Balance
			,IntLog.Rate
			,IntLog.Fday
			,IntLog.Intcal
			,'Remark' = CASE 
				WHEN IntLog.Valued IN (
						0
						,5
						)
					THEN 'Interest'
				ELSE CASE 
						WHEN IntLog.Valued IN (
								1
								,6
								)
							THEN 'Interest on Interest'
						ELSE CASE 
								WHEN IntLog.Valued IN (
										2
										,7
										)
									THEN 'Interest on Other'
								ELSE CASE 
										WHEN IntLog.Valued IN (
												3
												,8
												)
											THEN 'Penalty on Principal'
										ELSE CASE 
												WHEN IntLog.Valued IN (
														4
														,9
														)
													THEN 'Penalty on Interest'
												ELSE cast(IntLog.Valued AS NVARCHAR(25))
												END
										END
								END
						END
				END
			,ADetail.BrchID
			,IntLog.iaccno
		FROM fin.IntLog
		INNER JOIN fin.ADetail ON IntLog.IAccno = ADetail.IAccno
		INNER JOIN fin.ProductDetail ON ADetail.PID = ProductDetail.PID
		INNER JOIN fin.SchmDetails ON ProductDetail.SDID = SchmDetails.SDID
		WHERE (
				IntLog.Tdate BETWEEN @FDate
					AND @TDate
				)
			AND (ADetail.BrchID = @BranchId)
			AND SchmDetails.SType = 1
			AND IntLog.Valued NOT IN (
				0
				,5
				)
		ORDER BY IntLog.Valued
			,SchmDetails.SType
			,ProductDetail.PID
			,ADetail.Accno

	RETURN
END
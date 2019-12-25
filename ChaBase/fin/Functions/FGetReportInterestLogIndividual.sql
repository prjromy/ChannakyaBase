CREATE  FUNCTION [fin].[FGetReportInterestLogIndividual] (
	@FDATE SMALLDATETIME
	,@TDATE SMALLDATETIME
	,@IACCNO INT
	,@BRNCHID INT
	)
RETURNS TABLE
	---MARKED ENCRYPTION  
AS
RETURN (
		SELECT x.TDATE
			,x.ACCNO
			,x.Aname AS AccountName
			,cast(x.Balance AS DECIMAL(18, 2)) AS BALANCE
			,cast(x.Rate AS DECIMAL(18, 2)) AS RATE
			,cast(x.FDAY as tinyint) as FDAY
			,x.INTCAL
			,x.REMARK
			,x.PNAME
			,x.BRCHID
			,x.IACCNO
		FROM (
			SELECT IntLog.Tdate
				,ADetail.Accno
				,ADetail.aname
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
								THEN 'IntonInt'
							ELSE CASE 
									WHEN IntLog.Valued IN (
											2
											,7
											)
										THEN 'IntonOther'
									ELSE CASE 
											WHEN IntLog.Valued IN (
													3
													,8
													)
												THEN 'PonPrin'
											ELSE CASE 
													WHEN IntLog.Valued IN (
															4
															,9
															)
														THEN 'PonInt'
													ELSE CASE 
															WHEN IntLog.Valued IN (
																	10
																	,11
																	)
																THEN 'ODIntIncome'
															ELSE cast(IntLog.Valued AS NVARCHAR(25))
															END
													END
											END
									END
							END
					END
				,ADetail.BrchID AS BRCHID
				,IntLog.iaccno AS IACCNO
				,ProductDetail.PName AS PNAME
			FROM fin.IntLog
			INNER JOIN fin.ADetail ON IntLog.IAccno = ADetail.IAccno
			INNER JOIN fin.ProductDetail ON ADetail.PID = ProductDetail.PID
			INNER JOIN fin.SchmDetails ON ProductDetail.SDID = SchmDetails.SDID
			WHERE (
					IntLog.Tdate BETWEEN @Fdate
						AND @TDate
					)
				AND (ADetail.BrchID = @brnchID)
				AND (IntLog.iaccno = @iaccno)
			
			UNION ALL
			
			SELECT t1.TDate
				,t2.AccNo
				,t2.AName
				,0 'Balance'
				,0 'Rate'
				,0 'FDay'
				,t1.Amt 'IntCal'
				,'Remark' = CASE 
					WHEN t1.PID IN (
							0
							,5
							)
						THEN 'Interest'
					ELSE CASE 
							WHEN t1.PID IN (
									1
									,6
									)
								THEN 'IntonInt'
							ELSE CASE 
									WHEN t1.PID IN (
											2
											,7
											)
										THEN 'IntonOther'
									ELSE CASE 
											WHEN t1.PID IN (
													3
													,8
													)
												THEN 'PonPrin'
											ELSE CASE 
													WHEN t1.PID IN (
															4
															,9
															)
														THEN 'PonInt'
													ELSE CASE 
															WHEN t1.PID IN (
																	10
																	,11
																	)
																THEN 'ODIntIncome'
															ELSE cast(t1.PID AS NVARCHAR(25))
															END
													END
											END
									END
							END
					END
				,t2.BrchID 'BrchID'
				,t1.IAccNo
				,ProductDetail.PName AS PNAME
			FROM fin.aadjustmnt t1
			INNER JOIN fin.ADetail t2 ON t1.IAccNo = t2.IAccNo
			INNER JOIN fin.ProductDetail ON t2.PID = ProductDetail.PID
			WHERE (
					t1.TDate BETWEEN @Fdate
						AND @TDate
					)
				AND (t2.BrchID = @brnchID)
				AND (t1.iaccno = @iaccno)
			) X
		)
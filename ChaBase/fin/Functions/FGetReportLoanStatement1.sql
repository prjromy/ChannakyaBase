CREATE FUNCTION [fin].[FGetReportLoanStatement1] (
	@IACCNO INT
	,@FDATE SMALLDATETIME
	,@TDATE DATETIME
	)
RETURNS @STMNTTBL TABLE (
	SNO NUMERIC
	,IAccNo INT
	,TranscDate SMALLDATETIME
	,Particulars NVARCHAR(200)
	,DrPrincipal MONEY
	,OtherDr MONEY
	,Rebate MONEY
	,CrPrincipal MONEY
	,OtherCr MONEY
	,CrPrincInt MONEY
	,CrOtherInt MONEY
	,CrPenalty MONEY
	,Balance NVARCHAR(14)
	,OtherBal NVARCHAR(14)
	,DrCr NVARCHAR(14)
	)
	---MARKED ENCRYPTION 
AS
BEGIN
	DECLARE @Sno NUMERIC
	DECLARE @IIAccNo INT
	DECLARE @VVDate SMALLDATETIME
	DECLARE @PERTI NVARCHAR(200)
	DECLARE @DRAmt MONEY
	DECLARE @CRAMT MONEY
	--DECLARE @BALAMT money 
	DECLARE @notes NVARCHAR(200)
	DECLARE @PPbal NVARCHAR(100)
	DECLARE @OObal NVARCHAR(100)
	DECLARE @Pbal MONEY
	DECLARE @Obal MONEY
	DECLARE @RBT MONEY
	DECLARE @PDR MONEY
	DECLARE @PCR MONEY
	DECLARE @ODR MONEY
	DECLARE @OCR MONEY
	DECLARE @CRPEN MONEY
	DECLARE @CRPINT MONEY
	DECLARE @CROINT MONEY
	DECLARE @TNO MONEY
	DECLARE @DrCr NVARCHAR(12)
	SET @PBAL = 0
	SET @OBAL = 0
	SET @sno = 0

	BEGIN
		SELECT @PDR = sum(PriDr)
			,@ODR = sum(OthrDr)
			,@OCR = sum(othrCr)
			,@PCR = sum(PriCr)
			,@PBAL = isnull(sum(PriDr) - sum(PriCr), 0)
			,@OBAL = isnull(sum(othrCr) - sum(OthrDr), 0)
			,@CRPINT = (sum(intAPd) + sum(intRPd))
			,@CROINT = (sum(IonCAPd) + sum(IonCRPd))
			,@CRPEN = sum(IonIAPd + IonIRPd + PonPrAPd + PonPrRPd + PonIAPd + PonIRPd)
			,@RBT = sum(RbtInt + RbtPen)
		FROM ---  VWALnTra	
			--/*********************
			(
			SELECT AMTrns.iaccno
				,AMTrns.tno
				,trans.AMTrns.vdate
				,PriDr = CASE 
					WHEN EXISTS (
							SELECT tno
							FROM fin.AMTransLoan
							WHERE pid = 10
								AND fin.AMTransLoan.tno = AMTrns.tno
							)
						THEN (
								SELECT SUM(amt)
								FROM fin.AMTransLoan
								WHERE pid = 10
									AND fin.AMTransLoan.tno = AMTrns.tno
								)
					ELSE 0
					END
				,OthrDr = CASE 
					WHEN EXISTS (
							SELECT tno
							FROM fin.AMTransLoan
							WHERE pid = 11
								AND fin.AMTransLoan.tno = AMTrns.tno
							)
						THEN (
								SELECT SUM(amt)
								FROM fin.AMTransLoan
								WHERE pid = 11
									AND fin.AMTransLoan.tno = AMTrns.tno
								)
					ELSE 0
					END
				,PriCr = CASE 
					WHEN EXISTS (
							SELECT tno
							FROM fin.AMTransLoan
							WHERE pid = 13
								AND fin.AMTransLoan.tno = AMTrns.tno
							)
						THEN (
								SELECT SUM(amt)
								FROM fin.AMTransLoan
								WHERE pid = 13
									AND fin.AMTransLoan.tno = trans.AMTrns.tno
								)
					ELSE 0
					END
				,othrCr = CASE 
					WHEN EXISTS (
							SELECT tno
							FROM fin.AMTransLoan
							WHERE pid = 14
								AND fin.AMTransLoan.tno = trans.AMTrns.tno
							)
						THEN (
								SELECT SUM(amt)
								FROM fin.AMTransLoan
								WHERE pid = 14
									AND fin.AMTransLoan.tno = trans.AMTrns.tno
								)
					ELSE 0
					END
				,RbtInt = CASE 
					WHEN EXISTS (
							SELECT tno
							FROM fin.AMTransLoan
							WHERE pid = 12
								AND fin.AMTransLoan.tno = trans.AMTrns.tno
							)
						THEN (
								SELECT SUM(amt)
								FROM fin.AMTransLoan
								WHERE pid = 12
									AND fin.AMTransLoan.tno = trans.AMTrns.tno
								)
					ELSE 0
					END
				,RbtPen = CASE 
					WHEN EXISTS (
							SELECT tno
							FROM fin.AMTransLoan
							WHERE pid = 17
								AND fin.AMTransLoan.tno = trans.AMTrns.tno
							)
						THEN (
								SELECT SUM(amt)
								FROM fin.AMTransLoan
								WHERE pid = 17
									AND fin.AMTransLoan.tno = trans.AMTrns.tno
								)
					ELSE 0
					END
				,intAPd = CASE 
					WHEN EXISTS (
							SELECT tno
							FROM fin.AMTransLoan
							WHERE pid = 0
								AND fin.AMTransLoan.tno = trans.AMTrns.tno
							)
						THEN (
								SELECT SUM(amt)
								FROM fin.AMTransLoan
								WHERE pid = 0
									AND fin.AMTransLoan.tno = trans.AMTrns.tno
								)
					ELSE 0
					END
				,intRPd = CASE 
					WHEN EXISTS (
							SELECT tno
							FROM fin.AMTransLoan
							WHERE pid = 5
								AND fin.AMTransLoan.tno = trans.AMTrns.tno
							)
						THEN (
								SELECT SUM(amt)
								FROM fin.AMTransLoan
								WHERE pid = 5
									AND fin.AMTransLoan.tno = trans.AMTrns.tno
								)
					ELSE 0
					END
				,IonIAPd = CASE 
					WHEN EXISTS (
							SELECT tno
							FROM fin.AMTransLoan
							WHERE pid = 1
								AND fin.AMTransLoan.tno = trans.AMTrns.tno
							)
						THEN (
								SELECT SUM(amt)
								FROM fin.AMTransLoan
								WHERE pid = 1
									AND fin.AMTransLoan.tno = trans.AMTrns.tno
								)
					ELSE 0
					END
				,IonIRPd = CASE 
					WHEN EXISTS (
							SELECT tno
							FROM fin.AMTransLoan
							WHERE pid = 6
								AND fin.AMTransLoan.tno = trans.AMTrns.tno
							)
						THEN (
								SELECT SUM(amt)
								FROM fin.AMTransLoan
								WHERE pid = 6
									AND fin.AMTransLoan.tno = trans.AMTrns.tno
								)
					ELSE 0
					END
				,IonCAPd = CASE 
					WHEN EXISTS (
							SELECT tno
							FROM fin.AMTransLoan
							WHERE pid = 2
								AND fin.AMTransLoan.tno = trans.AMTrns.tno
							)
						THEN (
								SELECT SUM(amt)
								FROM fin.AMTransLoan
								WHERE pid = 2
									AND fin.AMTransLoan.tno = trans.AMTrns.tno
								)
					ELSE 0
					END
				,IonCRPd = CASE 
					WHEN EXISTS (
							SELECT tno
							FROM fin.AMTransLoan
							WHERE pid = 7
								AND fin.AMTransLoan.tno = trans.AMTrns.tno
							)
						THEN (
								SELECT SUM(amt)
								FROM fin.AMTransLoan
								WHERE pid = 7
									AND fin.AMTransLoan.tno = trans.AMTrns.tno
								)
					ELSE 0
					END
				,PonPrAPd = CASE 
					WHEN EXISTS (
							SELECT tno
							FROM fin.AMTransLoan
							WHERE pid = 3
								AND fin.AMTransLoan.tno = trans.AMTrns.tno
							)
						THEN (
								SELECT SUM(amt)
								FROM fin.AMTransLoan
								WHERE pid = 3
									AND fin.AMTransLoan.tno = trans.AMTrns.tno
								)
					ELSE 0
					END
				,PonPrRPd = CASE 
					WHEN EXISTS (
							SELECT tno
							FROM fin.AMTransLoan
							WHERE pid = 8
								AND fin.AMTransLoan.tno = trans.AMTrns.tno
							)
						THEN (
								SELECT SUM(amt)
								FROM fin.AMTransLoan
								WHERE pid = 8
									AND fin.AMTransLoan.tno = trans.AMTrns.tno
								)
					ELSE 0
					END
				,PonIAPd = CASE 
					WHEN EXISTS (
							SELECT tno
							FROM fin.AMTransLoan
							WHERE pid = 4
								AND fin.AMTransLoan.tno = trans.AMTrns.tno
							)
						THEN (
								SELECT SUM(amt)
								FROM fin.AMTransLoan
								WHERE pid = 4
									AND fin.AMTransLoan.tno = trans.AMTrns.tno
								)
					ELSE 0
					END
				,PonIRPd = CASE 
					WHEN EXISTS (
							SELECT tno
							FROM fin.AMTransLoan
							WHERE pid = 9
								AND fin.AMTransLoan.tno = trans.AMTrns.tno
							)
						THEN (
								SELECT SUM(amt)
								FROM fin.AMTransLoan
								WHERE pid = 9
									AND fin.AMTransLoan.tno = trans.AMTrns.tno
								)
					ELSE 0
					END
			FROM trans.AMTrns
			INNER JOIN fin.ADetail ON trans.AMTrns.IAccno = fin.ADetail.IAccno
			INNER JOIN fin.ProductDetail ON fin.ADetail.PID = fin.ProductDetail.PID
			INNER JOIN fin.SchmDetails ON fin.ProductDetail.SDID = fin.SchmDetails.SDID
			WHERE (fin.SchmDetails.SType = 1)
				AND trans.AMTrns.IAccno = @IACcNo
				AND trans.AMTrns.vdate <= @FDATE
			) x
		--**********************/
		WHERE (IAccno = @IACcNo)
			AND (vdate < @FDATE)

		IF @PBAL <> 0
			OR @OBAL <> 0
		BEGIN
			SET @SNO = @SNO + 1
			SET @PERTI = 'Opening Balance'

			IF @PBAL >= 0
			begin
				SET @PPbal = CAST(format(@PBAL, 'N0') AS NVARCHAR(10))  
				set @DrCr= 'Dr'
				end
			ELSE IF @PBAL < 0
			begin
				SET @PPbal = CAST(format(@PBAL, 'N0') AS NVARCHAR(10)) 
				set @DrCr= 'Cr'
				end
			IF @Obal > 0
				begin
				SET @OObal = CAST(format(@OBAL, 'N0') AS NVARCHAR(100)) 
				set @DrCr= 'Dr'
				end
			ELSE IF @Obal < 0
			begin
				SET @OObal = CAST(format(@OBAL, 'N0') AS NVARCHAR(100)) 
				set @DrCr= 'Cr'
				end

			--print 'aa'
			INSERT INTO @STMNTTbl
			SELECT @SNO
				,@IACCNO
				,@VVDate
				,@PERTI
				,@PDR
				,@ODR
				,@Rbt
				,@PCR
				,@OCR
				,@CRPINT
				,@CROINT
				,@CRPEN
				,@PPBAL
				,@OOBAL
				,@DrCr
		END

		DECLARE STMENT CURSOR
		FOR
		SELECT VWALnStm.iaccno
			,VWALnStm.vdate
			,NOTES
			,VWALnStm.PriDr
			,VWALnStm.OthrDr
			,VWALnStm.RbtInt + VWALnStm.RbtPen AS Rbt
			,VWALnStm.PriCr
			,VWALnStm.othrCr
			,(VWALnStm.intAPd + VWALnStm.intRPd) AS CRPINT
			,(VWALnStm.IonCAPd + VWALnStm.IonCRPd) AS CROINT
			,(VWALnStm.IonIAPd + VWALnStm.IonIRPd + VWALnStm.PonPrAPd + VWALnStm.PonPrRPd + VWALnStm.PonIAPd + VWALnStm.PonIRPd) AS CRPEN
		FROM -- VWALnTra
			-------------------
			(
			SELECT trans.AMTrns.iaccno
				,trans.AMTrns.tno
				,trans.AMTrns.vdate
				,PriDr = CASE 
					WHEN EXISTS (
							SELECT tno
							FROM fin.AMTransLoan
							WHERE pid = 10
								AND fin.AMTransLoan.tno = trans.AMTrns.tno
							)
						THEN (
								SELECT SUM(amt)
								FROM fin.AMTransLoan
								WHERE pid = 10
									AND fin.AMTransLoan.tno = trans.AMTrns.tno
								)
					ELSE 0
					END
				,OthrDr = CASE 
					WHEN EXISTS (
							SELECT tno
							FROM fin.AMTransLoan
							WHERE pid = 11
								AND fin.AMTransLoan.tno = trans.AMTrns.tno
							)
						THEN (
								SELECT SUM(amt)
								FROM fin.AMTransLoan
								WHERE pid = 11
									AND fin.AMTransLoan.tno = trans.AMTrns.tno
								)
					ELSE 0
					END
				,PriCr = CASE 
					WHEN EXISTS (
							SELECT tno
							FROM fin.AMTransLoan
							WHERE pid = 13
								AND fin.AMTransLoan.tno = trans.AMTrns.tno
							)
						THEN (
								SELECT SUM(amt)
								FROM fin.AMTransLoan
								WHERE pid = 13
									AND fin.AMTransLoan.tno = trans.AMTrns.tno
								)
					ELSE 0
					END
				,othrCr = CASE 
					WHEN EXISTS (
							SELECT tno
							FROM fin.AMTransLoan
							WHERE pid = 14
								AND fin.AMTransLoan.tno = trans.AMTrns.tno
							)
						THEN (
								SELECT SUM(amt)
								FROM fin.AMTransLoan
								WHERE pid = 14
									AND fin.AMTransLoan.tno = trans.AMTrns.tno
								)
					ELSE 0
					END
				,RbtInt = CASE 
					WHEN EXISTS (
							SELECT tno
							FROM fin.AMTransLoan
							WHERE pid = 12
								AND fin.AMTransLoan.tno = trans.AMTrns.tno
							)
						THEN (
								SELECT SUM(amt)
								FROM fin.AMTransLoan
								WHERE pid = 12
									AND fin.AMTransLoan.tno = trans.AMTrns.tno
								)
					ELSE 0
					END
				,RbtPen = CASE 
					WHEN EXISTS (
							SELECT tno
							FROM fin.AMTransLoan
							WHERE pid = 17
								AND fin.AMTransLoan.tno = trans.AMTrns.tno
							)
						THEN (
								SELECT SUM(amt)
								FROM fin.AMTransLoan
								WHERE pid = 17
									AND fin.AMTransLoan.tno = trans.AMTrns.tno
								)
					ELSE 0
					END
				,intAPd = CASE 
					WHEN EXISTS (
							SELECT tno
							FROM fin.AMTransLoan
							WHERE pid = 0
								AND fin.AMTransLoan.tno = trans.AMTrns.tno
							)
						THEN (
								SELECT SUM(amt)
								FROM fin.AMTransLoan
								WHERE pid = 0
									AND fin.AMTransLoan.tno = trans.AMTrns.tno
								)
					ELSE 0
					END
				,intRPd = CASE 
					WHEN EXISTS (
							SELECT tno
							FROM fin.AMTransLoan
							WHERE pid = 5
								AND fin.AMTransLoan.tno = trans.AMTrns.tno
							)
						THEN (
								SELECT SUM(amt)
								FROM fin.AMTransLoan
								WHERE pid = 5
									AND fin.AMTransLoan.tno = trans.AMTrns.tno
								)
					ELSE 0
					END
				,IonIAPd = CASE 
					WHEN EXISTS (
							SELECT tno
							FROM fin.AMTransLoan
							WHERE pid = 1
								AND fin.AMTransLoan.tno = trans.AMTrns.tno
							)
						THEN (
								SELECT SUM(amt)
								FROM fin.AMTransLoan
								WHERE pid = 1
									AND fin.AMTransLoan.tno = trans.AMTrns.tno
								)
					ELSE 0
					END
				,IonIRPd = CASE 
					WHEN EXISTS (
							SELECT tno
							FROM fin.AMTransLoan
							WHERE pid = 6
								AND fin.AMTransLoan.tno = trans.AMTrns.tno
							)
						THEN (
								SELECT SUM(amt)
								FROM fin.AMTransLoan
								WHERE pid = 6
									AND fin.AMTransLoan.tno = trans.AMTrns.tno
								)
					ELSE 0
					END
				,IonCAPd = CASE 
					WHEN EXISTS (
							SELECT tno
							FROM fin.AMTransLoan
							WHERE pid = 2
								AND fin.AMTransLoan.tno = trans.AMTrns.tno
							)
						THEN (
								SELECT SUM(amt)
								FROM fin.AMTransLoan
								WHERE pid = 2
									AND fin.AMTransLoan.tno = trans.AMTrns.tno
								)
					ELSE 0
					END
				,IonCRPd = CASE 
					WHEN EXISTS (
							SELECT tno
							FROM fin.AMTransLoan
							WHERE pid = 7
								AND fin.AMTransLoan.tno = trans.AMTrns.tno
							)
						THEN (
								SELECT SUM(amt)
								FROM fin.AMTransLoan
								WHERE pid = 7
									AND fin.AMTransLoan.tno = trans.AMTrns.tno
								)
					ELSE 0
					END
				,PonPrAPd = CASE 
					WHEN EXISTS (
							SELECT tno
							FROM fin.AMTransLoan
							WHERE pid = 3
								AND fin.AMTransLoan.tno = trans.AMTrns.tno
							)
						THEN (
								SELECT SUM(amt)
								FROM fin.AMTransLoan
								WHERE pid = 3
									AND fin.AMTransLoan.tno = trans.AMTrns.tno
								)
					ELSE 0
					END
				,PonPrRPd = CASE 
					WHEN EXISTS (
							SELECT tno
							FROM fin.AMTransLoan
							WHERE pid = 8
								AND fin.AMTransLoan.tno = trans.AMTrns.tno
							)
						THEN (
								SELECT SUM(amt)
								FROM fin.AMTransLoan
								WHERE pid = 8
									AND fin.AMTransLoan.tno = trans.AMTrns.tno
								)
					ELSE 0
					END
				,PonIAPd = CASE 
					WHEN EXISTS (
							SELECT tno
							FROM fin.AMTransLoan
							WHERE pid = 4
								AND fin.AMTransLoan.tno = trans.AMTrns.tno
							)
						THEN (
								SELECT SUM(amt)
								FROM fin.AMTransLoan
								WHERE pid = 4
									AND fin.AMTransLoan.tno = trans.AMTrns.tno
								)
					ELSE 0
					END
				,PonIRPd = CASE 
					WHEN EXISTS (
							SELECT tno
							FROM fin.AMTransLoan
							WHERE pid = 9
								AND fin.AMTransLoan.tno = trans.AMTrns.tno
							)
						THEN (
								SELECT SUM(amt)
								FROM fin.AMTransLoan
								WHERE pid = 9
									AND fin.AMTransLoan.tno = trans.AMTrns.tno
								)
					ELSE 0
					END
			FROM trans.AMTrns
			INNER JOIN fin.ADetail ON trans.AMTrns.IAccno = fin.ADetail.IAccno
			INNER JOIN fin.ProductDetail ON fin.ADetail.PID = fin.ProductDetail.PID
			INNER JOIN fin.SchmDetails ON fin.ProductDetail.SDID = fin.SchmDetails.SDID
			WHERE (fin.SchmDetails.SType = 1)
				AND trans.AMTrns.IAccno = @IACcNo
				AND trans.AMTrns.vdate >= @FDATE
			) VWALnStm
		-----------------------
		INNER JOIN trans.AMTrns ON VWALnStm.tno = trans.AMTrns.tno
		WHERE (VWALnStm.IAccno = @IACcNo)
			AND (
				VWALnStm.vdate BETWEEN @FDATE
					AND @TDATE
				)
		ORDER BY VWALnStm.VDATE
			,VWALnStm.TNO

		OPEN STMENT

		FETCH STMENT
		INTO @IACCNO
			,@VVDate
			,@PERTI
			,@PDR
			,@ODR
			,@Rbt
			,@PCR
			,@OCR
			,@CRPINT
			,@CROINT
			,@CRPEN
			,@DrCr
		WHILE @@FETCH_STATUS = 0
		BEGIN
			SET @SNO = @SNO + 1
			SET @PBAL = ROUND(@PBAL + @PDR - @PCR, 2)
			SET @OBAL = ROUND(@OBAL + @ODR - @OCR, 2)

			--set @notes=@perti
			--SET @PERTI=  dbo.GetTdesc(@TNO)
			--IF @PERTI=''
			--set @PERTI=@NOTES
			--BEGIN				
			IF @PBAL >= 0
			begin
				SET @PPBAL = CAST(@PBAL AS NVARCHAR(100)) 
				set @DrCr='Dr'
				end
			ELSE IF @PBAL < 0
				SET @PPBAL = CAST(@PBAL AS NVARCHAR(100)) 
				set @DrCr='Cr'
			IF @OBAL > 0
			begin
				SET @OOBAL = CAST(@OBAL AS NVARCHAR(100)) 
				set @DrCr= 'Dr'
				end
			ELSE IF @OBAL < 0
			begin
				SET @OOBAL = CAST(@OBAL AS NVARCHAR(100)) 
				set @DrCr='Cr'
end
			--END
			INSERT INTO @STMNTTbl
			SELECT @SNO
				,@IACCNO
				,@VVDate
				,@PERTI
				,@PDR
				,@ODR
				,@Rbt
				,@PCR
				,@OCR
				,@CRPINT
				,@CROINT
				,@CRPEN
				,@PPBAL
				,@OOBAL
				,@DrCr
			FETCH STMENT
			INTO @IACCNO
				,@VVDate
				,@PERTI
				,@PDR
				,@ODR
				,@Rbt
				,@PCR
				,@OCR
				,@CRPINT
				,@CROINT
				,@CRPEN
				,@DrCr
		END

		CLOSE STMENT

		DEALLOCATE STMENT
	END

	RETURN
END
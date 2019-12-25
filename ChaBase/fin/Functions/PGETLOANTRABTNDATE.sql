 
CREATE FUNCTION [fin].[PGETLOANTRABTNDATE] (
	@FDATE SMALLDATETIME
	,@TDATE SMALLDATETIME
	,@BRHID INT
	,@ISDIS BIT
	,@ISVERFIED BIT
	)
RETURNS @temptbl TABLE (
	 SdId int
		,PID int
		,PName varchar(100)
		,IAccno int
		,Accno varchar(50)
		,Aname varchar(500)
		,notes varchar(1000)
		,vdate datetime
		,PriDr decimal
		,OthrDr decimal
		,[Interest] decimal
		,IonI	Decimal	
		,IonCA Decimal
		,PonPr Decimal
		,PonInt Decimal
		,Rbt    Decimal
		,ttype int
		,brnhno int
		,Isverfied bit 
	)
AS
BEGIN
	IF @isDis = 1
			AND @isverfied = 1
	
		INSERT INTO @temptbl 
	 
		 	SELECT SchmDetails.SDID
				,ProductDetail.PID
				,ProductDetail.PName
				,ADetail.IAccno
				,ADetail.Accno
				,ADetail.Aname
				,AMTrns.notes
				,ALoanTra.vdate
				,ALoanTra.PriDr
				,ALoanTra.OthrDr
				,0 as [Interest] 
				,0 as IonI	 
				,0 as IonCA  
				,0 as PonPr  
				,0 as PonInt  
				,0 as Rbt   
				,ALoanTra.ttype
				,AMTrns.brnhno
				,1 AS Isverfied

			FROM fin.ProductDetail
			INNER JOIN fin.SchmDetails ON ProductDetail.SDID = SchmDetails.SDID
			INNER JOIN fin.ALoanTra
			INNER JOIN fin.ADetail ON ALoanTra.iaccno = ADetail.IAccno ON ProductDetail.PID = ADetail.PID INNER JOIN trans.AMTrns ON ALoanTra.iaccno = AMTrns.IAccno
				AND ALoanTra.tno = AMTrns.tno WHERE (ALoanTra.vdate BETWEEN @fdate AND @tdate)
				AND AMTrns.brnhno = @brhid
				AND (
					pridr <> 0
					OR othrdr <> 0
					)
			
	

	IF @isDis = 1
		AND @isverfied = 0
	
		INSERT INTO @temptbl
		SELECT *
		FROM (
				SELECT SchmDetails.SDID
					,ProductDetail.PID
					,ProductDetail.PName
					,ADetail.iaccno
					,ADetail.accno
					,ADetail.Aname
					,AsTrns.remarks
					,AsTrns.PostedOn
					,PriDr =  case when astrns.IsOtherLoan=0 then Amount else 0 end
					,OthrDr =  case when astrns.IsOtherLoan=1 then Amount else 0 end
				,0 as [Interest] 
				,0 as IonI	 
				,0 as IonCA  
				,0 as PonPr  
				,0 as PonInt  
				,0 as Rbt   
					,AsTrns.Mode
					,ADetail.BrchID
					,0 AS Isverfied
				FROM fin.FGetLoanDisburseDetails() as AsTrns
				INNER JOIN fin.ADetail ON AsTrns.IAccno = ADetail.IAccno
				INNER JOIN fin.ProductDetail ON ADetail.PID = ProductDetail.PID
				INNER JOIN fin.SchmDetails ON ProductDetail.SDID = SchmDetails.SDID
			 
				WHERE (fin.SchmDetails.SType = 1)
				 
					AND (
						AsTrns.PostedOn BETWEEN @fdate
							AND @tdate
						)
					AND ADetail.BrchID = @brhid and astrns.VNo is null
				) x
			WHERE x.pridr > 0
				OR x.othrdr > 0
			

		IF @isDis = 0
			AND @isverfied = 1
		
			INSERT INTO @temptbl
		 
				SELECT 
					 SchmDetails.SDID
					,ProductDetail.PID
					,ProductDetail.PName
					,ADetail.IAccno
					,ADetail.Accno
					,ADetail.Aname
					,AMTrns.notes
					,ALoanTra.vdate
					,PriCr
					,othrCr
					,intAPd + intRPd INT
					,IonIAPd + IonIRPd IonI
					,IonCAPd + IonCRPd IonCA
					,PonPrAPd + PonPrRPd PonPr
					,PonIAPd + PonIRPd PonInt
					,RbtInt + RbtPen Rbt
					,ALoanTra.ttype
					,AMTrns.brnhno
					,1 AS Isverfied
				FROM fin.ProductDetail
				INNER JOIN fin.SchmDetails ON ProductDetail.SDID = SchmDetails.SDID
				INNER JOIN fin.ALoanTra
				INNER JOIN fin.ADetail ON ALoanTra.iaccno = ADetail.IAccno ON ProductDetail.PID = ADetail.PID INNER JOIN trans.AMTrns ON ALoanTra.iaccno = AMTrns.IAccno
					AND ALoanTra.tno = AMTrns.tno WHERE 
					--(
						--.vdate BETWEEN @fdate
						--	AND @tdate
						--)
					--AND 
					--(AMTrns.brnhno = @brhid)
					--AND 
					pridr = 0
				ORDER BY ALoanTra.vdate
					,ProductDetail.PID
					,ADetail.IAccno
			 

		IF @isDis = 0
			AND @isverfied = 0
		
			INSERT INTO @temptbl
			 
		 
				SELECT SchmDetails.SDID
					,ProductDetail.PID
					,ProductDetail.PName
					,ADetail.iaccno
					,ADetail.accno
					,ADetail.Aname
					,AsTrns.notes
					,AsTrns.tdate
					,PriCr = (
						SELECT isnull(SUM(amt), 0)
						FROM fin.ASTransLoan
						WHERE pid = 13
							AND ASTransLoan.tno = AsTrns.tno
						)
					,othrCr = (
						SELECT SUM(amt)
						FROM fin.ASTransLoan
						WHERE pid = 14
							AND ASTransLoan.tno = AsTrns.tno
						)
					,intA = (
						SELECT isnull(SUM(amt), 0)
						FROM fin.ASTransLoan
						WHERE pid = 0
							AND ASTransLoan.tno = AsTrns.tno
						) + (
						SELECT isnull(SUM(amt), 0)
						FROM fin.ASTransLoan
						WHERE pid = 5
							AND ASTransLoan.tno = AsTrns.tno
						)
					,IonI = (
						SELECT SUM(amt)
						FROM fin.ASTransLoan
						WHERE pid = 1
							AND ASTransLoan.tno = AsTrns.tno
						) + (
						SELECT isnull(SUM(amt), 0)
						FROM fin.ASTransLoan
						WHERE pid = 6
							AND ASTransLoan.tno = AsTrns.tno
						)
					,IonCA = (
						SELECT isnull(SUM(amt), 0)
						FROM fin.ASTransLoan
						WHERE pid = 2
							AND ASTransLoan.tno = AsTrns.tno
						) + (
						SELECT isnull(SUM(amt), 0)
						FROM fin.ASTransLoan
						WHERE pid = 7
							AND ASTransLoan.tno = AsTrns.tno
						)
					,PonPr = (
						SELECT isnull(SUM(amt), 0)
						FROM fin.ASTransLoan
						WHERE pid = 3
							AND ASTransLoan.tno = AsTrns.tno
						) + (
						SELECT isnull(SUM(amt), 0)
						FROM fin.ASTransLoan
						WHERE pid = 8
							AND ASTransLoan.tno = AsTrns.tno
						)
					,PonInt = (
						SELECT isnull(SUM(amt), 0)
						FROM fin.ASTransLoan
						WHERE pid = 4
							AND ASTransLoan.tno = AsTrns.tno
						) + (
						SELECT isnull(SUM(amt), 0)
						FROM fin.ASTransLoan
						WHERE pid = 9
							AND ASTransLoan.tno = AsTrns.tno
						)
					,Rbt = (
						SELECT isnull(SUM(amt), 0)
						FROM fin.ASTransLoan
						WHERE pid = 12
							AND ASTransLoan.tno = AsTrns.tno
						) + (
						SELECT isnull(SUM(amt), 0)
						FROM fin.ASTransLoan
						WHERE pid = 17
							AND ASTransLoan.tno = AsTrns.tno
						)
					,AsTrns.ttype
					,AsTrns.brnhno
					,0 AS Isverfied
				FROM trans.AsTrns
				INNER JOIN fin.ADetail ON AsTrns.IAccno = ADetail.IAccno
				INNER JOIN fin.ProductDetail ON ADetail.PID = ProductDetail.PID
				INNER JOIN fin.SchmDetails ON ProductDetail.SDID = SchmDetails.SDID
				INNER JOIN fin.ASTransLoan ON astrns.tno = ASTransLoan.tno
				WHERE (fin.SchmDetails.SType = 1)
					AND ASTransLoan.pid NOT IN (
						10
						,11
						)
					AND (
						AsTrns.tdate BETWEEN @fdate
							AND @tdate
						)
					AND AsTrns.brnhno = @brhid
				 
		

		RETURN  
	END
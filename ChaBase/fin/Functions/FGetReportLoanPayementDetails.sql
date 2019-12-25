 
CREATE FUNCTION [fin].[FGetReportLoanPayementDetails] (
	@FDATE SMALLDATETIME
	,@TDATE SMALLDATETIME
	,@BRHID INT
	,@ISVERFIED BIT
	)
RETURNS @temptbl TABLE (
	 SdId int
		,PID int
		,PNAME varchar(100)
		,IAccno int
		,ACCNO varchar(50)
		,ANAME varchar(500)
		,NOTES varchar(1000)
		,VDATE datetime
		,PriDr decimal
		,OthrDr decimal
		,Amount decimal
		,[Interest] decimal
		,IonI	Decimal	
		,IonCA Decimal
		,PonPr Decimal
		,PonInt Decimal
		,Rbt    Decimal
		,TTYPE int
		,BRNHNO int
		,POSTEDBY int
		,Isverfied bit 
	)
AS
BEGIN
		if @isverfied = 1
		
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
					,cramt as totalamount
					,intAPd + intRPd  as interest
					,IonIAPd + IonIRPd IonI
					,IonCAPd + IonCRPd IonCA
					,PonPrAPd + PonPrRPd PonPr
					,PonIAPd + PonIRPd PonInt
					,RbtInt + RbtPen Rbt
					,ALoanTra.ttype
					,AMTrns.brnhno
					,amtrns.postedby
					,1 AS Isverfied
				FROM fin.ProductDetail
				INNER JOIN fin.SchmDetails ON ProductDetail.SDID = SchmDetails.SDID
				INNER JOIN fin.ALoanTra
				INNER JOIN fin.ADetail ON ALoanTra.iaccno = ADetail.IAccno ON ProductDetail.PID = ADetail.PID
				INNER JOIN trans.AMTrns ON ALoanTra.iaccno = AMTrns.IAccno
					AND ALoanTra.tno = AMTrns.tno WHERE 
					(
						trans.AMTrns.vdate BETWEEN @fdate
						AND @tdate
						)
					AND 
					(AMTrns.brnhno = @brhid)
					AND 
					pridr = 0
				ORDER BY ALoanTra.vdate
					,ProductDetail.PID
					,ADetail.IAccno
			 

		IF @isverfied = 0
		
			INSERT INTO @temptbl
			 
		 
				SELECT distinct SchmDetails.SDID
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
						
					,AsTrns.cramt,
					intA = (
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
					,ASTrns.postedby
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

					if @isverfied = 2
		
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
					,cramt as totalamount
					,intAPd + intRPd  as interest
					,IonIAPd + IonIRPd IonI
					,IonCAPd + IonCRPd IonCA
					,PonPrAPd + PonPrRPd PonPr
					,PonIAPd + PonIRPd PonInt
					,RbtInt + RbtPen Rbt
					,ALoanTra.ttype
					,AMTrns.brnhno
					,amtrns.postedby
					,1 AS Isverfied
				FROM fin.ProductDetail
				INNER JOIN fin.SchmDetails ON ProductDetail.SDID = SchmDetails.SDID
				INNER JOIN fin.ALoanTra
				INNER JOIN fin.ADetail ON ALoanTra.iaccno = ADetail.IAccno ON ProductDetail.PID = ADetail.PID
				INNER JOIN trans.AMTrns ON ALoanTra.iaccno = AMTrns.IAccno
					AND ALoanTra.tno = AMTrns.tno WHERE 
					(
						trans.AMTrns.vdate BETWEEN @fdate
						AND @tdate
						)
					AND 
					(AMTrns.brnhno = @brhid)
					AND 
					pridr = 0
				 
			 
			 union
		 
		
				SELECT distinct SchmDetails.SDID
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
						
					,AsTrns.cramt,
					intA = (
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
					,ASTrns.postedby
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
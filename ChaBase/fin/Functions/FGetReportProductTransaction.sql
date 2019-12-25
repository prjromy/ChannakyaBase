CREATE FUNCTION [fin].[FGetReportProductTransaction] (

	@FDATE SMALLDATETIME

	,@TDATE SMALLDATETIME

	,@BRCHID TINYINT

	,@SRCHTYP TINYINT

	,@ISDEP BIT

	)

RETURNS @TEMPTBL TABLE (

	PID INT

	,PNAME NVARCHAR(100)

	,ACCNO NVARCHAR(20)

	,ANAME NVARCHAR(500)

	,SCURRID INT

	,VDATE SMALLDATETIME

	,NOTES NVARCHAR(1000)

	,AMT MONEY

	,TTYPE INT

	,POSTEDBY INT

	,APPROVEDBY INT

	,BRNHNO INT

	,TNO NUMERIC

	,VNO NUMERIC

	,ISVERIFY BIT

	)

	---MARKED ENCRYPTION 

AS

BEGIN

	IF @ISdep = 1

	BEGIN

		IF @SrchTyp = 1 -- Cash Deposite

			INSERT INTO @tempTbl

			--Verified

			SELECT pd.pid

				,pd.pname

				,ad.Accno

				,ad.Aname

				,ad.CurrID AS scurrid

				,At.vdate

				,'notes' = 'Deposit By ' + at.notes

				,At.cramt

				,At.ttype

				,At.postedby

				,At.approvedby

				,At.brnhno AS tbrhno

				,At.tno

				,At.vno

				,1 AS IsVerify

			FROM fin.ProductDetail pd

			INNER JOIN fin.ADetail ad ON pd.PID = ad.PID

			INNER JOIN trans.AMTrns At ON ad.IAccno = At.IAccno

			INNER JOIN fin.SchmDetails sd ON pd.SDID = sd.SDID

			WHERE (sd.SType = 0)

				AND ttype = 1

				AND vdate BETWEEN @fdate

					AND @tdate

				AND dramt = 0

				AND cramt > 0

				AND at.brnhno = @brchid

			

			UNION ALL

			

			SELECT pd.PID

				,pd.PName

				,ad.Accno

				,ad.Aname

				,ad.CurrID

				,fs.Tdate

				,'notes' = 'Frx.Deposit By ' + fs.notes

				,fs.CrAmt

				,1 AS ttype

				,fs.PostedBy

				,'' AS approvedby

				,fs.BrnhId AS tbrhno

				,fs.TNo

				,fs.IVno

				,1 AS IsVerify

			FROM fin.ForeignBuySell fs

			INNER JOIN fin.ADetail ad ON fs.IAccNo = ad.IAccno

			INNER JOIN fin.ProductDetail pd ON ad.PID = pd.PID

			WHERE (fs.IAccNo IS NOT NULL)

				AND (

					tdate BETWEEN @fdate

						AND @tdate

					)

				AND fs.BrnhId = @brchid

			

			UNION ALL

			

			--------------------------------------------------Unverified Trns rabindra

			SELECT ProductDetail.PID

				,ProductDetail.PName

				,ADetail.Accno

				,ADetail.Aname

				,ADetail.CurrID

				,ASTrns.tdate

				,'notes' = 'Unverified Deposit By ' + ASTrns.notes

				,ASTrns.cramt

				,ASTrns.ttype

				,ASTrns.postedby

				,'' AS Approvedby

				,ASTrns.brnhno

				,ASTrns.tno

				,ASTrns.VNO

				,0 AS IsVerify

			FROM Trans.ASTrns

			INNER JOIN fin.ADetail ON ASTrns.IAccno = ADetail.IAccno

			INNER JOIN fin.ProductDetail ON ADetail.PID = ProductDetail.PID

			INNER JOIN fin.SchmDetails ON ProductDetail.SDID = SchmDetails.SDID

			WHERE (ASTrns.ttype = 1)

				AND (SchmDetails.SType = 0)

				AND (AStrns.Cramt > 0)

				AND (

					AStrns.tdate BETWEEN @fdate

						AND @tdate

					)

				AND ASTrns.Brnhno = @brchid

				AND (

					ASTrns.IsDeleted != 0

					OR ASTrns.IsDeleted IS NOT NULL

					)

			

			UNION ALL

			

			SELECT pd.PID

				,pd.PName

				,ad.Accno

				,ad.Aname

				,ad.CurrID

				,fs.Tdate

				,'notes' = 'Unverified Frx. Deposit By ' + fs.notes

				,fs.CrAmt

				,1 AS ttype

				,fs.PostedBy

				,'' AS approvedby

				,fs.BrnhId AS tbrhno

				,fs.TNo

				,fs.IVno

				,0 AS IsVerify

			FROM fin.ForeignSBuySell fs

			INNER JOIN fin.ADetail ad ON fs.IAccNo = ad.IAccno

			INNER JOIN fin.ProductDetail pd ON ad.PID = pd.PID

			WHERE (fs.IAccNo IS NOT NULL)

				AND (

					tdate BETWEEN @fdate

						AND @tdate

					)

				AND fs.BrnhId = @brchid

		---------------------------------------------------------

		IF @srchtyp = 2 -- ABBS Deposit trxn

			INSERT INTO @tempTbl

			--ABBS Cash Deposit

			SELECT pd.pid

				,pd.pname

				,ad.Accno

				,ad.Aname

				,ad.CurrID AS scurrid

				,At.vdate

				,'notes' = 'ABBS Cash Deposit By ' + at.notes

				,At.cramt

				,At.ttype

				,At.postedby

				,At.approvedby

				,At.brnhno AS tbrhno

				,At.tno

				,At.vno

				,1 AS IsVerify

			FROM fin.ProductDetail pd

			INNER JOIN fin.ADetail ad ON pd.PID = ad.PID

			INNER JOIN trans.AMTrns At ON ad.IAccno = At.IAccno

			INNER JOIN fin.SchmDetails sd ON pd.SDID = sd.SDID

			WHERE (sd.SType = 0)

				AND (ttype = 2)

				AND (

					vdate BETWEEN @fdate

						AND @tdate

					)

				AND (cramt > 0)

				AND at.brnhno = @brchid

			

			UNION ALL

			

			--ABBS Frx Currency Deposit

			SELECT pd.PID

				,pd.PName

				,ad.Accno

				,ad.Aname

				,ad.CurrID

				,fs.Tdate

				,'notes' = 'ABBS Frx. Currency Deposit By ' + fs.notes

				,fs.DrAmt AS cramt

				,1 AS ttype

				,fs.PostedBy

				,'' AS approvedby

				,fs.BrnhId AS tbrhno

				,fs.TNo

				,fs.IVno

				,1 AS IsVerify

			FROM fin.ForeignBuySell fs

			INNER JOIN fin.ADetail ad ON fs.IAccNo = ad.IAccno

			INNER JOIN fin.ProductDetail pd ON ad.PID = pd.PID

			WHERE (fs.IAccNo IS NOT NULL)

				AND (

					tdate BETWEEN @fdate

						AND @tdate

					)

				AND (ad.BrchID <> fs.BrnhId)

				AND (fs.BrnhId = @brchid)

			

			UNION ALL

			

			--ABBS Unverified Cash Deposit

			SELECT pd.pid

				,pd.pname

				,ad.Accno

				,ad.Aname

				,ad.CurrID AS scurrid

				,At.tdate

				,'notes' = 'Unverified ABBS Cash Deposit By ' + at.notes

				,At.cramt

				,At.ttype

				,At.postedby

				,'' approvedby

				,At.brnhno AS tbrhno

				,At.tno

				,At.vno

				,0 AS IsVerify

			FROM fin.ProductDetail pd

			INNER JOIN fin.ADetail ad ON pd.PID = ad.PID

			INNER JOIN Trans.ASTrns At ON ad.IAccno = At.IAccno

			INNER JOIN fin.SchmDetails sd ON pd.SDID = sd.SDID

			WHERE (sd.SType = 0)

				AND (ttype = 2)

				AND (

					tdate BETWEEN @fdate

						AND @tdate

					)

				AND (cramt > 0)

				AND at.brnhno = @brchid

				AND (

					At.IsDeleted != 0

					OR At.IsDeleted IS NOT NULL

					)

			

			UNION ALL

			

			--ABBS unverified Frx. Currency Deposit

			SELECT pd.PID

				,pd.PName

				,ad.Accno

				,ad.Aname

				,ad.CurrID

				,fs.Tdate

				,'notes' = 'Unverified ABBS Frx. Currency Deposit By ' + fs.notes

				,fs.DrAmt AS cramt

				,1 AS ttype

				,fs.PostedBy

				,'' AS approvedby

				,fs.BrnhId AS tbrhno

				,fs.TNo

				,fs.IVno

				,0 AS IsVerify

			FROM fin.ForeignSBuySell fs

			INNER JOIN fin.ADetail ad ON fs.IAccNo = ad.IAccno

			INNER JOIN fin.ProductDetail pd ON ad.PID = pd.PID

			WHERE (fs.IAccNo IS NOT NULL)

				AND (

					tdate BETWEEN @fdate

						AND @tdate

					)

				AND (ad.BrchID <> fs.BrnhId)

				AND (fs.BrnhId = @brchid)

			

			UNION ALL

			

			--ABBS Internal Chq Deposit By Branch Ac

			SELECT ProductDetail.PID

				,ProductDetail.PName

				,ADetail.Accno

				,ADetail.Aname

				,ADetail.CurrID

				,IChkDep.Tdate

				,'notes' = CASE 

					WHEN IChkDep.VerifiedBy IS NOT NULL

						THEN 'ABBS Internal Chq Deposited To ' + fin.GetAcNo(Tiaccno) + ' from Chq. No.' + Cast(IChkDep.SlpNo AS NVARCHAR(15))

					ELSE 'Pending ABBS Internal Chq Deposited To ' + fin.GetAcNo(Tiaccno) + ' from Chq. No.' + Cast(IChkDep.SlpNo AS NVARCHAR(15))

					END

				,IChkDep.Amt DrAmt

				,IChkDep.Ttype

				,IChkDep.postedby

				,IChkDep.verifiedby

				,IChkDep.BrchID

				,IChkDep.Tno

				,IChkDep.Vno

				,'IsVerify' = CASE 

					WHEN IChkDep.VerifiedBy IS NOT NULL

						THEN 1

					ELSE 0

					END

			FROM fin.IChkDep

			INNER JOIN fin.ADetail ON IChkDep.FIaccno = ADetail.IAccno

			INNER JOIN fin.ProductDetail ON ADetail.PID = ProductDetail.PID

			INNER JOIN fin.SchmDetails ON ProductDetail.SDID = SchmDetails.SDID

			INNER JOIN fin.ADetail ADetail_1 ON IChkDep.TIAccno = ADetail_1.IAccno

				AND ADetail.BrchID <> ADetail_1.BrchID

			WHERE (

					IChkDep.tdate BETWEEN @fdate

						AND @tdate

					)

				AND ADetail_1.brchid = @brchid

		IF @srchtyp = 3 -- non cash deposit trxn

			INSERT INTO @tempTbl

			SELECT pd.pid

				,pd.pname

				,ad.Accno

				,ad.Aname

				,ad.CurrID AS scurrid

				,At.vdate

				,At.notes

				,At.cramt

				,At.ttype

				,At.postedby

				,At.approvedby

				,At.brnhno AS tbrhno

				,At.tno

				,At.vno

				,1 AS IsVerify

			FROM fin.ProductDetail pd

			INNER JOIN fin.ADetail ad ON pd.PID = ad.PID

			INNER JOIN trans.AMTrns At ON ad.IAccno = At.IAccno

			INNER JOIN fin.SchmDetails sd ON pd.SDID = sd.SDID

			WHERE (sd.SType = 0)

				AND ttype NOT IN (

					1

					,2

					)

				AND (

					vdate BETWEEN @fdate

						AND @tdate

					)

				AND dramt = 0

				AND cramt > 0

				AND at.brnhno = @brchid

			

			UNION ALL

			

			--External Chq Clearance needs to be edited

			SELECT pd.PID

				,pd.PName

				,ad.Accno

				,ad.Aname

				,ad.CurrID

				,amc.cdate AS vdate

				,'OBC Chq No.' + amc.chqno + ' Cleared' AS notes

				,amc.camount AS cramt

				,5 AS ttype

				,amc.postedby

				,amc.verifiedby

				,amc.brchid AS tbrhno

				,amc.rno

				,amc.vno

				,1 AS IsVerify

			FROM fin.AMClearance amc

			INNER JOIN fin.ADetail ad ON amc.IAccno = ad.IAccno

			INNER JOIN fin.ProductDetail pd ON ad.PID = pd.PID

			WHERE cdate BETWEEN @fdate

					AND @tdate

				AND amc.BrchId = @brchid

			

			UNION ALL

			

			SELECT pd.PID

				,pd.PName

				,ad.Accno

				,ad.Aname

				,ad.CurrID

				,amc.vdate AS vdate

				,'OBC Chq No.' + amc.chqno + ' Clerance pending' AS notes

				,amc.camount AS cramt

				,5 AS ttype

				,amc.postedby

				,amc.postedby

				,ad.BrchID AS tbrhno

				,amc.rno

				,- 1 AS Vno

				,1 AS IsVerify

			FROM fin.AsClearance amc

			INNER JOIN ADetail ad ON amc.IAccno = ad.IAccno

			INNER JOIN ProductDetail pd ON ad.PID = pd.PID

			WHERE (amc.chqstate IS NULL)

				AND amc.vdate BETWEEN @fdate

					AND @tdate

				AND ad.BrchID = @brchid

			

			UNION ALL

			

			--------------------?????????????????????????????

			--SELECT pd.PID, pd.PName, ad.Accno, ad.Aname, ad.CurrID, amc.cdate AS vdate, 'OBC Chq No.' + amc.chqno + ' Clerance pending' 

			--	AS notes,amc.camount AS cramt, 5 AS ttype, amc.postedby, amc.verifiedby, amc.BrchID AS tbrhno, amc.rno, amc.AVno,0 As IsVerify

			--FROM AMClearanceHR amc 

			--	INNER JOIN ADetail ad ON amc.IAccno = ad.IAccno 

			--	INNER JOIN ProductDetail pd ON ad.PID = pd.PID

			--WHERE     (amc.chqstate IN (5))  and cdate between @fdate and   @tdate  and amc.BrchID= @brchid

			--union all

			--internal chq deposit rabindra

			SELECT ProductDetail.PID

				,ProductDetail.PName

				,ADetail.Accno

				,ADetail.Aname

				,ADetail.CurrID

				,IChkDep.Tdate

				,'note' = CASE 

					WHEN IChkDep.Ttype = 51

						THEN 'Transfered from ' + fin.GetAcNo(IChkDep.FIaccno) + ' Chq No ' + CAST(IChkDep.SlpNo AS NVARCHAR(10))

					ELSE 'Transfered from ' + fin.GetAcNo(IChkDep.FIaccno) + ' Chq No ' + CAST(IChkDep.SlpNo AS NVARCHAR(10)) + ' Pending'

					END

				,IChkDep.Amt AS Cramt

				,IChkDep.Ttype

				,IChkDep.postedby

				,IChkDep.verifiedby

				,IChkDep.BrchID

				,IChkDep.Tno

				,IChkDep.Vno

				,'IsVerify' = CASE 

					WHEN IChkDep.TType = 51

						THEN 1

					ELSE 0

					END

			FROM fin.IChkDep

			INNER JOIN fin.ProductDetail

			INNER JOIN fin.SchmDetails ON ProductDetail.SDID = SchmDetails.SDID

			INNER JOIN fin.ADetail ON ProductDetail.PID = ADetail.PID ON IChkDep.TIAccno = ADetail.IAccno WHERE (SchmDetails.SType = 0)

				AND (ADetail.BrchID = @brchid)

				AND (

					IChkDep.Tdate BETWEEN @fdate

						AND @tdate

					)

			

			UNION ALL

			

			---Interest Capitalizaton

			SELECT pd.PID

				,pd.PName

				,ad.Accno

				,ad.Aname

				,ad.CurrID

				,ai.Vdate

				,'Int. Trns From ' + (

					SELECT fin.Getacno(Fiaccno)

					) AS notes

				,ai.CRamt

				,5 AS ttype

				,'' AS postedby

				,'' AS verifiedby

				,ad.BrchID

				,ai.Trxno

				,ai.Ivno

				,1 AS IsVerify

			FROM fin.AintCap ai

			INNER JOIN fin.ADetail ad ON ai.TIaccno = ad.IAccno

			INNER JOIN fin.ProductDetail pd ON ad.PID = pd.PID

			WHERE (ai.IsSlf = 0)

				AND (ai.FIaccno IS NOT NULL)

				AND (

					AI.Vdate BETWEEN @fdate

						AND @tdate

					)

				AND (ad.BrchID = @brchid)

			

			UNION ALL

			

			SELECT pd.PID

				,pd.PName

				,ad.Accno

				,ad.Aname

				,ad.CurrID

				,ai.Vdate

				,'Int. Trns From ' + (

					SELECT fin.Getacno(TIaccno)

					) AS notes

				,ai.CRamt

				,5 AS ttype

				,'' AS postedby

				,'' AS verifiedby

				,ad.BrchID

				,ai.Trxno

				,ai.Ivno

				,1 AS IsVerify

			FROM fin.AintCap ai

			INNER JOIN fin.ADetail ad ON ai.TIaccno = ad.IAccno

			INNER JOIN fin.ProductDetail pd ON ad.PID = pd.PID

			WHERE (ai.IsSlf = 1)

				AND (ai.FIaccno IS NULL)

				AND (

					AI.Vdate BETWEEN @fdate

						AND @tdate

					)

				AND (ad.BrchID = @brchid)

		IF @srchtyp = 4 -- non cash deposit trxn all

			INSERT INTO @tempTbl

			SELECT pd.pid

				,pd.pname

				,ad.Accno

				,ad.Aname

				,ad.CurrID AS scurrid

				,At.vdate

				,'notes' = 'Deposit By ' + at.notes

				,At.cramt

				,At.ttype

				,At.postedby

				,At.approvedby

				,At.brnhno AS tbrhno

				,At.tno

				,At.vno

				,1 AS IsVerify

			FROM fin.ProductDetail pd

			INNER JOIN fin.ADetail ad ON pd.PID = ad.PID

			INNER JOIN trans.AMTrns At ON ad.IAccno = At.IAccno

			INNER JOIN fin.SchmDetails sd ON pd.SDID = sd.SDID

			WHERE (sd.SType = 0)

				AND ttype = 1

				AND vdate BETWEEN @fdate

					AND @tdate

				AND dramt = 0

				AND cramt > 0

				AND at.brnhno = @brchid

			

			UNION ALL

			

			SELECT pd.PID

				,pd.PName

				,ad.Accno

				,ad.Aname

				,ad.CurrID

				,fs.Tdate

				,'notes' = 'Frx.Deposit By ' + fs.notes

				,fs.CrAmt

				,1 AS ttype

				,fs.PostedBy

				,'' AS approvedby

				,fs.BrnhId AS tbrhno

				,fs.TNo

				,fs.IVno

				,1 AS IsVerify

			FROM fin.ForeignBuySell fs

			INNER JOIN fin.ADetail ad ON fs.IAccNo = ad.IAccno

			INNER JOIN fin.ProductDetail pd ON ad.PID = pd.PID

			WHERE (fs.IAccNo IS NOT NULL)

				AND (

					tdate BETWEEN @fdate

						AND @tdate

					)

				AND fs.BrnhId = @brchid

			

			UNION ALL

			

			--Unverified Trns rabindra

			SELECT ProductDetail.PID

				,ProductDetail.PName

				,ADetail.Accno

				,ADetail.Aname

				,ADetail.CurrID

				,ASTrns.tdate

				,'notes' = 'Unverified Deposit By ' + ASTrns.notes

				,ASTrns.cramt

				,ASTrns.ttype

				,ASTrns.postedby

				,'' AS Approvedby

				,ASTrns.brnhno

				,ASTrns.tno

				,ASTrns.VNO

				,0 AS IsVerify

			FROM trans.ASTrns

			INNER JOIN fin.ADetail ON ASTrns.IAccno = ADetail.IAccno

			INNER JOIN fin.ProductDetail ON ADetail.PID = ProductDetail.PID

			INNER JOIN fin.SchmDetails ON ProductDetail.SDID = SchmDetails.SDID

			WHERE (ASTrns.ttype = 1)

				AND (SchmDetails.SType = 0)

				AND (AStrns.Cramt > 0)

				AND (

					AStrns.tdate BETWEEN @fdate

						AND @tdate

					)

				AND ASTrns.Brnhno = @brchid

				AND (

					ASTrns.IsDeleted != 0

					OR ASTrns.IsDeleted IS NOT NULL

					)

			

			UNION ALL

			

			SELECT pd.PID

				,pd.PName

				,ad.Accno

				,ad.Aname

				,ad.CurrID

				,fs.Tdate

				,'notes' = 'Unverified Frx. Deposit By ' + fs.notes

				,fs.CrAmt

				,1 AS ttype

				,fs.PostedBy

				,'' AS approvedby

				,fs.BrnhId AS tbrhno

				,fs.TNo

				,fs.IVno

				,0 AS IsVerify

			FROM fin.ForeignSBuySell fs

			INNER JOIN fin.ADetail ad ON fs.IAccNo = ad.IAccno

			INNER JOIN fin.ProductDetail pd ON ad.PID = pd.PID

			WHERE (fs.IAccNo IS NOT NULL)

				AND (

					tdate BETWEEN @fdate

						AND @tdate

					)

				AND fs.BrnhId = @brchid

			

			UNION ALL

			

			--ABBS Cash Deposit

			SELECT pd.pid

				,pd.pname

				,ad.Accno

				,ad.Aname

				,ad.CurrID AS scurrid

				,At.vdate

				,'notes' = 'ABBS Cash Deposit By ' + at.notes

				,At.cramt

				,At.ttype

				,At.postedby

				,At.approvedby

				,At.brnhno AS tbrhno

				,At.tno

				,At.vno

				,1 AS IsVerify

			FROM fin.ProductDetail pd

			INNER JOIN fin.ADetail ad ON pd.PID = ad.PID

			INNER JOIN trans.AMTrns At ON ad.IAccno = At.IAccno

			INNER JOIN fin.SchmDetails sd ON pd.SDID = sd.SDID

			WHERE (sd.SType = 0)

				AND (ttype = 2)

				AND (

					vdate BETWEEN @fdate

						AND @tdate

					)

				AND (cramt > 0)

				AND at.brnhno = @brchid

			

			UNION ALL

			

			--ABBS Frx Currency Deposit

			SELECT pd.PID

				,pd.PName

				,ad.Accno

				,ad.Aname

				,ad.CurrID

				,fs.Tdate

				,'notes' = 'ABBS Frx. Currency Deposit By ' + fs.notes

				,fs.DrAmt AS cramt

				,1 AS ttype

				,fs.PostedBy

				,'' AS approvedby

				,fs.BrnhId AS tbrhno

				,fs.TNo

				,fs.IVno

				,1 AS IsVerify

			FROM fin.ForeignBuySell fs

			INNER JOIN fin.ADetail ad ON fs.IAccNo = ad.IAccno

			INNER JOIN fin.ProductDetail pd ON ad.PID = pd.PID

			WHERE (fs.IAccNo IS NOT NULL)

				AND (

					tdate BETWEEN @fdate

						AND @tdate

					)

				AND (ad.BrchID <> fs.BrnhId)

				AND (fs.BrnhId = @brchid)

			

			UNION ALL

			

			--ABBS Unverified Cash Deposit

			SELECT pd.pid

				,pd.pname

				,ad.Accno

				,ad.Aname

				,ad.CurrID AS scurrid

				,At.tdate

				,'notes' = 'Unverified ABBS Cash Deposit By ' + at.notes

				,At.cramt

				,At.ttype

				,At.postedby

				,'' approvedby

				,At.brnhno AS tbrhno

				,At.tno

				,At.vno

				,0 AS IsVerify

			FROM fin.ProductDetail pd

			INNER JOIN fin.ADetail ad ON pd.PID = ad.PID

			INNER JOIN trans.AMTrns At ON ad.IAccno = At.IAccno

			INNER JOIN fin.SchmDetails sd ON pd.SDID = sd.SDID

			WHERE (sd.SType = 0)

				AND (ttype = 2)

				AND (

					tdate BETWEEN @fdate

						AND @tdate

					)

				AND (cramt > 0)

				AND at.brnhno = @brchid

			

			UNION ALL

			

			--ABBS unverified Frx. Currency Deposit

			SELECT pd.PID

				,pd.PName

				,ad.Accno

				,ad.Aname

				,ad.CurrID

				,fs.Tdate

				,'notes' = 'Unverified ABBS Frx. Currency Deposit By ' + fs.notes

				,fs.DrAmt AS cramt

				,1 AS ttype

				,fs.PostedBy

				,'' AS approvedby

				,fs.BrnhId AS tbrhno

				,fs.TNo

				,fs.IVno

				,0 AS IsVerify

			FROM fin.ForeignSBuySell fs

			INNER JOIN fin.ADetail ad ON fs.IAccNo = ad.IAccno

			INNER JOIN fin.ProductDetail pd ON ad.PID = pd.PID

			WHERE (fs.IAccNo IS NOT NULL)

				AND (

					tdate BETWEEN @fdate

						AND @tdate

					)

				AND (ad.BrchID <> fs.BrnhId)

				AND (fs.BrnhId = @brchid)

			

			UNION ALL

			

			--ABBS Internal Chq Deposit By Branch Ac

			SELECT ProductDetail.PID

				,ProductDetail.PName

				,ADetail.Accno

				,ADetail.Aname

				,ADetail.CurrID

				,IChkDep.Tdate

				,'notes' = CASE 

					WHEN IChkDep.VerifiedBy IS NOT NULL

						THEN 'ABBS Internal Chq Deposited To ' + fin.GetAcNo(Tiaccno) + ' from Chq. No.' + Cast(IChkDep.SlpNo AS NVARCHAR(15))

					ELSE 'Pending ABBS Internal Chq Deposited To ' + fin.GetAcNo(Tiaccno) + ' from Chq. No.' + Cast(IChkDep.SlpNo AS NVARCHAR(15))

					END

				,IChkDep.Amt DrAmt

				,IChkDep.Ttype

				,IChkDep.postedby

				,IChkDep.verifiedby

				,IChkDep.BrchID

				,IChkDep.Tno

				,IChkDep.Vno

				,'IsVerify' = CASE 

					WHEN IChkDep.VerifiedBy IS NOT NULL

						THEN 1

					ELSE 0

					END

			FROM fin.IChkDep

			INNER JOIN fin.ADetail ON IChkDep.FIaccno = ADetail.IAccno

			INNER JOIN fin.ProductDetail ON ADetail.PID = ProductDetail.PID

			INNER JOIN fin.SchmDetails ON ProductDetail.SDID = SchmDetails.SDID

			INNER JOIN fin.ADetail ADetail_1 ON IChkDep.TIAccno = ADetail_1.IAccno

				AND ADetail.BrchID <> ADetail_1.BrchID

			WHERE (

					IChkDep.tdate BETWEEN @fdate

						AND @tdate

					)

				AND ADetail_1.brchid = @brchid

			

			UNION ALL

			

			SELECT pd.pid

				,pd.pname

				,ad.Accno

				,ad.Aname

				,ad.CurrID AS scurrid

				,At.vdate

				,At.notes

				,At.cramt

				,At.ttype

				,At.postedby

				,At.approvedby

				,At.brnhno AS tbrhno

				,At.tno

				,At.vno

				,1 AS IsVerify

			FROM fin.ProductDetail pd

			INNER JOIN fin.ADetail ad ON pd.PID = ad.PID

			INNER JOIN trans.AMTrns At ON ad.IAccno = At.IAccno

			INNER JOIN fin.SchmDetails sd ON pd.SDID = sd.SDID

			WHERE (sd.SType = 0)

				AND ttype NOT IN (

					1

					,2

					)

				AND (

					vdate BETWEEN @fdate

						AND @tdate

					)

				AND dramt = 0

				AND cramt > 0

				AND at.brnhno = @brchid

			

			UNION ALL

			

			---External Chq Clearance

			--SELECT     pd.PID, pd.PName, ad.Accno, ad.Aname, ad.CurrID, amc.cdate AS vdate,

			-- 		'OBC Chq No.' + amc.chqno + ' Clerance pending' AS notes,amc.camount AS cramt, 5 AS ttype, amc.postedby, 

			--	amc.verifiedby, amc.BrchID AS tbrhno, amc.rno, amc.AVno,0 As IsVerify

			--FROM         AMClearance amc 

			--	INNER JOIN ADetail ad ON amc.IAccno = ad.IAccno 

			--	INNER JOIN ProductDetail pd ON ad.PID = pd.PID

			--WHERE (amc.chqstate IN (3)) and cdate between @fdate and @tdate  and amc.BrchID= @brchid

			--union all

			--SELECT     pd.PID, pd.PName, ad.Accno, ad.Aname, ad.CurrID, amc.cdate AS vdate, 

			--	'OBC Chq No.' + amc.chqno + ' Clerance made' AS notes,amc.camount AS cramt, 5 AS ttype, amc.postedby, 

			--	amc.verifiedby, amc.BrchID AS tbrhno, amc.rno, amc.Vno,1 As IsVerify

			--FROM         AMClearanceH amc 

			--	INNER JOIN ADetail ad ON amc.IAccno = ad.IAccno 

			--	INNER JOIN ProductDetail pd ON ad.PID = pd.PID

			--WHERE     (amc.chqstate IN (4))   and cdate between @fdate and   @tdate  and amc.BrchID= @brchid

			--union all

			--SELECT pd.PID, pd.PName, ad.Accno, ad.Aname, ad.CurrID, amc.cdate AS vdate, 'OBC Chq No.' + amc.chqno + ' Clerance pending' 

			--	AS notes,amc.camount AS cramt, 5 AS ttype, amc.postedby, amc.verifiedby, amc.BrchID AS tbrhno, amc.rno, amc.AVno,0 As IsVerify

			--FROM         AMClearanceHR amc 

			--	INNER JOIN ADetail ad ON amc.IAccno = ad.IAccno 

			--	INNER JOIN ProductDetail pd ON ad.PID = pd.PID

			--WHERE     (amc.chqstate IN (5))  and cdate between @fdate and   @tdate  and amc.BrchID= @brchid

			--union all

			--internal chq deposit rabindra

			SELECT ProductDetail.PID

				,ProductDetail.PName

				,ADetail.Accno

				,ADetail.Aname

				,ADetail.CurrID

				,IChkDep.Tdate

				,'note' = CASE 

					WHEN IChkDep.Ttype = 51

						THEN 'Transfered from ' + fin.GetAcNo(IChkDep.FIaccno) + ' Chq No ' + CAST(IChkDep.SlpNo AS NVARCHAR(10))

					ELSE 'Transfered from ' + fin.GetAcNo(IChkDep.FIaccno) + ' Chq No ' + CAST(IChkDep.SlpNo AS NVARCHAR(10)) + ' Pending'

					END

				,IChkDep.Amt AS Cramt

				,IChkDep.Ttype

				,IChkDep.postedby

				,IChkDep.verifiedby

				,IChkDep.BrchID

				,IChkDep.Tno

				,IChkDep.Vno

				,'IsVerify' = CASE 

					WHEN IChkDep.TType = 51

						THEN 1

					ELSE 0

					END

			FROM fin.IChkDep

			INNER JOIN fin.ProductDetail

			INNER JOIN fin.SchmDetails ON ProductDetail.SDID = SchmDetails.SDID

			INNER JOIN fin.ADetail ON ProductDetail.PID = ADetail.PID ON IChkDep.TIAccno = ADetail.IAccno WHERE (SchmDetails.SType = 0)

				AND (ADetail.BrchID = @brchid)

				AND (

					IChkDep.Tdate BETWEEN @fdate

						AND @tdate

					)

			

			UNION ALL

			

			SELECT pd.PID

				,pd.PName

				,ad.Accno

				,ad.Aname

				,ad.CurrID

				,ai.Vdate

				,'Int cap' AS notes

				,ai.IntAmt AS cramt

				,5 AS ttype

				,'' AS postedby

				,'' AS verifiedby

				,ad.BrchID

				,ai.Trxno

				,ai.Ivno

				,1 AS IsVerify

			FROM fin.AintCap ai

			INNER JOIN fin.ADetail ad ON ai.TIaccno = ad.IAccno

			INNER JOIN fin.ProductDetail pd ON ad.PID = pd.PID

			WHERE (ai.IsSlf = 1)

				AND (ai.FIaccno IS NULL)

				AND AI.Vdate BETWEEN @fdate

					AND @tdate

				AND ad.BrchID = @brchid

			

			UNION ALL

			

			--Interest Capitalization 

			SELECT pd.PID

				,pd.PName

				,ad.Accno

				,ad.Aname

				,ad.CurrID

				,ai.Vdate

				,'Int. Trns From ' + (

					SELECT fin.Getacno(Fiaccno)

					) AS notes

				,ai.CRamt

				,5 AS ttype

				,'' AS postedby

				,'' AS verifiedby

				,ad.BrchID

				,ai.Trxno

				,ai.Ivno

				,1 AS IsVerify

			FROM fin.AintCap ai

			INNER JOIN fin.ADetail ad ON ai.TIaccno = ad.IAccno

			INNER JOIN fin.ProductDetail pd ON ad.PID = pd.PID

			WHERE (ai.IsSlf = 0)

				AND (ai.FIaccno IS NOT NULL)

				AND AI.Vdate BETWEEN @fdate

					AND @tdate

				AND ad.BrchID = @brchid

	END

	IF @ISdep = 0

	BEGIN

		IF @srchtyp = 1 -- cash withdraw

			INSERT INTO @tempTbl

			SELECT pd.pid

				,pd.pname

				,ad.Accno

				,ad.Aname

				,ad.CurrID AS scurrid

				,At.vdate

				,'notes' = CASE 

					WHEN At.IsSlp = 1

						THEN 'Withdrawn By ' + At.notes + ' From Slip #' + Cast(At.Slpno AS NVARCHAR(10))

					ELSE 'Withdrawn By ' + At.notes + ' From Chq. No.' + Cast(At.Slpno AS NVARCHAR(10))

					END

				,At.dramt

				,At.ttype

				,At.postedby

				,At.approvedby

				,At.brnhno AS tbrhno

				,At.tno

				,At.vno

				,1 AS IsVerify

			FROM fin.ProductDetail pd

			INNER JOIN fin.ADetail ad ON pd.PID = ad.PID

			INNER JOIN fin.SchmDetails sd ON pd.SDID = sd.SDID

			INNER JOIN trans.AMTrns At ON ad.IAccno = At.IAccno

			WHERE (sd.SType = 0)

				AND ttype = 1

				AND vdate BETWEEN @fdate

					AND @tdate

				AND dramt > 0

				AND cramt = 0

				AND at.brnhno = @brchid

			--Unverified rabindra

			

			UNION ALL

			

			SELECT ProductDetail.PID

				,ProductDetail.PName

				,ADetail.Accno

				,ADetail.Aname

				,ADetail.CurrID

				,ASTrns.tdate

				,'notes' = CASE 

					WHEN IsSlp = 1

						THEN 'Unverified Withdrawn By ' + ASTrns.notes + ' From Slip #' + Cast(AStrns.Slpno AS NVARCHAR(10))

					ELSE 'Unverified Withdrawn By ' + ASTrns.notes + ' From Chq. No.' + Cast(AStrns.Slpno AS NVARCHAR(10))

					END

				,ASTrns.Dramt

				,ASTrns.ttype

				,ASTrns.postedby

				,'' AS Approvedby

				,ASTrns.brnhno

				,ASTrns.tno

				,ASTrns.VNO

				,0 AS IsVerify

			FROM trans.ASTrns

			INNER JOIN fin.ADetail ON ASTrns.IAccno = ADetail.IAccno

			INNER JOIN fin.ProductDetail ON ADetail.PID = ProductDetail.PID

			INNER JOIN fin.SchmDetails ON ProductDetail.SDID = SchmDetails.SDID

			WHERE (ASTrns.ttype = 1)

				AND (SchmDetails.SType = 0)

				AND (AStrns.Dramt > 0)

				AND (

					AStrns.tdate BETWEEN @fdate

						AND @tdate

					)

				AND ASTrns.Brnhno = @brchid

				AND (

					ASTrns.IsDeleted != 0

					OR ASTrns.IsDeleted IS NOT NULL

					)

		IF @srchtyp = 2 -- ABBS trxn withdraw

			INSERT INTO @tempTbl

			--ABBS Cash Withdrawn

			SELECT pd.pid

				,pd.pname

				,ad.Accno

				,ad.Aname

				,ad.CurrID AS scurrid

				,At.vdate

				,'notes' = CASE 

					WHEN Isslp = 0

						THEN 'ABBS Cash Withdrawn By ' + at.notes + 'From Chq. No ' + Cast(at.Slpno AS NVARCHAR(15))

					ELSE 'ABBS Cash Withdrawn By ' + at.notes + 'From Slip. No ' + Cast(at.Slpno AS NVARCHAR(15))

					END

				,At.Dramt

				,At.ttype

				,At.postedby

				,At.approvedby

				,At.brnhno AS tbrhno

				,At.tno

				,At.vno

				,1 AS IsVerify

			FROM fin.ProductDetail pd

			INNER JOIN fin.ADetail ad ON pd.PID = ad.PID

			INNER JOIN trans.AMTrns At ON ad.IAccno = At.IAccno

			INNER JOIN fin.SchmDetails sd ON pd.SDID = sd.SDID

			WHERE (sd.SType = 0)

				AND (ttype = 2)

				AND (

					vdate BETWEEN @fdate

						AND @tdate

					)

				AND (Dramt > 0)

				AND at.brnhno = @brchid

			

			UNION ALL

			

			SELECT pd.pid

				,pd.pname

				,ad.Accno

				,ad.Aname

				,ad.CurrID AS scurrid

				,At.tdate

				,'notes' = CASE 

					WHEN Isslp = 0

						THEN 'Unverified ABBS Cash Withdrawn By ' + at.notes + 'From Chq. No ' + Cast(at.Slpno AS NVARCHAR(15))

					ELSE 'Unverified ABBS Cash Withdrawn By ' + at.notes + 'From Slip. No ' + Cast(at.Slpno AS NVARCHAR(15))

					END

				,At.Dramt

				,At.ttype

				,At.postedby

				,'' approvedby

				,At.brnhno AS tbrhno

				,At.tno

				,At.vno

				,0 AS IsVerify

			FROM fin.ProductDetail pd

			INNER JOIN fin.ADetail ad ON pd.PID = ad.PID

			INNER JOIN trans.ASTrns At ON ad.IAccno = At.IAccno

			INNER JOIN fin.SchmDetails sd ON pd.SDID = sd.SDID

			WHERE (sd.SType = 0)

				AND (ttype = 2)

				AND (

					tdate BETWEEN @fdate

						AND @tdate

					)

				AND (Dramt > 0)

				AND at.brnhno = @brchid

				AND (

					at.IsDeleted != 0

					OR at.IsDeleted IS NOT NULL

					)

			

			UNION ALL

			

			--ABBS Internal Chq Deposit To Branch Ac

			SELECT ProductDetail.PID

				,ProductDetail.PName

				,ADetail.Accno

				,ADetail.Aname

				,ADetail.CurrID

				,IChkDep.Tdate

				,'notes' = CASE 

					WHEN IChkDep.VerifiedBy IS NOT NULL

						THEN 'ABBS Internal Chq Deposit By ' + fin.GetAcNo(Tiaccno) + ' from Chq. No.' + Cast(IChkDep.SlpNo AS NVARCHAR(15))

					ELSE 'Pending ABBS Internal Chq Deposit By ' + fin.GetAcNo(Fiaccno) + ' from Chq. No.' + Cast(IChkDep.SlpNo AS NVARCHAR(15))

					END

				,IChkDep.Amt DrAmt

				,IChkDep.Ttype

				,IChkDep.postedby

				,IChkDep.verifiedby

				,IChkDep.BrchID

				,IChkDep.Tno

				,IChkDep.Vno

				,'IsVerify' = CASE 

					WHEN IChkDep.VerifiedBy IS NOT NULL

						THEN 1

					ELSE 0

					END

			FROM fin.IChkDep

			INNER JOIN fin.ADetail ON IChkDep.TIaccno = ADetail.IAccno

			INNER JOIN fin.ProductDetail ON ADetail.PID = ProductDetail.PID

			INNER JOIN fin.SchmDetails ON ProductDetail.SDID = SchmDetails.SDID

			INNER JOIN fin.ADetail ADetail_1 ON IChkDep.FIAccno = ADetail_1.IAccno

				AND ADetail.BrchID <> ADetail_1.BrchID

			WHERE (

					IChkDep.tdate BETWEEN @fdate

						AND @tdate

					)

				AND ADetail_1.brchid = @brchid

		IF @srchtyp = 3 -- nON CASH WITHDRAW

			INSERT INTO @tempTbl

			--

			SELECT pd.pid

				,pd.pname

				,ad.Accno

				,ad.Aname

				,ad.CurrID AS scurrid

				,At.vdate

				,At.notes

				,At.dramt

				,At.ttype

				,At.postedby

				,At.approvedby

				,At.brnhno AS tbrhno

				,At.tno

				,At.vno

				,1 AS IsVerify

			FROM fin.ProductDetail pd

			INNER JOIN fin.ADetail ad ON pd.PID = ad.PID

			INNER JOIN trans.AMTrns At ON ad.IAccno = At.IAccno

			INNER JOIN fin.SchmDetails sd ON pd.SDID = sd.SDID

			WHERE (sd.SType = 0)

				AND ttype NOT IN (

					1

					,2

					)

				AND dramt > 0

				AND cramt = 0

				AND vdate BETWEEN @fdate

					AND @tdate

				AND at.brnhno = @brchid

			

			UNION ALL

			

			SELECT pd.PID

				,pd.PName

				,ad.Accno

				,ad.Aname

				,ad.CurrID

				,ai.Vdate

				,'Overdrawn Service Charge' AS notes

				,ABS(ai.IntAmt) AS dramt

				,5 AS TTYPE

				,'' AS PostedBy

				,'' AS ApprovedBy

				,ad.BrchID

				,ai.Trxno

				,ai.Ivno

				,1 AS IsVerify

			FROM fin.AintCap ai

			INNER JOIN fin.ADetail ad ON ai.TIaccno = ad.IAccno

			INNER JOIN fin.ProductDetail pd ON ad.PID = pd.PID

			WHERE (ai.IsSlf = 1)

				AND (ai.FIaccno IS NULL)

				AND (ai.IntAmt < 0)

				AND ai.vdate BETWEEN @fdate

					AND @tdate

				AND ad.BrchID = @brchid

			/*







union all















SELECT     pd.PID, pd.PName, ad.Accno, ad.Aname, ad.CurrID, ai.Vdate, 'Tax Ded.' AS notes, ai.Tax dramt,







	5 AS ttype, '' AS PostedBy, '' AS ApprovedBy, ad.BrchID, ai.Trxno,  ai.Tvno







FROM         dbo.AintCap ai 







	INNER JOIN dbo.ADetail ad ON ai.TIaccno = ad.IAccno 







	INNER JOIN dbo.ProductDetail pd ON ad.PID = pd.PID







WHERE(ai.IsSlf=1) AND (ai.FIaccno IS NULL) AND (ai.Tax<>0) and ai.vdate between @fdate and @tdate and ad.BrchID=@brchid







*/

			

			UNION ALL

			

			--internal Chq Deposit

			SELECT ProductDetail.PID

				,ProductDetail.PName

				,ADetail.Accno

				,ADetail.Aname

				,ADetail.CurrID

				,IChkDep.Tdate

				,'note' = CASE 

					WHEN IChkDep.Ttype = 51

						THEN 'Transfered To ' + fin.GetAcNo(IChkDep.TIaccno) + ' By Chq No ' + CAST(IChkDep.SlpNo AS NVARCHAR(10))

					ELSE 'Transfered To ' + fin.GetAcNo(IChkDep.TIaccno) + ' By Chq No ' + CAST(IChkDep.SlpNo AS NVARCHAR(10)) + ' Pending'

					END

				,IChkDep.Amt AS Dramt

				,IChkDep.Ttype

				,IChkDep.postedby

				,IChkDep.verifiedby

				,IChkDep.BrchID

				,IChkDep.Tno

				,IChkDep.Vno

				,'IsVerify' = CASE 

					WHEN IChkDep.TType = 51

						THEN 1

					ELSE 0

					END

			FROM fin.IChkDep

			INNER JOIN fin.ProductDetail

			INNER JOIN fin.SchmDetails ON ProductDetail.SDID = SchmDetails.SDID

			INNER JOIN fin.ADetail ON ProductDetail.PID = ADetail.PID ON IChkDep.FIaccno = ADetail.IAccno WHERE (ADetail.BrchID = @brchid)

				AND (

					IChkDep.Tdate BETWEEN @fdate

						AND @tdate

					)

			

			UNION ALL

			

			-----External Chq clearance

			--SELECT     pd.PID, pd.PName, ad.Accno, ad.Aname, ad.CurrID, am.fdate, 'OBC Chq No.' + am.chqno + ' Rejected' AS notes, 

			--		am.camount AS dramt, 5 AS ttype,  am.postedby, am.finalizedby, ad.BrchID, am.tno, am.RVno,1 As IsVerify

			--FROM         dbo.AMClearanceHR am 

			--	INNER JOIN dbo.ADetail ad ON am.IAccno = ad.IAccno 

			--	INNER JOIN dbo.ProductDetail pd ON ad.PID = pd.PID

			--WHERE     (am.chqstate = 5) and  am.fdate  between @fdate and @tdate   and   ad.BrchID=@brchid

			--union all

			--Charge Deduction

			SELECT pd.PID

				,pd.PName

				,ad.Accno

				,ad.Aname

				,ad.CurrID

				,chg.Tdate

				,chm.ChgName AS notes

				,chg.Amt

				,5 AS ttype

				,chg.postedby

				,chg.approvedby

				,ad.BrchID

				,chg.TrnxNo

				,chg.Vno

				,1 AS IsVerify

			FROM trans.ChgSTrnx chg

			INNER JOIN fin.ChargeDetail chm ON chg.ChgId = chm.ChgID

			INNER JOIN fin.ADetail ad ON chg.Iaccno = ad.IAccno

			INNER JOIN fin.ProductDetail pd ON ad.PID = pd.PID

			WHERE (chg.Iaccno IS NOT NULL)

				AND (

					chg.Iaccno IN (

						SELECT *

						FROM fin.FGetAccType(0) GetIAcNoOf

						)

					)

				AND chg.Tdate BETWEEN @fdate

					AND @tdate

				AND ad.BrchID = @brchid

			

			UNION ALL

			

			SELECT pd.PID

				,pd.PName

				,ad.Accno

				,ad.Aname

				,ad.CurrID

				,chg.Tdate

				,chm.ChgName AS notes

				,chg.Amt

				,5 AS ttype

				,chg.postedby

				,chg.approvedby

				,ad.BrchID

				,chg.TrnxNo

				,chg.Vno

				,1 AS IsVerify

			FROM trans.ChgSTrnx chg

			INNER JOIN fin.ChargeDetail chm ON chg.ChgId = chm.ChgID

			INNER JOIN fin.ADetail ad ON chg.Iaccno = ad.IAccno

			INNER JOIN fin.ProductDetail pd ON ad.PID = pd.PID

			WHERE (chg.Iaccno IS NOT NULL)

				AND (

					chg.Iaccno IN (

						SELECT *

						FROM fin.FGetAccType(0) GetIAcNoOf

						)

					)

				AND chg.Tdate BETWEEN @fdate

					AND @tdate

				AND ad.BrchID = @brchid

			

			UNION ALL

			

			---Tax Deduction

			SELECT pd.PID

				,pd.PName

				,ad.Accno

				,ad.Aname

				,ad.CurrID

				,ai.Vdate

				,'Tax Ded.' AS notes

				,ai.Tax dramt

				,5 AS ttype

				,'' AS PostedBy

				,'' AS ApprovedBy

				,ad.BrchID

				,ai.Trxno

				,ai.Tvno

				,1 AS IsVerify

			FROM fin.AintCap ai

			INNER JOIN fin.ADetail ad ON ai.TIaccno = ad.IAccno

			INNER JOIN fin.ProductDetail pd ON ad.PID = pd.PID

			WHERE (ai.IsSlf = 1)

				AND (ai.FIaccno IS NULL)

				AND (ai.Tax <> 0)

				AND ai.vdate BETWEEN @fdate

					AND @tdate

				AND ad.BrchID = @brchid

		IF @srchtyp = 4 -- nON CASH WITHDRAW all

			INSERT INTO @tempTbl

			SELECT pd.pid

				,pd.pname

				,ad.Accno

				,ad.Aname

				,ad.CurrID AS scurrid

				,At.vdate

				,'notes' = CASE 

					WHEN At.IsSlp = 1

						THEN 'Withdrawn By ' + At.notes + ' From Slip #' + Cast(At.Slpno AS NVARCHAR(10))

					ELSE 'Withdrawn By ' + At.notes + ' From Chq. No.' + Cast(At.Slpno AS NVARCHAR(10))

					END

				,At.dramt

				,At.ttype

				,At.postedby

				,At.approvedby

				,At.brnhno AS tbrhno

				,At.tno

				,At.vno

				,1 AS IsVerify

			FROM fin.ProductDetail pd

			INNER JOIN fin.ADetail ad ON pd.PID = ad.PID

			INNER JOIN trans.AMTrns At ON ad.IAccno = At.IAccno

			INNER JOIN fin.SchmDetails sd ON pd.SDID = sd.SDID

			WHERE (sd.SType = 0)

				AND ttype = 1

				AND vdate BETWEEN @fdate

					AND @tdate

				AND dramt > 0

				AND cramt = 0

				AND at.brnhno = @brchid

			--Unverified rabindra

			

			UNION ALL

			

			SELECT ProductDetail.PID

				,ProductDetail.PName

				,ADetail.Accno

				,ADetail.Aname

				,ADetail.CurrID

				,ASTrns.tdate

				,'notes' = CASE 

					WHEN IsSlp = 1

						THEN 'Unverified Withdrawn By ' + ASTrns.notes + ' From Slip #' + Cast(AStrns.Slpno AS NVARCHAR(10))

					ELSE 'Unverified Withdrawn By ' + ASTrns.notes + ' From Chq. No.' + Cast(AStrns.Slpno AS NVARCHAR(10))

					END

				,ASTrns.Dramt

				,ASTrns.ttype

				,ASTrns.postedby

				,'' AS Approvedby

				,ASTrns.brnhno

				,ASTrns.tno

				,ASTrns.VNO

				,0 AS IsVerify

			FROM trans.ASTrns

			INNER JOIN fin.ADetail ON ASTrns.IAccno = ADetail.IAccno

			INNER JOIN fin.ProductDetail ON ADetail.PID = ProductDetail.PID

			INNER JOIN fin.SchmDetails ON ProductDetail.SDID = SchmDetails.SDID

			WHERE (ASTrns.ttype = 1)

				AND (SchmDetails.SType = 0)

				AND (AStrns.Dramt > 0)

				AND (

					AStrns.tdate BETWEEN @fdate

						AND @tdate

					)

				AND ASTrns.Brnhno = @brchid

				AND (

					ASTrns.IsDeleted != 0

					OR ASTrns.IsDeleted IS NOT NULL

					)

			

			UNION ALL

			

			SELECT pd.pid

				,pd.pname

				,ad.Accno

				,ad.Aname

				,ad.CurrID AS scurrid

				,At.vdate

				,'notes' = CASE 

					WHEN Isslp = 0

						THEN 'ABBS Cash Withdrawn By ' + at.notes + 'From Chq. No ' + Cast(at.Slpno AS NVARCHAR(15))

					ELSE 'ABBS Cash Withdrawn By ' + at.notes + 'From Slip. No ' + Cast(at.Slpno AS NVARCHAR(15))

					END

				,At.Dramt

				,At.ttype

				,At.postedby

				,At.approvedby

				,At.brnhno AS tbrhno

				,At.tno

				,At.vno

				,1 AS IsVerify

			FROM fin.ProductDetail pd

			INNER JOIN fin.ADetail ad ON pd.PID = ad.PID

			INNER JOIN trans.AMTrns At ON ad.IAccno = At.IAccno

			INNER JOIN fin.SchmDetails sd ON pd.SDID = sd.SDID

			WHERE (sd.SType = 0)

				AND (ttype = 2)

				AND (

					vdate BETWEEN @fdate

						AND @tdate

					)

				AND (Dramt > 0)

				AND at.brnhno = @brchid

			

			UNION ALL

			

			SELECT pd.pid

				,pd.pname

				,ad.Accno

				,ad.Aname

				,ad.CurrID AS scurrid

				,At.tdate

				,'notes' = CASE 

					WHEN Isslp = 0

						THEN 'Unverified ABBS Cash Withdrawn By ' + at.notes + 'From Chq. No ' + Cast(at.Slpno AS NVARCHAR(15))

					ELSE 'Unverified ABBS Cash Withdrawn By ' + at.notes + 'From Slip. No ' + Cast(at.Slpno AS NVARCHAR(15))

					END

				,At.Dramt

				,At.ttype

				,At.postedby

				,'' approvedby

				,At.brnhno AS tbrhno

				,At.tno

				,At.vno

				,0 AS IsVerify

			FROM fin.ProductDetail pd

			INNER JOIN fin.ADetail ad ON pd.PID = ad.PID

			INNER JOIN trans.ASTrns At ON ad.IAccno = At.IAccno

			INNER JOIN fin.SchmDetails sd ON pd.SDID = sd.SDID

			WHERE (sd.SType = 0)

				AND (ttype = 2)

				AND (

					tdate BETWEEN @fdate

						AND @tdate

					)

				AND (Dramt > 0)

				AND at.brnhno = @brchid

				AND (

					at.IsDeleted != 0

					OR at.IsDeleted IS NOT NULL

					)

			

			UNION ALL

			

			--ABBS Internal Chq Deposit To Branch Ac

			SELECT ProductDetail.PID

				,ProductDetail.PName

				,ADetail.Accno

				,ADetail.Aname

				,ADetail.CurrID

				,IChkDep.Tdate

				,'notes' = CASE 

					WHEN IChkDep.VerifiedBy IS NOT NULL

						THEN 'ABBS Internal Chq Deposit By ' + fin.GetAcNo(Tiaccno) + ' from Chq. No.' + Cast(IChkDep.SlpNo AS NVARCHAR(15))

					ELSE 'Pending ABBS Internal Chq Deposit By ' + fin.GetAcNo(Fiaccno) + ' from Chq. No.' + Cast(IChkDep.SlpNo AS NVARCHAR(15))

					END

				,IChkDep.Amt DrAmt

				,IChkDep.Ttype

				,IChkDep.postedby

				,IChkDep.verifiedby

				,IChkDep.BrchID

				,IChkDep.Tno

				,IChkDep.Vno

				,'IsVerify' = CASE 

					WHEN IChkDep.VerifiedBy IS NOT NULL

						THEN 1

					ELSE 0

					END

			FROM fin.IChkDep

			INNER JOIN fin.ADetail ON IChkDep.TIaccno = ADetail.IAccno

			INNER JOIN fin.ProductDetail ON ADetail.PID = ProductDetail.PID

			INNER JOIN fin.SchmDetails ON ProductDetail.SDID = SchmDetails.SDID

			INNER JOIN fin.ADetail ADetail_1 ON IChkDep.FIAccno = ADetail_1.IAccno

				AND ADetail.BrchID <> ADetail_1.BrchID

			WHERE (

					IChkDep.tdate BETWEEN @fdate

						AND @tdate

					)

				AND ADetail_1.brchid = @brchid

			

			UNION ALL

			

			SELECT pd.pid

				,pd.pname

				,ad.Accno

				,ad.Aname

				,ad.CurrID AS scurrid

				,At.vdate

				,At.notes

				,At.dramt

				,At.ttype

				,At.postedby

				,At.approvedby

				,At.brnhno AS tbrhno

				,At.tno

				,At.vno

				,1 AS IsVerify

			FROM fin.ProductDetail pd

			INNER JOIN fin.ADetail ad ON pd.PID = ad.PID

			INNER JOIN trans.AMTrns At ON ad.IAccno = At.IAccno

			INNER JOIN fin.SchmDetails sd ON pd.SDID = sd.SDID

			WHERE (sd.SType = 0)

				AND ttype NOT IN (

					1

					,2

					)

				AND dramt > 0

				AND cramt = 0

				AND vdate BETWEEN @fdate

					AND @tdate

				AND at.brnhno = @brchid

			

			UNION ALL

			

			--Overdrawn Service Charge

			SELECT pd.PID

				,pd.PName

				,ad.Accno

				,ad.Aname

				,ad.CurrID

				,ai.Vdate

				,'Overdrawn Service Charge' AS notes

				,ABS(ai.IntAmt) AS dramt

				,5 AS TTYPE

				,'' AS PostedBy

				,'' AS ApprovedBy

				,ad.BrchID

				,ai.Trxno

				,ai.Ivno

				,1 AS IsVerify

			FROM fin.AintCap ai

			INNER JOIN fin.ADetail ad ON ai.TIaccno = ad.IAccno

			INNER JOIN fin.ProductDetail pd ON ad.PID = pd.PID

			WHERE (ai.IsSlf = 1)

				AND (ai.FIaccno IS NULL)

				AND (ai.IntAmt < 0)

				AND ai.vdate BETWEEN @fdate

					AND @tdate

				AND ad.BrchID = @brchid

			

			UNION ALL

			

			--Tax Deduction

			SELECT pd.PID

				,pd.PName

				,ad.Accno

				,ad.Aname

				,ad.CurrID

				,ai.Vdate

				,'Tax Ded.' AS notes

				,ai.Tax dramt

				,5 AS ttype

				,'' AS PostedBy

				,'' AS ApprovedBy

				,ad.BrchID

				,ai.Trxno

				,ai.Tvno

				,1 AS IsVerify

			FROM fin.AintCap ai

			INNER JOIN fin.ADetail ad ON ai.TIaccno = ad.IAccno

			INNER JOIN fin.ProductDetail pd ON ad.PID = pd.PID

			WHERE (ai.IsSlf = 1)

				AND (ai.FIaccno IS NULL)

				AND (ai.Tax <> 0)

				AND ai.vdate BETWEEN @fdate

					AND @tdate

				AND ad.BrchID = @brchid

			

			UNION ALL

			

			--internal Chq Deposit

			SELECT ProductDetail.PID

				,ProductDetail.PName

				,ADetail.Accno

				,ADetail.Aname

				,ADetail.CurrID

				,IChkDep.Tdate

				,'note' = CASE 

					WHEN IChkDep.Ttype = 51

						THEN 'Transfered To ' + fin.GetAcNo(IChkDep.TIaccno) + ' By Chq No ' + CAST(IChkDep.SlpNo AS NVARCHAR(10))

					ELSE 'Transfered To ' + fin.GetAcNo(IChkDep.TIaccno) + ' By Chq No ' + CAST(IChkDep.SlpNo AS NVARCHAR(10)) + ' Pending'

					END

				,IChkDep.Amt AS Dramt

				,IChkDep.Ttype

				,IChkDep.postedby

				,IChkDep.verifiedby

				,IChkDep.BrchID

				,IChkDep.Tno

				,IChkDep.Vno

				,'IVerify' = CASE 

					WHEN IChkDep.TType = 51

						THEN 1

					ELSE 0

					END

			FROM fin.IChkDep

			INNER JOIN fin.ProductDetail

			INNER JOIN fin.SchmDetails ON ProductDetail.SDID = SchmDetails.SDID

			INNER JOIN fin.ADetail ON ProductDetail.PID = ADetail.PID ON IChkDep.FIaccno = ADetail.IAccno WHERE (ADetail.BrchID = @brchid)

				AND (

					IChkDep.Tdate BETWEEN @fdate

						AND @tdate

					)

			

			UNION ALL

			

			--External Chq Rejected

			SELECT pd.PID

				,pd.PName

				,ad.Accno

				,ad.Aname

				,ad.CurrID

				,am.tdate

				,'OBC Chq No.' + am.chqno + ' Rejected' AS notes

				,am.camount AS dramt

				,5 AS ttype

				,am.postedby

				,am.postedby

				,ad.BrchID

				,am.rno AS Tno

				,NULL AS vno

				,1 AS IsVerify

			FROM fin.ASClearance am

			INNER JOIN fin.ADetail ad ON am.IAccno = ad.IAccno

			INNER JOIN fin.ProductDetail pd ON ad.PID = pd.PID

			WHERE (am.chqstate IS NOT NULL)

				AND am.VDate BETWEEN @fdate

					AND @tdate

				AND ad.BrchID = @brchid

			

			UNION ALL

			

			--Charge Deduction

			SELECT pd.PID

				,pd.PName

				,ad.Accno

				,ad.Aname

				,ad.CurrID

				,chg.Tdate

				,chm.ChgName AS notes

				,chg.Amt

				,5 AS ttype

				,chg.postedby

				,chg.approvedby

				,ad.BrchID

				,chg.TrnxNo

				,chg.Vno

				,0 AS IsVerify

			FROM trans.ChgSTrnx chg

			INNER JOIN fin.ChargeDetail chm ON chg.ChgId = chm.ChgID

			INNER JOIN fin.ADetail ad ON chg.Iaccno = ad.IAccno

			INNER JOIN fin.ProductDetail pd ON ad.PID = pd.PID

			WHERE (chg.Iaccno IS NOT NULL)

				AND (

					chg.Iaccno IN (

						SELECT *

						FROM fin.FGetAccType(0) GetIAcNoOf

						)

					)

				AND chg.Tdate BETWEEN @fdate

					AND @tdate

				AND ad.BrchID = @brchid

			

			UNION ALL

			

			SELECT pd.PID

				,pd.PName

				,ad.Accno

				,ad.Aname

				,ad.CurrID

				,chg.Tdate

				,chm.ChgName AS notes

				,chg.Amt

				,5 AS ttype

				,chg.postedby

				,chg.approvedby

				,ad.BrchID

				,chg.TrnxNo

				,chg.Vno

				,1 AS IsVerify

			FROM trans.ChgMTrnx chg

			INNER JOIN fin.ChargeDetail chm ON chg.ChgId = chm.ChgID

			INNER JOIN fin.ADetail ad ON chg.Iaccno = ad.IAccno

			INNER JOIN fin.ProductDetail pd ON ad.PID = pd.PID

			WHERE (chg.Iaccno IS NOT NULL)

				AND (

					chg.Iaccno IN (

						SELECT *

						FROM fin.FGetAccType(0) GetIAcNoOf

						)

					)

				AND chg.Tdate BETWEEN @fdate

					AND @tdate

				AND ad.BrchID = @brchid

	END

	RETURN

END
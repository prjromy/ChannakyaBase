

CREATE FUNCTION [fin].[FGetReportUserTeller] (

	@transDate SMALLDATETIME

	,@userId INT

	)

RETURNS TABLE ---MARKED ENCRYPTION  as  



RETURN (

		
SELECT Brhid
             
			,cf.UserId as UserId
						
			,Tno

			,TDate

			,cfd.TEXT +case when @userId=FUserid then ' :--Send To :- '  + user1.UserName  else ' :--Send By :- '  +usr.UserName end AS Tdesc

			,TType

			,Currid,
			case when @userId=cf.UserID then Dramt else 0 end 
			as Dramt,
			case when @userId=FUserid then Dramt else 0 end
			as Cramt
			,NULL AS Iaccno
	 
		FROM fin.CashFlow AS cf

		INNER JOIN fin.FGetCashFlowDictionary() cfd ON cf.TType = cfd.Id

		INNER JOIN lg.[User] usr ON usr.UserId = cf.FUserid
		inner join lg.[User] user1 on user1.UserId=cf.UserID
		inner join trans.ttype tt on tt.id=cf.status
		WHERE ttype IN (

				1

				,2

				,3

				,4

				,5

				,6

				)

			
		 and tdate =@transDate AND (FUserid=@userId or cf.UserId=@userId)
		 AND STATUS != 23
		

		
		--deno exchange remaining

		

		UNION ALL

		

		SELECT ad.BrchID

			,am.postedby

			,am.tno

			,am.tdate

			,TDesc = CASE 

				WHEN am.Cramt > 0

					AND am.dramt = 0

					THEN 'Dep. by ' + am.notes

				ELSE 'Widr by ' + am.notes + CASE 

						WHEN (am.Isslp = 0)

							THEN ' Via Chq# ' + cast(am.slpno AS NVARCHAR(10))

						ELSE ' Via Slp# ' + cast(am.slpno AS NVARCHAR(10))

						END

				END

			,am.ttype

			,ad.CurrID

			,am.cramt AS Dramt

			,am.dramt AS Cramt

			,am.IAccno

		FROM trans.AMTrns am

		INNER JOIN fin.ADetail ad ON am.IAccno = ad.IAccno

			AND am.brnhno = ad.BrchID

		WHERE (

				(

					am.ttype = 1

					AND am.cramt > 0

					)

				OR (

					am.ttype = 1

					AND am.dramt > 0

					)

				)

			AND am.postedby = @userId

			AND am.tdate = @transDate

		

		UNION ALL

		

		 SELECT ad.BrchID

			,ast.postedby

			,ast.tno

			,ast.tdate

			,TDesc = CASE 

				WHEN ast.Cramt > 0

					AND ast.dramt = 0

					THEN 'Dep. by ' + ast.notes

				ELSE 'Widr by ' + ast.notes + CASE 

						WHEN (ast.Isslp = 0)

							THEN ' Via Chq# ' + cast(ast.slpno AS NVARCHAR(10))

						ELSE ' Via Slp# ' + cast(ast.slpno AS NVARCHAR(10))

						END

				END

			,ast.ttype

			,ad.CurrID

			,ast.cramt AS Dramt

			,ast.dramt AS Cramt

			,ast.IAccno

		FROM Trans.ASTrns ast

		INNER JOIN fin.ADetail ad ON ast.IAccno = ad.IAccno

			AND ast.brnhno = ad.BrchID

		WHERE (

				ast.ttype = 1

				AND ast.cramt > 0
			OR 
				ast.ttype = 1

				AND ast.dramt > 0

				)

			AND ast.postedby = @userId
			AND ast.tdate = @transDate
			and (IsDeleted=0 or IsDeleted is null)
		

		UNION ALL

		

		SELECT am.brnhno

			,am.postedby

			,am.tno

			,am.tdate

			,TDesc = CASE 

				WHEN am.Cramt > 0

					AND am.dramt = 0

					THEN 'ABBS Dep. by ' + am.notes

				ELSE 'ABBS Widr by ' + am.notes + CASE 

						WHEN (am.Isslp = 0)

							THEN ' Via Chq# ' + cast(am.slpno AS NVARCHAR(10))

						ELSE ' Via Slp# ' + cast(am.slpno AS NVARCHAR(10))

						END

				END

			,am.ttype

			,ad.CurrID

			,am.cramt AS Dramt

			,am.dramt AS Cramt

			,am.IAccno

		FROM Trans.AMTrns am

		INNER JOIN fin.ADetail ad ON am.IAccno = ad.IAccno

			AND am.brnhno <> ad.BrchID

		WHERE (

				(

					am.ttype = 2

					AND am.cramt > 0

					)

				OR (

					am.ttype = 2

					AND am.dramt > 0

					)

				)

			AND TDate = @transDate

			AND am.postedby = @userId

		

		UNION ALL

		

		SELECT astrans.brnhno

			,astrans.postedby

			,astrans.tno

			,astrans.tdate

			,TDesc = CASE 

				WHEN astrans.Cramt > 0

					AND astrans.dramt = 0

					THEN 'ABBS Dep. by ' + astrans.notes

				ELSE 'ABBS Widr by ' + astrans.notes + CASE 

						WHEN (astrans.Isslp = 0)

							THEN ' Via Chq# ' + cast(astrans.slpno AS NVARCHAR(10))

						ELSE ' Via Slp# ' + cast(astrans.slpno AS NVARCHAR(10))

						END

				END

			,astrans.ttype

			,ad.CurrID

			,astrans.cramt AS Dramt

			,astrans.dramt AS Cramt

			,astrans.IAccno

		FROM trans.ASTrns astrans

		INNER JOIN fin.ADetail ad ON astrans.IAccno = ad.IAccno

			AND astrans.brnhno <> ad.BrchID

		WHERE astrans.ttype IN (2)

			AND (

				astrans.cramt > 0

				OR astrans.dramt > 0

				)

			AND astrans.postedby = @userId

			AND astrans.tdate = @transDate

			and (IsDeleted=0 or IsDeleted is null)
		

		UNION ALL

		

		SELECT ADetail.BrchID

			,AintPayable.PostedBy

			,AintPayable.Tno

			,AintPayable.Tdate

			,'Int Payable payment made' AS description

			,AintPayable.Valued AS ttype

			,ADetail.CurrID

			,0 AS dramt

			,AintPayable.DrAmt AS cramt

			,AintPayable.Iaccno

		FROM fin.AintPayable

		INNER JOIN fin.ADetail ON AintPayable.Iaccno = ADetail.IAccno

		WHERE (AintPayable.ByTeller = 1)

			AND (AintPayable.DrAmt > 0)

			AND valued IN (

				0

				,1

				)

			AND AintPayable.PostedBy = @userId

			AND AintPayable.Tdate = @transDate

		

		UNION ALL

		

		--=============

		--share details

		SELECT brchid AS brchid

			,PostedBy

			,Tno

			,pdate AS Tdate

			,note AS Description

			,Ttype

			,82 AS CurrID

			,Amt AS Dramt

			,0 AS Cramt

			,NULL AS Iaccno

		FROM fin.ShrPurchase

		INNER JOIN fin.ScrtDtls ON ShrPurchase.Scrtno = ScrtDtls.sid

		WHERE ShrPurchase.Amt > 0

			AND Shrpurchase.ttype IN (1)

			AND PostedBy = @userId

			AND Pdate = @transDate

		

		UNION ALL

		

		SELECT brchid AS brchid

			,PostedBy

			,Tno

			,pdate AS Tdate

			,note AS Description

			,Ttype

			,1 AS CurrID

			,Amt AS Dramt

			,0 AS Cramt

			,NULL AS Iaccno

		FROM fin.ShrSPurchase

		WHERE ShrSPurchase.Amt > 0

			AND ShrSpurchase.ttype IN (1)

			AND PostedBy = @userId

			AND Pdate = @transDate
			
			and (IsDeleted=0 or IsDeleted is null)
		

		

		UNION ALL

		

		SELECT brchid

			,PostedBy

			,Tno

			,sdate AS Tdate

			,note AS Description

			,Ttype

			,1 AS CurrID

			,0 AS Dramt

			,Amt AS Cramt

			,NULL AS Iaccno

		FROM fin.ShrReturn

		WHERE shrreturn.Amt > 0

			AND shrreturn.ttype IN (1)

			AND PostedBy = @userId

			AND Sdate = @transDate

		

		UNION ALL

		

		SELECT Brchid AS brchid

			,PostedBy

			,Tno

			,sdate AS Tdate

			,note AS Description

			,Ttype

			,1 AS CurrID

			,0 AS Dramt

			,Amt AS Cramt

			,NULL AS Iaccno

		FROM fin.shrSreturn

		WHERE shrSreturn.Amt > 0

			AND shrSreturn.ttype IN (1)

			AND PostedBy = @userId

			AND Sdate = @transDate

			and (IsDeleted=0 or IsDeleted is null)


union all

--=========

				--Charge details 

		SELECT brhid, 

		       chgstrnx.postedby, 

		       chgstrnx.trnxno                AS Tno, 

		       chgstrnx.tdate                 AS Tdate, 

		       chargedetail.chgname + ', ' + CASE WHEN chgstrnx.remarks<>'' THEN 

		       (chgstrnx.remarks) ELSE '' END AS tdesc, 

               ttype, 

		       1                              AS currid, 

		       0                              AS Dr, 

		       chgstrnx.amt                   AS Cr, 

		       chgstrnx.iaccno                AS Iaccno 

		FROM   trans.chgstrnx 

		       INNER JOIN fin.chargedetail 

		               ON chgstrnx.chgid = chargedetail.chgid 

		WHERE  chgstrnx.ttype = 3 

		       AND ( isdeleted is null or  isdeleted=0)

		       AND chgstrnx.postedby = @userId 

		       AND tdate = @transdate 

		UNION ALL 

		SELECT brhid, 

		       chgstrnx.postedby, 

		       chgstrnx.trnxno                 AS Tno, 

		       chgstrnx.tdate                  AS Tdate, 

		       + chargedetail.chgname + ' , ' 

		       + 'Paid in Cash By: ' + ad.accno + ', ' + CASE WHEN chgstrnx.remarks<>'' 

		       THEN ( 

		       chgstrnx.remarks) ELSE + '' END AS tdesc, 

			   ttype, 

		       1                               AS currid, 

		       0                               AS Dr, 

		       chgstrnx.amt                    AS Cr, 

		       chgstrnx.iaccno                 AS Iaccno 

		FROM   trans.chgstrnx 

		       INNER JOIN fin.chargedetail 

		               ON chgstrnx.chgid = chargedetail.chgid 

		       INNER JOIN fin.adetail ad 

		               ON ad.iaccno = chgstrnx.iaccno 

		WHERE  chgstrnx.ttype = 1 

		       AND( isdeleted is null or  isdeleted=0)

		       AND chgstrnx.postedby = @userId 

		       AND tdate = @transdate 

		UNION ALL 

		SELECT brhid, 

		       chgstrnx.postedby, 

		       chgstrnx.trnxno                      AS Tno, 

		       chgstrnx.tdate                       AS Tdate, 

		       + chargedetail.chgname + ' , ' 

		       + 'Deducted from Account: ' + ad.accno + CASE WHEN chgstrnx.remarks<>'' 

		       THEN ( 

		       ', '+chgstrnx.remarks) ELSE + '' END AS tdesc, 

		       ttype, 

		       1                                    AS currid, 

		       0                                    AS Dr, 

		       chgstrnx.amt                         AS Cr, 

		       chgstrnx.iaccno                      AS Iaccno 

		FROM   trans.chgstrnx 

		       INNER JOIN fin.chargedetail 

		               ON chgstrnx.chgid = chargedetail.chgid 

		       INNER JOIN fin.adetail ad 

		               ON ad.iaccno = chgstrnx.iaccno 

		WHERE  chgstrnx.ttype = 5 

			   AND( isdeleted is null or  isdeleted=0)

		       AND chgstrnx.postedby = @userId 

		       AND tdate = @transdate 

		UNION ALL 

		SELECT brhid, 

		       chgmtrnx.postedby, 

		       chgmtrnx.trnxno                AS Tno, 

		       chgmtrnx.tdate                 AS Tdate, 

		       chargedetail.chgname + ', ' + CASE WHEN chgmtrnx.remarks<>'' THEN 

		       (chgmtrnx.remarks) ELSE '' END AS tdesc, 

		       ttype, 

		       1                              AS currid, 

		       0                              AS Dr, 

		       chgmtrnx.amt                   AS Cr, 

		       chgmtrnx.iaccno                AS Iaccno 

		FROM   trans.chgmtrnx 

		       INNER JOIN fin.chargedetail 

		               ON chgmtrnx.chgid = chargedetail.chgid 

		WHERE  chgmtrnx.ttype = 3 

		       AND chgmtrnx.postedby = @userId 

		       AND tdate = @transdate 

		UNION ALL 

		SELECT brhid, 

		       chgmtrnx.postedby, 

		       chgmtrnx.trnxno                 AS Tno, 

		       chgmtrnx.tdate                  AS Tdate, 

		       + chargedetail.chgname + ' , ' 

		       + 'Paid in Cash By: ' + ad.accno + ', ' + CASE WHEN chgmtrnx.remarks<>'' 

		       THEN ( 

		       chgmtrnx.remarks) ELSE + '' END AS tdesc, 

		       ttype, 

		       1                               AS currid, 

		       0                               AS Dr, 

		       chgmtrnx.amt                    AS Cr, 

		       chgmtrnx.iaccno                 AS Iaccno 

		FROM   trans.chgmtrnx 

		       INNER JOIN fin.chargedetail 

		               ON chgmtrnx.chgid = chargedetail.chgid 

		       INNER JOIN fin.adetail ad 

		               ON ad.iaccno = chgmtrnx.iaccno 

		WHERE  chgmtrnx.ttype = 1 

		       AND chgmtrnx.postedby = @userId 

		       AND tdate = @transdate 

		UNION ALL 

		SELECT brhid, 

		       chgmtrnx.postedby, 

		       chgmtrnx.trnxno                      AS Tno, 

		       chgmtrnx.tdate                       AS Tdate, 

		       + chargedetail.chgname + ' , ' 

		       + 'Deducted from Account: ' + ad.accno + CASE WHEN chgmtrnx.remarks<>'' 

		       THEN ( 

		       ', '+chgmtrnx.remarks) ELSE + '' END AS tdesc, 

		       ttype, 

		       1                                    AS currid, 

		       0                                    AS Dr, 

		       chgmtrnx.amt                         AS Cr, 

		       chgmtrnx.iaccno                      AS Iaccno 

		FROM   trans.chgmtrnx 

		       INNER JOIN fin.chargedetail 

		               ON chgmtrnx.chgid = chargedetail.chgid 

		       INNER JOIN fin.adetail ad 

		               ON ad.iaccno = chgmtrnx.iaccno 

		WHERE  chgmtrnx.ttype = 5 

		       AND chgmtrnx.postedby = @userId 

		       AND tdate = @transdate 

				)

	--=======================

	-- remaining

	--Retrieve from FINVPRQUEUE 
CREATE function [fin].[FGetReportverifiedTransaction](@branchId int=null,@Tdate smalldatetime)

returns table as return

(

select * from

(

SELECT ProductDetail.PID



			,ProductDetail.PName AS ProductName

			,ADetail.Accno as accountnumber

			,ADetail.Aname as AccountName

			,ADetail.CurrID

			,AMTrns.tdate

			,CASE 

				WHEN AMTrns.cramt > 0

					THEN
					 CASE WHEN SType=1 THEN 'Loan Payment By '+isnull(AMTrns.notes,'-') 

                ELSE 'Deposit By ' + isnull(AMTrns.notes,'-') end

				ELSE 
				 CASE WHEN SType=1 THEN 'Loan Disburse By '+isnull(AMTrns.notes,'-') +' -:'+tt.TtypeDesc

                ELSE   'Withdraw By ' + isnull(AMTrns.notes,'-') 	 END
				 		

				END AS notes

			,AMTrns.cramt

			,AMTrns.dramt

			,AMTrns.ttype

			,AMTrns.brnhno as branchId

			,AMTrns.tno

			

			,CASE 

				WHEN AMTrns.dramt > 0

					THEN 'withdraw'

				ELSE



				 'Deposit'

				END AS flag,

				SchmDetails.SType

		FROM Trans.AMTrns

		INNER JOIN fin.ADetail ON AMTrns.IAccno = ADetail.IAccno

		INNER JOIN fin.ProductDetail ON ADetail.PID = ProductDetail.PID

		INNER JOIN fin.SchmDetails ON ProductDetail.SDID = SchmDetails.SDID

		inner join trans.TType tt on tt.Id=AMTrns.ttype

		where  AMTrns.brnhno = COALESCE(@branchId, AMTrns.brnhno) and tdate=@Tdate 

		

		UNION

		

 SELECT ProductDetail.PID

			,ProductDetail.PName

			,ADetail.Accno

			,ADetail.Aname

			,ADetail.CurrID

			,IChkDep.Tdate

			,'Transfered from ' + fin.GetAcNo(IChkDep.FIaccno) + ' Chq No ' + CAST(IChkDep.SlpNo AS NVARCHAR(10)) + ' verified' AS notes

			,IChkDep.Amt AS cramt

			,0 AS dramt

			,IChkDep.Ttype

			,IChkDep.BrchID

			,IChkDep.Tno

			,'Cheque Deposit' AS flag,

			SchmDetails.SType

		FROM fin.IChkDep

		INNER JOIN fin.ProductDetail

		INNER JOIN fin.SchmDetails ON ProductDetail.SDID = SchmDetails.SDID

		INNER JOIN fin.ADetail ON ProductDetail.PID = ADetail.PID ON IChkDep.TIAccno = ADetail.IAccno

		 WHERE 	Ttype = 1

			AND IChkDep.BrchID = COALESCE(@branchId, IChkDep.BrchID) and IChkDep.Tdate=@Tdate
	

		UNION
	
	SELECT pd.PID

			,pd.PName

			,ad.Accno

			,ad.Aname

			,ad.CurrID

			,chg.Tdate

			,chm.ChgName AS notes

			,0 AS cramt

			,chg.Amt AS dramt

			,5 AS ttype

			,cast(chg.brhid as int)

			,chg.TrnxNo

			,'charge' AS flag,

			sd.SType

		FROM trans.ChgMTrnx chg

		INNER JOIN fin.ChargeDetail chm ON chg.ChgId = chm.ChgID

		LEFT JOIN fin.ADetail ad ON chg.Iaccno = ad.IAccno

		LEFT JOIN fin.ProductDetail pd ON ad.PID = pd.PID

		left join fin.SchmDetails sd on sd.SDID=pd.SDID

	    WHERE chg.Tdate=@Tdate

		AND chg.brhid = COALESCE(@branchId, chg.brhid)

		

		UNION

		
 SELECT pd.PID

			,pd.PName

			,ad.Accno

			,ad.Aname

			,ad.CurrID

			,amc.vdate AS vdate

			,'OBC Chq No.' + amc.chqno + ' Cleared' AS notes

			,amc.camount AS cramt

			,0 AS dramt

			,5 AS ttype

			,amc.BrchID AS tbrhno

			,amc.rno

			,'Clearance' AS flag

			,sd.SType

		FROM fin.AMClearance amc

		INNER JOIN fin.ADetail ad ON amc.IAccno = ad.IAccno

		INNER JOIN fin.ProductDetail pd ON ad.PID = pd.PID

		left join fin.SchmDetails sd on sd.SDID=pd.SDID

	WHERE amc.tdate=@Tdate

		AND ad.brchid = COALESCE(@branchId, amc.BrchID)

		

		UNION

		

 SELECT ADetail.PID

			,ProductDetail.PName

			,ADetail.Accno

			,ADetail.Aname

			,ForeignBuySell.Incurr

			,ForeignBuySell.Tdate AS tdate

			,

			--   ForeignBuySell.BrnhId as branchId,

			ForeignBuySell.notes AS notes

			,ForeignBuySell.CrAmt AS cramt

			,ForeignBuySell.DrAmt AS dramt

			,- 1 AS ttype

			,ForeignBuySell.BrnhId

			,ForeignBuySell.TNo

			,'frxdep' AS flag

			,0 as SType

		-- ForeignBuySell.TransNo

		FROM fin.ForeignBuySell

		INNER JOIN fin.ADetail ON ForeignBuySell.IAccNo = ADetail.IAccno

		INNER JOIN fin.ProductDetail ON ADetail.PID = ProductDetail.PID

		WHERE  Tdate=@Tdate and ForeignBuySell.BrnhId = COALESCE(@branchId, ForeignBuySell.BrnhId)

		) trns

	)



	--select * from trans.AMTrns
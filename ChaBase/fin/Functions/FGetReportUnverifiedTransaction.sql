
CREATE FUNCTION [fin].[FGetReportUnverifiedTransaction] (@branchId INT = NULL)

RETURNS TABLE

AS

RETURN (

select * from (

 SELECT ProductDetail.PID

			,ProductDetail.PName AS ProductName

			,ADetail.Accno AS accountnumber

			,ADetail.Aname AS AccountName

			,ADetail.CurrID

			,ASTrns.tdate

			,CASE 

				WHEN ASTrns.cramt > 0 then

				 CASE WHEN SType=1 THEN 'Loan Payment By '+ASTrns.notes

                ELSE  'Deposit By ' + ASTrns.notes END

					

				ELSE 'Withdraw By ' + ASTrns.notes

				END AS notes

			,ASTrns.cramt

			,ASTrns.dramt

			,ASTrns.ttype

			,ASTrns.brnhno AS branchId

			,ASTrns.tno

			,CASE 

				WHEN ASTrns.dramt > 0

					THEN 'withdraw'

				ELSE 'Deposit'

				END AS flag

			,SchmDetails.SType

		FROM Trans.ASTrns

		INNER JOIN fin.ADetail ON ASTrns.IAccno = ADetail.IAccno

		INNER JOIN fin.ProductDetail ON ADetail.PID = ProductDetail.PID

		INNER JOIN fin.SchmDetails ON ProductDetail.SDID = SchmDetails.SDID

		WHERE ASTrns.IsDeleted IS NULL

			AND ASTrns.brnhno = COALESCE(@branchId, ASTrns.brnhno)

		

		UNION

		

 SELECT ProductDetail.PID

			,ProductDetail.PName

			,ADetail.Accno

			,ADetail.Aname

			,ADetail.CurrID

			,IChkDep.Tdate

			,'Transfered from ' + fin.GetAcNo(IChkDep.FIaccno) + ' Chq No ' + CAST(IChkDep.SlpNo AS NVARCHAR(10)) + ' Pending' AS notes

			,IChkDep.Amt AS cramt

			,0 AS dramt

			,IChkDep.Ttype

			,IChkDep.BrchID

			,IChkDep.Tno

			,'Cheque Deposit' AS flag

			,SchmDetails.SType

		FROM fin.IChkDep

		INNER JOIN fin.ProductDetail

		INNER JOIN fin.SchmDetails ON ProductDetail.SDID = SchmDetails.SDID

		INNER JOIN fin.ADetail ON ProductDetail.PID = ADetail.PID ON IChkDep.TIAccno = ADetail.IAccno WHERE (SchmDetails.SType = 0)

			AND Ttype = 0

			AND IChkDep.BrchID = COALESCE(@branchId, IChkDep.BrchID)

		

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

			,cast(chg.brhid AS INT) AS brhid

			,chg.TrnxNo

			,'charge' AS flag

			,sd.SType

		FROM trans.ChgSTrnx chg

		INNER JOIN fin.ChargeDetail chm ON chg.ChgId = chm.ChgID

		LEFT JOIN fin.ADetail ad ON chg.Iaccno = ad.IAccno

		LEFT JOIN fin.ProductDetail pd ON ad.PID = pd.PID

		LEFT JOIN fin.SchmDetails sd ON sd.SDID = pd.SDID

		WHERE chg.IsDeleted IS NULL

			AND chg.brhid = COALESCE(@branchId, chg.brhid)

		

		UNION

		

 SELECT pd.PID

			,pd.PName

			,ad.Accno

			,ad.Aname

			,ad.CurrID

			,amc.vdate AS vdate

			,'OBC Chq No.' + amc.chqno + ' Clerance pending' AS notes

			,amc.camount AS cramt

			,0 AS dramt

			,5 AS ttype

			,amc.BrchID AS tbrhno

			,amc.rno

			,'Clearance' AS flag

			,sd.SType

		FROM fin.AsClearance amc

		INNER JOIN fin.ADetail ad ON amc.IAccno = ad.IAccno

		INNER JOIN fin.ProductDetail pd ON ad.PID = pd.PID

		INNER JOIN fin.SchmDetails sd ON sd.SDID = pd.SDID

		WHERE (amc.chqstate IS NULL)

			AND ad.brchid = COALESCE(@branchId, amc.BrchID)

		

		UNION

		

 SELECT ADetail.PID

			,ProductDetail.PName

			,ADetail.Accno

			,ADetail.Aname

			,ForeignsBuySell.Incurr

			,ForeignsBuySell.Tdate AS tdate

			,

			--   ForeignsBuySell.BrnhId as branchId,

			ForeignsBuySell.notes AS notes

			,ForeignsBuySell.CrAmt AS cramt

			,ForeignsBuySell.DrAmt AS dramt

			,- 1 AS ttype

			,ForeignsBuySell.BrnhId

			,ForeignsBuySell.TNo

			,'frxdep' AS flag

			,0 AS Stype

		-- ForeignsBuySell.TransNo

		FROM fin.ForeignsBuySell

		INNER JOIN fin.ADetail ON ForeignsBuySell.IAccNo = ADetail.IAccno

		INNER JOIN fin.ProductDetail ON ADetail.PID = ProductDetail.PID

		WHERE ForeignsBuySell.BrnhId = COALESCE(@branchId, ForeignsBuySell.BrnhId)

		

		UNION

		

 SELECT ad.PID

			,PName

			,ad.Accno

			,Aname

			,CurrID

			,fd.PostedOn

			,'Loan Disburse By'+isnull(Remarks,'-')

			,0 AS cramt

			,fd.Amount AS dramt

			,0 AS ttype

			,ad.BrchID

			,dc.TNo

			,'LoanDisburse' AS flag

			,1 AS stype

		FROM fin.FGetLoanDisburseDetails() fd

		INNER JOIN fin.ADetail ad ON ad.IAccno = fd.IAccno

		INNER JOIN fin.ProductDetail pd ON pd.PID = ad.PID

		LEFT JOIN fin.DisburseCash dc ON dc.DisburseId = fd.DisburseId

		WHERE fd.VNo is null  and ad.BrchID = COALESCE(@branchId, ad.BrchID)

		)X

		)

	--sp_helptext [fin.FGetReportUnverifiedTransaction]
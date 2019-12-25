CREATE function [fin].[FGetReportAccountInterestDetails]()
returns table
as return
(
SELECT     TOP 100 PERCENT fin.ADetail.IAccno, RuleICB.ICBID, RuleICB.ICB,
	'intCalpSchmID' = (SELECT RuleICBDuration.ICBDurid
                       FROM fin.RuleICBDuration INNER JOIN
                            fin.AICBDur ON RuleICBDuration.ICBDurID = AICBDur.ICBDurID
                       WHERE Iaccno = ADETAIL.Iaccno),                           
	'intCalpSchm' =   (SELECT RuleICBDuration.ICBDurNam
                       FROM fin.RuleICBDuration INNER JOIN
                            fin.AICBDur ON RuleICBDuration.ICBDurID = AICBDur.ICBDurID
                       WHERE Iaccno = ADETAIL.Iaccno),
                       fin.ProductDetail.MID, 
	'NIAccno'	=      CASE WHEN RuleMovement.MID = 3 THEN
                          (SELECT NIAccno
                            FROM fin.ANomAcc
                            WHERE ANomAcc.IAccno = ADetail.IAccno) WHEN RuleMovement.MID = 5 THEN
                          (SELECT NIAccno
                            FROM fin.ANomAcc
                            WHERE ANomAcc.IAccno = ADetail.IAccno) ELSE NULL END, CustTaxsetup.TaxRate, fin.ADur.MatDat AS MatDate, 
					round( inlg.totint - inlg.expint,2) 
					AS IntToExp, round(inlg.expint - inlg.IpdSlf - inlg.IpdNom-inlg.IpdPybl ,2) AS IntToCap, ADetail.BrchId					 
					FROM fin.ADetail INNER JOIN
						fin.ProductDetail ON fin.ADetail.PID = fin.ProductDetail.PID INNER JOIN                     
						fin.TaxsetupDef as CustTaxsetup INNER JOIN
						cust.CustType ON CustTaxsetup.TaxID = CustType.TaxID INNER JOIN
						cust.CustInfo ON CustInfo.CtypeID = CustType.CtypeID INNER JOIN
						fin.AOfCust ON fin.AOfCust.CID = CustInfo.CID INNER JOIN
						fin.ADetail a ON a.IAccno = fin.AOfCust.IAccno ON fin.ADetail.IAccno = a.IAccno INNER JOIN
						fin.SchmDetails ON fin.ProductDetail.SDID = fin.SchmDetails.SDID INNER JOIN
						fin.RuleICB ON fin.SchmDetails.ICBID = fin.RuleICB.ICBID INNER JOIN
						fin.RuleMovement ON fin.SchmDetails.MID = fin.RuleMovement.MID INNER JOIN
						fin.FGetReportInterestDetails() as inlg ON fin.ADetail.IAccno = inlg.IAccno LEFT OUTER JOIN
						fin.ADur ON fin.ADetail.IAccno = fin.ADur.IAccno
					where fin.SchmDetails.stype=0 and aofcust.sno in (0,1)
					ORDER BY fin.ADetail.IAccno
)
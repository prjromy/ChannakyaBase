CREATE function [fin].[FGetReportShareTransactionByDate](@fromDate datetime,@toDate datetime)
returns table as 
return(
SELECT     'Purchase' as Pur , cusName.Name, ShrPurchase.Pdate as SDate, SCrtDtls.FSno, SCrtDtls.TSno,
SCrtDtls.SQty, ShrPurchase.Amt,case when SCrtDtls.SType=1 then 'Ordinary' else 'Promoter' end as ShareType
FROM         fin.ShrPurchase INNER JOIN
                      fin.ShrReg ON ShrPurchase.Regno = ShrReg.RegNo INNER JOIN
                      cust.FGetCustName() cusName ON ShrReg.CId = cusName.CID INNER JOIN
                      fin.SCrtDtls ON ShrPurchase.SCrtno = SCrtDtls.SId
WHERE     
  ShrPurchase.Pdate between @fromDate AND @toDate and ShrPurchase.ttype in( 1,5)
union all
SELECT      'Return' as Pur ,  cusName.Name, ShrReturn.Sdate as SDate, SCrtDtls.FSno, SCrtDtls.TSno, 
SCrtDtls.SQty, ShrReturn.Amt, case when SCrtDtls.SType=1 then 'Ordinary' else 'Promoter' end as ShareType
FROM         fin.ShrReturn INNER JOIN
                      fin.ShrReg ON ShrReturn.Regno = ShrReg.RegNo INNER JOIN
                      cust.FGetCustName() cusName ON ShrReg.CId = cusName.CID INNER JOIN
                      fin.SCrtDtls ON ShrReturn.SCrtno = SCrtDtls.SId
WHERE     
 
( ShrReturn.Sdate between @fromDate AND @toDate and (ShrReturn.ttype in( 1,5)) 
 
)
)
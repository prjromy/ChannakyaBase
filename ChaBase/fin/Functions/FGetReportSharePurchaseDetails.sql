create function [fin].[FGetReportSharePurchaseDetails](@fromDate datetime,@toDate datetime)
returns table 
as return(
 SELECT     ShrReg.RegNo AS RegNo, cusname.Name AS Name, ShrPurchase.Pdate AS PDate,dbo.FGetDateBS(ShrPurchase.Pdate) As 'PDateNep', SCrtDtls.SCrtno AS SCrtNo, SCrtDtls.FSno AS FSNo, 
                      fin.SCrtDtls.TSno AS TSNo, SCrtDtls.SQty AS SQty, ShrPurchase.Amt AS Amt, SCrtDtls.SType AS SType, SCrtDtls.Status AS Status, 
                      ShrPurchase.Note AS Note, ShrPurchase.ttype
FROM         fin.ShrPurchase INNER JOIN
                      fin.SCrtDtls ON ShrPurchase.SCrtno = SCrtDtls.SId INNER JOIN
                      fin.ShrReg ON ShrPurchase.Regno = ShrReg.RegNo INNER JOIN
                      cust.FGetCustName() cusname ON ShrReg.CId = cusname.CID
WHERE     (ShrPurchase.ttype in( 1,5)) and ( ShrPurchase.Pdate between @fromDate and @toDate)  
)
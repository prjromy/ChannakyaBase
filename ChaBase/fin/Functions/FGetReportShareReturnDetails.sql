create function [fin].[FGetReportShareReturnDetails](@fromDate datetime,@toDate datetime)
returns table 
as return(
SELECT     cusname.Name, ShrReg.RegNo, ShrReturn.SCrtNo, SCrtDtls.FSno, SCrtDtls.TSno, ShrReturn.SQty, ShrReturn.Amt, SCrtDtls.SType, ShrReturn.ttype, 
                      ShrReturn.SoldTo, ShrReturn.Sdate,dbo.FGetDateBS(ShrReturn.Sdate) As 'SDateNep',ShrReturn.Note
FROM         fin.ShrReturn INNER JOIN
                      fin.ShrReg ON ShrReturn.Regno = ShrReg.RegNo INNER JOIN
                      cust.FGetCustName() cusname ON ShrReg.CId = cusname.CID INNER JOIN
                      fin.SCrtDtls ON ShrReturn.SCrtno = SCrtDtls.SId

WHERE     (ShrReturn.Sdate BETWEEN  @fromDate and @toDate)
)
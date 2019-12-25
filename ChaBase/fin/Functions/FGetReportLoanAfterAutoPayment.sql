create FUNCTION [fin].[FGetReportLoanAfterAutoPayment]

	 (@TDATE datetime,@FDate datetime,@branchId INT= null)

RETURNS  TABLE 

---MARKED ENCRYPTION 	AS

return(





SELECT AMTrns.tno,AMTrns.tdate,AMTrns.brnhno, ProductDetail.PID AS LPid, ProductDetail.PName AS LPName, ProductDetail_1.PID AS DPid, ProductDetail_1.PName AS DPName, 

                       ADetail.IAccno AS LIAccno,  ADetail.Accno AS LAccno, cname.Name, ADetail_1.IAccno AS DIAccno, ADetail_1.Accno AS DAccno, 

                      AMTrns_1.Dramt,

		      'Prin'=(SELECT SUM(ISNULL(amt,0)) FROM	fin.AMTransLoan where AMTransLoan.tno=AMTrns.tno and AMTransLoan.pid=13),
													 									 
		      'Int'=(SELECT SUM(ISNULL(amt,0))	FROM	fin.AMTransLoan where AMTransLoan.tno=AMTrns.tno and AMTransLoan.pid in (0,5)),
														 									 
		      'Penal'=(SELECT SUM(ISNULL(amt,0))FROM	fin.AMTransLoan where AMTransLoan.tno= AMTrns.tno and AMTransLoan.pid in (1,3,4,6,8,9)),
													 										   
		      'Charge'=(SELECT SUM(ISNULL(amt,0))FROM	fin.AMTransLoan where AMTransLoan.tno=AMTrns.tno and AMTransLoan.pid in (2,7,14))

FROM         fin.ADetail INNER JOIN

                      trans.AMTrns ON ADetail.IAccno =  AMTrns.IAccno INNER JOIN

                      fin.ProductDetail ON  ADetail.PID =  ProductDetail.PID INNER JOIN

                      fin.AOfCust ON  ADetail.IAccno =  AOfCust.IAccno INNER JOIN

                      cust.FGetCustName() cname ON AOfCust.CID = cname.CID INNER JOIN

                      trans.TranLnk ON trans.AMTrns.tno = trans.TranLnk.tno2 INNER JOIN

                      trans.AMTrns AMTrns_1 INNER JOIN

                      fin.ADetail ADetail_1 ON AMTrns_1.IAccno = ADetail_1.IAccno ON trans.TranLnk.tno1 = AMTrns_1.tno INNER JOIN

                      fin.ProductDetail ProductDetail_1 ON ADetail_1.PID = ProductDetail_1.PID

		      WHERE AMTrns.tdate between @FDate and @tdate AND AMTrns.brnhno =coalesce( @branchId,amtrns.brnhno)
			  )
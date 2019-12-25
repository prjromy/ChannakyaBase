CREATE FUNCTION [fin].[FGetReportShareStatement]

(

	@REGNO INT

)

RETURNS TABLE

---MARKED ENCRYPTION  

As

Return

(

	Select  1 As Pur,ShrReg.CId, ShrReg.RegNo, cusname.Name ,SCrtDtls.SCrtno, ShrPurchase.Pdate As PDate,

			SCrtDtls.FSno, SCrtDtls.TSno, SCrtDtls.SQty As PurQty, 0 As RetQty, ShrPurchase.Amt As Amount,

			SCrtDtls.SType, CustAddress.location, C.GFatherName, C.FatherName, ShrPurchase.Tno

	From fin.ShrPurchase 

	Inner Join fin.SCrtDtls On ShrPurchase.SCrtno = SCrtDtls.SId 

	Inner Join fin.ShrReg On ShrPurchase.Regno = ShrReg.RegNo 

	Inner Join cust.FGetCustName() cusname On ShrReg.CId = cusname.CID 

	Left Join cust.FGetCustLocation() As CustAddress On cusname.CID = CustAddress.CID 

	Left Join  cust.CustIndividual C On cusname.CID = C.CID

	Where     (ShrReg.RegNo = @RegNo) And (ShrPurchase.ttype In(1,5))



	union all

	

	Select  0 As Pur,ShrReg.CId, ShrReg.RegNo, cusname.Name , SCrtDtls.SCrtno, ShrReturn.Sdate As SDate, 

		 

			SCrtDtls.FSno, SCrtDtls.TSno, 0 As PurQty, SCrtDtls.SQty As RetQty,  ShrReturn.Amt * - 1 as Amount ,

			SCrtDtls.SType,CustAddress.location, C.GFatherName, C.FatherName, ShrReturn.Tno

	From  fin.ShrReturn 

	Inner Join fin.ShrReg On ShrReturn.Regno = ShrReg.RegNo 

	Inner Join cust.FGetCustName() cusname  On ShrReg.CId = cusname.CID 

	Inner Join fin.SCrtDtls On ShrReturn.SCrtno = SCrtDtls.SId 

	Left Join cust.FGetCustLocation() As CustAddress On cusname.CID = CustAddress.CID 

	Left Join  cust.CustIndividual C On cusname.CID = C.CID

	Where     (ShrReg.RegNo = @RegNo) And (ShrReturn.ttype In(1,5))

)
CREATE FUNCTION [fin].[FGetReportInterestCapitalization]

(

	@FromDate SMALLDATETIME,

	@ToDate SMALLDATETIME,

	@BranchId int=null

)

RETURNS TABLE

---MARKED ENCRYPTION 

As

Return

Select ADetail.AName As CustName,AIntCap.VDate,AIntCap.IntAmt,AIntCap.Tax,AIntCap.TaxRt,AIntCap.CRamt,'Self' As ToAccNo,

		ADetail.AccNo As FromAccNo,AIntCap.IsSlf,ADetail.IAccNo,ADetail.PId,ProductDetail.PName,SchmDetails.SDName

From fin.AIntCap 

Inner Join fin.ADetail On AIntCap.TIaccNo = ADetail.IAccNo 

Inner Join fin.ProductDetail On ADetail.PId = ProductDetail.PId 

Inner Join fin.SchmDetails On ProductDetail.SDId = SchmDetails.SDId

Where (AIntCap.IsSlf = 1) And (VDate Between @FromDate And @ToDate) 
And (Adetail.BrchId =coalesce(@BranchId,Adetail.BrchId))

Union All

Select ADetail.AName As CustName,AIntCap.VDate,AIntCap.IntAmt,AIntCap.Tax,AIntCap.TaxRt,AIntCap.CRamt,ADetail_1.AccNo As ToAccNo,

		ADetail.AccNo As FromAccNo,AIntCap.IsSlf,ADetail.IAccNo,ADetail.PId,ProductDetail.PName,SchmDetails.SDName

From fin.ProductDetail 

Inner Join fin.ADetail On ProductDetail.PID = ADetail.PID 

Inner Join fin.SchmDetails On ProductDetail.SDID = SchmDetails.SDID 

Inner Join fin.AIntCap On ADetail.IAccno = AIntCap.FIaccno 

Inner Join fin.ADetail ADetail_1 On AIntCap.TIaccno = ADetail_1.IAccno

Where (AIntCap.IsSlf = 0) And (vdate Between @FromDate And @ToDate) And (Adetail.BrchId  =coalesce(@BranchId,Adetail.BrchId))

Union All

Select ADetail.AName As CustName,AintPayable.TDate As VDate,AintPayable.IntAmt,AintPayable.Tax,AintPayable.TaxRt,AintPayable.CRamt,

		'To Payable ' As ToAccNo,ADetail.AccNo As FromAccNo, 0 As Isslf,ADetail.IAccNo,ProductDetail.PId,ProductDetail.PName,SchmDetails.SDName

From fin.AintPayable 

Inner Join fin.ADetail On AintPayable.IAccNo = ADetail.IAccNo 

Inner Join fin.ProductDetail On ADetail.PId = ProductDetail.PId 

Inner Join fin.SchmDetails On ProductDetail.SDId = SchmDetails.SDId

Where (TDate Between @FromDate And @ToDate) And (Adetail.BrchId =coalesce(@BranchId,Adetail.BrchId))
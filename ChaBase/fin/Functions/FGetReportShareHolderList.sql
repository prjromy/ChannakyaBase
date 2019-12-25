 

CREATE FUNCTION [fin].[FGetReportShareHolderList]
	(
		@BRCHID INT,--,@TDate SmallDateTime
		@Stype INT= null

	)


RETURNS TABLE
As
Return
	Select SType,ShrReg.RegNo,cusname.Name,Sum(SQtyP) As 'SQtyP',Sum(AmtP) As 'AmtP',Sum(SQtyR) As 'SQtyR',Sum(AmtR) As 'AmtR',
		Sum(SQtyP - SQtyR) As 'SQty',Sum(AmtP - AmtR) As 'Balance' 
	From cust.FGetCustName() cusname
	Inner Join fin.ShrReg On cusname.CID = ShrReg.CID
	Inner Join
		(
		Select SType,RegNo,Sum(ShrPurchase.SQty) As 'SQtyP',Sum(Amt) As 'AmtP',0 As 'SQtyR',0 As 'AmtR'
			From fin.ShrPurchase 
		Inner Join fin.ScrtDtls On ShrPurchase.SCrtno = SCrtDtls.Sid

		inner join lg.Company cmp on cmp.companyid=ShrPurchase.brchid
		Where ShrPurchase.TType In (1,5) And ShrPurchase.brchid = coalesce(@BRCHID, cmp.companyid) and Stype=coalesce(@Stype,stype)
		Group By SType,RegNo,brchid
		Union All 
		Select SType,RegNo,0 As 'SQtyP',0 As 'AmtP',Sum(ShrReturn.SQty) As 'SQtyR',Sum(Amt) As 'AmtR'
			From fin.ShrReturn
		Inner Join fin.ScrtDtls On ShrReturn.SCrtno = SCrtDtls.Sid
		inner join lg.Company cmp on cmp.companyid=ShrReturn.brchid
		Where ShrReturn.TType In (1,5) And ShrReturn.brchid  = coalesce(@BRCHID, cmp.companyid) and Stype=coalesce(@Stype,stype)--And SDate <= '7/16/2014'
		Group By SType,RegNo,brchid
		) X On ShrReg.RegNo = X.RegNo
	Group By SType,ShrReg.RegNo,[Name]
	Having Sum(AmtP - AmtR) <> 0
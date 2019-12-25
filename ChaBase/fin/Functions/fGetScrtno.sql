
create FUNCTION [fin].[fGetScrtno]
	(
		@REGNO NUMERIC,
		@STYPE NUMERIC
	)
RETURNS NUMERIC 
---MARKED ENCRYPTION 
As 
Begin

Declare @ScrtNo Numeric
If  IsNull((Select Max(ScrtNo) ScrtNo From 
		(Select RegNo,ScrtNo,Sum(SQtyP) SQtyP,Sum(SQtyR) SQtyR,Sum(SQtyP) - Sum(SQtyR) As SQtyB From (
		Select RegNo,SCrtDtls.ScrtNo,ShrPurchase.SQty As SQtyP ,0 As SQtyR From fin.ShrPurchase
			Inner Join fin.SCrtDtls On ShrPurchase.ScrtNo = ScrtDtls.SId
		Where RegNo = @RegNo And SType = @SType
		Union All
		Select RegNo,SCrtDtls.ScrtNo,0 As SQtyP,ShrReturn.SQty As SQtyR From fin.ShrReturn
		Inner Join fin.SCrtDtls On ShrReturn.ScrtNo = ScrtDtls.SId
		Where RegNo = @RegNo And SType = @SType) X
		Group By RegNo,ScrtNo
		Having Sum(SQtyP) - Sum(SQtyR) > 0) Y),0) <> 0
	Begin
		Set @Scrtno = (Select Max(ScrtNo) ScrtNo From 
					(Select RegNo,ScrtNo,Sum(SQtyP) SQtyP,Sum(SQtyR) SQtyR,Sum(SQtyP) - Sum(SQtyR) As SQtyB From (
					Select RegNo,SCrtDtls.ScrtNo,ShrPurchase.SQty As SQtyP ,0 As SQtyR From fin.ShrPurchase
						Inner Join fin.SCrtDtls On ShrPurchase.ScrtNo = ScrtDtls.SId
					Where RegNo = @RegNo And SType = @SType
					Union All
					Select RegNo,SCrtDtls.ScrtNo,0 As SQtyP,ShrReturn.SQty As SQtyR From fin.ShrReturn
					Inner Join fin.SCrtDtls On ShrReturn.ScrtNo = ScrtDtls.SId
					Where RegNo = @RegNo And SType = @SType) X
					Group By RegNo,ScrtNo
					Having Sum(SQtyP) - Sum(SQtyR) > 0) Y)
	End
Else 
Begin
	Set @Scrtno = (Select IsNull(Max(Scrtno)+1,(Select CrtNo + 1 From mast.Shrsetup))From fin.SCrtDtls)
End
Return @Scrtno
End
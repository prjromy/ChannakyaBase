
CREATE FUNCTION [fin].[FGETSHARERETURN](@REGNO INT,@BrchID Int)
RETURNS TABLE 
---MARKED ENCRYPTION
as
return 
Select x.Sid,ShrReg.RegNo,[Name],x.Scrtno, X.fsno as [From],X.Tsno as [To],cast(Stype as int) as ShareType,
Sum(SQtyP - SQtyR) As 'Qty', 
Stype = Case When  ( X.SType = 0 ) Then 'Ordinary Share' else  'Promoter Share' End,
Sum(SQtyP) As 'SQtyP',Sum(AmtP) As 'AmtP',Sum(SQtyR) As 'SQtyR',Sum(AmtR) As 'AmtR',
	Sum(AmtP - AmtR) As 'Balance' 
From [cust].[FGetCustName]() c 
Inner Join fin.ShrReg On c.CID = ShrReg.CID
Inner Join
	(
	Select SType,RegNo,Sid,ScrtDtls.Scrtno,ScrtDtls.fsno,ScrtDtls.Tsno, 
	Sum(ShrPurchase.SQty) As 'SQtyP',Sum(Amt) As 'AmtP',0 As 'SQtyR',0 As 'AmtR'
		From fin.ShrPurchase 
	Inner Join fin.ScrtDtls On ShrPurchase.SCrtno = SCrtDtls.Sid
	Where ShrPurchase.TType In (1,2,5) And brchid = @BrchID
	Group By SType,RegNo,Sid,ScrtDtls.Scrtno,ScrtDtls.fsno,ScrtDtls.Tsno

	Union All 

	Select SType,RegNo,Sid,ScrtDtls.Scrtno,ScrtDtls.fsno,ScrtDtls.Tsno, 
	0 As 'SQtyP',0 As 'AmtP',Sum(ShrReturn.SQty) As 'SQtyR',Sum(Amt) As 'AmtR'
		From fin.ShrReturn
	Inner Join fin.ScrtDtls On ShrReturn.SCrtno = SCrtDtls.Sid
	Where ShrReturn.TType In (1,2,5) and brchid = @BrchID
	Group By SType,RegNo,Sid,ScrtDtls.Scrtno,ScrtDtls.fsno,ScrtDtls.Tsno
	) X On ShrReg.RegNo = X.RegNo
where x.regno=@Regno
Group By SType,ShrReg.RegNo,[Name],x.Sid, x.Scrtno,X.fsno,X.Tsno
having Sum(AmtP - AmtR)> 0
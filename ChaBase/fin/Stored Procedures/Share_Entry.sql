CREATE PROCEDURE [fin].[Share_Entry]
	(			
		@TNO NUMERIC,	 			
		@APPROVEDBY INT			
	)
--With ENCRYPTION  
AS

BEGIN TRANSACTION [Tran1]

Declare @FSno Numeric
Declare @TSno Numeric
Declare @Scrtno Int
Declare @Sid Numeric
Declare @Amt Money
Declare @RegNo int
Declare @SType TINYINT
Declare @SQty int

select @SQty=SQty,@RegNo=Regno,@SType=SType from fin.ShrSPurchase where Tno=@TNO

Set @FSno = (Select IsNull(Max(TSno)+1,(Select Crtno+1 From mast.ShrSetup)) From fin.SCrtDtls)
Set @TSno = @FSno + @SQty - 1
Set @Amt  =  @SQty * (Select ShrRate from mast.ShrSetup)

Set @ScrtNo =  (Select fin.FGetScrtNo(@RegNo,@SType))

Insert Into fin.SCrtDtls(SCrtNo, FSno, TSno, SQty, STatus,Stype) select @Scrtno,@FSNo,@TSNo,SQty,1,SType from fin.ShrSPurchase where Tno=@TNO
Set @Sid = @@Identity
Insert Into fin.ShrPurchase (Regno,Scrtno,Tno,Pdate,SQty,Amt,Note,PostedBy,ApprovedBy,Ttype,BrchID,VNo) select Regno,@Sid,Tno,Pdate,SQty,Amt,Note,PostedBy,@APPROVEDBY,ttype,Brchid,Vno from fin.ShrSPurchase where Tno=@TNO

delete from fin.ShrSPurchase where Tno=@TNO


COMMIT TRANSACTION [Tran1]

CREATE PROCEDURE [fin].[PVerifyTran]
	(@tno 	bigint, @valueDate datetime,@approveby int,@branchId int)

as

--select * from Trans.AMTrns
BEGIN TRANSACTION [Tran1]

declare @iaccno int

declare @ioncr money
declare @ionca money
declare @othrbal money
declare @othrbaldr money
declare @ponir money
declare @ponprr money
declare @ionir money
declare @ponia money
declare @ponpra money
declare @ionia money
declare @ionpr money
declare @ionpa money
declare @princCr money
set @ioncr=0
set @ionca=0
set @othrbal=0
set @ponir=0
set @ponprr=0
set @ionir=0
set @ponia=0
set @ponpra=0
set @ionia=0
set @ionpr=0
set @ionpa=0
set @princCr=0

declare @RebateOnPenalty money
declare @RebateOnInterest money
Declare @PId int , @Amt money

Declare curX cursor for
select pid, Amt  from fin.ASTransLoan where tno  = @Tno
open curX
fetch from curX into @pid, @Amt
while @@FETCH_STATUS = 0
begin
if @pid=7
	set @ioncr=@Amt
else if @pid=2
	set @ionca=@Amt
else if @pid=14
	set @othrbal=@Amt
else if @pid=11
	set @othrbaldr=@Amt
else if @pid=9
	set @ponir=@Amt
else if @pid=8
	set @ponprr=@Amt
else if @pid=6
	set @ionir=@Amt
else if @pid=4
	set @ponia=@Amt
else if @pid=3
	set @ponpra=@Amt
else if @pid=1
	set @ionia=@Amt
else if @pid =5
	set @ionpr=@Amt
else if @pid=0
	set @ionpa=@Amt
	else if @pid=13
	 set @princCr=@Amt
	 else if @pid=12
	 set @RebateOnInterest=@Amt
	 else if @pid=17
	 set @RebateOnPenalty=@Amt

	fetch next from curX into @pid, @Amt
end
CLOSE  curX
	DEALLOCATE  curX

set @iaccno=(select IAccno from  Trans.ASTrns where tno=@tno )


update fin.ALoan set 
	ioncr=ioncr-@ioncr,
	ionca=ionca-@ionca,
	othrbal=isnull(othrbal,0)-@othrbal,
	ponir=ponir-@ponir,
	ponprr=ponprr-@ponprr,
	ionir=ionir-@ionir,
	ponia=ponia-@ponia,
	ponpra=ponpra-@ponpra,
	ionia=ionia-@ionia,
	ionpr=ionpr-@ionpr,
	ionpa=ionpa-@ionpa
	
where iaccno=@iaccno
 
INSERT INTO Trans.AMTrns SELECT tno, IAccno, tdate, @valueDate, notes, slpno, dramt, cramt, ttype, postedby, @approveby, brnhno, vno, IsSlp,PostedOn from Trans.ASTrns where ASTrns.tno=@tno

 insert into fin.AMTransLoan select  tno,PID, Amt,vNO from fin.ASTransLoan WHERE ASTransLoan.tno=@tno

IF @ionpa > 0 OR @ionpr>0
BEGIN
	DECLARE @tdate DATETIME
		
		SET @tdate = (SELECT tdate FROM Trans.ASTrns WHERE tno = @tno )
		DECLARE @intAmt MONEY
		SET @intAmt = @ionpa+@ionpr

		EXECUTE fin.PPymUpdIntSch @iaccno,@intAmt,@tdate
	
END

DECLARE @prin MONEY
DECLARE @revolving BIT

SET @revolving = (SELECT revolving FROM fin.ALoan WHERE iaccno = @iaccno)
if @princCr>0
begin 	

update adetail set Bal=(bal-@princCr) where iaccno=@iaccno
		
exec fin.PUpdateIOnBalOfAccBaseOnLoanTransaction @valueDate,@iaccno
end

IF (@princCr>0) AND @revolving = 1
BEGIN
DECLARE @balanceAmt money
	SELECT     @balanceAmt = Bal
		FROM fin.ALoan INNER JOIN fin.ADetail ON fin.ALoan.IAccno = fin.ADetail.IAccno
		WHERE     (fin.ALoan.iaccno = @iaccno)
	
	UPDATE alSch set schPrin=0, balPrin=@balanceAmt where iaccno=@iAccno
	UPDATE alSch set schPrin=@balanceAmt, balPrin=0 where iaccno=@iAccno and schDate = (select max(schDate) from alsch where iaccno=@iaccno)		  
	UPDATE alsch set pdPrin=NULL where iaccno=@iAccno

	update fin.ALoan set 
	PrincipleLoanOut=isnull(PrincipleLoanOut,0)-@princCr	
    where iaccno=@iaccno
END
ELSE IF (@princCr>0 AND @revolving = 0)
BEGIN	
	EXECUTE fin.PPymUpdPrinSch @iaccno,@princCr
END
 			       
EXECUTE [fin].[PAloanTraInsert] @tno
update fin.ADetail set LastTransDate=(select top 1 tdate from lg.Company where CompanyId=@branchId) where IAccno=@iaccno
DELETE FROM fin.ASTransLoan where tno=@tno
DELETE FROM Trans.ASTrns where tno=@tno


COMMIT TRANSACTION [Tran1]
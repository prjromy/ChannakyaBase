
CREATE proc [acc].[PSetOpeningBal](@VId int)
as 
BEGIN
	DECLARE @FId int
	DECLARE @FYId int
	DECLARE @DbAmt money
	DECLARE @CrAmt money
	DECLARE @BRId int

	DECLARE V2 CURSOR For
	select a.Fid,c.FYID,a.DebitAmount,a.CreditAmount,c.CompanyId from acc.voucher2 a 
	inner join acc.Voucher1 b on a.V1id=b.V1Id 
	inner join acc.voucherno c on b.VNId=c.VNId
	where b.V1id=16
	Open v2 
	FETCH from v2 into @FId,@FYId,@DbAmt,@CrAmt,@BRId
	while @@FETCH_STATUS=0
	BEGIN
		IF EXISTS (select FYId from acc.voucherbal where FYId=@FYId and FId=@FId and BranchId=@BRId)
		BEGIN
			UPDATE acc.VoucherBal SET CLBal=CLBal+ ISNULL(@DbAmt,0)-ISNULL(@CrAmt,0) where FYId=@FYId and FId=@FId and BranchId=@BRId
		END
		ELSE
		BEGIN
			INSERT INTO acc.VoucherBal (BranchId,FId,FYId,OPBal,CLBal) VALUES(@BRId,@FId,@FYId,0,ISNULL(@DbAmt,0)-ISNULL(@CrAmt,0))
		END
		FETCH NEXT from v2 into @FId,@FYId,@DbAmt,@CrAmt,@BRId
	
	END
	
END
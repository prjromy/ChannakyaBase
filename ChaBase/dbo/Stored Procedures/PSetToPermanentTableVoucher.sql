CREATE procedure [dbo].[PSetToPermanentTableVoucher](@TV1Id NUMERIC,@Narration varchar(MAX),@FYId int,@BranchId int,@V1Id int output)
as
begin
    set nocount on;
    declare @trancount int;
    set @trancount = @@trancount;
    begin try
        if @trancount = 0
            begin transaction
        else
            save transaction [dbo.PSetToPermanentTableVoucher];

DECLARE @TV2Id int
DECLARE  @VId int
DECLARE @V2Id int

Declare @VNo int

  set @VNo = (Select isnull(max(vno),0) + 1 from acc.voucher1 where VNId = (select vnid from acc.Voucher1T where V1TId = @TV1Id))

   INSERT INTO acc.Voucher1 (VNo,VNId,TDate,PostedBy,PostedOn,Narration,CTId) 
	SELECT @VNo,VNId,TDate,PostedBy,getdate(),isnull(@Narration,Narration),CTId FROM acc.Voucher1T
	where V1TId=@TV1Id; 

	SET @VId= @@IDENTITY
	

	-- SELECT V2TID from acc.Voucher2T where V1Tid=@TV1Id
	DECLARE V2 CURSOR FOR 
		 SELECT V2TID from acc.Voucher2T where V1Tid=@TV1Id
	OPEN V2 
	FETCH FROM V2 INTO @TV2Id
	WHILE @@FETCH_STATUS=0
	BEGIN
		INSERT INTO acc.Voucher2 (V1id,Particulars,Fid,DebitAmount,CreditAmount)
		SELECT @VId,Particulars,Fid,DebitAmount,CreditAmount from acc.Voucher2T where V2Tid=@TV2Id

		SET @V2Id= @@IDENTITY

		INSERT INTO acc.Voucher3 (V2Id,SId,Description,Amount)
		 SELECT @V2Id,SId,Description,Amount from acc.Voucher3T where V2Tid=@TV2Id

		INSERT INTO acc.Voucher4 (V2Id,DVId,DDID,Amount)
		SELECT @V2Id,DVId,DDID,Amount from acc.Voucher4T where V2Tid=@TV2Id

		INSERT INTO acc.Voucher5(V2Id,Payees,ChequeNo,ChequeDate,ChequeAmount)
		SELECT @V2Id,Payees,ChequeNo,ChequeDate,ChequeAmount from acc.Voucher5T where V2Tid=@TV2Id
 	
	
	

    DELETE  from acc.Voucher5T where V2TId =@TV2Id;
	DELETE  from acc.Voucher4T where V2TId=@TV2Id;
	DELETE  from acc.Voucher3T where V2TId=@TV2Id;
	DELETE  from acc.Voucher2T where V2Tid=@TV2Id;

	FETCH NEXT FROM V2 INTO @TV2Id
	END
	CLOSE V2
	DEALLOCATE V2

	--exec acc.PSetOpeningBal @VId

	DELETE  from acc.Voucher1T where V1TId=@TV1Id;
	SET @V1Id=@VId
	--SELECT @V1Id

	

lbexit:
        if @trancount = 0
            commit;
    end try
    begin catch
        declare @error int, @message varchar(4000), @xstate int;
        select @error = ERROR_NUMBER(), @message = ERROR_MESSAGE(), @xstate = XACT_STATE();
        if @xstate = -1
            rollback;
        if @xstate = 1 and @trancount = 0
            rollback
        if @xstate = 1 and @trancount > 0
            rollback transaction [dbo.PSetToPermanentTableVoucher];

        raiserror ('[dbo.PSetToPermanentTableVoucher]: %d: %s', 16, 1, @error, @message) ;
    end catch
end
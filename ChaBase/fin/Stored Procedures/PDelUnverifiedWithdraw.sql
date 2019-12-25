CREATE proc [fin].[PDelUnverifiedWithdraw](@Tno int)
as 
	begin 
	set nocount on;
    declare @trancount int;
    set @trancount = @@trancount;
    begin try
        if @trancount = 0
            begin transaction
        else
            save transaction [Fin.PDelUnverifiedWithdraw];
	begin
		declare @Iaccno int
		declare @Amt decimal
		declare @IsSlip int
		declare @SlipNo int
		declare @BranchId int
		select  @Iaccno=IAccno,@Amt=dramt,@IsSlip=IsSlp,@SlipNo=slpno from trans.ASTrns where tno=@Tno 
		select @BranchId=brchid  from fin.adetail where iaccno=@Iaccno
		if exists(select IAccno from fin.ABal where IAccno=@Iaccno and Flag=1 )
			begin
				update fin.ABal set Bal=bal-@Amt where IAccno=@Iaccno and Flag=1
			end
		if exists(select IAccno from fin.AchqFGdPymt where IAccno=@Iaccno and Chqno=@SlipNo and @IsSlip=0 and brnhid=@BranchId)
			begin
				UPDATE achqfgdpymt SET tstate = 4 WHERE chqno=@SlipNo AND brnhid=@BranchId  AND IAccNo=@Iaccno
			end
		if exists(SELECT astrns.IAccno FROM fin.ADETAIL inner join trans.astrns on astrns.iaccno = adetail.iaccno where astrns.tno =@Tno and AccState=3)
			begin
				UPDATE fin.ADETAIL SET ACCSTATE= 7 WHERE iACCNO in 
				(SELECT astrns.IAccno FROM fin.ADETAIL inner join trans.astrns on astrns.iaccno = adetail.iaccno where astrns.tno =@Tno and AccState=3)
			end
		if exists(SELECT tno from fin.AchqH where tno =@Tno)
			begin
				UPDATE fin.AchqH SET tno= null WHERE Tno=@Tno 				
			end
			if exists(select IAccno from Trans.ASTrns where tno=@Tno  )
		begin
			update  Trans.ASTrns set IsDeleted=1 where tno=@Tno and IAccno=@Iaccno and brnhno=@BranchId
		end		
	end

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
            rollback transaction usp_my_procedure_name;

        raiserror ('[Fin.PDelUnverifiedWithdraw]: %d: %s', 16, 1, @error, @message) ;
    end catch
end
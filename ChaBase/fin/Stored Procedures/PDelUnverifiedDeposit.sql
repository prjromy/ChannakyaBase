CREATE proc [fin].[PDelUnverifiedDeposit](@Tno int)
as 
begin 
	set nocount on;
    declare @trancount int;
    set @trancount = @@trancount;
    begin try
        if @trancount = 0
            begin transaction
        else
            save transaction [Fin.PDelUnverifiedDeposit];
		begin
			declare @Iaccno int
			declare @Amt decimal
			select @Iaccno=IAccno,@Amt=cramt from trans.ASTrns where tno = @Tno
			if exists(select IAccno from fin.ABal where IAccno = @Iaccno and Flag = 2 )
			begin
				update fin.ABal set Bal=bal-@Amt where IAccno = @Iaccno and Flag = 2
			end
			if exists(select IAccno from Trans.ASTrns where tno = @Tno  )
			begin
				update  Trans.ASTrns set IsDeleted=1 where tno = @Tno
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
            rollback transaction [Fin.PDelUnverifiedDeposit];

        raiserror ('[Fin.PDelUnverifiedDeposit]: %d: %s', 16, 1, @error, @message) ;
    end catch
end
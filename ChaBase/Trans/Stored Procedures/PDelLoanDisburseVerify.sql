CREATE procedure [Trans].[PDelLoanDisburseVerify](@disburseId int)
as
begin
    set nocount on;
    declare @trancount int;
	declare @tno int
	
	declare @cramount decimal	
	declare @LoanIaccno int	
	declare @Notes varchar(500)	
	declare @postedby int	    
	declare @isotherLoan int =fin.IsOtherLoan(( select(select FID from fin.DisburseVsPID where DisburseId=@disburseId)))
	declare @mode int
	 select @cramount= Amount,@Notes=Remarks from fin.DisburseVsPID where DisburseId=@disburseId
	 select @LoanIaccno= IAccno,@postedby=PostedBy,@mode=Mode from fin.DisbursementMain where DisburseId=@disburseId
	declare @depositAmount decimal
	declare @depositIaccno decimal
	declare @Previousbalance decimal
	declare @LoanOut decimal
	declare @Vno int=13
	declare @tdate datetime
	declare @branch int
	declare @fid int
	declare @cashamount decimal

    set @trancount = @@trancount;
    begin try
        if @trancount = 0
            begin transaction
        else
            save transaction [Trans.PSetLoanDisburseVerify];
			
	if(@mode=1)
		begin
			delete from fin.DisburseDeposit where DisburseId=@disburseId
		end
	else if(@mode=2)
		begin
			delete from fin.DisburseCheque where DisburseId=@disburseId
		end
	else if(@mode=3)
		begin
			delete from fin.DisburseCash where DisburseId=@disburseId	
		end
	
				delete from fin.TempALSch where IAccno=@LoanIaccno
				delete from fin.DisburseVsPID where DisburseId=@disburseId
				delete from fin.DisbursementMain where DisburseId=@disburseId
				delete from fin.DisburseCharge where DisburseId=@disburseId
				delete from Trans.ChgSTrnx where Iaccno=@LoanIaccno

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

        raiserror ('[Trans.PSetLoanDisburseVerify]: %d: %s', 16, 1, @error, @message) ;
    end catch
end
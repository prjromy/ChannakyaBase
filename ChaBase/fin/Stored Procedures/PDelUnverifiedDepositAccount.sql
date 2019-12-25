CREATE proc [fin].[PDelUnverifiedDepositAccount]( @Iaccno int)
as 
	begin 
	set nocount on;
    declare @trancount int;
    set @trancount = @@trancount;
    begin try
        if @trancount = 0
            begin transaction
        else
            save transaction [Fin.PDelUnverifiedDepositAccount];

		 
		 delete from fin.ANominee			where iaccno=@iaccno
		 delete from fin.ADur				where iaccno=@iaccno
		 delete from fin.AMinBal			where iaccno=@iaccno
		 delete from fin.ReferenceInfo		where iaccno=@iaccno
		 delete from fin.AOfCust			where iaccno=@iaccno
		 delete from fin.APolicyInterest	where iaccno=@iaccno		 								 
		 delete from fin.AICBDur			where iaccno=@iaccno
		 delete from fin.ATempFloatIntRate	where iaccno=@iaccno
		 delete from fin.AChq				where iaccno=@iaccno
		 delete from fin.ANomAcc			where iaccno=@iaccno		 								 
		 delete from fin.ARate				where iaccno=@iaccno
		 delete from fin.ADrLimit			where iaccno=@iaccno									 
		 delete from fin.APolicyInterest	where iaccno=@iaccno
		 delete from fin.AICBDur			where iaccno=@iaccno
		 delete from fin.ADSch				where iaccno=@iaccno	
		 delete from Trans.ChgSTrnx			where IAccno=@Iaccno
		 delete from fin.ADetail			where IAccno=@Iaccno
		if exists(select IAccno from Trans.ChgSTrnx where IAccno=@Iaccno  )
		begin
			update  Trans.ChgSTrnx set IsDeleted=1,Remarks='Rejected Account' where IAccno=@Iaccno
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

        raiserror ('[Fin.PDelUnverifiedDepositAccount]: %d: %s', 16, 1, @error, @message) ;
    end catch
end
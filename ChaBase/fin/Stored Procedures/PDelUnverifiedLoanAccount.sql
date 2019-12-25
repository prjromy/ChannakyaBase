CREATE proc [fin].[PDelUnverifiedLoanAccount]( @Iaccno int)
as 
	begin 
	set nocount on;
    declare @trancount int;
    set @trancount = @@trancount;
    begin try
        if @trancount = 0
            begin transaction
        else
            save transaction [Fin.PDelUnverifiedLoanAccount];
			declare @regid int
		 select @regid=regid from fin.ALRegistration where iAccno=@Iaccno 
		 delete from fin.ANominee			where iaccno=@iaccno
		 delete from fin.ADur				where iaccno=@iaccno
		 delete from fin.AOfCust			where iaccno=@iaccno
		 delete from fin.APolicyInterest	where iaccno=@iaccno		 								  
		 delete from fin.ANomAcc			where iaccno=@iaccno		 								 
		 delete from fin.ARate				where iaccno=@iaccno
		 delete from fin.ADrLimit			where iaccno=@iaccno									 
		 delete from fin.APolicyInterest	where iaccno=@iaccno	
		 delete from Trans.ChgSTrnx			where IAccno=@Iaccno
		
		 delete from fin.APolPen			where IAccno=@Iaccno
		
		 delete from fin.ALoanGrace			where IAccno=@Iaccno
		 delete from fin.APRate				where IAccno=@Iaccno
		 delete from fin.ALRegCusts			where RegId=@regId
		 delete from fin.ALinkloan			where IAccno=@Iaccno
		 delete from fin.Guarantor			where LIaccno=@Iaccno
		 delete from fin.ALoan				where IAccno=@Iaccno
		 delete from fin.ALRegistration		where IAccno=@Iaccno
		 delete from fin.ADetail			where IAccno=@Iaccno



		if exists(select IAccno from Trans.ChgSTrnx where IAccno=@Iaccno  )
		begin
			update  Trans.ChgSTrnx set IsDeleted=1,Remarks='Deleted After Loan Account Rejected' where IAccno=@Iaccno
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

        raiserror ('[Fin.PDelUnverifiedLoanAccount]: %d: %s', 16, 1, @error, @message) ;
    end catch
end
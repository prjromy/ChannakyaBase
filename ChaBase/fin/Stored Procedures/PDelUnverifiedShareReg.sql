CREATE procedure [fin].[PDelUnverifiedShareReg](@RegNo int)
as
begin
    set nocount on;
    declare @trancount int;
	declare @tno int
		
		delete from fin.Snominee where RegNo=@RegNo
		delete from fin.ReferenceInfo where IAccNo=@RegNo and RType=0
		delete from fin.ShrReg where RegNo=@RegNo
		
    set @trancount = @@trancount;
    begin try
        if @trancount = 0
            begin transaction
        else
            save transaction [Trans.PDelUnverifiedShareReg];
			

			

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

        raiserror ('[Trans.PDelUnverifiedShareReg]: %d: %s', 16, 1, @error, @message) ;
    end catch
end
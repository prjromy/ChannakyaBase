Create procedure [fin].[PDelUnverifiedShareReturn](@tno int)
as
begin
    set nocount on;
    declare @trancount int;
 
		update fin.ShrSReturn set isdeleted=1 where tno=@tno
	 
    set @trancount = @@trancount;
    begin try
        if @trancount = 0
            begin transaction
        else
            save transaction [Fin.PDelUnverifiedShareReturn];
			

			

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

        raiserror ('[Fin.PDelUnverifiedShareReturn]: %d: %s', 16, 1, @error, @message) ;
    end catch
end

CREATE proc [fin].[PDelTellerExceedPayment](@Rno int)
as 
begin 
	set nocount on;
    declare @trancount int;
    set @trancount = @@trancount;
    begin try
		if @trancount = 0
			begin transaction
		else
			save transaction [Fin.PDelTellerExceedPayment];
		begin
			declare @Iaccno int
			declare @Amt decimal

			Select @Iaccno=IAccno,@Amt=[amount] from [fin].[AWtdQueue]
		
			if exists(select IAccno from fin.ABal where IAccno=@Iaccno and Flag = 5 )
			begin
				update fin.ABal set Bal=bal-@Amt where IAccno=@Iaccno and Flag = 5
			end		
				delete from  [fin].[AWtdQueue]  where Rno = @Rno	
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
					rollback transaction [Fin.PDelTellerExceedPayment];
				raiserror ('[Fin.PDelTellerExceedPayment]: %d: %s', 16, 1, @error, @message) ;
			end catch
end
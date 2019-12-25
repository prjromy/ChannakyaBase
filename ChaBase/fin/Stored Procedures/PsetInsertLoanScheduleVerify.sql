
 
 CREATE proc [fin].[PsetInsertLoanScheduleVerify](@amount decimal(18,2), @IAccno int)
as
begin

begin try
   begin transaction
			
update fin.ALoan set PrincipleLoanOut=isnull(PrincipleLoanOut,0)+@amount where IAccno=@IAccno
 if  exists(select * from fin.ALSch where IAccno=@IAccno)
			 begin
				insert into ALSchHistry select [IAccno],[schDate],[schPrin],[schInt],[calcInt],[balPrin],[pdPrin],[pdInt],(select isnull(max(reSchFreq),0)+1 from ALSchHistry where iaccno=@IAccno) from fin.ALSch  where iaccno=@IAccno
				delete from fin.ALSch where IAccno =@IAccno
			 end
	  insert into fin.AlSch select [IAccno],[schDate],[schPrin],[schInt],[calcInt],[balPrin],[pdPrin],[pdInt] from [fin].[TempALSch] where iaccno=@IAccno
	  delete  from [fin].[TempALSch] where iaccno=@IAccno	  
            commit;
    end try
	 begin catch
        declare @error int, @message varchar(4000), @xstate int;
        select @error = ERROR_NUMBER(), @message = ERROR_MESSAGE(), @xstate = XACT_STATE();
        if @xstate = -1
            rollback;       
        raiserror ('[fin].[PsetInsertLoanScheduleVerify]: %d: %s', 16, 1, @error, @message) ;
    end catch
end
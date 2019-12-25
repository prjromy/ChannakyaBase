

CREATE procedure [Trans].[PSetLoanDisburseVerify](@disburseId int,@ApprovedBy int)
as
begin
    set nocount on;
    declare @trancount int;
	declare @tno int
	
	declare @cramount decimal	
	declare @LoanIaccno int	
	declare @Notes varchar(500)	
	declare @postedby int
	declare @postedon datetime	    
	declare @isotherLoan int =fin.IsOtherLoan(( select(select FID from fin.DisburseVsPID where DisburseId=@disburseId)))
	declare @mode int
	 select @cramount= Amount,@Notes=Remarks from fin.DisburseVsPID where DisburseId=@disburseId
	 select @LoanIaccno= IAccno,@postedby=PostedBy,@mode=Mode,@postedon=PostedOn from fin.DisbursementMain where DisburseId=@disburseId
	declare @depositAmount decimal
	declare @depositIaccno decimal
	declare @Previousbalance decimal
	declare @LoanOut decimal
	--declare @Vno int=13
	declare @tdate datetime
	declare @branch int
	declare @fid int
	declare @cashamount decimal
	declare @v1id  int
    set @trancount = @@trancount;
    begin try
        if @trancount = 0
            begin transaction
        else
            save transaction [Trans.PSetLoanDisburseVerify];
			  exec Fin.PsetDisbursementVoucher @disburseId,@ApprovedBy,@v1id output
			exec [Mast].[GetTransno] @tno out
			
						select @tdate=TDate,@branch=a.BrchID from fin.ADetail a inner join  lg.company l on a.brchid=l.companyId where iaccno=@LoanIaccno
					 
					 if(@isotherLoan=0)
					 begin
						set @LoanOut=(select top 1 Amount from fin.DisburseVsPID where DisburseId=@disburseId)
						 update fin.ALoan set PrincipleLoanOut=isnull(PrincipleLoanOut,0)+@LoanOut where IAccno=@LoanIaccno
						 update fin.ADetail set bal =bal+@LoanOut,LastTransDate=@tdate where IAccno=@LoanIaccno
					 end
					  if(@isotherLoan=1)
					 begin
						 set @LoanOut=(select top 1 Amount from fin.DisburseVsPID where DisburseId=@disburseId)
					  update fin.ALoan set OtherLoanOut=isnull(OtherLoanOut,0)+@LoanOut where IAccno=@LoanIaccno
					  update fin.ADetail set LastTransDate=@tdate where IAccno=@LoanIaccno
					 end
		
					 INSERT INTO [Trans].[AMTrns] (tno,IAccno,tdate,vdate,notes,slpno,dramt,cramt,ttype,postedby,approvedby,brnhno,vno,Isslp,PostedOn)
					 values(@tno,@LoanIaccno,@tdate,@tdate,@Notes,0,0,@cramount,2,@postedby,@ApprovedBy,@branch,@v1id,0,@postedon) 
					 insert into fin.AMTransLoan values(@tno,10,@LoanOut,@v1id)--20 is dummy voucher no
		if(@mode=1)
		begin
		EXECUTE fin.PAloanTraInsUpd @tno	
		 declare cur cursor  for
			 select DepositIaccno,Amount from fin.DisburseDeposit where DisburseId=@disburseId
		 open cur
		 fetch from cur into @depositIaccno,@depositAmount
		
		 while @@FETCH_STATUS=0
		 begin
			 exec [Mast].[GetTransno] @tno out
			  begin
				 INSERT INTO [Trans].[AMTrns] (tno,IAccno,tdate,vdate,notes,slpno,dramt,cramt,ttype,postedby,approvedby,brnhno,vno,Isslp,PostedOn)
				 values( @tno,@depositIaccno,@tdate,@tdate,@Notes,0,@depositAmount,0,2,@postedby,@ApprovedBy,@branch,@v1id,0,@postedon)
				begin
			
				  set @Previousbalance=(select top 1 Bal  from fin.ADetail where IAccno=@depositIaccno)
			IF EXISTS (
						SELECT *
						FROM fin.ABAL
						WHERE IACCNO = @depositIaccno
						AND fin.ABAL.FLAG = 3
					  )
			BEGIN
				UPDATE fin.ABAL
				SET BAL = BAL + @depositAmount
				WHERE fin.ABAL.FLAG = 3 and fin.ABal.IAccno=@depositIaccno
			END
			ELSE
			BEGIN
				SET @fid = (
						SELECT TOP 1 FID
						FROM fin.ADetail a  inner join fin.ProductDetail b on a.PID=b.PID
						WHERE IACCNO = @depositIaccno
						 
						)

				INSERT INTO fin.ABAL
				VALUES (
					@depositIaccno
					,@Fid
					,3
					,@depositAmount+isnull(@Previousbalance,0)
					)
			END
			    select @tdate=TDate,@branch=a.BrchID from fin.ADetail a inner join  fin.LicenseBranch l on a.brchid=l.brnhid where iaccno=@depositIaccno
			    update fin.ADETAIL 
             set BAL = (select BAL 
                          from fin.ABAL 
                         where FLAG = 3 
                               and IACCNO = @depositIaccno) 
							   ,
			LastTransDate=@tdate
           where IACCNO = @depositIaccno 

				end

			  end
			 fetch next from cur into @depositIaccno,@depositAmount
		 end
		 CLOSE  cur
	DEALLOCATE  cur
end
if(@mode=3)
		begin
			select top 1 @cashamount= amount from fin.DisburseCash where DisburseId=@disburseId
			 exec [Mast].[GetTransno] @tno out
			 INSERT INTO [Trans].[AMTrns] (tno,IAccno,tdate,vdate,notes,slpno,dramt,cramt,ttype,postedby,approvedby,brnhno,vno,Isslp,PostedOn)
			  values( @tno,@LoanIaccno,@tdate,@tdate,@Notes,0,@cashamount,0,41,@postedby,@ApprovedBy,@branch,@v1id,0,@postedon)
			  update fin.DisburseCash set TNo=@tno where DisburseId=@disburseId
		end
	if(@mode=2)
		begin
			select top 1 @cashamount= amount from fin.DisburseCheque where DisburseId=@disburseId
			 exec [Mast].[GetTransno] @tno out
			 EXECUTE fin.PAloanTraInsUpd @tno	
			 INSERT INTO [Trans].[AMTrns] ([tno],[IAccno] ,[tdate] ,[vdate],[notes],[slpno],[dramt],[cramt],[ttype],[postedby],[approvedby],[brnhno],[vno],[Isslp],PostedOn)
			  values( @tno,@LoanIaccno,@tdate,@tdate,@Notes,0,@cashamount,0,45,@postedby,@ApprovedBy,@branch,@v1id,0,@postedon)
		end
	 if  exists(select * from fin.ALSch where IAccno=@LoanIaccno)
			 begin
				insert into fin.ALSchHistry select [IAccno],[schDate],[schPrin],[schInt],[calcInt],[balPrin],[pdPrin],[pdInt],(select isnull(max(reSchFreq),0)+1 from fin.ALSchHistry where iaccno=@loanIaccno) from fin.ALSch  where iaccno=@loanIaccno
				delete from fin.ALSch where IAccno =@LoanIaccno
			 end
	  insert into fin.AlSch select [IAccno],[schDate],[schPrin],[schInt],[calcInt],[balPrin],[pdPrin],[pdInt] from [fin].[TempALSch] where iaccno=@loanIaccno

	  delete from fin.TempALSch where IAccno=@LoanIaccno
	
	  
	  update fin.DisbursementMain set ApprovedBy=@ApprovedBy,ApprovedOn=Getdate(),vno=@v1id where DisburseId=@disburseId
									    
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
            rollback transaction [PSetLoanDisburseVerify];

        raiserror ('[PSetLoanDisburseVerify]: %d: %s', 16, 1, @error, @message) ;
    end catch
end
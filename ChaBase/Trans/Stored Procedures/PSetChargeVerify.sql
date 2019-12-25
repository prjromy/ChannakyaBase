
CREATE procedure [Trans].[PSetChargeVerify] (@TNO BIGINT,@APPROVEDBY NVARCHAR(max))
as 
SET NOCOUNT ON;
BEGIN
	begin try 
		begin transaction 
			declare   
			@IAccno INT, 
            @tdate SMALLDATETIME, 
            @Remarks NVARCHAR(1000), 
            @slpno INT, 
            @amt MONEY, 
            @ttype TINYINT, 
			@postedby INT, 
			@brnhno INT, 
            @VNO NUMERIC(18, 0), 
            @Fid INT ,
			@ChgId int,
			@IsDeposit INT
        
		set @tdate = (select TDATE 
                          from [Trans].[ChgSTrnx]
                         where trnxno = @TNo) 
		set @ChgId = (select TOP 1 ChgId 
                          from [Trans].[ChgSTrnx] 
                         where trnxno = @TNo)
		set @amt = (select amt 
                          from [Trans].[ChgSTrnx] 
                         where trnxno = @TNo)  
        set @Remarks = (select Remarks 
                          from [Trans].[ChgSTrnx]
                         where trnxno = @TNo)     
        set @postedby = (select POSTEDBY 
                             from [Trans].[ChgSTrnx]
                            where trnxno = @TNo) 
		set @IAccno = (select IACCNO 
                           from [Trans].[ChgSTrnx]
                          where trnxno = @TNo) 
		set @VNO = (select VNO 
                           from [Trans].[ChgSTrnx] 
                          where trnxno = @TNo) 
       
		set @ttype = (select TTYPE 
                          from [Trans].[ChgSTrnx]
                         where trnxno = @TNo) 
        set @slpno = (select SLPNO 
                          from [Trans].[ChgSTrnx]
                         where trnxno = @TNo) 
		set @brnhno = (select brhid 
                           from [Trans].[ChgSTrnx] 
                          where trnxno = @TNo) 
        set @IsDeposit=(select Modules from fin.chargedetail where Chgid=@ChgId )
	
        insert into [Trans].[ChgMTrnx]
                      (Tdate
                       ,Chgid 
					   ,amt
                       ,TrnxNo 
                       ,Remarks 
                       ,Postedby 
                       ,ApprovedBy 
                       ,Iaccno 
                       ,Vno 
                       ,[TTYPE]  
                       ,slpno 
                       ,brhid)
                       
               values (@tdate   
                       ,@ChgId 
					   ,@amt
                       ,@TNO 
                       ,@Remarks 
                       ,@postedby 
                       ,@APPROVEDBY 
					   ,@iaccno
					   ,@vno
					   ,@ttype
					   ,@slpno                   
					   ,@brnhno)



		if((@ttype= 1 OR @ttype=2) AND (@IsDeposit=1))
			Begin
				UPDATE fin.ABAL	SET BAL = BAL - @amt WHERE fin.ABAL.FLAG = 1 AND IACCNO = @IAccno
				IF EXISTS (SELECT *	FROM fin.ABAL WHERE IACCNO = @IAccno AND fin.ABAL.FLAG = 3)
				BEGIN
					UPDATE fin.ABAL	SET BAL = BAL - @amt WHERE fin.ABAL.FLAG = 3 and fin.ABal.IAccno=@IAccno
				END
				update fin.ADETAIL set BAL = (select BAL 
											from fin.ABAL 
											where FLAG = 3 
											and IACCNO = @IAccno) 
				where IACCNO = @IAccno 

				delete from [Trans].[ChgSTrnx] where trnxno = @TNo 
			end
			else
			   delete from [Trans].[ChgSTrnx] where trnxno = @TNo 

        commit transaction 
        select 1 
      end try 
      begin catch 
	  declare @error int, @message varchar(4000), @xstate int;
        select @error = ERROR_NUMBER(), @message = ERROR_MESSAGE(), @xstate = XACT_STATE();
          rollback transaction 
		   raiserror ('usp_my_procedure_name: %d: %s', 16, 1, @error, @message) ;
          select 0 
      end catch 
END
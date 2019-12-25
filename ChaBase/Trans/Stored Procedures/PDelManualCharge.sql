
CREATE procedure [Trans].[PDelManualCharge] (@Tno BIGINT)
as 
SET NOCOUNT ON;
	begin 
	set nocount on;
    declare @trancount int;
    set @trancount = @@trancount;
    begin try
        if @trancount = 0
            begin transaction
        else
            save transaction [Fin.PDelManualCharge];

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
					 @ChgId int           
		  set @IAccno = (select IACCNO 
                           from [Trans].[ChgSTrnx]
                          where trnxno = @TNo) 
		  set @ttype = (select ttype 
                           from [Trans].[ChgSTrnx]
                          where trnxno = @TNo) 
		
begin
if(@IAccno is not null and @ttype=5)
select @tdate=TDate,@brnhno=a.BrchID from fin.ADetail a inner join  fin.LicenseBranch l on a.brchid=l.brnhid where iaccno=@IAccno
			begin	
			IF EXISTS (
						SELECT *
						FROM fin.ABAL
						WHERE IACCNO = @IAccno
						AND fin.ABAL.FLAG = 3
					  )
				BEGIN
					UPDATE fin.ABAL
					SET BAL = BAL +@amt
					WHERE fin.ABAL.FLAG = 3 and fin.ABal.IAccno=@IAccno
					UPDATE fin.ABAL
					SET BAL = BAL - @amt
					WHERE fin.ABAL.FLAG = 1--withdraw
						AND IACCNO = @IAccno
				END		
						
			    update fin.ADETAIL 
				 set BAL = (select BAL 
						    from fin.ABAL 
								where FLAG = 3 
                                and IACCNO = @IAccno
							) 
							   ,LastTransDate=@tdate
			    where IACCNO = @IAccno 
		  end
		
end
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

        raiserror ('[Fin.PDelManualCharge]: %d: %s', 16, 1, @error, @message) ;
    end catch
end
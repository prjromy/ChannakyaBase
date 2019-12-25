CREATE procedure [Trans].[PSetDepositWithdrawVerify] @TNO         BIGINT 
                                                    ,@APPROVEDBY NVARCHAR(max) 
as 
  begin 
      begin try 
          begin transaction 

          declare   @IAccno INT, 
                     @tdate SMALLDATETIME, 
                     @notes NVARCHAR(1000), 
                     @slpno INT, 
                     @dramt MONEY, 
                     @cramt MONEY, 
                     @ttype TINYINT, 
                  @postedby INT, 
                    @brnhno INT, 
                       @VNO NUMERIC(18, 0), 
                     @IsSlp BIT, 
                       @Fid INT 

						select @IAccno=IACCNO,@tdate=tdate,@notes=notes,@SlpNo=slpno,@dramt=dramt,@cramt=cramt,@ttype=ttype,@postedby=postedby,@brnhno=brnhno,@IsSlp=Isnull(ISSLP, 0)  from trans.ASTRNS 
                          where TNO = @TNo
                                       
          insert into [Trans].[AMTRNS] 
                      ([TNO] 
                       ,[IACCNO] 
                       ,[TDATE] 
                       ,[VDATE] 
                       ,[NOTES] 
                       ,[SLPNO] 
                       ,[DRAMT] 
                       ,[CRAMT] 
                       ,[TTYPE] 
                       ,[POSTEDBY] 
                       ,[APPROVEDBY] 
                       ,[BRNHNO] 
                       ,[ISSLP]) 
               values (@tno 
                       ,@IAccno 
                       ,@tdate 
                       ,@tdate 
                       ,@notes 
                       ,@slpno 
                       ,@dramt 
                       ,@cramt 
                       ,@ttype 
                       ,@postedby 
                       ,@APPROVEDBY 
                       ,@brnhno 
                       ,@Isslp) 

          --if exists(select * from fin.ABal where IAccno= (select top 1 iaccno from @tempTrans) and fin.ABal.Flag=2 
         IF (
		@cramt != 0.00
		and @cramt IS NOT NULL
		)
BEGIN
	UPDATE fin.ABAL
	SET BAL = BAL - @cramt
	WHERE fin.ABAL.FLAG = 2
		AND IACCNO = @IAccno

	IF EXISTS (
			SELECT *
			FROM fin.ABAL
			WHERE IACCNO = @IAccno
				AND fin.ABAL.FLAG = 3
			)
	BEGIN
		UPDATE fin.ABAL
		SET BAL = BAL + @cramt
		WHERE fin.ABAL.FLAG = 3 and fin.ABal.IAccno=@IAccno
	END
	ELSE
	BEGIN
		SET @fid = (
				SELECT TOP 1 FID
				FROM fin.ABAL
				WHERE IACCNO = @IAccno
					AND FLAG = 2
				)

		INSERT INTO fin.ABAL
		VALUES (
			@IAccno
			,@Fid
			,3
			,@cramt
			)
	END
END
ELSE IF (
		@dramt != 0.00
		and @dramt IS NOT NULL
		)
BEGIN
	UPDATE fin.ABAL
	SET BAL = BAL - @dramt
	WHERE fin.ABAL.FLAG = 1
		AND IACCNO = @IAccno

	IF EXISTS (
			SELECT *
			FROM fin.ABAL
			WHERE IACCNO = @IAccno
				AND fin.ABAL.FLAG = 3
			)
	BEGIN
		UPDATE fin.ABAL
		SET BAL = BAL - @dramt
		WHERE fin.ABAL.FLAG = 3 and fin.ABal.IAccno=@IAccno
	END
	ELSE
	BEGIN
		SET @fid = (
				SELECT TOP 1 FID
				FROM fin.ABAL
				WHERE IACCNO = @IAccno
					AND FLAG = 1
				)

		INSERT INTO fin.ABAL
		VALUES (
			@IAccno
			,@Fid
			,3
			,@cramt
			)
	END
END

          update fin.ADETAIL 
             set BAL = (select BAL 
                          from fin.ABAL 
                         where FLAG = 3 
                               and IACCNO = @IAccno) 
           where IACCNO = @IAccno 

          delete from trans.ASTRNS 
           where TNO = @TNo 

          commit transaction 

          select 1 
      end try 

      begin catch 
          rollback transaction 

          select 0 
      end catch 
  end

CREATE PROCEDURE [Trans].[PSetCollection] @ColSheetId bigint, @ApprovedBy INT 
AS 
begin
  set nocount on;
    declare @trancount int;
   set @trancount = @@trancount;
    begin try
        if @trancount = 0
            begin transaction
        else
            save transaction [Trans.PSetCollection];
          DECLARE @Iaccno INT 
          DECLARE @Amount DECIMAL 
          DECLARE @tdate DATETIME 
          DECLARE @branch INT 
 

          INSERT INTO trans.amtrns 
                      ([tno], 
                       [iaccno], 
                       [tdate], 
                       [vdate], 
                       [notes], 
                       [slpno], 
                       [dramt], 
                       [cramt], 
                       [ttype], 
                       [postedby], 
                       [approvedby], 
                       [brnhno], 
                       [vno], 
                       [isslp]
					   --, 
        --               [totalinterestpaydate]) 
		)
          SELECT b.tno, 
                 b.iaccno, 
                 a.tdate, 
                 a.vdate, 
                 b.[description], 
                 a.sheetno * -1, 
                 0.00, 
                 b.amount, 
                 1, 
                 a.postedby, 
                 @ApprovedBy, 
                 a.brchid, 
                 NULL, 
                 0
				 --, 
     --            NULL 
          FROM   fin.collsheet a 
                 INNER JOIN fin.collsheettrans b 
                         ON a.collsheetid = b.collsheetid 
          WHERE  a.collsheetid = @ColSheetId 

          UPDATE fin.collsheet 
          SET    approvedby = @ApprovedBy 
          WHERE  collsheetid = @ColSheetId 

          DECLARE curx CURSOR FOR 
            SELECT b.iaccno, 
                   b.amount
                  
            FROM   fin.collsheet a 
                   INNER JOIN fin.collsheettrans b 
                           ON a.collsheetid = b.collsheetid 
            WHERE  a.collsheetid = @ColSheetId 
			open curx
          FETCH FROM curx INTO @Iaccno, @Amount 

          WHILE @@FETCH_STATUS = 0 
            BEGIN 
                IF EXISTS (SELECT * 
                           FROM   fin.abal 
                           WHERE  iaccno = @IAccno 
                                  AND fin.abal.flag = 3) 
                  BEGIN 
                      UPDATE fin.abal 
                      SET    bal = bal + @Amount 
                      WHERE  fin.abal.flag = 3 
                             AND fin.abal.iaccno = @IAccno 
                  END 
				  ELSE
	BEGIN
	declare @fid int
		SET @fid = (
				SELECT TOP 1 pd.FID
				FROM fin.ADetail ad inner join fin.ProductDetail pd on ad.PID=pd.PID
				WHERE IACCNO = @IAccno
					 
				)

		INSERT INTO fin.ABAL
		VALUES (
			@IAccno
			,@Fid
			,3
			,@Amount
			)
	END
                SELECT @tdate = tdate, 
                       @branch = a.brchid 
                FROM   fin.adetail a 
                       INNER JOIN fin.licensebranch l 
                               ON a.brchid = l.brnhid 
                WHERE  iaccno = @IAccno 

                UPDATE fin.adetail 
                SET    bal = (SELECT bal 
                              FROM   fin.abal 
                              WHERE  flag = 3 
                                     AND iaccno = @IAccno), 
                       lasttransdate = @tdate 
                WHERE  iaccno = @IAccno 

                FETCH next FROM curx INTO @Iaccno, @Amount 
				end
                CLOSE curx

                DEALLOCATE curx
             
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

        raiserror ('[Trans.PSetCollection]: %d: %s', 16, 1, @error, @message) ;
    end catch
end
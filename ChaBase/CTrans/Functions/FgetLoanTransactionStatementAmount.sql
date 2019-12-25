CREATE FUNCTION [CTrans].[FgetLoanTransactionStatementAmount] (@tillDate DATE, 
                                                          @IAccno   INT) 
returns MONEY 
AS 
  BEGIN 
      DECLARE @BalID INT 
      DECLARE @Bal MONEY 
      DECLARE @IonBal MONEY 
      DECLARE @GOODBAL MONEY 
      DECLARE @PSID INT 
      DECLARE @OPSID INT 
      DECLARE @RID INT 
      DECLARE @DurID INT 
      DECLARE @SType TINYINT 
      DECLARE @TDate SMALLDATETIME 
      DECLARE @FinalAmount MONEY 

      SELECT @TDate = tdate + 1 
      FROM   fin.licensebranch l 
             INNER JOIN fin.adetail 
                     ON l.brnhid = adetail.brchid 
      WHERE  iaccno = @IAccno 

      SELECT @Bal = adetail.bal, 
             @IonBal = adetail.ionbal, 
             @SType = stype 
      FROM   fin.adetail 
             INNER JOIN fin.productdetail 
                     ON adetail.pid = productdetail.pid 
             INNER JOIN fin.schmdetails 
                     ON productdetail.sdid = schmdetails.sdid 
      WHERE  iaccno = @IAccNo 

      SELECT @Psid = psid 
      FROM   fin.apolicyinterest 
      WHERE  iaccno = @IAccNo 

      SELECT @RID = rid, 
             @BALID = balid, 
             @DURID = durid 
      FROM   fin.policyintcalc 
      WHERE  psid = @PSID 

      IF @BALID = 4 
         AND @DURID = 1 
         AND @tillDate = @TDate 
        -- Daily Outstanding Balance and For the transaction date edit only 
        BEGIN 
            SELECT @GOODBAL = Sum(pridr) - Sum(pricr) 
            FROM   [fin].[Fgetloantransactionamount](@IAccNo) 

            IF @GOODBAL >= 0 
              SET @FinalAmount = @GOODBAL 
            ELSE 
              BEGIN 
                  SET @FinalAmount = 0 
              END 
        END 

      IF @BALID = 6 
         AND @DURID = 1 
         AND @tillDate = @TDate 
        -- Daily Flat Balance and For the transaction date edit only 
        BEGIN 
            SELECT @GOODBAL = Sum(pridr) 
            FROM   [fin].[Fgetloantransactionamount](@IAccNo) 

            IF @GOODBAL <> @IonBal 
              SET @FinalAmount = @GOODBAL 
        END 

      RETURN isnull(@FinalAmount,0)
  END
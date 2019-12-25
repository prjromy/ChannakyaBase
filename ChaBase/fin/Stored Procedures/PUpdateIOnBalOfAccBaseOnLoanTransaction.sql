CREATE proc [fin].[PUpdateIOnBalOfAccBaseOnLoanTransaction] (@tillDate DATE, 
                                                          @IAccno   INT) 
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

      SELECT @TDate = TDate + 1 
      FROM   LG.Company l 
             INNER JOIN fin.adetail 
                     ON l.CompanyId = adetail.brchid 
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
             	Update fin.ADetail Set IonBal = @GOODBAL where  IAccNo = @IAccNo
            ELSE 
              BEGIN 
                 	Update fin.ADetail Set IonBal = 0 where  IAccNo = @IAccNo
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
             	Update fin.ADetail Set IonBal = @GOODBAL where  IAccNo = @IAccNo
        END 

     
  END
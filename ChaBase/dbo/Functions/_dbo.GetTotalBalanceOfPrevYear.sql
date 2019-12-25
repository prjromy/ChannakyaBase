CREATE FUNCTION [dbo].[dbo.GetTotalBalanceOfPrevYear] 
(
)
RETURNS   @finaltbl TABLE
(
TotalDebitAmount numeric(18,2) , TotalCreditAmount numeric(18,2)
)
AS
BEGIN
      -- Declare local variable here
	  DECLARE @FID int
      DECLARE @totalDebitAmount numeric(18,2)=0
      DECLARE @totalCreditAmount numeric(18,2)=0

      DECLARE my_cursor CURSOR LOCAL FORWARD_ONLY
          FOR
            SELECT FID
              FROM acc.VoucherBal 
             --WHERE FYId=12
               
 
      -- Open the cursor
      OPEN my_cursor
      -- ...go through each option and return the item description with comma
 
      FETCH NEXT FROM my_cursor INTO @FID
             WHILE @@FETCH_STATUS = 0
             BEGIN
               SET @totalDebitAmount = @totalDebitAmount + ISNULL((select DebitAmount from dbo.GetTrialBalanceDataPrevYear(@FID)),0)
			   SET @totalCreditAmount = @totalCreditAmount + ISNULL((select CreditAmount from dbo.GetTrialBalanceDataPrevYear(@FID)),0) 
               FETCH NEXT FROM my_cursor INTO @FID
 
            END
      -- Close the cursor
      CLOSE my_cursor
      DEALLOCATE my_cursor
	  insert into @finaltbl VALUES(@totalDebitAmount,@totalCreditAmount)
      
      RETURN
 
END
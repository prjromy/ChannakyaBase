CREATE PROCEDURE [fin].[PODIonBalupd] 
	(
		@IACCNO INT,
		@TDATE SMALLDATETIME
	)
---MARKED ENCRYPTION 
As
Begin
		Declare @Bal Money
		Declare @IonBal Money
		Declare @GoodBal Money
		Declare @OpBal Money
		Declare @OPSID TinyInt
		Declare @RID TinyInt
		Declare @BalID TinyInt
		Declare @DurID TinyInt
		Declare @DrAmt Money
		Declare @CrAmt Money
		Declare @TDate1 SmallDateTime
		Declare @MinBal Money
		
		Declare @accountBalance Money
		Declare @AsTransBal Money
			--Select  @Bal = SUM(CrAmt) - SUM(DrAmt) From  FGetVWADStmnt1(@IAccNo,@TDate1)
		Set @TDate1 = @TDate + 1
		SELECT     @AsTransBal= PriDr - PriCr FROM  fin.FgetLoanTransactionAmount(@IAccNo)	   
		 select @accountBalance=(select Bal from fin.ADetail where IAccno=@IACCNO)
		
		set  @Bal = @accountBalance-@AsTransBal
		If @Bal < 0
		Begin
			Select @OPSID = PSID From fin.APolicyInterest Where IAccNo = @IAccNo
			Select @RID = RID,@BalID = BalID,@DurID = DurID From PolicyIntCalc Where PSID = @OPSID
			
			Set @MinBal = (Select Case When Exists 
					(Select * From fin.AMinBal Where AMinBal.IAccNo = ADetail.IAccNo) 
				Then (Select MinBal From fin.AMinBal Where AMinBal.IAccNo = ADetail.IAccNo)
				Else
				(Select LAmt From fin.ProductDetail Where ProductDetail.Pid = ADetail.Pid)
				End
				From ADetail
			Where ADetail.IAccNo = @IAccNo)

			If /*@RID = 1 And */ @BalID = 4 And @DurID = 1
			Begin
				Set @IonBal = @Bal - @MinBal
				Update fin.ADetail Set IonBal = @IonBal Where IAccNo = @IAccNo
			End
			
			If /*@RID = 1 And*/ @BalID = 1 And @DurID = 1
			Begin
			--	(SELECT   @OpBal =  isnull( SUM(CrAmt)-SUM(DrAmt) ,0) FROM  fin.FGetVWADStmnt1(@IAccNo,@TDate) )
				Set @GoodBal = @Bal
				Set @IonBal = @GoodBal
				
				Declare RsTra Cursor  For  
				Select  DrAmt, CrAmt From  fin.FGetVWADStmnt2(@IAccNo,@TDate,@TDate) Order By VDate,TNo

				Open RsTra 
				Fetch RsTra Into @DrAmt,@CrAmt
				While @@Fetch_Status = 0
				Begin
					Set @GoodBal = @GoodBal - @DrAmt + @CrAmt
					If  @GoodBal < @IonBal 				
						Set @IonBal = @GoodBal					
				Fetch RsTra Into @DrAmt,@CrAmt					
				End
				Close RsTra
				Deallocate RsTra
				Update fin.ADetail Set IonBal = (@IonBal - @MinBal) Where IAccNo = @IAccNo
			End	
		End
End
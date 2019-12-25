CREATE PROCEDURE [fin].[PUIonbalOfIAcc] (@IACCNO NUMERIC ,@EDTDATE SMALLDATETIME)
---MARKED ENCRYPTION 
AS

/*
PSID	PSName				RID	BALID	DURID
1	Daily Outstanding Balance		1	4	1
2	Daily Min Balance			1	1	1
3	Daily Exceeding Balance		1	5	1
4	Monthly Min Balance			1	1	3		
5	Flat					1	6	1
*/

Declare @BalID Int
Declare @Bal Money
Declare @IonBal Money
Declare @GOODBAL Money
Declare @PSID Int
Declare @OPSID Int
Declare  @RID Int
Declare @DurID Int
Declare @SType TinyInt
Declare @OpBal Money
Declare @DrAmt Money
Declare @CrAmt Money
Declare @IntBal Money
Declare @Ffdate SmallDateTime
Declare @TDate SmallDateTime
Declare @NewIonBal Money 
Declare @Bal1 Money
Declare @TDate1 SmallDateTime
Declare @MDepPro TinyInt
Declare @WDepPro TinyInt
Declare @2_day_date SmallDateTime
Declare @IsOD Bit
Declare @MinBal Money
Declare @RDate SmallDateTime
Begin 

	Select @TDate = BrnchGlobal.Tdate + 1  
		From BrnchGlobal 
	INNER JOIN ADetail ON BrnchGlobal.BrnchID = ADetail.BrchID  
	WHERE (ADetail.IAccno = @IAccNo)

	Declare RSIACCNO CURSOR FOR 
	Select ADetail.IAccNo, ADetail.Bal, ADetail.IonBal,SType 
		FROM   ADetail 
	INNER JOIN ProductDetail ON ADetail.PID = ProductDetail.PID INNER JOIN
		SchmDetails ON ProductDetail.sdid = SchmDetails.SDID  Where IAccNo = @IAccNo

	OPEN RSIACCNO
	FETCH from RSIACCNO INTO @IAccNo,@Bal,@IonBal,@SType
	
	select @PSID=PSID from fin.APolicyInterest where IAccno= @IAccNo	
		
		Select  @OPSID =OPSID from fin.AOPolInt Where IAccNo = @IAccNo			
			
		SELECT @RID = RID,@BALID = BALID,@DURID = DURID FROM fin.PolicyIntCalc Where PSID = @PSID
			If @BALID = 4 AND @DURID = 1 and @EdtDate =  @TDate -- Daily Outstanding Balance and For the transaction date edit only
			Begin		
				Set @TDate1= @TDate + 1			
								
				SELECT     @GOODBAL=ISNULL(PriDr - PriCr,0)FROM  fin.FgetLoanTransactionAmount(@IAccNo)					
					
				If @GOODBAL >= 0							
					Update ADetail Set IonBal = @GOODBAL Where  IAccNo = @IAccNo
				Else
				Begin
					If (Select IsOD From ADur Where IAccNo = @IAccNo) = 1 And @SType = 0
						Exec fin.PODIonBalupd @IAccNo,@TDate
					Else 
						Update ADetail Set IonBal = 0 Where  IAccNo = @IAccNo
				End					
			End

			If @BALID = 6 AND @DURID = 1 and @EdtDate = @TDate -- Daily Flat Balance and For the transaction date edit only
			Begin		
				Set @TDate1= @TDate + 1			
							
				SELECT     @GOODBAL= ISNULL(PriDr,0)  FROM  fin.FgetLoanTransactionAmount(@IAccNo)					
					
				If @GOODBAL <> @IonBal							
					Update ADetail Set IonBal = @GOODBAL where  IAccNo = @IAccNo
			End


			If @BALID = 5 AND @DURID = 1 and @EdtDate = @TDate ----------Daily Excedding Balanceand For the transaction date edit only
			Begin		
				Set @TDate1= @TDate + 1					
				Declare @MinusBal Money		
				/*
				Select @MinusBal = 
				(Select 'x'=case when exists (Select IAccNo From AMinBal  Where IAccNo = @IAccNo) 
				Then (Select MinBal From AMinBal  Where IAccNo = @IAccNo) Else 0 End) 
				+ (Select isnull(LAmt,0) From ProductDetail Inner Join ADetail on ProductDetail.Pid=ADetail.Pid
				Where ADetail.IAccNo = @IAccNo)
				*/
				Select @MinusBal = (Select 'X' = Case When Exists (Select IAccNo From AMinBal  Where IAccNo = @IAccNo) 
							Then (Select MinBal From AMinBal  Where IAccNo = @IAccNo) 
							Else IsNull(LAmt,0) End 
						From ProductDetail Inner Join ADetail On ProductDetail.PID = ADetail.PID 
						Where ADetail.IAccNo = @IAccNo)

				--SELECT   @Bal1= SUM(CrAmt) - SUM(DrAmt) FROM   FGetVWADStmnt1(@IAccNo,@TDate1) 
				SELECT     @Bal1= ISNULL(PriDr- PriCr,0) FROM  fin.FgetLoanTransactionAmount(@IAccNo)
				If @Bal1 >= 0
				Begin
					Select  @NewIonBal = @Bal1- @MinusBal From ADetail Where IAccNo = @IAccNo
					If  @NewIonBal >= 0
						Update ADetail Set IonBal = @NewIonBal Where  IAccNo = @IAccNo
	 				Else
	 					Update ADetail Set IonBal = 0 Where  IAccNo = @IAccNo
				End
				Else 
				Begin
					If (Select IsOD From ADur Where IAccNo = @IAccNo) = 1 And @SType = 0
							Exec fin.PODIonBalupd @IAccNo,@TDate
					Else 
						Update ADetail Set IonBal = 0 Where  IAccNo = @IAccNo
				End
			End
			-----end of Daily Excedding Balance

			If @BALID = 1 AND @DURID = 1 and @EdtDate = @TDate -- Daily min Balance and For the transaction date edit only
			Begin
				Set @FfDate =  @TDate - 1
				Set @TDate1= @TDate + 1
		--		SELECT   @OpBAL =  isnull( SUM(CrAmt)-SUM(DrAmt) ,0) FROM  fin.FGetVWADStmnt1(@IAccNo,@TDate)
					
				(SELECT     @OpBAL= ISNULL(PriDr - PriCr,0) FROM  fin.FgetLoanTransactionAmount(@IAccNo))
				Set @GOODBAL = @OpBAL
				Set @IntBal = @GOODBAL
				
				Declare RsTra Cursor  for  
				SELECT  DrAmt, CrAmt FROM fin.FGetVWADStmnt2(@IAccNo,@TDate,@TDate) Order By VDate,TNo

				Open RsTra 
				Fetch RsTra Into @DrAmt,@CrAmt
				While @@Fetch_Status = 0
				Begin
					Set @GOODBAL = @GOODBAL - @DrAmt + @CrAmt
					If  @IntBal > @GOODBAL 				
						Set @IntBal = @GOODBAL					
				Fetch RsTra Into @DrAmt,@CrAmt					
				End
				Close RsTra
				Deallocate RsTra
			--	Select  IsNull( SUM(CrAmt) - SUM(DrAmt) ,0) From  fin.FGetVWADStmnt1(@IAccNo,@TDate1)
				If (SELECT   ISNULL(PriDr - PriCr,0) FROM  fin.FgetLoanTransactionAmount(@IAccNo) ) >= 0
				Begin					
						If  @IntBal > 0
							Update ADetail set IonBal = @IntBal Where  IAccNo = @IAccNo
	 					Else 
	 						Update ADetail set IonBal = 0 Where  IAccNo = @IAccNo
				End
				Else
				Begin
					If (Select IsOD From ADur Where IAccNo = @IAccNo) = 1 And @SType = 0
						Exec fin.PODIonBalupd @IAccNo,@TDate
					Else 
						Update ADetail Set IonBal = 0 Where  IAccNo = @IAccNo
				End
			End

			If @BALID = 1 AND @DURID = 3	-- Monthly min Balance
			Begin
				Set @Ffdate = (Select EngDate From  ConvertDate 
						Where  (d = 1) AND (m =(Select m From  ConvertDate Where   (EngDate = @TDate))) AND 
						(y = (Select y  From    ConvertDate  Where  (EngDate = @TDate))))
		
				If @EdtDate  >= @Ffdate and @EdtDate <=  @TDate
				Begin
					Set @TDate1 = @TDate + 1
					Set @MDepPro = 1
					Set @OpBaL = 0
					Set @2_day_date = @Ffdate +  @MDepPro
					Set @RDate = (Select Min(TDate) + 1  From Trans.AMTrns Where IAccNo = @IAccNo)
					If @RDate > @2_day_date
						Set @2_day_date = @RDate
					
					Set @OpBaL = (Select IsNull( SUM(CrAmt)-SUM(DrAmt) ,0)FROM  FGetVWADStmnt1(@IAccNo,@2_day_date))

					Declare RsTra Cursor  For  
					SELECT  DrAmt, CrAmt
					FROM   fin.FGetVWADStmnt2(@IAccNo,@2_day_date,@TDate)
					Order By VDate,TNo

					Set @GoodBal = @OpBal
					Set @IntBal = @GoodBal
					Open RsTra 
					Fetch RsTra Into @DrAmt,@CrAmt
					While @@Fetch_Status=0
					Begin
						Set @GoodBal = @GoodBal - @DrAmt + @CrAmt
						If @IntBal > @GoodBal 				
							Set @IntBal = @GoodBal
					Fetch RsTra Into @DrAmt,@CrAmt					
					End
					Close RsTra
					Deallocate RsTra
					--Select  IsNull( SUM(CrAmt) - SUM(DrAmt) ,0) From  fin.FGetVWADStmnt1(@IAccNo,@TDate1)
					If (SELECT   ISNULL(PriDr - PriCr,0) FROM  fin.FgetLoanTransactionAmount(@IAccNo) ) >= 0
					Begin						
							If @IntBal > 0
								Update ADetail Set IonBal = @IntBal Where  IAccNo = @IAccNo
		 					Else 
		 						Update ADetail Set IonBal = 0 Where  IAccNo = @IAccNo
					End
					Else
					Begin
						If (Select IsOD From ADur Where IAccNo = @IAccNo) = 1 And @SType = 0
							Exec fin.PODIonBalupd @IAccNo,@TDate
						Else 
							Update ADetail Set IonBal = 0 Where  IAccNo = @IAccNo
					End
				End
			End


			If @BALID = 1 AND @DURID = 2	-- Weekly min Balance and update made within the week		
			Begin
				Set @FfDate = @TDate - DATEPART(dw,@TDate ) + 1 --Detect the First day date of the @tdate				
				Set @WDepPro = 1
				Set @2_day_date = @FfDate + @MDepPro
				Set @TDate = @TDate + 1

				If @EdtDate>=@FfDate And @EdtDate <= @TDate
				Begin
					(SELECT @OpBAL = isnull(SUM(CrAmt)-SUM(DrAmt),0) FROM FGetVWADStmnt1(@IAccNo,@2_day_date))

					Declare RsTra Cursor  For  
					SELECT DrAmt,CrAmt FROM fin.FGetVWADStmnt2(@IAccNo,@2_day_date,@TDate)
					Order By VDate,TNo

					Set @GoodBal =  @OpBal
					Set @IntBal = @GoodBal

					Open RsTra 
					Fetch RsTra Into @DrAmt,@CrAmt
					While @@Fetch_Status=0
					Begin
						Set @GoodBal = @GoodBal - @DrAmt + @CrAmt
						If @IntBal > @GoodBal 				
							Set @IntBal = @GoodBal
						
					Fetch RsTra Into @DrAmt,@CrAmt					
					End
					Close RsTra
					Deallocate RsTra
					If (Select  IsNull( SUM(CrAmt) - SUM(DrAmt) ,0) From  fin.FGetVWADStmnt1(@IAccNo,@TDate1) ) >= 0
					Begin					
							If @IntBal > 0
								Update ADetail Set IonBal = @IntBal where  IAccNo = @IAccNo
		 					Else 
		 						Update ADetail Set IonBal = 0 where  IAccNo = @IAccNo
					End
					Else
					Begin
						If (Select IsOD From ADur Where IAccNo = @IAccNo) = 1 And @SType = 0
							Exec fin.PODIonBalupd @IAccNo,@TDate
						Else 
							Update ADetail Set IonBal = 0 Where  IAccNo = @IAccNo
					End
				End
			End
	CLOSE RSIACCNO
	DEALLOCATE RSIACCNO
End
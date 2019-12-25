CREATE PROCEDURE [fin].[PUpdAdjustedAmt]
(
	@IACCNO NUMERIC,
	@PID INT,
	@AMT MONEY,
	@USERID INT,
	@TDATE SMALLDATETIME,
	@NOTES NVARCHAR(200)
	
)
---MARKED ENCRYPTION 
As
Begin
	
	Declare @InstCalcInt Money
	Declare @Result NVarchar(100)
	
	If Exists (Select * From fin.FGetAccType(1) Where IAccno = @IACCNO)
	Begin
		If @TDate < (Select RDate From fin.ADetail Where IAccno = @IACCNO)
		Begin
			Raiserror ('Adjustment date should not be before A/c opened date',15,15)
			Goto GoOut
		End
		If @TDate > (Select DATEADD(D,1,Tdate) From LG.Company Where CompanyID = (Select BrchID From fin.ADetail Where IAccno =@IACCNO))
		Begin
			Raiserror('Adjustment date should not greater transaction date',15,15)
			Goto GoOut
		End
		If @PID = 0
		Begin
			If @Amt < 0
			Begin
				Set @InstCalcInt = IsNull((Select IsNull(CalcInt,0) From fin.FGetSchCalcInt(@IACCNO) 
										Where  @TDate >= FrmDate And @TDate < ToDate),0)
				If (@Amt + @InstCalcInt) < 0
				Begin
					Raiserror('Adjusted amount is greater than interest calculated on corresponding installment',15,15)
					Goto GoOut
				End
			End
		End
	End
	Declare @TNo Numeric
	 exec [Mast].[GetTransno] @TNO out 
	
    Insert Into fin.AAdjustmnt(Iaccno,Pid,Amt,TNo,UserId,TDate,Note) Values (@IACCNO,@PID,@AMT,@TNo,@USERID ,@TDATE,@NOTES)
	
	exec [dbo].[pset_updateinttocap]@IACCNO,@PID,@AMT
	
	GoOut:
End
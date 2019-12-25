--select * from brnchGlobal
--Select * From ALSch Where IAccno = 99 Order By schDate
--Select * From FGetSchCalcInt(99)
	
CREATE FUNCTION [fin].[FGetSchCalcInt]
(
	@IAccNo Numeric
)
Returns @Tb Table (SNo Numeric,FrmDate SmallDateTime,ToDate SmallDateTime,CalcInt Money)
As
Begin
	--Declare @IAccNo Numeric
	--Declare @PID Int
	--Declare @TDate SmallDateTime
	--Set @IAccNo = 85
	--Set @PID = 0
	--Set @TDate = '5/17/2015'

	Declare @SNo Int 
	Declare @SchDate SmallDateTime
	Declare @AccOpnDate SmallDateTime
	Declare @FrmDate SmallDateTime
	Declare @LstInstDate SmallDateTime
	Declare @CalcInt Money
	Declare @TempTB Table (Sno Int,FrmDate SmallDateTime,ToDate SmallDateTime,CalcInt Money)

	Set @AccOpnDate = (Select RDate From fin.ADetail Where IAccno = @IAccNo)
	Set @SNo = 1
	Set @FrmDate = @AccOpnDate
	Declare CrSr Cursor Local For
		Select schDate From fin.ALSch Where IAccno = @IAccNo Order By schDate
		
	Open CrSr
	Fetch From CrSr Into @SchDate
	While @@FETCH_STATUS = 0
	Begin
		Set @CalcInt = (Select IsNull(Sum(Intcal),0) From fin.IntLog 
							Where IAccno = @IAccNo And Valued = 0 And Tdate >= @FrmDate And Tdate < @SchDate) +
						(Select IsNull(Sum(Amt),0) From fin.Aadjustmnt 
							Where Iaccno = @IAccNo And Pid = 0 And TDate >= @FrmDate And TDate < @SchDate)
		Insert Into @TempTB Values (@SNo,@FrmDate,@SchDate,@CalcInt)
		Set @FrmDate = @SchDate
		Set @SNo = @SNo + 1
		Fetch Next From CrSr Into @SchDate
	End
	Close CrSr
	Deallocate CrSr
	
	Set @LstInstDate = (Select MAX(SchDate) From fin.ALSch Where IAccno = @IAccNo)
	If Exists (Select * From fin.IntLog Where IAccno = @IAccNo And Valued = 0 And Tdate >= @LstInstDate)
	Begin
		Set @CalcInt = (Select IsNull(Sum(Intcal),0) From fin.IntLog Where IAccno = @IAccNo And Valued = 0 And Tdate >= @LstInstDate)
		Set @SNo = (Select MAX(Sno) From @TempTB)
		Set @FrmDate = (Select Top 1 FrmDate From @TempTB Where Sno = @SNo)
		Set @SchDate = (Select DATEADD(D,2,Tdate) From BrnchGlobal Where BrnchID = (Select BrchID From fin.ADetail Where IAccno = @IAccNo))
		Update @TempTB Set ToDate = @SchDate Where Sno = @SNo
		Insert Into @TempTB Values (@SNo,@FrmDate,@SchDate,@CalcInt)
	End
	
	If Exists (Select * From fin.Aadjustmnt Where Iaccno = @IAccNo And Pid = 0 And TDate >= @LstInstDate)
	Begin
		Set @CalcInt = (Select IsNull(Sum(Amt),0) From fin.Aadjustmnt Where Iaccno = @IAccNo And Pid = 0 And TDate >= @LstInstDate)
		Set @SNo = (Select MAX(Sno) From @TempTB)
		Set @FrmDate = (Select Top 1 FrmDate From @TempTB Where Sno = @SNo)
		Set @SchDate = (Select DATEADD(D,2,Tdate) From lg.Company Where companyid = (Select BrchID From ADetail Where IAccno = @IAccNo))
		Update @TempTB Set ToDate = @SchDate Where Sno = @SNo
		Insert Into @TempTB Values (@SNo,@FrmDate,@SchDate,@CalcInt)
	End
	
	Insert Into @Tb
	--Select Sno,FrmDate,ToDate,CalcInt As CalcInt From @TempTB
	Select Sno,FrmDate,ToDate,IsNull(SUM(CalcInt),0) As CalcInt From @TempTB Group By Sno,FrmDate,ToDate

	Return
End
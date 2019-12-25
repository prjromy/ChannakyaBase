CREATE FUNCTION [fin].[FGetReportDepartBalanceTillDateAmountWise] 

	(

		@BRCHID INT,

		@TDATE SMALLDATETIME,

		@FAMT MONEY,

		@TAMT MONEY,

		@SORTID INT

	)

RETURNS @TEMP TABLE

	(

		AccountNumber NVARCHAR(50),AccountName NVARCHAR(100),IntToExp MONEY,IntToCap MONEY,Balance MONEY

	)

---MARKED ENCRYPTION 

AS

Begin

If @SortID = 1 -- For Between

Begin

Insert Into @Temp 

Select ADetail.AccNo,ADetail.AName,X.IntToExp,X.IntToCap,X.Bal From Fin.FGetReportDepositBalanceTillDate(@TDate,@Brchid) X

Inner Join fin.ADetail On X.IAccNo = ADetail.IAccNo

Where X.Bal between @FAmt and @TAmt

End



If @SortID = 2 -- For Greater Than Equal to 

Begin

Insert Into @Temp 

Select ADetail.AccNo,ADetail.AName,X.IntToExp,X.IntToCap,X.Bal From Fin.FGetReportDepositBalanceTillDate(@TDate,@Brchid) X

Inner Join fin.ADetail On X.IAccNo = ADetail.IAccNo

Where X.Bal >= @FAmt

End



If @SortID = 3 -- For Less Than Equal to 

Begin

Insert Into @Temp 

Select ADetail.AccNo,ADetail.AName,X.IntToExp,X.IntToCap,X.Bal From Fin.FGetReportDepositBalanceTillDate(@TDate,@Brchid) X

Inner Join fin.ADetail On X.IAccNo = ADetail.IAccNo

Where X.Bal <= @FAmt

End



Return

End
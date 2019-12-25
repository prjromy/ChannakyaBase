CREATE  FUNCTION [fin].[FGETReportLoanBeforeAutoPayment]

	(
	 @branchId INT=null,
	  @Pid INT=null
	)

RETURNS TABLE
As

Return

Select 	ALoan.IAccNo,ADetail.AccNo,ProductDetail.PName,ADetail.AName,ADetail.Bal,IsNull(Y.PostP,0) PostP,IsNull(PaidP,0) PaidP,
	IsNull(Y.PostP,0) - IsNull(PaidP,0) MatPrin,IsNull(PMDays,0) PMDays,
	IsNull(PostI,0) PostI,IsNull(PaidI,0) PaidI,IsNull(IMDays,0) IMDays,IsNull(MatInt,0) MatInt,
	IsNull(ALoan.PonPrA,0) + IsNull(ALoan.PonPrR,0) + IsNull(ALoan.PonIA,0) + 
	IsNull(ALoan.PonIR,0) + IsNull(ALoan.IonIA,0) + IsNull(ALoan.IonIR,0) AS PenAcc,
	IsNull(ALoan.OthrBal,0) + IsNull(ALoan.IonCA,0) + IsNull(ALoan.IonCR,0) AS Other,ALinkloan.ILinkAc,AD.AccNo LnkAccNo,PD.PName LnkPName,
	IsNull((Select Fin.FGetGETAVLBALANCE(ALinkloan.ILinkAc,(Select TDate From lg.Company Where CompanyId = @branchId),0)),0) As AvlBal,ALoan.Revolving,PD.PID
From fin.ALinkloan
Inner Join fin.ALoan On ALinkLoan.IAccNo = ALoan.IAccNo
Inner Join fin.ADetail On ALoan.IAccNo = ADetail.IAccNo
Inner Join fin.ProductDetail On ADetail.PID = ProductDetail.PID
Inner Join fin.ADetail AD On AD.IAccNo = ALinkloan.ILinkAc
Inner Join fin.ProductDetail PD On PD.PID = AD.PID
Left Join
(
Select IAccNo,BrchID,Sum(PostP) PostP,Sum(PaidP) PaidP,Sum(PMDays) PMDays,Sum(MatPri) MatPri,
Sum(PostI) PostI,Sum(PaidI) PaidI,Sum(IMDays) IMDays,Sum(MI) MatInt From 
(
	--For Non Revolving Loan Principal
		Select  ALSch.IAccNo, ADetail.BrchID,Sum(IsNull(SchPrin,0)) As PostP,Sum(IsNull(PdPrin,0)) As PaidP,
		DateDiff(Day,Min(SchDate),Company.TDate + 1) As PMdays,Sum(IsNull(SchPrin,0)) - Sum(IsNull(PdPrin,0)) As MatPri,
		0 As IMdays,0 As PostI,0 As PaidI ,0 As MI
		FROM  fin.ALSch
		Inner Join fin.ADetail On ALSch.IAccNo = ADetail.IAccNo
		Inner Join  lg.Company on ADetail.Brchid = lg.Company.CompanyId
		Inner Join fin.ALoan On ADetail.IAccNo = ALoan.IAccNo
		WHERE  SchDate <= Company.TDate And  (IsNull(SchPrin,0)) - (IsNull(PdPrin,0)) <> 0  and ADetail.Bal > 0 And ALoan.Revolving = 0 
		GROUP BY ALSch.IAccNo,Company.TDate,ADetail.BrchID
		Having DateDiff(Day,Min(SchDate),Company.TDate ) >= 0
		Union All
		---Added For Revolving Loan Principal
		Select  ALSch.IAccNo, ADetail.BrchID,Sum(IsNull(SchPrin,0)) As PostP,Sum(IsNull(PdPrin,0)) As PaidP,
		DateDiff(Day,Min(SchDate),Company.TDate ) As PMdays,Sum(IsNull(SchPrin,0)) - Sum(IsNull(PdPrin,0)) As MatPri,
		0 As IMdays,0 As PostI,0 As PaidI ,0 As MI
		FROM  fin.ALSch
		Inner Join fin.ADetail On ALSch.IAccNo = ADetail.IAccNo
		Inner Join  lg.Company on ADetail.Brchid = Company.CompanyId
		Inner Join fin.ALoan On ADetail.IAccNo = ALoan.IAccNo
		WHERE  SchDate <= Company.TDate  And  (IsNull(SchPrin,0)) - (IsNull(PdPrin,0)) <> 0  and ADetail.Bal > 0 And ALoan.Revolving = 1 
		GROUP BY ALSch.IAccNo,Company.TDate,ADetail.BrchID
		Union All
		----For Matured Interest Of Loan
		Select 	ALSch.IAccNo,ADetail.Brchid,0 As PostP,0 PaidP,0 As PMdays,0 As MatPri, 
		DateDiff(Day,Min(SchDate),Company.TDate) As IMDays, Sum(IsNull(CalcInt, 0)) As PostI, 
		Sum(IsNull(PdInt, 0)) AS PaidI,Sum(IsNull(CalcInt, 0)) - Sum(IsNull(PdInt, 0)) As MI
		FROM fin.ALSch
		Inner Join fin.ADetail On ALSch.IAccNo = ADetail.IAccNo
		Inner Join lg.Company On ADetail.BrchID = Company.CompanyId
		Where (SchDate <= Company.TDate) And (IsNull(CalcInt, 0) - IsNull(PdInt, 0) <> 0) 
		GROUP BY ALSch.IAccNo,Company.TDate,ADetail.BrchID 
	) X
Group By IAccNo,BrchID) Y On ALoan.IAccNo = Y.IAccNo
Where (ALoan.AutoPayment = 1) And (ADetail.AccState <> 2) And (ADetail.BrchID =coalesce(@branchId,ADetail.BrchID) ) and (ProductDetail.PID=coalesce(@Pid,ProductDetail.PID))
CREATE FUNCTION [fin].[FGetReportLoanMatured]

	(

		@TDATE SMALLDATETIME,

		@BRCHID INT=null

	)

RETURNS TABLE

---MARKED ENCRYPTION 

As

Return

	Select X.PName,X.IAccNo,X.AccNo,X.Name,X.ApprovedAmt,X.DisbursedAmt,X.Balance,Z.MDate,Z.MDays,Y.PrincipalPybl,Y.PrincipalPaid,

		Y.MaturedPrincipal ,Y.InterestPybl,Y.InterestPaid,Y.MaturedInterest,PrincipalPybl - PrincipalPaid As 'MaturedPrin' 

	From 

	(Select distinct ProductDetail.PName,ADetail.IAccNo,ADetail.AccNo,ADetail.AName Name,ADrLimit.AppAmt ApprovedAmt,

  'DisbursedAmt'=aloan.PrincipleLoanOut,

		 	'Balance'= ISNULL(ADrLimit.AppAmt,0)-isnull(ALoan.PrincipleLoanOut,0)

						From fin.ADetail

						Inner Join fin.ALoan On ADetail.IAccNo = ALoan.IAccNo

						Inner Join fin.ADrLimit On ADetail.IAccNo = ADrLimit.IAccNo

						Inner Join fin.ProductDetail On ProductDetail.Pid = ADetail.Pid 
						inner join  fin.FGetLoanDisburseDetails() a on ADetail.IAccno=a.IAccno
					
					
						Where ADetail.IAccNo In (Select * From fin.FGetAccType(1)) And ADetail.AccState <> 3) X 

	Inner Join (SELECT  ALSch.IAccNo, ADetail.BrchId,

		Sum(IsNull(SchPrin,0)) as PrincipalPybl,Sum(IsNull(PdPrin,0)) as PrincipalPaid,

		Sum(IsNull(SchPrin,0)) - Sum(IsNull(PdPrin,0)) as MaturedPrincipal,

		Sum(IsNull(CalcInt,0)) as InterestPybl,Sum(IsNull(PdInt,0)) as InterestPaid,

		Sum(IsNull(CalcInt,0)) - Sum(IsNull(PdInt,0)) as MaturedInterest

	FROM  fin.ALSch 

	Inner Join fin.ADetail on ALSch.IAccNo = ADetail.IAccNo

	Inner Join  lg.Company on ADetail.Brchid = Company.CompanyId

	WHERE SchDate <= @TDate /* And  (((IsNull(schprin,0)) -(IsNull(pdprin,0)) > 0) or ((IsNull(CalcInt,0)) -(IsNull(PdInt,0)) > 0))*/

		And ADetail.Bal > 0 and ADetail.AccState <> 3 And ADetail.BrchID =coalesce( @BrchID,ADetail.BrchID)

	GROUP BY ALSch.IAccNo, Company.TDate ,ADetail.BrchId

	Having DateDiff(Day,Min(SchDate),@TDate) > 0) Y On X.IAccNo =Y.IAccNo

	Inner Join 

	(SELECT  ALSch.IAccNo, ADetail.BrchId, Min(SchDate) AS MDate,DateDiff(Day,Min(SchDate),@TDate) as MDays

		FROM  fin.ALSch 

	Inner Join fin.ADetail on ALSch.IAccNo = ADetail.IAccNo

	Inner Join  lg.Company on ADetail.Brchid = Company.CompanyId

	WHERE SchDate <= @TDate And  (((IsNull(schprin,0)) -(IsNull(pdprin,0)) > 0) or ((IsNull(CalcInt,0)) -(IsNull(PdInt,0)) > 0))

		And ADetail.Bal > 0 and ADetail.AccState <> 3 And ADetail.BrchID = coalesce(@BrchID,ADetail.BrchID)

	GROUP BY ALSch.IAccNo, Company.TDate ,ADetail.BrchId

	Having DateDiff(Day,Min(SchDate),@TDate) > 0) Z On X.IAccNo = Z.IAccNo
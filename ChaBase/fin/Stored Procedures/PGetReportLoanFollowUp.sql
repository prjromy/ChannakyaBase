 CREATE proc [fin].[PGetReportLoanFollowUp]( 

 @TDATE	SMALLDATETIME,

@BRHID	INT,

 @IACNO	INT=null

 )



 as

 begin

CREATE TABLE #Temploanfollowup

	(

		pid tinyint ,

		lOANTYPE nvarchar(200) ,

		iaccno int ,

		LoanNo nvarchar(20) ,

		brhid tinyint,

		Loanee nvarchar(100) ,

		OUTBAL decimal(18,2) ,

		Matpri decimal(18,2) ,	

		IAcc decimal(18,2) ,		

		Pen decimal(18,2),

		OTH decimal(18,2) ,

		OTHINT decimal(18,2) ,		

		RATE decimal(18,2) ,

		NODAYS int ,

		FFINT  AS (round(([OUTBAL] * [RATE] * [NODAYS] / 36500),2)),

		TOTDUE  AS (Matpri +IAcc+Pen+ round(([OUTBAL] * [RATE] * [NODAYS] / 36500),2) + [OTH] + [OTHINT])

	)  ON [PRIMARY]



Declare @NODAYS int

Declare @brhdate smalldatetime



Set @brhdate = (SELECT Tdate FROM lg.Company WHERE (CompanyId = 1))

SET @NODAYS = DATEDIFF(day,@brhdate, @tdate)+1





Insert Into #Temploanfollowup 

SELECT ProductDetail.PID, ProductDetail.PName, ADetail.IAccno, ADetail.Accno, ADetail.BrchID, ADetail.Aname, ADetail.Bal, 

	IsNull((SELECT  sum(schprin)-sum(pdprin) FROM fin.ALSch Where iaccno=adetail.iaccno and schdate<=@tdate),0) Matpri ,

	ALoan.IonPA + ALoan.IonPR AS IntACC, ALoan.PonPrA + ALoan.PonPrR + ALoan.PonIA + ALoan.PonIR + ALoan.IonIR + ALoan.IonIA AS penalty, 

	ALoan.OthrBal, ALoan.IonCA + ALoan.IonCR AS IntOth,  ADetail.IRate as CRATE, @NODAYS AS Nodays

FROM fin.ALoan

INNER JOIN fin.ADetail ON ALoan.IAccno = ADetail.IAccno

INNER JOIN fin.ProductDetail ON ADetail.PID = ProductDetail.PID

WHERE (ADetail.AccState <> 2) And ADetail.BrchID=coalesce(@BRHID,ADetail.BrchID) and  Adetail.iaccno=coalesce(@IACNO,Adetail.iaccno)

GROUP BY ADetail.IAccno,ADetail.pid, ADetail.Accno, ADetail.BrchID, ADetail.Aname, ADetail.Bal, ALoan.IonPA, ALoan.IonPR, ALoan.PonPrA, ALoan.PonPrR, ALoan.PonIA, 

ALoan.PonIR, ALoan.IonIR, ALoan.IonIA, ALoan.OthrBal, ALoan.IonCA, ALoan.IonCR, ProductDetail.PID, ProductDetail.PName,ADetail.IRate





select pid as Pid,

lOANTYPE as ProductName,

iaccno as IAccno,

LoanNo as AccountNumber,

brhid as BranchId,

Loanee as AccountName,

isnull(OUTBAL,0) as OutStandingBalance,

isnull(Matpri,0) as MaturePrincipal,

isnull(IAcc,0) as InterestAccured,

isnull(Pen,0) as Penalty,

isnull(OTH,0) as Other,

isnull(OTHINT,0) as OtherInterest,

RATE as Rate,

NODAYS as NoDays,

isnull(FFINT,0) as FutureInterest,

isnull(TOTDUE,0) as TotalDue



 from #Temploanfollowup Order By LoanNo



drop table #Temploanfollowup 

end
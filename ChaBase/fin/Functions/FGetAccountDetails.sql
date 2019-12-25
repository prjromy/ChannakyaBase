-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

CREATE FUNCTION  [fin].[FGetAccountDetails]( @AccountId int)

RETURNS TABLE 
AS
RETURN 
(
	-- Add the SELECT statement with parameter references here
	SELECT acc.IAccno,fc.CTId,acc.BrchID,acc.RDate,pd.Apfix as Prefix,pd.PID,acc.Aname as AccountTitle,acc.Bal as Balance,acc.LastTransDate as LastTransactionDate,
	Astatus.AccountState,pd.PName as ProductName,sd.SDName as SchemeName,pd.IntraBrnhTrnx as AllowABBS,ISNULL(ad.IsOD, 0) AS IsOD,rcd.ICBDurNam as IntCalSchm,
	acc.IRate,pd.Nomianable,ISNULL(amb.Minbal,0) as BalToMan ,pic.PSName as IntCalschmDr,lb.BranchName as BranchName,sd.SType as Subsidiary,acc.Accno as AccounNumber
		FROM fin.ADetail acc
			INNER JOIN fin.CurrencyType fc ON acc.CurrID = fc.CTId
			INNER join lg.Company lb on lb.CompanyId=acc.BrchID
			INNER JOIN fin.ProductDetail pd ON acc.PID = pd.PID
			INNER JOIN fin.SchmDetails sd ON pd.SDID = sd.SDID
			INNER JOIN fin.AccountStatus Astatus on Astatus.AsId=acc.AccState
			LEFT JOIN fin.APolicyInterest api
			INNER JOIN fin.PolicyIntCalc pic ON pic.PSID = api.PSID ON api.IAccno = acc.IAccno 
			LEFT OUTER JOIN fin.AMinBal amb ON amb.IAccno = acc.IAccno 
			LEFT OUTER JOIN fin.RuleICBDuration rcd
			INNER JOIN fin.AICBDur aicdur ON rcd.ICBDurID = aicdur.ICBDurID ON acc.IAccno = aicdur.IAccno
			LEFT JOIN fin.ADur ad ON acc.IAccno = ad.IAccno
	 where acc.IAccno=@AccountId
)
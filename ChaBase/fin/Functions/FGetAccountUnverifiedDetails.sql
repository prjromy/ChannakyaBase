 CREATE FUNCTION [fin].[FGetAccountUnverifiedDetails] (
	@fromDate SMALLDATETIME
	,@toDate SMALLDATETIME
	)
RETURNS TABLE ---MARKED ENCRYPTION  as  

RETURN (
		--Account opening detail Report
		SELECT ad.PID
			,ad.IAccno AS AccountId
			,ad.Accno AS AccountNumber
			,ad.Aname AS AccountName
			,PD.PName AS ProductName
			,AD.RDate AS RegisterDate
		
			,ad.BrchID AS BranchId
		   ,ast.AsId AS AccountStatus
		  
		
			,ast.AccountState  
			,sd.SType AS AccountType
			,ug.UserName AS AccountOpenBy
			,CASE 
				WHEN (
						SELECT COUNT(iaccno)
						FROM fin.AOfCust
						WHERE iaccno = ad.iaccno
						) = 1
					THEN 'Single'
				ELSE 'Joint'
				END AS AccountOperating
			,ug.UserName AS AccountClosedBy
			,ct.CurrencySymbol
		FROM fin.ADetail ad
		INNER JOIN fin.ProductDetail pd ON ad.PID = pd.PID
		INNER JOIN fin.SchmDetails sd ON sd.SDID = pd.SDID
		INNER JOIN fin.CurrencyType ct ON ct.CTId = ad.CurrID
	
		LEFT JOIN fin.AccountStatus ast ON ast.AsId = ad.AccState
		LEFT JOIN fin.AccountStatus ast1 ON ast1.AsId = ad.AccState
		LEFT JOIN lg.[User] ug ON ug.UserId = ad.PostedBy
		WHERE ad.RDate BETWEEN @fromDate
				AND @toDate
		)
CREATE FUNCTION [fin].[FGetReportAccountOpeningDetails] (@fromDate SMALLDATETIME,@toDate SMALLDATETIME)
RETURNS TABLE ---MARKED ENCRYPTION  as  

RETURN (


		----Account opening detail Report
		--SELECT 
		--ad.PID 
		--	,ad.IAccno AS AccountId
		--	,ad.Accno AS AccountNumber
		--	,ad.Aname AS AccountName
		--	,PD.PName AS ProductName
		--	,AD.RDate AS RegisterDate
		--	,st.TDate AS ChangeOn
		--	,ad.BrchID AS BranchId
		--	,st.AccState  AS AccountStatus
		--	,ast.AccountState

		--	,sd.SType AS AccountType
		--	,ug.UserName AS AccountOpenBy
		--	,CASE 
		--		WHEN (
		--				SELECT COUNT(iaccno)
		--				FROM fin.AOfCust
		--				WHERE iaccno = ad.iaccno
		--				) = 1
		--			THEN 'Single'
		--		ELSE 'Joint'
		--		END AS AccountOperating
		--	,ug.UserName AS AccountClosedBy
		--	,ct.CurrencySymbol
		--FROM fin.ADetail ad
		--INNER JOIN fin.ProductDetail pd ON ad.PID = pd.PID
		--INNER JOIN fin.SchmDetails sd ON sd.SDID = pd.SDID
		--INNER JOIN fin.CurrencyType ct ON ct.CTId = ad.CurrID
		--left outer join fin.StatusChangeLog st ON ad.IAccno = st.IAccNo
		--LEFT JOIN fin.AccountStatus ast ON ast.AsId = st.AccState
		--LEFT JOIN fin.AccountStatus ast1 ON ast1.AsId = ad.AccState
		--LEFT JOIN lg.[User] ug ON ug.UserId = st.UserID
		--WHERE st.tdate BETWEEN @fromDate AND @toDate 
	
		
		--declare @fromDate date ='3/8/2018'
		--declare @toDate date ='6/12/2019'
		
		SELECT 
		ad.PID 
			,ad.IAccno AS AccountId
			,ad.Accno AS AccountNumber
			,ad.Aname AS AccountName
			,PD.PName AS ProductName
			,AD.RDate AS RegisterDate
			,st.TDate AS ChangeOn
			,ad.BrchID AS BranchId
			,st.AccState  AS AccountStatus
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
		cross apply (select top 1 l.IAccNo,tdate,userid,l.AccState,ChangeOn from fin.StatusChangeLog l 
								--inner join 	fin.ADetail a on l.IAccNo = a.IAccno
								 where  ad.iaccno =l.IAccNo													
		 order by changeon desc) st --ON ad.IAccno = st.IAccNo
		LEFT JOIN fin.AccountStatus ast ON ast.AsId = st.AccState
		LEFT JOIN fin.AccountStatus ast1 ON ast1.AsId = ad.AccState
		LEFT JOIN lg.[User] ug ON ug.UserId = st.UserID
		WHERE st.tdate BETWEEN @fromDate AND @toDate 	
		)
CREATE function [fin].[FgetReportDurationWise](@fromDate SMALLDATETIME

	,@toDate SMALLDATETIME
	,@stype tinyint
	)

	RETURNS TABLE ---MARKED ENCRYPTION  as  



RETURN (

SELECT 

    ad.PID,

    ad.IAccno as AccountId,

	ad.Accno AS AccountNumber,

	ad.Aname AS AccountName,

	PD.PName AS ProductName,

	AD.RDate AS RegisterDate,	

	ad.BrchID AS BranchId,	

	ad.Bal as Balance,

	adr.MatDat as MatureDate,
	SType
	 FROM fin.ADetail ad INNER JOIN fin.ProductDetail pd ON ad.PID = pd.PID 

	INNER JOIN fin.SchmDetails sd ON sd.SDID = pd.SDID 	

	INNER JOIN fin.AccountStatus asts ON asts.AsId = ad.AccState 

	join fin.ADur adr on adr.IAccno=ad.IAccno

	where ad.AccState!=6 and ad.AccState!=3 and adr.MatDat between convert(date, @fromDate, 105) and convert(date, @toDate, 105) and SType=@stype

	)
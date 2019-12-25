CREATE function [fin].[FGetReportDepostiStatementWithAccDetails]( @iaccno int)
returns table
as return(
		SELECT fin.ADetail.IAccno AS Iaccno, fin.ADetail.Accno, fin.ADetail.OldAccno, fin.ADetail.Aname,
		fin.ProductDetail.PName AS 'ProductName',   fin.SchmDetails.SDName AS 'SchemeName',fin.ARate.IRate AS 'InterestRate', fin.ADetail.RDate AS 'RegisteredDate', 
		fin.ADur.MatDat AS 'MaturedDate', fin.ADetail.IonBal, fin.ADetail.Bal AS 'ClosingBalance',cast(isnull((intdet.IntToExp+IntToCap),0)as decimal) as AccuredInterest FROM  fin.ADetail 
		INNER JOIN fin.ProductDetail ON fin.ADetail.PID = fin.ProductDetail.PID 
		INNER JOIN  fin.SchmDetails ON fin.ProductDetail.SDID = fin.SchmDetails.SDID
		LEFT OUTER JOIN fin.ARate ON fin.ADetail.IAccno = fin.ARate.IAccno
		LEFT OUTER JOIN fin.AICBDur ON fin.ADetail.IAccno = fin.AICBDur.IAccno 
		inner join fin.FGetReportAccountInterestDetails() intdet on intdet.IAccno=ADetail.IAccno
		LEFT OUTER JOIN fin.ADur ON fin.ADetail.IAccno = fin.ADur.IAccno  WHERE(fin.ADetail.IAccno = @iaccno)
)
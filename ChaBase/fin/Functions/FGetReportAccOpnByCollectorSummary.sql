CREATE Function  [fin].[FGetReportAccOpnByCollectorSummary]
(
@CollectorId int,
@FromDate smalldatetime,
@ToDate smalldatetime,
@Stauts int=null,
@PName nvarchar(50)
)
returns table 
as return
(


SELECT    ad.Accno, ad.Aname as AccountName,ad.RDate, ad.Bal as Balance,EmployeeName,PName, 
                      fin.SchmDetails.SDName
FROM         fin.FGetAllUsersWithDesignation() fn inner join
                      fin.ReferenceInfo ri on fn.UserId=ri.BroughtBy
					   INNER JOIN
                      fin.ADetail ad ON ri.IAccNo = ad.IAccno 
					  INNER JOIN
                      fin.ProductDetail ON ad.PID = fin.ProductDetail.PID
					   INNER JOIN
                      fin.SchmDetails ON fin.ProductDetail.SDID = fin.SchmDetails.SDID
WHERE     (ri.RType = 2) 
AND (ad.RDate BETWEEN @FromDate AND @ToDate)
And ad.AccState =COALESCE(@Stauts,ad.AccState)
AND ri.BroughtBy=@CollectorId 
AND PName=COALESCE(@PName,PName)
)
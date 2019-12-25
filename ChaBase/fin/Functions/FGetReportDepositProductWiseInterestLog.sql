CREATE Function [fin].[FGetReportDepositProductWiseInterestLog](@fromDate smalldatetime,@toDate smalldatetime, @branchId int)
returns table as return
SELECT     ProductDetail.PID, ProductDetail.PName, SUM(IntLog.Intcal) AS DInt
FROM         fin.IntLog INNER JOIN
                      fin.ADetail ON IntLog.IAccno = ADetail.IAccno INNER JOIN
                      fin.ProductDetail ON ADetail.PID = ProductDetail.PID INNER JOIN
                      fin.SchmDetails ON ProductDetail.SDID = SchmDetails.SDID
WHERE     (SchmDetails.SType = 0) AND (IntLog.Tdate BETWEEN  @fromDate  AND  @toDate ) and  (IntLog.Valued in(0,5)) AND  (ADetail.BrchID = @branchId)
GROUP BY ProductDetail.PID, ProductDetail.PName
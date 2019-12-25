CREATE function [fin].[FgetReportProductSummaryDetails](
 @fdate datetime,
 @tdate datetime,
 @branchId int=null
 )
returns table as return(

select PName ProductName, sum (dramt) as Dramt, sum(cramt) Cramt from (

SELECT     SUM(Trans.AMTrns.dramt) AS dramt, SUM(Trans.AMTrns.cramt) AS cramt, fin.ProductDetail.PID, fin.ProductDetail.PName
FROM         Trans.AMTrns INNER JOIN
                      fin.ADetail ON Trans.AMTrns.IAccno = fin.ADetail.IAccno INNER JOIN
                      fin.ProductDetail ON fin.ADetail.PID = fin.ProductDetail.PID

where vdate>=@fdate and vdate<=@tdate and (adetail.brchid=@branchId OR COALESCE(@branchId, 0) = 0)
GROUP BY   fin.ProductDetail.PID, fin.ProductDetail.PName 
)x group by x.PName
)
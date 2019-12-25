create function [fin].[FgetReportBalancePayable](
             
				@BranchId INT=null)
returns table as return
(
SELECT   
ProductDetail.PID,
fin.GetAcNo(fin.AintPayable.Iaccno) AS Accno, 

sum(isnull(cramt,0))-sum(isnull(dramt,0)) Balance,

fin.ProductDetail.PName,
fin.SchmDetails.SDName, 

fin.AintPayable.Iaccno             
FROM   fin.AintPayable INNER JOIN
                      fin.ADetail ON fin.AintPayable.Iaccno = fin.ADetail.IAccno INNER JOIN
                      fin.ProductDetail ON fin.ADetail.PID = fin.ProductDetail.PID INNER JOIN
                      fin.SchmDetails ON fin.ProductDetail.SDID = fin.SchmDetails.SDID
					  where BrchID  =COALESCE(@BranchId,adetail.brchid)
					  Group By AintPayable.Iaccno,ProductDetail.PName,SchmDetails.SDName,ProductDetail.PID
having sum(isnull(cramt,0))-sum(isnull(dramt,0))<>0)
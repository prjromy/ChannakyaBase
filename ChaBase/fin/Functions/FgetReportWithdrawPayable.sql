CREATE function [fin].[FgetReportWithdrawPayable](
               @FDate SMALLDATETIME,
				@TDate SMALLDATETIME,
				@BranchId INT=null)
returns table as return
(

SELECT   
ProductDetail.PID,
fin.GetAcNo(fin.AintPayable.Iaccno) AS Accno, 
fin.AintPayable.IntAmt,
fin.AintPayable.Tax,
fin.AintPayable.DrAmt as Amount, 
fin.AintPayable.TaxRt, 
fin.AintPayable.Tdate,
fin.ProductDetail.PName,
fin.SchmDetails.SDName, 

fin.AintPayable.Iaccno             
FROM   fin.AintPayable INNER JOIN
                      fin.ADetail ON fin.AintPayable.Iaccno = fin.ADetail.IAccno INNER JOIN
                      fin.ProductDetail ON fin.ADetail.PID = fin.ProductDetail.PID INNER JOIN
                      fin.SchmDetails ON fin.ProductDetail.SDID = fin.SchmDetails.SDID
WHERE (fin.AintPayable.DrAmt > 0) and adetail.brchid =COALESCE(@BranchId,adetail.brchid) and TDate Between @FDate And @TDate



 
 )
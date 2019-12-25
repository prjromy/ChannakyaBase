CREATE function [fin].[FgetReportLoanTransaction](@fdate datetime,@tdate datetime,@branchId int=null)
returns table as return
(


SELECT     ALoanTra.tno, ALoanTra.vdate, isnull(ALoanTra.PriDr,0) as PriDr , isnull(ALoanTra.PriCr,0) as PriCr,
                     isnull(ALoanTra.intAPd,0) + isnull(ALoanTra.intRPd,0) as 'Interest',isnull(ALoanTra.PonPrAPd,0)+ isnull(ALoanTra.PonPrRPd,0) as 'POnPr', 
isnull(ALoanTra.PonIAPd,0)+ isnull(ALoanTra.PonIRPd,0) as 'POnInt', isnull(ALoanTra.IonIAPd,0)+ isnull(ALoanTra.IonIRPd,0) as 'IOnI', 
                       ALoanTra.ttype, ProductDetail.PName as ProductName, ProductDetail.PID, 
                      ADetail.Accno, ADetail.Aname AS AccountName
FROM         fin.ALoanTra INNER JOIN
                      fin.ADetail ON ALoanTra.iaccno = ADetail.IAccno INNER JOIN
                      fin.ProductDetail ON ADetail.PID = ProductDetail.PID INNER JOIN
                      fin.SchmDetails ON ProductDetail.SDID = SchmDetails.SDID where ADetail.BrchId=COALESCE(@BranchId,adetail.brchid) And ALoanTra.vdate between @fdate and  @tdate

)
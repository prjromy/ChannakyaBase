
CREATE Function   [fin].[FgetAccountOpenByCollectorDetails]()
returns table 
as return
(
select * from fin.ADetail
--If 0 = 1
--SELECT     dbo.ADetail.Accno, dbo.ADetail.Aname, dbo.ADetail.RDate, dbo.ADetail.Bal, dbo.CustStaffInfo.StaffID, dbo.ProductDetail.PName, 
--                      dbo.SchmDetails.SDName
--FROM         dbo.CustStaffInfo INNER JOIN
--                      dbo.ReferenceInfo INNER JOIN
--                      dbo.ADetail ON dbo.ReferenceInfo.IAccNo = dbo.ADetail.IAccno ON dbo.CustStaffInfo.StaffID = dbo.ReferenceInfo.BroughtBy INNER JOIN
--                      dbo.ProductDetail ON dbo.ADetail.PID = dbo.ProductDetail.PID INNER JOIN
--                      dbo.SchmDetails ON dbo.ProductDetail.SDID = dbo.SchmDetails.SDID
--WHERE     (dbo.ReferenceInfo.RType = 2) AND (dbo.ADetail.RDate BETWEEN {d '2011-04-05'} AND {d '2017-04-08'})
--Else
--SELECT     dbo.ADetail.Accno, dbo.ADetail.Aname, dbo.ADetail.RDate, dbo.ADetail.Bal, dbo.CustStaffInfo.StaffID, dbo.ProductDetail.PName, 
--                      dbo.SchmDetails.SDName
--FROM         dbo.CustStaffInfo INNER JOIN
--                      dbo.ReferenceInfo INNER JOIN
--                      dbo.ADetail ON dbo.ReferenceInfo.IAccNo = dbo.ADetail.IAccno ON dbo.CustStaffInfo.StaffID = dbo.ReferenceInfo.BroughtBy INNER JOIN
--                      dbo.ProductDetail ON dbo.ADetail.PID = dbo.ProductDetail.PID INNER JOIN
--                      dbo.SchmDetails ON dbo.ProductDetail.SDID = dbo.SchmDetails.SDID
--WHERE     (dbo.ReferenceInfo.RType = 2) AND (dbo.ADetail.RDate BETWEEN {d '2011-04-05'} AND {d '2017-04-08'})
--And ADetail.AccState = 0
)
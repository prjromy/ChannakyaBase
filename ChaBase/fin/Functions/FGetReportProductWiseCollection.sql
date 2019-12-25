CREATE function [fin].[FGetReportProductWiseCollection](@fDate datetime,@todate DateTime)

returns table as 

return

(

select  EM.EmployeeName,sum(CT.Amount) as [TotalAmount],ad.PID,PName,CollectorId as EmployeeId

 FROM FIN.collsheettrans CT 

 inner join fin.CollSheet CM on CM.CollSheetId = CT.CollSheetId

 left join [fin].[FGetAllUsersWithDesignation]() EM on EM.UserId=CM.CollectorId

 inner join fin.adetail ad on ad.iaccno=ct.iaccno

 inner join fin.ProductDetail pd on pd.pid=ad.PID

 where cm.TDate between @fDate and TDate

 group by CM.CollSheetId, cm.TDate, cm.SheetNo, EM.EmployeeName,ad.PId,PName,CollectorId

 )
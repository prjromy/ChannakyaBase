 CREATE function [fin].[FGetReportAccountWiseCollectionSummary](@FDate Datetime,@TDate datetime,@Iaccno int,@branchId int,@collectorId int)

 returns table

 as 

 return(

 select  ad.iaccno,ad.Accno as Accountnumber,CM.TDate,employeename as Collector, 

 CT.Amount as [TotalAmount],note

 FROM FIN.collsheettrans CT 

 inner join fin.CollSheet CM on CM.CollSheetId = CT.CollSheetId

 left  join [fin].[FGetAllUsersWithDesignation]() EM on EM.UserId=CM.CollectorId

 inner join fin.ADetail ad on ad.IAccno=ct.IAccNo

 inner join fin.ProductDetail pd on pd.pid=ad.PID 

 where CM.ApprovedBy is not null

 and CollectorId=@collectorId

 and CM.TDate between @FDate and @TDate and cm.BrchId=@branchId

 and ct.IAccNo=@Iaccno

 )
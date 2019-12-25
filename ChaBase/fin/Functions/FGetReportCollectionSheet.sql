CREATE function [fin].[FGetReportCollectionSheet](@FDate datetime,@TDate datetime,@branchId int)

returns table as 

return

(

 select CM.collSheetId,CM.TDate, CM.SheetNo,CM.CollectorId,EM.UserId, EM.EmployeeName,sum(CT.Amount) as [TotalAmount], 

 [LoanAmount] = (SELECT SUM(AMOUNT) FROM fin.collsheettrans T WHERE T.CollSheetId = CM.CollSheetId AND SType=1),

 [DepositAmount] = (SELECT SUM(AMOUNT) FROM fin.collsheettrans T WHERE T.CollSheetId = CM.CollSheetId AND SType=0),

 [LoanCount] = (SELECT count(*) FROM fin.collsheettrans T WHERE T.CollSheetId = CM.CollSheetId AND SType=1),

 [DepositCount] = (SELECT count(*) FROM fin.collsheettrans T WHERE T.CollSheetId = CM.CollSheetId AND SType=0)  

 

 FROM FIN.collsheettrans CT 

 inner join fin.CollSheet CM on CM.CollSheetId = CT.CollSheetId

 left join [fin].[FGetAllUsersWithDesignation]() EM on EM.UserId=CM.CollectorId

 where ApprovedBy is not null and CM.TDate between @FDate and @TDate and cm.BrchId=@branchId

 group by CM.CollSheetId, cm.TDate, cm.SheetNo, EM.EmployeeName ,CM.CollectorId,EM.UserId

 

 )
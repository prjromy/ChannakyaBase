CREATE function [fin].[FGetReportAccountWiseCollection](@FDate datetime,@TDate datetime,@Branchid int,@collectorId int)

returns table 

as return

(

 select  ad.iaccno,ad.Accno as Accountnumber, ad.Aname,sum(CT.Amount) as [TotalAmount],

 PName =(Pname +CASE WHEN ct.SType=1 then '(Loan)' else '(Deposit)'end)

 

 FROM FIN.collsheettrans CT 

 inner join fin.CollSheet CM on CM.CollSheetId = CT.CollSheetId

  

 inner join fin.ADetail ad on ad.IAccno=ct.IAccNo

 inner join fin.ProductDetail pd on pd.pid=ad.PID 

 where CM.ApprovedBy is not null 

 and CM.TDate between @FDate and @TDate and cm.BrchId=@branchId

 and CollectorId=@collectorId

 group by  ad.Aname,ad.IAccno,SType,PName,Accno

 )
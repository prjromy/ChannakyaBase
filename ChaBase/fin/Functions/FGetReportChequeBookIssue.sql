CREATE function [fin].[FGetReportChequeBookIssue](@fromdate smalldatetime,@toDate smalldatetime,@branchId int=null,@iaccno int=null)
returns table as return
(
SELECT 
fin.ADetail.iaccno as IAccno , 
achq.tdate,
accno as AccountNumber, 
Aname as AccountName, 
cfrom,
cto,
cstate,
cb.Name as ChequeStatus,
'ChequeIssue' as ShowWith
FROM fin.ADetail INNER JOIN 
fin.AChq ON fin.ADetail.IAccno = fin.AChq.IAccno 
join [Mast].[ChequeBookStatus] cb on cb.id=AChq.cstate
where tdate between @fromdate and @toDate and brchid =  COALESCE(@branchId,brchid) and ADetail.IAccno=coalesce(@iaccno,ADetail.iaccno)
 )
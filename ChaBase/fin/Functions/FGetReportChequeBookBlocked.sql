CREATE function [fin].[FGetReportChequeBookBlocked](@fromdate smalldatetime,@toDate smalldatetime,@branchId int=null,@iaccno int=null)
returns table as return
(
select
 ad.Accno as AccountNumber,
 ad.Aname as AccountName,
 ah.ChkNo as ChequeNo,
 ahh.Tdate,
'ChequeBlocked' as ShowWith
 from fin.AChq ac
join fin.ADetail ad on ad.IAccno=ac.IAccno
join fin.AchqH ah on ah.Rno=ac.rno
join fin.AchqHH ahh on ahh.AchqHId=ah.AchqHId where ahh.Cstate  in (2,4) and  ahh.tdate  between @fromdate and @toDate and brchid =  COALESCE(@branchId,brchid) and ad.IAccno=coalesce(@iaccno,ad.iaccno)
)
CREATE function [fin].[FGetReportChequeBounce](@fromdate smalldatetime,@toDate smalldatetime,@branchId int=null,@iaccno int=null)

returns table as return

(

select 

ad.accno as AccountNumber,

ad.aname as  AccountName,

cast(ic.chkno as int) as  ChequeNo,

ic.tdate

,ic.rmks as 'Remarks',

'Chequebounce' as ShowWith

from fin.IchkBounce ic

 inner join  fin.adetail ad on ad.iaccno = ic.iaccno 

 where  ic.tdate between @fromdate and @toDate and brchid =  COALESCE(@branchId,brchid) and ad.IAccno=coalesce(@iaccno,ad.iaccno))
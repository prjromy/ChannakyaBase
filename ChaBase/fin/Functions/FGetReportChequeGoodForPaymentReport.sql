CREATE function [fin].[FGetReportChequeGoodForPaymentReport](@fromDate smalldatetime,@toDate smalldatetime,@branchId int=null,@ttype int=null)
returns table return
(

select ad.Accno as AccountNumber,Aname as AccountName,tno as Tno,Chqno as ChequeNumber,notes as Remarks ,amount as Amount,tstate as TType,payee as Payee,tdate as TransDate from fin.AchqFGdPymt af
join fin.ADetail ad 
on ad.IAccno =af.IAccno where tdate between @fromDate and @toDate and ad.BrchID=coalesce(@branchId,ad.BrchID) and tstate=coalesce(@ttype,tstate))
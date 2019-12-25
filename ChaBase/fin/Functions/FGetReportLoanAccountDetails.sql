CREATE function [fin].[FGetReportLoanAccountDetails](@iaccno int)

returns

table as

return(

select distinct ad.IAccno,ad.Accno,ad.OldAccno,SDName,PName,Ad.Aname,AccountState,case when al.Revolving=1 then 1 else 0 end 

Revolving,ad.Rdate,adur.matdat,adur.valdat,case when adur.durtype=1 then concat([Month],' Month(s)') else concat([days]*0.365, ' Month(s)') end as duration, 

al.PFreq, al.IFreq,rp.PRule, rpsh.schType,adr.QuotAmt,adr.AppAmt,ad.IRate,al.cDay as CDay,isnull(Amount,0) as DisburseAmount

,cca.location,cca.contact,aoc.cid,

isnull(al.overPay,0) as overPay

from fin.ADetail ad  left JOIN

 lg.Company ON ad.BrchID = Company.CompanyId 

left join fin.ProductDetail pd on pd.PID=ad.PID

left join  fin.SchmDetails sd on sd.SDID=pd.SDID

inner join fin.accountstatus acst on  acst.AsId=ad.AccState

left join fin.ALoan al on al.Iaccno=ad.IAccno

inner join fin.adur adur on adur.iaccno=ad.iaccno

left JOIN fin.ADrLimit adr ON	 adr.IAccno=ad.IAccno 

left join fin.RulePay rp ON al.PayID=rp.Payid

left  join fin.RulePaySch rpsh ON al.PaySID=rpsh.PaySID 

left join  fin.APolicyInterest apcy ON ad.IAccno = apcy.IAccno 

inner join fin.aofcust aoc on aoc.IAccno=ad.IAccno

inner join cust.FGetCustList() cca on cca.cid=aoc.cid

left join fin.fgetloandisbursedetails() fn on fn.iaccno=ad.iaccno

WHERE     (sd.SType = 1) and ad.iaccno=@iaccno

AND aoc.Sno IN (1,2)
)
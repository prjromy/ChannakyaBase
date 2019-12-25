CREATE function [fin].[FGetLoanShareDisbursement](@Iaccno int)
returns table 
as return
(
select al.IAccno,c.Name,isnull(sum(sqty),0) SQty,isnull(sum(amt),0) as ShareBalance,cast(s.RegNo as int) as RegNo from Cust.FGetCustName() c 
inner join fin.ShrReg s on s.CId =c.cid
inner join fin.AOfCust ac on ac.cid= c.cid 
inner join fin.ALoan al on al.iaccno = ac.iaccno 
inner join  fin.[ShrPurchase] sp on sp.regno=s.RegNo where al.iaccno=@iaccno group by sp.regno,sqty,al.IAccno,c.Name,s.RegNo )
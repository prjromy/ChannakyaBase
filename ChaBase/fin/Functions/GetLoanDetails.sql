CREATE FUNCTION [fin].[GetLoanDetails](@IAccno int)
RETURNS TABLE AS
RETURN
(
 select adetail.Accno,adetail.Aname,alregistration.PAYSID,alregistration.SAmt,alregistration.Remarks,
 adur.MatDat,ALinkloan.ILinkAc,ALinkloan.priority ,arate.IRateId,arate.IRate ,aprate.PIRate,aprate.PPRate,
 aloan.penGDys,aloan.PAYID,aloan.IFreq,aloan.PFreq,aloangrace.* 
 from fin.ADetail as adetail  
join fin.ALinkloan as ALinkloan
 on adetail.IAccno=ALinkloan.IAccno
 join fin.ADur as adur
 on adetail.IAccno=adur.IAccno
 join fin.ADrLimit as adrlimit
 on adetail.IAccno=adrlimit.IAccno
 join fin.ALoan as aloan
 on adetail.IAccno=aloan.IAccno
 join fin.APRate as aprate
 on adetail.IAccno =aprate.IAccno
 join fin.ALRegistration as alregistration
 on adetail.IAccno=alregistration.iAccno
join fin.APolPen as apolpen
on adetail.IAccno=apolpen.IAccno
join fin.APolicyInterest as apolicyinterest
on adetail.IAccno=apolicyinterest.IAccno
join fin.ARate as arate
on adetail.IAccno=arate.IAccno
full join fin.ALoanGrace as aloangrace
 on adetail.IAccno=aloangrace.IAccno
 join fin.ProductDetail as productdetail
 on adetail.PID=productdetail.PID where adetail.IAccno=@IAccno

 )
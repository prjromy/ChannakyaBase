CREATE function [fin].[FGetLoanAccountDetailsOnlyByIaccno](@Iaccno Int)

returns table

as return

(

select ad.IAccno,convert(decimal,isnull(adu.Days,0)) Days,
adu.Month,adu.DurType,
adr.AppAmt as SancationAmount,
ad.Revolving,ad.Ifreq as InterestFrequency,
ad.PFreq as PrincipalFrequency,
adu.MatDat as MaturityDate, 
schType as PayScheme,
rpy.PRule as PayRule,
isnull(PrincipleLoanOut,0) as PrincipleLoanOut  from fin.ALoan ad

inner join fin.RulePaySch rp on ad.PAYSID=rp.PAYSID

inner join fin.ADur adu on ad.IAccno=adu.IAccno

inner join fin.RulePay rpy on rpy.PAYID=ad.Payid

inner join fin.ADrLimit adr on adr.IAccno=ad.IAccno

where ad.iaccno=@Iaccno)
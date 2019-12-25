CREATE function [fin].[GetLoanScheduleParameter](@iaccno int)



returns table



as return



(







	select cast(PAYID as tinyint) as PaymentMode,cast(PAYSID as tinyint) ScheduleType,



	isnull(ag.GDateP,al.PostedOn) as PrincipalDate,isnull(ag.GDateI,al.PostedOn) as InterestDate,cast(PFreq as int) as PrincipalFrequency,



	cast(IFreq as int) as InterestFrequency ,isnull(cast(cDay as int),0) as [Day], cast(ar.IRate as decimal) as Rate,



	Flat= case when apol.PSID=5 then cast(1 as bit) else  cast(0 as bit) end,



	cast(isnull(ag.GDurI,0) as int) as Interest,cast(isnull(ag.GDurP,0) as int) as Principal,al.PostedOn as ValueDate,



	Duration=case when ad.DurType=1 then cast([month] as decimal) else cast([Days] as decimal) end,



	isnull(cast(ag.GraceOption as  tinyint),0) as GraceOption,

	isnull(cast(Revolving as  bit),0)as Revolving



	from fin.ALoan al left join fin.ALoanGrace ag on al.IAccno=ag.IAccno inner join fin.ARate ar on ar.IAccno=al.IAccno inner join fin.APolicyInterest apol on al.IAccno=apol.IAccno



	inner join fin.adur ad on ad.IAccno=al.IAccno



    where al.IAccno=@iaccno



)
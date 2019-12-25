CREATE Function [fin].[FGetLoanDisplay]
(
	@IAccNo Int
)
returns table
as return


Select Revolving,Accno,(adr.AppAmt-ad.Bal) as Amount From fin.ALinkLoan AL Inner Join 
  fin.ADetail AD On AL.IAccNo = AD.IAccNo 
Inner Join fin.ALoan ALn On AD.IAccNo = ALn.IAccNo 
join fin.ADrLimit adr on adr.IAccno=al.IAccno
 Where AD.AccState not in(3,6 )And ILinkAc = 1 And ALn.Revolving = 1
Create function [fin].[FGetLoanDisburseDetails]()
returns table
as return 
			(
			select m.DisburseId,m.IAccno,m.Mode,p.Amount,fin.IsOtherLoan(FID) as IsOtherLoan,FID,Accno+'('+Aname+')' as AccountNumber,AddtionalCharge,Charges,p.Remarks,m.ApprovedOn,m.PostedOn,m.PostedBy,m.ApprovedBy,m.VNo from fin.DisbursementMain m inner join (
			select dm.DisburseId,Amount from fin.DisbursementMain dm inner join fin.DisburseCash dc1 on dm.DisburseId=dc1.DisburseId --where ApprovedBy is null
			union							
			select dm.DisburseId,Amount from fin.DisbursementMain dm inner join fin.DisburseCheque dchk on dm.DisburseId=dchk.DisburseId -- where ApprovedBy is null
			union	
			select dd.DisburseId,SUM(Amount) as Amount from fin.DisbursementMain dm inner join fin.DisburseDeposit dd on dm.DisburseId=dd.DisburseId group by dd.DisburseId) as depo on depo.disburseId=m.disburseId
			inner join fin.DisburseVsPID p on p.DisburseId=m.DisburseId 
			inner join fin.ADetail ad on m.IAccno =ad.IAccno 
			left join(select sum(amount) as AddtionalCharge,DisburseId from fin.DisburseCharge group by DisburseId) dchg on dchg.DisburseId=m.DisburseId
			left join (select Sum(Amt) Charges,Iaccno from Trans.ChgSTrnx where Vno  is null group by iaccno) chg on m.IAccno=chg.Iaccno
			)
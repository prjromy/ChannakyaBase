CREATE function [fin].[FGetReportLoanOutStanding]
(@FDATE SMALLDATETIME,
 @TDATE SMALLDATETIME,
 @BRANCHID INT)
 returns table as return
 select ad.Accno,ad.Aname,pd.PName,adu.ValDat,adu.MatDat,isnull(al.Revolving,0) as Revolving  ,isnull((select Sum(Amount) from fin.FGetLoanDisburseDetails() where IAccno=ad.iaccno ),0)as DisbursedAmount,
isnull((select Sum(isnull(Amount,0)) from fin.FGetLoanDisburseDetails() where IAccno=ad.iaccno ),0)-
isnull((select sum(isnull(amt,0))from fin.FGetLoanPaymentDetails() where IAccno=ad.IAccno),0) as OutstandingAmount,
 (adr.AppAmt- ISNULL(al.PrincipleLoanOut,0)) as RemainingToDisbursed,
 adr.AppAmt,
 ad.IRate,
 ad.AccState
 from fin.ADetail ad 
 join fin.ALoan al on ad.IAccno=al.IAccno
  join fin.ADrLimit adr on adr.IAccno=al.IAccno 
  join fin.ADur adu on adu.iaccno=ad.iaccno
  join fin.ProductDetail pd on pd.pid=ad.pid
  where adu.MatDat between @FDATE  and @TDATE and ad.BrchId=coalesce(@BRANCHID,ad.BrchId)
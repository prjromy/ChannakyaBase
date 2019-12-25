
	Create function [acc].[GetLedgerReport](@FId int,@FDate datetime,@TDate datetime)
	returns table as
	return(
	select B.V1id, a.V2Id, a.fid, b.TDate, b.PostedOn,d.Prefix+'-'+e.Prefix+'-'+convert(varchar,b.vno)
   as VNo, a.Particulars, b.ApprovedBy,
   a.DebitAmount, a.CreditAmount, 0 as Balance from acc.voucher2 a inner join acc.Voucher1 b on a.V1id = b.V1Id
   inner join acc.VoucherNo c on b.VNId=c.VNId
   inner join acc.VoucherType d on c.VTypeId=d.VTypeID
   inner join acc.BatchDescription e on c.Bid=e.BId

   where Fid= @FId and TDate between @FDate and @TDate)
CREATE function GetNormalTBData(@FId int,@BranchId int,@TillDate datetime)
returns   @TempTbl TABLE
(
FId int,DebitAmount decimal(18,2),CreditAmount decimal(18,2) , PrYrBalDB decimal(18,2),PrYrBalCR decimal(18,2)
) as 
begin
Declare @FYId int




Set @FYId = (select fyid from lg.FiscalYears where @TillDate between StartDt and EndDt) -- Compare date and take fyid
--select @FYId

--select * from (
	insert into @TempTbl
	select * from(
	select fid, sum(Debit) as DebitAmount, sum(credit) as CreditAmount, PrYrBalDB = Case when sum(PrYrBal)>0 then sum(pryrbal) end,
	PrYrBalCR = Case when sum(PrYrBal)<0 then sum(pryrbal)*-1  end
	  from 
	(
	Select Fid, sum(DebitAmount) Debit, sum(CreditAmount) Credit, 0 as PrYrBal from acc.Voucher1 v1 
	inner join acc.Voucher2 v2 on v1.V1Id = v2.V1id 
	inner join acc.VoucherNo vno on vno.VNId = v1.VNId	
	where convert(date,v1.TDate) <= @TillDate and vno.CompanyId = case when @BranchId=0 then vno.CompanyId else @BranchId end and FYID = @FYId
	group by fid 

	union all

	select fid, case when OPBal>0 then OPBal else 0 end  as Debit, 
				case when opbal<0 then opbal else 0 end as Credit, 0 as PrYrBal
		 from acc.VoucherBal where fyid  = @Fyid and BranchId = case when @BranchId=0 then BranchId else @BranchId end 

	
	union all

	select fid, 0 as Debit, 
				 0 as Credit, CLBal as PrYrBal
		 from acc.VoucherBal where fyid  = @Fyid-1 and BranchId = case when @BranchId=0 then BranchId else @BranchId end 


) x where Fid=@FId group by fid ) y where y.DebitAmount!=0 or y.CreditAmount!=0 or y.PrYrBalCR!=0 or y.PrYrBalDB!=0


return
end
--Declare @B
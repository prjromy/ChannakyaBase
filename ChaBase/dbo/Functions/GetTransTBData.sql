CREATE function [dbo].[GetTransTBData](@Fid int,@BranchId int,@FromDate datetime,@TillDate datetime)
returns table as 
return
(
--Declare @TillDate date= getdate()
--Declare @FromDate date=getdate()-300
--Declare @FId int=307

--Declare @FYId int

--Set @FYId = 15 -- Compare date and take fyid


Select Fid,vno.CompanyId as CompanyId, sum(DebitAmount) as DebitAmount, sum(CreditAmount) as CreditAmount from acc.Voucher1 v1 
	inner join acc.Voucher2 v2 on v1.V1Id = v2.V1id 
	inner join acc.VoucherNo vno on vno.VNId = v1.VNId	
	where convert(date,v1.TDate) between @FromDate and @TillDate and Fid=@FId and vno.CompanyId=case when @BranchId=0 then vno.CompanyId else @BranchId end
	group by fid,vno.CompanyId 

	)
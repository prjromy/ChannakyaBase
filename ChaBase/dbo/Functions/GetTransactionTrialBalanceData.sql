
CREATE function [dbo].[GetTransactionTrialBalanceData]
(
@rootId int
)
RETURNS  @TempTbl TABLE
(
FId int,Fname varchar(500),DebitAmount numeric(18,2) , CreditAmount numeric(18,2),CompanyId int
)
AS 
BEGIN 
declare @BranchId int=0
declare  @RootFName varchar(500), @ChildId int 
declare @Tmp table (FId int, PId int, FName varchar(500));

with name_tree as 
(
   select Fid, PID, Fname from acc.FinAcc  where Fid =@rootId
   union all
   select C.FId, C.PID, c.Fname from acc.FinAcc c
   join name_tree p on p.FId = C.PID
)


insert into @Tmp
select * from name_tree
--select * from @Tmp
--select @RootId = FId, @RootFName = FName from @Tmp 
insert into @TempTbl 
select @RootId as FId,@RootFName as FName, Sum(DebitAmount) DebitAmount, sum(CreditAmount) CreditAmount,CompanyId
from (
		select g.FId, g.PId, t.Fname as LedgerName, sum(t.DebitAmount) as DebitAmount , sum(t.CreditAmount) as CreditAmount,t.CompanyId 
		from ( select b.fid,f.Fname, b.DebitAmount, b.CreditAmount,j.CompanyId
				from acc.Voucher1 a 
				inner join acc.Voucher2 b on a.V1Id=b.V1id 
				inner join acc.FinAcc f on f.Fid=b.Fid
				left outer join acc.voucherno j on a.Vnid=j.vnid
				) t 
		left outer join acc.FinAcc g on g.Fid = t.Fid
		where g.Fid in (select Fid from @Tmp)
		group by g.Fid,t.Fname, g.PId, g.Fname,t.CompanyId
)x group by CompanyId
--Select Fid,'' as Fname,sum(DebitAmount) as DebitAmount, sum(CreditAmount) as CreditAmount,vno.CompanyId as CompanyId  from acc.Voucher1 v1 
--	inner join acc.Voucher2 v2 on v1.V1Id = v2.V1id 
--	inner join acc.VoucherNo vno on vno.VNId = v1.VNId	
--	where convert(date,v1.TDate) between getdate()-300 and getdate() and Fid=@rootId and vno.CompanyId=case when @BranchId=0 then vno.CompanyId else @BranchId end
--	group by fid,vno.CompanyId 
return
end
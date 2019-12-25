CREATE function [dbo].[GetNormalTrialBalanceDataTest](@rootId int,@FYId int,@TillDate datetime)
RETURNS  @TempTbl TABLE
(FId int,Fname varchar(500),DebitAmount numeric(18,2) , CreditAmount numeric(18,2),PrYrBalDB numeric(18,2),PrYrBalCR numeric(18,2),BranchId varchar(100))
AS 
BEGIN 
declare  @RootFName varchar(500), @ChildId int 
declare @Tmp table (FId int, PId int, FName varchar(500));
with name_tree as 
	(
	   select FId,Pid,Fname from acc.finacc  where Fid =@rootId
	   union all
	   select C.FId, C.PID, c.Fname from acc.FinAcc c
	   join name_tree p on p.FId = C.PID
	)

	insert into @Tmp
	select * from name_tree
	insert into @TempTbl 
	

	
	select @RootId as FId,'' as FName ,Sum(DebitAmount),Sum(CreditAmount),Sum(PrYrBalDB),Sum(PrYrBalCr), BranchId from(
	

	select @RootId as FId,'' as FName, Sum(ISNULL(DebitAmount,0)) DebitAmount, sum(ISNULL(CreditAmount,0)) as CreditAmount,sum(ISNULL(z.PrYrBalDB,0)) as PrYrBalDB,sum(ISNULL(z.PrYrBalCr,0)) as PrYrBalCr,BranchId from (



	select yy.FID,g.Fname,sum(ISNULL(DebitAmount,0)) as DebitAmount,sum(ISNULL(CreditAmount,0)) as CreditAmount,sum(ISNULL(PrYrBalDB,0)) as PrYrBalDB,sum(ISNULL(PrYrBalCR,0)) as PrYrBalCR,branchid  from (
	
	
	select fid as FID,Fname=(select top 1 Fname from acc.finacc where Fid=y.fid),sum(ISNULL(DebitAmount,0)) as DebitAmount,sum(ISNULL(CreditAmount,0)) as CreditAmount,sum(ISNULL(PrYrBalDB,0)) as PrYrBalDB,sum(ISNULL(PrYrBalCR,0)) as PrYrBalCR,Branchid from (
	

	select fid, sum(Debit) as DebitAmount, sum(credit) as CreditAmount, PrYrBalDB = Case when sum(PrYrBal)>0 then sum(pryrbal) end,
	PrYrBalCR = Case when sum(PrYrBal)<0 then sum(pryrbal)*-1  end,Branchid
	  from 
	(
	
	--declare @Tmp table (FId int, PId int, FName varchar(500));
	--declare @rootId int=1
	--declare @FYId int=15
	--declare @TillDate datetime='2018-04-07'
	----declare @BranchId varchar(100)='1,13'
	Select Fid, sum(DebitAmount) Debit, sum(CreditAmount) Credit, 0 as PrYrBal,CompanyId Branchid from acc.Voucher1 v1 
	inner join acc.Voucher2 v2 on v1.V1Id = v2.V1id 
	inner join acc.VoucherNo vno on vno.VNId = v1.VNId	
	where convert(date,v1.TDate) <= @TillDate and  FYID = @FYId
	group by fid ,CompanyId

	union all

	select fid, case when OPBal>0 then OPBal else 0 end  as Debit, 
				case when opbal<0 then opbal else 0 end as Credit, 0 as PrYrBal,branchid
			from acc.VoucherBal where fyid  = @FYId 

	union all

	select fid, 0 as Debit, 0 as Credit, CLBal as PrYrBal,BranchId
		 from acc.VoucherBal  where fyid  = @FYId-1 
		 
)x group by x.Fid,Branchid --where Fid=@FId 

)y group by Y.Fid,Branchid
)YY

left outer join acc.FinAcc g on g.Fid = YY.Fid
where g.fid in (select Fid from @Tmp)

group by YY.Fid,Branchid,g.Fname
)z group by Branchid

)s

group by Branchid





/*

	declare @Tmp table (FId int, PId int, FName varchar(500));
	declare @rootId int=1
	declare @FYId int=15
	declare @TillDate datetime='2018-04-07'
	declare @BranchId varchar(100)='1,13'
	select yy.FID,g.Fname,branchid,sum(ISNULL(DebitAmount,0)) as DebitAmount,sum(ISNULL(CreditAmount,0)) as CreditAmount,sum(ISNULL(PrYrBalDB,0)) as PrYrBalDB,sum(ISNULL(PrYrBalCR,0)) as PrYrBalCR  from (
	select fid as FID,Branchid,Fname=(select top 1 Fname from acc.finacc where Fid=y.fid),sum(ISNULL(DebitAmount,0)) as DebitAmount,sum(ISNULL(CreditAmount,0)) as CreditAmount,sum(ISNULL(PrYrBalDB,0)) as PrYrBalDB,sum(ISNULL(PrYrBalCR,0)) as PrYrBalCR from (
	select fid,Branchid, sum(Debit) as DebitAmount, sum(credit) as CreditAmount, PrYrBalDB = Case when sum(PrYrBal)>0 then sum(pryrbal) end,
	PrYrBalCR = Case when sum(PrYrBal)<0 then sum(pryrbal)*-1  end
	  from 
	(
	Select Fid,CompanyId Branchid, sum(DebitAmount) Debit, sum(CreditAmount) Credit, 0 as PrYrBal from acc.Voucher1 v1 
	inner join acc.Voucher2 v2 on v1.V1Id = v2.V1id 
	inner join acc.VoucherNo vno on vno.VNId = v1.VNId	
	where convert(date,v1.TDate) <= @TillDate and vno.CompanyId  in (Select Value From _FNSplit(@BranchId,','))  and FYID = @FYId
	group by fid ,CompanyId

	union all

	select fid,branchid, case when OPBal>0 then OPBal else 0 end  as Debit, 
				case when opbal<0 then opbal else 0 end as Credit, 0 as PrYrBal
			from acc.VoucherBal where fyid  = @FYId and BranchId  in (Select Value From _FNSplit(@BranchId,','))

	union all

	select fid,BranchId, 0 as Debit, 0 as Credit, CLBal as PrYrBal
		 from acc.VoucherBal  where fyid  = @FYId-1 and BranchId  in (Select Value From _FNSplit(@BranchId,','))
		 
)x group by x.Fid,Branchid --where Fid=@FId 
)y group by Y.Fid,Branchid
)YY

left outer join acc.FinAcc g on g.Fid = YY.Fid
--where g.fid in (select Fid from @Tmp)

group by YY.Fid,Branchid,g.Fname
*/









--select @RootId as FId,@RootFName as FName, ISNULL(Sum(DebitAmount),0) DebitAmount, ISNULL(sum(CreditAmount),0) CreditAmount,BranchId

----CASE 

----	WHEN SUM(ISNULL(DebitAmount,0))+SUM(ISNULL(CreditAmount,0))>0

----THEN CONVERT(DECIMAL(18,2),SUM(ISNULL(DebitAmount,0))+SUM(ISNULL(CreditAmount,0)))

----ELSE 

----		CASE WHEN SUM(DebitAmount)!=0

----		THEN CONVERT(DECIMAL(18,2),0)

----		ELSE

----		 Null

----		 END

---- END as DebitAmount,

----CASE 

----	WHEN SUM(ISNULL(DebitAmount,0))+SUM(ISNULL(CreditAmount,0)) <0

----THEN CONVERT(DECIMAL(18,2),SUM(ISNULL(DebitAmount,0))+SUM(ISNULL(CreditAmount,0)))

----ELSE

----	CASE WHEN SUM(CreditAmount)!=0

----		THEN CONVERT(DECIMAL(18,2),0)

----		ELSE

----		 Null

----		 END

---- END as CreditAmount



--from (

--		select g.FId, g.PId, t.Fname as LedgerName, sum(t.DebitAmount) as DebitAmount , sum(t.CreditAmount) as CreditAmount ,BranchId

--		from ( 

--				select a.fid,a.fname,

--				 CAST(

--             CASE 

--                  WHEN b.CLBal>0  

--                     THEN b.CLBal

--                  ELSE null

--             END AS numeric(18,2)) as DebitAmount,  

--			  CAST(

--             CASE 

--                  WHEN b.CLBal<0  

--                     THEN b.CLBal 

--                  ELSE null

--             END AS numeric(18,2)) as CreditAmount,b.BranchId

-- from acc.finacc a inner join acc.VoucherBal b on a.fid=b.FId

 

--  --where FYId=@FYId



--				) t 

--		left outer join acc.FinAcc g on g.Fid = t.Fid

--		where g.Fid in (select Fid from @Tmp)

--		group by g.Fid,t.Fname, g.PId, g.Fname,t.BranchId

--)x group by BranchId

return

end
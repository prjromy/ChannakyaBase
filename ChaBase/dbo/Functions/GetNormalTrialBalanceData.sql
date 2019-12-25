--sp_helptext '[dbo].[GetNormalTrialBalanceData]'


--select * from [dbo].[GetNormalTrialBalanceData](1,15,'2018-04-07',1)
CREATE function [dbo].[GetNormalTrialBalanceData]

(

@rootId int,@FYId int,@TillDate datetime,@BranchId int

)

RETURNS  @TempTbl TABLE

(

FId int,Fname varchar(500),DebitAmount numeric(18,2) , CreditAmount numeric(18,2),PrYrBalDB numeric(18,2),PrYrBalCR numeric(18,2)

)

AS 

BEGIN 

declare  @RootFName varchar(500), @ChildId int 

declare @Tmp table (FId int, PId int, FName varchar(500));





with name_tree as 

(

   select FId,Pid,Fname from acc.FinAcc  where Fid =@rootId

   union all

   select C.FId, C.PID, c.Fname from acc.FinAcc c

   join name_tree p on p.FId = C.PID

)





insert into @Tmp

select * from name_tree

insert into @TempTbl 



select @RootId as FID,'' as FName,Sum(DebitAmount),Sum(CreditAmount),Sum(PrYrBalDB),Sum(PrYrBalCr) from(

select @RootId as FId,'' as FName, Sum(ISNULL(DebitAmount,0)) DebitAmount, sum(ISNULL(CreditAmount,0)) as CreditAmount,sum(ISNULL(z.PrYrBalDB,0)) as PrYrBalDB,sum(ISNULL(z.PrYrBalCr,0)) as PrYrBalCr from



(



select y.fid as FID,Fname=(select top 1 Fname from acc.finacc where Fid=y.fid),sum(ISNULL(DebitAmount,0)) as DebitAmount,sum(ISNULL(CreditAmount,0)) as CreditAmount,sum(ISNULL(PrYrBalDB,0)) as PrYrBalDB,sum(ISNULL(PrYrBalCR,0)) as PrYrBalCR from(



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

		 from acc.VoucherBal where fyid  = @FYId and BranchId = case when @BranchId=0 then BranchId else @BranchId end 



	

	union all



	select fid, 0 as Debit, 

				 0 as Credit, CLBal as PrYrBal

		 from acc.VoucherBal  where fyid  = @FYId-1 and BranchId = case when @BranchId=0 then BranchId else @BranchId end 

		 

)x group by x.Fid --where Fid=@FId 

)y

left outer join acc.FinAcc g on g.Fid = y.Fid

where g.fid in (select Fid from @Tmp)

group by y.Fid



)z group by z.FID,z.Fname)s

















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
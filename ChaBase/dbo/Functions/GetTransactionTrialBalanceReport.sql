CREATE FUNCTION [dbo].[GetTransactionTrialBalanceReport]
(	
	@fromDate datetime,@TillDate datetime,@BranchId int
)
RETURNS   @finaltbl TABLE
(
FId int,Fname varchar(500),PId int,TDate datetime,DebitAmount numeric(18,2) , CreditAmount numeric(18,2),BranchId int
)
AS
BEGIN 

DECLARE @MyCursor CURSOR;
declare @RootId int, @RootFName varchar(500), @ChildId int 
declare @myrootId int

declare @Tmp1 table (FId int, PId int, FName varchar(500));
  
   with name_tree as 
(
   select Fid, Pid, Fname from acc.FinAcc  --where Fid =1
   union all
   select C.Fid, C.Pid, c.Fname from acc.FinAcc c
   join name_tree p on p.Fid = C.Pid 
)
insert into @Tmp1
select * from name_tree

--select * from @Tmp1 where FName='Liabilities'

--select FId from @Tmp1
----------------------------------CURSOR REGION---------------------------
  SET @MyCursor = CURSOR FOR
  select distinct FId from @Tmp1

    OPEN @MyCursor 
    FETCH NEXT FROM @MyCursor 
    INTO @myrootId

    WHILE @@FETCH_STATUS = 0
    BEGIN
  insert into @finaltbl
  SELECT  FId,(select  Fname from acc.FinAcc where Fid=@myrootId),
  (select  PId from acc.FinAcc where FId=@myrootId),
  (select  top 1 a.TDate from acc.voucher1 a inner join acc.Voucher2 b on a.V1Id=b.V1id where b.FId=@myrootId),
-- case when ((ISNULL(DebitAmount,0)+ ISNULL(CreditAmount,0))>0)
-- THEN 
--(ISNULL(DebitAmount,0)+ ISNULL(CreditAmount,0))
-- end 
-- as DebitAmount,
-- case  
-- when ((ISNULL(DebitAmount,0)+ ISNULL(CreditAmount,0))<0)
-- THEN 
-- (ISNULL(DebitAmount,0)+ ISNULL(CreditAmount,0))
-- end 
-- as CreditAmount
DebitAmount as DebitAmount,CreditAmount as CreditAmount,CompanyId as BranchId
		from dbo.GetTransactionTrialBalanceData(@myrootId) 
		--from dbo.GetTransTBData(@myrootId,@BranchId,@fromDate,@TillDate)
			group by FId,DebitAmount,CreditAmount,CompanyId
      
      FETCH NEXT FROM @MyCursor 
      INTO @myrootId 
    END; 

    CLOSE @MyCursor ;
    DEALLOCATE @MyCursor;
	--------------END CURSOR------------------------
	
	return
end
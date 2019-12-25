-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[GetProfitAndLossReport]
(	
)
RETURNS   @finaltbl TABLE
(
FId int,Fname varchar(150),PId int,TDate datetime,DebitAmount numeric(18,2) , CreditAmount numeric(18,2)
)
AS
BEGIN 
DECLARE @MyCursor CURSOR;
declare @RootId int, @RootFName varchar(150), @ChildId int 
declare @myrootId int
declare @FPID int
declare @Tmp1 table (FId int, PId int, FName varchar(150));

   with name_tree as 
(
   select FId, PID, Fname from acc.FinAcc  where Fid !=1 AND Fid!=2
   union all
   select C.FId, C.PID, c.Fname from acc.FinAcc c 
   join name_tree p on p.FId = C.PID 
)
insert into @Tmp1
select * from name_tree

--select FId from @Tmp1
----------------------------------CURSOR REGION---------------------------
  SET @MyCursor = CURSOR FOR
  select distinct FId from @Tmp1

    OPEN @MyCursor 
    FETCH NEXT FROM @MyCursor 
    INTO @myrootId

    WHILE @@FETCH_STATUS = 0
    BEGIN
	
  

	  with name_tree12 as 
(
   select PId,FId
   from acc.FinAcc
   where Fid = @myrootId
   union all
   select c.Pid,c.FId
   from acc.FinAcc c
   join name_tree12 p on C.Fid = P.PID   
    --AND C.Fid<>c.pid
) 

--select top 1 FID from name_tree12 t order by Fid asc
	
	
	select  @FPID= t.Fid from name_tree12 t order by t.Fid desc

IF(@FPID!=1 AND @FPID!=2)
  insert into @finaltbl
  SELECT FId,(select Fname from acc.FinAcc where Fid=@myrootId), (select  PId from acc.FinAcc where FId=@myrootId),
  (select top 1 a.TDate from acc.voucher1 a inner join acc.Voucher2 b on a.V1Id=b.V1id where b.FId=@myrootId),
  case when (sum(ISNULL(DebitAmount,0)) + sum(ISNULL(CreditAmount,0))>0) 
 THEN 
 sum(ISNULL(DebitAmount,0))+sum(ISNULL(CreditAmount,0))  
 end 
 as DebitAmount,
 case  
 when (sum(ISNULL(DebitAmount,0)) + sum(ISNULL(CreditAmount,0))<0) 
 THEN 
 (sum(ISNULL(DebitAmount,0))+sum(ISNULL(CreditAmount,0))) *(-1)
 end 
 as CreditAmount from dbo.GetTransactionTrialBalanceData(@myrootId) group by FId,Fname,DebitAmount,CreditAmount
  
      
    FETCH NEXT FROM @MyCursor 
		INTO @myrootId 
		END; 

	CLOSE @MyCursor ;
    DEALLOCATE @MyCursor;
	--------------END CURSOR------------------------

return
end
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
 

CREATE FUNCTION [dbo].[GetNormalTrialBalanceReport]
(	
	@TillDate datetime,@BranchId varchar(100)
)
RETURNS   @finaltbl TABLE
(
	FId int,Fname varchar(500),PId int,TDate datetime,DebitAmount numeric(18,2) , CreditAmount numeric(18,2),PrevYrDbAmnt numeric(18,2),PrevYrCrAmnt numeric(18,2)
)
AS
BEGIN 
	DECLARE @MyCursor CURSOR;
	declare @RootId int, @RootFName varchar(500), @ChildId int 
	declare @myrootId int
	declare @FYId int
	declare @Tmp1 table (FId int, PId int, FName varchar(500));

	Set @FYId = (select fyid from lg.FiscalYears where @TillDate between StartDt and EndDt) ;	
  
    with name_tree as 
	(
		select Fid, Pid, Fname from acc.FinAcc  --where Fid =1
		union all
		select C.Fid, C.Pid, c.Fname from acc.FinAcc c
		join name_tree p on p.Fid = C.Pid 
	)
	insert into @Tmp1 Select * from name_tree

	--select * from @Tmp1 where FName='Liabilities'

	--select FId from @Tmp1
	----------------------------------CURSOR REGION---------------------------
	SET @MyCursor = CURSOR FOR
	select distinct FId from @Tmp1

    OPEN @MyCursor 
    FETCH NEXT FROM @MyCursor INTO @myrootId
    WHILE @@FETCH_STATUS = 0
    BEGIN
		insert into @finaltbl
		SELECT  @myrootId as FID,FName=(select top 1 Fname from acc.finacc where Fid=@myrootId),
		(select  PId from acc.FinAcc where FId=@myrootId),
		(select  top 1 a.TDate from acc.voucher1 a inner join acc.Voucher2 b on a.V1Id=b.V1id where b.FId=@myrootId),
		ISNULL(DebitAmount,0),ISNULL(CreditAmount,0),ISNULL(PrYrBalDB,0),ISNULL(PrYrBalCR,0)
		from [dbo].[GetNormalTrialBalanceData](@myrootId,@FYId,@TillDate,(Select Value From _FNSplit(@BranchId,','))) where DebitAmount!=0 or CreditAmount!=0 or PrYrBalDB!=0 or PrYrBalCR!=0 
		group by FId,DebitAmount,CreditAmount,PrYrBalDB,PrYrBalCR,Fname
      
		FETCH NEXT FROM @MyCursor INTO @myrootId 
    END; 
    CLOSE @MyCursor ;
    DEALLOCATE @MyCursor;
	--------------END CURSOR------------------------
	
	return
end
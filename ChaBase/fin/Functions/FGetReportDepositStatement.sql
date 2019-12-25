
CREATE  FUNCTION [fin].[FGetReportDepositStatement]
	(
		@AccountId INT, 
		@FromDate date, 
		@ToDate date,
		@SortBy int
	)
RETURNS @STATEMENT TABLE
	(
		Tno					 INT, 
		Tdate				SMALLDATETIME, 
		Vdate				SMALLDATETIME, 
		Description			VARCHAR(2000),  
		Debit			 MONEY, 
		Credit				MONEY, 
		Balance			 MONEY, 
		DC					VARCHAR(2),
		IsMarked		bit
	) 
---MARKED ENCRYPTION  
AS
BEGIN
declare @Ismarked bit 
declare @ChkedTill datetime 
set @ChkedTill= (select top 1 ChkedTill from fin.APBookChked where IAccNo=@AccountId)
Declare @TNo integer, @TDate smalldatetime, @VDate smalldatetime,  @TDateN nvarchar(10), @VDateN nvarchar(10)
Declare   @Particulars Varchar(2000),@Debit Money, @Credit Money, @Balance money, @DC varchar(2)


set @Balance = isnull(( select  OpeningBal from acc.SubsiVSOpeningBalance where FID in 
		(select Fid from acc.FinAcc a inner join acc.FinSys2  b on a.F2Type=b.F2id inner join acc.FinSys1 c on c.F1id=b.F1id where c.F1id=15) 
	and SubsiId=@AccountId),0) * -1


set @Balance  = isnull(@Balance,0) + isnull((select isnull(sum(cramt),0)-isnull(sum(dramt),0) from fin.FGetReportDepositTranx(@AccountId,'1-1-1990',DATEADD(day,-1,@fromdate))),0)


--Select @Balance = balance from  fin.FgetReportGeTDepositOBaL(@AccountId, @FromDate)
		 
if isnull(@Balance,0)  <> 0 
begin
	if @Balance >=0 
		begin
			set @DC = 'Cr'
		end
	else
		begin
			set @DC  = 'Dr'
		end
	insert @Statement(TDate,  Description,  Balance, DC,IsMarked) values ( @FromDate, 'Opening Balance', abs(@Balance), @DC,isnull(@Ismarked,0))
end	

set @ChkedTill =(select top 1 ChkedTill from fin.APBookChked where IAccNo=@AccountId order by ChkedOn desc )
if @sortby = 0
Begin
	DECLARE RStatementCur CURSOR FOR
		--SELECT	tno, tdate,  vdate,notes, dramt, cramt
		--FROM fin.FGetReportDepositTranx (@AccountId, @FromDate, @ToDate)
		--order by Vdate,tno 
		SELECT	tno, tdate,  vdate,notes, dramt, cramt ,case when (@ChkedTill is not null and tdate<= @ChkedTill)  then cast(1 as bit) else cast(0 as bit) end as IsMarked 
		FROM fin.FGetReportDepositTranx (@AccountId, @FromDate, @ToDate) a
		order by Vdate,tno 
end
else
Begin
	DECLARE RStatementCur CURSOR FOR
		--SELECT	tno, tdate,  vdate,notes, dramt, cramt
		--FROM fin.FGetReportDepositTranx (@AccountId, @FromDate, @ToDate)
		--order by tdate,tno 
		SELECT	tno, tdate,  vdate,notes, dramt, cramt ,case when (@ChkedTill is not null and tdate<= @ChkedTill)  then cast(1 as bit) else cast(0 as bit) end as IsMarked 
		FROM fin.FGetReportDepositTranx (@AccountId, @FromDate, @ToDate) a 
		order by Vdate,tno 
end


OPEN RStatementCur
FETCH RStatementCur INTO @TNo, @TDate, @VDate, @Particulars,@Debit,@Credit,@IsMarked
WHILE   @@FETCH_STATUS=0 
BEGIN
	set @Balance =ROUND(@Balance-@Debit+@Credit,2)
	BEGIN			
		IF @Balance>=0		
			set @DC = 'Cr'
		else
			set @DC = 'Dr'
	END
	
	insert into @Statement (TNo , TDate , VDate, Description ,  Debit , Credit , Balance , DC,IsMarked)
		values (@TNo, @TDate, @VDate, @Particulars, case when @Debit <>  0 then @Debit else null end, Case when @Credit <> 0 then @Credit else null end, abs(@Balance), @DC,@Ismarked)

	FETCH NEXT FROM  RStatementCur INTO @TNo, @TDate, @VDate, @Particulars,@Debit,@Credit,@Ismarked
END
CLOSE RStatementCur
DEALLOCATE RStatementCur
return  
end
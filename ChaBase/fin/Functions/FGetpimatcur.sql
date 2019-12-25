CREATE FUNCTION [fin].[FGetpimatcur](@IACCNO INT, @DT SMALLDATETIME)

RETURNS @TEMPTBL TABLE 
	(MaturePrincipal FLOAT, CurrentPrincipal FLOAT,CurrentDate SMALLDATETIME,  MatureInterest FLOAT, CurrentInterest FLOAT)
---MARKED ENCRYPTION 
AS
BEGIN
DECLARE @pM FLOAT
DECLARE @iM FLOAT

DECLARE @dC SMALLDATETIME
DECLARE @pC FLOAT
DECLARE @iC FLOAT

select @pM=sum(isnull(schPrin,0))-sum(isnull(pdPrin,0)), @iM=sum(isnull(calcInt,0))-sum(isnull(pdInt,0)) from fin.ALSch where schDate<@dt and iaccno=@iaccno

select @pC= sum(isnull(schPrin,0))-sum(isnull(pdPrin,0)),@iC  =sum(isnull(calcInt,0))-sum(isnull(pdInt,0)) 
		from fin.ALSch  where schDate>=@dt and iaccno=@iaccno 

--DECLARE @isRev bit
--select @isRev=revolving from ALoan where iaccno=@iaccno

--	DECLARE piMat CURSOR FOR
--		select sum(isnull(schPrin,0))-sum(isnull(pdPrin,0)) as PrinMat, sum(isnull(calcInt,0))-sum(isnull(pdInt,0)) as IntMat from ALSch where schDate<@dt and iaccno=@iaccno
--	OPEN piMat
--		FETCH piMat INTO @pM,@iM
--	CLOSE piMat
--	DEALLOCATE piMat
----if @isRev=1 
----begin
--	DECLARE piCur CURSOR FOR
--		select (select top 1 schDate  from ALSch  where schDate=@dt and iaccno=@iaccno order by schDate) as schDate,x.* from (select  sum(isnull(schPrin,0))-sum(isnull(pdPrin,0))  as PrinCur, sum(isnull(calcInt,0))-sum(isnull(pdInt,0)) as IntCur from ALSch  where schDate>=@dt and iaccno=@iaccno ) as x
--/*end
--else
--begin
--	DECLARE piCur CURSOR FOR
--		select top 1 schDate, isnull(schPrin,0)-isnull(pdPrin,0)  as PrinCur, isnull(calcInt,0)-isnull(pdInt,0) as IntCur from ALSch  where schDate=@dt and iaccno=@iaccno order by schDate

--end*/
--	OPEN piCur
--		FETCH piCur INTO @dC,@pC,@iC
--	CLOSE piCur
--	DEALLOCATE piCur
		
--	set @pM  = isnull(@pM, 0)
--	set @IM = isnull(@iM, 0)

--	set @pC = isnull(@pC,0)
--	set @iC = isnull(@iC, 0)

--	set @dC = isnull(@dC, @dt)


	INSERT @tempTbl SELECT isnull(@pM,0),isnull(@pC,0),@DT,isnull(@iM,0),isnull(@iC,0)
	RETURN 
END
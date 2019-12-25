CREATE PROCEDURE [fin].[GetSchDetailOnPymnt]
(
	@IACCNO INT, 
	@TDATE DATETIME
)
---MARKED ENCRYPTION 
AS
BEGIN 
declare @DPDate datetime
declare @DIDate datetime

declare @InstPri money
declare @InstInt money

declare @MatPri money
declare @MatInt money


set @DPDate = (select min(schdate) from fin.alsch where iaccno = @IAccno and schprin > 0 and schdate  >=  @tDate)
set @DIDate = (select min(schdate) from fin.alsch where iaccno = @IAccno and schint > 0 and schdate  >=  @tDate)
--
set @InstPri = (select schprin from fin.alsch where iaccno = @IAccno and schdate = @DPDate)
set @Instint = (select schint from fin.alsch where iaccno = @IAccno and schdate = @DiDate)
--
set @MatPri = (select sum(schprin) - isnull(sum(pdprin),0) from fin.alsch where iaccno = @IAccno and schdate  <  @tdate)
set @MatInt = ( select sum(calcInt) - isnull(sum(pdInt),0) from fin.alsch where iaccno = @IAccno and schdate < @tdate)

select @DPdate as DueDate,  isnull(@InstPri,0) as Installment , isnull(@MatPri,0) as Mature
	union all
select @DIdate as DueDate,  isnull(@InstInt,0) as Installment , isnull(@MatInt,0) as Mature


RETURN
END
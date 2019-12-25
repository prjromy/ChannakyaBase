CREATE FUNCTION [dbo].[GetLedgerType] 
(   
    @ledgerId int
)

RETURNS  int --Returning an int 

AS
BEGIN
DECLARE @RootId int
;with name_tree as 
(
   select FId,Pid,Fname from acc.FinAcc a  where Fid =@ledgerId
   union all
   select C.FId, C.PID, c.Fname from acc.FinAcc c
   join name_tree p on p.PId = c.FID
)
SELECT @RootId =(select top 1 FID from name_tree order by FID asc)

RETURN @RootId

END
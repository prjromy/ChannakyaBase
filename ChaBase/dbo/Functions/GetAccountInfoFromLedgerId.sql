CREATE function [dbo].[GetAccountInfoFromLedgerId](@ledgerId int)
returns table as 
return
(
select c.IAccno, c.AName, c.Accno from fin.ADetail c
 where PID =(select top 1 a.PID from fin.productdetail a where a.FID =@ledgerId) and AccState = 1
)
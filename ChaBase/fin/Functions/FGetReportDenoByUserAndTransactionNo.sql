﻿-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [fin].[FGetReportDenoByUserAndTransactionNo] 
(	
	-- Add the parameters for the function here
	@tno decimal,
	@UserId int


)
RETURNS TABLE 
AS
RETURN 
(
select dt.Denoid,
CurrencyName,
emp.EmployeeName,
ug.UserName,
ds.Deno,
 sum(dt.Pics) as Pics, 
 cast(sum(dt.Pics*Deno)as decimal(18,2)) as Amount
 from trans.DenoTrxn dt
join lg.[User] ug on ug.UserId=dt.UserId
join fin.Denosetup ds on ds.DenoID=dt.Denoid
join lg.Employees emp on emp.EmployeeId=ug.EmployeeId
join fin.CurrencyType ct on ct.CTId=ds.CurrID and ds.DenoID=dt.Denoid
where dt.Trxno=@tno and dt.UserId=@UserId
group by dt.Denoid,CurrencyName,emp.EmployeeName,ug.UserName,ds.Deno,CurrID
)
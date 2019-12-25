
CREATE function [fin].[FGetUserCashReceivedRejectedList](@userid int)
returns table
as return(
select distinct a.*, d.DesignationId, d.EmployeeName, d.EmployeeId, d.TUserId, d.DGName
from fin.CashFlow a inner join (select b.DesignationId, b.DGName, b.EmployeeId, b.EmployeeName, c.UserID as TUserId, c.FUserid
from fin.CashFlow c inner join fin.FGetAllUsersWithDesignation() b on c.FUserid = b.UserId where Status =21)
d on d.TUserId = a.UserID where Status =21 and a.UserID = @userid
union
select distinct a.*, d.DesignationId, d.EmployeeName, d.EmployeeId, d.TUserId, d.DGName
from fin.CashFlow a inner join (select b.DesignationId, b.DGName, b.EmployeeId, b.EmployeeName, c.UserID as TUserId, c.FUserid
from fin.CashFlow c inner join fin.FGetAllUsersWithDesignation() b on c.FUserid = b.UserId where Status = 24)
d on d.TUserId = a.UserID where Status =24 and a.FUserid = @userid)
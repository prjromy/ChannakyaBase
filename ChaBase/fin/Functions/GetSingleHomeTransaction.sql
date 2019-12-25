CREATE function fin.GetSingleHomeTransaction(@Tno int )
returns table
as return(
select 
	 a.TNO,b.DesignationId as DesignationId,b.UserId  as FUserid,a.Dramt as Dramt,a.Status, b.EmployeeName as EmployeeName, b.DGName as DGName,  b.EmployeeId as FromEmployeeId,
	c.UserId as ToUserId, c.DesignationId as ToDesignationId, c.EmployeeId as ToEmployeeId, c.EmployeeName as ToEmployeeName, c.DGName as ToDGName
from fin.CashFlow a 
left join fin.FGetAllUsersWithDesignation() b on a.FUserid = b.UserId
left join fin.FGetAllUsersWithDesignation() c on a.UserId = c.UserId
where a.TNO = @Tno )
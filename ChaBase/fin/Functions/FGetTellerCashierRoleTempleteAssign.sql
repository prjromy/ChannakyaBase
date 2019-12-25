-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create FUNCTION [fin].[FGetTellerCashierRoleTempleteAssign]
(	
	@eventId int,
	@branchid int
)

returns @temp table(UserId int, UserName varchar(100),EmployeeId int,EmployeeName varchar(100))
as
begin

Declare @MenuId int 
if @eventId = 11
	begin
		set @MenuId  = 5012
	end
	if @eventId = 12
	begin
		set @MenuId  = 5011
	end
insert into @temp
SELECT u.UserId,u.UserName,emp.EmployeeId, emp.EmployeeName FROM fin.EmployeeVsBranch E
join lg.[User] u on u.EmployeeId=e.EmpId
join lg.Employees emp on emp.EmployeeId=u.EmployeeId
JOIN LG.MenuTemplate MT ON e.CurrentRole=mt.DesignationId
join lg.MenuVsTemplate mvt on mvt.TemplateId=mt.MenuTemplateId

where MenuId=@MenuId and e.BranchId=@branchid and  e.StartFrom <= CONVERT(VARCHAR(10), GETDATE(), 110) and IsActive=1
return 
end
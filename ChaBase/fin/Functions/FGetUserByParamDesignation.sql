
CREATE FUNCTION [fin].[FGetUserByParamDesignation] (@DegId int, @BranchId int)
RETURNS TABLE AS RETURN
(
	--select e.EmployeeId,EmployeeNo, EmployeeName,UserId,UserName 
	--from lg.Employees e 
	--inner join( select EmployeeId,UserId,username 
	--			from lg.[user] 
	--			where MTId in( select MenuTemplateId 
	--						   from lg.Designation a 
	--						   inner join lg.ParamValue b on a.DesignationId=b.PValue 
	--						   inner join lg.menutemplate t on t.designationId=a.designationId 
	--						   where pid=@DegId) ) a on e.EmployeeId=a.EmployeeId 
	--inner join fin.EmployeeVsBranch b on a.EmployeeId = b.EmpId where b.BranchId=@BranchId)

	
	select U.UserId, E.EmployeeId, E.EmployeeNo, E.EmployeeName, U.UserName--, E.BranchId
	from LG.Employees E 
	inner join (select EmployeeId, UserId, UserName 
				from LG.[User] 
				where MTId in (select t.MenuTemplateId 
							   from LG.Designation d                            
							   inner join LG.MenuTemplate t on t.DesignationId = d.DesignationId 
							   where d.DesignationId = @DegId) ) U on U.EmployeeId = E.EmployeeId
	where E.BranchId = @BranchId

	union 

	select U.UserId, U.EmployeeId, E.EmployeeNo, E.EmployeeName, U.UserName--, UB.BranchId
	from LG.UserVSBranch UB
	inner join (select EmployeeId, UserId, UserName 
				from LG.[User] 
				where MTId in (select t.MenuTemplateId 
							   from LG.Designation d                            
							   inner join LG.MenuTemplate t on t.DesignationId = d.DesignationId 
							   where d.DesignationId = @DegId) ) U on U.UserId = UB.UserId
	inner join LG.Employees E on E.EmployeeId = U.EmployeeId
	where UB.BranchId = @BranchId and CAST(GETDATE() as date) between UB.[From] and ISNULL(UB.[To], CAST(GETDATE() as date))
)
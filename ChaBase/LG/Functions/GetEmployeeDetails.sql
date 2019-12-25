
CREATE function [LG].[GetEmployeeDetails]()
returns 
table 
as 
return
(
	select emp.EmployeeId, emp.EmployeeNo,emp.EmployeeName,dep.DeptName,deg.DGName 
		from [LG].[Employees] emp
		left outer join [LG].[Department] dep on dep.[DepartmentId]=emp.[DeptId]
		left outer join [LG].[Designation] deg on deg.[DesignationId]=emp.[DGId]
)
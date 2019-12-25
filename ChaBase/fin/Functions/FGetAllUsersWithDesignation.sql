CREATE function [fin].[FGetAllUsersWithDesignation]()  
 returns table 
 as 
 return
 (	
 select 
 distinct
		UserId,
		f.DesignationId,
		DGName,
		em.EmployeeId,
		EmployeeName,
		EmployeeNo,
		case when (select Count(BranchId) from lg.UserVSBranch a where IsEnable=1 and a.UserId=u.UserId)!=0 
		then (select top 1 BranchId from lg.UserVSBranch a where IsEnable=1 and a.UserId=u.UserId) else em.BranchId  end as BranchId,
		UserName,DeptId,DeptName,f.PId,DegOrder
	from lg.MenuTemplate e 
	inner join lg.[user] u on u.MTId=e.MenuTemplateId 
	inner join lg.Employees em on em.EmployeeId=u.EmployeeId
	inner join
		(select c.DesignationId,c.DGName,PValue,PId from lg.Designation c 
		 inner join
		(select a.PId,b.PName,PValue from lg.ParamValue a 
		 inner join
		(select * from lg.Param where parentid=2004) b on a.PId=b.PId) d on d.PValue=cast(c.DesignationId as varchar)) f on e.DesignationId=f.DesignationId
	--inner join fin.EmployeeVsBranch eb on eb.EmpId=em.EmployeeId
	inner join lg.FGetHierarchyWiseDesignation() lgdg on lgdg.PId=f.PId
	inner join lg.department d on em.DeptId=d.DepartmentId where em.Status=1 
 )
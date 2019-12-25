CREATE Function [fin].[FGetAllVault]()
 returns table 
 as 
 return
 (	select 
		UserId,
		f.DesignationId,
		DGName,
		em.EmployeeId,
		EmployeeName,
		eb.BranchId
	from lg.MenuTemplate e 
	inner join lg.[user] u on u.MTId=e.MenuTemplateId 
	inner join lg.Employees em on em.EmployeeId=u.EmployeeId
	inner join (select c.DesignationId,c.DGName from lg.Designation c 
	inner join (select a.PId,b.PName,PValue from lg.ParamValue a 
	inner join (select * from lg.Param where parentid=2004 and PId=2007) b on a.PId=b.PId) d on d.PValue=cast(c.DesignationId as varchar)) f on e.DesignationId=f.DesignationId
	inner join fin.EmployeeVsBranch eb on eb.EmpId=em.EmployeeId  where em.Status=1
 )
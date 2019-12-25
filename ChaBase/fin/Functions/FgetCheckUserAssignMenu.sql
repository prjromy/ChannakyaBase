create function [fin].[FgetCheckUserAssignMenu](@menuID int)
returns @temp table(UserId int, UserName varchar(100),EmployeeId int,EmployeeName varchar(100))
as
begin
insert into @temp
select UserId,UserName,e.EmployeeId,EmployeeName from lg.[User] u 
	 inner join lg.employees e on e.employeeid=u.employeeid
	 where
	 MTId in (select m.TemplateId from lg.MenuVsTemplate m where m.MenuId=@menuID) 
	 and isactive=1
	 return 
	 end
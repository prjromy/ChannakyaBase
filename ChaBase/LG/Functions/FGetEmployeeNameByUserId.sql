 create function [LG].[FGetEmployeeNameByUserId](@UserId int)
   returns varchar(100)
   as
   begin
		declare @EmployeeName varchar(500)

	select @EmployeeName=
		e.EmployeeName from lg.employees e 
		inner join
		lg.[User] u on e.EmployeeId = u.EmployeeId
		where 
		u.UserId=@UserId
		return @EmployeeName
   end
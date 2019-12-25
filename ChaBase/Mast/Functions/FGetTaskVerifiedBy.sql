﻿CREATE function [Mast].[FGetTaskVerifiedBy]( @Task1Id int)
  returns table
  as return(
  select a.Task1Id,lg.FGetEmployeeNameByUserId(TaskTo) as Verifier,TaskTo,VerifiedOn,
  UserName,EmployeeName,IsVerified
  from Mast.Task a inner join mast.TaskDetails b on a.Task1Id = b.Task1Id
  inner join lg.[User] u on a.RaisedBy = u.UserId
  inner join lg.employees e on e.employeeid=u.employeeid
  where  VerifiedOn is not null and b.Task1Id=
   @Task1Id)
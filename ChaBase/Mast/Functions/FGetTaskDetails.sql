CREATE function [Mast].[FGetTaskDetails]( @UserId int)
  returns table
  as return(
  select a.*,[IsRejected]= case
  when a.RejectedOn is null then cast(0 as bit)
  else  cast(1 as bit)
  end 
  from 
  (select a.Task1Id,RaisedOn,RaisedBy,EventId,EventValue,Message,TaskTo,VerifiedOn,SeenOn
  ,UserName,EmployeeName,IsVerified,RejectedOn
  from Mast.Task a inner join mast.TaskDetails b on a.Task1Id = b.Task1Id
  inner join lg.[User] u on a.RaisedBy = u.UserId
  inner join lg.employees e on e.employeeid=u.employeeid
  where b.TaskTo=@UserId and isverified=0 and RejectedOn is null
  union
   select a.Task1Id,RaisedOn,RaisedBy,EventId,EventValue,Message,TaskTo,VerifiedOn,SeenOn
  ,UserName,EmployeeName,IsVerified,RejectedOn
  from Mast.Task a inner join mast.TaskDetails b on a.Task1Id = b.Task1Id
  inner join lg.[User] u on a.RaisedBy = u.UserId
  inner join lg.employees e on e.employeeid=u.employeeid
   where RaisedBy=@UserId and RejectedOn is not null) as a
   )

CREATE function fin.GetRejectedOrAccepted(@userid int)
returns table
as return(
select x.TNO,x.Status,b.DesignationId as DesignationId,b.UserId as FUserid,x.Dramt as Dramt ,b.EmployeeName as EmployeeName, b.DGName as DGName, a.DGName as TDGName,a.EmployeeName as ToEmployeeName
 --x.*,a.DGName as fuserdesination ,

 from (

select * from fin.CashFlow where UserID=@userid  and Status =21

union
select * from fin.CashFlow where FUserid=@userid  and Status =24

)x  inner join fin.FGetAllUsersWithDesignation() a on x.Userid=a.UserId
 inner join fin.FGetAllUsersWithDesignation() b on x.FUserid=b.UserId)
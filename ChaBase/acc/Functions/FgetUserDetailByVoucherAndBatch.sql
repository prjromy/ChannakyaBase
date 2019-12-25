create function [acc].[FgetUserDetailByVoucherAndBatch](@VoucherType int,@BatchList nvarchar(max),@BrnhId int, @Fdate date,@TDate date)
returns table
as
return(
		--select distinct UserName,EmployeeName,e.EmployeeId,u.UserId from acc.UserVSVoucher UVV
		-- inner join lg.[user] u on UVV.UserId=u.UserId 
		-- inner join lg.Employees e on u.EmployeeId=e.EmployeeId
		-- where UserName like '%'+@query+'%' and UVV.VTypeId=@VoucherType and UVV.BatchId in(select value from dbo._FNSplit(@BatchList,','))

select distinct postedby as UserId ,UserName  from acc.voucher1 a inner join acc.voucherno b on a.vnid = b.vnid inner join lg.[user] c on a.PostedBy=c.UserId
	where bid in (select value from dbo._FNSplit(@BatchList,',')) and b.CompanyId =@BrnhId  and b.VTypeId = @VoucherType and tdate between @Fdate and @TDate
)
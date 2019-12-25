CREATE Function [acc].[FGetSubsiAccListFromFId](@FId int)
returns @Temp table(AccId int, AccNo varchar(25), AccName varchar(500),Cid int)
as
begin
	Declare @SubsiTblId int = acc.FGetSubsiTblIdFromFId(@FId)
	if @SubsiTblId = 1
	begin
		--get record from employee
		insert into @Temp
		Select SDID as AccId, sd.AccNo, e.EmployeeName as Name,sd.CId from acc.SubsiDetail sd inner join lg.Employees e on e.EmployeeId = sd.CId where FId = @FId
	end
	else if @SubsiTblId = 2
	begin
		--get record from user
		insert into @Temp
		Select SDID as AccId, sd.AccNo, e.EmployeeName as Name,sd.CId from acc.SubsiDetail sd inner join lg.[User] u on u.userId = sd.CId
	inner join lg.Employees e on e.EmployeeId = u.EmployeeId where FId = @FId
	end
	else if @SubsiTblId = 3
	begin
		--get record from customer
		insert into @Temp
		select SDID as AccId, AccNo, rtrim(ltrim(Name)) as Name ,sd.CId from acc.subsidetail sd inner join cust.fgetcustlist() c on sd.cid  = c.cid where fid = @FId
	end
	return
end
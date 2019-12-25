
 CREATE function [fin].[FGetAllCashierBranch1] (@DegParamId int, @BranchId int)

RETURNS  @rtnTable TABLE  (    

	EmployeeId int NOT NULL,
	EmployeeName nvarchar(255) NOT NULL,
	EmployeeNo nvarchar(255) NOT NULL,
	DGName nvarchar(255) NOT NULL,
	UserId int,
	UserName nvarchar(255) not null
)
AS
BEGIN
	Declare @DegId int 
	Declare @DGName  nvarchar(255) 
	Declare @Temp table (EmployeeId int,EmployeeNo nvarchar(255), EmployeeName nvarchar(255),isTemp bit, userId int,UserName nvarchar(255)  )
	Declare @TDate date = (Select TDate from fin.LicenseBranch where BrnhID = @BranchId)
	set @DegId = (select PValue from lg.ParamValue  where PId=@DegParamId)
	set @DGName=(select PDescription from lg.ParamValue  where PId=@DegParamId)

	insert into @Temp
	--select EmployeeId,EmployeeNo,EmployeeName,IsTemp,userId,UserName from (
	 select e.employeeId, e.EmployeeNo,e.EmployeeName, IsTemp = case when [To] is null then 0 else 1 end,
	  ub.userId, u.UserName from lg.uservsbranch ub 
		inner join 	lg.[User] u on u.UserId = ub.UserId
		inner join lg.[Employees] e on e.employeeId = u.EmployeeId
		where  ub.BranchId = @BranchId and 
		@TDate between [From] and isnull([To], @Tdate)
		--)X where IsTemp=0
    

	insert into @Temp
	 select e.EmployeeId, EmployeeNo,EmployeeName,    IsTemp = 0, u.UserId, u.UserName from lg.Employees e inner join lg.[user] u on u.employeeId = e.EmployeeId
	inner join lg.[MenuTemplate] mt on mt.MenuTemplateId = u.MTId
	where mt.designationId = @DegId and status = 1 and BranchId = @BranchId
	and e.employeeId not in (select EmployeeId from @Temp where isTemp = 0)



	insert into @rtnTable(EmployeeId ,
	EmployeeName ,	EmployeeNo ,DGName,	UserId,	UserName
	  )
	select distinct e.EmployeeId,  e.EmployeeName ,e.EmployeeNo, DGName = @DGName, t.userId ,t.UserName  from lg.Employees e
	 inner join lg.[MenuTemplate] mt on mt.DesignationId = e.DGId
	 inner join @Temp t on e.EmployeeId  = t.EmployeeId
	where e.EmployeeId in (select EmployeeId from @Temp where BranchId = @BranchId) and e.DGId   in (select PValue  from lg.ParamValue where PId = @DegParamId)
	return

end
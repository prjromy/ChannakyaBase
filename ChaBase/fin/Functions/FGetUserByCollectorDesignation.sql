CREATE function [fin].[FGetUserByCollectorDesignation] (@DegParamId int, @BranchId int)

RETURNS  @rtnTable TABLE  (

	EmployeeId int NOT NULL,

	EmployeeName nvarchar(255) NOT NULL,

	EmployeeNo nvarchar(255) NOT NULL,	

	UserName nvarchar(255) not null

)

AS

BEGIN



	Declare @DegId int 

	

	Declare @Temp table (EmployeeId int,EmployeeNo nvarchar(255), EmployeeName nvarchar(255),UserName nvarchar(255),isTemp bit  )

	Declare @TDate date = (Select TDate from fin.LicenseBranch where BrnhID = @BranchId)

	set @DegId = (select PValue from lg.ParamValue where PId = @DegParamId)



	insert into @Temp

	 select e.employeeId, e.EmployeeNo,e.EmployeeName,  u.UserName,IsTemp = case when [To] is null then 0 else 1 end  from lg.uservsbranch ub 

		inner join 	lg.[User] u on u.UserId = ub.UserId

		inner join lg.[Employees] e on e.employeeId = u.EmployeeId

		 where --ub.BranchId = @BranchId and 

			@TDate between [From] and isnull([To], @Tdate)



	insert into @Temp

	 select EmployeeId, EmployeeNo,EmployeeName,  '' as UserName ,  IsTemp = 0 from lg.Employees where dgid = @DegId and status = 1 and BranchId = @BranchId

		and employeeId not in (select EmployeeId from @Temp where isTemp = 0)

		 

		--Declare @UserName nvarchar

		

		--select @UserName =UserName from @Temp 

	insert into @rtnTable(EmployeeId ,

	EmployeeName ,

	EmployeeNo ,

	

	UserName )

	select EmployeeId,  EmployeeName ,EmployeeNo,(select top 1  UserName from @Temp) as UserName from lg.Employees where EmployeeId in (select EmployeeId from @Temp where BranchId = @BranchId)

	return

end

-- union all
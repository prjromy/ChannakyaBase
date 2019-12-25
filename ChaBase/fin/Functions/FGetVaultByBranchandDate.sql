CREATE function [fin].[FGetVaultByBranchandDate] (@DegParamId int, @BranchId int,@ByDate DateTime)

RETURNS  @rtnTable TABLE  (

	EmployeeId int NOT NULL,

	EmployeeName nvarchar(255) NOT NULL,

	EmployeeNo nvarchar(255) NOT NULL,
	DGName nvarchar(255) NOT NULL
	
)

AS

BEGIN



	Declare @DegId int 
	Declare @DGName  nvarchar(255) 
	

	Declare @Temp table (EmployeeId int,EmployeeNo nvarchar(255), EmployeeName nvarchar(255),isTemp bit  )

	--Declare @TDate date = (Select TDate from fin.LicenseBranch where BrnhID = @BranchId)

	set @DegId = (select PValue from lg.ParamValue  where PId=@DegParamId)
	set @DGName=(select PDescription from lg.ParamValue  where PId=@DegParamId)

--DEclare @BranchId int = 3
	--Declare @TDate date = (Select TDate from fin.LicenseBranch where BrnhID = @BranchId)
	--Declare @DegId int = 5
	insert into @Temp
	-- select e.employeeId, e.EmployeeNo,e.EmployeeName, IsTemp = case when [To] is null then 0 else 1 end , u.email, u.userName, e.DGId, mt.DesignationId
	select e.employeeId, e.EmployeeNo,e.EmployeeName, IsTemp = case when [To] is null then 0 else 1 end 
	  from lg.uservsbranch ub 

		inner join 	lg.[User] u on u.UserId = ub.UserId

		inner join lg.[Employees] e on e.employeeId = u.EmployeeId

		inner join lg.MenuTemplate mt on mt.MenuTemplateId = ub.RoleId and mt.DesignationId = @DegId
		 where 
		 ub.BranchId = @BranchId and 

			@ByDate between [From] and isnull([To], @ByDate)


	insert into @Temp

	 select e.EmployeeId, EmployeeNo,EmployeeName, IsTemp = 0 from lg.Employees e 
		inner join lg.[user] u on u.employeeId = e.EmployeeId
		inner join lg.[MenuTemplate] mt on mt.MenuTemplateId = u.MTId

	 where mt.designationId = @DegId and status = 1 and BranchId = @BranchId

		and e.employeeId not in (select EmployeeId from @Temp where isTemp = 0)

		 

		--Declare @UserName nvarchar

		

		--select @UserName =UserName from @Temp 

	insert into @rtnTable(EmployeeId ,

	EmployeeName ,

	EmployeeNo ,

	DGName 

	  )

	select EmployeeId,  EmployeeName ,EmployeeNo,DGName =@DGName  from lg.Employees where EmployeeId in (select EmployeeId from @Temp where BranchId = @BranchId)

	return

end

-- union all
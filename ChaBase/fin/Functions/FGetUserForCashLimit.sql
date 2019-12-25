CREATE FUNCTION [fin].[FGetUserForCashLimit](@BranchId int)

RETURNS  @rtnTable TABLE  (

	UserId int NOT NULL,
	EmployeeId int NOT NULL,
	EmployeeName nvarchar(255) NOT NULL,

	EmployeeNo nvarchar(255) NOT NULL,	

	UserName nvarchar(255) not null
	
	

)

AS

BEGIN



	Declare @DegId int 

	

	Declare @Temp table (UserId int, EmployeeId int,EmployeeNo nvarchar(255), EmployeeName nvarchar(255),UserName nvarchar(255),isTemp bit  )

	Declare @TDate date = (Select TDate from fin.LicenseBranch where BrnhID = @BranchId)

	--set @DegId = (select PValue from lg.ParamValue where PId in (2004,2005))



	insert into @Temp

	 select u.UserId, e.employeeId, e.EmployeeNo,e.EmployeeName,  u.UserName,
	 IsTemp = case when [To] is null then 0 else 1 end  from lg.uservsbranch ub 

		inner join 	lg.[User] u on u.UserId = ub.UserId

		inner join lg.[Employees] e on e.employeeId = u.EmployeeId

		inner join lg.MenuTemplate mt on mt.MenuTemplateId = u.mtid
		
		 where
		 ub.BranchId =@BranchId

		  and
		   mt.DesignationId in (select PValue from lg.ParamValue  where PId in (2005,2006)) and

			u.IsActive = 1 and

			@TDate between [From] and isnull([To], @Tdate) 
			--and e.BranchId=@BranchId

			--select * from @Temp
		

	insert into @Temp

	 select u.UserId, e.EmployeeId, EmployeeNo,EmployeeName,  u.UserName as UserName ,  IsTemp = 0 from 

	 lg.Employees e 

		inner join  

		lg.[user] u on e.employeeId = u.EmployeeId

		inner join

		lg.menuTemplate mt on mt.MenuTemplateId = u.MTId	 

	  where mt.DesignationId in (select PValue from lg.ParamValue where PId in (2005,2006)) and status = 1 and BranchId = @BranchId

		and e.employeeId not in (select EmployeeId from @Temp where isTemp = 0)


		--select * from @Temp
		
		 

		--Declare @UserName nvarchar

		

		--select @UserName =UserName from @Temp 

	--insert into @rtnTable(EmployeeId ,

	--EmployeeName ,

	--EmployeeNo ,

	

	--UserName )

	--select EmployeeId,  EmployeeName ,EmployeeNo, from lg.Employees where EmployeeId in (select EmployeeId from @Temp where BranchId = @BranchId)

		

		insert into @rtnTable(UserId ,
		EmployeeId,
		EmployeeName ,

		EmployeeNo ,	

		UserName )

		select  u.UserId,e.EmployeeId,  EmployeeName ,EmployeeNo, u.UserName  from lg.[User] u

			inner join lg.Employees e on e.employeeId = u.EmployeeId where u.userid in 

		(

		select userId from @Temp 

		)

	

	

	return

end

CREATE Function [LG].[FGetVerifierList](@EventId int)

returns @temp table(UserId int, UserName varchar(100),EmployeeId int,EmployeeName varchar(100),BranchId int)

as

begin

	Declare @MenuId int 

	--account Open

	if @EventId = 1

	begin

		set @MenuId  = 13129

	end

	--if @EventId = 2

	--begin

	--	set @MenuId  = 4005

	--end

	--transaction deposit, interest payment,withdraw,loan payment

	if @EventId = 3 or  @EventId = 6 or @EventId=22 or @EventId = 2

	begin

		set @MenuId  = 9018 

	end

	--manual charge

	if @EventId = 8

	begin

		set @MenuId  = 4007

	end

	--Cheque Registration Verification

	if @EventId = 9

	begin

		set @MenuId  = 4011

	end

	--Cheque Good For Payment Verification

	if @EventId = 13

	begin

		set @MenuId  = 9019

	end

	--Internal Cheque Deposit Verification

	if @EventId = 14

	begin

		set @MenuId  = 9020

	end

	--Remittance Deposit Verification

	if @EventId = 15

	begin

		set @MenuId  = 9021

	end

	--Remittance Payment Verification

	if @EventId = 16

	begin

		set @MenuId  = 9022

	end

	--Loan Registration Verification

	if @EventId = 17

	begin

		set @MenuId  = 6026

	end

	--Loan Account Open After Resistration

	if @EventId = 18

	begin

		set @MenuId  = 6022

	end

	--Loan Account Open Verify

	if @EventId = 19

	begin

		set @MenuId  = 6026

	end

	--Loan Disbusrement

	if @EventId = 21

	begin

		set @MenuId  = 7014

	end

	--if @EventId = 22

	--begin

	--	set @MenuId  = 8011

	--end

	--Share Registration

	if @EventId = 24

	begin

		set @MenuId  = 9014

	end

	--Share Purchase Verification

	if @EventId = 25

	begin

		set @MenuId  = 9016

	end

	--Share Return Verify

	if @EventId=26

	begin

		set @MenuId=9024

	end

	--Cheque Clearence

	if @EventId=27

	begin

		set @MenuId=9043

	end

	--Transaction Edit

	if @EventId=29

	begin

		set @MenuId=10089

	end

	if @EventId = 30

	begin

		set @MenuId  = 11124

	end

	if @EventId=0

	begin

	set @MenuId=null

	end

	insert into @temp

	 select UserId,UserName,e.EmployeeId,EmployeeName,e.BranchId from lg.[User] u 

	 inner join lg.employees e on e.employeeid=u.employeeid

	 where

	 MTId in (select m.TemplateId from lg.MenuVsTemplate m where m.MenuId=@MenuId) 

	 and isactive=1

return 

end
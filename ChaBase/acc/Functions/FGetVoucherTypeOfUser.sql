
  Create Function acc.FGetVoucherTypeOfUser(@FYId int, @UserId int, @BranchId int)
  returns
  table
  as
  return
  (
	select * from acc.VoucherType where VTypeId in (
	 select  distinct VTypeId from acc.voucherno where FYID = @FYId
		and VTypeId in (Select vtypeid from acc.uservsvoucher where userid  = @UserId)
		and CompanyId = @BranchId)
  )
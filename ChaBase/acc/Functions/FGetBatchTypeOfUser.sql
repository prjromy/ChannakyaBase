
CREATE Function [acc].[FGetBatchTypeOfUser](@VTypeId int, @FYId int, @BranchId int, @UserId int)
  returns
  table 
  as
  return
  (
  select * from acc.BatchDescription where BId in (
	 select  distinct BId from acc.voucherno where FYID = @FYId
		and BId in ( Select batchid from acc.uservsvoucher where  vtypeId=@VTypeId -- and userid  = @UserId 
		--Select BId from acc.uservsvoucher a inner join acc.voucherno b on a.BatchId=b.BId where userid  = @UserId and a.vtypeId=@VTypeId
		)
		and CompanyId = @BranchId)
 
--  
  )
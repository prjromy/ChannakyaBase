CREATE procedure [acc].[PUpdateVoucherNoOnFYClosing](@BranchId int)
as
Begin	
	Declare @TDate date=(Select tdate from fin.LicenseBranch where BrnhID=@BranchId)
	Declare @CurrentFYID int=(select fyid from lg.FiscalYears where @TDate between StartDt and EndDt)
	Declare @NewFYId int = @CurrentFYId + 1	

	if not exists(select * from acc.VoucherNo where CompanyId=@BranchId and FYID=@NewFYId)
	Begin
		insert into acc.VoucherNo (BId,CurrentNo,FYID,VTypeId,CompanyId)
		select BId,0 as CurrentNo,@NewFYId as FYID,VTypeId,CompanyId from acc.VoucherNo where fyid=@CurrentFYID 
	End
End
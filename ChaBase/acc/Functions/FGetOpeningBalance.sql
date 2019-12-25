CREATE function [acc].[FGetOpeningBalance](@FID int,@withSubsi bit, @FYId int)
returns  @Tmp1 table (Id int, FId int, LedgerName varchar(500),FYid int,OPBal decimal(18,2),CLBal decimal(18,2))
As 
begin	
	if @FId = 0
	begin
		insert into @Tmp1
		Select isnull(b.Id,0) as Id, a.FId, a.FName as LedgerName, @FYId as FYId, isnull(b.OPbal,0) OPBal, isnull(b.CLBal,0) as CLBal  from acc.FinAcc a 
			left outer join (Select * from acc.VoucherBal where FYId = @FYId )  b on a.FId = b.FId
			where a.FId in (Select FID from FGetLedgerWithType() where IsSubsi = @withSubsi)
	end
	else
	begin
		insert into @Tmp1
		Select isnull(b.Id,0) as Id, a.FId, a.FName as LedgerName, @FYId as FYId, isnull(b.OPbal,0) OPBal, isnull(b.CLBal,0) as CLBal  from acc.FinAcc a 
			left outer join (Select * from acc.VoucherBal where FYId = @FYId )  b on a.FId = b.FId
			where a.FId in (Select FID from FGetLedgerWithType() where IsSubsi = @withSubsi and FId = @FId)
	end
	return
end
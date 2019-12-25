CREATE function [acc].[GetOpeningBalanceData](@FID int,@withSubsi bit, @FYId int)
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
	--IF @withSubsi=1
	--	BEGIN
	--	 insert into @Tmp1
	--	 select ISNULL(b.Id,0) as Id,a.FId,a.FName as LedgerName,ISNULL(b.FYid,0) as FYId,ISNULL(b.opbal,0) as OPBal,ISNULL(b.clbal,0) as CLBal from acc.finacc a left join
	--acc.voucherbal b on a.fid=b.fid
	--left join
	--acc.SubsiVSOpeningBalance c on b.FId=c.FId
	--  where a.F2Type in (select F2Id from acc.finsys2 where F1Id in(
	--select F1Id from acc.finsys1 where isgroup=0  and F1Id not between 24 and 27 and F1Id not between 237 and 249 and 
	--F1Id in (7,15,125,300)
	--)) and a.Fid=case when @FID=0 then a.fid else @FID end
	--END
	--ELSE  
	--   BEGIN
	--   insert into @Tmp1
	--	 select ISNULL(b.Id,0) as Id,a.FId,a.FName as LedgerName,ISNULL(b.FYid,0) as FYId,ISNULL(b.opbal,0) as OPBal,ISNULL(b.clbal,0) as CLBal from acc.finacc a left join
	--acc.voucherbal b on a.fid=b.fid
	--left join
	--acc.SubsiVSOpeningBalance c on b.FId=c.FId
	--  where a.F2Type in (select F2Id from acc.finsys2 where F1Id in(
	--select F1Id from acc.finsys1 where isgroup=0  and F1Id not between 24 and 27 and F1Id not between 237 and 249 and 
	--F1Id not in (7,15,126,300)
	--)) and a.Fid=case when @FID=0 then a.fid else @FID end
	--END
	return
end
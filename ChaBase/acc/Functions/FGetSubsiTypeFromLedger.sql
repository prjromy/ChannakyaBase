--select * from acc.FgetSubsiStmnt(15582,12,4,getdate()-500,getdate(),1,10)



--select * from acc.finacc where FId=15582
--select   [acc].[GenerateSubsiTblIdFromFID](15582)
--select * from acc.voucher2 a inner join acc.voucher3 b on a.v2id=b.v2id where a.fid=15582

--select * from acc.subsitable a where a.STBId=(select   [acc].[GenerateSubsiTblIdFromFID](15582))



CREATE function [acc].[FGetSubsiTypeFromLedger](@FID int)
returns @Tmp table(Id int,Name varchar(200))
as 
begin
 Declare @STBid int 
 Declare @Id int
 Declare @Name varchar(200)
 
  set @STBid=(select a.STBId from acc.subsitable a where a.STBId = [acc].[GenerateSubsiTblIdFromFID](@FID))

  if(@STBid=1)
		 begin
			insert into @Tmp(Id,Name) select EmployeeId,EmployeeName from Lg.employees where EmployeeId in (select b.sid from acc.voucher2 a inner join acc.voucher3 b on a.v2id=b.v2id where a.fid=@FID)
			
			end
  else if(@STBid=2)
		 begin
			insert into @Tmp(Id,Name) select UserId,UserName from LG.[User] where UserId in (select b.sid from acc.voucher2 a inner join acc.voucher3 b on a.v2id=b.v2id where a.fid=@FID)
		 end
  else if(@STBid=3 or @STBid=4 or @STBid=5)
      begin
			--insert into @Tmp(id,Name) select CID, Name from cust.FGetCustList()
			--where CId in (select b.sid from acc.voucher2 a inner join acc.voucher3 b on a.v2id=b.v2id where a.fid=@ID)
			insert into @Tmp(id,name) Select sdid as id, name from SubsiDetail a inner join cust.FGetCustList() b on a.CId = b.CID
				where FId = @FID
		 end
return 
		 end


		 


		
  
  --select * from fin.LicenseBranch
  --update fin.LicenseBranch set fyid=1 where BrnhID=1
  --select * from [acc].[FGetSubsiTypeFromLedger](382)
  --select * from acc.FGetSubsiTableNameByFid(382)
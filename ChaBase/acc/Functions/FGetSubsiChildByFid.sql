
CREATE function [acc].[FGetSubsiChildByFid](@fid int,@FYID int,@BranchID int,@PageNo int,@PageSize int,@AccNo varchar(50),@AccountName varchar(50))
RETURNS @Tmp1 TABLE ( Id int,Accno varchar(60),Name varchar(150),OpeningBalance decimal,TotalCount int)
AS
begin
	Declare @TableID int
	Declare @STBId int
	Declare @MigDate smalldatetime

	Set @MigDate=(select TDate from fin.LicenseBranch where BrnhID=@BranchID)
	Set @TableID=(select c.F1id from acc.FinAcc a 
			inner join acc.FinSys2 b on a.F2Type = b.F2id 
			inner join acc.FinSys1 c on c.F1id = b.F1id where c.F1id in (15,7,126,300)
			and a.Fid=@FID)

	if(@TableID=7)
	begin
		set @STBId=(select b.STBId from acc.SubsiVSFId a inner join acc.SubsiSetup b on a.SSId=b.SSId where Fid = @fid)

		if(@STBId=1)
			Insert into @Tmp1 
				select * from	( select a.SDID as Id, a.AccNo as Accno, b.EmployeeName as Name ,ISNULL (c.OpeningBal,0) as OpeningBalance from SubsiDetail a 
			    inner join lg.GetEmployeeDetails() b on a.CId  = b.EmployeeId
				left outer join (Select * from acc.SubsiVSOpeningBalance where fid = @fid ) c on c.SubsiId = a.SDID where a.FId = @fid and b.EmployeeName like '%'+@AccountName+'%' and a.AccNo like '%'+@AccNo+'%' and a.PostedOn<@MigDate) 
				q cross join(select count(*) as TotalCount from SubsiDetail a 
			    inner join lg.GetEmployeeDetails() b on a.CId  = b.EmployeeId
				left outer join (Select * from acc.SubsiVSOpeningBalance where fid = @fid) c on c.SubsiId = a.SDID
				where a.FId = @fid and b.EmployeeName like '%'+@AccountName+'%' and a.AccNo like '%'+@AccNo+'%' and a.PostedOn<@MigDate) f order by Id OFFSET(@PageNo-1)*@PageSize ROWS FETCH NEXT @PageSize ROWS ONLY
	
		if(@STBId=2)
			Insert into @Tmp1 
				select * from(select a.SDID as Id, a.AccNo as Accno, b.EmployeeName as Name ,ISNULL( c.OpeningBal,0) as OpeningBalance from SubsiDetail a
		        inner join lg.[user] u on u.UserId = a.SDID
				inner join lg.GetEmployeeDetails() b on u.EmployeeId  = b.EmployeeId
				left outer join (Select * from acc.SubsiVSOpeningBalance where fid = @fid) c on c.SubsiId = a.SDID
				where a.FId = @fid and b.EmployeeName like '%'+@AccountName+'%' and a.AccNo like '%'+@AccNo+'%' and a.PostedOn < @MigDate) q cross join(select  count(*) as TotalCount from SubsiDetail a
			    inner join lg.[user] u on u.UserId = a.SDID
				inner join lg.GetEmployeeDetails() b on u.EmployeeId  = b.EmployeeId
				left outer join (Select * from acc.SubsiVSOpeningBalance where fid = @fid) c on c.SubsiId = a.SDID
				where a.FId = @fid and b.EmployeeName like '%'+@AccountName+'%' and a.AccNo like '%'+@AccNo+'%' and a.PostedOn < @MigDate) f order by Id OFFSET(@PageNo - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY


		if(@STBId=3) -- customer
			insert into @Tmp1 
				select  sd.SDID as Id, sd.AccNo , cu.Name,  isnull(op.OpeningBal,0) as OpeningBalance,
					TotalCount =(Select count(sdid) from acc.SubsiDetail sd_1 inner join cust.FGetCustList() cu_1 on cu_1.CID = sd_1.CId  
					where FId = @FId and PostedOn < @MigDate and cu_1.Name like '%' + @AccountName + '%' and sd_1.AccNo  like '%' + @AccNo + '%')
				from cust.FGetCustList() cu inner join acc.SubsiDetail sd on sd.CId = cu.CID
				left join acc.SubsiVSOpeningBalance op on op.SubsiId = sd.SDID and op.FId = @FId
				where sd.FId = @FId and sd.PostedOn < @MigDate and cu.Name like '%' + @AccountName + '%' and sd.AccNo  like '%' + @AccNo + '%'
				order by sd.AccNo OFFSET(@PageNo - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY
		end
		else
		begin
			insert into @Tmp1 
				Select * from( select ad.IAccno as Id, ad.Accno, ad.Aname as Name , isnull(bl.OpeningBal,0)as OpeningBalance from fin.ADetail ad left outer join
				(select * from acc.SubsiVSOpeningBalance where fyid  = @FYID)
				bl on ad.IAccno = bl.SubsiId where ad.PID = (select PID from fin.ProductDetail where fid = @fid) and  ad.Aname like '%'+@AccountName+'%' and ad.Accno like'%'+@AccNo+'%' and ad.RDate<@MigDate) q cross join(select count(*)as TotalCount from fin.ADetail ad left outer join
				(select * from acc.SubsiVSOpeningBalance where fyid  = @FYID)
				bl on ad.IAccno = bl.SubsiId where ad.PID = (select PID from fin.ProductDetail where fid = @fid) and  ad.Aname like '%'+@AccountName+'%' and ad.Accno like'%'+@AccNo+'%' and ad.RDate<@MigDate) a 
				order by Id OFFSET(@PageNo - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY
		end
return
end
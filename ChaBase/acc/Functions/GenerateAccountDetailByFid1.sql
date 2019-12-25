CREATE function [acc].[GenerateAccountDetailByFid1](@fid int,@FYID int,@PageNo int,@PageSize int,@AccNo varchar(50),@AccountName varchar(50))
RETURNS @Tmp1 TABLE ( TotalCount int ,Id int,Accno varchar(60),Name varchar(50),OpeningBalance decimal)
AS
begin
Declare @TableID int
  set @TableID=(select STBId from acc.FGetSubsiTableNameByFid(@fid))
--a.Fid,a.Fname,c.IAccno,c.Accno,c.Aname,ISNULL(d.OpeningBal,0) as OpeningBal
--if(@TableID=1)--Employee)
--		insert into @Tmp1 select * from( select ad.EmployeeId as Id, ad.EmployeeNo as Accno, ad.EmployeeName as Name , isnull(bl.OpeningBal,0)as OpeningBalance from lg.Employees ad left outer join
--		(select * from acc.SubsiVSOpeningBalance where fyid  = @FYID)
--		bl on ad.EmployeeId = bl.SubsiId where ad.PID = (select PID from fin.ProductDetail where fid = @fid) and  ad.EmployeeName like '%'+@AccountName+'%' and ad.EmployeeNo like'%'+@AccNo+'%') q cross join(select count(*)as TotalCount from  lg.Employees ad left outer join
--		(select * from acc.SubsiVSOpeningBalance where fyid  = @FYID)
--		bl on ad.EmployeeId = bl.SubsiId where ad.PID = (select PID from fin.ProductDetail where fid = @fid) and  ad.EmployeeName like '%'+@AccountName+'%' and ad.EmployeeNo like'%'+@AccNo+'%') a
--		order by Id OFFSET(@PageNo - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY
--if(@TableID=2)--user)
--         insert into @Tmp1 select * from( select ad.UserId as Id, ad.Accno, ad.UserName as Name , isnull(bl.OpeningBal,0)as OpeningBalance from [LG].[User] ad left outer join
--		 (select * from acc.SubsiVSOpeningBalance where fyid  = @FYID)
--		 bl on ad.UserId = bl.SubsiId where ad.PID = (select PID from fin.ProductDetail where fid = @fid) and  ad.Aname like '%'+@AccountName+'%' and ad.Accno like'%'+@AccNo+'%') q cross join(select count(*)as TotalCount from fin.ADetail ad left outer join
--		 (select * from acc.SubsiVSOpeningBalance where fyid  = @FYID)
--		 bl on ad.IAccno = bl.SubsiId where ad.PID = (select PID from fin.ProductDetail where fid = @fid) and  ad.Aname like '%'+@AccountName+'%' and ad.Accno like'%'+@AccNo+'%') a
--		 order by Id OFFSET(@PageNo - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY
--if(@TableID=3)--customer)
--          insert into @Tmp1 select * from( select ad.IAccno as Id, ad.Accno, ad.Aname as Name , isnull(bl.OpeningBal,0)as OpeningBalance from fin.ADetail ad left outer join
--          (select * from acc.SubsiVSOpeningBalance where fyid  = @FYID)
--		  bl on ad.IAccno = bl.SubsiId where ad.PID = (select PID from fin.ProductDetail where fid = @fid) and  ad.Aname like '%'+@AccountName+'%' and ad.Accno like'%'+@AccNo+'%') q cross join(select count(*)as TotalCount from fin.ADetail ad left outer join
--          (select * from acc.SubsiVSOpeningBalance where fyid  = @FYID)
--		  bl on ad.IAccno = bl.SubsiId where ad.PID = (select PID from fin.ProductDetail where fid = @fid) and  ad.Aname like '%'+@AccountName+'%' and ad.Accno like'%'+@AccNo+'%') a
--		  order by Id OFFSET(@PageNo - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY
if(@TableID=6 or @TableID= 7 or @TableID=8)
           insert into @Tmp1 select * from( select ad.IAccno as Id, ad.Accno, ad.Aname as Name , isnull(bl.OpeningBal,0)as OpeningBalance from fin.ADetail ad left outer join
         (select * from acc.SubsiVSOpeningBalance where fyid  = @FYID)
		  bl on ad.IAccno = bl.SubsiId where ad.PID = (select PID from fin.ProductDetail where fid = @fid) and  ad.Aname like '%'+@AccountName+'%' and ad.Accno like'%'+@AccNo+'%') q cross join(select count(*)as TotalCount from fin.ADetail ad left outer join
         (select * from acc.SubsiVSOpeningBalance where fyid  = @FYID)
		  bl on ad.IAccno = bl.SubsiId where ad.PID = (select PID from fin.ProductDetail where fid = @fid) and  ad.Aname like '%'+@AccountName+'%' and ad.Accno like'%'+@AccNo+'%') a
		  order by Id OFFSET(@PageNo - 1) * @PageSize ROWS FETCH NEXT @PageSize ROWS ONLY

return
end
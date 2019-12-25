
CREATE function [acc].[FGetLedgerDetailList](@PageNo int,@PageSize int,@query nvarchar(max),@Fid int, @Bank int)
 returns @temp table(Fid int,Fname nvarchar(max),Code nvarchar(max), totalCount int)
  as 
 begin 
 Declare @DAllID int
 Declare @count int=0
 if(@Fid=0)
		 begin
		set @DAllID=null
		 end
else
		 begin
		set @DAllID=@Fid
		 end

 declare @id int,@Fname nvarchar(max),@Code nvarchar(max)
 if(@Bank=0)
		 begin
		 select @count=count(Fid) from acc.FinAcc a inner join acc.FinSys2 b on a.f2type=b.F2id inner join acc.FinSys1 c on b.F1id=c.F1id where a.Fid=coalesce(@DAllID,a.fid) and fname like '%'+@query+'%' and c.isgroup=0
		 end
 else
		 begin
		 select @count=count(Fid) from acc.FinAcc a inner join acc.FinSys2 b on a.F2Type=b.F2id inner join acc.FinSys1 c on b.F1id=c.F1id where a.Fid=coalesce(@DAllID,a.fid) and fname like '%'+@query+'%' and c.f1id=@Bank and c.isgroup=0
		 end


if(@Bank=0) 
begin
		DECLARE V2 CURSOR FOR 
		select Fid,Fname,Code from acc.FinAcc a inner join acc.FinSys2 b on a.F2Type=b.F2id inner join acc.FinSys1 c on b.F1id=c.F1id where a.Fid=coalesce(@DAllID,a.fid) and fname like '%'+@query+'%' and c.isgroup=0
		  ORDER BY Fname  OFFSET(@PageNo - 1) * @PageSize ROWS FETCH NEXT
							@PageSize ROWS ONLY

		OPEN V2 
			FETCH FROM V2 INTO @id ,@Fname,@Code
			WHILE @@FETCH_STATUS=0
			BEGIN
		   insert into @temp
		   values( @id ,@Fname,@Code,@count )
			FETCH NEXT FROM V2 INTO   @id ,@Fname,@Code
			END
			CLOSE V2
			DEALLOCATE V2
			return
			end

else
		begin 
		DECLARE V2 CURSOR FOR 
		select Fid,Fname,Code from acc.FinAcc a inner join acc.FinSys2 b on a.F2Type=b.F2id inner join acc.FinSys1 c on b.F1id=c.F1id where a.Fid=coalesce(@DAllID,a.fid) and fname like '%'+@query+'%' and c.f1id=@Bank and c.isgroup=0
		  ORDER BY Fname OFFSET(@PageNo - 1) * @PageSize ROWS FETCH NEXT
							@PageSize ROWS ONLY

		OPEN V2 
			FETCH FROM V2 INTO @id ,@Fname,@Code
			WHILE @@FETCH_STATUS=0
			BEGIN
				   insert into @temp
				   values( @id ,@Fname,@Code,@count )
					FETCH NEXT FROM V2 INTO   @id ,@Fname,@Code
			END
			CLOSE V2
			DEALLOCATE V2
			return
			end
	return
end
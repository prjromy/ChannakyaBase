
create function [acc].[FGetMapSubsiList](@PageNo int,@PageSize int,@Fid nvarchar(max),@brnhid int)
 returns @temp table(SSId int,CId int,FId int,AccNo nvarchar(50),Status bit,Enable bit,DebitLimit numeric(18,2),CreditLimit numeric(18,2),PostedBy int,Postedon datetime,BranchId int, totalCount int)
  as 
 begin 
 Declare @DAllID int
 Declare @count int=0
 if(@brnhid=0)
		 begin
		set @DAllID=null
		 end
else
		 begin
		set @DAllID=@brnhid
		 end

 declare @SSId int,@CId int,@FId1 int,@AccNo nvarchar(50),@Status bit,@Enable bit,@DebitLimit numeric(18,2),@CreditLimit numeric(18,2),@PostedBy int,@Postedon datetime,@BranchId int
 select @count=count(sdid) from acc.subsidetail where FId in(select value from dbo.FNSplit(@fid,',')) and BranchId=@DAllID
DECLARE V2 CURSOR FOR 
select * from acc.subsidetail where FId in(select value from dbo.FNSplit(@fid,',')) and BranchId=@DAllID
  ORDER BY sdid desc OFFSET(@PageNo - 1) * @PageSize ROWS FETCH NEXT
                    @PageSize ROWS ONLY

OPEN V2 
	FETCH FROM V2 INTO @SSId ,@CId ,@FId1 ,@AccNo ,@Status,@Enable,@DebitLimit ,@CreditLimit,@PostedBy ,@Postedon ,@BranchId 
	WHILE @@FETCH_STATUS=0
	BEGIN
   insert into @temp
   values( @SSId ,@CId ,@FId1 ,@AccNo ,@Status,@Enable,@DebitLimit ,@CreditLimit,@PostedBy ,@Postedon ,@BranchId ,@count )
	FETCH NEXT FROM V2 INTO   @SSId ,@CId ,@FId1 ,@AccNo ,@Status,@Enable,@DebitLimit ,@CreditLimit,@PostedBy ,@Postedon ,@BranchId 
	END
	CLOSE V2
	DEALLOCATE V2
	return
	end
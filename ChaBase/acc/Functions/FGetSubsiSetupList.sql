
create function [acc].[FGetSubsiSetupList](@PageNo int,@PageSize int,@query nvarchar(max))
 returns @temp table(SSId int,STBId int,Title nvarchar(max),Prefix nvarchar(max),Length int,CurrentNo int,totalCount int)
  as 
 begin 
 Declare @count int=0
 declare @sid int,@STID int,@title1 nvarchar(max),@prefix1 nvarchar(max),@length1 int,@currenNo1 int
 select @count=count(SSId) from acc.SubsiSetup where Title like '%'+@query+'%'
 
DECLARE V2 CURSOR FOR 
select * from acc.SubsiSetup where Title like '%'+@query+'%'
  ORDER BY SSId desc OFFSET(@PageNo - 1) * @PageSize ROWS FETCH NEXT
                    @PageSize ROWS ONLY

OPEN V2 
	FETCH FROM V2 INTO @sid ,@STID ,@title1 ,@prefix1 ,@length1 ,@currenNo1 
	WHILE @@FETCH_STATUS=0
	BEGIN
   insert into @temp
   values( @sid ,@STID ,@title1 ,@prefix1 ,@length1 ,@currenNo1  ,@count )
	FETCH NEXT FROM V2 INTO @sid ,@STID ,@title1 ,@prefix1 ,@length1 ,@currenNo1 
	END
	CLOSE V2
	DEALLOCATE V2
	return
	end
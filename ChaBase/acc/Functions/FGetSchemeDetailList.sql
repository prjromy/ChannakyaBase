
CREATE function [acc].[FGetSchemeDetailList](@PageNo int,@PageSize int,@query nvarchar(max))
 returns @temp table(SDID tinyint,FID int, SDName nvarchar(max),SType tinyint,totalCount int)
  as 
 begin 
 Declare @count int=0
 declare @sid int,@LedID int,@SName nvarchar(max),@type tinyint
 select @count=count(SDID) from fin.SchmDetails where SDName like '%'+@query+'%'
 
DECLARE V2 CURSOR FOR 
select SDID ,FID ,SDName,SType from fin.SchmDetails where SDName like '%'+@query+'%'
  ORDER BY SDID desc OFFSET(@PageNo - 1) * @PageSize ROWS FETCH NEXT
                    @PageSize ROWS ONLY

OPEN V2 
	FETCH FROM V2 INTO @sid ,@LedID,@SName,@type
	WHILE @@FETCH_STATUS=0
	BEGIN
   insert into @temp
   values( @sid  ,@LedID,@SName,@type ,@count )
	FETCH NEXT FROM V2 INTO @sid  ,@LedID,@SName,@type
	END
	CLOSE V2
	DEALLOCATE V2
	return
	end
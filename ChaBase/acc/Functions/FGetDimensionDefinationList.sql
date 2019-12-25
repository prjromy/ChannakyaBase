CREATE function [acc].[FGetDimensionDefinationList](@PageNo int,@PageSize int,@query nvarchar(max))
 returns @temp table(DDid int,DefName nvarchar(max),IsManual tinyint,Tid int,totalCount int)
  as 
 begin 
 Declare @count int=0
 declare @ddid int,@defName nvarchar(max),@ismanual tinyint,@tid int
 select @count=count(ddid) from acc.dimensiondefination where DefName like '%'+@query+'%'
 
DECLARE V2 CURSOR FOR 
select DDid ,DefName ,IsManual ,TId from acc.dimensiondefination where DefName like '%'+@query+'%'
  ORDER BY DDId OFFSET(@PageNo - 1) * @PageSize ROWS FETCH NEXT
                    @PageSize ROWS ONLY

OPEN V2 
	FETCH FROM V2 INTO @ddid ,@defName ,@ismanual,@tid 
	WHILE @@FETCH_STATUS=0
	BEGIN
   insert into @temp
   values( @ddid ,@defName ,@ismanual,@tid ,@count )
	FETCH NEXT FROM V2 INTO  @ddid ,@defName ,@ismanual,@tid
	END
	CLOSE V2
	DEALLOCATE V2
	return
	end
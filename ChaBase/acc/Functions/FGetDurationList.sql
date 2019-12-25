
CREATE function [acc].[FGetDurationList](@PageNo int,@PageSize int,@query nvarchar(max))
 returns @temp table(Id int,Duration1 nvarchar(max),Value float,totalCount int)
  as 
 begin 
 Declare @count int=0
 declare @Id int,@Duration nvarchar(max),@Value float
 select @count=count(Id) from fin.Duration where Duration like '%'+@query+'%'
 
DECLARE V2 CURSOR FOR 
select * from fin.Duration where Duration like '%'+@query+'%'
  ORDER BY id desc OFFSET(@PageNo - 1) * @PageSize ROWS FETCH NEXT
                    @PageSize ROWS ONLY

OPEN V2 
	FETCH FROM V2 INTO   @Id ,@Duration ,@Value
	WHILE @@FETCH_STATUS=0
	BEGIN
   insert into @temp
   values(  @Id ,@Duration ,@Value,@count )
	FETCH NEXT FROM V2 INTO @Id ,@Duration ,@Value
	END
	CLOSE V2
	DEALLOCATE V2
	return
	end

CREATE function [acc].[FGetDimensionValueList](@PageNo int,@PageSize int,@query nvarchar(max),@DId int)
 returns @temp table(DVId int,DDId int,DimensionValue1 nvarchar(max),CodeNo nvarchar(max), totalCount int)
  as 
 begin 
 Declare @DAllID int
 Declare @count int=0
 if(@DId=0)
		 begin
		set @DAllID=null
		 end
else
		 begin
		set @DAllID=@DId
		 end

 declare @Dvid int,@id int,@DValue nvarchar(max),@CNo nvarchar(max)
 select @count=count(DVId) from acc.dimensionvalue dv inner join acc.DimensionDefination dd on dv.DDId=dd.DDId  where  dv.DDId=coalesce(@DAllID,dv.DDId) and IsManual=0 and DimensionValue like '%'+@query+'%'
 
DECLARE V2 CURSOR FOR 
select DVId , dv.DDId ,DimensionValue ,CodeNo from acc.dimensionvalue dv inner join acc.DimensionDefination dd on dv.DDId=dd.DDId  where  dv.DDId=coalesce(@DAllID,dv.DDId) and IsManual=0 and DimensionValue like '%'+@query+'%'
  ORDER BY dv.DDId desc OFFSET(@PageNo - 1) * @PageSize ROWS FETCH NEXT
                    @PageSize ROWS ONLY

OPEN V2 
	FETCH FROM V2 INTO @Dvid ,@id ,@DValue,@CNo 
	WHILE @@FETCH_STATUS=0
	BEGIN
   insert into @temp
   values( @Dvid ,@id ,@DValue,@CNo,@count )
	FETCH NEXT FROM V2 INTO   @Dvid ,@id ,@DValue,@CNo 
	END
	CLOSE V2
	DEALLOCATE V2
	return
	end

CREATE function [acc].[FGetProductDetailList](@PageNo int,@PageSize int,@query nvarchar(max))
 returns @temp table(SDID tinyint,PID int,FID int, PName nvarchar(max),Stype tinyint,totalCount int)
  as 
 begin 
 Declare @count int=0
 declare @sid int,@PrID int,@LedID int,@ProName nvarchar(max),@type tinyint
 select @count=count(PID) from fin.ProductDetail a inner join fin.SchmDetails b on a.SDID=b.SDID where PName like '%'+@query+'%'
 
DECLARE V2 CURSOR FOR 
select a.SDID ,PID ,a.FID ,PName, b.stype from fin.ProductDetail  a inner join fin.SchmDetails b on a.SDID=b.SDID where PName like '%'+@query+'%'
  ORDER BY PID desc OFFSET(@PageNo - 1) * @PageSize ROWS FETCH NEXT
                    @PageSize ROWS ONLY

OPEN V2 
	FETCH FROM V2 INTO @sid ,@PrID ,@LedID,@ProName ,@type
	WHILE @@FETCH_STATUS=0
	BEGIN
   insert into @temp
   values( @sid ,@PrID ,@LedID,@ProName ,@type,@count )
	FETCH NEXT FROM V2 INTO @sid ,@PrID ,@LedID,@ProName,@type
	END
	CLOSE V2
	DEALLOCATE V2
	return
	end

CREATE function [acc].[FGetEmployeeDetailList](@PageNo int,@PageSize int,@query nvarchar(max))
 returns @temp table(EmployeeId int,EmployeeName nvarchar(max),EmployeeNo nvarchar(max), totalCount int)
  as 
 begin 
 Declare @count int=0
 declare @EmployeeId int,@EmployeeNo nvarchar(max),@EmployeeName nvarchar(max)
 select @count=count(EmployeeId) from lg.Employees where  EmployeeName like '%'+@query+'%' 
DECLARE V2 CURSOR FOR 
select EmployeeId,EmployeeName,EmployeeNo from lg.Employees where  EmployeeName like '%'+@query+'%'
  ORDER BY EmployeeId desc OFFSET(@PageNo - 1) * @PageSize ROWS FETCH NEXT
                    @PageSize ROWS ONLY

OPEN V2 
	FETCH FROM V2 INTO @EmployeeId ,@EmployeeName,@EmployeeNo 
	WHILE @@FETCH_STATUS=0
	BEGIN
   insert into @temp
   values( @EmployeeId ,@EmployeeName,@EmployeeNo ,@count )
	FETCH NEXT FROM V2 INTO   @EmployeeId ,@EmployeeName ,@EmployeeNo 
	END
	CLOSE V2
	DEALLOCATE V2
	return
	end
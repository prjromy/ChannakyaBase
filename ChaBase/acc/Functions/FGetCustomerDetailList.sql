
CREATE function [acc].[FGetCustomerDetailList](@PageNo int,@PageSize int,@query nvarchar(max))
 returns @temp table(CID numeric(10,0),Fname nvarchar(200),Mname nvarchar(max),Lname nvarchar(max),Gender tinyint,Occpn smallint,GFatherName nvarchar(max),FatherName nvarchar(max),SpouseName nvarchar(max),occupation nvarchar(max), totalCount int)
  as 
 begin 
 Declare @count int=0
 
 declare @CID numeric(10,0),@Fname nvarchar(200),@Mname nvarchar(max),@Lname nvarchar(max),@Gender tinyint,@Occpn smallint,@GFatherName nvarchar(max),@FatherName nvarchar(max),@SpouseName nvarchar(max),@occupation nvarchar(max)
 select @count=count(cid) from cust.CustIndividual ci inner join cust.OccupationDef od on ci.Occpn=od.Occpn where  fname like '%'+@query+'%' or mname like '%'+@query+'%' or Lname like '%'+@query+'%'
DECLARE V2 CURSOR FOR 
select CID,Fname,Mname,Lname ,Gender,ci.Occpn ,GFatherName,FatherName ,SpouseName,od.occupation from cust.CustIndividual  ci inner join cust.OccupationDef od on ci.Occpn=od.Occpn where fname like '%'+@query+'%' or mname like '%'+@query+'%' or Lname like '%'+@query+'%'
  ORDER BY CID desc OFFSET(@PageNo - 1) * @PageSize ROWS FETCH NEXT
                    @PageSize ROWS ONLY

OPEN V2 
	FETCH FROM V2 INTO @CID,@Fname,@Mname,@Lname ,@Gender,@Occpn ,@GFatherName,@FatherName ,@SpouseName ,@occupation 
	WHILE @@FETCH_STATUS=0
	BEGIN
   insert into @temp
   values( @CID,@Fname,@Mname,@Lname ,@Gender,@Occpn ,@GFatherName,@FatherName ,@SpouseName ,@occupation,  @count )
	FETCH NEXT FROM V2 INTO   @CID,@Fname,@Mname,@Lname ,@Gender,@Occpn ,@GFatherName,@FatherName ,@SpouseName ,@occupation 
	END
	CLOSE V2
	DEALLOCATE V2
	return
	end

CREATE function [acc].[FGetVoucherNoList](@PageNo int,@PageSize int,@query nvarchar(max))
 returns @temp table(VNId int,BType varchar(max),BId int,CurrentNo int,FyName nvarchar(50),VTypeId int,CompanyId int,VoucherName nvarchar(max) ,totalCount int)
  as 
 begin 

 Declare @count int=0
 
 declare @VNId int,@BType varchar(max),@BId int,@CurrentNo int,@FYID nvarchar(50),@VTypeId int,@CompanyId int,@VoucherName nvarchar(max)
 select @count=count(vnid) from acc.voucherno vn inner join acc.vouchertype  vt on vn.VTypeId=vt.VTypeID where vt.vouchername like '%'+@query+'%'
 
DECLARE V2 CURSOR FOR 
select VNId,bd.BatchName,bd.BId ,currentno,fy.FyName,vn.VTypeId,companyid,vt.VoucherName from acc.voucherno vn 
inner join acc.vouchertype  vt on vn.VTypeId=vt.VTypeID inner join acc.BatchDescription  bd on vn.bid=bd.bid  
inner join lg.FiscalYears fy on vn.FYID=fy.FYID where vt.vouchername like '%'+@query+'%'
  ORDER BY vnid desc OFFSET(@PageNo - 1) * @PageSize ROWS FETCH NEXT
                    @PageSize ROWS ONLY

OPEN V2 
	FETCH FROM V2 INTO @VNId ,@BType,@BId ,@CurrentNo ,@FYID ,@VTypeId,@CompanyId,@VoucherName
	WHILE @@FETCH_STATUS=0
	BEGIN
   insert into @temp
   values(  @VNId ,@BType,@BId ,@CurrentNo ,@FYID ,@VTypeId,@CompanyId,@VoucherName,@count )
	FETCH NEXT FROM V2 INTO    @VNId ,@BType,@BId ,@CurrentNo ,@FYID ,@VTypeId,@CompanyId,@VoucherName
	END
	CLOSE V2
	DEALLOCATE V2
	return
	end
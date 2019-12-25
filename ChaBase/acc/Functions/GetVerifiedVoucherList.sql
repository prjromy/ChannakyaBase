create function [acc].[GetVerifiedVoucherList](@SDate date,@EDate date,@BranchId int,@VType int,@PostedBy int,@BatchNo varchar(max),@PageNo int,@PageSize int,@CurrentFYID int)
	returns @temp  table(V1Id int,FYID smallint,TDate datetime,Narration nvarchar(max),CompanyId int null,PostedBy int,UserName nvarchar(max) ,
	PostedOn datetime,Amount money,ApprovedBy  int null,VNo nvarchar(max),ApprovedOn datetime null,totalCount int)
	as begin
	Declare @TotalCount int
	--select VNId from acc.VoucherNo where VTypeId=@VType and bid=@BatchNo and FYID=@CurrentFYID and companyid=@BranchId
	 set @TotalCount=   (select count (v1id) from 
	 
	 (select distinct v1.V1Id from acc.voucher1 v1
			inner join acc.Voucher2 v2 on v1.V1Id=v2.V1id
			inner join acc.VoucherNo vno on v1.VNId=vno.VNId
			inner join acc.BatchDescription bt on vno.BId=bt.BId
			inner join acc.VoucherType vt on vno.VTypeId=vt.VTypeID
			inner join[LG].[User] u on v1.PostedBy=u.UserId 
				where ISNULL(DebitAmount,0)!=0
				and  v1.ApprovedBy IS NOT NULL 
				and convert(date, v1.TDate) between @SDate and @EDate
							 and vno.CompanyId=@BranchId
							 and vno.VTypeID=@VType 
							 and vno.BId in(select  value from dbo._FNSplit(@BatchNo,','))
							 and vno.FYID=@CurrentFYID
							 and v1.PostedBy in( select UserId from lg.[user] where userid=coalesce(case when @PostedBy=0 then null else @PostedBy end,userid))) x

							 --and v1.PostedBy in(case when @PostedBy=0 then (select UserId from lg.[User])else (select @PostedBy) end)
							
			--group by v1.v1Id,vno.FYID , vt.Prefix,v1.TDate,v1.Narration,vno.CompanyId
			--,v1.PostedBy,u.UserName,v1.PostedOn,v2.DebitAmount,v1.ApprovedBy,v1.vno,bt.prefix,vt.prefix, v1.ApprovedOn
			)


	insert into @temp 

		select v1.v1Id as V1Id,vno.FYID as FYID,v1.TDate as TDate ,v1.Narration as Narration ,vno.CompanyId as CompanyId,v1.PostedBy as PostedBy,u.UserName as UserName,
		v1.PostedOn as PostedOn, ISNULL(sum(v2.DebitAmount),0) as Amount,v1.ApprovedBy as ApprovedBy,vt.Prefix+'-'+bt.Prefix+'-'+convert(varchar,v1.vno) as VNo,
		v1.ApprovedOn as ApprovedOn,@TotalCount as totalCount
									from acc.voucher1 v1
									inner join acc.Voucher2 v2 on v1.V1Id=v2.V1id
									inner join acc.VoucherNo vno on v1.VNId=vno.VNId
									inner join acc.BatchDescription bt on vno.BId=bt.BId
									inner join acc.VoucherType vt on vno.VTypeId=vt.VTypeID
									inner join[LG].[User] u on v1.PostedBy=u.UserId 
										where ISNULL(DebitAmount,0)!=0
				and  v1.ApprovedBy IS NOT  NULL 
				and convert(date, v1.TDate) between @SDate and @EDate
							 and vno.CompanyId=@BranchId
							 and vno.VTypeID=@VType 
							 and vno.BId in(select  value from dbo._FNSplit(@BatchNo,','))
							 and vno.FYID=@CurrentFYID
							 and v1.PostedBy in( select UserId from lg.[user] where userid=coalesce(case when @PostedBy=0 then null else @PostedBy end,userid))
										group by v1.v1Id,vno.FYID , vt.Prefix,v1.TDate,v1.Narration,vno.CompanyId
										,v1.PostedBy,u.UserName,v1.PostedOn,v2.DebitAmount,v1.ApprovedBy,v1.vno,bt.prefix,vt.prefix, v1.ApprovedOn
										ORDER BY TDate OFFSET(@PageNo - 1) * @PageSize ROWS FETCH NEXT
												@PageSize ROWS ONLY



			return
		end
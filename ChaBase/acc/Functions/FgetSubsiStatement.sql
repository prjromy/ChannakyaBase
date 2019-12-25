--Select * from acc.SubsiDetail
--Select * from acc.SubsiVSOpeningBalance
--select * from acc.Voucher1
--select * from acc.Voucher2
--select * from acc.Voucher3

--select * from acc.VoucherType
--select * from acc.BatchDescription
--select * from acc.VoucherNo

--Go
--Declare @FDate date = getdate() -365
--Declare @TDate date = getdate() 
--Declare @SDId int =14

CREATE function [acc].[FgetSubsiStatement](@SDId int,@FDate date,@TDate date,@PageSize int,@pageNo int)
returns @Tmp1 table ( V2id int, TrDate datetime,VNo varchar(50),Description varchar(max),DebitAmount money,CreditAmount money,Balance money,TotalDbAmt money,TotalCrAmt money,TotalDbCount int,TotalCrCount int )
as begin
 Declare @OpBal money=0
 Declare @ClBal money=0
 Declare @TotalDBAmt money=0
 Declare @TotalCrAmt money=0
 Declare @TotalDBCount money
 Declare @TotalCrCount money
 Declare @FYId int

Declare @FId int = (Select fid from acc.SubsiDetail where SDID = @SDId)
Declare @BRId int = (Select BranchId from acc.SubsiDetail where SDID = @SDId)


 select @FYId=(select FYID from LG.Fiscalyears where @FDate between StartDt and EndDt )
set @OPBal =isnull((Select OpeningBal from acc.SubsiVSOpeningBalance where SubsiId = @SDId and FId = @FId),0)
--to filter using branchid
set @OPBal =isnull((Select OpeningBal from acc.SubsiVSOpeningBalance a inner join acc.VoucherBal b on a.FId=b.FId where SubsiId = @SDId and a.FId = @FId and a.FYId=@FYId and b.BranchId=@BRId),0)

select @TotalDBAmt =  sum(isnull(c.amount,0)),@TotalDBCount= count(c.amount) from acc.Voucher1 a 
						inner join acc.voucher2 b on a.V1Id = b.V1id 
						inner join acc.Voucher3 c on b.V2Id = c.V2Id 
							inner join acc.VoucherNo vn on vn.VNId = a.VNId
							where b.DebitAmount <> 0 and b.Fid = @FId and c.SId  = @SDId and convert(date,a.TDate) <convert(date, @FDate) and vn.CompanyId  = @BRId

select  @TotalCrAmt =  sum( isnull(c.amount,0)),@TotalCrCount=count(c.amount) from acc.Voucher1 a 
						inner join acc.voucher2 b on a.V1Id = b.V1id 
						inner join acc.Voucher3 c on b.V2Id = c.V2Id 
							inner join acc.VoucherNo vn on vn.VNId = a.VNId
							where b.CreditAmount <> 0 and b.Fid = @FId and c.SId  = @SDId and convert(date,a.TDate) < convert(date, @FDate) and vn.CompanyId = @BRId
		if @pageNo=1
		begin
			SET @OpBal=@OpBal+@TotalDBAmt-@TotalCrAmt
		end
		else
		begin
				declare @SubsiBal money
				set @SubsiBal=(select sum(x.DebitAmount)-sum(x.CreditAmout) from (
						   select isnull(b.DebitAmount,0) as DebitAmount,abs( isnull(b.CreditAmount,0)) as CreditAmout from acc.Voucher1 a 
						   inner join acc.voucher2 b on a.V1Id = b.V1id 
						   inner join acc.Voucher3 c on b.V2Id = c.V2Id 
						   inner join acc.VoucherNo vn on vn.VNId = a.VNId
						   where  b.Fid = @FId and c.SId  = @SDId and convert(date,a.TDate) < convert(date, @FDate) and vn.CompanyId  = @BRId
						   ORDER BY TDate OFFSET(0)  ROWS FETCH NEXT
										@PageSize * (@PageNo-1) ROWS ONLY) x)
--select * from acc.voucher2
			set @OpBal=@OpBal+@SubsiBal
		end


SET @ClBal=@OpBal
BEGIN 
insert into @Tmp1 VALUES(0  ,@FDate ,''  ,'Opening Balance' ,0 ,0,isnull(@ClBal,0),isnull(@TotalDBAmt,0),isnull(@TotalCrAmt,0),@TotalDBCount,@TotalCrCount)
END

declare @V2Id int, @TrDate datetime,@VNo varchar(25),@Description varchar(max),@Debit money,@Credit money
DECLARE V2 CURSOR FOR
select b.V2Id, a.TDate, vt.Prefix + '-' + bd.Prefix + '-'+ convert(varchar, a.vno) as VNo,c.Description, --a.PostedOn, 
	 Debit = case when b.DebitAmount <> 0 then c.Amount else 0 end, Credit  = case when b.CreditAmount <> 0 then c.Amount else 0 end
	  from acc.Voucher1 a 
		inner join acc.voucher2 b on a.V1Id = b.V1id 
		inner join acc.Voucher3 c on b.V2Id = c.V2Id 
		inner join acc.VoucherNo vn on vn.VNId = a.VNId
		inner join acc.BatchDescription bd on bd.BId  = vn.BId
		inner join acc.VoucherType vt on vt.VTypeID = vn.VTypeId
		where b.Fid = @FId and c.SId  = @SDId and convert(date, a.TDate) between convert(date, @FDate) and convert(date, @TDate) and vn.CompanyId  = @BRId
		ORDER BY TDate OFFSET(@pageNo-1)*@PageSize ROWS FETCH NEXT @PageSize ROWS ONLY

  OPEN V2
  FETCH FROM V2 INTO @V2Id,@TrDate ,@VNo ,@Description ,@Debit ,@Credit
  WHILE  @@FETCH_STATUS=0
	BEGIN
	set @ClBal=@ClBal+@Debit-@Credit
	insert into @Tmp1
	values(@V2id,@TrDate,@VNo,@Description,@Debit,@Credit,@ClBal,@TotalDBAmt,@TotalCrAmt,@TotalDBCount,@TotalCrCount)
	FETCH NEXT FROM V2 INTO @V2id ,@TrDate ,@VNo ,@Description ,@Debit ,@Credit
	END
	CLOSE V2
	DEALLOCATE V2
	return
end


--select * from acc.FgetSubsiStatement(14,'7/16/2010 12:00:00 AM','3/3/2018  12:00:00 AM',10,2)
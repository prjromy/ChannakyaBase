CREATE FUNCTION [acc].[FgetCashStmnt](@BrId int,@FDate date,@TDate date,@FYId int,@PageNo int,@PageSize int)
returns  @Tmp1 table (V1id int, V2id int, TrDate datetime,VNo varchar(50),Particulars varchar(max),DebitAmount money,CreditAmount money,Balance money,TotalDbAmt money,TotalCrAmt money,TotalDbCount int,TotalCrCount int )
as
begin

 Declare @OpBal money=0
 Declare @ClBal money=0
 Declare @TotalDBAmt money
 Declare @TotalCrAmt money
 Declare @TotalDBCount money
 Declare @TotalCrCount money
 

 --select @TotalDBAmt  = sum(debitamount), @TotalCrAmt = sum(creditamount) from acc.Voucher2 where Fid in
 -- (select FId from acc.FinAcc a inner join acc.FinSys2 b on a.F2Type=b.F2id where b.f1Id=3 )
  
 select @TotalDBAmt  = sum(debitamount), @TotalCrAmt = sum(creditamount)    from acc.voucher2 a
				      inner join acc.Voucher1 b on a.V1id = b.V1Id
					  inner join acc.VoucherNo c on b.VNId=c.VNId
					  where Fid in (select fid from acc.FinAcc a inner join acc.FinSys2 b on a.F2Type=b.F2id where b.f1Id=3 )
				      and TDate between convert(date, @FDate) and convert(date, @TDate) and c.CompanyId = @BrId and c.FYID = @FYId

 --select @TotalDBCount  = COUNT(debitamount), @TotalCrCount = COUNT(creditamount) from acc.Voucher2 where Fid in 
 --(select FId from acc.FinAcc a inner join acc.FinSys2 b on a.F2Type=b.F2id where b.f1Id=3 )
 
 select @TotalDBCount  = COUNT(debitamount), @TotalCrCount = COUNT(creditamount) from acc.voucher2 a
					inner join acc.Voucher1 b on a.V1id = b.V1Id 
					inner join acc.VoucherNo c on b.VNId=c.VNId
					 where Fid  in (select FId from acc.FinAcc a inner join acc.FinSys2 b on a.F2Type=b.F2id where b.f1Id=3 )
				    and  TDate between convert(date, @FDate) and convert(date, @TDate) and c.CompanyId = @BrId and c.FYID = @FYId

 --Declare @TotalDBAmt money=(select sum(DebitAmount) from acc.Voucher2 where FId=@FiD )
 --Declare @TotalCrAmt money=(select sum(CreditAmount) from acc.Voucher2 where FId=@FiD )
 
 declare @V1Id int ,@V2id int,@TrDate datetime,@VNo varchar(25),@Particulars varchar(max),@Debit money,@Credit money
 
 SET @OpBal = ISNULL(( select sum(OPBal) from acc.voucherbal  where FYId=@FYId and FId in
 (select FId from acc.FinAcc a inner join acc.FinSys2 b on a.F2Type=b.F2id where b.f1Id=3 )
 ),0)

 if @PageNo=1
		 begin
			 SET @OpBal=@OpBal + ISNULL( (select
			   sum(a.DebitAmount)-abs(sum(CreditAmount) ) as Balance from acc.voucher2 a inner join acc.Voucher1 b on a.V1id = b.V1Id
			   inner join acc.VoucherNo c on b.VNId=c.VNId
			   inner join acc.VoucherType d on c.VTypeId=d.VTypeID
			   inner join acc.BatchDescription e on c.Bid=e.BId
			   where a.fid in
			   (select FId from acc.FinAcc a inner join acc.FinSys2 b on a.F2Type=b.F2id where b.f1Id=3 )
				and TDate < convert(date, @FDate) and c.FYId=@FYId and c.companyid=@BrId),0) 
				--yeta FDATE lai TDate banako
		 end
		 else
		 begin 
		 Declare @CashBal money
			set @CashBal= (select sum(x.DebitAmount)-sum(x.CreditAmount) from(
						select isnull(a.DebitAmount,0) as DebitAmount,abs( isnull (a.CreditAmount,0)) as CreditAmount from acc.voucher2 a
							   inner join acc.Voucher1 b on a.V1id = b.V1Id
							   inner join acc.VoucherNo c on b.VNId=c.VNId
							   inner join acc.VoucherType d on c.VTypeId=d.VTypeID
							   inner join acc.BatchDescription e on c.Bid=e.BId
							   where a.fid in
							   (select FId from acc.FinAcc a inner join acc.FinSys2 b on a.F2Type=b.F2id where b.f1Id=3 )
								and TDate <convert(date, @FDate) and c.FYId=@FYId and c.companyid=@BrId
								  ORDER BY TDate OFFSET(0)  ROWS FETCH NEXT
									@PageSize * (@PageNo-1) ROWS ONLY 
									) x
							)
					set @OpBal=@OpBal+@CashBal
		 end
 

   SET @ClBal=@OpBal

   if(@ClBal!=0)
   BEGIN
   insert into @Tmp1
   values(0 ,0 ,@FDate ,''  ,'Opening Balance' ,0 ,0,@ClBal,@TotalDBAmt,@TotalCrAmt,@TotalDBCount,@TotalCrCount )
   END

 --declare @Tmp1 table (V1id int, V2id int, fid int,TDate datetime,PostedOn datetime,VNo varchar(50),Particulars varchar(200),ApprovedBy int,DebitAmount money,CreditAmount money,Balance money );
 DECLARE V2 CURSOR FOR 
	select B.V1id, a.V2Id, b.TDate, d.Prefix+'-'+e.Prefix+'-'+convert(varchar,b.vno)
	as VNo, a.Particulars,
	ISNULL(a.DebitAmount,0) as DebitAmount, abs(ISNULL(a.CreditAmount,0)) as CreditAmount
	from acc.voucher2 a inner join acc.Voucher1 b on a.V1id = b.V1Id
	inner join acc.VoucherNo c on b.VNId=c.VNId
	inner join acc.VoucherType d on c.VTypeId=d.VTypeID
	inner join acc.BatchDescription e on c.Bid=e.BId
	where a.fid in
	(select FId from acc.FinAcc a inner join acc.FinSys2 b on a.F2Type=b.F2id where b.f1Id=3 )
    and TDate between convert(date, @FDate) and convert(date, @TDate) and c.fyid=@FYId and c.CompanyId=@BrId
	ORDER BY TDate OFFSET(@PageNo - 1) * @PageSize ROWS FETCH NEXT
                    @PageSize ROWS ONLY

OPEN V2 
	FETCH FROM V2 INTO @V1Id ,@V2id ,@TrDate ,@VNo ,@Particulars ,@Debit ,@Credit 
	WHILE @@FETCH_STATUS=0
	BEGIN
		set @ClBal=@ClBal+@Debit-@Credit
		--set @TotalDBAmt = (select sum(DebitAmount) from acc.Voucher2 where FId=@FiD )
		--set @TotalCrAmt=(select sum(CreditAmount) from acc.Voucher2 where FId=@FiD )

   insert into @Tmp1
   values(@V1Id ,@V2id ,@TrDate ,@VNo ,@Particulars ,@Debit ,@Credit,@ClBal,@TotalDBAmt,@TotalCrAmt,@TotalDBCount,@TotalCrCount )

	FETCH NEXT FROM V2 INTO @V1Id ,@V2id ,@TrDate ,@VNo ,@Particulars ,@Debit ,@Credit
	END
	CLOSE V2
	DEALLOCATE V2

	--update @Tmp1 set TotalDbAmt=(select top 1 TotalDbAmt from @Tmp1 order by V1id desc)
	--update @Tmp1 set TotalCrAmt=(select top 1 TotalCrAmt from @Tmp1 order by V1id desc)
return 
end
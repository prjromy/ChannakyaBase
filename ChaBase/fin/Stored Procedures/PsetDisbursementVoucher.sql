
CREATE proc [fin].[PsetDisbursementVoucher]
(
@disburseId int,
@ApproveBy int,
@v1id int output

)
as 
begin

declare @iaccno int
	declare @batchId int
	declare @FyId int
	declare @vType int
	declare @Tdate date
	declare @Vnid int
	declare @Vno int
	declare @CompanyId int
	declare @CurrencyId int
	declare @narration varchar(500)
	--declare @V1Id int
	declare @PostedBy int
	declare @PostedOn datetime
	declare @tellerFid int
	declare @particular varchar(500)

	set @tdate=(select Tdate from fin.disbursementMain where disburseId=@disburseId)
	select @tdate=tdate,@postedOn=postedon,@postedby=postedby,@iaccno=iaccno from fin.disbursementMain where disburseId=@disburseId

	set @batchId=(select Pvalue from lg.ParamValue where pid=7011)
	set @vType=(select pvalue from lg.ParamValue where pid=7012)
	set @CurrencyId=(select Pvalue from lg.ParamValue where pid=7013)
	set @tellerFid=(select Fid from acc.FinAcc where F2Type=86)
	set @FyId=(select fyid from lg.FiscalYears where @tdate between startdt and enddt)
	set @companyId=(select brchid from fin.adetail where iaccno=@iaccno)
	set @particular=(select aname from fin.adetail where iaccno=@iaccno)
	set @narration='Being Loan Disbursement to '+@particular

	set @vnid=isnull((select vnid from acc.voucherno where fyid=@fyid and companyId=@companyId and bid=@batchId and vtypeId=@vtype),0)
	if(@Vnid=0)
	begin
		raiserror ('Error, Voucher and batch not declare',16,1)
	end
	update acc.voucherNo set currentNo=isnull(currentNo,0)+1 where vnid=@vnid


	set @vno=(select CurrentNo from acc.voucherNo where  vnid=@vnid)
	
	insert into acc.voucher1(vno,tdate,approvedby,approvedon,postedBy,postedon,ctid,vnid,Narration) values(@vno,@tdate,@ApproveBy,getdate(),@postedby,@postedon,@currencyId,@vnid,@narration)
	set @v1id=@@identity
	
	insert into acc.voucher2(fid,v1id,particulars,debitamount,creditamount) 
		select fid,@v1id,'',amount,0 from fin.DisburseVsPID where disburseId=@disburseId



    if exists(select disburseId from fin.disburseDeposit where disburseId=@disburseId)
	begin
		insert into acc.voucher2(fid,v1id,particulars,debitamount,creditamount) 
		select fid,@v1id as v1id,@particular as particulars,0,sum(d.amount) as creditamount from fin.DisburseDeposit d 
			inner join fin.adetail  a on a.iaccno=d.depositiaccno 
			inner join fin.productdetail p on p.pid=a.pid
			where d.disburseId=@disburseId
			group by fid
	end
	if exists(select disburseId from fin.DisburseCheque  where disburseId=@disburseId)
	begin
		insert into acc.voucher2(fid,v1id,particulars,debitamount,creditamount) 
		select b.Fid ,@V1Id as v1id,@particular as particulars,0 as debitamount,dc.amount as creditamount from fin.DisburseCheque dc 
			inner join acc.BankInfo b on b.bid=dc.bankid where DisburseId=@disburseId

	end
	if exists(select disburseId from fin.disbursecash  where disburseId=@disburseId)
	begin
		insert into acc.voucher2(fid,v1id,particulars,debitamount,creditamount) 
		select @tellerFid as fid,@v1id as v1id,@particular as particulars ,0 as debitamount ,Amount as creditamount from fin.DisburseCash where DisburseId=@disburseId
	end
	if exists(select disburseId from fin.DisburseCharge  where disburseId=@disburseId)
	begin
	 
		insert into acc.voucher2(fid,v1id,particulars,debitamount,creditamount) 
		select FID as fid,@v1id as v1id,@particular as particulars ,0 as debitamount ,sum(Amount) as creditamount from fin.DisburseCharge where DisburseId=@disburseId group by fid
	end

	exec acc.PSetOpeningBal @v1id
end
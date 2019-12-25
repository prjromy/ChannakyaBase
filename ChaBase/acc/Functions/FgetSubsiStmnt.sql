
CREATE FUNCTION [acc].[FgetSubsiStmnt](@FiD int,@BrId int,@SID int,@FDate date,@TDate date,@PageNo int,@PageSize int)
returns  @Tmp1 table ( V2id int, TrDate datetime,VNo varchar(50),Description varchar(max),DebitAmount money,CreditAmount money,SubsiName varchar(200),Balance money,TotalDbAmt money,TotalCrAmt money,TotalDbCount int,TotalCrCount int )
as
begin

 Declare @OpBal money=0
 Declare @ClBal money=0
 Declare @TotalDBAmt money=0
 Declare @TotalCrAmt money=0
 Declare @TotalDBCount money
 Declare @TotalCrCount money
 Declare @FYId int

 
 select @FYId=(select FYID from LG.Fiscalyears where @FDate between StartDt and EndDt )
 
   
--CreditAmount = case when ISNULL(a.DebitAmount,0)=0 then f.Amount else 0 end,

 select @TotalDBAmt  = sum(ISNULL(a.debitamount,0)), @TotalCrAmt = sum(ISNULL(a.creditamount,0)) 
 from acc.voucher2 a inner join acc.Voucher1 b on a.V1id = b.V1Id
   inner join acc.voucher3 f on a.v2id=f.V2Id
   inner join acc.VoucherNo c on b.VNId=c.VNId
   inner join acc.VoucherType d on c.VTypeId=d.VTypeID
   inner join acc.BatchDescription e on c.Bid=e.BId
   where a.fid=@FiD
and f.SId=@SID and c.FYID=@FYId
and c.CompanyId=@BrId


 select @TotalDBCount  = COUNT(debitamount), @TotalCrCount = COUNT(creditamount)
  from acc.voucher2 a inner join acc.Voucher1 b on a.V1id = b.V1Id
   inner join acc.voucher3 f on a.v2id=f.V2Id
   inner join acc.VoucherNo c on b.VNId=c.VNId
   inner join acc.VoucherType d on c.VTypeId=d.VTypeID
   inner join acc.BatchDescription e on c.Bid=e.BId
   where a.fid=@FiD
and f.SId=@SID  and c.FYID=@FYId
and c.CompanyId=@BrId
 --Declare @TotalDBAmt money=(select sum(DebitAmount) from acc.Voucher2 where FId=@FiD )
 --Declare @TotalCrAmt money=(select sum(CreditAmount) from acc.Voucher2 where FId=@FiD )
 


 declare @V2Id int, @TrDate datetime,@VNo varchar(25),@Description varchar(max),@Debit money,@Credit money,@SubsiName varchar(200)
 
 SET @OpBal = ISNULL(( select OpeningBal from acc.SubsiVSOpeningBalance  where FYId=@FYId and FId=@FId and SubsiId=@SID),0)

 SET @OpBal=@OpBal + ISNULL((select sum(Debit)-sum(Credit) from 
 (select Debit=case when b.debitamount is not null then c.amount else 0 end,
 Credit=case when b.creditamount is not null then c.amount else 0 end
   from acc.Voucher1 a inner join acc.Voucher2 b on a.V1Id=b.V1id
   inner join acc.voucherno d on a.vnid=d.vnid
 inner join acc.Voucher3 c on b.V2Id=c.V2Id where b.Fid=@FID and c.sid=@SID and d.FYID=@FYId and d.CompanyId=@BrId and a.TDate<@FDate)x),0)

   SET @ClBal=@OpBal

   --if(@ClBal!=0)
   BEGIN
   insert into @Tmp1
   values(0  ,@FDate ,''  ,'Opening Balance' ,0 ,0,'',@ClBal,@TotalDBAmt,@TotalCrAmt,@TotalDBCount,@TotalCrCount )
   END

 --declare @Tmp1 table (V1id int, V2id int, fid int,TDate datetime,PostedOn datetime,VNo varchar(50),Particulars varchar(200),ApprovedBy int,DebitAmount money,CreditAmount money,Balance money );
 DECLARE V2 CURSOR FOR 
select  a.V2Id, b.TDate, d.Prefix+'-'+e.Prefix+'-'+convert(varchar,b.vno)
   as VNo,f.Description,--a.debitamount,a.creditamount,
  -- ISNULL(a.DebitAmount,0) as DebitAmount, abs(ISNULL(a.CreditAmount,0)) as CreditAmount,
  DebitAmount= case when ISNULL(a.CreditAmount,0)=0 then f.Amount else 0 end,
CreditAmount = case when ISNULL(a.DebitAmount,0)=0 then f.Amount else 0 end,
   SubsiName= 
   case when acc.GenerateSubsiTblIdFromFID(a.fid)=1 then 
   (select top 1 EmployeeName from LG.Employees where EmployeeId=@SID)
   when 
   acc.FGetSubsiTblIdFromFId(a.Fid)=2 then 
   (select top 1 username from LG.[User] where UserId=@SID )
   when
   acc.FGetSubsiTblIdFromFId(a.Fid)=3 or acc.FGetSubsiTblIdFromFId(a.Fid)= 4 or
   acc.FGetSubsiTblIdFromFId(a.Fid)=5 then 
   (select top 1 Fname from cust.custindividual where CID=@SID) end 
   from acc.voucher2 a inner join acc.Voucher1 b on a.V1id = b.V1Id
   inner join acc.voucher3 f on a.v2id=f.V2Id
   inner join acc.VoucherNo c on b.VNId=c.VNId
   inner join acc.VoucherType d on c.VTypeId=d.VTypeID
   inner join acc.BatchDescription e on c.Bid=e.BId
   where a.fid=@FiD and f.Sid=@SID
 and TDate between @FDate and @TDate and c.fyid=@fyid and c.companyid=@BrId
   ORDER BY TDate OFFSET(@PageNo - 1) * @PageSize ROWS FETCH NEXT
                    @PageSize ROWS ONLY

OPEN V2 
	FETCH FROM V2 INTO  @V2id ,@TrDate ,@VNo ,@Description ,@Debit ,@Credit,@SubsiName
	WHILE @@FETCH_STATUS=0
	BEGIN
		set @ClBal=@ClBal+@Debit-@Credit
		--set @TotalDBAmt = (select sum(DebitAmount) from acc.Voucher2 where FId=@FiD )
		--set @TotalCrAmt=(select sum(CreditAmount) from acc.Voucher2 where FId=@FiD )

   insert into @Tmp1
   values(@V2id ,@TrDate ,@VNo ,@Description ,@Debit ,@Credit,@SubsiName, @ClBal,@TotalDBAmt,@TotalCrAmt,@TotalDBCount,@TotalCrCount )



	FETCH NEXT FROM V2 INTO @V2id ,@TrDate ,@VNo ,@Description ,@Debit ,@Credit,@SubsiName
	END
	CLOSE V2
	DEALLOCATE V2

	--update @Tmp1 set TotalDbAmt=(select top 1 TotalDbAmt from @Tmp1 order by V1id desc)
	--update @Tmp1 set TotalCrAmt=(select top 1 TotalCrAmt from @Tmp1 order by V1id desc)
return 
end
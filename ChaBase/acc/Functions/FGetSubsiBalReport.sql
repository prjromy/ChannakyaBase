   CREATE function [acc].[FGetSubsiBalReport](@FId int,@TDate date,@BId int,@PageNo int=1,@PageSize int=10)
   returns @Tmp1 table ( Accno varchar(200),SID int, SubsiName varchar(500),OpBal money,DebitAmount money,CreditAmount money,ClBal money)
   as begin
   DECLARE @FYId int
    select @FYId=(select FYID from LG.Fiscalyears where @TDate between StartDt and EndDt )
  insert into @Tmp1 

  select ISNULL(y.AccNo,'') as AccNo,sid as SID,x.subsiName as SubsiName,OpBal=ISNULL(y.OpeningBal,0),DebitAmount=x.DebitAmount
   ,CreditAmount=x.CrditAmount,ClBal=(ISNULL(y.ClosingBal,0)+ISNULL(x.DebitAmount,0)-abs(ISNULL(x.CrditAmount,0))) from(
   select --distinct a.V3id,a.v2id,b.username,a.sid
    b.SId,MAX(b.username) as SubsiName,sum(DebitAmount) as DebitAmount,sum(CreditAmount) as CrditAmount from acc.voucher3 a inner join
   (select a.V3id,a.v2id,b.username,a.sid,--a.Amount,
   DebitAmount= case when ISNULL(c.CreditAmount,0)=0 then a.Amount else 0 end,
   CreditAmount = case when ISNULL(c.DebitAmount,0)=0 then a.Amount else 0 end
    from acc.voucher3 a inner join LG.[User] b on a.sid=b.userid
	inner join acc.voucher2 c on a.V2Id=c.V2Id
	inner join acc.voucher1 f on f.V1id=c.V1id
	inner join acc.voucherno g on f.vnid=g.vnid
	 where a.V2Id in 
   (select b.V2Id from acc.FinAcc a inner join acc.voucher2 b on a.fid=b.fid
   where a.Fid=@FId and g.companyid=@BId and f.TDate>@TDate 
   and g.FYId=@FYID
  ) ) b on a.V3Id=b.V3Id
   group by b.SId) x left  join 
   (
   select distinct d.AccNo,c.SubsiID,a.fname,c.OpeningBal,c.ClosingBal
 from acc.finacc a
inner join acc.VoucherBal b on a.fid=b.fid
 inner join 
acc.SubsiVSOpeningBalance c on b.fid=c.fid 
inner join acc.subsidetail d on c.subsiid=d.cid where a.Fid=@FID and d.fid=@FID and c.fyid=@FYID
) y
on x.sid=y.SubsiId order by Fname OFFSET(@PageNo-1)*@PageSize ROWS FETCH NEXT
			 @PageSize ROWS ONLY


return 
end
--change from dbo.subsidetail to acc.subsidetail
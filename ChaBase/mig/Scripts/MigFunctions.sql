/*ChequeUserReport Start*/
GO
CREATE function mig.chequeUseReport()
returns @temptable table(rno int,chkno int,cstate int,accno int)
as begin
DECLARE @startnum INT
DECLARE @endnum INT
DECLARE @Iaccno INT
DECLARE @cstate INT
DECLARE @Rno INT
 
declare cur cursor for select rno,cstate,cfrom,cto,iaccno from nimbus.dbo.achq
open cur 
fetch next from cur into @Rno,@cstate,@startnum,@endnum,@Iaccno
 
while @@FETCH_STATUS =0
begin

  while @startnum<=@endnum
  begin

  insert into @temptable values(@rno,@startnum,@cstate   ,@Iaccno)
   set @startnum=@startnum+1
 end
fetch next from cur into  @Rno,@cstate,@startnum,@endnum,@Iaccno 
end
close cur
deallocate cur
return;
end

/*ChequeUserReport End*/

/*FgetDuration Start*/
GO
CREATE function mig.FgetDuration()



returns table as return



select  d.*,pd.DurationName,pd.OldDuration from fin.duration d

 join mig.ProductAndDuration() pd on d.duration=pd.DurationName

 /*FgetDuration End*/


  /*FGetUsedChk Start*/

 --Select * From FGetUsedChk() Where IAccNo = 23 And ChqNo = ?

 GO
CREATE FUNCTION mig.FGetUsedChk()
Returns Table
As
Return
SELECT     FIaccno AS 'IAccno', SlpNo AS 'ChqNo',Tno
FROM         nimbus.dbo.IChkDep
Where Ttype In (0,1)
UNION
SELECT     IAccno, SLPNO AS 'ChqNo',tno
FROM          nimbus.dbo.ASTRNS
WHERE     ISSLP = 0 
UNION
SELECT     IAccno, SLPNO AS 'ChqNo',tno
FROM          nimbus.dbo.AMTRNS
WHERE     ISSLP = 0 and dramt!=0
Union 
SELECT IAccno,Chqno,tno FROM  nimbus.dbo.ACHQFGDPYMT WHERE  TSTATE IN(1,2,5,4)
union 
SELECT IAccno,Chkno,0 as tno FROM  nimbus.dbo.ICHKBOUNCE

 /*FGetUsedChk End*/

  /*ProductAndDuration Start*/
  GO
 Create function mig.ProductAndDuration()
returns table as return
Select DurationName= case when Dur< 1 then  convert(varchar, Dur * 1000)  + ' Days' else convert(varchar,Dur) + ' Month(s)' end, Value = case when Dur< 1 then Dur * 1000 else Dur * 30 end,dur as OldDuration from (

select distinct Dur from nimbus.dbo.ADur ) x where x.Dur is not null
union

Select DurationName= case when Duration< 1 then  convert(varchar, duration * 1000)  + ' Days' else convert(varchar,Duration) + ' Month(s)' end, Value = case when Duration< 1 then duration * 1000 else Duration * 30 end,duration as OldDuration from (
select distinct Duration from nimbus.dbo.ProductDetail ) x where x.Duration is not null
 
 /*ProductAndDuration End*/


  /*ProductCalandCapRule Start*/
  GO
 CREATE function mig.ProductCalandCapRule()
returns table as return
Select * from (Select SDName,PName,IAccNo,AccNo,AName,SType,
'MinBal' = Case When Exists (Select * From nimbus.dbo.AMinBal Where IAccNo = ADetail.IAccNo) Then
			(Select MinBal From nimbus.dbo.AMinBal Where IAccNo = ADetail.IAccNo) Else
			ProductDetail.LAmt End,
'PSID' =  Case 
			When Exists
                	(Select * From nimbus.dbo.APolInt Where APolInt.IAccNo = ADetail.IAccNo) Then
                       	(Select nimbus.dbo.PolicyIntCalc.PSID From nimbus.dbo.APolInt 
			Inner Join nimbus.dbo.PolicyIntCalc On nimbus.dbo.APolInt.PSID = nimbus.dbo.PolicyIntCalc.PSID
                        Where  (APolInt.IAccNo = ADetail.IAccNo)) 
			When Exists
                        (SELECT * From nimbus.dbo.ProductPSID Where Pid = ADetail.pid) Then
                        (SELECT nimbus.dbo.PolicyIntCalc.PSID From nimbus.dbo.ProductPSID 
			Inner Join nimbus.dbo.PolicyIntCalc ON nimbus.dbo.ProductPSID.PSID = nimbus.dbo.PolicyIntCalc.PSID 
                        Where (nimbus.dbo.ProductPSID.IsDefault = 1) And (nimbus.dbo.ProductPSID.PID = ADetail.PID) )End,
	'ICBDurID' = Case When Exists
                       	(Select * From nimbus.dbo.AICBDur Where IAccNo = Adetail.IAccNo) Then
                          	(Select RuleICBDuration.ICBDurID From nimbus.dbo.RuleICBDuration 
				Inner Join nimbus.dbo.AICBDur ON RuleICBDuration.ICBDurID = AICBDur.ICBDurID
                        	Where IAccNo = ADETAIL.IAccNo) Else 
			(Select RuleICBDuration.ICBDurID  From nimbus.dbo.ProductICBDur 
			Inner Join nimbus.dbo.RuleICBDuration On nimbus.dbo.ProductICBDur.ICBDurID = nimbus.dbo.RuleICBDuration.ICBDurID 
                        Where (nimbus.dbo.ProductICBDur.IsDefault = 1) AND (nimbus.dbo.ProductICBDur.PID = Adetail.pid)) End
From nimbus.dbo.ADetail
Inner Join nimbus.dbo.ProductDetail On Adetail.PID = ProductDetail.PID
Inner Join nimbus.dbo.SchmDetails On ProductDetail.SDID = SchmDetails.SDID
)X

  /*ProductCalandCapRule End*/
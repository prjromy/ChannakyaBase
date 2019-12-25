CREATE function [fin].[FGetShareVerifiedDetails]( @tno int)

returns table 

as return

(

select s.RegNo,scd.Scrtno, scd.Sid,cast(Tno as int) Tno,s.sqty as Qty,Registrationcode,Name,cast(scd.Stype as int) as ShareType,scd.FSno as [From],FSno+s.SQty as [To],s.Sdate as Rdate,s.Sdate,s.PostedBy,s.Note

 from fin.shrreturn s

 inner join fin.ShrReg r on s.regno=r.regno

 inner join cust.fgetcustname() cs on cs.cid=r.cid inner join fin.scrtdtls scd on scd.sid=s.scrtno where tno=@tno

 )
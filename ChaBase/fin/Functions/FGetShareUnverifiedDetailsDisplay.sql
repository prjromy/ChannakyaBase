create function [fin].[FGetShareUnverifiedDetailsDisplay]( @tno int)

returns table 

as return

(
select s.Regno,scd.Scrtno, scd.Sid,cast(Tno as int) Tno,s.sqty as Qty,Registrationcode,
	Name,cast(scd.Stype as int) as ShareType,scd.FSno as [From],FSno+s.SQty as [To],
	s.Sdate as Rdate,s.Sdate,s.PostedBy,s.Note from [fin].[ShrSReturn] s
	inner join fin.ShrReg r on r.RegNo=s.Regno
	inner join cust.fgetcustname() cs on cs.CID=r.CId
	inner join fin.SCrtDtls scd on scd.SCrtno=s.SCrtNo
	where s.Tno=@tno

 )
CREATE function [fin].[FGetShareReturnVerifyList]()
returns table
as return
(
	select s.RegNo,scd.Scrtno, scd.Sid,cast(Tno as int) Tno,s.sqty as Qty,Registrationcode,Name,s.Stype as ShareType
	 from fin.shrsreturn s 
	 inner join fin.ShrReg r on s.regno=r.regno 
	 inner join cust.fgetcustname() cs on cs.cid=r.cid 
	 inner join fin.scrtdtls scd on scd.Sid=s.scrtno	 
)
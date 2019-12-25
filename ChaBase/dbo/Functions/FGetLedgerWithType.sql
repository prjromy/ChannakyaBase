CREATE Function FGetLedgerWithType()
returns
table
as
return
(


	Select f.FId, f.FName, f.PId as ParentId,p.fName as ParentName, f2.F2Id, f1.F1Id, IsSubsi= 0 from acc.finsys1 f1 
		inner join acc.Finsys2 f2 on f1.f1id = f2.f1id 
		inner join acc.finacc f on f.f2type = f2.f2id 
		left outer join acc.Finacc p on p.fid = f.pid 				 
	where isgroup <>1 and f1.f1id in (2, 3, 5, 8, 9, 237,239)
	union all
	Select f.FId, f.FName, f.PId,p.fName as ParentName, f2.F2Id, f1.F1Id, IsSubsi= 1 from acc.finsys1 f1 
		inner join acc.Finsys2 f2 on f1.f1id = f2.f1id 
		inner join acc.finacc f on f.f2type = f2.f2id 
		left outer join acc.Finacc p on p.fid = f.pid 				 
	where isgroup <>1 and f1.f1id in (7, 15, 126, 300)
)
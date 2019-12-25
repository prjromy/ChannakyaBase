
CREATE function [dbo].[GetTotalBalanceOfLedger]
(
@rootId int,@FYId int,@brnhId int
)
  RETURNS  numeric(18,2)
AS 
BEGIN 
--declare  @TotalBalance numeric(18,2)
declare @Tmp table (FId int, PId int, FName varchar(500));

with name_tree as 
(
  select FId,Pid,Fname from acc.finacc a  where Fid=@RootId
   union all
   select C.FId, C.PID, c.Fname from acc.FinAcc c
   join name_tree p on p.fid = c.Pid --where p.Fid=1
   AND C.Fid<>C.Pid 
)
insert into @Tmp
select * from name_tree
--insert into @TempTbl ]

return (select sum(CLBal) from (
SELECT  t.Fid,g.Fname,sum(t.CLBal) as CLBal from acc.voucherbal t left outer join acc.FinAcc g on g.Fid = t.Fid
		where 
		t.FYId=@FYId AND
		t.BranchId=@brnhId and
		 g.Fid in (select Fid from @Tmp) 
		group by t.Fid,g.Fname)x)
--return
end
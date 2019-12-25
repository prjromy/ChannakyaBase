create function mig.chequeUseReport()
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

CREATE function [dbo].[FGetAsLoanAmount] (@Tno int)
returns @Temp table (Pid   int,
					 amount   money
					 )


As 
begin


declare @ionpa   money
declare @ionia   money
declare @ioncr   money
declare @ponpra  money
declare @ponia   money
declare @ionpr   money
declare @ionir   money
declare @ionca   money
declare @ponprr  money
declare @ponir   money
declare @othrbal money


declare @amount   money




Declare @PId int , @Amt money

Declare curX cursor for
select pid, Amt  from fin.ASTransLoan where tno  = @Tno
open curX
fetch from curX into @pid, @Amt
while @@FETCH_STATUS = 0
begin
if @pid=1
  set @amount = @amt 
  else if @pid=2
  set  @amount=@Amt
  else if  @pid =3
  set  @amount=@Amt
  else if @pid=4
  set  @amount=@Amt
else if @pid =5
set @amount=@Amt
else if @pid =6
set  @amount=@Amt
else if @pid=7
set  @amount=@Amt 
else if @pid=8  
set @amount=@Amt
else if @pid=9
set  @amount=@Amt
else if @pid=10
set  @amount=@Amt
else if   @pid=11
set  @amount=@Amt
else if   @pid=12
set @amount=@Amt
else if   @pid=13
set @amount=@Amt
else if @pid=14
set @amount=@Amt
else 
set @amount=0
	insert into @Temp values(@PId,@amount)
	 
	fetch next from curX into @pid, @Amt
end

	return 
end


CREATE FUNCTION  [fin].[fgetInterestPaidDate](

@Iaccno int,

@Amount money,

@prevpaidDate date

)

RETURNS Date AS



begin

declare @interestPaidDate date

Declare @id int=0

Declare @amtActual money

Declare @tillDate date

Declare @totalAmount money=0

Declare  @TempTable TABLE  (Id int,

                           amount money,

						   currentDate date)



						  Declare  @TempTable1 TABLE   (

						    Tdate date,

                           Intcal money

						  )

						   if(@prevpaidDate is null)

						   begin

						    insert into @TempTable1 select Tdate,Intcal from fin.IntLog where IAccno=@Iaccno and Tdate<(select Tdate from lg.Company where CompanyId=(select BrchID from fin.ADetail where IAccno=@Iaccno))

						   end

						  else

						  begin 

						    insert into @TempTable1 select Tdate,Intcal from fin.IntLog where IAccno=@Iaccno and Tdate>@prevpaidDate 

						  end

declare curXInterestPaidDate cursor for



select Tdate,Intcal from @TempTable1 order by Tdate asc

open curXInterestPaidDate

fetch from curXInterestPaidDate into @tillDate,@amtActual

while @@FETCH_STATUS=0

begin

set @id+=1

set @totalAmount+=@amtActual

if(@totalAmount<=@Amount)

begin

insert into @TempTable values(@id,@totalAmount,@tillDate)

end

fetch next from curXInterestPaidDate into @tillDate,@amtActual

end

CLOSE  curXInterestPaidDate

	DEALLOCATE  curXInterestPaidDate

	select top 1 @interestPaidDate= max(currentDate)  from @TempTable 



	return @interestPaidDate

 

 end
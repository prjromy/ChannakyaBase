CREATE function [fin].[FGetCurrencyList](@PId int)

returns  @currencyList table(CTId int , CurrencyName varchar(100)) 

as

begin



declare  @f2id int



select @f2id= F2id from acc.FinSys2 where 

F2id=(select Pid from acc.FinSys2 where 

F2id=(select Pid from acc.FinSys2 where 

F2id =(select f2type from acc.FinAcc where

Fid=(select Fid from fin.ProductDetail where Pid=@PId ))))

if(@f2id=24)

			begin 

			insert into @currencyList select CTId,CurrencyName+'('+Country+'-'+currencycode+')' as CurrencyName from fin.CurrencyType

			end

else

begin

insert into @currencyList select CTId,CurrencyName+'('+Country+'-'+currencycode+')' as CurrencyName from fin.CurrencyType where CTId=1

end

return

end
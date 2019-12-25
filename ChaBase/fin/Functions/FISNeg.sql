create FUNCTION [fin].[FISNeg] (@inval money,@outval money)  
RETURNS money AS  
BEGIN 
if (select @inval)>=0
set @outval=@inval
return @outval
END
CREATE FUNCTION [fin].[FGetRecurringIntCapitalizeRule]   
(   
 -- Add the parameters for the function here  
 @Pid int,  
 @DVId int  
)  
RETURNS TABLE   
AS  
RETURN   
(  
 -- Add the SELECT statement with parameter references here  
select pdi.ICBId as InterestCapitalizeId,rd.ICBDurNam +' '+ '( '+cast(pdi.Value as varchar(20))+' )' as InterestCapitalizeName,pdi.Value as contributionValue,pdi.InterestRate from fin.RuleICBDuration rd join   
fin.ProductDurationInt pdi on pdi.ICBId=rd.ICBDurID   
  
where pdi.PId=@Pid and pdi.DVId=@DVId and pdi.DBId is not null  
)
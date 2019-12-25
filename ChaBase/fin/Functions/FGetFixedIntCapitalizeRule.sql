CREATE FUNCTION [fin].[FGetFixedIntCapitalizeRule]   
  
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
  
select pdi.ICBId as InterestCapitalizeId,rd.ICBDurNam as InterestCapitalizeName,pdi.InterestRate from fin.RuleICBDuration rd join   
  
fin.ProductDurationInt pdi on pdi.ICBId=rd.ICBDurID   
  
  
  
where pdi.DBId IS NULL and pdi.DVId=@DVId AND PDI.PId=@Pid  
  
)
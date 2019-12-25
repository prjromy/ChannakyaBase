  
CREATE FUNCTION [fin].[FGETSHARERETURNDISPLAY](@REGNO INT,@BrchID Int)  
RETURNS TABLE   
---MARKED ENCRYPTION  
as  
return   
select c.Name,sp.SCrtno,sd.FSno,sd.TSno,sd.SQty,sd.Stype  from fin.ShrReg s  
 inner join [cust].[FGetCustName]() c on c.CID = s.CID   
 inner join fin.ShrPurchase sp on sp.Regno = s.RegNo   
 inner join fin.SCrtDtls sd on sd.sid = sp.SCrtno  
 where s.RegNo=@REGNO and sd.Status=1 and sp.TType In (1,2,5) and sp.brchid=@BrchID

CREATE function [cust].[FGetDefaultCustContact]()
returns table
as return(   

select a.CID,CNo as Contact,CNoabb,CNodesc,b.CNotype from cust.CustContact a 
inner join 
cust.ContactDef b on a.CNotype=b.CNotype
inner join
cust.CustIRegContact c on a.CCID=c.CCID where b.CNotype=1
)
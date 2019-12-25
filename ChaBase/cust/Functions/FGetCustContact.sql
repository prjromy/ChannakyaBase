CREATE function [cust].[FGetCustContact]()
returns table
as return(select distinct CID,stuff((select ','+ cast(a.cno as varchar(400))[text()]
from (select c.CID,CNodesc+':- '+cno as cno from cust.custcontact c inner join cust.Contactdef b on c.Cnotype=b.cnotype) as a
where cid=b.cid
for xml path(''),type)
 .value('.','NVARCHAR(300)'),1,1,'') Contact
from cust.custcontact b )
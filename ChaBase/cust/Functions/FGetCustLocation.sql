CREATE function [cust].[FGetCustLocation]()

returns table
as return(
select distinct CID,stuff((select ','+ cast(a.loc as varchar(400))[text()]
from 
(select c.CID,LType+':- '+LocationName as loc from cust.CustAddress c 
inner join loc.LocationTypeDef b on c.ATypeId=b.ATypeId 
inner join  (select LID,loc.FGetDetailLocByLID(lid) as LocationName from loc.Location) e on c.LId=e.LId ) as a
where cid=b.cid
for xml path(''),type)
 .value('.','NVARCHAR(300)'),1,1,'') location
from cust.CustAddress b
 )
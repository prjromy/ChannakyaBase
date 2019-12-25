Create function [cust].[FGetCustContactLocation]()

returns table
as return(
select CID, loc.FGetDetailLocByLID(e.lid) as LocationName from cust.CustAddress c 
inner join loc.LocationTypeDef b on c.ATypeId=b.ATypeId 
inner join loc.location e on c.LId=e.LId where b.ATypeId=1 
 )



CREATE FUNCTION [loc].[FGetLocationTB]()
RETURNS TABLE
AS RETURN
(
	
select c.LId as LId,
 ISNULL( c.[LocationName],'' )+ 
 ISNULL(','+p.[LocationName],'')+  
 ISNULL(','+q.[LocationName],'')+  
 ISNULL(','+r.[LocationName],'')+  
 ISNULL(','+s.[LocationName],'') as [LocationName],
 p.LId as PLId 
	from loc.Location c 
	
	left join loc.Location p on c.PLId = p.LId 
	left join loc.Location q on p.PLId = q.LId 
	left join loc.Location r on q.PLId = r.LId
	left join loc.Location s on r.PLId = s.LId
	where c.IsGroup = 0

)
 CREATE function [loc].[FGetDetailLocByLID](@Lid int)
 returns varchar(500)
 as 
begin 
--declare @Tmp table (lid int, plid int, LocationName varchar(500))
declare @LocationName varchar(500)



;with name_tree as 
(
   select lid, plid,LocationName
   from loc.Location
   where lid = @Lid 
   union all
   select C.lid, C.plid, c.LocationName
   from loc.Location c
   join name_tree p on C.lid = P.plid   
    AND C.lid<>C.plid 
) 

--insert into @Tmp select * from name_tree

select top 1 @LocationName = STUFF((SELECT ', '  + CAST(LocationName AS VARCHAR(400)) [text()]  
			  FROM name_tree			  
			  FOR XML PATH(''), TYPE)
			 .value('.','NVARCHAR(MAX)'),1,1,'') 
	FROM name_tree t   

	return @LocationName
	 
end
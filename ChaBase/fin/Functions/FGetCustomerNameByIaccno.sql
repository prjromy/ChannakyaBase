Create function [fin].[FGetCustomerNameByIaccno](@iaccno int)
returns table
as

	return(select distinct b.iaccno,
			STUFF((SELECT ', '  + CAST(Name AS VARCHAR(150)) [text()]
			  FROM (select b.cid, Name,iaccno from fin.aofcust a inner join  cust.fgetcustname() b on a.cid=b.cid) a
			  WHERE a.IAccno = b.IAccno
			  FOR XML PATH(''), TYPE)
			 .value('.','NVARCHAR(MAX)'),1,1,'') CustName
	FROM (select b.cid, Name,iaccno from fin.aofcust a inner join  cust.fgetcustname() b on a.cid=b.cid) b where b.IAccno=@iaccno
	 )
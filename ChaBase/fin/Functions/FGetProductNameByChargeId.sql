CREATE function [fin].[FGetProductNameByChargeId](@chgid int)
returns table
as

	return(select distinct b.ChgId,
			STUFF((SELECT ', '  + CAST(Pname AS VARCHAR(150)) [text()]
			  FROM (select Chgid,Pid,Pname from FGetChargeVsProductTB() a inner join  fin.ProductDetail b on a.product=b.pid) a
			  WHERE a.chgid = b.chgid
			  FOR XML PATH(''), TYPE)
			 .value('.','NVARCHAR(MAX)'),1,1,'') Product
	FROM (select Chgid,Pid,Pname from FGetChargeVsProductTB() a inner join  fin.ProductDetail b on a.product=b.pid) b where b.chgid=@chgid
	 )
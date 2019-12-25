
 create function [fin].[FGetCustomerNameVsAccountId]()
 returns table
 as return
 (select distinct iaccno,
			STUFF((SELECT ', '  + CAST(Name AS VARCHAR(150)) [text()]
			  FROM (select  a.IAccno,Name from fin.AOfCust a  inner join [cust].[FGetCustList]() c on c.cid=a.cid) a
			  WHERE a.iaccno = b.IAccno
			  FOR XML PATH(''), TYPE)
			 .value('.','NVARCHAR(MAX)'),1,1,'') Aname
	FROM (select  a.IAccno,Name from fin.AOfCust a  inner join [cust].[FGetCustList]() c on c.cid=a.cid) b


	)
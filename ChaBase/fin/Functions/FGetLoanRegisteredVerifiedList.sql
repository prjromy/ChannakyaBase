CREATE function [fin].[FGetLoanRegisteredVerifiedList]()



 returns table



 as return



 (select distinct b.RegId,b.LAmt,b.SAmt,RegistrationDate,b.Duration,



			STUFF((SELECT ', '  + CAST(Name AS VARCHAR(150)) [text()]



			  FROM (select a.RegId,Alreg.CID,Name,a.LAmt,a.RegDate as RegistrationDate,a.SAmt,a.Duration from fin.ALRegistration a inner join fin.ALRegCusts alreg on a.RegId=alreg.RegId  inner join [cust].[FGetCustList]() c on c.cid=alreg.cid where status=2) a



			  WHERE a.RegId = b.RegId



			  FOR XML PATH(''), TYPE)



			 .value('.','NVARCHAR(MAX)'),1,1,'') Aname



	FROM (select a.RegId,Alreg.CID,Name,a.LAmt,a.RegDate as RegistrationDate,a.Duration,a.SAmt from fin.ALRegistration a inner join fin.ALRegCusts alreg on a.RegId=alreg.RegId  inner join [cust].[FGetCustList]() c on c.cid=alreg.cid where status=2) b











	)
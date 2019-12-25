CREATE function [cust].[FGetCustName]()

returns table

as return

(	

	 SELECT    CID  , Cast(RTrim(Coalesce(FName + ' ','') + Coalesce(MName + ' ', '') + Coalesce(Lname + ' ', '')) as varchar(200)) AS Name  FROM  cust.CustIndividual

     UNION all

	 SELECT CID, CCName AS Name FROM  cust.CustCompany 

)
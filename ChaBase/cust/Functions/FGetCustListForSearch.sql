CREATE function [cust].[FGetCustListForSearch]()
returns table
as return(

SELECT      
ci.CID,
 a.Name, 
 isnull(ccn.Contact,' ') Mobile,
 cl.LocationName As Address,
 ContactPerson,
 ct.isind as IsIndividual

FROM            cust.CustInfo AS ci 
					INNER JOIN
                             (SELECT   CID, RTrim(LTrim(Fname)) + ' '+ Case When Mname is Null Or Mname = ''  Then '' Else RTrim(LTrim(Mname)) + ' ' End+ RTrim(LTrim(Lname)) AS Name
                               FROM            cust.CustIndividual
                               UNION
                         
								SELECT cc.CID, CCName AS Name
								FROM         cust.CustCompany cc ) AS a ON a.CID = ci.CID
								 
					left  join 
						(SELECT x.CID,cast(x.CPName+ ' '+'('+x.CPCNo+')'as varchar(100)) As ContactPerson
								FROM ( SELECT DISTINCT CID FROM cust.CustContactPerson ) c
							    CROSS APPLY ( SELECT TOP 1 * FROM cust.CustContactPerson t WHERE c.CID = t.CID ) x) d on ci.CID=d.CID
				    left JOIN
                         cust.FGetDefaultCustContact() AS ccn ON ci.CID = ccn.CID 
				    inner join 
						cust.CustType ct on ci.CtypeID= ct.CtypeID
					
					left join 
						 cust.FGetCustContactLocation() cl on ci.CID=cl.CID
						 )
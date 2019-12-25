CREATE function [cust].[FGetCustList]()  
returns table  
as return(  
  
SELECT       ci.CID, a.Name,isnull(ccn.Contact,' ') as Contact,Ctype,ci.CtypeID,ci.PANNo,custadd.LocationName as location  
FROM            cust.CustInfo AS ci INNER JOIN  
                             (   SELECT    CID  , Cast(( Fname + ' ' + ISNULL(Mname+' ', '') + Lname) as varchar(200)) AS Name  
                               FROM            cust.CustIndividual  
  
  
                               UNION all  
                               SELECT        CID, CCName AS Name  
                               FROM            cust.CustCompany) AS a ON a.CID = ci.CID  
          left JOIN  
                         cust.FGetDefaultCustContact() AS ccn ON ci.CID = ccn.CID   
       inner join cust.CustType ct on ci.CtypeID= ct.CtypeID  
       left join  (select c.CID,LocationName from cust.CustAddress c   
       inner join loc.LocationTypeDef b on c.ATypeId=b.ATypeId   
       inner join  (select LID,loc.FGetDetailLocByLID(lid) as LocationName from loc.Location) e on c.LId=e.LId where b.ATypeId=1 ) as custadd on custadd.CID=ci.cid  
       )
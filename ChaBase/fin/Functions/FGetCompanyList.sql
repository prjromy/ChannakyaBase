
create function [fin].[FGetCompanyList]()
returns table as return (

select
CompanyId ,BranchName,Region ,State ,Address,PhoneNo,Email ,FaxNo ,Prefix ,ParentId,IsBranch ,AdditionalUser,IPAddress,TDate
from LG.Company
union all select CompanyId ,BranchName,Region ,State ,Address,PhoneNo,Email ,FaxNo ,Prefix ,ParentId,IsBranch ,AdditionalUser,IPAddress,TDate
 from fin.Fgetheadoffice()
)
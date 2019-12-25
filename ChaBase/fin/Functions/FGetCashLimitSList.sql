CREATE function [fin].[FGetCashLimitSList](@branchId int)

 RETURNS TABLE 

AS

RETURN (

select EmployeeName,

UserName,

DGName,

 cs.stfid,

 cs.cashLimitId,

 cs.dramt,

 cs.cramt

-- BranchId

from fin.FGetAllUsersWithDesignation() a inner 

join fin.CashLimits cs on cs.stfid=a.UserId

--where 
--(BranchId=@branchId

--OR COALESCE(@branchId, 1) = 1

--)

)
create FUNCTION [fin].[FGetChqInv](@BRANCHID INT, @QUANTITY INT)
RETURNS TABLE
---MARKED ENCRYPTION 
as
Return
(

SELECT top 100 percent    Tochqno - Lastindx AS Balance
FROM         fin.ChqInventory
WHERE     (Brnhid = @BranchId) AND (Lastindx <> Tochqno) AND (Tochqno - Lastindx >= @Quantity) AND (ISfinish = 0)
ORDER BY Lastindx
)
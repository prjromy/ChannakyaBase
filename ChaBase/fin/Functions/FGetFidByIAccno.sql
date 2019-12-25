
-- =============================================
CREATE FUNCTION [fin].[FGetFidByIAccno]
(	
@IAccno bigint
)
RETURNS TABLE 
AS
RETURN 
(
	select pd.FID from fin.ADetail ad join fin.ProductDetail pd on ad.PID=pd.PID
where ad.IAccno=@IAccno
)
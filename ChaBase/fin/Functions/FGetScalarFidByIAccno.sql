

create FUNCTION [fin].[FGetScalarFidByIAccno]

(	

@IAccno bigint

)

RETURNS int 

AS

begin
return 

(
		select pd.FID from fin.ADetail ad join fin.ProductDetail pd on ad.PID=pd.PID
		where ad.IAccno=@IAccno
)
end
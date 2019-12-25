

-- =============================================

create FUNCTION [fin].[FGetStypeIAccno]

(	

@IAccno bigint

)

RETURNS int 

AS

begin
return 

(
		select SType from fin.ADetail a inner join fin.ProductDetail b on a.PID=b.PID inner join fin.SchmDetails s on s.SDID=b.SDID where IAccno=@IAccno
)
end
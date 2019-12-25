CREATE FUNCTION [fin].[FGetAccType]

	 (@STYPE BIT)

RETURNS  TABLE 

---MARKED ENCRYPTION 	

AS

return(

SELECT     ADetail.IAccno

FROM         fin.SchmDetails INNER JOIN

                      fin.ProductDetail ON SchmDetails.SDID = ProductDetail.SDID INNER JOIN

                      fin.ADetail ON ProductDetail.PID = ADetail.PID

WHERE     (SchmDetails.SType =@STYPE ))
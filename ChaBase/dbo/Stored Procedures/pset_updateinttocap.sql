create PROCEDURE [dbo].[pset_updateinttocap](@IACCNO NUMERIC)
	
AS

begin
	SET NOCOUNT ON;

    update fin.VWTblADIntDtls Set IntToCap =
		(SELECT  Round(expint - IpdSlf - IpdNom - IpdPybl,2)AS IntToCap FROM
			(SELECT  expint = CASE WHEN EXISTS
		                (SELECT     Iaccno
		                FROM          fin.AIntExpenses
		                WHERE      valued = 5 AND fin.ADetail.IAccno = fin.AIntExpenses.Iaccno) THEN
		                (SELECT     SUM(IntAmt)+ (SELECT isnull(SUM(Amt),0) FROM fin.AAdjustmnt WHERE Iaccno=fin.ADetail.IAccno AND PID=5)
		                FROM          fin.AIntExpenses
		                WHERE      valued = 5 AND  fin.ADetail.IAccno = fin.AIntExpenses.Iaccno) 
				ELSE (SELECT isnull(SUM(Amt),0) FROM fin.AAdjustmnt WHERE Iaccno=ADetail.IAccno AND PID=5) END,
				
				IpdSlf = CASE WHEN EXISTS
		                (SELECT     TIaccno
		                FROM          fin.AintCap
		                WHERE      AintCap.TIaccno = fin.ADetail.IAccno and AintCap.IsSlf=1) THEN
		                (SELECT     SUM(AintCap.IntAmt)
		                FROM          fin.AintCap where AintCap.TIaccno = ADetail.IAccno and AintCap.IsSlf=1) ELSE 0 END,
				
				IpdNom = CASE WHEN EXISTS
		                (SELECT     FIaccno
		                FROM          fin.AintCap
		                WHERE      AintCap.FIaccno = ADetail.IAccno and AintCap.IsSlf=0) THEN
		                (SELECT     SUM(AintCap.IntAmt)
		                FROM          fin.AintCap where AintCap.FIaccno = ADetail.IAccno and AintCap.IsSlf=0 ) ELSE 0 END, 
		
				IpdPybl = CASE WHEN EXISTS
		                (SELECT     IAccno
		                FROM          fin.AintPayable
		                WHERE      AIntPayable.Iaccno = ADetail.IAccno) THEN
		                (SELECT     SUM(AIntPayable.IntAmt)
		                FROM          fin.AintPayable where AintPayable.Iaccno = ADetail.IAccno) ELSE 0 END
		
			FROM fin.ADetail  
			INNER JOIN fin.ProductDetail ON ADetail.PID = ProductDetail.PID 
			INNER JOIN fin.SchmDetails ON ProductDetail.SDID = SchmDetails.SDID AND SchmDetails.SType = 0 AND ADetail.IAccNo = @IACCNO
		Where Iaccno =@IACCNO)x)


		end
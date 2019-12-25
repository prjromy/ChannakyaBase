CREATE function [fin].[FGetReportInterestDetailsByIAccNo](@IAccNo int)

returns table

AS

return(

SELECT     fin.ADetail.IAccno,

totint =                      

			(SELECT     isnull(SUM(intcal),0)+ (SELECT isnull(SUM(AMT),0) FROM fin.Aadjustmnt WHERE IACCNO=ADETAIL.IACCNO AND PID=0)

                            FROM          fin.intlog

                            WHERE       (valued = 0 or valued = 5  ) 

		 AND adetail.iaccno = intlog.iaccno),

	expint =  

                          (SELECT     isnull(SUM(intamt),0)--+ (SELECT isnull(SUM(AMT),0) FROM fin.Aadjustmnt WHERE IACCNO=ADETAIL.IACCNO AND PID=5)

                            FROM          fin.AIntExpenses

                            WHERE      valued = 5 AND adetail.iaccno = fin.AIntExpenses.iaccno),

IpdSlf = 

                          (SELECT     isnull(SUM(AintCap.IntAmt),0)

                            FROM          fin.AintCap where AintCap.TIaccno = ADetail.IAccno and AintCap.IsSlf=1) ,

IpdNom = 

                          (SELECT     isnull(SUM(AintCap.IntAmt),0)

                            FROM          fin.AintCap where AintCap.FIaccno = ADetail.IAccno and AintCap.IsSlf=0), 

IpdPybl = 

                          (SELECT     isnull(SUM(AIntPayable.IntAmt),0)

                            FROM          fin.AIntPayable where AIntPayable.Iaccno = ADetail.IAccno), 

IonIA = 

                          (SELECT     isnull(SUM(intcal),0)+ (SELECT isnull(SUM(AMT),0) FROM fin.Aadjustmnt WHERE IACCNO=ADETAIL.IACCNO AND PID=1)

                            FROM          fin.intlog

                            WHERE       valued in(1,6) AND adetail.iaccno = intlog.iaccno),

 IOnIR = 

                          (SELECT     isnull(SUM(intamt),0)+ (SELECT isnull(SUM(AMT),0) FROM fin.Aadjustmnt WHERE IACCNO=ADETAIL.IACCNO AND PID=6)

                            FROM          fin.AIntExpenses

                            WHERE      valued = 6 AND adetail.iaccno = fin.AIntExpenses.iaccno),

IOnCA =

                          (SELECT    isnull(SUM(intcal),0)+ (SELECT isnull(SUM(AMT),0) FROM fin.Aadjustmnt WHERE IACCNO=ADETAIL.IACCNO AND PID=2)

                            FROM          fin.intlog

                            WHERE      valued in ( 2,7) AND adetail.iaccno = intlog.iaccno),

 IOnCR = 

                          (SELECT     isnull(SUM(intamt),0) + (SELECT isnull(SUM(AMT),0) FROM fin.Aadjustmnt WHERE IACCNO=ADETAIL.IACCNO AND PID=7)

                            FROM          fin.AIntExpenses

                            WHERE      valued = 7 AND adetail.iaccno = fin.AIntExpenses.iaccno), 

POnPrA =

                          (SELECT     isnull(SUM(intcal),0)+ (SELECT isnull(SUM(AMT),0) FROM fin.AAdjustmnt WHERE IACCNO=ADETAIL.IACCNO AND PID=3)

                            FROM          fin.intlog

                            WHERE      valued in( 3,8)  AND adetail.iaccno = intlog.iaccno),

 POnPrR = 

                          (SELECT     isnull(SUM(intamt),0)+ (SELECT isnull(SUM(AMT),0) FROM fin.Aadjustmnt WHERE IACCNO=ADETAIL.IACCNO AND PID=8)

                            FROM          fin.AIntExpenses

                            WHERE      valued = 8 AND adetail.iaccno = fin.AIntExpenses.iaccno),

 POnIA = 

                          (SELECT     isnull(SUM(intcal),0) + (SELECT isnull(SUM(AMT),0) FROM fin.Aadjustmnt WHERE IACCNO=ADETAIL.IACCNO AND PID=4)

                            FROM          fin.intlog

                            WHERE      valued in( 4,9) AND adetail.iaccno = intlog.iaccno),

 POnIR = 

                          (SELECT     isnull(SUM(intamt),0)+ (SELECT isnull(SUM(AMT),0) FROM fin.Aadjustmnt WHERE IACCNO=ADETAIL.IACCNO AND PID=9)

                            FROM          fin.AIntExpenses

                            WHERE      valued = 9 AND adetail.iaccno = fin.AIntExpenses.iaccno)

		 ,ADetail.BrchId

FROM         fin.adetail where adetail.iaccNo = @IAccNo)
CREATE FUNCTION [fin].[FgetReportProductTransactionSummary]

				(	

				

				@FDate SMALLDATETIME,

				@TDate SMALLDATETIME,
				@BranchId INT=null

				)

RETURNS TABLE

---MARKED ENCRYPTION  

As



Return(

Select X.PID,X.PName as ProductName,Sum(X.CashInFlow) as CashInFlow,Sum(X.NonCashInFlow) NonCashInFlow,Sum(X.CashOutFlow) CashOutFlow,Sum(X.NonCashOutFlow) NonCashOutFlow 

	From 

(SELECT     ProductDetail.PID, ProductDetail.PName, SUM(AMTrns.cramt) AS CashInFlow, 0 as NonCashInFlow, SUM(AMTrns.dramt) AS CashOutFlow,0 as NonCashOutFlow

	FROM         trans.AMTrns INNER JOIN

	                      fin.ADetail ON AMTrns.IAccno = ADetail.IAccno INNER JOIN

	                      fin.ProductDetail ON ADetail.PID = ProductDetail.PID

WHERE     (AMTrns.brnhno=COALESCE(@branchId,AMTrns.brnhno) ) AND (AMTrns.vdate BETWEEN @FDate AND @TDate) AND (AMTrns.ttype = 1)

GROUP BY ProductDetail.PID, ProductDetail.PName



Union ALL



SELECT     ProductDetail.PID, ProductDetail.PName, 0 AS CashInFlow, SUM(AMTrns.cramt) as NonCashInFlow, 0 AS CashOutFlow,SUM(AMTrns.dramt) as NonCashOutFlow

FROM         trans.AMTrns INNER JOIN

                      fin.ADetail ON AMTrns.IAccno = ADetail.IAccno INNER JOIN

                      fin.ProductDetail ON ADetail.PID = ProductDetail.PID

WHERE     (AMTrns.brnhno =COALESCE(@branchId,AMTrns.brnhno)) AND (AMTrns.vdate BETWEEN @FDate AND @TDate) AND (AMTrns.ttype = 5)

GROUP BY ProductDetail.PID, ProductDetail.PName



UNION ALL



SELECT     ProductDetail.PID, ProductDetail.PName, 0 AS CashInFlow, SUM(AintCap.IntAmt) as NonCashInFlow, 0 AS CashOutFlow,SUM(fin.AintCap.Tax) as NonCashOutFlow

FROM         fin.AintCap INNER JOIN

                      fin.ADetail ON AintCap.TIaccno = ADetail.IAccno INNER JOIN

                      fin.ProductDetail ON ADetail.PID = ProductDetail.PID

WHERE     (ADetail.BrchID= COALESCE(@branchId,ADetail.BrchID)) AND (AintCap.vdate BETWEEN  @FDate AND @TDate) AND 

isslf = 1 and Fiaccno is null  and INtamt>0

GROUP BY ProductDetail.PID, ProductDetail.PName



UNION ALL


SELECT     ProductDetail.PID, ProductDetail.PName, 0 AS CashInFlow, SUM(AintCap.CRamt) as NonCashInFlow, 0 AS CashOutFlow,0 as NonCashOutFlow

FROM         fin.AintCap INNER JOIN

                      fin.ADetail ON AintCap.TIaccno = ADetail.IAccno INNER JOIN

                      fin.ProductDetail ON ADetail.PID = ProductDetail.PID

WHERE     (ADetail.BrchID = COALESCE(@branchId,ADetail.BrchID)) AND (AintCap.vdate BETWEEN  @FDate AND @TDate) AND 

isslf = 0 and Fiaccno IS not null  and INtamt>0

GROUP BY ProductDetail.PID, ProductDetail.PName


UNION ALL


SELECT     ProductDetail.PID, ProductDetail.PName, 0 AS CashInFlow, 0 as NonCashInFlow, 0 AS CashOutFlow, SUM(AintCap.Tax) as NonCashOutFlow

FROM         fin.AintCap INNER JOIN

                      fin.ADetail ON AintCap.FIaccno = ADetail.IAccno INNER JOIN

                      fin.ProductDetail ON ADetail.PID = ProductDetail.PID

WHERE     (ADetail.BrchID= COALESCE(@branchId,ADetail.BrchID)) AND (AintCap.vdate BETWEEN @FDate AND @TDate) AND 

isslf = 0 and Fiaccno IS not null  and INtamt>0

GROUP BY ProductDetail.PID, ProductDetail.PName





) X

Group by X.PID,X.PName

)
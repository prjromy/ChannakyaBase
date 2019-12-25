create PROCEDURE [fin].[PAloanTraInsert](@TNO NUMERIC)
---MARKED ENCRYPTION 
AS
BEGIN



	BEGIN TRANSACTION Ins
	INSERT INTO fin.aloantra 
	SELECT   AMTrns.iaccno,  AMTrns.tno,AMTrns.vdate,
	PriDr= CASE WHEN EXISTS
	                        (SELECT     tno
	                            FROM          fin.AMTransLoan
	                            WHERE      pid = 10 and AMTransLoan.tno=AMTrns.tno) THEN
	                          (SELECT     SUM(amt)
	                            FROM          fin.AMTransLoan
	                            WHERE      pid = 10  and AMTransLoan.tno=AMTrns.tno) ELSE 0 END,
	OthrDr =CASE WHEN EXISTS
	                          (SELECT     tno
	                            FROM          fin.AMTransLoan
	                            WHERE      pid = 11 and AMTransLoan.tno=AMTrns.tno) THEN
	                          (SELECT     SUM(amt)
	                            FROM         fin.AMTransLoan
	                            WHERE      pid = 11 and AMTransLoan.tno=AMTrns.tno) ELSE 0 END,
	PriCr =CASE WHEN EXISTS
	                          (SELECT     tno
	                            FROM          fin.AMTransLoan
	                            WHERE      pid = 13 and AMTransLoan.tno=AMTrns.tno) THEN
	                          (SELECT     SUM(amt)
	                            FROM         fin.AMTransLoan
	                            WHERE      pid = 13 and AMTransLoan.tno=AMTrns.tno) ELSE 0 END,
	othrCr =CASE WHEN EXISTS
	                          (SELECT     tno
	                            FROM          fin.AMTransLoan
	                            WHERE      pid = 14 and AMTransLoan.tno=AMTrns.tno) THEN
	                          (SELECT     SUM(amt)
	                            FROM        fin.AMTransLoan
	                            WHERE      pid = 14 and AMTransLoan.tno=AMTrns.tno) ELSE 0 END,
	RbtInt =CASE WHEN EXISTS
	                          (SELECT     tno
	                            FROM         fin.AMTransLoan
	                            WHERE      pid = 12 and AMTransLoan.tno=AMTrns.tno) THEN
	                          (SELECT     SUM(amt)
	                            FROM         fin.AMTransLoan
	                            WHERE      pid = 12 and AMTransLoan.tno=AMTrns.tno) ELSE 0 END,
	
	RbtPen =CASE WHEN EXISTS
	                          (SELECT     tno
	                            FROM         fin.AMTransLoan
	                            WHERE      pid = 17 and AMTransLoan.tno=AMTrns.tno) THEN
	                          (SELECT     SUM(amt)
	                            FROM          fin.AMTransLoan
	                            WHERE      pid = 17 and AMTransLoan.tno=AMTrns.tno) ELSE 0 END,
	intAPd =CASE WHEN EXISTS
	                          (SELECT     tno
	                            FROM          fin.AMTransLoan
	                            WHERE      pid = 0 and AMTransLoan.tno=AMTrns.tno) THEN
	                          (SELECT     SUM(amt)
	                            FROM         fin.AMTransLoan
	                            WHERE      pid = 0 and AMTransLoan.tno=AMTrns.tno) ELSE 0 END,
	intRPd =CASE WHEN EXISTS
	                          (SELECT     tno
	                            FROM          fin.AMTransLoan
	                            WHERE      pid = 5 and AMTransLoan.tno=AMTrns.tno) THEN
	                          (SELECT     SUM(amt)
	                            FROM        fin.AMTransLoan
	                            WHERE      pid = 5 and AMTransLoan.tno=AMTrns.tno) ELSE 0 END,
	IonIAPd =CASE WHEN EXISTS
	                          (SELECT     tno
	                            FROM         fin.AMTransLoan
	                            WHERE      pid = 1 and AMTransLoan.tno=AMTrns.tno) THEN
	                          (SELECT     SUM(amt)
	                            FROM          fin.AMTransLoan
	                            WHERE      pid = 1 and AMTransLoan.tno=AMTrns.tno) ELSE 0 END,
	IonIRPd =CASE WHEN EXISTS
	                          (SELECT     tno
	                            FROM         fin.AMTransLoan
	                            WHERE      pid = 6 and AMTransLoan.tno=AMTrns.tno) THEN
	                          (SELECT     SUM(amt)
	                            FROM         fin.AMTransLoan
	                            WHERE      pid = 6 and AMTransLoan.tno=AMTrns.tno) ELSE 0 END,
	IonCAPd =CASE WHEN EXISTS
	                          (SELECT     tno
	                            FROM          fin.AMTransLoan
	                            WHERE      pid = 2 and AMTransLoan.tno=AMTrns.tno) THEN
	                          (SELECT     SUM(amt)
	                            FROM          fin.AMTransLoan
	                            WHERE      pid = 2 and AMTransLoan.tno=AMTrns.tno) ELSE 0 END,
	IonCRPd =CASE WHEN EXISTS
	                          (SELECT     tno
	                            FROM          fin.AMTransLoan
	                            WHERE      pid = 7 and AMTransLoan.tno=AMTrns.tno) THEN
	                          (SELECT     SUM(amt)
	                            FROM        fin.AMTransLoan
	                            WHERE      pid = 7 and AMTransLoan.tno=AMTrns.tno) ELSE 0 END,
	PonPrAPd =CASE WHEN EXISTS
	                          (SELECT     tno
	                            FROM         fin.AMTransLoan
	                            WHERE      pid = 3 and AMTransLoan.tno=AMTrns.tno) THEN
	                          (SELECT     SUM(amt)
	                            FROM          fin.AMTransLoan
	                            WHERE      pid = 3 and AMTransLoan.tno=AMTrns.tno) ELSE 0 END,
	PonPrRPd =CASE WHEN EXISTS
	                          (SELECT     tno
	                            FROM         fin.AMTransLoan
	                            WHERE      pid = 8 and AMTransLoan.tno=AMTrns.tno) THEN
	                          (SELECT     SUM(amt)
	                            FROM         fin.AMTransLoan
	                            WHERE      pid = 8 and AMTransLoan.tno=AMTrns.tno) ELSE 0 END,
	PonIAPd =CASE WHEN EXISTS
	                          (SELECT     tno
	                            FROM          fin.AMTransLoan
	                            WHERE      pid = 4 and AMTransLoan.tno=AMTrns.tno) THEN
	                          (SELECT     SUM(amt)
	                            FROM         fin.AMTransLoan
	                            WHERE      pid = 4 and AMTransLoan.tno=AMTrns.tno) ELSE 0 END,
	PonIRPd =CASE WHEN EXISTS
	                          (SELECT     tno
	                            FROM          fin.AMTransLoan
	                            WHERE      pid = 9 and AMTransLoan.tno=AMTrns.tno) THEN
	                          (SELECT     SUM(amt)
	                            FROM         fin.AMTransLoan
	                            WHERE      pid = 9 and AMTransLoan.tno=AMTrns.tno) ELSE 0 END,AMTrns.ttype
	
	FROM         trans.AMTrns INNER JOIN
	                      fin.ADetail ON AMTrns.IAccno = ADetail.IAccno INNER JOIN
	                      fin.ProductDetail ON ADetail.PID = ProductDetail.PID INNER JOIN
	                      fin.SchmDetails ON ProductDetail.SDID = SchmDetails.SDID
	WHERE     (fin.SchmDetails.SType = 1) and amtrns.tno=@tno
COMMIT TRANSACTION Ins

END
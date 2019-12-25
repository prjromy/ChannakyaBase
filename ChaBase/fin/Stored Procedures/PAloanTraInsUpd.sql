create PROCEDURE [fin].[PAloanTraInsUpd](@TNO NUMERIC)
---MARKED ENCRYPTION 
AS

BEGIN	      
	BEGIN TRANSACTION Upd

	UPDATE fin.aloantra SET 		
		PriDr= CASE WHEN EXISTS
		                        (SELECT     tno
		                            FROM          fin.AMTransLoan
		                            WHERE      pid = 10 and fin.AMTransLoan.tno=@tno) THEN
		                          (SELECT     SUM(amt)
		                            FROM          fin.AMTransLoan
		                            WHERE      pid = 10  and fin.AMTransLoan.tno=@tno) ELSE 0 END,
		OthrDr =CASE WHEN EXISTS
		                          (SELECT     tno
		                            FROM          fin.AMTransLoan
		                            WHERE      pid = 11 and fin.AMTransLoan.tno=@tno) THEN
		                          (SELECT     SUM(amt)
		                            FROM          fin.AMTransLoan
		                            WHERE      pid = 11 and fin.AMTransLoan.tno=@tno) ELSE 0 END,
		PriCr =CASE WHEN EXISTS
		                          (SELECT     tno
		                            FROM          fin.AMTransLoan
		                            WHERE      pid = 13 and fin.AMTransLoan.tno=@tno) THEN
		                          (SELECT     SUM(amt)
		                            FROM          fin.AMTransLoan
		                            WHERE      pid = 13 and fin.AMTransLoan.tno=@tno) ELSE 0 END,
		othrCr =CASE WHEN EXISTS
		                          (SELECT     tno
		                            FROM          fin.AMTransLoan
		                            WHERE      pid = 14 and fin.AMTransLoan.tno=@tno) THEN
		                          (SELECT     SUM(amt)
		                            FROM          fin.AMTransLoan
		                            WHERE      pid = 14 and fin.AMTransLoan.tno=@tno) ELSE 0 END,
		RbtInt =CASE WHEN EXISTS
		                          (SELECT     tno
		                            FROM          fin.AMTransLoan
		                            WHERE      pid = 12 and fin.AMTransLoan.tno=@tno) THEN
		                          (SELECT     SUM(amt)
		                            FROM          fin.AMTransLoan
		                            WHERE      pid = 12 and fin.AMTransLoan.tno=@tno) ELSE 0 END,
		
		RbtPen =CASE WHEN EXISTS
		                          (SELECT     tno
		                            FROM          fin.AMTransLoan
		                            WHERE      pid = 17 and fin.AMTransLoan.tno=@tno) THEN
		                          (SELECT     SUM(amt)
		                            FROM          fin.AMTransLoan
		                            WHERE      pid = 17 and fin.AMTransLoan.tno=@tno) ELSE 0 END,
		intAPd =CASE WHEN EXISTS
		                          (SELECT     tno
		                            FROM          fin.AMTransLoan
		                            WHERE      pid = 0 and fin.AMTransLoan.tno=@tno) THEN
		                          (SELECT     SUM(amt)
		                            FROM          fin.AMTransLoan
		                            WHERE      pid = 0 and fin.AMTransLoan.tno=@tno) ELSE 0 END,
		intRPd =CASE WHEN EXISTS
		                          (SELECT     tno
		                            FROM          fin.AMTransLoan
		                            WHERE      pid = 5 and fin.AMTransLoan.tno=@tno) THEN
		                          (SELECT     SUM(amt)
		                            FROM          fin.AMTransLoan
		                            WHERE      pid = 5 and fin.AMTransLoan.tno=@tno) ELSE 0 END,
		IonIAPd =CASE WHEN EXISTS
		                          (SELECT     tno
		                            FROM          fin.AMTransLoan
		                            WHERE      pid = 1 and fin.AMTransLoan.tno=@tno) THEN
		                          (SELECT     SUM(amt)
		                            FROM          fin.AMTransLoan
		                            WHERE      pid = 1 and fin.AMTransLoan.tno=@tno) ELSE 0 END,
		IonIRPd =CASE WHEN EXISTS
		                          (SELECT     tno
		                            FROM          fin.AMTransLoan
		                            WHERE      pid = 6 and fin.AMTransLoan.tno=@tno) THEN
		                          (SELECT     SUM(amt)
		                            FROM          fin.AMTransLoan
		                            WHERE      pid = 6 and fin.AMTransLoan.tno=@tno) ELSE 0 END,
		IonCAPd =CASE WHEN EXISTS
		                          (SELECT     tno
		                            FROM          fin.AMTransLoan
		                            WHERE      pid = 2 and fin.AMTransLoan.tno=@tno) THEN
		                          (SELECT     SUM(amt)
		                            FROM          fin.AMTransLoan
		                            WHERE      pid = 2 and fin.AMTransLoan.tno=@tno) ELSE 0 END,
		IonCRPd =CASE WHEN EXISTS
		                          (SELECT     tno
		                            FROM          fin.AMTransLoan
		                            WHERE      pid = 7 and fin.AMTransLoan.tno=@tno) THEN
		                          (SELECT     SUM(amt)
		                            FROM          fin.AMTransLoan
		                            WHERE      pid = 7 and fin.AMTransLoan.tno=@tno) ELSE 0 END,
		PonPrAPd =CASE WHEN EXISTS
		                          (SELECT     tno
		                            FROM          fin.AMTransLoan
		                            WHERE      pid = 3 and fin.AMTransLoan.tno=@tno) THEN
		                          (SELECT     SUM(amt)
		                            FROM          fin.AMTransLoan
		                            WHERE      pid = 3 and fin.AMTransLoan.tno=@tno) ELSE 0 END,
		PonPrRPd =CASE WHEN EXISTS
		                          (SELECT     tno
		                            FROM          fin.AMTransLoan
		                            WHERE      pid = 8 and fin.AMTransLoan.tno=@tno) THEN
		                          (SELECT     SUM(amt)
		                            FROM          fin.AMTransLoan
		                            WHERE      pid = 8 and fin.AMTransLoan.tno=@tno) ELSE 0 END,
		PonIAPd =CASE WHEN EXISTS
		                          (SELECT     tno
		                            FROM          fin.AMTransLoan
		                            WHERE      pid = 4 and fin.AMTransLoan.tno=@tno) THEN
		                          (SELECT     SUM(amt)
		                            FROM          fin.AMTransLoan
		                            WHERE      pid = 4 and fin.AMTransLoan.tno=@tno) ELSE 0 END,
		PonIRPd =CASE WHEN EXISTS
		                          (SELECT     tno
		                            FROM          fin.AMTransLoan
		                            WHERE      pid = 9 and fin.AMTransLoan.tno=@tno) THEN
		                          (SELECT     SUM(amt)
		                            FROM          fin.AMTransLoan
                            WHERE      pid = 9 and fin.AMTransLoan.tno=@tno) ELSE 0 END	
	WHERE tno = @tno 

	COMMIT TRANSACTION Upd
END
create function [CTrans].[FgetLoanTransactionDetais]( @Iaccno int)
returns 
table as return
(
SELECT        trans.AMTrns.IAccno, trans.AMTrns.tno, trans.AMTrns.vdate, CASE WHEN EXISTS
                             (SELECT        tno
                               FROM            fin.AMTransLoan
                               WHERE        pid = 10 AND AMTransLoan.tno = AMTrns.tno) THEN
                             (SELECT        SUM(amt)
                               FROM            fin.AMTransLoan
                               WHERE        pid = 10 AND AMTransLoan.tno = AMTrns.tno) ELSE 0 END AS PriDr, CASE WHEN EXISTS
                             (SELECT        tno
                               FROM            fin.AMTransLoan
                               WHERE        pid = 11 AND AMTransLoan.tno = AMTrns.tno) THEN
                             (SELECT        SUM(amt)
                               FROM            fin.AMTransLoan
                               WHERE        pid = 11 AND AMTransLoan.tno = AMTrns.tno) ELSE 0 END AS OthrDr, CASE WHEN EXISTS
                             (SELECT        tno
                               FROM            fin.AMTransLoan
                               WHERE        pid = 13 AND AMTransLoan.tno = AMTrns.tno) THEN
                             (SELECT        SUM(amt)
                               FROM            fin.AMTransLoan
                               WHERE        pid = 13 AND AMTransLoan.tno = AMTrns.tno) ELSE 0 END AS PriCr, CASE WHEN EXISTS
                             (SELECT        tno
                               FROM            fin.AMTransLoan
                               WHERE        pid = 14 AND AMTransLoan.tno = AMTrns.tno) THEN
                             (SELECT        SUM(amt)
                               FROM            fin.AMTransLoan
                               WHERE        pid = 14 AND AMTransLoan.tno = AMTrns.tno) ELSE 0 END AS othrCr, CASE WHEN EXISTS
                             (SELECT        tno
                               FROM            fin.AMTransLoan
                               WHERE        pid = 12 AND AMTransLoan.tno = AMTrns.tno) THEN
                             (SELECT        SUM(amt)
                               FROM            fin.AMTransLoan
                               WHERE        pid = 12 AND AMTransLoan.tno = AMTrns.tno) ELSE 0 END AS RbtInt, CASE WHEN EXISTS
                             (SELECT        tno
                               FROM            fin.AMTransLoan
                               WHERE        pid = 17 AND AMTransLoan.tno = AMTrns.tno) THEN
                             (SELECT        SUM(amt)
                               FROM            fin.AMTransLoan
                               WHERE        pid = 17 AND AMTransLoan.tno = AMTrns.tno) ELSE 0 END AS RbtPen, CASE WHEN EXISTS
                             (SELECT        tno
                               FROM            fin.AMTransLoan
                               WHERE        pid = 0 AND AMTransLoan.tno = AMTrns.tno) THEN
                             (SELECT        SUM(amt)
                               FROM            fin.AMTransLoan
                               WHERE        pid = 0 AND AMTransLoan.tno = AMTrns.tno) ELSE 0 END AS intAPd, CASE WHEN EXISTS
                             (SELECT        tno
                               FROM            fin.AMTransLoan
                               WHERE        pid = 5 AND AMTransLoan.tno = AMTrns.tno) THEN
                             (SELECT        SUM(amt)
                               FROM            fin.AMTransLoan
                               WHERE        pid = 5 AND AMTransLoan.tno = AMTrns.tno) ELSE 0 END AS intRPd, CASE WHEN EXISTS
                             (SELECT        tno
                               FROM            fin.AMTransLoan
                               WHERE        pid = 1 AND AMTransLoan.tno = AMTrns.tno) THEN
                             (SELECT        SUM(amt)
                               FROM            fin.AMTransLoan
                               WHERE        pid = 1 AND AMTransLoan.tno = AMTrns.tno) ELSE 0 END AS IonIAPd, CASE WHEN EXISTS
                             (SELECT        tno
                               FROM            fin.AMTransLoan
                               WHERE        pid = 6 AND AMTransLoan.tno = AMTrns.tno) THEN
                             (SELECT        SUM(amt)
                               FROM            fin.AMTransLoan
                               WHERE        pid = 6 AND AMTransLoan.tno = AMTrns.tno) ELSE 0 END AS IonIRPd, CASE WHEN EXISTS
                             (SELECT        tno
                               FROM            fin.AMTransLoan
                               WHERE        pid = 2 AND AMTransLoan.tno = AMTrns.tno) THEN
                             (SELECT        SUM(amt)
                               FROM            fin.AMTransLoan
                               WHERE        pid = 2 AND AMTransLoan.tno = AMTrns.tno) ELSE 0 END AS IonCAPd, CASE WHEN EXISTS
                             (SELECT        tno
                               FROM            fin.AMTransLoan
                               WHERE        pid = 7 AND AMTransLoan.tno = AMTrns.tno) THEN
                             (SELECT        SUM(amt)
                               FROM            fin.AMTransLoan
                               WHERE        pid = 7 AND AMTransLoan.tno = AMTrns.tno) ELSE 0 END AS IonCRPd, CASE WHEN EXISTS
                             (SELECT        tno
                               FROM            fin.AMTransLoan
                               WHERE        pid = 3 AND AMTransLoan.tno = AMTrns.tno) THEN
                             (SELECT        SUM(amt)
                               FROM            fin.AMTransLoan
                               WHERE        pid = 3 AND AMTransLoan.tno = AMTrns.tno) ELSE 0 END AS PonPrAPd, CASE WHEN EXISTS
                             (SELECT        tno
                               FROM            fin.AMTransLoan
                               WHERE        pid = 8 AND AMTransLoan.tno = AMTrns.tno) THEN
                             (SELECT        SUM(amt)
                               FROM            fin.AMTransLoan
                               WHERE        pid = 8 AND AMTransLoan.tno = AMTrns.tno) ELSE 0 END AS PonPrRPd, CASE WHEN EXISTS
                             (SELECT        tno
                               FROM            fin.AMTransLoan
                               WHERE        pid = 4 AND AMTransLoan.tno = AMTrns.tno) THEN
                             (SELECT        SUM(amt)
                               FROM            fin.AMTransLoan
                               WHERE        pid = 4 AND AMTransLoan.tno = AMTrns.tno) ELSE 0 END AS PonIAPd, CASE WHEN EXISTS
                             (SELECT        tno
                               FROM            fin.AMTransLoan
                               WHERE        pid = 9 AND AMTransLoan.tno = AMTrns.tno) THEN
                             (SELECT        SUM(amt)
                               FROM            fin.AMTransLoan
                               WHERE        pid = 9 AND AMTransLoan.tno = AMTrns.tno) ELSE 0 END AS PonIRPd, trans.AMTrns.ttype
FROM            trans.AMTrns INNER JOIN
                         fin.ADetail ON trans.AMTrns.IAccno = fin.ADetail.IAccno INNER JOIN
                         fin.ProductDetail ON fin.ADetail.PID = fin.ProductDetail.PID INNER JOIN
                         fin.SchmDetails ON fin.ProductDetail.SDID = fin.SchmDetails.SDID
WHERE        (fin.SchmDetails.SType = 1) and  fin.ADetail.IAccno=@Iaccno
)
create function [fin].[FGetLoanPaymentDetails]()
 returns table as return
select  pd.PName,ad.IAccno,ad.Accno,ad.Aname,ad.LastTransDate,ad.IRate,x.Amt  from (
SELECT    astl.Amt,IAccno,ast.tno,ast.tdate,brnhno FROM          fin.ASTransLoan astl INNER JOIN
trans.ASTrns ast ON ast.tno = astl.tno AND   astl.PId = 10
union all 
SELECT    aml.Amt,Iaccno,amt.tno,tdate,brnhno FROM          fin.AMTransLoan aml INNER JOIN
trans.AMTrns amt ON amt.tno = aml.tno AND   aml.PId = 10
)as X
join fin.ADetail ad on ad.IAccno=x.IAccno
join fin.ProductDetail pd on pd.pid=ad.pid
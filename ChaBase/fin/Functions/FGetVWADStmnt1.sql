CREATE FUNCTION [fin].[FGetVWADStmnt1]
	(
		@IACCNO NUMERIC,
		@FDATE SMALLDATETIME
	)


RETURNS  TABLE 
---MARKED ENCRYPTION  	
AS
return(


--1
SELECT     IAccno, tdate,  vdate, tno,
                          --(SELECT     dbo.Gettdesc(tno)) 
'' notes
 , dramt, cramt
FROM         trans.AMTrns
WHERE   (  (dramt <> 0) or (cramt <> 0)) AND (IAccno =@iaccno) and vdate  < @fdate 


Union
--2

SELECT     IAccno, TDATE, TDATE as Vdate, tno,
                          'Pending Withdraw'  AS notes, dramt, cramt
FROM         Trans.ASTrns
WHERE     (dramt > 0) AND (IAccno  =@iaccno) and tdate   < @fdate 


UNION

--3
SELECT     TIaccno,   vdate as TDATE,  Vdate, Trxno AS tno, 'Int. Cap' AS notes, 0 AS dramt, IntAmt AS Cramt
FROM         fin.AintCap
WHERE     isslf = 1 and Fiaccno is null and  TIaccno=@iaccno and vdate  < @fdate 

UNION

SELECT     TIaccno, Vdate AS tdate,  vdate, Trxno AS tno, 'Tax Ded.' AS notes, Tax AS dramt, 0 AS Cramt
FROM         fin.AintCap
WHERE     isslf = 1 and Fiaccno is null and tax<>0  and  TIaccno=@iaccno and vdate < @fdate 

union
--4

SELECT     TIaccno, vdate as  tdate,  vdate, Trxno AS tno, 'Int. Trns From '+ (select fin.Getacno (Fiaccno)) as  notes, 0 AS dramt, Cramt
FROM         fin.AintCap
WHERE     isslf = 0 and Fiaccno is not null  and  TIaccno=@iaccno and vdate   < @fdate 
union

--5
SELECT     FIaccno,   vdate as TDATE,  Vdate, Trxno AS tno, 'Int. Post: '+ cast(intamt as nvarchar(10)) + char(13) + char(10) + 'Tax : ' +cast(tax as nvarchar(10))+  char(13) + char(10)+ 'Int. Trns To '+ (select fin.Getacno (Tiaccno))+ ': '+ cast(cramt as nvarchar(10)) AS notes, 0 AS dramt, 0 AS Cramt
FROM         fin.AintCap
WHERE     isslf = 0 and Fiaccno is not null  and  fIaccno=@iaccno and vdate  < @fdate 



Union
--6

SELECT     FIaccno AS Iaccno,  tdate,  vdate, Tno, 'Transfer To ' +
                          (SELECT     fin.Getacno(Tiaccno))+ ' By Chq# '+ cast(slpno as nvarchar) AS notes, Amt AS dramt, 0 AS Cramt
FROM         IChkDep
WHERE     (Ttype = 1)  and  fIaccno=@iaccno and vdate < @fdate 

union

--7
SELECT     TIaccno AS Iaccno, tdate,  vdate, Tno, 'Transfer From ' +
                          (SELECT     fin.Getacno(Fiaccno))+ ' By Chq# '+ cast(slpno as nvarchar) AS notes, 0 AS dramt, amt AS Cramt
FROM         IChkDep
WHERE     (Ttype = 1)  and  TIaccno=@iaccno and vdate < @fdate 

Union
--8

SELECT     FIaccno AS Iaccno,  tdate,  vdate, Tno, 'Pending Chq Dep To ' +
                          (SELECT     fin.Getacno(Tiaccno))+ ' By Chq# '+ cast(slpno as nvarchar) AS notes, Amt AS dramt, 0 AS Cramt
FROM         IChkDep
WHERE     (Ttype = 0)  and  fIaccno=@iaccno and vdate   < @fdate



union
--9
/*
SELECT     IAccno , tdate , tdate+4 as vdate,rno ASTno, 'OBC Chq No.' + chqno + ' Clerance pending' As notes, 0 as dramt,camount AS cramt
FROM         AMClearance
where chqstate in (1,2,3)  and  Iaccno= 1042 and tdate + 4   < '3/25/2011'
Union
*/
--10
SELECT     IAccno,  tdate, fdate AS vdate, Tno, 'OBC Chq No:' + chqno + ' Cleareance made' AS notes, 0 AS dramt, camount AS cramt
FROM         fin.AMClearanceH
WHERE     (chqstate = 4)  and  Iaccno=@iaccno and fdate  < @fdate

union
--11
/* --- Records deleted from amclearance/amclearanceH while chqstate = 5
SELECT     IAccno , fdate as tdate , fdate as vdate,rno AS Tno, 'OBC Chq No.' + chqno + ' Rejected' As notes, camount as dramt,0 AS cramt
FROM         AMClearanceHR
where chqstate = 5 and  Iaccno=@iaccno and fdate  < @fdate

union 

SELECT     IAccno , tdate , fdate as vdate,rno AS Tno, 'OBC Chq No.' + chqno + ' posted' As notes, 0 as dramt,camount AS cramt
FROM         AMClearanceHR
where chqstate = 5 and  Iaccno=@iaccno and fdate  < @fdate

Union
*/
--12
SELECT     ChgSTrnx.Iaccno, ChgSTrnx.Tdate AS  tdate , ChgSTrnx.Tdate AS vdate, ChgSTrnx.TrnxNo AS tno, ChargeDetail.ChgName AS notes, ChgSTrnx.Amt AS dramt, 
                      0 AS cramt
FROM         Trans.ChgSTrnx INNER JOIN
                      fin.ChargeDetail ON ChgSTrnx.ChgId = ChargeDetail.ChgID
WHERE     (ChgSTrnx.Iaccno IS NOT NULL)
AND (ChgSTrnx.Iaccno =@iaccno ) and ChgSTrnx.ChgId not in  (SELECT     ChgID
FROM         fin.ChargeDetail ) and ChgSTrnx.tdate < @fdate 




union
--13
SELECT     IAccNo,   Tdate, tdate AS vdate, TransNo, 'Frx. Dep ' +
                          (SELECT     Symbol
                            FROM          fin.FrxCurrency
                            WHERE      (CurrID = ForeignBuySell.incurr)) AS notes, 0 AS dramt, CrAmt
FROM         fin.ForeignBuySell
WHERE     (IAccNo IS NOT NULL) and  Iaccno=@iaccno and tdate < @fdate 
)
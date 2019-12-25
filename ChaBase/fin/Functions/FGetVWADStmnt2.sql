CREATE FUNCTION [fin].[FGetVWADStmnt2]
	(
		@IACCNO NUMERIC,
		@FDATE SMALLDATETIME ,
		@TDATE SMALLDATETIME
	)
RETURNS  TABLE 
---MARKED ENCRYPTION 
AS
return(


	--1
	SELECT    AMTrns.IAccno, tdate,  vdate, tno,
							  --(SELECT     dbo.Gettdesc(tno)) 
	 'notes'= case 
	when dramt<>0 and isslp=1 AND (ttype = 2)and  AMTrns.brnhno = ADetail.BrchID  then 'Widr by '+ notes + case when slpno<>0 then (' via slp# '+cast(slpno as nvarchar)) else '' end
	when dramt<>0 and isslp=1 AND (ttype = 2) and  AMTrns.brnhno <> ADetail.BrchID then 'ABBS Widr by '+ notes + case when slpno<>0 then (' via slp# '+cast(slpno as nvarchar)) else '' end
	when dramt<>0 and isslp=1 AND (ttype in( 5,10)) then 'Transfer To '+ notes + case when slpno<>0 then (' via slp# '+cast(slpno as nvarchar)) else ''end
	when dramt<>0 and isslp=0 AND (ttype = 2)and AMTrns.brnhno = ADetail.BrchID then 'Widr by '+ notes + case when slpno<>0 then (' via chk# '+cast(slpno as nvarchar)) else '' end
	when dramt<>0 and isslp=0 AND (ttype = 2)and AMTrns.brnhno <> ADetail.BrchID then 'ABBS Widr by '+ notes + case when slpno<>0 then (' via chk# '+cast(slpno as nvarchar)) else '' end
	when dramt<>0 and isslp=0 AND (ttype = 1)then 'Widr by '+ notes + case when slpno<>0 then (' via chk# '+cast(slpno as nvarchar)) else '' end
	when dramt<>0 and isslp=1 AND (ttype = 1)then 'Widr by '+ notes + case when slpno<>0 then (' via slp# '+cast(slpno as nvarchar)) else '' end

	when  (cramt< > 0) AND (Isslp = 1) AND (ttype = 2) and AMTrns.brnhno = ADetail.BrchID  then 'Dep by '+ notes + case when slpno<>0 then (' via slp# '+cast(slpno as nvarchar)) else ''  end
	when  (cramt< > 0) AND (Isslp = 1) AND (ttype = 2)and AMTrns.brnhno <> ADetail.BrchID  then 'ABBS Dep by '+ notes + case when slpno<>0 then (' via slp# '+cast(slpno as nvarchar))else '' end

	when  (cramt <> 0) AND (Isslp = 1) AND (ttype in  (5,11)) then 'Transfer From '+ notes + case when slpno<>0 then (' via slp# '+cast(slpno as nvarchar) )  else '' end
	when  (cramt <> 0) AND (Isslp = 0) AND (ttype = 2)and AMTrns.brnhno = ADetail.BrchID then 'Dep by '+ notes  + case when slpno<>0 then (' via slp# '+cast(slpno as nvarchar)) else '' end
	when  (cramt <> 0) AND (Isslp = 0) AND (ttype = 2)and AMTrns.brnhno <> ADetail.BrchID then 'ABBS Dep by '+ notes  + case when slpno<>0 then (' via slp# '+cast(slpno as nvarchar)) else '' end
	when Cramt<>0 and isslp=0 AND (ttype = 1)then 'Dep by '+ notes + case when slpno<>0 then (' via chk# '+cast(slpno as nvarchar)) else '' end
	when Cramt<>0 and isslp=1 AND (ttype = 1)then 'Dep by '+ notes + case when slpno<>0 then (' via slp# '+cast(slpno as nvarchar)) else '' end
	end 
	,
	 dramt, cramt
	FROM         trans.AMTrns inner join adetail on amtrns.iaccno=adetail.iaccno
	WHERE   (  (dramt <> 0) or (cramt <> 0)) AND (AMTrns.IAccno =@iaccno) and vdate  between @fdate and @tdate


	Union
	--2

	SELECT     IAccno, TDATE, TDATE as Vdate, tno,
							  'Pending Withdraw'  AS notes, dramt, cramt
	FROM         trans.ASTrns
	WHERE     (dramt > 0) AND (IAccno  =@iaccno) and tdate  between @fdate and @tdate


	UNION

	--3
	SELECT     TIaccno,   vdate as TDATE,  Vdate, Trxno AS tno, 'Int. Cap' AS notes, 0 AS dramt, IntAmt AS Cramt
	FROM         fin.AintCap
	WHERE     isslf = 1 and Fiaccno is null and  TIaccno=@iaccno and vdate  between @fdate and @tdate and INtamt>0
	union
	SELECT     TIaccno,   vdate as TDATE,  Vdate, Trxno AS tno, 'Overdrawn Service Charge' AS notes, abs(IntAmt) AS dramt, 0 AS Cramt
	FROM         fin.AintCap
	WHERE     isslf = 1 and Fiaccno is null and  TIaccno=@iaccno and vdate  between @fdate and @tdate  and INtamt<0

	UNION

	SELECT     TIaccno, Vdate AS tdate,  vdate, Trxno AS tno, 'Tax Ded.' AS notes, Tax AS dramt, 0 AS Cramt
	FROM         fin.AintCap
	WHERE     isslf = 1 and Fiaccno is null and tax<>0  and  TIaccno=@iaccno and vdate  between @fdate and @tdate

	union
	--4

	SELECT     TIaccno, vdate as  tdate,  vdate, Trxno AS tno, 'Int. Trns From '+ (select fin.Getacno (Fiaccno)) as  notes, 0 AS dramt, Cramt
	FROM         fin.AintCap
	WHERE     isslf = 0 and Fiaccno is not null  and  TIaccno=@iaccno and vdate  between @fdate and @tdate
	union

	--5
	SELECT     FIaccno,   vdate as TDATE,  Vdate, Trxno AS tno, 'Int. Post: '+ cast(intamt as nvarchar(10)) + char(13) + char(10) + 'Tax : ' +cast(tax as nvarchar(10))+  char(13) + char(10)+ 'Int. Trns To '+ (select fin.Getacno (Tiaccno))+ ': '+ cast(cramt as nvarchar(10)) AS notes, 0 AS dramt, 0 AS Cramt
	FROM         fin.AintCap
	WHERE     isslf = 0 and Fiaccno is not null  and  fIaccno=@iaccno and vdate  between @fdate and @tdate



	Union
	--6

	SELECT     FIaccno AS Iaccno,  tdate,  vdate, Tno, 'Transfer To ' +
							  (SELECT     fin.Getacno(Tiaccno))+ ' By Chq# '+ cast(slpno as nvarchar) AS notes, Amt AS dramt, 0 AS Cramt
	FROM         IChkDep
	WHERE     (Ttype = 1)  and  fIaccno=@iaccno and vdate  between @fdate and @tdate

	union

	--7
	SELECT     TIaccno AS Iaccno, tdate,  vdate, Tno, 'Transfer From ' +
							  (SELECT     fin.Getacno(Fiaccno))+ ' By Chq# '+ cast(slpno as nvarchar) AS notes, 0 AS dramt, amt AS Cramt
	FROM         IChkDep
	WHERE     (Ttype = 1)  and  TIaccno=@iaccno and vdate  between @fdate and @tdate

	Union
	--8

	SELECT     FIaccno AS Iaccno,  tdate,  vdate, Tno, 'Pending Chq Dep To ' +
							  (SELECT     fin.Getacno(Tiaccno))+ ' By Chq# '+ cast(slpno as nvarchar) AS notes, Amt AS dramt, 0 AS Cramt
	FROM         IChkDep
	WHERE     (Ttype = 0)  and  fIaccno=@iaccno and vdate  between @fdate and @tdate



	union
	/*
	--9
	SELECT     IAccno , tdate , tdate+4 as vdate,rno ASTno, 'OBC Chq No.' + chqno + ' Clerance pending' As notes, 0 as dramt,camount AS cramt
	FROM         AMClearance
	where chqstate in (1,2,3)  and  Iaccno=@iaccno and tdate  between @fdate and @tdate
	Union
	*/

	--10
	SELECT     IAccno,  tdate, fdate AS vdate, Tno, 'OBC Chq No:' + chqno + ' Cleareance made' AS notes, 0 AS dramt, camount AS cramt
	FROM         fin.AMClearanceH
	WHERE     (chqstate = 4)  and  Iaccno=@iaccno and fdate  between @fdate and @tdate


	union

	/*
	--11

	SELECT     IAccno , fdate as tdate , fdate as vdate,rno AS Tno, 'OBC Chq No.' + chqno + ' Rejected' As notes, camount as dramt,0 AS cramt
	FROM         AMClearanceHR
	where chqstate =5 and  Iaccno=@iaccno and fdate  between @fdate and @tdate

	union 

	SELECT     IAccno , tdate , fdate as vdate,rno AS Tno, 'OBC Chq No.' + chqno + ' posted' As notes, 0 as dramt,camount AS cramt
	FROM         AMClearanceHR
	where chqstate =5 and  Iaccno=@iaccno and fdate  between @fdate and @tdate
	Union
	*/


	--12
	SELECT     ChgSTrnx.Iaccno, ChgSTrnx.Tdate AS  tdate , ChgSTrnx.Tdate AS vdate, ChgSTrnx.TrnxNo AS tno, ChargeDetail.ChgName AS notes, ChgSTrnx.Amt AS dramt, 
						  0 AS cramt
	FROM         trans.ChgSTrnx INNER JOIN
						  fin.ChargeDetail ON ChgSTrnx.ChgId = ChargeDetail.ChgID
	WHERE     (ChgSTrnx.Iaccno IS NOT NULL)
	AND (ChgSTrnx.Iaccno =@iaccno ) and ChgSTrnx.ChgId not in  (SELECT     ChgID
	FROM         fin.ChargeDetail) and ChgSTrnx.tdate  between @fdate and @tdate




	union
	--13
	SELECT     IAccNo,   Tdate, tdate AS vdate, TransNo, 'Frx. Dep ' +
							  (SELECT     Symbol
								FROM          fin.FrxCurrency
								WHERE      (CurrID = ForeignBuySell.incurr)) AS notes, 0 AS dramt, CrAmt
	FROM         fin.ForeignBuySell
	WHERE     (IAccNo IS NOT NULL) and  Iaccno=@iaccno and tdate  between @fdate and @tdate
)
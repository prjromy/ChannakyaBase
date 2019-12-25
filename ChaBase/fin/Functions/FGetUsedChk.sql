
--Select * From FGetUsedChk() Where IAccNo = 23 And ChqNo = ?
CREATE FUNCTION [fin].[FGetUsedChk]()
Returns Table
As
Return
SELECT     FIaccno AS 'IAccno', SlpNo AS 'ChqNo'
FROM         fin.IChkDep
Where Ttype In (50,51)
UNION
SELECT     IAccno, SLPNO AS 'ChqNo'
FROM         trans.ASTRNS
WHERE     ISSLP = 0
UNION
SELECT     IAccno, SLPNO AS 'ChqNo'
FROM         trans.AMTRNS
WHERE     ISSLP = 0
--Union 
--Select IAccNo,ChkNo From AChkU
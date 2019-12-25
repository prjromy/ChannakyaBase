--Select * From FGetUsedChk() Where IAccNo = 23 And ChqNo = ?



CREATE FUNCTION mig.FGetUsedChk()



Returns Table



As



Return



SELECT     FIaccno AS 'IAccno', SlpNo AS 'ChqNo',Tno



FROM         nimbus.dbo.IChkDep



Where Ttype In (0,1)



UNION



SELECT     IAccno, SLPNO AS 'ChqNo',tno



FROM          nimbus.dbo.ASTRNS



WHERE     ISSLP = 0 



UNION



SELECT     IAccno, SLPNO AS 'ChqNo',tno



FROM          nimbus.dbo.AMTRNS



WHERE     ISSLP = 0 and dramt!=0

Union 



SELECT IAccno,Chqno,tno FROM  nimbus.dbo.ACHQFGDPYMT WHERE  TSTATE IN(1,2,5,4)

union 

SELECT IAccno,Chkno,0 as tno FROM  nimbus.dbo.ICHKBOUNCE
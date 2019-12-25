CREATE Function [fin].[FGetReportLoanProductWiseInterestLog](@fromDate smalldatetime,@toDate smalldatetime, @branchId int)
returns table as return
select x.pid,x.PName,sum(x.LnInt)LnInt,sum(x.penOnPrin) PenOnPrin,sum(x.PenOnInt) PenOnInt,sum(x.IntOnInt) IntOnInt,sum(x.IntOnOther) IntOnOther  from 
(
SELECT     ProductDetail.PID, ProductDetail.PName,IntLog.Valued , 
          'Lnint'=case when IntLog.Valued in (0,5) then  SUM(IntLog.Intcal) else 0  end,
 'PenOnPrin'=case when IntLog.Valued in (3,8) then  SUM(IntLog.Intcal) else 0  end,
'PenOnint'=case when IntLog.Valued in (4,9) then  SUM(IntLog.Intcal) else 0  end,
'IntOnInt'=case when IntLog.Valued in (1,6) then  SUM(IntLog.Intcal) else 0  end,
'IntonOther'=case when IntLog.Valued in (2,7) then  SUM(IntLog.Intcal) else 0  end,
SUM(IntLog.Intcal)tot
FROM         fin.IntLog INNER JOIN
                      fin.ADetail ON IntLog.IAccno = ADetail.IAccno INNER JOIN
                      fin.ProductDetail ON ADetail.PID = ProductDetail.PID INNER JOIN
                      fin.SchmDetails ON ProductDetail.SDID = SchmDetails.SDID
WHERE     (SchmDetails.SType = 1) AND (IntLog.Tdate BETWEEN @fromDate AND @toDate ) and (ADetail.BrchID = @branchId)
GROUP BY ProductDetail.PID, ProductDetail.PName,IntLog.Valued 
) x
group by x.pid,x.PName
CREATE function [fin].[FgetReportChequeClearence](@fromDate smalldatetime,@toDate smalldatetime,@branchID int)

returns table as return

(

select ad.Accno as AccountNumber,ad.Aname as AccountName,am.bankname,am.payee,am.camount,am.VDate, CONVERT(date,am.cdate)

            as cdate from fin.ADetail ad join fin.AMClearance am on ad.IAccno=am.IAccno where 

          

              am.cdate BETWEEN @fromDate and @toDate and am.BrchId=@branchID

			  )
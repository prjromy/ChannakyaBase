CREATE function [fin].[FgetReportChequeWithdraw](
               @FDate SMALLDATETIME,
				@TDate SMALLDATETIME,
				@BranchId INT=null)
returns table as return
(

 SELECT AMTrns.tdate as TransactionDate , 
ADetail.Accno as AccountNumber,
ADetail.Aname as AccountName,
 AMTrns.brnhno as branchId, 
 AMTrns.slpno as ChequeNo,
 
  AMTrns.dramt as Amount, 
  AMTrns.notes as Notes 
  FROM  trans.AMTrns INNER JOIN  fin.ADetail ON AMTrns.IAccno = ADetail.IAccno 
  where dramt <>0  and isslp=0 and brnhno = COALESCE(@branchId,AMTrns.brnhno)  and tdate between @FDate and @TDate
 
 )
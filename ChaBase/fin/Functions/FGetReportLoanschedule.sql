  create function [fin].[FGetReportLoanschedule](@IAccno int)
  returns table  


  as return(
select *,(isnull(schPrin,0))-(isnull(pdPrin,0)) RemainingPrin, (isnull(calcInt,0))-(isnull(pdInt,0)) RemainingInt from fin.ALSch where IAccno=@IAccno)
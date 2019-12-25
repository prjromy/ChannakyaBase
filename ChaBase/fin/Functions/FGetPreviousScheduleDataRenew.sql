 CREATE function [fin].[FGetPreviousScheduleDataRenew](@IAccno int)

  returns table


  as return(

	 select CONVERT(bit,1) as PreviousYear,  
	-- isnull(sum(pdprin),0)  PrincipalInstall, 
	-- isnull(sum(pdint),0) InterestInstall,
	
	 isnull(sum(schprin),0)-isnull(sum(pdprin),0) as Amount,
	 isnull(sum(calcInt),0)-isnull(sum(pdInt),0) as InterestInstall,
	 isnull(sum(calcInt),0)-isnull(sum(pdInt),0)  as TotalInstall,
	  0.00 As Balance

	 from fin.ALSch group by IAccno having IAccno=@IAccno


			)
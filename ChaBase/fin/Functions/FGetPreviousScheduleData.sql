
 CREATE function [fin].[FGetPreviousScheduleData](@IAccno int)

  returns table


  as return(

	 select CONVERT(bit,1) as PreviousYear,  
	 0.00 as  PrincipalInstall, 

	 0.00 as TotalInstall,
	isnull(sum(calcInt)-sum(pdInt),0) as InterestInstall,
	 isnull(sum(schprin),0)-isnull(sum(pdprin),0) as Balance
	 
	 from fin.ALSch group by IAccno having IAccno=@IAccno


			)
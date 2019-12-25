
CREATE function [fin].[FGetReportLoanScheduleList](@iaccno int)
returns table
as return(
SELECT     Iaccno,Schdate,IsNull(schprin,0)+IsNull(schint,0) as 'InstallmentAmount',schprin as 'Principal',schint as 'Interest',balprin as 'Balance', dbo.fgetdatebs(SchDate) AS 'NepDate'		
FROM         fin.ALSch
where iaccno=@iaccno
)
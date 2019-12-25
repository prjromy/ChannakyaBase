
create function [fin].[FGetFidOtherLoanByPID](@PID int)
returns int
as 
begin
	return (select Fid from acc.FinAcc where fid in (select Fid+9 from fin.ProductDetail where Pid=@PID))
end
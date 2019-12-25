
CREATE FUNCTION [fin].[FGetReportAccOpnByCollector]
(
	@BrchID Int,
	@FrmDate SmallDateTime,
	@ToDate SmallDatetime,
	@Status Int
)
Returns @AccOpn Table (Name NVarchar(500),BroughtBy Int,NoOfCustomers Numeric,PName NVarchar(500),PCount Numeric)
As
Begin
If @Status = 0
Begin
	Insert Into @AccOpn

	select EmployeeName as Name,UserId as BroughtBy,COUNT(ref.IAccNo) AS NoOfCustomers,pn.PName,count(pn.PName) as PCount from fin.FGetAllUsersWithDesignation() fn  
	INNER JOIN fin.ReferenceInfo ref ON fn.userid=  ref.BroughtBy 
	INNER JOIN fin.ADetail ad ON ref.IAccNo = ad.IAccno
		INNER JOIN fin.ProductDetail pn ON ad.PID=pn.PID
	WHERE (ref.RType = 2) -- and 	fn.PId= 2009
	AND (ad.RDate BETWEEN @FrmDate AND @ToDate) And ad.BrchID = @BrchID
	GROUP BY ref.BroughtBy,EmployeeName,UserId,pn.PName
End
else 
Begin
	Insert Into @AccOpn
	select EmployeeName as Name,UserId BroughtBy,COUNT(ref.IAccNo) AS NoOfCustomers,pn.PName,count(pn.PName) as PCount from fin.FGetAllUsersWithDesignation() fn  
	INNER JOIN fin.ReferenceInfo ref ON fn.userid=  ref.BroughtBy 
	INNER JOIN fin.ADetail ad ON ref.IAccNo = ad.IAccno
	INNER JOIN fin.ProductDetail pn ON ad.PID=pn.PID
	WHERE (ref.RType = 2) -- and fn.PId= 2009
	AND (ad.RDate BETWEEN @FrmDate AND @ToDate) And ad.BrchID = @BrchID
	And ad.AccState = 1 
	GROUP by ref.BroughtBy,EmployeeName,UserId,pn.PName
End
Return
End
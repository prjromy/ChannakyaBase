-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [fin].[BranchEmployeeList] 
(	
	
)
RETURNS TABLE 
AS
RETURN 
(
	
select eb.MapId,eb.StartFrom,lb.BrnhNam,e.EmployeeName,e.EmployeeNo,d.DGName,eb.BranchId from fin.EmployeeVsBranch eb join  lg.Employees e on eb.EmpId=e.EmployeeId
left join LicenseBranch lb on lb.BrnhID=eb.BranchId
left join lg.Designation d on d.DesignationId=eb.CurrentRole 
)
create function ListBranch(@branchid nvarchar(100))
returns table 
as
return
(
--declare @BranchId nvarchar(100)


select * from fin.[LicenseBranch] 
 where .fin.[LicenseBranch].BrnhID in (Select Value From _FNSplit(@BranchId,','))

 )
CREATE FUNCTION  [mast].[GetPostedOn](



)

RETURNS Datetime AS
begin
declare @postedOn datetime

set @postedOn=(select GETDATE())
return @postedOn
end
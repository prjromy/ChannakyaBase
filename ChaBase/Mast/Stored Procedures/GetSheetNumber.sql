
CREATE PROCEDURE [Mast].[GetSheetNumber](@SheetNo int OUTPUT,@BranchId int,@FyId int)
---MARKED ENCRYPTION 
AS
begin
update  Mast.SheetNo set  CurrentNo=CurrentNo+1 where mast.SheetNo.BranchId=@BranchId and mast.SheetNo.FYId=@FyId
set @SheetNo=(select CurrentNo from mast.SheetNo where mast.SheetNo.BranchId=@BranchId and mast.SheetNo.FYId=@FyId)
select @SheetNo as SheetNo
end

CREATE PROCEDURE [Mast].[GetTransno](@TNO Bigint OUTPUT)
---MARKED ENCRYPTION 
AS
begin	
	if not exists(select * from mast.uttno)
	begin
		set @TNO = 1
		insert into mast.uttno values(@TNO)
	end
	else
	begin
		set @TNO = (Select Tno from mast.uttno) + 1
		Update mast.uttno set tno = @TNO
	end	
	select @TNO as uttno
	--set @TNO  = 100
	--select @TNO as uttno
end
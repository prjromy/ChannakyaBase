Create function test(@postedBy int)
returns @Temp table(userId int)
as
begin
	if @PostedBy  = 0
	begin
		insert into @Temp select userid from lg.[user]
	end
	else
	begin
		insert into @Temp values (@PostedBy)
	end
	return
end
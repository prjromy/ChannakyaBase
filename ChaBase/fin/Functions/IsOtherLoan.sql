
  CREATE function [fin].[IsOtherLoan](@Fid int)
  returns bit
  as
  begin
  declare @isOtherLoan bit
		if exists(select Fid from acc.FinAcc a inner join 
		acc.FinSys2 b on a.F2Type=b.F2id where F1id=248 and Fid=@Fid)
		begin
			set @isOtherLoan=1
		end
		else
		begin
			set @isOtherLoan=0
		end
		return @isOtherLoan
  end
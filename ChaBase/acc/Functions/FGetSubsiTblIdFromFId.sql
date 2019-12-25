
   Create Function [acc].FGetSubsiTblIdFromFId(@FId int)
   returns int
   as
   begin
		Declare @SubsiTableId int

		
		select @SubsiTableId=(select d.STBId from acc.FinAcc a inner join acc.subsivsfid b on a.Fid=b.Fid 
		 inner join acc.SubsiSetup c on b.SSId=c.SSId
		inner join acc.subsitable d on c.STBId=d.STBId where a.Fid=@FID)

		return @SubsiTableId


	
   end
create function [acc].[GenerateSubsiTblIdFromFID](@FID int)
   returns int as
   BEGIN
 
 select @FID=(select d.STBId from acc.FinAcc a inner join acc.subsivsfid b on a.Fid=b.Fid 
   inner join acc.SubsiSetup c on b.SSId=c.SSId
   inner join acc.subsitable d on c.STBId=d.STBId where a.Fid=@FID)
   return @FID
   END
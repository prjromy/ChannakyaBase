CREATE function [fin].[FGetCurrentDenoList](@userId int,@userLevel int,@currencyId int)



RETURNS TABLE



AS RETURN



(



select dt.DenoID,dt.CurrID,dt.Deno,dbal.DenoBalId as DenoBalId,isnull(dbal.UserId,@userId) as userId,dbal.Piece,dbal.UserLevel



 from fin.Denosetup dt



 left join fin.DenoBal  dbal  



on dt.DenoID = dbal.DenoId and dbal.UserId=@userId and dbal.UserLevel=@userLevel where dt.CurrID=@currencyId



)
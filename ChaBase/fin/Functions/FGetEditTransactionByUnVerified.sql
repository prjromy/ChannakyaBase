CREATE FUNCTION [fin].[FGetEditTransactionByUnVerified](@tNO decimal,@tDate DATETIME)







RETURNS TABLE 







AS







RETURN 







(







SELECT adetail.Iaccno,adetail.Accno,adetail.Aname, astrans.tdate,Usr.username as Username,astrans.IsSlp, astrans.slpno, company.BranchName,'Amt'=case when astrans.dramt=0 then astrans.cramt else astrans.dramt end, 







 fxcurr.CurrencyName,'IsDeposit'=case when astrans.dramt=0 then 1 else 0 end ,







  Usr.UserId,astrans.postedby as PostedBy,astrans.IsDeleted as IsDeleted   FROM trans.ASTrns astrans INNER JOIN fin.ADetail adetail ON astrans.IAccno = adetail.IAccno 







   INNER JOIN fin.CurrencyType fxcurr ON ADetail.CurrID = fxcurr.CTId 







   INNER JOIN Lg.Company company ON astrans.brnhno = company.CompanyId







    INNER JOIN LG.[User] Usr ON astrans.PostedBy = Usr.UserId where astrans.tno=@tNO  and astrans.tdate=@tDate







 )
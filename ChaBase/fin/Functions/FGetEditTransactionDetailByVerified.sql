CREATE FUNCTION [fin].[FGetEditTransactionDetailByVerified](@tNO decimal,@tDate DATETIME)



RETURNS TABLE 



AS



RETURN 



(



SELECT ADetail.Accno,Adetail.Iaccno, amtrans.tdate, amtrans.IsSlp, amtrans.slpno,'Amt'=case when amtrans.dramt=0 then amtrans.cramt else amtrans.dramt end,



 fxcurr.CurrencyName, company.BranchName,



  'IsDeposit'=case when amtrans.dramt=0 then 1 else 0 end ,Adetail.Aname,



  Usr.UserName  as Username, Usr.UserId,amtrans.postedby as PostedBy FROM trans.AMTrns amtrans



   INNER JOIN fin.ADetail adetail ON amtrans.IAccno = adetail.IAccno



    INNER JOIN fin.CurrencyType fxcurr ON adetail.CurrID = fxcurr.CTId  INNER JOIN



	 LG.Company company ON amtrans.brnhno = company.CompanyId



	  INNER JOIN LG.[User] Usr ON amtrans.PostedBy = Usr.UserId   where amtrans.tno=@tNO and amtrans.tdate=@tDate 



	  and (dramt > 0 or cramt > 0)







  



  )
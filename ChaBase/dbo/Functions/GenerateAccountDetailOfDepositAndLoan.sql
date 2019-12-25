CREATE function [dbo].[GenerateAccountDetailOfDepositAndLoan]
(
@ledgerId int
)
RETURNS TABLE 
AS  
RETURN   
(  
    select a.Fid,a.Fname,c.IAccno,c.Accno,c.Aname,ISNULL(d.OpeningBal,0) as OpeningBal
	 from acc.finacc a inner join fin.ProductDetail b
	 on a.fid=b.FID inner join fin.ADetail c on b.PID=c.PID
	 left join acc.SubsiVSOpeningBalance d on a.Fid=d.FId and d.SubsiId=c.IAccno
	  where a.fid=@ledgerId
	
 
);
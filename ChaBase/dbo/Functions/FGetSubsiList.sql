-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[FGetSubsiList] 
(	
	-- Add the parameters for the function here
	@STBId int 
)
RETURNS TABLE 
AS
RETURN 
(
	
select f.CID AS CustId,f.FName as FirstName,f.MName as MiddleName,f.LName as LastName from acc.SubsiTable a inner join acc.SubsiSetup b on a.STBId=b.STBId
inner join acc.SubsiVSFId c on b.SSId=c.SSId inner join acc.FinAcc d on c.Fid=d.Fid
inner join acc.SubsiDetail e on d.Fid=e.FId inner join cust.CustIndividual f on e.CId=f.CID where a.STBId=@STBId
)
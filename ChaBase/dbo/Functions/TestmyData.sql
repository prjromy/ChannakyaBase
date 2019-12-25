-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[TestmyData] 
(	

)
RETURNS TABLE 
AS
RETURN 
(
	-- Add the SELECT statement with parameter references here

   select C.FId as FIDs, C.PID as PIDs, c.Fname as Fnames from (
   
   select g.FId, g.PId, g.FName, sum(t.DebitAmount) as DebitAmount , sum(t.CreditAmount) as CreditAmount 
		from ( select b.fid, b.DebitAmount, b.CreditAmount
				from acc.Voucher1 a 
				inner join acc.Voucher2 b on a.V1Id=b.V1id 
				inner join acc.FinAcc f on f.Fid=b.Fid
				left outer join acc.voucher3 c on b.V2Id=c.V2Id
				left outer join acc.Voucher4 d on b.V2Id=d.V2Id
				left outer join acc.Voucher5 e on b.V2Id=e.V2Id 
				) t 
		left outer join acc.FinAcc g on g.Fid = t.Fid
		--where g.Fid=7 --in (select Fid from @Tmp)
		group by g.Fid, g.PId, g.Fname) c
)
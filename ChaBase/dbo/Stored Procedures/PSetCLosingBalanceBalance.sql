-- =============================================
-- Author:		Bishal Thapaliya
-- Create date:1/25/2018	
-- Description:	To Update the OpeningBalance when it is updated
-- =============================================
CREATE PROCEDURE [dbo].[PSetCLosingBalanceBalance]( 
@OPBal decimal(18,2),
@CLBal decimal(18,2),
@FId int,
@FYId int
)
AS
BEGIN
	update acc.VoucherBal set CLBal=@CLBal,OPBal=@OPBal where FId=@FId and FYId=@FYId
END
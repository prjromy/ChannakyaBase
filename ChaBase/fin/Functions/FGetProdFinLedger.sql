-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [fin].[FGetProdFinLedger]
(	
	
)
RETURNS TABLE 
AS
RETURN 
(
    select 0 as Id,'IonPA-Interest on Principle Account' as Text
     union
	select 1 as Id,'IonIA-Interest on Interest Accrued' as Text
		union 
	select 2 as Id, 'IonCA-Interest on other Account' as Text
	union 
	select 3 as Id,'PonPrA-Penalty  on Principle Accrued' as Text
	union
	select 4 as Id, 'PonIA-Penalty on Interest Accrued' as Text
	union 
	select 5 as Id, 'IonPR-Interest on Principle Receivable' as Text
		union 
	select 6 as Id, 'IonIR-Interest on Interest Receivable' as Text
	union 
	select 7 as Id,'IonCR-Interest on other Receivable' as Text

	union 
	select 8 as Id, 'PonPrR-Penalty on Principle Receivable' as Text
	union 
	select 9 as Id, 'PonIR-Penalty on Interest Receivable' as Text
		union
		 select 10 as Id,'PrinDr' as text
		 union 
	select 12 as Id,'RebateOnInterest' as Text
	union
	select 13 as Id, 'PrinCr' as Text
	union 
	select 14 as Id, 'OtherBal-Other loan balance' as Text
	union 
	select 17 as Id, 'RebateOnPenalty' as Text
	

	
)
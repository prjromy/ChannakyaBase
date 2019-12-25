

CREATE FUNCTION [dbo].[FGetPaymentModeDictionary]()

RETURNS TABLE 
AS
RETURN 
(
	
	select 1 as Id,'Equal Principal' as Text
			union
		select 2 as Id,'Equal Installment' as Text 
			union								
		select 3 as Id,'Customized Installment' as Text 
			
)
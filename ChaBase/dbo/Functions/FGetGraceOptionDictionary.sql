

CREATE FUNCTION [dbo].[FGetGraceOptionDictionary]()

RETURNS TABLE 
AS
RETURN 
(
	
	select 1 as Id,'No Grace' as Text
			union
		select 2 as Id,'In Days' as Text 
			union								
		select 3 as Id,'In Months' as Text 
		union								
		select 4 as Id,'Specified Date' as Text 
			
)
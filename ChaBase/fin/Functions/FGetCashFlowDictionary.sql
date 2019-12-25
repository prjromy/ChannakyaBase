CREATE function [fin].[FGetCashFlowDictionary]()
returns table 
as return
(
		select 1 as Id,'Vault->Cashier' as Text, 2007 as FDeg,2005 As TDeg
			union
		select 2 as Id,'Cashier->Vault' as Text , 2005 as FDeg,2007 As TDeg
			union								
		select 3 as Id,'Cashier->Teller' as Text, 2005 as FDeg,2006 As TDeg 
			union								
		select 4 as Id,'Teller->Cashier' as Text, 2006 as FDeg,2005 As TDeg 
			union							 
	   select 5 as Id,'Cashier->Cashier' as Text, 2005 as FDeg,2005 As TDeg 
			union							 	
		select 6 as Id,'Teller->Teller' as Text , 2006 as FDeg,2006 As TDeg
)
CREATE Function [dbo].[FGetLoanTransType]()
returns 
table
as
return
(
	select top 100 TType, [Description] from (
	select 10 as TType, 'PriDr' as [Description]
	union all
	select 11 as TType, 'OtherDr' as [Description]
	union all
	select 13 as TType, 'PriCr' as [Description]
	union all
	Select 14 as TType, 'OtherCr' as [Description]
	union all 
	select 12 as TType, 'RbtInt' as [Description]
	union all
	select 17 as TType, 'RbtPen' as [Description]
	union all
	select 0 as TType, 'intAPd' as [Description]
	union all
	select 5 as TType, 'intRPd' as [Description]
	union all
	select 1 as TType, 'IonIAPd' as [Description]
	union all
	select 6 as TType, 'IonIRPd' as [Description]
	union all
	select 2 as TType, 'IonCAPd' as [Description]
	union all
	select 7 as TType, 'IonCRPd' as [Description]
	union all
	select 3 as TType, 'PonPrAPd' as [Description]
	union all
	select 8 as TType, 'PonPrRPd' as [Description]
	union all
	select 4 as TType, 'PonIAPd' as [Description]
	union all
	select 9 as TType, 'PonIRPd' as [Description]) x 
	order by TType
)
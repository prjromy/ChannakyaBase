

-- =============================================

-- Author:		<Author,,Name>

-- Create date: <Create Date,,>

-- Description:	<Description,,>

-- =============================================

CREATE FUNCTION [fin].[FGetAccountNumberListA]

(	

	@currrencyId int,
	@branchId int,
	@productId int,
	@filterName varchar(150)
)

RETURNS TABLE 

AS

RETURN 

(



select

ad.Accno as AccountNumber,

f.Name as AccountName,

ad.IAccno as AccountId,

ad.Aname as AccountHolder,

f.CID as CustomerId,

acs.AccountState as AccountStatus,

ad.AccState,

f.ContactPerson,

f.Mobile,

ad.BrchID,

ad.PID,

ad.CurrID,

sd.SType,


 case sd.SType when 0 then 'Deposit Account' else 'Loan Account' end as AccountType,

isnull(p.HasCheque,0) HasCheque,

isnull(p.Withdrawal,0) Withdrawal,

isnull(p.MultiDep,0) MultiDep,

isnull(sd.HasDuration,0) HasDuration,

isnull(p.Nomianable,0) Nomianable,

isnull(sd.Revolving,0) Revolving,
ad.IRate as InterestRate

 from cust.FGetCustListForSearch() f

 join fin.AOfCust ac on ac.CID=f.CID

 join fin.ADetail ad on ad.IAccno=ac.IAccno

 join fin.AccountStatus acs on acs.AsId=ad.AccState

 join fin.ProductDetail p on ad.PID=p.PID

 join fin.SchmDetails sd on sd.SDID=p.SDID

 where ad.BrchID=@branchId and ad.CurrID=@currrencyId and ad.AccState<>6 and p.PID=Isnull(@productId,p.PID)

)
CREATE function [dbo].[MigrationFinAcc]()
returns table as
return
(
--select * from tempFinAcc where acode=7 --acode in (7,8,3)
select * ,22 as F2Id from tempFinAcc where FID=7 --acode in (7,8,3) 
union
select *,F2Id=case when FId=235 then 23 else 24 end from tempFinAcc where apid=7 --acode in (7,8,3)
union
select *,F2Id=25 from tempFinAcc where apid=255--this is scheme
union
select *,F2Id=26 from tempFinAcc where apid in (select FId from tempFinAcc where apid=255)--thi sis product
union
select *,F2Id=27 from tempFinAcc where FID=26--Interest Expenses
union
select *,F2Id=30 from tempFinAcc where FID=233--Interest Expenses on Deposit
union
select *,F2Id=32 from tempFinAcc where FID=89--Interest Payable on Deposit(LOCAL)
union
select *,F2Id=33 from tempFinAcc where FID=237--Interest Payable on Deposit(FOREIGN)
union
select *,F2Id=34 from tempFinAcc where FID=234--Interest Accured Payable on Deposit(LOCAL)
union
select *,F2Id=35 from tempFinAcc where FID=236--Interest Accured Payable on Deposit(FOREIGN)
union
select *,F2Id=41 from tempFinAcc where FID=241--Interest Receivable
union
select *,F2Id=44 from tempFinAcc where FID=37--Interest Income
union 
select *,F2Id=46 from tempFinAcc where FID=245--Interest Income on Loan
union
select *,F2Id=48 from tempFinAcc where FID=249--Other Loan Interest Income
union
select *,F2Id=49 from tempFinAcc where FID=243--Interest Suspense
union
select *,F2Id=52 from tempFinAcc where FID=247--Interest Rebate on Loan
union
select *,F2Id=36 from tempFinAcc where FID=238--Tax Local Currency
union
select *,F2Id=37 from tempFinAcc where FID=239--Tax Foreign Currency
union
select *,F2Id=42 from tempFinAcc where FID=242--Penalty Receivable
union
select *,F2Id=50 from tempFinAcc where FID=244--Penalty Suspense
union
select *,F2Id=47 from tempFinAcc where FID=246--Penalty Income on Loan
union
select *,F2Id=53 from tempFinAcc where FID=248--Penalty Rebate on Loan
union
select *,F2Id=43 from tempFinAcc where FID=250--Other Loan
union
select *,F2Id=28 from tempFinAcc where FID=17--Loans and Advances
union
select *,F2Id=107 from tempFinAcc where FID=251--Loan Assests
union
select *,F2Id=29 from tempFinAcc where acode=10 and apid=251----Loan Scheme
union
select *,F2Id=54 from tempFinAcc where acode=13 and apid in
(select FID from tempFinAcc where acode=10 and apid=251)----Loan Product
union
select *,F2Id=48 from tempFinAcc where FID=249--Other Loan Interest Income
union
select *,F2Id=3 from tempFinAcc where FID=12 or FID=13--Cash and Bank Balance
union
-----NOW FOR GROUP INSIDE THE ABOVE LEDGERS-LEVEL2-------------
--Interest Payable on Deposit(LOCAL)
select *,F2Id=65 from tempFinAcc where apid=89
union
--Interest Accured Payable
select *,F2Id=67 from tempFinAcc where apid=234
union

--Interest Receivable
select *,F2Id=73 from tempFinAcc where apid=241
union
--Interest Income
select *,F2Id=71 from tempFinAcc where apid=37
union
--Interest Income on Loan
select *,F2Id=78 from tempFinAcc where apid=245
union
--Other Loan Interest Income
select *,F2Id=77 from tempFinAcc where apid=249
union
--Interest Suspense
select *,F2Id=71 from tempFinAcc where apid=243
union
--Interest Rebate on Loan
select *,F2Id=79 from tempFinAcc where apid=247
union
--Tax Local
select *,F2Id=69 from tempFinAcc where apid=238
union
--TAX Foreign
select *,F2Id=70 from tempFinAcc where apid=239
union
--Penalty Receivable
select *,F2Id=74 from tempFinAcc where apid=242
union
--Penalty Suspense
select *,F2Id=72 from tempFinAcc where apid=244
union
--Penalty Income on Loan
select *,F2Id=76 from tempFinAcc where apid=246
union
--Penalty Rebate on Loan
select *,F2Id=80 from tempFinAcc where apid=248
union
--Other Loan
select *,F2Id=75 from tempFinAcc where apid=250
union
--Other Loan Interest Income
select *,F2Id=77 from tempFinAcc where apid=249
union

--------- Bank Balance----------------

select *,F2Id=case 
	when a.FID=103 then 12
	when a.FID=258 then 14
	when a.FID=259 then 21
	when a.FID=260 then 15
	else 4  end 
from tempFinAcc a  where apid=13
union
--A Class Banks
select *,F2Id=13 from tempFinAcc where apid=103
union
--B Class Banks
select *,F2Id=16 from tempFinAcc where apid=257
union
--C Class Banks
select *,F2Id=14 from tempFinAcc where apid=258
union
-------------------------------------------
--D Class Banks
select *,F2Id=62 from tempFinAcc where apid=259
union
--Finance Companies
select *,F2Id=17 from tempFinAcc where apid=260

-------------------------------------------------------------



union
--------------------Cash Balance------------------------
select *,F2Id=9
from tempFinAcc a where apid=12
union
--Transaction Cash
select *,F2Id=case when a.FID=253 then 85
when a.FID=254 then 86 end
  from tempFinAcc a where apid=252
  -------------------------------------------------------------------
  ---------------SHARE------------------------
  
  union
  select *,F2Id=91 from tempFinAcc where FID=5
  union
  select *,F2Id=92 from tempFinAcc where apid=5 and FID=42
  union
  select *,F2Id=94 from tempFinAcc where apid=5 and FID=46
  union
  select *,F2Id=95 from tempFinAcc where apid=5 and FID=47
  union
  select *,F2Id=96 from tempFinAcc where apid=5 and FID=48
  union
  select *,F2Id=97 from tempFinAcc where apid=5 and FID=49
  union
  select *,F2Id=99 from tempFinAcc where apid=5 and FID=61
  union
  select *,F2Id=93 from tempFinAcc where apid=42
  -----------------------------

  -----------------CHARGE--------------
  union
  select *,F2Id=60 from tempFinAcc where FID=40
  union
  select *,F2Id=61 from tempFinacc where FID=214

  -------------------------------
  --------REMITTANCE--------------
  union
  select *,F2Id=63 from tempFinAcc where FID=147
  union
  select *,F2Id=64 from tempFinAcc where apid=147
  ) 


  --use [Nimbus]
  ----select * from schmdetails where SDID=16
  ----select * from productdetail where PName like '%Instant%'
  --use [ChannakyabaseMigration]
  --select * from tempFinAcc where apid=120 or apid=121 or apid=122 or apid=1079 or apid=1088
  ----select * from tempFinAcc where FID=21 or FID=37 or FID=9
  --select * from tempFinAcc where aname like '%OD%'

CREATE proc  [fin].[PSetAccountNumberListA]    
(     
 @currrencyId int,    
 @branchId int,    
 @productId int=0,    
 @filterName varchar(150)=null,    
 @AccountNameOrNumber varchar(150)=null,    
 @accountType varchar(20)=null,    
 @pageNo int,    
 @pageSize int    
)    
AS    
 Set NoCount ON    

    /* Variable Declaration */    
    Declare @SQLQuery AS nVarchar(4000)  
    Declare @ParamDefinition AS NVarchar(2000)   
set @SQLQuery=    

'select ad.Accno as AccountNumber, 
f.Name as AccountName, 
ad.IAccno as AccountId, 
ad.Aname as AccountHolder,
acs.AccountState as AccountStatus,
ad.AccState,
f.ContactPerson,
f.Mobile,
ad.BrchID, 
ad.PID, 
ad.CurrID, 
sd.SType,
case sd.SType when 0 then ''DepositAccount'' else ''LoanAccount'' end as AccountType,    
 isnull(p.HasCheque,0) HasCheque, 
 isnull(p.Withdrawal,0) Withdrawal, 
 isnull(p.MultiDep,0) MultiDep, 
 isnull(sd.HasDuration,0) HasDuration, 
 isnull(p.Nomianable,0) Nomianable,
 isnull(sd.Revolving,0) Revolving,
 ad.IRate as InterestRate   
 from cust.FGetCustListForSearch() f join fin.AOfCust ac on ac.CID=f.CID    
 join fin.ADetail ad on ad.IAccno=ac.IAccno join fin.AccountStatus acs on acs.AsId=ad.AccState  
 join fin.ProductDetail p on ad.PID=p.PID  join fin.SchmDetails sd on sd.SDID=p.SDID    
 where (ad.BrchID=@branchId) and (ad.CurrID=@currrencyId) '    
 if @productId!=0    
 set @SQLQuery=@SQLQuery+' and (p.PID=@productId)'    
 if @AccountNameOrNumber is not null    
  set @SQLQuery=@SQLQuery+' and (ad.Aname like ''%''+@AccountNameOrNumber+''%'' or ad.Accno like ''%''+@AccountNameOrNumber+''%'')'
  if @filterName='Nominee'    
  set @SQLQuery=@SQLQuery+' and ad.AccState<>6 and ad.AccState<>8 and ad.AccState<>7 and p.Nomianable=1 and sd.HasDuration=0' 
   if @filterName='AllowCheque'    
  set @SQLQuery=@SQLQuery+' and ad.AccState<>6 and ad.AccState<>8 and ad.AccState<>7and p.HasCheque=1'    
  if @filterName='FixedAccountOnly'    
  set @SQLQuery=@SQLQuery+' and ad.AccState<>6 and isnull(p.MultiDep,0)=0 and sd.HasDuration=1 '
  --isnull(p.Withdrawal,0)=0 is removed for fixed account with discussion from sir 

  if @filterName = 'WithoutFixedDeposit'
    set @SQLQuery=@SQLQuery+' and ad.AccState = 1 and not ( isnull(p.MultiDep,0)=0 and sd.HasDuration=1 )' 
   if @filterName='UnverifiedAccountList'    
   set @SQLQuery=@SQLQuery+'  and ad.AccState=6'    
    if @filterName='AccountClose'    
   set @SQLQuery=@SQLQuery+'and ad.AccState<>6 and ad.AccState<>7'  
    if @filterName='DepositAccept'  
 set @SQLQuery=@SQLQuery+'and ad.AccState<>6 and ad.AccState<>8'  
    if @filterName='AdjustmentAccept'  
 set @SQLQuery=@SQLQuery+'and ad.AccState<>7 and ad.AccState<>6 and ad.AccState<>8'  
    if @filterName='WithdrawAccept'  
 set @SQLQuery=@SQLQuery+'and ad.AccState<>7 and ad.AccState<>8  and AccState<>6'
 if @filterName='ImageAccept'  
 set @SQLQuery=@SQLQuery+'and ad.AccState<>7 and ad.AccState<>8  and AccState<>6'
 if @filterName='LoanAccept'
 set @SQLQuery=@SQLQuery+'and ad.AccState<>6 and ad.AccState<>8  and ad.AccState<>7'

  if @accountType='Loan'    
  set @SQLQuery=@SQLQuery+'and sd.SType=1'    
  if @accountType='Normal'    
  set @SQLQuery=@SQLQuery+'and sd.SType=0'    

    if @accountType='Other'    
  set @SQLQuery=@SQLQuery+'and sd.SType in (0,1)'    
   DECLARE @SQL NVARCHAR(4000)    
set @SQL=';WITH Account_Table AS( (' + @SQLQuery + ') ), Count_CTE AS (  SELECT COUNT(*) AS[TotalCount]   FROM Account_Table )    
SELECT *   FROM Account_Table, Count_CTE   ORDER BY Account_Table.AccountName     OFFSET ((@pageNo  - 1) *   @pageSize) ROWS      FETCH NEXT @pageSize  ROWS ONLY'    

    Set @ParamDefinition =      '@currrencyId int,   @branchId int,         @productId int,  @filterName varchar(150),   @AccountNameOrNumber nvarchar(150),    
     @accountType varchar(20),     @pageNo int,    @pageSize int'    
    /* Execute the Transact-SQL String with all parameter value's     
       Using sp_executesql Command */    
    Execute sp_Executesql     
	@SQL, @ParamDefinition, @currrencyId, @branchId,    
	@productId, @filterName,  @AccountNameOrNumber,    
    @accountType,@pageNo , @pageSize     
    If @@ERROR <> 0 GoTo ErrorHandler    
    Set NoCount OFF   
    Return(0)  
		ErrorHandler:    
    Return(@@ERROR)
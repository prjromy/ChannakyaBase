

-- =============================================

-- Author:		<Author,,Name>

-- Create date: <Create Date,,>

-- Description:	<Description,,>

-- =============================================

CREATE proc  [fin].[PSetAccountNumberListB]
(	
	@AccountNumber varchar(150),
	@filterName    varchar(150),	
	@accountType    varchar(20)	
)
AS
 Set NoCount ON
    /* Variable Declaration */
    Declare @SQLQuery AS nVarchar(4000)
    Declare @ParamDefinition AS NVarchar(2000) 

set @SQLQuery=
 'select ad.IAccno IAccno,ad.Accno Accno,ad.aname as AccountName from  fin.ADetail ad  
	join fin.ProductDetail p on ad.PID=p.PID
	join fin.SchmDetails sd on sd.SDID=p.SDID
 where  ad.AccState<>6 and ad.Accno like ''%''+@AccountNumber+''%'''

 if @filterName='Nominee'

  set @SQLQuery=@SQLQuery+' and p.Nomianable=1'
   if @filterName='AllowCheque'

  set @SQLQuery=@SQLQuery+' and p.HasCheque=1'

  if @filterName='FixedAccountOnly'

  set @SQLQuery=@SQLQuery+' and p.Withdrawal=0 and p.MultiDep=0 and sd.HasDuration=1'

  if @filterName='AccountClose'
   set @SQLQuery=@SQLQuery+' and ad.AccState<>7'

  if @accountType='Loan'
  set @SQLQuery=@SQLQuery+'and sd.SType=1'

  if @accountType='Normal'
  set @SQLQuery=@SQLQuery+'and sd.SType=0'
   
  				

    Set @ParamDefinition =  '@AccountNumber varchar(150),
	                         @filterName    varchar(150),	
	                         @accountType    varchar(20)'
								 
								 								 								 								 								 								 								  
    /* Execute the Transact-SQL String with all parameter value's 
       Using sp_executesql Command */
    Execute sp_Executesql     @SQLQuery, 
                @ParamDefinition, 
                @AccountNumber,
                @filterName ,  
                @accountType  
                                								                             
    If @@ERROR <> 0 GoTo ErrorHandler
    Set NoCount OFF
    Return(0)
  
ErrorHandler:
    Return(@@ERROR)
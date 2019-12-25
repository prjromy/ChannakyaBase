   CREATE function Rejected(@tno int)

   returns table 

   as return(

   

   select CashFlow.Brhid,CashFlow.FUserid as UserId1,CashFlow.UserID as UserId2,CashFlow.TType,CashFlow.TNO,CashFlow.AcceptedOn,CashFlow.Status from fin.CashFlow where TNO=@tno and CashFlow.Status=24

   )
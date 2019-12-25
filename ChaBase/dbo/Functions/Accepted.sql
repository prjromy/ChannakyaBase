 create function Accepted(@tno int)
   returns table 
   as return(
   
   select CashFlow.Brhid,CashFlow.FUserid,CashFlow.UserID,CashFlow.TType,CashFlow.TNO,CashFlow.AcceptedOn,CashFlow.Status from fin.CashFlow where TNO=@tno and CashFlow.Status=22
   )
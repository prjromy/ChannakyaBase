  CREATE function [fin].[FGetReportInternalChequeDeposit](@fromDate smalldatetime,@toDate smalldatetime,@branchId int=null,@ttype int=null)
  returns table as return
  (
   SELECT  IChkDep.Tdate,
    ProductDetail.PName AS 'DrProduct', 
    ProductDetail_1.PName AS 'CrProduct', 
    ADetail.Accno AS 'DrAccountNo', 
    ADetail.Aname AS 'DrName',
    ADetail_1.Accno AS 'CrAccountNo', 
    ADetail_1.Aname AS 'CrName',
    IChkDep.Tno AS 'TrnxNo', 
	IChkDep.Amt AS Amount, 
	IChkDep.Ttype, 
	IChkDep.BrchID FROM   fin.ADetail 
	INNER JOIN fin.IChkDep ON ADetail.IAccno = IChkDep.FIaccno
	 INNER JOIN fin.ADetail ADetail_1 ON IChkDep.TIAccno = ADetail_1.IAccno  
	 INNER JOIN fin.ProductDetail ON ADetail.PID = ProductDetail.PID 
	 INNER JOIN fin.ProductDetail ProductDetail_1 ON ADetail_1.PID = ProductDetail_1.PID  
	 WHERE IChkDep.Tdate between  @fromDate and @toDate and ttype = coalesce(@ttype,IChkDep.Ttype) and IChkDep.BrchID=coalesce(@branchId,IChkDep.BrchID)
	 )

	 
	



--	 select * from fin.IChkDep  
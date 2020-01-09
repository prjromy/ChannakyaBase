using ChannakyaBase.BLL.Repository;
using ChannakyaBase.Model.Models;
using ChannakyaBase.DAL.DatabaseModel;
using ChannakyaBase.Model.ViewModel;
using Loader.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using PagedList;

namespace ChannakyaBase.BLL.Service
{

    public class ReportService
    {
        private GenericUnitOfWork uow = null;
        ReturnBaseMessageModel returnMessage = null;
        private CommonService commonService = null;
        private Loader.Repository.GenericUnitOfWork luw = null;
        public ReportService()
        {
            uow = new GenericUnitOfWork();
            returnMessage = new ReturnBaseMessageModel();
            commonService = new CommonService();
            luw = new Loader.Repository.GenericUnitOfWork();
        }
        public List<AccountReportViewModel> AccountOpenCloseDetailsReportAllList(DateTime? fromDate, DateTime? toDate, int branchID, int productId, int accountStatus, int accountType,int pageNo, int pageSize)
        {
            string query = "";

            query = @"select * from [fin].[FGetReportAccountOpeningDetails]({0},{1})";

            if (accountType == 2)
            {
                query += "where  AccountType=0";
            }
            else if (accountType == 3)
            {
                query += "where  AccountType=1";
            }
            else
            {
                query += "where  AccountType in(0,1)";
            }
            if (branchID != 0)
            {
                query += " and  BranchId={2}";
            }
            if (productId != 0)
            {
                query += " and  PID={3}";
            }
            if (accountStatus != 0)
            {
                query += " and  AccountStatus={4}";
            }
            query += @" ORDER BY  AccountId
                       OFFSET ((" + pageNo + @" - 1) * " + pageSize + @") ROWS
                       FETCH NEXT " + pageSize + " ROWS ONLY";

            //query += @" ORDER BY  AccountName
            //           OFFSET ((" + pageNo + @" - 1) * " + pageSize + @") ROWS
            //           FETCH NEXT " + pageSize + " ROWS ONLY";
            var AccountDetails = uow.Repository<AccountReportViewModel>().SqlQuery(query, fromDate, toDate).ToList();
            return AccountDetails;
        }
        public IPagedList<AccountReportViewModel> AccountOpenCloseDetailsReportList(DateTime? fromDate, DateTime? toDate, int branchID, int productId, int accountStatus, int accountType, int pageNo, int pageSize)
        {
            string query = "";
          
            query = @"select * from [fin].[FGetReportAccountOpeningDetails]({0},{1})";

            if (accountType == 2)
            {
                query += "where  AccountType=0";
            }
            else if (accountType == 3)
            {
                query += "where  AccountType=1";
            }
            else
            {
                query += "where  AccountType in(0,1)";
            }
            if (branchID != 0)
            {
                query += " and  BranchId={2}";
            }
            if (productId != 0)
            {
                query += " and  PID={3}";
            }
            if (accountStatus != 0)
            {
                query += " and  AccountStatus={4}";
            }

            //query += @" ORDER BY  AccountName
            //           OFFSET ((" + pageNo + @" - 1) * " + pageSize + @") ROWS
            //           FETCH NEXT " + pageSize + " ROWS ONLY";
            var AccountDetails = uow.Repository<AccountReportViewModel>().SqlQuery(query, fromDate, toDate, branchID, productId, accountStatus).OrderBy(x=>x.AccountName).ToList();
            var pagedList = AccountDetails.ToPagedList(pageNo, pageSize);
            return pagedList;
        }

        public List<AccountReportViewModel> MatureDurationWiseAccountPageListing(DateTime fromDate, DateTime toDate, int branchID, int productId, short sType, int pageNo, int pageSize)
        {
            string query = "";

            query = @"select COUNT(*) OVER () AS TotalCount,* from [fin].[FgetReportDurationWise]({0},{1},{2})";
            if (branchID != 0 && productId != 0)
            {
                query += "where  BranchId={3} and PID={4}";
            }
            else if (branchID != 0 && productId == 0)
            {
                query += "where  BranchId={3}";
            }
            else if (branchID == 0 && productId != 0)
            {
                query += "where PID={4}";
            }

            query += @" ORDER BY AccountNumber
                       OFFSET ((" + pageNo + @" - 1) * " + pageSize + @") ROWS
                       FETCH NEXT " + pageSize + " ROWS ONLY";


            //query += @"OFFSET ((" + pageNo + @" - 1) * " + pageSize + @") ROWS
            //           FETCH NEXT " + pageSize + " ROWS ONLY";
            var AccountDetails = uow.Repository<AccountReportViewModel>().SqlQuery(query, fromDate, toDate, sType, branchID, productId, pageNo, pageSize).ToList();
            return AccountDetails;
        }

        public IPagedList<AccountReportViewModel> MatureDurationWiseAccount(DateTime? fromDate, DateTime? toDate, int branchID, int productId, short sType, int pageNo, int pageSize)
        {
            string query = "";

             query = @"select COUNT(*) OVER () AS TotalCount,* from [fin].[FgetReportDurationWise]({0},{1},{2})";
            //query = @"select COUNT(*) OVER () AS TotalCount,* from [fin].[FgetReportDurationWise]((convert(date, '08/04/2018 00:00:00', 105)),(convert(date, '15/04/2018 00:00:00', 105)),0)";
            if (branchID != 0 && productId != 0)
            {
                query += "where  BranchId={3} and PID={4}";
            }
            else if (branchID != 0 && productId == 0)
            {
                query += "where  BranchId={3}";
            }
            else if (branchID == 0 && productId != 0)
            {
                query += "where PID={4}";
            }

            //query += @" ORDER BY  AccountNumber
            //           OFFSET ((" + pageNo + @" - 1) * " + pageSize + @") ROWS
            //           FETCH NEXT " + pageSize + " ROWS ONLY";
            var AccountDetails = uow.Repository<AccountReportViewModel>().SqlQuery(query, fromDate, toDate, sType, branchID, productId).OrderBy(x=>x.AccountNumber).ToList();
            var pagedList = AccountDetails.ToPagedList(pageNo, pageSize);
            return pagedList;
        }

        public List<DenoTransactionViewModel> DenoReportByUser(int userId, int currencyId)
        {
            var denoUserReport = uow.Repository<DenoTransactionViewModel>().SqlQuery("SELECT * FROM[fin].[FGetReportDenoTransactionByUser]({0},{1})", userId, currencyId).ToList();
            return denoUserReport;
        }
        public List<DenoTransactionViewModel> DenoByUserAndTransactionNo(int userId, long tno)
        {
            var denoUserReport = uow.Repository<DenoTransactionViewModel>().SqlQuery("SELECT * FROM[fin].[FGetReportDenoByUserAndTransactionNo]({0},{1})", tno, userId).ToList();
            return denoUserReport;
        }
        public List<DenoTransactionViewModel> DenoByUserAndTransactionHistory(DateTime transactionDate, int userId, int currId)
        {
            var denoUserReport = uow.Repository<DenoTransactionViewModel>().SqlQuery("SELECT * FROM[fin].[FGetReportDenoTransactionHistory]({0},{1},{2})", transactionDate, userId, currId).ToList();
            return denoUserReport;
        }
        public List<CashFlowViewModel> GetAllUserReportDetailsList(DateTime fromDate, DateTime toDate, int userId, int currId)
        {
            string sqlQuery = "SELECT * FROM[fin].[FGetReportCashFlow]({0},{1},{2},{3},{4})";

            sqlQuery += "ORDER BY tno,Cramt";
            var CashFlow = uow.Repository<CashFlowViewModel>().SqlQuery(sqlQuery, fromDate, toDate, Global.BranchId, userId, currId).ToList();
            return CashFlow;
        }
        public List<CashFlowViewModel> GetProductSummaryReport(DateTime fromDate, DateTime toDate, int? branchId)
        {
            if (branchId == 0)
            {
                branchId = null;
            }
            string sqlQuery = "select * from fin.FgetReportProductSummaryDetails({0},{1},{2})";
            var productSummary = uow.Repository<CashFlowViewModel>().SqlQuery(sqlQuery, fromDate, toDate, branchId).ToList();
            return productSummary;
        }
        public List<ProductTransactionSummeryModel> GetProductTransactionSummary(DateTime fromDate, DateTime toDate, int? branchId)
        {
            if (branchId == 0)
            {
                branchId = null;
            }
            string sqlQuery = "select * from fin.FgetReportProductTransactionSummary({0},{1},{2})";
            var productTransactionSummary = uow.Repository<ProductTransactionSummeryModel>().SqlQuery(sqlQuery, fromDate, toDate, branchId).ToList();
            return productTransactionSummary;
        }
        public List<ChequeWithdewModel> GetChequeWithdrawTransaction(DateTime fromDate, DateTime toDate, int? branchId)
        {
            if (branchId == 0)
            {
                branchId = null;
            }
            string sqlQuery = "select * from fin.FgetReportChequeWithdraw({0},{1},{2})";
            var chequeSummary = uow.Repository<ChequeWithdewModel>().SqlQuery(sqlQuery, fromDate, toDate, branchId).ToList();
            return chequeSummary;
        }
        public List<TransactionPayableModel> GetInterestPayableTransaction(DateTime fromDate, DateTime toDate, int? branchId)
        {
            if (branchId == 0)
            {
                branchId = null;
            }
            string sqlQuery = "select * from fin.FgetReportTransactionPayable({0},{1},{2})";
            var interestPayableSummary = uow.Repository<TransactionPayableModel>().SqlQuery(sqlQuery, fromDate, toDate, branchId).ToList();
            return interestPayableSummary;
        }
        public List<TransactionPayableModel> GetInterestPayableWithdraw(DateTime fromDate, DateTime toDate, int? branchId)
        {
            if (branchId == 0)
            {
                branchId = null;
            }
            string sqlQuery = "select * from fin.FgetReportWithdrawPayable({0},{1},{2})";
            var interestPayableSummary = uow.Repository<TransactionPayableModel>().SqlQuery(sqlQuery, fromDate, toDate, branchId).ToList();
            return interestPayableSummary;
        }
        public List<TransactionPayableModel> GetInterestPayableBalance(int? branchId)
        {
            if (branchId == 0)
            {
                branchId = null;
            }
            string sqlQuery = "select * from fin.FgetReportBalancePayable({0})";
            var interestPayableSummary = uow.Repository<TransactionPayableModel>().SqlQuery(sqlQuery, branchId).ToList();
            return interestPayableSummary;
        }
        public List<LoanTransactionModel> GetLoanTransaction(DateTime fromDate, DateTime toDate, int? branchId)
        {
            if (branchId == 0)
            {
                branchId = null;
            }
            string sqlQuery = "select * from fin.FgetReportLoanTransaction({0},{1},{2})";
            var loanTransaction = uow.Repository<LoanTransactionModel>().SqlQuery(sqlQuery, fromDate, toDate, branchId).ToList();
            return loanTransaction;
        }
        public List<UnVerifiedTransactionModel> UnverifiedWithdrawDepositReport(int? branchId, int stype)
        {
            if (branchId == 0)
            {
                branchId = null;
            }
            string sqlQuery = "select * from fin.FGetReportUnverifiedTransaction({0})";
            if (stype == 2)
            {
                sqlQuery += "where SType=0";
            }
            else if (stype == 3)
            {
                sqlQuery += "where SType=1";
            }
            var Transaction = uow.Repository<UnVerifiedTransactionModel>().SqlQuery(sqlQuery, branchId).ToList();
            return Transaction;
        }
        public List<UnVerifiedTransactionModel> verifiedWithdrawDepositReport(int? branchId, DateTime transDate, int stype)
        {
            if (branchId == 0)
            {
                branchId = null;
            }
            string sqlQuery = "select * from fin.FGetReportverifiedTransaction({0},{1})";
            if (stype == 2)
            {
                sqlQuery += "where SType=0";
            }
            else if (stype == 3)
            {
                sqlQuery += "where SType=1";
            }
            var Transaction = uow.Repository<UnVerifiedTransactionModel>().SqlQuery(sqlQuery, branchId, transDate).ToList();
            return Transaction;
        }
        public List<AChqModel> ChequeeIssueBounceAndBlockReport(DateTime fromDate, DateTime toDate, int? branchId, byte status, int? iaccno)
        {
            if (branchId == 0)
            {
                branchId = null;
            }
            if (iaccno == 0)
            {
                iaccno = null;
            }
            string sqlQuery = "";
            if (status == 1)
            {
                sqlQuery = "select * from fin.FGetReportChequeBookIssue({0},{1},{2},{3})";
            }
            else if (status == 2)
            {
                sqlQuery = "select * from fin.FGetReportChequeBookBlocked({0},{1},{2},{3})";
            }
            else
            {
                sqlQuery = "select * from fin.FGetReportChequeBounce({0},{1},{2},{3})";
            }
            var chequeRelated = uow.Repository<AChqModel>().SqlQuery(sqlQuery, fromDate, toDate, branchId, iaccno).ToList();
            return chequeRelated;
        }
        public List<InternalChequeDepositReportModel> InternalChequeDepositByChequeReport(DateTime fromDate, DateTime toDate, int? branchId, byte? status)
        {
            if (branchId == 0)
            {
                branchId = null;
            }
            if (status == 0)
            {
                status = 50;
            }
            else if (status == 1)
            {
                status = 51;
            }
            else
            {
                status = null;
            }
            string sqlQuery = "select * from fin.FGetReportInternalChequeDeposit({0},{1},{2},{3})";
            var chequeRelated = uow.Repository<InternalChequeDepositReportModel>().SqlQuery(sqlQuery, fromDate, toDate, branchId, status).ToList();
            return chequeRelated;
        }
        public List<WithdrawViewModel> ChequeGoodForPaymentRepor(DateTime fromDate, DateTime toDate, int? branchId, byte? status)
        {
            if (branchId == 0)
            {
                branchId = null;
            }
            if (status == 0)
            {
                status = null;
            }
            string sqlQuery = "select * from fin.FGetReportChequeGoodForPaymentReport({0},{1},{2},{3})";
            var chequeRelated = uow.Repository<WithdrawViewModel>().SqlQuery(sqlQuery, fromDate, toDate, branchId, status).ToList();
            return chequeRelated;
        }
        public List<LoanBalanceTillDateModel> GetLoanBalanceTillDate(DateTime fromDate, int? branchId)
        {
            if (branchId == 0)
            {
                branchId = null;
            }

            string sqlQuery = "select * from fin.FGetReportLoanBalanceTillDate({0},{1})";
            var loanTillDate = uow.Repository<LoanBalanceTillDateModel>().SqlQuery(sqlQuery, fromDate, branchId).ToList();
            return loanTillDate;
        }
        public List<LoanBalanceTillDateModel> GetIndividualLoanTillDate(DateTime fromDate, byte pid, int branchId)
        {
            string sqlQuery = @"select ad.Accno,Aname,PID,lb.* from fin.FGetReportAllTotalLoanBalanceByTransDate({0})lb
                                join fin.ADetail ad on ad.IAccno=lb.IAccNo where PID=" + pid;
            if (branchId != 0)
            {
                sqlQuery += "and BrchID=" + branchId;
            }
            var loanTillDate = uow.Repository<LoanBalanceTillDateModel>().SqlQuery(sqlQuery, fromDate).ToList();
            return loanTillDate;
        }
        public List<LoanFollowUpModel> LoanFollowUp(DateTime fromDate, int? iAccno, int branchId)
        {
            if (iAccno == 0)
            {
                iAccno = null;
            }
            var loanFollowUp = uow.Repository<LoanFollowUpModel>().SqlQuery("exec [fin].[PGetReportLoanFollowUp] @TDATE,@BRHID,@IACNO",
                        new SqlParameter("@TDATE", fromDate),
                        new SqlParameter("@BRHID", branchId),
                        new SqlParameter("@IACNO", (object)iAccno ?? DBNull.Value)
                      ).ToList();
            return loanFollowUp;
        }
        public List<LoanAccruedInterestListingModel> LoanAccruedInterestListing(DateTime fromDate, int branchId)
        {

            string sqlQuery = @"Select a.*,c.PName as ProductName,(MAccured+UMAccured) as TotalAmount From Fin.FGetReportLoanAccruedInterestListing({0},{1}) a
                                inner join fin.adetail b on a.iaccno = b.iaccno
                                inner join fin.productdetail c on b.pid = c.pid
                                Order by c.pname,a.IAccNo";
            var accruedInterest = uow.Repository<LoanAccruedInterestListingModel>().SqlQuery(sqlQuery, branchId, fromDate).ToList();
            return accruedInterest;
        }

        public IEnumerable<DepositBalanceTillDateAmountWiseModel> GetDepositBalanceTillDteAmountWise(DateTime tDate, int branchId, int sortId, decimal fAmount, decimal tAmount)
        {

            var tilldateBalance = uow.Repository<DepositBalanceTillDateAmountWiseModel>().SqlQuery("select * from fin.FGetReportDepartBalanceTillDateAmountWise (@BRCHID,@TDATE ,@FAMT,@TAMT,@SORTID)",
                        new SqlParameter("@BRCHID", branchId),
                        new SqlParameter("@TDATE", tDate),
                        new SqlParameter("@FAMT", fAmount),
                        new SqlParameter("@TAMT", tAmount),
                        new SqlParameter("@SORTID", sortId)
                      ).ToList();
            return tilldateBalance;
        }
        public IEnumerable<LoanOutstandingModel> LoanOutStanding(DateTime fromDate, DateTime toDate, int branchId, int revolving)
        {
            string query = "select * from fin.FGetReportLoanOutStanding (@FDATE,@TDATE ,@BRANCHID)";
            if (revolving == 2)
            {
                query += "where Revolving=1";
            }
            if (revolving == 3)
            {
                query += "where Revolving=0";
            }
            var outstandingBal = uow.Repository<LoanOutstandingModel>().SqlQuery(query,
                        new SqlParameter("@BRANCHID", branchId),
                        new SqlParameter("@FDATE", fromDate),
                        new SqlParameter("@TDATE", toDate)

                      ).ToList();
            return outstandingBal;
        }
        public List<AllDesignationViewModel> VaultTellerAndCashierUser()
        {
            var allReciever = uow.Repository<AllDesignationViewModel>().SqlQuery("select UserId,EmployeeName+'( '+UserName+' )' As EmployeeName,DegOrder from [fin].[FGetAllUsersWithDesignation]() where DegOrder<8 and DegOrder>2 and DegOrder!=4").OrderBy(x=>x.EmployeeName).ToList();
            return allReciever;
        }
        public List<AllDesignationViewModel> GetAllUserReportList()
        {
            List<AllDesignationViewModel> allReciever = new List<AllDesignationViewModel>();
            List<AllDesignationViewModel> myDetails = new List<AllDesignationViewModel>();
            List<AllDesignationViewModel> mDetails = new List<AllDesignationViewModel>();

            int userId = Global.UserId;
            var user = uow.Repository<User>().FindBy(x => x.UserId == userId).SingleOrDefault();

            var employees = uow.Repository<DAL.DatabaseModel.Employee>().FindBy(x => x.EmployeeId == user.EmployeeId).SingleOrDefault();
           var menuTemplate= uow.Repository<User>().FindBy(x => x.UserId == userId).Select(x=>x.MTId).SingleOrDefault();
            // var designationId = uow.Repository<DAL.DatabaseModel.ParamValue>().FindBy(x => x.PValue == employees.DGId.ToString()).Select(x=>x.PId).SingleOrDefault();

            //mDetails = uow.Repository<AllDesignationViewModel>().SqlQuery("select * from [fin].[FGetAllUsersWithDesignation]() where userid=" + userId).FirstOrDefault();
            //allReciever = uow.Repository<AllDesignationViewModel>().SqlQuery(" select * from [fin].[FGetAllUsersWithDesignation]() where DegOrder> " + mDetails.DegOrder).ToList();

            if (menuTemplate == 1002)/// list must contain cashier,teller of same department
            {
                //allReciever = uow.Repository<AllDesignationViewModel>().SqlQuery(" select * from  [fin].[FGetAllCashierBranch](2005," + Global.BranchId + ") where DesignationId= " + employees.DGId + "and DeptId=" + employees.DeptId).ToList();
                allReciever = uow.Repository<AllDesignationViewModel>().SqlQuery(" select UserId,EmployeeId,EmployeeName+'( '+UserName+' )' As EmployeeName from  [fin].[FGetAllUsersWithDesignation]() where DesignationId= " + employees.DGId + "and DeptId=" + employees.DeptId).ToList();
                mDetails = uow.Repository<AllDesignationViewModel>().SqlQuery("select  UserId,EmployeeId,EmployeeName+'( '+UserName+' )' As EmployeeName from [fin].[FGetAllUsersWithDesignation]() where PId=" + 2006+ "and DeptId=" + employees.DeptId).ToList();
                allReciever.AddRange(mDetails);
            }
            else if(menuTemplate == 1001)///teller list
            {
                //allReciever = uow.Repository<AllDesignationViewModel>().SqlQuery(" select * from [fin].[FGetAllUsersWithDesignation]() where DesignationId =" + employees.DGId + "and DeptId="+employees.DeptId).ToList();
                allReciever = uow.Repository<AllDesignationViewModel>().SqlQuery(" select UserId,EmployeeId,EmployeeName+'( '+UserName+' )' As EmployeeName from [fin].[FGetAllUsersWithDesignation]() where UserId =" + userId ).ToList();
            }
            else/// list contains vault,cashier,teller same deparment
            {
                allReciever = uow.Repository<AllDesignationViewModel>().SqlQuery(" select  UserId,EmployeeId,EmployeeName+'( '+UserName+' )' As EmployeeName from [fin].[FGetAllUsersWithDesignation]() where DesignationId =" + employees.DGId + "and DeptId=" + employees.DeptId).ToList();
                mDetails = uow.Repository<AllDesignationViewModel>().SqlQuery("select  UserId,EmployeeId,EmployeeName+'( '+UserName+' )' As EmployeeName from [fin].[FGetAllUsersWithDesignation]() where PId=" + 2005 + "and DeptId=" + employees.DeptId).ToList();
                myDetails  = uow.Repository<AllDesignationViewModel>().SqlQuery("select  UserId,EmployeeId,EmployeeName+'( '+UserName+' )' As EmployeeName from [fin].[FGetAllUsersWithDesignation]() where PId=" + 2006 + "and DeptId=" + employees.DeptId).ToList();
                allReciever.AddRange(mDetails);
                allReciever.AddRange(myDetails);
            }
           
            return allReciever.OrderBy(x=>x.EmployeeName).ToList();
        }
        public UserReportViewModel UserReportList()
        {
            int userId = Loader.Models.Global.UserId;
            UserReportViewModel userReport = new UserReportViewModel();
            userReport.TDate = commonService.GetBranchTransactionDate();
            userReport.UserList = new SelectList(GetAllUserReportList(), "UserId", "EmployeeName");
            return userReport;
        }
        public List<UserReportViewModel> GetAllUserReportDetailsList(int userId, DateTime BranchTDate)
        {
            List<UserReportViewModel> allDetailsList = new List<UserReportViewModel>();
            allDetailsList = uow.Repository<UserReportViewModel>().SqlQuery("SELECT * FROM[fin].[FGetReportUserTeller](" + "'" + BranchTDate + "'" + "," + userId + ")").ToList();
            return allDetailsList;
        }
        public DepositStatementViewModel DepositStatement()
        {
            DepositStatementViewModel depositStatement = new DepositStatementViewModel();
            depositStatement.FromDate = commonService.GetBranchTransactionDate().AddMonths(-1);
            depositStatement.ToDate = commonService.GetBranchTransactionDate();
            return depositStatement;
        }
        public IEnumerable<LoanStatementModel> GetLoanStatement(DateTime fromDate, DateTime toDate, int iaccno)
        {
            string query = "select *,CrPrincipal  + CrPrincInt  - Rebate + OtherCr  +  CrOtherInt  + CrPenalty as TotalPayment from [fin].[FGetReportLoanStatement](@IACCNO,@FDATE,@TDATE)";
            var loanStatement = uow.Repository<LoanStatementModel>().SqlQuery(query,
                         new SqlParameter("@IACCNO", iaccno),
                         new SqlParameter("@FDATE", fromDate),
                         new SqlParameter("@TDATE", toDate)

                       ).ToList();
            return loanStatement;
        }
        public decimal AccuredPenalty(int iaccno)
        {
            string query = "Select Cast(Isnull(SUM(ponprout+poniout),0) as decimal(18,2))as AccuredPenalty from fin.FGetIndividualTotalLoanAccountBalance('" + Global.TransactionDate + "')where Iaccno=" + iaccno;
            var loanStatement = uow.Repository<LoanStatementModel>().SqlQuery(query).FirstOrDefault();
            return loanStatement.AccuredPenalty;
        }
        public decimal AccuredInterest(int iaccno)
        {
            string query = "Select top 1 Cast(Isnull(IntOut,0) as decimal(18,2))as AccuredInterest from fin.FGetIndividualTotalLoanAccountBalance('" + Global.TransactionDate + "')where Iaccno=" + iaccno;
            var loanStatement = uow.Repository<LoanStatementModel>().SqlQuery(query).FirstOrDefault();
            return loanStatement.AccuredInterest;
        }
        public IEnumerable<GuarantorReportModel> GetLoanGuarantor(int? branchId)
        {
            if (branchId == 0)
            {
                branchId = null;
            }
            string sql = "select * from Fin.FGetReportGuarantor(@BRCHID)";
            var loanGuarantor = uow.Repository<GuarantorReportModel>().SqlQuery(sql, new SqlParameter("@BRCHID", (object)branchId ?? DBNull.Value)).ToList();
            return loanGuarantor;

        }

        public DepositStatementViewModel DepositStatementWithDetails(int Iaccno, DateTime? FromDate, DateTime? ToDate)
        {
            DepositStatementViewModel depositStatement = new DepositStatementViewModel();
            depositStatement = uow.Repository<DepositStatementViewModel>().SqlQuery("select * from Fin.FGetReportDepostiStatementWithAccDetails(" + Iaccno + ")").FirstOrDefault();

            depositStatement.DepositStatementViewModelList = uow.Repository<DepositStatementViewModel>().SqlQuery("select * from Fin.[FGetReportDepositStatement](" + Iaccno + "," + "'" + FromDate + "'" + "," + "'" + ToDate + "'" + "," + 0 + ")").ToList();
            depositStatement.TotalDebited = depositStatement.DepositStatementViewModelList.Sum(x => Convert.ToDecimal(x.Debit));
            depositStatement.TotalCredited = depositStatement.DepositStatementViewModelList.Sum(x => Convert.ToDecimal(x.Credit));
            depositStatement.TotalDebitCount = depositStatement.DepositStatementViewModelList.Where(x => x.Debit != null).Count();
            depositStatement.TotalCreditCount = depositStatement.DepositStatementViewModelList.Where(x => x.Credit != null).Count();
            var desc = depositStatement.DepositStatementViewModelList.Where(p => p.Tdate != null).GroupBy(p => p.Tdate).Select(grp => grp.FirstOrDefault());
            var dateList = desc.Select(s => new DepositStatementViewModel
            {
                Tdate = s.Tdate,
                MarkDateSelect = s.Tdate.Value.ToString("MM/dd/yyyy")
            }).ToList();
            depositStatement.MarkDateList = new SelectList(dateList, "Tdate", "MarkDateSelect");
            return depositStatement;

        }



        public List<DepositStatementViewModel> DepositStatementList(int Iaccno, DateTime? FromDate, DateTime? ToDate)
        {
            List<DepositStatementViewModel> depositStatement = new List<DepositStatementViewModel>();

            depositStatement = uow.Repository<DepositStatementViewModel>().SqlQuery("select * from Fin.[FGetReportDepositStatement](" + Iaccno + "," + "'" + FromDate + "'" + "," + "'" + ToDate + "'" + "," + 0 + ")").ToList();

            return depositStatement;

        }
        public DateTime? MarkDate(int Iaccno)
        {

            return uow.Repository<APBookChked>().FindByMany(x => x.IAccNo == Iaccno).OrderByDescending(x => x.ChkedTill).Select(x => x.ChkedTill).FirstOrDefault();
        }
        public ReturnBaseMessageModel MarkPassBook(int Iaccno, DateTime checkedTill)
        {
            try
            {

                APBookChked apassbookMark = uow.Repository<APBookChked>().SqlQuery("Select * from fin.APBookChked where iaccno=" + Iaccno).FirstOrDefault();
                if (apassbookMark == null)
                {
                    apassbookMark = new APBookChked();
                    apassbookMark.ChkedBy = Loader.Models.Global.UserId;
                    apassbookMark.ChkedOn = commonService.GetBranchTransactionDate();
                    apassbookMark.ChkedTill = checkedTill;
                    apassbookMark.IAccNo = Iaccno;
                    uow.Repository<APBookChked>().Add(apassbookMark);
                }
                else
                {
                    apassbookMark.ChkedBy = Loader.Models.Global.UserId;
                    apassbookMark.ChkedOn = commonService.GetBranchTransactionDate();
                    apassbookMark.ChkedTill = checkedTill;
                    uow.Repository<APBookChked>().Edit(apassbookMark);
                }
                uow.Commit();
                returnMessage.Success = true;
                returnMessage.Msg = "Successfully Marked.";


            }
            catch (Exception ex)
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Could Not Marked.";
                throw;
            }
            return returnMessage;
        }
        public ProductTransactionReport ProductTransactionReport()
        {
            ProductTransactionReport productTransactionReport = new ProductTransactionReport();
            productTransactionReport.FromDate = commonService.GetBranchTransactionDate().AddMonths(-1);
            productTransactionReport.ToDate = commonService.GetBranchTransactionDate();
            productTransactionReport.DepositFlag = 1;
            productTransactionReport.SearchParamFlag = 4;
            productTransactionReport.VerifyFlag = 2;
            return productTransactionReport;

        }
        public List<ProductTransactionReport> ProductTransactionReportList(DateTime FDate, DateTime TDate, int searchType, int iSDep, int isVerify)
        {
            int branchId = Global.BranchId;

            string query = "Select * from [fin].[FGetReportProductTransaction](" + "'" + FDate + "'" + "," + "'" + TDate + "'" + "," + branchId + "," + searchType + "," + iSDep + ")";
            if (isVerify != 2)
            {
                query = query + "Where IsVerify =" + isVerify + " order by tno";
            }
            var reportTransactionList = uow.Repository<ProductTransactionReport>().SqlQuery(query).ToList();
            return reportTransactionList;
        }
        public ProductTransactionReport LoanTransactionDisbursePayment()
        {
            ProductTransactionReport productTransactionReport = new ProductTransactionReport();
            productTransactionReport.FromDate = commonService.GetBranchTransactionDate().AddMonths(-1);
            productTransactionReport.ToDate = commonService.GetBranchTransactionDate();
            productTransactionReport.DepositFlag = 1;
            productTransactionReport.SearchParamFlag = 4;
            productTransactionReport.VerifyFlag = 2;
            return productTransactionReport;

        }
        public ProductTransactionReport LoanTransactionDisbursePaymentList(DateTime FDate, DateTime TDate, int searchType, int iSDis, int isVerify)
        {
            ProductTransactionReport productTransactionReport = new ProductTransactionReport();
            productTransactionReport.PaymentDisburseFlag = iSDis;
            int branchId = Global.BranchId;
            string query = "";
            if (iSDis == 1)
            {
                query = "Select * from [fin].[FGetReportLoanDisburseDetails](" + "'" + FDate + "'" + "," + "'" + TDate + "'" + "," + branchId + "," + isVerify + ")";


                if (searchType != 3)
                {
                    if (searchType == 1)
                        searchType = 5;
                    else
                        searchType = 1;
                    query = query + " Where TType =" + searchType;
                }

            }
            else
            {

                query = "Select * from [fin].[FGetReportLoanPayementDetails](" + "'" + FDate + "'" + "," + "'" + TDate + "'" + "," + branchId + "," + isVerify + ")";
                if (searchType != 3)
                {
                    if (searchType != 1)
                    {
                        searchType = 5;
                    }
                    else
                    {
                        searchType = 1;
                    }
                    query = query + "Where ttype =" + searchType;
                }
            }
            var reportTransactionList = uow.Repository<ProductTransactionReport>().SqlQuery(query).ToList();
            productTransactionReport.ProductTransactionReportList = reportTransactionList;
            return productTransactionReport;
        }

        public IEnumerable<InterestLogModel> GetInterestLog(int branchId, DateTime fromDate, DateTime toDate, int? option, int? loanInterest, int? accountType, int iAccNo)
        {
            string sqlQuery = "";
            if (option == 1)
            {
                sqlQuery = "select * from fin.FGetReportInterestLog('" + fromDate + "'" + "," + "'" + toDate + "'," + branchId + "," + accountType + "," + loanInterest + ")";
            }
            else
            {
                sqlQuery = "select * from fin.FGetReportInterestLogIndividual('" + fromDate + "'" + "," + "'" + toDate + "'," + iAccNo + "," + branchId + ")";
            }
            var interestLog = uow.Repository<InterestLogModel>().SqlQuery(sqlQuery).ToList();
            return interestLog;
        }

        public List<RemittanceReportModel> GetRemittance(DateTime fromDate, DateTime toDate, int remit, int branchId)
        {
            if (remit == 1)
            {
                

                var ramittanceDep = uow.Repository<RemitDeposit>().FindBy(x => x.PostedOn >= fromDate && x.PostedOn <= toDate && x.BranchId == branchId).Select(y => new RemittanceReportModel()
                {
                    Tno = y.Tno,
                    TDate = y.PostedOn,
                    RemitCompany = y.RemittanceCustomer.CustCompany.CCName,
                    Particular = "From " + y.SenderName + " to " + y.ReceiverName,
                    Amount = y.Amount
                }).ToList();
                return ramittanceDep;
            }
            else
            {
                var ramittancePay = uow.Repository<RemitPayment>().FindBy(x => x.PostedOn >= fromDate && x.PostedOn <= toDate && x.BranchId == branchId).Select(y => new RemittanceReportModel()
                {
                    Tno = y.Tno,
                    TDate = y.PostedOn,
                    RemitCompany = y.RemittanceCustomer.CustCompany.CCName,
                    Particular = "Cash Paid to " + y.ReceiverName,
                    Amount = y.Amount
                }).ToList();
                return ramittancePay;

            }
        }

        public IEnumerable<OverdraftBalanceModel> GetOverDraftBalance(DateTime fromDate, int branchId)
        {
            string sql = "select * from fin.FGetReportOverdraftBalance('" + fromDate + "'," + branchId + ")";
            var overDraft = uow.Repository<OverdraftBalanceModel>().SqlQuery(sql).ToList();
            return overDraft;
        }

        public IEnumerable<OverdraftInterestCapitalizationModel> GetOverdraftInterestCapitalization(int branchId, DateTime fromDate, DateTime toDate)
        {
            string sql = "select * from fin.FGetReportOverdraftInterestCapitalization('" + fromDate + "'" + "," + "'" + toDate + "'," + branchId + ")  ORDER BY Tdate, TNo";
            var interestCapitalize = uow.Repository<OverdraftInterestCapitalizationModel>().SqlQuery(sql).ToList();
            return interestCapitalize;
        }

        public IEnumerable<InterestCapitalizationModel> GetInterestCapitalization(int branchId, DateTime fromDate, DateTime toDate)
        {
            string sql = "select * from fin.FGetReportInterestCapitalization('" + fromDate + "'" + "," + "'" + toDate + "'," + branchId + ")";
            var interestCapitalize = uow.Repository<InterestCapitalizationModel>().SqlQuery(sql).ToList();
            return interestCapitalize;
        }

        public IEnumerable<InterestTOCapitalizeAccountModel> GetInterestTOCapitalizeAccount(DateTime fromDate, int branchId)
        {

            var InterestTOCapitalize = uow.Repository<InterestTOCapitalizeAccountModel>().SqlQuery("exec [fin].[PGetReportLoanFollowUp] @TDATE,@BRNCHID",
                         new SqlParameter("@TDATE", fromDate),
                         new SqlParameter("@BRNCHID", branchId)

                       ).ToList();
            return InterestTOCapitalize;
        }

        public BranchDetailsModel GetBranchDetails(int branchId)
        {
            var query = "select BrnhID,BrnhNam,Addr,TDate from fin.LicenseBranch where BrnhID = " + branchId;
            var branchDetails = uow.Repository<BranchDetailsModel>().SqlQuery(query).FirstOrDefault();
            return branchDetails;
        }

        public List<ProductWiseInterestLogModel> GetProductWiseInterestLog(int branchId, DateTime fromDate, DateTime toDate, int productOption)
        {
            string SqlQuery = "";
            if (productOption == 1)
            {
                SqlQuery = "select * from fin.FGetReportDepositProductWiseInterestLog('" + fromDate + "'" + "," + "'" + toDate + "'," + branchId + ")";
            }
            else
            {
                SqlQuery = "select * from fin.FGetReportLoanProductWiseInterestLog('" + fromDate + "'" + "," + "'" + toDate + "'," + branchId + ")";
            }
            var productIntLog = uow.Repository<ProductWiseInterestLogModel>().SqlQuery(SqlQuery).ToList();
            return productIntLog;
        }

        public CollectionSheetTransReportViewModel CollectionSheetTransReport()
        {
            CollectionSheetTransReportViewModel collectionSheetTransReport = new CollectionSheetTransReportViewModel();
            collectionSheetTransReport.FromDate = commonService.GetBranchTransactionDate().AddMonths(-1);
            collectionSheetTransReport.ToDate = commonService.GetBranchTransactionDate();
            return collectionSheetTransReport;

        }



        public List<CollectionSheetTransReportViewModel> CollectionSheetTransReport(DateTime FDate, DateTime TDate, int? pageNo = 1, int pageSize = 20, string search = null)
        {
            CollectionSheetTransReportViewModel collectionSheetTransReport = new CollectionSheetTransReportViewModel();
            int branchId = Global.BranchId;
            string query = "";
            query = "Select * from [fin].[FGetReportCollectionSheet](" + "'" + FDate + "'" + "," + "'" + TDate + "'" + "," + branchId + ")";
            var allReportTransactionList = uow.Repository<CollectionSheetTransReportViewModel>().SqlQuery(query).ToList();
            int totalCount = allReportTransactionList.Count();

            if (search == "" || search == null)
            {
                query += @" ORDER BY  EmployeeName
                       OFFSET ((" + pageNo + @" - 1) * " + pageSize + @") ROWS
                       FETCH NEXT " + pageSize + " ROWS ONLY";
            }
            else
            {
                query += @"where lower(EmployeeName) like '%" + search + "%'";
                query += @" ORDER BY  EmployeeName
                       OFFSET ((" + pageNo + @" - 1) * " + pageSize + @") ROWS
                       FETCH NEXT " + pageSize + " ROWS ONLY";
            }

            var reportTransactionList = uow.Repository<CollectionSheetTransReportViewModel>().SqlQuery(query).ToList();
            foreach (var item in reportTransactionList)
            {
                item.totalCount = totalCount;
            }
            collectionSheetTransReport.CollectionSheetTransReportList = reportTransactionList;
            return reportTransactionList;
        }



        #region AccountWiseCollectionSheetTransReport
        public AccountWiseCollectionModel AccountWiseCollectionSheetTransReport()
        {
            AccountWiseCollectionModel accountWiseCollection = new AccountWiseCollectionModel();
            accountWiseCollection.FromDate = commonService.GetBranchTransactionDate().AddMonths(-1);
            accountWiseCollection.ToDate = commonService.GetBranchTransactionDate();
            return accountWiseCollection;
        }

        public AccountWiseCollectionModel AccountWiseCollectionSheetTransReport(DateTime fDate, DateTime tDate, int collectorId)
        {
            AccountWiseCollectionModel accountWiseCollection = new AccountWiseCollectionModel();
            int branchId = Global.BranchId;
            string query;
            query = "Select * from [fin].[FGetReportAccountWiseCollection](" + "'" + fDate + "'" + "," + "'" + tDate + "'" + "," + branchId + "," + collectorId + ")";
            var accountWiseCollectionSheetReport = uow.Repository<AccountWiseCollectionModel>().SqlQuery(query).ToList();
            accountWiseCollection.AccountWiseCollectionList = accountWiseCollectionSheetReport;
            return accountWiseCollection;
        }
        public List<AccountWiseCollectionModel> AccountWiseCollectionSheetTransReportPagination(DateTime fDate, DateTime tDate, int collectorId,int page=1,int pageSize=10)
        {
            AccountWiseCollectionModel accountWiseCollection = new AccountWiseCollectionModel();
            int branchId = Global.BranchId;
            string query;
            query = "Select * from [fin].[FGetReportAccountWiseCollection](" + "'" + fDate + "'" + "," + "'" + tDate + "'" + "," + branchId + "," + collectorId + ")";
            var accountWiseCollectionSheetReportAll = uow.Repository<AccountWiseCollectionModel>().SqlQuery(query).ToList();

            int totalCount = accountWiseCollectionSheetReportAll.Count();

                query += @" ORDER BY  Aname
                       OFFSET ((" + page + @" - 1) * " + pageSize + @") ROWS
                       FETCH NEXT " + pageSize + " ROWS ONLY";

            var accountWiseCollectionSheetReport = uow.Repository<AccountWiseCollectionModel>().SqlQuery(query).ToList();
            foreach (var item in accountWiseCollectionSheetReport)
            {
                item.totalCount = totalCount;
            }

            return accountWiseCollectionSheetReport;
        }

        public AccountWiseCollectionModelSummary AccountWiseCollectionSummary(int Iaccno, DateTime fDate, DateTime tDate, int collectorId)
        {
            AccountWiseCollectionModelSummary accountWiseCollectionModelSummary = new AccountWiseCollectionModelSummary();
            int branchId = Global.BranchId;
            string query;
            query = "Select * from [fin].[FGetReportAccountWiseCollectionSummary](" + "'" + fDate + "'" + "," + "'" + tDate + "'" + "," + Iaccno + "," + branchId + "," + collectorId + ")";
            var accountWiseCollectionSummaryReport = uow.Repository<AccountWiseCollectionModelSummary>().SqlQuery(query).ToList();
            accountWiseCollectionModelSummary.AccountWiseCollectionListSummary = accountWiseCollectionSummaryReport;
            return accountWiseCollectionModelSummary;
        }
        #endregion

        public ProductWiseCollectionSheet ProductWiseCollectionSheet()
        {
            ProductWiseCollectionSheet productWiseCollectionSheet = new ProductWiseCollectionSheet();
            productWiseCollectionSheet.FromDate = commonService.GetBranchTransactionDate().AddMonths(-1);
            productWiseCollectionSheet.ToDate = commonService.GetBranchTransactionDate();
            return productWiseCollectionSheet;

        }
        public List<ProductWiseCollectionSheet> ProductWiseCollectionSheet(DateTime fDate, DateTime tDate, int? pageNo, int pageSize)
        {
            ProductWiseCollectionSheet productWiseCollectionSheet = new ProductWiseCollectionSheet();
            int branchId = Global.BranchId;
            string query = "";
            query = "Select * from [fin].[FGetReportProductWiseCollection](" + "'" + fDate + "'" + "," + "'" + fDate + "'" + ")";
            var allProductWiseCollectionSheetList = uow.Repository<ProductWiseCollectionSheet>().SqlQuery(query).ToList();

            int totalCount = allProductWiseCollectionSheetList.Count();

            //if (search == "" || search == null)
            //{
            //    query += @" ORDER BY  EmployeeName
            //           OFFSET ((" + pageNo + @" - 1) * " + pageSize + @") ROWS
            //           FETCH NEXT " + pageSize + " ROWS ONLY";
            //}
            //else
            //{
            //query += @"where lower(EmployeeName) like '%" + search + "%'";
            query += @" ORDER BY  EmployeeName
                       OFFSET ((" + pageNo + @" - 1) * " + pageSize + @") ROWS
                       FETCH NEXT " + pageSize + " ROWS ONLY";
            //}

            var productWiseCollectionSheetList = uow.Repository<ProductWiseCollectionSheet>().SqlQuery(query).ToList();
            foreach (var item in productWiseCollectionSheetList)
            {
                item.totalCount = totalCount;
            }

            //productWiseCollectionSheet.ProductWiseCollectionSheetList = productWiseCollectionSheetList;
            return productWiseCollectionSheetList;

        }
        public ProductWiseCollectionSheet ProductWiseCollectionSheet(DateTime fDate, DateTime tDate)
        {
            ProductWiseCollectionSheet productWiseCollectionSheet = new ProductWiseCollectionSheet();
            int branchId = Global.BranchId;
            string query = "";
            query = "Select * from [fin].[FGetReportProductWiseCollection](" + "'" + fDate + "'" + "," + "'" + fDate + "'" + ")";
            var productWiseCollectionSheetList = uow.Repository<ProductWiseCollectionSheet>().SqlQuery(query).ToList();
            productWiseCollectionSheet.ProductWiseCollectionSheetList = productWiseCollectionSheetList;
            return productWiseCollectionSheet;
        }
        public AccOpenByCollectorViewModel AccountOpenByCollector()
        {
            AccOpenByCollectorViewModel accOpenByCollectorViewModel = new AccOpenByCollectorViewModel();
            accOpenByCollectorViewModel.FromDate = commonService.GetBranchTransactionDate().AddMonths(-1);
            accOpenByCollectorViewModel.ToDate = commonService.GetBranchTransactionDate();
            return accOpenByCollectorViewModel;

        }

        public AccOpenByCollectorViewModel AccountOpenByCollector(DateTime tdate, DateTime fdate, int status)
        {
            AccOpenByCollectorViewModel collectorList = new AccOpenByCollectorViewModel();

            int branchId = Global.BranchId; ;
            string query = @"Select * from [fin].[FGetReportAccOpnByCollector]('" + branchId + "','" + fdate + "','" + tdate + "','" + status + "')";
            collectorList.AccOpenByCollectorViewModelList = uow.Repository<AccOpenByCollectorViewModel>().SqlQuery(query).ToList();
            // var parentList = collectorList.AccOpenByCollectorViewModelList.Where(x => x.BroughtBy == collectorList.BroughtBy && x.Name == collectorList.Name).Distinct().ToList();
            //List<Acc childList = collectorList.AccOpenByCollectorViewModelList.Where(x => x.BroughtBy == collectorList.BroughtBy).ToList();
            return collectorList;
        }

        public List<CollectorDetailViewModel> AccountOpenByCollectorDetail(int collectorId, DateTime tdate, DateTime fdate, int? status, string Pname, int? pageNo = 1, int pageSize = 20)
        {
            CollectorDetailViewModel collectorList = new CollectorDetailViewModel();
            string statusString;
            if (status == 0)
            {
                status = null;
                statusString = "null";
            }
            else
            {
                statusString = status.ToString();
            }
            if (Pname == "")
            {
                Pname = "null";
            }
            else
            {
                Pname = "'" + Pname + "'";
            }

            string query = @"Select * from [fin].[FGetReportAccOpnByCollectorSummary](" + collectorId + ",'" + fdate + "','" + tdate + "'," + statusString + "," + Pname + ")";

            //string query1 = @"Select * from [fin].[FGetReportAccOpnByCollectorSummary]({0},{1},{2},{3},{4})";
            collectorList.collectorDetailViewModel = uow.Repository<CollectorDetailViewModel>().SqlQuery(query).ToList();
            //collectorList.collectorDetailViewModel = uow.Repository<CollectorDetailViewModel>().SqlQuery(query1, collectorId, fdate, tdate, status, Pname).ToList();
            int totalCount = collectorList.collectorDetailViewModel.Count();


            query += @" ORDER BY  EmployeeName
                       OFFSET ((" + pageNo + @" - 1) * " + pageSize + @") ROWS
                       FETCH NEXT " + pageSize + " ROWS ONLY";



            collectorList.collectorDetailViewModel = uow.Repository<CollectorDetailViewModel>().SqlQuery(query).ToList();
            foreach (var item in collectorList.collectorDetailViewModel)
            {
                item.totalCount = totalCount;
            }

            return collectorList.collectorDetailViewModel;
        }

        public ShareReportByDateViewModel DateWiseShareReport()
        {
            ShareReportByDateViewModel shareReportByDate = new ShareReportByDateViewModel();
            shareReportByDate.FromDate = commonService.GetBranchTransactionDate().AddMonths(-1);
            shareReportByDate.ToDate = commonService.GetBranchTransactionDate();
            return shareReportByDate;
        }

        public LoanAutoPayment LoanBeforeAutoPayment()
        {
            LoanAutoPayment collectionSheetTransReport = new LoanAutoPayment();

            return collectionSheetTransReport;

        }
        public LoanAutoPayment LoanBeforeAutoPayment(int? branchId, int? Pid)
        {
            if (branchId == 0)
            {
                branchId = null;
            }
            if (Pid == 0)
            {
                Pid = null;
            }

            LoanAutoPayment loanAutoPayment = new LoanAutoPayment();

            string query = "Select * from [fin].[FGETReportLoanBeforeAutoPayment]({0},{1})";
            var loanAutoPaymentList = uow.Repository<LoanAutoPayment>().SqlQuery(query, branchId, Pid).ToList();
            loanAutoPayment.LoanAutoPaymentList = loanAutoPaymentList;
            return loanAutoPayment;
        }
        public LoanAfterAutoPayment LoanAfterAutoPayment()
        {
            LoanAfterAutoPayment loanAfterAutoPayment = new LoanAfterAutoPayment();
            loanAfterAutoPayment.FromDate = commonService.GetBranchTransactionDate().AddMonths(-1);
            loanAfterAutoPayment.ToDate = commonService.GetBranchTransactionDate();
            return loanAfterAutoPayment;

        }
        public LoanAfterAutoPayment LoanAfterAutoPayment(DateTime toDate, DateTime fdate, int? branchId)
        {
            if (branchId == 0)
            {
                branchId = null;
            }

            LoanAfterAutoPayment loanAfterAutoPayment = new LoanAfterAutoPayment();

            string query = "Select * from [fin].[FGetReportLoanAfterAutoPayment]({0},{1},{2})";
            var loanAutoPaymentList = uow.Repository<LoanAfterAutoPayment>().SqlQuery(query, toDate, fdate, branchId).ToList();
            loanAfterAutoPayment.LoanAfterAutoPaymentList = loanAutoPaymentList;
            return loanAfterAutoPayment;
        }
        public CommonReportModel CommonReportService()
        {
            CommonReportModel loanReportAccountDetails = new CommonReportModel();
            loanReportAccountDetails.FromDate = commonService.GetBranchTransactionDate().AddMonths(-1);
            loanReportAccountDetails.ToDate = commonService.GetBranchTransactionDate();
            loanReportAccountDetails.branchId = ReportUtilityService.GetBranchId();
            return loanReportAccountDetails;

        }
        public LoanReportAccountDetails LoanAccountDetail(int iaccno)
        {
            LoanReportAccountDetails LoanReportAccountDetails = new LoanReportAccountDetails();
            string query = "Select * from [fin].[FGetReportLoanAccountDetails]({0})";
            LoanReportAccountDetails = uow.Repository<LoanReportAccountDetails>().SqlQuery(query, iaccno).FirstOrDefault();
            return LoanReportAccountDetails;
        }
        public IEnumerable<LoanAccountWiseSchedule> LoanAccountScheduleList(int iaccno)
        {
            string query = "Select * from [fin].[FGetReportLoanScheduleList]({0})";
            var LoanAccountWiseSchedule = uow.Repository<LoanAccountWiseSchedule>().SqlQuery(query, iaccno).ToList();
            return LoanAccountWiseSchedule;
        }
        public IEnumerable<LoanMatured> LoanMatured(DateTime toDate, int? branchId)
        {
            if (branchId == 0)
            {
                branchId = null;
            }
            string query = "Select * from [fin].[FGetReportLoanMatured]({0},{1})";
            var shareReturnDetails = uow.Repository<LoanMatured>().SqlQuery(query, toDate, branchId).ToList();
            return shareReturnDetails;
        }
        #region ShareReports
        public IEnumerable<ShareTransaction> ShareTransactionByDate(DateTime fdate, DateTime toDate)
        {
            string query = "Select * from [fin].[FGetReportShareTransactionByDate]({0},{1})";
            var shareTransaction = uow.Repository<ShareTransaction>().SqlQuery(query, fdate, toDate).ToList();
            return shareTransaction;
        }
        public IEnumerable<ShareHolderList> ShareHolderList(int? branchId, int? stype)
        {
            if (branchId == 0)
            {
                branchId = null;
            }


            if (stype == 0)
                stype = null;
            else if (stype == 1)
                stype = 0;
            else
                stype = 1;

            string query = "Select * from [fin].[FGetReportShareHolderList]({0},{1})";
            var shareHolderList = uow.Repository<ShareHolderList>().SqlQuery(query, branchId, stype).ToList();
            return shareHolderList;
        }
        public IEnumerable<SharePurchaseDetails> SharePurchaseDetails(DateTime fdate, DateTime toDate)
        {
            string query = "Select * from [fin].[FGetReportSharePurchaseDetails]({0},{1})";
            var sharePurchaseDetails = uow.Repository<SharePurchaseDetails>().SqlQuery(query, fdate, toDate).ToList();
            return sharePurchaseDetails;
        }
        public IEnumerable<ShareReturnDetails> ShareReturnDetails(DateTime fdate, DateTime toDate)
        {
            string query = "Select * from [fin].[FGetReportShareReturnDetails]({0},{1})";
            var shareReturnDetails = uow.Repository<ShareReturnDetails>().SqlQuery(query, fdate, toDate).ToList();
            return shareReturnDetails;
        }
        public IEnumerable<ShareStatement> ShareStatement(int RegNo)
        {

            string query = "Select * from [fin].[FGetReportShareStatement]({0})";
            var shareReturnDetails = uow.Repository<ShareStatement>().SqlQuery(query, RegNo).ToList();
            return shareReturnDetails;
        }

        #endregion

        public CompanyViewModel CompanyDetails(int branchId = 0)
        {
            if (branchId == 0)
            {
                branchId = Global.BranchId;

            }
            CompanyViewModel company = uow.Repository<CompanyViewModel>().SqlQuery("select * from FGetCompanyList() where companyid=" + branchId).FirstOrDefault();

            return company;
        }

        public CompanyViewModel CompanyNameDetails()
        {
            CompanyViewModel companyViewModel = new CompanyViewModel();
            var company = luw.Repository<Loader.Models.ParamValue>().FindBy(x => x.PId == 26).Select(x => x.PValue).SingleOrDefault();
            companyViewModel.CompanyName = company;




            return companyViewModel;
        }
    }
}

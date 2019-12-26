using ChannakyaBase.BLL;
using ChannakyaBase.BLL.Service;
using ChannakyaBase.Model.Models;
using ChannakyaBase.Model.ViewModel;
using ExportExcel.Code;
using Loader;
using Loader.Models;
using Microsoft.Ajax.Utilities;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChannakyaBase.Web.Controllers
{
    [MyAuthorize]
    public class ReportController : Controller
    {
        ReturnBaseMessageModel returnMessage = null;
        private CommonService commonService = null;
        private ReportService reportService = null;
        private Loader.Service.ParameterService parameterService = null;
        public ReportController()
        {
            returnMessage = new ReturnBaseMessageModel();
            commonService = new CommonService();
            reportService = new ReportService();
            parameterService = new Loader.Service.ParameterService();
        }
        #region Teller Report
        #region Account Details Reports
        public ActionResult AccountopenDetailsReport()
        {
            AccountReportViewModel accountReportView = new AccountReportViewModel();
            int branchID = 0;
            if (!ReportUtilityService.IsAllowAllBranch())
            {
                branchID = commonService.GetBranchIdByEmployeeUserId();
            }
            int accountType = 0;
            if (ReportUtilityService.AllowBoth())
            {

                accountType = 1;
            }
            else if (ReportUtilityService.AllowDeposit())
            {
                accountType = 2;
            }
            else if (ReportUtilityService.AllowLoan())
            {
                accountType = 3;
            }

            var date = commonService.GetBranchTransactionDate();
            accountReportView.FromDate = date.AddMonths(-1);
            accountReportView.ToDate = date;
            accountReportView.BranchId = branchID;
            var accountdetails = reportService.AccountOpenCloseDetailsReportList(accountReportView.FromDate, accountReportView.ToDate, branchID, 0, 0, accountType, 1, 6);
            accountReportView.AccountReportList = accountdetails;
            //accountReportView.AccountReportList = new StaticPagedList<AccountReportViewModel>(accountdetails, pageNo, pageSize, (accountdetails.Count == 0) ? 0 : accountdetails.FirstOrDefault().TotalCount);
            return PartialView(accountReportView);
        }

        public ActionResult _AccountopenDetailsReport(DateTime fromDate, DateTime toDate, int branchid = 0, int productId = 0, int accountStatus = 0, int accountType = 0, int pageNo = 1, int pageSize = 6)
        {
            AccountReportViewModel accountReportView = new AccountReportViewModel();
            var date = commonService.GetBranchTransactionDate();
            accountReportView.FromDate = date.AddMonths(-1);
            accountReportView.ToDate = date;
            var accountdetails = reportService.AccountOpenCloseDetailsReportList(fromDate, toDate, branchid, productId, accountStatus, accountType, pageNo, pageSize);
            accountReportView.AccountReportList = accountdetails;
            return PartialView(accountReportView);
        }
        public ActionResult PaginationAccountopenDetailsReport(DateTime fromDate, DateTime toDate, int branchid = 0, int productId = 0, int accountStatus = 0, int accountType = 0, int pageNo = 1, int pageSize = 6)
        {
            AccountReportViewModel accountReportView = new AccountReportViewModel();
            var accountdetails = reportService.AccountOpenCloseDetailsReportList(fromDate, toDate, branchid, productId, accountStatus, accountType, pageNo, pageSize);
            accountReportView.AccountReportList = accountdetails;
            return PartialView("_AccountopenDetailsReport", accountReportView);
        }
        [HttpGet]
        public FileContentResult AccountOpenDetailsReportExporttoExcel(DateTime fromDate, DateTime toDate, int accountState, string accountStateText, int accountType, string accountTypeText, int branchId, string branchIdText, int productId, string productIdText, int pageNo = 1, int pageSize = 6)
        {
            if (!ReportUtilityService.IsAllowAllBranch())
            {
                branchId = commonService.GetBranchIdByEmployeeUserId();
            }
            //var acountopenDetailsList = reportService.AccountOpenCloseDetailsReportList(fromDate, toDate, branchId, productId, accountState, accountType, pageNo, pageSize).ToList();
            var acountopenDetailsList = reportService.AccountOpenCloseDetailsReportList(fromDate, toDate, branchId, productId, accountState, accountType, pageNo, pageSize).ToList();
            var accountOpenDetailsSelectedList = acountopenDetailsList.Select(x => new AccountReportExportToExcelViewModel()
            {

                AccountNumber = x.AccountNumber,
                AccountName = x.AccountName,
                ProductName = x.ProductName,
                RegisterDate = x.RegisterDate,
                AccountStatus = x.AccountState,
                ChangeOn = x.ChangeOn,
                AccountClosedBy = x.AccountClosedBy


            }).ToList();
            var pathimag = Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath + parameterService.GetImageFromdatabase();


            var company = reportService.CompanyNameDetails();
            string[] columns = { "AccountNumber", "AccountName", "ProductName", "Date", "AccountStatus", "ChangeOn", "Changed By" };
            string[] parameterSearch = { "From Date:" + fromDate.ToShortDateString() + " - To Date:" + toDate.ToShortDateString(), "Account Status:" + accountStateText + "-Branch Name:" + branchIdText + "-Account Type:" + accountTypeText + "-Product Name:" + productIdText };
            byte[] filecontent = ExcelExportHelper.ExportExcel(accountOpenDetailsSelectedList, company, parameterSearch,  columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "AccountopenDetails.xlsx");
        }
        public ActionResult MatureDurationWiseAccount()
        {
            AccountReportViewModel accountReportView = new AccountReportViewModel();

            int branchID = ReportUtilityService.GetBranchId();
            accountReportView.ViewForm = "DepositReport";
            var date = commonService.GetBranchTransactionDate();
            accountReportView.FromDate = date;
            accountReportView.ToDate = date.AddDays(7);
            accountReportView.BranchId = branchID;
            accountReportView.SType = 0;
            var accountdetails = reportService.MatureDurationWiseAccount(accountReportView.FromDate, accountReportView.ToDate, branchID, 0, 0, 1, 10);
            //accountReportView.AccountReportList = new StaticPagedList<AccountReportViewModel>(accountdetails, 1, 10, (accountdetails.Count == 0) ? 0 : accountdetails.FirstOrDefault().TotalCount);
            accountReportView.AccountReportList = accountdetails;
            return PartialView(accountReportView);

        }
        public ActionResult MatureDurationWiseAccountLoan()
        {
            AccountReportViewModel accountReportView = new AccountReportViewModel();
            int branchID = 0;
            if (!ReportUtilityService.IsAllowAllBranch())
            {
                branchID = commonService.GetBranchIdByEmployeeUserId();
            }
            accountReportView.ViewForm = "LoanReport";
            var date = commonService.GetBranchTransactionDate();
            accountReportView.FromDate = date;
            accountReportView.ToDate = date.AddDays(7);
            accountReportView.BranchId = branchID;
            accountReportView.SType = 1;
            var accountdetails = reportService.MatureDurationWiseAccount(accountReportView.FromDate, accountReportView.ToDate, branchID, 0, 1, 1, 10);
            //accountReportView.AccountReportList = new StaticPagedList<AccountReportViewModel>(accountdetails,1,5, (accountdetails.Count == 0) ? 0 : accountdetails.FirstOrDefault().TotalCount);
            accountReportView.AccountReportList = accountdetails;
            return PartialView("MatureDurationWiseAccount", accountReportView);

        }
        public ActionResult _MatureDurationWiseAccount(String fromDate, String toDate, int branchid, int productId, short sType = 0, int pageNo = 1, int pageSize = 10)
        {
            AccountReportViewModel accountReportView = new AccountReportViewModel();

            var accountdetails = reportService.MatureDurationWiseAccount(Convert.ToDateTime(fromDate), Convert.ToDateTime(toDate), branchid, productId, sType, pageNo, pageSize);

            accountReportView.AccountReportList = accountdetails;


            //    var accountdetails = reportService.MatureDurationWiseAccount(accountReportView.FromDate, accountReportView.ToDate, branchID, 0, 0, 1, 3);
            //accountReportView.AccountReportList = new StaticPagedList<AccountReportViewModel>(accountdetails, pageNo, pageSize, (accountdetails.Count == 0) ? 0 : accountdetails.FirstOrDefault().TotalCount);
            return PartialView(accountReportView);

        }

        [HttpGet]
        public FileContentResult MatureDurationWiseAccountExporttoExcel(DateTime fromDate, DateTime toDate, string branchText, string productText, int branchid, int productId, short sType = 0)
        {
            //int branchID = 0;
            //if (!ReportUtilityService.IsAllowAllBranch())
            //{
            //    branchID = commonService.GetBranchIdByEmployeeUserId();
            //}
            var MatureDurationWiseList = reportService.MatureDurationWiseAccount(fromDate, toDate, branchid, productId, sType, 1, 10).ToList();

            var matureDurationWiseSelectedList = MatureDurationWiseList.Select(x => new MatureDurationWiseAccountExportToExcelViewModel()
            {

                AccountNumber = x.AccountNumber,
                AccountName = x.AccountName,
                ProductName = x.ProductName,
                RegisterDate = x.RegisterDate,
                MatureDate = x.MatureDate,
                Balance = x.Balance
            }).ToList();
            //var company = reportService.CompanyDetails(branchid);
            var company = reportService.CompanyNameDetails();
            string[] columns = { "AccountNumber", "AccountName", "ProductName", "Date", "MDate", "Balance" };
            string[] parameterSearch = { "From Date:" + fromDate.ToShortDateString() + " - To Date:" + toDate.ToShortDateString(), "Product Name:" + productText + "-Branch Name:" + branchText };
            byte[] filecontent = ExcelExportHelper.ExportExcel(matureDurationWiseSelectedList, company, parameterSearch, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "MatureDuration.xlsx");
        }
        #endregion
        #endregion

        #region  Deno Report
        public ActionResult DenoReportByUser()
        {
            return PartialView();
        }
        public ActionResult _DenoReportByUser(int userId, int currencyId)
        {
            DenoTransactionViewModel denoTransactionModel = new DenoTransactionViewModel();
            var getDenouserReport = reportService.DenoReportByUser(userId, currencyId);
            denoTransactionModel.DenoReportList = getDenouserReport;
            return PartialView(denoTransactionModel);
        }
        [HttpGet]
        public FileContentResult DenoReportByUserExportToExcel(int currencyId, string currencyText, int userId, string userText)
        {
            var DenoReportByUserList = reportService.DenoReportByUser(userId, currencyId).ToList();
            var accountOpenDetailsSelectedList = DenoReportByUserList.Select(x => new DenoTransactionByUserExportToExcelViewModel()
            {

                Deno = x.Deno,
                Pics = x.Pics,
                Amount = x.Amount


            }).ToList();
            // var company = reportService.CompanyDetails(0);
            var company = reportService.CompanyNameDetails();
            string[] columns = { "Deno", "Pieces", "Amount" };
            string[] parameterSearch = { "Currency:" + currencyText + " - User:" + userText };
            byte[] filecontent = ExcelExportHelper.ExportExcel(accountOpenDetailsSelectedList, company, parameterSearch, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "DenoReportByUser.xlsx");
        }


        public ActionResult DenoByUserAndTransactionNo()
        {
            return PartialView();
        }
        public ActionResult _DenoByUserAndTransactionNo(int userId, long tno)
        {
            DenoTransactionViewModel denoTransactionModel = new DenoTransactionViewModel();
            var getDenouserReport = reportService.DenoByUserAndTransactionNo(userId, tno);
            denoTransactionModel.DenoReportList = getDenouserReport;
            return PartialView("_DenoReportByUser", denoTransactionModel);
        }
        [HttpGet]
        public FileContentResult DenoReportByTransactionExportToExcel(int userId, int tno, string userText)
        {
            var DenoReportByTransactionList = reportService.DenoByUserAndTransactionNo(userId, tno).ToList();
            var denoReportByTransactionSelectedList = DenoReportByTransactionList.Select(x => new DenoTransactionByUserExportToExcelViewModel()
            {

                Deno = x.Deno,
                Pics = x.Pics,
                Amount = x.Amount


            }).ToList();
            // var company = reportService.CompanyDetails(0);
            var company = reportService.CompanyNameDetails();
            string[] columns = { "Deno", "Pieces", "Amount" };
            string[] parameterSearch = { "Tno:" + tno + " - User:" + userText };
            byte[] filecontent = ExcelExportHelper.ExportExcel(denoReportByTransactionSelectedList, company, parameterSearch, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "DenoReportByTransaction.xlsx");
        }

        public ActionResult DenoByUserAndTransactionHistory()
        {
            DenoTransactionViewModel denoTransactionModel = new DenoTransactionViewModel();
            denoTransactionModel.Date = commonService.GetBranchTransactionDate();
            return PartialView(denoTransactionModel);
        }
        public ActionResult _DenoByUserAndTransactionHistory(DateTime transactionDate, int userId, int currencyId)
        {
            DenoTransactionViewModel denoTransactionModel = new DenoTransactionViewModel();
            var getDenouserReport = reportService.DenoByUserAndTransactionHistory(transactionDate, userId, currencyId);
            denoTransactionModel.DenoReportList = getDenouserReport;
            return PartialView("_DenoReportByUser", denoTransactionModel);
        }



        [HttpGet]
        public FileContentResult DenoReportByUserAndTransactionHistoryExportToExcel(int currencyId, string currencyText, int userId, string userText, DateTime date)
        {
            var DenoReportByUserAndTransactionList = reportService.DenoByUserAndTransactionHistory(date, userId, currencyId).ToList();
            var DenoReportByUserAndTransactionSelectedList = DenoReportByUserAndTransactionList.Select(x => new DenoTransactionByUserExportToExcelViewModel()
            {

                Deno = x.Deno,
                Pics = x.Pics,
                Amount = x.Amount


            }).ToList();
            //var company = reportService.CompanyDetails(0);
            var company = reportService.CompanyNameDetails();
            string[] columns = { "Deno", "Pieces", "Amount" };
            string[] parameterSearch = { "Currency:" + currencyText + " - User:" + userText, "Date:" + date };
            byte[] filecontent = ExcelExportHelper.ExportExcel(DenoReportByUserAndTransactionSelectedList, company, parameterSearch, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "DenoReportByUserAndTransactionHistory.xlsx");
        }
        #endregion

        #region UserReport
        public ActionResult UserReport()
        {
            var accountdetails = reportService.UserReportList();

            return PartialView(accountdetails);
        }
        public ActionResult _UserTellerReportDetails(int ReportUserId, DateTime tDate)
        {
            var accountdetails = reportService.GetAllUserReportDetailsList(ReportUserId, tDate);
            return PartialView(accountdetails);
        }
        [HttpGet]
        public FileContentResult UserReportExportToExcel(int ReportUserId, string ReportUserText, DateTime tDate)
        {
            var UserReportList = reportService.GetAllUserReportDetailsList(ReportUserId, tDate).ToList();
            var UserReportSelectedList = UserReportList.Select(x => new UserReportExportToExcel()
            {

                AccountNo = commonService.GetAccountFullNameByIaccno(Convert.ToInt32(x.Iaccno)),
                EmployeeName = x.EmployeeName,
                Dramt = x.Dramt,
                Cramt = x.Cramt,
                Tdesc = x.Tdesc,
                Tno = x.Tno

            }).ToList();
            //var company = reportService.CompanyDetails(0);
            var company = reportService.CompanyNameDetails();
            string[] columns = { "Account Number ", "Customer Name", "Dr Amount", "Cr Amount", "Description", "Trxn.No" };
            string[] parameterSearch = { "Transaction Date:" + tDate + " - User:" + ReportUserText };
            byte[] filecontent = ExcelExportHelper.ExportExcel(UserReportSelectedList, company, parameterSearch, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "UserReport.xlsx");

        }

        #endregion

        #region Internal CashFlow
        public ActionResult InternalCashFlow()
        {
            CashFlowViewModel cashFlowModel = new CashFlowViewModel();
            var date = commonService.GetBranchTransactionDate();
            cashFlowModel.FromDate = date.AddMonths(-1);
            cashFlowModel.ToDate = date;
            return PartialView(cashFlowModel);
        }
        public ActionResult _InternalCashFlow(DateTime fromDate, DateTime toDate, int userId = 0, int currId = 0)
        {
            CashFlowViewModel cashFlowModel = new CashFlowViewModel();
            var cashFlow = reportService.GetAllUserReportDetailsList(fromDate, toDate, userId, currId);
            cashFlowModel.CashFlowList = cashFlow;
            return PartialView(cashFlowModel);
        }
        [HttpGet]
        public FileContentResult InternalCashFlowExportToExcel(int userId, string userIdText, DateTime fromDate, DateTime toDate, int CurrID, string CurrIDText)
        {
            var internalCashFlowList = reportService.GetAllUserReportDetailsList(fromDate, toDate, userId, CurrID).ToList();
            var internalCashFlowSelectedList = internalCashFlowList.Select(x => new CashFlowExportToExcelViewModel()
            {

                UserId1 = commonService.EmployeeInfoUserId(x.UserId1).EmployeeName,
                UserId2 = commonService.EmployeeInfoUserId(x.UserId2).EmployeeName,
                Tdesc = x.Tdesc,
                TDate = x.TDate,
                Tno = x.Tno,
                Dramt = x.Dramt,
                Cramt = x.Cramt,
                Remarks = x.Remarks

            }).ToList();
            // var company = reportService.CompanyDetails(0);
            var company = reportService.CompanyNameDetails();
            string[] columns = { "UserId1", "UserId2", "Tdesc", "TDate", "Tno", "Dramt", "Cramt", "Remarks" };
            string[] parameterSearch = { "Currency:" + CurrIDText + " - User:" + userIdText, "From  Date:" + fromDate + "-To Date :" + toDate };
            byte[] filecontent = ExcelExportHelper.ExportExcel(internalCashFlowSelectedList, company, parameterSearch, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "InternalCashFlow.xlsx");

        }

        #endregion
        // GET: Report.0
        #region Transaction Report
        public ActionResult ProductSummaryReport()
        {
            CashFlowViewModel productReportModel = new CashFlowViewModel();
            var date = commonService.GetBranchTransactionDate();
            productReportModel.FromDate = date.AddMonths(-1);
            productReportModel.ToDate = date;
            productReportModel.branchId = ReportUtilityService.GetBranchId();
            return PartialView(productReportModel);
        }
        public ActionResult _ProductSummaryReport(DateTime fromDate, DateTime toDate, int branchId)
        {
            CashFlowViewModel productReportModel = new CashFlowViewModel();
            productReportModel.CashFlowList = reportService.GetProductSummaryReport(fromDate, toDate, branchId);

            return PartialView(productReportModel);
        }
        [HttpGet]
        public FileContentResult ProductSummaryExportToExcel(int branchId, string branchText, DateTime fromDate, DateTime toDate)
        {
            var ProductSummaryList = reportService.GetProductSummaryReport(fromDate, toDate, branchId).ToList();
            var ProductSummarySelectedList = ProductSummaryList.Select(x => new ProductSummaryExportToExcel()
            {
                ProductName = x.ProductName,
                Cramt = x.Cramt,
                Dramt = x.Dramt,
                Balance = x.Dramt - x.Cramt

            }).ToList();
            //var company = reportService.CompanyDetails(branchId);
            var company = reportService.CompanyNameDetails();
            string[] columns = { "ProductName", "Debit Amount", "Credit Amount ", "Balance" };
            string[] parameterSearch = { "Branch Name:" + branchText, "From  Date:" + fromDate + "-To Date :" + toDate };
            byte[] filecontent = ExcelExportHelper.ExportExcel(ProductSummarySelectedList, company, parameterSearch, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "ProductSummary.xlsx");

        }
        public ActionResult ProductTransactionSummaryReport()
        {
            ProductTransactionSummeryModel productTransactionSummeryModel = new ProductTransactionSummeryModel();
            var date = commonService.GetBranchTransactionDate();
            productTransactionSummeryModel.FromDate = date.AddMonths(-1);
            productTransactionSummeryModel.ToDate = date;
            productTransactionSummeryModel.branchId = ReportUtilityService.GetBranchId();
            return PartialView(productTransactionSummeryModel);
        }
        public ActionResult _ProductTransactionSummaryReport(DateTime fromDate, DateTime toDate, int branchId)
        {
            ProductTransactionSummeryModel productTransactionSummeryModel = new ProductTransactionSummeryModel();
            productTransactionSummeryModel.ProductSummeryList = reportService.GetProductTransactionSummary(fromDate, toDate, branchId);
            return PartialView(productTransactionSummeryModel);
        }
        [HttpGet]
        public FileContentResult ProductTransactionSummaryExportToExcel(int branchId, string branchText, DateTime fromDate, DateTime toDate)
        {
            var ProductTransactionSummaryList = reportService.GetProductTransactionSummary(fromDate, toDate, branchId).ToList();
            var ProductTransactionSummarySelectedList = ProductTransactionSummaryList.Select(x => new ProductTransactionSummeryModelExportToExcel()
            {
                ProductName = x.ProductName,
                CashInFlow = x.CashInFlow,
                NonCashInFlow = x.NonCashInFlow,
                CashOutFlow = x.CashOutFlow,
                NonCashOutFlow = x.NonCashOutFlow

            }).ToList();
            //var company = reportService.CompanyDetails(branchId);
            var company = reportService.CompanyNameDetails();
            string[] columns = { "ProductName", "Cash In Flow", "Non Cash In Flow", "Cash Out Flow", "Non Cash Out Flow" };
            string[] parameterSearch = { "Branch Name:" + branchText, "From  Date:" + fromDate + "-To Date :" + toDate };
            byte[] filecontent = ExcelExportHelper.ExportExcel(ProductTransactionSummarySelectedList, company, parameterSearch, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "ProductTransactionSummary.xlsx");

        }

        public ActionResult ChequeWithdraw()
        {
            ChequeWithdewModel ChequeWithdrawModel = new ChequeWithdewModel();
            var date = commonService.GetBranchTransactionDate();
            ChequeWithdrawModel.FromDate = date.AddMonths(-1);
            ChequeWithdrawModel.ToDate = date;
            ChequeWithdrawModel.branchId = ReportUtilityService.GetBranchId();
            return PartialView(ChequeWithdrawModel);
        }
        public ActionResult _ChequeWithdraw(DateTime fromDate, DateTime toDate, int branchId)
        {
            ChequeWithdewModel ChequeWithdrawModel = new ChequeWithdewModel();
            ChequeWithdrawModel.ChequeWithdrawList = reportService.GetChequeWithdrawTransaction(fromDate, toDate, branchId);
            return PartialView(ChequeWithdrawModel);
        }

        [HttpGet]
        public FileContentResult ChequeWithdrawExportToExcel(int branchId, string branchText, DateTime fromDate, DateTime toDate)
        {
            var ChequeWithdrawList = reportService.GetChequeWithdrawTransaction(fromDate, toDate, branchId).ToList();
           var ChequeWithdrawSelectedList = ChequeWithdrawList.Select(x => new ChequeWithdrawViewModelExportToExcel()
           {

               AccountNumber = x.AccountNumber,
               TransactionDate = x.TransactionDate,
               AccountName = x.AccountName,
                ChequeNo = x.ChequeNo,
                Amount = x.Amount,
               Notes = x.Notes

            }).ToList();
            // var company = reportService.CompanyDetails(branchId);
            var company = reportService.CompanyNameDetails();
            string[] columns = { "AccountNumber", "Date", "AccountName", "ChequeNo", "Amount", "Notes" };
           string[] parameterSearch = { "Branch Name:" + branchText + "From  Date:" + fromDate + "-To Date :" + toDate };
           byte[] filecontent = ExcelExportHelper.ExportExcel(ChequeWithdrawSelectedList, company, parameterSearch, columns);
           return File(filecontent, ExcelExportHelper.ExcelContentType, "ChequeWithdraw.xlsx");

        }

        public ActionResult TransactionPayable()
        {
            TransactionPayableModel PayableModel = new TransactionPayableModel();
            var date = commonService.GetBranchTransactionDate();
            PayableModel.FromDate = date.AddMonths(-1);
            PayableModel.ToDate = date;
            PayableModel.branchId = ReportUtilityService.GetBranchId();
            ViewBag.report = "Transaction";
            return PartialView(PayableModel);
        }
        public ActionResult _TransactionPayable(DateTime fromDate, DateTime toDate, int branchId)
        {
            TransactionPayableModel PayableModel = new TransactionPayableModel();
            PayableModel.InterestList = reportService.GetInterestPayableTransaction(fromDate, toDate, branchId);
            return PartialView(PayableModel);
        }
        [HttpGet]
        public FileContentResult TransactionPayableExportToExcel(int branchId, string branchText, DateTime fromDate, DateTime toDate)
        {
            var TransactionPayableList = reportService.GetInterestPayableTransaction(fromDate, toDate, branchId).ToList();
            var TransactionPayableSelectedList = TransactionPayableList.Select(x => new TransactionPayableModelExportToExcel()
            {

                PName = x.PName,
                Accno = x.Accno,
                AName = x.AName,
                Tdate = x.Tdate,
                TaxRt = x.TaxRt,
                Tax = x.Tax,
                IntAmt = x.IntAmt,
                Amount = x.Amount

            }).ToList();
            //var company = reportService.CompanyDetails(branchId);
            var company = reportService.CompanyNameDetails();
            string[] columns = { "ProductName", "Accno", "AccountName", "Date", "TaxRate", "Tax", "Interest Amount", "Amount" };
            string[] parameterSearch = { "Branch Name:" + branchText, "From  Date:" + fromDate + "-To Date :" + toDate };
            byte[] filecontent = ExcelExportHelper.ExportExcel(TransactionPayableSelectedList, company, parameterSearch, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "InterestPayable.xlsx");

        }
        public ActionResult WithdrawPayable()
        {
            TransactionPayableModel PayableModel = new TransactionPayableModel();
            var date = commonService.GetBranchTransactionDate();
            PayableModel.FromDate = date.AddMonths(-1);
            PayableModel.ToDate = date;
            PayableModel.branchId = ReportUtilityService.GetBranchId();
            ViewBag.report = "Withdraw";
            return PartialView("TransactionPayable", PayableModel);
        }
        public ActionResult _WithdrawPayable(DateTime fromDate, DateTime toDate, int branchId)
        {
            TransactionPayableModel PayableModel = new TransactionPayableModel();
            PayableModel.InterestList = reportService.GetInterestPayableWithdraw(fromDate, toDate, branchId);
            return PartialView("_TransactionPayable", PayableModel);
        }
        public ActionResult BalancePayable()
        {
            TransactionPayableModel PayableModel = new TransactionPayableModel();

            PayableModel.branchId = ReportUtilityService.GetBranchId();
            return PartialView(PayableModel);
        }
        public ActionResult _BalancePayable(int branchId)
        {
            TransactionPayableModel PayableModel = new TransactionPayableModel();
            PayableModel.InterestList = reportService.GetInterestPayableBalance(branchId);
            return PartialView(PayableModel);
        }

        [HttpGet]
        public FileContentResult BalancePayableExportToExcel(int branchId, string branchText)
        {
            var BalancePayableList = reportService.GetInterestPayableBalance(branchId).ToList();
            var BalancePayableSelectedList = BalancePayableList.Select(x => new BalancePayableExportToExcel()
            {

                PName = x.PName,
                Accno = x.Accno,
                AName = commonService.GetCustomerNameByIaccno(x.Iaccno),
                Balance = x.Balance

            }).ToList();
            //var company = reportService.CompanyDetails(branchId);
            var company = reportService.CompanyNameDetails();
            string[] columns = { "Product Name", "Account Number", "AccountName", "Balance" };
            string[] parameterSearch = { "Branch Name:" + branchText };
            byte[] filecontent = ExcelExportHelper.ExportExcel(BalancePayableSelectedList, company, parameterSearch, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "BalancePayable.xlsx");

        }
        #endregion

        #region Loan Report
        public ActionResult LoanTransaction()
        {
            LoanTransactionModel loanTrans = new LoanTransactionModel();
            var date = commonService.GetBranchTransactionDate();
            loanTrans.FromDate = date.AddMonths(-1);
            loanTrans.ToDate = date;
            loanTrans.branchId = ReportUtilityService.GetBranchId();
            return PartialView(loanTrans);
        }
        public ActionResult _LoanTransaction(DateTime fromDate, DateTime toDate, int branchId)
        {
            LoanTransactionModel loanTrans = new LoanTransactionModel();
            loanTrans.LoanTransList = reportService.GetLoanTransaction(fromDate, toDate, branchId);
            return PartialView(loanTrans);
        }
        [HttpGet]
        public FileContentResult loantransactionExportToExcel(int branchId, string branchName, DateTime fromDate, DateTime toDate)
        {
            var loanTransactionList = reportService.GetLoanTransaction(fromDate, toDate, branchId);
            var loanTransactionExportExcel = loanTransactionList.Select(x => new LoanTransactioExportExcelModel()
            {
                ProductName = x.ProductName,
                Accno = x.Accno,
                AccountName = x.AccountName,
                vdate = x.vdate,
                PriDr = x.PriDr,
                PriCr = x.PriCr,
                Interest = x.Interest,
                POnPr = x.POnPr,
                POnInt = x.POnInt,
                IOnI = x.IOnI
            }).ToList();

            //var company = reportService.CompanyDetails(branchId);
            var company = reportService.CompanyNameDetails();
            string[] columns = { "Product Name", "Account Number", "Name", "Date", "Prin Dr", "Prin Cr", "Int", "POnPr", "POnInt", "IonI" };
            string[] parameterSearch = { "Branch Name=" + branchName + " - From Date: " + fromDate + " - To Date:" + toDate };
            byte[] filecontent = ExcelExportHelper.ExportExcel(loanTransactionExportExcel, company, parameterSearch, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "loanTransaction.xlsx");
        }
        public ActionResult LoanBalanceTillDate()
        {
            CommonReportModel commonReportModel = new CommonReportModel();
            var date = commonService.GetBranchTransactionDate();
            commonReportModel.FromDate = date;
            commonReportModel.branchId = ReportUtilityService.GetBranchId();
            return PartialView(commonReportModel);
        }
        public ActionResult _LoanBalanceTillDate(DateTime fromDate, int branchId)
        {
            var loanBalance = reportService.GetLoanBalanceTillDate(fromDate, branchId);
            return PartialView(loanBalance);
        }
        [HttpGet]
        public FileContentResult LoanBalanceTillDateExportToExcel(int branchId, string branchIdText, DateTime fromDate)
        {
            var LoanBalanceTillDateList = reportService.GetLoanBalanceTillDate(fromDate, branchId);
            var LoanBalanceTillDateExportExcel = LoanBalanceTillDateList.Select(x => new LoanBalanceTillDateExportToExcelViewModel()
            {
                PName = x.PName,
                PrinOut = x.PrinOut,
                IntOut = x.IntOut,
                POnPrOut = x.POnPrOut,
                POnIOut = x.POnIOut,
                IOnIOut = x.IOnIOut
            }).ToList();


            // var company = reportService.CompanyDetails(branchId);
            var company = reportService.CompanyNameDetails();
            string[] columns = { "Product Name", "PrinOut", "IntOut", "POnPrOut", "POnIOut", "IOnIOut" };
            string[] parameterSearch = { "Branch Name=" + branchIdText, " - From Date: " + fromDate };
            byte[] filecontent = ExcelExportHelper.ExportExcel(LoanBalanceTillDateExportExcel, company, parameterSearch, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "LoanBalanceTillDate.xlsx");
        }
        public ActionResult _IndividualLoanTillDate(DateTime fromDate, byte pid, int branchId, string pName)
        {
            var loanBalance = reportService.GetIndividualLoanTillDate(fromDate, pid, branchId);
            ViewBag.PID = pid;
            ViewBag.PName = pName;
            return PartialView(loanBalance);
        }

        [HttpGet]
        public FileContentResult IndividualLoanTillDateExportToExcel(int branchId, string branchIdText, byte pid, DateTime fromDate, string pName)
        {
            var IndividualLoanBalanceTillDateList = reportService.GetIndividualLoanTillDate(fromDate, pid, branchId);
            var IndividualLoanBalanceTillDateExportExcel = IndividualLoanBalanceTillDateList.Select(x => new IndividualLoanBalanceTillDateExportToExcelViewModel()
            {
                Accno = x.Accno,
                Aname = x.Aname,
                PrinOut = x.PrinOut,
                IntOut = x.IntOut,
                POnPrOut = x.POnPrOut,
                POnIOut = x.POnIOut,
                IOnIOut = x.IOnIOut
            }).ToList();

            //var company = reportService.CompanyDetails(branchId);
            var company = reportService.CompanyNameDetails();
            string[] columns = { "Account Nunmber", "Account Name", "PrinOut", "IntOut", "POnPrOut", "POnIOut", "IOnIOut" };
            string[] parameterSearch = { "Branch Name:" + branchIdText + "PName:" + pName, " - From Date: " + fromDate };
            byte[] filecontent = ExcelExportHelper.ExportExcel(IndividualLoanBalanceTillDateExportExcel, company, parameterSearch, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "IndividualLoanTillDate.xlsx");
        }

        public ActionResult LoanFollowUp()
        {
            CommonReportModel commonReportModel = new CommonReportModel();
            var date = commonService.GetBranchTransactionDate();
            commonReportModel.FromDate = date;
            commonReportModel.branchId = ReportUtilityService.GetBranchId();
            return PartialView(commonReportModel);

        }
        public ActionResult _LoanFollowUp(DateTime fromDate, int iAccno, int branchId)
        {
            var loanFollowUpList = reportService.LoanFollowUp(fromDate, iAccno, branchId);
            return PartialView(loanFollowUpList);
        }
        [HttpGet]
        public FileContentResult LoanFollowupExportToExcel(int branchId, String branchName, int? iAccno, DateTime fromDate)
        {
            var LoanFollowupList = reportService.LoanFollowUp(fromDate, iAccno, branchId);
            var LoanFollowupExcelList = LoanFollowupList.Select(x => new LoanFollowUpExcelExportModel()
            {
                ProductName = x.ProductName,
                AccountNumber = x.AccountNumber,
                AccountName = x.AccountName,
                OutStandingBalance = x.OutStandingBalance,
                MaturePrincipal = x.MaturePrincipal,
                InterestAccured = x.InterestAccured,
                Penalty = x.Penalty,
                Other = x.Other,
                OtherInterest = x.OtherInterest,
                Rate = x.Rate,
                NoDays = x.NoDays,
                FutureInterest = x.FutureInterest,
                TotalDue = x.TotalDue

            }).ToList();

            //var company = reportService.CompanyDetails(branchId);
            var company = reportService.CompanyNameDetails();
            string[] columns = { "Product Name", "Account Number", "Account Name", "OutStandingBalance", "MaturePrincal", "InterestAccured", "Penalty", "Other", "Other Interest", "Rate", "No. Of Days", "Future Interest", "Total Due" };
            string[] parameterSearch = { "Branch Name:" + branchName + "Date: " + fromDate };
            byte[] filecontent = ExcelExportHelper.ExportExcel(LoanFollowupExcelList, company, parameterSearch, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "LoanFollowup.xlsx");
        }

        public ActionResult LoanAccruedInterestListing()
        {
            CommonReportModel commonReportModel = new CommonReportModel();
            var date = commonService.GetBranchTransactionDate();
            commonReportModel.FromDate = date;
            commonReportModel.branchId = ReportUtilityService.GetBranchId();
            return PartialView(commonReportModel);

        }
        public ActionResult _LoanAccruedInterestListing(DateTime fromDate, int branchId)
        {
            var accruedListing = reportService.LoanAccruedInterestListing(fromDate, branchId);
            return PartialView(accruedListing);

        }
        [HttpGet]
        public FileContentResult loanAccuredInterestListingExportToExcel(int branchId, String branchName, DateTime fromDate)
        {
            var LoanFollowupList = reportService.LoanAccruedInterestListing(fromDate, branchId);
            var LoanFollowupExcelList = LoanFollowupList.Select(x => new LoanAccruedInterestListingExcelExportModel()
            {
                ProductName = x.ProductName,
                AName = x.AName,
                AccNo = x.AccNo,
                MAccured = x.MAccured,
                UMAccured = x.UMAccured,
                TotalAmount = x.TotalAmount
            }).ToList();

            //var company = reportService.CompanyDetails(branchId);
            var company = reportService.CompanyNameDetails();
            string[] columns = { "Product Name", "Account Number", "Account Name", "MAccured", "UMAccured", "Total Amount" };
            string[] parameterSearch = { "Branch Name:" + branchName + "Date: " + fromDate };
            byte[] filecontent = ExcelExportHelper.ExportExcel(LoanFollowupExcelList, company, parameterSearch, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "LoanAccuredInterestListing.xlsx");
        }

        public ActionResult LoanOutStanding()
        {
            CommonReportModel loanTrans = new CommonReportModel();
            var date = commonService.GetBranchTransactionDate();
            loanTrans.FromDate = date.AddMonths(-1);
            loanTrans.ToDate = date;
            loanTrans.branchId = ReportUtilityService.GetBranchId();
            return PartialView(loanTrans);
        }
        public ActionResult _LoanOutStanding(DateTime fromDate, DateTime toDate, int branchId, int revolving)
        {

            var loanOutstanding = reportService.LoanOutStanding(fromDate, toDate, branchId, revolving);
            return PartialView(loanOutstanding);
        }
        [HttpGet]
        public FileContentResult loanOutstandingExportToExcel(int branchId, string branchName, DateTime fromDate, DateTime toDate, int revolving)
        {
            var LoanOutstandingList = reportService.LoanOutStanding(fromDate, toDate, branchId, revolving);
            var LoanOutstandingExcelList = LoanOutstandingList.Select(x => new LoanOutstandingExcelExportModel()
            {
                Accno = x.Accno,
                Aname = x.Aname,
                PName = x.PName,
                MatDat = x.MatDat,
                DisbursedAmount = x.DisbursedAmount,
                OutstandingAmount = x.OutstandingAmount,
                RemainingToDisbursed = x.RemainingToDisbursed,
                AppAmt = x.AppAmt,
                IRate = x.IRate
            }).ToList();

            //var company = reportService.CompanyDetails(branchId);
            var company = reportService.CompanyNameDetails();
            string[] columns = { "Account Number", "Account Name", "Product Name", "MDate", "Disbursed Amount", "Outstanding Amount", "Remaining To Disbursed", "Approved Amount", "Interest Rate" };
            string[] parameterSearch = { "Branch Name:" + branchName + " - From Date: " + fromDate + " - To Date: " + toDate };
            byte[] filecontent = ExcelExportHelper.ExportExcel(LoanOutstandingExcelList, company, parameterSearch, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "LoanOutstanding.xlsx");
        }
        public ActionResult LoanStatement()
        {
            CommonReportModel loanTrans = new CommonReportModel();
            var date = commonService.GetBranchTransactionDate();
            loanTrans.FromDate = date.AddMonths(-1);
            loanTrans.ToDate = date;

            return PartialView(loanTrans);
        }
        public ActionResult _LoanStatement(DateTime fromDate, DateTime toDate, int iaccno)
        {
            TellerService tellerService = new TellerService();
            LoanStatementModel loanStatement = new LoanStatementModel();
            loanStatement.AccountDetails = tellerService.GetAccountDetailsViewShow(iaccno);
            loanStatement.LoanAccountDetails = tellerService.GetLoanOnlyAccountDetailsViewShow(iaccno);
            loanStatement.LoanStatementList = reportService.GetLoanStatement(fromDate, toDate, iaccno);
            loanStatement.AccuredPenalty = reportService.AccuredPenalty(iaccno);
            loanStatement.AccuredInterest = reportService.AccuredInterest(iaccno);
            DateTime? markDate = reportService.MarkDate(iaccno);

            var desc = loanStatement.LoanStatementList.Where(p => p.TranscDate != null).GroupBy(p => p.TranscDate).Select(grp => grp.FirstOrDefault());
            if (markDate != null)
            {
                var dateList = desc.Select(s => new DepositStatementViewModel
                {
                    Tdate = s.TranscDate,
                    MarkDateSelect = s.TranscDate.Value.ToString("MM/dd/yyyy")
                }).Where(x => x.Tdate > markDate).ToList();
                loanStatement.MarkDateList = new SelectList(dateList, "Tdate", "MarkDateSelect");
            }
            else
            {
                var dateList = desc.Select(s => new DepositStatementViewModel
                {
                    Tdate = s.TranscDate,
                    MarkDateSelect = s.TranscDate.Value.ToString("MM/dd/yyyy")
                }).ToList();
                loanStatement.MarkDateList = new SelectList(dateList, "Tdate", "MarkDateSelect");
            }


            return PartialView(loanStatement);
        }
        [HttpGet]
        public FileContentResult LoanStatementExportToExcel(int accountId, DateTime fromDate, DateTime toDate)
        {
            TellerService tellerService = new TellerService();
            LoanStatementModel loanStatement = new LoanStatementModel();
            loanStatement.AccountDetails = tellerService.GetAccountDetailsViewShow(accountId);
            loanStatement.LoanAccountDetails = tellerService.GetLoanOnlyAccountDetailsViewShow(accountId);
            loanStatement.LoanStatementList = reportService.GetLoanStatement(fromDate, toDate, accountId);
            loanStatement.AccuredPenalty = reportService.AccuredPenalty(accountId);
            loanStatement.AccuredInterest = reportService.AccuredInterest(accountId);

            var LoanStatementExcelList = loanStatement.LoanStatementList.Select(x => new LoanStatementExportExcelModel()
            {
                TranscDate = x.TranscDate,
                DrPrincipal = x.DrPrincipal,
                OtherDr = x.OtherDr,
                Rebate = x.Rebate,
                CrPrincipal = x.CrPrincipal,
                OtherCr = x.OtherCr,
                CrPrincInt = x.CrPrincInt,
                CrOtherInt = x.CrOtherInt,
                CrPenalty = x.CrPenalty,
                Balance = x.Balance,
                OtherBal = x.OtherBal,
                TotalPayment = x.TotalPayment,
                Particulars = x.Particulars,
                LoanStatementList = x.LoanStatementList
            }).ToList();

            var company = reportService.CompanyDetails();
            string[] columns = { "Date", "Dr Principal", "Other Dr", "Rebate", "Cr Principal", "Other Cr", "CrPrincInt", "CrOtherInt", "CrPenalty", "Balance", "OtherBal", "Total Payment", "Particulars", "-" };
            string[] parameterSearch = {"Account No:"+loanStatement.AccountDetails.AccounNumber+ " - Reg Date"+loanStatement.AccountDetails.RDate+
                    " - App Amount:"+loanStatement.LoanAccountDetails.SancationAmount+ " - Duration:"+loanStatement.LoanAccountDetails.Month+
                    " - Maturity Date:"+loanStatement.LoanAccountDetails.MaturityDate
                    ,"A/C Holder:"+loanStatement.AccountDetails.AccountTitle+ " - Product Name"+loanStatement.AccountDetails.ProductName+
                    "- Interest Rate:"+loanStatement.AccountDetails.IRate+" - Accured Int:"+loanStatement.AccuredInterest+" - Accured Penalty:"+loanStatement.AccuredPenalty
                    ,"From Date: " + fromDate + " - To Date: " + toDate};
            byte[] filecontent = ExcelExportHelper.ExportExcel(LoanStatementExcelList, company, parameterSearch, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "LoanStatement.xlsx");
        }
        public ActionResult LoanGuarantor()
        {
            CommonReportModel commonReportModel = new CommonReportModel();
            commonReportModel.branchId = ReportUtilityService.GetBranchId();
            return PartialView(commonReportModel);


        }
        public ActionResult _LoanGuarantor(int branchId)
        {
            var accruedListing = reportService.GetLoanGuarantor(branchId);
            return PartialView(accruedListing);

        }
        [HttpGet]
        public FileContentResult GuarantorReportModelExportToExcel(int branchId, string branchIdText)
        {
            var GuarantorReportModel = reportService.GetLoanGuarantor(branchId).ToList();
            //var GuarantorReportModelSelectedList = GuarantorReportModel.Select(x => new GuarantorReportModel()
            //{



            //}).ToList();
            // var company = reportService.CompanyDetails(branchId);
            var company = reportService.CompanyNameDetails();
            string[] columns = { "LoaneeAccNo", "LoaneeName", "GuarantorAccNo", "GuarantorName", "Amount", "LoanApproved", "LoanBalance", "AccBalance" };
            string[] parameterSearch = { "Branch Name:" + branchIdText };
            byte[] filecontent = ExcelExportHelper.ExportExcel(GuarantorReportModel, company, parameterSearch, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "GuarantorReport.xlsx");

        }



        #endregion

        #region UnVerified Transaction Report
        public ActionResult UnverifiedWithdrawDepositReport()
        {
            UnVerifiedTransactionModel asTransModel = new UnVerifiedTransactionModel();

            asTransModel.branchId = ReportUtilityService.GetBranchId();
            return PartialView(asTransModel);
        }
        public ActionResult _UnverifiedWithdrawDepositReport(int branchId, int stype)
        {
            UnVerifiedTransactionModel asTransModel = new UnVerifiedTransactionModel();

            asTransModel.AstransactionList = reportService.UnverifiedWithdrawDepositReport(branchId, stype);
            return PartialView(asTransModel);
        }

        public ActionResult verifiedWithdrawDepositReport()
        {
            UnVerifiedTransactionModel asTransModel = new UnVerifiedTransactionModel();
            asTransModel.tdate = commonService.GetBranchTransactionDate();
            asTransModel.branchId = ReportUtilityService.GetBranchId();
            return PartialView(asTransModel);
        }
        public ActionResult _verifiedWithdrawDepositReport(DateTime transDate, int branchId, int stype)
        {
            UnVerifiedTransactionModel asTransModel = new UnVerifiedTransactionModel();
            asTransModel.AstransactionList = reportService.verifiedWithdrawDepositReport(branchId, transDate, stype);
            return PartialView("_UnverifiedWithdrawDepositReport", asTransModel);
        }
        #endregion

        public ActionResult DepositStatement()
        {
            return PartialView(reportService.DepositStatement());
        }
        public ActionResult _DepositStatementWithDetails(int Iaccno, DateTime? FromDate, DateTime? ToDate)
        {
            return PartialView(reportService.DepositStatementWithDetails(Iaccno, FromDate, ToDate));
        }
        public ActionResult _DepositStatementList(int Iaccno, DateTime? FromDate, DateTime? ToDate)
        {
            return PartialView(reportService.DepositStatementList(Iaccno, FromDate, ToDate));
        }
        [HttpGet]
        public FileContentResult DepositStatementExportToExcel(int accountId, DateTime? FromDate, DateTime? ToDate)
        {
            var DepositStatementWithDetails = reportService.DepositStatementWithDetails(accountId, FromDate, ToDate);
            var DepositStatementList = reportService.DepositStatementList(accountId, FromDate, ToDate);

            List<DepositStatementexportToExcelViewModel> depositStatementExcel = new List<DepositStatementexportToExcelViewModel>();

            var DepositStatementSelectedList = DepositStatementList.Select(x => new DepositStatementexportToExcelViewModel()
            {
                Tdate = x.Tdate,
                Vdate = x.Vdate,
                Description = x.Description,
                Debit = Convert.ToDecimal(x.Debit).ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture),
                Credit = Convert.ToDecimal(x.Credit).ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture),
                Balance = x.Balance,
                DC = x.DC
            }).ToList();
            //var company = reportService.CompanyDetails(0);
            var company = reportService.CompanyNameDetails();
            string[] columns = { "Date ", "VDate", "Description", "Debit", "Credit", "Balance", "DC" };
            string[] parameterSearch = { "Transaction Date:" + DepositStatementWithDetails.Accno + " -Reg. Date" + DepositStatementWithDetails.RegisteredDate+"-Interest Date" + DepositStatementWithDetails.InterestRate +
                    "-Interest On Balance"+ DepositStatementWithDetails.IonBal ,"-A/C Holder"+DepositStatementWithDetails.Aname+"-Accured Interest"+DepositStatementWithDetails.Aname+"-Product Name"+DepositStatementWithDetails.ProductName+"-Sheme Name "+DepositStatementWithDetails.SchemeName

            };
            byte[] filecontent = ExcelExportHelper.ExportExcel(DepositStatementSelectedList, company, parameterSearch, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "DepositStatement.xlsx");
        }
        public ActionResult _MarkPassBook(int AccountId, DateTime markDate)
        {
            try
            {
                returnMessage = reportService.MarkPassBook(AccountId, markDate);
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                returnMessage.Success = false;
                returnMessage.Msg = "error";
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
        }

        #region  Cheque Related Report
        public ActionResult ChequeeIssueBounceAndBlockReport()
        {
            AChqModel achqModel = new AChqModel();
            var date = commonService.GetBranchTransactionDate();
            achqModel.FromDate = date.AddMonths(-1);
            achqModel.ToDate = date;
            return PartialView(achqModel);
        }
        public ActionResult _ChequeeIssueBounceAndBlockReport(DateTime fromDate, DateTime toDate, int branchId, byte status, int iaccno = 0)
        {
            AChqModel achqModel = new AChqModel();
            var date = commonService.GetBranchTransactionDate();
            achqModel.FromDate = date.AddMonths(-1);
            achqModel.ToDate = date;
            achqModel.cstate = status;
            achqModel.AchqList = reportService.ChequeeIssueBounceAndBlockReport(fromDate, toDate, branchId, status, iaccno);
            return PartialView(achqModel);
        }
        [HttpGet]
        public FileContentResult ChequeIssueBlockAndBounceExportToExcel(int branchid, string branchName, byte status, DateTime fromDate, DateTime toDate, int iaccno = 0)
        {
            AChqModel achqModel = new AChqModel();
            var date = commonService.GetBranchTransactionDate();
            achqModel.FromDate = date.AddMonths(-1);
            achqModel.ToDate = date;
            achqModel.cstate = status;
            var ChequeIssueBlockAndBounceList = reportService.ChequeeIssueBounceAndBlockReport(fromDate, toDate, branchid, status, iaccno);
            var ChequeIssueBlockAndBounceExcelExportList = ChequeIssueBlockAndBounceList.Select(x => new CheqqueIssueBounceWithdrawExportExcelModel()
            {
                tdate = x.tdate,
                AccountName = x.AccountName,
                AccountNumber = x.AccountNumber,
                cfrom = x.cfrom,
                cto = x.cto,
                ChequeStatus = x.ChequeStatus

            }).ToList();

            //var company = reportService.CompanyDetails(branchid);
            var company = reportService.CompanyNameDetails();
            string[] columns = { "Date", "Account Name", "Account Number", "From Cheque", "To Cheque", "Status" };
            string[] parameterSearch = { "Branch Name=" + branchName, " - From Date=" + fromDate.ToShortDateString() + " - To Date=" + toDate.ToShortDateString() };
            byte[] filecontent = ExcelExportHelper.ExportExcel(ChequeIssueBlockAndBounceExcelExportList, company, parameterSearch, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "ChequeIssueBlockAndBounce.xlsx");
        }
        public ActionResult InternalChequeDepositByCheque()
        {
            CommonReportModel commonReportModel = new CommonReportModel();
            var date = commonService.GetBranchTransactionDate();
            commonReportModel.FromDate = date.AddMonths(-1);
            commonReportModel.ToDate = date;
            ViewBag.internalChq = "Internal";
            return PartialView(commonReportModel);
        }
        public ActionResult _InternalChequeDepositByCheque(DateTime fromDate, DateTime toDate, int branchId, byte status)
        {
            return PartialView(reportService.InternalChequeDepositByChequeReport(fromDate, toDate, branchId, status));
        }
        [HttpGet]
        public FileContentResult InternalChequeDepositExportToExcel(int branchid, string branchName, byte status, DateTime fromDate, DateTime toDate)
        {
            var InternalChequeDepositList = reportService.InternalChequeDepositByChequeReport(fromDate, toDate, branchid, status).ToList();
            var InternalChequeDepositExcelExportList = InternalChequeDepositList.Select(x => new InternalChequeDepositExportExcelModel()
            {
                TrnxNo = x.TrnxNo,
                Tdate = x.Tdate,
                DrProduct = x.DrProduct,
                CrProduct = x.CrProduct,
                DrAccountNo = x.DrAccountNo,
                DrName = x.DrName,
                CrAccountNo = x.CrAccountNo,
                CrName = x.CrName,
                Amount = x.Amount

            }).ToList();

            //var company = reportService.CompanyDetails(branchid);
            var company = reportService.CompanyNameDetails();
            string[] columns = { "TrnxNo", "Date", "Dr Product", "Cr Product", "Dr Acount No.", "Dr Name", "Cr Account No", "Cr Name", "Amount" };
            string[] parameterSearch = { "Branch Name=" + branchName + " - From Date=" + fromDate.ToShortDateString() + " - To Date=" + toDate.ToShortDateString() };
            byte[] filecontent = ExcelExportHelper.ExportExcel(InternalChequeDepositExcelExportList, company, parameterSearch, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "ChequeGoodForPayment.xlsx");
        }
        public ActionResult ChequeGoodForPaymentReport()
        {
            CommonReportModel commonReportModel = new CommonReportModel();
            var date = commonService.GetBranchTransactionDate();
            commonReportModel.FromDate = date.AddMonths(-1);
            commonReportModel.ToDate = date;
            ViewBag.internalChq = "GoodFor";
            return PartialView("InternalChequeDepositByCheque", commonReportModel);
        }
        public ActionResult _ChequeGoodForPaymentReport(DateTime fromDate, DateTime toDate, int branchId, byte status)
        {
            WithdrawViewModel withdrawModel = new WithdrawViewModel();
            withdrawModel.WithdrawNoPageList = reportService.ChequeGoodForPaymentRepor(fromDate, toDate, branchId, status);
            return PartialView(withdrawModel);
        }
        [HttpGet]
        public FileContentResult ChequeGoodForPaymentExportToExcel(int branchid, string branchName, byte status, DateTime fromDate, DateTime toDate)
        {
            var ChequeIssueBlockAndBounceList = reportService.ChequeGoodForPaymentRepor(fromDate, toDate, branchid, status).ToList();
            var ChequeIssueBlockAndBounceExcelExportList = ChequeIssueBlockAndBounceList.Select(x => new ChequeGoodsForPayementExportExcelModel()
            {
                Tno = x.Tno,
                AccountNumber = x.AccountNumber,
                AccountName = x.AccountName,
                EngDate = x.TransDate,
                ChequeNumber = x.ChequeNumber,
                Amount = x.Amount,
                Payee = x.Payee,
                notes = x.Remarks
            }).ToList();

            //var company = reportService.CompanyDetails(branchid);
            var company = reportService.CompanyNameDetails();
            string[] columns = { "Tno", "Account Name", "Account Number", "Date", "Cheque Number", "Amount", "Payee", "Notes" };
            string[] parameterSearch = { "Branch Name=" + branchName + " - From Date=" + fromDate.ToShortDateString() + " - To Date=" + toDate.ToShortDateString() };
            byte[] filecontent = ExcelExportHelper.ExportExcel(ChequeIssueBlockAndBounceExcelExportList, company, parameterSearch, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "ChequeGoodForPayment.xlsx");
        }

        #endregion

        #region MIS Report
        public ActionResult DepositBalanceTillDteAmountWise()
        {
            DepositBalanceTillDateAmountWiseModel depositBalanceTill = new DepositBalanceTillDateAmountWiseModel();
            depositBalanceTill.BranchId = ReportUtilityService.GetBranchId();
            depositBalanceTill.TDate = commonService.GetBranchTransactionDate();
            return PartialView(depositBalanceTill);
        }
        public ActionResult _DepositBalanceTillDteAmountWise(DateTime tDate, int branchId, int sortId, decimal fAmount = 0, decimal tAmount = 0)
        {
            var depositBalanceTill = reportService.GetDepositBalanceTillDteAmountWise(tDate, branchId, sortId, fAmount, tAmount);
            return PartialView(depositBalanceTill);
        }
        [HttpGet]
        public FileContentResult DepositBalanceAmountWiseExportToExcel(int branchId, string branchName, decimal FAmount, decimal ToAmount, int SortId, DateTime date)
        {
            var DepositBalanceAmountWiseList = reportService.GetDepositBalanceTillDteAmountWise(date, branchId, SortId, FAmount, ToAmount).ToList();
            var DepositBalanceAmountWiseExportToExcelList = DepositBalanceAmountWiseList.Select(x => new DepositBalanceTillDateAmountWiseExcelExportModel()
            {
                AccountNumber = x.AccountNumber,
                AccountName = x.AccountName,
                IntToExp = x.IntToExp,
                IntToCap = x.IntToCap,
                Balance = x.Balance

            }).ToList();

            //var company = reportService.CompanyDetails(branchId);
            var company = reportService.CompanyNameDetails();
            string[] columns = { "Account Number", "Account Name", "IntToExp", "IntToCap", "Balance" };
            string[] parameterSearch = { "Branch=" + branchName, "- Date = " + date };
            byte[] filecontent = ExcelExportHelper.ExportExcel(DepositBalanceAmountWiseExportToExcelList, company, parameterSearch, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "DepositBalanceAmountWise.xlsx");
        }
        //remaining to add in menu
        public ActionResult InterestCapitalization()
        {
            CommonReportModel commonReportModel = new CommonReportModel();
            var date = commonService.GetBranchTransactionDate();
            commonReportModel.FromDate = date.AddMonths(-1);
            commonReportModel.ToDate = date;
            commonReportModel.branchId = ReportUtilityService.GetBranchId();
            return PartialView(commonReportModel);
        }
        public ActionResult _InterestCapitalization(int branchId, DateTime fromDate, DateTime toDate)
        {
            var interestCapitalization = reportService.GetInterestCapitalization(branchId, fromDate, toDate);
            return PartialView(interestCapitalization);
        }
        [HttpGet]
        public FileContentResult InterestCapitalizationExportToExcel(int branchId, string branchName, DateTime fromDate, DateTime toDate)
        {
            var interestCapitalization = reportService.GetInterestCapitalization(branchId, fromDate, toDate).ToList();
            var interestCapitalizationExportExcelList = interestCapitalization.Select(x => new InterestCapitalizationModelExcelExport()
            {
                PName = x.PName,
                CustName = x.CustName,
                FromAccNo = x.FromAccNo,
                VDate = x.VDate,
                IntAmt = x.IntAmt,
                Tax = x.Tax,
                TaxRt = x.TaxRt,
                CRamt = x.CRamt,
                ToAccNo = x.ToAccNo
            }).ToList();

            //var company = reportService.CompanyDetails(branchId);
            var company = reportService.CompanyNameDetails();
            string[] columns = { "Product Name", "Account Name", "Account Number", "Date", "Tax Rate", "Tax Amount", "Interest Amount", "Capitalize", "Posted To A/C" };
            string[] parameterSearch = { "Branch=" + branchName, "From Date=" + fromDate + "- To Date = " + toDate };
            byte[] filecontent = ExcelExportHelper.ExportExcel(interestCapitalizationExportExcelList, company, parameterSearch, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "InterestCapitalization.xlsx");
        }

        public ActionResult InterestLog()
        {
            CommonReportModel commonReportModel = new CommonReportModel();
            int branchID = 0;
            if (!ReportUtilityService.IsAllowAllBranch())
            {
                branchID = commonService.GetBranchIdByEmployeeUserId();
            }
            var date = commonService.GetBranchTransactionDate();
            commonReportModel.FromDate = date.AddMonths(-1);
            commonReportModel.ToDate = date;
            //commonReportModel.branchId = ReportUtilityService.GetBranchId();
            commonReportModel.branchId = branchID;
            return PartialView(commonReportModel);
        }
        public ActionResult _InterestLog(int branchId, DateTime fromDate, DateTime toDate, int? option, int? loanInterest, int? accountType, int iaccNo)
        {
            var interestLog = reportService.GetInterestLog(branchId, fromDate, toDate, option, loanInterest, accountType, iaccNo);
            return PartialView(interestLog);
        }
        [HttpGet]
        public FileContentResult InterestLogExportToExcel(int option, int iAccno, int branchId, string branchName, int AccountTypeId, string AccountTypeName, int LoanInterestId, string LoanInterestName, DateTime fromDate, DateTime toDate)
        {
            var InterestLogList = reportService.GetInterestLog(branchId, fromDate, toDate, option, LoanInterestId, AccountTypeId, iAccno).ToList();
            var InterestLogExcelExport = InterestLogList.Select(x => new InterestLogExcelExportModel()
            {
                TDATE = x.TDATE,
                IACCNO = x.IACCNO,
                PNAME = x.PNAME,
                RATE = x.RATE,
                INTCAL = x.INTCAL,
                BALANCE = x.BALANCE,
                REMARK = x.REMARK
            }).ToList();

            //var company = reportService.CompanyDetails(branchId);
            var company = reportService.CompanyNameDetails();
            string[] columns = { "Date", "Account Number", "Product Name", "Rate", "Int.Cal", "Balance", "Remarks" };
            string[] parameterSearch = { "Branch=" + branchName, "From Date=" + fromDate + "- To Date = " + toDate };
            byte[] filecontent = ExcelExportHelper.ExportExcel(InterestLogExcelExport, company, parameterSearch, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "InterestLog.xlsx");
        }

        public ActionResult Remittance()
        {
            CommonReportModel commonReportModel = new CommonReportModel();
            var date = commonService.GetBranchTransactionDate();
            commonReportModel.FromDate = date.AddMonths(-1);
            commonReportModel.ToDate = date;
            commonReportModel.branchId = ReportUtilityService.GetBranchId();
            return PartialView(commonReportModel);
        }
        public ActionResult _Remittance(DateTime fromDate, DateTime toDate, int branchId, int remit)
        {
            var remittance = reportService.GetRemittance(fromDate, toDate, remit, branchId);
            return PartialView(remittance);
        }
        [HttpGet]
        public FileContentResult RemittanceTransactionExportToExcel(int optionId, string optionName, int branchId, string branchName, DateTime fromDate, DateTime toDate)
        {
            var RemittanceTransactionList = reportService.GetRemittance(fromDate, toDate, optionId, branchId).ToList();
            var RemittanceTransactionExcelExportList = RemittanceTransactionList.Select(x => new RemittanceReportExportExcelModel()
            {
                Tno = x.Tno,
                TDate = x.TDate,
                RemitCompany = x.RemitCompany,
                Particular = x.Particular,
                Amount = x.Amount
            }).ToList();

            //var company = reportService.CompanyDetails(branchId);
            var company = reportService.CompanyNameDetails();
            string[] columns = { "Tno", "TDate", "RemitCompany", "Particular", "Amount" };
            string[] parameterSearch = { "Branch Name=" + branchName + " - From Date=" + fromDate.ToShortDateString() + " - To Date=" + toDate.ToShortDateString() };
            byte[] filecontent = ExcelExportHelper.ExportExcel(RemittanceTransactionExcelExportList, company, parameterSearch, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "RemittanceTransaction.xlsx");
        }

        public ActionResult OverDraftBalance()
        {
            CommonReportModel commonReportModel = new CommonReportModel();
            var date = commonService.GetBranchTransactionDate();
            commonReportModel.FromDate = date;
            commonReportModel.branchId = ReportUtilityService.GetBranchId();
            return PartialView(commonReportModel);
        }
        public ActionResult _OverDraftBalance(DateTime fromDate, int branchId)
        {
            var overDraft = reportService.GetOverDraftBalance(fromDate, branchId);
            return PartialView(overDraft);
        }
        [HttpGet]
        public FileContentResult OverDraftBalanceExportToExcel(int branchId, string branchName, DateTime Date)
        {
            var OverDraftBalanceList = reportService.GetOverDraftBalance(Date, branchId).ToList();
            var OverDraftBalanceExcelExportList = OverDraftBalanceList.Select(x => new OverdraftBalanceExcelExportModel()
            {
                Accno = x.Accno,
                Aname = x.Aname,
                Bal = x.Bal,
                ODInterest = x.ODInterest,
                AppAmt = x.AppAmt,
                ValDat = x.ValDat,
                Month = x.Month,
                MatDat = x.MatDat
            }).ToList();

            //var company = reportService.CompanyDetails(branchId);
            var company = reportService.CompanyNameDetails();
            string[] columns = { "Account Number", "Account Name", "Balance", "OD Interest", "Approve Amount", "Value Date", "Duration", "Mature Date" };
            string[] parameterSearch = { "Branch Name=" + branchName + " - Date=" + Date.ToShortDateString() };
            byte[] filecontent = ExcelExportHelper.ExportExcel(OverDraftBalanceExcelExportList, company, parameterSearch, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "OverDraftBalance.xlsx");
        }

        public ActionResult OverdraftInterestCapitalization()
        {
            CommonReportModel commonReportModel = new CommonReportModel();
            var date = commonService.GetBranchTransactionDate();
            commonReportModel.FromDate = date.AddMonths(-1);
            commonReportModel.ToDate = date;
            commonReportModel.branchId = ReportUtilityService.GetBranchId();
            return PartialView(commonReportModel);
        }
        public ActionResult _OverdraftInterestCapitalization(int branchId, DateTime fromDate, DateTime toDate)
        {
            var interestCapitalization = reportService.GetOverdraftInterestCapitalization(branchId, fromDate, toDate);
            return PartialView(interestCapitalization);
        }
        [HttpGet]
        public FileContentResult OverDraftInterestCapitalizationExportToExcel(int branchId, string branchName, DateTime fromDate, DateTime toDate)
        {
            var OverDraftInterestCapitalizationist = reportService.GetOverdraftInterestCapitalization(branchId, fromDate, toDate).ToList();
            var OverDraftInterestCapitalizationExcelExportList = OverDraftInterestCapitalizationist.Select(x => new OverdraftInterestCapitalizationExportExcelModel()
            {
                Accno = x.Accno,
                Aname = x.Aname,
                Tdate = x.Tdate,
                CapAmt = x.CapAmt,
                TransferedTo = x.TransferedTo,
                tno = x.tno,
                vno = x.vno
            }).ToList();

            //var company = reportService.CompanyDetails(branchId);
            var company = reportService.CompanyNameDetails();
            string[] columns = { "Account Number", "Account Name", "Trnx Date", "Capitalize Amount", "Transferred To", "Trnx Number", "Voucher Number" };
            string[] parameterSearch = { "Branch Name=" + branchName + " - From Date=" + fromDate.ToShortDateString() + " - To Date=" + toDate.ToShortDateString() };
            byte[] filecontent = ExcelExportHelper.ExportExcel(OverDraftInterestCapitalizationExcelExportList, company, parameterSearch, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "OverDraftInterestCapitalization.xlsx");
        }

        public ActionResult ProductWiseInterestLog()
        {
            CommonReportModel commonReportModel = new CommonReportModel();
            var date = commonService.GetBranchTransactionDate();
            commonReportModel.FromDate = date.AddMonths(-1);
            commonReportModel.ToDate = date;
            commonReportModel.branchId = ReportUtilityService.GetBranchId();
            return PartialView(commonReportModel);
        }
        public ActionResult _ProductWiseInterestLog(int branchId, DateTime fromDate, DateTime toDate, int productOption)
        {
            var productWiseIntLog = reportService.GetProductWiseInterestLog(branchId, fromDate, toDate, productOption);
            if (productOption == 1)
                return PartialView("_DepositProductWiseInterestLog", productWiseIntLog);
            else
                return PartialView("_LoanProductWiseInterestLog", productWiseIntLog);

        }
        [HttpGet]
        public FileContentResult ProductWiseInterestLogExportToExcel(int productId, string productName, int branchId, string branchName, DateTime fromDate, DateTime toDate)
        {
            var ProductWiseInterestLogList = reportService.GetProductWiseInterestLog(branchId, fromDate, toDate, productId).ToList();
            var ProductWiseInterestLogExcelExportList = ProductWiseInterestLogList.Select(x => new ProductWiseInterestLogExcelExportModel()
            {
                PName = x.PName,
                DInt = x.DInt
            }).ToList();

            // var company = reportService.CompanyDetails(branchId);
            var company = reportService.CompanyNameDetails();
            string[] columns = { "Product Name", "Interest" };
            string[] parameterSearch = { "Branch Name=" + branchName + " - From Date=" + fromDate.ToShortDateString() + " - To Date=" + toDate.ToShortDateString() };
            byte[] filecontent = ExcelExportHelper.ExportExcel(ProductWiseInterestLogExcelExportList, company, parameterSearch, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "ProductWiseInterestLog.xlsx");
        }

        public ActionResult InterestTOCapitalizeAccount()
        {
            CommonReportModel commonReportModel = new CommonReportModel();
            var date = commonService.GetBranchTransactionDate();
            commonReportModel.FromDate = date;
            commonReportModel.branchId = ReportUtilityService.GetBranchId();
            return PartialView(commonReportModel);
        }
        public ActionResult _InterestTOCapitalizeAccount(DateTime fromDate, int branchId)
        {
            var intoCapitalize = reportService.GetInterestTOCapitalizeAccount(fromDate, branchId);
            return PartialView(intoCapitalize);
        }

        [HttpGet]
        public FileContentResult InterestToCapitalizeAccountExportToExcel(int branchId, string branchName, DateTime Date)
        {
            var InterestToCapitalizeAccountList = reportService.GetInterestTOCapitalizeAccount(Date, branchId).ToList();
            var InterestToCapitalizeAccountExcelList = InterestToCapitalizeAccountList.Select(x => new InterestTOCapitalizeAccountExcelExportModel()
            {
                Accno = x.Accno,
                Aname = x.Aname,
                PName = x.PName,
                intCalpSchm = x.intCalpSchm,
                IRate = x.IRate
            }).ToList();

            // var company = reportService.CompanyDetails(branchId);
            var company = reportService.CompanyNameDetails();
            string[] columns = { "Account Number", "Account Name", "Product Name", "IntCapSchm", "Irate" };
            string[] parameterSearch = { "Branch Name=" + branchName + " - Date=" + Date.ToShortDateString() };
            byte[] filecontent = ExcelExportHelper.ExportExcel(InterestToCapitalizeAccountExcelList, company, parameterSearch, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "InterestToCapitalize.xlsx");
        }

        public ActionResult BalanceCertificate()
        {
            CommonReportModel commonReportModel = new CommonReportModel();
            var date = commonService.GetBranchTransactionDate();
            commonReportModel.FromDate = date;
            commonReportModel.branchId = ReportUtilityService.GetBranchId();
            return PartialView(commonReportModel);
        }
        public ActionResult _BalanceCertificate(DateTime fromDate, int accountNo)
        {
            int branchId = Loader.Models.Global.BranchId;                                                   //currently logged in branch
            BalanceCertificateModel branchCertificate = new BalanceCertificateModel();
            TellerService tellerService = new TellerService();
            var accountDetails = tellerService.GetAccountDetailsViewShow(accountNo);
            var currencyType = tellerService.GetCurrencyByProductId(accountDetails.PID).FirstOrDefault();
            var branchDetails = reportService.GetBranchDetails(branchId);
            //currencyType.FirstOrDefault();
            branchCertificate.currency = currencyType.CurrencyName;
            branchCertificate.AccountNumber = accountDetails.AccounNumber;
            branchCertificate.AccountTitle = accountDetails.AccountTitle;
            branchCertificate.TDate = branchDetails.TDate;
            branchCertificate.BrnchName = branchDetails.BrnhNam;
            branchCertificate.SchemeName = accountDetails.SchemeName;
            branchCertificate.Balance = accountDetails.Balance;
            branchCertificate.Address = branchDetails.Addr;
            branchCertificate.BalanceInWords = ReportUtilityService.ConvertToWords(Convert.ToString(accountDetails.Balance));

            //branchCertificate.BalanceOfDate =                                                             //get balance of date
            return PartialView(branchCertificate);
        }
        public ActionResult TaxCertificate()
        {
            return View();
        }
        public ActionResult _TaxCertificate(int FYID, int accountNo)
        {
            return PartialView();
        }
        #endregion


        public ActionResult ProductTransactionReport()
        {
            return PartialView(reportService.ProductTransactionReport());
        }
        public ActionResult _ProductTransactionReportList(DateTime FDate, DateTime TDate, int searchType, int iSDep, int isVerify)
        {
            return PartialView(reportService.ProductTransactionReportList(FDate, TDate, searchType, iSDep, isVerify));
        }


        [HttpGet]
        public FileContentResult ProductTransactionExportToExcel(DateTime FromDate, DateTime ToDate, int searchType, string searchTypeText, int iSDep, string iSDepText, int isVerify, string isVerifyText)
        {
            var ProductTransactionList = reportService.ProductTransactionReportList(FromDate, ToDate, searchType, iSDep, isVerify);
            var ProductTransactionExportExcel = ProductTransactionList.Select(x => new ProductTransactionExportExcelModel()
            {
                TNO = x.TNO,
                TDate = Convert.ToDateTime(x.VDATE),
                ACCNO = x.ACCNO,
                ANAME = x.ANAME,
                AMT = x.AMT,
                NOTES = x.NOTES

            }).ToList();

            //var company = reportService.CompanyDetails();
            var company = reportService.CompanyNameDetails();
            string[] columns = { "Trxn No.", "Date", "Account Number", "Name", "Amount", "Notes" };
            string[] parameterSearch = { "Deposit/Withdraw: " + iSDepText + " - Verified/Unverified:" + isVerifyText, "Non-Cash/Cash:" + searchTypeText };
            byte[] filecontent = ExcelExportHelper.ExportExcel(ProductTransactionExportExcel, company, parameterSearch, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "ProductTransaction.xlsx");
        }
        public ActionResult LoanTransactionDisbursePayment()
        {
            return PartialView(reportService.LoanTransactionDisbursePayment());
        }
        public ActionResult _LoanTransactionDisbursePayment(DateTime FDate, DateTime TDate, int searchType, int iSDep, int isVerify)
        {
            return PartialView(reportService.LoanTransactionDisbursePaymentList(FDate, TDate, searchType, iSDep, isVerify));
        }
        [HttpGet]
        public FileContentResult LoanTransactionDisbursePaymentExportToExcel(int depositFlag, int verifyFlag, int searchParamFlag, DateTime FromDate, DateTime ToDate)
        {
            var LoanTransactionDisbursePaymentList = reportService.LoanTransactionDisbursePaymentList(FromDate, ToDate, searchParamFlag, depositFlag, verifyFlag);
            var LoanTransactionDisbursePaymentExportExcel = LoanTransactionDisbursePaymentList.ProductTransactionReportList.Select(x => new LoanTransactionDispursePaymentExcelExportModel()
            {
                PNAME = x.PNAME,
                ACCNO = x.ACCNO,
                ANAME = x.ANAME,
                VDATE = x.VDATE,
                PriDr = x.PriDr,
                OthrDr = x.OthrDr,
                NOTES = x.NOTES

            }).ToList();

            //var company = reportService.CompanyDetails();
            var company = reportService.CompanyNameDetails();
            string[] columns = { "Product Name", "Account Number", "Account Name", "Date", "PriDr", "OtherDr", "Notes" };
            string[] parameterSearch = { "From Date: " + FromDate + " - To Date:" + ToDate };
            byte[] filecontent = ExcelExportHelper.ExportExcel(LoanTransactionDisbursePaymentExportExcel, company, parameterSearch, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "AccountWiseCollection.xlsx");
        }
        public ActionResult CollectionSheetReport()
        {
            return PartialView(reportService.CollectionSheetTransReport());
        }
        public ActionResult _CollectionSheetReport(DateTime FDate, DateTime TDate, int? page = 1, string search = null)
        {
            int pageSize = 20;

            //var list = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.SubsiSetup>().GetAll().AsQueryable().OrderByDescending(x=>x.SSId);
            //var pagedList = new Pagination<Models.Models.SubsiSetup>(list, page, pageSize);
            //ViewBag.CountPager = new Pagination<Models.Models.SubsiSetup>(list, page, pageSize).TotalPages;
            var pagedList = reportService.CollectionSheetTransReport(FDate, TDate, page, pageSize, search).ToList();
            ViewBag.CountPager = 0;
            if (pagedList.Count() > 0)
            {
                ViewBag.CountPager = Math.Ceiling((Convert.ToDecimal(pagedList.FirstOrDefault().totalCount)) / (pageSize * 1));
            }
            ViewBag.ActivePager = page;
            return PartialView(pagedList);
            //return PartialView();
        }

        public ActionResult _CollectionSheetReportPartial(DateTime FDate, DateTime TDate, int? page = 1, string search = null)
        {
            int pageSize = 20;

            //var list = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.SubsiSetup>().GetAll().AsQueryable().OrderByDescending(x=>x.SSId);
            //var pagedList = new Pagination<Models.Models.SubsiSetup>(list, page, pageSize);
            //ViewBag.CountPager = new Pagination<Models.Models.SubsiSetup>(list, page, pageSize).TotalPages;
            var pagedList = reportService.CollectionSheetTransReport(FDate, TDate, page, pageSize, search).ToList();
            ViewBag.CountPager = 0;
            if (pagedList.Count() > 0)
            {
                ViewBag.CountPager = Math.Ceiling((Convert.ToDecimal(pagedList.FirstOrDefault().totalCount)) / (pageSize * 1));
            }
            ViewBag.ActivePager = page;
            return PartialView(pagedList);
            //return PartialView();
        }

        [HttpGet]
        public FileContentResult CollectionSheetReportExporttoExcel(DateTime fromDate, DateTime toDate)
        {
            var fromtodate = reportService.CollectionSheetTransReport(fromDate, toDate); //just for now
            var collectionSelectedList = fromtodate.Select(x => new CollectionSheetTransExcelReportViewModel()
            {
                TDate = x.TDate,
                SheetNo = x.SheetNo,
                EmployeeName = x.EmployeeName,
                TotalAmount = x.TotalAmount,
                LoanAmount = x.LoanAmount,
                DepositAmount = x.DepositAmount,
                LoanCount = x.LoanCount,
                DepositCount = x.DepositCount

            }).ToList();
            //var company = reportService.CompanyDetails(0);
            var company = reportService.CompanyNameDetails();
            string[] columns = { "Transaction Date", "SheetNo", "EmployeeName", "TotalAmount", "LoanAmount", "DepositAmount", "LoanCount", "DepositCount" };
            string[] parameterSearch = { "From Date:" + fromDate.ToShortDateString() + " - To Date:" + toDate.ToShortDateString() };
            byte[] filecontent = ExcelExportHelper.ExportExcel(collectionSelectedList, company, parameterSearch, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "CollectionSheetReport.xlsx");
        }
        public ActionResult ProductWiseCollectionSheet()
        {
            return PartialView(reportService.ProductWiseCollectionSheet());
        }
        public ActionResult _ProductWiseCollectionSheet(DateTime fDate, DateTime tDate)
        {
            ViewBag.fromDate = fDate;
            ViewBag.ToDate = tDate;
            ViewBag.Status = 1;
            return PartialView(reportService.ProductWiseCollectionSheet(fDate, tDate));
        }
        //public ActionResult _ProductWiseCollectionSheetPartial(DateTime fDate, DateTime tDate, int? page = 1)
        //{
        //    int pageSize = 10;
        //    ViewBag.fromDate = fDate;
        //    ViewBag.ToDate = tDate;
        //    ViewBag.Status = 1;

        //    var pagedList = reportService.ProductWiseCollectionSheet(fDate, tDate, page, pageSize);
        //    ViewBag.CountPager = 0;
        //    if (pagedList.Count() > 0)
        //    {
        //        ViewBag.CountPager = Math.Ceiling((Convert.ToDecimal(pagedList.FirstOrDefault().totalCount)) / (pageSize * 1));
        //    }
        //    ViewBag.ActivePager = page;
        //    return PartialView(pagedList);
        //}
        //[HttpGet]
        //public FileContentResult ProductWiseReportCollectionSheetExporttoExcel(DateTime fromDate, DateTime toDate)
        //{
        //    var fromtodate = reportService.ProductWiseCollectionSheet(fromDate, toDate);
        //    var collectionSelectedList = fromtodate.ProductWiseCollectionSheetList.Select(x => new ProductWiseCollectionExportToExcelSheet()
        //    {
        //        EmployeeName = x.EmployeeName,
        //        TotalAmount = x.TotalAmount,
        //        PName = x.PName

        //    }).ToList();
        //    var company = reportService.CompanyDetails(0);
        //    string[] columns = { "EmployeeName", "TotalAmount", "Product Name" };
        //    string[] parameterSearch = { "From Date:" + fromDate.ToShortDateString() + " - To Date:" + toDate.ToShortDateString() };
        //    byte[] filecontent = ExcelExportHelper.ExportExcel(collectionSelectedList, company, parameterSearch, columns);
        //    return File(filecontent, ExcelExportHelper.ExcelContentType, "ProductWiseReportCollectionSheetExporttoExcel.xlsx");
        //}


        public ActionResult ProductWiseReportCollectionSheetExporttoExcel(DateTime fromDate, DateTime toDate, int branchId = 0)
        {
            Response.AddHeader("content-disposition", "attachment; filename=ProductWiseReportCollectionSheet.xls");
            Response.ContentType = "application/ms-excel";
            ViewBag.fromDate = fromDate;
            ViewBag.ToDate = toDate;
            ViewBag.Status = 0;
            return PartialView("_ProductWiseCollectionSheet", reportService.ProductWiseCollectionSheet(fromDate, toDate));

        }
        public ActionResult AccountWiseCollectionSheetReport()
        {
            return PartialView(reportService.AccountWiseCollectionSheetTransReport());
        }
        public ActionResult _AccountWiseCollectionSheetReport(DateTime fDate, DateTime tDate, int collectorId, int page = 1)
        {
            int pageSize = 10;

            //var list = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.SubsiSetup>().GetAll().AsQueryable().OrderByDescending(x=>x.SSId);
            //var pagedList = new Pagination<Models.Models.SubsiSetup>(list, page, pageSize);
            //ViewBag.CountPager = new Pagination<Models.Models.SubsiSetup>(list, page, pageSize).TotalPages;
            var pagedList = reportService.AccountWiseCollectionSheetTransReportPagination(fDate, tDate, collectorId, page, pageSize).ToList();
            ViewBag.CountPager = 0;
            if (pagedList.Count() > 0)
            {
                ViewBag.CountPager = Math.Ceiling((Convert.ToDecimal(pagedList.FirstOrDefault().totalCount)) / (pageSize * 1));
            }
            ViewBag.ActivePager = page;
            return PartialView(pagedList);
            //return PartialView(reportService.AccountWiseCollectionSheetTransReport(fDate, tDate, collectorId));
        }

        public ActionResult _AccountWiseCollectionSheetReportPartial(DateTime fDate, DateTime tDate, int collectorId, int page = 1)
        {
            int pageSize = 10;

            //var list = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<Models.Models.SubsiSetup>().GetAll().AsQueryable().OrderByDescending(x=>x.SSId);
            //var pagedList = new Pagination<Models.Models.SubsiSetup>(list, page, pageSize);
            //ViewBag.CountPager = new Pagination<Models.Models.SubsiSetup>(list, page, pageSize).TotalPages;
            var pagedList = reportService.AccountWiseCollectionSheetTransReportPagination(fDate, tDate, collectorId, page, pageSize).ToList();
            ViewBag.CountPager = 0;
            if (pagedList.Count() > 0)
            {
                ViewBag.CountPager = Math.Ceiling((Convert.ToDecimal(pagedList.FirstOrDefault().totalCount)) / (pageSize * 1));
            }
            ViewBag.ActivePager = page;
            return PartialView(pagedList);
            //return PartialView(reportService.AccountWiseCollectionSheetTransReport(fDate, tDate, collectorId));
        }

        [HttpGet]
        public FileContentResult AccountWiseCollectionExportToExcel(int collectorId, DateTime fromDate, DateTime toDate)
        {
            var shareHolderList = reportService.AccountWiseCollectionSheetTransReport(fromDate, toDate, collectorId);
            var shareHolderListExportExcel = shareHolderList.AccountWiseCollectionList.Select(x => new AccountWiseCollectionExportExcelModel()
            {
                Accountnumber = x.Accountnumber,
                Aname = x.Aname,
                TotalAmount = x.TotalAmount,
                PName = x.PName
            }).ToList();

            //var company = reportService.CompanyDetails();
            var company = reportService.CompanyNameDetails();
            string[] columns = { "Account Number", "Account Name", "Total Anmount", "Product Name" };
            string[] parameterSearch = { "From Date: " + fromDate + " - To Date:" + toDate };
            byte[] filecontent = ExcelExportHelper.ExportExcel(shareHolderListExportExcel, company, parameterSearch, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "AccountWiseCollection.xlsx");
        }
        public ActionResult _AccountWiseCollectionSummary(int Iaccno, DateTime fDate, DateTime tDate, int collectorId)
        {
            return PartialView(reportService.AccountWiseCollectionSummary(Iaccno, fDate, tDate, collectorId));
        }

        public ActionResult LoanBeforeAutoPayment()
        {
            return PartialView(reportService.LoanBeforeAutoPayment());
        }
        public ActionResult _LoanBeforeAutoPayment(int branchId, int? Pid)
        {
            return PartialView(reportService.LoanBeforeAutoPayment(branchId, Pid));
        }
        public ActionResult LoanAfterAutoPayment()
        {
            return PartialView(reportService.LoanAfterAutoPayment());
        }
        public ActionResult _LoanAfterAutoPayment(DateTime fDate, DateTime tDate, int branchId)
        {
            return PartialView(reportService.LoanAfterAutoPayment(fDate, tDate, branchId));
        }

        public ActionResult LoanInstallmentDetails()
        {
            return PartialView(reportService.CommonReportService());
        }
        public ActionResult _LoanInstallmentDetails(int accountId)
        {
            return PartialView(reportService.LoanAccountDetail(accountId));
        }
        [HttpGet]
        public FileContentResult loaninstallmentdetailsExporttoExcel(int accountId)
        {
            var loaninstallmentdetails = reportService.LoanAccountDetail(accountId);
            var loaninstallmentscheduleList = reportService.LoanAccountScheduleList(accountId);

            var loaninstallmentscheduleListExportExcel = loaninstallmentscheduleList.Select(x => new LoanAccountWiseScheduleExcelExport()
            {
                Schdate = x.Schdate,
                InstallmentAmount = x.InstallmentAmount,
                Principal = x.Principal,
                Interest = x.Interest,
                Balance = x.Balance,
                NepDate = x.NepDate
            }).ToList();

            //var company = reportService.CompanyDetails();
            var company = reportService.CompanyNameDetails();
            string[] columns = { "Schdate", "InstallmentAmount", "Principal", "Interest", "Balance", "Date" };
            string[] parameterSearch = { "Account Name: "+loaninstallmentdetails.Aname+ " - Account Number: "+loaninstallmentdetails.Accno+" - Scheme Name: "+loaninstallmentdetails.schType+ " - Product Name: " + loaninstallmentdetails.PName+ " - Payment Rule: " + loaninstallmentdetails.PRule + " - Account State: " + loaninstallmentdetails.AccountState,
            "Registered Date: "+loaninstallmentdetails.Rdate+ " - Value Date: "+loaninstallmentdetails.valdat+ " - Matured Date: " +loaninstallmentdetails.matdat+ " - Revolving: " +loaninstallmentdetails.Revolving+ " - Duration:"+loaninstallmentdetails.duration+ " - Rate: "+loaninstallmentdetails.IRate+ " - Quotation Amount: "+loaninstallmentdetails.QuotAmt+" Approved Amount: "+loaninstallmentdetails.AppAmt};
            byte[] filecontent = ExcelExportHelper.ExportExcel(loaninstallmentscheduleListExportExcel, company, parameterSearch, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "LoanScheduledetails.xlsx");
        }

        public ActionResult _LoanInstallmentScheduleList(int accountId)
        {
            return PartialView(reportService.LoanAccountScheduleList(accountId));
        }
        public ActionResult LoanMatured()
        {
            return PartialView(reportService.CommonReportService());
        }
        public ActionResult _LoanMatured(DateTime toDate, int branchId)
        {
            return PartialView(reportService.LoanMatured(toDate, branchId));
        }
        [HttpGet]
        public FileContentResult loanmaturedExporttoExcel(int branchId, string branchName, DateTime toDate)
        {
            var loanMatured = reportService.LoanMatured(toDate, branchId);
            var loanMaturedExportExcel = loanMatured.Select(x => new LoanMaturedExcelExport()
            {
                AccNo = x.AccNo,
                Name = x.Name,
                PName = x.PName,
                MDate = x.MDate,
                MDays = x.MDays,
                ApprovedAmt = x.ApprovedAmt,
                DisbursedAmt = x.DisbursedAmt,
                Balance = x.Balance,
                Total = x.PrincipalPaid,
                PrincipalPaid = x.PrincipalPaid,
                PrincipalPybl = x.PrincipalPybl,
                PrincipalTotal = (x.PrincipalPybl - x.PrincipalPaid),
                InterestPaid = x.InterestPaid,
                InterestPybl = x.InterestPybl,
                InterestTotalDue = (x.InterestPybl - x.InterestPaid)

            }).ToList();

            //var company = reportService.CompanyDetails(branchId);
            var company = reportService.CompanyNameDetails();
            string[] columns = { "AccNo", "Name", "Product Name", "MDate", "Due Days", "Approved Amount", "Disbursed Amount", "Balance", "Total", "Principal Paid", "Principal Due", "Principal Total", "Interest Paid", "Interest Due", "Interest Total Due Balance" };
            string[] parameterSearch = { "Branch Name:" + branchName };
            byte[] filecontent = ExcelExportHelper.ExportExcel(loanMaturedExportExcel, company, parameterSearch, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "LoanMatured.xlsx");
        }
        #region Share Reports
        public ActionResult ShareTransactionByDate()
        {
            return PartialView(reportService.CommonReportService());
        }
        public ActionResult _ShareTransactionByDate(DateTime fromDate, DateTime toDate)
        {
            ViewBag.fromDate = fromDate;
            ViewBag.ToDate = toDate;
            ViewBag.Status = 1;
            return PartialView(reportService.ShareTransactionByDate(fromDate, toDate));

        }
        [HttpGet]
        public FileContentResult ShareTransactionExportToExcel(DateTime fromDate, DateTime toDate)
        {
            var technologies = reportService.ShareTransactionByDate(fromDate, toDate).ToList();
            //var company = reportService.CompanyDetails(0);
            var company = reportService.CompanyNameDetails();
            string[] columns = { "Purchase-Return", "Name", "Date", "From", "To", "Qty", "Amount", "Stype" };
            string[] parameterSearch = { "From Date:" + fromDate.ToShortDateString() + " - To Date:" + toDate.ToShortDateString() };
            byte[] filecontent = ExcelExportHelper.ExportExcel(technologies, company, parameterSearch, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "ShareTransaction.xlsx");
        }


        public ActionResult ShareHolderList()
        {
            return PartialView(reportService.CommonReportService());
        }
        public ActionResult _ShareHolderList(int branchId, int? stype)
        {
            return PartialView(reportService.ShareHolderList(branchId, stype));
        }

        [HttpGet]
        public FileContentResult ShareHolderExportToExcel(int branchId, string branchName, int verifyStatusId, string verifyStatusName)
        {
            var shareHolderList = reportService.ShareHolderList(branchId, verifyStatusId).ToList();
            var shareHolderListExportExcel = shareHolderList.Select(x => new ShareHolderListExportExcel()
            {
                RegNo = x.RegNo,
                Name = x.Name,
                SQtyP = x.SQtyP,
                AmtP = x.AmtP,
                SQtyR = x.SQtyR,
                AmtR = x.AmtR,
                SQty = x.SQty,
                Balance = x.Balance
            }).ToList();

            //var company = reportService.CompanyDetails(branchId);
            var company = reportService.CompanyNameDetails();
            string[] columns = { "Reg Number", "Name", "Purchase Quantity", "Purchase Amount", "Return Quantity", "Return Amount", "Quantity", "Balance" };
            string[] parameterSearch = { "Branch=" + branchName, "Status =" + verifyStatusName };
            byte[] filecontent = ExcelExportHelper.ExportExcel(shareHolderListExportExcel, company, parameterSearch, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "ShareHolderList.xlsx");
        }

        public ActionResult SharePurchaseDetails()
        {
            return PartialView(reportService.CommonReportService());
        }
        public ActionResult _SharePurchaseDetails(DateTime fromDate, DateTime toDate)
        {
            return PartialView(reportService.SharePurchaseDetails(fromDate, toDate));
        }
        [HttpGet]
        public FileContentResult SharePurchaseDetailExportToExcel(DateTime fromDate, DateTime toDate)
        {
            var sharePurchaseList = reportService.SharePurchaseDetails(fromDate, toDate).ToList();
            var sharePurchaseExcelList = sharePurchaseList.Select(x => new SharePurchaseExcelDetails()
            {
                RegNo = x.RegNo,
                Name = x.Name,
                PDate = x.PDate,
                SCrtNo = x.SCrtNo,
                FSNo = x.FSNo,
                TSNo = x.TSNo,
                SQty = x.SQty,
                Amt = x.Amt,
                SType = x.SType == 1 ? "Ordinary" : "Promotor",
                Note = x.Note,
                ttype = x.ttype == 1 ? "Cash" : "Non-Cash"

            }).ToList();

            //var company = reportService.CompanyDetails(0);
            var company = reportService.CompanyNameDetails();
            string[] columns = { "RegNo", "Name", "Date", "CertificateNo", "From", "To", "Quantity", "Amount", "Stype", "Note", "Cash/Non Cash" };
            string[] parameterSearch = { "From Date=" + fromDate.ToShortDateString() + " - To Date=" + toDate.ToShortDateString() };
            byte[] filecontent = ExcelExportHelper.ExportExcel(sharePurchaseExcelList, company, parameterSearch, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "SharePurchaseDetail.xlsx");
        }

        public ActionResult ShareReturnDetails()
        {
            return PartialView(reportService.CommonReportService());
        }
        public ActionResult _ShareReturnDetails(DateTime fromDate, DateTime toDate)
        {
            return PartialView(reportService.ShareReturnDetails(fromDate, toDate));
        }
        [HttpGet]
        public FileContentResult ShareReturnDetailsExportToExcel(DateTime fromDate, DateTime toDate)
        {
            var shareReturnDetailsList = reportService.ShareReturnDetails(fromDate, toDate).ToList();
            var shareReturnDetailsExcelList = shareReturnDetailsList.Select(x => new ShareReturnDetailsExportExcel()
            {
                Name = x.Name,
                RegNo = x.RegNo,
                SCrtNo = x.SCrtNo,
                FSno = x.FSno,
                TSno = x.TSno,
                SQty = x.SQty,
                Amt = x.Amt,
                SType = x.SType == 1 ? "Ordinary" : "Promotor",
                ttype = x.ttype == 1 ? "Cash" : "Non-Cash",
                SoldTo = x.SoldTo,
                Sdate = x.Sdate,
                Note = x.Note
            }).ToList();

            //var company = reportService.CompanyDetails(0);
            var company = reportService.CompanyNameDetails();
            string[] columns = { "Name", "RegNo", "CertificateNo", "From", "To", "Quantity", "Amount", "Stype", "Cash/Non Cash", "SoldTo", "Date", "Note" };
            string[] parameterSearch = { "From Date=" + fromDate.ToShortDateString() + " - To Date=" + toDate.ToShortDateString() };
            byte[] filecontent = ExcelExportHelper.ExportExcel(shareReturnDetailsExcelList, company, parameterSearch, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "SharePurchaseDetail.xlsx");
        }
        public ActionResult ShareStatement()
        {
            return PartialView(reportService.CommonReportService());
        }
        public ActionResult _ShareStatement(int RegNo)
        {
            return PartialView(reportService.ShareStatement(RegNo));
        }
        [HttpGet]
        public FileContentResult ShareStatementExportToExcel(int RegNo)
        {
            var shareStatementList = reportService.ShareStatement(RegNo).ToList();
            var shareStatementSelectedList = shareStatementList.Select(x => new ShareStatementExportToExcel()
            {
                PurchaseAndReturn = x.Pur == 0 ? "Return" : "Purchase",
                RegNo = x.RegNo,
                Name = x.Name,
                SCrtno = x.SCrtno,
                PDate = x.PDate,
                FSno = x.FSno,
                TSno = x.TSno,
                PurQty = x.PurQty,
                RetQty = x.RetQty,
                Amount = x.Amount,
                GFatherName = x.GFatherName,
                FatherName = x.FatherName,
                Tno = x.Tno

            }).ToList();

            //var company = reportService.CompanyDetails(0);
            var company = reportService.CompanyNameDetails();
            string[] columns = { "PurchaseAndReturn", "RegNo", "Name", "Certificate No.", "Date", "From", "To", "Purchase Quantity", "Return Quantity", "Amount", "Grand Father Name", "Father Name", "Tno" };
            string[] parameterSearch = { "Registration No.:" + RegNo };
            byte[] filecontent = ExcelExportHelper.ExportExcel(shareStatementSelectedList, company, parameterSearch, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "ShareStatement.xlsx");
        }
        #endregion
        #region Collector Report
        public ActionResult CollectorWiseAccountOpen()
        {

            return PartialView(reportService.AccountOpenByCollector());
        }
        public ActionResult _CollectorWiseAccountOpen(DateTime fdate, DateTime tdate, int status)
        {


            var collectorWiseAccountOpen = reportService.AccountOpenByCollector(fdate, tdate, status);

            var collectorWiseList = collectorWiseAccountOpen.AccOpenByCollectorViewModelList;

            var check = collectorWiseList.GroupBy(x => new { x.BroughtBy, x.Name }).ToList();
            List<ChannakyaBase.Model.ViewModel.AccOpenByCollectorViewModel> finalList = new List<AccOpenByCollectorViewModel>();
            foreach (var item in check)
            {
                // int count = item.Count();

                AccOpenByCollectorViewModel individualItem = new AccOpenByCollectorViewModel();
                //individualItem.Name = item.SingleOrDefault().Name;
                //individualItem.PName = item.SingleOrDefault().PName;


                int count = 0;
                foreach (var item1 in item)
                {
                    AccOpenByCollectorViewModel childItem = new AccOpenByCollectorViewModel();
                    childItem.BroughtBy = item1.BroughtBy;
                    childItem.Name = item1.Name;
                    individualItem.Name = item1.Name;
                    childItem.PCount = item1.PCount;
                    childItem.PName = item1.PName;
                    individualItem.AccOpenByCollectorViewModelList.Add(childItem);
                    individualItem.BroughtBy = item1.BroughtBy;
                    count = count + (int)item1.PCount;

                }
                individualItem.NoOfCustomers = count;
                finalList.Add(individualItem);
            }

            //foreach (var item in check)
            //{
            //    foreach (var item1 in item)
            //    {
            //        item1.
            //    }
            //}

            //var consolidatedChildren = (from c in kj group c by new
            //                        {
            //                         c.Name,
            //                    c.BroughtBy,

            //                    } into gcs
            //                 select new AccOpenByCollectorViewModel()
            //                    {
            //                       Name=gcs.Key.Name,
            //                       BroughtBy=gcs.Key.BroughtBy,
            //                       PCount= x.dumgcs.FirstOrDefault().NoOfCustomers
            //                       //PName=gcs.FirstOrDefault().PName
            //                       //PCount=gcs.ToList(),

            //                 }).ToList();

            //var ParentCollectorWiseList = collectorWiseList.GroupBy(x => new { x.BroughtBy, x.Name }).Select(x => new AccOpenByCollectorViewModel()
            //{
            //    Name = x.FirstOrDefault().Name,
            //    PName = x.FirstOrDefault().PName,
            //    NoOfCustomers = x.Sum(z => z.NoOfCustomers),

            //}).ToList();
            //collectorWiseAccountOpen.AccOpenByCollectorViewModelList = ParentCollectorWiseList;

            return PartialView(finalList);
        }

        public ActionResult CollectorWiseAccountOpenDetail(int collectorId, DateTime fdate, DateTime tdate, int status, string Pname = "", int? page = 1)
        {
            TempData["collectorId"] = collectorId;
            TempData["Pname"] = Pname;

            int pageSize = 8;
            var pagedList = reportService.AccountOpenByCollectorDetail(collectorId, fdate, tdate, status, Pname, page, pageSize).ToList();
            ViewBag.CountPager = 0;
            if (pagedList.Count() > 0)
            {
                ViewBag.CountPager = Math.Ceiling((Convert.ToDecimal(pagedList.FirstOrDefault().totalCount)) / (pageSize * 1));
            }
            ViewBag.ActivePager = page;

            return PartialView(pagedList);
        }

        public ActionResult CollectorWiseAccountOpenDetailPartial(int collectorId, DateTime fdate, DateTime tdate, int status, string Pname = "", int? page = 1, string search = null)
        {
            TempData["collectorId"] = collectorId;
            TempData["Pname"] = Pname;

            int pageSize = 8;
            var pagedList = reportService.AccountOpenByCollectorDetail(collectorId, fdate, tdate, status, Pname, page, pageSize).ToList();
            ViewBag.CountPager = 0;
            if (pagedList.Count() > 0)
            {
                ViewBag.CountPager = Math.Ceiling((Convert.ToDecimal(pagedList.FirstOrDefault().totalCount)) / (pageSize * 1));
            }
            ViewBag.ActivePager = page;

            return PartialView(pagedList);
        }
        [HttpGet]
        public FileContentResult CollectorWiseAccountOpenDetailExportExcel(DateTime toDate, DateTime fromDate, int status)
        {
            int collectorId = Convert.ToInt32(TempData["collectorId"]);
            string Pname = "";
            var CollectorWiseAccountOpenDetailList = reportService.AccountOpenByCollectorDetail(collectorId, toDate, fromDate, status, Pname);
            var CollectorWiseAccountOpenDetailExportExcel = CollectorWiseAccountOpenDetailList.Select(x => new CollectorDetailExportExcelViewModel()
            {
                Accno = x.Accno,
                AccountName = x.AccountName,
                RDate = x.RDate,
                Balance = x.Balance,
                EmployeeName = x.EmployeeName,
                PName = x.PName,
                SDName = x.SDName
            }).ToList();

            // var company = reportService.CompanyDetails();
            var company = reportService.CompanyNameDetails();
            string[] columns = { "Account Number", "Account Name", "Register Date", "Balance", "Employee Name", "Product Name", "Scheme Name" };
            string[] parameterSearch = { "From Date: " + fromDate + " - To Date: :" };
            byte[] filecontent = ExcelExportHelper.ExportExcel(CollectorWiseAccountOpenDetailExportExcel, company, parameterSearch, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "CollectorWiseAccountOpenDetails.xlsx");
        }

        [HttpGet]
        public FileContentResult CollectorWiseAccountOpenExportToExcel(DateTime fdate, DateTime tdate, int status)
        {

            var CollectorWiseAccountOpenList = reportService.AccountOpenByCollector(fdate, tdate, status);
            var collectorWiseList = CollectorWiseAccountOpenList.AccOpenByCollectorViewModelList;

            var check = collectorWiseList.GroupBy(x => new { x.BroughtBy, x.Name }).ToList();
            List<ChannakyaBase.Model.ViewModel.AccOpenByCollectorViewModel> finalList = new List<AccOpenByCollectorViewModel>();

            foreach (var item in check)
            {
                // int count = item.Count();

                AccOpenByCollectorViewModel individualItem = new AccOpenByCollectorViewModel();
                //individualItem.Name = item.SingleOrDefault().Name;
                //individualItem.PName = item.SingleOrDefault().PName;


                int count = 0;
                foreach (var item1 in item)
                {

                    // individualItem.AccOpenByCollectorViewModelList.Add(childItem);
                    individualItem.Name = item1.Name;

                    individualItem.BroughtBy = item1.BroughtBy;
                    count = count + (int)item1.PCount;

                }
                individualItem.NoOfCustomers = count;
                finalList.Add(individualItem);
            }
            var CollectorWiseAccountOpenListExportExcel = finalList.Select(x => new AccOpenByCollectorExportToExcel()
            {
                Name = x.Name,
                //  PName=x.PName,
                PCount = x.NoOfCustomers
            }).ToList();

            // var company = reportService.CompanyDetails();
            var company = reportService.CompanyNameDetails();
            string[] columns = { "Name", "Product Total" };
            string[] parameterSearch = { "From Date: " + fdate + " - To Date:" + tdate };
            byte[] filecontent = ExcelExportHelper.ExportExcel(CollectorWiseAccountOpenListExportExcel, company, parameterSearch, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "CollectorWiseAccountOpen.xlsx");
        }


        #endregion
        public ActionResult ShareReportByDate()
        {
            return View(reportService.DateWiseShareReport());
        }
        public ActionResult _ShareReportByDate(DateTime fdate, DateTime tdate)
        {
            return PartialView();
        }


        public ActionResult CompanyHeading(string fromDate, string toDate, int branchId = 0)
        {
            ViewBag.fromDate = fromDate;
            ViewBag.toDate = toDate;
            var company = reportService.CompanyDetails(branchId);
            return PartialView("_ReportHeading", company);

        }

     


    


    }
}
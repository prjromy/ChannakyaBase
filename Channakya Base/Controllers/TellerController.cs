//using ChannakyaAccounting.Models.ViewModel;
using Channakya_Base.Models;
using ChannakyaBase.BLL;
using ChannakyaBase.BLL.Service;
using ChannakyaBase.DAL.DatabaseModel;
using ChannakyaBase.Model.Models;
using ChannakyaBase.Model.ViewModel;
using ChannakyaCustomeDatePicker.Models;
using ChannakyaCustomeDatePicker.Service;
using Loader;
using PagedList;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace ChannakyaBase.Web.Controllers
{
    [MyAuthorize]
    public class TellerController : Controller
    {
        TellerService tellerService = null;
        Model.Models.ReturnBaseMessageModel returnMessage = null;
        DatePickerService datePickerService = null;
        private CommonService commonService = null;
        FinanceParameterService financeParameterService = null;
        TransactionService transactionService = null;
        CreditService creditService = null;
        public TellerController()
        {
            tellerService = new TellerService();
            returnMessage = new Model.Models.ReturnBaseMessageModel();
            datePickerService = new DatePickerService();
            commonService = new CommonService();
            transactionService = new TransactionService();
            creditService = new CreditService();
        }
        // GET: Teller

        #region Account Details
        public ActionResult AccountOpenIndex()
        {
            AccountListViewModel aModel = new AccountListViewModel();
            //DateTime from = new DateTime(2019, 01, 01, 0, 0, 0);
            //DateTime to = new DateTime(2019, 01, 01, 0, 0, 0);


            ////aModel.AccountOpenList = tellerService.GetAllAccountList(0, 1, 10);
            //var accountFiteredList = tellerService.GetAllFilteredAccountList(from, to, 1, 10, "", 0, "");
            //aModel.AccountOpenList = new StaticPagedList<AccountListViewModel>(accountFiteredList, 1, 10, (accountFiteredList.Count == 0) ? 0 : accountFiteredList.FirstOrDefault().TotalCount);
            return PartialView(aModel);

        }

        public ActionResult _AccountOpenIndex(/*DateTime? FromDate, DateTime? ToDate,*/ int pageNo = 1, int pageSize = 10, string AccountName = "", int PID = 0, string AccountNumber = "")
        {
            AccountListViewModel aModel = new AccountListViewModel();




            //DateTime FromDateNew = DateTime.ParseExact(FromDat, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            //DateTime ToDateNew = DateTime.ParseExact(ToDat, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            //DateTime FromDateNew = DateTime.Parse(FromDat);
            //DateTime ToDateNew = DateTime.Parse(ToDat);



            //       DateTime FromDateNew = DateTime.ParseExact(FromDat.ToString(), "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            //     DateTime ToDateNew = DateTime.ParseExact(ToDat.ToString(), "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            DateTime date = commonService.GetBranchTransactionDate();

            var FromDate = date;
            var  ToDate= date.AddMonths(-1);
            var accountFiteredList = tellerService.GetAllFilteredAccountListIndex(FromDate, ToDate, pageNo, pageSize, AccountName, PID, AccountNumber);
            aModel.AccountOpenList = new StaticPagedList<AccountListViewModel>(accountFiteredList, pageNo, pageSize, (accountFiteredList.Count == 0) ? 0 : accountFiteredList.FirstOrDefault().TotalCount);
            // aModel.AccountOpenListCheck = accountFiteredList;
            return PartialView(aModel);
        }

        public ActionResult _AccountList(DateTime? FromDate, DateTime? ToDate, int pageNo = 1, int pageSize = 10, string AccountName ="", int PID = 0,string AccountNumber = "")
        {
            AccountListViewModel aModel = new AccountListViewModel();

            var accountFiteredList= tellerService.GetAllFilteredAccountList( FromDate, ToDate ,pageNo, pageSize, AccountName, PID, AccountNumber);
            aModel.AccountOpenList = new StaticPagedList<AccountListViewModel>(accountFiteredList, pageNo, pageSize, (accountFiteredList.Count == 0) ? 0 : accountFiteredList.FirstOrDefault().TotalCount);
           // aModel.AccountOpenListCheck = accountFiteredList;
            return PartialView(aModel);
        }

        public ActionResult GetTransactionDate ()
        {
            AccountsViewModel accountModel = new AccountsViewModel();
            DurationViewModel duration = new DurationViewModel();
            var matDate = commonService.GetBranchTransactionDate();
            var dateTime = datePickerService.GetDateBSAndAD(matDate);
            duration.EnglishDate = dateTime.DateAD;
            duration.NepaliDate = dateTime.DateBS;
            duration.Date = dateTime.Date;
            accountModel.duration = duration;
            //DateTime currentDate = commonService.GetBranchTransactionDate();
            //AccountDetailsViewModel accountDetailsModel = new AccountDetailsViewModel();
            //accountDetailsModel.MaturationDate = currentDate;
            //string getDate = accountModel.ToShortDateString();
            return Json(accountModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateEditAccount(int? IAccno)
        {
            DateTime currentDate = commonService.GetBranchTransactionDate();
            AccountDetailsViewModel accountDetailsModel = new AccountDetailsViewModel();
            accountDetailsModel.RDate = currentDate;
            accountDetailsModel.MaturationDate = currentDate;
            accountDetailsModel.AccountNominee = new ANomineeViewModel();
            string type = commonService.DateType();
            if (type == "1")
            {
                accountDetailsModel.DateType = true;
            }
            return PartialView(accountDetailsModel);
        }

        public ActionResult CheckAgreementAmountWithLAmountProduct(decimal agreeAmount,int PID)
        {
            var checkAgreementAmount = tellerService.AgreementAmountWithLAmountProduct(agreeAmount,PID);
            return Json(checkAgreementAmount, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult CreateEditAccount(AccountDetailsViewModel accountModelDetails, Model.ViewModel.TaskVerificationList TaskVerifier, List<ChargeDetail> ChargeDetailsList)
        {
            try
            {
                returnMessage = tellerService.InsertUpdateAccountOpen(accountModelDetails, TaskVerifier, ChargeDetailsList);
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                returnMessage.Success = false;
                returnMessage.Msg = "error";
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }

        }

        //public ActionResult _AccountSearch(string AccountName = "", string SchemeType = "", string ProductType = "", DateTime,)
        //{

        //}

        public ActionResult AddNominee(AccountNomineeViewModel aNominee)
        {
            ANomineeViewModel accountNominee = new ANomineeViewModel();
            accountNominee.CCertID = aNominee.CCertID;
            accountNominee.CCertno = aNominee.CCertno;
            accountNominee.ContactAddress = aNominee.ContactAddress;
            accountNominee.ContactNo = aNominee.ContactNo;
            accountNominee.NomNam = aNominee.NomNam;
            accountNominee.NomRel = aNominee.NomRel;
            accountNominee.CertificateName = TellerUtilityService.GetCertificateForNominee().Where(x => x.Value == aNominee.CCertID.ToString()).Select(x => x.Text).FirstOrDefault();
            accountNominee.Share = aNominee.Share;
            return PartialView("_AccountNominee", accountNominee);
        }

        [HttpGet]
        public ActionResult _AccountDetails(int IAccno = 0, string ModelFrom = "")
        {
            int sType = commonService.GetStypeByIaccno(IAccno);
            var accountDetials = tellerService.GetSingleAccountDetails(IAccno);
            accountDetials.AccountsWiseCustomer = tellerService.GetAccountsWiseCustomer(IAccno);
            accountDetials.StatusLogList = tellerService.AccountStatusLog(IAccno);
            ViewBag.ModelFrom = ModelFrom;
            if (sType == 0)
            {
                return PartialView(accountDetials);
            }
            else
            {
                return PartialView("~/Views/Credit/LoanAccountDetails.cshtml", accountDetials);
            }
        }
        #endregion

        #region Scheme Product Dropdown Change event
        public ActionResult GetProductBySchemeId(int schemeId = 0)
        {
            bool ifexist = creditService.checkSchemeMappedProduct(schemeId);
            AccountsViewModel accountModel = new AccountsViewModel();
            if (ifexist==true)
            {
                accountModel.Product = tellerService.GetAllProductBySchemeId(schemeId);
                //     accountModel.schemeDetails = tellerService.GetFixedOrRecurringDepositDuration(schemeId);
                //bool result = true;
                return Json(accountModel, JsonRequestBehavior.AllowGet);
                
                
            }
            else
            {
                bool result = false;
                return Json(result, JsonRequestBehavior.AllowGet);
                
            }

           
        }
      
        public JsonResult CheckInterestParam(int interestRate,int productId = 0, int captId = 0, decimal? contrubAmount = 0, int durationId = 0, int basicId = 0)
        {
            ReturnCheckInterestParam returnCheckInterestParam = new ReturnCheckInterestParam();
            var GetinterestRate = tellerService.GetSingleInterestProductDurInt(productId, captId, contrubAmount, durationId, basicId);
          
            if (tellerService.CheckInterestValue(Convert.ToDecimal (interestRate), GetinterestRate))
            {
                returnCheckInterestParam.result = true;
               
                return Json(returnCheckInterestParam, JsonRequestBehavior.AllowGet);
            }
            else
        {
                returnCheckInterestParam.OldInterestRate = GetinterestRate;
                return Json(returnCheckInterestParam, JsonRequestBehavior.AllowGet);
            }
            
        }



        public ActionResult GetAllProductDetailsByProductId(int productId = 0)
        {
            AccountsViewModel accountModel = new AccountsViewModel();

            accountModel.ProductDetails = tellerService.GetProductDetails(productId);

            accountModel.productDurationList = tellerService.GetDurationList(productId);

            accountModel.Policy = tellerService.GetAllInterestCalculationRuleByProductId(productId);

            // accountModel.FloatingInterest = tellerService.GetAllFloatingInterestByProductId(productId);

            accountModel.Currency = tellerService.GetCurrencyByProductId(productId);
            accountModel.InterestCapital = tellerService.GetInterestCapitalizeByProductId(productId);
            accountModel.ChkProductType = tellerService.CheckProductStatusType(productId);
            accountModel.IsChargeAvailable = tellerService.IsChargeAvailable(productId, 4);

            return Json(accountModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCaptRuleByProductAndMatDuration(DateTime registerDate, int productId = 0, int durationId = 0, bool datetype = false)
        {
            AccountsViewModel accountModel = new AccountsViewModel();
            DurationViewModel duration = new DurationViewModel();
            Duration durationById = tellerService.GetDurationByDurationId(durationId);
            double Value = 0;
            if (durationById != null)
            {
                Value = durationById.Value;
            }
            int matDuration = 0;
            string type = commonService.DateType();
            if (datetype == true && type == "2")
            {
                type = "1";
            }
            if (TellerUtilityService.IsCheckByDays(Value))
            {

                decimal days = TellerUtilityService.GetmatDurationDays(Value);
                var matDate = commonService.GetMatDate(registerDate, days, type);
                var dateTime = datePickerService.GetDateBSAndAD(matDate);
                duration.EnglishDate = dateTime.DateAD;
                duration.NepaliDate = dateTime.DateBS;
                duration.Date = dateTime.Date;

            }
            else
            {
                matDuration = TellerUtilityService.GetmatDurationMonth(Value);
                var matDate = commonService.GetMatDate(registerDate, matDuration, type);
                var dateTime = datePickerService.GetDateBSAndAD(matDate);
                duration.EnglishDate = dateTime.DateAD;
                duration.NepaliDate = dateTime.DateBS;
                duration.Date = dateTime.Date;
            }
            accountModel.duration = duration;
            accountModel.InterestCapital = tellerService.GetIntCapitalizeRuleForFixedAndRecurringAndOtherProduct(productId, durationId);
            accountModel.ChkProductType = tellerService.CheckProductStatusType(productId);
            return Json(accountModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCaptRuleByProductDuration(int productId = 0, int durationId = 0)
        {
            AccountsViewModel accountModel = new AccountsViewModel();
            DurationViewModel duration = new DurationViewModel();
            var durationById = tellerService.GetDurationByDurationId(durationId);
            //int matDuration = 0;
            string type = commonService.DateType();
            //if (datetype == true && type == "2")
            //{
            //    type = "1";
            //}
            //if (TellerUtilityService.IsCheckByDays(durationById.Value))
            //{

            //    decimal days = TellerUtilityService.GetmatDurationDays(durationById.Value);
            //    //var matDate = commonService.GetMatDate(registerDate, days, type);
            //    //var dateTime = datePickerService.GetDateBSAndAD(matDate);
            //    //duration.EnglishDate = dateTime.DateAD;
            //    //duration.NepaliDate = dateTime.DateBS;
            //    //duration.Date = dateTime.Date;

            //}
            //else
            //{
            //    matDuration = TellerUtilityService.GetmatDurationMonth(durationById.Value);
            //    //var matDate = commonService.GetMatDate(registerDate, matDuration, type);
            //    //var dateTime = datePickerService.GetDateBSAndAD(matDate);
            //    //duration.EnglishDate = dateTime.DateAD;
            //    //duration.NepaliDate = dateTime.DateBS;
            //    //duration.Date = dateTime.Date;
            //}
            accountModel.duration = duration;
            accountModel.InterestCapital = tellerService.GetIntCapitalizeRuleForFixedAndRecurringAndOtherProduct(productId, durationId);
            accountModel.ChkProductType = tellerService.CheckProductStatusType(productId);
            return Json(accountModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCaptRuleIntRateByProductAndDurationId(int productId, int captId = 0, decimal contrubAmount = 0, int durationId = 0, int basicId = 0)
        {
            var currentInterest = tellerService.GetSingleInterestProductDurInt(productId, captId, contrubAmount, durationId, basicId);
            return Json(currentInterest, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetRecurringBasic(double contrubAmount = 0, int productId = 0, int durationId = 0)
        {
            var recuringBasic = tellerService.GetRecurringBasic(contrubAmount, productId, durationId);
            return Json(recuringBasic, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInterestCapitalizeForRecurringProduct(int durationId = 0, int productId = 0, int basicId = 0, double value = 0)
        {
            var recuttingRuleForCapt = tellerService.GetInterestCapitalizeForRecurringProduct(durationId, productId, basicId, value);
            return Json(recuttingRuleForCapt, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetForceCloseAccountSearchList(byte batchID)
        {
            var accountsList = tellerService.GetAccountListByPID(batchID);
            return PartialView("_ForceCloseAccountsList", accountsList);
        }
        public ActionResult GetAccountSearchList2(int productId = 0, int durationId = 0, int captId = 0, int basicId = 0, decimal value = 0)
        {
            List<AccountDetailsViewModel> recuttingRuleForCapt = tellerService.GetAccountListFilterStype(productId, durationId, captId, basicId, value);
            if(recuttingRuleForCapt.Count() == 0)
            {
                Response.Headers["Counter"] = "0";
            }
            else
            {
                Response.Headers["Counter"] = "1";
            }
            return PartialView("_ChangeProductInterestList", recuttingRuleForCapt);
            //return RedirectToAction("_ChangeProductInterestList",new { advm = recuttingRuleForCapt });
        }
        public ActionResult GetLoanCurrentInterest(int productId = 0) // 5/24/2018 ma banako for loan interest
        {
            ProductDetail productDetail = tellerService.GetProductDetailsByProductId(productId);
            return Json(productDetail, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetStypeChangeInterest(int productId = 0) // 5/24/2018 ma banako for loan interest
        {
            AccountDetailsViewModel stype = transactionService.GetProductType(productId);
            return Json(stype, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Force Close Accounts

        //authorised users can only force close
        [HttpGet]
        public ActionResult ForceCloseAccountsIndex()
        {
            AccountForceCloseViewModel accountForceCloseViewModel = new AccountForceCloseViewModel();
            return View(accountForceCloseViewModel);
        }

        [HttpPost]
        public ActionResult ForceCloseAccountsIndex(AccountForceCloseViewModel accountForceClose) //contents not updated
        {

            try
            {
                var returnMessage = tellerService.ForceCloseAccount(accountForceClose);
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return JavaScript(ex.Message);
            }
        }

        #endregion

        #region Account Search
        public ActionResult _AcountDetailsViewShow(Int64 accountId, string showType)
        {
            AccountDetailsShowViewModel adcShowModel = new AccountDetailsShowViewModel();

            var accountShow = tellerService.GetAccountDetailsViewShow(accountId);
            adcShowModel = accountShow;
            adcShowModel.ShowType = showType;
            adcShowModel.customerAddress = tellerService.GetAccountsWiseCustomer(accountId);
            adcShowModel.AccountBalance = tellerService.GetAccountBalanceByFlag(accountId);
            //if (showType == "LoanDetails")
            //{
            adcShowModel.LoanAccountDetails = tellerService.GetLoanOnlyAccountDetailsViewShow(accountId);
            //}
            return PartialView("_AcountDetailsViewShow", adcShowModel);
        }
        public ActionResult AccountNumberSearch(string accountNumber = "", string filterAccount = "", string accountType = "", int? pid = null)
        {
            AccountSearchViewModel aAccountSearch = new AccountSearchViewModel();
            AccountDetailsViewModel code = new AccountDetailsViewModel();
            if (accountNumber != "")
            {
                code = tellerService.GetCodeByAccountNumber(accountNumber);
                if (code != null)
                {
                    if (pid == null)
                        aAccountSearch.ProductCode = TellerUtilityService.GetProductCode(code.PID);
                }
                else
                {
                    code = new AccountDetailsViewModel();
                    code.BrchID = commonService.GetBranchIdByEmployeeUserId(); ;
                    code.CurrID = commonService.DefultCurrency();
                }

            }
            else
            {
                code.BrchID = commonService.GetBranchIdByEmployeeUserId(); ;
                code.CurrID = commonService.DefultCurrency();
            }
            if (pid != null)
            {
                code = new AccountDetailsViewModel();
                code.PID = Convert.ToByte(pid);
            }
            aAccountSearch.FilterAction = filterAccount;
            aAccountSearch.AccountType = accountType;
            aAccountSearch.BranchId = Convert.ToInt32(code.BrchID);
            aAccountSearch.CurrencyId = code.CurrID;
            aAccountSearch.ProductId = code.PID;
            var accountList = tellerService.GetAccountNumberList(aAccountSearch.BranchId, code.PID, aAccountSearch.CurrencyId, "", filterAccount, accountType, 1, 10);
            aAccountSearch.AccountList = new StaticPagedList<AccountSearchViewModel>(accountList, 1, 10, (accountList.Count == 0) ? 0 : accountList.Select(x => x.TotalCount).FirstOrDefault());
            return PartialView(aAccountSearch);
        }
        public ActionResult _AccountNumberSearch(int branchCode = 0, int productCode = 0, int currencyCode = 0, string customerName = "", string filterAccount = "", string accountType = "", int pageNo = 1, int pageSize = 10)
        {

            AccountSearchViewModel aAccountSearch = new AccountSearchViewModel();
            var accountList = tellerService.GetAccountNumberList(branchCode, productCode, currencyCode, customerName, filterAccount, accountType, pageNo, pageSize);
            aAccountSearch.AccountList = new StaticPagedList<AccountSearchViewModel>(accountList, pageNo, pageSize, (accountList.Count == 0) ? 0 : accountList.Select(x => x.TotalCount).FirstOrDefault());
            //aAccountSearch.BranchId = branchCode;
            //aAccountSearch.CurrencyId = currencyCode;
            return PartialView(aAccountSearch);
        }
        public ActionResult GetSelectAccountNumber(Int64 accountNumber)
        {
            var singleAccountNumber = tellerService.GetSelectAccountNumber(accountNumber);
            return Json(singleAccountNumber, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAccountNumber(string accountNumber, string filterAccount = "", string accountType = "")
        {
            var manyAccountNumber = tellerService.GetAccountNumber(accountNumber, filterAccount, accountType);
            return Json(manyAccountNumber, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ProductList(string productCode, int pageNo = 1, int pageSize = 5)
        {
            ProductViewModel pModel = new ProductViewModel();
            pModel.ProductList = tellerService.GetAllProductList(productCode, pageNo, pageSize);
            return PartialView(pModel);

        }
        public ActionResult _ProductList(string procuctName, int pageNo = 1, int pageSize = 5)
        {
            ProductViewModel pModel = new ProductViewModel();
            pModel.ProductList = tellerService.GetAllProductListByName(procuctName, pageNo, pageSize);
            return PartialView(pModel);
        }
        public ActionResult GetProductByCode(string productCode)
        {
            var productList = tellerService.GetAllProductList(productCode);
            return Json(productList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSelectProduct(int productId)
        {
            ProductViewModel pViewMpdel = new ProductViewModel();
            pViewMpdel = tellerService.GetSingleProduct(productId);
            if (pViewMpdel.IntraBrnhTrnx == false)
            {
                pViewMpdel.BranchList = TellerUtilityService.CurrentBranch(commonService.GetBranchIdByEmployeeUserId());
            }
            else
            {
                pViewMpdel.BranchList = TellerUtilityService.GetBranchList();
            }
            return Json(pViewMpdel, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Deposit Transaction
        public ActionResult Deposit()
        {
            returnMessage = TellerUtilityService.CheckUserActivateOrNot();

            if (returnMessage.Success)

                return PartialView();
            else
                return PartialView("~/Views/Shared/UserNoActivated.cshtml", new HandleErrorInfo(new HttpException(403, "Please activate user to access Deposit transaction!!" + returnMessage.Msg), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()));
        }
        [HttpPost]
        public ActionResult Deposit(DepositViewModel depositModel, DenoInOutViewModel denoInOutModel, Model.ViewModel.TaskVerificationList taskVerifier)
        {
            if (!ModelState.IsValid)
            {
                returnMessage.Success = false;
                returnMessage.Msg = "please fill all input field.";
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
            try
            {
                returnMessage = tellerService.InsertDepositTransaction(depositModel, denoInOutModel, taskVerifier);
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult GetCurrencyRateAndExchangeRate(int currencyId = 0)
        {
            CurrencyRateViewModel currencyAndExchange = new CurrencyRateViewModel();
            currencyAndExchange = tellerService.GetCurrencyRateAndExchangeRate(currencyId);
            if (currencyAndExchange == null)
            {
                currencyAndExchange = new CurrencyRateViewModel();
            }
            currencyAndExchange.IsTransWithDeno = commonService.IsTransactionWithDeno();
            return Json(currencyAndExchange, JsonRequestBehavior.AllowGet);
        }

       public ActionResult DisplayDefaultCurrency(int accountId)
        {
            
            var  tellerservice=tellerService.GetAccountRelatedCurrency(accountId);
            return Json(tellerservice, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Transaction verify details
        public ActionResult UnVerifiedIndex()
        {
            ASTrnViewModel astModel = new ASTrnViewModel();
            var unvefifiedList = tellerService.GetUnverifiedTransactionList("", 1, 10);
            astModel.transactionList = new StaticPagedList<ASTrnViewModel>(unvefifiedList, 1, 10, (unvefifiedList.Count == 0) ? 0 : unvefifiedList.Select(x => x.TotalCount).FirstOrDefault());
            return PartialView(astModel);
        }
        public ActionResult _UnVerifiedIndex(string accountNumber, int pageNo = 1, int pageSize = 10)
        {
            var transactionList = tellerService.GetUnverifiedTransactionList(accountNumber, pageNo, pageSize);
            var UnverifiedtransactionList = new StaticPagedList<ASTrnViewModel>(transactionList, pageNo, pageSize, (transactionList.Count == 0) ? 0 : transactionList.Select(x => x.TotalCount).FirstOrDefault());
            return PartialView(UnverifiedtransactionList);
        }
        public ActionResult VerifyTransaction(Int64 utno, string routeTo)
        {
            ASTrnViewModel asTransModel = new ASTrnViewModel();

            asTransModel = tellerService.GetSingleUnverifiedTransaction(utno);
            ViewBag.RouteTo = routeTo;
            return PartialView("VerifyDeposit", asTransModel);


            //{
            //    return PartialView("~/Views/Credit/LoanAccountDetails.cshtml", asTransModel);
            //}
        }

        public ActionResult CheckCurrentAccountStatus(int accountId)
        {
            var checkAccountStatus = TellerUtilityService.GetCheckValidAccountStatus(accountId);
            return Json(checkAccountStatus, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Deno Setup for transaction
        public ActionResult UseDenoList(Int64 utno)
        {
            DenoInViewModel denoInModel = new DenoInViewModel();
            denoInModel.DenoInList = tellerService.GetDenoByTransactionNumber(utno);

            return PartialView(denoInModel);
        }
        public ActionResult DenoTransaction(int currencyId, string denoInOut)
        {
            DenoInOutViewModel denoInOutModel = new DenoInOutViewModel();
            if (currencyId == 0)
            {
                currencyId = commonService.DefultCurrency();
            }

            denoInOutModel = tellerService.DenoList(currencyId);
            denoInOutModel.DenoInOut = denoInOut;
            denoInOutModel.IsTransactionWithDeno = commonService.IsTransactionWithDeno();
            return PartialView("_DenoTransaction", denoInOutModel);
        }

        public ActionResult DenoTransactionByUser(int currencyId, string denoInOut,int UserId)///for transaction edit to getdeno of posted by 
        {
            DenoInOutViewModel denoInOutModel = new DenoInOutViewModel();
            if (currencyId == 0)
            {
                currencyId = commonService.DefultCurrency();
            }

            denoInOutModel = tellerService.DenoListByUser(currencyId, UserId);
            denoInOutModel.DenoInOut = denoInOut;
            denoInOutModel.IsTransactionWithDeno = commonService.IsTransactionWithDeno();
            return PartialView("_DenoTransaction", denoInOutModel);
        }
        #endregion

        #region Withdraw Transaction
        public ActionResult WithdrawTransaction()
        {
            returnMessage = TellerUtilityService.CheckUserActivateOrNot();
            if (returnMessage.Success)
            {
                WithdrawViewModel withdrawModel = new WithdrawViewModel();
                withdrawModel.IsActiveWithdraw = true;
                return PartialView(withdrawModel);
            }
            else
                return PartialView("~/Views/Shared/UserNoActivated.cshtml", new HandleErrorInfo(new HttpException(403, "Please activate user to access Withdraw transaction!!" + returnMessage.Msg), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()));



        }

        public ActionResult WithdrawTransactionVerifiedByCashier()
        {
            returnMessage = TellerUtilityService.CheckUserActivateOrNot();
            if (returnMessage.Success)
            {
                WithdrawViewModel withdrawModel = new WithdrawViewModel();
                withdrawModel.IsActiveWithdraw = true;
                return PartialView(withdrawModel);
            }
            else
                return PartialView("~/Views/Shared/UserNoActivated.cshtml", new HandleErrorInfo(new HttpException(403, "Please activate user to access Withdraw transaction!!" + returnMessage.Msg), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()));



        }
        [HttpPost]
        public ActionResult WithdrawTransaction(ChannakyaBase.Model.ViewModel.WithdrawViewModel withdrawModel, DenoInOutViewModel denoInOutModel, InterestPayableViewModel intPayModel, Model.ViewModel.TaskVerificationList taskVerifier, ScheduleTrialModel scheduleTModel)
        {

            try
            {
                returnMessage = tellerService.WithdrawTransaction(withdrawModel, denoInOutModel, taskVerifier, intPayModel, scheduleTModel);
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult PayableInterestPayment()
        {
            return PartialView();
        }
        public ActionResult InterestPayable(int accountId = 0)
        {
            var data = tellerService.InterestPayable(accountId);

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckWithdrawPopUpValidation(int accountId)
        {
            var validation = TellerUtilityService.CheckWithDrawPopUpSelectCondition(accountId);
            return Json(validation, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CheckDepositPopUpSelectCondition(int accountId)
        {
            var validation = TellerUtilityService.CheckDepositPopUpSelectCondition(accountId);
            return Json(validation, JsonRequestBehavior.AllowGet);
        }
        public ActionResult checkCheque(int chequeNumber = 0, int accId = 0)
        {
            returnMessage = TellerUtilityService.CheckChequeValidation(chequeNumber, accId);
            return Json(returnMessage, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CheckFixedDepositInternalCheque(int accountId, decimal Amount)
       {
            var result = tellerService.InternalChequeFixedDepositValidAmount(accountId, Amount);


            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult TransactionLimit(decimal amount, int accountId)
        {
            CommonLoanWithdrawModel limitNadRevolving = new CommonLoanWithdrawModel();
            CreditService creditService = new CreditService();
            bool tellerAmount = TellerUtilityService.IsTellerAmountExceed(amount);
            limitNadRevolving.TellerLimit = tellerAmount;

            //  limitNadRevolving.IsRevolving = TellerUtilityService.IsRevolving(accountId);
            //if (limitNadRevolving.IsRevolving)
            //{
            //    var loandetails = creditService.LoanAccountDetails(accountId);
            //    int interestCalRule = loandetails.PAYID;
            //    if (interestCalRule == 3)
            //        limitNadRevolving.HasCustomisedSchedule = true;
            //    else
            //        limitNadRevolving.HasCustomisedSchedule = false;
            //}
            return Json(limitNadRevolving, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UserLimitExceedDetails(int rno)
        {
            var userLimitExceed = tellerService.UserLimitExceedDetails(rno);
            return PartialView(userLimitExceed);
        }

        public ActionResult InterestPayableDetails(int tno)
        {
            InterestPayableViewModel interestPayable = new InterestPayableViewModel();
            interestPayable = tellerService.GetAccountInterestPayabledetails(tno);
            return PartialView(interestPayable);
        }
        public ActionResult UserLimitExceedDetailsVeryfyAfterByCashier(int rno, int eventId = 0, int taskId = 0, string buttonShow = "")
        {
            BaseTaskVerificationService taskVerService = new BaseTaskVerificationService();
            var userLimitExceed = tellerService.UserLimitExceedDetails(rno);
            userLimitExceed.EventId = eventId;
            userLimitExceed.TaskId = taskId;
            var seenOrNotVerification = taskVerService.VerificationSeenOrNot(taskId);
            if (seenOrNotVerification.SeenOn != null)
            {
                taskVerService.NotificationSeenOn(taskId);
            }
            if (taskVerService.VerifiedBy(taskId) != "")
            {
                userLimitExceed.VerifyBy = taskVerService.VerifiedBy(taskId);
            }
            else
            {
                userLimitExceed.VerifyBy = "";
            }
            ViewBag.buttonShow = buttonShow;
            return PartialView(userLimitExceed);
        }

        public ActionResult UserLimitExceedDetailsVerify(int rno,string buttonShow = "")
        {
            BaseTaskVerificationService taskVerService = new BaseTaskVerificationService();
        
            var userLimitExceed = tellerService.UserLimitExceedDetails(rno);
            ViewBag.buttonShow = buttonShow;
            return PartialView(userLimitExceed);
        }

        [HttpPost]
        public ActionResult UserLimitExceedDetailsVeryfyAfterByCashier(AWtdQueueModel awtdModel, DenoInOutViewModel denoInOutModel)
        {
            try
            {
                returnMessage = tellerService.PaymentOfCashLimitExceedAfterVerifyByCashier(awtdModel, denoInOutModel);
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult TellerExceedPayment(int pageNo = 1, int pageSize = 10)
        {
            AWtdQueueModel awtdQueModel = new AWtdQueueModel();
            awtdQueModel.TellerExceedList = tellerService.TellerExceedPayment(pageNo, pageSize, "");
            
            return PartialView(awtdQueModel);
        }
        public ActionResult _TellerExceedPayment(string accountNumber = "", int pageNo = 1, int pageSize = 10)
        {
            AWtdQueueModel awtdQueModel = new AWtdQueueModel();
            awtdQueModel.TellerExceedList = tellerService.TellerExceedPayment(pageNo, pageSize, accountNumber);

         //   int eventId = tellerService.GetEventIdFromRNO(rno);
         //   int task1Id = tellerService.GetTask1IdFromRNO(rno);
           // ViewBag.eventId = eventId;
         //   ViewBag.task1Id = task1Id;
            return PartialView(awtdQueModel);
        }
        #endregion

        #region Charge Details
        public ActionResult ManualChargeIndex()
        {
            return PartialView();
        }
        [HttpGet]
        public ActionResult _ManualChargeCreate(int ChgID = 0)
        {

            financeParameterService = new FinanceParameterService();
            if (ChgID == 0)
            {
                ChargeTransactionViewModel chargeDetail = new ChargeTransactionViewModel();
                return PartialView(chargeDetail);

            }
            else
                return PartialView(financeParameterService.GetSingleChargeSetupById(ChgID));

        }
        [HttpPost]
        public ActionResult ManualChargeCreate(ChargeTransactionViewModel chargeDetails, Model.ViewModel.TaskVerificationList TaskVerifierList, DenoInOutViewModel denoInOutModel)
        {

            financeParameterService = new FinanceParameterService();
            try
            {
                 returnMessage = financeParameterService.ManualChargeUnverifiedTransaction(chargeDetails, TaskVerifierList, denoInOutModel);
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                returnMessage.Success = false;
                returnMessage.Msg = "error";
                return Json(returnMessage, JsonRequestBehavior.AllowGet);

            }


        }
        public ActionResult GetChargeDetailsByChargeId(int chargeId = 0)
        {
            financeParameterService = new FinanceParameterService();
            ChargeDetail chargedet = new ChargeDetail();
            var chargeDetails = financeParameterService.GetSingleChargeSetupById(chargeId);
            chargedet.CAmount = chargeDetails.CAmount;
            chargedet.Ratio = chargeDetails.Ratio;
            chargedet.ChrType = chargeDetails.ChrType;
            chargedet.ChgID = chargeDetails.ChgID;
            chargedet.Chngble = chargeDetails.Chngble;

            return Json(chargedet, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCascadedChargesByModules(int? id)
        {
            SelectList chargeList = FinanceParameterUtilityService.chargesByModules(Convert.ToByte(id));
            return Json(chargeList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _UnVerifiedManualCharge(Int64 utno)
        {
            financeParameterService = new FinanceParameterService();
            ChargeTransactionViewModel unverifiedChargeDetails = new ChargeTransactionViewModel();
            unverifiedChargeDetails = financeParameterService.GetSingleUnverifiedChargeTransaction(utno);

            return PartialView(unverifiedChargeDetails);
        }

        public ActionResult GetFixedAccountsByAccountId(int Iaccno)
        {
            int sType = commonService.GetStypeByIaccno(Iaccno);
            var product = tellerService.GetSingleAccountDetails(Iaccno);

            bool isFixed = TellerUtilityService.IsFixedAccount(product.PID);
            return Json(isFixed, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Collection sheet Details
        public ActionResult CollectionSheetIndex(int retId = 0, int taskId = 0)
        {
            ViewBag.TaskId = taskId;
            ViewBag.RetId = commonService.RetNoBySheetNo(retId);
            return PartialView();
        }
        [HttpGet]
        public ActionResult _CollectionSheetCreate(int retId = 0, int taskId = 0, string message = "")
        {

            if (retId == 0)
            {
                CollectionSheetViewModel CollectionSheet = new CollectionSheetViewModel();

                return PartialView(CollectionSheet);

            }
            else
            {
                CollectionSheetViewModel finalList = tellerService.GetCollectedSheetByRetId(retId);
                finalList.TaskId = taskId;
                finalList.RejectedRemarks = message;
                ViewBag.CollectorSearchName = finalList.COllectorName;
                ViewBag.CollectorName = tellerService.GetCollectors();
                return PartialView(finalList);
            }

        }
        [HttpPost]
        public ActionResult _CollectionSheetCreate(CollectionSheetViewModel collectionSheetViewModel)
        {
            try
            {
                returnMessage = tellerService.CollectionSheetSave(collectionSheetViewModel);
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                returnMessage.Success = false;
                returnMessage.Msg = "error";
                return Json(returnMessage, JsonRequestBehavior.AllowGet);

            }
        }
        [HttpPost]
        public ActionResult _TempCollectionSheet(CollectionSheetViewModel CollectionIndSheet)
        {
            int retId = tellerService.CollectionSheetTempSave(CollectionIndSheet).ReturnId;
            int taskId = CollectionIndSheet.TaskId;
            return RedirectToAction("_CollectionSheetCreate","Teller", new { retId = retId, taskId = taskId });
        }

        public JsonResult CheckSheetNo(int? SheetNo, int RetId = 0)
        {

            bool ifExists = tellerService.CheckSheetNo(SheetNo, RetId);
            return Json(ifExists, JsonRequestBehavior.AllowGet);

        }

        public ActionResult _CollectedSheetList(int retId, string accountNumber = "")
        {
            CollectedAccountSheet collectionList = new CollectedAccountSheet();
            collectionList.Name = accountNumber;
            collectionList.CollectedAccountSheetList = tellerService.TempCollectionSheetList(retId, accountNumber);
            collectionList.TempTotal = collectionList.CollectedAccountSheetList.Select(x => x.Amount).Sum();
            return PartialView(collectionList);

        }
        public ActionResult _CollectedSheetAccountProductSummary(int retId)
        {
            CollectedAccountSheetSummary collectedAccountSheetSummaryList = new CollectedAccountSheetSummary();

            collectedAccountSheetSummaryList.CollectedAccountSheetSummaryList = tellerService.TempCollectionSheetListSummary(retId);

            return PartialView(collectedAccountSheetSummaryList);

        }

        public ActionResult _CollectionLogList()
        {
            CollectedAccountSheet collectionLogList = new CollectedAccountSheet();

            collectionLogList.CollectedAccountSheetList = tellerService.GetCollectionLogList();

            return PartialView(collectionLogList);


        }
        public ActionResult _CollectionLogDetails()
        {
            CollectedAccountSheet collectionLogList = new CollectedAccountSheet();

            collectionLogList.CollectedAccountSheetList = tellerService.GetCollectionLogList();

            return PartialView(collectionLogList);

        }
        public ActionResult DeleteSingleLogHistory(int retId, int TempCollId)
        {
            tellerService.DeleteSingleCollectionLog(retId, TempCollId);
            return RedirectToAction("_CollectionSheetCreate", new { retId = retId });
        }
        public ActionResult DeleteMainLogHistory(int retId)
        {
            try
            {
                tellerService.DeleteCollectionLog(retId);
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                returnMessage.Success = false;
                returnMessage.Msg = "error";
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult _CollectionSheetUnverifiedList()
        {
            List<SmallCollectionSheetViewModel> CollectionLogList = new List<SmallCollectionSheetViewModel>();
            CollectionLogList = tellerService.GetAllUnverifiedCollectionList();
            return PartialView(CollectionLogList);
        }
        public ActionResult _CollectedSheetVerifyAcoountsList(int collSheetId, string accountNumber = "")
        {
            CollectedAccountSheet collectionList = new CollectedAccountSheet();
            collectionList.CollectedAccountSheetList = tellerService.UnverifiedIndividaulCollectionSheetList(collSheetId, accountNumber);
            collectionList.TempTotal = collectionList.CollectedAccountSheetList.Select(x => x.Amount).Sum();
            return PartialView(collectionList);
        }
        [HttpGet]
        public ActionResult _CollectionSheetVerify(int collSheetId)
        {
            return PartialView(tellerService.GetUnverifiedCollectedSheetBycollSheetId(collSheetId));
        }
        [HttpPost]
        public ActionResult _CollectionSheetVerifyPost(CollectionSheetViewModel collectionSheetViewModel, DenoInOutViewModel denoInOutModel, int collSheetId = 0)
        {
            try
            {
                returnMessage = tellerService.VerifySheetBycollSheetId(collectionSheetViewModel.CollSheetId, denoInOutModel);
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                returnMessage.Success = false;
                returnMessage.Msg = "error";
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult _RejectCollectionSheetRemarks()
        {
            CollectionSheetViewModel csvm = new CollectionSheetViewModel();
            return PartialView(csvm);
        }
        [HttpPost]
        public ActionResult _CollectionSheetRejectPost(int sheetNo, string rejectRemarks)
        {
            try
            {
                returnMessage = tellerService.RejectCollectionSheetTransactionByCollSheetId(sheetNo, rejectRemarks);
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                returnMessage.Success = false;
                returnMessage.Msg = "error";
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Remittance Details 
        public ActionResult RemittanceDeposit()
        {
            returnMessage = TellerUtilityService.CheckUserActivateOrNot();
            if (returnMessage.Success)
            {
                return PartialView();
            }
            else
                return PartialView("~/Views/Shared/UserNoActivated.cshtml", new HandleErrorInfo(new HttpException(403, "Please activate user to access !!" + returnMessage.Msg), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()));

        }
        [HttpPost]
        public ActionResult RemittanceDeposit(RemitDepositModel remitDeposit, DenoInOutViewModel denoInOutModel, Model.ViewModel.TaskVerificationList taskVerifier)
        {

            if (!ModelState.IsValid)
            {
                returnMessage.Success = false;
                returnMessage.Msg = "please fill all input field.";
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
            try
            {
                returnMessage = tellerService.RemitanceDeposit(remitDeposit, taskVerifier, denoInOutModel);
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult RemittancedepositDetails(Int64 tno)
        {
            return PartialView(tellerService.RemittanceDepositDetails(tno));
        }
        public ActionResult UnVerifiedRemittancedeposit()
        {
            RemitDepositModel remitDeposit = new RemitDepositModel();
            remitDeposit.RemitDepositList = tellerService.UnverifiedRemittanceDeposit(1, 10);
            return PartialView(remitDeposit);
        }
        public ActionResult _UnVerifiedRemittancedeposit(int pageNo = 1, int pageSize = 10)
        {
            RemitDepositModel remitDeposit = new RemitDepositModel();
            remitDeposit.RemitDepositList = tellerService.UnverifiedRemittanceDeposit(pageNo, pageSize);
            return PartialView(remitDeposit);
        }
        public ActionResult RemittancePayment()
        {
            returnMessage = TellerUtilityService.CheckUserActivateOrNot();
            if (returnMessage.Success)
            {
                return PartialView();
            }
            else
                return PartialView("~/Views/Shared/UserNoActivated.cshtml", new HandleErrorInfo(new HttpException(403, "Please activate user to access !!" + returnMessage.Msg), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()));
        }
        [HttpPost]
        public ActionResult RemittancePayment(RemitPaymentModel remitDeposit, DenoInOutViewModel denoInOutModel, Model.ViewModel.TaskVerificationList taskVerifier)
        {

            if (!ModelState.IsValid)
            {
                returnMessage.Success = false;
                returnMessage.Msg = "please fill all input field.";
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
            try
            {
                returnMessage = tellerService.RemitancePayment(remitDeposit, taskVerifier, denoInOutModel);
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult RemittancePaymentDetails(Int64 tno)
        {
            return PartialView(tellerService.RemittancePaymentDetails(tno));
        }
        public ActionResult UnVerifiedRemittancePayment()
        {
            RemitPaymentModel remitPayment = new RemitPaymentModel();
            remitPayment.RemitPaymentList = tellerService.UnverifiedRemittancePayment(1, 10);
            return PartialView(remitPayment);
        }
        public ActionResult _UnVerifiedRemittancePayment(int pageNo = 1, int pageSize = 10)
        {
            RemitPaymentModel remitPayment = new RemitPaymentModel();
            remitPayment.RemitPaymentList = tellerService.UnverifiedRemittancePayment(pageNo, pageSize);
            return PartialView(remitPayment);
        }
        #endregion

        #region Cheque Clearence
        [HttpGet]

        public ActionResult ChequeClearenceInsert()
        {
            ChequeClearenceViewModel chequeClearenceViewModel = new ChequeClearenceViewModel();
            chequeClearenceViewModel.VDate = commonService.GetBranchTransactionDate();

            return PartialView();
        }
        public ActionResult CheckFixedDepositValidAmount (int accountId, decimal Amount)
        {
            var result = tellerService.FixedDepositValidAmount(accountId, Amount);
          

            return Json(result,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ChequeClearenceInsert(ChequeClearenceViewModel ChequeDetail, Model.ViewModel.TaskVerificationList verificationList)
        {
            ////try
            ////{

                returnMessage = tellerService.ChequeClearenceCreate(ChequeDetail, verificationList);
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            //}
            //catch (Exception)
            //{

            //    return Json(returnMessage, JsonRequestBehavior.AllowGet);
            //}
        }
        public ActionResult ChequeClearenceUnVerificationList()
        {
            ChequeClearenceViewModel listForVerification = new ChequeClearenceViewModel();
            var VerificationList = tellerService.GetVerificationList(1, 10);
            listForVerification.ChequeClearenceWithIpageList = new StaticPagedList<ChequeClearenceViewModel>(VerificationList, 1, 10, (VerificationList.Count == 0) ? 0 : VerificationList.Select(x => x.TotalCount).FirstOrDefault());
            return PartialView(listForVerification);
        }
        public ActionResult _ChequeClearenceUnVerificationList(int pageNo = 1, int pageSize = 10)
        {
            ChequeClearenceViewModel listForVerification = new ChequeClearenceViewModel();
            //   listForVerification.ChequeClearenceList = tellerService.chequeListForVerification();
            var VerificationList = tellerService.GetVerificationList(pageNo, pageSize);
            listForVerification.ChequeClearenceWithIpageList = new StaticPagedList<ChequeClearenceViewModel>(VerificationList, pageNo, pageSize, (VerificationList.Count == 0) ? 0 : VerificationList.Select(x => x.TotalCount).FirstOrDefault());
            return PartialView(listForVerification);
        }


        public ActionResult CheckClearenceDetail(int rno, int? calledFromVerify)
        {
            ChequeClearenceViewModel Detail = new ChequeClearenceViewModel();
            Detail = tellerService.ChequeClearenceDetail(rno);
            if (calledFromVerify != null)
                Detail.calledFromVerify = 1;
            return PartialView(Detail);
        }

        public ActionResult ClearedChequeList()
        {
            AMClearenceViewModel chequeClearedList = new AMClearenceViewModel();
            //var getChequeClearenceList = tellerService.GetAllChequeClearenceList();
            //chequeClearedList.AnclearenceList = getChequeClearenceList;
             DateTime date = commonService.GetBranchTransactionDate();
            chequeClearedList.toDate = date;
            chequeClearedList.fromDate = date.AddMonths(-1);
      
            var clearedList = tellerService.GetClearedlist("", "", date.AddMonths(-1), date, 1, 10);
            chequeClearedList.AmclearenceWithIPageList = new StaticPagedList<AMClearenceViewModel>(clearedList, 1, 10, (clearedList.Count == 0) ? 0 : clearedList.Select(x => x.TotalCount).FirstOrDefault());
            return PartialView(chequeClearedList);
        }

        public ActionResult _ClearedChequeList(string IAccno, string bankname, DateTime toDate, DateTime fromdate, int pageNo = 1, int pageSize = 10)
        {
            AMClearenceViewModel ChequeClearedList = new AMClearenceViewModel();
            var chequeClearedList = tellerService.GetClearedlist(IAccno, bankname, fromdate, toDate, pageNo, pageSize);
            ChequeClearedList.AmclearenceWithIPageList = new StaticPagedList<AMClearenceViewModel>(chequeClearedList, pageNo, pageSize, (chequeClearedList.Count == 0) ? 0 : chequeClearedList.Select(x => x.TotalCount).FirstOrDefault());
            return PartialView(ChequeClearedList);
        }
        #endregion

        public ActionResult DepositAccountRenew(int iaccno, byte productId)
        {
            AccountRenewModel accountDetailsModel = new AccountRenewModel();
            DateTime currentDate = commonService.GetBranchTransactionDate();
            accountDetailsModel.MaturationDate = currentDate;
            accountDetailsModel.IAccno = iaccno;
            accountDetailsModel.PID = productId;
            accountDetailsModel.RDate = currentDate;
            accountDetailsModel.IsFixedAccount = TellerUtilityService.IsFixedAccount(productId);
            accountDetailsModel.ProductDurationList = tellerService.GetDurationList(productId);
            accountDetailsModel.AgreementAmount = TellerUtilityService.DebitLimitAmount(iaccno);
            accountDetailsModel.Policy = tellerService.GetAllInterestCalculationRuleByProductId(productId);
            return PartialView(accountDetailsModel);
        }

        [HttpPost]
        public ActionResult DepositAccountRenew(AccountRenewModel renewModel)
        {
            try
            {
                returnMessage = tellerService.DepositAccountRenewSave(renewModel);
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                returnMessage.Success = true;
                returnMessage.Msg = "error";
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
        }
    }
}


using ChannakyaBase.BLL;
using ChannakyaBase.BLL.CustomHelper;
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

namespace ChannakyaBase.Web.Controllers
{

    [MyAuthorize]
    public class CreditController : BaseController
    {
        ReturnBaseMessageModel returnMessage = null;
        DatePickerService datePickerService = null;
        private CommonService commonService = null;
        private CreditService creditService = null;
        private ShareService shareService = null;
        TellerService tellerService = null;
        
        DatePickerService dateService = null;
        public CreditController()
        {
            creditService = new CreditService();
            returnMessage = new ReturnBaseMessageModel();
            datePickerService = new DatePickerService();
            commonService = new CommonService();
            tellerService = new TellerService();
            dateService = new DatePickerService();
            shareService = new ShareService();
        }

        public ActionResult Index()
        {
            return View();
        }

        #region Schedule Trial
        public ActionResult ScheduleTrialIndex()
        {
            ScheduleModel schModel = new ScheduleModel();
            schModel.ValueDate = commonService.GetBranchTransactionDate();
            schModel.InterestDate = commonService.GetBranchTransactionDate();
            schModel.PrincipalDate = commonService.GetBranchTransactionDate();
            schModel.PrincipalFrequency = 1;
            schModel.InterestFrequency = 1;
            return PartialView(schModel);
        }

        public JsonResult PreviewScheduleTrial(ScheduleModel scheduleModel)
        {
            try
            {
                ScheduleTrialModel scheduleTModel = new ScheduleTrialModel();
                ScheduleTrialModel scheduleTModel1 = new ScheduleTrialModel();
                List<ScheduleTrialModel> scheduleTModelList = new List<ScheduleTrialModel>();

                if (scheduleModel.IAccno != 0)
                {

                    if (scheduleModel.RequestFrom == "renew")
                    {
                        scheduleTModel1 = creditService.ScheduleDataRenewByIaccno(scheduleModel.IAccno);
                        if (scheduleModel.PaymentMode == 3)
                        {
                            scheduleTModel1.HasInterest = true;
                            scheduleTModel1.DateAd = scheduleTModel1.DateAd.AddDays(-1);
                        }
                    }
                    else
                        scheduleTModel1 = creditService.ScheduleDataByIaccno(scheduleModel.IAccno);
                    if (scheduleTModel1 != null)
                        scheduleTModelList.Add(scheduleTModel1);
                }
                var trialSchedule = creditService.GetScheduleDetails(scheduleModel);
                scheduleTModelList.AddRange(trialSchedule);
                scheduleTModel.scheduleList = scheduleTModelList;
                //foreach (var item in trialSchedule)
                //{
                //    scheduleTModel.scheduleList.AddRange(item);
                //}
                if (scheduleModel.PaymentMode == 3)
                {

                    scheduleTModel.ButtonTap = 1;
                    scheduleTModel.Remprecent = 100;
                    scheduleTModel.Percent = 0;
                    scheduleTModel.IsFlat = scheduleModel.Flat;
                    scheduleTModel.Amount = scheduleModel.Amount;
                    scheduleTModel.Rate = scheduleModel.Rate;
                    scheduleTModel.StartDate = scheduleModel.ValueDate;
                    scheduleTModel.RequestFrom = scheduleModel.RequestFrom;
                    scheduleTModel.IAccno = scheduleModel.IAccno;
                    return Json(new
                    {
                        Status = true,
                        Message = "",
                        MaxJsonLength = 50000000,
                        htmlToShow = RenderPartialViewToString("_CustomizeInstallment", scheduleTModel),
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        Status = true,
                        Message = "",
                        MaxJsonLength = 50000000,
                        htmlToShow = RenderPartialViewToString("PreviewScheduleTrial", scheduleTModel),
                    }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {

                return Json(new
                {
                    Status = false,
                    Message = ex.Message,
                }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public ActionResult _CustomizeInstallmentAmountCalculation(ScheduleTrialModel scheduleModel)
        {

            ScheduleTrialModel scheduleTModel = new ScheduleTrialModel();
            try
            {

                scheduleTModel.IsFlat = scheduleModel.IsFlat;
                scheduleTModel.Amount = scheduleModel.Amount;
                scheduleTModel.Rate = scheduleModel.Rate;
                scheduleTModel.StartDate = scheduleModel.StartDate;
                scheduleTModel.RequestFrom = scheduleModel.RequestFrom;
                scheduleTModel.IAccno = scheduleModel.IAccno;
                if (scheduleModel.ButtonTap == 1)
                {

                    scheduleTModel.Remprecent = 100;
                    scheduleTModel.Percent = 0;

                    scheduleTModel.scheduleList = creditService.GetCustmizeInstellment(scheduleModel);
                }
                else
                {
                    //decimal remPer = scheduleTModel.Remprecent - scheduleTModel.Precent;
                    //scheduleTModel.Remprecent = remPer;
                    //scheduleTModel.Precent = remPer;
                    scheduleTModel = creditService.GetFinalizeCustmizeInstellment(scheduleModel);
                }
                scheduleTModel.ButtonTap = 2;
                ModelState.Clear();
            }

            catch (Exception ex)
            {

                return Json(new
                {
                    Status = false,
                    Message = ex.Message,
                }, JsonRequestBehavior.AllowGet);
            }

            if (scheduleTModel.scheduleList[scheduleTModel.scheduleList.Count() - 1].IsChecked == true)
            {
                if (scheduleModel.RequestFrom == "renew")
                {
                    var scheduleTModel1 = creditService.ScheduleDataRenewByIaccno(scheduleModel.IAccno);
                    scheduleTModel.scheduleList[0].InterestInstall = scheduleTModel1.InterestInstall;
                    scheduleTModel.scheduleList[0].Balance = 0;

                }

                return Json(new
                {
                    Status = true,
                    Message = scheduleTModel.RequestFrom,
                    htmlToShow = RenderPartialViewToString("PreviewScheduleTrial", scheduleTModel),
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    Status = true,
                    Message = "",
                    htmlToShow = RenderPartialViewToString("_CustomizeInstallmentAmountCalculation", scheduleTModel),
                }, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion

        #region Loan Payment
        public ActionResult LoanRebate()
        {
            //returnMessage = TellerUtilityService.CheckUserActivateOrNot();

            //if (returnMessage.Success)
            //{
            LoanRebateModel loanPayment = new LoanRebateModel();
                return PartialView(loanPayment);
            //}
            //else
            //    return PartialView("~/Views/Shared/UserNoActivated.cshtml", new HandleErrorInfo(new HttpException(403, "Please activate user to access loan payment!!" + returnMessage.Msg), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()));

        }
        public ActionResult LoanRebateDetails(int? accountId)
        {
            //returnMessage = TellerUtilityService.CheckUserActivateOrNot();

            //if (returnMessage.Success)
            //{
            LoanRebateModel loanPayment = new LoanRebateModel();
            loanPayment = creditService.GetTotalAmount(accountId);
            return PartialView(loanPayment);
            //}
            //else
            //    return PartialView("~/Views/Shared/UserNoActivated.cshtml", new HandleErrorInfo(new HttpException(403, "Please activate user to access loan payment!!" + returnMessage.Msg), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()));

        }
        [HttpPost]
        public ActionResult InsertRebate(LoanRebateModel loanRebate, Model.ViewModel.TaskVerificationList TaskVerifier)
        {
            try
            {
                returnMessage = creditService.InsertLoanRebate(loanRebate, TaskVerifier);
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult LoanPaymentIndex()
        {
            returnMessage = TellerUtilityService.CheckUserActivateOrNot();

            if (returnMessage.Success)
            {
                LoanPaymentModel loanPayment = new LoanPaymentModel();
                return PartialView(loanPayment);
            }
            else
                return PartialView("~/Views/Shared/UserNoActivated.cshtml", new HandleErrorInfo(new HttpException(403, "Please activate user to access loan payment!!" + returnMessage.Msg), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()));

        }
        public ActionResult LoanAccountPayment(int accountId, string radioPayment)
        {
            if (radioPayment == "0")
            {
                var showLoanPayment = creditService.GetAccountLoanPaymentDetails(accountId);
                showLoanPayment.IsMature = commonService.IsShowMatureInterestOnly();
                return PartialView(showLoanPayment);

            }
            else
            {
                return PartialView("NonCashLoanPayment");
            }
         
        }
        public ActionResult _LoanAccountDetailsInformationShow(int accountId)
        {
            var loanInformationShow = creditService.LoanAccountDetailsInformation(accountId);
            return PartialView(loanInformationShow);
        }
        public JsonResult CheckTotalAmountPaid(int accountId, decimal balance/*, decimal discount*/, bool isMatInterest)
        {
            decimal CalculateTotal = CreditUtilityService.PayCalcutation(accountId, "Other");
            var matureAmount = creditService.CurrentMaturePrinciapAndInterest(accountId);
            decimal totalAmount = (decimal)matureAmount.CurrentPrincipal + CalculateTotal + (decimal)matureAmount.MaturePrincipal;
            if (isMatInterest)
            {
                totalAmount = totalAmount - Convert.ToDecimal(creditService.GetLoanPayment(accountId).IonPA);
            }
            decimal finalPay = totalAmount /*- discount*/;
            if (balance > finalPay)
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Payment cannot be greater than Outstanding amount of this A/c which is:" + finalPay +
                                     ".Payment amount will be automatically adjusted to this amount.";
                returnMessage.ValueOne = finalPay;
            }
            else
            {
                returnMessage.Success = true;
            }
            return Json(returnMessage, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCalculateExtraInterest(int accountId, int paymentMode, decimal currentRemainingPrincipal, DateTime matDate)
        {
            if (paymentMode == 1)
            {
                matDate = commonService.GetBranchTransactionDate();
            }
            else if (paymentMode == 2)
            {
                matDate = commonService.GetBranchTransactionDate().AddDays(1);
            }
            else if (paymentMode == 3)
            {
                matDate = creditService.GetNextIntallmentDate(accountId);
            }
            DurationViewModel duration = new DurationViewModel();
            var dateTime = datePickerService.GetDateBSAndAD(matDate);
            duration.EnglishDate = dateTime.DateAD;
            duration.NepaliDate = dateTime.DateBS;
            duration.Date = dateTime.Date;
            var calculateInterest = creditService.GetCalculateExtraInterest(accountId, currentRemainingPrincipal, matDate);

            return Json(new { duration, calculateInterest }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckTotalRebateAmount(int accountId, decimal discount)
        {
            decimal CalculateTotal = CreditUtilityService.PayCalcutation(accountId, "");

            if (discount > CalculateTotal)
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Maximum Rebate allowed for this payment is:" + CalculateTotal +
                                    "Rebate amount will be adjusted accordingly.";
                returnMessage.ValueOne = CalculateTotal;
            }
            else
            {
                returnMessage.Success = true;
            }
            return Json(returnMessage, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult InsertLoanPayment(LoanPaymentModel loanPayment, DenoInOutViewModel denoInOutModel, Model.ViewModel.TaskVerificationList TaskVerificationList)
        {
            try
            {
                returnMessage = creditService.InsertLoanPayment(loanPayment, denoInOutModel, TaskVerificationList);
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult LoanPaymentVerification(Int64 utno)
        {
            ASTrnViewModel asTransModel = new ASTrnViewModel();
            asTransModel = tellerService.GetSingleUnverifiedTransaction(utno);
            asTransModel.ASTransLoanList = creditService.GetASTransLoanList(utno);
            return PartialView(asTransModel);
        }

        public ActionResult GetLedgerDetails( string flag , int? page = 1)
        {

            const int pageSize = 12;
            IQueryable<FinAcc> finaccList;
            if (flag == "1")
            {
                finaccList = new BLL.Repository.GenericUnitOfWork().Repository<FinAcc>().FindBy(x => x.FinSys2.FinSys1.IsGroup == false).Where(x => x.FinSys2.FinSys1.F1id == 15).AsQueryable();

            }
            else if (flag == "2")
            {
                finaccList = new BLL.Repository.GenericUnitOfWork().Repository<FinAcc>().FindBy(x => x.FinSys2.FinSys1.IsGroup == false).Where(x => x.FinSys2.FinSys1.F1id == 300).AsQueryable();

            }
            else
            {
                finaccList = new BLL.Repository.GenericUnitOfWork().Repository<FinAcc>().FindBy(x => x.FinSys2.FinSys1.IsGroup == false).Where(x => x.FinSys2.FinSys1.F1id == 2 || x.FinSys2.FinSys1.F1id == 7 || x.FinSys2.FinSys1.F1id == 5).AsQueryable();

            }

            //if (fId != 0)
            //{
            //    finaccList = new ChannakyaAccounting.Repository.UnitOfWork.Implementation_Classes.GenericUnitOfWork().Repository<FinAcc>().FindBy(x => x.Fid == fId && x.FinSys2.FinSys1.IsGroup == false).Where(x => x.FinSys2.FinSys1.F1id != 3).AsQueryable();
            //}
            ViewBag.ActivePager = page;
            //if (!string.IsNullOrEmpty(query))
            //{
            //    finaccList = finaccList. Where(m => m.Fname.Contains(query));
            //    ViewBag.Query = query;
            //}
            var dataList = finaccList.OrderBy(m => m.Fname);
            //var pagedList = new Pagination<FinAcc>(dataList, page ?? 0, pageSize);
            //ViewBag.CountPager = new Pagination<Models.Models.FinAcc>(dataList, page ?? 0, pageSize).TotalPages;
            //ViewBag.CountPager = pagedList.TotalPages;
            //ViewBag.ActivePager = page;
            return Json(dataList, JsonRequestBehavior.AllowGet);

        }


        #endregion

        #region Collateral entery
        public ActionResult InsertUpdateCollateral(int ncId = 0, int colId = 0, int landId = 0, int fixedDepoId = 0, int vehicleId = 0, int iAccno = 0)
        {

            ALCollModel alcModel = new ALCollModel();
            if (iAccno != 0)
            {
                var acountdetails = commonService.GetAccountNameByIaccno(iAccno);
                alcModel.AccountNumber = acountdetails.Accno;
            }
            alcModel.Sno = colId;
            alcModel.IAccno = iAccno;
            alcModel.NCID = ncId;
            alcModel.LandId = landId;
            alcModel.fixedDepoId = fixedDepoId;
            alcModel.VehicleId = vehicleId;

            //List<Guarantor> collaterallist = creditService.GetCollateralListforPledgeInternalfromGarantor(iAccno);
            //List<GuarantorModel> guarantorModelsListInternal = new List<GuarantorModel>();
            //int test = guarantorModelsListInternal.Count();
            
            //int num = collaterallist.Count();
           
            //for (int j = 0; j < num; j++)
            //{

            //    guarantorModelsListInternal.Add(new GuarantorModel
            //    {
            //        GId = collaterallist[j].GId,
            //        LIaccno = collaterallist[j].LIaccno,
            //        GIaccno = collaterallist[j].GIaccno,
            //        BlockedAmt = collaterallist[j].BlockedAmt,
            //        DisplayMsg = collaterallist[j].DisplayMsg,
            //        IsPercent = collaterallist[j].IsPercent,
            //        Status = collaterallist[j].Status,
            //        PostedBy = collaterallist[j].PostedBy,
            //        PostedOn = collaterallist[j].PostedOn

            //    });

            //}

            //alcModel.guarantorModel = guarantorModelsListInternal;

            return PartialView(alcModel);
        }
        public ActionResult CollateralDetails(int iAccno)
        {

            var collateralDetails = creditService.GetCollateralListByIAccno(iAccno);

            return PartialView(collateralDetails);
        }
        public ActionResult _AccountLoanCollateral(int colId = 0)
        {
            ALCollModel alcModel = new ALCollModel();
            if (colId != 0)
            {
                alcModel = creditService.GetAlColById(colId);
            }

            return PartialView(alcModel);
        }
        public ActionResult _InternalFixedAccountCollateral(int alFixedId = 0, bool isInternal = false,int IAccno=0)
        {
            ALFixedDepositModel fixedDepositModel = new ALFixedDepositModel();

            if (alFixedId != 0)
            {
                fixedDepositModel = creditService.GetInternalFixedAccountCollateralById(alFixedId);
            }
            else
            {
                fixedDepositModel.IsInternalAccount = isInternal;
            }


            List<Guarantor> collaterallist = creditService.GetCollateralListforPledgeInternalfromGarantor(IAccno);
            List<GuarantorModel> guarantorModelsListInternal = new List<GuarantorModel>();
            int test = guarantorModelsListInternal.Count();

            int num = collaterallist.Count();

            for (int j = 0; j < num; j++)
            {

                guarantorModelsListInternal.Add(new GuarantorModel
                {
                    GId = collaterallist[j].GId,
                    LIaccno = collaterallist[j].LIaccno,
                    GIaccno = collaterallist[j].GIaccno,
                    BlockedAmt = collaterallist[j].BlockedAmt,
                    DisplayMsg = collaterallist[j].DisplayMsg,
                    IsPercent = collaterallist[j].IsPercent,
                    Status = collaterallist[j].Status,
                    PostedBy = collaterallist[j].PostedBy,
                    PostedOn = collaterallist[j].PostedOn

                });

            }

            fixedDepositModel.guarantorModel = guarantorModelsListInternal;


            return PartialView(fixedDepositModel);
        }
        public ActionResult _LandAndBuilding(int landId = 0)
        {
            ALCollLandModel acolLandModel = new ALCollLandModel();
            if (landId != 0)
            {
                acolLandModel = creditService.GetLandAndBuildingById(landId);
            }
            return PartialView(acolLandModel);
        }
        public ActionResult _Vehicles(int vechileId = 0)
        {
            ALCollVehicleModel alVehicleModel = new ALCollVehicleModel();
            if (vechileId != 0)
            {
                alVehicleModel = creditService.GetVehicleById(vechileId);
            }
            return PartialView(alVehicleModel);
        }
        [HttpPost]
        public ActionResult InsertUpdateCollateral(ALCollModel alcModel, ALFixedDepositModel fixedDepositModel, ALCollLandModel alLandModel, ALCollVehicleModel alVehicleModel)
        {
            try
            {
                returnMessage = creditService.InsertUpdateCollateral(alcModel, fixedDepositModel, alLandModel, alVehicleModel);
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult CollateralInternalAccountNoDetails(int iAccNo)
        {
            var accountDetails = tellerService.GetSingleAccount(iAccNo);

            DateModel dModel = dateService.GetDateBSAndAD(accountDetails.RDate);

            return Json(new { endDate = dModel.DateAD, nepDate = dModel.DateBS, date = accountDetails.RDate.ToShortDateString(), Amount = accountDetails.Bal }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CollateralList()
        {
            ALCollModel alcModel = new ALCollModel();
            var collList = creditService.GetCollateralList("", 1, 10);
            alcModel.CollList = new StaticPagedList<ALCollModel>(collList, 1, 10, (collList.Count == 0) ? 0 : collList.Select(x => x.TotalCount).FirstOrDefault());
            return PartialView(alcModel);
        }
        public ActionResult _CollateralList(string accountNumber = "", int pageNo = 1, int pageSize = 10)
        {
            ALCollModel alcModel = new ALCollModel();
            var collList = creditService.GetCollateralList(accountNumber, pageNo, pageSize);
            alcModel.CollList = new StaticPagedList<ALCollModel>(collList, pageNo, pageSize, (collList.Count == 0) ? 0 : collList.Select(x => x.TotalCount).FirstOrDefault());
            return PartialView(alcModel);
        }
        #endregion


        public ActionResult GuarantorEntry()
        {
            ViewBag.ModelFrom = "From Gurantor";
            return PartialView();
        }

        public JsonResult GetGuarantorDetail(int accountid)
        {
            List<GuarantorModel> guarantorModel = new List<GuarantorModel>();
            guarantorModel = tellerService.GetGuarantorListForLoanAccount(accountid);
            if (guarantorModel != null)
            {
                TempData["Key"] = true;
            }
            else
            {
                TempData["Key"] = false;
            }
            return Json(new
            {
                Status = returnMessage.Success,
                Message = returnMessage.Msg,
                htmlToShow = RenderPartialViewToString("GetGuarantorDetail", guarantorModel),
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddGuarantor(GuarantorModel guarantorModel)
        {  
            GuarantorModel objguarantorModel = new GuarantorModel();
            objguarantorModel.AccountNumber = guarantorModel.AccountNumber;
            objguarantorModel.GIaccno = guarantorModel.GIaccno;
            objguarantorModel.IsPercent = guarantorModel.IsPercent;
            objguarantorModel.BlockedAmt = guarantorModel.BlockedAmt;
            if (guarantorModel.IsPercent == false)
            {
                returnMessage = CreditUtilityService.CheckAvailableBalance(guarantorModel.GIaccno, guarantorModel.BlockedAmt);
            }
            else
            {
                returnMessage.Success = true;
            }

            return Json(new
            {
                Status = returnMessage.Success,
                Message = returnMessage.Msg,
                htmlToShow = RenderPartialViewToString("AddGuarantor", objguarantorModel),
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult InsertUpdateGuarantor(GuarantorModel guarantorModel)
        {
            try
            {

                returnMessage = creditService.InsertUpdateGuarantor(guarantorModel);
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
        }
        #region Loan Register Details      
        public ActionResult LoanRegistration()
        {
            return View(creditService.LoanRegistration());
        }
        [HttpPost]
        public ActionResult LoanRegistration(LoanRegAccOpenViewModel loanRegAccOpenViewModel, Model.ViewModel.TaskVerificationList TaskVerificationList)
        {
            try
            {

                returnMessage = creditService.LoanRegistrationSave(loanRegAccOpenViewModel, TaskVerificationList);
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }

        }



        public ActionResult GetAllLoanProductDetailsByPid(int productId)
        {

            //bool ifexist = creditService.checkSchemeMappedProduct(productId);
            //if (ifexist == true)
            //{
            ProductViewModel productViewModel = new ProductViewModel();
            productViewModel = creditService.GetProductDetails(productId);
            return Json(productViewModel, JsonRequestBehavior.AllowGet);



        }
        public ActionResult GetAllInterestRatebyPid(int productId)
        {

            var productPSID = creditService.GetAllRuleCalculation(productId);
            return Json(productPSID, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllPenaltyCalbyPid(byte productId)
        {

            var productPSID = creditService.GetAllPenalty(productId);
            return Json(productPSID, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetProductDetailsbyPid(int productId)
        {
            var productPSID = creditService.GetSingleProductDetails(productId);
            productPSID.IsChargeAvailable = tellerService.IsChargeAvailable(productId, 23);
            return Json(productPSID, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UnverifiedCustomerDetails(int regId = 0)
        {
            List<CustomerAccountsViewModel> loanCustomer = new List<CustomerAccountsViewModel>();
            loanCustomer = creditService.LoanCustomerByregId(regId);
            return PartialView(loanCustomer);
        }

        public ActionResult LoanRegistrationVerifyDetails(int regId, int taskId = 0)
        {
            LoanRegAccOpenViewModel data = creditService.LoanRegistration(regId, taskId);
            return PartialView(data);
        }
        public ActionResult VerifiedRegisteredLoanAccountsIndex()
        {
            return PartialView();
        }

        public ActionResult _VerifiedRegisteredLoanAccounts(string customerName = null)

        {
            if (customerName == null)
            {
                return PartialView(creditService.AllLoanRegVefifiedCustomer());
                //return Json(creditService.AllLoanRegVefifiedCustomer(), JsonRequestBehavior.AllowGet);
            }

            else
            {
                var singleLoanVerifiedCustomer = creditService.SingleLoanRegVerifiedCustomer(customerName);

                return PartialView(singleLoanVerifiedCustomer);
            }
        }

        public ActionResult LoanAccountOpen(int regId = 0, int taskId = 0, int isAfterRegistration = 0)
        {
            return View(creditService.LoanRegistration(regId, isAfterRegistration: isAfterRegistration, taskId: taskId));
        }
        [HttpPost]
        public ActionResult LoanAccountOpen(LoanRegAccOpenViewModel LoanAccOpen, Model.ViewModel.TaskVerificationList TaskVerificationList, List<ChargeDetail> ChargeDetailsList, List<GuarantorModel> GuarantorList)
        {
            try
            {
                returnMessage = creditService.LoanAccountOpen(LoanAccOpen, TaskVerificationList, ChargeDetailsList, GuarantorList);
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult _LinkAccountDetails(int iaccno, bool isDetailView = false, string ModelFrom = "")
        {
            ViewBag.ModelFrom = ModelFrom;
            return PartialView(creditService.GetSingleLinkAccountDetailsByIaccno(iaccno, isDetailView));
        }
        public ActionResult _LoanAttributes(int PId)
        {
            return PartialView(creditService.GetLoanProductAttributes(PId));
        }

        public ActionResult GetCaptRuleByProductAndMatDuration(DateTime registerDate, decimal duration = 0, bool datetype = false, int durtype = 0, int productId = 0)
        {
            AccountsViewModel accountModel = new AccountsViewModel();
            DurationViewModel durationall = new DurationViewModel();
            string type = commonService.DateType();
            var productDetails = creditService.GetProductDetails((byte)productId);
            if (productDetails.HasIndividualDuration == false)
                duration = (decimal)productDetails.Duration;
            if (datetype == true && type == "2")
            {
                type = "1";
            }
            if (durtype == 0)
            {

                decimal days = TellerUtilityService.GetmatDurationDays((double)duration);
                var matDate = commonService.GetMatDate(registerDate, days, type);
                var dateTime = datePickerService.GetDateBSAndAD(matDate);


                //     DateTime res = DateTime.Parse(dateTime.DateAD, CultureInfo.GetCultureInfo("en-gb"));


                durationall.EnglishDate = dateTime.DateAD;
                durationall.NepaliDate = dateTime.DateBS;
                durationall.Date = dateTime.Date;

            }
            else
            {
                var matDate = commonService.GetMatDate(registerDate, duration, type);
                var dateTime = datePickerService.GetDateBSAndAD(matDate);
                durationall.EnglishDate = dateTime.DateAD;
                durationall.NepaliDate = dateTime.DateBS;
                durationall.Date = dateTime.Date;
            }
            accountModel.duration = durationall;
            return Json(accountModel, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]

        public ActionResult LoanAccountDetails(int IAccno = 0,string modelFrom="")
        {
            TellerService ts = new TellerService();
            var accountDetials = ts.GetSingleAccountDetails(IAccno);
            accountDetials.AccountsWiseCustomer = ts.GetAccountsWiseCustomer(IAccno);
            accountDetials.APolicyInterest = ts.GetSingleInterestPolicyDetails(IAccno);
            accountDetials.StatusLogList = ts.AccountStatusLog(IAccno);
            ViewBag.modelFrom = modelFrom;
            return PartialView(accountDetials);
        }
        public ActionResult LoanAccountIndex()
        {
            AccountDetailsViewModel aModel = new AccountDetailsViewModel();
            var accountOpenList = creditService.GetAllAccountList(0, 1, 10);
            aModel.AccountOpenList = aModel.AccountOpenList = new StaticPagedList<AccountDetailsViewModel>(accountOpenList, 1, 10, (accountOpenList.Count == 0) ? 0 : accountOpenList.Select(x => x.TotalCount).FirstOrDefault());
            return PartialView(aModel);
        }
        public ActionResult _LoanAccountList(int accState = 0, int pageNo = 1, int pageSize = 10)
        {
            AccountDetailsViewModel aModel = new AccountDetailsViewModel();
            var accountOpenList = creditService.GetAllAccountList(accState, pageNo, pageSize);
            aModel.AccountOpenList = new StaticPagedList<AccountDetailsViewModel>(accountOpenList, pageNo, pageSize, (accountOpenList.Count == 0) ? 0 : accountOpenList.Select(x => x.TotalCount).FirstOrDefault());
            return PartialView(aModel);
        }

        public JsonResult GetGraceDate(int graceOption, decimal dayMonth, DateTime date)
        {
            if (graceOption == 2)
            {
                dayMonth = TellerUtilityService.GetmatDurationDays(Convert.ToDouble(dayMonth));
            }
            string type = commonService.DateType();

            DateTime returnDate = commonService.GetMatDate(date, dayMonth, type);
            DurationViewModel duration = new DurationViewModel();
            var dateTime = datePickerService.GetDateBSAndAD(returnDate.AddDays(1));
            duration.EnglishDate = dateTime.DateAD;
            duration.NepaliDate = dateTime.DateBS;
            duration.Date = dateTime.Date;
            return Json(duration, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LoanDisbursement()
        {
            LoanDisbursement aModel = new LoanDisbursement();
            aModel.IsOtherLoan = false;
            return PartialView("LoanDisbursementIndex", aModel);
        }
        public ActionResult OtherLoanDisbursement()
        {
            LoanDisbursement aModel = new LoanDisbursement();
            aModel.IsOtherLoan = true;
            //ViewBag.ModelFrom = "OtherLoan";
            return PartialView("LoanDisbursementIndex", aModel);
        }
        [HttpPost]
        public ActionResult LoanDisbursementCreate(LoanDisbursement loanDisbursement, DisburseChequeDepositViewModel loandisburseCheque, DisburseCashDepositViewModel loanDisburseCash, DisburseChargeDepositViewModel AddtionalCharge, List<DisburseDepositViewModel> DisburseDepositList, List<ChargeDetail> ChargeDetailsList, Model.ViewModel.TaskVerificationList TaskVerificationList, ScheduleTrialModel scheduleTrialModel, List<DisburseShareViewModel> ShareList)
        {

            try
            {
                returnMessage = creditService.LoanDisburseSave(loanDisbursement, loandisburseCheque, loanDisburseCash, AddtionalCharge, DisburseDepositList, ChargeDetailsList, TaskVerificationList, scheduleTrialModel, ShareList);
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                //  returnMessage.Success = false;
                // returnMessage.Msg = "error";
                return Json(returnMessage, JsonRequestBehavior.AllowGet);

            }
        }
        public ActionResult _LoanDepositAccountsForDisbursement(int iaccno = 0,string ModelFrom="")
        {
            DisburseDepositViewModel disburseDeposit = new DisburseDepositViewModel();
            disburseDeposit.DisburseDepositList = creditService.DisburseLinkAccounts(iaccno);
            ViewBag.ModelFrom = ModelFrom;
            return PartialView(disburseDeposit);
        }

        public ActionResult _LoanDisburseByCheque(int iaccno = 0)
        {
            DisburseChequeDepositViewModel DisburseChequeDeposit = new DisburseChequeDepositViewModel();
            return PartialView(DisburseChequeDeposit);
        }
        public ActionResult _LoanDisburseByCash(int iaccno = 0)
        {
            DisburseCashDepositViewModel DisburseCash = new DisburseCashDepositViewModel();
            return PartialView(DisburseCash);
        }
        public ActionResult _AddtionalChargeForDisburse()
        {
            DisburseChargeDepositViewModel DisburseCash = new DisburseChargeDepositViewModel();
            return PartialView(DisburseCash);
        }
        public ActionResult _AddtionalChargeList()
        {
            DisburseChargeDepositViewModel DisburseCash = new DisburseChargeDepositViewModel();
            return PartialView(DisburseCash);
        }

        public ActionResult _LoanDisburseToShare(int iaccno)
        {
            DisburseShareViewModel DisburseShare = new DisburseShareViewModel();
            DisburseShare.DisburseShareList = creditService.DisburseLoanToShare(iaccno);
        
            if (DisburseShare.DisburseShareList.Count == 0)
            {
                return Json(new
                {
                    Status = false,
                    Message = "",
                    htmlToShow = "",
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    Status = true,
                    Message = "",
                    htmlToShow = RenderPartialViewToString("_LoanDisburseToShare", DisburseShare),
                }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult LoanDisburseDetailsByIaccno(bool isOtherLoan, int accountId = 0)
        {
            LoanDisbursement accountModel = new LoanDisbursement();
            accountModel.IsOtherLoan = isOtherLoan;
            var loandetails = creditService.LoanAccountDetails(accountId);
            int interestCalRule = loandetails.PAYID;
            accountModel.IsRevolving = Convert.ToBoolean(loandetails.ADetail.ALoan.Revolving);
            var count = loandetails.ADetail.DisbursementMains.Count();
            if (count > 0)
            {
                accountModel.PrevChargeDeductMode = Convert.ToInt32(loandetails.IsChargeOnSancation);
                //    ViewBag.sanctionstatus = false;
                var resultsanctionCheck = creditService.GetSanctionStatusforRemaingAmountonChange(accountId);
                if (resultsanctionCheck == false)

                {
                    accountModel.PrevChargeDeductMode = 0;
                }



            }
            else
                accountModel.PrevChargeDeductMode = 2;//here 2 means none
            if (accountModel.IsRevolving == true)
            {
                accountModel.ChargeDeductOnSanction = true;
                accountModel.ApprovedAmount = loandetails.ADetail.ADrLimit.AppAmt;
            }
            if (interestCalRule == 3)
            {
                accountModel.HasCustomisedSchedule = true;
            }
            else
                accountModel.HasCustomisedSchedule = false;
            int productId = tellerService.GetSingleAccountDetails(accountId).PID;
            accountModel.IsChargeAvailable = tellerService.IsChargeAvailable(productId, 20);
            if (accountModel.IsOtherLoan)
            {
                accountModel.DisbursableAmount = loandetails.ADetail.ADrLimit.AppAmt - Convert.ToDecimal(loandetails.ADetail.ALoan.OtherLoanOut);
            }
            else
            {
                accountModel.DisbursableAmount = loandetails.ADetail.ADrLimit.AppAmt - Convert.ToDecimal(loandetails.ADetail.ALoan.PrincipleLoanOut);
            }

            accountModel.Product = new ProductViewModel();
            accountModel.Product.ProductId = productId;

            if (commonService.ChargeDeductOnDisbursement() == 2)
            {
                accountModel.ChargeDeductOnDisburse = true;
            }
            else
            {
                accountModel.ChargeDeductOnSanction = true;
            }
            return PartialView(accountModel);
        }
        public ActionResult DisburseVerifyIndex(int disburseId = 0, int taskId = 0)
        {
            return PartialView(creditService.GetDisbursementMainDetailsBy(disburseId, taskId));
        }
        public ActionResult CheckPreviousLoanDisburse(int accountId = 0)
        {
            return Json(creditService.CheckExistingLoans(accountId), JsonRequestBehavior.AllowGet);
        }
        public ActionResult _LoanDisbursementDetailsForVerify(int disburseId = 0,string modelFrom="")
        {
            ShrSPurchase shrSPurchase = new ShrSPurchase();
            String stringDisburse = Convert.ToString(disburseId);
            
            shrSPurchase = shareService.GetService(stringDisburse);
            ViewData["ShareList"] = shrSPurchase;
            ViewBag.modelFrom = modelFrom;
            return PartialView(creditService.GetDisbursementMainDetailsBy(disburseId));
        }
        [HttpPost]
        public ActionResult _LoanDisbursementDetailsForVerifyConfirm(int disburseId = 0)
        {
            try
            {
                returnMessage = creditService.LoanDisburseVerifyConfirm(disburseId);
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                returnMessage.Success = false;
                returnMessage.Msg = "error";
                return Json(returnMessage, JsonRequestBehavior.AllowGet);

            }

        }
        public ActionResult DisburseCashVerifyByTeller(DisbursementMain disbursementmain, DenoInOutViewModel denoInOutModel)
        {
            try
            {
                returnMessage = creditService.DisburseCashVerifyByTeller(disbursementmain, denoInOutModel);
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                returnMessage.Success = false;
                returnMessage.Msg = "error";
                return Json(returnMessage, JsonRequestBehavior.AllowGet);

            }

        }
        public ActionResult LoanDisburseVerifyIndex()
        {
            LoanDisbursement aModel = new LoanDisbursement();
            var accountOpenList = creditService.AllLoanDisburseVerifiedCustomer(0, 1, 10);
            aModel.LoanDisbursementList = aModel.LoanDisbursementList = new StaticPagedList<LoanDisbursement>(accountOpenList, 1, 10, (accountOpenList.Count == 0) ? 0 : accountOpenList.Select(x => x.TotalCount).FirstOrDefault());
            return PartialView(aModel);
        }
        public ActionResult _LoanDisburseVerifyList(int accState = 0, int pageNo = 1, int pageSize = 10)
        {
            LoanDisbursement aModel = new LoanDisbursement();
            var LoanDisburseUnverifiedList = creditService.AllLoanDisburseVerifiedCustomer(accState, pageNo, pageSize);
            aModel.LoanDisbursementList = new StaticPagedList<LoanDisbursement>(LoanDisburseUnverifiedList, pageNo, pageSize, (LoanDisburseUnverifiedList.Count == 0) ? 0 : LoanDisburseUnverifiedList.Select(x => x.TotalCount).FirstOrDefault());
            return PartialView(aModel);
        }
        public ActionResult IsFlatScheduleType()
        {
            return View();
        }
        public JsonResult _CustomisedLoanSchedule(decimal regularLoan, int accountId = 0)
        {
            ScheduleTrialModel scheduleTModel = new ScheduleTrialModel();
            ScheduleTrialModel scheduleTModel1 = new ScheduleTrialModel();
            List<ScheduleTrialModel> scheduleTModelList = new List<ScheduleTrialModel>();
            decimal previousamount = 0;
            var scheduleparams = creditService.ScheduleParameters(accountId);
            scheduleparams.RequestFrom = "disbursement";
            scheduleparams.Amount = regularLoan;


            ScheduleModel scheduleModel = new ScheduleModel();
            scheduleModel = scheduleparams;
            var lastdisburse = creditService.ScheduleDataByIaccno(accountId);
            if (lastdisburse != null)
            {

                scheduleparams.IAccno = accountId;
                previousamount = creditService.ScheduleDataByIaccno(accountId).Balance;
            }

            //if (scheduleModel.IAccno != 0)
            //{

            //    if (scheduleModel.RequestFrom == "renew")
            //    {
            //        scheduleTModel1 = creditService.ScheduleDataRenewByIaccno(scheduleModel.IAccno);
            //        if (scheduleModel.PaymentMode == 3)
            //        {
            //            scheduleTModel1.HasInterest = true;
            //            scheduleTModel1.DateAd = scheduleTModel1.DateAd.AddDays(-1);
            //        }
            //    }
            //    else
            //        scheduleTModel1 = creditService.ScheduleDataByIaccno(scheduleModel.IAccno);
            //    if (scheduleTModel1 != null)
            //        scheduleTModelList.Add(scheduleTModel1);
            //}



            var trialSchedule = creditService.GetScheduleDetails(scheduleModel);
            scheduleTModelList.AddRange(trialSchedule);
            scheduleTModel.scheduleList = scheduleTModelList;
            //return PartialView(scheduleTModel);
            return Json(new
            {
                Status = true,
                MaxJsonLength = 5000000000,
                Message = "",
                htmlToShow = RenderPartialViewToString("PreviewScheduleTrial", scheduleTModel),
            }, JsonRequestBehavior.AllowGet);

            //decimal previousamount = 0;
            //scheduleparams.RequestFrom = "disbursement";
            //var lastdisburse = creditService.ScheduleDataByIaccno(accountId);
            //if (lastdisburse != null)
            //{

            //    scheduleparams.IAccno = accountId;
            //    previousamount = creditService.ScheduleDataByIaccno(accountId).Balance;
            //}
            //scheduleparams.Amount = regularLoan + previousamount;
            //DateTime transactionDate = commonService.GetBranchTransactionDate();
            //DateTime MatureDate = CreditUtilityService.GetMatureDate(accountId);
            //scheduleparams.OldMatureDate = MatureDate;
            //scheduleparams.InterestDate = transactionDate;
            //scheduleparams.PrincipalDate = transactionDate;
            //return PartialView(scheduleparams);
            //return redir("PreviewScheduleTrial", scheduleparams);
        }

        public ActionResult LoanRenewSchedule(int iAccno = 0)
        {
            var scheduleparams = creditService.ScheduleParameters(iAccno);
            scheduleparams.RequestFrom = "renew";

            var lastdisburse = creditService.ScheduleDataRenewByIaccno(iAccno);
            scheduleparams.Amount = Math.Round(lastdisburse.Amount, 2);
            DateTime transactionDate = commonService.GetBranchTransactionDate();
            scheduleparams.InterestDate = transactionDate;
            scheduleparams.PrincipalDate = transactionDate;

            return PartialView(scheduleparams);
        }
        public ActionResult _LoanRenewSchedule(ScheduleModel scheduleModel)
        {
            ScheduleModel scheduleparams = new ScheduleModel();
            DateTime transactionDate = commonService.GetBranchTransactionDate();
            if (scheduleModel.IsRestructure && scheduleModel.IsReschedule)
            {
                scheduleparams = scheduleModel;
                DateTime matureDate = CreditUtilityService.GetMatureDate(scheduleModel.IAccno);
                scheduleparams.OldMatureDate = matureDate;


            }
            else if (scheduleModel.IsRestructure && !scheduleModel.IsReschedule)
            {
                scheduleparams = scheduleModel;
                scheduleparams.ValueDate = transactionDate;
            }
            else if (!scheduleModel.IsRestructure && scheduleModel.IsReschedule)
            {
                //scheduleparams = creditService.ScheduleParameters(scheduleModel.IAccno);
                scheduleparams = scheduleModel;
                DateTime matureDate = CreditUtilityService.GetMatureDate(scheduleModel.IAccno);
                scheduleparams.OldMatureDate = matureDate;
                scheduleparams.InterestDate = transactionDate;
                scheduleparams.PrincipalDate = transactionDate;
            }
            else
            {
                scheduleparams = creditService.ScheduleParameters(scheduleModel.IAccno);

                scheduleparams.InterestDate = transactionDate;
                scheduleparams.PrincipalDate = transactionDate;
                scheduleparams.ValueDate = transactionDate;
            }


            scheduleparams.RequestFrom = "renew";


            // var lastdisburse = creditService.ScheduleDataRenewByIaccno(scheduleModel.IAccno);

            scheduleparams.Amount = scheduleModel.Amount;
            scheduleparams.IAccno = scheduleModel.IAccno;
            return PartialView(scheduleparams);
        }
        #endregion

        public JsonResult GetFDLoanDetails(int accountId,int Pid,string Modelfrom,int? FDIaccno)
        {
            var fdLoanDetils = creditService.GetFdLoanDetails(accountId, Pid,Modelfrom, FDIaccno);
            return Json(fdLoanDetils, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CheckFDLoanDetails( int accountId)
        {
            var fdLoanDetils = creditService.GetFdLoanDetailsForLoanAccount(accountId);
            return Json(fdLoanDetils, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetWithOutFDLoanDetails(Int32 Pid, decimal quotationAmount,decimal sAmount)
        {
            var fdLoanDetils = creditService.GetWithOutFDLoanDetails(Pid, quotationAmount, sAmount);
            return Json(fdLoanDetils, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult CreateReschedule(ScheduleTrialModel scheduleTrialModel, ScheduleModel scheduleModel)
        {
            try
            {
                returnMessage = creditService.InsertReschedule(scheduleTrialModel, scheduleModel);
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult LoanReSanction(int iAccno)
        {
            ReSenctionModel loanSenction = new ReSenctionModel();
            loanSenction.SenctionAmount = TellerUtilityService.DebitLimitAmount(iAccno);
            loanSenction.IAccno = iAccno;
            loanSenction.OutstandingAmount = TellerUtilityService.GoodBalance(iAccno);
            return PartialView(loanSenction);
        }
        [HttpPost]
        public ActionResult LoanReSanction(ReSenctionModel reSenctionModel)
        {
            try
            {
                returnMessage = creditService.LoanReSanctionSave(reSenctionModel);
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetCheckSanctionStatusFromDisbursementMain(int accountNumber)
        {

            ChannakyaBase.DAL.DatabaseModel.DisbursementMain checkSanctionObject = creditService.GetSanctionStatus(accountNumber);
            return Json(checkSanctionObject, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCheckSanctionStatusFromDisbursementMaininRemaingAmount(int accountNumber)
        {

            var checkSanctionObject = creditService.GetSanctionStatusforRemaingAmount(accountNumber);

            return Json(checkSanctionObject, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetCheckSanctionStatusFromDisbursementMaininRemaingAmountonChange(int accountNumber)
        {

            var checkSanctionObject = creditService.GetSanctionStatusforRemaingAmountonChange(accountNumber);

            return Json(checkSanctionObject, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetBranchTransactionDate()
        {


            DateTime currentDate = commonService.GetBranchTransactionDate();

            return Json(currentDate, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetProductDetails(int pid)
        {
            ChannakyaBase.Model.ViewModel.ProductViewModel productdetail = creditService.GetProductDetails(pid);

            return Json(productdetail, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSchemeDetailsBySchemeId(int SchId)
        {
            var schemeDetails = creditService.GetSchemeDetailFromSchemeId(SchId);
            bool? isFDLoan = false;
            isFDLoan = schemeDetails.IsFDLoan;

            return Json(isFDLoan, JsonRequestBehavior.AllowGet);
        }

    }
}

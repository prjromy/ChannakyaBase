using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChannakyaBase.BLL.Service;
using ChannakyaBase.DAL.DatabaseModel;
using Loader;
using ChannakyaBase.Model.ViewModel;
using ChannakyaBase.Model.Models;
using ChannakyaBase.BLL;

namespace ChannakyaBase.Web.Controllers
{
    [MyAuthorize]
    public class TransactionSetupController : Controller
    {
        private TransactionService transactionService = null;
        private TellerService tellerservice = null;
        //private ReturnBaseMessageModel returnMessage = null;
        CommonService commonServic = null;
        ReturnBaseMessageModel returnMessage = null;
        public TransactionSetupController()
        {
            transactionService = new TransactionService();
            tellerservice = new TellerService();
            commonServic = new CommonService();
            returnMessage = new ReturnBaseMessageModel();
        }
        // GET: TransactionSetup
        public ActionResult FloatingInterestIndex()
        {
            return View();
        }

        public ActionResult _TempInterestRateIndex()
        {
            return PartialView();
        }

        public ActionResult _TempInterestRateList()
        {

            return PartialView(transactionService.GetAllTempIntRate().ToList());
        }

        [HttpGet]
        public ActionResult _TempInterestRateCreate(int TID = 0)
        {
            if (TID != 0)
            {
                var tempInterestRate = transactionService.GetSingleTempIntRate(TID);

                return PartialView(tempInterestRate);
            }
            TempIntRate tempIntRate = new TempIntRate();
            return PartialView(tempIntRate);
        }

        [HttpPost]
        public ActionResult _TempInterestRateCreate(TempIntRate tempIntRate)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var saveTempIntRate = transactionService.SaveTempIntRate(tempIntRate);
                    return Json(saveTempIntRate, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    returnMessage.Success = false;
                    returnMessage.Msg = "Please fill out form please!!";
                    return Json(returnMessage, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return JavaScript(ex.Message);
            }

        }

        [HttpGet]
        public ActionResult TempInterestRateDelete(int TID)
        {
            bool result = false;
            if (transactionService.CheckTempIntRate(TID))
            {
                result = true;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult TempInterestRateDeleteConfirm(int TID)
        {
            TempIntRate tempIntRate = transactionService.GetSingleTempIntRate(TID);
            transactionService.TempIntRateDelete(tempIntRate);
            return RedirectToAction("_TempInterestRateList");
        }

        public ActionResult _TempInterestRateValIndex()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult _TempInterestRateValList(int TID = 0)
        {
            return PartialView(transactionService.GetAllTempIntRateVal(TID).ToList());
        }

        [HttpGet]
        public ActionResult _TempInterestRateValCreate(int TIRID = 0)
        {
            if (TIRID != 0)
            {
                var csc = transactionService.GetSingleTempIntRateVal(TIRID);
                return PartialView(csc);
            }
            //  ViewBag.CustomerList = customerService.GetCustomerListFromMaster(); TempIntRateVal TempIntRateVal = new TempIntRateVal();
            //ViewBag.TemplateList = transactionService.GetTemplateListForFloatingInterest();
            TempIntRateVal TempIntRateVal = new TempIntRateVal();
            return PartialView(TempIntRateVal);
        }

        [HttpPost]
        public ActionResult _TempInterestRateValCreate(TempIntRateVal tempIntRateVal)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var saveTempIntRateVal = transactionService.SaveTempIntRateVal(tempIntRateVal);
                    //return RedirectToAction("_TempInterestRateValIndex");
                    return Json(saveTempIntRateVal, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    returnMessage.Success = false;
                    returnMessage.Msg = "Please fill out form please!!";
                    return Json(returnMessage, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return JavaScript(ex.Message);
            }
        }

        [HttpGet]
        public ActionResult TempInterestRateValDelete(int TID)
        {
            bool result = false;
            if (transactionService.CheckTempIntRateInFloatingPointInterest(TID))
            {
                result = true;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult TempInterestRateValDeleteConfirm(int TIRID)
        {
            TempIntRateVal tempIntRate = transactionService.GetSingleTempIntRateVal(TIRID);
            transactionService.TempIntRateValDelete(tempIntRate);
            return RedirectToAction("_TempInterestRateValList");
        }

        //rule part=> interest policy setup
        public ActionResult InterestPolicyIndex()
        {
            return View();
        }

        public ActionResult _InterestPolicyList()
        {
            return PartialView(transactionService.GetAllInterestPolicy().ToList());
        }

        [HttpGet]
        public ActionResult _InterestPolicyCreate(int PSID = 0)
        {
            if (PSID != 0)
            {
                var interestPolicy = transactionService.GetSingleInterestPolicy(PSID);
                return PartialView(interestPolicy);
            }
            PolicyIntCalc policyIntCalc = new PolicyIntCalc();
            return PartialView(policyIntCalc);
        }

        [HttpPost]
        public ActionResult _InterestPolicyCreate(PolicyIntCalc policyIntCalc)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var Message = transactionService.SaveInterestPolicy(policyIntCalc);
                    return Json(Message, JsonRequestBehavior.AllowGet);
                }
                returnMessage.Msg = "Interest Policy Mapped Edited Successfully";
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return JavaScript(ex.Message);
            }
        }

        [HttpGet]
        public ActionResult InterestPolicyDelete(int PSID)
        {
            bool result = false;
            if (transactionService.CheckInterestPolicy(PSID))
            {
                result = true;
            return Json(result, JsonRequestBehavior.AllowGet);

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult InterestPolicyDeleteConfirm(int PSID)
        {
            try
            {
                PolicyIntCalc policyIntCalc = transactionService.GetSingleInterestPolicy(PSID);
                transactionService.InterestPolicyDelete(policyIntCalc);
                return RedirectToAction("_InterestPolicyList");
            }
            catch (Exception ex)
            {

                throw;
            }
          
        }

        //rule part=> Penalty policy setup
        public ActionResult PenaltyPolicyIndex()
        {
            return View();
        }

        public ActionResult _PenaltyPolicyList()
        {
            return PartialView(transactionService.GetAllPenaltyPolicy().ToList());
        }

        [HttpGet]
        public ActionResult _PenaltyPolicyCreate(int PCID = 0)
        {
            if (PCID != 0)
            {
                var penaltyPolicy = transactionService.GetSinglePenaltyPolicy(PCID);
                return PartialView(penaltyPolicy);
            }
            PolicyPenCalc policyPenCalc = new PolicyPenCalc();
            return PartialView(policyPenCalc);
        }

        public JsonResult CheckPolicyName(string PSName, int PSID = 0)
          {

            bool ifExists = transactionService.CheckExists(PSName, PSID);
            return Json(ifExists, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckPenaltyPolicyName(string PCNAME, int PCID = 0)
        {

            bool ifExists = transactionService.CheckExistsPenaltyPolicy(PCNAME, PCID);
            return Json(ifExists, JsonRequestBehavior.AllowGet);
        }

         [HttpPost]
        public ActionResult _PenaltyPolicyCreate(PolicyPenCalc policyPenCalc)
        {
            try
            {
                if (ModelState.IsValid)
                {
                   var message= transactionService.SavePenaltyPolicy(policyPenCalc);
                    return Json(message, JsonRequestBehavior.AllowGet); 
                    
                }
                returnMessage.Msg = "Penalty Policy Not Saved";
                returnMessage.Success = false;
                return Json(returnMessage, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return JavaScript(ex.Message);
            }
        }

        [HttpGet]
        public ActionResult PenaltyPolicyDelete(int PCID)
        {
            bool result = false;
            if (transactionService.CheckPenaltyPolicy(PCID))
            {
                result = true;
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult PenaltyPolicyDeleteConfirm(int PCID)
        {
            try
            {
                PolicyPenCalc policyPenCalc = transactionService.GetSinglePenaltyPolicy(PCID);
                transactionService.InterestPenaltyDelete(policyPenCalc);
                return RedirectToAction("_PenaltyPolicyList");
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }

        #region Change Products and Product Account's interest

        [HttpGet]
        public ActionResult ChangeProductInterestIndex()
        {
            return View();
        }

        public ActionResult _ChangeProductInterestList(int? batchID, List<AccountDetailsViewModel> accountDetailsViewModel)
        {
            if (batchID != null)
            {
                var accountList = tellerservice.GetAccountListByProductId(batchID);
                return PartialView(accountList);
            }
            else if (accountDetailsViewModel != null)
            {
                return PartialView(accountDetailsViewModel);
            }
            else
            {
                List<AccountDetailsViewModel> accountDetailsViewModelEmpty = new List<AccountDetailsViewModel>();
                return PartialView(accountDetailsViewModelEmpty);
            }
        }

        [HttpGet]
        public ActionResult _ChangeProductInterest()
        {
            ChangeProductInterestViewModel changeProductInterestViewModel = new ChangeProductInterestViewModel();
            changeProductInterestViewModel.InterestChangeDate = commonServic.GetBranchTransactionDate();
            return PartialView(changeProductInterestViewModel);
        }

        [HttpPost]
        public ActionResult _ChangeProductInterest(ChangeProductInterestViewModel changeProductInterest, List<AccountDetailsViewModel> accountInterestList, bool applyToProductOnly = false)
        {
            try
            {
                returnMessage = transactionService.ChangeProductInterest(changeProductInterest, accountInterestList, applyToProductOnly);
                //return RedirectToAction("ChangeProductInterestIndex");
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return JavaScript(ex.Message);
            }
        }

        //[HttpPost]
        public ActionResult GetProductByProductType(int batchID)
        {
            var ProductList = TransactionUtilityService.GetProductDetails(batchID);
            return Json(ProductList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetStypeByProductId(int batchID)
        {
            var ProductList = transactionService.GetProductType(batchID);
            return Json(ProductList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCurrentInterestProductType(int batchID)
        {
            //var intCapList = tservice.GetInterestCapitalizeByProductId(batchID);
            var intCapList = tellerservice.GetProductDetails(batchID);
            float? interestRate = intCapList.InterestRate;
            return Json(interestRate, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInterestCapitalizeByProductType(int batchID)
        {
            var intCapList = tellerservice.GetInterestCapitalizeByProductId(batchID);
            //float? interestRate = intCapList.InterestRate;
            return Json(intCapList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckTemplateName(string tName = "", int TID = 0)
        {
            var ifExists = tellerservice.CheckTemplateName(tName, TID);
            //float? interestRate = intCapList.InterestRate;
            return Json(ifExists, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}

using ChannakyaBase.BLL;
using ChannakyaBase.BLL.Service;
using ChannakyaBase.Model.Models;
using ChannakyaBase.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChannakyaBase.Web.Controllers
{
    public class CorrectionController : Controller
    {
        // GET: Correction
        ReturnBaseMessageModel returnMessage = null;
        private CorrectionService correctionService = null;
        private CommonService commonService = null;
        //private TellerService tellerService = null;
        public CorrectionController()
        {

            returnMessage = new ReturnBaseMessageModel();
            correctionService = new CorrectionService();
            commonService = new CommonService();
            //tellerService = new TellerService();
        }

        #region Adjustment

       
        public ActionResult Adjustment()
        {
            return PartialView();
        }

        public ActionResult GetAdjustmentType(int accNo)
        {
            int sType = commonService.GetStypeByIaccno(accNo);
             SelectList adjustmentType= correctionService.adjustmentTypeList(sType);
            return Json(adjustmentType, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetOldBalance(int accNo,int index)
        {
            CorrectionViewModel correctionViewModel = new CorrectionViewModel();
            correctionViewModel = correctionService.GetOldBalance(accNo, index);
            decimal getbalance = correctionViewModel.OldBalance;
            return Json(getbalance, JsonRequestBehavior.AllowGet);

        }


        public ActionResult SaveAdjustment(CorrectionViewModel correctionViewModel,string optradio)
        {
            if (ModelState.IsValid)
            {
                returnMessage = correctionService.saveAdjustment(correctionViewModel, optradio);
                    return Json(returnMessage, JsonRequestBehavior.AllowGet);
                   
            }
         
           else
            {
                returnMessage.Success= false;
                returnMessage.Msg = "Please fill out the form please!!";
                return Json(returnMessage, JsonRequestBehavior.AllowGet);

            }
          
        }

        public ActionResult AdjustmentLoan()
        {
            return PartialView();
        }

        public ActionResult AdjustmentDeposit()
        {
            return PartialView();
        }
        #endregion


        #region TransactionEdit
      public ActionResult TransactionEdit()
        {

            return PartialView();
        } 

       
        public ActionResult GetTransactionEditByTno(decimal tNo, string getRadioValue)
        {
      
            TransactionEditViewModel transactionEditViewModel = new TransactionEditViewModel();

            var tnoDetails = correctionService.GetTransactionEditValue(tNo,getRadioValue);
            if (tnoDetails != null)
            {
                if (tnoDetails.transactioneditCount == 1)//no match of transaxtion Date
                {
                    return Json(tnoDetails.transactioneditCount, JsonRequestBehavior.AllowGet);
                }
                else if (tnoDetails.transactioneditCount == 2)
                {

                    return Json(tnoDetails.transactioneditCount, JsonRequestBehavior.AllowGet);
                }
                else if (tnoDetails.transactioneditCount == 3)
                {

                    return Json(tnoDetails.transactioneditCount, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    transactionEditViewModel = tnoDetails;
                    //return Json(transactionEditViewModel, JsonRequestBehavior.AllowGet);
                    return PartialView(transactionEditViewModel);
                }
               
            }
            else
            {
                    transactionEditViewModel = tnoDetails;
                return Json(transactionEditViewModel, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult CheckusermatchTransactionEdit(int tno)
        {
            bool result = false;
            if (correctionService.CheckUserMatch(tno)==true)
            {
                 result = true;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
               
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EditTransaction(TransactionEditViewModel transactionEditViewModel, DenoInOutViewModel denoInOutViewModel, TaskVerificationList taskVerifier)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    returnMessage = correctionService.EditDepositTransaction(transactionEditViewModel, denoInOutViewModel, taskVerifier);
                    return Json(returnMessage, JsonRequestBehavior.AllowGet);
                }
                catch (Exception exc)
                {
                    var err = ModelState.Values.SelectMany(v => v.Errors);
                    return Json(returnMessage, JsonRequestBehavior.AllowGet);
                }




            }
            else {
                
                returnMessage.Success = false;
                returnMessage.Msg = "please fill all input field.";
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
        }



        public ActionResult TransactionEditVerify()
        {
            TransactionEditViewModel transactionEditViewModel = new TransactionEditViewModel();
            var transactionEditList = correctionService.GetAllVerifyList();
            transactionEditViewModel.transactionList = transactionEditList;
           
            return PartialView(transactionEditViewModel.transactionList);
        }

        public ActionResult SingleTransactionEditVerify(int tno)
        {
            TransactionEditViewModel transactionEditViewModel = new TransactionEditViewModel();
            transactionEditViewModel = correctionService.GetSingleTransactionEditVerify(tno);
            return PartialView(transactionEditViewModel);

        }
        public ActionResult VerifiedEdit()
        {

            return PartialView();
        }

        public ActionResult DeleteTransactionEdit(int Tno)
        {
            correctionService.DeleteUnverifiedEditTransaction(Tno);
            return RedirectToAction("TransactionEditVerify");
        }
        #endregion
    }
}
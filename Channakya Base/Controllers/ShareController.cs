using ChannakyaBase.BLL;
using ChannakyaBase.BLL.Service;
using ChannakyaBase.Model.Models;
using ChannakyaBase.Model.ViewModel;
using Loader;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChannakyaBase.Web.Controllers
{
    [MyAuthorize]
    public class ShareController : Controller
    {
        // GET: Share
        ReturnBaseMessageModel returnMessage = null;

        private CommonService commonService = null;
        private ShareService shareService = null;

        public ShareController()
        {
            returnMessage = new ReturnBaseMessageModel();
            commonService = new CommonService();
            shareService = new ShareService();
        }
       
        public ActionResult ShareRegistrationDetail(int regId)
        {
            ShareNomineeModel shareNomineeModel = new ShareNomineeModel();
            var shareRegistrationDetails = shareService.ShareRegistrationDetails(regId);
            shareNomineeModel.CustomerDetails = shareRegistrationDetails;
            shareNomineeModel.ShareNomineeList = shareService.GetShareNomineeListByRegId(regId);
            return PartialView(shareNomineeModel);
        }
        public ActionResult ShareRegistration()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult ShareRegistration(ShareRegisterViewModel shareRegModel, Model.ViewModel.TaskVerificationList TaskVerifier)
        {
            try
            {
                returnMessage = shareService.ShareRegistrationSave(shareRegModel, TaskVerifier);
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UnverifiedShareRegistration()
        {
            ShareRegisterViewModel shareRegModel = new ShareRegisterViewModel();
            var sharePurchaseDetails = shareService.SharePurchaseUnverifiedList(1,10);
            shareRegModel.ShareRegistrationList = sharePurchaseDetails;
            return PartialView(shareRegModel);
        }
        public ActionResult _UnverifiedShareRegistration(int pageNo = 1, int pageSize = 10)
        {
            ShareRegisterViewModel shareRegModel = new ShareRegisterViewModel();
            var sharePurchaseDetails = shareService.SharePurchaseUnverifiedList(pageNo, pageSize);
            shareRegModel.ShareRegistrationList = sharePurchaseDetails;
            return PartialView(shareRegModel);
        }
        public ActionResult SharePurchase()
        {
            returnMessage = TellerUtilityService.CheckUserActivateOrNot();
            if (returnMessage.Success)
            {
                ShareUtilityService.GetShareRegNumber();
                ShrSPurchaseModel shareModel = new ShrSPurchaseModel();
                shareModel.Rate = shareService.ShareSetupRate();
                return PartialView(shareModel);
            }
            else
                return PartialView("~/Views/Shared/UserNoActivated.cshtml", new HandleErrorInfo(new HttpException(403, "Please activate user to access Share purchase!!" + returnMessage.Msg), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()));


            
        }
        [HttpPost]
        public ActionResult SharePurchase(ShrSPurchaseModel sharePurchaseModel, DenoInOutViewModel denoInOutModel, Model.ViewModel.TaskVerificationList TaskVerifier)
        {
            try
            {
                returnMessage = shareService.SharePurchaseSave(sharePurchaseModel, denoInOutModel, TaskVerifier);
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SharePurchaseDetails(Int64 tno)
        {
            var sharePurchaseDetails = shareService.SharePurchaseDetails(tno);
            return PartialView(sharePurchaseDetails);
        }
        public ActionResult UnverifiedPurchaseIndex()
        {
            ShrSPurchaseModel shrRegsModel = new ShrSPurchaseModel();
            var unverifiedpurchase = shareService.UnverifiedPurchase(1, 10);
            shrRegsModel.SharePurchaseList = unverifiedpurchase;
            return PartialView(shrRegsModel);
        }
        public ActionResult _UnverifiedPurchaseIndex(int pageNo = 1, int pageSize = 10)
        {
            ShrSPurchaseModel shrRegsModel = new ShrSPurchaseModel();
            var unverifiedpurchase = shareService.UnverifiedPurchase(pageNo, pageSize);
            shrRegsModel.SharePurchaseList = unverifiedpurchase;
            return PartialView(shrRegsModel);
        }
        public ActionResult ShareReturn()
        {
            returnMessage = TellerUtilityService.CheckUserActivateOrNot();
            if (returnMessage.Success)
            {
                ShareReturnViewModel shareModel = new ShareReturnViewModel();
                shareModel.Rate = shareService.ShareSetupRate();
                return PartialView(shareModel);
            }
            else
                return PartialView("~/Views/Shared/UserNoActivated.cshtml", new HandleErrorInfo(new HttpException(403, "Please activate user to access Share return!!" + returnMessage.Msg), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()));
                 
        }
        [HttpPost]
        public ActionResult ShareReturn(ShareReturnViewModel shareReturn, DenoInOutViewModel denoInOutModel, Model.ViewModel.TaskVerificationList TaskVerifier,ShareCustomerDetailsViewModel ShareCustomerDetailsList)
        {
            try
            {
                returnMessage = shareService.ShareReturnSave(shareReturn, denoInOutModel, TaskVerifier);
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult CustomerListShareReturn(int employeeId)
        {
            //employee id here is regno
            ShareCustomerDetailsViewModel shareModel = new ShareCustomerDetailsViewModel();
            shareModel.ShareCustomerDetailsList = shareService.ShareCustomerList(employeeId);
            return PartialView(shareModel);
        }
        public ActionResult ShareReturnUnVerifiedIndex()
        {
            ShareCustomerDetailsViewModel aModel = new ShareCustomerDetailsViewModel();
            var sharereturnUnverifiedList = shareService.ShareReturnUnverifiedIPageList(0, 1, 10);
            aModel.ShareCustomerDetailsIPageList = aModel.ShareCustomerDetailsIPageList = new StaticPagedList<ShareCustomerDetailsViewModel>(sharereturnUnverifiedList, 1, 10, (sharereturnUnverifiedList.Count == 0) ? 0 : sharereturnUnverifiedList.Select(x => x.TotalCount).FirstOrDefault());
            return PartialView(aModel);
        }
        public ActionResult _ShareReturnUnVerifiedIndexList(int accState = 0, int pageNo = 1, int pageSize = 10)
        {
            ShareCustomerDetailsViewModel aModel = new ShareCustomerDetailsViewModel();
            var sharereturnUnverifiedList = shareService.ShareReturnUnverifiedIPageList(accState, pageNo, pageSize);
            aModel.ShareCustomerDetailsIPageList = new StaticPagedList<ShareCustomerDetailsViewModel>(sharereturnUnverifiedList, pageNo, pageSize, (sharereturnUnverifiedList.Count == 0) ? 0 : sharereturnUnverifiedList.Select(x => x.TotalCount).FirstOrDefault());
            return PartialView(aModel);
        }

        public ActionResult _ShareReturnDetails(int tno)
        {
            ShareCustomerDetailsViewModel aModel = new ShareCustomerDetailsViewModel();
            //     aModel= shareService.ShareReturnUnverifiedDetailDisplay(tno);
            aModel = shareService.ShareReturnUnverifiedDetails(tno);
            return PartialView(aModel);
        }


    }
}
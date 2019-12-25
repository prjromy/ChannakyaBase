using ChannakyaBase.BLL.Service;
using ChannakyaBase.Model.Models;
using ChannakyaBase.Model.ViewModel;
using Loader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChannakyaBase.Web.Controllers
{
    [MyAuthorize]
    public class HomeTransactionsController : Controller
    {
        private HomeTransactionService homeTransactionService = null;
        ReturnBaseMessageModel returnMessage = null;
        public HomeTransactionsController()
        {
            homeTransactionService = new HomeTransactionService();
            returnMessage = new ReturnBaseMessageModel();
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult SendToLower()
        {
            var UserId = Loader.Models.Global.UserId;
            var isVault = homeTransactionService.IsUserAVault(UserId);
            if (!isVault)
            {
                returnMessage = TellerUtilityService.CheckUserActivateOrNot();
                if (returnMessage.Success)
                {


                    return PartialView("HomeTransactionsSend", homeTransactionService.HomeTransactionGet(1));
                }
                else
                {
                    return PartialView("~/Views/Shared/UserNoActivated.cshtml", new HandleErrorInfo(new HttpException(403, "Please activate user to access Deposit transaction!!" + returnMessage.Msg), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()));

                }
            }
            else
            {
                return PartialView("HomeTransactionsSend", homeTransactionService.HomeTransactionGet(1));
            }

        }
        public ActionResult SendToHigher()
        {
            returnMessage = TellerUtilityService.CheckUserActivateOrNot();
            if (returnMessage.Success)
            {
                return PartialView("HomeTransactionsSend", homeTransactionService.HomeTransactionGet(2));
            }
            else
            {
                return PartialView("~/Views/Shared/UserNoActivated.cshtml", new HandleErrorInfo(new HttpException(403, "Please activate user to access Deposit transaction!!" + returnMessage.Msg), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()));
            }
        }
        public ActionResult SendToSameLevel()
        {
            returnMessage = TellerUtilityService.CheckUserActivateOrNot();
            if (returnMessage.Success)
            {
                return PartialView("HomeTransactionsSend", homeTransactionService.HomeTransactionGet(3));
            }
            else
            {
                return PartialView("~/Views/Shared/UserNoActivated.cshtml", new HandleErrorInfo(new HttpException(403, "Please activate user to access Deposit transaction!!" + returnMessage.Msg), this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString()));
            }
        }
        [HttpPost]
        public ActionResult HomeTransactionsSend(HomeTransactionsViewModel homeTransactionsViewModel, DenoInOutViewModel denoInOutModel)
        {
            try
            {
               
                returnMessage = homeTransactionService.HomeTransactionSave(homeTransactionsViewModel, denoInOutModel);
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult RecieverDetails(int userId)
        {
            var amount = homeTransactionService.RecieverDetails(userId);
            return Json(amount, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckUserIfVaultByUserId(int userId)
        {
            bool isVault = homeTransactionService.IsUserAVault(userId);
            return Json(isVault, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RecieveHome()
        {
            HomeTransactionsViewModel cashFlow = new HomeTransactionsViewModel();
            cashFlow.HomeTransactionsList = homeTransactionService.GetRecievingAmountsDetails();
            
            return PartialView(cashFlow);
        }
        
        public ActionResult CashAcceptRejectConfirm(int tno,int operation)
        {
            try
            {
                returnMessage = homeTransactionService.CashAcceptReject(tno,operation);
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
        }

        #region ViewDetailCashTransaction
        public ActionResult ViewDetailCashTransaction(int TNO)
        {
            HomeTransactionsViewModel cashFlow = new HomeTransactionsViewModel();
            cashFlow = homeTransactionService.GetViewDetailCashTransaction(TNO);
            return PartialView(cashFlow);
        }

        
        #endregion

    }
}
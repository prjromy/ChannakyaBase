using ChannakyaBase.BLL.Service;
using ChannakyaBase.DAL.DatabaseModel;
using ChannakyaBase.Model.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChannakyaBase.BLL.CustomHelper;
using Loader.Service;
using ChannakyaBase.Model.Models;
using Loader;
using ChannakyaBase.BLL;

namespace ChannakyaBase.Web.Controllers
{

    [MyAuthorize]
    public class TaskVerificationController : Controller
    {
       private BaseTaskVerificationService taskVerification = null;
      
        ReturnBaseMessageModel returnMessage = null;



        TellerService tellerService = null;
        //TellerService ts = new TellerService();
        private CommonService commonService = null;


        public TaskVerificationController()
        {
            tellerService = new TellerService();
            returnMessage = new ReturnBaseMessageModel();
            taskVerification = new BaseTaskVerificationService();
            commonService = new CommonService();
        }
        public ActionResult VerifierList(int eventid = 0,bool ismultiVerify=false)
        {
            Model.ViewModel.TaskVerificationList tModel = new Model.ViewModel.TaskVerificationList();
            var task = taskVerification.VerifierList(eventid);
            tModel.VerifierList = task;
            tModel.EventId = eventid;
            if(ismultiVerify==true)
            {
                tModel.IsMultiVerifier = ismultiVerify;
            }
            return PartialView(tModel);
        }
        public ActionResult UserCashLimtVerification(int eventid = 0, bool ismultiVerify = false)
        {
            Model.ViewModel.TaskVerificationList tModel = new Model.ViewModel.TaskVerificationList();
            var task = taskVerification.UserCashLimtVerification();
            tModel.VerifierList = task;
            tModel.EventId = eventid;
            if (ismultiVerify == true)
            {
                tModel.IsMultiVerifier = ismultiVerify;
            }
            return PartialView("VerifierList", tModel);
        }
        public ActionResult _VerifierDetails(int eventid=0)
        {
            
            var task=taskVerification.VerifierList(eventid);
            return PartialView("VerifierList",task);
        }
        public ActionResult SaveTaskDetails(int eventid = 0)
        {

            var task = taskVerification.VerifierList(eventid);
            return PartialView("VerifierList", task);
        }

        public ActionResult _MyNotifications()
        {
            TaskViewModel tasklistMod = new TaskViewModel();
            var tasklist = taskVerification.TaskList();
            tasklistMod.TaskDetailList = tasklist;
            return PartialView(tasklistMod);
        }
        public ActionResult _VerificationModal(int task1id,string taskMain="")
        {
           TaskViewModel tsk= taskVerification.GetSingleTask(task1id);
            if (taskMain != "")
            {
                tsk.TaskMessage = taskMain;
            }
            else
            {
                tsk.TaskMessage = "";
            }
            return PartialView(tsk);
        }

        public ActionResult _DenoModalAfterVerification(int currencyId, string denoInOut)
        {
            DenoInOutViewModel denoInOutModel = new DenoInOutViewModel();
            if (currencyId == 0)
            {
                currencyId = commonService.DefultCurrency();
            }

            denoInOutModel = tellerService.DenoList(currencyId);
            denoInOutModel.DenoInOut = denoInOut;
            denoInOutModel.IsTransactionWithDeno = commonService.IsTransactionWithDeno();
                   // @{ Html.RenderAction("DenoTransaction", "Teller", new { currencyId = new CommonService().DefultCurrency(), denoInOut = EDeno.DenoOut.GetDescription() }); }
            return PartialView("_DenoModalAfterVerification", denoInOutModel);
        }

        //public ActionResult DenoTransaction(int currencyId, string denoInOut)
        //{
        //    DenoInOutViewModel denoInOutModel = new DenoInOutViewModel();
        //    if (currencyId == 0)
        //    {
        //        currencyId = commonService.DefultCurrency();
        //    }

        //    denoInOutModel = tellerService.DenoList(currencyId);
        //    denoInOutModel.DenoInOut = denoInOut;
        //    denoInOutModel.IsTransactionWithDeno = commonService.IsTransactionWithDeno();
        //    return PartialView("_DenoTransaction", denoInOutModel);
        //}

        public ActionResult _EventRaisedDetails(int task1id)
        {
            TaskViewModel tsk = taskVerification.GetSingleTask(task1id);
            return PartialView(tsk);
        }
        [HttpPost]
        public ActionResult VerificationConfirm(long eventValue=0, int eventId=0, int task1Id=0,string remarks="",bool isReject=false,DateTime? dateTrans=null,int? GrantedDuration = null, decimal? SAmt=null)
        {                    
            try
            {
                if (isReject == true)
                {
                    returnMessage = taskVerification.TaskRejectionConfirm(eventValue, eventId, task1Id, remarks);
                }

                else
                {
                    returnMessage = taskVerification.VerifyTaskConfirmation(eventValue, eventId, task1Id, remarks, dateTrans, GrantedDuration, SAmt);              
                }

                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                returnMessage.Success = false;
                returnMessage.Msg = "error";
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }


        }
        public ActionResult _ViewAllPendingTaskIndex()
        {
            TaskViewModel tasklistMod = new TaskViewModel();
            var tasklist = taskVerification.ViewAllTasks("",0,1, 10);
            tasklistMod.TaskDetailWithIPageList = new StaticPagedList<TaskViewModel>(tasklist, 1, 10, (tasklist.Count == 0) ? 0 : tasklist.FirstOrDefault().TotalCount); ;  
            return View(tasklistMod);
        }
        public ActionResult _ViewAllPendingTaskList(string employeeName, int? eventId, int pageNo = 1, int pageSize=10)
        {
            TaskViewModel tasklistMod = new TaskViewModel();
            var tasklist = taskVerification.ViewAllTasks(employeeName, eventId, pageNo, pageSize);
          var TaskDetailWithIPageList = new StaticPagedList<TaskViewModel>(tasklist, pageNo, pageSize, (tasklist.Count == 0) ? 0 : tasklist.FirstOrDefault().TotalCount);
            return PartialView(TaskDetailWithIPageList);   
        }

        [HttpPost]
        public ActionResult DeleteTaskConfirm(long eventValue = 0, int eventId = 0, int task1Id = 0, string remarks = "", bool isReject = false)
        {
            try
            {             
                returnMessage = taskVerification.TaskDeleteConfirm(eventValue, eventId, task1Id, remarks);
                return Json(returnMessage, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                returnMessage.Success = false;
                returnMessage.Msg = "error";
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }


        }
    }
}

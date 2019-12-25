using ChannakyaBase.BLL.Repository;
using ChannakyaBase.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChannakyaBase.DAL.DatabaseModel;
using ChannakyaBase.Model.Models;
using System.Web.Mvc;
using PagedList;
using Loader.Models;

namespace ChannakyaBase.BLL.Service
{
    public class BaseTaskVerificationService
    {
        GenericUnitOfWork uow = null;
        ReturnBaseMessageModel returnMessage = null;
        FinanceParameterService financeParameterService = null;
        public BaseTaskVerificationService()
        {
            returnMessage = new ReturnBaseMessageModel();
            uow = new GenericUnitOfWork();
            financeParameterService = new FinanceParameterService();
        }
        public List<TaskVerificationList> VerifierList(int EventId = 1)
        {
            int branchId = Global.BranchId;
            var verificationList = uow.Repository<TaskVerificationList>().SqlQuery(@"select * from lg.fgetverifierlist(" + EventId + ") where BranchId="+branchId).ToList();
            return verificationList;
        }
        public List<TaskVerificationList> UserCashLimtVerification()
        {
            CommonService commonService = new CommonService();
            int branchId = commonService.GetBranchIdByEmployeeUserId(); 
            var cashierList = commonService.GetAllCashier();
            var verificationList = cashierList.Select(x => new TaskVerificationList()
            {
                EmployeeId = x.EmployeeId,
                UserId = x.UserId,
                EmployeeName = x.EmployeeName,
                UserName = x.DGName
            }).ToList();
            return verificationList;
        }
        public List<TaskVerificationList> UserCashLimtTellerVerification()
        {
            CommonService commonService = new CommonService();
            int branchId = commonService.GetBranchIdByEmployeeUserId();
            var teller = commonService.GetAllTeller().Where(x => x.BranchId == branchId).ToList();
            var verificationList = teller.Select(x => new TaskVerificationList()
            {
                EmployeeId = x.EmployeeId,
                UserId = x.UserId,
                EmployeeName = x.EmployeeName,
                UserName = x.UserName
            }).ToList();
            return verificationList;
        }
        public int CountOfNofication()
        {
            int UserId = Loader.Models.Global.UserId;
            var taskList = uow.Repository<TaskViewModel>().SqlQuery(@"select count(*) as CountOfTask from mast.FGetTaskDetails(" + UserId + ")where (SeenOn is null and IsVerified=0) or (rejectedOn is not null)").FirstOrDefault();
            //var taskList = uow.Repository<TaskViewModel>().SqlQuery(@"select count(*) as CountOfTask from mast.FGetTaskDetails(" + UserId + ") where SeenOn is null and IsVerified=0").FirstOrDefault();
            return taskList.CountOfTask;
        }
        public List<TaskViewModel> TaskList()
        {
            int UserId = Loader.Models.Global.UserId;
            var taskList = uow.Repository<TaskViewModel>().SqlQuery(@"select * from mast.FGetTaskDetails(" + UserId + ") where (SeenOn is null and IsVerified=0) or (rejectedOn is not null and VerifiedOn is null) order by RaisedOn Desc").ToList();
            //var taskList = uow.Repository<TaskViewModel>().SqlQuery(@"select * from mast.FGetTaskDetails(" + UserId + ")where SeenOn is null and IsVerified=0 order by RaisedOn Desc").ToList();
            return taskList;
        }
        public List<TaskViewModel> ViewAllTasks(string employeeName, int? eventId, int pageNumber, int pageSize)
        {
            //List<string> date = dateRange.Split('|').ToList<string>();
            int UserId = Loader.Models.Global.UserId;
            string query = "select COUNT(*) OVER () AS TotalCount, * from mast.FGetTaskDetails(" + UserId + ")  where VerifiedOn is null";
            if (employeeName != "")
            {
                query += " and EmployeeName like'" + employeeName + "%'";
            }
            if (eventId != 0 && eventId != null)
            {
                query += " and EventId=" + eventId;
            }
            query += @" ORDER BY RaisedOn Desc
                       OFFSET ((" + pageNumber + @" - 1) * " + pageSize + @") ROWS
                       FETCH NEXT " + pageSize + " ROWS ONLY";
            var taskList = uow.Repository<TaskViewModel>().SqlQuery(query).ToList();
            return taskList;
        }
        public TaskViewModel GetSingleTask(int task1Id)
        {
            int UserId = Loader.Models.Global.UserId;
            var taskList = uow.Repository<TaskViewModel>().SqlQuery(@"select * from mast.FGetTaskDetails(" + UserId + ") where Task1id=" + task1Id).FirstOrDefault();
            NotificationSeenOn(task1Id);
            return taskList;
        }
        public string VerifiedBy(int task1Id)
        {
            //int UserId = Loader.Models.Global.UserId;
            var taskList = uow.Repository<TaskViewModel>().SqlQuery(@"select * from mast.[FGetTaskVerifiedBy](" + task1Id + ")").FirstOrDefault();
            if (taskList == null)
                return "";
            else
                return taskList.Verifier;
        }
        public string RejectedBy(int task1Id)
        {
            //int UserId = Loader.Models.Global.UserId;
            var taskList = uow.Repository<TaskViewModel>().SqlQuery(@"select * from mast.[FGetTaskRejectedBy](" + task1Id + ")").FirstOrDefault();
            if (taskList == null)
                return "";
            else
                return taskList.Verifier;
        }
        public string DeletedBy(int task1Id)
        {
            //int UserId = Loader.Models.Global.UserId;
            var taskList = uow.Repository<TaskViewModel>().SqlQuery(@"select * from mast.[FGetTaskDeletedBy](" + task1Id + ")").FirstOrDefault();
            if (taskList == null)
                return "";
            else
                return taskList.Verifier;
        }

        public void SaveTaskNotification(TaskVerificationList TaskVerifierList, Int64 eventValue, int eventId)
        {
            try
            {
                CommonService commonService = new CommonService();
                DAL.DatabaseModel.Task task = new DAL.DatabaseModel.Task();
                task.EventValue = eventValue;
                task.EventId = eventId;
                task.RaisedBy = Loader.Models.Global.UserId;
                task.RaisedOn = DateTime.Now;
                task.Message = TaskVerifierList.Message;
                task.IsVerified = false;
                bool isStrictlyVerifiable = commonService.isStrictlyVerifiable();
                bool isNew;
                //int taskId = 0;
                if (isStrictlyVerifiable == false)
                {
                    if (TaskVerifierList.IsExceedAmount == true)
                    {
                        TaskVerifierList.VerifierList = UserCashLimtVerification();
                    }
                    else
                    {
                        TaskVerifierList.VerifierList = VerifierList(eventId).ToList();
                    }
                }
                if (task.Task1Id == 0)
                {
                    uow.Repository<DAL.DatabaseModel.Task>().Add(task);
                    foreach (var item in TaskVerifierList.VerifierList)
                    {
                        if (item.IsSelected == true || isStrictlyVerifiable == false)
                        {
                            TaskDetail taskDetails = new TaskDetail();
                            taskDetails.TaskTo = item.UserId;
                            task.TaskDetails.Add(taskDetails);
                        }
                    }
                    isNew = true;
                }
                else
                {
                    uow.Repository<DAL.DatabaseModel.Task>().Edit(task);
                    isNew = false;
                }
                uow.Commit();
                if (isNew == true)
                {
                    var recentTask = uow.Repository<DAL.DatabaseModel.Task>().GetAll().OrderByDescending(x => x.Task1Id).FirstOrDefault();
                    if (recentTask != null)
                    {
                        Loader.Service.ParameterService Ob = new Loader.Service.ParameterService();

                        Ob.SendNotification(recentTask.Task1Id);
                    }
                }
            }
            catch (Exception ex)
            {
                new Exception(ex.Message);
            }
        }
        public void NotificationSeenOn(int task1id)
        {
            int userid = Loader.Models.Global.UserId;
            TaskDetail tskm = new TaskDetail();
            var tsk = uow.Repository<TaskDetail>().FindBy(x => x.Task1Id == task1id && x.TaskTo == userid).FirstOrDefault();
            tskm = tsk;
            if (tskm != null)
            {
                tskm.SeenOn = new CommonService().GetDate();
                uow.Repository<TaskDetail>().Edit(tskm);
                uow.Commit();
            }
        }
        public TaskDetail VerificationSeenOrNot(int task1id)
        {
            int userid = Loader.Models.Global.UserId;
            var tsk = uow.Repository<TaskDetail>().FindBy(x => x.Task1Id == task1id && x.TaskTo == userid).FirstOrDefault();
            return tsk;
        }
        public List<Event> GetEventByEventId(int EventId = 0)
        {
            var EvenList = (from e in uow.Repository<Event>().FindBy(x => x.EventId == EventId)
                            join
                          t in uow.Repository<DAL.DatabaseModel.Task>().FindBy(x => x.EventId == EventId) on e.EventId equals t.EventId into ps
                            select new Event()
                            {
                                EventId = e.EventId,
                                EventName = e.EventName
                            }).ToList();
            return EvenList;
        }
        public ReturnBaseMessageModel VerifyTaskConfirmation(long eventValue, int eventId, int task1Id, string remarks, DateTime? cdate, int? GrantedDuration = null, decimal? SAmt = null)
        {
            TellerService tellerService = new TellerService();
            CreditService creditService = new CreditService();
            ShareService shareServices = new ShareService();
            InformationService informationService = new InformationService();
            CorrectionService correctionService = new CorrectionService();
            DateTime transdate = Convert.ToDateTime(cdate);
            int msg;
            if (RejectedBy(task1Id) != "")
            {
                returnMessage.Msg = "Already Rejected By:" + RejectedBy(task1Id);
                returnMessage.Success = false;
                return returnMessage;
            }
            switch (eventId)
            {
                case 1:
                    if (VerifiedBy(task1Id) == "")
                    {
                        returnMessage = tellerService.VerifyAccount(eventValue);
                        if (returnMessage.Success == true)
                        {
                            VerifiedOn(task1Id);
                        }
                    }
                    else
                    {
                        returnMessage.Msg = "Already Verified By:" + VerifiedBy(task1Id);
                        returnMessage.Success = false;
                    }
                    break;
                case 2:
                    returnMessage = tellerService.VerifyDepositWithdrawTransaction(eventValue);
                    if (returnMessage.Success == true)
                    {
                        returnMessage.Msg = "Deposit Successfully verified with Transaction No #" + eventValue.ToString();
                        returnMessage.Success = true;
                        VerifiedOn(task1Id);
                    }
                    else
                    {
                        returnMessage.Msg = "Error in Transaction";
                        returnMessage.Success = false;
                    }
                    break;
                case 3:
                    returnMessage = tellerService.VerifyDepositWithdrawTransaction(eventValue);
                    if (returnMessage.Success == true)
                    {
                        VerifiedOn(task1Id);
                        returnMessage.Msg = "Withdraw Successfully verified with Transaction No" + eventValue.ToString();
                        returnMessage.Success = true;
                    }
                    else
                    {
                        returnMessage.Msg = "Error in Transaction";
                        returnMessage.Success = false;
                    }
                    break;
                case 7:
                    msg = financeParameterService.VerifyChargeTransaction(eventValue);
                    if (msg > 0)
                    {
                        returnMessage.Msg = "Charge Successfully verified with Transaction No #" + eventValue.ToString();
                        returnMessage.Success = true;
                        VerifiedOn(task1Id);
                    }
                    break;
                case 8:
                    msg = financeParameterService.VerifyChargeTransaction(eventValue);
                    if (msg != 0)
                    {
                        returnMessage.Msg = "Charge Successfully verified with Transaction No #" + eventValue.ToString();
                        returnMessage.Success = true;
                        VerifiedOn(task1Id);
                    }
                    break;
                case 9:
                    if (VerifiedBy(task1Id) == "")
                    {
                        returnMessage = informationService.ChequeVerification(eventValue);
                        if (returnMessage.Success == true)
                        {
                            VerifiedOn(task1Id);
                        }
                    }
                    else
                    {
                        returnMessage.Msg = "Already Verified By:" + VerifiedBy(task1Id);
                        returnMessage.Success = false;
                    }
                    break;
                case 11:
                    if (VerifiedBy(task1Id) == "")
                    {
                        returnMessage = tellerService.ApprovedUserCashLimitExceed(eventValue);
                        if (returnMessage.Success == true)
                        {
                            VerifiedOn(task1Id);
                        }

                    }
                    else
                    {
                        returnMessage.Msg = "Already Verified By:" + VerifiedBy(task1Id);
                        returnMessage.Success = false;
                    }
                    break;
                case 6:
                    if (VerifiedBy(task1Id) == "")
                    {
                        returnMessage = tellerService.ApproveInterestPayable(eventValue);
                        if (returnMessage.Success == true)
                        {
                            VerifiedOn(task1Id);
                        }

                    }
                    else
                    {
                        returnMessage.Msg = "Already Verified By:" + VerifiedBy(task1Id);
                        returnMessage.Success = false;
                    }
                    break;
                case 13:
                    if (VerifiedBy(task1Id) == "")
                    {
                        returnMessage = informationService.ApproveChequeGoodForPayment(eventValue, remarks);
                        if (returnMessage.Success == true)
                        {
                            VerifiedOn(task1Id);
                        }
                    }
                    else
                    {
                        returnMessage.Msg = "Already Verified By:" + VerifiedBy(task1Id);
                        returnMessage.Success = false;
                    }
                    break;
                case 14:
                    if (VerifiedBy(task1Id) == "")
                    {
                        returnMessage = informationService.ApproveInternalChequeDepositDetails(eventValue);
                        if (returnMessage.Success == true)
                        {
                            VerifiedOn(task1Id);
                        }
                    }
                    else
                    {
                        returnMessage.Msg = "Already Verified By:" + VerifiedBy(task1Id);
                        returnMessage.Success = false;
                    }
                    break;
                case 16:
                    if (VerifiedBy(task1Id) == "")
                    {
                        returnMessage = tellerService.VerifyRemitPayment(eventValue);
                        if (returnMessage.Success == true)
                        {
                            VerifiedOn(task1Id);
                        }
                    }
                    else
                    {
                        returnMessage.Msg = "Already Verified By:" + VerifiedBy(task1Id);
                        returnMessage.Success = false;
                    }
                    break;
                case 15:
                    if (VerifiedBy(task1Id) == "")
                    {
                        returnMessage = tellerService.VerifyRemitDeposit(eventValue);
                        if (returnMessage.Success == true)
                        {
                            VerifiedOn(task1Id);
                        }
                    }
                    else
                    {
                        returnMessage.Msg = "Already Verified By:" + VerifiedBy(task1Id);
                        returnMessage.Success = false;
                    }
                    break;

                case 17:
                    if (VerifiedBy(task1Id) == "")
                    {
                        returnMessage = creditService.VerifyLoanRegistration(eventValue, GrantedDuration, SAmt);
                        if (returnMessage.Success == true)
                        {
                            VerifiedOn(task1Id);
                        }
                    }
                    else
                    {
                        returnMessage.Msg = "Already Verified By:" + VerifiedBy(task1Id);
                        returnMessage.Success = false;
                    }
                    break;
                case 19:
                    if (VerifiedBy(task1Id) == "")
                    {
                        returnMessage = tellerService.VerifyAccount(eventValue);
                        if (returnMessage.Success == true)
                        {
                            VerifiedOn(task1Id);
                        }
                    }
                    else
                    {
                        returnMessage.Msg = "Already Verified By:" + VerifiedBy(task1Id);
                        returnMessage.Success = false;
                    }
                    break;
                case 22:
                    if (VerifiedBy(task1Id) == "")
                    {
                        returnMessage = creditService.ApproveLoanPaymentVerification(eventValue);
                        if (returnMessage.Success == true)
                        {
                            VerifiedOn(task1Id);
                        }
                    }
                    else
                    {
                        returnMessage.Msg = "Already Verified By:" + VerifiedBy(task1Id);
                        returnMessage.Success = false;
                    }
                    break;
                case 21:
                    if (VerifiedBy(task1Id) == "")
                    {
                        returnMessage = creditService.LoanDisburseVerifyConfirm(Convert.ToInt32(eventValue));
                        if (returnMessage.Success == true)
                        {
                            VerifiedOn(task1Id);
                        }
                    }
                    else
                    {
                        returnMessage.Msg = "Already Verified By:" + VerifiedBy(task1Id);
                        returnMessage.Success = false;
                    }
                    break;
                case 24:
                    if (VerifiedBy(task1Id) == "")
                    {
                        returnMessage = shareServices.VerifyShareRegistration(eventValue);
                        if (returnMessage.Success == true)
                        {
                            VerifiedOn(task1Id);
                        }
                    }
                    else
                    {
                        returnMessage.Msg = "Already Verified By:" + VerifiedBy(task1Id);
                        returnMessage.Success = false;
                    }
                    break;
                case 25:
                    if (VerifiedBy(task1Id) == "")
                    {
                        returnMessage = shareServices.VerifySharePurchase(eventValue);
                        if (returnMessage.Success == true)
                        {
                            VerifiedOn(task1Id);
                        }
                    }
                    else
                    {
                        returnMessage.Msg = "Already Verified By:" + VerifiedBy(task1Id);
                        returnMessage.Success = false;
                    }
                    break;
                case 26:
                    if (VerifiedBy(task1Id) == "")
                    {
                        returnMessage = shareServices.VerifyReturnShare(Convert.ToInt32(eventValue));
                        if (returnMessage.Success == true)
                        {
                            VerifiedOn(task1Id);
                        }
                    }
                    else
                    {
                        returnMessage.Msg = "Already Verified By:" + VerifiedBy(task1Id);
                        returnMessage.Success = false;
                    }
                    break;
                case 27:
                    if (VerifiedBy(task1Id) == "")
                    {
                        returnMessage = tellerService.VerifyChequeClearence(Convert.ToInt32(eventValue), transdate, remarks);
                        if (returnMessage.Success == true)
                        {
                            VerifiedOn(task1Id);
                        }
                    }
                    else
                    {
                        returnMessage.Msg = "Already Verified by " + VerifiedBy(task1Id);
                        returnMessage.Success = false;
                    }
                    break;
                case 29:
                    if (VerifiedBy(task1Id) == "")
                    {
                        returnMessage = correctionService.VerifyTransactionEdit(Convert.ToInt32(eventValue));
                        if (returnMessage.Success == true)
                        {
                            VerifiedOn(task1Id);
                        }
                        else
                        {
                            returnMessage.Msg = "Already Verified by" + VerifiedBy(task1Id);
                            returnMessage.Success = false;
                        }
                    }
                    break;
                default:
                    break;
            }
            return returnMessage;
        }

        public ReturnBaseMessageModel TaskRejectionConfirm(long eventValue, int eventId, int task1Id, string remarks)
        {
            TellerService tellerService = new TellerService();
            CreditService creditService = new CreditService();
            ShareService shareServices = new ShareService();
            InformationService informationService = new InformationService();
            if (VerifiedBy(task1Id) != "")
            {
                returnMessage.Msg = "Already Verified by " + VerifiedBy(task1Id);
                returnMessage.Success = false;
                return returnMessage;
            }
            //int msg;
            switch (eventId)
            {
                case 1:
                    if (RejectedBy(task1Id) == "")
                    {
                        RejectedOn(task1Id);
                        tellerService.RejectAccount(eventValue);
                        returnMessage.Success = true;
                        returnMessage.Msg = "Successfully Rejected.";
                    }
                    else
                    {
                        returnMessage.Msg = "Already Rejected";
                        returnMessage.Success = false;
                    }

                    break;
                case 2:
                    if (RejectedBy(task1Id) == "")
                    {
                        RejectedOn(task1Id);
                        returnMessage.Success = true;
                        returnMessage.Msg = "Successfully Rejected.";
                    }
                    else
                    {
                        returnMessage.Msg = "Already Rejected By:" + RejectedBy(task1Id);
                        returnMessage.Success = false;
                    }
                    break;
                case 3:

                    if (RejectedBy(task1Id) == "")
                    {
                        RejectedOn(task1Id);
                        returnMessage.Success = true;
                        returnMessage.Msg = "Successfully Rejected.";
                    }
                    else
                    {
                        returnMessage.Msg = "Already Rejected By:" + RejectedBy(task1Id);
                        returnMessage.Success = false;
                    }
                    break;
                case 7:
                    if (RejectedBy(task1Id) == "")
                    {
                        RejectedOn(task1Id);
                        returnMessage.Success = true;
                        returnMessage.Msg = "Successfully Rejected.";
                    }
                    else
                    {
                        returnMessage.Msg = "Already Rejected By:" + RejectedBy(task1Id);
                        returnMessage.Success = false;
                    }
                    //msg = financeParameterService.VerifyChargeTransaction(eventValue);
                    //if (msg > 0)
                    //{
                    //    returnMessage.Msg = "Charge Successfully verified with Transaction No #" + eventValue.ToString();
                    //    returnMessage.Success = true;
                    //    VerifiedOn(task1Id);
                    //}
                    break;
                case 8:
                    if (RejectedBy(task1Id) == "")
                    {
                        RejectedOn(task1Id);
                        returnMessage.Success = true;
                        returnMessage.Msg = "Successfully Rejected.";
                    }
                    else
                    {
                        returnMessage.Msg = "Already Rejected By:" + RejectedBy(task1Id);
                        returnMessage.Success = false;
                    }
                    //msg = financeParameterService.VerifyChargeTransaction(eventValue);
                    //if (msg != 0)
                    //{
                    //    returnMessage.Msg = "Charge Successfully verified with Transaction No #" + eventValue.ToString();
                    //    returnMessage.Success = true;
                    //    VerifiedOn(task1Id);
                    //}
                    break;
                case 9:
                    if (RejectedBy(task1Id) == "")
                    {
                        RejectedOn(task1Id);
                        returnMessage.Success = true;
                        returnMessage.Msg = "Successfully Rejected.";
                    }
                    else
                    {
                        returnMessage.Msg = "Already Rejected By:" + RejectedBy(task1Id);
                        returnMessage.Success = false;
                    }
                    break;
                case 11:
                    if (VerifiedBy(task1Id) == "")
                    {
                        returnMessage = tellerService.ApprovedUserCashLimitExceed(eventValue);
                        if (returnMessage.Success == true)
                        {
                            VerifiedOn(task1Id);
                        }
                    }
                    else
                    {
                        returnMessage.Msg = "Already Verified By:" + VerifiedBy(task1Id);
                        returnMessage.Success = false;
                    }
                    break;
                case 6:
                    if (VerifiedBy(task1Id) == "")
                    {
                        returnMessage = tellerService.ApproveInterestPayable(eventValue);
                        if (returnMessage.Success == true)
                        {
                            VerifiedOn(task1Id);
                        }
                    }
                    else
                    {
                        returnMessage.Msg = "Already Verified By:" + VerifiedBy(task1Id);
                        returnMessage.Success = false;
                    }
                    break;
                case 13:
                    if (RejectedBy(task1Id) == "")
                    {
                        RejectedOn(task1Id);
                        tellerService.RejectChequeGoodForPayment(eventValue);
                        returnMessage.Success = true;
                        returnMessage.Msg = "Successfully Rejected.";
                    }
                    else
                    {
                        returnMessage.Msg = "Already Rejected";
                        returnMessage.Success = false;
                    }
                    break;
                case 14:
                    if (RejectedBy(task1Id) == "")
                    {
                        RejectedOn(task1Id);
                        tellerService.RejectInternalChequeDeposit(eventValue);
                        returnMessage.Success = true;
                        returnMessage.Msg = "Successfully Rejected.";
                    }
                    else
                    {
                        returnMessage.Msg = "Already Rejected";
                        returnMessage.Success = false;
                    }
                    break;
                case 16:
                    if (VerifiedBy(task1Id) == "")
                    {
                        RejectedOn(task1Id);
                        returnMessage.Success = true;
                        returnMessage.Msg = "Successfully Rejected.";
                    }
                    else
                    {
                        returnMessage.Msg = "Already Verified By:" + VerifiedBy(task1Id);
                        returnMessage.Success = false;
                    }
                    break;
                case 15:
                    if (VerifiedBy(task1Id) == "")
                    {
                       
                        RejectedOn(task1Id);
                        returnMessage.Success = true;
                        returnMessage.Msg = "Successfully Rejected.";
                    }
                    else
                    {
                        returnMessage.Msg = "Already Verified By:" + VerifiedBy(task1Id);
                        returnMessage.Success = false;
                    }
                    break;
                case 17:
                    if (RejectedBy(task1Id) == "")
                    {

                        RejectedOn(task1Id);
                        tellerService.RejectLoanRegistration(eventValue);
                        returnMessage.Success = true;
                        returnMessage.Msg = "Successfully Rejected.";
                    }
                    else
                    {
                        returnMessage.Msg = "Already Rejected By:" + RejectedBy(task1Id);
                        returnMessage.Success = false;
                    }
                    break;
                case 19:
                    if (RejectedBy(task1Id) == "")
                    {

                        RejectedOn(task1Id);
                        tellerService.RejectAccount(eventValue);
                        returnMessage.Success = true;
                        returnMessage.Msg = "Successfully Rejected.";
                    }
                    else
                    {
                        returnMessage.Msg = "Already Rejected By:" + RejectedBy(task1Id);
                        returnMessage.Success = false;
                    }
                    break;
                case 22:
                    if (VerifiedBy(task1Id) == "")
                    {
                        returnMessage = creditService.ApproveLoanPaymentVerification(eventValue);
                        if (returnMessage.Success == true)
                        {
                            VerifiedOn(task1Id);
                        }
                    }
                    else
                    {
                        returnMessage.Msg = "Already Verified By:" + VerifiedBy(task1Id);
                        returnMessage.Success = false;
                    }
                    break;
                case 21:
                    if (RejectedBy(task1Id) == "")
                    {
                        RejectedOn(task1Id);
                        returnMessage.Success = true;
                        returnMessage.Msg = "Successfully Rejected.";
                    }
                    else
                    {
                        returnMessage.Msg = "Already Rejected By:" + RejectedBy(task1Id);
                        returnMessage.Success = false;
                    }
                    break;
                case 24:
                    if (RejectedBy(task1Id) == "")
                    {
                        RejectedOn(task1Id);
                        returnMessage.Success = true;
                        returnMessage.Msg = "Successfully Rejected.";
                    }
                    else
                    {
                        returnMessage.Msg = "Already Rejected By:" + RejectedBy(task1Id);
                        returnMessage.Success = false;
                    }
                    break;
                case 25:
                    if (RejectedBy(task1Id) == "")
                    {
                        RejectedOn(task1Id);
                        returnMessage.Success = true;
                        returnMessage.Msg = "Successfully Rejected.";
                    }
                    else
                    {
                        returnMessage.Msg = "Already Rejected By:" + RejectedBy(task1Id);
                        returnMessage.Success = false;
                    }
                    break;
                case 26:
                    if (RejectedBy(task1Id) == "")
                    {
                        RejectedOn(task1Id);
                        returnMessage.Success = true;
                        returnMessage.Msg = "Successfully Rejected.";
                    }
                    else
                    {
                        returnMessage.Msg = "Already Rejected By:" + RejectedBy(task1Id);
                        returnMessage.Success = false;
                    }
                    break;
                case 27:
                    if (RejectedBy(task1Id) == "")
                    {
                        TellerService editchqstate = new TellerService();
                        RejectedOn(task1Id);
                        returnMessage.Success = true;
                        editchqstate.AcceptRejectChequeVerification(Convert.ToInt32(eventValue), remarks);
                        returnMessage.Msg = "Successfully Rejected.";
                    }
                    break;
                case 29:
                    if (RejectedBy(task1Id) == "")
                    {
                        CorrectionService correctionService = new CorrectionService();
                        RejectedOn(task1Id);
                        correctionService.AcceptRejectEditTransaction(Convert.ToInt32(eventValue));
                        returnMessage.Msg = "Succesfully Rejected";
                        returnMessage.Success = true;
                    }
                    else
                    {
                        returnMessage.Msg = "Already Rejected By:" + RejectedBy(task1Id);
                        returnMessage.Success = false;
                    }
                    break;
                default:
                    break;
            }
            return returnMessage;
        }
        public ReturnBaseMessageModel TaskDeleteConfirm(long eventValue, int eventId, int task1Id, string remarks)
        {
            TellerService tellerService = new TellerService();
            CreditService creditService = new CreditService();
            ShareService shareServices = new ShareService();
            InformationService informationService = new InformationService();
            CorrectionService correctionService = new CorrectionService();
            int msg;
            switch (eventId)
            {
                case 1:
                    if (DeletedBy(task1Id) != "")
                    {
                        
                        returnMessage = tellerService.DeleteUnverifiedAccount(eventValue, eventId);
                        DeleteAfterReject(task1Id);
                    }
                    else
                    {
                        returnMessage.Msg = "Already Deleted";
                        returnMessage.Success = false;
                    }
                    break;
                case 2:
                    if (DeletedBy(task1Id) != "")
                    {
                        DeleteAfterReject(task1Id);
                        returnMessage = tellerService.DeleteUnverifiedTransactions(Convert.ToInt32(eventValue), eventId);
                    }
                    else
                    {
                        returnMessage.Msg = "Already Deleted";
                        returnMessage.Success = false;
                    }
                    break;
                case 3:
                    if (DeletedBy(task1Id) != "")
                    {
                        DeleteAfterReject(task1Id);
                        returnMessage = tellerService.DeleteUnverifiedTransactions(Convert.ToInt32(eventValue), eventId);
                    }
                    else
                    {
                        returnMessage.Msg = "Already Deleted";
                        returnMessage.Success = false;
                    }
                    break;
                case 8:
                    if (DeletedBy(task1Id) != "")
                    {
                        DeleteAfterReject(task1Id);
                        returnMessage = tellerService.DeleteUnverifiedTransactions(Convert.ToInt32(eventValue), eventId);
                    }
                    else
                    {
                        returnMessage.Msg = "Already Deleted";
                        returnMessage.Success = false;
                    }
                    break;
                case 7:
                    msg = financeParameterService.VerifyChargeTransaction(eventValue);
                    if (msg != 0)
                    {
                        returnMessage.Msg = "Charge Successfully verified with Transaction No #" + eventValue.ToString();
                        returnMessage.Success = true;
                        VerifiedOn(task1Id);
                    }
                    break;
                case 9:
                    if (DeletedBy(task1Id) != "")
                    {
                        DeleteAfterReject(task1Id);
                        returnMessage = informationService.DeleteChqRqst(Convert.ToInt16(eventValue));
                    }
                    else
                    {
                        returnMessage.Msg = "Already Deleted";
                        returnMessage.Success = false;
                    }
                    break;
                case 11:
                    if (DeletedBy(task1Id) != "")
                    {
                        DeleteAfterReject(task1Id);
                        returnMessage = tellerService.DeleteUnVerifiedTellerExceedPayment(Convert.ToInt16(eventValue));
                    }
                    else
                    {
                        returnMessage.Msg = "Already Deleted";
                        returnMessage.Success = false;
                    }
                    break;
                case 6:
                    if (VerifiedBy(task1Id) == "")
                    {
                        returnMessage = tellerService.ApproveInterestPayable(eventValue);
                        if (returnMessage.Success == true)
                        {
                            VerifiedOn(task1Id);
                        }

                    }
                    else
                    {
                        returnMessage.Msg = "Already Verified By:" + VerifiedBy(task1Id);
                        returnMessage.Success = false;
                    }
                    break;
                case 13:
                    if (DeletedBy(task1Id) != "")
                    {

                        returnMessage = tellerService.DeleteChequeGoodForPayment(eventValue);
                        DeleteAfterReject(task1Id);
                    }
                    else
                    {
                        returnMessage.Msg = "Already Deleted";
                        returnMessage.Success = false;
                    }
                    break;
                case 14:
                    if (VerifiedBy(task1Id) == "")
                    {
                        returnMessage = informationService.ApproveInternalChequeDepositDetails(eventValue);
                        if (returnMessage.Success == true)
                        {
                            VerifiedOn(task1Id);
                        }
                    }
                    else
                    {
                        returnMessage.Msg = "Already Verified By:" + VerifiedBy(task1Id);
                        returnMessage.Success = false;
                    }
                    break;
                case 16:
                    if (DeletedBy(task1Id) != "")
                    {
                        returnMessage = tellerService.DeleteRemitPayment(eventValue, eventId);
                        DeleteAfterReject(task1Id);

                    }
                    else
                    {
                        returnMessage.Msg = "Already Deleted";
                        returnMessage.Success = false;
                    }
                    break;
                case 17:
                    if (DeletedBy(task1Id) != "")
                    {
                        returnMessage = tellerService.DeleteUnverifiedLoanRegistration(eventValue, eventId);
                      
                        DeleteAfterReject(task1Id);
                    }
                    else
                    {
                        returnMessage.Msg = "Already Deleted";
                        returnMessage.Success = false;
                    }
                    break;

                case 15:
                    if (DeletedBy(task1Id) != "")
                    {
                      
                        returnMessage = tellerService.DeleteRemitDeposit(eventValue, eventId);
                        DeleteAfterReject(task1Id);
                    }
                    else
                    {
                        returnMessage.Msg = "Already Deleted";
                        returnMessage.Success = false;
                    }
                    break;
                case 19:
                    if (DeletedBy(task1Id) != "")
                    {
                        DeleteAfterReject(task1Id);
                        returnMessage = tellerService.DeleteUnverifiedAccount(eventValue, eventId);
                    }
                    else
                    {
                        returnMessage.Msg = "Already Deleted";
                        returnMessage.Success = false;
                    }
                    break;
                case 22:
                    if (VerifiedBy(task1Id) == "")
                    {
                        returnMessage = creditService.ApproveLoanPaymentVerification(eventValue);
                        if (returnMessage.Success == true)
                        {
                            VerifiedOn(task1Id);
                        }

                    }
                    else
                    {
                        returnMessage.Msg = "Already Verified By:" + VerifiedBy(task1Id);
                        returnMessage.Success = false;
                    }
                    break;
                case 21:
                    if (DeletedBy(task1Id) != "")
                    {
                        DeleteAfterReject(task1Id);
                        returnMessage = creditService.DeleteUnverifiedDisburse(Convert.ToInt32(eventValue));
                    }
                    else
                    {
                        returnMessage.Msg = "Already Deleted";
                        returnMessage.Success = false;
                    }

                    break;
                case 24:
                    if (DeletedBy(task1Id) != "")
                    {
                        DeleteAfterReject(task1Id);
                        returnMessage = shareServices.DeleteUnVerifiedShareRegistration(Convert.ToInt32(eventValue));
                    }
                    else
                    {
                        returnMessage.Msg = "Already Deleted";
                        returnMessage.Success = false;
                    }

                    break;
                case 25:
                    if (DeletedBy(task1Id) != "")
                    {
                        DeleteAfterReject(task1Id);
                        returnMessage = tellerService.DeleteUnverifiedTransactions(Convert.ToInt32(eventValue), eventId);
                    }
                    else
                    {
                        returnMessage.Msg = "Already Deleted";
                        returnMessage.Success = false;
                    }
                    break;
                case 26:

                    if (DeletedBy(task1Id) != "")
                    {
                        DeleteAfterReject(task1Id);
                        returnMessage = tellerService.DeleteUnverifiedTransactions(Convert.ToInt32(eventValue), eventId);
                    }
                    else
                    {
                        returnMessage.Msg = "Already Deleted";
                        returnMessage.Success = false;
                    }
                    break;
                case 27:
                    if (DeletedBy(task1Id) != "")
                    {
                        DeleteAfterReject(task1Id);
                        returnMessage = tellerService.DeleteUnverifiedTransactions(Convert.ToInt32(eventValue), eventId);
                    }
                    else
                    {
                        returnMessage.Msg = "Already Deleted";
                        returnMessage.Success = false;
                    }
                    break;
                case 29:
                    if (DeletedBy(task1Id) != "")
                    {
                        DeleteAfterReject(task1Id);
                        returnMessage = correctionService.DeleteUnverifiedEditTransaction(Convert.ToInt32(eventValue));
                    }
                    else
                    {
                        returnMessage.Msg = "Already Deleted";
                        returnMessage.Success = false;
                    }
                    break;

                default:
                    break;

            }
            return returnMessage;


        }
        public void VerifiedOn(int task1id)
        {
            int userid = Loader.Models.Global.UserId;
            TaskDetail tskm = new TaskDetail();
            var tsk = uow.Repository<TaskDetail>().FindBy(x => x.Task1Id == task1id && x.TaskTo == userid).FirstOrDefault();

            if (tsk != null)
            {
                tskm = tsk;
                tskm.VerifiedOn = new CommonService().GetDate();

                uow.Repository<TaskDetail>().Edit(tskm);
                uow.Commit();
            }
        }
        public void RejectedOn(int task1id)
        {
            int userid = Loader.Models.Global.UserId;
            TaskDetail tskm = new TaskDetail();
            ChannakyaBase.DAL.DatabaseModel.Task task1 = new ChannakyaBase.DAL.DatabaseModel.Task();
            var tsk2 = uow.Repository<ChannakyaBase.DAL.DatabaseModel.Task>().FindBy(x => x.Task1Id == task1id).FirstOrDefault();
            var tsk = uow.Repository<TaskDetail>().FindBy(x => x.Task1Id == task1id && x.TaskTo == userid).FirstOrDefault();
            tskm = tsk;
            task1 = tsk2;
            task1.RaisedOn = new CommonService().GetDate();
            tskm.RejectedOn = new CommonService().GetDate();
            uow.Repository<TaskDetail>().Edit(tskm);
            uow.Repository<ChannakyaBase.DAL.DatabaseModel.Task>().Edit(task1);
            uow.Commit();
        }
        public void DeleteAfterReject(int task1id)
        {
            try
            {
                int userid = Loader.Models.Global.UserId;
                List<TaskDetail> tskm = new List<TaskDetail>();
                tskm = uow.Repository<TaskDetail>().FindBy(x => x.Task1Id == task1id).ToList();
                foreach (var item in tskm)
                {
                    uow.Repository<TaskDetail>().Delete(item);
                }
                ChannakyaBase.DAL.DatabaseModel.Task tsk = new ChannakyaBase.DAL.DatabaseModel.Task();
                tsk = uow.Repository<ChannakyaBase.DAL.DatabaseModel.Task>().FindBy(x => x.Task1Id == task1id).FirstOrDefault();
                uow.Repository<ChannakyaBase.DAL.DatabaseModel.Task>().Delete(tsk);
                uow.Commit();
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}

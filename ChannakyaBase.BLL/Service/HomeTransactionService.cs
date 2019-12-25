using ChannakyaBase.BLL.Repository;
using ChannakyaBase.DAL.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChannakyaBase.Model.Models;
using ChannakyaBase.Model.ViewModel;
using PagedList;
using System.Data.SqlClient;
using System.Web.Mvc;
using Loader.Models;

namespace ChannakyaBase.BLL.Service
{
    public class HomeTransactionService
    {
        ReturnBaseMessageModel returnMessage = null;
    //    private ChannakyaBaseEntities _context = null;
        private GenericUnitOfWork uow = null;
        private Loader.Repository.GenericUnitOfWork luow = null;
        private CommonService commonService = null;
        public HomeTransactionService()
        {
            commonService = new CommonService();
            luow = new Loader.Repository.GenericUnitOfWork();
            uow = new GenericUnitOfWork();
            returnMessage = new ReturnBaseMessageModel();
           // _context = new ChannakyaBaseEntities();
        }
        public HomeTransactionsViewModel HomeTransactionGet(int level)
        {
            int userId = Loader.Models.Global.UserId;
            var userDesignation = uow.Repository<HomeTransactionsViewModel>().SqlQuery(" select * from [fin].[FGetAllUsersWithDesignation]() where userid=" + userId).FirstOrDefault();
            userDesignation.RecieverList = new SelectList(commonService.GetAllRecieverList(userDesignation.PId, level), "UserId", "EmployeeName");
            userDesignation.CurrencyList = new SelectList(commonService.GetMyBalanceWithCurrency(), "CTId", "CurrencyName");
            userDesignation.AmountWithCurrency = commonService.GetMyBalanceWithCurrency();
            userDesignation.CurrentBalance = commonService.GetUserUserBalance(Loader.Models.Global.UserId);
            userDesignation.operationType = level;
            userDesignation.ActionType = userDesignation.DGName + " > " + commonService.GetAllRecieverList(userDesignation.PId, level).Select(x => x.DGName).FirstOrDefault();
            var isVault = IsUserAVault(userId);
            var checkUserActivate= TellerUtilityService.CheckUserActivateOrNot();
            if (isVault ||checkUserActivate.Success == true)
            {
                userDesignation.UserState = true;
            }
            else
            {
                userDesignation.UserState = false;
            }
            
            return userDesignation;
        }
        public ReturnBaseMessageModel HomeTransactionSave(HomeTransactionsViewModel homeTransactions, DenoInOutViewModel denoInOutModel)
        {
            try
            {
                DateTime transactionDate = commonService.GetBranchTransactionDate();
                bool IsTrnsWithDeno = commonService.IsTransactionWithDeno();
                if (IsTrnsWithDeno)
                {
                    if (!TellerUtilityService.BalanceWithDenoAmount(denoInOutModel, homeTransactions.Amount))
                    {
                        returnMessage.Msg = "Amount is not match with deno balance.!!";
                        returnMessage.Success = false;
                        return returnMessage;
                    }
                    returnMessage = TellerUtilityService.AvailableDenoNumber(denoInOutModel.DenoOutList);
                    if (!returnMessage.Success)
                    {
                        return returnMessage;
                    }

                }
                var userDesignaionParam = uow.Repository<HomeTransactionsViewModel>().SqlQuery("select * from [fin].[FGetAllUsersWithDesignation]() where userid=" + homeTransactions.UserId).ToList();
                var userDesignaion = uow.Repository<HomeTransactionsViewModel>().SqlQuery("select * from [fin].[FGetAllUsersWithDesignation]() where userid=" + Global.UserId).ToList();

                if (userDesignaion.Where(x => x.DegOrder == 3).Count() == 0)
                {
                    returnMessage = TellerUtilityService.CheckUserActivateOrNot();
                    if (returnMessage.Success == false)
                    {
                        return returnMessage;
                    }
                }
                var jk = userDesignaionParam.Select(x => x.DesignationId).SingleOrDefault();
                var jl = userDesignaion.Select(x => x.DesignationId).SingleOrDefault();
                if (userDesignaionParam.Select(x=>x.DesignationId).SingleOrDefault()== userDesignaion.Select(x => x.DesignationId).SingleOrDefault())
                {
                    
                    var activeUser = uow.Repository<CashFlow>().FindBy(x => x.UserID == homeTransactions.UserId && x.TDate == transactionDate && x.Status == 22).FirstOrDefault();
                    if (activeUser == null)
                    {
                        returnMessage.Msg = "Reciever Teller is not Activated";
                        returnMessage.Success = false;
                        return returnMessage;
                    }
                  
                    
                       
                   
                }

                if (userDesignaionParam.Select(x => x.DesignationId).SingleOrDefault() == userDesignaion.Select(x => x.DesignationId).SingleOrDefault())
                {
                    
                    var activeUser = uow.Repository<CashFlow>().FindBy(x => x.UserID == homeTransactions.UserId && x.TDate == transactionDate && x.Status == 22).FirstOrDefault();
                    if (activeUser == null)
                    {
                        returnMessage.Msg = "Reciever Cashier is not Activated";
                        returnMessage.Success = false;
                        return returnMessage;
                    }


                   

                }

                //if(userDesignaion.Select(x => x.DesignationId).SingleOrDefault()== 4 )///for higher send....if user is cashier then vault cannot be activated
                //{
                //    var isVault = IsUserAVault(homeTransactions.UserId);
                //    if (isVault)
                //    {
                //        var activeUser = uow.Repository<CashFlow>().FindBy(x => x.UserID == homeTransactions.UserId && x.TDate == transactionDate && x.Status == 22).FirstOrDefault();
                //        if (activeUser == null)
                //        {
                //            returnMessage.Msg = "User Not Activated.User Cannot be activated from lower level";
                //            returnMessage.Success = false;
                //            return returnMessage;
                //        }
                //    }
                
                    
                //}

                if(userDesignaion.Select(x => x.DesignationId).SingleOrDefault() == 5)///for higher send....if user is teller then cashier cannot be activated
                {
                    var isCashier=IsUserCashier(homeTransactions.UserId);
                    if (isCashier)
                    {
                        var activeUser = uow.Repository<CashFlow>().FindBy(x => x.UserID == homeTransactions.UserId && x.TDate == transactionDate && x.Status == 22).FirstOrDefault();
                        if (activeUser == null)
                        {
                            returnMessage.Msg = "User Not Activated.User Cannot be activated from lower level";
                            returnMessage.Success = false;
                            return returnMessage;
                        }
                    }
                }
                CashFlow cashFlow = new CashFlow();
                cashFlow.Dramt = homeTransactions.Amount;
                cashFlow.FUserid = Loader.Models.Global.UserId;
                cashFlow.UserID = homeTransactions.UserId;
                cashFlow.TNO = commonService.GetUtno();
                cashFlow.Brhid = commonService.GetBranchIdByEmployeeUserId();
                cashFlow.TDate = commonService.GetBranchTransactionDate();

                cashFlow.TType = Convert.ToByte(GetTType(homeTransactions.PId, userDesignaionParam.FirstOrDefault().PId));
                cashFlow.Currid = homeTransactions.Currid;
                cashFlow.Status = 21;
                uow.Repository<CashFlow>().Add(cashFlow);
                commonService.SaveUpdateMyBalance(Loader.Models.Global.UserId, homeTransactions.Currid, homeTransactions.Amount);

                if (IsTrnsWithDeno)
                {
                    commonService.DenoInOutTransaction(denoInOutModel, cashFlow.TNO);
                }

                uow.Commit();

                returnMessage.Msg = "Successfully Send";
                returnMessage.Success = true;
                returnMessage.ReturnId = homeTransactions.operationType;
                return returnMessage;
            }
            catch (Exception ex)
            {
                returnMessage.Msg = "Some Error" + ex.Message;
                returnMessage.Success = false;
                return returnMessage;
            }


        }
        
        public HomeTransactionsViewModel RecieverDetails(int UserId)
        {
            HomeTransactionsViewModel homeTransactionsViewModel = new HomeTransactionsViewModel();
            homeTransactionsViewModel.Dramt = commonService.GetUserDebitLimit(UserId);
            return homeTransactionsViewModel;
        }
        public bool IsUserAVault(int UserId)
        {
            bool isVault = false;
            //var userRole= uow.Repository<UserRole>().SqlQuery(" select * from lg.UserRole where UserId ="+UserId+"").FirstOrDefault();
            var userDesignationId = luow.Repository<Loader.Models.ApplicationUser>().FindBy(x => x.Id == UserId).Select(x => x.UserDesignationId).FirstOrDefault();
            var stringDesignationId = Convert.ToString(userDesignationId);
            var paramvalue = luow.Repository<Loader.Models.ParamValue>().FindBy(x => x.PValue == stringDesignationId).Select(x=>x.PId).FirstOrDefault();
            //var Mtid = uow.Repository<User>().FindBy(x => x.UserId == UserId).Select(x => x.MTId).SingleOrDefault();
            if (paramvalue != 0)
            {
                if (paramvalue == 2007)
                {
                    isVault = true;
                }
                return isVault;
            }

            else
            {
                return isVault;
            }
        }
        public bool IsUserCashier(int UserId)
        {
            bool isCashier = false;
            var userDesignationId = luow.Repository<Loader.Models.ApplicationUser>().FindBy(x => x.Id == UserId).Select(x => x.UserDesignationId).FirstOrDefault();
            var stringDesignationId = Convert.ToString(userDesignationId);
            var paramvalue = luow.Repository<Loader.Models.ParamValue>().FindBy(x => x.PValue == stringDesignationId).Select(x => x.PId).FirstOrDefault();
            if (paramvalue == 2005)
            {
                isCashier = true;
            }
            return isCashier;

        }
        public int GetTType(int FDegId, int TDegId)
        {
            int fromUserId = Loader.Models.Global.UserId;
            var cashFlowDictionary = uow.Repository<CashFlowTypeDictionary>().SqlQuery(" select * from [fin].[FGetCashFlowDictionary]() where FDeg=" + FDegId + "and TDeg=" + TDegId).FirstOrDefault();
            return cashFlowDictionary.Id;
        }
        public List<HomeTransactionsViewModel> GetRecievingAmountsDetails()
        {
            int UserId = Loader.Models.Global.UserId;
            var tempList = uow.Repository<HomeTransactionsViewModel>().SqlQuery(@"select* from fin.GetRejectedOrAccepted(" + UserId + ")").ToList();
            return tempList;
        }
        public ReturnBaseMessageModel CashAcceptReject(long tno, int operation)
        {
            try
            {
                CashFlow cashFlow = new CashFlow();
                cashFlow = uow.Repository<CashFlow>().FindBy(x => x.TNO == tno).FirstOrDefault();
                cashFlow.AcceptedOn = commonService.GetDate();
                int FUserId = Convert.ToInt32(cashFlow.FUserid);
                int TUserId = Convert.ToInt32(cashFlow.UserID);
                decimal Amount = cashFlow.Dramt;
                short currId = cashFlow.Currid;

            

                //if (Deleted(tno) != "")
                //{
                //    returnMessage.Msg = "Already Deleted  ";
                //    returnMessage.Success = false;
                //    return returnMessage;
                //}
                if (operation == 1)
                {
                    if (Accepted(tno) != "")
                    {
                        returnMessage.Msg = "Already Verified  ";
                        returnMessage.Success = false;
                        return returnMessage;
                    }


                    if (Rejected(tno) != "")
                    {
                        returnMessage.Msg = "Already Rejected  ";
                        returnMessage.Success = false;
                        return returnMessage;
                    }
                    if (Accepted(tno) == "")
                    {
                        cashFlow.Status = 22;
                        uow.Repository<CashFlow>().Edit(cashFlow);
                        commonService.DenoAccept(tno, Loader.Models.Global.UserId, 1);
                        commonService.SaveUpdateMyBalance(0, currId, Amount, TUserId, 22);
                        returnMessage.Msg = "Successfully Accepted";
                        returnMessage.Success = true;
                    }
                    else
                    {
                        returnMessage.Msg = "Already Accepted";
                        returnMessage.Success = false;
                    }
                }

                if (operation == 2)
                {
                    if (Deleted(tno) != "")
                    {
                        returnMessage.Msg = "Already Deleted  ";
                        returnMessage.Success = false;
                        return returnMessage;
                    }
                    if (Deleted(tno) == "")
                    {
                        cashFlow.Status = 23;

                        commonService.DenoAccept(tno, FUserId, 1);
                        uow.Repository<CashFlow>().Edit(cashFlow);
                        commonService.SaveUpdateMyBalance(FUserId, currId, Amount, TUserId, 24);
                        returnMessage.Msg = "Successfully Deleted";
                        returnMessage.Success = true;
                    }
                    else
                    {
                        returnMessage.Msg = "Already Deleted";
                        returnMessage.Success = false;
                    }
                }
                if (operation == 3)
                {
                    if (Accepted(tno) != "")
                    {
                        returnMessage.Msg = "Already Verified  ";
                        returnMessage.Success = false;
                        return returnMessage;
                    }


                    if (Rejected(tno) != "")
                    {
                        returnMessage.Msg = "Already Rejected  ";
                        returnMessage.Success = false;
                        return returnMessage;
                    }
                    if (Rejected(tno) == "")
                    {
                        cashFlow.Status = 24;
                        uow.Repository<CashFlow>().Edit(cashFlow);
                        returnMessage.Msg = "Successfully Rejected";
                        returnMessage.Success = true;
                    }
                    else
                    {
                        returnMessage.Msg = "Already Rejected";
                        returnMessage.Success = false;
                    }
                }
                uow.Commit();
                return returnMessage;
            }
            catch (Exception ex)
            {
                returnMessage.Msg = "Some Error" + ex.Message;
                returnMessage.Success = false;
                return returnMessage;
            }


        }

        #region Rejected HomeTransaction
        public string Rejected(long tno)
        {
            //int UserId = Loader.Models.Global.UserId;
            var taskList = uow.Repository<CashFlowViewModel>().SqlQuery(@"select * from Rejected(" + tno + ")").FirstOrDefault();
            if (taskList == null)
                return "";
            else
                return "Already Rejected";
        }

        #endregion

        #region Accepted Hometransaction
        public string Accepted(long tno)
        {
            //int UserId = Loader.Models.Global.UserId;
            var taskList = uow.Repository<CashFlowViewModel>().SqlQuery(@"select * from Accepted(" + tno + ")").FirstOrDefault();
            if (taskList == null)
                return "";
            else
                return "Already Accepted";
        }
        #endregion

        #region Deleted HomeTransaction
        public string Deleted(long tno)
        {
            //int UserId = Loader.Models.Global.UserId;
            var taskList = uow.Repository<CashFlowViewModel>().SqlQuery(@"select * from Deleted(" + tno + ")").FirstOrDefault();
            if (taskList == null)
                return "";
            else
                return "Already Deleted";
        }
        #endregion


        #region ViewDetailCashTransaction


        public HomeTransactionsViewModel GetViewDetailCashTransaction(int TNO)
        {
            //var isverified = uow.Repository<CashFlow>().FindBy(x => x.TNO == TNO && x.Status==21).SingleOrDefault();
            var tempList = new HomeTransactionsViewModel();
            //if (isverified !=null)
            // {
            //tempList = uow.Repository<HomeTransactionsViewModel>().SqlQuery(@"select distinct a.*,d.DesignationId,d.EmployeeName,d.EmployeeId,d.TUserId,d.DGName 
            //               from  fin.CashFlow a inner join(select b.DesignationId, b.DGName, b.EmployeeId, b.EmployeeName, c.UserID as TUserId, c.FUserid  
            //               from fin.CashFlow c inner join fin.FGetAllUsersWithDesignation() b on c.FUserid = b.UserId where Status=21) d on 
            //           d.TUserId = a.UserID where Status =21 and a.TNO=" + TNO).FirstOrDefault();
            //}
            //else
            //{
            //   tempList = uow.Repository<HomeTransactionsViewModel>().SqlQuery(@"select distinct a.*,d.DesignationId,d.EmployeeName,d.EmployeeId,d.TUserId,d.DGName 
            //                   from  fin.CashFlow a inner join(select b.DesignationId, b.DGName, b.EmployeeId, b.EmployeeName, c.UserID as TUserId, c.FUserid  
            //                   from fin.CashFlow c inner join fin.FGetAllUsersWithDesignation() b on c.FUserid = b.UserId ) d on 
            //               d.TUserId = a.UserID where   a.TNO=" + TNO).FirstOrDefault();
            //}

            tempList = uow.Repository<HomeTransactionsViewModel>().SqlQuery(@"select* from fin.GetSingleHomeTransaction(" + TNO + ")").SingleOrDefault();
            return tempList;
        }
        #endregion
        //public ReturnBaseMessageModel CashAccept(long tno)
        //{
        //    try
        //    {
        //        CashFlow cashFlow = new CashFlow();
        //        cashFlow = uow.Repository<CashFlow>().FindBy(x => x.TNO == tno).FirstOrDefault();
        //        cashFlow.AcceptedOn = DateTime.Now;
        //        cashFlow.Status = 22;
        //        uow.Repository<CashFlow>().Edit(cashFlow);

        //        returnMessage.Msg = "Successfully Accepted";
        //        returnMessage.Success = true;
        //        return returnMessage;
        //    }
        //    catch (Exception ex)
        //    {
        //        returnMessage.Msg = "Some Error";
        //        returnMessage.Success = false;
        //        return returnMessage;
        //    }

        //}
        //public ReturnBaseMessageModel CashReject(long tno)
        //{
        //    try
        //    {
        //        CashFlow cashFlow = new CashFlow();tra
        //        cashFlow = uow.Repository<CashFlow>().FindBy(x => x.TNO == tno).FirstOrDefault();
        //        cashFlow.AcceptedOn = DateTime.Now;
        //        cashFlow.Status = 23;
        //        decimal amount = cashFlow.Dramt;
        //        uow.Repository<CashFlow>().Edit(cashFlow);

        //        returnMessage.Msg = "Successfully Accepted";
        //        returnMessage.Success = true;
        //        return returnMessage;
        //    }
        //    catch (Exception ex)
        //    {
        //        returnMessage.Msg = "Some Error";
        //        returnMessage.Success = false;
        //        return returnMessage;
        //    }

        //}


    }
}

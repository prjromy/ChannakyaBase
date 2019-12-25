using ChannakyaBase.BLL.Repository;
using ChannakyaBase.DAL.DatabaseModel;
using ChannakyaBase.Model.Models;
using ChannakyaBase.Model.ViewModel;
using Loader.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;

namespace ChannakyaBase.BLL.Service
{
    public class CorrectionService
    {
        ReturnBaseMessageModel returnBaseMessage = null;
        private ChannakyaBaseEntities _context = null;
        private GenericUnitOfWork uow = null;
        private CommonService CommonService = null;
        private ReturnBaseMessageModel returnBaseMessageModel = null;
        private BaseTaskVerificationService taskVerificationService = null;

        public CorrectionService()
        {
            returnBaseMessage = new ReturnBaseMessageModel();
            CommonService = new CommonService();
            uow = new GenericUnitOfWork();
            _context = new ChannakyaBaseEntities();
            returnBaseMessageModel = new ReturnBaseMessageModel();
            taskVerificationService = new BaseTaskVerificationService();

        }


        public SelectList adjustmentTypeList(int sType)
        {

            List<SelectListItem> adjustmenttypeList = new List<SelectListItem>();
            if (sType == 1)
            {
                //adjustmenttypeList.Add(new SelectListItem { Text = "Please Select", Value = "-1", });
                //adjustmenttypeList.Add(new SelectListItem { Text = "Accrued Interest", Value = "1", });
                //adjustmenttypeList.Add(new SelectListItem { Text = "Interest on Interest", Value = "6", });
                //adjustmenttypeList.Add(new SelectListItem { Text = "Interest on Charge", Value = "2", });
                //adjustmenttypeList.Add(new SelectListItem { Text = "Penalty on Principal ", Value = "3", });
                //adjustmenttypeList.Add(new SelectListItem { Text = "Penalty on Interest", Value = "9" });

                adjustmenttypeList.Add(new SelectListItem { Text = "Please Select", Value = "-1", });
                adjustmenttypeList.Add(new SelectListItem { Text = "Accrued Interest", Value = "0", });
                adjustmenttypeList.Add(new SelectListItem { Text = "Interest on Interest", Value = "1", });
                adjustmenttypeList.Add(new SelectListItem { Text = "Interest on Charge", Value = "2", });
                adjustmenttypeList.Add(new SelectListItem { Text = "Penalty on Principal ", Value = "3", });
                adjustmenttypeList.Add(new SelectListItem { Text = "Penalty on Interest", Value = "4" });

            }
            else

            {
                adjustmenttypeList.Add(new SelectListItem { Text = "Please Select", Value = "-1", });
                adjustmenttypeList.Add(new SelectListItem { Text = "Accrued Interest", Value = "0" });

            }
            return new SelectList(adjustmenttypeList, "Value", "Text");

        }

        public CorrectionViewModel GetOldBalance(int accNo, int index)
        {

            CorrectionViewModel correction = new CorrectionViewModel();
            string query;
            query = "Select [fin].[FGetValueForAdjustmentReport](" + "'" + accNo + "'" + "," + index + ")";
            correction.OldBalance = _context.Database.SqlQuery<decimal>(query).SingleOrDefault();
            //correction.correctionList = sumInterest;
            return correction;
        }

        public ReturnBaseMessageModel saveAdjustment(CorrectionViewModel correctionViewModel, string optradio)
        {
            try
            {
                var singleCorrectionData = uow.Repository<AAdjustmnt>().FindBy(x => x.Iaccno == correctionViewModel.Iaccno).FirstOrDefault();
                if (singleCorrectionData == null)
                {
                    singleCorrectionData = new AAdjustmnt();
                }

                //singleCorrectionData.Iaccno = correctionViewModel.Iaccno;
                //singleCorrectionData.Pid = Convert.ToByte(correctionViewModel.AdjustmentType);
                if (correctionViewModel.Amt < 0)
                {
                    returnBaseMessage.Success = false;
                    returnBaseMessage.Msg = "Negative Balance is not Allowed!!";
                    return returnBaseMessage;
                }

                var calAdjustment = correctionViewModel.AdjustmentAmount;
                if (optradio == "-")
                {
                    calAdjustment = calAdjustment * -1;
                }
                //    else
                //    {
                //        newCalAdjustment = calAdjustment;
                //}
                uow.ExecWithStoreProcedure("[fin].[PUpdAdjustedAmt] @IACCNO,@PID,@AMT,@USERID,@TDATE,@NOTES",
                    new SqlParameter("@IACCNO", correctionViewModel.Iaccno),
                    new SqlParameter("@PID", correctionViewModel.AdjustmentType),
                    new SqlParameter("@AMT", calAdjustment),
                     new SqlParameter("@USERID", Global.UserId),
                    new SqlParameter("@TDATE", Global.TransactionDate),
                    new SqlParameter("@NOTES", correctionViewModel.note)
                    );


                returnBaseMessage.Success = true;
                returnBaseMessage.Msg = "Adjustment Saved successfully";
                return returnBaseMessage;



            }
            catch (Exception ex)
            {


                returnBaseMessage.Success = false;
                returnBaseMessage.Msg = ex.Message;
                return returnBaseMessage;
            }
        }

        public bool CheckUserMatch(long tno)
        {
            var userId = Global.UserId;
            bool status = false;
            var astrnData = uow.Repository<ASTrn>().FindBy(x => x.tno == tno).Count();
            if(astrnData >0)
            {
                var astrnValue = uow.Repository<ASTrn>().FindBy(x => x.tno == tno).FirstOrDefault();
                if (userId == astrnValue.postedby)
                {
                    status = true;
                }
                else
                {
                    status = false;
                }
            }
          
            //{
            //    var astrnValue = uow.Repository<AMTrn>().FindBy(x => x.tno == tno).FirstOrDefault();
            //    if(userId== astrnValue.postedby)
            //    {
            //        status = true;
            //    }
            //    else
            //    {
            //        status = false;
            //    }
            //}
            return status;
        }




        #region TransactionEdit

        public TransactionEditViewModel GetTransactionEditValue(decimal tNo, string getRadioValue)
        {
            //TransactionEditViewModel transactionEditViewModel = new TransactionEditViewModel();
            try
            {


                DateTime dateTime = CommonService.GetBranchTransactionDate();
                string query = "";
                bool check = true;
                if (getRadioValue == "0")
                {
                    
                        int result = uow.Repository<ASTrn>().FindBy(x => x.tno == tNo).Count();/*FindBy(x=>x.tno== tNo)*//*GetAll().Select(x=>x.tno==tNo).Count();*/
                        int resultfromAMTrn  = uow.Repository<AMTrn>().FindBy(x => x.tno == tNo).Count();

                    if ((result > 0 )&&(resultfromAMTrn==0))
                        {
                            query = "select * from  [fin].[FGetEditTransactionByUnVerified](" + tNo + "," + "'" + dateTime + "'" +   /*+ DateTime.Parse("2015-03-17") +*/ ")";
                        }
                        else if ((result == 0) && (resultfromAMTrn > 0))
                        {
                            query = "";
                        }
                    else
                    {
                        check = false;//no data in both amtrn and astrn;
                    }
                }
                else if (getRadioValue == "1")
                {
                    int result = uow.Repository<AMTrn>().FindBy(x => x.tno == tNo).Count();
                    int resultfromASTrn = uow.Repository<ASTrn>().FindBy(x => x.tno == tNo).Count();
                    if ((result > 0)&&(resultfromASTrn==0))
                    {
                        query = "select * from  [fin].[FGetEditTransactionDetailByVerified](" + tNo + "," + "'" + dateTime + "'" + ")";
                        //query = "select * from  [fin].[FGetEditTransactionDetailByVerified](" + tNo + ",'" /*+ "'" + dateTime + "'" +*/ + DateTime.Parse("2015-03-17") + "')";

                    }
                    else if((result==0)&&(resultfromASTrn > 0))
                    {
                        query = "";
                    }
                    else{
                        check = false;//no data in both amtrn and astrn;
                    }

                }
                //else
                //{
                //query = "no data in amtrans and astrans";

                //if there is no data in amtrns and astrns
                // }
                //if ((query != "") && (query!=null))
                if ((query !=null)&& (check!=false) &&(query !=""))
                {
                    TransactionEditViewModel transactioneditList = new TransactionEditViewModel();
                    transactioneditList = uow.Repository<TransactionEditViewModel>().SqlQuery(query).FirstOrDefault();
                    if (transactioneditList == null)
                    {
                        TransactionEditViewModel transactioneditListNull = new TransactionEditViewModel();
                        transactioneditListNull.transactioneditCount=1;///for not match transaction date
                        return transactioneditListNull;
                    }
                    if (transactioneditList.IsDeleted == true)
                    {
                        TransactionEditViewModel transactioneditListNull = new TransactionEditViewModel();
                        transactioneditListNull.transactioneditCount = 3;///for deleted data in astrns
                        return transactioneditListNull;
                    }
                    transactioneditList.Tno = tNo;
               
                    return transactioneditList;
                }
                else if(check == false)
                {
                    TransactionEditViewModel transactioneditListNull = new TransactionEditViewModel();
                    transactioneditListNull.transactioneditCount = 2;//No data in both amtrn nd astrn
                    return transactioneditListNull;
                }
                else
                {

                    return null;
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }


        public ReturnBaseMessageModel EditDepositTransaction(TransactionEditViewModel transactionEditViewModel, DenoInOutViewModel denoInOutViewModel, TaskVerificationList taskVerifier)
        {
            var user = Global.UserId;
            using (var transaction = uow.GetContext().Database.BeginTransaction())
            {
                try
                {


                    bool IsTrnsWithDeno = CommonService.IsTransactionWithDeno();
                    if (IsTrnsWithDeno)
                    {
                        if (!TellerUtilityService.BalanceWithDenoAmount(denoInOutViewModel, transactionEditViewModel.newAmount))
                        {
                            returnBaseMessage.Msg = "Amount is not match with deno balance.!!";
                            returnBaseMessage.Success = false;
                            return returnBaseMessage;
                        }

                    }

                    //check tno is already edited or not
                    var tmEditLogCount = uow.Repository<TmEditLog>().FindBy(x => x.Tno == transactionEditViewModel.Tno && x.Status == 3).Count();
                    if (tmEditLogCount > 0)
                    {
                        returnBaseMessage.Msg = transactionEditViewModel.Tno + "No. is already been edited!!";

                        returnBaseMessage.Success = false;
                        return returnBaseMessage;
                    }
                    else
                    {


                        TmEditLog singleEditTransactionLog = new TmEditLog();
                        ADetail newAdetail =new ADetail();
                        var crabal = uow.Repository<ABal>().FindBy(x => x.IAccno == transactionEditViewModel.Iaccno&&x.Flag==2).FirstOrDefault();
                        if (crabal == null)
                        {
                            crabal = new ABal();
                        }
                        var drabal = uow.Repository<ABal>().FindBy(x => x.IAccno == transactionEditViewModel.Iaccno && x.Flag==1).FirstOrDefault();
                        if (drabal == null)
                        {
                            drabal = new ABal();
                        }

                        singleEditTransactionLog.Tno = transactionEditViewModel.Tno;
                        if (transactionEditViewModel.newAmount != 0)
                        {
                            singleEditTransactionLog.Amount = transactionEditViewModel.newAmount;
                        }
                        else   
                        {
                            singleEditTransactionLog.Amount = 0;
                        }
                        if (transactionEditViewModel.newAccountNo != 0)
                        {

                            singleEditTransactionLog.Iaccno = transactionEditViewModel.newAccountNo;
                        }
                        else
                        {
                            singleEditTransactionLog.Iaccno = 0;
                        }
                      
                        singleEditTransactionLog.Tdate = transactionEditViewModel.tdate;
                        singleEditTransactionLog.BrnchID = CommonService.GetBranchIdByEmployeeUserId();
                        singleEditTransactionLog.PostedBy = user;
                        singleEditTransactionLog.PostedOn = DateTime.Now;


                        //auto update astrans if users match 
                        var getPostedbyFromAsTrans = uow.Repository<ASTrn>().FindBy (x => x.tno == transactionEditViewModel.Tno).SingleOrDefault();
                        var checkDepositWithdraw = uow.Repository<ASTrn>().FindBy(x => x.tno == transactionEditViewModel.Tno && x.dramt == 0).Count();
                        var myBalance = uow.Repository<MyBalance>().FindBy(x => x.UserID == Global.UserId).Select(x=>x.Amount).FirstOrDefault();
                        var aBal = uow.Repository<ABal>().FindBy(x => x.IAccno == transactionEditViewModel.Iaccno);
                        var aDetail = uow.Repository<ADetail>().FindBy(x => x.IAccno == transactionEditViewModel.Iaccno);

                        if (getPostedbyFromAsTrans != null)
                        {
                            if (user == getPostedbyFromAsTrans.postedby)
                            {
                                if (transactionEditViewModel.newAccountNo != 0)
                                {

                                    getPostedbyFromAsTrans.IAccno = transactionEditViewModel.newAccountNo;
                                }
                                else
                                {
                                    getPostedbyFromAsTrans.IAccno = getPostedbyFromAsTrans.IAccno;
                                }

                                decimal? Amt = 0;
                                
                                if (checkDepositWithdraw > 0)
                                {
                                    if (transactionEditViewModel.newAmount != 0)
                                    {
                                        getPostedbyFromAsTrans.cramt = transactionEditViewModel.newAmount;
                                    }
                                    else
                                    {
                                        getPostedbyFromAsTrans.cramt = transactionEditViewModel.Amt;
                                    }
                                }
                                else
                                {
                                    if (transactionEditViewModel.newAmount != 0)
                                    {
                                        getPostedbyFromAsTrans.dramt = transactionEditViewModel.newAmount;
                                    }
                                    else
                                    {
                                        getPostedbyFromAsTrans.dramt = transactionEditViewModel.Amt;
                                    }

                                }
                                singleEditTransactionLog.Status = 1;



                                ///for deno delete
                                bool availableForInsert = false;
                                bool? isDeposit = false;
                                bool autoupdate = false;
                                ///for account number only
                                if (transactionEditViewModel.newAccountNo != 0)
                                {
                                    if (checkDepositWithdraw > 0)
                                    {
                                       
                                        crabal.Bal = crabal.Bal - transactionEditViewModel.Amt;//update cr amount of abal

                                        CommonService.InsertAvailableBalance(2, transactionEditViewModel.newAccountNo, transactionEditViewModel.Amt);
                                        uow.Repository<ABal>().Edit(crabal);
                                    }
                                    else
                                    {
                                        drabal.Bal = drabal.Bal - transactionEditViewModel.Amt;//update dr amount of old account no
                                        CommonService.InsertAvailableBalance(1, transactionEditViewModel.newAccountNo, transactionEditViewModel.Amt);
                                        uow.Repository<ABal>().Edit(drabal);
                                    }

                                 }


                                if (transactionEditViewModel.newAmount != 0)
                                {

                                    if (checkDepositWithdraw > 0)
                                    {
                                        /////for deposit transaction
                                        //Amt = (myBalance - transactionEditViewModel.Amt) + transactionEditViewModel.newAmount;

                                        Amt = transactionEditViewModel.newAmount - transactionEditViewModel.Amt;

                                        ABal shadowBalance = uow.Repository<ABal>().FindBy(x => x.IAccno == transactionEditViewModel.newAccountNo && x.Flag == 2).SingleOrDefault();
                                     

                                        if (transactionEditViewModel.newAccountNo != 0)
                                        {
                                            // crabal.Bal = aBal.Where(x => x.Flag == 2).Select(x => x.Bal).FirstOrDefault() - transactionEditViewModel.Amt;//update cr amount of abal

                                            crabal.Bal = crabal.Bal - transactionEditViewModel.Amt;//update cr amount of abal

                                            CommonService.InsertAvailableBalance(2, transactionEditViewModel.newAccountNo, transactionEditViewModel.newAmount);

                                        }



                                  
                                    else
                                    {
                                        // crabal.Bal = (aBal.Where(x => x.Flag == 2).Select(x => x.Bal).FirstOrDefault() - transactionEditViewModel.Amt) + transactionEditViewModel.newAmount;//update cr amount of abal

                                        crabal.Bal = (crabal.Bal - transactionEditViewModel.Amt) + transactionEditViewModel.newAmount;//update cr amount of abal

                                    }
                                    uow.Repository<ABal>().Edit(crabal);
                                    CommonService.SaveUpdateMyBalance(0, CommonService.DefultCurrency(), Convert.ToDecimal(Amt), transactionEditViewModel.PostedBy);
                                        isDeposit = true;
                                        autoupdate = true;
                                    CommonService.DenoTransactionAutoUpdateForTransactionEdit(transactionEditViewModel, denoInOutViewModel, Convert.ToInt64(transactionEditViewModel.Tno), Global.UserId, isDeposit, autoupdate );

                                }
                                else
                                {

                                    /////for withdraw transaction
                                    //Amt = (myBalance + transactionEditViewModel.Amt) - transactionEditViewModel.newAmount;
                                    Amt = -(transactionEditViewModel.newAmount - transactionEditViewModel.Amt);
                                    //ABal shadowBalance = uow.Repository<ABal>().FindBy(x => x.IAccno == transactionEditViewModel.newAccountNo && x.Flag == 1).SingleOrDefault();

                                    if (transactionEditViewModel.newAccountNo != 0)
                                    {
                                        //drabal.Bal = aBal.Where(x => x.Flag == 1).Select(x => x.Bal).FirstOrDefault() - transactionEditViewModel.Amt;//update dr amount of old account no
                                        drabal.Bal = drabal.Bal - transactionEditViewModel.Amt;//update dr amount of old account no
                                        CommonService.InsertAvailableBalance(1, transactionEditViewModel.newAccountNo, transactionEditViewModel.newAmount);




                                        //if (shadowBalance == null)
                                        //{
                                        //    shadowBalance = new ABal();
                                        //    availableForInsert = true;
                                        //    shadowBalance.Bal = shadowBalance.Bal + transactionEditViewModel.Amt;
                                        //}
                                        //else
                                        //{
                                        //    shadowBalance.Bal = shadowBalance.Bal + transactionEditViewModel.Amt;

                                        //}

                                        //if (availableForInsert)
                                        //{
                                        //    uow.Repository<ABal>().Add(shadowBalance);
                                        //}
                                        //else
                                        //{
                                        //    uow.Repository<ABal>().Edit(shadowBalance);
                                        //}
                                    }
                                    else
                                    {
                                        drabal.Bal = (drabal.Bal - transactionEditViewModel.Amt) + transactionEditViewModel.newAmount;//update dr amount of abal
                                        //uow.Repository<ABal>().Edit(drabal);
                                    }
                                        autoupdate = true;
                                        uow.Repository<ABal>().Edit(drabal);
                                        
                                    CommonService.SaveUpdateMyBalance(transactionEditViewModel.PostedBy, CommonService.DefultCurrency(), Convert.ToDecimal(Amt), 0);
                                    CommonService.DenoTransactionAutoUpdateForTransactionEdit(transactionEditViewModel, denoInOutViewModel, Convert.ToInt64(transactionEditViewModel.Tno), Global.UserId, isDeposit, autoupdate );

                                }

                                }                              
                              
                                uow.Repository<ASTrn>().Edit(getPostedbyFromAsTrans);
                               
                                uow.Repository<TmEditLog>().Add(singleEditTransactionLog);
                                uow.Commit();

                            }
                            else
                            {
                                singleEditTransactionLog.Status = 3;
                                //Add new TmEditLog for Unverified Transaction  

                                // if( transactionEditViewModel.newAmount != 0) {
                                //    foreach (var item in denoInOutViewModel.DenoInList)
                                // {
                                //   DenoTrxnLog DenoTrxnLog = new DenoTrxnLog();
                                //     if (item.DenoNumber != 0)
                                //   {

                                //  DenoTrxnLog.Trxno = Convert.ToDecimal(transactionEditViewModel.Tno);
                                //DenoTrxnLog.vdate = CommonService.GetBranchTransactionDate();
                                //DenoTrxnLog.Denoid = item.DenoID;
                                //DenoTrxnLog.Pics = item.DenoNumber;
                                //DenoTrxnLog.UserId = Convert.ToInt32(item.UserId);
                                //uow.Repository<DenoTrxnLog>().Add(DenoTrxnLog);
                                //   }

                                // }
                                //if (IsTrnsWithDeno)
                                //{
                                //    CommonService.DenoInOutTransaction(denoInOutViewModel, Convert.ToInt64(transactionEditViewModel.Tno));
                                //}
                                // }
                                EditTransactionDifferentUser(transactionEditViewModel, denoInOutViewModel);
                                uow.Repository<TmEditLog>().Add(singleEditTransactionLog);
                                uow.Commit();
                                taskVerificationService.SaveTaskNotification(taskVerifier, Convert.ToInt64(transactionEditViewModel.Tno), 29);
                               
                            }
                        }
                        else  //Add new TmEditLog for Verified Transaction
                        {
                            singleEditTransactionLog.Status = 3;

                            //if (transactionEditViewModel.newAmount != 0)
                            //{
                            //    foreach (var item in denoInOutViewModel.DenoInList)
                            //    {
                            //        DenoTrxnLog DenoTrxnLog = new DenoTrxnLog();
                            //        if (item.DenoNumber != 0)
                            //        {

                            //            DenoTrxnLog.Trxno = Convert.ToDecimal(transactionEditViewModel.Tno);
                            //            DenoTrxnLog.vdate = CommonService.GetBranchTransactionDate();
                            //            DenoTrxnLog.Denoid = item.DenoID;
                            //            DenoTrxnLog.Pics = item.DenoNumber;
                            //            DenoTrxnLog.UserId = Convert.ToInt32(item.UserId);
                            //            uow.Repository<DenoTrxnLog>().Add(DenoTrxnLog);
                            //            //uow.Commit();
                            //        }


                            // }


                            //if (IsTrnsWithDeno)
                            //{
                            //    CommonService.DenoInOutTransaction(denoInOutViewModel, Convert.ToInt64(transactionEditViewModel.Tno));
                            //}
                            // }
                            EditTransactionDifferentUser(transactionEditViewModel, denoInOutViewModel);
                            uow.Repository<TmEditLog>().Add(singleEditTransactionLog);
                            uow.Commit();
                            taskVerificationService.SaveTaskNotification(taskVerifier, Convert.ToInt64(transactionEditViewModel.Tno), 29);
                            
                        }

                    }

                    transaction.Commit();
                    returnBaseMessage.Msg = "Edited Saved Successfully";
                    returnBaseMessage.Success = true;
                    return returnBaseMessage;
                }

                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void EditTransactionDifferentUser(TransactionEditViewModel transactionEditViewModel, DenoInOutViewModel denoInOutViewModel)
        {
            using (TransactionScope transaction = new TransactionScope(
                         // a new transaction will always be created
                         TransactionScopeOption.RequiresNew,
                         // we will allow volatile data to be read during transaction
                         new TransactionOptions()
                         {
                             IsolationLevel = IsolationLevel.ReadUncommitted,

                         }
                     ))
            {
                ASTrn aSTrn = uow.Repository<ASTrn>().FindBy(x => x.tno == transactionEditViewModel.Tno).SingleOrDefault();
                AMTrn aMTrn = uow.Repository<AMTrn>().FindBy(x => x.tno == transactionEditViewModel.Tno).SingleOrDefault();
                var checkdepositwithdrawinastrns = uow.Repository<ASTrn>().FindBy(x => x.tno == transactionEditViewModel.Tno && x.dramt == 0).Count();
                int result = uow.Repository<ASTrn>().FindBy(x => x.tno == transactionEditViewModel.Tno).Count(); //unverified transaction
                ABal crabal = new ABal();
                ABal drabal = new ABal();
                ABal goodBalanceOfAccountNo = new ABal();
                if (aSTrn != null)
                {
                    crabal = uow.Repository<ABal>().FindBy(x => x.IAccno == aSTrn.IAccno && x.Flag == 2).FirstOrDefault();
                    if (crabal == null)
                    {
                        crabal = new ABal();
                    }
                   drabal = uow.Repository<ABal>().FindBy(x => x.IAccno == aSTrn.IAccno && x.Flag == 1).FirstOrDefault();
                    if (drabal == null)
                    {
                        drabal = new ABal();
                    }
                }

                if (aMTrn != null)
                {
                     goodBalanceOfAccountNo = uow.Repository<ABal>().FindBy(x => x.IAccno == aMTrn.IAccno && x.Flag == 3).FirstOrDefault();
                    if (goodBalanceOfAccountNo == null)
                    {
                        goodBalanceOfAccountNo = new ABal();
                    }
                }
               
                

               
                var myBalance = uow.Repository<MyBalance>().FindBy(x => x.UserID == transactionEditViewModel.PostedBy).Select(x => x.Amount).FirstOrDefault();
                
                bool autoupdate = false;//for diffrentuser
                if (result > 0)//unverified transaction
                {

                    if (transactionEditViewModel.newAccountNo != 0)
                    {
                        if (checkdepositwithdrawinastrns > 0)
                        {
                          

                            ABal shadowBalance = uow.Repository<ABal>().FindBy(x => x.IAccno == transactionEditViewModel.newAccountNo && x.Flag == 2).SingleOrDefault();
                            crabal.Bal = crabal.Bal - transactionEditViewModel.Amt;//update cr amount of abal

                            CommonService.InsertAvailableBalance(2, transactionEditViewModel.newAccountNo, transactionEditViewModel.Amt);
                            uow.Repository<ABal>().Edit(crabal);
                        }
                        else
                        {
                            drabal.Bal = drabal.Bal - transactionEditViewModel.Amt;//update dr amount of old account no
                            CommonService.InsertAvailableBalance(1, transactionEditViewModel.newAccountNo, transactionEditViewModel.Amt);
                            uow.Repository<ABal>().Edit(drabal);
                        }

                    }
                    var aBal = uow.Repository<ABal>().FindBy(x => x.IAccno == aSTrn.IAccno);
                    var aDetail = uow.Repository<ADetail>().FindBy(x => x.IAccno == aSTrn.IAccno).SingleOrDefault();
                    bool availableForInsert = false;
                    bool? isDeposit = false;

                    if (transactionEditViewModel.newAmount != 0)
                    {
                        decimal? Amt = 0;
                        if (checkdepositwithdrawinastrns > 0)
                        {
                            //for deposit transaction
                            Amt = transactionEditViewModel.newAmount - transactionEditViewModel.Amt;
                            ABal shadowBalance = uow.Repository<ABal>().FindBy(x => x.IAccno == transactionEditViewModel.newAccountNo && x.Flag == 2).SingleOrDefault();

                            if (transactionEditViewModel.newAccountNo != 0)
                            {
                                crabal.Bal = crabal.Bal - aSTrn.cramt;
                                CommonService.InsertAvailableBalance(2, transactionEditViewModel.newAccountNo, transactionEditViewModel.newAmount);

                            }
                            else
                            {
                                crabal.Bal = (crabal.Bal - transactionEditViewModel.Amt) + transactionEditViewModel.newAmount;//update cr amount of abal
                            }
                            uow.Repository<ABal>().Edit(crabal);
                            CommonService.SaveUpdateMyBalance(0, CommonService.DefultCurrency(), Convert.ToDecimal(Amt), transactionEditViewModel.PostedBy);
                            isDeposit = true;
                
                            CommonService.DenoTransactionAutoUpdateForTransactionEdit(transactionEditViewModel, denoInOutViewModel, Convert.ToInt64(transactionEditViewModel.Tno), transactionEditViewModel.PostedBy, isDeposit, autoupdate);
                            //aSTrn.cramt = Convert.ToDecimal(singlepmEditLog.Amount);
                        }
                        else
                        {
                            /////for withdraw transaction
                            Amt = -(transactionEditViewModel.newAmount - transactionEditViewModel.Amt);
                            if (transactionEditViewModel.newAccountNo != 0)
                            {
                                drabal.Bal = drabal.Bal - transactionEditViewModel.Amt;//update dr amount of old account no
                                CommonService.InsertAvailableBalance(1, transactionEditViewModel.newAccountNo, transactionEditViewModel.newAmount);
                            }
                            else
                            {
                                drabal.Bal = (drabal.Bal - transactionEditViewModel.Amt) + transactionEditViewModel.newAmount;//update dr amount of abal
                            }
                            uow.Repository<ABal>().Edit(drabal);
                            CommonService.SaveUpdateMyBalance(transactionEditViewModel.PostedBy, CommonService.DefultCurrency(), Convert.ToDecimal(Amt), 0);
               
                            CommonService.DenoTransactionAutoUpdateForTransactionEdit(transactionEditViewModel, denoInOutViewModel, Convert.ToInt64(transactionEditViewModel.Tno), transactionEditViewModel.PostedBy, isDeposit, autoupdate);
                        }
                    }



                    //check whether withdraw or not 

                    //decimal? Amt = 0;

                    //if (checkdepositwithdrawinastrns > 0)
                    //{
                    //    Amt = -(singlepmEditLog.Amount - aSTrn.cramt);
                    //    aSTrn.cramt = Convert.ToDecimal(singlepmEditLog.Amount);

                    //    CommonService.InsertAvailableBalance(1, singlepmEditLog.Iaccno, Convert.ToDecimal(Amt));
                    //    CommonService.SaveUpdateMyBalance(0, CommonService.DefultCurrency(), Convert.ToDecimal(Amt), Global.UserId);
                    //}
                    //else
                    //{
                    //    Amt = singlepmEditLog.Amount - aSTrn.dramt;
                    //    aSTrn.dramt = Convert.ToDecimal(singlepmEditLog.Amount);
                    //    CommonService.InsertAvailableBalance(2, singlepmEditLog.Iaccno, Convert.ToDecimal(Amt));
                    //    CommonService.SaveUpdateMyBalance(0, CommonService.DefultCurrency(), Convert.ToDecimal(Amt), Global.UserId);

                    //}

                    if (transactionEditViewModel.Iaccno != 0)
                    {
                        aSTrn.IAccno = transactionEditViewModel.Iaccno;
                    }
                    else
                    {
                        aSTrn.IAccno = aSTrn.IAccno;
                    }

                    if (checkdepositwithdrawinastrns > 0)
                    {
                        if (transactionEditViewModel.newAmount != 0)
                        {
                            aSTrn.cramt = Convert.ToDecimal(transactionEditViewModel.newAmount);
                        }
                        else
                        {
                            aSTrn.cramt = aSTrn.cramt;
                        }
                    }
                    else
                    {
                        if (transactionEditViewModel.newAmount != 0)
                        {
                            aSTrn.dramt = Convert.ToDecimal(transactionEditViewModel.newAmount);
                        }
                        else
                        {
                            aSTrn.dramt = aSTrn.dramt;
                        }

                    }
                    uow.Repository<ASTrn>().Edit(aSTrn);
                    //bool IsTrnsWithDeno = CommonService.IsTransactionWithDeno();
                    //if (IsTrnsWithDeno)
                    //{
                    //    CommonService.DenoInOutTransaction(denoInOutModel, transactionNumber);
                    //}

                    // uow.Commit();
                    transaction.Complete();

                }
                else
                {
                    var aBal = uow.Repository<ABal>().FindBy(x => x.IAccno == aMTrn.IAccno);
                    var aDetail = uow.Repository<ADetail>().FindBy(x => x.IAccno == aMTrn.IAccno).SingleOrDefault();
                    ///if account number is only changed
                    if (transactionEditViewModel.newAccountNo != 0)
                    {
                        if (checkdepositwithdrawinastrns > 0)//for deposit
                        {
                       
                            goodBalanceOfAccountNo.Bal = goodBalanceOfAccountNo.Bal - aMTrn.cramt;

                            CommonService.InsertAvailableBalance(3, transactionEditViewModel.Iaccno, Convert.ToDecimal(transactionEditViewModel.Amt));
                        }
                        else
                        {
                            

                            goodBalanceOfAccountNo.Bal = goodBalanceOfAccountNo.Bal + aMTrn.dramt;
                            CommonService.InsertAvailableBalance(3, transactionEditViewModel.Iaccno, -Convert.ToDecimal(transactionEditViewModel.Amt));
                        }
                        uow.Repository<ABal>().Edit(goodBalanceOfAccountNo);
                    }

                    //verified transaction
                    var checkdepositwithdrawinamtrns = uow.Repository<AMTrn>().FindBy(x => x.tno == transactionEditViewModel.Tno && x.dramt == 0).Count();
                    if (transactionEditViewModel.newAmount != 0)
                    {
                        bool? isDeposit = false;
                        decimal? Amt = 0;
                        if (checkdepositwithdrawinamtrns > 0)
                        {
                            //for deposit transaction
                            Amt = transactionEditViewModel.newAmount - transactionEditViewModel.Amt;
                            if (transactionEditViewModel.newAccountNo != 0)
                            {
                                var newAdetail = uow.Repository<ADetail>().FindBy(x => x.IAccno == transactionEditViewModel.Iaccno).SingleOrDefault();
                                goodBalanceOfAccountNo.Bal = goodBalanceOfAccountNo.Bal - aMTrn.cramt;

                                CommonService.InsertAvailableBalance(3, transactionEditViewModel.Iaccno, Convert.ToDecimal(transactionEditViewModel.newAmount));

                                aDetail.Bal = goodBalanceOfAccountNo.Bal;
                                var newAbal = uow.Repository<ABal>().FindBy(x => x.IAccno == transactionEditViewModel.newAccountNo && x.Flag==3).SingleOrDefault();
                                if (newAbal == null)
                                {
                                    newAbal = new ABal();
                                }
                                newAbal.Bal = newAbal.Bal + transactionEditViewModel.newAmount;
                                uow.Repository<ABal>().Edit(newAbal);
                                newAdetail.Bal = newAbal.Bal;
                                uow.Repository<ADetail>().Edit(newAdetail);
                            }
                            else
                            {
                                goodBalanceOfAccountNo.Bal = (goodBalanceOfAccountNo.Bal - aMTrn.cramt) + Convert.ToDecimal(transactionEditViewModel.newAmount);
                                aDetail.Bal = goodBalanceOfAccountNo.Bal;


                            }
                            uow.Repository<ADetail>().Edit(aDetail);
                            uow.Repository<ABal>().Edit(goodBalanceOfAccountNo);

                            CommonService.SaveUpdateMyBalance(0, CommonService.DefultCurrency(), Convert.ToDecimal(Amt), transactionEditViewModel.PostedBy);
                            isDeposit = true;
                            CommonService.DenoTransactionAutoUpdateForTransactionEdit(transactionEditViewModel, denoInOutViewModel, Convert.ToInt64(transactionEditViewModel.Tno), transactionEditViewModel.PostedBy, isDeposit, autoupdate);
                        }
                        else
                        {

                            Amt = -(transactionEditViewModel.newAmount - transactionEditViewModel.Amt);
                            if (transactionEditViewModel.newAccountNo != 0)
                            {
                                var newAdetail = uow.Repository<ADetail>().FindBy(x => x.IAccno == transactionEditViewModel.Iaccno).SingleOrDefault();

                                goodBalanceOfAccountNo.Bal = goodBalanceOfAccountNo.Bal + aMTrn.dramt;
                                CommonService.InsertAvailableBalance(3, transactionEditViewModel.Iaccno, -Convert.ToDecimal(transactionEditViewModel.newAmount));
                                var newAbal = uow.Repository<ABal>().FindBy(x => x.IAccno == transactionEditViewModel.newAccountNo && x.Flag==3).SingleOrDefault();
                                if (newAbal == null)
                                {
                                    newAbal = new ABal();
                                }
                                newAbal.Bal = newAbal.Bal - transactionEditViewModel.newAmount;
                                uow.Repository<ABal>().Edit(newAbal);
                                newAdetail.Bal = newAbal.Bal;
                                //  newAdetail.Bal = newAbal.Bal;
                                uow.Repository<ADetail>().Edit(newAdetail);

                            }
                            else
                            {
                                goodBalanceOfAccountNo.Bal = (goodBalanceOfAccountNo.Bal - aMTrn.dramt) + Convert.ToDecimal(transactionEditViewModel.newAmount);
                                aDetail.Bal = goodBalanceOfAccountNo.Bal;
                            }
                            uow.Repository<ADetail>().Edit(aDetail);
                            uow.Repository<ABal>().Edit(goodBalanceOfAccountNo);
                            CommonService.SaveUpdateMyBalance(0, CommonService.DefultCurrency(), Convert.ToDecimal(Amt), transactionEditViewModel.PostedBy);
                         
                            CommonService.DenoTransactionAutoUpdateForTransactionEdit(transactionEditViewModel, denoInOutViewModel, Convert.ToInt64(transactionEditViewModel.Tno), transactionEditViewModel.PostedBy, isDeposit, autoupdate);
                        }
                    }



                    //if (checkdepositwithdrawinamtrns > 0)
                    //{
                    //    aMTrn.cramt = Convert.ToDecimal(singlepmEditLog.Amount);
                    //    CommonService.InsertAvailableBalance(1, singlepmEditLog.Iaccno, Convert.ToDecimal(Amt));
                    //    CommonService.SaveUpdateMyBalance(0, CommonService.DefultCurrency(), Convert.ToDecimal(Amt), Global.UserId);
                    //}
                    //else
                    //{
                    //    Amt = singlepmEditLog.Amount - aMTrn.dramt;
                    //    aMTrn.dramt = Convert.ToDecimal(singlepmEditLog.Amount);
                    //    CommonService.InsertAvailableBalance(2, singlepmEditLog.Iaccno, Convert.ToDecimal(Amt));
                    //    CommonService.SaveUpdateMyBalance(0, CommonService.DefultCurrency(), Convert.ToDecimal(Amt), Global.UserId);

                    //}


                    if (transactionEditViewModel.newAccountNo != 0)
                    {
                        aMTrn.IAccno = transactionEditViewModel.newAccountNo;
                    }
                    else
                    {
                        aMTrn.IAccno = aMTrn.IAccno;
                    }

                    if (checkdepositwithdrawinamtrns > 0)
                    {
                        if (transactionEditViewModel.newAmount != 0)
                        {
                            aMTrn.cramt = Convert.ToDecimal(transactionEditViewModel.newAmount);
                        }
                        else
                        {
                            aMTrn.cramt = aMTrn.cramt;
                        }
                    }
                    else
                    {
                        if (transactionEditViewModel.newAmount != 0)
                        {
                            aMTrn.dramt = Convert.ToDecimal(transactionEditViewModel.newAmount);
                        }
                        else
                        {
                            aMTrn.dramt = aMTrn.dramt;
                        }

                    }
                    uow.Repository<AMTrn>().Edit(aMTrn);
                    //bool IsTrnsWithDeno = CommonService.IsTransactionWithDeno();
                    //if (IsTrnsWithDeno)
                    //{
                    //    CommonService.DenoInOutTransaction(denoInOutModel, transactionNumber);
                    //}

                    uow.Commit();
                    transaction.Complete();


                }
            }
        }

        public List<TransactionEditViewModel> GetAllVerifyList()
        {
            var getAllVerifyList = (from d in uow.Repository<TmEditLog>().GetAll()
                                    join b in uow.Repository<ChannakyaBase.DAL.DatabaseModel.LicenseBranch>().GetAll() on d.BrnchID equals b.BrnhID
                                    select new TransactionEditViewModel()
                                    {
                                        Tno = d.Tno,
                                        newAmount = Convert.ToDecimal(d.Amount),
                                        newAccountNo = Convert.ToInt32(d.Iaccno),
                                        tdate = d.Tdate,
                                        BranchName = b.BrnhNam,
                                        PostedBy = Convert.ToInt32(d.PostedBy),
                                        PostedOn = Convert.ToDateTime(d.PostedOn),
                                        Status = d.Status



                                    }
                                    ).ToList();


            return getAllVerifyList;

        }


        public TransactionEditViewModel GetSingleTransactionEditVerify(int tno)
        {
            string query = "select a.Amount as newAmount,a.Iaccno as newAccountNo,a.Tno,a.Tdate,l.BrnhNam as BranchName,a.PostedBy,a.PostedOn  from[ChannakyabaseMigrationTesting].[Trans].[TmEditLog]as a inner join fin.LicenseBranch as l on a.BrnchID= l.BrnhID where a.Tno= " + tno+ "and VerifiedOn is  null";
            var singleTransactionEditVerify = uow.Repository<TransactionEditViewModel>().SqlQuery(query).FirstOrDefault();



            /*(from d in uow.Repository<TmEditLog>().GetAll()*/
            //                                   join b in uow.Repository<LicenseBranch>().GetAll() on d.BrnchID equals b.BrnhID
            //                                   select new TransactionEditViewModel()
            //                                   {

            //                                       newAmount = Convert.ToDecimal(d.Amount),
            //                                       newAccountNo = Convert.ToInt32(d.Iaccno),
            //                                       tdate = d.Tdate,
            //                                       BranchName = b.BrnhNam

            //                                   }
            //                        ).Where(x=>x.Tno== TNO).FirstOrDefault();

            //
            return singleTransactionEditVerify;
        }

        public ReturnBaseMessageModel VerifyTransactionEdit(int tno)
        {
            using (TransactionScope transaction = new TransactionScope(
                       // a new transaction will always be created
                       TransactionScopeOption.RequiresNew,
                       // we will allow volatile data to be read during transaction
                       new TransactionOptions()
                       {
                           IsolationLevel = IsolationLevel.ReadUncommitted,

                       }
                   ))
            {


                try
                {
                    int UserId = Global.UserId;

                    TmEditLog singlepmEditLog = uow.Repository<TmEditLog>().FindBy(x => x.Tno == tno && x.Status==3).SingleOrDefault();
                    ASTrn aSTrn = uow.Repository<ASTrn>().FindBy(x => x.tno == tno).SingleOrDefault();
                    AMTrn aMTrn = uow.Repository<AMTrn>().FindBy(x => x.tno == tno).SingleOrDefault();
                    
                   // var DenoTrxnLog = uow.Repository<DenoTrxn>().FindBy(x => x.Trxno == tno).SingleOrDefault();

                    singlepmEditLog.Status = 1;
                    singlepmEditLog.VerifiedBy = UserId;
                    singlepmEditLog.VerifiedOn = DateTime.Now;

                    uow.Repository<TmEditLog>().Edit(singlepmEditLog);



                    uow.Commit();
                    transaction.Complete();
                    returnBaseMessage.Msg = "Successfully Verified Edit Transaction ";
                    returnBaseMessage.Success = true;
                    return returnBaseMessage;


                }
                catch (Exception ex)
                {

                    transaction.Dispose();
                    returnBaseMessage.Success = false;
                    returnBaseMessage.Msg = "Not saved.!!";
                    return returnBaseMessage;
                }
            }
        }

        public void AcceptRejectEditTransaction(int tno)
        {
            using (TransactionScope transaction = new TransactionScope(
          // a new transaction will always be created
          TransactionScopeOption.RequiresNew,
          // we will allow volatile data to be read during transaction
          new TransactionOptions()
          {
              IsolationLevel = IsolationLevel.ReadUncommitted,

          }
      ))
            {
                try
                {

                    TmEditLog rejecteditlog = uow.Repository<TmEditLog>().FindBy(x => x.Tno == tno).SingleOrDefault();
            

                    var getDenoFromDenoTrxnLog = uow.Repository<DenoTrxnLog>().FindBy(x => x.Trxno == tno).ToList();
                    //uow.Repository<DenoTrxn>().Delete(getDenoFromDenoTxn);
                    //DenoInViewModel denoInViewModelTwo = new DenoInViewModel
                    //{
                    //    DenoInList = getDenoFromDenoTxn
                    //};
                    foreach (var item in getDenoFromDenoTrxnLog)
                    {
                      
                        uow.Repository<DenoTrxnLog>().Delete(item);

                    }
                    rejecteditlog.Status = 2;
                    rejecteditlog.RejectedBy = Global.UserId;
                    rejecteditlog.RejectedOn = DateTime.Now;
                    uow.Repository<TmEditLog>().Edit(rejecteditlog);
                    uow.Commit();
                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    throw ex;
                }
            }
        }

        public ReturnBaseMessageModel DeleteUnverifiedEditTransaction(int tno)
        {
            using (TransactionScope transaction = new TransactionScope(
        // a new transaction will always be created
        TransactionScopeOption.RequiresNew,
        // we will allow volatile data to be read during transaction
        new TransactionOptions()
        {
            IsolationLevel = IsolationLevel.ReadUncommitted,

        }
    ))
            {
                try
                {
                    bool IsTrnsWithDeno = CommonService.IsTransactionWithDeno();
                    int UserId = Loader.Models.Global.UserId;
                    //int postedby = 0;
                    var singleunacceptreject = uow.Repository<TmEditLog>().FindBy(x => x.Tno == tno).SingleOrDefault();
                    var getDenoFromDenoTrxnLog = uow.Repository<DenoTrxnLog>().FindBy(x => x.Trxno == tno).ToList();
                    //uow.Repository<DenoTrxn>().Delete(getDenoFromDenoTxn);
                    //DenoInViewModel denoInViewModelTwo = new DenoInViewModel
                    //{
                    //    DenoInList = getDenoFromDenoTxn
                    //};
                    foreach (var item in getDenoFromDenoTrxnLog)
                    {
                        //DenoTrxnLog.Trxno = item.Trxno;
                        //DenoTrxnLog.vdate = item.vdate;
                        //DenoTrxnLog.Denoid = item.Denoid;
                        //DenoTrxnLog.Pics = item.Pics;
                        //DenoTrxnLog.UserId = item.UserId;

                        uow.Repository<DenoTrxnLog>().Delete(item);

                    }
                    if (singleunacceptreject.Status == 3) //if edit transaction is neither been accepted of rejected
                    {
                        uow.Repository<TmEditLog>().Delete(singleunacceptreject);
                    }

                    uow.Commit();
                    returnBaseMessage.Msg = "Successfully  Deleted";
                    returnBaseMessage.Success = true;
                    transaction.Complete();
                    return returnBaseMessage;
                }
                catch (Exception ex)
                {

                    transaction.Dispose();
                    returnBaseMessage.Msg = "Cannot Be Deleted";
                    returnBaseMessage.Success = false;

                }
                return returnBaseMessage;
            }
        }
  
        public bool IsVerified(decimal? tno)
        {

            var tmEditLog = uow.Repository<TmEditLog>().FindBy(x => x.Tno == tno).SingleOrDefault();

            if (tmEditLog!=null && tmEditLog.Status == 3)
            {
                return true;
            }

            return false;
        } 

        #endregion
    }
}

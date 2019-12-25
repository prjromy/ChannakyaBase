using ChannakyaBase.BLL.Repository;
using ChannakyaBase.DAL.DatabaseModel;
using ChannakyaBase.Model.Models;
using ChannakyaBase.Model.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loader.Models;
using Loader.Service;
using System.Transactions;
using System.Drawing.Printing;

namespace ChannakyaBase.BLL.Service
{
    public class InformationService
    {
        private GenericUnitOfWork uow = null;
        ReturnBaseMessageModel returnMessage = null;
        private CommonService commonService = null;
        BaseTaskVerificationService taskUow = null;
        private ParameterService paramService = null;
        // ChannakyaBaseEntities _context = null;
        public InformationService()
        {
            uow = new GenericUnitOfWork();
            returnMessage = new ReturnBaseMessageModel();
            commonService = new CommonService();
            taskUow = new BaseTaskVerificationService();
            paramService = new ParameterService();
            //_context = new ChannakyaBaseEntities();
        }
        #region CHEQUE INFORMATION


        public IPagedList<ChqRqstModel> GetAllRegisterChequeList(int? accountNumber, int pageNo=0, int pageSize=0)
        {
            var cheqList = (from ch in uow.Repository<ChqRqst>().GetAll()
                            join a in uow.Repository<ADetail>().GetAll() on ch.Iaccno equals a.IAccno
                            where a.IAccno.Equals(accountNumber)

                            select new ChqRqstModel()
                            {
                                AccountNo = a.Accno,
                                Pics = ch.Pics,
                                Tdate = ch.Tdate,
                                Rno = ch.Rno
                            })
                           .ToPagedList(pageNo, pageSize);
            return cheqList;
        }
        public IPagedList<ChqRqstModel> GetAllRegisterUnverifiedChequeList(int accountNumber = 0, int pageNo = 0, int pageSize = 0)
        {
            var cheqList = (from ch in uow.Repository<ChqRqst>().GetAll()
                            join a in uow.Repository<ADetail>().GetAll() on ch.Iaccno equals a.IAccno
                            

                            select new ChqRqstModel()
                            {
                                AccountNo = a.Accno,
                                Pics = ch.Pics,
                                Tdate = ch.Tdate,
                                Rno = ch.Rno
                            })
                           .ToPagedList(pageNo, pageSize);
            return cheqList;
        }


        public ReturnBaseMessageModel InsertChqRqst(ChqRqstModel chqRqstModel, TaskVerificationList TaskVerifierList)
        {
            using (var transaction = uow.GetContext().Database.BeginTransaction())
            //{
            //    IsolationLevel = IsolationLevel.ReadUncommitted
            //}
            //))
            {
                try
                {


                    var ProductId = uow.Repository<ADetail>().FindBy(x => x.IAccno == chqRqstModel.Iaccno).Select(x => x.PID).FirstOrDefault();
                   
                    if (!InformationUtilityService.IsAllowChequeNumber(ProductId))
                    {
                        returnMessage.Success = false;
                        returnMessage.Msg = "Cheque book can't issued for this account.!! ";
                        return returnMessage;
                    }
                    int branchId = commonService.GetBranchIdByEmployeeUserId();
                    string branchName = commonService.BranchIdToName(branchId);
                    var availableStock = uow.Repository<ReturnSingleValueModdel>().SqlQuery("select isnull(min(balance),0) as AmountValue from fin.fgetchqinv(" + branchId + ", " + chqRqstModel.Pics + ")").FirstOrDefault();
                    var lastIndex = uow.Repository<ChqInventory>().SqlQuery("select * from fin.chqinventory where Tochqno - Lastindx =" + availableStock.AmountValue + " AND (Brnhid = " + branchId + ") AND(ISfinish = 0)").FirstOrDefault();
                    if (availableStock.AmountValue == 0)
                    {
                        var availableNumber = uow.Repository<ChqInventory>().FindBy(x => x.Brnhid == branchId && x.ISfinish == false).FirstOrDefault();
                        if (availableNumber == null)
                        {
                            returnMessage.Success = false;
                            returnMessage.Msg = "Cheque setup is not available in this branch" + branchName + ".Please do setup in Master>check inventory setup";
                            return returnMessage;
                        }
                        else
                        {
                            int remainingStock = Convert.ToInt32(availableNumber.Tochqno - availableNumber.Lastindx);
                            returnMessage.Success = false;
                            returnMessage.Msg = "Only" + remainingStock + "Numbers of piece is available in stock.";
                            return returnMessage;
                        }
                    }
                    ChqRqst chqRqstRow = new ChqRqst();
                    chqRqstRow.Iaccno = Convert.ToInt32(chqRqstModel.Iaccno);
                    chqRqstRow.Pics =chqRqstModel.Pics;
                    chqRqstRow.Tdate = commonService.GetBranchTransactionDate();
                    chqRqstRow.PostedBy = Loader.Models.Global.UserId;
                    uow.Repository<ChqRqst>().Add(chqRqstRow);
                    uow.Commit();
                    taskUow.SaveTaskNotification(TaskVerifierList, chqRqstRow.Rno, 9);
                    //uow.Commit();
                    transaction.Commit();

                    returnMessage.Success = true;
                    returnMessage.Msg = "cheque issued successfully.!!";
                    return returnMessage;
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    returnMessage.Success = false;
                    returnMessage.Msg = "Error.!!Not save." + ex.Message;
                    return returnMessage;
                }
            }
        }




        //public ReturnBaseMessageModel InsertChqRqst(ChqRqstModel chqRqstModel, TaskVerificationList TaskVerifierList)
        //{
        //    using (var transaction = uow.GetContext().Database.BeginTransaction())
        //    //{
        //    //    IsolationLevel = IsolationLevel.ReadUncommitted
        //    //}
        //    //))
        //    {
        //        try
        //        {


        //            var ProductId = uow.Repository<ADetail>().FindBy(x => x.IAccno == chqRqstModel.Iaccno).Select(x => x.PID).FirstOrDefault();
        //            if (!InformationUtilityService.IsAllowChequeNumber(ProductId))
        //            {
        //                returnMessage.Success = false;
        //                returnMessage.Msg = "Cheque book can't issued for this account.!! ";
        //                return returnMessage;
        //            }
        //            int branchId = commonService.GetBranchIdByEmployeeUserId();
        //            string branchName = commonService.BranchIdToName(branchId);
        //            var availableStock = uow.Repository<ReturnSingleValueModdel>().SqlQuery("select isnull(min(balance),0) as AmountValue from fin.fgetchqinv(" + branchId + ", " + chqRqstModel.Pics + ")").FirstOrDefault();
        //            var lastIndex = uow.Repository<ChqInventory>().SqlQuery("select * from fin.chqinventory where Tochqno - Lastindx =" + availableStock.AmountValue + " AND (Brnhid = " + branchId + ") AND(ISfinish = 0)").FirstOrDefault();

        //            if (availableStock.AmountValue == 0)
        //            {
        //                returnMessage.Success = false;
        //                returnMessage.Msg = "Cheque setup is not available in this branch" + branchName + ".Please do setup in Master>check inventory setup";
        //                return returnMessage;

        //            }
        //            else
        //            {

        //                var availableNumber = uow.Repository<ChqInventory>().FindBy(x => x.Brnhid == branchId && x.ISfinish == false).FirstOrDefault();

        //                int remainingStock = Convert.ToInt32(availableNumber.Tochqno - availableNumber.Lastindx);
        //                if (remainingStock < chqRqstModel.Pics)
        //                {
        //                    returnMessage.Success = false;
        //                    returnMessage.Msg = "Only" + remainingStock + "Numbers of piece is available in stock.";
        //                    return returnMessage;
        //                }




        //                ChqRqst chqRqstRow = new ChqRqst();
        //                chqRqstRow.Iaccno = Convert.ToInt32(chqRqstModel.Iaccno);
        //                chqRqstRow.Pics = chqRqstModel.Pics;
        //                chqRqstRow.Tdate = commonService.GetBranchTransactionDate();
        //                chqRqstRow.PostedBy = Loader.Models.Global.UserId;
        //                uow.Repository<ChqRqst>().Add(chqRqstRow);
        //                uow.Commit();
        //                taskUow.SaveTaskNotification(TaskVerifierList, chqRqstRow.Rno, 9);
        //                //uow.Commit();
        //                transaction.Commit();

        //                returnMessage.Success = true;
        //                returnMessage.Msg = "cheque issued successfully.!!";
        //                return returnMessage;


        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            transaction.Dispose();
        //            returnMessage.Success = false;
        //            returnMessage.Msg = "Error.!!Not save." + ex.Message;
        //            return returnMessage;
        //        }
        //    }
        //}
        public ReturnBaseMessageModel DeleteChqRqst(int Rno)
        {
            using (var Transaction = uow.BeginTransaction())
            {
                try
                {
                    ChqRqst chqRqstRow = new ChqRqst();
                    chqRqstRow = uow.Repository<ChqRqst>().FindBy(x => x.Rno == Rno).FirstOrDefault();
                    uow.Repository<ChqRqst>().Delete(chqRqstRow);
                    uow.Commit();

                    Transaction.Commit();

                    returnMessage.Success = true;
                    returnMessage.Msg = "Cheque issued Rejected successfully.!!";
                    return returnMessage;
                }
                catch (Exception ex)
                {
                    Transaction.Rollback();
                    returnMessage.Success = false;
                    returnMessage.Msg = "Error.!!Not save." + ex.Message;
                    return returnMessage;
                }
            }
        }

        public ChqRqstModel UnVerifiedChequeRegistration(int rNo)
        {
            var unVerifiedcheque = (from ch in uow.Repository<ChqRqst>().GetAll()
                                    join a in uow.Repository<ADetail>().GetAll() on ch.Iaccno equals a.IAccno

                                    where ch.Rno == rNo
                                    select new ChqRqstModel()
                                    {
                                        AccountNo = a.Accno,
                                        Pics = ch.Pics,
                                        Tdate = ch.Tdate,

                                    }).FirstOrDefault();

            return unVerifiedcheque;
        }

        public ReturnBaseMessageModel ChequeVerification(long rNo)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions()
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            }
            ))
            {
                try
                {

                   // int branchId = commonService.GetBranchIdByEmployeeUserId();
                    int branchId = commonService.GetBranchIdByEmployeeUserId();
                    string branchName = commonService.BranchIdToName(branchId);
                    var unverifyCheque = uow.Repository<ChqRqst>().GetSingle(x => x.Rno == rNo);
                    var availableStock = uow.Repository<ReturnSingleValueModdel>().SqlQuery("select isnull(min(balance),0) as AmountValue from fin.fgetchqinv(" + branchId + ", " + unverifyCheque.Pics + ")").FirstOrDefault();
                    var lastIndex = uow.Repository<ChqInventory>().SqlQuery("select * from fin.chqinventory where Tochqno - Lastindx =" + availableStock.AmountValue + " AND (Brnhid = " + branchId + ") AND(ISfinish = 0) order by Brnhid").FirstOrDefault();
                    //if (availableStock.AmountValue <= 0)
                    //{
                    //    returnMessage.Success = false;
                    //    returnMessage.Msg = "please check the available stock";
                    //    return returnMessage;
                    //}

                    if (availableStock.AmountValue == 0)
                    {
                        var availableNumber = uow.Repository<ChqInventory>().FindBy(x => x.Brnhid == branchId && x.ISfinish == false).FirstOrDefault();
                        if (availableNumber == null)
                        {
                            returnMessage.Success = false;
                            returnMessage.Msg = "Cheque setup is not available in this branch" + branchName + ".Please do setup in Master>check inventory setup";
                            return returnMessage;
                        }
                        else
                        {

                            int remainingStock = Convert.ToInt32(availableNumber.Tochqno - availableNumber.Lastindx);
                            returnMessage.Success = false;
                            returnMessage.Msg = "Only" + remainingStock + "Numbers of piece is available in stock.";
                            return returnMessage;
                        }

                    }
                    
                    //var chqInventory = uow.Repository<ChqInventory>().FindBy(x => x.Brnhid == branchId && x.ISfinish == true).ToList();

                    AChq aChqModel = new AChq();
                    aChqModel.approvedby = Global.UserId;
                    aChqModel.postedby = unverifyCheque.PostedBy;
                    aChqModel.IAccno = unverifyCheque.Iaccno;
                    aChqModel.cfrom = Convert.ToInt32(lastIndex.Lastindx);
                    aChqModel.cto = (Convert.ToInt32(lastIndex.Lastindx) + unverifyCheque.Pics) - 1; 
                    aChqModel.cstate = 2;
                    aChqModel.tdate = commonService.GetBranchTransactionDate();
                    aChqModel.IsPrinted = false;
                    uow.Repository<ChqRqst>().Delete(unverifyCheque);


                    var invChqRow = uow.Repository<ChqInventory>().GetSingle(x => x.Brnhid == branchId && x.Lastindx == lastIndex.Lastindx && x.Tochqno == lastIndex.Tochqno);
                    invChqRow.Lastindx = invChqRow.Lastindx + unverifyCheque.Pics;
                    if (invChqRow.Lastindx == invChqRow.Tochqno)
                    {
                        invChqRow.ISfinish = true;
                    }
                    uow.Repository<ChqInventory>().Edit(invChqRow);


                  
                    for (int i = aChqModel.cfrom; i <= aChqModel.cto; i++)
                    {
                        AchqH aChqH = new AchqH();
                        aChqH.cstate = 2;
                        aChqH.ChkNo = i;
                        aChqModel.AchqHs.Add(aChqH);
                    }
                    uow.Repository<AChq>().Add(aChqModel);

                    uow.Commit();
                    transaction.Complete();
                    returnMessage.Success = true;
                    returnMessage.Msg = "cheque Verified Successfully.Cheque number from#" + aChqModel.cfrom + " to#" + aChqModel.cto;
                    return returnMessage;

                }

                catch (Exception ex)
                {
                    transaction.Dispose();
                    returnMessage.Success = false;
                    returnMessage.Msg = "Error....Not save" + ex.Message;
                    return returnMessage;
                }
            }

        }

        public IPagedList<AChqModel> GetAChqList(string accNo, int pageNo, int pageSize)
        {
            var AChqs = uow.Repository<AChq>().Queryable();
            var adetails = uow.Repository<ADetail>().Queryable();
            var achqList = from chq in AChqs
                           join
                           ac in adetails on chq.IAccno equals ac.IAccno
                           where ac.Accno.Contains(accNo)
                           select new AChqModel
                           {
                               AccountNumber = ac.Accno,
                               cfrom = chq.cfrom,
                               cto = chq.cto,
                               cstate = chq.cstate,
                               tdate = chq.tdate,
                               rno = chq.rno,
                               IAccno = chq.IAccno
                           };


            return achqList.OrderByDescending(x => x.rno).ToPagedList(pageNo, pageSize); ;
        }

        public List<AChqModel> GetChqListByIAccno(int iaccNo)
        {
            var chqList = uow.Repository<AChq>().FindByMany(x => x.IAccno == iaccNo).Select(x => new AChqModel()
            {
                rno = x.rno,
                cfrom = x.cfrom,
                cto = x.cto,
                cstate = x.cstate,
                IAccno = x.IAccno,
                tdate = x.tdate

            }).ToList();

            return chqList;
        }

        public List<AChqModel> GetChequeStatusByIAccNoAndStatuId(int statusId, int iAccNo)
        {
            List<AChqModel> chqList = new List<AChqModel>();
            //if (statusId == 3)
            //{
            //    chqList = uow.Repository<AChqModel>().SqlQuery("select *from fin.AChq where cstate in(4,3) and IAccno=" + iAccNo).ToList();
            //}
            //else
            //{
            if (statusId != 2)
            {
                chqList = uow.Repository<AChq>().FindByMany(x => x.IAccno == iAccNo && x.cstate == statusId).Select(x => new AChqModel()
                {
                    rno = x.rno,
                    cfrom = x.cfrom,
                    cto = x.cto,
                    cstate = x.cstate,
                    IAccno = x.IAccno,
                    tdate = x.tdate

                }).ToList();
            }
            else
            {
                chqList = uow.Repository<AChq>().FindByMany(x => x.IAccno == iAccNo && x.cstate != 5 && x.cstate != 3).Select(x => new AChqModel()
                {
                    rno = x.rno,
                    cfrom = x.cfrom,
                    cto = x.cto,
                    cstate = x.cstate,
                    IAccno = x.IAccno,
                    tdate = x.tdate

                }).ToList();
            }

            //}
            return chqList;
        }

        public List<AChqHModel> GetAchqHList(int rno, int applyStatus)
        {
            List<AChqHModel> getChqNumber = new List<AChqHModel>();
            if (applyStatus != 0)
            {
                getChqNumber = uow.Repository<AchqH>().FindByMany(x => x.Rno == rno && x.Tno == null && x.cstate == applyStatus).Select(x => new AChqHModel()
                {
                    ChkNo = x.ChkNo,
                    cstate = x.cstate,
                    AchqHId = x.AchqHId,
                    Rno = x.Rno
                }).ToList();
            }
            else
            {
                getChqNumber = uow.Repository<AchqH>().FindByMany(x => x.Rno == rno && x.Tno == null).Select(x => new AChqHModel()
                {
                    ChkNo = x.ChkNo,
                    cstate = x.cstate,
                    AchqHId = x.AchqHId,
                    Rno = x.Rno
                }).ToList();
            }



            return getChqNumber;
        }

        public List<AChqHModel> GetDetailsChequeStatusByIAccNoAndStatuId(int statusId, int iAccNo)
        {
            var Achq = uow.Repository<AChq>().Queryable();
            var AchqH = uow.Repository<AchqH>().Queryable();
            var getStatusCheque = (from ch in Achq
                                   join chqh in AchqH on ch.rno equals chqh.Rno
                                   where ch.IAccno == iAccNo && ch.cstate == statusId && chqh.Tno == null
                                   select new AChqHModel()
                                   {
                                       AchqHId = chqh.AchqHId,
                                       ChkNo = chqh.ChkNo,
                                       cstate = chqh.cstate,

                                   }
                                 ).ToList();

            return getStatusCheque;
        }

        public ReturnBaseMessageModel UpdateChequeBlockUnBlockDeactivateChequeStatus(AChqModel achqModel)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions()
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            }
            ))
            {
                try
                {
                    foreach (var item in achqModel.AchqList)
                    {
                        var rAchq = uow.Repository<AChq>().GetSingle(x => x.rno == item.rno);
                        if (achqModel.AchqHDetailsList == null)
                        {
                            returnMessage.Success = false;
                            returnMessage.Msg = "There isn't any cheque pieces.Please select cheque piece.";
                            return returnMessage;
                        }
                        var chequeselectList = achqModel.AchqHDetailsList.Where(x => x.Rno == item.rno && x.IsSelectet == true).Count();
                        if (item.IsSelectet == true)
                        {
                            var isTnoExist = uow.Repository<AchqH>().GetSingle(x => x.Rno == item.rno && x.Tno != null);
                            if (isTnoExist != null)
                            {
                                if (achqModel.ApplyStatus == 3 || achqModel.ApplyStatus == 5)
                                {
                                    returnMessage.Success = false;
                                    returnMessage.Msg = "Cheque book is in used.Only piece block is allowed.Block and Deactive cheque book is not allowed.!!#(" + rAchq.cfrom + " - " + rAchq.cto + ")";
                                    return returnMessage;
                                }
                            }
                            if (chequeselectList > 0)
                            {
                                rAchq.cstate = achqModel.ApplyStatus;
                            }

                        }
                        else
                        {
                            if (chequeselectList > 0)
                            {
                                if (achqModel.ApplyStatus != 2)
                                {
                                    rAchq.cstate = achqModel.ApplyStatus;
                                }
                                else
                                {
                                    rAchq.cstate = achqModel.cstate;
                                }
                            }

                        }
                        if (chequeselectList > 0)
                        {
                            uow.Repository<AChq>().Edit(rAchq);
                            uow.Commit();
                        }


                        var chequeDetailsList = achqModel.AchqHDetailsList.Where(x => x.Rno == item.rno).ToList();
                        foreach (var detItem in chequeDetailsList)
                        {
                            if (detItem.IsSelectet == true)
                            {
                                var chqDetailsRow = uow.Repository<AchqH>().GetSingle(x => x.AchqHId == detItem.AchqHId);
                                AchqHH chqDetailsHis = new AchqHH();

                                chqDetailsRow.cstate = achqModel.ApplyStatus;

                                chqDetailsHis.AchqHId = detItem.AchqHId;
                                chqDetailsHis.Cstate = achqModel.ApplyStatus;
                                chqDetailsHis.Postedby = Loader.Models.Global.UserId;
                                chqDetailsHis.PostedOn = commonService.GetBranchTransactionDate();
                                chqDetailsHis.Tdate = commonService.GetBranchTransactionDate();
                                chqDetailsHis.Remarks = achqModel.Remarks;

                                uow.Repository<AchqH>().Edit(chqDetailsRow);
                                // chqDetailsRow.AchqHHs.Add(chqDetailsHis);
                                uow.Repository<AchqHH>().Add(chqDetailsHis);
                                uow.Commit();
                            }
                        }

                    }
                    transaction.Complete();
                    returnMessage.Success = true;
                    returnMessage.Msg = "Cheque Book  status change Succesfully.!!";
                    return returnMessage;
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    returnMessage.Success = false;
                    returnMessage.Msg = "Cheque Book  status not change.!!." + ex.Message;
                    return returnMessage;
                }
            }
        }


        public IPagedList<ChqRqstModel> ChequeVerificationList(int pageNo, int pageSize)
        {
            var unVerifiedchequeList = from ch in uow.Repository<ChqRqst>().Queryable()
                                       join a in uow.Repository<ADetail>().Queryable() on ch.Iaccno equals a.IAccno


                                       select new ChqRqstModel()
                                       {
                                           AccountNo = a.Accno,
                                           Pics = ch.Pics,
                                           Tdate = ch.Tdate,

                                       };

            return unVerifiedchequeList.ToPagedList(pageNo, pageSize);
        }

        public List<AChqModel> GetChequeRegisteredNotPrinted()
        {
            List<AChqModel> getCheqRegNotPrinted = new List<AChqModel>();
            getCheqRegNotPrinted = uow.Repository<AChq>().FindBy(x => x.cstate == 2 && x.IsPrinted == false).Select(x => new AChqModel()
            {
                rno = x.rno,
                IAccno = x.IAccno,
                cfrom = x.cfrom,
                cto = x.cto,
                AccountNumber = x.ADetail.Accno,
                AccountName = x.ADetail.Aname,
                AccountType = x.ADetail.ProductDetail.PName
            }).ToList();

            return getCheqRegNotPrinted;
        }

        public AChqModel GetChequeDetailsFromRno(int RNo)
        {
            AChqModel getCheqDetails = new AChqModel();
            getCheqDetails = uow.Repository<AChq>().GetAll().Where(x => x.rno == RNo).Select(x => new AChqModel()
            {
                rno = x.rno,
                IAccno = x.IAccno,
                cfrom = x.cfrom,
                cto = x.cto,
                AccountNumber = x.ADetail.Accno,
                AccountName = x.ADetail.Aname,
                AccountType = x.ADetail.ProductDetail.PName
            }).FirstOrDefault();
            return getCheqDetails;
        }

        public void AddChequeAttribute(ChqAttributeViewModel chqAttributeViewModel)
        {
            ChqAttribute chqAttribute = new ChqAttribute();
            chqAttribute = uow.Repository<ChqAttribute>().GetSingle(x => x.BranchId == Global.BranchId);
            if (chqAttribute != null)
            {
                chqAttributeViewModel.Id = chqAttribute.Id;
            }
            if (chqAttributeViewModel.Id != 0)
            {
                chqAttribute.AccountNamePostion = chqAttributeViewModel.AccountNamePostion;
                chqAttribute.AccountNumberPosition = chqAttributeViewModel.AccountNumberPosition;
                chqAttribute.AccountTypePosition = chqAttributeViewModel.AccountTypePosition;
                chqAttribute.BranchNamePosition = chqAttributeViewModel.BranchNamePosition;
                chqAttribute.ChequeNumberPosition = chqAttributeViewModel.ChequeNumberPosition;
                chqAttribute.Cheque2NumberPosition = chqAttributeViewModel.Cheque2NumberPosition;
                chqAttribute.BranchAddressPosition = chqAttributeViewModel.BranchAddressPosition;
                chqAttribute.BranchPhoneNumberPosition = chqAttributeViewModel.BranchPhoneNumberPosition;

                chqAttribute.BranchId = Global.BranchId;
                chqAttribute.Id = chqAttributeViewModel.Id;
                uow.Repository<ChqAttribute>().Edit(chqAttribute);
            }
            else
            {
                var chqAttritbute2 = new ChqAttribute();
                chqAttritbute2.AccountNamePostion = chqAttributeViewModel.AccountNamePostion;
                chqAttritbute2.AccountNumberPosition = chqAttributeViewModel.AccountNumberPosition;
                chqAttritbute2.AccountTypePosition = chqAttributeViewModel.AccountTypePosition;
                chqAttritbute2.BranchNamePosition = chqAttributeViewModel.BranchNamePosition;
                chqAttritbute2.ChequeNumberPosition = chqAttributeViewModel.ChequeNumberPosition;
                chqAttritbute2.Cheque2NumberPosition = chqAttributeViewModel.Cheque2NumberPosition;
                chqAttribute.BranchAddressPosition = chqAttributeViewModel.BranchAddressPosition;
                chqAttribute.BranchPhoneNumberPosition = chqAttributeViewModel.BranchPhoneNumberPosition;

                chqAttribute.BranchId = Global.BranchId;
                chqAttribute = chqAttritbute2;

                uow.Repository<ChqAttribute>().Add(chqAttribute);
            }

            uow.Commit();
        }
        public void AddChequeSize(string height, string width)
        {
            ChqSize chqSize = new ChqSize();
            height = (Convert.ToDecimal(height) * 96).ToString();
            width = (Convert.ToDecimal(width) * 96).ToString();
            var c = uow.Repository<ChqSize>().GetAll().FirstOrDefault();
            //chqSize = uow.Repository<ChqSize>().GetSingle(x => (x.Height == height) && (x.Width == width));
            if (c == null)
            {
                c = new ChqSize();
                c.Height = height + "px";
                c.Width = width + "px";
                uow.Repository<ChqSize>().Add(c);
            }
            else
            {
                c.Height = height + "px";
                c.Width = width + "px";
                uow.Repository<ChqSize>().Edit(c);
            }
            uow.Commit();
        }

        public void SetPrinter(string printerName)
        {
            var chqAttribute = uow.Repository<ChqAttribute>().GetAll().FirstOrDefault();
            chqAttribute.DefaultPrinterName = printerName;
            //chqAttribute.PaperSize = paperSize;
            uow.Repository<ChqAttribute>().Edit(chqAttribute);
            uow.Commit();
        }
        public ChqAttribute GetPrinter()
        {
            var chqAttribute = uow.Repository<ChqAttribute>().GetAll().FirstOrDefault();
            return chqAttribute;

        }
        public List<Loader.Models.ParamValue> GetCompanyDetailForCheque()
        {
            Loader.Repository.GenericUnitOfWork loaderUOW = new Loader.Repository.GenericUnitOfWork();

            //  List<Loader.Models.ParamValue> companyList = loaderUOW.Repository<Loader.Models.ParamValue>().FindBy(x => x.PId == (int)Loader.Helper.EnumValue.Parameter.CompanyName || x.PId == (int)Loader.Helper.EnumValue.Parameter.CompanyAddress || x.PId == (int)Loader.Helper.EnumValue.Parameter.PhoneNumber).ToList();
            List<Loader.Models.ParamValue> companyList = loaderUOW.Repository<Loader.Models.ParamValue>().FindBy(x => x.PId == 4 || x.PId == 4 || x.PId == 1).ToList();

            return companyList;

        }
        public ChqAttribute GetChequeAttribute()
        {
            ChqAttribute chqAttribute = new ChqAttribute();
            chqAttribute = uow.Repository<ChqAttribute>().GetAll().FirstOrDefault();
            return chqAttribute;
        }
        public ChqSize GetChequeSize()
        {
            ChqSize chqSize = new ChqSize();
            chqSize = uow.Repository<ChqSize>().GetAll().FirstOrDefault();
            return chqSize;
        }
        public string GetChequePrintType()
        {
            var param = paramService.GetSingle(8012); //later add pid in seeding pid
            return param.paramValue.PValue;
        }
        public void UpdatePrintedCheques(List<AChqModel> aChqModel)
        {
            List<AChq> achq = new List<AChq>();

            for (int i = 0; i < aChqModel.Count; i++)
            {
                var pkey = aChqModel[i].rno;
                var achqTemp = uow.Repository<AChq>().GetSingle(x => x.rno == pkey);
                achqTemp.IsPrinted = aChqModel[i].IsPrinted;
                achq.Add(achqTemp);
            }
            foreach (var item in achq)
            {
                uow.Repository<AChq>().Edit(item);
            }

            uow.Commit();
        }

        #endregion

        #region cheque Good for Payment
        public ReturnBaseMessageModel ChequeGoodForPayment(WithdrawViewModel withdrawModel, TaskVerificationList taskVerifier)
        {
            //using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions()
            //{
            //    IsolationLevel = IsolationLevel.ReadUncommitted
            //}
            //))


            using (var transaction = uow.GetContext().Database.BeginTransaction())
            {

                DateTime transactionDate = commonService.GetBranchTransactionDate();





                {
                    try
                    {
                        if (withdrawModel.IsBounce)
                        {
                            withdrawModel.ChequeNumber = withdrawModel.BounceChequeNumber;
                        }
                        bool IsExiestscheque = TellerUtilityService.CheckChequeNumber(withdrawModel.ChequeNumber, withdrawModel.AccountId);
                        if (IsExiestscheque == false)
                        {
                            returnMessage.Msg = "This cheque number doesn't belongs to account holder.";
                            returnMessage.Success = false;
                            return returnMessage;
                        }
                        if (TellerUtilityService.IsDuplicateChequeNo(withdrawModel.AccountId, withdrawModel.ChequeNumber))
                        {
                            if (InformationUtilityService.IsChequeBounce(withdrawModel.AccountId, withdrawModel.ChequeNumber))
                            {
                                returnMessage.Msg = "Cheque is bounce for reason:-" + InformationUtilityService.BounceReason(withdrawModel.AccountId, withdrawModel.ChequeNumber) + ".!!";
                                returnMessage.Success = false;
                                return returnMessage;
                            }
                            else
                            {
                                returnMessage.Msg = "Cheque  number is already used.!!";
                                returnMessage.Success = false;
                                return returnMessage;
                            }

                        }
                        returnMessage = TellerUtilityService.CheckChequeState(withdrawModel.AccountId, withdrawModel.ChequeNumber);
                        if (!returnMessage.Success)
                        {
                            return returnMessage;
                        }
                        if (TellerUtilityService.IsDeactiveChequeBook(withdrawModel.AccountId, withdrawModel.ChequeNumber))
                        {
                            returnMessage.Msg = "Cheque book is finished.!!";
                            returnMessage.Success = false;
                            return returnMessage;
                        }

                        if (withdrawModel.IsBounce)
                        {
                            if (withdrawModel.BounceChequeNumber != 0 && (withdrawModel.ChqBounceReason != null || withdrawModel.ChqBounceReason != ""))
                            {
                                DateTime currentDate = commonService.GetBranchTransactionDate();
                                IchkBounce ichqBounce = new IchkBounce();
                                ichqBounce.Chkno = withdrawModel.BounceChequeNumber;
                                ichqBounce.Rmks = withdrawModel.ChqBounceReason;
                                ichqBounce.IAccno = withdrawModel.AccountId;
                                ichqBounce.TDate = currentDate;
                                ichqBounce.Postedon = currentDate;
                                ichqBounce.PostedBy = Global.UserId;
                                uow.Repository<IchkBounce>().Add(ichqBounce);
                                uow.Commit();
                                commonService.UpdateTnoInChqNumber(withdrawModel.ChequeNumber, 0);
                                // transaction.Complete();
                                transaction.Commit();
                                returnMessage.Msg = "Cheque Bounce reported successfully.!!";
                                returnMessage.Success = true;
                                return returnMessage;
                            }
                            else
                            {
                                returnMessage.Msg = "Please fill Bounce Cheque Number  or Cheque Bounce Reason.!!";
                                returnMessage.Success = false;
                                return returnMessage;
                            }
                        }
                        else
                        {

                            decimal currentAmount = TellerUtilityService.AvailableBalance(withdrawModel.AccountId);

                            if (currentAmount < withdrawModel.Amount)
                            {
                                returnMessage.Msg = "Withdraw amount exceeds the available balance which is-" + currentAmount + " !!.";
                                returnMessage.Success = false;
                                returnMessage.ReturnId = 1;
                                return returnMessage;
                            }
                            Int64 transactionNum = commonService.GetUtno();
                            AchqFGdPymt achqPaymentRow = new AchqFGdPymt();
                            achqPaymentRow.IAccno = withdrawModel.AccountId;
                            achqPaymentRow.tno = transactionNum;
                            achqPaymentRow.brnhid = (short)commonService.GetBranchIdByEmployeeUserId();
                            achqPaymentRow.Chqno = withdrawModel.ChequeNumber;
                            achqPaymentRow.notes = withdrawModel.Remarks;
                            achqPaymentRow.amount = withdrawModel.Amount;
                            achqPaymentRow.tdate = commonService.GetBranchTransactionDate();
                            achqPaymentRow.payee = withdrawModel.Payee;
                            achqPaymentRow.postedby = Global.UserId;
                            achqPaymentRow.tstate = 1;
                            uow.Repository<AchqFGdPymt>().Add(achqPaymentRow);

                            commonService.InsertAvailableBalance(5, withdrawModel.AccountId, withdrawModel.Amount);
                            uow.Commit();
                            commonService.UpdateTnoInChqNumber(withdrawModel.ChequeNumber, transactionNum);
                            taskUow.SaveTaskNotification(taskVerifier, transactionNum, 13);
                            //  transaction.Complete();
                            transaction.Commit();
                            returnMessage.Msg = "Cheque good for payment save successfully with Transaction No##" + transactionNum + ".!!";
                            returnMessage.Success = true;
                        }

                        return returnMessage;
                    }
                    catch (Exception ex)
                    {

                        returnMessage.Msg = "OOps somthing wrong.!!Not Save.!!" + ex.Message;
                        returnMessage.Success = false;
                        transaction.Dispose();
                        return returnMessage;

                    }
                }
            }
        }


        public WithdrawViewModel GetChequeGoodForPaymentDetails(long tno)
        {
            var goodForPayment = uow.Repository<AchqFGdPymt>().FindByMany(x => x.tno == tno).Select(singleGoodForPayment =>
           new WithdrawViewModel()
           {
               Amount = singleGoodForPayment.amount,
               AccountId = singleGoodForPayment.IAccno,
               Payee = singleGoodForPayment.payee,
               ChequeNumber = singleGoodForPayment.Chqno,
               AccountNumber = singleGoodForPayment.ADetail.Accno,
               AccountName = singleGoodForPayment.ADetail.Aname,
               //  BranchName = singleGoodForPayment..BrnhNam,
               Tno = singleGoodForPayment.tno,
               TransDate = singleGoodForPayment.tdate,
               Remarks = singleGoodForPayment.notes

           });

            return goodForPayment.FirstOrDefault();
        }
        public IPagedList<WithdrawViewModel> GetChequeGoodForPaymentList(int pageNo, int pageSize)
        {
            var singleGoodForPaymentList = uow.Repository<AchqFGdPymt>().FindByMany(x => x.approvedby == null).Select(
             singleGoodForPayment => new WithdrawViewModel()
             {
                 Amount = singleGoodForPayment.amount,
                 AccountId = singleGoodForPayment.IAccno,
                 Payee = singleGoodForPayment.payee,
                 ChequeNumber = singleGoodForPayment.Chqno,
                 AccountNumber = singleGoodForPayment.ADetail.Accno,
                 AccountName = singleGoodForPayment.ADetail.Aname,
                 // BranchName = singleGoodForPayment.LicenseBranch.BrnhNam,
                 Tno = singleGoodForPayment.tno,
                 TransDate = singleGoodForPayment.tdate,
                 Remarks = singleGoodForPayment.notes

             }).OrderBy(x => x.Tno);

            return singleGoodForPaymentList.ToPagedList(pageNo, pageSize);
        }

        public ReturnBaseMessageModel ApproveChequeGoodForPayment(long eventValue, string Remarks)
        {
            try
            {
                var singleGoodForPayment = uow.Repository<AchqFGdPymt>().GetSingle(x => x.tno == eventValue);
                singleGoodForPayment.approvedby = Global.UserId;
                singleGoodForPayment.tstate = 2;
                if (Remarks != "")
                {
                    singleGoodForPayment.notes = Remarks;
                }
                singleGoodForPayment.IsRejected = false;
                uow.Repository<AchqFGdPymt>().Edit(singleGoodForPayment);
                uow.Commit();
                returnMessage.Msg = "Cheque good for payment verify successfully with Transaction No##" + singleGoodForPayment.tno + ".!!";
                returnMessage.Success = true;
                return returnMessage;
            }
            catch (Exception ex)
            {
                returnMessage.Msg = "Not Save.!!" + ex.Message;
                returnMessage.Success = false;
                return returnMessage;
            }


        }
        #endregion

        #region internal Cheque deposit
        public ReturnBaseMessageModel InsertInternalChequeDeposit(InternalChequeDepositModel internalChqModel, TaskVerificationList taskVerifier)
        {
            // using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions()
            // {
            //     IsolationLevel = IsolationLevel.ReadUncommitted
            // }
            //))

            using (var transaction = uow.GetContext().Database.BeginTransaction())
            {

                DateTime transactionDate = commonService.GetBranchTransactionDate();
                {
                    try
                    {
                        if (internalChqModel.IsBounce)
                        {
                            internalChqModel.SlpNo = internalChqModel.BounceChequeNumber;
                        }
                        bool IsExiestscheque = TellerUtilityService.CheckChequeNumber(internalChqModel.SlpNo, internalChqModel.FIaccno);
                        if (IsExiestscheque == false)
                        {
                            returnMessage.Msg = "This cheque number doesn't belongs to account holder.";
                            returnMessage.Success = false;
                            return returnMessage;
                        }
                        if (TellerUtilityService.IsDuplicateChequeNo(internalChqModel.FIaccno, internalChqModel.SlpNo))
                        {
                            if (InformationUtilityService.IsChequeBounce(internalChqModel.FIaccno, internalChqModel.SlpNo))
                            {
                                returnMessage.Msg = "Cheque is bounce for reason:-" + InformationUtilityService.BounceReason(internalChqModel.FIaccno, internalChqModel.SlpNo) + ".!!";
                                returnMessage.Success = false;
                                return returnMessage;
                            }
                            else
                            {
                                returnMessage.Msg = "Cheque  number is already used.!!";
                                returnMessage.Success = false;
                                return returnMessage;
                            }

                        }
                        returnMessage = TellerUtilityService.CheckChequeState(internalChqModel.FIaccno, internalChqModel.SlpNo);
                        if (!returnMessage.Success)
                        {
                            return returnMessage;
                        }
                        if (TellerUtilityService.IsDeactiveChequeBook(internalChqModel.FIaccno, internalChqModel.SlpNo))
                        {
                            returnMessage.Msg = "Cheque book is finished.!!";
                            returnMessage.Success = false;
                            return returnMessage;
                        }
                        if (internalChqModel.IsBounce)
                        {
                            if (internalChqModel.BounceChequeNumber != 0 && internalChqModel.ChqBounceReason != null)
                            {
                                DateTime currentDate = commonService.GetBranchTransactionDate();
                                IchkBounce ichqBounce = new IchkBounce();
                                ichqBounce.Chkno = internalChqModel.BounceChequeNumber;
                                ichqBounce.Rmks = internalChqModel.ChqBounceReason;
                                ichqBounce.IAccno = internalChqModel.FIaccno;
                                ichqBounce.TDate = currentDate;
                                ichqBounce.Postedon = currentDate;
                                ichqBounce.PostedBy = Global.UserId;
                                uow.Repository<IchkBounce>().Add(ichqBounce);
                                uow.Commit();
                                commonService.UpdateTnoInChqNumber(internalChqModel.SlpNo, 0);
                                transaction.Commit();
                               // transaction.Complete();
                                returnMessage.Msg = "Cheque number bounce report successfully.!!";
                                returnMessage.Success = true;
                                return returnMessage;
                            }
                        }
                        else
                        {
                            if (internalChqModel.FIaccno == internalChqModel.TIAccno)
                            {
                                returnMessage.Msg = "Please select different Account Number for receipient a/c no.!!";
                                returnMessage.Success = false;
                                return returnMessage;
                            }
                            decimal availableBalance = TellerUtilityService.AvailableBalance(internalChqModel.FIaccno);
                            if (availableBalance < internalChqModel.Amt)
                            {
                                returnMessage.Msg = "Withdraw amount exceeds the available balance which is-" + availableBalance + "!!.";
                                returnMessage.Success = false;
                                returnMessage.ReturnId = 1;
                                return returnMessage;
                            }
                            int accounState = TellerUtilityService.GetAccountStatus(internalChqModel.TIAccno);
                            if (accounState == 9)//Credit Restricted
                            {
                                returnMessage.Msg = "Deposit Faild.This Account is in Credit Restricted state.!!";
                                returnMessage.Success = false;
                                return returnMessage;
                            }

                            if (accounState == 7)//Ready close
                            {
                                returnMessage.Msg = "Deposit Faild.This Account is in Ready To be Closed state.!!";
                                returnMessage.Success = false;
                                return returnMessage;
                            }
                            returnMessage = TellerUtilityService.GetCheckValidAccountStatus(internalChqModel.TIAccno);
                            if (!returnMessage.Success)
                            {
                                return returnMessage;
                            }
                            returnMessage = new TellerService().GuarantorBlockAmount(internalChqModel.FIaccno, internalChqModel.Amt, availableBalance);
                            if (!returnMessage.Success)
                            {
                                return returnMessage;
                            }
                            int productId = TellerUtilityService.GetPid(internalChqModel.TIAccno);
                            bool IsFixedAccount = TellerUtilityService.IsFixedAccount(productId);


                            if (IsFixedAccount)
                            {
                                decimal adrLimit = TellerUtilityService.DebitLimitAmount(internalChqModel.TIAccno);
                                decimal currentAmount = TellerUtilityService.AvailableGoodBalanceWithShadowCr(internalChqModel.TIAccno);
                                decimal addedAmount = currentAmount + internalChqModel.Amt;

                                if (adrLimit == currentAmount)
                                {
                                    returnMessage.Msg = "The Approved Amount has already been Deposited.No additional deposit is allowed in this Fixed Account.!!";
                                    returnMessage.Success = false;
                                    return returnMessage;
                                }
                                if (addedAmount > adrLimit)
                                {
                                    returnMessage.Msg = "Deposit Amount you entered EXCEEDS Approved amount of this account.!!";
                                    returnMessage.Success = false;
                                    return returnMessage;
                                }
                                DateTime maturedate = TellerUtilityService.GetCheckMatureDateForFixed(internalChqModel.TIAccno);
                                if (commonService.GetBranchTransactionDate() >= maturedate)
                                {
                                    returnMessage.Msg = "The account is already Matured.NO additional Deposit is allowed unless the account is Renewed.!!";
                                    returnMessage.Success = false;
                                    return returnMessage;
                                }
                            }
                            if (TellerUtilityService.IsRecurringDeposit(productId))
                            {
                                DateTime maturedate = TellerUtilityService.GetCheckMatureDateForRecurring(internalChqModel.TIAccno);
                                if (maturedate != DateTime.MinValue)
                                {
                                    if (commonService.GetBranchTransactionDate() >= maturedate)
                                    {
                                        returnMessage.Msg = "The account is already Matured.NO additional Deposit is allowed unless the account is Renewed.!!";
                                        returnMessage.Success = false;
                                        return returnMessage;
                                    }
                                }
                               

                                int accounState1 = TellerUtilityService.GetAccountStatus(internalChqModel.FIaccno);
                                int productId1 = TellerUtilityService.GetPid(internalChqModel.FIaccno);
                                bool IsFixedAccount1 = TellerUtilityService.IsFixedAccount(productId1);
                                if (IsFixedAccount1)
                                {
                                    bool isMature = TellerUtilityService.IsAlreadyMatured(internalChqModel.FIaccno);
                                    if (accounState1 != 7)
                                    {
                                        if (!isMature)
                                        {
                                            returnMessage.Msg = "Withdraw is not Allowed.! \n Account NOT Matured Yet..!!";
                                            returnMessage.Success = false;
                                            return returnMessage;
                                        }
                                    }
                                }
                                if (accounState1 == 5)//Debit Restricted
                                {
                                    returnMessage.Msg = "Withdraw Faild.This Account is in Debit Restricted state.!!";
                                    returnMessage.Success = false;
                                    return returnMessage;
                                }
                            }

                            int AccountBranchId = commonService.AccountHolderBranchId(internalChqModel.TIAccno);
                            int fromAccountBranchId = commonService.AccountHolderBranchId(internalChqModel.FIaccno);

                            if (AccountBranchId == fromAccountBranchId)
                                internalChqModel.IBChq = true;
                            else
                                internalChqModel.IBChq = false;

                            Int64 transactionNum = commonService.GetUtno();
                            IChkDep ichequqDeposit = new IChkDep();
                            ichequqDeposit.FIaccno = internalChqModel.FIaccno;
                            ichequqDeposit.TIAccno = internalChqModel.TIAccno;
                            ichequqDeposit.Tdate = commonService.GetBranchTransactionDate();
                            ichequqDeposit.Tno = transactionNum;
                            ichequqDeposit.Amt = internalChqModel.Amt;
                            ichequqDeposit.Ttype = 50;
                            ichequqDeposit.SlpNo = internalChqModel.SlpNo;
                            ichequqDeposit.postedby = Global.UserId;
                            ichequqDeposit.BrchID = (short)commonService.GetBranchIdByEmployeeUserId(); ;
                            ichequqDeposit.IBChq = internalChqModel.IBChq;
                            uow.Repository<IChkDep>().Add(ichequqDeposit);

                            commonService.InsertAvailableBalance(1, internalChqModel.FIaccno, internalChqModel.Amt);
                            commonService.InsertAvailableBalance(2, internalChqModel.TIAccno, internalChqModel.Amt);

                            uow.Commit();
                            commonService.UpdateTnoInChqNumber(internalChqModel.SlpNo, transactionNum);
                            taskUow.SaveTaskNotification(taskVerifier, transactionNum, 14);
                            transaction.Commit();
                            //transaction.Complete();
                            returnMessage.Msg = "Cheque deposit save successfully with Transaction No##" + transactionNum + ".!!";
                            returnMessage.Success = true;
                        }

                        return returnMessage;
                    }
                    catch (Exception ex)
                    {

                        returnMessage.Msg = "Not Save.!!" + ex.Message;
                        returnMessage.Success = false;
                        transaction.Dispose();
                        return returnMessage;

                    }
                }
            }
        }

        public InternalChequeDepositModel InternalChequeDepositDetails(long tno)
        {
            var singleChequeDeposit = uow.Repository<IChkDep>().FindByMany(i => i.Tno == tno).Select(x => new InternalChequeDepositModel()
            {
                FromAccountNumber = x.ADetail.Accno,
                AccountName = x.ADetail.Aname,
                ToAccountName = x.ADetail1.Aname,
                ToAccountNumber = x.ADetail1.Accno,
                Amt = x.Amt,
                Tdate = x.Tdate,
                SlpNo = x.SlpNo,
                Tno = x.Tno

            }).FirstOrDefault();
            return singleChequeDeposit;
        }
        public IPagedList<InternalChequeDepositModel> InternalChequeDepositList(int pageNo, int pageSize)
        {
            var singleChequeDeposit = uow.Repository<IChkDep>().FindByMany(i => i.verifiedby == null).Select(x => new InternalChequeDepositModel()
            {
                FromAccountNumber = x.ADetail.Accno,
                AccountName = x.ADetail.Aname,
                ToAccountName = x.ADetail1.Aname,
                ToAccountNumber = x.ADetail1.Accno,
                Amt = x.Amt,
                Tdate = x.Tdate,
                SlpNo = x.SlpNo,
                Tno = x.Tno

            }).OrderBy(x=>x.Tno);
            return singleChequeDeposit.ToPagedList(pageNo, pageSize);
        }
        public ReturnBaseMessageModel ApproveInternalChequeDepositDetails(long tno)
        {
            // using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions()
            // {
            //     IsolationLevel = IsolationLevel.ReadUncommitted
            // }
            //))

            using (var transaction = uow.GetContext().Database.BeginTransaction())
            {

                DateTime transactionDate = commonService.GetBranchTransactionDate();
                {

                    try
                    {
                        var getsingle = uow.Repository<IChkDep>().FindBy(x => x.Tno == tno).FirstOrDefault();
                        getsingle.Ttype = 51;
                        getsingle.verifiedby = Global.UserId;
                        getsingle.Vdate = commonService.GetBranchTransactionDate();
                        uow.Repository<IChkDep>().Edit(getsingle);

                        uow.Commit();
                        #region FromChequeDeposit

                        commonService.InsertAvailableBalance(1, getsingle.FIaccno, (-getsingle.Amt));
                        commonService.InsertAvailableBalance(3, getsingle.FIaccno, (-getsingle.Amt));
                        commonService.InsertAvailableBalance(3, getsingle.TIAccno, getsingle.Amt);
                        commonService.InsertAvailableBalance(2, getsingle.TIAccno, (-getsingle.Amt));

                        var singleFromAccount = uow.Repository<ADetail>().GetSingle(x => x.IAccno == getsingle.FIaccno);
                        var singleToAccount = uow.Repository<ADetail>().GetSingle(x => x.IAccno == getsingle.TIAccno);
                        singleFromAccount.Bal = singleFromAccount.Bal - getsingle.Amt;
                        singleToAccount.Bal = singleToAccount.Bal + getsingle.Amt;
                        uow.Repository<ADetail>().Edit(singleFromAccount);
                        uow.Repository<ADetail>().Edit(singleToAccount);
                        #endregion
                        uow.Commit();
                        transaction.Commit();
                        //transaction.Complete();
                        returnMessage.Msg = "Cheque deposit verify successfully with Transaction No##" + getsingle.Tno + ".!!";
                        returnMessage.Success = true;
                        return returnMessage;
                    }

                    catch (Exception ex)
                    {
                        returnMessage.Msg = "Not Save.!!" + ex.Message;
                        returnMessage.Success = false;
                        transaction.Dispose();
                        return returnMessage;
                    }

                }

            }


        }
        #endregion
    }
}

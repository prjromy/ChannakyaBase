using ChannakyaBase.BLL.Repository;
using ChannakyaBase.DAL.DatabaseModel;
using ChannakyaBase.Model.Models;
using ChannakyaCustomeDatePicker.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChannakyaBase.Model.ViewModel;
using Loader.Models;
using ChannakyaBase.BLL.CustomHelper;
using PagedList;
using System.Data.SqlClient;
using System.Transactions;

namespace ChannakyaBase.BLL.Service
{

    public class ShareService
    {
        ReturnBaseMessageModel returnMessage = null;
        private ChannakyaBaseEntities _context = null;
        private GenericUnitOfWork uow = null;
        private CommonService commonService = null;
        DatePickerService datePickerService = null;

        BaseTaskVerificationService taskUow = null;
        public ShareService()
        {
            commonService = new CommonService();
            datePickerService = new DatePickerService();
            taskUow = new BaseTaskVerificationService();
            uow = new GenericUnitOfWork();
            returnMessage = new ReturnBaseMessageModel();
            _context = new ChannakyaBaseEntities();
        }

      

        public CustInformationViewModel ShareRegistrationDetails(int regId)
        {
            var regDetails = uow.Repository<CustInformationViewModel>().SqlQuery(@"select cl.CId,Name,Mobile,Address,ContactPerson,IsIndividual from cust.FGetCustListForSearch() cl join fin.ShrReg sg
                                                                              on sg.CId = cl.CID where sg.Regno =" + regId).FirstOrDefault();
            return regDetails;
        }

       

        public List<ShareNomineeModel> GetShareNomineeListByRegId(int regId)
        {
            var shareNomineeList = uow.Repository<Snominee>().FindBy(x => x.RegNo == regId).Select(x => new ShareNomineeModel()
            {
                CCertno = x.CCertno,
                NomNam = x.NomNam,
                NomRel = x.NomRel,
                Share = x.Share,
                SnomineeId = x.SnomineeId,
                ContactNo=x.ContactNo
            }).ToList();
            return shareNomineeList;
        }
        public ReturnBaseMessageModel ShareRegistrationSave(ShareRegisterViewModel shareRegModel, TaskVerificationList taskVerifier)
        {
            //using (TransactionScope transaction = new TransactionScope(
            //     // a new transaction will always be created
            //     TransactionScopeOption.RequiresNew,
            //     // we will allow volatile data to be read during transaction
            //     new TransactionOptions()
            //     {
            //         IsolationLevel = IsolationLevel.ReadUncommitted,

            //     }
            // ))

            using (var transaction = uow.GetContext().Database.BeginTransaction())

            {

                {

                    try
                    {
                        if (shareRegModel.CId.Count() == 0)
                        {
                            returnMessage.Msg = "Please Select customer!!";
                            returnMessage.Success = false;
                            return returnMessage;
                        }
              
                        if (shareRegModel.ReferredBy.Count() == 0)
                        {
                            returnMessage.Msg = "Please Select ReferredBy!!";
                            returnMessage.Success = false;
                            return returnMessage;
                        }
                        if (shareRegModel.BroughtBy == 0)
                        {
                            returnMessage.Msg = "Please Select collector!!";
                            returnMessage.Success = false;
                            return returnMessage;
                        }
                        float totalShare = 0;
                        foreach (var item in shareRegModel.ANominees)
                        {
                            totalShare += item.Share;
                        }
                        if (totalShare > 100)
                        {
                            returnMessage.Msg = "Nominees share is not more then 100%!!";
                            returnMessage.Success = false;
                            return returnMessage;
                        }
                        else if (totalShare < 100)
                        {
                            returnMessage.Msg = "Nominees share is not less then 100%!!";
                            returnMessage.Success = false;
                            return returnMessage;
                        }
                        var customerId = shareRegModel.CId[0];
                        var count = uow.Repository<ShrReg>().FindBy(x => x.CId ==customerId).Count();
                        if (count >= 1)
                        {
                            returnMessage.Msg = "Share Registration already exists!!";
                            returnMessage.Success = false;
                            return returnMessage;
                        }
                            ShrReg objRegShare = new ShrReg();
                            objRegShare.CId = shareRegModel.CId[0];
                            objRegShare.Rdate = shareRegModel.Rdate;
                            objRegShare.PostedBy = Global.UserId;
                            objRegShare.RegistrationCode = ShareUtilityService.GetShareRegNumber();
                            objRegShare.PostedOn = commonService.GetDate();
                            uow.Repository<ShrReg>().Add(objRegShare);
                                  uow.Commit();
                            #region Account Nominee
                            // Account Nominee Details
                            foreach (var item in shareRegModel.ANominees)
                            {
                                Snominee accountNominee = new Snominee();

                                accountNominee.NomNam = item.NomNam;
                                accountNominee.NomRel = item.NomRel;
                                accountNominee.CCertID = item.CCertID.ToString();
                                accountNominee.CCertno = item.CCertno;
                                accountNominee.Share = item.Share;
                                accountNominee.ContactNo = item.ContactNo;
                                accountNominee.ContactAddress = item.ContactAddress;
                                objRegShare.Snominees.Add(accountNominee);

                            }
                            //       uow.Commit();
                            #endregion

                            ShareReferenceInfo reference = new ShareReferenceInfo();
                            reference.IAccNo = Convert.ToInt32(objRegShare.RegNo);
                            //       reference.IAccNo = (int)objRegShare.RegNo;
                            reference.ReferredBy = shareRegModel.ReferredBy[0];
                            reference.BroughtBy = shareRegModel.BroughtBy;
                            reference.RType = 0;
                            uow.Repository<ShareReferenceInfo>().Add(reference);
                            uow.Commit();
                            taskUow.SaveTaskNotification(taskVerifier, Convert.ToInt32(objRegShare.RegNo), 24);
                            transaction.Commit();
                            //  transaction.Complete();
                            returnMessage.Msg = "Share Register Successfully";
                            returnMessage.Success = true;
                      //  }


                       
                    }
                    catch (Exception ex)
                    {

                        transaction.Dispose();
                        returnMessage.Msg = "Share  not save.!!" + ex.Message;
                        returnMessage.Success = false;
                    }

                }

                return returnMessage;
            }

        }
        public IPagedList<ShareRegisterViewModel> SharePurchaseUnverifiedList(int pageNo, int pageSize)
        {
            ShrSPurchaseModel shareModel = new ShrSPurchaseModel();
            CustomerService custService = new CustomerService();
            var shareDetails = uow.Repository<ShrReg>().FindBy(x => x.ApprovedBy == 0).Select(x => new ShareRegisterViewModel()
            {
                RegNo=x.RegNo,
                Name= custService.GetSelectedCustomer(Convert.ToInt32(x.RegNo), TypeOfCustomer.ShareCustomer.GetDescription()).Name,
                RegistrationCode = x.RegistrationCode
              
            }).ToPagedList(pageNo, pageSize);
          
            return shareDetails;
        }
        public ShrSPurchaseModel SharePurchaseDetails(long tno)
        {
            ShrSPurchaseModel shareModel = new ShrSPurchaseModel();
            CustomerService custService = new CustomerService();
            var shareDetails = uow.Repository<ShrSPurchase>().FindBy(x => x.Tno == tno).Select(x => new ShrSPurchaseModel()
            {
                RegistrationNo = x.Regno,
                SQty = x.SQty,
                Amt = x.Amt,
                Note = x.Note,
                Pdate = x.Pdate,
                Tno = x.Tno

            }).FirstOrDefault();
            var shareRegUsere = custService.GetSelectedCustomer(Convert.ToInt32(shareDetails.RegistrationNo), TypeOfCustomer.ShareCustomer.GetDescription());
            shareDetails.Name = shareRegUsere.Name;
            return shareDetails;
        }
        public IPagedList<ShrSPurchaseModel> UnverifiedPurchase(int pageNo, int pageSize)
        {
            CustomerService custService = new CustomerService();
            var shareDetails = uow.Repository<ShrSPurchase>().GetAll().Select(x => new ShrSPurchaseModel()
            {
                RegistrationNo = x.Regno,
                SQty = x.SQty,
                Amt = x.Amt,
                Note = x.Note,
                Pdate = x.Pdate,
                Tno = x.Tno,
                Name = custService.GetSelectedCustomer(Convert.ToInt32(x.Regno), TypeOfCustomer.ShareCustomer.GetDescription()).Name

            }).ToPagedList(pageNo, pageSize);
            return shareDetails;
        }
        public ReturnBaseMessageModel SharePurchaseSave(ShrSPurchaseModel sharePurchaseModel, DenoInOutViewModel denoInOutModel, TaskVerificationList taskVerifier)
        {
            using (var transaction = uow.GetContext().Database.BeginTransaction())
            {

                try
                {
                    bool IsTrnsWithDeno = commonService.IsTransactionWithDeno();
                    if (sharePurchaseModel.Regno == 0)
                    {
                        returnMessage.Msg = "Please Select Register Account!!";
                        returnMessage.Success = false;
                        return returnMessage;
                    }
                    if (sharePurchaseModel.SQty == 0)
                    {
                        returnMessage.Msg = "share Quantity is Required!!";
                        returnMessage.Success = false;
                        return returnMessage;
                    }

                    decimal shareRate = ShareSetupRate();
                    decimal totalShare = shareRate * sharePurchaseModel.SQty;
                    if (totalShare != sharePurchaseModel.Amt)
                    {
                        returnMessage.Msg = "Invalid share purchase amount!!";
                        returnMessage.Success = false;
                        return returnMessage;
                    }
                    if (IsTrnsWithDeno)
                    {
                        if (!TellerUtilityService.BalanceWithDenoAmount(denoInOutModel, sharePurchaseModel.Amt))
                        {
                            returnMessage.Msg = "Amount deosnot match with deno balance.!!";
                            returnMessage.Success = false;
                            return returnMessage;
                        }
                        returnMessage = TellerUtilityService.AvailableDenoNumber(denoInOutModel.DenoOutList);
                        if (!returnMessage.Success)
                        {
                            return returnMessage;
                        }

                    }
                    Int64 transactionNumber = 0;
                    ShrSPurchase objSharePurchaseRow = uow.Repository<ShrSPurchase>().FindBy(x => x.Tno == sharePurchaseModel.Tno).FirstOrDefault();
                    if (objSharePurchaseRow == null)
                    {
                        objSharePurchaseRow = new ShrSPurchase();

                    }
                    if (sharePurchaseModel.Note == null)
                    {
                        sharePurchaseModel.Note = "Being share purchased";
                    }
                    transactionNumber = commonService.GetUtno();
                    objSharePurchaseRow.Note = sharePurchaseModel.Note;
                    objSharePurchaseRow.Brchid = Convert.ToByte(commonService.GetBranchIdByEmployeeUserId());
                    objSharePurchaseRow.Pdate = commonService.GetBranchTransactionDate();
                    objSharePurchaseRow.PostedBy = Global.UserId;
                    objSharePurchaseRow.Regno = sharePurchaseModel.Regno;
                    objSharePurchaseRow.SQty = sharePurchaseModel.SQty;
                    objSharePurchaseRow.SType = sharePurchaseModel.SType;
                    objSharePurchaseRow.Tno = transactionNumber;
                    objSharePurchaseRow.ttype = 1;
                    objSharePurchaseRow.Amt = sharePurchaseModel.Amt;
                    if (IsTrnsWithDeno)
                    {
                        commonService.DenoInOutTransaction(denoInOutModel, transactionNumber);
                    }


                    uow.Repository<ShrSPurchase>().Add(objSharePurchaseRow);
                    uow.Commit();
                    taskUow.SaveTaskNotification(taskVerifier, transactionNumber, 25);
                    commonService.SaveUpdateMyBalance(0, commonService.DefultCurrency(), sharePurchaseModel.Amt, Global.UserId);
                    transaction.Commit();
                    returnMessage.Msg = "Share purchase save Successfully";
                    returnMessage.Success = true;
                }
                catch (Exception ex)
                {

                    transaction.Dispose();
                    returnMessage.Msg = "Share  not save.!!" + ex.Message;
                    returnMessage.Success = false;
                }

            }

            return returnMessage;
        }

        public decimal ShareSetupRate()
        {
            decimal shareRate = uow.Repository<ShrSetup>().GetAll().Select(x => x.ShrRate).FirstOrDefault();
            return shareRate;
        }

        public ReturnBaseMessageModel VerifyShareRegistration(long eventValue)
        {
            var shareRegRow = uow.Repository<ShrReg>().GetSingle(x => x.RegNo == eventValue);
            shareRegRow.ApprovedBy = Global.UserId;
            uow.Repository<ShrReg>().Edit(shareRegRow);
            uow.Commit();
            returnMessage.Msg = "Share Register verify Successfully";
            returnMessage.Success = true;
            return returnMessage;
        }
        public ReturnBaseMessageModel DeleteUnVerifiedShareRegistration(long Regno)
        {

            try
            { 
                int isSuccess = uow.ExecWithStoreProcedure("[Fin].[PDelUnverifiedShareReg] @RegNo" , new SqlParameter("@RegNo", Regno));
                returnMessage.Msg = "Share Registered Successfully Deleted";
                returnMessage.Success = true;
                return returnMessage;
            }
            catch (Exception ex)
            {

                returnMessage.Success = false;
                returnMessage.Msg = " Cannot Be Deleted!!" + ex.Message;
                return returnMessage;
            }
        }
        //public ReturnBaseMessageModel DeleteUnVerifiedSharePurchase(long tno)
        //{

        //    try
        //    {
        //        int isSuccess = uow.ExecWithStoreProcedure("[Fin].[PDelUnverifiedSharePurchase] @tno ", new SqlParameter("@tno", tno));
        //        returnMessage.Msg = "Share Unverified Purchase Successfully Deleted";
        //        returnMessage.Success = true;
        //        return returnMessage;
        //    }
        //    catch (Exception ex)
        //    {

        //        returnMessage.Success = false;
        //        returnMessage.Msg = " Cannot Be Deleted!!";
        //        return returnMessage;
        //    }
        //}

        public ReturnBaseMessageModel VerifySharePurchase(long tno)
        {

            int approveBy = Global.UserId;
            int count = uow.ExecWithStoreProcedure("[fin].[Share_Entry] " + tno + "," + approveBy + "");
            if (count > 0)
            {
                returnMessage.Success = true;
                returnMessage.Msg = "Share Purchase verified successfully with Transaction number #" + tno;
                return returnMessage;
            }
            else
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Payment not verified.!!";
                return returnMessage;
            }
        }

    
         public List<ShareCustomerDetailsViewModel> ShareCustomerList(int employeeId)
        {           
            int branch = commonService.GetBranchIdByEmployeeUserId();;
            var customerList = uow.Repository<ShareCustomerDetailsViewModel>().SqlQuery("select * from fin.[FGETSHARERETURNDISPLAY](" + employeeId + "," + branch + ")").ToList();
            return customerList;
        }
        public ReturnBaseMessageModel ShareReturnSave(ShareReturnViewModel shareReturn, DenoInOutViewModel denoInOutModel, TaskVerificationList taskVerifier)
        {
            //using (TransactionScope transaction = new TransactionScope(
            //     // a new transaction will always be created
            //     TransactionScopeOption.RequiresNew,
            //     // we will allow volatile data to be read during transaction
            //     new TransactionOptions()
            //     {
            //         IsolationLevel = IsolationLevel.ReadUncommitted,

            //     }
            // ))

            using (var transaction = uow.GetContext().Database.BeginTransaction())
            {

                {
                    try
                    {
                        bool IsTrnsWithDeno = commonService.IsTransactionWithDeno();
                        decimal shareRate = ShareSetupRate();
                        decimal totalShare = shareRate * shareReturn.Qty;
                        if (totalShare != shareReturn.Amount)
                        {
                            returnMessage.Msg = "Invalid share Return amount!!";
                            returnMessage.Success = false;
                            return returnMessage;
                        }
                        if (IsTrnsWithDeno)
                        {
                            if (!TellerUtilityService.BalanceWithDenoAmount(denoInOutModel, shareReturn.Amount))
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
                        ShrSReturn shr = new ShrSReturn();
                        shr.Regno = shareReturn.RegNo;
                        shr.SCrtNo = shareReturn.Scrtno;
                        shr.SoldTo = null;
                        shr.Tno = commonService.GetUtno();
                        shr.Sdate = commonService.GetBranchTransactionDate();
                        shr.SQty = shareReturn.Qty;
                        shr.Amt = totalShare;
                        shr.Note = shareReturn.Note;
                        shr.PostedBy = Loader.Models.Global.UserId;
                        shr.ttype = 1;
                        shr.SType = shareReturn.SType;
                        shr.Brchid = Convert.ToByte(commonService.GetBranchIdByEmployeeUserId());
                        uow.Repository<ShrSReturn>().Add(shr);
                        uow.Commit();
                        if (IsTrnsWithDeno)
                        {
                            commonService.DenoInOutTransaction(denoInOutModel, (long)shr.Tno);
                        }
                        commonService.SaveUpdateMyBalance(0, commonService.DefultCurrency(), totalShare * -1, Global.UserId);

                        taskUow.SaveTaskNotification(taskVerifier, Convert.ToInt32(shr.Tno), 26);
                        transaction.Commit();
                        returnMessage.Msg = "Share Return Saved Successfully";
                        returnMessage.Success = true;
                    }
                    catch (Exception ex)
                    {

                        transaction.Dispose();
                        returnMessage.Msg = "Share Return Not Saved.!!" + ex.Message;
                        returnMessage.Success = false;
                    }

                }

                return returnMessage;
            }
        }

        public List<ShareCustomerDetailsViewModel> ShareReturnUnverifiedIPageList(int accState, int pageNumber, int pageSize)
        {
            string query = "SELECT COUNT(*) OVER () AS TotalCount,RegNo,Scrtno,	Tno,Qty,Registrationcode,Name,ShareType from [fin].[FGetShareReturnVerifyList] ()";
            query += @" ORDER BY  Name
                       OFFSET ((" + pageNumber + @" - 1) * " + pageSize + @") ROWS
                       FETCH NEXT " + pageSize + " ROWS ONLY";
            //var account = uow.Repository<ADetail>().SqlQuery(query).ToList();
            var shareList = uow.Repository<ShareCustomerDetailsViewModel>().SqlQuery(query).ToList();
            return shareList;
        }
        public ShareCustomerDetailsViewModel ShareReturnUnverifiedDetails(int tno)
        {

            var unverifieddetails = uow.Repository<ShareCustomerDetailsViewModel>().SqlQuery("select * from Fin.FGetShareUnverifiedDetails(" + tno + ")").FirstOrDefault();
            if (unverifieddetails == null)
            {
                unverifieddetails = uow.Repository<ShareCustomerDetailsViewModel>().SqlQuery("select * from Fin.FGetShareVerifiedDetails(" + tno + ")").FirstOrDefault();
            }
            return unverifieddetails;
        }

        public ShareCustomerDetailsViewModel ShareReturnUnverifiedDetailDisplay(int tno)
        {

            var unverifieddetails = uow.Repository<ShareCustomerDetailsViewModel>().SqlQuery("select * from Fin.FGetShareUnverifiedDetailsDisplay(" + tno + ")").FirstOrDefault();
           
            return unverifieddetails;
        }


        //public ReturnBaseMessageModel DeleteUnVerifiedShareReturn(long tno)
        //{

        //    try
        //    {
        //        int isSuccess = uow.ExecWithStoreProcedure("[Fin].[PDelUnverifiedShareReturn] @tno )", new SqlParameter("@tno", tno));
        //        returnMessage.Msg = "Share Unverified Purchase Successfully Deleted";
        //        returnMessage.Success = true;
        //        return returnMessage;
        //    }
        //    catch (Exception ex)
        //    {

        //        returnMessage.Success = false;
        //        returnMessage.Msg = " Cannot Be Deleted!!";
        //        return returnMessage;
        //    }
        //}
        public ReturnBaseMessageModel VerifyReturnShare(int tno)
        {
            try
            {
                int UserId = Loader.Models.Global.UserId;
                int isSuccess = _context.Database.ExecuteSqlCommand("exec fin.PSetShareReturnVerify @TNO,@ApprovedBy", new SqlParameter("@TNO", tno), new SqlParameter("@ApprovedBy", UserId));
                returnMessage.Msg = "Successfully Saved";
                returnMessage.Success = true;
                return returnMessage;
            }
            catch (Exception ex)
            {

                returnMessage.Success = false;
                returnMessage.Msg = " Not saved.!!" + ex.Message;
                return returnMessage;
            }

        }

        public  ShrSPurchase GetService(string disburseId)
        {
            var shareService = uow.Repository<ShrSPurchase>().FindBy(x => x.ReferenceId == disburseId).SingleOrDefault();
            return shareService;
        }
    }
}

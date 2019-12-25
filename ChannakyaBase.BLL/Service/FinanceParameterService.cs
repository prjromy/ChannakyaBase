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
using Loader.Models;
using System.Transactions;
using System.Web.Mvc;
using MoreLinq;

namespace ChannakyaBase.BLL.Service
{
    public class FinanceParameterService
    {
        ReturnBaseMessageModel returnMessage = null;
        private ChannakyaBaseEntities _context = null;
        private GenericUnitOfWork uow = null;
        BaseTaskVerificationService taskUow = null;
        private CommonService commonService = null;
        private Loader.Repository.GenericUnitOfWork luow = null;

        public FinanceParameterService()
        {
            commonService = new CommonService();

            uow = new GenericUnitOfWork();
            returnMessage = new ReturnBaseMessageModel();
            _context = new ChannakyaBaseEntities();
            luow = new Loader.Repository.GenericUnitOfWork();
        }

        public List<ChannakyaBase.DAL.DatabaseModel.LicenseBranch> GetAllBranch()
        {
            var allbranch = uow.Repository<ChannakyaBase.DAL.DatabaseModel.LicenseBranch>().GetAll().ToList();
            return allbranch;
        }
        public ChannakyaBase.DAL.DatabaseModel.LicenseBranch GetSingleBranchById(int BrnhID)
        {
            var branch = uow.Repository<ChannakyaBase.DAL.DatabaseModel.LicenseBranch>().GetSingle(x => x.BrnhID == BrnhID);
            return branch;
        }
        public ReturnBaseMessageModel SaveBranch(ChannakyaBase.DAL.DatabaseModel.LicenseBranch licenseBranch)
        {
            DAL.DatabaseModel.Company company = new DAL.DatabaseModel.Company();
            ProductBrnch productBranch = new ProductBrnch();
            string query = "select FYID from lg.FiscalYears where '" + licenseBranch.MigDate + "' between StartDt and EndDt";
            //Loader.Models.FiscalYear lb2 = luow.Repository<Loader.Models.FiscalYear>().SqlQuery(query).FirstOrDefault();
            Loader.Models.FiscalYear lb2 = luow.Repository<Loader.Models.FiscalYear>().GetSingle(x => x.StartDt < licenseBranch.MigDate && x.EndDt > licenseBranch.MigDate);

            licenseBranch.FYID = lb2.FYID;
            licenseBranch.atclosing = false; //it will be altered from other place ... "just put in view dnt update it", Bikash Sir
            licenseBranch.TDate = Convert.ToDateTime(licenseBranch.TDate.ToShortDateString());
            licenseBranch.MigDate = Convert.ToDateTime(licenseBranch.MigDate.Value.ToShortDateString());
            //licenseBranch.ExtraUsrNo

            if (licenseBranch.BrnhID == 0)
            {
                company.BranchName = licenseBranch.BrnhNam;
                company.PhoneNo = licenseBranch.Phone;
                company.Address = licenseBranch.Addr;
                company.Email = licenseBranch.Email;
                company.FaxNo = licenseBranch.Fax;
                company.IPAddress = licenseBranch.IPAdd;
                company.TDate = licenseBranch.TDate;
                company.State = licenseBranch.State;
                company.Prefix = licenseBranch.Prefix;
                company.AdditionalUser = Convert.ToInt32(licenseBranch.ExtraUsrNo);
                company.IsBranch = true;
                company.AdditionalUser = Convert.ToInt32(licenseBranch.ExtraUsrNo);
                company.ParentId = Convert.ToInt32(licenseBranch.PBrnhID);
                company.Region = Convert.ToString(licenseBranch.RegID);

                //company.Region = FinanceParameterUtilityService.GetRegionByRegionId(Convert.ToInt32(licenseBranch.RegID));
            }
            else
            {
                company = uow.Repository<DAL.DatabaseModel.Company>().GetAll().Where(x => x.CompanyId == licenseBranch.BrnhID).FirstOrDefault();
                company.BranchName = licenseBranch.BrnhNam;
                company.PhoneNo = licenseBranch.Phone;
                company.Address = licenseBranch.Addr;
                company.Email = licenseBranch.Email;
                company.FaxNo = licenseBranch.Fax;
                company.IPAddress = licenseBranch.IPAdd;
                company.TDate = licenseBranch.TDate;
                company.State = licenseBranch.State;
                company.Prefix = licenseBranch.Prefix;
                company.IsBranch = true;
                company.AdditionalUser = Convert.ToInt32(licenseBranch.ExtraUsrNo);
                company.ParentId = Convert.ToInt32(licenseBranch.PBrnhID);
                company.Region = Convert.ToString(licenseBranch.RegID);
            }

            if (licenseBranch.BrnhID == 0 && company.CompanyId == 0)
            {
                uow.Repository<ChannakyaBase.DAL.DatabaseModel.LicenseBranch>().Add(licenseBranch);
                uow.Repository<ChannakyaBase.DAL.DatabaseModel.Company>().Add(company);

                uow.Commit();

                ProductBranchMap();

                returnMessage.Msg = "License Branch Saved Successfully";
                returnMessage.Success = true;
                return returnMessage;
            }
            else
            {
                uow.Repository<ChannakyaBase.DAL.DatabaseModel.LicenseBranch>().Edit(licenseBranch);
                uow.Repository<ChannakyaBase.DAL.DatabaseModel.Company>().Edit(company);

                uow.Commit();

                returnMessage.Msg = "License Branch Edited Successfully";
                returnMessage.Success = true;
                return returnMessage;
            }
        }

        public void ProductBranchMap()
        {
            DAL.DatabaseModel.Company branch = uow.Repository<DAL.DatabaseModel.Company>().GetAll().LastOrDefault();
            IEnumerable<ProductBrnch> productBranch = uow.Repository<ProductBrnch>().GetAll().DistinctBy(x => x.PID);
            foreach (var item in productBranch)
            {
                ProductBrnch productBranchNew = new ProductBrnch();
                productBranchNew.BrnchID = branch.CompanyId;
                productBranchNew.PID = item.PID;
                productBranchNew.StartAcNo = item.StartAcNo;
                productBranchNew.LastAcNo = item.StartAcNo;
                productBranchNew.ProductDetail = productBranchNew.ProductDetail;
                uow.Repository<ProductBrnch>().Add(productBranchNew);
            }
            uow.Commit();
        }

        public void BranchDelete(ChannakyaBase.DAL.DatabaseModel.LicenseBranch Branch)
        {
            DAL.DatabaseModel.Company company = new DAL.DatabaseModel.Company();
            company = uow.Repository<DAL.DatabaseModel.Company>().GetAll().Where(x => x.CompanyId == Branch.BrnhID).FirstOrDefault();

            uow.Repository<ChannakyaBase.DAL.DatabaseModel.Company>().Delete(company);
            uow.Repository<ChannakyaBase.DAL.DatabaseModel.LicenseBranch>().Delete(Branch);
            uow.Commit();


        }
        public List<ProductTID> GetAllProductFloatingInterest()
        {
            var productFloatingIneterst = uow.Repository<ProductTID>().GetAll().ToList();
            return productFloatingIneterst;
        }

        public List<ProductTID> GetAllProductFloatingInterestSelect(int PID)
        {
            if (PID == 0)
            {
                return uow.Repository<ProductTID>().GetAll().ToList();
            }
            else
                return uow.Repository<ProductTID>().GetAll().Where(x => x.PID == PID).ToList();
        }

        public ProductTID GetSingleProductFloatingInterestById(int PFIID)
        {
            var productFloatingIneterst = uow.Repository<ProductTID>().GetSingle(x => x.PFIID == PFIID);
            return productFloatingIneterst;
        }

        public List<ProductTID> GetSingleProduct(int PID)
        {
            var productFloatingIneterst = uow.Repository<ProductTID>().FindBy(x => x.PID == PID).ToList();
            return productFloatingIneterst;
        }


        public ReturnBaseMessageModel SaveProductFloatingInterest(ProductTID productFloatingIneterst)
        {
            int UserId = Global.UserId;
            DateTime currentDate = DateTime.Now;

            var getAllProduct = GetSingleProduct(productFloatingIneterst.PID);
            if (getAllProduct.Count > 1)
            {
                getAllProduct.ForEach(x => x.Flag = false);
            }
            if (productFloatingIneterst.PFIID == 0 || productFloatingIneterst.PFIID != 0)
            {

                //// var getAllProduct = GetSingleProduct(productFloatingIneterst.PID);
                // if (productFloatingIneterst.PID == getAllProduct.PID && productFloatingIneterst.TID != getAllProduct.TID )
                // {
                //     bool flagSet = true;
                //     productFloatingIneterst.Flag = flagSet;

                // }
                // else
                // {
                //     bool flagSet = false;
                //     productFloatingIneterst.Flag = flagSet;
                // }

                productFloatingIneterst.Flag = true;
                productFloatingIneterst.UpdatedBy = UserId;
                productFloatingIneterst.UpdatedOn = currentDate;

                uow.Repository<ProductTID>().Add(productFloatingIneterst);
                returnMessage.Msg = "Product Floating Interest Added Successfully";
                returnMessage.Success = true;
            }
            //else
            //{
            //    var productfloatinginterest = GetSingleProductFloatingInterestById(productFloatingIneterst.PFIID);
            //    if (productfloatinginterest != null)
            //    {
            //        //bool flagSet = false;
            //        productfloatinginterest.UpdatedOn = currentDate;
            //        productfloatinginterest.UpdatedBy = UserId;
            //        //productFloatingIneterst.Flag = flagSet;
            //        productfloatinginterest.PID= productFloatingIneterst.PID;
            //        productfloatinginterest.TID = productFloatingIneterst.TID;
            //        uow.Repository<ProductTID>().Edit(productfloatinginterest);
            //        returnMessage.Msg = "Product Floating Interest Edited Successfully";
            //        returnMessage.Success = true;
            //    }


            //    else
            //    {
            //        returnMessage.Msg = "Edit Unsuccessful";
            //        returnMessage.Success = false;
            //    }
            //}
            uow.Commit();
            return returnMessage;
        }
        //public int CheckProductFloatingInterest(int? TID)
        //{
        //    int count = uow.Repository<...>().FindBy(x => x.TID == TID).Count();
        //    return count;

        //}
        public bool CheckExists(int PID, int TID, int PFIID = 0)
        {
            int count = uow.Repository<ProductTID>().GetAll().Where(x => x.PID == PID && x.TID == TID).Where(x => x.PFIID != PFIID).Count();

            if (count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckMap(int CID, int FID)
        {
            int count = uow.Repository<RemittanceCustomer>().GetAll().Where(x => x.CID == CID || x.CID == CID && x.FID == FID || x.CID == CID && x.FID != FID).Count();

            if (count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool CheckBranchNameExists(string BrnhNam, int? BrnhID)
        {
            int count = uow.Repository<DAL.DatabaseModel.LicenseBranch>().GetAll().Where(x => x.BrnhNam.ToLower().Trim() == BrnhNam.ToLower().Trim() && x.BrnhID != BrnhID).Count();
            //int count = uow.Repository<DAL.DatabaseModel.LicenseBranch>().FindBy(x => x.BrnhNam.Trim().ToLower() == BrnhNam.Trim() && x.BrnhID != BrnhID).Count();
            if (count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public void ProductFloatingInterestDelete(ProductTID productTID)
        {
            uow.Repository<ProductTID>().Delete(productTID);
            uow.Commit();


        }
        public List<ChargeDetail> GetAllChargeSetup()
        {
            var chargeDetail = uow.Repository<ChargeDetail>().GetAll().ToList();
            foreach (var item in chargeDetail)
            {
                item.Product = ProductNameByChargeId(item.ChgID);
            }
            return chargeDetail;
        }

        public ChargeDetail GetSingleChargeSetupById(int ChgID)
        {
            var chargeDetail = uow.Repository<ChargeDetail>().GetSingle(x => x.ChgID == ChgID);
            return chargeDetail;
        }
        public ReturnBaseMessageModel SaveChargeSetup(ChargeDetail chargeDetail, List<ProductViewModel> productList)
        {
            string productIds = "";
            if (chargeDetail.Modules != 3) //module share condition
            {
                foreach (var item in productList)
                {

                    if (item.HasDuration == true)
                    {
                        if (productIds.Length > 0)
                        {
                            productIds += ",";
                        }
                        productIds += item.ProductId;
                    }
                }
            }

            //productIds = "," + productIds;
            if (chargeDetail.ChgID == 0)
            {
                if (chargeDetail.Modules != 3) //module share condition
                {
                    chargeDetail.Product = productIds;
                }
                uow.Repository<ChargeDetail>().Add(chargeDetail);
                returnMessage.Msg = "Charge Added Successfully";
                returnMessage.Success = true;

            }
            else
            {

                ChargeDetail chg = new ChargeDetail();
                chg = chargeDetail;
                if (chargeDetail.ChrType == 1)
                {
                    chg.Ratio = null;
                }
                else
                    chg.CAmount = null;
                if (chargeDetail.Modules != 3) //module share condition
                {
                    chg.Product = productIds;
                }
                uow.Repository<ChargeDetail>().Edit(chg);
                returnMessage.Msg = "charge Edited Successfully";
                returnMessage.Success = true;

            }
            uow.Commit();
            return returnMessage;
        }

        public bool CheckMap(int chgID)
        {
            var getChgIdFromChgSTranx = uow.Repository<ChgSTrnx>().FindBy(x => x.ChgId == chgID).Count();
            var getChgIdFromChgmTranx = uow.Repository<ChgMTrnx>().FindBy(x => x.ChgId == chgID).Count();

            if (getChgIdFromChgSTranx >= 1 || getChgIdFromChgmTranx >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ChargeSetupDelete(ChargeDetail chargeDetail)
        {
            uow.Repository<ChargeDetail>().Delete(chargeDetail);
            uow.Commit();


        }
        public string ProductNameByChargeId(int chargeId)
        {
            string productName = uow.Repository<string>().SqlQuery(@"select Product from fin.FGetProductNameByChargeId(" + chargeId + ")").FirstOrDefault();
            return productName;
        }
        public List<ChargeDetail> GetChargeDetailsByProductId(int productId, int modevent)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                List<ChargeDetail> charges = new List<ChargeDetail>();
                var chargeByPId = uow.Repository<ChargeTransactionViewModel>().SqlQuery(@"select distinct ChgID from FGetChargeVsProductTB() where Product=" + productId).ToList();
                foreach (var item in chargeByPId)
                {
                    var chargedetails = uow.Repository<ChargeDetail>().FindBy(x => x.ChgID == item.ChgID).Where(x => x.ModEveID == modevent && x.Status == true).FirstOrDefault();
                    if (chargedetails != null)
                    {
                        charges.Add(chargedetails);
                    }
                }
                //var chargeByPIds = uow.Repository<ChargeDetail>().FindBy(x => (x.Product == productId || x.Product == 0) && (x.ModEveID == 4 && x.Status == true)).ToList();
                return charges.ToList();
            }
        }

        public List<ChargeDetail> GetChargeDetailsAndAmountChargeByProductId(int productId, int modevent, decimal SanctionAmount, bool? checkSanction, decimal RegularLoan, int accountNumber)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                List<ChargeDetail> charges = new List<ChargeDetail>();

                var chargeByPId = uow.Repository<ChargeTransactionViewModel>().SqlQuery(@"select distinct ChgID from FGetChargeVsProductTB() where Product=" + productId).ToList();
                decimal ChargedAmount;

                ChannakyaBase.DAL.DatabaseModel.DisbursementMain checkSanctinStatus = new DisbursementMain();
                checkSanctinStatus = uow.Repository<ChannakyaBase.DAL.DatabaseModel.DisbursementMain>().FindBy(x => x.IAccno == accountNumber).FirstOrDefault();

                //   decimal totalchargesum=0;
                if (checkSanctinStatus == null)
                {
                    if (checkSanction == false)
                    {
                        SanctionAmount = RegularLoan;
                    }

                    foreach (var item in chargeByPId)
                    {
                        var chargedetails = uow.Repository<ChargeDetail>().FindBy(x => x.ChgID == item.ChgID).Where(x => x.ModEveID == modevent && x.Status == true).FirstOrDefault();

                        if (chargedetails != null)
                        {


                            if (checkSanction == true)
                            {
                                if (chargedetails.Ratio != null)
                                {
                                    ChargedAmount = SanctionAmount * (decimal)(chargedetails.Ratio / 100);
                                    chargedetails.AmountChargedd = ChargedAmount;
                                }
                            }

                            if (checkSanction == false)
                            {
                                if (chargedetails.Ratio != null)
                                {
                                    ChargedAmount = RegularLoan * (decimal)(chargedetails.Ratio / 100);
                                    chargedetails.AmountChargedd = ChargedAmount;
                                }
                            }
                        }

                        if (chargedetails != null)
                        {
                            charges.Add(chargedetails);
                        }
                    }


                }



                else
                {
                    checkSanction = checkSanctinStatus.CheckSanction;
                    foreach (var item in chargeByPId)
                    {
                        var chargedetails = uow.Repository<ChargeDetail>().FindBy(x => x.ChgID == item.ChgID).Where(x => x.ModEveID == modevent && x.Status == true).FirstOrDefault();

                        if (chargedetails != null)
                        {

                            if (checkSanctinStatus.CheckSanction == false)
                            {
                                if (checkSanction == true)
                                {
                                    if (chargedetails.Ratio != null)
                                    {
                                        ChargedAmount = SanctionAmount * (decimal)(chargedetails.Ratio / 100);
                                        chargedetails.AmountChargedd = ChargedAmount;
                                    }
                                }

                                if (checkSanction == false)
                                {
                                    if (chargedetails.Ratio != null)
                                    {
                                        ChargedAmount = RegularLoan * (decimal)(chargedetails.Ratio / 100);
                                        chargedetails.AmountChargedd = ChargedAmount;
                                    }
                                }
                            }


                        }

                        if (chargedetails != null)
                        {
                            charges.Add(chargedetails);
                        }


                    }
                }








                return charges.ToList();
            }
        }

        public List<CashLimitModel> getSingleCashLimitData(int UserId = 0)
        {
           int branchId = commonService.GetBranchIdByEmployeeUserId();
            string sqlQuery = "SELECT * FROM fin.FGetCashLimitSList(" + branchId + ")" ;
            if (UserId != 0)
            {
                sqlQuery += "where stfid=" + UserId; 
            }
            var tellerCashlimit = uow.Repository<CashLimitModel>().SqlQuery(sqlQuery).ToList();
            return tellerCashlimit;
        }

        public List<ChargeDetail> GetChargeDetailsByProductIdForAccountOpen(int productId, int modevent)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                List<ChargeDetail> charges = new List<ChargeDetail>();

                var chargeByPId = uow.Repository<ChargeTransactionViewModel>().SqlQuery(@"select distinct ChgID from FGetChargeVsProductTB() where Product=" + productId).ToList();
                // decimal ChargedAmount;

                foreach (var item in chargeByPId)
                {
                    var chargedetails = uow.Repository<ChargeDetail>().FindBy(x => x.ChgID == item.ChgID).Where(x => x.ModEveID == modevent && x.Status == true).FirstOrDefault();
                    if (chargedetails != null)
                    {
                        charges.Add(chargedetails);
                    }
                }

                return charges.ToList();
            }
        }







        public bool ChargeNameExist(string ChgName, int? ChgID)
        {
            //int count = uow.Repository<DAL.DatabaseModel.LicenseBranch>().GetAll().Where(x => x.BrnhNam.ToLower() == BrnhNam.ToLower() && x.BrnhID != BrnhID).Count();
            int count = uow.Repository<DAL.DatabaseModel.ChargeDetail>().FindBy(x => x.ChgName.Trim().ToLower() == ChgName.Trim() && x.ChgID != ChgID).Count();
            if (count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public void ChargeUnverifiedTransaction(CommonService comService, List<ChargeDetail> chargeDetails, int iaccno, TaskVerificationList TaskVerifierList, byte ttype = 0)
        {

            try
            {
                if (chargeDetails != null)
                {
                    foreach (var item in chargeDetails)
                    {
                        if((item.AmountChargedd!=0 && item.CAmount ==null)||(item.AmountChargedd ==null && item.CAmount != 0))
                        {
                            DAL.DatabaseModel.ChgSTrnx chargetrnx = new DAL.DatabaseModel.ChgSTrnx();
                            if (item.AmountChargedd == 0)
                            {
                                chargetrnx.Amt = Convert.ToDecimal(item.CAmount);
                            }
                            else
                            {
                                chargetrnx.Amt = Convert.ToDecimal(item.AmountChargedd);
                            }
                            chargetrnx.ChgId = item.ChgID;
                            chargetrnx.Postedby = Convert.ToInt16(Global.UserId);
                            chargetrnx.Remarks = "";
                            //chargetrnx.Amt = Convert.ToDecimal(item.CAmount);
                            chargetrnx.brhid = commonService.GetBranchIdByEmployeeUserId();
                            chargetrnx.TrnxNo = comService.GetUtno();
                            chargetrnx.Tdate = commonService.GetBranchTransactionDate();
                            chargetrnx.Iaccno = iaccno;
                            chargetrnx.ttype = ttype;
                            uow.Repository<ChgSTrnx>().Add(chargetrnx);
                            if (item.Triggertype == 2)
                            {
                                taskUow = new BaseTaskVerificationService();
                                taskUow.SaveTaskNotification(TaskVerifierList, chargetrnx.TrnxNo, 7);
                            }
                        }
                        
                    }
                    uow.Commit();
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }


        }
        public ReturnBaseMessageModel ManualChargeUnverifiedTransaction(ChargeTransactionViewModel chargeDetails, TaskVerificationList TaskVerifierList, DenoInOutViewModel denoInOutModel)
        {
            using (var transaction = uow.GetContext().Database.BeginTransaction())
            ////using (TransactionScope transaction = new TransactionScope(
            ////    // a new transaction will always be created
            ////    TransactionScopeOption.RequiresNew,
            ////    // we will allow volatile data to be read during transaction
            ////    new TransactionOptions()
            ////    {
            ////        IsolationLevel = IsolationLevel.ReadUncommitted,

            ////    }
            ////))
            {
                try
                {

                    DAL.DatabaseModel.ChgSTrnx chargetrnx = new DAL.DatabaseModel.ChgSTrnx();
                    chargeDetails.ChrType = uow.Repository<ChargeDetail>().FindBy(x => x.ChgID == chargeDetails.ChgID).Select(x => x.ChrType).FirstOrDefault();
                    chargetrnx.ChgId = chargeDetails.ChgID;
                    chargetrnx.Postedby = Convert.ToInt16(Loader.Models.Global.UserId);
                    chargetrnx.Remarks = chargeDetails.Remarks;
                    if (chargeDetails.ChrType == 1)
                    {
                        chargetrnx.Amt = Convert.ToDecimal(chargeDetails.CAmount);
                    }
                    else
                    {
                        chargetrnx.Amt = Convert.ToDecimal(chargeDetails.CAmount * (Convert.ToDecimal(chargeDetails.Ratio / 100)));
                    }

                    //chargetrnx.Amt = Convert.ToDecimal(chargeDetails.CAmount);
                    chargetrnx.brhid = commonService.GetBranchIdByEmployeeUserId(); ;
                    chargetrnx.TrnxNo = commonService.GetUtno();
                    chargetrnx.Tdate = commonService.GetBranchTransactionDate();
                    if (chargeDetails.Modules == 1)
                    {
                        chargetrnx.Iaccno = chargeDetails.Iaccno;
                    }
                    else if (chargeDetails.Modules == 2)
                    {
                        chargetrnx.Iaccno = chargeDetails.LIaccno;
                    }
                    else
                    {
                        chargetrnx.Iaccno = chargeDetails.SIaccno;
                    }

                    if ((chargeDetails.Iaccno == 0 || chargeDetails.Iaccno == null) && (chargeDetails.LIaccno == 0 || chargeDetails.LIaccno == null) && (chargeDetails.SIaccno == null || chargeDetails.SIaccno == 0))
                    {
                        chargetrnx.ttype = 3;
                    }

                    else

                    {
                        int Iaccno = Convert.ToInt32(chargeDetails.Iaccno);
                        int SIaccno = Convert.ToInt32(chargeDetails.SIaccno);
                        int LIaccno = Convert.ToInt32(chargeDetails.LIaccno);
                        int userBranchId = commonService.GetBranchIdByEmployeeUserId();


                        if ((chargeDetails.Iaccno != 0 || chargeDetails.Iaccno != null) && ((chargeDetails.LIaccno == null ||
                            chargeDetails.LIaccno == 0) && (chargeDetails.SIaccno == null || chargeDetails.SIaccno == 0)))

                        {
                            ADetail accountDetail = uow.Repository<ADetail>().FindBy(x => x.IAccno == Iaccno).FirstOrDefault();
                            bool isFixedAccount = TellerUtilityService.IsFixedAccount(accountDetail.PID);
                            bool isRecurring = TellerUtilityService.IsRecurringDeposit(accountDetail.PID);
                            if (isFixedAccount || isRecurring)
                            {
                                bool isMature = TellerUtilityService.IsAlreadyMatured(Iaccno);
                                if (accountDetail.AccState != 7)
                                {
                                    if (!isMature)
                                    {
                                        returnMessage.Msg = "Withdraw is not Allowed.! \n Account NOT Matured Yet..!!";
                                        returnMessage.Success = false;
                                        return returnMessage;
                                    }
                                }
                            }

                            decimal availableBalance = TellerUtilityService.AvailableBalance(Iaccno);
                            if (availableBalance < chargetrnx.Amt)
                            {
                                returnMessage.Msg = "Withdraw NOT Allowed.! \n Amount is greater than Good balance.";
                                returnMessage.Success = false;
                                return returnMessage;
                            }
                        }

                        if (chargeDetails.CashRelated == 1)// dont change mybal but change account bal
                        {
                            int AccountBranchId = 0;
                            if ((chargeDetails.Iaccno != 0 || chargeDetails.Iaccno != null) && ((chargeDetails.LIaccno == null ||
                            chargeDetails.LIaccno == 0) && (chargeDetails.SIaccno == null || chargeDetails.SIaccno == 0)))
                            {
                                AccountBranchId = commonService.AccountHolderBranchId(Iaccno);
                                DateTime accountBranchDate = commonService.GetOtherBranchTransactionDateByBranchId(AccountBranchId);
                            }
                            if ((chargeDetails.SIaccno != 0 || chargeDetails.SIaccno != null) && ((chargeDetails.LIaccno == null ||
                            chargeDetails.LIaccno == 0) && (chargeDetails.Iaccno == null || chargeDetails.Iaccno == 0)))
                            {
                                AccountBranchId = commonService.AccountHolderBranchId(SIaccno);
                                DateTime accountBranchDate = commonService.GetOtherBranchTransactionDateByBranchId(AccountBranchId);
                            }
                            if ((chargeDetails.LIaccno != 0 || chargeDetails.LIaccno != null) && ((chargeDetails.SIaccno == null ||
                            chargeDetails.SIaccno == 0) && (chargeDetails.Iaccno == null || chargeDetails.Iaccno == 0)))
                            {
                                AccountBranchId = commonService.AccountHolderBranchId(LIaccno);
                                DateTime accountBranchDate = commonService.GetOtherBranchTransactionDateByBranchId(AccountBranchId);
                            }

                            DateTime userBranchDate = commonService.GetBranchTransactionDate();
                            if (userBranchId == AccountBranchId)
                                chargetrnx.ttype = 1;
                            else
                                chargetrnx.ttype = 2;


                            //  decimal shadowDr = 0;
                            //ABal availableForInsert = null;
                            //ABal availableBal = uow.Repository<ABal>().GetSingle(x => x.IAccno == chargeDetails.Iaccno && x.Flag == 1);//flag 1->shawdowdr
                            //if (availableBal == null)
                            //{
                            //    availableBal = new ABal();
                            //  availableForInsert = new ABal();
                            //}
                            //else
                            //{
                            //  shadowDr = availableBal.Bal;
                            //    availableForInsert = availableBal;
                            //}
                            //availableBal.IAccno = Iaccno;
                            //availableBal.Flag = 1;
                            //availableBal.FId = commonService.GetFidByIAccno(Iaccno);

                            if (chargeDetails.Modules == 1)
                            {
                                commonService.InsertAvailableBalance(1, Convert.ToInt32(chargeDetails.Iaccno), Convert.ToDecimal(chargeDetails.CAmount));
                            }
                            //if (availableForInsert == null)
                            //{
                            //    uow.Repository<ABal>().Add(availableBal);
                            //}
                            //else
                            //{
                            //    uow.Repository<ABal>().Edit(availableBal);
                            //}
                        }
                        else //cash related 
                        {
                            //add charge to mybal but do not change account bal
                            chargetrnx.ttype = 5;



                        }
                    }
                    if ((chargeDetails.Iaccno == 0) || (chargeDetails.SIaccno == 0) || (chargeDetails.LIaccno == 0) || (chargeDetails.Iaccno != 0 && chargeDetails.CashRelated == 0) || (chargeDetails.LIaccno != 0 && chargeDetails.CashRelated == 0) || (chargeDetails.SIaccno != 0 && chargeDetails.CashRelated == 0))
                    {
                        bool IsTrnsWithDeno = commonService.IsTransactionWithDeno();
                        if (IsTrnsWithDeno)
                        {
                            if (!TellerUtilityService.BalanceWithDenoAmount(denoInOutModel, chargetrnx.Amt))
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

                        if (IsTrnsWithDeno)
                        {
                            commonService.DenoInOutTransaction(denoInOutModel, chargetrnx.TrnxNo);
                        }
                        commonService.SaveUpdateMyBalance(0, commonService.DefultCurrency(), chargetrnx.Amt, Global.UserId);
                    }



                    // ChgSTrnx getLatestSlipNo = uow.Repository<ChgSTrnx>().SqlQuery(@"select top 1 * from Trans.ChgSTrnx order by slpno DESC").FirstOrDefault();
                    //.slpno = chargeDetails.slpno;
                    //if (getLatestSlipNo != null)
                    //{

                    //}
                    //ChgSTrnx getLatestSlipNo = uow.Repository<ChgSTrnx>().SqlQuery(@"select top 1 * from Trans.ChgSTrnx order by slpno DESC").FirstOrDefault();
                    //Decimal getLatestSlip = uow.Repository<Decimal>().SqlQuery("select top 1 * from Trans.ChgSTrnx order by slpno DESC").FirstOrDefault();
                    //ChgSTrnx getLatestSlipNo = uow.Repository<ChgSTrnx>().FindBy(x => x.slpno).OrderByDescending(x => x.slpno).FirstOrDefault(); ;

                    // chargetrnx.slpno = getLatestSlipNo.slpno + 1;
                    chargetrnx.slpno = chargeDetails.slpno;
                    uow.Repository<ChgSTrnx>().Add(chargetrnx);
                    taskUow = new BaseTaskVerificationService();

                    taskUow.SaveTaskNotification(TaskVerifierList, chargetrnx.TrnxNo, 8);
                    uow.Commit();
                    returnMessage.Msg = "Charge created Successfully";
                    returnMessage.Success = true;
                }

                catch (Exception ex)
                {
                    transaction.Dispose();
                    returnMessage.Msg = "Charge not saved.!!" + ex.Message;
                    returnMessage.Success = false;
                }

                return returnMessage;
            }
        }



        public bool DenoInDenoTrnxCheck(int denoID)
        {
            int count = uow.Repository<DenoTrxn>().FindBy(x => x.Denoid == denoID).Count();
            if (count >= 1)
            {

                return false;


            }
            else
            {
                return true;
            }
        }

        public bool DenoAlreadyExistCheck(short currID, double Deno)
        {
            int count = uow.Repository<Denosetup>().FindBy(x => x.Deno == Deno && x.CurrID == currID).Count();
            if (count >= 1)
            {

                return false;


            }
            else
            {
                return true;
            }
        }

        //public List<Denosetup> GetAllDenosetup(int CurrID)
        //{
        //    if (CurrID == 0)
        //    {
        //        return uow.Repository<Denosetup>().GetAll().ToList();
        //    }
        //    else
        //        return uow.Repository<Denosetup>().GetAll().Where(x => x.CurrID == CurrID).ToList();
        //}

        public List<DenoInViewModel> GetAllDenoSetup(int CurrID)
        {
            List<DenoInViewModel> denoAllList = new List<DenoInViewModel>();

            if (CurrID == 0)
            {
                denoAllList = (from d in uow.Repository<Denosetup>().GetAll().OrderBy(d => d.Deno)
                               join c in uow.Repository<CurrencyType>().GetAll() on d.CurrID equals c.CTId
                               select new DenoInViewModel()
                               {
                                   Currency = c.CurrencyName,
                                   Deno = d.Deno,
                                   DenoID = d.DenoID,



                               }
                             ).ToList();
            }
            else
            {
                denoAllList = (from d in uow.Repository<Denosetup>().GetAll().OrderBy(d => d.Deno).Where(x => x.CurrID == CurrID)
                               join c in uow.Repository<CurrencyType>().GetAll() on d.CurrID equals c.CTId
                               select new DenoInViewModel()
                               {
                                   Currency = c.CurrencyName,
                                   Deno = d.Deno,
                                   DenoID = d.DenoID,



                               }
                                ).ToList();

            }


            return denoAllList;
        }





        public DenoInViewModel EmployeeInfoDenoSetupId(int DenoID)
        {
            var getDenoIn = uow.Repository<DenoInViewModel>().SqlQuery(@"select u.DenoID,u.Deno,e.CurrencyName from fin.[Denosetup] u join
                        fin.CurrencyType e on e.CTId=u.CurrID where u.DenoID=" + DenoID).FirstOrDefault();
            return getDenoIn;
        }

        public ReturnBaseMessageModel SaveDenoSetup(DenoInViewModel denosetup)
        {
            try
            {
                int count = 0;



                var singleDeno = uow.Repository<Denosetup>().FindBy(x => x.DenoID == denosetup.DenoID).FirstOrDefault();
                if (singleDeno == null)
                {
                    count = uow.Repository<Denosetup>().FindBy(x => x.Deno == denosetup.Deno && x.CurrID == denosetup.CurrID).Count();
                    if (count >= 1)
                    {


                        returnMessage.Success = false;
                        returnMessage.Msg = "Deno Already Exists";
                        return returnMessage;

                    }
                    singleDeno = new Denosetup();
                }
                singleDeno.CurrID = denosetup.CurrID;


                singleDeno.Deno = denosetup.Deno;


                if (denosetup.DenoID == 0)
                {

                    uow.Repository<Denosetup>().Add(singleDeno);
                    returnMessage.Success = true;
                    returnMessage.Msg = "Deno Saved successfully";
                }
                else
                {

                    uow.Repository<Denosetup>().Edit(singleDeno);
                    returnMessage.Success = true;
                    returnMessage.Msg = "Deno Edited successfully";
                }


                uow.Commit();

                return returnMessage;

            }
            catch (Exception ex)
            {

                returnMessage.Success = false;
                returnMessage.Msg = "not save" + ex.Message;
                return returnMessage;
            }
        }

        public List<DenoInViewModel> GetAllCurrval(int CurrID)
        {
            List<DenoInViewModel> denoAllList = new List<DenoInViewModel>();
            if (CurrID == 0)
            {
                denoAllList = (from d in uow.Repository<Denosetup>().GetAll().OrderBy(d => d.Deno)
                               join c in uow.Repository<CurrencyType>().GetAll() on d.CurrID equals c.CTId
                               select new DenoInViewModel()
                               {
                                   Currency = c.CurrencyName,
                                   Deno = d.Deno,
                                   DenoID = d.DenoID,

                               }
                              ).ToList();

            }
            else
            {

                denoAllList = (from d in uow.Repository<Denosetup>().GetAll().OrderBy(d => d.Deno).Where(x => x.CurrID == CurrID)
                               join c in uow.Repository<CurrencyType>().GetAll() on d.CurrID equals c.CTId
                               select new DenoInViewModel()
                               {
                                   Currency = c.CurrencyName,
                                   Deno = d.Deno,
                                   DenoID = d.DenoID,

                               }
                                    ).ToList();
            }
            return denoAllList;
        }



        public DenoInViewModel GetSingleDenoSetupById(int DenoID)
        {


            return (from d in uow.Repository<Denosetup>().FindBy(x => x.DenoID == DenoID)
                    join c in uow.Repository<CurrencyType>().GetAll() on d.CurrID equals c.CTId
                    select new DenoInViewModel()

                    {

                        DenoID = d.DenoID,
                        Currency = c.CurrencyName,
                        Deno = d.Deno,
                        CurrID = d.CurrID


                    }).FirstOrDefault();
        }
        public byte IsDeposit(int ChargeId)
        {
            var isDeposit = uow.Repository<ChargeDetail>().FindBy(x => x.ChgID == ChargeId).Select(x => x.Modules).SingleOrDefault();
            return isDeposit;
        }
        public ChargeTransactionViewModel GetSingleUnverifiedChargeTransaction(Int64 utno)
        {
            ChargeTransactionViewModel unverifiedCharge = new ChargeTransactionViewModel();
            var manualCharge = uow.Repository<ChgSTrnx>().GetSingle(x => x.TrnxNo == utno);
            var isDeposit = uow.Repository<ChargeDetail>().FindBy(x => x.ChgID == manualCharge.ChgId).Select(x => x.Modules).SingleOrDefault();
            if (manualCharge != null)
            {
                if (manualCharge.Iaccno != null && manualCharge.Iaccno != null && isDeposit == 1)
                {
                    unverifiedCharge = (from a in uow.Repository<ChgSTrnx>().GetAll()
                                        join ac in uow.Repository<ADetail>().GetAll() on a.Iaccno equals ac.IAccno
                                        join c in uow.Repository<ChargeDetail>().GetAll() on a.ChgId equals c.ChgID
                                        where a.TrnxNo == utno
                                        select new ChargeTransactionViewModel()
                                        {
                                            ChgID = a.ChgId,
                                            Amt = a.Amt,
                                            TrnxNo = a.TrnxNo,
                                            Remarks = a.Remarks,
                                            Postedby = a.Postedby,
                                            Approvedby = a.Approvedby,
                                            Iaccno = a.Iaccno,
                                            Vno = a.Vno,
                                            ttype = a.ttype,
                                            slpno = a.slpno,
                                            brhid = a.brhid,
                                            FID = c.FID,
                                            ChgName = c.ChgName,
                                            accountStatus = ac.AccState
                                        }
                              ).FirstOrDefault();
                    //   return unverifiedCharge;
                }
                else
                {
                    unverifiedCharge = (from a in uow.Repository<ChgSTrnx>().GetAll()
                                            //join ac in uow.Repository<ADetail>().GetAll() on a.Iaccno equals ac.IAccno
                                        join c in uow.Repository<ChargeDetail>().GetAll() on a.ChgId equals c.ChgID
                                        where a.TrnxNo == utno
                                        select new ChargeTransactionViewModel()
                                        {
                                            ChgID = a.ChgId,
                                            Amt = a.Amt,
                                            TrnxNo = a.TrnxNo,
                                            Remarks = a.Remarks,
                                            Postedby = a.Postedby,
                                            Approvedby = a.Approvedby,
                                            Iaccno = a.Iaccno,
                                            Vno = a.Vno,
                                            ttype = a.ttype,
                                            slpno = a.slpno,
                                            brhid = a.brhid,
                                            FID = c.FID,
                                            ChgName = c.ChgName
                                            //accountStatus = ac.AccState
                                        }
                                ).FirstOrDefault();
                }

            }
            if (manualCharge == null)
            {
                unverifiedCharge = (from a in uow.Repository<ChgMTrnx>().GetAll()
                                        //join ac in uow.Repository<ADetail>().GetAll() on a.Iaccno equals ac.IAccno
                                    join c in uow.Repository<ChargeDetail>().GetAll() on a.ChgId equals c.ChgID
                                    where a.TrnxNo == utno
                                    select new ChargeTransactionViewModel()
                                    {
                                        ChgID = a.ChgId,
                                        Amt = a.Amt,
                                        TrnxNo = a.TrnxNo,
                                        Remarks = a.Remarks,
                                        Postedby = a.Postedby,
                                        Approvedby = a.Approvedby,
                                        Iaccno = a.Iaccno,
                                        Vno = a.Vno,
                                        ttype = a.ttype,
                                        slpno = a.slpno,
                                        brhid = a.brhid,
                                        FID = c.FID,
                                        ChgName = c.ChgName
                                    }
                               ).FirstOrDefault();
                //  return verifiedCharge;
            }
            return unverifiedCharge;

        }

        public int VerifyChargeTransaction(Int64 tNo)
        {
            string UserId = Loader.Models.Global.UserId.ToString();
            int isSuccess = _context.Database.ExecuteSqlCommand("exec [Trans].[PSetChargeVerify] @TNO,@APPROVEDBY", new SqlParameter("@TNO", tNo), new SqlParameter("@APPROVEDBY", UserId));
            return isSuccess;
        }
        public List<ChargeTransactionViewModel> ChargeTranxNoByEventIdAndEventValue(Int64 eventId, Int64 eventValue)
        {
            var chargeList = uow.Repository<ChargeTransactionViewModel>().SqlQuery(@"select TrnxNo from Trans.ChgSTrnx a  inner join fin.chargedetail b on a.ChgId=b.ChgID where Iaccno=" + eventValue + " and Triggertype =" + 1 + " and ModEveID =" + eventId).ToList();
            return chargeList;
        }
        public List<RemittanceCustomer> GetAllRemittanceCustomer(int CID)
        {

            if (CID == 0)
            {
                return uow.Repository<RemittanceCustomer>().GetAll().ToList();
            }
            else
                return uow.Repository<RemittanceCustomer>().GetAll().Where(x => x.CID == CID).ToList();
        }
        public RemittanceCustomer GetSingleRemittanceCustomerById(int RID)
        {
            var remittanceCustomer = uow.Repository<RemittanceCustomer>().FindBy(x => x.RID == RID).SingleOrDefault();
            return remittanceCustomer;
        }
        public ReturnBaseMessageModel SaveRemittanceCustomer(RemittanceCustomer remittanceCustomer)
        {


            if (remittanceCustomer.RID == 0)
            {

                uow.Repository<RemittanceCustomer>().Add(remittanceCustomer);
                returnMessage.Msg = " Saved Successfully";
                returnMessage.Success = true;
            }
            else
            {
                var remittanceCustomerList = GetSingleRemittanceCustomerById(remittanceCustomer.RID);
                if (remittanceCustomerList != null)
                {
                    remittanceCustomerList.FID = remittanceCustomer.FID;
                    remittanceCustomerList.CID = remittanceCustomer.CID;

                    uow.Repository<RemittanceCustomer>().Edit(remittanceCustomerList);
                    returnMessage.Msg = "Edited Successfully";
                    returnMessage.Success = true;
                }
                else
                {
                    returnMessage.Msg = "Edited Unsuccessful";
                    returnMessage.Success = false;
                }

            }

            uow.Commit();
            return returnMessage;
        }
        public bool RemittanceCustomerDelete(int RID)
        {

            var remitDeposit = uow.Repository<RemitDeposit>().GetSingle(x => x.RemitCompanyId == RID);
            var remitPayment = uow.Repository<RemitPayment>().GetSingle(x => x.RemitCompanyId == RID);
            var result = true;
            if (remitDeposit != null || remitPayment != null)
            {
                //isExists = true;
                result = false;

            }

            //var remitPayment = uow.Repository<RemitPayment>().GetSingle(x => x.RemitCompanyId == remittanceCustomer.RID);
            //if (remitPayment != null )
            //{
            //    isExists = true;
            //    returnMessage.Msg = "Remittance Customer cannot be deleted";
            //}

            ////if (isExists==false)
            ////{
            ////    uow.Repository<RemittanceCustomer>().Delete(remittanceCustomer);
            ////    returnMessage.Msg = "Remittance Customer deleted successfully";
            ////}
            else
            {
                // returnMessage.Msg = "Remittance Customer cannot be deleted";
                //uow.Repository<RemittanceCustomer>().Delete(remittanceCustomer);
                //returnMessage.Msg = "Remittance Customer deleted successfully";
                result = true;

            }

            return result;

        }

        public void RemittanceCustomerDeleteConfirm(RemittanceCustomer remittanceCustomer)
        {
            uow.Repository<RemittanceCustomer>().Delete(remittanceCustomer);
            uow.Commit();

        }

        public ReturnBaseMessageModel InsertUpdateBranchEmployeeMap(EmployeeBranchMapModel beModel)
        {
            try
            {
                //foreach (var item in beModel.EmployeeBranchList)
                //{
                var empRow = uow.Repository<EmployeeVsBranch>().GetSingle(x => x.MapId == beModel.MapId);
                if (empRow == null)
                {
                    empRow = new EmployeeVsBranch();
                }
                empRow.PostedBy = Loader.Models.Global.UserId;
                empRow.PostedOn = commonService.GetDate();
                empRow.StartFrom = beModel.StartFrom;
                empRow.BranchId = beModel.EmpBranchId;
                empRow.EmpId = beModel.EmpId;
                if (beModel.IsCurrentRole == false)
                {
                    empRow.CurrentRole = beModel.CurrentRole;
                }
                else
                {
                    var CurrentRole = uow.Repository<EmployeeVsBranch>().FindBy(x => x.EmpId == beModel.EmpId).OrderByDescending(x => x.StartFrom).Select(x => x.CurrentRole).FirstOrDefault();
                    if (CurrentRole == 0)
                    {
                        var current = uow.Repository<EmployeeBranchMapModel>().SqlQuery(@"select RoleId as CurrentRole from [LG].[Employees] e
                                                                                 join lg.role r on e.DGId=r.DGId
                                                                                 where EmployeeId=" + beModel.EmpId).FirstOrDefault();
                        empRow.CurrentRole = current.CurrentRole;
                    }
                    else
                    {
                        empRow.CurrentRole = CurrentRole;
                    }

                }

                if (beModel.MapId == 0)
                {
                    uow.Repository<EmployeeVsBranch>().Add(empRow);
                    returnMessage.Msg = "Employee branch update successfully";
                }
                else
                {
                    uow.Repository<EmployeeVsBranch>().Edit(empRow);
                    returnMessage.Msg = "Employee branch save successfully";
                }
                //}
                uow.Commit();
                returnMessage.Success = true;

                return returnMessage;
            }
            catch (Exception ex)
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Not Save" + ex.Message;
                return returnMessage;

            }

        }

        public List<EmployeeViewModel> BranchEmployeeList(string employeeName, int branchId, int pageNo, int pageSize)
        {

            string query = "select COUNT(*) OVER () AS TotalCount, * from [fin].[BranchEmployeeList]()";
            if (employeeName != "" && branchId != 0)
            {
                query += " where EmployeeName like'%" + employeeName + "%' and BranchId=" + branchId;
            }
            else if (employeeName != "" && branchId == 0)
            {
                query += "where EmployeeName like'%" + employeeName + "%'";
            }
            else if (employeeName == "" && branchId != 0)
            {
                query += "where BranchId=" + branchId;
            }
            query += @" ORDER BY  StartFrom desc
                       OFFSET ((" + pageNo + @" - 1) * " + pageSize + @") ROWS
                       FETCH NEXT " + pageSize + " ROWS ONLY";
            var EmplaoyeeList = uow.Repository<EmployeeViewModel>().SqlQuery(query).ToList();

            return EmplaoyeeList;
        }

        public EmployeeBranchMapModel GetSingleBranchEmployee(int mapId)
        {
            var singleBranchEmployee = uow.Repository<ChannakyaBase.DAL.DatabaseModel.EmployeeVsBranch>().FindBy(x => x.MapId == mapId).Select(x => new EmployeeBranchMapModel
            {
                EmpBranchId = x.BranchId,
                EmpId = x.EmpId,
                CurrentRole = x.CurrentRole,
                MapId = x.MapId,
                StartFrom = x.StartFrom,

            }).FirstOrDefault();


            return singleBranchEmployee;
        }

        #region Teller cash Limit
        public List<CashLimitModel> GetUserTellerCashLimitList(int employeeId)
        {
            int branchId = commonService.GetBranchIdByEmployeeUserId();
            string sqlQuery = "SELECT * FROM fin.FGetCashLimitSList(" + branchId + ")";
            if (employeeId != 0)
            {
                sqlQuery += "where emp.EmployeeId=" + employeeId;
            }
            var tellerCashlimit = uow.Repository<CashLimitModel>().SqlQuery(sqlQuery).ToList();
            return tellerCashlimit;
        }

        public CashLimitModel GetSingleCashLimit(int cashLimitId)
        {
            return uow.Repository<CashLimit>().FindBy(x => x.cashLimitId == cashLimitId).Select(x => new CashLimitModel()
            {
                cashLimitId = x.cashLimitId,
                cramt = x.cramt,
                dramt = x.dramt,
                stfid = x.stfid
            }).FirstOrDefault();

        }

        public ReturnBaseMessageModel InsertUpdateCashLimit(CashLimitModel cashLimitModel)
        {
            try
            {
                int userId = Global.UserId;
                int branchId = commonService.GetBranchIdByEmployeeUserId();
                var cRow = uow.Repository<CashLimit>().GetSingle(x => x.cashLimitId == cashLimitModel.cashLimitId || (x.BranchId == branchId && x.stfid == cashLimitModel.stfid));
                if (cRow == null)
                {
                    cRow = new CashLimit();

                    cRow.stfid = cashLimitModel.stfid;
                    cRow.dramt = cashLimitModel.dramt;
                    cRow.cramt = cashLimitModel.cramt;
                    cRow.BranchId = branchId;

                    //if (cashLimitModel.cashLimitId == 0)
                    //{
                    uow.Repository<CashLimit>().Add(cRow);
                    //}
                }
                else
                {
                    ////cRow = uow.Repository<CashLimit>().FindBy(x => x.cashL).Select(x => new CashLimitModel()
                    ////{
                    ////    cashLimitId = x.cashLimitId,
                    ////    cramt = x.cramt,
                    ////    dramt = x.dramt,
                    ////    stfid = x.stfid
                    ////}).FirstOrDefault();

                    cRow.dramt = cashLimitModel.dramt;
                    cRow.cramt = cashLimitModel.cramt;
                    uow.Repository<CashLimit>().Edit(cRow);
                }
                uow.Commit();
                returnMessage.Success = true;
                returnMessage.Msg = "Cash Limit Saved Successfully.!!";
                return returnMessage;
            }
            catch (Exception ex)
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Faild to save!!" + ex.Message;
                return returnMessage;
            }
        }

        public List<Loader.Models.Users> GetUserRoleByEmployeeId(int employeeId)
        {
            var usserNameByEmpId = luow.Repository<Loader.Models.Users>().SqlQuery(@"select

           UserId, UserName + '(' + DGName + ')' as UserName  from  [fin].[FGetAllUsersWithDesignation]() where EmployeeId=" + employeeId + "").ToList();
            return usserNameByEmpId;
        }


        #endregion


        #region ChequeInventorySetup 


        public ReturnBaseMessageModel saveChequeInventorySetup(ChequeInventorySetupModel chequeInventorySetup)
        {

            try
            {
                var singleChequeInventorySetup = uow.Repository<ChqInventory>().GetSingle(x => x.chqInvId == chequeInventorySetup.ChequeInventorySetupId);

                if (singleChequeInventorySetup == null)
                {
                    singleChequeInventorySetup = new ChqInventory();
                }

                if (chequeInventorySetup.ChequeInventorySetupId == 0)
                {
                    var chqInventoryAll = uow.Repository<ChqInventory>().GetAll();
                    //long? isFinishCheck = chqInventoryAll.ISfinish;
                    //decimal? balance = 0;
                    if (chqInventoryAll.Count() >= 0)
                    {
                        //balance = chqInventoryAll.Tochqno - chqInventoryAll.Lastindx;
                        //returnMessage.Msg = balance + "no of cheque is still available ";
                        //return returnMessage;

                        //if (chequeInventorySetup.fromCheckNo < chqInventoryAll.FrmChqno)
                        //{
                        //    var startfrominventory = chqInventoryAll.Tochqno + 1;
                        //    returnMessage.Msg = chequeInventorySetup.fromCheckNo + "is less than " + chqInventoryAll.Lastindx + ". Start from " + startfrominventory;
                        //    return returnMessage;

                        //}

                        //chequeInventorySetupModel.fromCheckNo= uow.Repository<string>().SqlQuery(@"select top 1 Frmchqno  from fin.ChqInventory").FirstOrDefault();



                        var FirstfromChequeNo = (from che in uow.Repository<ChqInventory>().GetAll()
                                                 orderby che.chqInvId ascending
                                                 select new ChequeInventorySetupModel
                                                 {
                                                     fromCheckNo = che.FrmChqno

                                                 }).FirstOrDefault();

                        var LastToChequeNo = (from che in uow.Repository<ChqInventory>().GetAll()
                                              orderby che.chqInvId descending
                                              select new ChequeInventorySetupModel
                                              {
                                                  toCheckNo = che.Tochqno

                                              }).FirstOrDefault();


                        //var toChqnoFromDatabase = uow.Repository<ChequeInventorySetupModel>().SqlQuery(@"select top 1 Tochqno from fin.ChqInventory   order by chqInvId desc ").FirstOrDefault(;

                        //var lastIndex = uow.Repository<ChqInventory>().FindBy(x => x.Lastindx==chequeInventorySetup.Lastindx).OrderByDescending(x => x.Lastindx);





                        if (chequeInventorySetup.fromCheckNo > chequeInventorySetup.toCheckNo)
                        {


                            returnMessage.Msg = "To cheque number must be greater than from cheque number";
                            returnMessage.Success = false;
                            return returnMessage;
                        }
                        else if (chequeInventorySetup.fromCheckNo >= FirstfromChequeNo.fromCheckNo && chequeInventorySetup.fromCheckNo <= LastToChequeNo.toCheckNo)
                        {
                            returnMessage.Msg = "Entered value is within the limit";
                            returnMessage.Success = false;
                            return returnMessage;
                        }
                        else if (chequeInventorySetup.toCheckNo >= FirstfromChequeNo.fromCheckNo && chequeInventorySetup.toCheckNo <= LastToChequeNo.toCheckNo)
                        {
                            returnMessage.Msg = "Entered value is within the limit";
                            returnMessage.Success = false;
                            return returnMessage;
                        }
                    }








                    var checkBranchById = uow.Repository<ChqInventory>().FindBy(x => x.Brnhid == chequeInventorySetup.BranchId).FirstOrDefault();

                    if (checkBranchById != null)
                    {
                        if (checkBranchById.FrmChqno == checkBranchById.Tochqno)
                        {
                            singleChequeInventorySetup.ISfinish = true;
                        }

                        else
                        {
                            singleChequeInventorySetup.ISfinish = false;
                        }
                    }
                    singleChequeInventorySetup.Brnhid = Convert.ToInt16(chequeInventorySetup.BranchId);
                    singleChequeInventorySetup.chqInvId = chequeInventorySetup.ChequeInventorySetupId;
                    singleChequeInventorySetup.FrmChqno = Convert.ToDecimal(chequeInventorySetup.fromCheckNo);
                    singleChequeInventorySetup.Tochqno = Convert.ToDecimal(chequeInventorySetup.toCheckNo);


                    singleChequeInventorySetup.Lastindx = chequeInventorySetup.fromCheckNo;



                    singleChequeInventorySetup.PostedBy = Global.UserId;
                    singleChequeInventorySetup.PostedOn = commonService.GetDate();
                    uow.Repository<ChqInventory>().Add(singleChequeInventorySetup);
                }
                //3rd day             




                uow.Commit();
                returnMessage.Msg = "Cheque Inventory Saved successfully";
                returnMessage.Success = true;
                return returnMessage;
            }

            catch (Exception ex)
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Not Save" + ex.Message;
                return returnMessage;
            }

        }

        public decimal? GetFromChequeByBranch(int BranchID)
        {

            var getAllFromChequeByBranch = uow.Repository<ChqInventory>().GetAll();
            decimal? fromChqno;
            if (getAllFromChequeByBranch.Count() == 0)
            {


                fromChqno = 0;
            }
            else
            {

                fromChqno = getAllFromChequeByBranch.LastOrDefault().Tochqno + 1;
            }
            return fromChqno;

        }

        public List<ChequeInventorySetupModel> GetAllChequeInventory()
        {
            var chequeInventoryList = (from chqinventory in uow.Repository<ChqInventory>().GetAll()
                                       join branch in uow.Repository<DAL.DatabaseModel.LicenseBranch>().GetAll() on chqinventory.Brnhid equals (short)branch.BrnhID
                                       select new ChequeInventorySetupModel()
                                       {
                                           BranchId = branch.BrnhID,
                                           BranchName = branch.BrnhNam,
                                           ChequeInventorySetupId = chqinventory.chqInvId,
                                           fromCheckNo = chqinventory.FrmChqno,
                                           toCheckNo = chqinventory.Tochqno,
                                           Lastindx = chqinventory.Lastindx,
                                           ISfinish = chqinventory.ISfinish,
                                           PostedBy = chqinventory.PostedBy,
                                           PostedOn = chqinventory.PostedOn,
                                           Balance = chqinventory.Tochqno - chqinventory.Lastindx

                                       }

                                  ).ToList();

            return chequeInventoryList;
        }
        #endregion
    }
}

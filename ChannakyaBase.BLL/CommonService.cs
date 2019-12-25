using ChannakyaBase.BLL.Repository;
using ChannakyaBase.DAL.DatabaseModel;
using Loader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ChannakyaBase.Model.ViewModel;
using System.Data.SqlClient;
using System.Data;
using ChannakyaBase.Model.Models;
using Loader.Service;
using ChannakyaCustomeDatePicker.Service;
using ChannakyaCustomeDatePicker.Models;



namespace ChannakyaBase.BLL
{

    public class CommonService
    {
        private GenericUnitOfWork uow = null;

        // ChannakyaBaseEntities _context = null;
        private Loader.Repository.GenericUnitOfWork low = null;

        public CommonService()
        {
            uow = new GenericUnitOfWork();
            low = new Loader.Repository.GenericUnitOfWork();
            // _context = new ChannakyaBaseEntities();
        }

        public string RegionIdToName(int RegID)
        {
            var getName = uow.Repository<LicenseRegion>().GetSingle(x => x.RegID == RegID);
            // return getName;
            if (getName != null)
            {
                return getName.RegionNam;
            }
            else
                return "";
        }

        public string BranchIdToName(int BrnhID)
        {
            var getName = uow.Repository<DAL.DatabaseModel.LicenseBranch>().GetSingle(x => x.BrnhID == BrnhID);
            // return getName;
            if (getName != null)
            {
                return getName.BrnhNam;
            }
            else
                return "";
        }

        public List<DAL.DatabaseModel.Role> GetRoleList()
        {
            var designation = uow.Repository<DAL.DatabaseModel.Role>().SqlQuery("SELECT RoleId,Name,MTId,DGId FROM LG.Role").ToList();
            return designation;
        }

        public string GetEmployeeName(int empId)
        {
            EmployeeService empService = new EmployeeService();
            var employeeName = empService.GetSingle(empId);
            if (employeeName != null)
                return employeeName.EmployeeName;
            else
                return "";
        }
        public string PosterNamebyID(int userId)
        {
            var getEmpolyee = uow.Repository<EmployeeViewModel>().SqlQuery(@"select e.EmployeeId,e.EmployeeName from lg.[User] u join
                        lg.Employees e on e.EmployeeId=u.EmployeeId
                        where u.UserId=" + userId).FirstOrDefault();
            if (getEmpolyee!=null)
                return getEmpolyee.EmployeeName;
            else
                return "Super User";
        }
        public string ParentBranchNamebyID(int BrnhID)
        {
            var parentBranchName = "";
            var getParentBranch = low.Repository<Loader.Models.LicenseBranch>().FindBy(x => x.BrnhID == BrnhID).Select(x => x.PBrnhID).SingleOrDefault();
            if (getParentBranch == null)
            {
                 parentBranchName = low.Repository<Loader.Models.LicenseBranch>().FindBy(x => x.BrnhID == BrnhID).Select(x => x.BrnhNam).SingleOrDefault();

            }
            else
            {
                parentBranchName = low.Repository<Loader.Models.LicenseBranch>().FindBy(x => x.BrnhID == getParentBranch.Value).Select(x => x.BrnhNam).SingleOrDefault();

            }
            if (parentBranchName != null)
                return parentBranchName;
            else
                return "";
        }

        public SelectList GetDesignationRoleList()
        {
            return new SelectList(GetRoleList(), "RoleId", "Name");
        }

        public int GetBranchIdByEmployeeUserId()
        {
            return Global.BranchId;
        }
        public DateTime GetDate()
        {
            var getDate = uow.Repository<ReturnSingleValueModdel>().SqlQuery("select [mast].[GetPostedOn]() as DateValue").FirstOrDefault();
            return (DateTime)getDate.DateValue;
        }
        public int AccountHolderBranchId(int iaccNo)
        {
            var accountBranc = uow.Repository<ADetail>().FindByMany(x => x.IAccno == iaccNo).Select(x => x.BrchID);
            return Convert.ToInt32(accountBranc.FirstOrDefault());

        }
        public string GetProductNameByPid(int? PID)
        {
            string productName = uow.Repository<string>().SqlQuery(@"select Pname from fin.ProductDetail where Pid=" + PID).FirstOrDefault();
            return productName;
        }

        public AccountDetailsViewModel GetAccountNameByIaccno(int iAccno)
        {
            var account = uow.Repository<ADetail>().FindByMany(x => x.IAccno == iAccno).Select(x => new AccountDetailsViewModel()
            {
                Accno = x.Accno,
                Aname = x.Aname,
                IAccno = x.IAccno
            });
            return account.FirstOrDefault();
        }
        public string GetAccountFullNameByIaccno(int iAccno)
        {
            var Accnumber = uow.Repository<ADetail>().FindBy(x => x.IAccno == iAccno).Select(x => new AccountDetailsViewModel()
            {
                Accno = x.Accno
            }).FirstOrDefault();
            if(Accnumber==null)
            {
                return "";
            }
            return Accnumber.Accno;
        }
        public string GetCustomerNameByIaccno(int Iaccno = 0)
        {
            string CustomerName = uow.Repository<string>().SqlQuery(@"select CustName from [fin].[FGetCustomerNameByIaccno](" + Iaccno + ")").FirstOrDefault();
            return CustomerName;
        }


        public List<DisburseChequeDepositViewModel> GetAllBankDetails(int bankId = 0)
        {

            if (bankId == 0)
            {
                return uow.Repository<DisburseChequeDepositViewModel>().SqlQuery("SELECT bid as BankId,f.Fname  +' ('+ b.AccountNo+')' as BankName FROM [acc].[BankInfo] b inner join acc.FinAcc f on f.Fid=b.Fid").ToList();
            }
            else
            {
                return uow.Repository<DisburseChequeDepositViewModel>().SqlQuery("SELECT bid as BankId,f.Fname  +' ('+ b.AccountNo+')' as BankName FROM [acc].[BankInfo] b inner join acc.FinAcc f on f.Fid=b.Fid where b.bid=" + bankId).ToList();
            }

        }
        public DateTime GetBranchTransactionDate()
        {
            var transactionDate = uow.Repository<ReturnSingleValueModdel>().SqlQuery("select TDate as DateValue  from lg.Company where CompanyId=" + Global.BranchId).FirstOrDefault();
            return Convert.ToDateTime(transactionDate.DateValue);
        }
        public DateTime GetOtherBranchTransactionDateByBranchId(int branchId)
        {
            var transactionDate = uow.Repository<ReturnSingleValueModdel>().SqlQuery("select TDate as DateValue  from lg.Company where CompanyId=" + branchId).FirstOrDefault();
            return Convert.ToDateTime(transactionDate.DateValue);
        }
        public bool IsTransactionWithDeno()
        {

            var isWithDeno = uow.Repository<CurrencyRateViewModel>().SqlQuery("select CAST(PValue as bit) as IsTransWithDeno from LG.ParamValue where PId=32").FirstOrDefault();
            if (isWithDeno == null)
            {
                return false;
            }
            else
            {
                return isWithDeno.IsTransWithDeno;
            }

        }

        public bool IsShowMatureInterestOnly()
        {
            ParameterService parameterService = new ParameterService();
            var isMatureInterestOnly = uow.Repository<CommonLoanWithdrawModel>().SqlQuery("select CAST(PValue as bit) as IsBoolValue from LG.ParamValue where PId=5005").FirstOrDefault();
            return isMatureInterestOnly.IsBoolValue;
        }

        public Int64 GetUtno()
        {

            var uttno = new SqlParameter();
            uttno.ParameterName = "@TNO";

            uttno.Direction = ParameterDirection.Output;
            uttno.SqlDbType = SqlDbType.BigInt;
            var getUttno = uow.Repository<UttnoModel>().SqlQuery("exec [Mast].[GetTransno] @TNO out", uttno).FirstOrDefault();
            return getUttno.uttno;
        }

        public int GetSheetno()
        {

            var Sheet = new SqlParameter();

            var BranchId = new SqlParameter();
            var fyId = new SqlParameter();
            BranchId.ParameterName = "@BranchId";
            BranchId.SqlDbType = SqlDbType.Int;
            BranchId.SqlValue = 1;
            fyId.ParameterName = "@FyId";
            fyId.SqlDbType = SqlDbType.Int;
            fyId.SqlValue = 15;
            Sheet.ParameterName = "@SheetNo";
            Sheet.Direction = ParameterDirection.Output;
            Sheet.SqlDbType = SqlDbType.Int;
            var sheetNo = uow.Repository<CollectSheetNumber>().SqlQuery("exec [Mast].[GetSheetNumber] @SheetNo out,@BranchId,@FyId", Sheet, BranchId, fyId).FirstOrDefault();
            return sheetNo.SheetNo;
        }

        public void InsertAvailableBalance(byte flag, int iaccno, decimal amount)
        {
            try
            {

                bool availableForInsert = false;
                ABal availableBal = uow.Repository<ABal>().GetSingle(x => x.IAccno == iaccno && x.Flag == flag);
                if (availableBal == null)
                {
                    availableBal = new ABal();
                    availableForInsert = true;
                }

                int fid = GetFidByIAccno(iaccno);
                availableBal.IAccno = iaccno;
                availableBal.Flag = flag;
                availableBal.FId = fid;
                availableBal.Bal = availableBal.Bal + amount;
                if (availableForInsert)
                {
                    uow.Repository<ABal>().Add(availableBal);
                }
                else
                {
                    uow.Repository<ABal>().Edit(availableBal);
                }
                uow.Commit();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void DenoInOutTransaction(DenoInOutViewModel denoInOutModel, Int64 transactionNumber)
        {
            try
            {
                foreach (var item in denoInOutModel.DenoInList)
                {
                    DenoTrxn denoTranx = new DenoTrxn();
                    var getSingle = uow.Repository<DenoBal>().GetSingle(x => x.DenoBalId == item.DenoBalId);
                    if (getSingle == null)
                    {
                        getSingle = new DenoBal();
                    }

                    var currentNumber = uow.Repository<DenoBal>().FindBy(x => x.DenoBalId == item.DenoBalId).Select(x => x.Piece).FirstOrDefault();
                    var OutPiece = denoInOutModel.DenoOutList.Where(x => x.DenoBalIdOut == item.DenoBalId).Select(x => x.DenoNumberOut).FirstOrDefault();

                    int totalDeno = currentNumber + item.DenoNumber - OutPiece;
                    getSingle.DenoId = item.DenoID;
                    getSingle.UserId = Global.UserId;
                    getSingle.UserLevel = 2;
                    getSingle.Piece = totalDeno;

                    denoTranx.Denoid = item.DenoID;
                    denoTranx.Trxno = transactionNumber;
                    denoTranx.vdate = GetBranchTransactionDate();
                    denoTranx.Pics = item.DenoNumber;
                    denoTranx.UserId = Global.UserId;
                    if (item.DenoNumber != 0)
                    {
                        uow.Repository<DenoTrxn>().Add(denoTranx);
                    }
                    if (item.DenoBalId == null)
                    {
                        if (getSingle.Piece != 0)
                        {
                            uow.Repository<DenoBal>().Add(getSingle);
                        }
                    }
                    else
                    {
                        uow.Repository<DenoBal>().Edit(getSingle);
                    }
                }
                foreach (var item in denoInOutModel.DenoOutList)
                {

                    DenoTrxn denoTranx = new DenoTrxn();
                    denoTranx.Denoid = item.DenoIDOut;
                    denoTranx.Trxno = transactionNumber;
                    denoTranx.vdate = GetBranchTransactionDate();
                    denoTranx.Pics = (-item.DenoNumberOut);
                    denoTranx.UserId = Global.UserId;
                    if (item.DenoNumberOut != 0)
                    {
                        uow.Repository<DenoTrxn>().Add(denoTranx);
                    }
                }

                uow.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void DenoInOutTransactionDifferentUser(DenoInOutViewModel denoInOutModel, Int64 transactionNumber,int PostedBy)
        {
            try
            {
                foreach (var item in denoInOutModel.DenoInList)
                {
                    DenoTrxn denoTranx = new DenoTrxn();
                    var getSingle = uow.Repository<DenoBal>().GetSingle(x => x.DenoBalId == item.DenoBalId);
                    if (getSingle == null)
                    {
                        getSingle = new DenoBal();
                    }

                    var currentNumber = uow.Repository<DenoBal>().FindBy(x => x.DenoBalId == item.DenoBalId).Select(x => x.Piece).FirstOrDefault();
                    var OutPiece = denoInOutModel.DenoOutList.Where(x => x.DenoBalIdOut == item.DenoBalId).Select(x => x.DenoNumberOut).FirstOrDefault();

                    int totalDeno = currentNumber + item.DenoNumber - OutPiece;
                    getSingle.DenoId = item.DenoID;
                    getSingle.UserId = PostedBy;
                    getSingle.UserLevel = 2;
                    getSingle.Piece = totalDeno;

                    denoTranx.Denoid = item.DenoID;
                    denoTranx.Trxno = transactionNumber;
                    denoTranx.vdate = GetBranchTransactionDate();
                    denoTranx.Pics = item.DenoNumber;
                    denoTranx.UserId = PostedBy;
                    if (item.DenoNumber != 0)
                    {
                        uow.Repository<DenoTrxn>().Add(denoTranx);
                    }
                    if (item.DenoBalId == null)
                    {
                        if (getSingle.Piece != 0)
                        {
                            uow.Repository<DenoBal>().Add(getSingle);
                        }
                    }
                    else
                    {
                        uow.Repository<DenoBal>().Edit(getSingle);
                    }
                }
                foreach (var item in denoInOutModel.DenoOutList)
                {

                    DenoTrxn denoTranx = new DenoTrxn();
                    denoTranx.Denoid = item.DenoIDOut;
                    denoTranx.Trxno = transactionNumber;
                    denoTranx.vdate = GetBranchTransactionDate();
                    denoTranx.Pics = (-item.DenoNumberOut);
                    denoTranx.UserId = PostedBy;
                    if (item.DenoNumberOut != 0)
                    {
                        uow.Repository<DenoTrxn>().Add(denoTranx);
                    }
                }

                uow.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// this functions
        /// </summary>
        /// <param name="denoInOutModel"></param>
        /// <param name="transactionNumber"></param>
        /// <param name="postedby"></param>
        public void DenoTransactionAutoUpdateForTransactionEdit(TransactionEditViewModel transactionEditViewModel, DenoInOutViewModel denoInOutModel,Int64 transactionNumber, int postedby,bool? isDeposit,bool autoupdate)
        {
       
            bool IsTrnsWithDeno = IsTransactionWithDeno();
            if (isDeposit == true)//for deposit transaction
            {
                var getDenoFromDenoTxn = uow.Repository<DenoTrxn>().FindBy(x => x.Trxno == transactionNumber)
                              .Select(d => new DenoInViewModel()
                              {
                                  DenoID = d.Denoid,
                                  Piece = d.Pics,
                              }).ToList();

                DenoInViewModel denoInViewModel = new DenoInViewModel
                {
                    DenoInList = getDenoFromDenoTxn
                };

                DenoUnverifiedDelete(denoInViewModel, Convert.ToInt64(transactionNumber), postedby);
                foreach (var item in denoInOutModel.DenoInList)
                {
                    DenoTrxnLog DenoTrxnLog = new DenoTrxnLog();
                    if (item.DenoNumber != 0)
                    {

                        DenoTrxnLog.Trxno = Convert.ToDecimal(transactionNumber);
                        DenoTrxnLog.vdate = GetBranchTransactionDate();
                        DenoTrxnLog.Denoid = item.DenoID;
                        DenoTrxnLog.Pics = item.DenoNumber;
                        DenoTrxnLog.UserId = Convert.ToInt32(item.UserId);
                        uow.Repository<DenoTrxnLog>().Add(DenoTrxnLog);
                    }

                }
             
            }
            else
            {
                var getDenoFromDenoTxn = uow.Repository<DenoTrxn>().FindBy(x => x.Trxno == transactionNumber)//for withdraw transction
                              .Select(d => new DenoOutViewModel()
                              {
                                  DenoIDOut = d.Denoid,
                                  PieceOut = d.Pics,
                              }).ToList();

              
                DenoOutViewModel denoOutViewModel = new DenoOutViewModel
                {
                    DenoOutList = getDenoFromDenoTxn
                };


                DenoUnverifiedDeleteForDenoOut(denoOutViewModel, Convert.ToInt64(transactionNumber), postedby);
                foreach (var item in denoInOutModel.DenoOutList)
                {
                    DenoTrxnLog DenoTrxnLog = new DenoTrxnLog();
                    if (item.DenoNumberOut != 0)
                    {

                        DenoTrxnLog.Trxno = Convert.ToDecimal(transactionNumber);
                        DenoTrxnLog.vdate = GetBranchTransactionDate();
                        DenoTrxnLog.Denoid = item.DenoIDOut;
                        DenoTrxnLog.Pics = item.DenoNumberOut;
                        DenoTrxnLog.UserId = Convert.ToInt32(item.UserIdOut);
                        uow.Repository<DenoTrxnLog>().Add(DenoTrxnLog);
                    }

                }
            }
            if (IsTrnsWithDeno)
            {
                if (autoupdate == true)
                {
                    DenoInOutTransaction(denoInOutModel, Convert.ToInt64(transactionNumber));
                }
                else
                {
                    DenoInOutTransactionDifferentUser(denoInOutModel, Convert.ToInt64(transactionNumber), postedby);
                }
            }
        }
        public void DenoTransactionAfterVerifyUpdate(Int64 transactionNumber,bool withdraw)
        {
            ASTrn aSTrn = uow.Repository<ASTrn>().FindBy(x => x.tno == transactionNumber).SingleOrDefault();
            // var DenoTrxnLog = uow.Repository<DenoTrxnLog>().FindBy(x => x.Trxno == transactionNumber).ToList();
           
            //var currencyId=
            //if (currencyId == 0)
            //{
                var currencyId = DefultCurrency();
            //}
                DenoInOutViewModel denoInOutViewModel = new DenoInOutViewModel();
            int UserLevel = 2;
            var deloList = uow.Repository<DenoInViewModel>().SqlQuery("select * from fin.FGetCurrentDenoList(" + aSTrn.postedby + "," + UserLevel + "," + currencyId + ")").ToList();
            var denoOutList = deloList.Select(x => new DenoOutViewModel()
            {
                CurrIDOut = x.CurrID,
                DenoBalIdOut = x.DenoBalId,
                DenoIDOut = x.DenoID,
                DenoOut = x.Deno,
                PieceOut = x.Piece,
                UserIdOut = x.UserId,
                UserLevelOut = x.UserLevel
            }).ToList();


           // denoInOutViewModel.DenoInList = deloList;
            //denoInOutViewModel.DenoOutList = denoOutList;

            bool IsTrnsWithDeno = IsTransactionWithDeno();
            if (withdraw == true)
            {
                var getDenoFromDenoTxn = uow.Repository<DenoTrxn>().FindBy(x => x.Trxno == transactionNumber)
              .Select(d => new DenoOutViewModel()
              {
                  Tno=d.Trxno,
                  DenoIDOut = d.Denoid,
                  PieceOut = d.Pics,
                  UserIdOut=d.UserId,
                  vdate=d.vdate
              }).ToList();
                //uow.Repository<DenoTrxn>().Delete(getDenoFromDenoTxn);
                DenoOutViewModel denoOutViewModel = new DenoOutViewModel
                {
                    DenoOutList = getDenoFromDenoTxn
                };
                DenoInOutViewModel denoInOut = new DenoInOutViewModel
                {
                    DenoOutList = getDenoFromDenoTxn
                };
                DenoUnverifiedDeleteForDenoOut(denoOutViewModel, Convert.ToInt64(transactionNumber), aSTrn.postedby);
                denoInOutViewModel.DenoOutList = denoOutViewModel.DenoOutList;
                denoInOutViewModel.DenoInList = denoInOut.DenoInList;
               // denoInOutViewModel.DenoOutList= denoOutViewModel.de

            }
            else
            {
                var getDenoFromDenoTxn = uow.Repository<DenoTrxn>().FindBy(x => x.Trxno == transactionNumber)
             .Select(d => new DenoInViewModel()
             {
                 DenoID = d.Denoid,
                 Piece = d.Pics,
                 UserId=d.UserId,
                 Tno=d.Trxno,
                 vdate=d.vdate
             }).ToList();
                //uow.Repository<DenoTrxn>().Delete(getDenoFromDenoTxn);
                DenoInViewModel denoInViewModel = new DenoInViewModel
                {
                    DenoInList = getDenoFromDenoTxn
                };
                DenoInOutViewModel denoInOut = new DenoInOutViewModel
                {
                    DenoInList = getDenoFromDenoTxn
                };
                DenoUnverifiedDelete(denoInViewModel, Convert.ToInt64(transactionNumber), aSTrn.postedby);
                denoInOutViewModel.DenoInList = denoInViewModel.DenoInList;
                denoInOutViewModel.DenoOutList = denoInOut.DenoOutList;
            }
            //add new deno


            //var getDenoFromDenoTrxnLog = uow.Repository<DenoTrxnLog>().FindBy(x => x.Trxno == transactionNumber).ToList();
            ////uow.Repository<DenoTrxn>().Delete(getDenoFromDenoTxn);
            ////DenoInViewModel denoInViewModelTwo = new DenoInViewModel
            ////{
            ////    DenoInList = getDenoFromDenoTxn
            ////};
            //foreach (var item in getDenoFromDenoTrxnLog)
            //{
            //    DenoTrxnLog.Trxno = item.Trxno;
            //    DenoTrxnLog.vdate = item.vdate;
            //    DenoTrxnLog.Denoid = item.Denoid;
            //    DenoTrxnLog.Pics = item.Pics;
            //    DenoTrxnLog.UserId = item.UserId;
            //    uow.Repository<DenoTrxn>().Add(DenoTrxnLog);
            //    uow.Commit();
            //    uow.Repository<DenoTrxnLog>().Delete(item);
            //    uow.Commit();
            //}
         
            if (IsTrnsWithDeno)
            {
                DenoInOutTransaction(denoInOutViewModel, Convert.ToInt64(transactionNumber));
            }

        }
        public void DenoUnverifiedDelete(DenoInViewModel denoInOutModel, Int64 transactionNumber, int postedby)
        {
            try
            {
                foreach (var item in denoInOutModel.DenoInList)
                {
                    DenoTrxn denoTranx = new DenoTrxn();
                    var getSingle = uow.Repository<DenoBal>().GetSingle(x => x.DenoId == item.DenoID && x.UserId == postedby);
                    if (getSingle == null)
                    {
                        getSingle = new DenoBal();
                    }

                    //var currentNumber = uow.Repository<DenoBal>().FindBy(x => x.DenoBalId == item.DenoBalId).Select(x => x.Piece).FirstOrDefault();
                    //var OutPiece = denoInOutModel.DenoOutList.Where(x => x.DenoBalIdOut == item.DenoBalId).Select(x => x.DenoNumberOut).FirstOrDefault();
                    //int totalDeno = currentNumber - item.DenoNumber ;
                    getSingle.DenoId = item.DenoID;
                    getSingle.UserId = postedby;
                    getSingle.UserLevel = 2;
                    getSingle.Piece = getSingle.Piece - Convert.ToInt32(item.Piece);
                    if (getSingle.DenoBalId != 0)
                    {
                        uow.Repository<DenoBal>().Edit(getSingle);
                    }

                }
                List<DenoTrxn> denotrans = new List<DenoTrxn>();
                denotrans = uow.Repository<DenoTrxn>().FindBy(x => x.Trxno == transactionNumber).ToList();
                foreach (var item in denotrans)
                {
                    uow.Repository<DenoTrxn>().Delete(item);
                }

                uow.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void DenoUnverifiedDeleteForDenoOut(DenoOutViewModel denoInOutModel, Int64 transactionNumber, int postedby)
        {
            try
            {
                foreach (var item in denoInOutModel.DenoOutList)
                {
                    DenoTrxn denoTranx = new DenoTrxn();
                    var getSingle = uow.Repository<DenoBal>().GetSingle(x => x.DenoId == item.DenoIDOut && x.UserId == postedby);
                    if (getSingle == null)
                    {
                        getSingle = new DenoBal();
                    }

                    //var currentNumber = uow.Repository<DenoBal>().FindBy(x => x.DenoBalId == item.DenoBalId).Select(x => x.Piece).FirstOrDefault();
                    //var OutPiece = denoInOutModel.DenoOutList.Where(x => x.DenoBalIdOut == item.DenoBalId).Select(x => x.DenoNumberOut).FirstOrDefault();
                    //int totalDeno = currentNumber - item.DenoNumber ;
                    getSingle.DenoId = item.DenoIDOut;
                    getSingle.UserId = postedby;
                    getSingle.UserLevel = 2;
                    getSingle.Piece = getSingle.Piece - Convert.ToInt32(item.PieceOut);
                    if (getSingle.DenoBalId != 0)
                    {
                        uow.Repository<DenoBal>().Edit(getSingle);
                    }

                }
                List<DenoTrxn> denotrans = new List<DenoTrxn>();
                denotrans = uow.Repository<DenoTrxn>().FindBy(x => x.Trxno == transactionNumber).ToList();
                foreach (var item in denotrans)
                {
                    uow.Repository<DenoTrxn>().Delete(item);
                }

                uow.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void DenoAccept(Int64 transactionNumber, int postedby, int accept)
        {
            try
            {
                var DenoTrxn = uow.Repository<DenoTrxn>().Queryable();
                var Denosetup = uow.Repository<Denosetup>().Queryable();
                var CurrencyType = uow.Repository<CurrencyType>().Queryable();
                var denoNumberList = (from dt in DenoTrxn
                                      join d in Denosetup on dt.Denoid equals d.DenoID
                                      join c in CurrencyType on d.CurrID equals c.CTId
                                      where dt.Trxno == transactionNumber
                                      select new DenoInViewModel()
                                      {
                                          Deno = d.Deno,
                                          Piece = dt.Pics,
                                          Currency = c.CurrencyName + "(" + c.Country + ")",
                                          DenoID = d.DenoID,
                                          Id = dt.Id

                                      }).ToList();

                foreach (var item in denoNumberList)
                {
                    DenoTrxn denoTranx = new DenoTrxn();
                    var getSingle = uow.Repository<DenoBal>().GetSingle(x => x.DenoId == item.DenoID && x.UserId == postedby);
                    if (getSingle == null)
                    {
                        getSingle = new DenoBal();
                    }
                    getSingle.DenoId = item.DenoID;
                    getSingle.UserId = postedby;
                    getSingle.UserLevel = 2;
                    getSingle.Piece = getSingle.Piece - Convert.ToInt32(item.Piece);
                    if (getSingle.DenoBalId != 0)
                    {
                        uow.Repository<DenoBal>().Edit(getSingle);
                    }
                    else
                    {
                        uow.Repository<DenoBal>().Add(getSingle);
                    }

                    if (accept == 1)
                    {
                        denoTranx.Denoid = item.DenoID;
                        denoTranx.Trxno = transactionNumber;
                        denoTranx.vdate = GetBranchTransactionDate();
                        denoTranx.Pics = (int)item.Piece * -1;
                        denoTranx.UserId = Global.UserId;
                        uow.Repository<DenoTrxn>().Add(denoTranx);
                    }

                    else
                    {
                        var denoDelete = uow.Repository<DenoTrxn>().FindBy(x => x.Id == item.Id).FirstOrDefault();
                        uow.Repository<DenoTrxn>().Delete(denoDelete);
                    }

                }
                uow.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public bool isStrictlyVerifiable()
        {
            ParameterService parameterService = new ParameterService();
           // var paramValueCheck = low.Repository<Loader.Models.ParamValue>().SqlQuery("select * from LG.ParamValue where PId=2002");
            var paramValueCheck = low.Repository<Loader.Models.ParamValue>().SqlQuery("select * from LG.ParamValue where PId=2002").FirstOrDefault();
            var isVerifiable = Convert.ToBoolean(paramValueCheck.PValue);
            return Convert.ToBoolean(isVerifiable);
        }

        public int GetFidByIAccno(Int64 iAccno)
        {
            var iaccNo = uow.Repository<ReturnSingleValueModdel>().SqlQuery("select Fid as IdValue from fin.FGetFidByIAccno(" + iAccno + ")").FirstOrDefault();
            return iaccNo.IdValue;
        }

        public string DateType()
        {
            string dateType = uow.Repository<string>().SqlQuery("select PValue from LG.ParamValue where PId=36").FirstOrDefault(); //DateCommonService.DateType(); 
            return dateType;
        }

        public string GetNepaliDate(DateTime date)
        {

            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                string nepaliDate = uow.Repository<string>().SqlQuery("Select [dbo].[FGetDateBS](@EngDate)",
                    new SqlParameter("@EngDate", date)).FirstOrDefault();
                return nepaliDate;
            }
        }

        public string GetDate(DateTime date)
        {

            if (DateType() == "1")
            {
                string getenglishDate = new DatePickerService().GetFormatedDateAd(date);
                return getenglishDate;
            }
            else
            {
                DateModel getDate = new DatePickerService().GetDateBSAndAD(date);
                return getDate.DateBS;
            }
        }
        public string GetEnglishDate(DateTime date)
        {
            string getenglishDate = new DatePickerService().GetFormatedDateAd(date);
            return getenglishDate;
        }

        public DateTime GetMatDate(DateTime date, decimal month, string type)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                bool isBs = false;
                if (type == "1")
                {
                    isBs = false;
                }
                else
                {
                    isBs = true;
                }
                string sqlQuery = "Select [dbo].[FGetMatDat](@DateAD,@AddMonthDay,@IsDateBS) as DateValue";

                var nepaliDate = uow.Repository<ReturnSingleValueModdel>().SqlQuery(sqlQuery,
                new SqlParameter("@DateAD", date.ToShortDateString()),
                new SqlParameter("@AddMonthDay", month),
                new SqlParameter("@IsDateBS", isBs)
                ).FirstOrDefault();
                return Convert.ToDateTime(nepaliDate.DateValue);
            }
        }

        public int GetAccStatusByIaccno(Int64 TrnxNo = 0)
        {
            int? iaccno = 0;
            iaccno = uow.Repository<ChgSTrnx>().FindBy(x => x.TrnxNo == TrnxNo).Select(x => x.Iaccno).FirstOrDefault();
            if (iaccno != null || iaccno == 0)
            {
                int accstate = uow.Repository<ADetail>().FindBy(x => x.IAccno == iaccno).Select(x => x.AccState).FirstOrDefault();
                return accstate;
            }
            return 0;
        }

        public byte GetStypeByIaccno(int iaccno)
        {
            var stype = uow.Repository<ReturnSingleValueModdel>().SqlQuery("select SType as ByteValue from fin.ADetail a inner join fin.ProductDetail b on a.PID=b.PID inner join fin.SchmDetails s on s.SDID=b.SDID where IAccno=" + iaccno).FirstOrDefault();
            return stype.ByteValue;

        }

        public Int64 RetNoBySheetNo(int sheetNo)
        {

            var collsheet = uow.Repository<CollMainTemp>().GetSingle(x => x.SheetNo == sheetNo);
            if (collsheet != null)
                return collsheet.RetId;
            else
                return 0;
        }

        public List<AllDesignationViewModel> GetAllRecieverList(int degParamValue, int level)
        {
            var UserId = Global.UserId;
            List<AllDesignationViewModel> allReciever = new List<AllDesignationViewModel>();
            AllDesignationViewModel allRecieverModal = new AllDesignationViewModel();
            if (degParamValue == 2007 && level == 1)
            {
               allReciever = GetAllCashier(); // vault to cashier
               // allReciever = GetAllCashier().Where(x => !x.UserName.Contains(userName)).ToList();
            }
            if (degParamValue == 2007 && level == 3)
            {
                //allReciever = GetAllVault(); //vault to vault
                allReciever = GetAllVault().Where(x => !x.UserId.Equals(UserId)).ToList();
            }
            if (degParamValue == 2006 && level == 2)
            {
                allReciever = GetAllCashier(); //teller to cashier
                //allReciever = GetAllCashier().Where(x => !x.UserName.Contains(userName)).ToList();
            }
            if (degParamValue == 2006 && level == 3)
            {
               // allReciever = GetAllTeller(); //teller to teller
                allReciever = GetAllTeller().Where(x => !x.UserId.Equals(UserId)).ToList();
            }

            if (degParamValue == 2005 && level == 1)
            {
                allReciever = GetAllTeller(); //cashier to teller
                //allReciever = GetAllTeller().Where(x => !x.UserName.Contains(userName)).ToList();
            }
            if (degParamValue == 2005 && level == 2)
            {
                //allReciever = GetAllVault().Where(x => !x.UserName.Contains(userName)).ToList();

                allReciever = GetAllVault(); //cashier to vault
            }            
            if (degParamValue == 2005 && level == 3)
            {
                
                // allReciever = GetAllCashier(); //cashier to cashier
                allReciever = GetAllCashier().Where(x => !x.UserId.Equals(UserId)).ToList();
                //foreach (var item in allReciever)
                //{
                //    allRecieverModal.FullName = item.EmployeeName + "(" + item.UserName + ")";
                    
                //}
            }
           // allReciever.Add(allRecieverModal);



            return allReciever;
        }

        public List<AllDesignationViewModel> GetAllCashier(int cashierId = 0)
        {

            var branid = "SELECT * FROM[fin].[FGetAllCashierBranch](2005, " + Global.BranchId + ")";
            if (cashierId == 0)
                //return uow.Repository<AllDesignationViewModel>().SqlQuery("SELECT * FROM  [fin].[FGetAllCashierBranch](2005," + Global.BranchId + ") ").ToList();
                return uow.Repository<AllDesignationViewModel>().SqlQuery("SELECT EmployeeId,EmployeeName+'( '+UserName+' )' As EmployeeName,EmployeeNo,DGName,UserId,UserName FROM  [fin].[FGetAllCashierBranch](2005," + Global.BranchId + ") ").ToList();

            else
                //return uow.Repository<AllDesignationViewModel>().SqlQuery("SELECT * FROM [fin].[FGetAllCashier]() where UserId=" + cashierId).ToList();

                throw new NotImplementedException();
        }
        public List<AllDesignationViewModel> GetAllTeller(int tellerId = 0)
        {
            if (tellerId == 0)
                return uow.Repository<AllDesignationViewModel>().SqlQuery("SELECT EmployeeId,EmployeeName+'( '+UserName+' )' As EmployeeName,EmployeeNo,DGName,UserId,UserName FROM [fin].[FGetAllTellerBranch](2006," + Global.BranchId + ") ").ToList();
            else
                return uow.Repository<AllDesignationViewModel>().SqlQuery("SELECT * FROM [fin].[FGetAllTeller]() where UserId=" + tellerId).ToList();
                throw new NotImplementedException();

        }

        public List<AllDesignationViewModel> GetAllVault()
        {
            var allTeller = uow.Repository<AllDesignationViewModel>().SqlQuery("SELECT EmployeeId,EmployeeName+'( '+UserName+' )' As EmployeeName,EmployeeNo,DGName,UserId,UserName FROM [fin].[FGetAllVaultBranch](2007," + Global.BranchId + ")").ToList();
            return allTeller;
        }

        public List<CurrencyModel> GetMyBalanceWithCurrency()
        {
            List<CurrencyModel> allCurrency = uow.Repository<CurrencyModel>().SqlQuery("select CTId,(CurrencyCode+'('+Country+')') as CurrencyName,Amount,UserID from acc.CurrencyType c  inner join fin.MyBalance b on c.CTId =b.Currid where UserID=" + Loader.Models.Global.UserId).ToList();
            if (allCurrency.Count == 0)
            {
                allCurrency = uow.Repository<CurrencyModel>().SqlQuery("select CTId,(CurrencyCode+'('+Country+')') as CurrencyName  from acc.CurrencyType where CTId=" + DefultCurrency()).ToList();
            }
            return allCurrency;
        }

        public EmployeeViewModel EmployeeInfoUserId(int userId)
        {
            var getEmpolyee = uow.Repository<EmployeeViewModel>().SqlQuery(@"select e.EmployeeId,e.EmployeeName from lg.[User] u join
                        lg.Employees e on e.EmployeeId=u.EmployeeId
                        where u.UserId=" + userId).FirstOrDefault();
            if (getEmpolyee == null)
            {
                getEmpolyee = new EmployeeViewModel();
            }
            return getEmpolyee;
        }

        public EmployeeViewModel EmployeeUserInfoEmployeeId(int employeeId)
        {
            var getEmpolyee = uow.Repository<EmployeeViewModel>().SqlQuery(@"select e.EmployeeId,e.EmployeeName from lg.[User] u join
                        lg.Employees e on e.EmployeeId=u.EmployeeId
                        where e.EmployeeId =" + employeeId).FirstOrDefault();
            if (getEmpolyee == null)
            {
                getEmpolyee = new EmployeeViewModel();
            }
            return getEmpolyee;
        } 

        public void UpdateTnoInChqNumber(int chqNumber, long tno)
        {
            try
            {
                var chqRow = uow.Repository<AchqH>().GetSingle(x => x.ChkNo == chqNumber);
                chqRow.Tno = tno;
                uow.Repository<AchqH>().Edit(chqRow);
                uow.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public decimal GetUserDebitLimit(int UserId)
        {
            var dramt = uow.Repository<CashLimit>().FindBy(x => x.stfid == UserId).Select(x => x.dramt).FirstOrDefault();
            return dramt;
        }

        public decimal GetUserUserBalance(int userId)
        {
            var abc = uow.Repository<MyBalance>().FindBy(x => x.UserID == userId).Select(x => x.Amount).FirstOrDefault();
            if (abc != null)
                return abc.Value;
            else
                return 0;
        }

        public void SaveUpdateMyBalance(int FUserId, int currId = 0, decimal Amount = 0, int TUserId = 0, int status = 0)
        {
            MyBalance myBalsend = new MyBalance();
            MyBalance myBalRecieve = new MyBalance();
            myBalsend = uow.Repository<MyBalance>().FindBy(x => x.UserID == FUserId && x.Currid == currId).FirstOrDefault();
            myBalRecieve = uow.Repository<MyBalance>().FindBy(x => x.UserID == TUserId && x.Currid == currId).FirstOrDefault();//question
            if (FUserId != 0 && TUserId == 0)
            {
                if (myBalsend != null)
                {
                    myBalsend.Amount = myBalsend.Amount - Amount;
                    myBalsend.UserID = FUserId;
                    uow.Repository<MyBalance>().Edit(myBalsend);

                }
                else if (myBalsend == null)
                {
                    myBalsend = new MyBalance();
                    myBalsend.Amount = Amount;
                    myBalsend.UserID = FUserId;
                    myBalsend.Currid = currId;
                    uow.Repository<MyBalance>().Add(myBalsend);
                }
            }
            if (FUserId == 0 && TUserId != 0)
            {
                if (myBalRecieve != null)
                {
                    myBalRecieve.Amount = myBalRecieve.Amount + Amount;
                    myBalRecieve.UserID = TUserId;
                    uow.Repository<MyBalance>().Edit(myBalRecieve);
                }
                else if (myBalRecieve == null)
                {
                    myBalRecieve = new MyBalance();
                    myBalRecieve.Amount = Amount;
                    myBalRecieve.UserID = TUserId;
                    myBalRecieve.Currid = currId;
                    uow.Repository<MyBalance>().Add(myBalRecieve);
                }
            }
            else if (FUserId != 0 && TUserId != 0 && status == 24)
            {
                if (myBalsend != null)
                {
                    myBalsend.Amount = myBalsend.Amount + Amount;
                    myBalsend.UserID = FUserId;
                    uow.Repository<MyBalance>().Edit(myBalsend);
                }
                else if (myBalsend == null)
                {
                    myBalsend = new MyBalance();
                    myBalsend.Amount = Amount;
                    myBalsend.UserID = FUserId;
                    myBalsend.Currid = currId;
                    uow.Repository<MyBalance>().Edit(myBalsend);
                }
            }
            uow.Commit();
        }
        public bool isDisburseWithCashandBank()
        {
            var paramValueCheck = low.Repository<Loader.Models.ParamValue>().FindBy(x=>x.PId== 4004).Select(x=>x.PValue).SingleOrDefault();
            bool isDisburseWithCash = Convert.ToBoolean(paramValueCheck);
            return isDisburseWithCash;
        }

        public int GetFidOtherLoanByPID(int PID)
        {
            var fidOtherLoan = uow.Repository<ReturnSingleValueModdel>().SqlQuery("select fin.FGetFidOtherLoanByPID(" + PID + ") as IdValue").FirstOrDefault();
            return fidOtherLoan.IdValue;
        }
        public int ChargeDeductOnDisbursement()
        {

            var paramValueCheck = low.Repository<Loader.Models.ParamValue>().SqlQuery("select * from LG.ParamValue where PId=5006").FirstOrDefault();
            int isDisburseWithCash = Convert.ToInt32(paramValueCheck.PValue);
            return isDisburseWithCash;
        }

        public int DefultCurrency()
        {
            return 1;
        }
        public void SaveLoanTempSchedule(ScheduleTrialModel scheduleTModel, int accountId)
        {
            foreach (var item in scheduleTModel.scheduleList)
            {
                TempALSch aschedule = new TempALSch();
                aschedule.schDate = item.DateAd;
                aschedule.schPrin = item.PrincipalInstall;
                aschedule.schInt = item.InterestInstall;
                aschedule.balPrin = item.Balance;
                aschedule.IAccno = accountId;
                uow.Repository<TempALSch>().Add(aschedule);
            }
            uow.Commit();
        }
        public ReturnBaseMessageModel ValidateAccountNo(int iaccno)
        {
            ReturnBaseMessageModel msg = new ReturnBaseMessageModel();
            if (iaccno == 0)
            {
                msg.Success = false;
                msg.Msg = "Account Number is Required";
            }
            else
            {
                msg.Success = true;
            }
            return msg;
        }
        public TaskVerificationList GetUserAssignMenu(int menuId, int userId)
        {
            var userAssignMenu = uow.Repository<TaskVerificationList>().SqlQuery("select * from fin.FgetCheckUserAssignMenu(@menuID) where UserId=" + userId
                , new SqlParameter("@menuID", menuId)).FirstOrDefault();
            return userAssignMenu;
        }
        public void AccountStatusLogChange(short status, int iaccNO)
        {
            StatusChangeLog statusChangeLog = new StatusChangeLog();
            statusChangeLog.AccState = status;
            statusChangeLog.IAccNo = iaccNO;
            statusChangeLog.TDate = GetBranchTransactionDate();
            statusChangeLog.UserID = (short)Global.UserId;
            statusChangeLog.ChangeOn = GetDate();
            uow.Repository<StatusChangeLog>().Add(statusChangeLog);
            uow.Commit();
        }
        public List<ProductViewModel> GetAllProductsByStype(int sType)
        {
            List<ProductViewModel> productList = new List<ProductViewModel>();
            productList = uow.Repository<ProductViewModel>().SqlQuery(@"select  cast(PID as int) as ProductId,Pname as ProductName from fin.ProductDetail pd inner join fin.SchmDetails sc on sc.SDID=pd.SDID where sc.stype=" + sType).ToList();
            return productList;
        }

        public bool IsAuthotityToVerify(Int64 eventValue, int eventId)
        {
            var userAssignMenu = uow.Repository<TaskViewModel>().SqlQuery(@"select * from Mast.Task tk
                                                                          join Mast.TaskDetails td on td.Task1Id = tk.Task1Id 
                                                                          where EventValue = " + eventValue + " and EventId = " + eventId + " and TaskTo =" + Global.UserId+ "and VerifiedOn is null and RejectedOn is null").FirstOrDefault();
            if (userAssignMenu != null)
                return true;
            else
                return false;

        }
        public bool IsAuthorityToDelete(Int64 eventValue, int eventId)
        {
            var userAssignMenu = uow.Repository<TaskViewModel>().SqlQuery(@"select * from Mast.Task tk
                                                                          join Mast.TaskDetails td on td.Task1Id = tk.Task1Id 
                                                                          where EventValue = " + eventValue + " and EventId = " + eventId + " and RaisedBy =" + Global.UserId+ "and RejectedOn is not null" ).FirstOrDefault();
            if (userAssignMenu != null)
                return true;
            else
                return false;

        }
        public string GetAccountNumberToString(int accountNumber)
        {
            if (accountNumber != 0)
            {

                string accstate = uow.Repository<ADetail>().FindBy(x => x.IAccno == accountNumber).Select(x => x.Accno).FirstOrDefault();
                return accstate;
            }
            else
                return "Account number is not returned";
          
        }

        public string GetAccNumToString(int accountNumber)
        {
            var getAccountNum = uow.Repository<ADetail>().GetSingle(x => x.IAccno == accountNumber);
            // return getName;
            if (getAccountNum != null)
            {
                return getAccountNum.Accno;
            }
            else
                return "";
           
        }
    }
}




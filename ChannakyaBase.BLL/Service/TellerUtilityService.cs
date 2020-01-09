using ChannakyaBase.BLL.Repository;
using ChannakyaBase.DAL.DatabaseModel;
using ChannakyaBase.Model.Models;
using ChannakyaBase.Model.ViewModel;
using Loader.Models;
using Loader.Service;
using MoreLinq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ChannakyaBase.BLL.Service
{
    public static class TellerUtilityService
    {


        private static TellerService tellerService = null;

        static ChannakyaBaseEntities _context = null;


        static TellerUtilityService()
        {
            tellerService = new TellerService();
            _context = new ChannakyaBaseEntities();

        }


        public static SelectList GetAllDepositScheme()
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                //var schemeList = uow.Repository<SchmDetail>().FindBy(x => x.SType == 0).Select(x => new SchemeModel()
                //{
                //    SchemeID = x.SDID,
                //    SchemeName = x.SDName
                //}).OrderBy(x => x.SchemeName).ToList();
                var schemeList = uow.Repository<SchmDetail>().FindBy(x => x.SType == 0 && x.SEnable == true);
                var schmeListFromProduct = uow.Repository<ProductDetail>().Queryable();
                var schemelist = (from s in schemeList
                                  join sp in schmeListFromProduct on new { a = s.SDID } equals new { a = sp.SDID }
                                  select new SchemeModel
                                  {
                                      SchemeID = sp.SDID,
                                      SchemeName = s.SDName
                                  }).DistinctBy(x => x.SchemeName).DistinctBy(x => x.SchemeID).OrderBy(x => x.SchemeName).ToList();
                //var schemelist = query.Distinct().OrderBy(x => x.SchemeName).ToList();

                //var query = "select distinct SDName as SchemeName,fin.ProductDetail.SDID as SchemeID from fin.SchmDetails join fin.ProductDetail on fin.SchmDetails.SDID = fin.ProductDetail.SDID where SType = 0 order by SDName";
                //var schemeList = uow.Repository<SchemeModel>().SqlQuery(query).ToList();
                return new SelectList(schemelist, "SchemeID", "SchemeName");
            }

        }
        public static SelectList GetCertificateForNominee()
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var certlist = uow.Repository<CertificateDef>().GetAll().Take(3).OrderBy(x => x.CCCertID).ToList();
                var certificate = uow.Repository<CertificateDef>().FindBy(x => x.CCCertID == 25).FirstOrDefault();
                certlist.Add(certificate);
                return new SelectList(certlist, "CCCertID", "CCCert");
            }

        }

        public static SelectList GetAllProductBySchemeId(int schemeId)
        {
            return new SelectList(tellerService.GetAllProductBySchemeId(schemeId), "ProductId", "ProductName");
        }
        public static SelectList GetAllInterestCalculationRuleByProductId(int productId)
        {
            return new SelectList(tellerService.GetAllInterestCalculationRuleByProductId(productId), "PloicyIntCalId", "PolicyIntCalName");
        }
        public static SelectList GetCurrencyByProductId(int productId)
        {
            return new SelectList(tellerService.GetCurrencyByProductId(productId), "CTId", "CurrencyName");
        }
        public static SelectList GetFloatingInterest(int productId)
        {
            return new SelectList(tellerService.GetAllFloatingInterestByProductId(productId), "FloatingId", "FloatingName");
        }
        public static SelectList GetInterestCapitalizeByProductId(int productId)
        {
            return new SelectList(tellerService.GetInterestCapitalizeByProductId(productId), "PloicyIntCalId", "PolicyIntCalName");
        }
        public static string PolicyIntCal(int psid)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                string policyInterest = uow.Repository<PolicyIntCalc>().FindByMany(x => x.PSID == psid).Select(x => x.PSName).FirstOrDefault();
                return policyInterest;
            }
        }
        public static string RuleICBDuration(int icbdurId)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                string icbdurName = uow.Repository<RuleICBDuration>().FindByMany(x => x.ICBDurID == icbdurId).Select(x => x.ICBDurNam).FirstOrDefault();
                return icbdurName;
            }
        }
        #region Condition
        public static bool IsMovementAccount(int productId)
        {
            var IsMovementAccount = tellerService.GetProductDetails(productId);
            if ((IsMovementAccount.Nomianable == true) && (IsMovementAccount.MovementId == 3 || IsMovementAccount.MovementId == 5))
                return true;
            else
                return false;

        }
        public static bool IsNominee(int productId)
        {
            var IsMovementAccount = tellerService.GetProductDetails(productId);
            if ((IsMovementAccount.Nomianable == true) && (IsMovementAccount.MovementId == 3))
                return true;
            else
                return false;

        }

        public static bool IsFixedAccount(int productId)
        {
            var SchemeDetails = tellerService.GetFixedOrRecurringDepositDuration(productId);
            if (SchemeDetails.HasDuration == true && SchemeDetails.MultiDeposit == false && SchemeDetails.AllowWithdraw == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsRecurringDeposit(int productId)
        {
            var SchemeDetails = tellerService.GetFixedOrRecurringDepositDuration(productId);
            if (SchemeDetails.HasDuration == true && SchemeDetails.MultiDeposit == true && SchemeDetails.AllowWithdraw == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsOtherTypeOfRecurringDeposit(int productId)
        {
            var SchemeDetails = tellerService.GetFixedOrRecurringDepositDuration(productId);
            if (SchemeDetails.HasDuration == true && SchemeDetails.MultiDeposit == true && SchemeDetails.AllowWithdraw == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsOtherTypeOfRemainingProducts(int productId)
        {
            var SchemeDetails = tellerService.GetFixedOrRecurringDepositDuration(productId);
            if (SchemeDetails.HasDuration == true && SchemeDetails.MultiDeposit == false && SchemeDetails.AllowWithdraw == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool HasDuration(int productId)
        {
            var SchemeDetails = tellerService.GetFixedOrRecurringDepositDuration(productId);
            if (SchemeDetails.HasDuration == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsSchedule(int productId)
        {
            var SchemeDetails = tellerService.GetFixedOrRecurringDepositDuration(productId);
            if (SchemeDetails.Schedule == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool IsFromNow(byte schemeId)
        {
            byte icbId = tellerService.GetRuleICB(schemeId);
            if (icbId == 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool HasIndividualDuration(byte productId)
        {
            var productDetails = tellerService.GetProductDetails(productId);
            if (productDetails.HasIndividualDuration == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool HasIndiviualInterestRate(int productId)
        {
            var productDetails = tellerService.GetProductDetails(productId);
            if (productDetails.HasIndividualRate == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool HasIndividualLimit(byte productId)
        {
            var productDetails = tellerService.GetProductDetails(productId);
            if (productDetails.HasIndividualLimit == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public static AccountDetailsViewModel GetDateDiffInMonthDays(DateTime startdate, DateTime enddate)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {

                var monthsdays = uow.Repository<AccountDetailsViewModel>().SqlQuery("select* from FGetDateInMonthDays(@startdate,@enddate)",
                    new SqlParameter("@startdate", startdate), new SqlParameter("@enddate", enddate)).FirstOrDefault();

                return monthsdays;
            }
        }

        public static SelectList GetAccountOpenCurrency()
        {
            return new SelectList(tellerService.GetAllCurrencyList(), "CTId", "CurrencyName");
        }

        public static SelectList GetBranchList()
        {
            List<DAL.DatabaseModel.LicenseBranch> BranchListName = new List<DAL.DatabaseModel.LicenseBranch>();
            //IEnumerable<SelectListItem> BranchListName;
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                //Loader.Repository.GenericUnitOfWork luow = new Loader.Repository.GenericUnitOfWork();
                BranchSetupService branchService = new BranchSetupService();



                var userId = Global.UserId;
                //ApplicationUser lai userid gareko xa
                //var employeeId = uow.Repository<Loader.Models.ApplicationUser>().FindBy(x => x.Id == userId).Select(x => x.EmployeeId).Single();

                var employeeId = Loader.Service.UserVSBranchService.GetEmployeeIdByUserId();
                //var branchListFromUserVsBranch = uow.Repository<UserVSBranch>().GetAll().Where(x => x.UserId == userId).Select(x => x.BranchId).ToList();

                var branchListIdFromUserVsBranch = uow.Repository<DAL.DatabaseModel.UserVSBranch>().GetAll().Where(x => (x.UserId == userId) && (x.IsEnable == true)).Select(x => x.BranchId).ToList();

                //  IEnumerable<int>

                var branchListIdFromEmployeenew = uow.Repository<DAL.DatabaseModel.Employee>().FindBy(x => x.EmployeeId == employeeId).Select(x => x.BranchId).ToList();
                var branchListIdFromUserVsBrancha = branchListIdFromEmployeenew.Cast<int>();
                //  IEnumerable<int?>

                var unionBranchIdList = branchListIdFromUserVsBranch.Union(branchListIdFromUserVsBrancha);


                //private List<int> unionBranchList=new List<int>;


                foreach (var item in unionBranchIdList)
                {
                    var lst = uow.Repository<DAL.DatabaseModel.LicenseBranch>().GetAll().Where(x => x.BrnhID == item).Single();
                    //  var lst = uow.Repository<LicenseBranch>().FindBy(x => x.BrnhID==item).Select(x => x.BrnhNam).Single();
                    BranchListName.Add(lst);
                }


                //unionBranchList = branchListFromUserVsBranch.Union(branchListFromEmployee);

                // var branchListFromEmployeeId = uow.Repository<Employee>().GetAll().Where(x=);

                //var BranchList = from u in uow.Repository<Users>().GetAll()
                //                 join e in uow.Repository<Employee>().FindBy(x => x.EmployeeId == employeeId).ToList()
                //                  // join e in uow.Repository<Employee>().GetAll() 
                //                  on u.EmployeeId equals e.EmployeeId
                //                 join ub in uow.Repository<UserVSBranch>().FindBy(x => x.UserId == userId).ToList()
                //                 on u.UserId equals ub.UserId
                //                 select new
                //                 {
                //                     employeebranchId = e.BranchId,
                //                     userBranchId = ub.BranchId
                //                 };






                //    List<SelectListItem> options = new List<SelectListItem>();

                //    options.Add(new SelectListItem { Text = "Optional", Value = "1" });
                //    options.Add(new SelectListItem { Text = "Compulsory", Value = "2" });
                //    return options;


                //foreach (var item in BranchList)
                //{

                //    var BranchNameFromUserBranch = uow.Repository<Loader.Models.LicenseBranch>().FindBy(x => x.BrnhID == item.userBranchId).Select(x => x.BrnhNam).Single();
                //    var BranchNameFromEmployee = uow.Repository<Loader.Models.LicenseBranch>().FindBy(x => x.BrnhID == item.employeebranchId).Select(x => x.BrnhNam).Single();

                //    li.Add(BranchNameFromUserBranch);
                //.Add(new SelectListItem {Text= uow.Repository<Loader.Models.LicenseBranch>().FindBy(x => x.BrnhID == item.branchId).Select(x => x.BrnhNam),Value=item.branchId.ToString()})  })

            }



            //var getAllCompanyBranch = branchService.GetAll();
            //var mainBranch = getAllCompanyBranch.Where(x => x.ParentId == 0);
            //var branchList = getAllCompanyBranch.Except(mainBranch).ToList();
            //return new SelectList(branchName, "CompanyId", "BranchName");
            return new SelectList(BranchListName, "BrnhID", "BrnhNam");

        }

        public static SelectList GetBranchListForEmployeeBranchMap()
        {
            List<Loader.Models.LicenseBranch> BranchListName = new List<Loader.Models.LicenseBranch>();
            //IEnumerable<SelectListItem> BranchListName;
            using (Loader.Repository.GenericUnitOfWork luow = new Loader.Repository.GenericUnitOfWork())
            {
                //Loader.Repository.GenericUnitOfWork luow = new Loader.Repository.GenericUnitOfWork();
                BranchSetupService branchService = new BranchSetupService();



                var userId = Global.UserId;
                //ApplicationUser lai userid gareko xa
                //var employeeId = uow.Repository<Loader.Models.ApplicationUser>().FindBy(x => x.Id == userId).Select(x => x.EmployeeId).Single();

                var employeeId = Loader.Service.UserVSBranchService.GetEmployeeIdByUserId();
                //var branchListFromUserVsBranch = uow.Repository<UserVSBranch>().GetAll().Where(x => x.UserId == userId).Select(x => x.BranchId).ToList();

                var branchListIdFromUserVsBranch = luow.Repository<Loader.Models.UserVSBranch>().GetAll().Where(x => (x.UserId == userId) && (x.IsEnable == true)).Select(x => x.BranchId).ToList();

                //  IEnumerable<int>

                var branchListIdFromEmployeenew = luow.Repository<Loader.Models.Employee>().FindBy(x => x.EmployeeId == employeeId).Select(x => x.BranchId).ToList();
                var branchListIdFromUserVsBrancha = branchListIdFromEmployeenew.Cast<int>();
                //  IEnumerable<int?>

                var unionBranchIdList = branchListIdFromUserVsBranch.Union(branchListIdFromUserVsBrancha);


                //private List<int> unionBranchList=new List<int>;


                foreach (var item in unionBranchIdList)
                {
                    var lst = luow.Repository<Loader.Models.LicenseBranch>().GetAll().Where(x => x.BrnhID == item).Single();
                    //  var lst = uow.Repository<LicenseBranch>().FindBy(x => x.BrnhID==item).Select(x => x.BrnhNam).Single();
                    BranchListName.Add(lst);
                }


                //unionBranchList = branchListFromUserVsBranch.Union(branchListFromEmployee);

                // var branchListFromEmployeeId = uow.Repository<Employee>().GetAll().Where(x=);

                //var BranchList = from u in uow.Repository<Users>().GetAll()
                //                 join e in uow.Repository<Employee>().FindBy(x => x.EmployeeId == employeeId).ToList()
                //                  // join e in uow.Repository<Employee>().GetAll() 
                //                  on u.EmployeeId equals e.EmployeeId
                //                 join ub in uow.Repository<UserVSBranch>().FindBy(x => x.UserId == userId).ToList()
                //                 on u.UserId equals ub.UserId
                //                 select new
                //                 {
                //                     employeebranchId = e.BranchId,
                //                     userBranchId = ub.BranchId
                //                 };






                //    List<SelectListItem> options = new List<SelectListItem>();

                //    options.Add(new SelectListItem { Text = "Optional", Value = "1" });
                //    options.Add(new SelectListItem { Text = "Compulsory", Value = "2" });
                //    return options;


                //foreach (var item in BranchList)
                //{

                //    var BranchNameFromUserBranch = uow.Repository<Loader.Models.LicenseBranch>().FindBy(x => x.BrnhID == item.userBranchId).Select(x => x.BrnhNam).Single();
                //    var BranchNameFromEmployee = uow.Repository<Loader.Models.LicenseBranch>().FindBy(x => x.BrnhID == item.employeebranchId).Select(x => x.BrnhNam).Single();

                //    li.Add(BranchNameFromUserBranch);
                //.Add(new SelectListItem {Text= uow.Repository<Loader.Models.LicenseBranch>().FindBy(x => x.BrnhID == item.branchId).Select(x => x.BrnhNam),Value=item.branchId.ToString()})  })

            }



            //var getAllCompanyBranch = branchService.GetAll();
            //var mainBranch = getAllCompanyBranch.Where(x => x.ParentId == 0);
            //var branchList = getAllCompanyBranch.Except(mainBranch).ToList();
            //return new SelectList(branchName, "CompanyId", "BranchName");
            return new SelectList(BranchListName, "BrnhID", "BrnhNam");

        }
        public static SelectList CurrentBranch(int branchId)
        {
            BranchSetupService branchService = new BranchSetupService();
            var branchList = branchService.GetAll().ToList();
            return new SelectList(branchList, "CompanyId", "BranchName", branchId);
        }


        public static string GetAccountStateNameByAccStateId(int accState)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                string accountState = uow.Repository<string>().SqlQuery("Select AccountState from fin.AccountStatus where AsId=" + accState).FirstOrDefault();
                return accountState;
            }
        }

        public static SelectList DurationList(int pid)
        {
            return new SelectList(tellerService.GetDurationList(pid), "Id", "Duration");
        }
        //public static string GetBranchCode(int branchId)
        //{
        //    string branchCode = "";
        //    if (branchId < 10)
        //    {
        //        branchCode = "00" + branchId;
        //    }
        //    else if (branchId < 100)
        //    {
        //        branchCode = "0" + branchId;
        //    }
        //    else
        //    {
        //        branchCode = branchId.ToString();
        //    }
        //    return branchCode;
        //}
        //public static string GetCurrencyCode(int CurrencyId)
        //{
        //    string currencyCode = "";
        //    if (CurrencyId < 10)
        //    {
        //        currencyCode = "00" + CurrencyId;
        //    }
        //    else if (CurrencyId < 100)
        //    {
        //        currencyCode = "0" + CurrencyId;
        //    }
        //    else
        //    {
        //        currencyCode = CurrencyId.ToString();
        //    }
        //    return currencyCode;
        //}
        public static string GetProductCode(int productId)
        {
            using (GenericUnitOfWork _uow = new GenericUnitOfWork())
            {
                var productCode = _uow.Repository<ProductDetail>().GetSingle(x => x.PID == productId);
                return productCode.Apfix;
            }
        }

        public static ReturnBaseMessageModel AvailableDenoNumber(List<DenoOutViewModel> denoOutList)
        {
            ReturnBaseMessageModel returnMessage = new ReturnBaseMessageModel();


            foreach (var item in denoOutList.Where(x => x.DenoNumberOut > 0))
            {
                //if (item.DenoNumberOut != 0)
                //{
                using (GenericUnitOfWork _uow = new GenericUnitOfWork())
                {
                    var availablePiece = _uow.Repository<DenoBal>().FindBy(x => x.DenoBalId == item.DenoBalIdOut).Select(x => x.Piece).FirstOrDefault();

                    decimal totaldenoPiece = availablePiece - item.DenoNumberOut;
                    if (totaldenoPiece < 0)
                    {
                        returnMessage.Success = false;
                        returnMessage.Msg = "You don't have sufficient balance for+" + item.DenoOut + "!!";
                        return returnMessage;
                    }
                }
                //}

            }
            returnMessage.Success = true;
            return returnMessage;
        }


        public static bool BalanceWithDenoAmount(DenoInOutViewModel denoInOutList, decimal amount)
        {
            using (GenericUnitOfWork _uow = new GenericUnitOfWork())
            {
                decimal sum = 0;
                foreach (var item in denoInOutList.DenoInList)
                {
                    // var actualDeno = _uow.Repository<DenoBal>().FindBy(x => x.DenoBalId == item.DenoBalId && x.UserId == Global.UserId).FirstOrDefault();

                    var OutPiece = denoInOutList.DenoOutList.Where(x => x.DenoBalIdOut == item.DenoBalId).Select(x => x.DenoNumberOut).FirstOrDefault();
                    double deno = item.Deno;
                    //if (actualDeno != null)
                    //{
                    //    if (actualDeno.Piece < OutPiece)
                    //    {
                    //        return false;
                    //    }
                    //}
                    //else
                    //{
                    //    OutPiece = 0;
                    //    item.DenoNumber = 0;
                    //}


                    decimal totalDeno = Convert.ToDecimal((item.DenoNumber * deno) - (OutPiece * deno));
                    sum = sum + totalDeno;
                }
                if (Math.Abs(sum) != amount)
                    return false;
                else
                    return true;
            }
        }

        public static int GetmatDurationMonth(double value)
        {
            int matDuration = Convert.ToInt32(Math.Floor(value / 30));
            return matDuration;
        }

        public static decimal GetmatDurationDays(double value)
        {
            decimal matDuration = Convert.ToDecimal(value / 1000);
            return matDuration;
        }

        public static bool IsCheckByDays(double value = 0)
        {
            if (value < 180)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool CheckChequeNumber(int chequeNumber, int accId)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var cheque = uow.Repository<AChq>().FindBy(x => x.IAccno == accId && x.cto >= chequeNumber && x.cfrom <= chequeNumber).FirstOrDefault();
                if (cheque != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static bool CheckMaturityDate(int iAccno)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                CommonService commonService = new CommonService();
                DateTime branchDate = commonService.GetBranchTransactionDate();
                var maturityDate = uow.Repository<ADur>().FindBy(x => x.IAccno == iAccno && x.IsOD == false && x.MatDat <= branchDate).FirstOrDefault();
                if (maturityDate == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }

        }
        public static bool IsRevolving(int accountId)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                CommonService commonService = new CommonService();
                var stype = uow.Repository<SchemeModel>().SqlQuery("Select IsNull(SType,0) as SType From fin.SchmDetails SD Inner Join fin.ProductDetail PD On SD.SDID = PD.SDID Inner Join fin.ADetail AD On PD.PID = AD.PID Where SD.Revolving = 1 And AD.IAccNo =" + accountId).FirstOrDefault();
                if (stype != null)
                {
                    if (stype.SType == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                else
                {
                    return false;
                }

            }
        }
        public static bool IsDuplicateChequeNo(int iAccno, int chequeNo)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var tempTrans = uow.Repository<UttnoModel>().SqlQuery(@"SELECT Tno  FROM [fin].AchqH ah join fin.AChq ac
                          on ah.Rno = ac.rno where ac.IAccno = " + iAccno + " and ah.ChkNo =" + chequeNo).FirstOrDefault();
                if (tempTrans.Tno != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public static bool IsDuplicateChequeNoOld(int iAccno, int chequeNo)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var tempTrans = uow.Repository<CheckChequeNo>().SqlQuery("SELECT * FROM [fin].[FGetUsedChk]() where IAccno=" + iAccno + " and ChqNo=" + chequeNo).FirstOrDefault();
                if (tempTrans != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static ReturnBaseMessageModel CheckChequeState(int iAccno, int chequeNo)
        {
            ReturnBaseMessageModel rModel = new ReturnBaseMessageModel();
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var chequeState = (from ach in uow.Repository<AChq>().FindBy(x => x.IAccno == iAccno)
                                   join
                                   achh in uow.Repository<AchqH>().FindBy(x => x.ChkNo == chequeNo) on ach.rno equals achh.Rno
                                   select new { achh.cstate })


                    .FirstOrDefault();
                if (chequeState.cstate == 3)
                {
                    rModel.Success = false;
                    rModel.Msg = "Cheque book is blocked.!!";
                }
                else if (chequeState.cstate == 4)
                {
                    rModel.Success = false;
                    rModel.Msg = "This Cheque piece is blocked.!!";
                }
                else
                {
                    rModel.Success = true;
                }
                return rModel;
            }

        }

        public static bool IsDeactiveChequeBook(int iAccno, int chequeNo)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var deactiveChequeBook = uow.Repository<AChq>().FindBy(x => x.IAccno == iAccno && x.cto >= chequeNo && x.cfrom <= chequeNo && x.cstate == 5).FirstOrDefault();
                if (deactiveChequeBook != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public static bool IsCheckUsedInGoodForPayment(int iAccno, int chequeNo)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var deactiveChequeBook = uow.Repository<AchqFGdPymt>().FindBy(x => x.IAccno == iAccno && x.Chqno == chequeNo && x.tstate == 2).FirstOrDefault();
                if (deactiveChequeBook != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public static bool IsTellerAmountExceed(decimal amount)
        {
            var tellerAmount = tellerService.TellerCashExceedAmount();
            decimal tellerCrAmount = 0;
            if (tellerAmount != null)
            {
                tellerCrAmount = tellerAmount.cramt;
            }
            if (tellerAmount != null)
            {
                if (tellerCrAmount <= amount)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }

        public static bool IsDurationComplete(int accountId)

        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var duration = uow.Repository<ADetail>().FindBy(x => x.IAccno == accountId && x.AccState == 7).FirstOrDefault();
                if (duration == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        #endregion
        public static bool IsAlreadyMatured(int iaccNo)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                CommonService commonService = new CommonService();
                DateTime branchDate = commonService.GetBranchTransactionDate();
                //         DateTime? matureDate = uow.Repository<ADur>().FindBy(x => x.IAccno == iaccNo && x.MatDat <= branchDate).Select(x => x.MatDat).FirstOrDefault();
                DateTime? matureDate = uow.Repository<ADur>().FindBy(x => x.IAccno == iaccNo).Select(x => x.MatDat).FirstOrDefault();

                //     if (matureDate != null)
                if (matureDate > branchDate)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }

        }
        public static SelectList GetCollectorList()
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                CommonService cs = new CommonService();
                int branchid = cs.GetBranchIdByEmployeeUserId();
                var collectorList = uow.Repository<EmployeeViewModel>().SqlQuery("select EmployeeId,EmployeeNo, EmployeeName,UserId,UserName from fin.FGetAllUsersWithDesignation() where Pid=2009 and BranchId=" + branchid + "").ToList();

                return new SelectList(collectorList, "UserId", "EmployeeName");
            }
        }
        public static SelectList GetCollectorListByBranchId()
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                CommonService cs = new CommonService();
                int branchid = cs.GetBranchIdByEmployeeUserId();
                var collectorList = uow.Repository<EmployeeViewModel>().SqlQuery("select * from [fin].[FGetUserByCollectorDesignation](2009, " + branchid + ") ").ToList();

                return new SelectList(collectorList, "EmployeeId", "EmployeeName");
            }
        }

        #region Account Info Validation
        public static decimal GoodBalance(int iaccNo)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                CommonService commonService = new CommonService();
                int stype = commonService.GetStypeByIaccno(iaccNo);
                decimal goodBalance = 0;
                if (stype == 0)
                {
                    goodBalance = uow.Repository<ABal>().FindBy(x => x.IAccno == iaccNo && x.Flag == 3).Select(x => x.Bal).FirstOrDefault();
                }
                else
                {
                    goodBalance = tellerService.GetSingleAccount(iaccNo).Bal;
                }
                return goodBalance;
            }
        }

        public static decimal ShadowCreditAmount(int iaccNo)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var crAmount = uow.Repository<ABal>().FindBy(x => x.IAccno == iaccNo && x.Flag == 2).Select(x => x.Bal).FirstOrDefault();
                return crAmount;
            }
        }
        public static decimal ShadowDebitAmount(int iaccNo)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var drAmount = uow.Repository<ABal>().FindBy(x => x.IAccno == iaccNo && x.Flag == 1).Select(x => x.Bal).FirstOrDefault();
                return drAmount;
            }
        }
        public static decimal FreezeAmount(int iaccNo)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var frAmount = uow.Repository<ABal>().FindBy(x => x.IAccno == iaccNo && x.Flag == 4).Select(x => x.Bal).FirstOrDefault();
                return frAmount;
            }
        }
        public static decimal ReserveAmount(int iaccNo)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var rAmount = uow.Repository<ABal>().FindBy(x => x.IAccno == iaccNo && x.Flag == 5).Select(x => x.Bal).FirstOrDefault();
                return rAmount;
            }
        }
        public static DateTime GetCheckMatureDateForFixed(int iaccNo)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var matDate = uow.Repository<ReturnSingleValueModdel>().SqlQuery(@"
                    SELECT   ADur.MatDat as DateValue FROM fin.ADetail 
                    INNER JOIN fin.ADur  ON ADetail.IAccno = ADur.IAccno
                    INNER JOIN fin.ProductDetail ON ADetail.PID = ProductDetail.PID 
                    INNER JOIN fin.SchmDetails ON ProductDetail.SDID = SchmDetails.SDID
                     WHERE(SchmDetails.SType = 0)  And(SchmDetails.MultiDeposit = 0) And accstate <> 3
                     and ADetail.iaccno=" + iaccNo).FirstOrDefault();
                // condition ommited from query to get matdate
                // And(ProductDetail.Withdrawal = 0)
                if (matDate == null)
                {
                    return DateTime.MinValue;
                }
                else
                {
                    return Convert.ToDateTime(matDate.DateValue);
                }


            }
        }
        public static DateTime GetCheckMatureDateForRecurring(int iaccNo)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var matDate = uow.Repository<ReturnSingleValueModdel>().SqlQuery(@"SELECT   ADur.MatDat as DateValue FROM fin.ADetail INNER JOIN fin.ADur  ON ADetail.IAccno = ADur.IAccno INNER JOIN
                    fin.ProductDetail ON ADetail.PID = ProductDetail.PID INNER JOIN fin.SchmDetails ON ProductDetail.SDID = SchmDetails.SDID
                     WHERE(SchmDetails.SType = 0) And(ProductDetail.Withdrawal = 1) And(SchmDetails.MultiDeposit = 1) And accstate <> 3  and (SchmDetails.HasDuration = 1)
                     and ADetail.iaccno=" + iaccNo).FirstOrDefault();
                if (matDate == null)
                {
                    return DateTime.MinValue;
                }
                else
                {
                    return Convert.ToDateTime(matDate.DateValue);
                }


            }
        }

        public static int AccountCurrency(int iaccno)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var currencyId = uow.Repository<ADetail>().FindBy(x => x.IAccno == iaccno).Select(x => x.CurrID).FirstOrDefault();
                return currencyId;
            }
        }
        public static int GetAccountStatus(int iaccNo)
        {
            int AccountStatus = tellerService.GetSingleAccount(iaccNo).AccState;
            return AccountStatus;
        }
        public static ReturnBaseMessageModel GetCheckValidAccountStatus(int iaccNo)
        {
            ReturnBaseMessageModel returnMessage = new ReturnBaseMessageModel();
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var AccountDetails = uow.Repository<ADetail>().FindBy(x => x.IAccno == iaccNo).Select(x => new AccountDetailsViewModel()
                {
                    BrchID = x.BrchID,
                    Bal = x.Bal,
                    PID = (x.PID),
                    AccState = x.AccState

                }).FirstOrDefault();

                bool isFixedAccount = TellerUtilityService.IsFixedAccount(AccountDetails.PID);
                bool isRecurring = TellerUtilityService.IsOtherTypeOfRecurringDeposit(AccountDetails.PID);

                if (isFixedAccount || isRecurring)
                {
                    bool isMature = TellerUtilityService.IsAlreadyMatured(iaccNo);
                    if (AccountDetails.AccState != 7)
                    {
                        if (isMature == false) // !isMature replace with isMature
                        {
                            returnMessage.Msg = "Withdraw is not Allowed.! \n Account NOT Matured Yet..!!";
                            returnMessage.Success = false;
                            return returnMessage;
                        }
                    }
                }
            }
            int AccountStatus = tellerService.GetSingleAccount(iaccNo).AccState;

            if (AccountStatus == 2)
            {
                returnMessage.Msg = "No transaction allowed for this account.This Account is in Blocked state.!!";
                returnMessage.Success = false;
                return returnMessage;
            }
            else if (AccountStatus == 3)
            {
                returnMessage.Msg = "No transaction allowed for this account.This Account is in Closed state.!!";
                returnMessage.Success = false;
                return returnMessage;
            }
            else if (AccountStatus == 8)
            {
                returnMessage.Msg = "No transaction allowed for this account.This Account is in Cancelled state.!!";
                returnMessage.Success = false;
                return returnMessage;
            }
            else if (AccountStatus == 10)
            {
                returnMessage.Msg = "No transaction allowed for this account.This Account is in Dormacy state.!!";
                returnMessage.Success = false;
                return returnMessage;
            }

            else
            {
                returnMessage.Success = true;
                return returnMessage;
            }

        }

        public static bool CheckIsGuarantor(int accountId)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var guarantorList = uow.Repository<GuarantorModel>().SqlQuery(@"select * from [fin].[FCheckGurantor](" + accountId + ")").ToList();
                var result = false;
                if (guarantorList.Count > 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
                return result;
                //.Repository<GuarantorModel>().SqlQuery(@"select * from [fin].[FCheckGurantor](" + accountId + ")").ToList();
            }

        }

        public static ReturnBaseMessageModel AccountNumberRequired(int iaccNo)
        {
            ReturnBaseMessageModel returnMessage = new ReturnBaseMessageModel();
            if (iaccNo == 0)
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Account Number is Required.!!";
                return returnMessage;
            }
            else
            {
                returnMessage.Success = true;
            }
            return returnMessage;
        }
        public static decimal UserCurrentBalance(int currencyId)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                currencyId = new CommonService().DefultCurrency();
                var availableBal = uow.Repository<MyBalance>().FindBy(x => x.Currid == currencyId && x.UserID == Global.UserId).Select(x => x.Amount).FirstOrDefault();
                return Convert.ToDecimal(availableBal);
            }
        }

        public static bool HasRevLinkLoan(int iaccNo)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var revLinkLoan = uow.Repository<ReturnSingleValueModdel>().SqlQuery(@"Select IAccNo as IdValue From fin.ADetail AD Inner Join fin.ProductDetail PD On AD.PID = PD.PID Where AccState <> 2 And Revolving = 1 And IAccNo In(Select IAccNo From fin.ALinkLoan Where ILinkAc = " + iaccNo).ToList();
                if (revLinkLoan != null)
                {
                    if (revLinkLoan.Count() > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
        public static decimal AvailableGoodBalanceWithShadowCr(int iaccNo)
        {
            decimal goodBalance = 0;
            var crAmount = ShadowCreditAmount(iaccNo);
            var goodAmount = GoodBalance(iaccNo);
            goodBalance = crAmount + goodAmount;
            return goodBalance;
        }
        public static decimal AvailableBalance(int iaccNo)
        {
            decimal availableBalance = 0;
            CommonService commonService = new CommonService();
            int stype = commonService.GetStypeByIaccno(iaccNo);
            decimal goodBalance = GoodBalance(iaccNo);
            decimal transshadowDr = ShadowDebitAmount(iaccNo);
            decimal transShadowCr = ShadowCreditAmount(iaccNo);
            decimal freezeAmount = FreezeAmount(iaccNo); ;
            decimal drLimit = 0;
            decimal reserveAmount = ReserveAmount(iaccNo);
            decimal gurantorAmount = TellerUtilityService.CheckForGuarantor(iaccNo);
            if (stype == 1)
            {
                drLimit = DebitLimitAmount(iaccNo);
                availableBalance = drLimit - goodBalance;
                return availableBalance;
            }

            //  decimal minBalance = GetMinBalance(iaccNo);

            //  decimal totalfreezeAmount = freezeAmount + minBalance;
            int pid = tellerService.GetSingleAccount(iaccNo).PID;
            bool hasOverdrew = Hasoverdraw(pid);
            if (hasOverdrew == true)
            {
                decimal Limit = LimitAmount(iaccNo);
                //availableBalance = (goodBalance + Limit + transShadowCr) - (transshadowDr + reserveAmount + freezeAmount);
                availableBalance = (goodBalance + Limit) - (reserveAmount + freezeAmount);

            }
            else
            {
                //availableBalance = goodBalance + transShadowCr - (transshadowDr + reserveAmount + freezeAmount);
                availableBalance = goodBalance - (reserveAmount + freezeAmount);
            }

            return availableBalance;
        }

        public static bool Hasoverdraw(int pid)
        {
            var productDetails = tellerService.GetFixedOrRecurringDepositDuration(pid);
            if (productDetails.Hasoverdraw == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static decimal DebitLimitAmount(int iaccno)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var adrLimit = uow.Repository<ADrLimit>().FindBy(x => x.IAccno == iaccno).Select(x => x.AppAmt).FirstOrDefault();
                return adrLimit;
            }
        }
        public static decimal LimitAmount(int iaccno)
        {
            CommonService commonService = new CommonService();
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                DateTime branchDate = commonService.GetBranchTransactionDate();
                var adrLimit = uow.Repository<ReturnSingleValueModdel>().SqlQuery(@"select adrLimit.AppAmt as AmountValue from fin.ADrLimit as adrLimit  inner join fin.ADur as adur  on adrLimit.IAccno=adur.IAccno where adrLimit.IAccno =" + iaccno + "and adur.MatDat >='" + branchDate + "'").FirstOrDefault();

                if (adrLimit == null)
                {

                    return 0;
                }
                else
                {
                    return adrLimit.AmountValue;
                }

            }
        }
        public static decimal GetMinBalance(int iaccno)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                decimal minBalance = uow.Repository<AMinBal>().FindBy(x => x.IAccno == iaccno).Select(x => x.Minbal).FirstOrDefault();
                return minBalance;
            }
        }

        public static decimal CheckForGuarantor(int iAccno)
        {
            decimal totalAmount = 0;
            var guarantor = tellerService.GetGuarantorList(iAccno);
            if (guarantor.Count() != 0)
            {
                foreach (var item in guarantor)
                {
                    //if (item.IsPercent)
                    //{
                    //    decimal availableBal = AvailableBalance(iAccno);
                    //    totalAmount += (availableBal / 100) * Convert.ToDecimal(item.BlockedAmt);
                    //}
                    //else
                    //{
                    //    totalAmount += Convert.ToDecimal(item.BlockedAmt);
                    //}


                    totalAmount += Convert.ToDecimal(item.BlockedAmt);

                }
                return totalAmount;
            }
            return totalAmount;


        }

        public static decimal CheckForGuarantorForLoanee(int iAccno)
        {
            decimal totalAmount = 0;
            var guarantor = tellerService.GetGuarantorListForLoanAccount(iAccno);
            if (guarantor.Count() != 0)
            {
                foreach (var item in guarantor)
                {
                    //if (item.IsPercent)
                    //{
                    //    decimal availableBal = AvailableBalance(iAccno);
                    //    totalAmount += (availableBal / 100) * Convert.ToDecimal(item.BlockedAmt);
                    //}
                    //else
                    //{
                    //    totalAmount += Convert.ToDecimal(item.BlockedAmt);
                    //}


                    totalAmount += Convert.ToDecimal(item.BlockedAmt);

                }
                return totalAmount;
            }
            return totalAmount;

        }

        public static byte HasOverDrawnFacility(int iAccno, decimal ReqAmt, decimal AvlBal)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                byte returnValue = 0;
                CommonService commonService = new CommonService();
                DateTime transactionDate = commonService.GetBranchTransactionDate();
                var query = "select * from fin.adur where  ValDat <='" + transactionDate + "' and MatDat>= '" + transactionDate + "'  and IsOD = 1)";
                var overdraftTrue = uow.Repository<ADur>().SqlQuery("select * from fin.adur where  ValDat <='" + transactionDate + "' and MatDat>= '" + transactionDate + "'  and IsOD = 1").FirstOrDefault();
                if (overdraftTrue == null)
                {
                    //  If there is no record in [Adur] table, then this account doesnot have overdrawn facility or the facility has expired.
                    return returnValue;
                }
                else
                {
                    //  If the account has Overdrawn Facilty, then check the withdraw limit amount in [ADrlimit] table
                    decimal debitLimitAmount = DebitLimitAmount(iAccno);
                    if (debitLimitAmount != 0)
                    {
                        if ((AvlBal >= ReqAmt))
                        {
                            returnValue = 1;
                            return returnValue;
                        }
                        else
                        {
                            returnValue = 2;
                            return returnValue;
                        }
                    }
                    else
                    {
                        return returnValue = 3;
                    }
                }

            }
        }

        public static decimal RevolvingAvailableBalance(int iAccno)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var availableBalance = uow.Repository<ReturnSingleValueModdel>().SqlQuery(@"Select (IsNull(ADrLimit.AppAmt,0) -(IsNull(ADetail.Bal,0) + IsNull((SELECT SUM(dramt)
                                                                                          FROM trans.ASTrns WHERE (IAccno =" + iAccno + @")),0)))As 'AmountValue' From fin.ADrLimit
                                                                                          Inner Join fin.ADetail On ADrLimit.IAccNo = ADetail.IAccNo Where ADetail.IAccNo =" + iAccno).FirstOrDefault();
                return availableBalance.AmountValue;
            }
        }

        public static ReturnBaseMessageModel CheckWithDrawPopUpSelectCondition(int iaccNo)
        {
            ReturnBaseMessageModel returnMessage = new ReturnBaseMessageModel();
            CommonService commonService = new CommonService();
            int userBranchId = commonService.GetBranchIdByEmployeeUserId();
            int AccountBranchId = commonService.AccountHolderBranchId(iaccNo);
            DateTime userBranchDate = commonService.GetBranchTransactionDate();
            DateTime accountBranchDate = commonService.GetOtherBranchTransactionDateByBranchId(AccountBranchId);
            int AccountStatus = tellerService.GetSingleAccount(iaccNo).AccState;

            if (userBranchDate != accountBranchDate)
            {
                returnMessage.Msg = "Transaction Date of the other Branch does not Match !.";
                returnMessage.Success = false;
                return returnMessage;

            }
            if (AccountStatus == 5)//Debit Restricted
            {
                returnMessage.Msg = "Withdraw not Allowed.This Account is in Debit Restricted state.!!";
                returnMessage.Success = false;
                return returnMessage;
            }
            var guarantor = tellerService.GetGuarantorList(iaccNo);
            bool IsLonee = guarantor.Select(x => x.IsLoanee).FirstOrDefault();
            decimal amount = CheckForGuarantor(iaccNo);
            if (IsLonee)
            {
                returnMessage.Msg = "This  is Loanee of account No. #(" + guarantor.Select(x => x.AccountNumber).FirstOrDefault() + ")#  with Blocked Amount " + amount.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture);
                returnMessage.Success = true;
                return returnMessage;
            }
            else
            {
                if (guarantor.Count() > 1)
                {
                    returnMessage.Msg = "This account  is Guarantor of other Loan Accounts  with Total Blocked Amount" + amount;
                    returnMessage.Success = true;
                    return returnMessage;
                }
                else if (guarantor.Count() == 1)
                {
                    returnMessage.Msg = "This  is Loanee of account No. #(" + guarantor.Select(x => x.AccountNumber).FirstOrDefault() + ")#  with Blocked Amount " + amount.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture);
                    returnMessage.Success = true;
                    return returnMessage;
                }
            }
            int pid = GetPid(iaccNo);
            bool isFixedAccount = IsFixedAccount(pid);
            bool isRecurring = IsOtherTypeOfRecurringDeposit(pid);
            if (isFixedAccount || isRecurring)
            {
                bool isMature = IsAlreadyMatured(iaccNo);
                if (AccountStatus != 7)
                {
                    if (isMature)   // replaced !isMature from here
                    {
                        var availabbe = tellerService.InterestPayable(iaccNo);
                        if (availabbe.Amount > 0)
                        {
                            returnMessage.Msg = "Interest payment allowed but ";
                        }
                        returnMessage.Msg = returnMessage.Msg + "Withdraw is not Allowed.!  Account NOT Matured Yet.!!";
                        returnMessage.Success = false;
                        return returnMessage;
                    }
                }
            }
            returnMessage.ReturnId = 1;
            returnMessage.Success = true;
            return returnMessage;
        }

        public static int GetPid(int iAccno)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                int pid = uow.Repository<ADetail>().FindBy(x => x.IAccno == iAccno).Select(x => x.PID).FirstOrDefault();
                return pid;
            }
        }

        public static ReturnBaseMessageModel CheckDepositPopUpSelectCondition(int accountId)
        {
            ReturnBaseMessageModel returnMessage = new ReturnBaseMessageModel();
            CommonService commonService = new CommonService();

            int userBranchId = commonService.GetBranchIdByEmployeeUserId();
            int AccountBranchId = commonService.AccountHolderBranchId(accountId);
            DateTime userBranchDate = commonService.GetBranchTransactionDate();
            DateTime accountBranchDate = commonService.GetOtherBranchTransactionDateByBranchId(AccountBranchId);
            int AccountStatus = tellerService.GetSingleAccount(accountId).AccState;

            if (userBranchDate != accountBranchDate)
            {
                returnMessage.Msg = "Transaction Date of the other Branch does not Match !.";
                returnMessage.Success = false;
                return returnMessage;

            }
            if (AccountStatus == 9)//Credit Restricted
            {
                returnMessage.Msg = "Deposit not Allowed.This Account is in Credit Restricted state.!!";
                returnMessage.Success = false;
                return returnMessage;
            }
            else if (AccountStatus == 7)//Credit Restricted
            {
                returnMessage.Msg = "Deposit not Allowed.This Account is in Ready To be Closed state.!!";
                returnMessage.Success = false;
                return returnMessage;
            }
            int pid = GetPid(accountId);
            if (userBranchId != AccountBranchId)
            {

                bool allowABBS = Convert.ToBoolean(tellerService.GetProductDetails(pid).IntraBrnhTrnx);

                if (allowABBS == false)
                {
                    returnMessage.Msg = "ABBS Deposit is not allowed for this Account.!!";
                    returnMessage.Success = false;
                    return returnMessage;
                }
            }
            bool isFixedAccount = IsFixedAccount(pid);
            if (isFixedAccount)
            {
                DateTime maturedate = GetCheckMatureDateForFixed(accountId);
                if (commonService.GetBranchTransactionDate() >= maturedate)
                {
                    returnMessage.Msg = "The account is already Matured.NO additional Deposit is allowed unless the account is Renewed.!!";
                    returnMessage.Success = false;
                    return returnMessage;
                }
                decimal adrLimit = DebitLimitAmount(accountId);
                decimal currentAmount = AvailableGoodBalanceWithShadowCr(accountId);


                if (adrLimit == currentAmount)
                {
                    returnMessage.Msg = "The Approved Amount has already been Deposited.No additional deposit is allowed in this Fixed Account.!!";
                    returnMessage.Success = false;
                    return returnMessage;
                }
            }
            if (IsRecurringDeposit(pid))
            {
                DateTime maturedate = GetCheckMatureDateForRecurring(accountId);
                if (maturedate != DateTime.MinValue)
                {
                    if (commonService.GetBranchTransactionDate() >= maturedate)
                    {
                        returnMessage.Msg = "The account is already Matured.NO additional Deposit is allowed unless the account is Renewed.!!";
                        returnMessage.Success = false;
                        return returnMessage;
                    }
                }

            }

            returnMessage.Success = true;
            return returnMessage;
        }

        public static bool CheckChequeNumberInAWtdQueue(int iaccNo, int chequeNo)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var checkChequeNumber = uow.Repository<AWtdQueue>().FindBy(x => x.IAccno == iaccNo && x.chqno == chequeNo).Select(x => x.Rno).FirstOrDefault();
                if (checkChequeNumber == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }

        public static bool CheckUnverifiedInChequeGoodForPayment(int iaccNo, int chequeNo)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var checkChequeNumber = uow.Repository<AchqFGdPymt>().FindBy(x => x.IAccno == iaccNo && x.Chqno == chequeNo && x.tstate == 1).FirstOrDefault();
                if (checkChequeNumber != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }
        public static ReturnBaseMessageModel CheckChequeValidation(int chequeNumber, int accId)
        {
            ReturnBaseMessageModel returnMessage = new ReturnBaseMessageModel();
            bool IsExiestscheque = CheckChequeNumber(chequeNumber, accId);
            if (IsExiestscheque)
            {

                if (IsDeactiveChequeBook(accId, chequeNumber))
                {
                    returnMessage.Msg = "Cheque book is finished.!!";
                    returnMessage.Success = false;
                    return returnMessage;
                }
                else if (IsDuplicateChequeNo(accId, chequeNumber))
                {
                    if (IsCheckUsedInGoodForPayment(accId, chequeNumber))
                    {
                        if (CheckChequeNumberInAWtdQueue(accId, chequeNumber))
                        {
                            returnMessage.Success = true;
                            returnMessage.TransactionType = "GoodForPayment";
                            var goodForPayment = tellerService.GetGoodForPaymentDetails(accId, chequeNumber);
                            returnMessage.Value = goodForPayment.payee;
                            returnMessage.ValueOne = goodForPayment.amount;
                            returnMessage.BoolValue = IsTellerAmountExceed(goodForPayment.amount);
                            return returnMessage;
                        }

                        else
                        {
                            returnMessage.Success = false;
                            returnMessage.Msg = "Cheque number is already used.!!";
                            return returnMessage;
                        }

                    }
                    else if (CheckUnverifiedInChequeGoodForPayment(accId, chequeNumber) == false)


                    {
                        returnMessage.Success = false;
                        returnMessage.Msg = "Cheque number for good for payement  is not verified Yet.!!";
                        return returnMessage;
                    }
                    else if (InformationUtilityService.IsChequeBounce(accId, chequeNumber))
                    {
                        returnMessage.Success = false;
                        returnMessage.Msg = "Cheque is bounce for a reason :- " + InformationUtilityService.BounceReason(accId, chequeNumber) + "!!";
                        return returnMessage;
                    }

                    returnMessage.Success = false;
                    returnMessage.Msg = "Cheque number is already used.!!";
                    return returnMessage;
                }
                else
                {
                    returnMessage = CheckChequeState(accId, chequeNumber);
                    if (!returnMessage.Success)
                    {
                        return returnMessage;
                    }
                }
            }
            else
            {

                returnMessage.Msg = "This cheque number doesn't belongs to account holder.";
                returnMessage.Success = false;
                return returnMessage;
            }
            returnMessage.Success = true;
            return returnMessage;
        }

        public static string GetTType(int ttype, bool isslip)
        {
            string transactionType = "";
            if (ttype == 1 && isslip == true)
            {
                transactionType = "cash withdraw same branch";
            }
            if (ttype == 1 && isslip == false)
            {
                transactionType = "cheque withdraw same branch";
            }
            if (ttype == 2 && isslip == true)
            {
                transactionType = "cash withdraw different branch";
            }
            if (ttype == 2 && isslip == false)
            {
                transactionType = "cheque withdraw different branch";
            }
            return transactionType;
        }
        public static ReturnBaseMessageModel IsVerifyPreviousTransaction(int iAccno)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                ReturnBaseMessageModel returnMessage = new ReturnBaseMessageModel();
                var previoustrancastion = uow.Repository<ASTrn>().FindBy(x => x.IAccno == iAccno).FirstOrDefault();
                if (previoustrancastion != null)
                {
                    returnMessage.Msg = "There is pending transaction for this account.!!Please verify Previous transaction first.";
                    returnMessage.Success = false;
                    return returnMessage;
                }
                else
                {

                    returnMessage.Success = true;
                    return returnMessage;
                }
            }
        }
        #endregion
        public static bool IsActiveTransactionUser()
        {
            using (GenericUnitOfWork _uow = new GenericUnitOfWork())
            {
                CommonService commonService = new CommonService();
                DateTime transactionDate = commonService.GetBranchTransactionDate();
                var activeUser = _uow.Repository<CashFlow>().FindBy(x => x.UserID == Global.UserId && x.TDate == transactionDate && x.Status == 22).FirstOrDefault();
                if (activeUser != null)
                {
                    //if (activeUser.Dramt != 0)
                    //{
                    return true;
                    //}
                    //else
                    //{
                    //    return false;
                    //}
                }
                else
                {
                    return false;
                }

            }
        }

        public static ReturnBaseMessageModel CheckUserActivateOrNot()
        {
            ReturnBaseMessageModel returnMessage = new ReturnBaseMessageModel();
            bool activateOrNot = IsActiveTransactionUser();
            if (activateOrNot == false)
            {
                returnMessage.Msg = "User Not Activate.";
                returnMessage.Success = false;
                return returnMessage;
            }
            else
            {
                returnMessage.Success = true;
                return returnMessage;
            }
        }

    }

}



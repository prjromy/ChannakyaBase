using ChannakyaBase.BLL.CustomHelper;
using ChannakyaBase.BLL.Repository;
using ChannakyaBase.DAL.DatabaseModel;
using ChannakyaBase.Model.Models;
using ChannakyaBase.Model.ViewModel;
using ChannakyaCustomeDatePicker.Service;
using Loader.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using MoreLinq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Globalization;

namespace ChannakyaBase.BLL.Service
{
    public class TellerService
    {
        private GenericUnitOfWork uow = null;
        // private ChannakyaBaseEntities _context = null;
        ReturnBaseMessageModel returnMessage = null;
        BaseTaskVerificationService taskUow = null;
        FinanceParameterService financeParameterService = null;
        private CommonService commonService = null;
        private Loader.Repository.GenericUnitOfWork luow = null;

        public TellerService()
        {
            financeParameterService = new FinanceParameterService();
            taskUow = new BaseTaskVerificationService();
            uow = new GenericUnitOfWork();
            // _context = new ChannakyaBaseEntities();
            returnMessage = new ReturnBaseMessageModel();
            commonService = new CommonService();
            luow = new Loader.Repository.GenericUnitOfWork();

        }

        #region Account Open Scheme Event

        public CheckProductTypeModel CheckProductStatusType(int productId = 0)
        {
            var getDetails = GetFixedOrRecurringDepositDuration(productId);
            CheckProductTypeModel chkProduct = new CheckProductTypeModel();
            chkProduct.IsDuration = Convert.ToBoolean(getDetails.HasDuration);
            bool isFixed = false;
            if (getDetails.HasDuration == true && getDetails.MultiDeposit == false/* && getDetails.AllowWithdraw == false*/)
            {
                isFixed = true;
            }
            bool IsMovementAccount = false;
            if ((getDetails.Nomianable == true) && (getDetails.MovementId == 3 || getDetails.MovementId == 5))
                IsMovementAccount = true;

            chkProduct.IsFixed = isFixed;
            chkProduct.IsIndividualLimit = Convert.ToBoolean(getDetails.HasIndividualLimit);
            chkProduct.IsIndiviualInterestRate = Convert.ToBoolean(getDetails.HasIndividualRate);
            chkProduct.IsMovement = IsMovementAccount;

            bool IsRecurringDeposit = false;
            if (getDetails.HasDuration == true && getDetails.MultiDeposit == true /*&& getDetails.AllowWithdraw == true*/)
            {
                IsRecurringDeposit = true;
            }
            chkProduct.IsRecurring = IsRecurringDeposit;
            bool IsOtherTypeRecurring = false;
            if (getDetails.HasDuration == true && getDetails.MultiDeposit == true /*&& getDetails.AllowWithdraw == false*/)
            {
                IsOtherTypeRecurring = true;
            }
            chkProduct.IsOtherTypeRecurring = IsOtherTypeRecurring;

            bool IsOtherTypeRemaining = false;
            if (getDetails.HasDuration == true && getDetails.MultiDeposit == false && getDetails.AllowWithdraw == true)
            {
                IsOtherTypeRemaining = true;
            }
            chkProduct.IsOtherTypeRemaining = IsOtherTypeRemaining;

            if ((getDetails.Nomianable == true) && (getDetails.MovementId == 3))
                chkProduct.IsNomianable = true;
            else
                chkProduct.IsNomianable = false;
            chkProduct.SchemeId = getDetails.SchemeId;
            return chkProduct;

        }
        public List<ProductDurationViewModel> GetDurationList(int pid = 0)
        {
            var productDurationInt = uow.Repository<ProductDurationInt>().Queryable();
            var Duration = uow.Repository<Duration>().Queryable();

            var query = from pd in productDurationInt
                        join d in Duration on new { a = pd.DVId } equals new { a = d.Id }
                        select new ProductDurationViewModel
                        {
                            Id = d.Id,
                            Duration = d.Duration1,
                            ProductId = pd.PId

                        };
            var duration = query.Where(x => x.ProductId == pid).Distinct().OrderBy(x => x.Id);
            return duration.ToList();
        }

        public Duration GetDurationByDurationId(int durId = 0)
        {
            var duration = uow.Repository<Duration>().FindBy(x => x.Id == durId).FirstOrDefault();
            return duration;
        }
        public List<InterestCapitalModel> GetInterestCapitalizeByProductId(int productId = 0)
        {
            var productIcbDur = uow.Repository<ProductICBDur>().Queryable();
            var ruleICBDUR = uow.Repository<RuleICBDuration>().Queryable();
            var query = from prod in productIcbDur
                        join rule in ruleICBDUR
                        on new { a = prod.ICBDurID } equals new { a = rule.ICBDurID }

                        select new InterestCapitalModel()
                        {
                            InterestCapitalizeId = prod.PICBDurID,
                            InterestCapitalizeName = rule.ICBDurNam,
                            IsDefault = prod.IsDefault,
                            ProductId = prod.PID

                        };
            var policyIntCalByProductId = query.Where(x => x.ProductId == productId).OrderByDescending(x => x.IsDefault);

            return policyIntCalByProductId.ToList();

        }

        public AccountDetailsViewModel GetSingleAccount(int IAccno = 0)
        {
            var accdetail = uow.Repository<ADetail>().FindByMany(x => x.IAccno == IAccno).Select(account => new AccountDetailsViewModel
            {

                Accno = account.Accno,
                AccState = account.AccState,
                Aname = account.Aname,
                PID = account.PID,
                RDate = account.RDate,
                CurrID = account.CurrID,
                BrchID = account.BrchID,
                PostedBy = account.PostedBy,
                ApprovedBy = account.ApprovedBy,
                Bal = account.Bal,
                IonBal = account.IonBal,
                InterestRate = account.IRate,
            });
            // accdetail.Duration = Convert.ToInt32(account.ADur.Days);
            return accdetail.FirstOrDefault();

        }

        public bool AgreementAmountWithLAmountProduct(decimal agreeAmount, int PID)
      {
            var LAMount = uow.Repository<ProductDetail>().FindBy(x => x.PID == PID).Select(x => x.LAmt).FirstOrDefault();
            var result = true;
            if (agreeAmount <= LAMount)
            {
                result = false;
                return result;


            }

            else
            {
                return result;
            }
        }

        public ADetail GetSingleAccountDetails(int IAccno = 0)
        {

            var account = uow.Repository<ADetail>().FindBy(x => x.IAccno == IAccno).FirstOrDefault();

            return account;


        }

        public APolicyInterest GetSingleInterestPolicyDetails(int IAccno = 0)
        {

            var policy = uow.Repository<APolicyInterest>().FindBy(x => x.IAccno == IAccno).FirstOrDefault();

            return policy;


        }
        public List<StatusChangeLogModel> AccountStatusLog(int IAccno = 0)
        {
            var statusChange = uow.Repository<StatusChangeLog>().Queryable();
            var status = uow.Repository<AccountStatu>().Queryable();
            var query = from s in statusChange
                        join st in status on s.AccState equals st.AsId

                        select new StatusChangeLogModel()
                        {
                            StatusName = st.AccountState,
                            ChangeOn = s.ChangeOn,
                            TDate = s.TDate,
                            UserID = s.UserID,
                            IAccNo = s.IAccNo

                        };
            var statusLog = query.Where(x => x.IAccNo == IAccno).OrderByDescending(x => x.ChangeOn);
            return statusLog.ToList();
        }

        public bool CheckInterestValue(decimal interestRate, decimal GetinterestRate)
        {
            var interestUpDownArray = luow.Repository<Loader.Models.ParamValue>().FindBy(x => x.PId == 9059).Select(x => x.PValue).FirstOrDefault();

            //char[] splitchar = { ',' };
            //var array =  interestUpDownArray.Split(splitchar);
            string[] p = interestUpDownArray.Split(',', '[', ']');
            var totallowerlimit = GetinterestRate - Convert.ToInt32(p[1]);

            var totalupperlimit = GetinterestRate + Convert.ToInt32(p[2]);
            if (interestRate > totalupperlimit || interestRate < totallowerlimit)
            {
                return false;
            }


            //var 
            else
            {
                return true;
            }

        }

        public ProductViewModel GetProductDetails(int productId = 0)
        {
            var productDetails = uow.Repository<ProductDetail>().Queryable();
            var schmDetails = uow.Repository<SchmDetail>().Queryable();

            var quey = from x in productDetails
                       join s in schmDetails on x.SDID equals s.SDID
                       where x.PID == productId
                       select new ProductViewModel()
                       {
                           MinimumAmount = x.LAmt,
                           InterestRate = x.IRate,
                           Duration = x.Duration,
                           Nomianable = x.Nomianable,
                           MovementId = s.MID,
                           DurState = x.durState == 0 ? "Day" : x.durState == 1 ? "Month" : "DayMonth",
                           DurationType = x.durState,
                           HasIndividualDuration = x.HasIndDuration,
                           HasIndividualLimit = x.HasIndivLimit,
                           HasIndividualRate = x.HasIndivRate,
                           HasDuration = s.HasDuration,
                           IntraBrnhTrnx = x.IntraBrnhTrnx == null ? false : x.IntraBrnhTrnx,
                           PPRate = x.PPRate,
                           PIRate = x.PIRate,
                           OIRate = x.OIRate,
                           IRate = x.IRate,
                           HasOverdrew = x.HasOverdraw

                       };
            var interestRateAmount = quey.FirstOrDefault();
            if (interestRateAmount != null)
            {
                if (interestRateAmount.Duration != null)
                {
                    if (interestRateAmount.Duration % 1 != 0)
                    {
                        interestRateAmount.Duration = interestRateAmount.Duration.Value * 1000;
                    }
                }
            }

            if (interestRateAmount == null)
            {
                ProductViewModel InterestRateAmount1 = new ProductViewModel();
                return InterestRateAmount1;
            }
            return interestRateAmount;
        }

        public decimal GetSingleInterestProductDurInt(int productId = 0, int captId = 0, decimal? contrubAmount = 0, int durationId = 0, int basicId = 0)
        {
            decimal interestRate = 0;


            if (TellerUtilityService.IsFixedAccount(productId))
            {
                var productDurInt = uow.Repository<ProductDurationInt>().FindBy(x => x.ICBId == captId && x.PId == productId && x.DVId == durationId && x.DBId == null).Select(x => x.InterestRate).FirstOrDefault();
                interestRate = Convert.ToDecimal(productDurInt);
            }
            else if (TellerUtilityService.IsRecurringDeposit(productId) || TellerUtilityService.IsOtherTypeOfRecurringDeposit(productId))
            {
                var productDurInt = uow.Repository<ProductDurationInt>().FindBy(x => x.ICBId == captId && x.PId == productId && x.DVId == durationId && x.Value == (double)contrubAmount && x.DBId == basicId).Select(x => x.InterestRate).FirstOrDefault();
                interestRate = Convert.ToDecimal(productDurInt);

            }
            else if (TellerUtilityService.IsOtherTypeOfRemainingProducts(productId))
            {
                var productDurInt = uow.Repository<ProductDurationInt>().FindBy(x => x.ICBId == captId && x.PId == productId && x.DVId == durationId).Select(x => x.InterestRate).FirstOrDefault();
                interestRate = Convert.ToDecimal(productDurInt);
            }
            else
            {
                var InterestCapitalize = uow.Repository<ProductICBDur>().FindBy(x => x.PICBDurID == captId && x.PID == productId).Select(x => x.IRate).FirstOrDefault();
                interestRate = Convert.ToDecimal(InterestCapitalize);
            }

            return interestRate;


        }

        public List<ProductDurationViewModel> GetRecurringBasic(double contrubAmount = 0, int productId = 0, int durationId = 0)
        {
            var recDuration = (from di in uow.Repository<ProductDurationInt>().FindBy(x => x.Value == contrubAmount && x.PId == productId && x.DVId == durationId && x.DBId != null)
                               join d in uow.Repository<DepositBasi>().GetAll() on di.DBId equals d.Id
                               select new ProductDurationViewModel()
                               {
                                   Id = d.Id,
                                   Duration = d.Name
                               }).DistinctBy(x => x.Id).OrderBy(x => x.Duration).ToList();
            return recDuration;
        }

        public float GetCapitalizeRuleValueByProductDurationIntId(int id)
        {
            float RuleICBDurationValue = uow.Repository<RuleICBDuration>().FindBy(x => x.ICBDurID == id).Select(x => x.ICBDurVal).FirstOrDefault();

            return RuleICBDurationValue;
        }
        public List<InterestCapitalModel> GetIntCapitalizeRuleForFixedAndRecurringAndOtherProduct(int productId = 0, int durationId = 0)
        {

            List<InterestCapitalModel> RecurringCapitalizeRule = new List<InterestCapitalModel>();

            if (TellerUtilityService.IsFixedAccount(productId))
            {
                RecurringCapitalizeRule = uow.Repository<InterestCapitalModel>().SqlQuery("select * from fin.FGetFixedIntCapitalizeRule(" + productId + "," + durationId + ") order by InterestCapitalizeId").Select(x => new InterestCapitalModel
                {
                    InterestCapitalizeId = x.InterestCapitalizeId,
                    InterestCapitalizeName = x.InterestCapitalizeName
                }).ToList();
                //var ruleICBDUR = uow.Repository<RuleICBDuration>().Queryable();
                //var productDurInt = uow.Repository<ProductDurationInt>().Queryable();
                //RecurringCapitalizeRule = (from prodInt in productDurInt
                //            join rule in ruleICBDUR
                //            on new { a = prodInt.ICBId} equals new { a = rule.ICBDurID }

                //            select new InterestCapitalModel
                //            {
                //                InterestCapitalizeId = Convert.ToInt32(prodInt.ICBId),
                //                InterestCapitalizeName = rule.ICBDurNam,


                //            }).ToList();
                //RecurringCapitalizeRule = uow.Repository<ProductDurationInt>().FindBy(x => x.PId == productId && x.DBId == null && x.DVId == durationId).Select(x => new InterestCapitalModel
                //{
                //    InterestCapitalizeId = Convert.ToInt32(x.ICBId),
                //    InterestCapitalizeName=
                //}).ToList();

                return RecurringCapitalizeRule;

            }
            else if (TellerUtilityService.IsRecurringDeposit(productId) || TellerUtilityService.IsOtherTypeOfRecurringDeposit(productId))
            {
                RecurringCapitalizeRule = uow.Repository<InterestCapitalModel>().SqlQuery("select * from fin.FGetRecurringIntCapitalizeRule(" + productId + "," + durationId + ")").Select(x => new InterestCapitalModel
                //RecurringCapitalizeRule = uow.Repository<ProductDurationInt>().FindBy(x => x.PId == productId && x.DBId != null && x.DVId == durationId).Select(x => new InterestCapitalModel
                //{3
                {
                    contributionValue = x.contributionValue,
                    InterestCapitalizeId = x.InterestCapitalizeId,
                    InterestCapitalizeName = x.InterestCapitalizeName

                }).DistinctBy(x => x.contributionValue).ToList();

                //var ruleICBDUR = uow.Repository<RuleICBDuration>().Queryable();
                //var productDurInt = uow.Repository<ProductDurationInt>().Queryable();
                //RecurringCapitalizeRule = (from prodInt in productDurInt
                //                           join rule in ruleICBDUR
                //                           on new { a = Convert.ToInt32(prodInt.ICBId) } equals new { a = Convert.ToInt32(rule.ICBDurID) }

                //                           select new InterestCapitalModel
                //                           {
                //                               InterestCapitalizeId = Convert.ToInt32(prodInt.ICBId),
                //                               InterestCapitalizeName = rule.ICBDurNam,


                //                           }).DistinctBy(x=>x.contributionValue).ToList();

                return RecurringCapitalizeRule;
            }
            else if (TellerUtilityService.IsOtherTypeOfRemainingProducts(productId))
            {
                RecurringCapitalizeRule = uow.Repository<InterestCapitalModel>().SqlQuery("select * from fin.FGetRecurringIntCapitalizeRule(" + productId + "," + durationId + ")").ToList();
                //RecurringCapitalizeRule = uow.Repository<ProductDurationInt>().FindByMany(x => x.PId == productId && x.DBId == null && x.DVId == durationId).Select(x => new InterestCapitalModel
                //{
                //    InterestRate = x.InterestRate.Value,
                //    InterestCapitalizeId = x.ICBId.Value
                //}).ToList();

                var ruleICBDUR = uow.Repository<RuleICBDuration>().Queryable();
                var productDurInt = uow.Repository<ProductDurationInt>().Queryable();

                RecurringCapitalizeRule = (from prodInt in productDurInt
                                           join rule in ruleICBDUR
             on prodInt.ICBId equals rule.ICBDurID
                                           select new InterestCapitalModel
                                           {
                                               InterestCapitalizeId = prodInt.ICBId.Value,
                                               InterestCapitalizeName = rule.ICBDurNam,
                                               ProductId = prodInt.PId,

                                               DVId = prodInt.DVId
                                           }).ToList();

                RecurringCapitalizeRule = RecurringCapitalizeRule.Where(x => x.ProductId == productId && x.DVId == durationId).ToList();
            }
            else
            {
                var productIcbDur = uow.Repository<ProductICBDur>().Queryable();
                var ruleICBDUR = uow.Repository<RuleICBDuration>().Queryable();
                var query = from prod in productIcbDur
                            join rule in ruleICBDUR
                            on new { a = prod.ICBDurID } equals new { a = rule.ICBDurID }

                            select new InterestCapitalModel()
                            {
                                InterestCapitalizeId = prod.PICBDurID,
                                InterestCapitalizeName = rule.ICBDurNam,
                                IsDefault = prod.IsDefault,
                                ProductId = prod.PID

                            };
                var policyIntCalByProductId = query.Where(x => x.ProductId == productId).OrderByDescending(x => x.IsDefault);
                RecurringCapitalizeRule = policyIntCalByProductId.ToList();
            }

            return RecurringCapitalizeRule;

        }
        public List<FloatingInterestModel> GetAllFloatingInterestByProductId(int productId = 0)
        {
            var productTid = uow.Repository<ProductTID>().Queryable();
            var tempRate = uow.Repository<TempIntRate>().Queryable();

            var floatingInterest = from prod in productTid
                                   join rate in tempRate
                                   on prod.TID equals rate.TID
                                   where prod.PID == productId
                                   select new FloatingInterestModel()
                                   {
                                       FloatingId = prod.PFIID,
                                       FloatingName = rate.Tname
                                   };
            return floatingInterest.ToList();
        }

        public List<CurrencyModel> GetCurrencyByProductId(int productId = 0)
        {

            var currencyList = uow.Repository<CurrencyModel>().SqlQuery("select * from fin.FGetCurrencyList(" + productId + ")").OrderBy(x => x.CurrencyName).ToList();
            return currencyList;
        }

        public List<ProductViewModel> GetAllProductBySchemeId(int schemeId = 0)
        {
            var productListByScheme = uow.Repository<ProductDetail>().FindByMany(x => x.SDID == schemeId && x.enabled == true).Select(x => new ProductViewModel()
            {
                ProductId = x.PID,
                ProductName = x.PName,
            }).OrderBy(x => x.ProductName);
            return productListByScheme.ToList();

        }

        public ReturnBaseMessageModel ApproveInterestPayable(long tno)
        {
            try
            {
                var rPayable = uow.Repository<AintPayable>().GetSingle(x => x.Tno == tno);
                rPayable.VerifiedBy = Global.UserId;
                uow.Repository<AintPayable>().Edit(rPayable);
                var aDetails = uow.Repository<ADetail>().GetSingle(x => x.IAccno == rPayable.Iaccno);
                aDetails.IonBal = aDetails.IonBal - Convert.ToDecimal(rPayable.DrAmt);
                uow.Repository<ADetail>().Edit(aDetails);
                uow.Commit();
                returnMessage.Msg = "Interest payable Successfully verified with Transaction No# " + tno.ToString();
                returnMessage.Success = true;
                return returnMessage;
            }
            catch (Exception ex)
            {

                returnMessage.Msg = "Not save" + ex.Message;
                returnMessage.Success = false;
                return returnMessage; ;
            }
        }

        public List<PolicyModel> GetAllInterestCalculationRuleByProductId(int productId = 0)
        {
            var productPSID = uow.Repository<ProductPSID>().Queryable();
            var policyIntCalc = uow.Repository<PolicyIntCalc>().Queryable();
            var query = from prod in productPSID
                        join pol in policyIntCalc
                        on prod.PSID equals pol.PSID
                        where prod.PID == productId
                        select new PolicyModel()
                        {
                            PloicyIntCalId = prod.PSID,
                            PolicyIntCalName = pol.PSName,
                            IsDefault = prod.IsDefault

                        };
            var policyIntCalByProductId = query.OrderByDescending(x => x.IsDefault);
            return policyIntCalByProductId.ToList();


        }



        public bool IsChargeAvailable(int productId = 0, int modevent = 0)
        {
            var chargeByPId = financeParameterService.GetChargeDetailsByProductId(productId, modevent);
            if (chargeByPId.Count() > 0)
                return true;
            else
                return false;
        }

        public ProductViewModel GetFixedOrRecurringDepositDuration(int productId = 0)
        {
            var schmDetail = uow.Repository<SchmDetail>().Queryable();
            var productDetail = uow.Repository<ProductDetail>().Queryable();
            var query = from s in schmDetail
                        join p in productDetail on s.SDID equals p.SDID
                        where p.PID == productId
                        select new ProductViewModel()
                        {
                            HasDuration = s.HasDuration == null ? false : s.HasDuration,
                            MultiDeposit = p.MultiDep == null ? false : p.MultiDep,
                            AllowWithdraw = p.Withdrawal == null ? false : p.Withdrawal,
                            Schedule = p.Schedule == null ? false : p.Schedule,
                            Hasoverdraw = p.HasOverdraw,
                            IsOverdraft = s.HasOverdraw,
                            HasIndividualDuration = p.HasIndDuration,
                            HasIndividualLimit = p.HasIndivLimit,
                            HasIndividualRate = p.HasIndivRate,
                            SchemeId = s.SDID,
                            IntraBrnhTrnx = p.IntraBrnhTrnx == null ? false : p.IntraBrnhTrnx,
                            MovementId = s.MID,
                            Nomianable = s.Nomianable

                        };
            ProductViewModel fixDeposit = query.FirstOrDefault();

            return fixDeposit;
        }

        public byte GetRuleICB(int schemeId)
        {
            byte? icbId = uow.Repository<SchmDetail>().FindByMany(x => x.SDID == schemeId).Select(x => x.ICBID).FirstOrDefault();
            return Convert.ToByte(icbId);


        }

        public List<InterestCapitalModel> GetInterestCapitalizeForRecurringProduct(int durationId = 0, int productId = 0, int basicId = 0, double? value = 0)
        {
            //var productDurationInt = uow.Repository<ProductDurationInt>().Queryable();
            //var ruleICBDuration = uow.Repository<RuleICBDuration>().Queryable();
            //var recuringBasic = (from dbi in productDurationInt
            //                     where dbi.PId == productId && dbi.Value == value && dbi.DBId == basicId && dbi.DVId == durationId
            //                     join rule in ruleICBDuration on dbi.ICBId equals rule.ICBDurID
            //                     select new InterestCapitalModel()
            //                     {
            //                         InterestCapitalizeId = rule.ICBDurID,
            //                         InterestCapitalizeName = rule.ICBDurNam,

            //                     }

            //                   ).ToList();

            //return recuringBasic;

            List<InterestCapitalModel> RecurringCapitalizeRule = new List<InterestCapitalModel>();

            if (TellerUtilityService.IsFixedAccount(productId))
            {
                RecurringCapitalizeRule = uow.Repository<InterestCapitalModel>().SqlQuery("select * from fin.FGetFixedIntCapitalizeRule(" + productId + "," + durationId + ") order by InterestCapitalizeId").ToList();
                //var ruleICBDUR = uow.Repository<RuleICBDuration>().Queryable();
                //var productDurInt = uow.Repository<ProductDurationInt>().Queryable();
                //RecurringCapitalizeRule = (from prodInt in productDurInt
                //                           join rule in ruleICBDUR
                //                           on new { a = Convert.ToByte(prodInt.ICBId) } equals new { a = rule.ICBDurID }

                //                           select new InterestCapitalModel
                //                           {
                //                               InterestCapitalizeId = Convert.ToInt32(prodInt.ICBId),
                //                               InterestCapitalizeName = rule.ICBDurNam,


                //                           }).ToList();
            }
            else if (TellerUtilityService.IsRecurringDeposit(productId) || TellerUtilityService.IsOtherTypeOfRecurringDeposit(productId))
            {
                //RecurringCapitalizeRule = uow.Repository<ProductDurationInt>().FindBy(x => x.PId == productId && x.DBId != null && x.DVId == durationId).Select(x => new InterestCapitalModel
                //{
                //    contributionValue = x.Value
                //}).DistinctBy(x => x.contributionValue).ToList();
                // RecurringCapitalizeRule = uow.Repository<InterestCapitalModel>().SqlQuery("select * from fin.FGetRecurringIntCapitalizeRule(" + productId + "," + durationId + ")").ToList();
                //var ruleICBDUR = uow.Repository<RuleICBDuration>().Queryable();
                //var productDurInt = uow.Repository<ProductDurationInt>().Queryable();
                //RecurringCapitalizeRule = (from prodInt in productDurInt
                //                           join rule in ruleICBDUR
                //                           on new { a = Convert.ToByte(prodInt.ICBId) } equals new { a = rule.ICBDurID }

                //                           select new InterestCapitalModel
                //                           {
                //                               InterestCapitalizeId = Convert.ToInt32(prodInt.ICBId),
                //                               InterestCapitalizeName = rule.ICBDurNam,


                //                           }).DistinctBy(x => x.contributionValue).ToList();
                var query = "select prod.ICBId as InterestCapitalizeId,rul.ICBDurNam as InterestCapitalizeName from fin.ProductDurationInt as prod join fin.RuleICBDuration as rul on prod.ICBId = rul.ICBDurID where prod.PId=" + productId + " and prod.DBId = " + basicId + " and prod.DVId = " + durationId + " and Value = " + value + "";
                RecurringCapitalizeRule = uow.Repository<InterestCapitalModel>().SqlQuery(query).ToList();

            }
            else
            {
                var productIcbDur = uow.Repository<ProductICBDur>().Queryable();
                var ruleICBDUR = uow.Repository<RuleICBDuration>().Queryable();
                var query = from prod in productIcbDur
                            join rule in ruleICBDUR
                            on new { a = prod.ICBDurID } equals new { a = rule.ICBDurID }

                            select new InterestCapitalModel()
                            {
                                InterestCapitalizeId = prod.PICBDurID,
                                InterestCapitalizeName = rule.ICBDurNam,
                                IsDefault = prod.IsDefault,
                                ProductId = prod.PID

                            };
                var policyIntCalByProductId = query.Where(x => x.ProductId == productId).OrderByDescending(x => x.IsDefault);
                RecurringCapitalizeRule = policyIntCalByProductId.ToList();
            }

            return RecurringCapitalizeRule;

        }

        public ReturnBaseMessageModel RejectInternalChequeDeposit(long eventValue)
        {
            using (var scope = uow.BeginTransaction())
            {
                try
                {

                    returnMessage.Msg = "Internal Cheque Deposit Rejected Successfully";
                    returnMessage.Success = true;
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    returnMessage.Msg = "Internal Cheque Deposit Cannot Rejected" + ex.Message;
                    returnMessage.Success = false;

                }
                return returnMessage;
            }
        }

        public ReturnBaseMessageModel RejectChequeGoodForPayment(long eventValue)
        {
            using (var scope = uow.BeginTransaction())
            {
                try
                {
                    var chqGoodPayment = uow.Repository<AchqFGdPymt>().FindBy(x => x.tno == eventValue).SingleOrDefault();
                    decimal reserveAmount = 0;
                    chqGoodPayment.tstate = 3;
                    var ReserveAmount = uow.Repository<ABal>().FindBy(x => x.IAccno == chqGoodPayment.IAccno && x.Flag == 5).FirstOrDefault();
                    // var NewReserveAmount = uow.Repository<ABal>().FindBy(x => x.IAccno == chqGoodPayment.IAccno ).FirstOrDefault();
                    reserveAmount = ReserveAmount.Bal - chqGoodPayment.amount;
                    ReserveAmount.Bal = reserveAmount;
                    chqGoodPayment.IsRejected = true;
                    uow.Repository<ABal>().Edit(ReserveAmount);
                    uow.Repository<AchqFGdPymt>().Edit(chqGoodPayment);

                    uow.Commit();
                    scope.Commit();
                    returnMessage.Msg = "Cheque Good For Payment Rejected Successfully";
                    returnMessage.Success = true;
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    returnMessage.Msg = "Cheque Good For Payment Cannot Rejected" + ex.Message;
                    returnMessage.Success = false;

                }
                return returnMessage;
            }
        }
        public ReturnBaseMessageModel DeleteChequeGoodForPayment(long eventValue)
        {
            using (var scope = uow.BeginTransaction())
            {
                try
                {
                    var chqGoodPayment = uow.Repository<AchqFGdPymt>().FindBy(x => x.tno == eventValue).SingleOrDefault();

                    uow.Repository<AchqFGdPymt>().Delete(chqGoodPayment);

                    uow.Commit();
                    scope.Commit();
                    returnMessage.Msg = "Cheque Good For Payment Deleted Successfully";
                    returnMessage.Success = true;
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    returnMessage.Msg = "Cheque Good For Payment Cannot Deleted" + ex.Message;
                    returnMessage.Success = false;

                }
                return returnMessage;
            }
        }
        public string GetAccountRelatedCurrency(int accountId)
        {
            var currencyType = uow.Repository<ADetail>().FindBy(x => x.IAccno == accountId).Select(x => x.CurrID).FirstOrDefault();
            var currencyName = uow.Repository<CurrencyType>().FindBy(x => x.CTId == currencyType).Select(x => x.CurrencyName).FirstOrDefault();
            var countryName = uow.Repository<CurrencyType>().FindBy(x => x.CTId == currencyType).Select(x => x.Country).FirstOrDefault();

            string currencyFullName = currencyName + "(" + countryName + ")";
            return currencyFullName;
        }
        #endregion

        #region Account Open

        public ReturnBaseMessageModel InsertUpdateAccountOpen(AccountDetailsViewModel aModelDetails, TaskVerificationList TaskVerifierList, List<ChargeDetail> ChargeDetailsList)
        {


            using (var transaction = uow.GetContext().Database.BeginTransaction())
            {

                DateTime transactionDate = commonService.GetBranchTransactionDate();
                //using (TransactionScope transaction = new TransactionScope(
                //    // a new transaction will always be created
                //    TransactionScopeOption.Required,
                //    // we will allow volatile data to be read during transaction
                //    new TransactionOptions()
                //    {
                //        IsolationLevel = IsolationLevel.ReadCommitted,
                //        Timeout = TimeSpan.MaxValue

                // }
                //))
                // {



                try
                {
                    float totalShare = 0;
                    var checkCondition = CheckProductStatusType(aModelDetails.PID);
                    byte SchemeId = (byte)checkCondition.SchemeId;


                    bool isFixedProduct = checkCondition.IsFixed;
                    bool isNominable = checkCondition.IsNomianable;
<<<<<<< Updated upstream
                    var LAMount = uow.Repository<ProductDetail>().FindBy(x => x.PID == aModelDetails.PID).Select(x => x.LAmt).FirstOrDefault();
                    if (aModelDetails.AgreementAmount <= LAMount)
=======
                    if (aModelDetails.AgreementAmount.Equals(null))
>>>>>>> Stashed changes
                    {
                        //var LAMount = uow.Repository<ProductDetail>().FindBy(x => x.PID == aModelDetails.PID).Select(x => x.LAmt).FirstOrDefault();
                        //if (aModelDetails.AgreementAmount <=LAMount)
                        //{
                        //    returnMessage.Msg = "Agreement Amount must be greater than  Limit amount of product!!";
                        //    returnMessage.Success = false;
                        //    return returnMessage;
                        //}
                    }
                  
                    foreach (var item in aModelDetails.ANominees)
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
                        returnMessage.Msg = "Nominee share should be 100% !!";
                        returnMessage.Success = false;
                        return returnMessage;
                    }
                    if (isFixedProduct)
                    {
                        if (aModelDetails.AgreementAmount == 0)
                        {
                            returnMessage.Msg = "Agreement amount is required!!";
                            returnMessage.Success = false;
                            return returnMessage;
                        }
                    }
                    if (isNominable)
                    {
                        if (aModelDetails.MovementAcc == 0)
                        {
                            returnMessage.Msg = "Movement account is required!!";
                            returnMessage.Success = false;
                            return returnMessage;
                        }
                    }

                    var productDetails = this.GetProductDetails(aModelDetails.PID);

                    ADetail accountDetails = new ADetail();

                    accountDetails.PID = aModelDetails.PID;
                    //accountDetails.Accno = "00-00-00-00";
                    accountDetails.RDate = aModelDetails.RDate;
                    accountDetails.CurrID = aModelDetails.CurrID;
                    accountDetails.BrchID = (byte)commonService.GetBranchIdByEmployeeUserId();
                    accountDetails.AccState = 6;
                    accountDetails.Aname = aModelDetails.Aname;
                    if (aModelDetails.DateType == true)
                    {
                        accountDetails.DateType = 1;
                    }
                    else
                    {
                        accountDetails.DateType = 2;
                    }

                    #region Account Nominee
                    // Account Nominee Details
                    foreach (var item in aModelDetails.ANominees)
                    {
                        ANominee accountNominee = new ANominee();

                        accountNominee.NomNam = item.NomNam;
                        accountNominee.NomRel = item.NomRel;
                        accountNominee.CCertID = item.CCertID;
                        accountNominee.CCertno = item.CCertno;
                        accountNominee.Share = item.Share;
                        accountNominee.ContactNo = item.ContactNo;
                        accountNominee.ContactAddress = item.ContactAddress;

                        accountDetails.ANominees.Add(accountNominee);
                    }


                    #endregion

                    #region Customer Account
                    //Account Of Customer
                    byte count = 1;
                    foreach (var item in aModelDetails.CustomerId)
                    {
                        AOfCust accountOfCustomer = new AOfCust();

                        accountOfCustomer.CID = item;
                        accountOfCustomer.Sno = count;
                        count++;

                        accountDetails.AOfCusts.Add(accountOfCustomer);
                    }
                    #endregion

                    #region movement Account
                    //If movement Account given
                    bool isMovementAcc = checkCondition.IsMovement;
                    if (isMovementAcc)
                    {
                        ANomAcc aNomineeAcc = new ANomAcc();

                        if (aModelDetails.MovementAcc != 0)
                        {
                            aNomineeAcc.NIAccno = aModelDetails.MovementAcc;

                            accountDetails.ANomAccs.Add(aNomineeAcc);
                        }

                    }
                    #endregion

                    #region reference
                    //reference Information
                    ReferenceInfo reference = new ReferenceInfo();

                    reference.ReferredBy = aModelDetails.ReferredBy[0];
                    reference.BroughtBy = aModelDetails.BroughtBy;
                    reference.RType = 2;

                    accountDetails.ReferenceInfoes.Add(reference);

                    #endregion

                    #region agreement amount
                    //if agreement amount  given

                    bool isRecurring = checkCondition.IsRecurring;
                    bool isotherRecurring = checkCondition.IsOtherTypeRecurring;
                    if (isFixedProduct || isRecurring || isotherRecurring)
                    {
                        ADrLimit agreementDrLimit = new ADrLimit();

                        if (isRecurring || isotherRecurring)
                        {
                            agreementDrLimit.AppAmt = aModelDetails.ContributionAmount;
                        }
                        else
                        {
                            agreementDrLimit.AppAmt = aModelDetails.AgreementAmount;
                        }

                        accountDetails.ADrLimit = agreementDrLimit;
                    }
                    bool hasDuration = checkCondition.IsDuration;
                    if (hasDuration)
                    {
                        string type = commonService.DateType();
                        ADur accountDuration = new ADur();

                        if (TellerUtilityService.IsFromNow(SchemeId))
                        {

                            if (aModelDetails.DateType == true)
                            {
                                type = "1";
                            }

                            var durationById = this.GetDurationByDurationId(aModelDetails.Duration);
                            int matDuration = 0;
                            int interestCal = 0;
                            matDuration = TellerUtilityService.GetmatDurationMonth(durationById.Value);
                            float value = GetCapitalizeRuleValueByProductDurationIntId(aModelDetails.InterestCapitalize);
                            try
                            {
                                interestCal = Convert.ToInt32(matDuration / value);
                            }
                            catch
                            {
                                interestCal = 0;
                            }
                            float currentMonth = value;
                            DateTime startDate = aModelDetails.RDate;
                            for (int i = 0; i < interestCal; i++)
                            {
                                ADSch adSch = new ADSch();
                                if (type == "1")
                                {
                                    DateTime scheduleDate = commonService.GetMatDate(startDate, Convert.ToDecimal(currentMonth), type);
                                    adSch.MDate = scheduleDate;
                                    accountDetails.ADSches.Add(adSch);
                                    currentMonth += value;
                                }
                                else
                                {
                                    DateTime scheduleDate = commonService.GetMatDate(startDate, Convert.ToDecimal(value), type);
                                    adSch.MDate = scheduleDate;
                                    accountDetails.ADSches.Add(adSch);
                                    startDate = scheduleDate;
                                }

                            }

                        }
                        accountDuration.DurationId = aModelDetails.Duration;
                        accountDuration.ValDat = aModelDetails.RDate;
                        accountDuration.MatDat = aModelDetails.MaturationDate;
                        accountDuration.DurType = Convert.ToInt16(type);
                        accountDuration.IsOD = productDetails.HasOverdrew;

                        accountDetails.ADur = accountDuration;


                    }
                    #endregion

                    #region Interest Rate
                    //Account Interest Rate
                    ARateMaster accountRateMaster = new ARateMaster();


                    accountRateMaster.PostedBy = Global.UserId;
                    accountRateMaster.PostedDate = transactionDate;
                    accountRateMaster.EffectiveDate = transactionDate;

                    uow.Repository<ARateMaster>().Add(accountRateMaster);

                    ARate accountInterestRate = new ARate();


                    if (checkCondition.IsIndiviualInterestRate)
                    {
                        accountInterestRate.IRate = (float)aModelDetails.InterestRate;
                        accountDetails.IRate = aModelDetails.InterestRate;
                    }
                    else
                    {
                        decimal interestRate = GetSingleInterestProductDurInt(aModelDetails.PID, aModelDetails.InterestCapitalize, aModelDetails.ContributionAmount, aModelDetails.Duration, aModelDetails.Basic);
                        accountInterestRate.IRate = (float)interestRate;
                        accountDetails.IRate = interestRate;
                    }

                    accountDetails.ARates.Add(accountInterestRate);
                    accountRateMaster.ARates.Add(accountInterestRate);

                    #endregion

                    #region Account Minimun balance Change
                    AMinBal accountMinBal = new AMinBal();

                    //if (aModelDetails.MinimumBalance != 0)
                    //{


                    if (isFixedProduct)
                    {
                        accountMinBal.Minbal = aModelDetails.AgreementAmount;
                    }
                    else
                    {
                        if (checkCondition.IsIndividualLimit)
                        {
                            accountMinBal.Minbal = aModelDetails.MinimumBalance;
                        }
                        else
                        {
                            accountMinBal.Minbal = productDetails.MinimumAmount;
                        }
                        if (accountMinBal.Minbal != 0)
                        {
                            accountDetails.AMinBal = accountMinBal;
                        }
                    }
                    uow.Repository<AMinBal>().Add(accountMinBal);

                    #endregion

                    #region PolicyInterest
                    var policyInterest = new APolicyInterest();

                    policyInterest.PSID = aModelDetails.InterestCalRule;
                    var ruleIcdDuration = new AICBDur();

                    ruleIcdDuration.ICBDurID = aModelDetails.InterestCapitalize;

                    accountDetails.APolicyInterest = policyInterest;
                    accountDetails.AICBDur = ruleIcdDuration;


                    // AOPolInt apolInt = new AOPolInt();
                    #endregion

                    accountDetails.PostedBy = Global.UserId.ToString();
                    uow.Repository<ADetail>().Add(accountDetails);

                    uow.Commit();

                    int iaccno = accountDetails.IAccno;
                    commonService.AccountStatusLogChange(6, iaccno);
                    taskUow.SaveTaskNotification(TaskVerifierList, iaccno, 1);
                    if (ChargeDetailsList != null)
                    {

                        financeParameterService.ChargeUnverifiedTransaction(commonService, ChargeDetailsList, iaccno, TaskVerifierList, 5);
                    }
                    transaction.Commit();
                    returnMessage.Msg = "Account Created Successfully" + iaccno;
                    returnMessage.Success = true;
                }

                catch (Exception ex)
                {
                    transaction.Dispose();
                    returnMessage.Msg = ex.InnerException.Message + "-Account  not save.!!" + ex.GetBaseException().Message;
                    returnMessage.Success = false;
                }


            }
            return returnMessage;

        }

        public bool InternalChequeFixedDepositValidAmount(int accountId, decimal amount)
      {
            int productId = TellerUtilityService.GetPid(accountId);
            bool IsFixedAccount = TellerUtilityService.IsFixedAccount(productId);
            var returnMessage = true;

            if (IsFixedAccount)
            {
                decimal adrLimit = TellerUtilityService.DebitLimitAmount(accountId);
                decimal currentAmount = TellerUtilityService.AvailableGoodBalanceWithShadowCr(accountId);
                decimal addedAmount = currentAmount + amount;

                if (adrLimit == currentAmount)
                {

                    returnMessage = false;
                    return returnMessage;
                }
                if (addedAmount > adrLimit)
                {

                    returnMessage = false;
                    return returnMessage;
                }
                DateTime maturedate = TellerUtilityService.GetCheckMatureDateForFixed(accountId);
                if (commonService.GetBranchTransactionDate() >= maturedate)
                {

                    returnMessage = false;
                    return returnMessage;
                }
            }

            else
            {
                return returnMessage;
            }

            return returnMessage;
        }

        public ReturnBaseMessageModel RejectLoanRegistration(long eventValue)
        {
            using (var scope = uow.BeginTransaction())
            {
                try
                {
                    var registration = uow.Repository<ALRegistration>().GetSingle(x => x.RegId == eventValue);
                    registration.Status = 0;
                    registration.Remarks = "Rejected";

                    uow.Commit();
                    scope.Commit();
                    returnMessage.Msg = "Account Rejected Successfully";
                    returnMessage.Success = true;
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    returnMessage.Msg = "Account Cannot Rejected" + ex.Message;
                    returnMessage.Success = false;

                }
                return returnMessage;
            }
        }

        public CurrencyRateViewModel GetCurrencyRateAndExchangeRate(int currencyId)
        {
            var currencyRate = uow.Repository<FrxCurrencyRate>().FindByMany(x => x.CurrID == currencyId).Select(x => new CurrencyRateViewModel()
            {
                BuyingRate = x.Brate,
                RatePerUnit = x.BSPerUnit,
                Srate = x.Srate

            });
            return currencyRate.FirstOrDefault();
        }

        public IPagedList<AccountListViewModel> GetAllAccountList(int accState, int pageNumber, int pageSize)
        {

            var accountOpenList = uow.Repository<ADetail>().FindByMany(x => x.AccState == 6).Select(x => new AccountListViewModel
            {
                AccountNumber = x.Accno,
                AccountName = x.Aname,
                RegisterDate = x.RDate,
                AccountId = x.IAccno,
                AccountStatus = x.AccState,
                AccountState = x.AccountStatu.AccountState
            });

            return accountOpenList.OrderByDescending(x => x.AccountId).ToPagedList(pageNumber, pageSize);


        }
        public List<AccountListViewModel> GetAllFilteredAccountListIndex(DateTime? FromDate, DateTime? ToDate, int pageNo, int pageSize, string AccountName, int PID, string AccountNumber)
        {


            //var SchemeId = uow.Repository<SchmDetail>().Queryable();
            //var ProductId = uow.Repository<ProductDetail>().Queryable();
            //var ProductList = 
            //var accountFilteredList = uow.Repository<ADetail>().FindByMany(x => x.AccState == 6).Where(x =>/* x.RDate >= FromDate || x.RDate <= ToDate ||*/ x.Aname == AccountName /*|| x.PID == ProductType*/).Select(x => new AccountDetailsViewModel
            //{
            //    Accno = x.Accno,
            //    Aname = x.Aname,
            //    RDate = x.RDate,
            //    IAccno = x.IAccno,
            //    AccState = x.AccState,
            //});
            //    AccountStatus = x.AccountStatu.AccountState

            //return accountFilteredList.OrderByDescending(x => x.IAccno).ToPagedList(pageNumber, pageSize);


            //string AccountNumberToString = commonService.GetAccNumToString(AccountNumber);
            //Convert.ToDateTime(FromDate).ToShortDateString() 

            // Convert.ToDateTime(FromDate).ToShortDateString();





            //DateTime date1 = DateTime.ParseExact("28/08/2016", "MM/dd/yyyy HH:mm:ss", null);

            //    DateTime date = DateTime.ParseExact(FromDate.ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture);

            //string date = "28/08/2012";
            //System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("en-GB");
            //String newDate=Convert.ToDateTime(date, ci.DateTimeFormat).ToString("d");

            //     DateTime myConvertedDate = DateTime.ParseExact(FromDate.ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            //   myConvertedDate.ToString("MM/dd/yyyy HH:mm:ss");

            // String FromDat1 = DateTime.ParseExact("28/02/2016", "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);

            //String FromDat = DateTime.ParseExact(FromDate.ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            //String ToDat = DateTime.ParseExact(ToDate.ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            string query = "";


            //DateTime FromDateNew = DateTime.ParseExact(FromDat, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            //DateTime ToDateNew = DateTime.ParseExact(ToDat, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            //String FromDateN = DateTime.ParseExact(FromDate.ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            //String ToDateN = DateTime.ParseExact(ToDate.ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);


            //DateTime FromDateNew = DateTime.ParseExact(FromDate.ToString(), "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            //DateTime ToDateNew = DateTime.ParseExact(ToDate.ToString(), "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            //DateTime FromDate1 = DateTime.ParseExact(FromDate.ToString(), "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            //DateTime Todate1 = DateTime.ParseExact(FromDate.ToString(), "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            //DateTime FromDate2 = DateTime.ParseExact(FromDate.ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            //DateTime ToDate2 = DateTime.ParseExact(ToDate.ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            query = @"select COUNT(*) OVER () AS TotalCount, * from [fin].[FGetAccountUnverifiedDetails]('" + FromDate + "','" + ToDate + "') where  AccountName like'%" + AccountName.Trim() + "%' and AccountStatus=6 or AccountStatus=8";

            //if (AccountName != "")
            //{
            //    query += "where  AccountName='"+AccountName.Trim()+"'";
            //}
            if (PID != 0)
            {
                query += " and PID=" + PID;
            }
            if (AccountNumber != "")
            {
                query += "and AccountNumber='" + AccountNumber.Trim() + "'";
            }

            query += @" ORDER BY  AccountName
                       OFFSET ((" + pageNo + @" - 1) * " + pageSize + @") ROWS
                       FETCH NEXT " + pageSize + " ROWS ONLY";

            var accountFilteredList = uow.Repository<AccountListViewModel>().SqlQuery(query).ToList();
            return accountFilteredList;

        }

        public bool FixedDepositValidAmount(int accountId, decimal amount)
        {
            int productId = TellerUtilityService.GetPid(accountId);
            bool IsFixedAccount = TellerUtilityService.IsFixedAccount(productId);
            var returnMessage = true;

            if (IsFixedAccount)
            {
                decimal adrLimit = TellerUtilityService.DebitLimitAmount(accountId);
                decimal currentAmount = TellerUtilityService.AvailableGoodBalanceWithShadowCr(accountId);
                decimal addedAmount = currentAmount + amount;

                if (adrLimit == currentAmount)
                {

                    returnMessage = false;
                    return returnMessage;
                }
                if (addedAmount > adrLimit)
                {

                    returnMessage = false;
                    return returnMessage;
                }
                DateTime maturedate = TellerUtilityService.GetCheckMatureDateForFixed(accountId);
                if (commonService.GetBranchTransactionDate() >= maturedate)
                {

                    returnMessage = false;
                    return returnMessage;
                }
            }

            else
            {
                return returnMessage;
            }

            return returnMessage;

        }

        public List<AccountListViewModel> GetAllFilteredAccountList(DateTime? FromDate, DateTime? ToDate, int pageNo, int pageSize, string AccountName, int PID, string AccountNumber)
        {


            //var SchemeId = uow.Repository<SchmDetail>().Queryable();
            //var ProductId = uow.Repository<ProductDetail>().Queryable();
            //var ProductList = 
            //var accountFilteredList = uow.Repository<ADetail>().FindByMany(x => x.AccState == 6).Where(x =>/* x.RDate >= FromDate || x.RDate <= ToDate ||*/ x.Aname == AccountName /*|| x.PID == ProductType*/).Select(x => new AccountDetailsViewModel
            //{
            //    Accno = x.Accno,
            //    Aname = x.Aname,
            //    RDate = x.RDate,
            //    IAccno = x.IAccno,
            //    AccState = x.AccState,
            //});
            //    AccountStatus = x.AccountStatu.AccountState

            //return accountFilteredList.OrderByDescending(x => x.IAccno).ToPagedList(pageNumber, pageSize);


            //string AccountNumberToString = commonService.GetAccNumToString(AccountNumber);
            //Convert.ToDateTime(FromDate).ToShortDateString() 

            // Convert.ToDateTime(FromDate).ToShortDateString();

            string query = "";



            //DateTime date1 = DateTime.ParseExact("28/08/2016", "MM/dd/yyyy HH:mm:ss", null);

            //    DateTime date = DateTime.ParseExact(FromDate.ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture);

            //string date = "28/08/2012";
            //System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.CreateSpecificCulture("en-GB");
            //String newDate=Convert.ToDateTime(date, ci.DateTimeFormat).ToString("d");

            //     DateTime myConvertedDate = DateTime.ParseExact(FromDate.ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            //   myConvertedDate.ToString("MM/dd/yyyy HH:mm:ss");

            // String FromDat1 = DateTime.ParseExact("28/02/2016", "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);



            query = @"select COUNT(*) OVER () AS TotalCount, * from [fin].[FGetAccountUnverifiedDetails]('" + FromDate + "','" + ToDate + "') where  AccountName like '%" + AccountName.Trim() + "%' and AccountStatus in ('6','8')";

            //if (AccountName != "")
            //{
            //    query += "where  AccountName='"+AccountName.Trim()+"'";
            //}
            if (PID != 0)
            {
                query += " and PID=" + PID;
            }
            if (AccountNumber != "")
            {
                query += "and AccountNumber='" + AccountNumber.Trim() + "'";
            }

            query += @" ORDER BY  AccountName
                       OFFSET ((" + pageNo + @" - 1) * " + pageSize + @") ROWS
                       FETCH NEXT " + pageSize + " ROWS ONLY";

            var accountFilteredList = uow.Repository<AccountListViewModel>().SqlQuery(query).ToList();
            return accountFilteredList;

        }

        public List<CurrencyModel> GetAllCurrencyList()
        {
            var addtails = uow.Repository<ADetail>().Queryable();
            var CurrencyTypes = uow.Repository<CurrencyType>().Queryable();
            var query = from c in CurrencyTypes
                        join a in addtails on c.CTId equals a.CurrID into currencyname
                        from a in currencyname.DefaultIfEmpty()
                        select new CurrencyModel
                        {
                            CurrencyName = c.CurrencyName + "(" + c.Country + ")",
                            CTId = c.CTId,
                            Country = c.Country
                        };
            var CodeCurrencyList = query.Distinct().OrderBy(x=>x.CurrencyName).Where(x=>x.CTId==1).ToList();
            return CodeCurrencyList;
        }

        //public List<CurrencyModel> GetAllCurrencyList(string currencyCode, string currencyName)
        //{
        //    var CurrencyList = GetAllCurrencyList();
        //    if (currencyCode == "" && currencyName == "")
        //    {
        //        var CodeCurrencyList = CurrencyList.Select(x => new CurrencyModel
        //        {
        //            CurrencyCode = TellerUtilityService.GetCurrencyCode(x.CTId),
        //            CurrencyName = x.CurrencyName + "(" + x.Country + ")",
        //            CTId = x.CTId
        //        }).ToList();
        //        return CodeCurrencyList;
        //    }
        //    else if (currencyCode != "" && currencyName == "")
        //    {
        //        var CodeCurrencyList = GetAllCurrencyList(currencyCode);
        //        return CodeCurrencyList;
        //    }
        //    else
        //    {
        //        var CodeCurrencyList = CurrencyList.Select(x => new CurrencyModel
        //        {
        //            CurrencyCode = TellerUtilityService.GetCurrencyCode(x.CTId),
        //            CurrencyName = x.CurrencyName + "(" + x.Country + ")",
        //            CTId = x.CTId
        //        }).Where(x => x.CurrencyName.Contains(currencyCode)).ToList();
        //        return CodeCurrencyList;
        //    }

        //}

        //public List<CurrencyModel> GetAllCurrencyList(string currencyCode)
        //{
        //    var CurrencyList = GetAllCurrencyList();
        //    var CodeCurrencyList = CurrencyList.Select(x => new CurrencyModel
        //    {
        //        CurrencyCode = TellerUtilityService.GetCurrencyCode(x.CTId),
        //        CurrencyName = x.CurrencyName + "(" + x.Country + ")",
        //        CTId = x.CTId
        //    }).ToList();

        //    var a = CodeCurrencyList.Where(x => x.CurrencyCode.Contains(currencyCode)).ToList();
        //    return a;
        //}
        public byte GetSchemeIdByProductId(int productId)
        {
            return uow.Repository<ProductDetail>().FindBy(x => x.PID == productId).Select(x => x.SDID).FirstOrDefault();

        }
        public IPagedList<ProductViewModel> GetAllProductList(string productCode, int pageNumber, int pageSize)
        {
            if (productCode == "")
            {
                var ProductList = uow.Repository<ProductDetail>().Queryable().Select(x => new ProductViewModel
                {
                    ProductCode = x.Apfix,
                    ProductId = x.PID,
                    ProductName = x.PName
                });
                return ProductList.OrderBy(x => x.ProductCode).ToPagedList(pageNumber, pageSize);
            }
            else
            {
                var ProductList = uow.Repository<ProductDetail>().FindByMany(x => x.Apfix.Contains(productCode)).Select(x => new ProductViewModel
                {
                    ProductCode = x.Apfix,
                    ProductId = x.PID,
                    ProductName = x.PName
                });
                return ProductList.OrderBy(x => x.ProductCode).ToPagedList(pageNumber, pageSize);
            }

        }

        public IPagedList<ProductViewModel> GetAllProductListByName(string productName, int pageNumber, int pageSize)
        {
            var ProductList = uow.Repository<ProductDetail>().FindByMany((x => x.PName.Contains(productName) || x.Apfix.Contains(productName))).Select(x => new ProductViewModel
            {
                ProductCode = x.Apfix,
                ProductId = x.PID,
                ProductName = x.PName
            });
            return ProductList.OrderBy(x => x.ProductCode).ToPagedList(pageNumber, pageSize);
        }

        public List<ProductViewModel> GetAllProductList(string productCode)
        {
            var ProductList = uow.Repository<ProductDetail>().FindByMany(x => x.Apfix.Contains(productCode)).Select(x => new ProductViewModel
            {
                ProductCode = x.Apfix,
                ProductId = x.PID,
                ProductName = x.PName
            });
            return ProductList.ToList();
        }

        public ProductViewModel GetSingleProduct(int productId)
        {
            var singleProduct = uow.Repository<ProductDetail>().FindBy(x => x.PID == productId).Select(x => new ProductViewModel
            {
                ProductCode = x.Apfix,
                ProductId = x.PID,
                IntraBrnhTrnx = x.IntraBrnhTrnx
            }).FirstOrDefault();
            return singleProduct;
        }

        public ProductDetail GetProductDetailsByProductId(int productId)
        {
            var productDetail = uow.Repository<ProductDetail>().SqlQuery("select * from fin.ProductDetail where PID = " + productId + "").FirstOrDefault();
            return productDetail;
        }


        //public CurrencyModel GetSingleCurrency(int currencyId)
        //{
        //    var CurrencyList = GetAllCurrencyList();
        //    var CodeCurrencyList = CurrencyList.Select(x => new CurrencyModel
        //    {
        //        CurrencyCode = TellerUtilityService.GetCurrencyCode(x.CTId),
        //        CurrencyName = x.CurrencyName + "(" + x.Country + ")",
        //        CTId = x.CTId
        //    }).Where(x => x.CTId == currencyId).FirstOrDefault();
        //    return CodeCurrencyList;
        //}

        public AccountDetailsViewModel GetCodeByAccountNumber(string accountNumber)
        {
            try
            {
                var Getsingle = uow.Repository<ADetail>().FindBy(x => x.Accno.Contains(accountNumber)).Select(x => new AccountDetailsViewModel()
                {
                    PID = Convert.ToByte(x.PID),
                    CurrID = x.CurrID,
                    BrchID = x.BrchID,
                    IAccno = x.IAccno
                }).FirstOrDefault();


                return Getsingle;
            }
            catch (Exception ex)
            {

                throw new Exception("");
            }
        }

        //GetAccountNumberForImage
        public AccountDetailsViewModel GetAccountNumber(string accountNumber)
        {
            try
            {
                var Getsingle = uow.Repository<ADetail>().FindBy(x => x.Accno.Equals(accountNumber)).Select(x => new AccountDetailsViewModel()
                {

                    IAccno = x.IAccno
                }).FirstOrDefault();


                return Getsingle;
            }
            catch (Exception ex)
            {

                throw new Exception("");
            }
        }


        //public List<AccountSearchViewModel> GetAccountNumberList(string accountNumber, int pageNo, int pageSize)
        //{

        //    string query = "";

        //    query = @"select COUNT(*) OVER () AS TotalCount,* from fin.FGetAccountNumberList()";
        //    if (accountNumber != "")
        //    {

        //        query += " where AccountNumber like '%" + accountNumber + "%'";
        //    }
        //    else
        //    {
        //        query += " where BrchID=1";
        //    }
        //    query += @" ORDER BY  AccountName
        //               OFFSET ((" + pageNo + @" - 1) * " + pageSize + @") ROWS
        //               FETCH NEXT " + pageSize + " ROWS ONLY";
        //    var accountSearchList = uow.Repository<AccountSearchViewModel>().SqlQuery(query).ToList();
        //    return accountSearchList;
        //}

        public List<AccountDetailsViewModel> GetAccountListByProductId(int? ProductId) //used in change product interest
        {

            string query = "";

            query = @"select * from fin.F`ountNumberList() where PID = " + ProductId + " ";

            //if (ProductId == null)
            //{
            //    //view all 
            //}
            //else
            //{
            //    query += "where PID = "+ProductId+"";
            //}
            //query += @" ORDER BY  AccountName
            //           OFFSET ((" + pageNo + @" - 1) * " + pageSize + @") ROWS
            //           FETCH NEXT " + pageSize + " ROWS ONLY";
            var accountSearchList = uow.Repository<AccountDetailsViewModel>().SqlQuery(query).ToList();
            return accountSearchList;
        }

        public ReturnBaseMessageModel DeleteUnverifiedLoanRegistration(long eventValue, int eventId)
        {
            using (var transaction = uow.GetContext().Database.BeginTransaction()

   )
            {
                try
                {

                    int UserId = Loader.Models.Global.UserId;
                    var singleunacceptreject = uow.Repository<ALRegistration>().FindBy(x => x.RegId == eventValue).SingleOrDefault();

                    if (singleunacceptreject.Status == 0) //if edit transaction is neither been accepted of rejected
                    {
                        uow.Repository<ALRegistration>().Delete(singleunacceptreject);
                    }

                    uow.Commit();
                    returnMessage.Msg = "Successfully  Deleted";
                    returnMessage.Success = true;
                    transaction.Commit();
                    return returnMessage;
                }
                catch (Exception ex)
                {

                    transaction.Dispose();
                    returnMessage.Msg = "Cannot Be Deleted";
                    returnMessage.Success = false;

                }
                return returnMessage;
            }
        }
        public ReturnBaseMessageModel DeleteRemitPayment(long eventValue, int eventId)
        {


            using (var transaction = uow.GetContext().Database.BeginTransaction()

   )
            {
                try
                {


                    var singleunacceptreject = uow.Repository<RemitPayment>().FindBy(x => x.Tno == eventValue).SingleOrDefault();
                    var userAssignMenu = uow.Repository<TaskViewModel>().SqlQuery(@"select RejectedOn from Mast.Task tk
                                                                          join Mast.TaskDetails td on td.Task1Id = tk.Task1Id 
                                                                          where EventValue = " + eventValue + " and EventId = " + eventId + " and RaisedBy =" + Global.UserId).FirstOrDefault();
                    if (userAssignMenu != null) //if edit transaction is neither been accepted of rejected
                    {
                        uow.Repository<RemitPayment>().Delete(singleunacceptreject);
                    }

                    uow.Commit();
                    returnMessage.Msg = "Successfully  Deleted";
                    returnMessage.Success = true;
                    transaction.Commit();
                    return returnMessage;
                }
                catch (Exception ex)
                {

                    transaction.Dispose();
                    returnMessage.Msg = "Cannot Be Deleted";
                    returnMessage.Success = false;

                }
                return returnMessage;
            }
        }


        public ReturnBaseMessageModel DeleteRemitDeposit(long eventValue, int eventId)
        {
            using (var transaction = uow.GetContext().Database.BeginTransaction()

   )
            {
                try
                {


                    var singleunacceptreject = uow.Repository<RemitDeposit>().FindBy(x => x.Tno == eventValue).SingleOrDefault();
                    var userAssignMenu = uow.Repository<TaskViewModel>().SqlQuery(@"select RejectedOn from Mast.Task tk
                                                                          join Mast.TaskDetails td on td.Task1Id = tk.Task1Id 
                                                                          where EventValue = " + eventValue + " and EventId = " + eventId + " and RaisedBy =" + Global.UserId).FirstOrDefault();
                    if (userAssignMenu != null) //if edit transaction is neither been accepted of rejected
                    {
                        uow.Repository<RemitDeposit>().Delete(singleunacceptreject);
                    }

                    uow.Commit();
                    returnMessage.Msg = "Successfully  Deleted";
                    returnMessage.Success = true;
                    transaction.Commit();
                    return returnMessage;
                }
                catch (Exception ex)
                {

                    transaction.Dispose();
                    returnMessage.Msg = "Cannot Be Deleted";
                    returnMessage.Success = false;

                }
                return returnMessage;
            }
        }
        public List<AccountSearchViewModel> GetAccountNumberList(int branchCode, int productCode, int currencyCode, string customerName, string filterAccount, string accountType, int pageNo, int pageSize)
        {
            AccountDetailsViewModel getAccountCode = new AccountDetailsViewModel();
            //string query = "";
            //if (filterAccount == EAccountFilter.WithdrawWithRevolving.GetDescription())
            //{
            //    query = @"select COUNT(*) OVER () AS TotalCount,* from
            //             (
            //               select * from fin.FGetAccountNumberList()
            //               where SType = 0
            //               union
            //               select * from fin.FGetAccountNumberList()
            //               where Revolving = 1
            //               )a where BrchID = " + branchCode + " and CurrID = " + currencyCode + " and AccState not in (6,3) ";
            //}
            //else
            //{

            //}
            //query = @"select * from fin.FGetAccountNumberList() where BrchID=" + branchCode + "and CurrID=" + currencyCode + " and AccState<>6";
            //if (productCode != 0)
            //{
            //    query += " and  PID=" + productCode;
            //}
            //if (customerName != "")
            //{
            //    query += " and (AccountName like '%" + customerName + "%' or AccountNumber like '%" + customerName + "%'  )";
            //}


            //if (filterAccount == EAccountFilter.Nominee.GetDescription())
            //{
            //    query += " and Nomianable=1";
            //}
            //else if (filterAccount == EAccountFilter.AllowCheque.GetDescription())
            //{
            //    query += "and HasCheque=1";
            //}
            //else if (filterAccount == EAccountFilter.FixedAccountOnly.GetDescription())
            //{
            //    query += " and Withdrawal=0 and MultiDep=0 and HasDuration=1";
            //}
            //if (accountType == EAccountType.Normal.GetDescription())
            //{
            //    query += " and  SType=0";
            //}
            //else if (accountType == EAccountType.Loan.GetDescription())
            //{
            //    query += " and  SType=1";
            //}
            //string FinalQuery = @";WITH Account_Table AS(
            //                         " + query + @"
            //                                              )
            //                              , Count_CTE AS (
            //               SELECT COUNT(*) AS[TotalCount]
            //             FROM Account_Table
            //               )";
            //FinalQuery += @"SELECT *
            //            FROM Account_Table, Count_CTE
            //           ORDER BY Account_Table.AccountName
            //           OFFSET ((" + pageNo + @" - 1) * " + pageSize + @") ROWS
            //           FETCH NEXT " + pageSize + " ROWS ONLY";


            var accountSearchList = uow.Repository<AccountSearchViewModel>().SqlQuery("exec [fin].[PSetAccountNumberListA] @currrencyId,@branchId,@productId,@filterName,@AccountNameOrNumber,@accountType,@pageNo,@pageSize",
                new SqlParameter("@currrencyId", currencyCode),
                new SqlParameter("@branchId", branchCode),
                new SqlParameter("@productId", productCode),
                new SqlParameter("@filterName", filterAccount),
                new SqlParameter("@AccountNameOrNumber", customerName),
                new SqlParameter("@accountType", accountType),
                new SqlParameter("@pageNo", pageNo),
                 new SqlParameter("@pageSize", pageSize)).ToList();

           
            if (filterAccount== "FixedAccountOnly")
            {
                foreach (var item in accountSearchList.ToList())
                {
                    bool isMature = TellerUtilityService.IsAlreadyMatured(item.AccountId);
                    AccountSearchViewModel FixedDeposit = item;
                    List<AccountSearchViewModel> accountSearch = new List<AccountSearchViewModel>();
                    if (isMature)
                    {
                        accountSearchList.Remove( FixedDeposit);
                    }
                }
            }
                return accountSearchList;
        }

        public List<AMClearenceViewModel> GetAllChequeClearenceList()
        {
            //string query = @"select accno,
            //   Bankname,
            //   camount,                           
            //   payee,
            //     from fin.ASClearance where chqstate is null and postedby=" + userId + " ";
            string query = "select * from [fin].[ASClearance]";
            var chequeClearenceList = uow.Repository<ASClearance>().SqlQuery(query).Select(x => new AMClearenceViewModel
            {
                AccountNumber = x.accno,

                payee = x.payee,
                camount = x.camount,
                bankname = x.Bankname

            }).ToList();

            return chequeClearenceList;

        }

        public AccountSearchViewModel GetSelectAccountNumber(Int64 accountNumber)
        {
            var getSingelAccountNumber = uow.Repository<ADetail>().FindByMany(x => x.IAccno == accountNumber).Select(x => new AccountSearchViewModel
            {
                AccountId = x.IAccno,
                AccountNumber = x.Accno,
                AccountName = x.Aname
            }).FirstOrDefault();

            return getSingelAccountNumber;
        }
        public List<AccountNumberViewModel> GetAccountNumber(string accountNumber, string filterAccount, string accountType)
        {
            try
            {
                //string query = "";


                //query = @"select distinct AccountId as IAccno,AccountNumber as Accno from fin.FGetAccountNumberList()  where AccountNumber like '%" + accountNumber + "%'";

                //if (filterAccount == EAccountFilter.Nominee.GetDescription())
                //{
                //    query += " and Nomianable=1";
                //}
                //else if (filterAccount == EAccountFilter.AllowCheque.GetDescription())
                //{
                //    query += " and HasCheque=1";
                //}
                //else if (filterAccount == EAccountFilter.FixedAccountOnly.GetDescription())
                //{
                //    query += " and Withdrawal=0 and MultiDep=0 and HasDuration=1";
                //}
                //if (accountType == EAccountType.Normal.GetDescription())
                //{
                //    query += " and  SType=0";
                //}
                //else if (accountType == EAccountType.Loan.GetDescription())
                //{
                //    query += " and  SType=1";
                //}
                var getManyAccountNumber = uow.Repository<AccountNumberViewModel>().SqlQuery("exec [fin].[PSetAccountNumberListB] @AccountNumber,@filterName,@accountType",
                     new SqlParameter("@AccountNumber", accountNumber),
                new SqlParameter("@filterName", filterAccount),
                new SqlParameter("@accountType", accountType)
                ).ToList();
                //return accountSearchList;
                //var getManyAccountNumber = uow.Repository<ADetail>().FindBy(x => x.Accno.Contains(accountNumber) && x.AccState != 6 && x.AccState != 3).Select(x => new AccountDetailsViewModel()
                //{
                //    Accno = x.Accno,
                //    IAccno = x.IAccno
                //}).ToList();


                return getManyAccountNumber;
            }
            catch (Exception ex)
            {

                throw new Exception("");
            }
        }
        #endregion

        #region Account Client Details
        public List<CustomerAccountsViewModel> GetAccountsWiseCustomer(Int64 IAccno = 0)
        {
            var customerAccounts = uow.Repository<CustomerAccountsViewModel>().SqlQuery(@"select IAccno,cl.CID,Name,Contact,location from fin.AOfCust ac 
                                                                                           inner join cust.FGetCustList() cl on ac.CID=cl.cid where IAccno = (" + IAccno + ")").ToList();

            return customerAccounts;
        }
        #endregion

        #region Account Verify
        public ReturnBaseMessageModel VerifyAccount(long IAccno = 0)
        {
            using (var scope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.RequiresNew))

            {
                try
                {

                    var cst = financeParameterService.ChargeTranxNoByEventIdAndEventValue(4, IAccno);

                    foreach (var item in cst)
                    {
                        financeParameterService.VerifyChargeTransaction(item.TrnxNo);
                    }
                    ADetail ad = uow.Repository<ADetail>().GetSingle(x => x.IAccno == IAccno);

                    if (ad.AMinBal != null)
                    {
                        ABal availableBal = new ABal();
                        availableBal.IAccno = ad.IAccno;
                        availableBal.Flag = 4;
                        availableBal.FId = commonService.GetFidByIAccno(ad.IAccno);
                        availableBal.Bal = ad.AMinBal.Minbal;
                        uow.Repository<ABal>().Add(availableBal);
                    }
                    ad.AccState = 1;
                    ad.ApprovedBy = Loader.Models.Global.UserId.ToString();
                    uow.Repository<ADetail>().Edit(ad);
                    commonService.AccountStatusLogChange(1, ad.IAccno);
                    uow.Commit();
                    scope.Complete();
                    returnMessage.Msg = "Account Verified Successfully";
                    returnMessage.Success = true;
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    returnMessage.Msg = "Account Cannot Verified Successfully" + ex.Message;
                    returnMessage.Success = false;

                }
                return returnMessage;
            }

        }
        public ReturnBaseMessageModel RejectAccount(long IAccno = 0)
        {
            //using (var scope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.RequiresNew))
            using (var scope = uow.BeginTransaction())
            {
                try
                {

                    ADetail aDetail = uow.Repository<ADetail>().FindByMany(x => x.IAccno == IAccno).FirstOrDefault();
                    aDetail.AccState = 8;
                    uow.Repository<ADetail>().Edit(aDetail);
                    uow.Commit();
                    scope.Commit();
                    returnMessage.Msg = "Account Rejected Successfully";
                    returnMessage.Success = true;
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    returnMessage.Msg = "Account Cannot Rejected" + ex.Message;
                    returnMessage.Success = false;

                }
                return returnMessage;
            }

        }
        public ReturnBaseMessageModel DeleteUnverifiedAccount(long IAccno, int eventId = 0)
        {
            //using (var scope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.RequiresNew))
            using (TransactionScope transaction = new TransactionScope(
                TransactionScopeOption.RequiresNew, new TransactionOptions()
                {
                    IsolationLevel = IsolationLevel.ReadCommitted
                }
                  ))
            {
                try
                {
                    int UserId = Loader.Models.Global.UserId;
                    if (eventId == 1)
                    {
                        int isSuccess = uow.ExecWithStoreProcedure("fin.PDelUnverifiedDepositAccount @Iaccno", new SqlParameter("@Iaccno", IAccno));
                    }
                    if (eventId == 19)
                    {
                        int isSuccess = uow.ExecWithStoreProcedure("[fin].[PDelUnverifiedLoanAccount] @Iaccno", new SqlParameter("@Iaccno", IAccno));

                    }
                    returnMessage.Msg = "Successfully Deleted";
                    returnMessage.Success = true;
                    transaction.Complete();
                    return returnMessage;
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    returnMessage.Msg = "Account Cannot Be Deleted" + ex.Message;
                    returnMessage.Success = false;

                }
                return returnMessage;
            }

        }

        #endregion

        #region DenoList
        public DenoInOutViewModel DenoList(int currencyId = 0)
        {
            DenoInOutViewModel denoInOutModel = new DenoInOutViewModel();
            int UserId = Loader.Models.Global.UserId;
            int UserLevel = 2;
            var deloList = uow.Repository<DenoInViewModel>().SqlQuery("select * from fin.FGetCurrentDenoList(" + UserId + "," + UserLevel + "," + currencyId + ")").ToList();

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
            denoInOutModel.DenoInList = deloList;
            denoInOutModel.DenoOutList = denoOutList;
            return denoInOutModel;
        }
        public DenoInOutViewModel DenoListByUser(int currencyId = 0, int UserId = 0)
        {
            DenoInOutViewModel denoInOutModel = new DenoInOutViewModel();
            //int UserId = Loader.Models.Global.UserId;
            int UserLevel = 2;
            var deloList = uow.Repository<DenoInViewModel>().SqlQuery("select * from fin.FGetCurrentDenoList(" + UserId + "," + UserLevel + "," + currencyId + ")").ToList();

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
            denoInOutModel.DenoInList = deloList;
            denoInOutModel.DenoOutList = denoOutList;
            return denoInOutModel;
        }

        #endregion

        #region Depost transaction
        public AccountDetailsShowViewModel GetAccountDetailsViewShow(Int64 accountId = 0)
        {
            var accountDetailsShow = uow.Repository<AccountDetailsShowViewModel>().SqlQuery("select * from fin.FGetAccountDetails(" + accountId + ")").FirstOrDefault();
            return accountDetailsShow;
        }
        public LoanAccountDetailsShowModel GetLoanOnlyAccountDetailsViewShow(Int64 accountId = 0)
        {
            var accountDetailsShow = uow.Repository<LoanAccountDetailsShowModel>().SqlQuery("select * from fin.FGetLoanAccountDetailsOnlyByIaccno(" + accountId + ")").FirstOrDefault();
            return accountDetailsShow;
        }

        public int GetSchemeIdByAccountId(Int64 accounId)
        {
            var schemeId = uow.Repository<ReturnSingleValueModdel>().SqlQuery(@"select s.SDID as ByteValue from fin.ADetail ad join fin.ProductDetail pd on ad.PID=pd.PID
                                                               join fin.SchmDetails s on s.SDID = pd.SDID where ad.IAccno =" + accounId).FirstOrDefault();
            return schemeId.ByteValue;
        }
        public ReturnBaseMessageModel InsertDepositTransaction(DepositViewModel depositModel, DenoInOutViewModel denoInOutModel, TaskVerificationList taskVerifierList)
        {
            using (var transaction = uow.GetContext().Database.BeginTransaction())
            {
                try
                {
                    returnMessage = TellerUtilityService.CheckUserActivateOrNot();
                    if (!returnMessage.Success)
                    {
                        return returnMessage;
                    }
                    returnMessage = TellerUtilityService.AccountNumberRequired(depositModel.AccountId);
                    if (returnMessage.Success == false)
                    {
                        return returnMessage;
                    }

                    //returnMessage = TellerUtilityService.IsVerifyPreviousTransaction(depositModel.AccountId);
                    //if (!returnMessage.Success)
                    //{
                    //    return returnMessage;
                    //}
                    int userBranchId = commonService.GetBranchIdByEmployeeUserId();
                    int AccountBranchId = commonService.AccountHolderBranchId(depositModel.AccountId);
                    DateTime accountBranchDate = commonService.GetOtherBranchTransactionDateByBranchId(AccountBranchId);
                    DateTime userBranchDate = commonService.GetBranchTransactionDate();


                    if (userBranchDate != accountBranchDate)
                    {
                        returnMessage.Msg = "Transaction Dates for the two Branches don't match. \n Cannot complete ABBS transaction.";
                        returnMessage.Success = false;
                        return returnMessage;

                    }
                    if (userBranchId == AccountBranchId)
                        depositModel.TType = 1;
                    else
                        depositModel.TType = 2;
                    int accounState = TellerUtilityService.GetAccountStatus(depositModel.AccountId);
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
                    returnMessage = TellerUtilityService.GetCheckValidAccountStatus(depositModel.AccountId);
                    if (!returnMessage.Success)
                    {
                        return returnMessage;
                    }
                    int productId = TellerUtilityService.GetPid(depositModel.AccountId);
                    bool IsFixedAccount = TellerUtilityService.IsFixedAccount(productId);
                    bool IsTrnsWithDeno = commonService.IsTransactionWithDeno();

                    if (IsFixedAccount)
                    {
                        decimal adrLimit = TellerUtilityService.DebitLimitAmount(depositModel.AccountId);
                        decimal currentAmount = TellerUtilityService.AvailableGoodBalanceWithShadowCr(depositModel.AccountId);
                        decimal addedAmount = currentAmount + depositModel.Amount;

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
                        DateTime maturedate = TellerUtilityService.GetCheckMatureDateForFixed(depositModel.AccountId);
                        if (maturedate != null)
                        {
                            if (commonService.GetBranchTransactionDate() >= maturedate)
                            {
                                returnMessage.Msg = "The account is already Matured.NO additional Deposit is allowed unless the account is Renewed.!!";
                                returnMessage.Success = false;
                                return returnMessage;
                            }
                        }
                       
                    }
                    if (TellerUtilityService.IsRecurringDeposit(productId))
                    {
                        DateTime maturedate = TellerUtilityService.GetCheckMatureDateForRecurring(depositModel.AccountId);
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
                    if (IsTrnsWithDeno)
                    {
                        if (!TellerUtilityService.BalanceWithDenoAmount(denoInOutModel, depositModel.Amount))
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
                    if (userBranchId != AccountBranchId)
                    {
                        bool allowABBS = Convert.ToBoolean(GetProductDetails(productId).IntraBrnhTrnx);
                        if (allowABBS == false)
                        {
                            returnMessage.Msg = "ABBS Deposit is not allowed for this Account.!!";
                            returnMessage.Success = false;
                            return returnMessage;
                        }
                    }
                    commonService.SaveUpdateMyBalance(0, depositModel.AmountCurrency, depositModel.Amount, Global.UserId);
                    if (depositModel.AmountCurrency == commonService.DefultCurrency())
                    {

                        ASTrn asTransaction = new ASTrn();
                        asTransaction.IAccno = depositModel.AccountId;
                        asTransaction.tdate = commonService.GetBranchTransactionDate();
                        asTransaction.notes = depositModel.DepositBy;
                        asTransaction.slpno = depositModel.ReceiptNo;
                        asTransaction.cramt = depositModel.Amount;
                        asTransaction.ttype = depositModel.TType;
                        asTransaction.postedby = Global.UserId;
                        asTransaction.IsSlp = true;
                        asTransaction.brnhno = commonService.GetBranchIdByEmployeeUserId();
                        Int64 transactionNumber = commonService.GetUtno();
                        asTransaction.tno = transactionNumber;
                        asTransaction.PostedOn = commonService.GetDate();
                        if (IsTrnsWithDeno)
                        {
                            commonService.DenoInOutTransaction(denoInOutModel, transactionNumber);
                        }
                        commonService.InsertAvailableBalance(2, depositModel.AccountId, depositModel.Amount);

                        uow.Repository<ASTrn>().Add(asTransaction);
                        uow.Commit();
                        taskUow.SaveTaskNotification(taskVerifierList, transactionNumber, 2);
                        transaction.Commit();
                        returnMessage.Success = true;
                        returnMessage.Msg = "Deposit save successfully with Transaction number #" + transactionNumber;
                        return returnMessage;
                    }
                    else
                    {
                        if (TellerUtilityService.AccountCurrency(depositModel.AccountId) == depositModel.AmountCurrency)
                        {
                            Int64 transactionNumber = commonService.GetUtno();
                            ForeignSBuySell foreignSByRow = new ForeignSBuySell();
                            foreignSByRow.BrnhId = commonService.GetBranchIdByEmployeeUserId();
                            foreignSByRow.CrAmt = depositModel.Amount;
                            foreignSByRow.Tdate = commonService.GetBranchTransactionDate();
                            foreignSByRow.InCurr = (short)depositModel.AmountCurrency;
                            foreignSByRow.PostedBy = (short)Global.UserId;
                            foreignSByRow.OutCurr = (short)commonService.DefultCurrency();
                            foreignSByRow.TNo = transactionNumber;
                            foreignSByRow.IAccNo = depositModel.AccountId;
                            foreignSByRow.slpno = depositModel.ReceiptNo;
                            foreignSByRow.notes = depositModel.DepositBy;
                            foreignSByRow.rate = Convert.ToDouble(GetCurrencyRateAndExchangeRate(depositModel.AmountCurrency).BuyingRate);
                            if (IsTrnsWithDeno)
                            {
                                commonService.DenoInOutTransaction(denoInOutModel, transactionNumber);
                            }
                            commonService.InsertAvailableBalance(2, depositModel.AccountId, depositModel.Amount);
                            uow.Repository<ForeignSBuySell>().Add(foreignSByRow);
                            taskUow.SaveTaskNotification(taskVerifierList, transactionNumber, 27);
                            uow.Commit();
                            transaction.Commit();
                            returnMessage.Success = true;
                            returnMessage.Msg = "Deposit save successfully with Transaction number #" + transactionNumber;
                            return returnMessage;
                        }
                        else
                        {
                            returnMessage.Success = false;
                            returnMessage.Msg = "Currency Not match!!";
                            return returnMessage;
                        }


                    }

                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    returnMessage.Success = false;
                    returnMessage.Msg = "Deposit not save.!!" + ex.Message + "-Extra Error-" + ex.InnerException.Message;
                    return returnMessage;
                }
            }
        }
        public ReturnBaseMessageModel DeleteUnverifiedTransactions(int Tno = 0, int transType = 0)
        {
            //using (var scope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.RequiresNew))
            using (TransactionScope transaction = new TransactionScope(
              TransactionScopeOption.RequiresNew, new TransactionOptions()
              {
                  IsolationLevel = IsolationLevel.ReadCommitted
              }
                ))
            {
                string MSG = "";
                try
                {
                    int UserId = Loader.Models.Global.UserId;
                    bool IsTrnsWithDeno = commonService.IsTransactionWithDeno();
                    int postedby = 0;
                    decimal tno = Convert.ToDecimal(Tno);
                    postedby = uow.Repository<ASTrn>().FindByMany(x => x.tno == Tno).Select(x => x.postedby).FirstOrDefault();
                    if (IsTrnsWithDeno)
                    {
                        DenoInViewModel denoViewModel = new DenoInViewModel();
                        denoViewModel.DenoInList = uow.Repository<DenoTrxn>().FindBy(x => x.Trxno == tno).Select(x => new DenoInViewModel
                        {
                            DenoID = x.Denoid,
                            Piece = x.Pics
                        }).ToList();
                        commonService.DenoUnverifiedDelete(denoViewModel, Tno, postedby);
                    }
                    int isSuccess = 0;
                    if (transType == 2)//for deposit event is 2
                    {
                        MSG = "Deposit Transaction";
                        isSuccess = uow.ExecWithStoreProcedure("fin.PDelUnverifiedDeposit @Tno", new SqlParameter("@Tno", Tno));
                    }
                    else if (transType == 3)//for deposit event is 3
                    {
                        isSuccess = uow.ExecWithStoreProcedure("fin.PDelUnverifiedWithdraw @Tno", new SqlParameter("@Tno", Tno));
                        MSG = "Withdraw Transaction";
                    }
                    else if (transType == 8)//for manual charge event is 7
                    {
                        isSuccess = uow.ExecWithStoreProcedure("[Trans].[PDelManualCharge] @Tno", new SqlParameter("@Tno", Tno));
                        MSG = "Manual Charge Transaction";
                    }
                    else if (transType == 25)//for share purchase event is 25
                    {
                        isSuccess = uow.ExecWithStoreProcedure("[Fin].[PDelUnverifiedSharePurchase] @tno", new SqlParameter("@tno", Tno));
                        MSG = "Share Purchase Transaction";
                    }
                    else if (transType == 26)//for share purchase event is 26
                    {
                        isSuccess = uow.ExecWithStoreProcedure("[Fin].[PDelUnverifiedShareReturn] @tno", new SqlParameter("@tno", Tno));
                        MSG = "Share Return Transaction";
                    }

                    returnMessage.Msg = MSG + "Successfully be Deleted";

                    returnMessage.Success = true;
                    transaction.Complete();
                    return returnMessage;
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    returnMessage.Msg = MSG + "Cannot Be Deleted" + ex.Message;
                    returnMessage.Success = false;

                }
                return returnMessage;
            }

        }

        public ReturnBaseMessageModel DeleteUnVerifiedTellerExceedPayment(int Rno = 0)
        {

            try
            {
                int isSuccess = uow.ExecWithStoreProcedure("[Fin].[PDelTellerExceedPayment] @Rno", new SqlParameter("@Rno", Rno));
                returnMessage.Msg = "Teller Exceed Payment Successfully Deleted";
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

        public List<AccountBalanceViewModel> GetAccountBalanceByFlag(Int64 iAccno = 0)
        {
            var balance = uow.Repository<ABal>().Queryable();
            var flag = uow.Repository<Flag>().Queryable();
            var accountBalance = from ab in balance
                                 join f in flag on ab.Flag equals f.FId
                                 where ab.IAccno == iAccno
                                 select new AccountBalanceViewModel()
                                 {
                                     Amount = ab.Bal,
                                     FlagName = f.FlagName,
                                     FlagId = f.FId

                                 };
            return accountBalance.ToList();
        }

        public List<ASTrnViewModel> GetUnverifiedTransactionList(string accountNumber, int pageNo, int PageSize)
        {

            string query = @"select 
                             sd.SType as AccountType,
                             ad.accno as accountnumber,
                            ast.tdate,
                            tno,
                            notes,
                            slpno,
                            dramt,
                            cramt,
                            TtypeDesc as ttype
                              from Trans.ASTrns ast
                            join fin.ADetail ad on ad.IAccno = ast.iaccno
                            join trans.TType tt on tt.Id = ast.ttype
                            join fin.ProductDetail pd on pd.pid = ad.pid
                            join fin.SchmDetails sd on sd.SDID = pd.SDID
                             where ast.brnhno = " + commonService.GetBranchIdByEmployeeUserId();


            if (!string.IsNullOrEmpty(accountNumber))
            {
                query += "and accno='" + accountNumber + "'";
            }


            string finalQuery = @";WITH TransTable_Table AS(
                                   " + query + @"
                                                          )
                                          , Count_CTE AS (
                           SELECT COUNT(*) AS[TotalCount]
                         FROM TransTable_Table
                           )
 

                          SELECT *
                        FROM TransTable_Table, Count_CTE
                       ORDER BY TransTable_Table.accountnumber
                       OFFSET ((" + pageNo + @" - 1) * " + PageSize + @") ROWS
                       FETCH NEXT " + PageSize + " ROWS ONLY";


            var unverifiedList = uow.Repository<ASTrnViewModel>().SqlQuery(finalQuery).ToList();

            return unverifiedList;
        }
        //public List<UnVerifiedTransactionModel> UnverifiedWithdrawDepositReport(string accountNumber, int pageNo, int PageSize)
        //{
        //    int branchId = commonService.GetBranchIdByEmployeeUserId();
        //    string sqlQuery = "select * from fin.FGetReportUnverifiedTransaction({0})";

        //    var Transaction = uow.Repository<UnVerifiedTransactionModel>().SqlQuery(sqlQuery, branchId).ToList();
        //    return Transaction;
        //}
        public ASTrnViewModel LocalCurrencyUnVerifiedTransaction(Int64 utno)
        {
            ASTrnViewModel unverifiedList = new ASTrnViewModel();
            var aSTrn = uow.Repository<ASTrn>().Queryable();
            var aDetail = uow.Repository<ADetail>().Queryable();
            var currencyType = uow.Repository<CurrencyType>().Queryable();
            var query = from a in aSTrn
                        join ac in aDetail on a.IAccno equals ac.IAccno
                        join c in currencyType on ac.CurrID equals c.CTId
                        where a.tno == utno
                        select new ASTrnViewModel()
                        {
                            IAccno = a.IAccno,
                            tdate = a.tdate,
                            tno = a.tno,
                            notes = a.notes,
                            slpno = a.slpno,
                            dramt = a.dramt,
                            cramt = a.cramt,
                            currency = c.CurrencyName + "(" + c.Country + ")",
                            IsSlp = a.IsSlp

                        };

            unverifiedList = query.FirstOrDefault();
            if (unverifiedList == null)
            {
                var amTrn = uow.Repository<AMTrn>().Queryable();
                var query1 = from a in amTrn
                             join ac in aDetail on a.IAccno equals ac.IAccno
                             join c in currencyType on ac.CurrID equals c.CTId
                             where a.tno == utno
                             select new ASTrnViewModel()
                             {
                                 IAccno = a.IAccno,
                                 tdate = a.tdate,
                                 tno = a.tno,
                                 notes = a.notes,
                                 slpno = a.slpno,
                                 dramt = a.dramt,
                                 cramt = a.cramt,
                                 currency = c.CurrencyName + "(" + c.Country + ")",
                                 IsSlp = a.Isslp
                             };

                unverifiedList = query1.FirstOrDefault();
            }


            return unverifiedList;
        }

        public ASTrnViewModel ForeignCurrencyUnVerifiedTransaction(Int64 utno)
        {
            ASTrnViewModel unverifiedList = new ASTrnViewModel();

            unverifiedList = (from a in uow.Repository<ForeignSBuySell>().GetAll()
                              join ac in uow.Repository<ADetail>().GetAll() on a.IAccNo equals ac.IAccno
                              join c in uow.Repository<CurrencyType>().GetAll() on ac.CurrID equals c.CTId
                              //join d in uow.Repository<CurrencyType>().GetAll() on a.OutCurr equals d.CTId
                              where a.TNo == utno
                              select new ASTrnViewModel()
                              {
                                  IAccno = a.IAccNo,
                                  tdate = a.Tdate,
                                  tno = a.TNo,
                                  notes = a.notes,
                                  slpno = a.slpno,
                                  dramt = a.DrAmt,
                                  cramt = a.CrAmt,
                                  currency = c.CurrencyName + "(" + c.Country + ")",
                                  IsSlp = a.IsSlp

                              }
                               ).FirstOrDefault();




            return unverifiedList;
        }
        public ASTrnViewModel GetSingleUnverifiedTransaction(Int64 utno)
        {
            ASTrnViewModel transactionDetails = new ASTrnViewModel();
            if (CurrencyByTno(utno) == commonService.DefultCurrency())
            {
                transactionDetails = LocalCurrencyUnVerifiedTransaction(utno);
            }
            else
            {
                transactionDetails = ForeignCurrencyUnVerifiedTransaction(utno);
            }
            return transactionDetails;
        }
        public int CurrencyByTno(Int64 tno)
        {
            var query = from a in uow.Repository<ASTrn>().Queryable()
                        join ac in uow.Repository<ADetail>().Queryable() on a.IAccno equals ac.IAccno
                        where a.tno == tno
                        select new { ac.CurrID }
                     ;
            var currencyTno = query.FirstOrDefault();
            if (currencyTno != null)
                return currencyTno.CurrID;
            else
                return Convert.ToInt32(0);
        }
        public List<DenoInViewModel> GetDenoByTransactionNumber(Int64 utno)
        {
            var denoNumberList = (from dt in uow.Repository<DenoTrxn>().FindBy(x => x.Trxno == utno)
                                  join d in uow.Repository<Denosetup>().GetAll() on dt.Denoid equals d.DenoID
                                  join c in uow.Repository<CurrencyType>().GetAll() on d.CurrID equals c.CTId
                                  where dt.Trxno == utno
                                  select new DenoInViewModel()
                                  {
                                      Deno = d.Deno,
                                      Piece = dt.Pics,
                                      Currency = c.CurrencyName + "(" + c.Country + ")",
                                      DenoID = d.DenoID

                                  }).ToList();


            return denoNumberList;
        }
        public ReturnBaseMessageModel VerifyDepositWithdrawTransaction(Int64 tNo)
        {
            try
            {
                string UserId = Global.UserId.ToString();
                if (CurrencyByTno(tNo) == commonService.DefultCurrency())
                {
                    uow.ExecWithStoreProcedure("[Trans].[PSetDepositWithdrawVerify] @TNO,@APPROVEDBY", new SqlParameter("@TNO", tNo), new SqlParameter("@APPROVEDBY", UserId));
                }
                returnMessage.Success = true;

            }
            catch (Exception)
            {
                returnMessage.Success = false;
            }
            return returnMessage;
        }

        #endregion

        #region Withdraw transaction
        public ReturnBaseMessageModel WithdrawTransaction(WithdrawViewModel withdrawModel, DenoInOutViewModel denoInOutModel, TaskVerificationList taskVerifier, InterestPayableViewModel intPayModel, ScheduleTrialModel scheduleTrialModel)
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
                        returnMessage = TellerUtilityService.CheckUserActivateOrNot();
                        if (!returnMessage.Success)
                        {
                            return returnMessage;
                        }
                        //returnMessage = TellerUtilityService.IsVerifyPreviousTransaction(withdrawModel.AccountId);
                        //if (!returnMessage.Success)
                        //{
                        //    return returnMessage;
                        //}
                        Int64 genereteTno = 0;
                        bool IsTrnsWithDeno = commonService.IsTransactionWithDeno();

                        int userBranchId = commonService.GetBranchIdByEmployeeUserId();
                        int AccountBranchId = commonService.AccountHolderBranchId(withdrawModel.AccountId);
                        DateTime userBranchDate = commonService.GetBranchTransactionDate();
                        DateTime accountBranchDate = commonService.GetOtherBranchTransactionDateByBranchId(AccountBranchId);

                        if (userBranchDate != accountBranchDate)
                        {
                            returnMessage.Msg = "Transaction Dates for the two Branches don't match. \n Cannot complete ABBS transaction.";
                            returnMessage.Success = false;
                            return returnMessage;

                        }
                        var AccountDetails = uow.Repository<ADetail>().FindBy(x => x.IAccno == withdrawModel.AccountId).Select(x => new AccountDetailsViewModel()
                        {
                            BrchID = x.BrchID,
                            Bal = x.Bal,
                            PID = (x.PID),
                            AccState = x.AccState

                        }).FirstOrDefault();
                        if (withdrawModel.Amount != 0 && intPayModel.InterestAmount != 0 && withdrawModel.Payee != "" && intPayModel.InterestPayee != "")
                        {
                            returnMessage.Msg = "Please fill either withdraw or interest payable.!! \n Both is NOT Allowed. ";
                            returnMessage.Success = false;
                            return returnMessage;
                        }
                        decimal goodBalance = 0;

                        //if (TellerUtilityService.IsFixedAccount(AccountDetails.PID))
                        //{
                        //    bool durationComplete = TellerUtilityService.IsDurationComplete(withdrawModel.AccountId);
                        //    if (!durationComplete)
                        //    {
                        //        returnMessage.Msg = "Withdraw is not Allowed.! \n Account NOT Matured Yet..";
                        //        returnMessage.Success = false;
                        //        return returnMessage;
                        //    }
                        //}

                        if (withdrawModel.IsActiveWithdraw)
                        {
                            bool isFixedAccount = TellerUtilityService.IsFixedAccount(AccountDetails.PID);
                            bool isRecurring = TellerUtilityService.IsOtherTypeOfRecurringDeposit(AccountDetails.PID);
                            if (isFixedAccount || isRecurring)
                            {
                                bool isMature = TellerUtilityService.IsAlreadyMatured(withdrawModel.AccountId);
                                if (AccountDetails.AccState != 7)
                                {
                                    if (isMature) // !isMature replace with isMature
                                    {
                                        returnMessage.Msg = "Withdraw is not Allowed.! \n Account NOT Matured Yet..!!";
                                        returnMessage.Success = false;
                                        return returnMessage;
                                    }
                                }
                            }
                            bool isGoodForPayment = TellerUtilityService.IsCheckUsedInGoodForPayment(withdrawModel.AccountId, withdrawModel.ChequeNumber);
                            if (isGoodForPayment)
                            {
                                var goodForPayment = GetGoodForPaymentDetails(withdrawModel.AccountId, withdrawModel.ChequeNumber);
                                if (goodForPayment.amount != withdrawModel.Amount)
                                {
                                    returnMessage.Msg = "Good for payment amount not match with withdraw amount. ";
                                    returnMessage.Success = false;
                                    return returnMessage;
                                }
                                if (goodForPayment.payee != withdrawModel.Payee)
                                {
                                    returnMessage.Msg = "Good for payment payee not match with withdraw payee. ";
                                    returnMessage.Success = false;
                                    return returnMessage;
                                }
                            }
                            //bool isRevolving = TellerUtilityService.IsRevolving(withdrawModel.AccountId);
                            #region input field Validation
                            if (withdrawModel.Amount == 0 || withdrawModel.Payee == "" || withdrawModel.Payee == null)
                            {
                                returnMessage.Msg = "Please fill all withdraw field. ";
                                returnMessage.Success = false;
                                return returnMessage;
                            }
                            if (withdrawModel.Amount < 0)
                            {
                                returnMessage.Msg = "Negative value is not Allowed. ";
                                returnMessage.Success = false;
                                return returnMessage;
                            }
                            if (withdrawModel.withdraw == "Cheque")
                            {
                                if (withdrawModel.ChequeNumber == 0)
                                {
                                    returnMessage.Msg = "Cheque Number is Required. ";
                                    returnMessage.Success = false;
                                    return returnMessage;
                                }
                            }
                            else
                            {
                                if (withdrawModel.SlipNo == 0)
                                {
                                    returnMessage.Msg = "Withdraw slip No is Required. ";
                                    returnMessage.Success = false;
                                    return returnMessage;
                                }
                            }
                            returnMessage = TellerUtilityService.GetCheckValidAccountStatus(withdrawModel.AccountId);
                            if (!returnMessage.Success)
                            {
                                return returnMessage;
                            }
                            if (AccountDetails.AccState == 5)//Debit Restricted
                            {
                                returnMessage.Msg = "Withdraw Faild.This Account is in Debit Restricted state.!!";
                                returnMessage.Success = false;
                                return returnMessage;
                            }
                            #endregion

                            //teller current available balance
                            decimal userCurrentBalance = TellerUtilityService.UserCurrentBalance(commonService.DefultCurrency());

                            if (userCurrentBalance < withdrawModel.Amount)
                            {
                                returnMessage.Msg = "Request amount exceeds available Cash!. ";
                                returnMessage.Success = false;
                                return returnMessage;
                            }
                            #region Cheque Validation
                            if (withdrawModel.withdraw == "Cheque")
                            {
                                returnMessage = TellerUtilityService.CheckChequeValidation(withdrawModel.ChequeNumber, withdrawModel.AccountId);
                                if (!returnMessage.Success)
                                {
                                    return returnMessage;
                                }
                            }
                            #endregion
                            if (withdrawModel.withdraw == "Cheque")
                            {
                                withdrawModel.SlipNo = withdrawModel.ChequeNumber;
                                withdrawModel.IsSlp = false;
                            }
                            else
                            {
                                withdrawModel.SlipNo = withdrawModel.SlipNo;
                                withdrawModel.IsSlp = true;
                            }
                            //if (isRevolving)
                            //{

                            //    if (TellerUtilityService.CheckMaturityDate(withdrawModel.AccountId))
                            //    {
                            //        returnMessage.Msg = "Account already matured. Withdraw not allowed.";
                            //        returnMessage.Success = false;
                            //        return returnMessage;
                            //    }

                            //    decimal allowedWithdraw = TellerUtilityService.RevolvingAvailableBalance(withdrawModel.AccountId);
                            //    if (allowedWithdraw < 0)
                            //    {
                            //        returnMessage.Msg = "Sanctioned Amount is less than Principal Outstanding.Withdraw is not allowed.";
                            //        returnMessage.Success = false;
                            //        return returnMessage;
                            //    }
                            //    if (allowedWithdraw < withdrawModel.Amount)
                            //    {
                            //        returnMessage.Msg = "Withdraw amount is greater than Allowed withdraw amount.Withdraw is not allowed.";
                            //        returnMessage.Success = false;
                            //        return returnMessage;
                            //    }
                            //    bool tellerAmount = TellerUtilityService.IsTellerAmountExceed(withdrawModel.Amount);
                            //    if (scheduleTrialModel.scheduleList == null)
                            //    {

                            //        returnMessage.Msg = "please add loan schedle trial schedule.";
                            //        returnMessage.Success = false;
                            //        return returnMessage;
                            //    }
                            //    if (tellerAmount)
                            //    {
                            //        TellerAmountExceedSave(withdrawModel, taskVerifier);
                            //        commonService.SaveLoanTempSchedule(scheduleTrialModel, withdrawModel.AccountId);
                            //        transaction.Complete();
                            //        returnMessage.Success = true;
                            //        returnMessage.Msg = "Posted for higher approval.!!";
                            //        return returnMessage;
                            //    }
                            //    else
                            //    {

                            //        Int64 transactionNumber = commonService.GetUtno();
                            //        genereteTno = transactionNumber;
                            //        WithdrawTransactionSaveRevolving(withdrawModel, taskVerifier, transactionNumber);
                            //        if (IsTrnsWithDeno)
                            //        {
                            //            commonService.DenoInOutTransaction(denoInOutModel, transactionNumber);
                            //        }
                            //        ASTransLoan asTransLoan = new ASTransLoan();
                            //        asTransLoan.tno = transactionNumber;
                            //        asTransLoan.PId = 10;
                            //        asTransLoan.Amt = withdrawModel.Amount;
                            //        uow.Repository<ASTransLoan>().Add(asTransLoan);
                            //        commonService.SaveLoanTempSchedule(scheduleTrialModel, withdrawModel.AccountId);
                            //        commonService.SaveUpdateMyBalance(Global.UserId, commonService.DefultCurrency(), withdrawModel.Amount, 0);
                            //    }
                            //}
                            //else
                            //{
                            #region payment Amount Validation
                            //account holder total available current balance which exclude reserve amount freez and transaction shadow debit amount
                            decimal availableBalance = TellerUtilityService.AvailableBalance(withdrawModel.AccountId);
                            decimal maxAvailableAmount = availableBalance;

                            if (AccountDetails.AccState == 7)//Ready to be close
                            {
                                goodBalance = TellerUtilityService.GoodBalance(withdrawModel.AccountId);
                                maxAvailableAmount = goodBalance;
                            }
                            //Guarantor block amount check with payment amount
                            returnMessage = GuarantorBlockAmount(withdrawModel.AccountId, withdrawModel.Amount, maxAvailableAmount);
                            if (!returnMessage.Success)
                            {
                                return returnMessage;
                            }
                            bool tellerAmount = TellerUtilityService.IsTellerAmountExceed(withdrawModel.Amount);
                            //available  balance check with payment amount
                            if (maxAvailableAmount < withdrawModel.Amount)
                            {
                                if (AccountDetails.AccState != 7)
                                {
                                    byte overDrawnFacility = TellerUtilityService.HasOverDrawnFacility(withdrawModel.AccountId, withdrawModel.Amount, maxAvailableAmount);
                                    if (overDrawnFacility == 0 || overDrawnFacility == 3)//no Overdrawn Facility or odlimit amount is 0
                                    {
                                        returnMessage.Msg = "Withdraw NOT Allowed.!! \n Payment amount cannot be greater than available balance.";
                                        returnMessage.Success = false;
                                        return returnMessage;
                                    }
                                    else if (overDrawnFacility == 1) // For account having Overdrawn Facility, Post for higher approval
                                    {
                                        tellerAmount = true;
                                    }
                                    else
                                    {
                                        returnMessage.Msg = "Withdraw NOT Allowed.! \n Overdrawn Limit Expired. Payment amount cannot be greater than available balance.";
                                        returnMessage.Success = false;
                                        return returnMessage;
                                    }
                                }
                                else
                                {
                                    //  if(maxAvailableAmount!= withdrawModel.Amount)
                                    returnMessage.Msg = "Withdraw NOT Allowed.! \n Amount is greater than Good balance.";
                                    returnMessage.Success = false;
                                    return returnMessage;
                                }
                            }
                            #endregion

                            if (userBranchId == AccountBranchId)
                                withdrawModel.TType = 1;
                            else
                                withdrawModel.TType = 2;

                            if (tellerAmount)
                            {
                                TellerAmountExceedSave(withdrawModel, taskVerifier, isGoodForPayment);
                                transaction.Commit();
                                returnMessage.Success = true;
                                returnMessage.Msg = "Posted for higher approval.!!";
                                return returnMessage;
                            }
                            else
                            {
                                if (IsTrnsWithDeno)
                                {
                                    if (!TellerUtilityService.BalanceWithDenoAmount(denoInOutModel, withdrawModel.Amount))
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
                                Int64 transactionNumber = commonService.GetUtno();
                                genereteTno = transactionNumber;

                                WithdrawTransactionSave(withdrawModel, taskVerifier, transactionNumber, isGoodForPayment);
                                commonService.SaveUpdateMyBalance(Global.UserId, commonService.DefultCurrency(), withdrawModel.Amount, 0);
                                if (IsTrnsWithDeno)
                                {
                                    commonService.DenoInOutTransaction(denoInOutModel, transactionNumber);
                                }
                                if (AccountDetails.AccState == 7 && withdrawModel.Amount == goodBalance)//Ready to be close
                                {
                                    var accountRow = uow.Repository<ADetail>().FindBy(x => x.IAccno == withdrawModel.AccountId).FirstOrDefault();
                                    accountRow.AccState = 3;
                                    uow.Repository<ADetail>().Edit(accountRow);
                                    uow.Commit();
                                    AccountClosedSave(withdrawModel.AccountId);
                                    commonService.AccountStatusLogChange(3, withdrawModel.AccountId);
                                }

                            }
                            //}
                        }
                        else if (intPayModel.IsActiveInterest)//Payable Interest Payment
                        {
                            if (intPayModel.InterestAmount == 0 || intPayModel.InterestPayee == "" || intPayModel.InterestPayee == null)
                            {
                                returnMessage.Msg = "Please fille all interest payable field. ";
                                returnMessage.Success = false;
                                return returnMessage;
                            }
                            var interestAvailable = InterestPayable(withdrawModel.AccountId);
                            decimal sumInterest = interestAvailable.Amount - intPayModel.InterestAmount;
                            if (sumInterest < 0)
                            {
                                returnMessage.Msg = "Withdraw NOT Allowed.!! \n Amount is greater then available amount.";
                                returnMessage.Success = false;
                                return returnMessage;
                            }
                            //check for teller current available balance
                            decimal userCurrentBalance = TellerUtilityService.UserCurrentBalance(commonService.DefultCurrency());
                            if (userCurrentBalance < intPayModel.InterestAmount)
                            {
                                returnMessage.Msg = "Request amount exceeds available Cash!. ";
                                returnMessage.Success = false;
                                return returnMessage;
                            }

                            if (userBranchId != AccountBranchId)
                            {
                                returnMessage.Msg = "Payble Interest Payment can only be done by same branch where account is registered.!!!";
                                returnMessage.Success = false;
                                return returnMessage;
                            }
                            if (IsTrnsWithDeno)
                            {
                                if (!TellerUtilityService.BalanceWithDenoAmount(denoInOutModel, intPayModel.InterestAmount))
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

                            Int64 transactionNumber = commonService.GetUtno();
                            genereteTno = transactionNumber;

                            AccountInterestPayableSave(intPayModel, transactionNumber, withdrawModel.AccountId, taskVerifier);
                            if (IsTrnsWithDeno)
                            {
                                commonService.DenoInOutTransaction(denoInOutModel, transactionNumber);
                            }
                            commonService.SaveUpdateMyBalance(Global.UserId, commonService.DefultCurrency(), intPayModel.InterestAmount, 0);
                        }

                        transaction.Commit();
                        returnMessage.Success = true;
                        returnMessage.Msg = "withdraw save successfully with Transaction number #" + genereteTno;
                        return returnMessage;


                    }
                    catch (Exception ex)
                    {
                        transaction.Dispose();
                        returnMessage.Success = false;
                        returnMessage.Msg = "withdraw not save.!!" + ex.Message;
                        return returnMessage;
                    }
                }
            }

        }
        public ReturnBaseMessageModel GuarantorBlockAmount(int iaccNo = 0, decimal amount = 0, decimal availableBalance = 0)
        {
            decimal blockAmount = TellerUtilityService.CheckForGuarantor(iaccNo);
            if (blockAmount != -1)
            {
                decimal totalBlockAmount = blockAmount + amount;
                if (totalBlockAmount > availableBalance)
                {
                    returnMessage.Msg = "Amount equivalent to  " + blockAmount + " is blocked for this account.Payment amount cannot be greater than available balance.";
                    returnMessage.Success = false;
                    return returnMessage;
                }
                else
                {
                    returnMessage.Success = true;
                    return returnMessage;
                }
            }
            else
            {
                returnMessage.Success = true;
                return returnMessage;
            }
        }
        public void AccountClosedSave(int iaccNo)
        {
            Aclosed aclosedRow = new Aclosed();
            aclosedRow.Iaccno = iaccNo;
            aclosedRow.Vdate = commonService.GetBranchTransactionDate();
            aclosedRow.postby = Global.UserId;
            uow.Repository<Aclosed>().Add(aclosedRow);
            decimal minBal = TellerUtilityService.GetMinBalance(iaccNo);
            commonService.InsertAvailableBalance(5, iaccNo, (-minBal));
            uow.Commit();
        }

        //public bool GetGuarantorAccount(int Iaccno) // by dorje... if not deleted until 4th june 2018 kindly delete it
        //{
        //    var guarantorAccount = uow.Repository<GuarantorModel>().SqlQuery(@"select * from fin.Guarantor where GIaccno = " + Iaccno + ")").FirstOrDefault(); 
        //    if(guarantorAccount == null)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}

        public List<GuarantorModel> GetGuarantorList(int Iaccno)
        {
            var guarantorList = uow.Repository<GuarantorModel>().SqlQuery(@"select * from fin.FCheckIsGuarantorOrLoanee(" + Iaccno + ")").ToList();
            return guarantorList;
        }
        public List<GuarantorModel> GetGuarantorListForLoanAccount(int Iaccno)
        {
            var guarantorList = uow.Repository<GuarantorModel>().SqlQuery(@"select * from fin.FCheckGurantor(" + Iaccno + ")").ToList();
            return guarantorList;
        }
        private void TellerAmountExceedSave(WithdrawViewModel withdrawModel, TaskVerificationList taskVerifier, bool isGoodForPayment)
        {
            try
            {
                AWtdQueue awtdQue = new AWtdQueue();
                awtdQue.IAccno = withdrawModel.AccountId;
                awtdQue.tdate = commonService.GetBranchTransactionDate();
                awtdQue.notes = withdrawModel.Payee;
                if (withdrawModel.withdraw == "Cheque")
                {
                    awtdQue.chqno = withdrawModel.ChequeNumber;
                    awtdQue.isslip = false;
                    commonService.UpdateTnoInChqNumber(withdrawModel.ChequeNumber, 0);
                }
                else
                {
                    awtdQue.chqno = withdrawModel.SlipNo;
                    awtdQue.isslip = true;
                }
                awtdQue.amount = withdrawModel.Amount;
                awtdQue.ttype = withdrawModel.TType;
                awtdQue.postedby = Global.UserId;
                awtdQue.BrnhID = commonService.GetBranchIdByEmployeeUserId();

                awtdQue.Qstate = 1;
                awtdQue.currid = commonService.DefultCurrency();
                uow.Repository<AWtdQueue>().Add(awtdQue);

                if (!isGoodForPayment)
                {
                    commonService.InsertAvailableBalance(5, withdrawModel.AccountId, withdrawModel.Amount);
                }

                uow.Commit();
                if (taskVerifier.Message == null || taskVerifier.Message == "")
                {
                    taskVerifier.Message = "Teller cash amount exceed verification message.!!";
                }
                taskVerifier.IsExceedAmount = true;

                taskUow.SaveTaskNotification(taskVerifier, awtdQue.Rno, 11);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void WithdrawTransactionSave(WithdrawViewModel withdrawModel, TaskVerificationList taskVerifier, Int64 transactionNumber, bool isGoodForPayment)
        {
            try
            {
                int UserId = Global.UserId;
                int branchId = commonService.GetBranchIdByEmployeeUserId();

                ASTrn asTransaction = new ASTrn();
                asTransaction.IAccno = withdrawModel.AccountId;
                asTransaction.tdate = commonService.GetBranchTransactionDate();
                asTransaction.notes = withdrawModel.Payee;

                asTransaction.slpno = withdrawModel.SlipNo;
                asTransaction.IsSlp = withdrawModel.IsSlp;
                asTransaction.dramt = withdrawModel.Amount;
                asTransaction.ttype = withdrawModel.TType;
                asTransaction.postedby = UserId;
                asTransaction.brnhno = branchId;
                asTransaction.tno = transactionNumber;
                asTransaction.PostedOn = commonService.GetDate();
                uow.Repository<ASTrn>().Add(asTransaction);
                if (!withdrawModel.IsSlp)
                {
                    commonService.UpdateTnoInChqNumber(withdrawModel.ChequeNumber, transactionNumber);
                }
                if (isGoodForPayment)
                {
                    commonService.InsertAvailableBalance(5, withdrawModel.AccountId, (-withdrawModel.Amount));
                    commonService.InsertAvailableBalance(1, withdrawModel.AccountId, withdrawModel.Amount);
                    var rGoodForPayment = uow.Repository<AchqFGdPymt>().GetSingle(x => x.IAccno == withdrawModel.AccountId && x.Chqno == withdrawModel.ChequeNumber && x.brnhid == branchId);
                    rGoodForPayment.tstate = 4;
                    uow.Repository<AchqFGdPymt>().Edit(rGoodForPayment);
                }
                else
                {
                    commonService.InsertAvailableBalance(1, withdrawModel.AccountId, withdrawModel.Amount);

                }

                uow.Commit();
                taskUow.SaveTaskNotification(taskVerifier, transactionNumber, 3);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void UpdateLastTransactionDateInAdetail(int iAccno)
        {
            var accountRow = uow.Repository<ADetail>().FindBy(x => x.IAccno == iAccno).FirstOrDefault();
            accountRow.LastTransDate = commonService.GetBranchTransactionDate();
            uow.Repository<ADetail>().Edit(accountRow);
            uow.Commit();
        }
        private void WithdrawTransactionSaveRevolving(WithdrawViewModel withdrawModel, TaskVerificationList taskVerifier, Int64 transactionNumber)
        {
            try
            {
                int UserId = Global.UserId;
                int branchId = commonService.GetBranchIdByEmployeeUserId();

                ASTrn asTransaction = new ASTrn();
                asTransaction.IAccno = withdrawModel.AccountId;
                asTransaction.tdate = commonService.GetBranchTransactionDate();
                asTransaction.notes = withdrawModel.Payee;

                asTransaction.slpno = withdrawModel.SlipNo;
                asTransaction.IsSlp = withdrawModel.IsSlp;
                asTransaction.dramt = withdrawModel.Amount;
                asTransaction.ttype = 1;
                asTransaction.postedby = UserId;
                asTransaction.brnhno = branchId;
                asTransaction.tno = transactionNumber;
                asTransaction.PostedOn = commonService.GetDate();
                uow.Repository<ASTrn>().Add(asTransaction);
                if (!withdrawModel.IsSlp)
                {
                    commonService.UpdateTnoInChqNumber(withdrawModel.ChequeNumber, transactionNumber);
                }
                //if (TellerUtilityService.IsCheckUsedInGoodForPayment(withdrawModel.AccountId, withdrawModel.ChequeNumber))
                //{
                //    var rGoodForPayment = uow.Repository<AchqFGdPymt>().GetSingle(x => x.IAccno == withdrawModel.AccountId && x.Chqno == withdrawModel.ChequeNumber && x.brnhid == branchId);
                //    rGoodForPayment.tstate = 5;
                //    uow.Repository<AchqFGdPymt>().Edit(rGoodForPayment);
                //}
                uow.Commit();
                taskUow.SaveTaskNotification(taskVerifier, transactionNumber, 3);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void AccountInterestPayableSave(InterestPayableViewModel intPayModel, Int64 transactionNumber, int AccountId, TaskVerificationList taskVerifier)
        {
            try
            {
                string remarks = string.Empty;
                remarks = "Paid";
                if (intPayModel.InterestPayee != "")
                {
                    remarks = " to " + intPayModel.InterestPayee;
                }
                if (intPayModel.InterestSlipNo != 0)
                {
                    remarks = remarks + " for slip no. " + intPayModel.InterestSlipNo;
                }
                if (remarks == "Paid")
                {
                    remarks = "";
                }
                AintPayable aintPayableRow = new AintPayable();
                aintPayableRow.Iaccno = AccountId;
                aintPayableRow.Tdate = commonService.GetBranchTransactionDate();
                aintPayableRow.IntAmt = 0;
                aintPayableRow.TaxRt = 0;
                aintPayableRow.DrAmt = intPayModel.InterestAmount;
                aintPayableRow.PostedBy = Global.UserId;
                aintPayableRow.Valued = 0;
                aintPayableRow.Tno = transactionNumber;
                aintPayableRow.ByTeller = true;
                aintPayableRow.Remarks = remarks;

                uow.Repository<AintPayable>().Add(aintPayableRow);
                uow.Commit();
                taskUow.SaveTaskNotification(taskVerifier, transactionNumber, 6);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public CashLimit TellerCashExceedAmount()
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var tellerAllowAmount = uow.Repository<CashLimit>().FindBy(x => x.stfid == Global.UserId).FirstOrDefault();
                return tellerAllowAmount;
            }
        }

        public decimal GetAmountLimitForTeller()
        {
            var tellerAllowAmount = uow.Repository<CashLimit>().FindBy(x => x.stfid == Global.UserId).Select(x => x.cramt).FirstOrDefault();
            return tellerAllowAmount;
        }

        //
        public int GetEventIdFromRNO(int rno)
        {
            var tellerAllowAmount = uow.Repository<DAL.DatabaseModel.Task>().FindBy(x => (x.EventValue == rno) && (x.IsVerified == false)).Select(x => x.EventId).FirstOrDefault();
            return tellerAllowAmount;
        }

        public int GetTask1IdFromRNO(int rno, int eventId)
        {
            var tellerAllowAmount = uow.Repository<DAL.DatabaseModel.Task>().FindBy(x => (x.EventValue == rno) && (x.IsVerified == false) && (x.EventId == eventId)).Select(x => x.Task1Id).FirstOrDefault();
            return tellerAllowAmount;
        }

        public int GetTask1IdFromRNOAndVerified(int rno, int eventId)
        {
            var tellerAllowAmount = uow.Repository<DAL.DatabaseModel.Task>().FindBy(x => (x.EventValue == rno) && (x.IsVerified == false) && (x.EventId == eventId)).Select(x => x.Task1Id).FirstOrDefault();
            return tellerAllowAmount;
        }



        public int GetTaskRaisedToFromTask1Id(int task1Id)
        {
            var tellerAllowAmount = uow.Repository<DAL.DatabaseModel.TaskDetail>().FindBy(x => x.Task1Id==task1Id).Select(x => x.TaskTo).FirstOrDefault();
            return tellerAllowAmount;
        }

        public InterestPayableModel InterestPayable(int accountId = 0)
        {
            var productId = uow.Repository<ADetail>().FindBy(x => x.IAccno == accountId).Select(x => x.PID).FirstOrDefault();
            bool isNominee = TellerUtilityService.IsNominee(productId);
            decimal balance = 0;
            if (isNominee == true)
            {
                var intAmount = uow.Repository<ReturnSingleValueModdel>().SqlQuery("SELECT 'AmountValue'=isnull(SUM(CRAmt),0)-isnull(SUM(DRAmt),0) FROM fin.AIntPayable WHERE iaccno=" + accountId + " GROUP BY iaccno").FirstOrDefault();
                if (intAmount == null)
                {
                    balance = 0;
                }
                else
                {
                    balance = intAmount.AmountValue;
                }

            }
            InterestPayableModel interestPayable = new InterestPayableModel();
            interestPayable.Amount = balance;
            interestPayable.IsNomee = isNominee;
            return interestPayable;
        }
        public List<TaskVerificationList> UserCashLimtVerification(int EventId = 1)
        {
            CommonService commonService = new CommonService();
            int branchId = commonService.GetBranchIdByEmployeeUserId();
            var verificationList = uow.Repository<TaskVerificationList>().SqlQuery(@"select * from fin.FGetTellerCashierRoleTempleteAssign(" + EventId + "," + branchId + ")").ToList();
            return verificationList;
        }
        public ReturnBaseMessageModel ApprovedUserCashLimitExceed(long rno)
        {

            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions()
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            }
            ))
            {
                try
                {
                    AWtdQueue awtdQue = uow.Repository<AWtdQueue>().FindBy(x => x.Rno == rno).FirstOrDefault();
                    awtdQue.Qstate = 2;
                    awtdQue.Approvedby = Global.UserId;
                    uow.Repository<AWtdQueue>().Edit(awtdQue);
                    decimal goodBalance = TellerUtilityService.GoodBalance(awtdQue.IAccno);
                    int accountStatus = TellerUtilityService.GetAccountStatus(awtdQue.IAccno);
                    if (accountStatus == 7)
                    {
                        if (goodBalance == awtdQue.amount)
                        {
                            var accountRow = uow.Repository<ADetail>().FindBy(x => x.IAccno == awtdQue.IAccno).FirstOrDefault();
                            accountRow.AccState = 3;
                            uow.Repository<ADetail>().Edit(accountRow);
                        }
                    }
                    if (awtdQue.isslip == false)
                    {
                        if (TellerUtilityService.IsCheckUsedInGoodForPayment(awtdQue.IAccno, Convert.ToInt32(awtdQue.chqno)))
                        {
                            int branchId = commonService.GetBranchIdByEmployeeUserId();
                            var achqPayment = uow.Repository<AchqFGdPymt>().FindBy(x => x.IAccno == awtdQue.IAccno && x.Chqno == awtdQue.chqno && x.brnhid == branchId).FirstOrDefault();
                            achqPayment.tstate = 5;
                            uow.Repository<AchqFGdPymt>().Edit(achqPayment);
                        }
                    }
                    DAL.DatabaseModel.Task task = new DAL.DatabaseModel.Task();
                    task.EventValue = rno;
                    task.EventId = 12;
                    task.RaisedBy = Global.UserId;
                    task.RaisedOn = commonService.GetDate();
                    string cashierName = commonService.GetAllCashier().Where(x => x.UserId == Global.UserId).Select(x => x.EmployeeName).FirstOrDefault();

                    task.Message = "User cash limit exceed Payment verify by cashier.!!" + cashierName;
                    task.IsVerified = false;
                    TaskVerificationList TaskVerifierList = new TaskVerificationList();
                    uow.Repository<DAL.DatabaseModel.Task>().Add(task);
                    TaskDetail taskDetails = new TaskDetail();
                    taskDetails.TaskTo = awtdQue.postedby;
                    task.TaskDetails.Add(taskDetails);

                    uow.Commit();
                    transaction.Complete();
                    returnMessage.Success = true;
                    returnMessage.Msg = "Transaction(s) has been verified successfull.!!";
                    return returnMessage;
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    returnMessage.Success = false;
                    returnMessage.Msg = "Transaction(s) could not be verified.!!" + ex.Message;
                    return returnMessage;
                }
            }

        }
        public AWtdQueueModel UserLimitExceedDetails(int rno)
        {
            var singlUserLimit = uow.Repository<AWtdQueue>().GetSingle(x => x.Rno == rno);

            AWtdQueueModel awtdQueue = new AWtdQueueModel();
            awtdQueue.amount = singlUserLimit.amount;
            awtdQueue.chqno = singlUserLimit.chqno;
            awtdQueue.isslip = singlUserLimit.isslip;
            awtdQueue.Qstate = singlUserLimit.Qstate;
            awtdQueue.tdate = singlUserLimit.tdate;
            awtdQueue.notes = singlUserLimit.notes;
            awtdQueue.AccountNumber = singlUserLimit.ADetail.Accno;
            awtdQueue.AccountName = singlUserLimit.ADetail.Aname;
            awtdQueue.currid = singlUserLimit.currid;
            awtdQueue.IAccno = singlUserLimit.IAccno;
            awtdQueue.Type = TellerUtilityService.GetTType((int)singlUserLimit.ttype, singlUserLimit.isslip);
            return awtdQueue;
        }

        public ReturnBaseMessageModel PaymentOfCashLimitExceedAfterVerifyByCashier(AWtdQueueModel awtdModel, DenoInOutViewModel denoInOutModel)
        {
            using (TransactionScope transaction = new TransactionScope(
               // a new transaction will always be created
               TransactionScopeOption.RequiresNew,
               // we will allow volatile data to be read during transaction
               new TransactionOptions()
               {
                   IsolationLevel = IsolationLevel.ReadUncommitted
               }
           ))
            {
                try
                {
                    if (taskUow.VerifiedBy(awtdModel.TaskId) != "")
                    {
                        returnMessage.Msg = "Transaction completed By:" + taskUow.VerifiedBy(awtdModel.TaskId);
                        returnMessage.Success = false;
                        return returnMessage;
                    }
                    var rawtdModel = uow.Repository<AWtdQueue>().GetSingle(x => x.Rno == awtdModel.Rno);
                    bool IsTrnsWithDeno = commonService.IsTransactionWithDeno();

                    //returnMessage = TellerUtilityService.IsVerifyPreviousTransaction(rawtdModel.IAccno);
                    //if (!returnMessage.Success)
                    //{
                    //    return returnMessage;
                    //}

                    //bool IsRevolving = TellerUtilityService.IsRevolving(rawtdModel.IAccno);
                    if (IsTrnsWithDeno)
                    {
                        if (!TellerUtilityService.BalanceWithDenoAmount(denoInOutModel, rawtdModel.amount))
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
                    Int64 transactionNumber = commonService.GetUtno();
                    if (IsTrnsWithDeno)
                    {
                        commonService.DenoInOutTransaction(denoInOutModel, transactionNumber);
                    }

                    //if (IsRevolving)
                    //{
                    //    AMTransLoan asTransLoan = new AMTransLoan();
                    //    asTransLoan.tno = transactionNumber;
                    //    asTransLoan.Pid = 10;
                    //    asTransLoan.Amt = rawtdModel.amount;
                    //    uow.Repository<AMTransLoan>().Add(asTransLoan);
                    //    uow.ExecWithStoreProcedure("fin.PsetInsertLoanScheduleVerify(" + rawtdModel.amount + "," + rawtdModel.IAccno + ")");

                    //}
                    AMTrn asTransaction = new AMTrn();
                    asTransaction.IAccno = rawtdModel.IAccno;
                    asTransaction.tdate = rawtdModel.tdate;
                    asTransaction.notes = rawtdModel.notes;

                    asTransaction.slpno = Convert.ToInt32(rawtdModel.chqno);
                    asTransaction.Isslp = rawtdModel.isslip;

                    asTransaction.dramt = rawtdModel.amount;
                    asTransaction.ttype = Convert.ToByte(rawtdModel.ttype);
                    asTransaction.postedby = rawtdModel.postedby;
                    asTransaction.brnhno = rawtdModel.BrnhID;
                    asTransaction.tno = transactionNumber;
                    asTransaction.vdate = commonService.GetBranchTransactionDate();
                    uow.Repository<AMTrn>().Add(asTransaction);

                    rawtdModel.Qstate = 4;
                    rawtdModel.tno = transactionNumber;
                    uow.Repository<AWtdQueue>().Edit(rawtdModel);
                    if (!rawtdModel.isslip)
                    {
                        commonService.UpdateTnoInChqNumber(Convert.ToInt32(rawtdModel.chqno), transactionNumber);
                    }

                    commonService.SaveUpdateMyBalance(Global.UserId, commonService.DefultCurrency(), rawtdModel.amount, 0);

                    //if (!IsRevolving)
                    //{
                    commonService.InsertAvailableBalance(3, rawtdModel.IAccno, (-rawtdModel.amount));
                    commonService.InsertAvailableBalance(5, rawtdModel.IAccno, (-rawtdModel.amount));

                    var aRow = uow.Repository<ADetail>().GetSingle(x => x.IAccno == rawtdModel.IAccno);
                    aRow.Bal = aRow.Bal - awtdModel.amount;
                    aRow.LastTransDate = commonService.GetBranchTransactionDate();
                    uow.Repository<ADetail>().Edit(aRow);
                    //}
                    //else
                    //{
                    //    var aRow = uow.Repository<ADetail>().GetSingle(x => x.IAccno == rawtdModel.IAccno);
                    //    aRow.Bal = aRow.Bal + awtdModel.amount;
                    //    aRow.LastTransDate = commonService.GetBranchTransactionDate();
                    //    uow.Repository<ADetail>().Edit(aRow);
                    //}


                    taskUow.VerifiedOn(awtdModel.TaskId);
                    uow.Commit();
                    transaction.Complete();
                    returnMessage.Success = true;
                    returnMessage.Msg = "withdraw save successfully with Transaction number #" + transactionNumber;
                    return returnMessage;

                }
                catch (Exception ex)
                {
                    returnMessage.Success = false;
                    returnMessage.Msg = "withdraw not save.!!" + ex.Message;
                    transaction.Dispose();
                    return returnMessage;
                }

            }
        }

        public IPagedList<AWtdQueueModel> TellerExceedPayment(int pageNo, int pageSize, string accountNumber)
        {
            var cashier = taskUow.UserCashLimtVerification().Where(x => x.UserId == Global.UserId).FirstOrDefault();
            var tailer = taskUow.UserCashLimtTellerVerification().Where(x => x.UserId == Global.UserId).FirstOrDefault();
            int branchId = commonService.GetBranchIdByEmployeeUserId();
            if (tailer != null)
            {
                var getTellerExceedDate = uow.Repository<AWtdQueue>().FindBy(x => x.ADetail.Accno.Contains(accountNumber) && x.BrnhID == branchId && x.Qstate == 2).Select(x => new AWtdQueueModel()
                {
                    AccountName = x.ADetail.Aname,
                    AccountNumber = x.ADetail.Accno,
                    Rno = x.Rno,
                    notes = x.notes,
                    amount = x.amount,
                    chqno = x.chqno,
                    tdate = x.tdate,
                    isslip = x.isslip,
                    ttype = x.ttype
                }).OrderByDescending(x => x.Rno).ToPagedList(pageNo, pageSize);
                return getTellerExceedDate;
            }
            else if (cashier != null)
            {
                var getTellerExceedDate = uow.Repository<AWtdQueue>().FindBy(x => x.ADetail.Accno.Contains(accountNumber) && x.BrnhID == branchId && x.Qstate == 1).Select(x => new AWtdQueueModel()
                {
                    AccountName = x.ADetail.Aname,
                    AccountNumber = x.ADetail.Accno,
                    Rno = x.Rno,
                    notes = x.notes,
                    amount = x.amount,
                    chqno = x.chqno,
                    tdate = x.tdate,
                    isslip = x.isslip,
                    ttype = x.ttype
                }).OrderByDescending(x => x.Rno).ToPagedList(pageNo, pageSize);
                return getTellerExceedDate;
            }
            else
            {
                var getTellerExceedDate = uow.Repository<AWtdQueue>().FindBy(x => x.ADetail.Accno.Contains(accountNumber) && x.BrnhID == branchId && x.Qstate != 4).Select(x => new AWtdQueueModel()
                {
                    AccountName = x.ADetail.Aname,
                    AccountNumber = x.ADetail.Accno,
                    IAccno = x.IAccno,
                    Rno = x.Rno,
                    notes = x.notes,
                    amount = x.amount,
                    chqno = x.chqno,
                    tdate = x.tdate,
                    isslip = x.isslip,
                    Qstate = x.Qstate,
                    ttype = x.ttype
                }).OrderByDescending(x => x.Rno).ToPagedList(pageNo, pageSize);


                return getTellerExceedDate;
            }

        }

        public InterestPayableViewModel GetAccountInterestPayabledetails(int tno)
        {
            var aInterestPayable = uow.Repository<AintPayable>().GetSingle(x => x.Tno == tno);
            var interestPay = new InterestPayableViewModel
            {
                InterestAmount = Convert.ToDecimal(aInterestPayable.DrAmt),
                InterestPayee = aInterestPayable.Remarks,
                AccountNumber = aInterestPayable.ADetail.Accno,
                AccountName = aInterestPayable.ADetail.Aname,
                Tno = aInterestPayable.Tno,
                TransactionDate = aInterestPayable.Tdate
            };

            return interestPay;
        }

        public AchqFGdPymt GetGoodForPaymentDetails(int iAccno, int chequeNo)
        {
            var goodForPayment = uow.Repository<AchqFGdPymt>().FindBy(x => x.IAccno == iAccno && x.Chqno == chequeNo).FirstOrDefault();
            return goodForPayment;
        }
        #endregion

        #region CollectionSheet
        public ReturnBaseMessageModel CollectionSheetTempSave(CollectionSheetViewModel collectionSheet)
        {
            try
            {
                CollMainTemp collMainTemp = new CollMainTemp();
                if (collectionSheet.RetId == 0)
                {

                    collMainTemp.CollectionAmt = collectionSheet.TotalAmount;
                    collMainTemp.CollectorId = Convert.ToInt32(collectionSheet.CollectorId);
                    collMainTemp.TDate = commonService.GetBranchTransactionDate();
                    collMainTemp.VDate = collectionSheet.VDate;
                    collMainTemp.PostedBy = Loader.Models.Global.UserId;
                    //collMainTemp.SheetNo = commonService.GetSheetno();
                    collMainTemp.SheetNo = collectionSheet.SheetNo;
                    collMainTemp.BrchId = commonService.GetBranchIdByEmployeeUserId();
                    uow.Repository<CollMainTemp>().Add(collMainTemp);
                }
                else
                {
                    collMainTemp = uow.Repository<CollMainTemp>().FindBy(x => x.RetId == collectionSheet.RetId).FirstOrDefault();

                    collMainTemp.TotalAmt = collectionSheet.TempTotal;
                    collMainTemp.CollectionAmt = collectionSheet.TotalAmount;
                    uow.Repository<CollMainTemp>().Edit(collMainTemp);
                }

                CollTemp collsheettemp = new CollTemp();

                collsheettemp.Amount = collectionSheet.Amount;


                collsheettemp.RetId = collMainTemp.RetId;
                collsheettemp.IAccNo = collectionSheet.IAccNo;
                collsheettemp.FId = commonService.GetFidByIAccno(collectionSheet.IAccNo);
                collsheettemp.Description = collectionSheet.Description;
                collsheettemp.SType = commonService.GetStypeByIaccno(collectionSheet.IAccNo);
                uow.Repository<CollTemp>().Add(collsheettemp);

                uow.Commit();
                returnMessage.Msg = "Successfully Saved";
                returnMessage.Success = true;
                returnMessage.ReturnId = collMainTemp.RetId;
                return returnMessage;
            }
            catch (Exception ex)
            {

                returnMessage.Success = false;
                returnMessage.Msg = " Not saved.!!" + ex.Message;
                return returnMessage;
            }
        }

        public CollectionSheetViewModel GetCollectedSheetByRetId(int retId)
        {
            CollectionSheetViewModel coll = new CollectionSheetViewModel();
            coll.RetId = retId;
            coll.CollectedAccountSheet = TempCollectionSheetList(retId);
            coll.CollectedAccountSheetSummary = TempCollectionSheetListSummary(retId);
            var collectedAccountSheet = uow.Repository<CollMainTemp>().GetSingle(x => x.RetId == retId);
            if (collectedAccountSheet != null)
            {
                coll.CollectorId = collectedAccountSheet.CollectorId;
                coll.COllectorName = GetCollectorNameByCollectorID(coll.CollectorId);
                coll.TotalAmount = collectedAccountSheet.CollectionAmt;
                coll.SheetNo = collectedAccountSheet.SheetNo;
            }
            return coll;

        }


        public string GetCollectorNameByCollectorID(Int64 collectorId)
        {
            int smallCollectorId = Convert.ToInt32(collectorId);
            string collectorName = uow.Repository<DAL.DatabaseModel.Employee>().FindBy(x => x.EmployeeId == smallCollectorId).FirstOrDefault().EmployeeName;

            return collectorName;

        }


        public List<CollectedAccountSheet> TempCollectionSheetList(int retNo, string accountNumber = "")
        {
            if (accountNumber == "" || accountNumber == null)
            {
                return uow.Repository<CollectedAccountSheet>().SqlQuery(@"select Aname as Name,Accno as AccountNumber,Description as Description,Amount As Amount,CollectorId,CollectionAmt as TotalAmount,b.RetId,Id,SheetNo from fin.CollMainTemp a  inner join  fin.CollTemp b on a.RetId=b.RetId inner join fin.ADetail c on b.IAccNo=c.IAccno where b.RetId=" + retNo + "order by b.id desc").ToList();
            }
            else
                return uow.Repository<CollectedAccountSheet>().SqlQuery(@"select Aname as Name,Accno as AccountNumber,Description as Description,Amount As Amount,CollectorId,CollectionAmt as TotalAmount,b.RetId,Id,SheetNo from fin.CollMainTemp a  inner join  fin.CollTemp b on a.RetId=b.RetId inner join fin.ADetail c on b.IAccNo=c.IAccno  where (Accno like" + "'%" + accountNumber + "%' and b.retid=" + retNo + ") or (aname like" + "'%" + accountNumber + "%' and b.retid=" + retNo + ")order by b.id desc").ToList();
        }
        public List<CollectedAccountSheetSummary> TempCollectionSheetListSummary(int retNo)
        {
            var collectionListSummary = uow.Repository<CollectedAccountSheetSummary>().SqlQuery(@"select PName,sum(Amount) as Amount,count(*) as NoOfAccount from fin.ADetail a inner join fin.CollTemp c on a.IAccNo=c.IAccNo inner join fin.ProductDetail p on a.PID=p.PID where RetId=" + retNo + "group by a.pid,PName");

            return collectionListSummary.ToList();
        }
        public string GetCollectors()
        {
            var collectors = luow.Repository<Loader.Models.Employee>().GetSingle(x => x.DGId == 7).EmployeeName;
            return collectors;
        }

        public List<CollectedAccountSheet> GetCollectionLogList()
        {
            var userId = Loader.Models.Global.UserId;
            var branchId = Loader.Models.Global.BranchId;
            var collectionLogList = uow.Repository<CollectedAccountSheet>().SqlQuery(@"select RetId,CollectorId,CollectionAmt as TotalAmount from fin.CollMainTemp c  where c.status = 0 and c.PostedBy = " + userId + " and c.BrchId = " + branchId); //employee table inner join removed
            return collectionLogList.ToList();
        }
        public void DeleteSingleCollectionLog(int retId, int TempCollId)
        {
            int count = uow.Repository<CollTemp>().FindBy(x => x.RetId == retId).Count();

            CollTemp collTemp = new CollTemp();
            CollMainTemp collMainTemp = new CollMainTemp();
            collMainTemp = uow.Repository<CollMainTemp>().GetSingle(x => x.RetId == retId);
            collTemp = uow.Repository<CollTemp>().GetSingle(x => x.Id == TempCollId);
            uow.Repository<CollTemp>().Delete(collTemp);
            if (count == 1)
            {
                uow.Repository<CollMainTemp>().Delete(collMainTemp);
            }
            uow.Commit();
        }
        public ReturnBaseMessageModel DeleteCollectionLog(int retId)
        {
            try
            {
                List<CollTemp> collTemp = new List<CollTemp>();
                CollMainTemp collMainTemp = new CollMainTemp();
                collMainTemp = uow.Repository<CollMainTemp>().GetSingle(x => x.RetId == retId);
                collTemp = uow.Repository<CollTemp>().FindBy(x => x.RetId == retId).ToList();
                foreach (var item in collTemp)
                {
                    uow.Repository<CollTemp>().Delete(item);
                }
                uow.Repository<CollMainTemp>().Delete(collMainTemp);
                uow.Commit();
                returnMessage.Msg = "Successfully deleted";
                returnMessage.Success = true;
                return returnMessage;
            }
            catch (Exception ex)
            {
                returnMessage.Success = false;
                returnMessage.Msg = " Not Deleted.!!" + ex.Message;
                return returnMessage;
            }
        }
        public ReturnBaseMessageModel CollectionSheetSave(CollectionSheetViewModel collectionSheet)
        {
            using (var transaction = uow.GetContext().Database.BeginTransaction())
            {

                try
                {
                    CollSheet collSheet = new CollSheet();
                    //int SheetNo = uow.Repository<CollectionSheetViewModel>().SqlQuery(@"select isnull(max(sheetno),0) as SheetNo from fin.[CollSheet]").Select(x => x.SheetNo).FirstOrDefault();
                    //SheetNo += 1;

                    collSheet.CollectorId = Convert.ToInt32(collectionSheet.CollectorId);
                    collSheet.CollSheetId = commonService.GetUtno();
                    collSheet.TDate = commonService.GetBranchTransactionDate();
                    collSheet.VDate = commonService.GetBranchTransactionDate();
                    collSheet.PostedBy = Loader.Models.Global.UserId;
                    collSheet.BrchId = commonService.GetBranchIdByEmployeeUserId();
                    collSheet.TotalAmount = collectionSheet.TotalAmount;
                    collSheet.note = collectionSheet.note;
                    collSheet.SheetNo = collectionSheet.SheetNo;
                    uow.Repository<CollSheet>().Add(collSheet);
                    uow.Commit();
                    if (collectionSheet.TaskId != 0)
                    {
                        taskUow.VerifiedOn(collectionSheet.TaskId);
                    }
                    List<CollTemp> colltemp = new List<CollTemp>();
                    colltemp = uow.Repository<CollTemp>().FindBy(x => x.RetId == collectionSheet.RetId).ToList();
                    foreach (var item in colltemp)
                    {
                        CollSheetTran collsheettrans = new CollSheetTran();
                        collsheettrans.Amount = item.Amount;
                        collsheettrans.IAccNo = item.IAccNo;
                        collsheettrans.SType = item.SType;
                        collsheettrans.Description = item.Description;
                        collsheettrans.TNo = commonService.GetUtno();
                        collsheettrans.CollSheetId = collSheet.CollSheetId;
                        uow.Repository<CollSheetTran>().Add(collsheettrans);
                        uow.Repository<CollTemp>().Delete(item);

                        //shadow credit
                        commonService.InsertAvailableBalance(2, item.IAccNo, item.Amount);
                    }

                    CollMainTemp collMainTemp = new CollMainTemp();
                    collMainTemp = uow.Repository<CollMainTemp>().GetSingle(x => x.RetId == collectionSheet.RetId);
                    uow.Repository<CollMainTemp>().Delete(collMainTemp);
                    uow.Commit();
                    transaction.Commit();
                    returnMessage.Msg = "Successfully Saved";
                    returnMessage.Success = true;
                    returnMessage.ReturnId = collMainTemp.RetId;
                    return returnMessage;
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    returnMessage.Success = false;
                    returnMessage.Msg = " Not saved.!!" + ex.Message;
                    return returnMessage;
                }

            }
        }

        public List<SmallCollectionSheetViewModel> GetAllUnverifiedCollectionList()
        {
            string query = "select a.CollSheetId, a.CollectorId,a.SheetNo, TotalAmount, note,SheetNo from fin.CollSheet a inner join(select distinct CollSheetId from fin.CollSheetTrans) b on a.collsheetId=b.collsheetId where ApprovedBy is null";
            var unverifiedCollectionList = uow.Repository<SmallCollectionSheetViewModel>().SqlQuery(query).ToList();
            return unverifiedCollectionList;
        }
        public List<CollectedAccountSheet> UnverifiedIndividaulCollectionSheetList(int collSheetId, string accountNumber)
        {
            if (accountNumber == "" || accountNumber == null)
            {
                List<CollectedAccountSheet> collectionAccountSheet = new List<CollectedAccountSheet>();
                Int64 bigCollSheetId = Convert.ToInt64(collSheetId);
                collectionAccountSheet = uow.Repository<CollectedAccountSheet>().SqlQuery(@"select Aname as Name,Accno as AccountNumber,Description as Description,Amount As Amount,CollectorId,TotalAmount as TotalAmount,a.CollSheetId,TDate,VDate,note,SheetNo  from fin.CollSheet a  inner join  fin.CollSheetTrans b on a.CollSheetId=b.CollSheetId inner join fin.ADetail c on b.IAccNo=c.IAccno where b.CollSheetId=" + bigCollSheetId + " order by b.id desc").ToList();
                return collectionAccountSheet;

            }
            else
                return uow.Repository<CollectedAccountSheet>().SqlQuery(@"select Aname as Name,Accno as AccountNumber,Description as Description,Amount As Amount,CollectorId,TotalAmount as TotalAmount,a.CollSheetId,TDate,VDate,note,SheetNo  from fin.CollSheet a  inner join  fin.CollSheetTrans b on a.CollSheetId=b.CollSheetId inner join fin.ADetail c on b.IAccNo=c.IAccno where (Accno like" + "'%" + accountNumber + "%' and b.CollSheetId=" + collSheetId + ") or (aname like" + "'%" + accountNumber + "%' and b.CollSheetId=" + collSheetId + ") order by b.id desc").ToList();
        }
        public CollectionSheetViewModel GetUnverifiedCollectedSheetBycollSheetId(int collSheetId)
        {
            CollectionSheetViewModel coll = new CollectionSheetViewModel();

            coll.CollectedAccountSheet = UnverifiedIndividaulCollectionSheetList(collSheetId, "");

            var collectedAccountSheet = UnverifiedIndividaulCollectionSheetList(collSheetId, "").FirstOrDefault();
            if (collectedAccountSheet != null)
            {
                coll.TDate = collectedAccountSheet.TDate;
                coll.VDate = collectedAccountSheet.VDate;
                coll.CollSheetId = collSheetId;
                coll.CollectorId = collectedAccountSheet.CollectorId;
                coll.TotalAmount = collectedAccountSheet.TotalAmount;
                coll.note = collectedAccountSheet.note;
                coll.SheetNo = collectedAccountSheet.SheetNo;
            }
            return coll;
        }
        public int VerifyCollectionSheetTransaction(long collSheetId)
        {
            string UserId = Global.UserId.ToString();
            int isSuccess = uow.ExecWithStoreProcedure("[Trans].[PSetCollection] @ColSheetId,@ApprovedBy", new SqlParameter("@ColSheetId", collSheetId), new SqlParameter("@ApprovedBy", UserId));
            return isSuccess;
        }

        public ReturnBaseMessageModel VerifySheetBycollSheetId(long collSheetId, DenoInOutViewModel denoInOutModel)
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
                    CollSheet collsheet = new CollSheet();
                    var amount = uow.Repository<CollSheet>().FindBy(x => x.CollSheetId == collSheetId).Select(x => x.TotalAmount).FirstOrDefault();
                    VerifyCollectionSheetTransaction(collSheetId);
                    bool IsTrnsWithDeno = commonService.IsTransactionWithDeno();
                    if (IsTrnsWithDeno)
                    {
                        if (!TellerUtilityService.BalanceWithDenoAmount(denoInOutModel, amount))
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
                        if (IsTrnsWithDeno)
                        {
                            commonService.DenoInOutTransaction(denoInOutModel, collSheetId);
                        }
                        commonService.SaveUpdateMyBalance(0, commonService.DefultCurrency(), amount, Global.UserId);
                    }
                    #region account update, good balance, shadow balance and AMTrans
                    //good balance, adetails and amtrans are already updated from backend
                    //shadow balance is updated here
                    string query = "select Aname as Name,c.IAccno as Iaccno,Accno as AccountNumber,Description as Description,Amount As Amount,CollectorId,TotalAmount as TotalAmount,a.CollSheetId,TDate,VDate,note,SheetNo ,b.TNo from fin.CollSheet a  inner join  fin.CollSheetTrans b on a.CollSheetId=b.CollSheetId inner join fin.ADetail c on b.IAccNo=c.IAccno where b.CollSheetId=" + collSheetId + " order by b.id desc";
                    List<CollectedAccountSheet> collectedAccountSheet = uow.Repository<CollectedAccountSheet>().SqlQuery(query).ToList();
                    foreach (var item in collectedAccountSheet)
                    {
                        bool availableForInsert = false;
                        int fid = commonService.GetFidByIAccno(item.IAccNo);

                        //deduct in shadow balance
                        ABal shadowBalance = uow.Repository<ABal>().GetSingle(x => x.IAccno == item.IAccNo && x.Flag == 2);
                        if (shadowBalance == null)
                        {
                            shadowBalance = new ABal();
                            availableForInsert = true;
                        }
                        shadowBalance.IAccno = item.IAccNo;
                        shadowBalance.Flag = 2;
                        shadowBalance.FId = fid;
                        shadowBalance.Bal = shadowBalance.Bal - item.Amount;
                        if (availableForInsert)
                        {
                            uow.Repository<ABal>().Add(shadowBalance);
                        }
                        else
                        {
                            uow.Repository<ABal>().Edit(shadowBalance);
                        }
                    }
                    #endregion

                    uow.Commit();
                    transaction.Complete();
                    returnMessage.Msg = "Successfully Saved";
                    returnMessage.Success = true;
                    //returnMessage.ReturnId = uow.Repository<CollSheet>().GetSingle(x => x.CollSheetId == collSheetId).SheetNo;
                    return returnMessage;
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    returnMessage.Success = false;
                    returnMessage.Msg = " Not saved.!!" + ex.Message;
                    return returnMessage;
                }
            }
        }

        public int RejectCollectionSheetTransaction(int sheetNo, string RejectedMessage)
        {
            string UserId = Loader.Models.Global.UserId.ToString();
            int isSuccess = uow.ExecWithStoreProcedure("[Trans].[PSetRejectCollection] @SheetNo,@RejectedBy,@RejectRemarks", new SqlParameter("@SheetNo", sheetNo), new SqlParameter("@RejectedBy", UserId), new SqlParameter("@RejectRemarks", RejectedMessage));
            return isSuccess;
        }
        public ReturnBaseMessageModel RejectCollectionSheetTransactionByCollSheetId(int sheetno, string RejectedMessage)
        {

            try
            {
                var collsheet = uow.Repository<CollSheet>().FindBy(x => x.SheetNo == sheetno).FirstOrDefault();


                string query = "select Aname as Name,c.IAccno as Iaccno,Accno as AccountNumber,Description as Description,Amount As Amount,CollectorId,TotalAmount as TotalAmount,a.CollSheetId,TDate,VDate,note,SheetNo ,b.TNo from fin.CollSheet a  inner join  fin.CollSheetTrans b on a.CollSheetId=b.CollSheetId inner join fin.ADetail c on b.IAccNo=c.IAccno where b.collsheetid=" + collsheet.CollSheetId + " order by b.id desc";
                List<CollectedAccountSheet> collectedAccountSheet = uow.Repository<CollectedAccountSheet>().SqlQuery(query).ToList();
                foreach (var item in collectedAccountSheet)
                {
                    bool availableForInsert = false;
                    int fid = commonService.GetFidByIAccno(item.IAccNo);

                    //deduct in shadow balance
                    ABal shadowBalance = uow.Repository<ABal>().GetSingle(x => x.IAccno == item.IAccNo && x.Flag == 2);
                    if (shadowBalance == null)
                    {
                        shadowBalance = new ABal();
                        availableForInsert = true;
                    }
                    shadowBalance.IAccno = item.IAccNo;
                    shadowBalance.Flag = 2;
                    shadowBalance.FId = fid;
                    shadowBalance.Bal = shadowBalance.Bal - item.Amount;
                    if (availableForInsert)
                    {
                        uow.Repository<ABal>().Add(shadowBalance);
                    }
                    else
                    {
                        uow.Repository<ABal>().Edit(shadowBalance);
                    }
                }
                RejectCollectionSheetTransaction(sheetno, RejectedMessage);
                uow.Commit();
                TaskVerificationList tvl = new TaskVerificationList();
                tvl.Message = RejectedMessage;
                tvl.IsMultiVerifier = false;
                tvl.UserId = collsheet.PostedBy;
                tvl.IsSelected = true;
                tvl.VerifierList.Add(tvl);
                taskUow.SaveTaskNotification(tvl, sheetno, 10);
                returnMessage.Msg = "Successfully Rejected";
                returnMessage.Success = true;


                if (collsheet == null)
                {
                    returnMessage.ReturnId = uow.Repository<CollMainTemp>().FindBy(x => x.SheetNo == sheetno).FirstOrDefault().SheetNo;
                }
                else
                    returnMessage.ReturnId = collsheet.SheetNo;

                return returnMessage;
            }
            catch (Exception ex)
            {

                returnMessage.Success = false;
                returnMessage.Msg = " Not saved.!!" + ex.Message;
                return returnMessage;
            }
        }
        public ReturnBaseMessageModel RejectSheetBySheetNo(int sheetNo, string rejectRemarks)
        {

            try
            {

                VerifyCollectionSheetTransaction(sheetNo);
                returnMessage.Msg = "Successfully Cancelled";
                returnMessage.Success = true;
                returnMessage.ReturnId = uow.Repository<CollSheet>().GetSingle(x => x.SheetNo == sheetNo).SheetNo;
                return returnMessage;
            }
            catch (Exception ex)
            {

                returnMessage.Success = false;
                returnMessage.Msg = " Not saved.!!" + ex.Message;
                return returnMessage;
            }
        }
        #endregion

        #region  remittance details
        public ReturnBaseMessageModel RemitanceDeposit(RemitDepositModel remitDeposit, TaskVerificationList taskVerifier, DenoInOutViewModel denoInOutModel)
        {
            using (var transaction = uow.GetContext().Database.BeginTransaction())

            {
                try
                {
                    bool IsTrnsWithDeno = commonService.IsTransactionWithDeno();
                    if (IsTrnsWithDeno)
                    {
                        if (!TellerUtilityService.BalanceWithDenoAmount(denoInOutModel, remitDeposit.Amount))
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
                    Int64 transactionNumber = commonService.GetUtno();
                    if (IsTrnsWithDeno)
                    {
                        commonService.DenoInOutTransaction(denoInOutModel, transactionNumber);
                    }
                    var remitRow = uow.Repository<RemitDeposit>().GetSingle(x => x.RemitDepositId == remitDeposit.RemitDepositId);
                    if (remitRow == null)
                    {
                        remitRow = new RemitDeposit();
                    }
                    int userId = Global.UserId;

                    remitRow.Tno = transactionNumber;
                    remitRow.SenderContact = remitDeposit.SenderContact;
                    remitRow.SenderName = remitDeposit.SenderName;
                    remitRow.RemitCompanyId = remitDeposit.RemitCompanyId;

                    remitRow.Amount = remitDeposit.Amount;
                    remitRow.Remarks = remitDeposit.Remarks;
                    remitRow.ReceiverContact = remitDeposit.ReceiverContact;
                    remitRow.ReceiverAddress = remitDeposit.ReceiverAddress;
                    remitRow.ReceiverName = remitDeposit.ReceiverName;

                    remitRow.PostedBy = userId;
                    remitRow.PostedOn = commonService.GetBranchTransactionDate();
                    remitRow.BranchId = (byte)commonService.GetBranchIdByEmployeeUserId();
                    uow.Repository<RemitDeposit>().Add(remitRow);
                    uow.Commit();
                    taskUow.SaveTaskNotification(taskVerifier, transactionNumber, 15);
                    commonService.SaveUpdateMyBalance(0, commonService.DefultCurrency(), remitDeposit.Amount, Global.UserId);
                    transaction.Commit();
                    returnMessage.Success = true;
                    returnMessage.Msg = "Remit deposit save successfully with Transaction number #" + transactionNumber;
                    return returnMessage;
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    returnMessage.Success = false;
                    returnMessage.Msg = "Remit deposit  not save.!!" + ex.Message;
                    return returnMessage;
                }
            }

        }

        public RemitDepositModel RemittanceDepositDetails(Int64 tno)
        {
            var remitRow = uow.Repository<RemitDeposit>().GetSingle(x => x.Tno == tno);
            RemitDepositModel remitDeposit = new RemitDepositModel()
            {

                SenderContact = remitRow.SenderContact,
                SenderName = remitRow.SenderName,
                RemitCompanyId = remitRow.RemitCompanyId,
                BranchId = remitRow.BranchId,
                Amount = remitRow.Amount,
                Remarks = remitRow.Remarks,
                ReceiverContact = remitRow.ReceiverContact,
                ReceiverAddress = remitRow.ReceiverAddress,
                ReceiverName = remitRow.ReceiverName,
                //    BranchName = remitRow..BrnhNam,
                CompanyName = remitRow.RemittanceCustomer.CustCompany.CCName

            };

            return remitDeposit;
        }



        public ReturnBaseMessageModel RemitancePayment(RemitPaymentModel remitPayment, TaskVerificationList taskVerifier, DenoInOutViewModel denoInOutModel)
        {
            using (var transaction = uow.GetContext().Database.BeginTransaction())

            {

                try
                {
                    bool IsTrnsWithDeno = commonService.IsTransactionWithDeno();
                    if (IsTrnsWithDeno && remitPayment.UserTellerLimit == false)
                    {
                        if (!TellerUtilityService.BalanceWithDenoAmount(denoInOutModel, remitPayment.Amount))
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
                    Int64 transactionNumber = commonService.GetUtno();
                    if (IsTrnsWithDeno)
                    {
                        commonService.DenoInOutTransaction(denoInOutModel, transactionNumber);
                    }
                    var remitRow = uow.Repository<RemitPayment>().GetSingle(x => x.RemitPaymentId == remitPayment.RemitPaymentId);
                    if (remitRow == null)
                    {
                        remitRow = new RemitPayment();
                    }
                    int userId = Global.UserId;

                    remitRow.Tno = transactionNumber;
                    remitRow.RemitCompanyId = remitPayment.RemitCompanyId;
                    remitRow.ReceiverIdNumber = remitPayment.ReceiverIdNumber;
                    remitRow.ReceiverAddress = remitPayment.ReceiverAddress;
                    remitRow.ReceiverName = remitPayment.ReceiverName;
                    remitRow.RemittanceCode = remitPayment.RemittanceCode;
                    remitRow.Amount = remitPayment.Amount; ;
                    remitRow.BranchId = (byte)commonService.GetBranchIdByEmployeeUserId();
                    remitRow.Remarks = remitPayment.Remarks;
                    remitRow.PostedBy = userId;
                    remitRow.PostedOn = commonService.GetBranchTransactionDate();
                    uow.Repository<RemitPayment>().Add(remitRow);
                    uow.Commit();
                    taskUow.SaveTaskNotification(taskVerifier, transactionNumber, 16);
                    commonService.SaveUpdateMyBalance(Global.UserId, commonService.DefultCurrency(), remitPayment.Amount, 0);
                    transaction.Commit();
                    returnMessage.Success = true;
                    returnMessage.Msg = "Remit payment save successfully with Transaction number #" + transactionNumber;
                    return returnMessage;


                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    returnMessage.Success = false;
                    returnMessage.Msg = "Remit payment not save.!!" + ex.Message;
                    return returnMessage;
                }
            }

        }



        //public ReturnBaseMessageModel RemitancePayment(RemitPaymentModel remitPayment, TaskVerificationList taskVerifier, DenoInOutViewModel denoInOutModel)
        //{
        //    using (var transaction = uow.GetContext().Database.BeginTransaction())

        //    {

        //        try
        //        {
        //            bool IsTrnsWithDeno = commonService.IsTransactionWithDeno();
        //            if (IsTrnsWithDeno&&remitPayment.UserTellerLimit==false)
        //            {
        //                if (!TellerUtilityService.BalanceWithDenoAmount(denoInOutModel, remitPayment.Amount))
        //                {
        //                    returnMessage.Msg = "Amount is not match with deno balance.!!";
        //                    returnMessage.Success = false;
        //                    return returnMessage;
        //                }
        //                returnMessage = TellerUtilityService.AvailableDenoNumber(denoInOutModel.DenoOutList);
        //                if (!returnMessage.Success)
        //                {
        //                    return returnMessage;
        //                }

        //            }




        //            bool tellerAmount = TellerUtilityService.IsTellerAmountExceed(remitPayment.Amount);

        //            if (tellerAmount)
        //            {
        //                TellerAmountExceedSaveForRemit(remitPayment, taskVerifier);
        //                transaction.Commit();
        //                returnMessage.Success = true;
        //                returnMessage.Msg = "Posted for higher approval.!!";
        //                return returnMessage;
        //            }
        //            else
        //            {
        //                if (IsTrnsWithDeno)
        //                {
        //                    if (!TellerUtilityService.BalanceWithDenoAmount(denoInOutModel, remitPayment.Amount))
        //                    {
        //                        returnMessage.Msg = "Amount is not match with deno balance.!!";
        //                        returnMessage.Success = false;
        //                        return returnMessage;
        //                    }
        //                    returnMessage = TellerUtilityService.AvailableDenoNumber(denoInOutModel.DenoOutList);
        //                    if (!returnMessage.Success)
        //                    {
        //                        return returnMessage;
        //                    }

        //                }
        //                Int64 transactionNumber = commonService.GetUtno();
        //                if (IsTrnsWithDeno)
        //                {
        //                    commonService.DenoInOutTransaction(denoInOutModel, transactionNumber);
        //                }
        //                var remitRow = uow.Repository<RemitPayment>().GetSingle(x => x.RemitPaymentId == remitPayment.RemitPaymentId);
        //                if (remitRow == null)
        //                {
        //                    remitRow = new RemitPayment();
        //                }
        //                int userId = Global.UserId;

        //                remitRow.Tno = transactionNumber;
        //                remitRow.RemitCompanyId = remitPayment.RemitCompanyId;
        //                remitRow.ReceiverIdNumber = remitPayment.ReceiverIdNumber;
        //                remitRow.ReceiverAddress = remitPayment.ReceiverAddress;
        //                remitRow.ReceiverName = remitPayment.ReceiverName;
        //                remitRow.RemittanceCode = remitPayment.RemittanceCode;
        //                remitRow.Amount = remitPayment.Amount; ;
        //                remitRow.BranchId = (byte)commonService.GetBranchIdByEmployeeUserId();
        //                remitRow.Remarks = remitPayment.Remarks;
        //                remitRow.PostedBy = userId;
        //                remitRow.PostedOn = commonService.GetBranchTransactionDate();
        //                uow.Repository<RemitPayment>().Add(remitRow);
        //                uow.Commit();
        //                taskUow.SaveTaskNotification(taskVerifier, transactionNumber, 16);
        //                commonService.SaveUpdateMyBalance(Global.UserId, commonService.DefultCurrency(), remitPayment.Amount, 0);
        //                transaction.Commit();
        //                returnMessage.Success = true;
        //                returnMessage.Msg = "Remit payment save successfully with Transaction number #" + transactionNumber;
        //                return returnMessage;




        //            }






        //        }
        //        catch (Exception ex)
        //        {
        //            transaction.Dispose();
        //            returnMessage.Success = false;
        //            returnMessage.Msg = "Remit payment not save.!!" + ex.Message;
        //            return returnMessage;
        //        }
        //    }

        //}

        public RemitPaymentModel RemittancePaymentDetails(Int64 tno)
        {
            var remitPayment = uow.Repository<RemitPayment>().GetSingle(x => x.Tno == tno);
            RemitPaymentModel remitPaymentModel = new RemitPaymentModel()
            {

                Amount = remitPayment.Amount,
                BranchId = remitPayment.BranchId,
                ReceiverName = remitPayment.ReceiverName,
                ReceiverAddress = remitPayment.ReceiverAddress,
                ReceiverIdNumber = remitPayment.ReceiverIdNumber,
                Remarks = remitPayment.Remarks,
                RemitCompanyId = remitPayment.RemitCompanyId,
                CompanyName = remitPayment.RemittanceCustomer.CustCompany.CCName,
                //    BranchName = remitPayment.LicenseBranch.BrnhNam
            };
            return remitPaymentModel;
        }
        public ReturnBaseMessageModel VerifyRemitDeposit(long tno)
        {
            try
            {
                var remitRow = uow.Repository<RemitDeposit>().GetSingle(x => x.Tno == tno);
                remitRow.ApproveBy = Global.UserId;
                remitRow.ApproveOn = commonService.GetBranchTransactionDate();
                uow.Repository<RemitDeposit>().Edit(remitRow);
                uow.Commit();
                returnMessage.Success = true;
                returnMessage.Msg = "Remit deposit Verify successfully!!!";
                return returnMessage;
            }
            catch (Exception ex)
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Remit deposit not Verify.!!" + ex.Message;
                return returnMessage;
            }
        }

        public ReturnBaseMessageModel RejectRemitDeposit(long tno)
        {
            try
            {
                var remitRow = uow.Repository<RemitDeposit>().GetSingle(x => x.Tno == tno);
                remitRow.ApproveBy = Global.UserId;
                remitRow.ApproveOn = commonService.GetBranchTransactionDate();
                uow.Repository<RemitDeposit>().Delete(remitRow);
                uow.Commit();
                returnMessage.Success = true;
                returnMessage.Msg = "Remit Deposit Rejected successfully!!!";
                return returnMessage;
            }
            catch (Exception ex)
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Remit Deposit Not Rejected.!!" + ex.Message;
                return returnMessage;
            }
        }
        public ReturnBaseMessageModel VerifyRemitPayment(long tno)
        {
            try
            {
                var remitRow = uow.Repository<RemitPayment>().GetSingle(x => x.Tno == tno);
                remitRow.ApproveBy = Global.UserId;
                remitRow.ApproveOn = commonService.GetBranchTransactionDate();
                uow.Repository<RemitPayment>().Edit(remitRow);
                uow.Commit();
                returnMessage.Success = true;
                returnMessage.Msg = "Remit payment Verify successfully";
                return returnMessage;
            }
            catch (Exception ex)
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Remit payment not Verify.!!" + ex.Message;
                return returnMessage;
            }

        }



        public IPagedList<RemitDepositModel> UnverifiedRemittanceDeposit(int pageNo, int pageSize)
        {
            var remitDeposit = uow.Repository<RemitDeposit>().FindBy(x => x.ApproveBy == null).Select(remitRow => new RemitDepositModel()
            {

                SenderContact = remitRow.SenderContact,
                SenderName = remitRow.SenderName,
                RemitCompanyId = remitRow.RemitCompanyId,

                Amount = remitRow.Amount,
                Remarks = remitRow.Remarks,
                ReceiverContact = remitRow.ReceiverContact,
                ReceiverAddress = remitRow.ReceiverAddress,
                ReceiverName = remitRow.ReceiverName,
                // BranchName = remitRow.LicenseBranch.BrnhNam,
                CompanyName = remitRow.RemittanceCustomer.CustCompany.CCName,
                Tno = remitRow.Tno

            }).ToPagedList(pageNo, pageSize);

            return remitDeposit;
        }
        public IPagedList<RemitPaymentModel> UnverifiedRemittancePayment(int pageNo, int pageSize)
        {

            var remitPaymentList = uow.Repository<RemitPayment>().FindBy(x => x.ApproveBy == null).Select(remitPayment => new RemitPaymentModel()
            {

                Amount = remitPayment.Amount,
                BranchId = remitPayment.BranchId,
                ReceiverName = remitPayment.ReceiverName,
                ReceiverAddress = remitPayment.ReceiverAddress,
                ReceiverIdNumber = remitPayment.ReceiverIdNumber,
                Remarks = remitPayment.Remarks,
                RemitCompanyId = remitPayment.RemitCompanyId,
                CompanyName = remitPayment.RemittanceCustomer.CustCompany.CCName,
                Tno = remitPayment.Tno
                //  BranchName = remitPayment.LicenseBranch.BrnhNam
            }).ToPagedList(pageNo, pageSize);
            return remitPaymentList;
        }
        #endregion

        #region Cheque Clearence
        public ReturnBaseMessageModel ChequeClearenceCreate(ChequeClearenceViewModel ChequeDetail, TaskVerificationList taskVerifierList)
        {
            //  using (TransactionScope transaction = new TransactionScope(
            //    // a new transaction will always be created
            //    TransactionScopeOption.RequiresNew,
            //    // we will allow volatile data to be read during transaction
            //    new TransactionOptions()
            //    {
            //        IsolationLevel = IsolationLevel.ReadUncommitted,

            //    }
            //))
            using (var transaction = uow.GetContext().Database.BeginTransaction())

            {
                try
                {

                    int pId = TellerUtilityService.GetPid(ChequeDetail.IAccno);
                    bool IsFixedAccount = TellerUtilityService.IsFixedAccount(pId);

                    if (IsFixedAccount)
                    {
                        var agreementAmount = uow.Repository<ADrLimit>().FindBy(x => x.IAccno == ChequeDetail.IAccno).Select(x => x.AppAmt).SingleOrDefault();
                        var goodBalance = uow.Repository<ABal>().FindBy(x => x.IAccno == ChequeDetail.IAccno && x.Flag == 3).Select(x => x.Bal).SingleOrDefault();
                        var shadowCr = uow.Repository<ABal>().FindBy(x => x.IAccno == ChequeDetail.IAccno && x.Flag == 2).Select(x => x.Bal).SingleOrDefault();
                        //var result = true;
                        if (ChequeDetail.camount != 0 || agreementAmount != 0 || goodBalance != 0)
                        {
                            if ((agreementAmount < ChequeDetail.camount + goodBalance + shadowCr))
                            {
                                returnMessage.Msg = "Amount Inserted is greater than contribution amount.";
                                returnMessage.Success = false;
                                return returnMessage;

                            }


                        }
                    }

                    returnMessage = TellerUtilityService.AccountNumberRequired(ChequeDetail.IAccno);
                    if (returnMessage.Success == false)
                    {
                        return returnMessage;
                    }
                    int userBranchId = commonService.GetBranchIdByEmployeeUserId();
                    int AccountBranchId = commonService.AccountHolderBranchId(ChequeDetail.IAccno);
                    DateTime accountBranchDate = commonService.GetOtherBranchTransactionDateByBranchId(AccountBranchId);
                    DateTime userBranchDate = commonService.GetBranchTransactionDate();


                    if (userBranchDate != accountBranchDate)
                    {
                        returnMessage.Msg = "Transaction Dates for the two Branches don't match. \n Cannot complete Cheque Clearence .";
                        returnMessage.Success = false;
                        return returnMessage;

                    }

                    int accounState = TellerUtilityService.GetAccountStatus(ChequeDetail.IAccno);
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
                    returnMessage = TellerUtilityService.GetCheckValidAccountStatus(ChequeDetail.IAccno);
                    if (!returnMessage.Success)
                    {
                        return returnMessage;
                    }
                    int productId = TellerUtilityService.GetPid(ChequeDetail.IAccno);
                    if (userBranchId != AccountBranchId)
                    {
                        bool allowABBS = Convert.ToBoolean(GetProductDetails(productId).IntraBrnhTrnx);
                        if (allowABBS == false)
                        {
                            returnMessage.Msg = "Deposit is not allowed for this Account.!!";
                            returnMessage.Success = false;
                            return returnMessage;
                        }
                    }
                    ASClearance chequeDetail = new ASClearance();
                    chequeDetail.IAccno = ChequeDetail.IAccno;
                    chequeDetail.Bankname = ChequeDetail.Bankname;
                    chequeDetail.Brnhname = ChequeDetail.Brnhname;
                    chequeDetail.chqno = ChequeDetail.chqno;
                    chequeDetail.payee = ChequeDetail.payee;
                    chequeDetail.tdate = commonService.GetBranchTransactionDate();
                    chequeDetail.camount = ChequeDetail.camount;
                    chequeDetail.remarks = ChequeDetail.remarks;
                    chequeDetail.postedby = ChequeDetail.postedby;
                    chequeDetail.accno = ChequeDetail.accno;
                    chequeDetail.VDate = ChequeDetail.VDate;
                    chequeDetail.Postedon = commonService.GetDate();
                    chequeDetail.postedby = Global.UserId;
                    chequeDetail.BrchId = Global.BranchId;
                    uow.Repository<ASClearance>().Add(chequeDetail);

                    commonService.InsertAvailableBalance(2, ChequeDetail.IAccno, ChequeDetail.camount);

                    uow.Commit();

                    taskUow.SaveTaskNotification(taskVerifierList, chequeDetail.rno, 27);
                    transaction.Commit();
                    returnMessage.Success = true;
                    returnMessage.Msg = " Cheque Saved Sucessfully";
                    return returnMessage;

                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    returnMessage.Success = false;
                    returnMessage.Msg = "Cheque Clearence Not Saved" + ex.Message;
                    return returnMessage;
                }
            }
        }
        public List<ChequeClearenceViewModel> chequeListForVerification()
        {
            var chequeList = uow.Repository<ASClearance>().FindByMany(x => x.chqstate == null).Select(x => new ChequeClearenceViewModel()
            {
                accno = x.accno,
                Bankname = x.Bankname,
                Brnhname = x.Brnhname,
                camount = x.camount,
                chqno = x.chqno,
                IAccno = x.IAccno,
                VDate = x.VDate,
                payee = x.payee,
                remarks = x.remarks,
                chqstate = x.chqstate,
                postedby = x.postedby,
                Postedon = x.Postedon,
                tdate = x.tdate,
                rno = x.rno
            }).ToList();
            return chequeList;
        }
        public ChequeClearenceViewModel ChequeClearenceDetail(int rno)
        {
            ChequeClearenceViewModel Detail = uow.Repository<ASClearance>().FindBy(x => x.rno == rno).Select(x => new ChequeClearenceViewModel()
            {
                accno = x.accno,
                Bankname = x.Bankname,
                Brnhname = x.Brnhname,
                camount = x.camount,
                chqno = x.chqno,
                IAccno = x.IAccno,
                VDate = x.VDate,
                payee = x.payee,
                remarks = x.remarks,
                chqstate = x.chqstate,
                postedby = x.postedby,
                Postedon = x.Postedon,
                tdate = x.tdate,
                rno = x.rno
            }).SingleOrDefault();
            return Detail;
        }
        public void AcceptRejectChequeVerification(int rno, string remarks)
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
                    ASClearance rejectitem = uow.Repository<ASClearance>().FindBy(x => x.rno == rno).SingleOrDefault();
                    rejectitem.chqstate = false;
                    rejectitem.RejectRemarks = remarks;
                    commonService.InsertAvailableBalance(2, rejectitem.IAccno, (-rejectitem.camount));
                    uow.Repository<ASClearance>().Edit(rejectitem);
                    uow.Commit();
                    transaction.Complete();
                }
                catch (Exception)
                {
                    transaction.Dispose();
                    throw;
                }
            }

        }
        public ReturnBaseMessageModel VerifyChequeClearence(int rno, DateTime clearanceDate, string remarks)
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
                    ASClearance verifiedCheque = uow.Repository<ASClearance>().FindBy(x => x.rno == rno).SingleOrDefault();
                    AMClearance verifiedChequesubmit = new AMClearance();

                    verifiedChequesubmit.accno = verifiedCheque.accno;
                    verifiedChequesubmit.bankname = verifiedCheque.Bankname;
                    verifiedChequesubmit.brnhname = verifiedCheque.Brnhname;
                    verifiedChequesubmit.camount = verifiedCheque.camount;
                    verifiedChequesubmit.chqno = verifiedCheque.chqno;
                    verifiedChequesubmit.IAccno = verifiedCheque.IAccno;
                    verifiedChequesubmit.VDate = verifiedCheque.VDate;
                    verifiedChequesubmit.payee = verifiedCheque.payee;
                    if (remarks != "")
                        verifiedChequesubmit.remarks = verifiedCheque.remarks + ".\n. Clearence Message:" + remarks;
                    else
                        verifiedChequesubmit.remarks = verifiedCheque.remarks;
                    verifiedChequesubmit.postedon = verifiedCheque.Postedon;
                    verifiedChequesubmit.postedby = verifiedCheque.postedby;
                    verifiedChequesubmit.tdate = verifiedCheque.tdate;
                    verifiedChequesubmit.rno = verifiedCheque.rno;
                    verifiedChequesubmit.BrchId = verifiedCheque.BrchId;
                    verifiedChequesubmit.verifiedon = commonService.GetDate();
                    verifiedChequesubmit.verifiedby = UserId;
                    verifiedChequesubmit.cdate = Convert.ToDateTime(clearanceDate);

                    uow.Repository<ASClearance>().Delete(verifiedCheque);
                    uow.Repository<AMClearance>().Add(verifiedChequesubmit);
                    uow.Commit();
                    commonService.InsertAvailableBalance(2, verifiedChequesubmit.IAccno, (-verifiedChequesubmit.camount));
                    commonService.InsertAvailableBalance(3, verifiedChequesubmit.IAccno, verifiedChequesubmit.camount);

                    var account = uow.Repository<ADetail>().FindBy(x => x.IAccno == verifiedChequesubmit.IAccno).FirstOrDefault();
                    account.Bal = account.Bal + verifiedChequesubmit.camount;
                    uow.Repository<ADetail>().Edit(account);
                    uow.Commit();
                    transaction.Complete();
                    returnMessage.Msg = "Successfully Cheque Cleared";
                    returnMessage.Success = true;
                    return returnMessage;
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    returnMessage.Success = false;
                    returnMessage.Msg = " Not saved.!!" + ex.Message;
                    return returnMessage;
                }
            }
        }

        public List<AMClearenceViewModel> GetClearedlist(string IAccno, string bankname, DateTime fromdate, DateTime toDate, int pageNo, int pageSize)
        {
            int branchID = Global.BranchId;
            string query = "";
            query = @"select COUNT(*) OVER () AS TotalCount,* from fin.FgetReportChequeClearence('" + fromdate + "','" + toDate + "'," + branchID + ")";
            if (IAccno != "" && bankname != "")
            {
                query += " where AccountNumber like'%" + IAccno + "%' and bankname like'%" + bankname + "%'";
            }
            else if (IAccno == "" && bankname != "")
            {
                query += " where  bankname like'%" + bankname + "%'";
            }
            else if (IAccno != "" && bankname == "")
            {
                query += " where AccountNumber like'%" + IAccno + "%'";
            }

            query += @" ORDER BY  AccountName
                       OFFSET ((" + pageNo + @" - 1) * " + pageSize + @") ROWS
                       FETCH NEXT " + pageSize + " ROWS ONLY";
            var clearedList = uow.Repository<AMClearenceViewModel>().SqlQuery(query).ToList();
            return clearedList;
        }

        public List<AccountDetailsViewModel> GetAccountList(int pid = 0, int DVId = 0, int ICBId = 0, int DBId = 0, decimal Value = 0)
        {
            List<AccountDetailsViewModel> accountList = new List<AccountDetailsViewModel>();

            if (TellerUtilityService.IsFixedAccount(pid))
            {
                accountList = uow.Repository<AccountDetailsViewModel>().SqlQuery(@"select a.PID,a.IRate,Accno,Aname,a.IAccno from fin.ADetail a inner join fin.AICBDur  b on a.IAccno=b.IAccno  inner join fin.ProductDurationInt c on  a.PID=c.PId where a.PID = " + pid + " and c.DVId = " + DVId + " and AccState = 1").ToList();
            }
            else if (TellerUtilityService.IsRecurringDeposit(pid) || TellerUtilityService.IsOtherTypeOfRecurringDeposit(pid))
            {
                accountList = uow.Repository<AccountDetailsViewModel>().SqlQuery("select a.PID,a.IRate,Accno,Aname,a.IAccno from fin.ADetail a inner join fin.AICBDur  b on a.IAccno = b.IAccno inner join fin.ProductDurationInt c on  a.PID = c.PId where a.PID = " + pid + "  and c.DVId = " + DVId + " and c.ICBId = " + ICBId + " and c.DBId = " + DBId + " and c.Value = " + Value + " and AccState = 1").ToList();
            }
            else
            {
                accountList = uow.Repository<AccountDetailsViewModel>().SqlQuery("select a.PID,cast(c.IRate as decimal(18,2)) as IRate,Accno,Aname,a.IAccno from fin.ADetail a inner join fin.AICBDur  b on a.IAccno = b.IAccno inner join fin.ProductICBDur c on  a.PID = c.PId where b.ICBDurID = " + ICBId + " and a.pid =" + pid + " and AccState = 1").ToList();
            }
            return accountList;
        }

        public List<AccountDetailsViewModel> GetAccountListFilterStype(int pid = 0, int DVId = 0, int ICBId = 0, int DBId = 0, decimal Value = 0)
        {
            List<AccountDetailsViewModel> accountList = new List<AccountDetailsViewModel>();
            int SDID = uow.Repository<ProductDetail>().GetSingle(x => x.PID == pid).SDID;
            int sType = uow.Repository<SchmDetail>().GetSingle(x => x.SDID == SDID).SType;

            if (sType == 0)
            {
                if (TellerUtilityService.IsFixedAccount(pid))
                {
                    accountList = uow.Repository<AccountDetailsViewModel>().SqlQuery(@"select a.PID,a.IRate,Accno,Aname,a.IAccno from fin.ADetail a inner join fin.AICBDur  b on a.IAccno=b.IAccno  inner join fin.ProductDurationInt c on  a.PID=c.PId where a.PID = " + pid + " and c.DVId = " + DVId + " and AccState = 1").ToList();
                }
                else if (TellerUtilityService.IsRecurringDeposit(pid) || TellerUtilityService.IsOtherTypeOfRecurringDeposit(pid))
                {
                    accountList = uow.Repository<AccountDetailsViewModel>().SqlQuery("select a.PID,a.IRate,Accno,Aname,a.IAccno from fin.ADetail a inner join fin.AICBDur  b on a.IAccno = b.IAccno inner join fin.ProductDurationInt c on  a.PID = c.PId where a.PID = " + pid + "  and c.DVId = " + DVId + " and c.ICBId = " + ICBId + " and c.DBId = " + DBId + " and c.Value = " + Value + " and AccState = 1").ToList();
                }
                else
                {
                    accountList = uow.Repository<AccountDetailsViewModel>().SqlQuery("select a.PID,cast(c.IRate as decimal(18,2)) as IRate,Accno,Aname,a.IAccno from fin.ADetail a inner join fin.AICBDur  b on a.IAccno = b.IAccno inner join fin.ProductICBDur c on  a.PID = c.PId where b.ICBDurID = " + ICBId + " and a.pid =" + pid + " and AccState = 1").ToList();
                }
            }
            else
            {
                accountList = uow.Repository<AccountDetailsViewModel>().SqlQuery(@"select PID,IRate,Accno,Aname,IAccno from fin.ADetail where PID =" + pid + "and AccState = 1").ToList();
            }
            return accountList;
        }

        public List<AccountForceCloseViewModel> GetAccountListByPID(byte pid = 0)
        {
            List<AccountForceCloseViewModel> accountList = new List<AccountForceCloseViewModel>();

            if (TellerUtilityService.IsFixedAccount(pid))
            {
                accountList = uow.Repository<AccountForceCloseViewModel>().SqlQuery(@"select a.PID,a.IRate,Accno,Aname,a.IAccno from fin.ADetail a inner join fin.AICBDur  b on a.IAccno=b.IAccno  inner join fin.ProductDurationInt c on  a.PID=c.PId where a.PID = " + pid + " ").ToList();
            }
            else if (TellerUtilityService.IsRecurringDeposit(pid) || TellerUtilityService.IsOtherTypeOfRecurringDeposit(pid))
            {
                accountList = uow.Repository<AccountForceCloseViewModel>().SqlQuery("select a.PID,a.IRate,Accno,Aname,a.IAccno from fin.ADetail a inner join fin.AICBDur  b on a.IAccno = b.IAccno inner join fin.ProductDurationInt c on  a.PID = c.PId where a.PID = " + pid + " ").ToList();
            }
            else
            {
                accountList = uow.Repository<AccountForceCloseViewModel>().SqlQuery("select a.PID,a.IRate,Accno,Aname,a.IAccno from fin.ADetail a inner join fin.AICBDur  b on a.IAccno = b.IAccno inner join fin.ProductICBDur c on  a.PID = c.PId where a.pid =" + pid + " ").ToList();
            }
            return accountList;

        }

        public ReturnBaseMessageModel ForceCloseAccount(AccountForceCloseViewModel accountForceClose)
        {
            //using (TransactionScope transaction = new TransactionScope(
            //  // a new transaction will always be created
            //  TransactionScopeOption.Required,
            //  // we will allow volatile data to be read during transaction
            //  new TransactionOptions()
            //  {
            //      IsolationLevel = IsolationLevel.ReadCommitted,
            //      Timeout = TimeSpan.MaxValue

            //  }))



            using (var transaction = uow.GetContext().Database.BeginTransaction())

            {



                try
                {


                    var getUserAssignMenu = commonService.GetUserAssignMenu(14136, Global.UserId);
                    if (getUserAssignMenu != null)
                    {

                        ReturnBaseMessageModel returnMessage = new ReturnBaseMessageModel();
                        bool isMature = TellerUtilityService.IsAlreadyMatured(accountForceClose.IAccno);

                        var guarantor = GetGuarantorList(accountForceClose.IAccno);


                        bool IsLonee = guarantor.Select(x => x.IsLoanee).FirstOrDefault();
                        decimal amount = TellerUtilityService.CheckForGuarantor(accountForceClose.IAccno);

                        int Pid = TellerUtilityService.GetPid(accountForceClose.IAccno);
                        bool duration = TellerUtilityService.HasDuration(Pid);
                        //if (IsLonee)
                        //{
                        //    returnMessage.Msg = "This  is Loanee of account No. #(" + guarantor.Select(x => x.AccountNumber).FirstOrDefault() + ")#  with Blocked Amount " + amount.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture);
                        //    returnMessage.Success = false;
                        //    return returnMessage;
                        //}

                        if (IsLonee)
                        {
                            returnMessage.Msg = "This  is Loanee of account No. #(" + guarantor.Select(x => x.AccountNumber).FirstOrDefault() + ")#  with Blocked Amount " + amount.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture);
                            returnMessage.Success = false;
                            return returnMessage;
                        }
                        if (guarantor.Count() == 1)
                        {
                            returnMessage.Msg = "This  is Guarantor of account No. #(" + guarantor.Select(x => x.AccountNumber).FirstOrDefault() + ")#  with Blocked Amount " + amount.ToString("0,0.00", System.Globalization.CultureInfo.InvariantCulture);
                            returnMessage.Success = false;
                            return returnMessage;
                        }
                        else if (guarantor.Count() > 1)
                        {
                            returnMessage.Msg = "This account  is Guarantor of other Loan Accounts  with Total Blocked Amount" + amount;
                            returnMessage.Success = false;
                            return returnMessage;
                        }
                        if (duration) //saving and normal account haina
                        {
                            if (!isMature)
                            {
                                if (!accountForceClose.IsForceClosed)
                                {
                                    returnMessage.Msg = returnMessage.Msg + " Closing is not Allowed.!  Account is immatured.!!";
                                    returnMessage.Success = false;
                                    returnMessage.ReturnId = 1; //check premature account to close option
                                    return returnMessage;
                                }
                            }
                        }
                        var interestPayable = InterestPayable(accountForceClose.IAccno);

                        if (interestPayable != null)
                        {
                            if (interestPayable.Amount > 0)
                            {
                                returnMessage.Msg = "This account has Payable Interest";
                                returnMessage.Success = false;
                                return returnMessage;
                            }
                        }
                   



                        commonService.AccountStatusLogChange(7, accountForceClose.IAccno); //ready to close

                        var accountToClose = uow.Repository<ADetail>().FindBy(x => x.IAccno == accountForceClose.IAccno).FirstOrDefault();
                        accountToClose.AccState = 7;
                        uow.Repository<ADetail>().Edit(accountToClose);
                        uow.Commit();
                        transaction.Commit();
                        //     transaction.Complete();

                        returnMessage.Msg = "Account successfully CLOSED";
                        returnMessage.Success = true;
                        return returnMessage;
                    }
                    else
                    {
                        returnMessage.Msg = "This User has no permission to close  Account";
                        returnMessage.Success = false;
                        return returnMessage;
                    }
                }

                catch (Exception ex)
                {
                    returnMessage.Success = false;
                    returnMessage.Msg = "Account Not Closed" + ex.Message;
                    transaction.Dispose();
                    return returnMessage;

                }
            }
        }
        public List<ChequeClearenceViewModel> GetVerificationList(int pageNo, int pageSize)
        {

            int branchId = Global.BranchId;
            string query = "";
            //  query = @"select COUNT(*) OVER () AS TotalCount,* from fin.FgetReportChequeClearence('" + fromdate + "','" + toDate + "')";
            query = @"select COUNT(*) OVER () AS TotalCount, accno,
               Bankname,
               Brnhname,
               camount,
               chqno,
               IAccno,
               VDate,
               payee,
               remarks,
               chqstate,
              postedby,
              Postedon,
              tdate,
               rno  from fin.ASClearance where chqstate is null and BrchId=" + branchId + " ";


            query += @" ORDER BY  Bankname
                       OFFSET ((" + pageNo + @" - 1) * " + pageSize + @") ROWS
                       FETCH NEXT " + pageSize + " ROWS ONLY";
            var clearedList = uow.Repository<ChequeClearenceViewModel>().SqlQuery(query).ToList();
            return clearedList;
        }
        #endregion

        public ReturnBaseMessageModel DepositAccountRenewSave(AccountRenewModel renewModel)
        {
            using (TransactionScope transaction = new TransactionScope(
               // a new transaction will always be created
               TransactionScopeOption.Required,
               // we will allow volatile data to be read during transaction
               new TransactionOptions()
               {
                   IsolationLevel = IsolationLevel.ReadCommitted,
                   Timeout = TimeSpan.MaxValue

               }
           ))
            {
                try
                {
                    DateTime transactionDate = commonService.GetBranchTransactionDate();
                    HADRenew depositRenewRow = new HADRenew();
                    depositRenewRow.IAccNo = renewModel.IAccno;
                    depositRenewRow.DurationId = renewModel.Duration;
                    depositRenewRow.ValDate = transactionDate;
                    depositRenewRow.MatDate = renewModel.MaturationDate;
                    depositRenewRow.IntCalcRule = renewModel.InterestCalRule;
                    depositRenewRow.IntCaptDur = renewModel.InterestCapitalize;
                    depositRenewRow.IntRate = renewModel.InterestRate;
                    if (TellerUtilityService.IsFixedAccount(renewModel.PID))
                    {
                        depositRenewRow.AgrAmt = renewModel.AgreementAmount;
                    }
                    else
                    {
                        depositRenewRow.AgrAmt = renewModel.ContributionAmount;
                        depositRenewRow.Basic = renewModel.Basic;
                    }
                    depositRenewRow.RNDate = commonService.GetDate();
                    uow.Repository<HADRenew>().Add(depositRenewRow);
                    byte schemeId = GetSchemeIdByProductId(renewModel.PID);

                    ADrLimit drLimitRow = uow.Repository<ADrLimit>().FindByMany(x => x.IAccno == renewModel.IAccno).FirstOrDefault();
                    drLimitRow.AppAmt = (decimal)depositRenewRow.AgrAmt;
                    uow.Repository<ADrLimit>().Edit(drLimitRow);
                    //duration mature date update based on new duration
                    ADur accountDuration = uow.Repository<ADur>().FindByMany(x => x.IAccno == renewModel.IAccno).FirstOrDefault();
                    accountDuration.MatDat = renewModel.MaturationDate;
                    uow.Repository<ADur>().Edit(accountDuration);

                    ARateMaster accountRateMaster = new ARateMaster();
                    accountRateMaster.PostedBy = Global.UserId;
                    accountRateMaster.PostedDate = transactionDate;
                    accountRateMaster.EffectiveDate = transactionDate;

                    ARate accountInterestRate = new ARate();
                    accountInterestRate.IAccno = (int)depositRenewRow.IAccNo;
                    accountInterestRate.IRate = (float)renewModel.InterestRate;
                    accountRateMaster.ARates.Add(accountInterestRate);
                    uow.Repository<ARateMaster>().Add(accountRateMaster);

                    ADetail accountDetails = uow.Repository<ADetail>().FindByMany(x => x.IAccno == renewModel.IAccno).FirstOrDefault();
                    accountDetails.IRate = renewModel.InterestRate;
                    accountDetails.AccState = 1;
                    uow.Repository<ADetail>().Edit(accountDetails);


                    var policyInterest = uow.Repository<APolicyInterest>().FindByMany(x => x.IAccno == renewModel.IAccno).FirstOrDefault();

                    policyInterest.PSID = renewModel.InterestCalRule;
                    uow.Repository<APolicyInterest>().Edit(policyInterest);
                    var ruleIcdDuration = uow.Repository<AICBDur>().FindByMany(x => x.IAccno == renewModel.IAccno).FirstOrDefault();

                    ruleIcdDuration.ICBDurID = renewModel.InterestCapitalize;
                    uow.Repository<AICBDur>().Edit(ruleIcdDuration);


                    //if schedule calculate is from now then interest calculation date schedule created
                    if (TellerUtilityService.IsFromNow(schemeId))
                    {
                        string type = commonService.DateType();

                        var AdshOldList = uow.Repository<ADSch>().FindByMany(x => x.IAccno == renewModel.IAccno).ToList();
                        foreach (var item in AdshOldList)
                        {
                            ADSchLog adlogRow = new ADSchLog();
                            adlogRow.IAccno = item.IAccno;
                            adlogRow.MDate = item.MDate;
                            uow.Repository<ADSchLog>().Add(adlogRow);
                            uow.Repository<ADSch>().Delete(item);
                        }

                        var durationById = this.GetDurationByDurationId(renewModel.Duration);
                        int matDuration = 0;
                        int interestCal = 0;
                        matDuration = TellerUtilityService.GetmatDurationMonth(durationById.Value);
                        float value = GetCapitalizeRuleValueByProductDurationIntId(renewModel.InterestCapitalize);
                        try
                        {
                            interestCal = Convert.ToInt32(matDuration / value);
                        }
                        catch
                        {
                            interestCal = 0;
                        }
                        float currentMonth = value;
                        DateTime startDate = (DateTime)depositRenewRow.ValDate;

                        for (int i = 0; i < interestCal; i++)
                        {
                            ADSch adSch = new ADSch();
                            if (type == "1")
                            {
                                DateTime scheduleDate = commonService.GetMatDate(startDate, Convert.ToDecimal(currentMonth), type);
                                adSch.IAccno = renewModel.IAccno;
                                adSch.MDate = scheduleDate;

                                currentMonth += value;
                            }
                            else
                            {
                                DateTime scheduleDate = commonService.GetMatDate(startDate, Convert.ToDecimal(value), type);
                                adSch.IAccno = renewModel.IAccno;
                                adSch.MDate = scheduleDate;

                                startDate = scheduleDate;
                            }
                            uow.Repository<ADSch>().Add(adSch);
                        }
                    }
                    commonService.AccountStatusLogChange(1, renewModel.IAccno);
                    uow.Commit();
                    transaction.Complete();
                    returnMessage.Msg = "Renew Account Sussessfully.";
                    returnMessage.Success = true;
                }

                catch (Exception ex)
                {
                    transaction.Dispose();
                    returnMessage.Msg = ex.InnerException.Message + "-Error.!!";
                    returnMessage.Success = false;
                }
                return returnMessage;
            }
        }
        public bool CheckTemplateName(string Tname = "", int TID = 0)
        {

            int count = uow.Repository<TempIntRate>().GetAll().Where(x => x.Tname.ToLower().Trim() == Tname.ToLower().Trim()).Where(x => x.TID != TID).Count();


            if (count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckSheetNo(int? SheetNo, int RetId = 0)
        {
            var branch = Loader.Models.Global.BranchId;
            var fiscalYear = Loader.Models.Global.getCurrentFYID(branch);
            int count = 0;
            var fiscalYearFull = luow.Repository<Loader.Models.FiscalYear>().GetSingle(x => x.FYID == fiscalYear);
            var collSheet = uow.Repository<CollSheet>().GetSingle(x => x.SheetNo == SheetNo && x.VDate > fiscalYearFull.StartDt && x.VDate < fiscalYearFull.EndDt);
            var collMainTemp = uow.Repository<CollMainTemp>().GetSingle(x => x.SheetNo == SheetNo && x.VDate > fiscalYearFull.StartDt && x.VDate < fiscalYearFull.EndDt && x.Status == true); // for reject case

            if (collSheet != null)
            {
                if (collSheet.TDate > fiscalYearFull.StartDt)
                {
                    if (collSheet.TDate < fiscalYearFull.EndDt)
                    {
                        count++;
                    }
                }

            }
            if (collMainTemp != null)
            {
                if (collMainTemp.TDate > fiscalYearFull.StartDt)
                {
                    if (collMainTemp.TDate < fiscalYearFull.EndDt)
                    {
                        count++;
                    }
                }

            }

            if (count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}



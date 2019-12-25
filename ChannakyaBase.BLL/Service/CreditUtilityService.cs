using ChannakyaBase.BLL.Repository;
using ChannakyaBase.DAL.DatabaseModel;
using ChannakyaBase.Model.Models;
using ChannakyaBase.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ChannakyaBase.BLL.Service
{
    public static class  CreditUtilityService
    {
        private static GenericUnitOfWork uow = null;
        static CreditUtilityService()
        {
            uow = new GenericUnitOfWork();
        }
        public static SelectList PaymentModeList()
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var paymentMode = uow.Repository<DropdownModel>().SqlQuery("select * from FGetPaymentModeDictionary()").ToList();
                return new SelectList(paymentMode, "Id", "Text");
            }
        }
        public static SelectList GraceOption()
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var paymentMode = uow.Repository<DropdownModel>().SqlQuery("select * from FGetGraceOptionDictionary()").ToList();
                return new SelectList(paymentMode, "Id", "Text");
            }
        }
        public static SelectList GetAllLoanScheme()
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var schemeList = uow.Repository<DropdownModelForSchemeName>().SqlQuery("select distinct SD.SDID as SDID, SD.SDName as SchemeName from [fin].[SchmDetails] SD join [fin].[ProductDetail] PD on PD.SDID = SD.SDID where SD.SType = 1 and SEnable=1 ").ToList();
                //var schemeList = uow.Repository<SchmDetail>().FindBy(x => x.SType == 1).Select(x => new SchemeModel()
                //{
                //    SchemeID = x.SDID,
                //    SchemeName = x.SDName
                //}).OrderBy(x => x.SchemeName).ToList();
                return new SelectList(schemeList, "SDID", "SchemeName");
            }

        }
        public static SelectList GetAllRulePaySchedule()
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var rulePaysch = uow.Repository<RulePaySch>().FindBy(x => x.active == true).ToList();
                return new SelectList(rulePaysch, "PAYSID", "schType");
            }

        }
        public static SelectList GetNCollateralDetail()
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var collater = uow.Repository<NCollateralDetail>().FindBy(x => x.IsActive == true).ToList().OrderBy(x=>x.Collateral);
                return new SelectList(collater, "NCId", "Alias");
            }
        }
        public static SelectList VechileType()
        {
            List<SelectListItem> objVechicleOption = new List<SelectListItem>();

            objVechicleOption.Add(new SelectListItem { Text = "Two Wheeler", Value = "1" });
            objVechicleOption.Add(new SelectListItem { Text = "Three Wheeler", Value = "2" });
            objVechicleOption.Add(new SelectListItem { Text = "Four Wheeler", Value = "3" });
            objVechicleOption.Add(new SelectListItem { Text = "Other", Value = "4" });
            return new SelectList(objVechicleOption, "Value", "Text");
        }
        public static SelectList GetAllRuleCalculation(int ProductId = 0)
        {
            CreditService cs = new CreditService();
            var intcalrule = cs.GetAllRuleCalculation(ProductId);
            return new SelectList(intcalrule, "InCalRuleId", "InterestCalRule");


        }
        public static SelectList GetAllPenaltyCalculation(int ProductId = 0)
        {
            CreditService cs = new CreditService();
            var intcalrule = cs.GetAllPenalty(ProductId);
            return new SelectList(intcalrule, "PCID", "PCNAME");
        }
        public static SelectList GetAllPaymentMode(int ProductId = 0)
        {
            CreditService cs = new CreditService();
            var paymode = cs.GetAllPaymentMode(ProductId);
            return new SelectList(paymode, "PAYID", "PRule");
        }

        public static decimal PayCalcutation(int iaccNo, string param)
        {
            CreditService creditService = new CreditService();
            var getPayAmount = creditService.GetLoanPayment(iaccNo);
            decimal total = 0;
            if (param == "Other")
            {
                total = Convert.ToDecimal(getPayAmount.IonCA + getPayAmount.IonCR +
                    getPayAmount.IonIA + getPayAmount.IonIR +
                    getPayAmount.IonPA + getPayAmount.IonPR +
                    getPayAmount.OthrBal + getPayAmount.PonIA + getPayAmount.PonIR + getPayAmount.PonPrA + getPayAmount.PonPrR);
            }
            else
            {
                total = Convert.ToDecimal(getPayAmount.IonCA + getPayAmount.IonCR +
                   getPayAmount.IonIA + getPayAmount.IonIR +
                   getPayAmount.IonPA + getPayAmount.IonPR +
                   getPayAmount.PonIA + getPayAmount.PonIR + getPayAmount.PonPrA + getPayAmount.PonPrR);
            }
            return total;
        }
        public static SelectList PrePaymentMode()
        {
            List<SelectListItem> PreMode = new List<SelectListItem>();
            PreMode.Add(new SelectListItem { Text = "Select one", Value = "-1" });
            PreMode.Add(new SelectListItem { Text = "Till Today", Value = "1" });
            PreMode.Add(new SelectListItem { Text = "Till Next Day", Value = "2" });
            PreMode.Add(new SelectListItem { Text = "Till Next Installment", Value = "3" });
            PreMode.Add(new SelectListItem { Text = "Specify Date", Value = "4" });
            return new SelectList(PreMode, "Value", "Text");
        }

        public static ReturnBaseMessageModel ValidatePayment(LoanPaymentModel loanPayment)
        {
            ReturnBaseMessageModel returnMessage = new ReturnBaseMessageModel();

            CreditService creditService = new CreditService();
            var matureAndCurrentPa = creditService.CurrentMaturePrinciapAndInterest(loanPayment.IAccno);
            decimal MaturePA =Math.Round(((decimal)matureAndCurrentPa.CurrentPrincipal + (decimal)matureAndCurrentPa.MaturePrincipal),2);
            var getPayAmount = creditService.GetAccountLoanPaymentDetails(loanPayment.IAccno);
            decimal totalPayment = 0;
            #region OtherLoan
            decimal RemainingIonca = Convert.ToDecimal(getPayAmount.IonCA) /*- loanPayment.OtherIonCARebate*/ - loanPayment.OtherIonCAPayment;
            decimal paymentIonCA = Convert.ToDecimal(getPayAmount.IonCA) /*- loanPayment.OtherIonCARebate*/;
            totalPayment = totalPayment + loanPayment.OtherIonCAPayment;
            if (loanPayment.IsMature)
            {
                getPayAmount.IonPA = creditService.GetTillDateMatureInterest(loanPayment.IAccno);
            }
            else
            {
                getPayAmount.IonPA = creditService.GetTillCurrentDateMatureAmount(loanPayment.IAccno);
            }
            if (RemainingIonca != loanPayment.RemainingIonCA)
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Invalid other's ABNR Interest remaining amount.!!";
                return returnMessage;
            }

            if (paymentIonCA < Convert.ToDecimal(loanPayment.OtherIonCAPayment))
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Other's ABNR interest payment  amount not match with balance.!!";
                return returnMessage;
            }
            else
            {
                if (paymentIonCA < 0)
                {
                    returnMessage.Success = false;
                    returnMessage.Msg = "Other's ABNR payment  amount must be non negative.!!";
                    return returnMessage;
                }
            }

            decimal remainingIonCR = Convert.ToDecimal(getPayAmount.IonCR) /*- loanPayment.OtherIonCRRebate*/ - loanPayment.OtherIonCRPayment;
            decimal paymentIonCR = Convert.ToDecimal(getPayAmount.IonCR) /*- loanPayment.OtherIonCRRebate*/;

            totalPayment = totalPayment + loanPayment.OtherIonCRPayment;

            if (remainingIonCR != Convert.ToDecimal(loanPayment.RemainingIonCR))
            {
                returnMessage.Success = false;
                returnMessage.Msg = " Other's  Interest income remaining amount not match with balance.!!";
                return returnMessage;
            }
            if (paymentIonCR < Convert.ToDecimal(loanPayment.OtherIonCRPayment))
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Other's Interest income payment  amount not match with balance.!!";
                return returnMessage;
        }
            else
            {
                if (paymentIonCR < 0)
                {
                    returnMessage.Success = false;
                    returnMessage.Msg = "Other's  Interest income payment amount must be non negative.!!";
                    return returnMessage;
                }
            }


            decimal remainingOthrBal = Convert.ToDecimal(getPayAmount.OthrBal) - loanPayment.OtherOthrBalPayment;
            totalPayment = totalPayment + loanPayment.OtherOthrBalPayment;

            if (remainingOthrBal != Convert.ToDecimal(loanPayment.RemainingOthrBal))
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Other's  Outstanding remaining amount not match with balance.!!";
                return returnMessage;
            }

            if (loanPayment.OtherOthrBalPayment > Convert.ToDecimal(getPayAmount.OthrBal))
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Other's  Outstanding payment amount not match with balance.!!";
                return returnMessage;
            }
            #endregion

            #region Penalty 
            decimal RemainingPonIR = Convert.ToDecimal(getPayAmount.PonIR) /*- loanPayment.PenaltyAnbrPonIRRebate*/ - loanPayment.PenaltyAnbrPonIRPayment;
            decimal paymentPonIR = Convert.ToDecimal(getPayAmount.PonIR) /*- loanPayment.PenaltyAnbrPonIRRebate*/;

            totalPayment = totalPayment + loanPayment.PenaltyAnbrPonIRPayment;
            if (RemainingPonIR != loanPayment.RemainingPonIR)
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Penalty ABNR Interest remaining amount not match with actual remaining.!!";
                return returnMessage;
            }

            if (paymentPonIR < Convert.ToDecimal(loanPayment.PenaltyAnbrPonIRPayment))
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Penalty ABNR Interest payment  amount not match with balance.!!";
                return returnMessage;
            }
            else
            {
                if (paymentPonIR < 0)
                {
                    returnMessage.Success = false;
                    returnMessage.Msg = "Penalty ABNR Interest payment  amount must be non negative.!!";
                    return returnMessage;
                }
            }


            decimal RemainingPonPrR = Convert.ToDecimal(getPayAmount.PonPrR) /*- loanPayment.PenaltyAnbrPonPrRRebate*/ - loanPayment.PenaltyAnbrPonPrRPayment;
            decimal paymentPonPrR = Convert.ToDecimal(getPayAmount.PonPrR) /*- loanPayment.PenaltyAnbrPonPrRRebate*/;
            totalPayment = totalPayment + loanPayment.PenaltyAnbrPonPrRPayment;

            if (RemainingPonPrR != loanPayment.RemainingPonPrR)
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Penalty ABNR Principal amount not match with remaining amount.!!";
                return returnMessage;
            }

            if (paymentPonPrR < Convert.ToDecimal(getPayAmount.PenaltyAnbrPonPrRPayment))
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Penalty ABNR Principal payment  amount not match with balance.!!";
                return returnMessage;
            }
            else
            {
                if (paymentPonPrR < 0)
                {
                    returnMessage.Success = false;
                    returnMessage.Msg = "Penalty ABNR Principal payment  amount must be non negative.!!";
                    return returnMessage;
                }
            }
            decimal remainingIonIR = Convert.ToDecimal(getPayAmount.IonIR) /*- loanPayment.PenaltyAnbrIonIRRebate*/ - loanPayment.PenaltyAnbrIonIRPayment;
            decimal paymentIonIR = Convert.ToDecimal(getPayAmount.IonIR) /*- loanPayment.PenaltyAnbrIonIRRebate*/;
            totalPayment = totalPayment + loanPayment.PenaltyAnbrIonIRPayment;

            if (remainingIonIR != loanPayment.RemainingIonIR)
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Penalty ABNR IntOnInt amount not match with remaining amount.!!";
                return returnMessage;
            }
            if (paymentIonIR < Convert.ToDecimal(loanPayment.PenaltyAnbrIonIRPayment))
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Penalty ABNR IntOnInt payment  amount not match with balance.!!";
                return returnMessage;
            }
            else
            {
                if (paymentIonIR < 0)
                {
                    returnMessage.Success = false;
                    returnMessage.Msg = "Penalty ABNR IntOnInt payment  amount must be non negative.!!";
                    return returnMessage;
                }
            }

            decimal remainingPonIA = Convert.ToDecimal(getPayAmount.PonIA) /*- loanPayment.PenaltyIncomePonIARebate */- loanPayment.PenaltyIncomePonIAPayment;
            decimal paymentPonIA = Convert.ToDecimal(getPayAmount.PonIA) /*- loanPayment.PenaltyIncomePonIARebate*/;
            totalPayment = totalPayment + loanPayment.PenaltyIncomePonIAPayment;

            if (remainingPonIA != loanPayment.RemainingPonIA)
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Penalty Income Interest amount not match with remaining amount.!!";
                return returnMessage;
            }
            if (paymentPonIA < Convert.ToDecimal(loanPayment.PenaltyIncomePonIAPayment))
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Penalty Income Interest payment  amount not match with balance.!!";
                return returnMessage;
            }
            else
            {
                if (paymentPonIA < 0)
                {
                    returnMessage.Success = false;
                    returnMessage.Msg = "Penalty Income Interest payment  amount must be non negative.!!";
                    return returnMessage;
                }
            }

            decimal remainingPonPrA = Convert.ToDecimal(getPayAmount.PonPrA) /*- loanPayment.PenaltyIncomePonPrARebate*/ - loanPayment.PenaltyIncomePonPrAPayment;
            decimal paymentPonPrA = Convert.ToDecimal(getPayAmount.PonPrA) /*- loanPayment.PenaltyIncomePonPrARebate*/;
            totalPayment = totalPayment + loanPayment.PenaltyIncomePonPrAPayment;

            if (remainingPonPrA != loanPayment.RemainingPonPrA)
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Penalty Income Principal amount not match with remaining amount.!!";
                return returnMessage;
            }
            if (paymentPonPrA < Convert.ToDecimal(loanPayment.PenaltyIncomePonPrAPayment))
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Penalty Income Principal payment  amount not match with balance.!!";
                return returnMessage;
            }
            else
            {
                if (paymentPonPrA < 0)
                {
                    returnMessage.Success = false;
                    returnMessage.Msg = "Penalty Income Principal payment  amount must be non negative.!!";
                    return returnMessage;
                }
            }

            decimal remainingIonIA = Convert.ToDecimal(getPayAmount.IonIA) /*- loanPayment.PenaltyIncomeIonIARebate*/ - loanPayment.PenaltyIncomeIonIAPayment;
            decimal paymentIonIA = Convert.ToDecimal(getPayAmount.IonIA) /*- loanPayment.PenaltyIncomeIonIARebate*/;
            totalPayment = totalPayment + loanPayment.PenaltyIncomeIonIAPayment;

            if (remainingIonIA != loanPayment.RemainingIonIA)
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Penalty Income IntOnInt amount not match with remaining amount.!!";
                return returnMessage;
            }
            if (paymentIonIA < Convert.ToDecimal(loanPayment.PenaltyIncomeIonIAPayment))
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Penalty Income IntOnInt payment  amount not match with balance.!!";
                return returnMessage;
            }
            else
            {
                if (paymentIonIA < 0)
                {
                    returnMessage.Success = false;
                    returnMessage.Msg = "Penalty Income IntOnInt payment  amount must be non negative.!!";
                    return returnMessage;
                }
            }
            #endregion

            #region Interest
            decimal remainingIonPR = Convert.ToDecimal(getPayAmount.IonPR) /*- loanPayment.InterestIonPRRebate*/ - loanPayment.InterestIonPRPayment;
            decimal paymentIonPR = Convert.ToDecimal(getPayAmount.IonPR) /*- loanPayment.InterestIonPRRebate*/;
            totalPayment = totalPayment + loanPayment.InterestIonPRPayment;

            if (remainingIonPR != loanPayment.RemainingIonPR)
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Interest ABNR Principal amount not match with remaining amount.!!";
                return returnMessage;
            }
            if (paymentIonPR < Convert.ToDecimal(loanPayment.InterestIonPRPayment))
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Interest ABNR Principal payment  amount not match with balance.!!";
                return returnMessage;
            }
            else
            {
                if (paymentIonPR < 0)
                {
                    returnMessage.Success = false;
                    returnMessage.Msg = "Interest ABNR Principal payment  amount must be non negative.!!";
                    return returnMessage;
                }
            }

            decimal remainingIonPA = Convert.ToDecimal(getPayAmount.IonPA) /*- loanPayment.InterestIonPARebate*/ - loanPayment.InterestIonPAPayment;
            decimal paymentIonPA = Convert.ToDecimal(getPayAmount.IonPA)/* - loanPayment.InterestIonPARebate*/;
            totalPayment = totalPayment + loanPayment.InterestIonPAPayment;

            if (remainingIonPA != loanPayment.RemainingIonPA)
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Interest Income  Principal amount not match with remaining amount.!!";
                return returnMessage;
            }
            if (paymentIonPA < Convert.ToDecimal(loanPayment.InterestIonPAPayment))
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Interest Income  Principal payment  amount not match with balance.!!";
                return returnMessage;
            }
            else
            {
                if (paymentIonPA < 0)
                {
                    returnMessage.Success = false;
                    returnMessage.Msg = "Interest Income  Principal payment  amount must be non negative.!!";
                    return returnMessage;
                }
            }


            #endregion
            decimal remainingBalance = MaturePA - loanPayment.PrincipalMaturPayment;
            totalPayment = totalPayment + loanPayment.PrincipalMaturPayment;
            if (remainingBalance != loanPayment.RemainingMaturePA)
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Mature principal remaining amount not match with actual remaining.!!";
                return returnMessage;
            }
            if (MaturePA < loanPayment.PrincipalMaturPayment)
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Payment is greater than balance.!!";
                return returnMessage;
            }
            returnMessage.Success = true;
            return returnMessage;
        }

        public static bool CheckPendingTransaction(int accno)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var remainingTrans = uow.Repository<ASTrn>().FindBy(x => x.IAccno == accno).FirstOrDefault();
                if (remainingTrans != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public static DateTime GetMatureDate(int accno)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                DateTime? MatureDate= uow.Repository<ADur>().FindByMany(x => x.IAccno == accno).Select(x=>x.MatDat).FirstOrDefault();
                return (DateTime)MatureDate;


            }
        }
        public static ReturnBaseMessageModel CheckForPrePayment(LoanPaymentModel loanPayment)
        {
            ReturnBaseMessageModel rtnMessange = new ReturnBaseMessageModel();
            if (loanPayment.PaymentInterest != 0)
            {
                decimal total = Convert.ToDecimal(loanPayment.IonCA + loanPayment.IonCR +
                     loanPayment.IonIA + loanPayment.IonIR +
                     loanPayment.IonPA + loanPayment.IonPR +
                     loanPayment.OthrBal + loanPayment.PonIA + loanPayment.PonIR + loanPayment.PonPrA + loanPayment.PonPrR);
                if (total > 0)
                {
                    rtnMessange.Success = false;
                    rtnMessange.Msg = "Cannot Take Pre Payment When Due Amount Is Remaining";
                    return rtnMessange;
                }

            }
            return rtnMessange;
        }
  
        public static SelectList GetAllBankList()
        {
            CreditService cs = new CreditService();
            var banklist = cs.GetAllBankList();
            return new SelectList(banklist, "BankId", "BankName");


        }
        public static SelectList GetAllTellerList()
        {
            CommonService cs = new CommonService();
            var banklist = cs.GetAllTeller();

            return new SelectList(banklist, "UserId", "EmployeeName");

        }

        public static ReturnBaseMessageModel CheckAvailableBalance(int iaccNo,decimal? amount)
        {
            ReturnBaseMessageModel returnMsg = new ReturnBaseMessageModel();
            TellerService tellerService = new TellerService();
             returnMsg.Success = false;
            var AccountBalanceAmount = tellerService.GetSingleAccountDetails(iaccNo);
       
                if (AccountBalanceAmount.Bal < amount)
                {
                    returnMsg.Success = false;
                    returnMsg.Msg = "Apply amount is grater than balance to block.";
                }
                else
                {
                    returnMsg.Success = true;
                    returnMsg.Msg = "Garanter Added";
                }
           

          
            return returnMsg;
        }
        public static SelectList GetAllProductsByStype(int stype)
        {
            CommonService cs = new CommonService();
            var ProductList = cs.GetAllProductsByStype(stype);

            return new SelectList(ProductList, "ProductId", "ProductName");

        }
        public static bool IsAllowPrePayment(int iaccno)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var remainingTrans = uow.Repository<ALoan>().FindBy(x => x.IAccno == iaccno).FirstOrDefault();
                return remainingTrans.overPay;
            }
        }
    }
}
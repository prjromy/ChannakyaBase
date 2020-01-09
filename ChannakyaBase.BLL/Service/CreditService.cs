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
using ChannakyaCustomeDatePicker.Service;
using ChannakyaBase.BLL.CustomHelper;
using System.Data.Entity;
using MoreLinq;
using System.Transactions;


namespace ChannakyaBase.BLL.Service
{
    public class CreditService
    {
        ReturnBaseMessageModel returnMessage = null;
        //    private ChannakyaBaseEntities _context = null;
        private GenericUnitOfWork uow = null;
        private CommonService commonService = null;
        DatePickerService datePickerService = null;
        private ShareService shareService = null;
        BaseTaskVerificationService taskUow = null;
        public CreditService()
        {
            commonService = new CommonService();
            datePickerService = new DatePickerService();
            taskUow = new BaseTaskVerificationService();
            uow = new GenericUnitOfWork();
            returnMessage = new ReturnBaseMessageModel();
            shareService = new ShareService();
            //  _context = new ChannakyaBaseEntities();
        }
        public LoanRegAccOpenViewModel LoanRegistration()
        {
            LoanRegAccOpenViewModel loanregistration = new LoanRegAccOpenViewModel();
            loanregistration.RegistrationDate = commonService.GetBranchTransactionDate();
            loanregistration.AccountDetailsViewModel = new AccountDetailsViewModel();
            return loanregistration;
        }

        #region Schedule calculate
        public List<ScheduleTrialModel> GetScheduleDetails(ScheduleModel scheduleModel)
        {
            DateTime dtMature = default(DateTime);
            EScheduleType scheduleType = GetEScheduleTypeByVlue(scheduleModel.ScheduleType);
            try
            {
                if (scheduleModel.OldMatureDate == null)
                {
                    dtMature = GetMatureDate(scheduleModel.ValueDate, scheduleType, (int)scheduleModel.Duration);
                }
                else
                {
                    dtMature = (DateTime)scheduleModel.OldMatureDate;
                    scheduleModel.ValueDate = commonService.GetBranchTransactionDate();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Invalid maturity date.!!");

            }
            List<ScheduleTrialModel> scheduleList = GetSchedule(scheduleType, scheduleModel.PaymentMode, scheduleModel.ValueDate,
                                                         dtMature, scheduleModel.PrincipalDate, scheduleModel.InterestDate, scheduleModel.PrincipalFrequency,
                                                            scheduleModel.InterestFrequency, scheduleModel.Day, scheduleModel.Amount, scheduleModel.Rate, scheduleModel.Flat);

            return scheduleList;
        }

        private List<ScheduleTrialModel> GetSchedule(EScheduleType scheduleType, byte nPaymentMode, DateTime dStartDate, DateTime dMatureDate, DateTime dPrinGraceDate, DateTime dIntGraceDate, int nPrincipalFrequency, int nInterestFrequency, int nCustomDay, decimal nDisburseAmount, decimal nIntRate, bool IsFlat)
        {
            List<ScheduleTrialModel> scheduleTrial = new List<ScheduleTrialModel>();
            int nProvisionDays = 0;
            nProvisionDays = this.FGetSchProvision();
            if (nCustomDay == 0)
            {
                if (scheduleType == EScheduleType.DefaultEnglishDate)
                {
                    nCustomDay = dStartDate.Day;
                }
                else if (scheduleType == EScheduleType.DefaultNepaliDate)
                {
                    string strDateBS = datePickerService.GetDateBS(dStartDate);
                    nCustomDay = datePickerService.GetBSDay(strDateBS);
                }
            }
            if (nPaymentMode > 2)
            {
                scheduleTrial = this.GetScheduleFormatTB(scheduleType, dStartDate, dMatureDate, nPrincipalFrequency, nInterestFrequency, dPrinGraceDate, dIntGraceDate, nProvisionDays, nCustomDay);
            }
            else
            {

                scheduleTrial = this.GetScheduleFormatTB(scheduleType, dStartDate, dMatureDate, nPrincipalFrequency, nInterestFrequency, dPrinGraceDate, dIntGraceDate, nProvisionDays, nCustomDay);
                scheduleTrial = this.InsertDays(dStartDate, scheduleTrial, scheduleType);
                //Equal Principal
                if (nPaymentMode == 1)
                {
                    scheduleTrial = this.FEqPrin(scheduleTrial, nDisburseAmount, nIntRate, IsFlat);
                    //Equal Installment
                }
                else if (nPaymentMode == 2)
                {
                    scheduleTrial = this.FEqInstDaily(scheduleTrial, dStartDate, nDisburseAmount, nIntRate, IsFlat);
                }

            }
            return scheduleTrial;
        }

        public ScheduleTrialModel GetFinalizeCustmizeInstellment(ScheduleTrialModel scheduleModel)
        {
            try
            {
                if (scheduleModel.scheduleList.Where(x => x.IsChecked == true).Count() == 0)
                {
                    throw new Exception("Invalid Customize Point.please select row!!");
                }
                if (scheduleModel.scheduleList[scheduleModel.scheduleList.Where(x => x.IsChecked == true).Count() - 1].HasPrincipal == false)
                {
                    throw new Exception("Cannot Proceed.. Coz [Has Principal] = False");
                }
                if (scheduleModel.Percent == 0)
                {
                    throw new Exception("Please Enter Distribute Percentage!!!");
                }
                if (scheduleModel.Remprecent - scheduleModel.Percent < 0)
                {
                    throw new Exception("Percentage Cannot Grater Than 100!!!");
                }
                if (scheduleModel.Remprecent - scheduleModel.Percent == 0)
                {
                    if (scheduleModel.scheduleList[scheduleModel.scheduleList.Count() - 1].IsChecked == false)
                    {
                        throw new Exception("Percentage Cannot Exceed 100!!!");
                    }
                }
                if (scheduleModel.scheduleList[scheduleModel.scheduleList.Count() - 1].IsChecked == true)
                {
                    if (scheduleModel.Remprecent - scheduleModel.Percent != 0)
                    {
                        throw new Exception("Percentage Must Reach 100 At Last Installment !!!");
                    }
                }
                if (scheduleModel.scheduleList.Where(x => x.IsChecked == true).Count() == 0)
                {
                    throw new Exception("Invalid Customize Point !!!");
                }
                int i = scheduleModel.scheduleList.Where(x => x.HasPrincipal == true && x.IsChecked == true && x.Balance == 0).Count();

                decimal npc = scheduleModel.Percent;
                decimal nPrincipal = scheduleModel.Amount;
                decimal nBalance = 0;
                if (scheduleModel.RemainingBalance == 0)
                {
                    nBalance = nPrincipal;
                }
                else
                {
                    nBalance = scheduleModel.RemainingBalance;
                }

                decimal nTotalInst = Math.Round(nPrincipal * npc / 100, 2);
                decimal nInstallment;
                decimal nRunningInst = 0;

                nInstallment = Math.Round(nTotalInst / i, 2);
                for (int n = 0; n < scheduleModel.scheduleList.Count; n++)
                {
                    if (scheduleModel.scheduleList[n].HasPrincipal == true)
                    {
                        if (scheduleModel.scheduleList[n].IsChecked == true)
                        {
                            if (scheduleModel.scheduleList[n].Balance == 0)
                            {
                                scheduleModel.scheduleList[n].PrincipalInstall = nInstallment;
                                nRunningInst = Math.Round(nRunningInst + nInstallment, 2);
                                nBalance = Math.Round(nBalance - nInstallment, 2);
                                scheduleModel.scheduleList[n].Balance = nBalance;
                            }
                        }
                    }
                    else
                    {
                        if (scheduleModel.scheduleList[n].IsChecked == true)
                        {
                            if (scheduleModel.scheduleList[n].Balance == 0)
                            {
                                scheduleModel.scheduleList[n].Balance = nBalance;
                            }
                        }
                    }
                }

                if (nTotalInst != nRunningInst)
                {
                    decimal nDiffInst;
                    nDiffInst = nTotalInst - nRunningInst;
                    if (nDiffInst > 0)
                    {
                        nBalance = nBalance + nDiffInst;
                        scheduleModel.scheduleList[scheduleModel.scheduleList.Where(x => x.IsChecked == true).Count() - 1].PrincipalInstall = Math.Round(nInstallment - nDiffInst, 2);
                        scheduleModel.scheduleList[scheduleModel.scheduleList.Where(x => x.IsChecked == true).Count() - 1].Balance = nBalance;
                    }
                    else if (nDiffInst < 0)
                    {
                        nBalance = nBalance - nDiffInst;
                        scheduleModel.scheduleList[scheduleModel.scheduleList.Where(x => x.IsChecked == true).Count() - 1].PrincipalInstall = Math.Round(nInstallment + nDiffInst);
                        scheduleModel.scheduleList[scheduleModel.scheduleList.Where(x => x.IsChecked == true).Count() - 1].Balance = nBalance;
                    }
                }

                //nLastIndex = this.DataGridView1.CurrentCell.RowIndex + 1;
                scheduleModel.Remprecent = scheduleModel.Remprecent - scheduleModel.Percent;
                scheduleModel.Percent = scheduleModel.Remprecent;
                if (scheduleModel.scheduleList[scheduleModel.scheduleList.Count() - 1].IsChecked == true)
                {
                    if (nBalance != 0)
                    {
                        scheduleModel.scheduleList[scheduleModel.scheduleList.Count() - 1].PrincipalInstall = scheduleModel.scheduleList[scheduleModel.scheduleList.Count() - 1].PrincipalInstall + nBalance;
                        scheduleModel.scheduleList[scheduleModel.scheduleList.Count() - 1].Balance = scheduleModel.scheduleList[scheduleModel.scheduleList.Count() - 1].Balance - nBalance;
                    }

                    scheduleModel.scheduleList = GetCustomizeSch(scheduleModel.scheduleList, scheduleModel.Amount, scheduleModel.Rate, scheduleModel.IsFlat);

                }
                scheduleModel.RemainingBalance = nBalance;
                return scheduleModel;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }


        }

        public LoanRebateModel GetTotalAmount(int? accountId)
        {
            var transactionDate = commonService.GetBranchTransactionDate();
            var sql= "select * from [fin].[LoanRebateData](" + accountId + ",'" + transactionDate + "')";
            var getTotalAmount = uow.Repository<LoanRebateModel>().SqlQuery(@"select * from [fin].[LoanRebateData]("+ accountId + ",'"+ transactionDate + "')").SingleOrDefault();
            return getTotalAmount;
        }

        public ReturnBaseMessageModel InsertLoanRebate(LoanRebateModel loanRebate, TaskVerificationList TaskVerifierList)
        {
            using (var transaction = uow.GetContext().Database.BeginTransaction())
            {
                try


                {
                    #region ForRebate
                    var pid = uow.Repository<ADetail>().FindBy(x => x.IAccno == loanRebate.IAccno).Select(x=>x.PID).FirstOrDefault();
                     Int64 transactionNumber = commonService.GetUtno();
                    ASTrn asTransaction = new ASTrn();
                    asTransaction.IAccno = loanRebate.IAccno;
                    asTransaction.notes = "Rebate";
                    asTransaction.slpno = 0;
                  
                    asTransaction.tdate = commonService.GetBranchTransactionDate();

                    ASTransLoan objAstransLoanRowSingle = new ASTransLoan();

                    if (loanRebate.RebateInterestOnOther != 0 && loanRebate.RebateInterestOnOther != null)
                    {
                       
                        objAstransLoanRowSingle.tno = transactionNumber;
                        asTransaction.dramt = Convert.ToDecimal(loanRebate.RebateInterestOnOther);
                        objAstransLoanRowSingle.PId = 2;
                        objAstransLoanRowSingle.Amt = Convert.ToDecimal(loanRebate.InterestOnOther);
                        uow.Repository<ASTransLoan>().Add(objAstransLoanRowSingle);
                        
                    }
                    else if (loanRebate.RebateIntrestOnInterest != 0 && loanRebate.RebateIntrestOnInterest != null)
                    {
                       
                        objAstransLoanRowSingle.tno = transactionNumber;
                        asTransaction.dramt = Convert.ToDecimal(loanRebate.RebateIntrestOnInterest);
                        objAstransLoanRowSingle.PId = 1;
                        objAstransLoanRowSingle.Amt = Convert.ToDecimal(loanRebate.RebateIntrestOnInterest);
                        uow.Repository<ASTransLoan>().Add(objAstransLoanRowSingle);

                    }
                    else if (loanRebate.RebatePenaltyOnPrincipal != 0 && loanRebate.RebatePenaltyOnPrincipal != null)
                    {
                        //voucher2TDebit.DebitAmount = Convert.ToDecimal(loanRebate.RebatePenaltyOnPrincipal); //for entry in voucher2t
                        objAstransLoanRowSingle.tno = transactionNumber;
                        asTransaction.dramt = Convert.ToDecimal(loanRebate.RebatePenaltyOnPrincipal);

                        objAstransLoanRowSingle.PId = 3;
                        objAstransLoanRowSingle.Amt = Convert.ToDecimal(loanRebate.RebatePenaltyOnPrincipal);
                        uow.Repository<ASTransLoan>().Add(objAstransLoanRowSingle);

                    }
                    else if (loanRebate.RebatePenaltyOnInterest != 0 && loanRebate.RebatePenaltyOnInterest != null)
                    {
                        //voucher2TDebit.DebitAmount = Convert.ToDecimal(loanRebate.RebatePenaltyOnInterest); //for entry in voucher2t
                        objAstransLoanRowSingle.tno = transactionNumber;
                        asTransaction.dramt = Convert.ToDecimal(loanRebate.RebatePenaltyOnInterest);

                        objAstransLoanRowSingle.PId = 4;
                        objAstransLoanRowSingle.Amt = Convert.ToDecimal(loanRebate.RebatePenaltyOnInterest);
                        uow.Repository<ASTransLoan>().Add(objAstransLoanRowSingle);
                    }
                    else
                    {
                        //voucher2TDebit.DebitAmount = Convert.ToDecimal(loanRebate.RebateInterestOnPrincipal);//for entry in voucher2t
                        objAstransLoanRowSingle.tno = transactionNumber;
                        asTransaction.dramt = Convert.ToDecimal(loanRebate.RebateInterestOnPrincipal);

                        objAstransLoanRowSingle.PId = 0;
                        objAstransLoanRowSingle.Amt = Convert.ToDecimal(loanRebate.RebateInterestOnPrincipal);
                        uow.Repository<ASTransLoan>().Add(objAstransLoanRowSingle);
                    }
                    asTransaction.ttype = 5;
                    asTransaction.postedby = Global.UserId;
                    asTransaction.IsSlp = false;
                    asTransaction.brnhno = Global.BranchId;
                    asTransaction.tno = transactionNumber;
                    asTransaction.PostedOn = DateTime.Now;
                    uow.Repository<ASTrn>().Add(asTransaction);
                   
                    uow.Commit();
                    #endregion

                    #region For Principal Out,Other Balance
                    ReferenceTnoLink referenceTnoLink = new ReferenceTnoLink();
                    ASTrn asTransactionForPrincipalAndOther = new ASTrn();
                    if (loanRebate.NewPrincpalOut != 0 && loanRebate.NewPrincpalOut != null)
                    {



                        Int64 OtherPrincipalTno = commonService.GetUtno();
                       
                       
                        asTransactionForPrincipalAndOther.IAccno = loanRebate.IAccno;
                        asTransactionForPrincipalAndOther.notes = "PrincipalOut";
                        asTransactionForPrincipalAndOther.slpno = 0;
                        asTransactionForPrincipalAndOther.tdate = commonService.GetBranchTransactionDate();
                      


                      
                            asTransactionForPrincipalAndOther.cramt = Convert.ToDecimal(loanRebate.NewPrincpalOut);
                        
                        asTransactionForPrincipalAndOther.ttype = 5;
                        asTransactionForPrincipalAndOther.postedby = Global.UserId;
                        asTransactionForPrincipalAndOther.IsSlp = false;
                        asTransactionForPrincipalAndOther.brnhno = Global.BranchId;
                        asTransactionForPrincipalAndOther.tno = OtherPrincipalTno;
                        asTransactionForPrincipalAndOther.PostedOn = DateTime.Now;

                        uow.Repository<ASTrn>().Add(asTransactionForPrincipalAndOther);                   
                            var fid = uow.Repository<ProductVfin>().FindBy(x => x.PID == pid && x.F1id == 126).Select(x => x.FID).FirstOrDefault();
                            ASTransLoan objAstransLoanPrn = new ASTransLoan();
                            objAstransLoanPrn.tno = OtherPrincipalTno;
                            objAstransLoanPrn.PId = 13;
                            objAstransLoanPrn.Amt = Convert.ToDecimal(loanRebate.NewPrincpalOut);                         
                            uow.Repository<ASTransLoan>().Add(objAstransLoanPrn);
                        referenceTnoLink.Amount = Convert.ToDecimal(loanRebate.NewPrincpalOut);
                        referenceTnoLink.Fid = fid;//for now
                            referenceTnoLink.ReferenceTno = transactionNumber;
                        referenceTnoLink.tno = OtherPrincipalTno;
                        referenceTnoLink.IAccno = loanRebate.IAccno;
                        uow.Repository<ReferenceTnoLink>().Add(referenceTnoLink);
                        uow.Commit();
                    }
                    #endregion
                    #region newotherbalance


                    if (loanRebate.NewOtherBalance != 0 && loanRebate.NewOtherBalance != null)
                    {



                        Int64 OtherBalanceTno = commonService.GetUtno();
                      
                        asTransactionForPrincipalAndOther.IAccno = loanRebate.IAccno;
                        asTransactionForPrincipalAndOther.notes = "Other Balance";
                        asTransactionForPrincipalAndOther.slpno = 0;
                        asTransactionForPrincipalAndOther.tdate = commonService.GetBranchTransactionDate();




                        asTransactionForPrincipalAndOther.cramt = Convert.ToDecimal(loanRebate.NewPrincpalOut);
                       

                        asTransactionForPrincipalAndOther.ttype = 5;
                        asTransactionForPrincipalAndOther.postedby = Global.UserId;
                        asTransactionForPrincipalAndOther.IsSlp = false;
                        asTransactionForPrincipalAndOther.brnhno = Global.BranchId;
                        asTransactionForPrincipalAndOther.tno = OtherBalanceTno;
                        asTransactionForPrincipalAndOther.PostedOn = DateTime.Now;

                        uow.Repository<ASTrn>().Add(asTransactionForPrincipalAndOther);


                      //for other in astransloan

                            var fid = uow.Repository<ProductVfin>().FindBy(x => x.PID == pid && x.F1id == 249).Select(x => x.FID).FirstOrDefault();

                            ASTransLoan objAstransLoanPrnOther = new ASTransLoan();
                            objAstransLoanPrnOther.tno = OtherBalanceTno;
                            objAstransLoanPrnOther.PId = 14;
                            objAstransLoanPrnOther.Amt = Convert.ToDecimal(loanRebate.NewOtherBalance);
                        
                            uow.Repository<ASTransLoan>().Add(objAstransLoanPrnOther);
                            referenceTnoLink.Fid = fid;
                        referenceTnoLink.tno = OtherBalanceTno;
                        referenceTnoLink.Amount = Convert.ToDecimal(loanRebate.NewOtherBalance);
                        referenceTnoLink.ReferenceTno = transactionNumber;
                        referenceTnoLink.IAccno = loanRebate.IAccno;
                        uow.Repository<ReferenceTnoLink>().Add(referenceTnoLink);
                        uow.Commit();
                    }
                  
               

                  
                    
                    #endregion

                    #region AlinkLoan

                   
                    if (loanRebate.NewLinkBalance != 0 && loanRebate.NewLinkBalance != null)
                    {
                        Int64 OtherLinkAccountTno = commonService.GetUtno();
                        var linkPid = uow.Repository<ADetail>().FindBy(x => x.IAccno == loanRebate.OldLinkAccount).Select(x => x.PID).FirstOrDefault();//link account number

                        var fid = uow.Repository<ProductVfin>().FindBy(x => x.PID == linkPid && x.F1id == 126).Select(x => x.FID).FirstOrDefault();
                        ASTrn asTransactionAlinkLoan = new ASTrn();
                        asTransactionAlinkLoan.IAccno = loanRebate.IAccno;
                        asTransactionAlinkLoan.notes = "ALinkLoan";
                        asTransactionAlinkLoan.slpno = 0;
                        asTransactionAlinkLoan.tdate = commonService.GetBranchTransactionDate();
                        asTransactionAlinkLoan.cramt = Convert.ToDecimal(loanRebate.NewLinkBalance);
                       
                        asTransactionAlinkLoan.ttype = 5;
                        asTransactionAlinkLoan.postedby = Global.UserId;
                        asTransactionAlinkLoan.IsSlp = false;
                        asTransactionAlinkLoan.brnhno = Global.BranchId;
                        asTransactionAlinkLoan.tno = OtherLinkAccountTno;
                        asTransactionAlinkLoan.PostedOn = DateTime.Now;
                      
                        commonService.InsertAvailableBalance(1, Convert.ToInt32(loanRebate.IAccno), Convert.ToDecimal(loanRebate.NewLinkBalance));
                        uow.Repository<ASTrn>().Add(asTransactionAlinkLoan);
                        referenceTnoLink.ReferenceTno = transactionNumber;
                        referenceTnoLink.Fid = fid;
                        referenceTnoLink.IAccno = loanRebate.OldLinkAccount;
                        referenceTnoLink.tno = OtherLinkAccountTno;
                        uow.Repository<ReferenceTnoLink>().Add(referenceTnoLink);
                        uow.Commit();

                    }
                    #endregion

                   
                    transaction.Commit();
                    taskUow.SaveTaskNotification(TaskVerifierList, transactionNumber, 32);
  
                    returnMessage.Msg = "Rebate Added Successfully";
                    returnMessage.Success = true;
                    return returnMessage;
                }

                catch (Exception ex)
                {

                    throw ex;
                }
            }
      
         
        }

        private List<ScheduleTrialModel> GetCustomizeSch(List<ScheduleTrialModel> DTSch, decimal nPrincipal, decimal nRate, bool IsFlat)
        {
            int n;
            decimal nBalance = nPrincipal;
            decimal nInterest;
            for (n = 0; n <= DTSch.Count - 1; n++)
            {
                if (IsFlat == true)
                {
                    nInterest = (nPrincipal * DTSch[n].Day * nRate) / 36500;
                }
                else
                {
                    nInterest = (nBalance * DTSch[n].Day * nRate) / 36500;
                }

                DTSch[n].InterestInstall = nInterest;
                nBalance = nBalance - DTSch[n].PrincipalInstall;
                DTSch[n].TotalInstall = DTSch[n].PrincipalInstall + nInterest;
            }
            return DTSch;
        }

        public List<ScheduleTrialModel> GetCustmizeInstellment(ScheduleTrialModel scheduleModel)
        {
            try
            {
                List<ScheduleTrialModel> newSchedule = new List<ScheduleTrialModel>();
                var scheduleList = scheduleModel.scheduleList.Where(x => x.HasInterest == true).ToList();
                if (scheduleModel.scheduleList[scheduleModel.scheduleList.Count() - 1].HasPrincipal == false || scheduleModel.scheduleList[scheduleModel.scheduleList.Count() - 1].HasInterest == false)
                {
                    throw new Exception("Please select last installment!!");
                }
                DateTime startDate = scheduleModel.StartDate;

                for (int i = 0; i <= scheduleList.Count() - 1; i++)
                {
                    if (i == 0)
                    {
                        scheduleList[i].Day = (scheduleList[i].DateAd - startDate).Days + 1;
                    }
                    else
                    {
                        scheduleList[i].Day = (scheduleList[i].DateAd - scheduleList[i - 1].DateAd).Days;
                    }
                    startDate = scheduleList[i].DateAd;
                }

                return scheduleList;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        private List<ScheduleTrialModel> FEqInstDaily(List<ScheduleTrialModel> scheduleTrial, DateTime dStartDate, decimal nPrincipal, decimal nRate, bool IsFlat)
        {

            try
            {
                decimal nDisburseAmount = nPrincipal;
                decimal nBalance = 0;

                decimal nAsAmount = 0;

                decimal nIntInst = 0;
                decimal nPrincInst = 0;


                int nRowCount = default(Int16);
                Int16 lCounter = default(Int16);

                int nPInstCount = 0;
                nPInstCount = scheduleTrial.Where(x => x.HasPrincipal == true).Count();
                nAsAmount = nPrincipal / nPInstCount;

                nRowCount = scheduleTrial.Count() - 1;

                decimal nLastPrinBal = 1;

                int nLoop = 0;

                while (nLastPrinBal != 0)
                {
                    var mcalSchedule = scheduleTrial.FirstOrDefault();
                    nBalance = nDisburseAmount;

                    nPrincipal = nDisburseAmount;

                    nIntInst = ((nBalance * nRate * mcalSchedule.Day) / 36500);

                    if (mcalSchedule.HasPrincipal == false)
                    {
                        nPrincInst = 0;
                    }
                    else
                    {

                        nPrincInst = nAsAmount - nIntInst;

                    }

                    nPrincipal = nPrincipal - nPrincInst;
                    scheduleTrial[0].PrincipalInstall = nPrincInst;
                    scheduleTrial[0].InterestInstall = nIntInst;
                    scheduleTrial[0].TotalInstall = nPrincInst + nIntInst;
                    scheduleTrial[0].Balance = nBalance - nPrincInst;

                    nBalance = nBalance - nPrincInst;

                    for (lCounter = 0; lCounter <= nRowCount - 1; lCounter++)
                    {



                        if (IsFlat == true)
                        {
                            nIntInst = ((nDisburseAmount * nRate * scheduleTrial[lCounter + 1].Day) / 36500);
                            //Other
                        }
                        else
                        {
                            nIntInst = (nBalance * nRate * scheduleTrial[lCounter + 1].Day) / 36500;
                        }

                        if (scheduleTrial[lCounter + 1].HasPrincipal == false)
                        {
                            nPrincInst = 0;
                        }
                        else
                        {
                            nPrincInst = nAsAmount - nIntInst;
                        }

                        nPrincipal = nPrincipal - nPrincInst;
                        scheduleTrial[lCounter + 1].PrincipalInstall = nPrincInst;
                        scheduleTrial[lCounter + 1].InterestInstall = nIntInst;
                        scheduleTrial[lCounter + 1].TotalInstall = nPrincInst + nIntInst;
                        scheduleTrial[lCounter + 1].Balance = nBalance - nPrincInst;
                        nBalance = nBalance - nPrincInst;

                    }

                    if (nLastPrinBal == scheduleTrial.Select(x => x.Balance).LastOrDefault())
                    {
                        nLastPrinBal = 0;
                    }
                    else
                    {
                        nLastPrinBal = scheduleTrial.Select(x => x.Balance).LastOrDefault();
                    }


                    if (nPrincipal > 0)
                    {
                        if (nRowCount == 1)
                        {
                            nAsAmount = nAsAmount + (nPrincipal / Convert.ToDecimal(1.4));
                        }
                        else
                        {
                            nAsAmount = nAsAmount + nPrincipal / (nRowCount + 1);
                        }
                    }
                    else
                    {
                        if (nRowCount == 1)
                        {
                            nAsAmount = (nAsAmount - (Math.Abs(nPrincipal) / Convert.ToDecimal(1.4)));
                        }
                        else
                        {
                            nAsAmount = (nAsAmount - Math.Abs(nPrincipal) / (nRowCount + 1));
                        }
                    }

                    nLoop = nLoop + 1;
                    if (nLoop > 100)
                    {
                        break; // TODO: might not be correct. Was : Exit While
                    }
                }
                //Adjusting on last installment
                if (scheduleTrial[lCounter].Balance != 0)
                {
                    decimal deviation = 0;
                    deviation = 0 - scheduleTrial[lCounter].Balance;
                    lCounter = Convert.ToInt16(scheduleTrial.Count() - 1);
                    scheduleTrial[lCounter].TotalInstall = scheduleTrial[lCounter].TotalInstall - deviation;
                    scheduleTrial[lCounter].PrincipalInstall = scheduleTrial[lCounter].PrincipalInstall - deviation;
                    scheduleTrial[lCounter].Balance = scheduleTrial[lCounter].Balance;
                }
                if (scheduleTrial.Where(x => x.TotalInstall < 0 || x.PrincipalInstall < 0).Count() != 0)
                {
                    throw new Exception("Principal amount can't be negative value.Please try other amount.");
                }
                return scheduleTrial;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }

        public ReturnBaseMessageModel VerifyLoanRegistration(long eventValue, int? GrantedDuration = null, decimal? SAmt = null)
        {
            //try
            //{
            //    var remitRow = uow.Repository<RemitPayment>().GetSingle(x => x.Tno == tno);
            //    remitRow.ApproveBy = Global.UserId;
            //    remitRow.ApproveOn = commonService.GetBranchTransactionDate();
            //    uow.Repository<RemitPayment>().Edit(remitRow);
            //    uow.Commit();
            //    returnMessage.Success = true;
            //    returnMessage.Msg = "Remit payment Verify successfully";
            //    return returnMessage;
            //}
            //catch (Exception ex)
            //{
            //    returnMessage.Success = false;
            //    returnMessage.Msg = "Remit payment not Verify.!!" + ex.Message;
            //    return returnMessage;
            //}

            try
            {
                var registration = uow.Repository<ALRegistration>().GetSingle(x => x.RegId == eventValue);
                registration.ApprovedOn = commonService.GetDate(); ;
                registration.ApprovedBy = Loader.Models.Global.UserId;

                registration.Status = 2;
                registration.SAmt = SAmt;
                registration.ADuration = GrantedDuration;
                registration.Remarks = "Verified";
                uow.Repository<ALRegistration>().Edit(registration);
                //int  taskid = uow.Repository<ChannakyaBase.DAL.DatabaseModel.Task>().FindBy(x => x.EventId == 17 && x.EventValue == eventValue).Select(c => c.Task1Id).FirstOrDefault();
                // taskUow.VerifiedOn(taskid);
                uow.Commit();
                returnMessage.Success = true;
                returnMessage.Msg = "Loan Registered Verified successfully";
                return returnMessage;
            }
            catch (Exception ex)
            {

                returnMessage.Success = false;
                returnMessage.Msg = "Remit payment not Verify.!!" + ex.Message;
                return returnMessage;
            }



        }




        private List<ScheduleTrialModel> FEqPrin(List<ScheduleTrialModel> scheduleTrial, decimal nPrincipal, decimal nRate, bool IsFlat)
        {
            int nPInst = scheduleTrial.Where(x => x.HasPrincipal == true).Count();
            decimal nPInstAmt = 0;
            decimal nBalAmount = nPrincipal;
            Int16 n = default(Int16);

            nPInstAmt = nPrincipal / nPInst;
            int countInt = scheduleTrial.Count() - 1;

            for (n = 0; n <= scheduleTrial.Count() - 1; n++)
            {
                if (scheduleTrial[n].HasPrincipal == true)
                {
                    scheduleTrial[n].PrincipalInstall = nPInstAmt;
                }

            }
            for (n = 0; n <= scheduleTrial.Count - 1; n++)
            {
                if (scheduleTrial[n].HasInterest == true)
                {
                    if (IsFlat == true)
                    {
                        scheduleTrial[n].InterestInstall = ((nPrincipal * scheduleTrial[n].Day * nRate) / 36500);
                    }
                    else
                    {
                        scheduleTrial[n].InterestInstall = ((nBalAmount * scheduleTrial[n].Day * nRate) / 36500);
                    }
                }

                nBalAmount = nBalAmount - scheduleTrial[n].PrincipalInstall;
                scheduleTrial[n].Balance = nBalAmount;
                scheduleTrial[n].TotalInstall = scheduleTrial[n].PrincipalInstall + scheduleTrial[n].InterestInstall;
            }

            scheduleTrial[n - 1].PrincipalInstall = nPInstAmt + scheduleTrial[n - 1].Balance;
            scheduleTrial[n - 1].TotalInstall = scheduleTrial[n - 1].InterestInstall + scheduleTrial[n - 1].PrincipalInstall;
            scheduleTrial[n - 1].Balance = 0;

            return scheduleTrial;
        }

        public List<LoanRegAccOpenViewModel> SingleLoanRegVerifiedCustomer(string customerName = null)
        {
            var query = "Select * from Fin.FGetLoanRegisteredVerifiedList() where Aname like '%" + customerName + "%'";
            List<LoanRegAccOpenViewModel> loanRegAccOpenViewModel = uow.Repository<LoanRegAccOpenViewModel>().SqlQuery(query).ToList();

            //LoanRegAccOpenViewModel loanRegAccOpenViewModel = uow.Repository<LoanRegAccOpenViewModel>().SqlQuery(query).FirstOrDefault();
            return loanRegAccOpenViewModel;
        }

        private List<ScheduleTrialModel> GetScheduleFormatTB(EScheduleType scheduleType, DateTime dStartDate, DateTime dMatureDate, int nPrincipalFrequency, int nInterestFrequency, DateTime dPrinGraceDate, DateTime dIntGraceDate, int nProvisionDays, int nCustomDay)
        {
            List<ScheduleTrialModel> scheduleList = new List<ScheduleTrialModel>();

            if ((nInterestFrequency == nPrincipalFrequency) & (dPrinGraceDate == dIntGraceDate))
            {
                scheduleList = this.DistributeDate(scheduleType, dStartDate, dMatureDate, dPrinGraceDate, nProvisionDays, nPrincipalFrequency, nCustomDay, DisbOf.Both);


            }
            else
            {

                List<ScheduleTrialModel> schedulePrincipalList = new List<ScheduleTrialModel>();
                scheduleList = this.DistributeDate(scheduleType, dStartDate, dMatureDate, dIntGraceDate, nProvisionDays, nInterestFrequency, nCustomDay, DisbOf.Interest);
                schedulePrincipalList = DistributeDate(scheduleType, dStartDate, dMatureDate, dPrinGraceDate, nProvisionDays, nPrincipalFrequency, nCustomDay, DisbOf.Principal);

                for (int counter = 0; counter < schedulePrincipalList.Count; counter++)
                {
                    int index = scheduleList.FindIndex(x => x.DateAd == schedulePrincipalList[counter].DateAd);
                    if (index != -1)
                    {
                        scheduleList[index].HasPrincipal = schedulePrincipalList[counter].HasPrincipal;
                    }
                    // int Index = (nPrincipalFrequency / nInterestFrequency) * counter;

                }

            }
            return scheduleList;
        }

        private List<ScheduleTrialModel> DistributeDate(EScheduleType scheduleType, DateTime dStartDate, DateTime MatureDate, DateTime GraceDate, int ProvisionDays, int Frequency, int CustomDay, DisbOf PIF)
        {
            try
            {
                bool hasInterest = false;
                bool hasPrincipal = false;
                List<ScheduleTrialModel> scheduleList = new List<ScheduleTrialModel>();
                if (PIF == DisbOf.Both)
                {
                    hasInterest = true;
                    hasPrincipal = true;
                }
                else if (PIF == DisbOf.Interest)
                {
                    hasInterest = true;
                }
                else
                {
                    hasPrincipal = true;
                }
                DateTime dInstDate = dStartDate;

                if (scheduleType == EScheduleType.EndOfEnglishMonth | scheduleType == EScheduleType.EndOfNepaliMonth)
                {
                    if (Frequency > 1)
                    {
                        dInstDate = new System.DateTime(dInstDate.Year, dInstDate.Month, System.DateTime.DaysInMonth(dInstDate.Year, dInstDate.Month));
                    }
                }

                dInstDate = GetNextInstallmentDate(dInstDate, scheduleType, Frequency, CustomDay);
                if (dInstDate > MatureDate)
                {
                    ScheduleTrialModel schedule = new ScheduleTrialModel();
                    schedule.DateAd = MatureDate;
                    schedule.NepaliDate = datePickerService.GetDateBS(MatureDate);
                    schedule.HasInterest = hasInterest;
                    schedule.HasPrincipal = hasPrincipal;
                    scheduleList.Add(schedule);
                    return scheduleList;
                }
                else
                {

                    if (GraceDate > dInstDate)
                    {
                        dInstDate = GetNextInstallmentDate(dInstDate, scheduleType, Frequency, CustomDay);
                    }

                    while (dStartDate.AddDays(ProvisionDays) > dInstDate)
                    {
                        dInstDate = GetNextInstallmentDate(dInstDate, scheduleType, Frequency, CustomDay);
                    }
                    ScheduleTrialModel schedule = new ScheduleTrialModel();
                    schedule.DateAd = dInstDate;
                    schedule.NepaliDate = datePickerService.GetDateBS(dInstDate);
                    schedule.HasInterest = hasInterest;
                    schedule.HasPrincipal = hasPrincipal;
                    scheduleList.Add(schedule);
                    while (dInstDate < MatureDate)
                    {
                        dInstDate = GetNextInstallmentDate(dInstDate, scheduleType, Frequency, CustomDay);
                        if (dInstDate > MatureDate)
                        {
                            //    break;
                        }
                        else
                        {
                            ScheduleTrialModel scheduleTrial = new ScheduleTrialModel();
                            scheduleTrial.DateAd = dInstDate;
                            scheduleTrial.NepaliDate = datePickerService.GetDateBS(dInstDate);
                            scheduleTrial.HasInterest = hasInterest;
                            scheduleTrial.HasPrincipal = hasPrincipal;
                            scheduleList.Add(scheduleTrial);
                        }
                    }

                    System.DateTime DLastInstDate = default(System.DateTime);

                    if (scheduleList.Count > 0)
                    {
                        DLastInstDate = scheduleList.Select(x => x.DateAd).LastOrDefault();
                    }
                    else
                    {
                        DLastInstDate = dStartDate;
                    }


                    if (scheduleList.Select(x => x.DateAd).LastOrDefault() < MatureDate)
                    {
                    }
                    if (scheduleList.Select(x => x.DateAd).LastOrDefault() < MatureDate)
                    {
                        if (scheduleType == EScheduleType.EndOfEnglishMonth | scheduleType == EScheduleType.EndOfNepaliMonth)
                        {
                            if ((MatureDate - DLastInstDate).Days > 15)
                            {
                                ScheduleTrialModel lastSchedulegoto = new ScheduleTrialModel();
                                lastSchedulegoto.DateAd = MatureDate;
                                lastSchedulegoto.NepaliDate = datePickerService.GetDateBS(MatureDate);
                                lastSchedulegoto.HasInterest = hasInterest;
                                lastSchedulegoto.HasPrincipal = hasPrincipal;
                                scheduleList.Add(lastSchedulegoto);
                            }
                            else
                            {
                                var removeRow = scheduleList.LastOrDefault();
                                var lastScheduleList = new ScheduleTrialModel();
                                lastScheduleList.DateAd = MatureDate;
                                lastScheduleList.NepaliDate = datePickerService.GetDateBS(MatureDate);
                                lastScheduleList.HasInterest = hasInterest;
                                lastScheduleList.HasPrincipal = hasPrincipal;
                                scheduleList[scheduleList.FindIndex(x => x.DateAd == removeRow.DateAd)] = lastScheduleList;

                            }
                        }
                        else if (scheduleType == EScheduleType.EndOfWeekEnd)
                        {
                            if ((MatureDate - DLastInstDate).Days > 4)
                            {
                                ScheduleTrialModel lastSchedulegoto = new ScheduleTrialModel();
                                lastSchedulegoto.DateAd = MatureDate;
                                lastSchedulegoto.NepaliDate = datePickerService.GetDateBS(MatureDate);
                                lastSchedulegoto.HasInterest = hasInterest;
                                lastSchedulegoto.HasPrincipal = hasPrincipal;
                                scheduleList.Add(lastSchedulegoto);
                            }
                            else
                            {
                                var removeRow = scheduleList.LastOrDefault();
                                var lastScheduleList = new ScheduleTrialModel();
                                lastScheduleList.DateAd = MatureDate;
                                lastScheduleList.NepaliDate = datePickerService.GetDateBS(MatureDate);
                                lastScheduleList.HasInterest = hasInterest;
                                lastScheduleList.HasPrincipal = hasPrincipal;
                                scheduleList[scheduleList.FindIndex(x => x.DateAd == removeRow.DateAd)] = lastScheduleList;

                            }
                        }
                        else
                        {
                            ScheduleTrialModel lastSchedulegoto = new ScheduleTrialModel();
                            lastSchedulegoto.DateAd = MatureDate;
                            lastSchedulegoto.NepaliDate = datePickerService.GetDateBS(MatureDate);
                            lastSchedulegoto.HasInterest = hasInterest;
                            lastSchedulegoto.HasPrincipal = hasPrincipal;
                            scheduleList.Add(lastSchedulegoto);
                        }


                    }
                    else
                    {
                        var removeRow = scheduleList.LastOrDefault();
                        var lastScheduleList = new ScheduleTrialModel();
                        lastScheduleList.DateAd = MatureDate;
                        lastScheduleList.NepaliDate = datePickerService.GetDateBS(MatureDate);
                        lastScheduleList.HasInterest = hasInterest;
                        lastScheduleList.HasPrincipal = hasPrincipal;
                        scheduleList[scheduleList.FindIndex(x => x.DateAd == removeRow.DateAd)] = lastScheduleList;
                    }
                    return scheduleList;
                }
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private List<ScheduleTrialModel> InsertDays(DateTime dStartDate, List<ScheduleTrialModel> scheduleTrial, EScheduleType scheduleType)
        {
            //if (scheduleType != EScheduleType.Daily)
            //{
            //    dStartDate = dStartDate.AddDays(-1);
            //}

            List<ScheduleTrialModel> scheduleTrialDays = new List<ScheduleTrialModel>();

            foreach (var item in scheduleTrial)
            {
                ScheduleTrialModel schedule = new ScheduleTrialModel();
                schedule = item;
                schedule.Day = (item.DateAd - dStartDate).Days;
                dStartDate = item.DateAd;
                scheduleTrialDays.Add(schedule);
            }
            return scheduleTrialDays;
        }

        private short FGetSchProvision()
        {
            var functionReturnValue = uow.Repository<string>().SqlQuery("Select cast(Isnull(schprovdys,0)as varchar(10)) from fin.LicenseBranch where BrnhID=" + commonService.GetBranchIdByEmployeeUserId()).FirstOrDefault();
            return Convert.ToInt16(functionReturnValue);
        }
        private EScheduleType GetEScheduleTypeByVlue(int value)
        {
            EScheduleType valueName = (EScheduleType)Enum.Parse(typeof(EScheduleType), value.ToString());
            return valueName;
        }


        #endregion

        #region Loan Payment

        public LoanPaymentModel GetLoanPayment(int accountId)
        {
            var loanPayment = uow.Repository<ALoan>().FindByMany(x => x.IAccno == accountId).Select(x => new LoanPaymentModel()
            {
                OthrBal = x.OthrBal == null ? 0 : x.OthrBal,
                IonPA = x.IonPA == null ? 0 : x.IonPA,
                IonPR = x.IonPR == null ? 0 : x.IonPR,
                IonCA = x.IonCA == null ? 0 : x.IonCA,
                IonCR = x.IonCR == null ? 0 : x.IonCR,
                PonPrA = x.PonPrA == null ? 0 : x.PonPrA,
                PonPrR = x.PonPrR == null ? 0 : x.PonPrR,
                PonIA = x.PonIA == null ? 0 : x.PonIA,
                PonIR = x.PonIR == null ? 0 : x.PonIR,
                IonIR = x.IonIR == null ? 0 : x.IonIR,
                IonIA = x.IonIA == null ? 0 : x.IonIA
            });
            return loanPayment.FirstOrDefault();
        }
        public LoanPaymentModel GetAccountLoanPaymentDetails(int accountId)
        {

            var loanPayment = GetLoanPayment(accountId);

            bool isMatureShow = commonService.IsShowMatureInterestOnly();

            if (isMatureShow == true)
            {
                decimal mature = GetTillDateMatureInterest(accountId);
                loanPayment.IonPA = mature;
            }
            var matureAndCurrentPa = CurrentMaturePrinciapAndInterest(accountId);
            loanPayment.MaturePA = Math.Round(((decimal)matureAndCurrentPa.CurrentPrincipal + (decimal)matureAndCurrentPa.MaturePrincipal), 2);
            decimal total = Convert.ToDecimal(loanPayment.IonCA + loanPayment.IonCR +
                  loanPayment.IonIA + loanPayment.IonIR +
                  loanPayment.IonPA + loanPayment.IonPR +
                  loanPayment.OthrBal + loanPayment.PonIA + loanPayment.PonIR + loanPayment.PonPrA + loanPayment.PonPrR);


            loanPayment.TotalBalance = (decimal)matureAndCurrentPa.MaturePrincipal + total + (decimal)matureAndCurrentPa.CurrentPrincipal;
            return loanPayment;
        }


        public decimal GetTillDateMatureInterest(int accountId)
        {
            var getTillDatePaymentAmount = GetSchDetailOnPymnt(accountId);
            decimal matureInterest = getTillDatePaymentAmount[1].Mature;
            return matureInterest;

        }

        public FDLoanViewModel GetFdLoanDetails(int accountId, int Pid,string Modelfrom, int? FDIaccno)
        {
            bool? isFDLoan= false;
            FDLoanViewModel fdLoanModel = new FDLoanViewModel();
            var fddetailSDID = uow.Repository<ProductDetail>().FindBy(x => x.PID == Pid).Select(x => x.SDID).SingleOrDefault();//top account number 
            if (Modelfrom== "fromfd")
            {

                
                isFDLoan = uow.Repository<SchmDetail>().FindBy(x => x.SDID == fddetailSDID).Select(x => x.IsFDLoan).SingleOrDefault();
            }
            else
            {
                var adetail = uow.Repository<ADetail>().FindBy(x => x.IAccno == accountId).Select(x => x.PID).SingleOrDefault();
                var SDID = uow.Repository<ProductDetail>().FindBy(x => x.PID == adetail).Select(x => x.SDID).SingleOrDefault();
                if(accountId != FDIaccno)
                {
                    isFDLoan = uow.Repository<SchmDetail>().FindBy(x => x.SDID == SDID).Select(x => x.IsFDLoan).SingleOrDefault();
                }
                else
                {
                    isFDLoan = uow.Repository<SchmDetail>().FindBy(x => x.SDID == fddetailSDID).Select(x => x.IsFDLoan).SingleOrDefault();
                }
               
            }

            if (isFDLoan == true)
            {
                var getAccountDetails = uow.Repository<ADetail>().FindByMany(x => x.IAccno == accountId).Select(x => new AccountDetailsViewModel()
                {
                    Bal = x.Bal,
                    MaturationDate = x.ADur.MatDat.Value,
                    InterestRate = x.IRate,
                    PID = x.PID

                }).FirstOrDefault();

                var durState = uow.Repository<ProductDetail>().FindBy(x => x.PID == Pid).Select(x => x.durState).SingleOrDefault();

                double difference = (getAccountDetails.MaturationDate - commonService.GetBranchTransactionDate()).TotalDays;
                double differenceMonth = Math.Truncate((difference % 365) / 30);
                if (durState == 1)
                {
                    fdLoanModel.fixedDuration = Convert.ToInt32(differenceMonth);
                }
                else
                {
                    fdLoanModel.fixedDuration = Convert.ToInt32(difference);
                }
                decimal amount = TellerUtilityService.CheckForGuarantor(accountId);



                var pValue = uow.Repository<DAL.DatabaseModel.ParamValue>().FindBy(x => x.PId == 10074).Select(x => x.PValue).SingleOrDefault();
                decimal totalloanWithdrawBalance = (getAccountDetails.Bal / 100) * Convert.ToDecimal(pValue);
                fdLoanModel.Balance = totalloanWithdrawBalance - amount;
                //fdLoanModel.Balance = totalloanWithdrawBalance;
                //if (totalloanWithdrawBalance < amount)
                //{
                //    fdLoanModel.Balance = 0;
                //}
                fdLoanModel.IRate = getAccountDetails.InterestRate + 2;
                var dateTime = datePickerService.GetDateBSAndAD(getAccountDetails.MaturationDate);
                fdLoanModel.EnglishDate = dateTime.DateAD;
                fdLoanModel.NepaliDate = dateTime.DateBS;
                fdLoanModel.Date = dateTime.Date;
                fdLoanModel.IsGurantor = TellerUtilityService.CheckIsGuarantor(accountId);
                if (fdLoanModel.Balance <= 0)
                {
                    fdLoanModel.hasBalance = false;
                }
                else
                {
                    fdLoanModel.hasBalance = true;
                }

                return fdLoanModel;
            }
            return fdLoanModel;
        }

        public FDLoanViewModel GetFdLoanDetailsForLoanAccount(int accountId)
        {
            FDLoanViewModel fdLoanModel = new FDLoanViewModel();
            var getAccountDetails = uow.Repository<ADetail>().FindByMany(x => x.IAccno == accountId).Select(x => new AccountDetailsViewModel()
            {
                Bal = x.Bal,
                MaturationDate = x.ADur.MatDat.Value,
                InterestRate = x.IRate

            }).FirstOrDefault();


            decimal amount = TellerUtilityService.CheckForGuarantorForLoanee(accountId);
            var pValue = uow.Repository<DAL.DatabaseModel.ParamValue>().FindBy(x => x.PId == 10074).Select(x => x.PValue).SingleOrDefault();
            decimal totalloanWithdrawBalance = (getAccountDetails.Bal / 100) * Convert.ToDecimal(pValue);
            fdLoanModel.Balance = totalloanWithdrawBalance - amount;
            //fdLoanModel.MinValue= Math.Min(Math.Min(LAmount, quotationAmount), fdLoanModel.Balance);
            fdLoanModel.MinValue = fdLoanModel.Balance;
            //fdLoanModel.Balance = totalloanWithdrawBalance;


            return fdLoanModel;
        }

        public decimal GetTillCurrentDateMatureAmount(int accountId)
        {
            var loanPayment = GetLoanPayment(accountId);
            return Convert.ToDecimal(loanPayment.IonPA);
        }

        public LoanPaymentInformationModel LoanAccountDetailsInformation(int accountId)
        {
            TellerService tellerService = new TellerService();
            LoanPaymentInformationModel informationLoan = new LoanPaymentInformationModel();
            var AccountDetails = tellerService.GetSingleAccountDetails(accountId);
            var loanPaymentDetails = GetLoanPayment(accountId);
            var matureAmount = CurrentMaturePrinciapAndInterest(accountId);
            informationLoan.LoanPaymentList = GetSchDetailOnPymnt(accountId);
            decimal CalculateTotal = CreditUtilityService.PayCalcutation(accountId, "Other");
            informationLoan.TotalInstallMentAmount = tellerService.GetSingleAccountDetails(accountId).Bal + CreditUtilityService.PayCalcutation(accountId, "");
            informationLoan.ToPay = AccountDetails.Bal + CalculateTotal;
            informationLoan.Other = Convert.ToDecimal(loanPaymentDetails.OthrBal);
            informationLoan.MatureDate = uow.Repository<ADur>().FindBy(x => x.IAccno == accountId).Select(x => x.MatDat).FirstOrDefault();
            informationLoan.OutstatdingPrincipal = AccountDetails.Bal;
            informationLoan.Balance = (decimal)matureAmount.CurrentPrincipal + CalculateTotal + (decimal)matureAmount.MaturePrincipal;
            informationLoan.CurrentInterest = Convert.ToDecimal(loanPaymentDetails.IonPA);
            informationLoan.PerDayInterest = ((AccountDetails.IonBal * AccountDetails.IRate / 100) / 365);
            return informationLoan;

        }

        public ReturnBaseMessageModel InsertReschedule(ScheduleTrialModel scheduleTrialModel, ScheduleModel scheduleModel)
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

                    var singleAloan = uow.Repository<ALoan>().FindBy(x => x.IAccno == scheduleTrialModel.IAccno).SingleOrDefault();
                    ADur aDur = uow.Repository<ADur>().FindBy(x => x.IAccno == scheduleTrialModel.IAccno).SingleOrDefault();
                    APolicyInterest aPolicyInterest = uow.Repository<APolicyInterest>().FindBy(x => x.IAccno == scheduleTrialModel.IAccno).SingleOrDefault();
                    ALoanGrace aLoanGrace = uow.Repository<ALoanGrace>().FindBy(x => x.IAccno == scheduleTrialModel.IAccno).SingleOrDefault();
                    ADetail aDetail = uow.Repository<ADetail>().FindBy(x => x.IAccno == scheduleTrialModel.IAccno).SingleOrDefault();

                    //  Add data of alsch to ALSchHistry
                    var schedule = uow.Repository<ALSch>().FindBy(x => x.IAccno == scheduleTrialModel.IAccno).ToList();

                    foreach (var item in schedule)
                    {
                        ALSchHistry aLSchHistory = new ALSchHistry();
                        aLSchHistory.schDate = item.schDate;
                        aLSchHistory.schPrin = item.schPrin;
                        aLSchHistory.schInt = item.schInt;
                        aLSchHistory.balPrin = item.balPrin;
                        aLSchHistory.IAccno = item.IAccno;
                        uow.Repository<ALSchHistry>().Add(aLSchHistory);
                        uow.Repository<ALSch>().Delete(item);
                        uow.Commit();
                    }




                    foreach (var item in scheduleTrialModel.scheduleList)
                    {
                        ALSch aschedule = new ALSch();

                        aschedule.schDate = item.DateAd;
                        aschedule.schPrin = item.PrincipalInstall;
                        aschedule.schInt = item.InterestInstall;
                        aschedule.balPrin = item.Balance;
                        aschedule.IAccno = scheduleTrialModel.IAccno;
                        uow.Repository<ALSch>().Add(aschedule);
                        uow.Commit();
                    }


                    //Renew Case
                    if ((!scheduleTrialModel.IsReschedule) && (!scheduleTrialModel.IsRestructure))
                    {


                        HALRenew hALRenew = new HALRenew();
                        if (aPolicyInterest != null && aLoanGrace != null)
                        {
                            hALRenew.PSID = aPolicyInterest.PSID;
                            hALRenew.GDayP = aLoanGrace.GDayP;
                            hALRenew.GDayI = aLoanGrace.GDayI;
                            hALRenew.GDateP = aLoanGrace.GDateP;
                            hALRenew.GDateI = aLoanGrace.GDateI;
                        }

                        else
                        {

                            hALRenew.GDayP = 0;
                            hALRenew.GDayI = 0;
                        }
                        hALRenew.IAccNo = scheduleModel.IAccno;
                        hALRenew.Duration = Convert.ToInt32(scheduleModel.Duration);
                        hALRenew.StDate = commonService.GetBranchTransactionDate();
                        hALRenew.MDate = scheduleTrialModel.scheduleList.LastOrDefault().DateAd;
                        hALRenew.PF = scheduleModel.PrincipalFrequency;
                        hALRenew.IF = scheduleModel.InterestFrequency;
                        hALRenew.Days = scheduleModel.Day;
                        hALRenew.PMI = scheduleModel.PaymentMode;
                        hALRenew.Rate = Convert.ToInt32(scheduleModel.Rate);
                        hALRenew.RNDate = commonService.GetDate();
                        hALRenew.Flag = 1;
                        uow.Repository<HALRenew>().Add(hALRenew);
                        uow.Commit();
                        //Insert Data in Adur

                        //adurSingle.IAccno = scheduleTrialModel.IAccno;


                        aDur.Month = (int)scheduleModel.Duration;


                        uow.Repository<ADur>().Edit(aDur);


                        //Insert Data in Arate


                        if (aDetail.IRate != scheduleModel.Rate)
                        {
                            aDetail.IRate = scheduleModel.Rate;
                            uow.Repository<ADetail>().Edit(aDetail);
                        }


                        ARateMaster aRateMaster = new ARateMaster();
                        aRateMaster.EffectiveDate = commonService.GetBranchTransactionDate();
                        aRateMaster.PostedDate = commonService.GetDate();
                        aRateMaster.PostedBy = Global.UserId;
                        ARate aRate = new ARate();
                        //aRate.ADetail = aDetail;
                        aRate.IAccno = scheduleModel.IAccno;
                        aRate.IRate = Convert.ToSingle(scheduleModel.Rate);
                        aRateMaster.ARates.Add(aRate);
                        uow.Repository<ARateMaster>().Add(aRateMaster);
                        uow.Commit();


                    }
                    //For Reschedule
                    else if ((scheduleTrialModel.IsReschedule) && (!scheduleTrialModel.IsRestructure))
                    {
                        HALRenew hALRenew = new HALRenew();
                        if (aPolicyInterest != null && aLoanGrace != null && aDur != null)
                        {
                            hALRenew.PSID = aPolicyInterest.PSID;
                            hALRenew.GDayP = aLoanGrace.GDayP;
                            hALRenew.GDayI = aLoanGrace.GDayI;
                            hALRenew.GDateP = aLoanGrace.GDateP;
                            hALRenew.GDateI = aLoanGrace.GDateI;
                            hALRenew.MDate = aDur.MatDat;
                        }

                        else
                        {

                            hALRenew.GDayP = 0;
                            hALRenew.GDayI = 0;
                        }

                        hALRenew.IAccNo = scheduleModel.IAccno;
                        hALRenew.Duration = Convert.ToInt32(scheduleModel.Duration);
                        hALRenew.StDate = commonService.GetDate();
                        hALRenew.PF = scheduleModel.PrincipalFrequency;
                        hALRenew.IF = scheduleModel.InterestFrequency;
                        hALRenew.GDateP = scheduleModel.PrincipalDate;
                        hALRenew.GDateI = scheduleModel.InterestDate;
                        hALRenew.Days = scheduleModel.Day;
                        hALRenew.PMI = scheduleModel.PaymentMode;
                        hALRenew.Rate = Convert.ToInt32(scheduleModel.Rate);
                        hALRenew.RNDate = commonService.GetDate();
                        hALRenew.Flag = 2;
                        uow.Repository<HALRenew>().Add(hALRenew);

                        //Insert Data in Arate
                        if (aDetail.IRate != scheduleModel.Rate)
                        {
                            aDetail.IRate = scheduleModel.Rate;
                            uow.Repository<ADetail>().Edit(aDetail);
                        }


                        ARateMaster aRateMaster = new ARateMaster();
                        aRateMaster.EffectiveDate = commonService.GetBranchTransactionDate();
                        aRateMaster.PostedDate = commonService.GetDate();
                        aRateMaster.PostedBy = Global.UserId;
                        ARate aRate = new ARate();
                        //aRate.ADetail = aDetail;
                        aRate.IAccno = scheduleModel.IAccno;
                        aRate.IRate = Convert.ToSingle(scheduleModel.Rate);
                        aRateMaster.ARates.Add(aRate);
                        uow.Repository<ARateMaster>().Add(aRateMaster);
                        uow.Commit();
                    }
                    //for renew type restructure
                    else if ((!scheduleTrialModel.IsReschedule) && (scheduleTrialModel.IsRestructure))
                    {
                        HALRenew hALRenew = new HALRenew();
                        ALoan aLoan = new ALoan();
                        if (aPolicyInterest != null && aLoanGrace != null && aDur != null)
                        {
                            hALRenew.PSID = aPolicyInterest.PSID;
                            hALRenew.GDayP = aLoanGrace.GDayP;
                            hALRenew.GDayI = aLoanGrace.GDayI;
                            hALRenew.GDateP = aLoanGrace.GDateP;
                            hALRenew.GDateI = aLoanGrace.GDateI;
                            hALRenew.MDate = aDur.MatDat;
                        }

                        else
                        {

                            hALRenew.GDayP = 0;
                            hALRenew.GDayI = 0;
                        }
                        hALRenew.IAccNo = scheduleModel.IAccno;
                        hALRenew.Duration = Convert.ToInt32(scheduleModel.Duration);
                        hALRenew.StDate = scheduleModel.ValueDate;
                        hALRenew.PF = scheduleModel.PrincipalFrequency;
                        hALRenew.IF = scheduleModel.InterestFrequency;
                        hALRenew.Days = scheduleModel.Day;
                        hALRenew.PMI = scheduleModel.PaymentMode;
                        hALRenew.Rate = Convert.ToInt32(scheduleModel.Rate);
                        hALRenew.RNDate = commonService.GetDate();
                        hALRenew.Flag = 3;
                        uow.Repository<HALRenew>().Add(hALRenew);

                        //Insert Data in Adur

                        //adurSingle.IAccno = scheduleTrialModel.IAccno



                        if (scheduleModel.ScheduleType == 10)
                        {
                            aDur.Days = (int)scheduleModel.Duration;
                        }
                        else
                        {
                            aDur.Month = (int)scheduleModel.Duration;
                        }
                        uow.Repository<ADur>().Edit(aDur);


                        //Insert Data in Arate

                        if (aDetail.IRate != scheduleModel.Rate)
                        {
                            aDetail.IRate = scheduleModel.Rate;
                            uow.Repository<ADetail>().Edit(aDetail);
                        }


                        ARateMaster aRateMaster = new ARateMaster();
                        aRateMaster.EffectiveDate = commonService.GetBranchTransactionDate();
                        aRateMaster.PostedDate = commonService.GetDate();
                        aRateMaster.PostedBy = Global.UserId;
                        //for rate
                        ARate aRate = new ARate();
                        //aRate.ADetail = aDetail;
                        aRate.IAccno = scheduleModel.IAccno;
                        aRate.IRate = Convert.ToSingle(scheduleModel.Rate);
                        aRateMaster.ARates.Add(aRate);
                        uow.Repository<ARateMaster>().Add(aRateMaster);
                        //for ifrequency and pfrequency

                        singleAloan.PFreq = scheduleModel.PrincipalFrequency;
                        singleAloan.IFreq = scheduleModel.InterestFrequency;
                        singleAloan.PAYSID = scheduleModel.ScheduleType;
                        uow.Repository<ALoan>().Edit(singleAloan);
                 
                        uow.Commit();


                    }

                    else//for reschedule type restructure
                    {
                        HALRenew hALRenew = new HALRenew();
                        ALoan aLoan = new ALoan();
                        if (aPolicyInterest != null && aLoanGrace != null && aDur != null)
                        {
                            hALRenew.PSID = aPolicyInterest.PSID;
                            hALRenew.GDayP = aLoanGrace.GDayP;
                            hALRenew.GDayI = aLoanGrace.GDayI;
                            hALRenew.GDateP = aLoanGrace.GDateP;
                            hALRenew.GDateI = aLoanGrace.GDateI;
                            hALRenew.MDate = aDur.MatDat;
                        }

                        else
                        {

                            hALRenew.GDayP = 0;
                            hALRenew.GDayI = 0;
                        }
                        hALRenew.IAccNo = scheduleModel.IAccno;
                        hALRenew.Duration = Convert.ToInt32(scheduleModel.Duration);
                        hALRenew.StDate = scheduleModel.ValueDate;
                        hALRenew.PF = scheduleModel.PrincipalFrequency;
                        hALRenew.IF = scheduleModel.InterestFrequency;
                        hALRenew.Days = scheduleModel.Day;
                        hALRenew.PMI = scheduleModel.PaymentMode;
                        hALRenew.Rate = Convert.ToInt32(scheduleModel.Rate);
                        hALRenew.RNDate = commonService.GetDate();
                        hALRenew.Flag = 4;
                        uow.Repository<HALRenew>().Add(hALRenew);

                        if (aDetail.IRate != scheduleModel.Rate)
                        {
                            aDetail.IRate = scheduleModel.Rate;
                            uow.Repository<ADetail>().Edit(aDetail);
                        }


                        ARateMaster aRateMaster = new ARateMaster();
                        aRateMaster.EffectiveDate = commonService.GetBranchTransactionDate();
                        aRateMaster.PostedDate = commonService.GetDate();
                        aRateMaster.PostedBy = Global.UserId;
                        ARate aRate = new ARate();
                        //aRate.ADetail = aDetail;
                        aRate.IAccno = scheduleModel.IAccno;
                        aRate.IRate = Convert.ToSingle(scheduleModel.Rate);
                        aRateMaster.ARates.Add(aRate);
                        uow.Repository<ARateMaster>().Add(aRateMaster);
                        //for ifrequency and pfrequency

                        singleAloan.PFreq = scheduleModel.Principal;
                        singleAloan.IFreq = scheduleModel.InterestFrequency;
                        singleAloan.PAYSID = scheduleModel.ScheduleType;
                        uow.Repository<ALoan>().Edit(singleAloan);
                        uow.Commit();

                    }


                    uow.Commit();


                    transaction.Complete();
                    returnMessage.Msg = "Reschedule Saved Successfully !";
                    returnMessage.Success = true;
                    return returnMessage;
                }


                catch (Exception e)
                {

                    transaction.Dispose();
                    returnMessage.Msg = "Reschedule not saved";
                    returnMessage.Success = false;
                    return returnMessage;
                }
            }
        }

        public LoanRegAccOpenViewModel GetWithOutFDLoanDetails(int pid, decimal quotationAmount, decimal sAmount)
        {
            LoanRegAccOpenViewModel loanAccountOpen = new LoanRegAccOpenViewModel();

            var LAmount = uow.Repository<ProductDetail>().FindBy(x => x.PID == pid).Select(x => x.LAmt).SingleOrDefault();
            //loanAccountOpen.MinValue = Math.Min(LAmount, quotationAmount);
            loanAccountOpen.MinValue = LAmount;


            //    if (sAmount > minimumValue) { 
            //    flag = false;
            //    return flag;
            //}
            //    else{
            //        return flag;
            //    }
            return loanAccountOpen;
        }

        public decimal GetCalculateExtraInterest(int accountId, decimal currentRemainingPrincipal, DateTime date)
        {
            TellerService tellerService = new TellerService();
            int intCalSchId = uow.Repository<APolicyInterest>().FindBy(x => x.IAccno == accountId).Select(x => x.PSID).FirstOrDefault();
            var AccountDetails = tellerService.GetSingleAccountDetails(accountId);

            int days = (date - commonService.GetBranchTransactionDate().AddDays(-1)).Days;
            decimal finalCalcAmount = 0;
            //if (intCalSchId == 5)
            //{
            //    finalCalcAmount = Math.Round(((AccountDetails.IonBal * AccountDetails.IRate / 100) / 365) * days, 2);
            //}
            //else
            //{
            //   var matureAmount = CurrentMaturePrinciapAndInterest(accountId);

            finalCalcAmount = Math.Round(((currentRemainingPrincipal * AccountDetails.IRate / 100) / 365) * days, 2);
            //}
            return finalCalcAmount;
        }

        public DateTime GetNextIntallmentDate(int accountId)
        {
            DateTime tDate = commonService.GetBranchTransactionDate();
            DateTime? getDate = uow.Repository<ALSch>().FindBy(x => x.IAccno == accountId && x.schDate > tDate).Min(y => y.schDate);
            if (getDate == null)
            {
                getDate = commonService.GetBranchTransactionDate();
            }
            return Convert.ToDateTime(getDate);
        }

        public List<LoanPaymentInformationModel> GetSchDetailOnPymnt(int accountId)
        {
            var schDetailOnP = uow.Repository<LoanPaymentInformationModel>().SqlQuery("Exec fin.GetSchDetailOnPymnt " + accountId + ",'" + commonService.GetBranchTransactionDate() + "'").ToList();
            return schDetailOnP;
        }

        public MaturePrincipalInterestModel CurrentMaturePrinciapAndInterest(int iAacon)
        {
            var matureDetail = uow.Repository<MaturePrincipalInterestModel>().SqlQuery("select * from fin.FGetpimatcur(" + iAacon + ",'" + commonService.GetBranchTransactionDate() + "')").FirstOrDefault();
            return matureDetail;
        }

        public ReturnBaseMessageModel InsertLoanPayment(LoanPaymentModel loanPayment, DenoInOutViewModel denoInOutModel, TaskVerificationList taskVerificationList)
        {
            using (var transaction = uow.GetContext().Database.BeginTransaction())
            {
                try
                {

                    bool IsTrnsWithDeno = commonService.IsTransactionWithDeno();
                    if (IsTrnsWithDeno)
                    {
                        if (!TellerUtilityService.BalanceWithDenoAmount(denoInOutModel, loanPayment.Payment))
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
                    if (loanPayment.PrePaymentMode != -1)
                    {
                        if (loanPayment.ExtraInterest < loanPayment.PaymentInterest)
                        {
                            returnMessage.Msg = "Interest payment  is more than extra interest.!!";
                            returnMessage.Success = false;
                            return returnMessage;
                        }
                        if (!CreditUtilityService.IsAllowPrePayment(loanPayment.IAccno))
                        {
                            returnMessage.Msg = "Pre-payment not allowed for this account.!!";
                            returnMessage.Success = false;
                            return returnMessage;
                        }
                    }

                    returnMessage = CreditUtilityService.ValidatePayment(loanPayment);
                    if (returnMessage.Success == false)
                    {
                        return returnMessage;
                    }

                    returnMessage = commonService.ValidateAccountNo(loanPayment.IAccno);
                    if (!returnMessage.Success)
                    {
                        return returnMessage;
                    }

                    //decimal rebatePayment = CreditUtilityService.PayCalcutation(loanPayment.IAccno, "");
                    //if (rebatePayment < loanPayment.Rebate)
                    //{
                    //    returnMessage.Success = false;
                    //    returnMessage.Msg = "Rebate amount is not more than" + rebatePayment;
                    //    return returnMessage;
                    //}

                    decimal calculateTotal = CreditUtilityService.PayCalcutation(loanPayment.IAccno, "Other");
                    var matureAmount = CurrentMaturePrinciapAndInterest(loanPayment.IAccno);
                    decimal balance = Math.Round(((decimal)matureAmount.CurrentPrincipal + calculateTotal + (decimal)matureAmount.MaturePrincipal), 2);
                    //decimal totalBalance = balance - loanPayment.Rebate; removed rebate
                    decimal totalBalance = balance;
                    if (totalBalance < loanPayment.Payment)
                    {
                        returnMessage.Success = false;
                        returnMessage.Msg = "Payment amount is not more than" + totalBalance;
                        return returnMessage;
                    }

                    if (loanPayment.Payment == 0)
                    {
                        returnMessage.Success = false;
                        returnMessage.Msg = "Payment amount  can not be Zero.!!";
                        return returnMessage;
                    }
                    if (loanPayment.PaymentInterest != 0)
                    {
                        returnMessage = CreditUtilityService.CheckForPrePayment(loanPayment);
                        if (!returnMessage.Success)
                        {
                            return returnMessage;
                        }
                    }
                    bool checkPendingTransaction = CreditUtilityService.CheckPendingTransaction(loanPayment.IAccno);

                    if (checkPendingTransaction)
                    {
                        returnMessage.Success = false;
                        returnMessage.Msg = "There is a pending transaction for this account.Please verify the pending transaction!!.!!";
                        return returnMessage;
                    }
                    //bool checkforPrePayment = CreditUtilityService.CheckForPrePayment(loanPayment);

                    Int64 transactionNumber = commonService.GetUtno();
                    ASTrn asTransaction = new ASTrn();

                    var interestPaidDate = GetInterestPaidDate(loanPayment.IAccno, Convert.ToDecimal(loanPayment.IonPA));
                    if (loanPayment.PaymentInterest != 0 && interestPaidDate != null)
                    {
                        interestPaidDate.DateValue = loanPayment.Date;
                    }
                    asTransaction.IAccno = loanPayment.IAccno;
                    asTransaction.tdate = commonService.GetBranchTransactionDate();
                    if (loanPayment.Notes == "" || loanPayment.Notes == null)
                    {
                        loanPayment.Notes = "Payment";
                    }
                    asTransaction.notes = loanPayment.Notes;
                    asTransaction.slpno = 0;
                    asTransaction.cramt = loanPayment.Payment;
                    asTransaction.ttype = 1;
                    asTransaction.postedby = Global.UserId;
                    asTransaction.IsSlp = true;
                    asTransaction.brnhno = commonService.GetBranchIdByEmployeeUserId(); ;
                    asTransaction.tno = transactionNumber;

                    asTransaction.PostedOn = commonService.GetDate();
                    uow.Repository<ASTrn>().Add(asTransaction);

                    //Pid dictionary define on FGetProdFinLedger()
                    if (loanPayment.OtherIonCAPayment != 0)
                    {

                        ASTransLoan objAstransLoanRowSingle = new ASTransLoan();
                        objAstransLoanRowSingle.tno = transactionNumber;
                        objAstransLoanRowSingle.PId = 2;
                        // objAstransLoanRowSingle.Amt = loanPayment.OtherIonCAPayment + loanPayment.OtherIonCARebate;
                        objAstransLoanRowSingle.Amt = loanPayment.OtherIonCAPayment;
                        uow.Repository<ASTransLoan>().Add(objAstransLoanRowSingle);

                    }

                    if (loanPayment.OtherIonCRPayment != 0)
                    {
                        ASTransLoan objAstransLoanRowSingle1 = new ASTransLoan();
                        objAstransLoanRowSingle1.tno = transactionNumber;
                        objAstransLoanRowSingle1.PId = 7;
                        // objAstransLoanRowSingle1.Amt = loanPayment.OtherIonCRPayment + loanPayment.OtherIonCRRebate;
                        objAstransLoanRowSingle1.Amt = loanPayment.OtherIonCRPayment;
                        uow.Repository<ASTransLoan>().Add(objAstransLoanRowSingle1);

                    }

                    if (loanPayment.OtherOthrBalPayment != 0)
                    {

                        ASTransLoan objAstransLoanRowSingle2 = new ASTransLoan();
                        objAstransLoanRowSingle2.tno = transactionNumber;
                        objAstransLoanRowSingle2.PId = 14;
                        objAstransLoanRowSingle2.Amt = loanPayment.OtherOthrBalPayment;
                        uow.Repository<ASTransLoan>().Add(objAstransLoanRowSingle2);

                    }

                    if (loanPayment.PenaltyAnbrPonIRPayment != 0)
                    {

                        ASTransLoan objAstransLoanRowSingle3 = new ASTransLoan();
                        objAstransLoanRowSingle3.tno = transactionNumber;
                        objAstransLoanRowSingle3.PId = 9;
                        //objAstransLoanRowSingle3.Amt = loanPayment.PenaltyAnbrPonIRPayment + loanPayment.PenaltyAnbrPonIRRebate;
                        objAstransLoanRowSingle3.Amt = loanPayment.PenaltyAnbrPonIRPayment;
                        uow.Repository<ASTransLoan>().Add(objAstransLoanRowSingle3);

                    }


                    if (loanPayment.PenaltyAnbrPonPrRPayment != 0)
                    {

                        ASTransLoan objAstransLoanRowSingle4 = new ASTransLoan();
                        objAstransLoanRowSingle4.tno = transactionNumber;
                        objAstransLoanRowSingle4.PId = 8;
                        //objAstransLoanRowSingle4.Amt = loanPayment.PenaltyAnbrPonPrRPayment + loanPayment.PenaltyAnbrPonPrRRebate;
                        objAstransLoanRowSingle4.Amt = loanPayment.PenaltyAnbrPonPrRPayment;
                        uow.Repository<ASTransLoan>().Add(objAstransLoanRowSingle4);

                    }

                    if (loanPayment.PenaltyAnbrIonIRPayment != 0)
                    {

                        ASTransLoan objAstransLoanRowSingle5 = new ASTransLoan();
                        objAstransLoanRowSingle5.tno = transactionNumber;
                        objAstransLoanRowSingle5.PId = 6;
                        //objAstransLoanRowSingle5.Amt = loanPayment.PenaltyAnbrIonIRPayment + loanPayment.PenaltyAnbrIonIRRebate;
                        objAstransLoanRowSingle5.Amt = loanPayment.PenaltyAnbrIonIRPayment;
                        uow.Repository<ASTransLoan>().Add(objAstransLoanRowSingle5);

                    }


                    if (loanPayment.PenaltyIncomePonIAPayment != 0)
                    {

                        ASTransLoan objAstransLoanRowSingle6 = new ASTransLoan();
                        objAstransLoanRowSingle6.tno = transactionNumber;
                        objAstransLoanRowSingle6.PId = 4;
                        //objAstransLoanRowSingle6.Amt = loanPayment.PenaltyIncomePonIAPayment + loanPayment.PenaltyIncomePonIARebate;
                        objAstransLoanRowSingle6.Amt = loanPayment.PenaltyIncomePonIAPayment;
                        uow.Repository<ASTransLoan>().Add(objAstransLoanRowSingle6);

                    }

                    if (loanPayment.PenaltyIncomePonPrAPayment != 0)
                    {

                        ASTransLoan objAstransLoanRowSingle7 = new ASTransLoan();
                        objAstransLoanRowSingle7.tno = transactionNumber;
                        objAstransLoanRowSingle7.PId = 3;
                        //objAstransLoanRowSingle7.Amt = loanPayment.PenaltyIncomePonPrAPayment + loanPayment.PenaltyIncomePonPrARebate;
                        objAstransLoanRowSingle7.Amt = loanPayment.PenaltyIncomePonPrAPayment;
                        uow.Repository<ASTransLoan>().Add(objAstransLoanRowSingle7);

                    }


                    if (loanPayment.PenaltyIncomeIonIAPayment != 0)
                    {

                        ASTransLoan objAstransLoanRowSingle8 = new ASTransLoan();
                        objAstransLoanRowSingle8.tno = transactionNumber;
                        objAstransLoanRowSingle8.PId = 1;
                        // objAstransLoanRowSingle8.Amt = loanPayment.PenaltyIncomeIonIAPayment + loanPayment.PenaltyIncomeIonIARebate;
                        objAstransLoanRowSingle8.Amt = loanPayment.PenaltyIncomeIonIAPayment;
                        uow.Repository<ASTransLoan>().Add(objAstransLoanRowSingle8);

                    }


                    if (loanPayment.InterestIonPRPayment != 0)
                    {

                        ASTransLoan objAstransLoanRowSingle9 = new ASTransLoan();
                        objAstransLoanRowSingle9.tno = transactionNumber;
                        objAstransLoanRowSingle9.PId = 5;
                        //objAstransLoanRowSingle9.Amt = loanPayment.InterestIonPRPayment + loanPayment.InterestIonPRRebate;
                        objAstransLoanRowSingle9.Amt = loanPayment.InterestIonPRPayment;
                        uow.Repository<ASTransLoan>().Add(objAstransLoanRowSingle9);

                    }

                    if (loanPayment.InterestIonPAPayment != 0)
                    {

                        ASTransLoan objAstransLoanRowSingle10 = new ASTransLoan();
                        objAstransLoanRowSingle10.tno = transactionNumber;
                        objAstransLoanRowSingle10.PId = 0;
                        //objAstransLoanRowSingle10.Amt = loanPayment.InterestIonPAPayment + loanPayment.InterestIonPARebate + loanPayment.PaymentInterest;
                        objAstransLoanRowSingle10.Amt = loanPayment.InterestIonPAPayment + loanPayment.PaymentInterest;
                        uow.Repository<ASTransLoan>().Add(objAstransLoanRowSingle10);
                    }


                    if (loanPayment.PrincipalMaturPayment != 0)
                    {

                        ASTransLoan objAstransLoanRowSingle11 = new ASTransLoan();
                        objAstransLoanRowSingle11.tno = transactionNumber;
                        objAstransLoanRowSingle11.PId = 13;
                        objAstransLoanRowSingle11.Amt = loanPayment.PrincipalMaturPayment;
                        uow.Repository<ASTransLoan>().Add(objAstransLoanRowSingle11);

                    }
                    //if (loanPayment.Rebate != 0)
                    //{

                    //    decimal rebatonPenalty = loanPayment.OtherIonCARebate + loanPayment.OtherIonCRRebate
                    //        + loanPayment.PenaltyAnbrPonIRRebate + loanPayment.PenaltyAnbrPonPrRRebate
                    //        + loanPayment.PenaltyAnbrIonIRRebate + loanPayment.PenaltyIncomePonIARebate
                    //        + loanPayment.PenaltyIncomePonPrARebate + loanPayment.PenaltyIncomeIonIARebate;

                    //    if (rebatonPenalty != 0)
                    //    {
                    //        ASTransLoan objAstransLoanRowSingle12 = new ASTransLoan();
                    //        objAstransLoanRowSingle12.tno = transactionNumber;
                    //        objAstransLoanRowSingle12.PId = 17;
                    //        objAstransLoanRowSingle12.Amt = rebatonPenalty;
                    //        uow.Repository<ASTransLoan>().Add(objAstransLoanRowSingle12);
                    //    }


                    //    decimal rebateonInterest = loanPayment.InterestIonPRRebate + loanPayment.InterestIonPARebate;
                    //    if (rebateonInterest != 0)
                    //    {
                    //        ASTransLoan objAstransLoanRowSingle13 = new ASTransLoan();
                    //        objAstransLoanRowSingle13.tno = transactionNumber;
                    //        objAstransLoanRowSingle13.PId = 12;
                    //        objAstransLoanRowSingle13.Amt = rebateonInterest;
                    //        uow.Repository<ASTransLoan>().Add(objAstransLoanRowSingle13);
                    //    }

                    //}

                    if (IsTrnsWithDeno)
                    {
                        commonService.DenoInOutTransaction(denoInOutModel, transactionNumber);
                    }
                    if (loanPayment.ReadyToClose == true)
                    {
                        if (loanPayment.RemainingMaturePA != 0)
                        {
                            returnMessage.Success = false;
                            returnMessage.Msg = "Account Close is not allowed";
                            return returnMessage;
                        }
                        else
                        {
                            var accountRow = uow.Repository<ADetail>().FindBy(x => x.IAccno == loanPayment.IAccno).FirstOrDefault();
                            accountRow.AccState = 7;
                            commonService.AccountStatusLogChange(7, loanPayment.IAccno);
                            var loanGuarantor = uow.Repository<Guarantor>().FindByMany(x => x.LIaccno == loanPayment.IAccno).ToList();
                            foreach (var item in loanGuarantor)
                            {
                                var guarantorRow = uow.Repository<Guarantor>().FindByMany(x => x.LIaccno == loanPayment.IAccno).FirstOrDefault();
                                guarantorRow.Status = false;
                                uow.Repository<Guarantor>().Edit(guarantorRow);
                                uow.Commit();
                            }
                            uow.Repository<ADetail>().Edit(accountRow);
                        }
                          
                    }
                    //else
                    //{
                    //    if (balance == loanPayment.Payment)//Ready to be close
                    //    {
                    //        var accountRow = uow.Repository<ADetail>().FindBy(x => x.IAccno == loanPayment.IAccno).FirstOrDefault();
                    //        accountRow.AccState = 3;
                    //        uow.Repository<ADetail>().Edit(accountRow);
                    //        uow.Commit();
                    //       AccountClosedSave(loanPayment.IAccno);
                    //        commonService.AccountStatusLogChange(3, loanPayment.IAccno);
                    //    }
                    //}
                    var aloanRow = uow.Repository<ALoan>().FindBy(x => x.IAccno == loanPayment.IAccno).FirstOrDefault();
                    aloanRow.LastTotalInterestPaidDate = interestPaidDate.DateValue;
                    uow.Repository<ALoan>().Edit(aloanRow);
                    commonService.SaveUpdateMyBalance(0, commonService.DefultCurrency(), loanPayment.Payment, Global.UserId);
                    taskUow.SaveTaskNotification(taskVerificationList, transactionNumber, 22);
                    uow.Commit();
                    transaction.Commit();
                    returnMessage.Success = true;
                    returnMessage.Msg = "Payment save successfully with Transaction number #" + transactionNumber;
                    return returnMessage;
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    returnMessage.Success = false;
                    returnMessage.Msg = "Payment not save.!!" + ex.Message;
                    return returnMessage;
                }
            }
        }
        public void AccountClosedSave(int iaccNo)
        {
            Aclosed aclosedRow = new Aclosed();
            aclosedRow.Iaccno = iaccNo;
            aclosedRow.Vdate = commonService.GetBranchTransactionDate();
            aclosedRow.postby = Global.UserId;
            uow.Repository<Aclosed>().Add(aclosedRow);

            uow.Commit();
        }
        public ReturnSingleValueModdel GetInterestPaidDate(int iaccNo, decimal Amount)
        {
            DateTime? currentDate = uow.Repository<ALoan>().FindByMany(x => x.IAccno == iaccNo).Select(x => x.LastTotalInterestPaidDate).FirstOrDefault();

            var date = uow.Repository<ReturnSingleValueModdel>().SqlQuery("select  fin.fgetInterestPaidDate(" + iaccNo + "," + Amount + ",'" + currentDate + "')").FirstOrDefault();
            return date;
        }
        public ReturnBaseMessageModel ApproveLoanPaymentVerification(Int64 tno)
        {
            DateTime currrentDate = commonService.GetBranchTransactionDate();
            int approveBy = Global.UserId;
            int userBranch = Global.BranchId;
            int count = uow.ExecWithStoreProcedure("[fin].[PVerifyTran] " + tno + ",'" + currrentDate + "'," + approveBy + "," + userBranch + "");
            if (count > 0)
            {

                returnMessage.Success = true;
                returnMessage.Msg = "Payment verified successfully with Transaction number #" + tno;
                return returnMessage;
            }
            else
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Payment not verified.!!";
                return returnMessage;
            }
        }
        public List<ASTransLoanModel> GetASTransLoanList(long utno)
        {
            List<ASTransLoanModel> unverifiedList = new List<ASTransLoanModel>();

            unverifiedList = uow.Repository<ASTransLoan>().FindByMany(x => x.tno == utno).Select(x => new ASTransLoanModel()
            {
                Amt = x.Amt,
                PId = x.PId,
                tno = x.tno
            }).ToList();

            if (unverifiedList == null)
            {
                unverifiedList = uow.Repository<AMTransLoan>().FindByMany(x => x.tno == utno).Select(x => new ASTransLoanModel()
                {
                    Amt = x.Amt,
                    PId = x.Pid,
                    tno = x.tno
                }).ToList();
            }
            return unverifiedList;
        }
        #endregion

        #region date calculation
        private DateTime GetNextInstallmentDate(DateTime CurrentDate, EScheduleType ScheduleType, int Frequency, int CustomDay)
        {

            DateTime dResult = default(DateTime);

            if (ScheduleType == EScheduleType.DefaultEnglishDate | ScheduleType == EScheduleType.CustomEnglishDay)
            {
                if (CustomDay == 0)
                    CustomDay = CurrentDate.Day;
                dResult = new System.DateTime(CurrentDate.Year, CurrentDate.Month, 1);
                dResult = dResult.AddMonths(Frequency);
                if (System.DateTime.DaysInMonth(dResult.Year, dResult.Month) < CustomDay)
                {
                    CustomDay = System.DateTime.DaysInMonth(dResult.Year, dResult.Month);
                }
                dResult = new System.DateTime(dResult.Year, dResult.Month, CustomDay);
                //End of English Month
            }
            else if (ScheduleType == EScheduleType.EndOfEnglishMonth)
            {
                CustomDay = CurrentDate.Day;
                int DaysInMonth = System.DateTime.DaysInMonth(CurrentDate.Year, CurrentDate.Month);
                if (DaysInMonth != CurrentDate.Day)
                {
                    Frequency = Frequency - 1;
                }
                dResult = new System.DateTime(CurrentDate.Year, CurrentDate.Month, 1);
                dResult = dResult.AddMonths(Frequency);
                dResult = new System.DateTime(dResult.Year, dResult.Month, System.DateTime.DaysInMonth(dResult.Year, dResult.Month));
                //Default Date Weekly
            }
            else if (ScheduleType == EScheduleType.DefaultWeekly)
            {
                dResult = CurrentDate.AddDays(7 * Frequency);
                //End of Weekend
            }
            else if (ScheduleType == EScheduleType.EndOfWeekEnd)
            {
                DayOfWeek weekDay = CurrentDate.DayOfWeek;
                int dWeekDay = Convert.ToInt16(weekDay);
                int dAddDay = 0;
                if (dWeekDay != 5)
                {
                    if (dWeekDay > 5)
                    {
                        dAddDay = 5 - dWeekDay + 7;
                    }
                    else if (dWeekDay < 5)
                    {
                        dAddDay = 5 - dWeekDay;
                    }
                    dAddDay = ((Frequency - 1) * 7) + dAddDay;

                    dResult = CurrentDate.AddDays(dAddDay);
                }
                else
                {
                    dResult = CurrentDate.AddDays(7 * Frequency);
                }
                //Daily
            }
            else if (ScheduleType == EScheduleType.Daily)
            {
                dResult = CurrentDate.AddDays(Frequency);

            }
            else if (ScheduleType == EScheduleType.CustomNepaliDay | ScheduleType == EScheduleType.DefaultNepaliDate | ScheduleType == EScheduleType.EndOfNepaliMonth)
            {
                string DNDate = datePickerService.GetDateBS(CurrentDate);

                if (ScheduleType != EScheduleType.EndOfNepaliMonth)
                {
                    if (CustomDay == 0)
                        CustomDay = datePickerService.GetBSDay(DNDate);
                }
                else
                {
                    CustomDay = datePickerService.GetBSDay(DNDate);
                }

                var GetDate = datePickerService.GetDate(2, datePickerService.GetBSYear(DNDate), datePickerService.GetBSMonth(DNDate), 1);
                DNDate = GetDate.DateBS;

                int nDaysInMonth = datePickerService.GetDaysInMonthByYearAndMonth(datePickerService.GetBSYear(DNDate), datePickerService.GetBSMonth(DNDate));

                if (ScheduleType == EScheduleType.EndOfNepaliMonth)
                {
                    if (nDaysInMonth != CustomDay)
                    {
                        Frequency = Frequency - 1;
                    }
                }

                DNDate = this.AddMonth(DNDate, Frequency);

                nDaysInMonth = datePickerService.GetDaysInMonthByYearAndMonth(datePickerService.GetBSYear(DNDate), datePickerService.GetBSMonth(DNDate));

                if (ScheduleType != EScheduleType.EndOfNepaliMonth)
                {

                    if (nDaysInMonth > CustomDay)
                    {
                        dResult = datePickerService.GetDateAD(datePickerService.GetBSYear(DNDate), datePickerService.GetBSMonth(DNDate), (short)CustomDay);
                    }
                    else
                    {
                        dResult = datePickerService.GetDateAD(datePickerService.GetBSYear(DNDate), datePickerService.GetBSMonth(DNDate), (short)nDaysInMonth);
                    }
                }
                else
                {
                    dResult = datePickerService.GetDateAD(datePickerService.GetBSYear(DNDate), datePickerService.GetBSMonth(DNDate), (short)nDaysInMonth);
                }
            }
            return dResult;
        }

        private DateTime GetMatureDate(DateTime StartDate, EScheduleType ScheduleType, int Duration, bool DurState = true)
        {

            DateTime dResult = default(DateTime);
            if (DurState == true)
            {
                if (ScheduleType == EScheduleType.CustomEnglishDay | ScheduleType == EScheduleType.DefaultEnglishDate | ScheduleType == EScheduleType.EndOfEnglishMonth)
                {
                    dResult = StartDate.AddMonths(Duration);
                    dResult = dResult.AddDays(-1);
                }
                else if (ScheduleType == EScheduleType.Daily)
                {
                    dResult = StartDate.AddDays(Duration);
                }
                else if (ScheduleType == EScheduleType.EndOfWeekEnd | ScheduleType == EScheduleType.DefaultWeekly)
                {
                    dResult = StartDate.AddDays(Duration * 7);
                    dResult = dResult.AddDays(-1);
                }
                else
                {
                    string DNDate = datePickerService.GetDateBS(StartDate);
                    int CustomDay = 0;

                    if (CustomDay == 0)
                        CustomDay = datePickerService.GetBSDay(DNDate);

                    var GetDate = datePickerService.GetDate(2, datePickerService.GetBSYear(DNDate), datePickerService.GetBSMonth(DNDate), 1);
                    DNDate = GetDate.DateBS;

                    DNDate = this.AddMonth(DNDate, Duration);

                    int nDaysInMonth = datePickerService.GetDaysInMonthByYearAndMonth(datePickerService.GetBSYear(DNDate), datePickerService.GetBSMonth(DNDate));

                    if (nDaysInMonth > CustomDay)
                    {
                        dResult = datePickerService.GetDateAD(datePickerService.GetBSYear(DNDate), datePickerService.GetBSMonth(DNDate), (short)CustomDay);
                    }
                    else
                    {
                        dResult = datePickerService.GetDateAD(datePickerService.GetBSYear(DNDate), datePickerService.GetBSMonth(DNDate), (short)nDaysInMonth);
                    }
                    dResult = dResult.AddDays(-1);
                }
            }
            else
            {
                dResult = StartDate.AddDays(Duration);
            }

            return dResult;
        }

        private string AddMonth(string CurrentDate, int AddValue)
        {
            int nYear = datePickerService.GetBSYear(CurrentDate);
            int nMonth = datePickerService.GetBSMonth(CurrentDate);
            int nDay = datePickerService.GetBSDay(CurrentDate);
            int nAddMonth;
            int nAddYear;
            int nNewMonth;
            int nNewYear;
            nAddMonth = (nMonth + AddValue);
            if ((nAddMonth > 12))
            {
                double floor1 = Math.Floor(nAddMonth / 12.00);
                nAddYear = Convert.ToInt32(floor1);
                nNewMonth = (nAddMonth
                            - (nAddYear * 12));
                if ((nNewMonth == 0))
                {
                    nNewMonth = 12;
                    nAddYear = (nAddYear - 1);
                }

            }
            else
            {
                nAddYear = 0;
                nNewMonth = nAddMonth;
            }

            nNewYear = (nYear + nAddYear);
            string Result;
            int nDaysInMonth;
            nDaysInMonth = datePickerService.GetDaysInMonthByYearAndMonth(nNewYear, nNewMonth);
            if ((nDaysInMonth > nDay))
            {

                var GetDate = datePickerService.GetDate(2, nNewYear, (short)nNewMonth, (short)nDay);
                Result = GetDate.DateBS;

            }
            else
            {

                var GetDate = datePickerService.GetDate(2, nNewYear, (short)nNewMonth, (short)nDaysInMonth);
                Result = GetDate.DateBS;
            }

            return Result;
        }
        #endregion

        #region Collateral

        public ReturnBaseMessageModel InsertUpdateCollateral(ALCollModel alcModel, ALFixedDepositModel fixedDepositModel, ALCollLandModel alLandModel, ALCollVehicleModel alVehicleModel)
        {
            try
            {

                var alcModelRow = uow.Repository<ALColl>().GetSingle(x => x.Sno == alcModel.Sno);
                var accountState = TellerUtilityService.GetAccountStatus(alcModel.IAccno);
                if (accountState == 3)
                {
                    returnMessage.Msg = "Account is already Closed.";
                    returnMessage.Success = false;
                    return returnMessage;
                }
                if (alcModelRow == null)
                {
                    alcModelRow = new ALColl();
                }
                if (fixedDepositModel.fdAccno != null && fixedDepositModel.fdAccno != "")
                {
                    var fixedRow = uow.Repository<ALFixedDeposit>().GetSingle(x => x.AlFixedId == fixedDepositModel.AlFixedId);
                    if (fixedRow == null)
                    {
                        fixedRow = new ALFixedDeposit();
                    }
                    fixedRow.IsInternal = fixedDepositModel.IsInternalAccount;
                    fixedRow.fdAccno = fixedDepositModel.fdAccno;
                    fixedRow.amount = fixedDepositModel.Balance;
                    fixedRow.openDt = fixedDepositModel.openDt;
                    fixedRow.matDt = fixedDepositModel.matDt;
                    fixedRow.bank = fixedDepositModel.bank;
                    fixedRow.brnh = fixedDepositModel.brnh;

                    alcModel.CValue = (decimal)fixedDepositModel.amount;

                    if (fixedDepositModel.AlFixedId == 0)
                    {
                        alcModelRow.ALFixedDeposits.Add(fixedRow);
                    }
                    else
                    {
                        uow.Repository<ALFixedDeposit>().Edit(fixedRow);
                    }

                }
                if (alLandModel.Kittano != null && alLandModel.Kittano != "")
                {
                    var alLandRow = uow.Repository<ALCollLand>().GetSingle(x => x.AcolLandId == alLandModel.AcolLandId);
                    if (alLandRow == null)
                    {
                        alLandRow = new ALCollLand();
                    }

                    alLandRow.Fname = alLandModel.Fname;
                    alLandRow.Gname = alLandModel.Gname;
                    alLandRow.Location = alLandModel.Location;
                    alLandRow.Area = alLandModel.Area;
                    alLandRow.Direction = alLandModel.Direction;
                    alLandRow.Seatno = alLandModel.Seatno;
                    alLandRow.Kittano = alLandModel.Kittano;
                    alLandRow.BArea = alLandModel.BArea;

                    alcModel.CValue = alLandModel.Amount;
                    if (alLandModel.AcolLandId == 0)
                    {
                        alcModelRow.ALCollLands.Add(alLandRow);
                    }
                    else
                    {
                        uow.Repository<ALCollLand>().Edit(alLandRow);
                    }

                }
                if (alVehicleModel.LicNo != null && alVehicleModel.LicNo != "")
                {
                    var alVehicleRow = uow.Repository<ALCollVehicle>().GetSingle(x => x.ColVechicleId == alVehicleModel.ColVechicleId);
                    if (alVehicleRow == null)
                    {
                        alVehicleRow = new ALCollVehicle();
                    }
                    alVehicleRow.LicNo = alVehicleModel.LicNo;
                    alVehicleRow.LicRight = alVehicleModel.LicRight;
                    alVehicleRow.IssueOff = alVehicleModel.IssueOff;
                    alVehicleRow.ModNo = alVehicleModel.ModNo;
                    alVehicleRow.ChassisNo = alVehicleModel.ChassisNo;
                    alVehicleRow.EngNo = alVehicleModel.EngNo;
                    alVehicleRow.Color = alVehicleModel.Color;
                    alVehicleRow.CC = alVehicleModel.CC;
                    alVehicleRow.MC = alVehicleModel.MC;
                    alVehicleRow.MD = alVehicleModel.MD;
                    alVehicleRow.type = alVehicleModel.type;
                    alVehicleRow.VehicleNo = alVehicleModel.VehicleNo;
                    alVehicleRow.description = alVehicleModel.description;

                    alcModel.CValue = alVehicleModel.Amount;
                    if (alVehicleModel.ColVechicleId == 0)
                    {
                        alcModelRow.ALCollVehicles.Add(alVehicleRow);
                    }
                    else
                    {
                        uow.Repository<ALCollVehicle>().Edit(alVehicleRow);
                    }

                }

                alcModelRow.IAccno = alcModel.IAccno;
                alcModelRow.NCID = alcModel.NCID;
                alcModelRow.CValue = alcModel.CValue;
                alcModelRow.Weightage = alcModel.Weightage;
                alcModelRow.OName = alcModel.OName;
                alcModelRow.OAdd = alcModel.OAdd;
                alcModelRow.contactNo = alcModel.contactNo;
                alcModelRow.citizenshipNo = alcModel.citizenshipNo;
                alcModelRow.RegNo = alcModel.RegNo;
                alcModelRow.RegDate = alcModel.RegDate;
                alcModelRow.Remarks = alcModel.Remarks;
                if (alcModelRow.Sno == 0)
                {
                    alcModelRow.IsAc = true;
                    alcModelRow.IsActive = true;
                    alcModelRow.PostedBy = Global.UserId;
                    alcModelRow.PostedOn = commonService.GetDate();
                    uow.Repository<ALColl>().Add(alcModelRow);
                }
                else
                {
                    uow.Repository<ALColl>().Edit(alcModelRow);
                }

                Guarantor guarantorData = uow.Repository<Guarantor>().GetAll().Where(x => x.Sno == alcModel.Sno).SingleOrDefault();




                if (alcModel.NCID == 5)
                {
                    if (guarantorData != null)
                    {




                        guarantorData.LIaccno = alcModel.IAccno;
                        guarantorData.GIaccno = Int32.Parse(fixedDepositModel.fdAccno);
                        guarantorData.BlockedAmt = fixedDepositModel.amount;
                        if (alcModel.Remarks != null)
                        {
                            guarantorData.DisplayMsg = true;
                        }
                        else
                        {
                            guarantorData.DisplayMsg = false;
                        }
                        //     guarantorModel.DisplayMsg = alcModel.Remarks;
                        if (fixedDepositModel.QuationPer > 0)
                        {
                            guarantorData.IsPercent = true;
                        }
                        else
                        {
                            guarantorData.IsPercent = false;
                        }

                        guarantorData.Status = true;
                        guarantorData.PostedBy = Global.UserId;
                        guarantorData.PostedOn = commonService.GetDate();
                        uow.Repository<Guarantor>().Edit(guarantorData);

                    }
                    else
                    {
                        Guarantor guarantor = new Guarantor();
                        guarantor.LIaccno = alcModel.IAccno;
                        guarantor.GIaccno = Int32.Parse(fixedDepositModel.fdAccno);
                        guarantor.BlockedAmt = fixedDepositModel.amount;
                        if (alcModel.Remarks != null)
                        {
                            guarantor.DisplayMsg = true;
                        }
                        else
                        {
                            guarantor.DisplayMsg = false;
                        }
                        //     guarantorModel.DisplayMsg = alcModel.Remarks;
                        if (fixedDepositModel.QuationPer > 0)
                        {
                            guarantor.IsPercent = true;
                        }
                        else
                        {
                            guarantor.IsPercent = false;
                        }

                        guarantor.Status = true;
                        guarantor.PostedBy = Global.UserId;
                        guarantor.PostedOn = commonService.GetDate();
                        uow.Repository<Guarantor>().Add(guarantor);
                    }

                }



                uow.Commit();
                returnMessage.Msg = "Successfully Saved";
                returnMessage.Success = true;
                returnMessage.ReturnId = alcModel.IAccno;

                return returnMessage;
            }
            catch (Exception ex)
            {

                returnMessage.Success = false;
                returnMessage.Msg = " Not saved.!!" + ex.Message;
                return returnMessage;
            }
        }

        public ALCollVehicleModel GetVehicleById(int vechileId)
        {
            var vechile = uow.Repository<ALCollVehicle>().FindByMany(x => x.ColVechicleId == vechileId).Select(x => new ALCollVehicleModel()
            {
                ColVechicleId = x.ColVechicleId,
                Sno = x.Sno,
                LicNo = x.LicNo,
                LicRight = x.LicRight,
                IssueOff = x.IssueOff,
                ModNo = x.ModNo,
                ChassisNo = x.ChassisNo,
                EngNo = x.EngNo,
                Color = x.Color,
                CC = x.CC,
                MC = x.MC,
                MD = x.MD,
                type = x.type,
                description = x.description
            }).FirstOrDefault();
            vechile.Amount = GetAlColById(vechile.Sno).CValue;
            return vechile;
        }

        public ALCollLandModel GetLandAndBuildingById(int landId)
        {
            var land = uow.Repository<ALCollLand>().FindByMany(x => x.AcolLandId == landId).Select(x => new ALCollLandModel()
            {
                AcolLandId = x.AcolLandId,
                Sno = x.Sno,
                Fname = x.Fname,
                Gname = x.Gname,
                Location = x.Location,
                Area = x.Area,
                Direction = x.Direction,
                Seatno = x.Seatno,
                Kittano = x.Kittano,
                BArea = x.BArea
            }).FirstOrDefault();
            land.Amount = GetAlColById(land.Sno).CValue;
            return land;
        }

        public ALFixedDepositModel GetInternalFixedAccountCollateralById(int alFixedId)
        {
            var alFixedDeposit = uow.Repository<ALFixedDeposit>().FindByMany(x => x.AlFixedId == alFixedId).Select(x => new ALFixedDepositModel()
            {
                AlFixedId = x.AlFixedId,
                Sno = x.Sno,
                IsInternalAccount = x.IsInternal,
                fdAccno = x.fdAccno,
                Balance = x.amount,
                openDt = x.openDt,
                matDt = x.matDt,
                bank = x.bank,
                brnh = x.brnh
            }).FirstOrDefault();
            alFixedDeposit.amount = GetAlColById(alFixedDeposit.Sno).CValue;
            if (alFixedDeposit.IsInternalAccount)
            {
                alFixedDeposit.QuationPer = Convert.ToDecimal((alFixedDeposit.amount / alFixedDeposit.Balance) * 100);
                alFixedDeposit.AccountNumber = commonService.GetAccountNameByIaccno(Convert.ToInt32(alFixedDeposit.fdAccno)).Accno;
            }
            return alFixedDeposit;
        }

        public List<ALCollModel> GetCollateralList(string accountNumber, int pageNo, int pageSize)
        {
            string query = "select  COUNT(*) OVER () AS TotalCount, * from fin.FGetCollateralList()";
            if (accountNumber != "")
            {

                query += " where AccountNumber like '%" + accountNumber + "%'";
            }

            query += @" ORDER BY  Sno desc
                       OFFSET ((" + pageNo + @" - 1) * " + pageSize + @") ROWS
                       FETCH NEXT " + pageSize + " ROWS ONLY";
            return uow.Repository<ALCollModel>().SqlQuery(query).ToList();
        }
        public List<ALCollModel> GetCollateralListByIAccno(int iaccno)
        {
            string query = "select  * from fin.FGetCollateralList() where IAccno=" + iaccno;

            return uow.Repository<ALCollModel>().SqlQuery(query).ToList();
        }
        public ALCollModel GetAlColById(int colId)
        {
            return uow.Repository<ALColl>().FindByMany(x => x.Sno == colId).Select(x => new ALCollModel()
            {
                Sno = x.Sno,
                NCID = x.NCID,
                IAccno = x.IAccno,
                CValue = x.CValue,
                OName = x.OName,
                OAdd = x.OAdd,
                contactNo = x.contactNo,
                citizenshipNo = x.citizenshipNo,
                RegNo = x.RegNo,
                RegDate = x.RegDate,
                Remarks = x.Remarks,


            }).FirstOrDefault();
        }
        #endregion

        #region  Loan Registration Details
        public ReturnBaseMessageModel InsertUpdateGuarantor(GuarantorModel guarantorModel)
        {
            try
            {
                if (guarantorModel.GuarantorList.Count > 0)
                {
                    foreach (var item in guarantorModel.GuarantorList)
                    {
                        Guarantor gaurentorRow = new Guarantor();
                        gaurentorRow.LIaccno = guarantorModel.LIaccno;
                        gaurentorRow.GIaccno = item.GIaccno;
                        gaurentorRow.IsPercent = item.IsPercent;
                        gaurentorRow.Status = true;
                        gaurentorRow.BlockedAmt = item.BlockedAmt;
                        gaurentorRow.DisplayMsg = guarantorModel.DisplayMsg;
                        gaurentorRow.PostedBy = Global.UserId;
                        gaurentorRow.PostedOn = commonService.GetDate();
                        uow.Repository<Guarantor>().Add(gaurentorRow);
                    }
                    uow.Commit();
                    returnMessage.Msg = "Successfully Saved";
                    returnMessage.Success = true;


                }
                else
                {
                    returnMessage.Msg = "there is no data to save";
                    returnMessage.Success = true;


                }
                return returnMessage;
            }
            catch (Exception ex)
            {

                returnMessage.Success = false;
                returnMessage.Msg = " Not saved.!!" + ex.Message;
                return returnMessage;
            }

        }
        public byte GetSchemeIdByProductId(int productId)
        {
            return uow.Repository<ProductDetail>().FindByMany(x => x.PID == productId).Select(x => x.SDID).FirstOrDefault();
        }

        public LoanRegAccOpenViewModel LoanRegistration(int regId = 0, int taskId = 0, int isAfterRegistration = 0)
        {
            if (regId == 0)
            {
                LoanRegAccOpenViewModel loanregistration = new LoanRegAccOpenViewModel();
                loanregistration.AccountDetailsViewModel = new AccountDetailsViewModel();
                loanregistration.ScheduleTrialModel = new ScheduleTrialModel();
                loanregistration.RegistrationDate = commonService.GetBranchTransactionDate();
                loanregistration.IFreq = 1;
                loanregistration.PFreq = 1;
                return loanregistration;
            }
            else
            {
                var list = uow.Repository<LoanRegAccOpenViewModel>().SqlQuery("select RegId,PID,LAmt,Duration,isnull(ADuration,0) as GrantedDuration ,Status,RegDate as RegistrationDate,isnull(SAmt,0) as SAmt,isnull(Remarks,0),isnull(iAccno,0) as FDLoanAcId,cast(PAYSID as tinyint) as PAYSID  from fin.ALRegistration where regid=" + regId).FirstOrDefault();
                ProductDetail pd = new ProductDetail();
                list.SchemeModel = new SchemeModel();
                list.ScheduleTrialModel = new ScheduleTrialModel();
                pd = uow.Repository<ProductDetail>().SqlQuery("select * from fin.ProductDetail where Pid=" + list.PID).FirstOrDefault();
                list.AccountDetailsViewModel = new AccountDetailsViewModel();
                list.AccountDetailsViewModel.SchemeId = GetSchemeIdByProductId(list.PID);
                list.AccountDetailsViewModel.PID = list.PID;
                list.LoanProductDetails = new LoanProductDetails();
                //     list.LoanProductDetails.PName = GetProductDetails(Convert.ToByte(list.PID)).ProductName;
                list.LoanProductDetails.PName = GetProductDetails((list.PID)).ProductName;
                list.SchemeModel.SchemeName = GetSingleSchemeDetails(Convert.ToByte(list.AccountDetailsViewModel.SchemeId)).SchemeName;
                //      list.AccountDetailsViewModel.DateFormat = Convert.ToByte(GetProductDetails(Convert.ToByte(list.PID)).DurState);
                list.AccountDetailsViewModel.DateFormat = Convert.ToByte(GetProductDetails((list.PID)).DurState);
                list.isAfterRegistration = isAfterRegistration;
                list.InterestRate = Convert.ToDecimal(pd.IRate);
                list.penGDys = pd.penGDys;
                //     list.FDLoanAcId = list.iAccno;
                // list.FDLoanAcId = commonService.GetAccountNumberToString(list.FDLoanAcId);

                list.PrincipalPenaltyRate = Convert.ToDecimal(pd.PPRate);
                list.InterestPenaltyRate = Convert.ToDecimal(pd.PIRate);

                list.Aname = uow.Repository<LoanRegAccOpenViewModel>().SqlQuery("Select top 1 Aname from Fin.FGetLoanRegisteredVerifiedList() where Regid=" + regId).Select(x => x.Aname).FirstOrDefault();
                if (isAfterRegistration == 1)
                {
                    FinanceParameterService financeParameterService = new FinanceParameterService();
                    var chargeAvailable = financeParameterService.GetChargeDetailsByProductId(list.AccountDetailsViewModel.PID, 23).ToList();
                    if (chargeAvailable.Count != 0)
                        list.IsChargeAvailable = true;
                    else
                        list.IsChargeAvailable = false;
                    string dateType = commonService.DateType();
                    if (GetProductDetails(list.PID).DurationType == 0)
                    {

                        decimal days = TellerUtilityService.GetmatDurationDays((double)list.GrantedDuration);
                        var matDate = commonService.GetMatDate(list.RegistrationDate, days, dateType);
                        var dateTime = datePickerService.GetDateBSAndAD(matDate);
                        list.MaturationDate = matDate;

                    }
                    else
                    {
                        var matDate = commonService.GetMatDate(list.RegistrationDate, list.GrantedDuration, dateType);
                        var dateTime = datePickerService.GetDateBSAndAD(matDate);
                        list.MaturationDate = matDate;
                    }

                }

                return list;
            }
        }

        public LoanRegAccOpenViewModel LoanAccountDetails(int iaccno = 0, int taskId = 0)
        {

            LoanRegAccOpenViewModel loanregistration = new LoanRegAccOpenViewModel();


            var list = uow.Repository<LoanRegAccOpenViewModel>().SqlQuery("select PAYID,IAccno as iAccno,PAYSID,PFreq,IFreq,cDay,overPay,IsInsured,AutoPayment,penGDys,Revolving,deprived,PostedBy,PostedOn from fin.ALoan where IAccno" + iaccno).FirstOrDefault();
            list.AccountDetailsViewModel = new AccountDetailsViewModel();
            var accountDetails = uow.Repository<ADetail>().FindBy(x => x.IAccno == iaccno).FirstOrDefault();
            list.AccountDetailsViewModel.IAccno = accountDetails.IAccno;
            list.AccountDetailsViewModel.PID = Convert.ToByte(accountDetails.PID);
            list.AccountDetailsViewModel.SchemeId = GetSchemeIdByProductId(accountDetails.PID);
            var adrlimit = uow.Repository<ADrLimit>().FindBy(x => x.IAccno == iaccno).FirstOrDefault();
            if (adrlimit != null)
            {
                list.SAmt = adrlimit.AppAmt;
                list.LAmt = (decimal)adrlimit.QuotAmt;
            }


            return list;

        }

        public ProductViewModel GetProductDetails(int pid)
        {
            var productDetail = uow.Repository<ProductDetail>().FindByMany(x => x.PID == pid).Select(pd =>
                            new ProductViewModel
                            {
                                ProductId = pd.PID,
                                Duration = pd.Duration,
                                //DurState = SqlFunctions.StringConvert((decimal)pd.durState),
                                //DurState=Convert.ToString (pd.durState),
                                // DurState = pd.durState == 0 ? "0" : pd.durState.ToString()|| pd.durState == 0 ? "0" : pd.durState.ToString(),
                                DurState = pd.durState == 0 ? "0" : pd.durState == null?  "0" : pd.durState.ToString(),
                                HasIndividualDuration = pd.HasIndDuration == null ? false : true,
                                ProductName = pd.PName,
                                DurationType = pd.durState == null ? 0 : pd.durState,
                                IsFDLoan = pd.SchmDetail.IsFDLoan == null ? false : pd.SchmDetail.IsFDLoan == false ? false : true,
                                SchemeId = pd.SDID
                            }).FirstOrDefault();
            int durState = Convert.ToInt32(productDetail.DurState);
            if (durState == 0)
                productDetail.Duration = productDetail.Duration * 365;

            return productDetail;
        }

        public SchemeModel GetSingleSchemeDetails(byte schemeId)
        {
            var scheme = uow.Repository<SchmDetail>().FindByMany(x => x.SDID == schemeId).Select(sd =>
                           new SchemeModel
                           {
                               SchemeID = sd.SDID,
                               SchemeName = sd.SDName,
                           }).FirstOrDefault();

            return scheme;
        }

        public List<CustomerAccountsViewModel> LoanCustomerByregId(int regId)
        {
            var list = uow.Repository<CustomerAccountsViewModel>().SqlQuery("select ci.*,alreg.RegId from [cust].[FGetCustList]() ci inner join fin.ALRegCusts alreg on alreg.cid=ci.CID where regId=" + regId).ToList();
            return list;
        }

        public ReturnBaseMessageModel LoanRegistrationSave(LoanRegAccOpenViewModel loanReg, TaskVerificationList taskVerificationList, int IsAccepted = 0)
        {
            using (var transaction = uow.GetContext().Database.BeginTransaction())
            //TransactionScopeOption.RequiresNew, new TransactionOptions()
            //{
            //    IsolationLevel = IsolationLevel.ReadCommitted
            //}
            //  ))
            {

                if (loanReg.Duration < 0)
                {
                    returnMessage.Msg = "Loan Date is Matured";
                    returnMessage.BoolValue = false;
                    return returnMessage;

                }
                string message;
                try
                {

                    int eventid = 0;
                    int eventvalue = 0;
                    int taskid = 0;
                    ALRegistration registration = new ALRegistration();
                    //for new registration
                    if (loanReg.RegId == 0)
                    {
                        registration.RegDate = loanReg.RegistrationDate;
                        registration.LAmt = loanReg.LAmt;
                        registration.PAYSID = loanReg.PAYSID;
                        // registration.PID = Convert.ToByte(loanReg.PID);
                        registration.PID = loanReg.PID;
                        registration.Duration = loanReg.Duration;
                        registration.PostedBy = Loader.Models.Global.UserId;
                        registration.Status = 1;

                        eventid = 17;
                        if (loanReg.FDLoanAcc != null)
                        {
                            loanReg.FDLoanAcId = int.Parse(loanReg.FDLoanAcc);
                            registration.iAccno = loanReg.FDLoanAcId;
                            registration.iAccno = loanReg.FDLoanAcId;
                        }

                        uow.Repository<ALRegistration>().Add(registration);

                        byte count = 1;
                        foreach (var item in loanReg.CustomerId)
                        {
                            ALRegCust accountOfCustomer = new ALRegCust();
                            accountOfCustomer.CID = item;
                            accountOfCustomer.SNo = count;
                            count++;
                            registration.ALRegCusts.Add(accountOfCustomer);

                        }
                        uow.Commit();
                        message = "Successfully Saved with RegNo" + registration.RegId;
                        eventvalue = registration.RegId;
                        taskUow.SaveTaskNotification(taskVerificationList, registration.RegId, 17);
                    }

                    //for approve registration
                    else
                    {


                        registration = uow.Repository<ALRegistration>().FindBy(x => x.RegId == loanReg.RegId).FirstOrDefault();
                        registration.ApprovedOn = commonService.GetDate(); ;
                        registration.ApprovedBy = Loader.Models.Global.UserId;
                        if (loanReg.isAccepted == 0)
                        {
                            registration.Status = 2;
                            registration.SAmt = loanReg.SAmt;
                            registration.ADuration = loanReg.GrantedDuration;
                            eventid = 18;
                            eventvalue = loanReg.RegId;
                            if (taskVerificationList.VerifierList.Count != 0 || loanReg.RegId == 0)
                            {
                                taskUow.SaveTaskNotification(taskVerificationList, eventvalue, 18);
                            }
                            message = "Successfully Saved with RegNo" + registration.RegId;
                        }
                        else
                        {
                            registration.Status = 0;
                            registration.Remarks = "Rejected";
                            message = "Successfully Rejected with RegNo" + registration.RegId;
                        }
                        uow.Repository<ALRegistration>().Edit(registration);
                        taskid = uow.Repository<ChannakyaBase.DAL.DatabaseModel.Task>().FindBy(x => x.EventId == 17 && x.EventValue == eventvalue).Select(c => c.Task1Id).FirstOrDefault();
                        taskUow.VerifiedOn(taskid);

                    }
                    uow.Commit();
                    transaction.Commit();
                    returnMessage.Msg = message;
                    returnMessage.Success = true;
                    returnMessage.ReturnId = registration.RegId;
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

        public List<LoanRegAccOpenViewModel> AllLoanRegVefifiedCustomer()
        {

            List<LoanRegAccOpenViewModel> list = uow.Repository<LoanRegAccOpenViewModel>().SqlQuery("Select * from Fin.FGetLoanRegisteredVerifiedList()").OrderByDescending(x => x.RegId).ToList();
            return list;
        }
        public ReturnBaseMessageModel LoanAccountOpen(LoanRegAccOpenViewModel LoanAccOpen, TaskVerificationList TaskVerificationList, List<ChargeDetail> ChargeDetailsList, List<GuarantorModel> GuarantorList)
        {
            using (var transaction = uow.GetContext().Database.BeginTransaction())
            {


                try
                {
                    //if already registered product and other feilds are pre set
                    if (LoanAccOpen.isAfterRegistration == 1)
                    {
                        var LoanRegDetails = uow.Repository<LoanRegAccOpenViewModel>().SqlQuery("select RegId,PID,LAmt,Duration,ADuration as GrantedDuration ,Status,RegDate,SAmt,Remarks,iAccno,Convert(tinyint,PAYSID) as PAYSID  from fin.ALRegistration where regid=" + LoanAccOpen.RegId).FirstOrDefault();
                        {
                            LoanAccOpen.PID = LoanRegDetails.PID;
                            LoanAccOpen.LAmt = LoanRegDetails.LAmt;
                            LoanAccOpen.GrantedDuration = LoanRegDetails.GrantedDuration;
                            LoanAccOpen.SAmt = LoanRegDetails.SAmt;
                            LoanAccOpen.PAYSID = LoanRegDetails.PAYSID;
                        }
                    }

                    TellerService ts = new TellerService();
                    string type = commonService.DateType();
                    //var SchemeId = uow.Repository<ProductDetail>().FindBy(x => x.PID == LoanAccOpen.PID).FirstOrDefault();
                    var productDetails = GetProductDetails(LoanAccOpen.PID);
                    //for adding in account details
                    ADetail accountDetails = uow.Repository<ADetail>().GetSingle(x => x.IAccno == LoanAccOpen.iAccno);
                    if (accountDetails == null)
                    {
                        accountDetails = new ADetail();
                    }
                    accountDetails.PID = LoanAccOpen.PID;
                    accountDetails.RDate = LoanAccOpen.RegistrationDate;
                    accountDetails.CurrID = commonService.DefultCurrency();
                    accountDetails.BrchID = (byte)commonService.GetBranchIdByEmployeeUserId(); ;
                    accountDetails.AccState = 6;
                    accountDetails.PostedBy = Convert.ToString(Loader.Models.Global.UserId);
                    accountDetails.Aname = LoanAccOpen.Aname;
                    accountDetails.IRate = LoanAccOpen.InterestRate;
                    if (LoanAccOpen.DateType == true)
                    {
                        accountDetails.DateType = 1;
                    }
                    else
                    {
                        accountDetails.DateType = 2;
                    }
                    uow.Repository<ADetail>().Add(accountDetails);
                    byte count = 1;
                    //for transferring the preassigned customers during the registration process from alreg to the aofcust
                    if (LoanAccOpen.isAfterRegistration == 1)
                    {
                        var aregCust = uow.Repository<ALRegCust>().FindBy(x => x.RegId == LoanAccOpen.RegId).ToList();
                        foreach (var item in aregCust)
                        {
                            AOfCust aofCust = new AOfCust();
                            aofCust.CID = (decimal)item.CID;
                            aofCust.Sno = (byte)item.SNo;
                            accountDetails.AOfCusts.Add(aofCust);

                        }
                    }
                    //for direct account open to add customer to the aofcust
                    else
                    {
                        foreach (var item in LoanAccOpen.CustomerId)
                        {
                            AOfCust accountOfCustomer = uow.Repository<AOfCust>().FindBy(x => x.IAccno == LoanAccOpen.iAccno && x.CID == item).FirstOrDefault();
                            if (accountOfCustomer == null)
                            {
                                accountOfCustomer = new AOfCust();
                            }
                            accountOfCustomer.CID = item;
                            accountOfCustomer.Sno = count;
                            count++;
                            accountDetails.AOfCusts.Add(accountOfCustomer);
                        }
                    }
                    byte priority = 0;
                    //for loan nominee accounts
                    foreach (var item in LoanAccOpen.LinkedAccountsList)
                    {
                        priority += 1;
                        ALinkloan alinkloan = new ALinkloan();

                        alinkloan.ILinkAc = item.LinkIaccno;
                        alinkloan.priority = priority;
                        accountDetails.ALinkloans.Add(alinkloan);

                    }
                    //for the saving the duration related data in adur in both of the cases
                    ADur adur = new ADur();
                    adur.IAccno = accountDetails.IAccno;
                    adur.ValDat = commonService.GetBranchTransactionDate();
                    adur.MatDat = LoanAccOpen.MaturationDate;
                    if (!productDetails.IsFDLoan)
                    {
                        adur.DurType = Convert.ToInt32(productDetails.DurState);
                        if (adur.DurType == 1)
                            adur.Month = LoanAccOpen.GrantedDuration;
                        else
                            adur.Days = LoanAccOpen.GrantedDuration;
                    }
                    else
                    {
                        adur.DurType = 0;

                        adur.Days = LoanAccOpen.GrantedDuration;
                    }


                    accountDetails.ADur = adur;
                    //for the saving the Qouation and approved related data in adrlimit in both of the cases
                    ADrLimit adrLimit = new ADrLimit();
                    adrLimit.AppAmt = (decimal)LoanAccOpen.SAmt;
                    adrLimit.QuotAmt = (decimal)LoanAccOpen.LAmt;

                    accountDetails.ADrLimit = adrLimit;
                    //for saving the data of the related rates and effective dates
                    ARateMaster aratemaster = new ARateMaster();
                    aratemaster.PostedDate = commonService.GetBranchTransactionDate();
                    aratemaster.EffectiveDate = LoanAccOpen.RegistrationDate;
                    aratemaster.PostedBy = Loader.Models.Global.UserId;
                    uow.Repository<ARateMaster>().Add(aratemaster);
                    uow.Commit();
                    ARate arate = new ARate();
                    arate.IRate = (float)LoanAccOpen.InterestRate;
                    aratemaster.ARates.Add(arate);
                    accountDetails.ARates.Add(arate);

                    //for interest calculation rate
                    if (LoanAccOpen.InCalRuleId != 0)
                    {
                        APolicyInterest policyInterest = new APolicyInterest();
                        policyInterest.PSID = LoanAccOpen.InCalRuleId;
                        accountDetails.APolicyInterest = policyInterest;
                    }

                    if (LoanAccOpen.PenaltyCalculation != 0)
                    {
                        //for penalty calculation rate
                        APolPen apolpen = new APolPen();
                        apolpen.PCID = LoanAccOpen.PenaltyCalculation;
                        accountDetails.APolPen = apolpen;
                    }

                    //for aloan calculation
                    ALoan aloan = new ALoan();

                    aloan.PAYID = LoanAccOpen.PAYID;
                    aloan.PAYSID = LoanAccOpen.PAYSID;
                    aloan.PFreq = LoanAccOpen.PFreq;
                    aloan.IFreq = LoanAccOpen.IFreq;
                    aloan.cDay = LoanAccOpen.cDay;
                    aloan.overPay = LoanAccOpen.overPay;
                    aloan.IsInsured = LoanAccOpen.IsInsured;
                    aloan.AutoPayment = LoanAccOpen.AutoPayment;
                    aloan.penGDys = LoanAccOpen.penGDys;
                    aloan.Revolving = LoanAccOpen.Revolving;
                    aloan.deprived = LoanAccOpen.deprived;
                    aloan.PostedBy = Loader.Models.Global.UserId;
                    aloan.PostedOn = commonService.GetDate(); ;
                    accountDetails.ALoan = aloan;
                    //for grace calculation
                    ALoanGrace aloangrace = new ALoanGrace();
                    //for grace calculation in days
                    if (LoanAccOpen.ScheduleTrialModel.GraceOption == 2)
                    {

                        aloangrace.GDurP = 0;
                        aloangrace.GDurI = 0;
                        aloangrace.GDayI = (short)LoanAccOpen.ScheduleTrialModel.Interest;
                        aloangrace.GDayP = (short)LoanAccOpen.ScheduleTrialModel.Principal;
                    }
                    //for grace calculation in months
                    else if (LoanAccOpen.ScheduleTrialModel.GraceOption == 3)
                    {

                        aloangrace.GDurI = (short)LoanAccOpen.ScheduleTrialModel.Interest;
                        aloangrace.GDurP = (short)LoanAccOpen.ScheduleTrialModel.Principal;
                        aloangrace.GDayI = 0;
                        aloangrace.GDayP = 0;
                    }
                    //for grace calculation in specificdate
                    else if (LoanAccOpen.ScheduleTrialModel.GraceOption == 4)
                    {

                        aloangrace.GDurI = 0;
                        aloangrace.GDurP = 0;
                        aloangrace.GDayI = 0;
                        aloangrace.GDayP = 0;
                    }
                    //for grace calculation in except no grace
                    if (LoanAccOpen.ScheduleTrialModel.GraceOption != 1)
                    {
                        aloangrace.GDateI = LoanAccOpen.ScheduleTrialModel.InterestDate;
                        aloangrace.GDateP = LoanAccOpen.ScheduleTrialModel.PrincipalDate;
                        aloangrace.GraceOption = LoanAccOpen.ScheduleTrialModel.GraceOption;
                        accountDetails.ALoanGrace = aloangrace;

                    }
                    if (!productDetails.IsFDLoan)
                    {
                        //for interest and penalty rates
                        APRate aprate = new APRate();
                        aprate.PIRate = (float)LoanAccOpen.InterestPenaltyRate;
                        aprate.PPRate = (float)LoanAccOpen.PrincipalPenaltyRate;
                        accountDetails.APRate = aprate;
                    }


                    ALRegistration alregistration = uow.Repository<ALRegistration>().FindBy(x => x.RegId == LoanAccOpen.RegId).FirstOrDefault();
                    //for direct account open inserting in the alregistration
                    if (alregistration == null)
                    {
                        alregistration = new ALRegistration();
                        alregistration.PID = LoanAccOpen.PID;
                        alregistration.PAYSID = LoanAccOpen.PAYSID;
                        alregistration.Duration = LoanAccOpen.Duration;
                        alregistration.ADuration = LoanAccOpen.GrantedDuration;
                        alregistration.Status = 2;
                        alregistration.RegDate = LoanAccOpen.RegistrationDate;
                        alregistration.LAmt = LoanAccOpen.LAmt;
                        alregistration.SAmt = LoanAccOpen.SAmt;
                        alregistration.Remarks = LoanAccOpen.Remarks;
                        alregistration.PostedBy = Loader.Models.Global.UserId;
                        alregistration.ApprovedBy = Loader.Models.Global.UserId;
                        alregistration.ApprovedOn = LoanAccOpen.RegistrationDate;
                        accountDetails.ALRegistrations.Add(alregistration);
                        //for direct account open inserting in the alregistration
                        foreach (var item in LoanAccOpen.CustomerId)
                        {
                            ALRegCust accountOfCustomer = new ALRegCust();
                            accountOfCustomer.CID = item;
                            accountOfCustomer.SNo = count;
                            count++;
                            alregistration.ALRegCusts.Add(accountOfCustomer);
                        }
                    }
                    if (GuarantorList != null)
                    {
                        foreach (var item in GuarantorList)
                        {
                            Guarantor gaurentorRow = new Guarantor();

                            gaurentorRow.GIaccno = item.GIaccno;
                            gaurentorRow.IsPercent = item.IsPercent;
                            gaurentorRow.Status = true;
                            gaurentorRow.BlockedAmt = item.BlockedAmt;
                            gaurentorRow.DisplayMsg = LoanAccOpen.GuarantorModel.DisplayMsg;
                            gaurentorRow.PostedBy = Global.UserId;
                            gaurentorRow.PostedOn = commonService.GetDate();
                            accountDetails.Guarantors.Add(gaurentorRow);
                        }
                    }
                    uow.Commit();

                    int iaccno = accountDetails.IAccno;
                    //for direct account open updating in the alregistration
                    if (LoanAccOpen.RegId != 0)
                    {
                        alregistration.Remarks = LoanAccOpen.Remarks;
                        alregistration.iAccno = iaccno;
                        uow.Repository<ALRegistration>().Edit(alregistration);
                    }
                    commonService.AccountStatusLogChange(6, iaccno);
                    uow.Commit();
                    //for direct account open updating in the verifying the task details
                    if (LoanAccOpen.RegId != 0)
                    {
                        int taskid = uow.Repository<ChannakyaBase.DAL.DatabaseModel.Task>().FindBy(x => x.EventId == 18 && x.EventValue == LoanAccOpen.RegId).Select(c => c.Task1Id).FirstOrDefault();
                        if (taskid != 0)
                        {
                            taskUow.VerifiedOn(taskid);
                        }
                    }
                    if (ChargeDetailsList != null)
                    {
                        FinanceParameterService financeParameterService = new FinanceParameterService();

                        financeParameterService.ChargeUnverifiedTransaction(commonService, ChargeDetailsList, iaccno, TaskVerificationList, 23);
                    }
                    taskUow.SaveTaskNotification(TaskVerificationList, iaccno, 19);

                    transaction.Commit();
                    returnMessage.Msg = "Account create Successfully";
                    returnMessage.Success = true;
                }

                catch (Exception ex)
                {
                    transaction.Dispose();
                    returnMessage.Msg = "Account  not save.!!" + ex.Message;
                    returnMessage.Success = false;
                }
            }
            return returnMessage;
        }

        public List<InterestCalculation> GetAllRuleCalculation(int ProductId = 0, int PSID = 0)
        {
            string query = "";
            if (PSID != 0)
                query = "SELECT PSID as InCalRuleId,PSNAME as InterestCalRule FROM fin.POLICYINTCALC where PSID=" + PSID;
            else
                query = "SELECT POLICYINTCALC.PSID as InCalRuleId, POLICYINTCALC.PSNAME as InterestCalRule FROM fin.POLICYINTCALC as POLICYINTCALC " +
                    "INNER JOIN fin.PRODUCTPSID ON POLICYINTCALC.PSID = PRODUCTPSID.PSID " +
                    "where PRODUCTPSID.pid=" + ProductId;

            var intcalrule = uow.Repository<InterestCalculation>().SqlQuery(query).ToList();

            return intcalrule;
        }

        public List<RulePaySch> GetAllRuleSchedule(int PAYSID = 0)
        {
            string query = "";

            query = "select * from fin.RulePaySch where PAYSID=" + PAYSID;
            var schedule = uow.Repository<RulePaySch>().SqlQuery(query).ToList();

            return schedule;
        }

        public List<PenaltyCalculation> GetAllPenalty(int ProductId = 0, int PCID = 0)
        {
            string query = "";
            if (PCID != 0)
            {
                query = "select PCID,PCNAME from fin.PolicyPenCalc where PCID=" + PCID;
            }
            else
                query = "select pp.PCID,pp.PCNAME from fin.PolicyPenCalc pp inner join fin.ProductPCID pc on pp.PCID=pc.PCID where pid=" + ProductId;

            var penaltytcalrule = uow.Repository<PenaltyCalculation>().SqlQuery(query).ToList();

            return penaltytcalrule;
        }

        public List<InterestCapitalisation> GetAllPaymentMode(int ProductId = 0, int payId = 0)
        {
            string query = "";
            if (payId != 0)
            {
                query = "select PAYID, PRule,active from fin.RulePay where PAYID =" + payId;
            }
            else
                query = "select r.PAYID, r.PRule,active from fin.ProductPay p inner join fin.RulePay r on p.PAYID = r.PAYID where pid =" + ProductId + "order by r.PayID";

            var PaymentMode = uow.Repository<InterestCapitalisation>().SqlQuery(query).ToList();

            return PaymentMode;
        }

        public LoanProductDetails GetSingleProductDetails(int ProductId = 0)
        {
            LoanProductDetails loanProductDetails = new LoanProductDetails();
            loanProductDetails = uow.Repository<LoanProductDetails>().SqlQuery(@"SELECT [SDID]as[SDID],[PID] as[PID]	,[FID] as[FID],[PName]as [PName],isnull([Duration],0)as[Duration]
	                                                                             ,isnull([HasOverdraw],0)as[HasOverdraw],isnull([HasCheque],0)as[HasCheque],isnull([HasCard],0)as[HasCard],isnull([HasCertificate],0)as[HasCertificate]
		                                                                        ,isnull([HasIndivLimit]	,0)as[HasIndivLimit],isnull([HasIndDuration],0)as[HasIndDuration],isnull([DormantPeriod],0)as[DormantPeriod]
		                                                                        ,isnull([HasIndivRate],0)as[HasIndivRate]	,isnull([IRate],0)as[IRate],isnull([OIRate],0)as[OIRate],isnull([PPRate],0)as[PPRate]
		                                                                        ,isnull([PIRate],0)as[PIRate],isnull([MID],0)as[MID],isnull([RNWID],0)as[RNWID],isnull([LAmt],0)as[LAmt],isnull([PGRACE],0)as[PGRACE],
		                                                                        isnull([ODuration],0)as[ODuration],isnull([Nomianable],0)as[Nomianable],isnull([enabled],0)as[enabled],isnull([MultiDep],0)as[MultiDep]
		                                                                        ,isnull([Withdrawal],0)as[Withdrawal]	,isnull([IntraBrnhTrnx]	,0)as[IntraBrnhTrnx],isnull([Apfix]	,0)as[Apfix],isnull([aCtr],0)as[aCtr],
		                                                                        isnull([IsInsured],0)as[IsInsured],isnull([NSId],0)as[NSId],isnull([Schedule],0)as[Schedule],isnull([penGDys],0)as[penGDys],
		                                                                        isnull([durState],0)as[durState],isnull([Revolving],0)as[Revolving],isnull([HASSMS],0)as[HASSMS],
		                                                                        (select IsFDLoan from fin.SchmDetails s where s.SDID=ProductDetail.SDID) as IsFDLoan
		                                                                         FROM [fin].[ProductDetail]		
		                                                                         where pid=" + ProductId).FirstOrDefault();
            int durState = (int)loanProductDetails.durState;
            // if (durState == 0)
            // loanProductDetails.Duration = loanProductDetails.Duration * 365;

            var interestCapitalisation = GetAllPaymentMode(ProductId);
            loanProductDetails.InterestCapitalisation = interestCapitalisation;
            var penaltyCalculation = GetAllPenalty(ProductId);
            loanProductDetails.PenaltyCalculationList = penaltyCalculation;
            return loanProductDetails;
        }

        public LoanLinkedAccounts GetSingleLinkAccountDetailsByIaccno(int iaccno, bool isDetailView = false)
        {
            LoanLinkedAccounts linkdetails = new LoanLinkedAccounts();
            var accountdetails = uow.Repository<ADetail>().FindBy(x => x.IAccno == iaccno).FirstOrDefault();
            linkdetails.LAccountName = accountdetails.Aname;
            linkdetails.LAccountNumber = accountdetails.Accno;
            linkdetails.LinkIaccno = accountdetails.IAccno;
            if (isDetailView == true)
                linkdetails.isDetailView = true;
            return linkdetails;

        }

        public LoanAttributes GetLoanProductAttributes(int PId)
        {
            var LoanAttributes = uow.Repository<LoanAttributes>().SqlQuery("select isnull(HasCheque,0),isnull(HasCard,0),isnull(HasIndivRate,0) as HasIndRate,isnull(HasIndivLimit,0) as HasIndLimit,isnull(HASSMS,0) as HasSMS,isnull(Revolving,0) as Revolving,isnull(IsInsured,0) as IsInsured,cast(isnull(durState,0)as int) as Durtype,HasIndDuration  from fin.ProductDetail where PID=" + PId).FirstOrDefault();
            return LoanAttributes;
        }

        public List<AccountDetailsViewModel> GetAllAccountList(int accState, int pageNumber, int pageSize)
        {
            string query = @"Select * from (SELECT    [PID],[IAccno],[Accno],[RDate],[CurrID],[BrchID],[PostedBy],[ApprovedBy],[AccState],[Aname],[OldAccno],[Bal],[IonBal],[IRate],[DateType] as DateFormat,[LastTransDate] 
                        FROM[fin].[ADetail]
                        where IAccno in (select IAccno from[fin].ALoan) and ADetail.ApprovedBy is null
                         )t1 cross join(select count(*) TotalCount from[fin].[ADetail]  where IAccno in (select IAccno from[fin].ALoan) and ADetail.ApprovedBy is null) t2 ";
            query += @" ORDER BY  t1.Aname
                       OFFSET ((" + pageNumber + @" - 1) * " + pageSize + @") ROWS
                       FETCH NEXT " + pageSize + " ROWS ONLY";
            //var account = uow.Repository<ADetail>().SqlQuery(query).ToList();
            var account1 = uow.Repository<AccountDetailsViewModel>().SqlQuery(query).ToList();
            return account1;
        }

        public List<DisburseDepositViewModel> DisburseLinkAccounts(int iaccno)
        {
            var linkAccountListFordisburse = uow.Repository<DisburseDepositViewModel>().SqlQuery("select ad.Aname as AccName,ad.Accno,ad.Bal as GoodsBalance,ad.IAccno as DepositIaccno,Accno as DepositAccounNumber from fin.ALoan al inner join  fin.ALinkloan b on al.iaccno=b.Iaccno inner join fin.ADetail ad on ad.IAccno=b.ILinkAc where al.IAccno=" + iaccno).ToList();
            return linkAccountListFordisburse;
        }
        public List<DisburseChequeDepositViewModel> GetAllBankList(int bankid = 0)
        {
            return commonService.GetAllBankDetails(bankid);
        }
        public ALoan LoanAccountDetails(int iaccno)
        {
            var loanAccountDetails = uow.Repository<ALoan>().FindByMany(x => x.IAccno == iaccno).FirstOrDefault();

            return loanAccountDetails;
        }

        public List<DisburseShareViewModel> DisburseLoanToShare(int iaccno)
        {
            var DisburseLoanToShare = uow.Repository<DisburseShareViewModel>().SqlQuery("select Distinct Name,RegNo,IAccno from fin.FGetLoanShareDisbursement( " + iaccno + ")").ToList();
            DisburseLoanToShare.SingleOrDefault().Rate= shareService.ShareSetupRate();
            return DisburseLoanToShare;
        }
        public ReturnBaseMessageModel LoanDisburseSave(LoanDisbursement loanDisbursement, DisburseChequeDepositViewModel loandisburseCheque, DisburseCashDepositViewModel loanDisburseCash, DisburseChargeDepositViewModel AddtionalChargeList, List<DisburseDepositViewModel> DisburseDepositList, List<ChargeDetail> ChargeDetailsList, TaskVerificationList TaskVerificationList, ScheduleTrialModel scheduleTModel, List<DisburseShareViewModel> ShareList, int IsAccepted = 0)
        {
            using (var transaction = uow.GetContext().Database.BeginTransaction())
            {

                try
                {
                    int isChargeOnSanction = 0;
                    var count = uow.Repository<DisbursementMain>().FindBy(x => x.IAccno == loanDisbursement.AccountId && x.VNo == null).ToList().Count();

                    if (count > 0)
                    {
                        returnMessage.Msg = "Please verify the previous disbursement to Proceed. !!!";
                        returnMessage.Success = false;
                        return returnMessage;
                    }
                    #region Disbursable amount check
                    var loandetails = uow.Repository<ALoan>().FindBy(x => x.IAccno == loanDisbursement.AccountId).FirstOrDefault();




                    var sanctionStatusCheck = uow.Repository<DisbursementMain>().FindBy(x => x.IAccno == loanDisbursement.AccountId).FirstOrDefault();
                    //if (loanDisbursement.IsChargeAvailable)
                    //{
                    //    if (checkSanction.CheckSanction == false)

                    if (sanctionStatusCheck == null)
                    {
                        if (loanDisbursement.ChargeDeductOnSanction)
                        {
                            isChargeOnSanction = 1;
                        }
                        if (loanDisbursement.ChargeDeductOnDisburse)
                        {
                            isChargeOnSanction = 2;
                        }

                    }
                    else if (sanctionStatusCheck.CheckSanction == null)
                    { 
                        if (sanctionStatusCheck.CheckSanction == null)
                        {
                            if (loanDisbursement.ChargeDeductOnSanction == true)
                            {
                                isChargeOnSanction = 1;
                            }
                            else if (loanDisbursement.ChargeDeductOnDisburse == true)
                            {
                                isChargeOnSanction = 2;
                            }

                            else
                            {
                                if (commonService.ChargeDeductOnDisbursement() == 2)//charge on disbursement
                                {
                                    isChargeOnSanction = 2;
                                }
                                else
                                {
                                    isChargeOnSanction = 1;
                                }
                            }
                        }

                    }
                    else if (sanctionStatusCheck.CheckSanction != null)
                    {
                        if (sanctionStatusCheck.CheckSanction == false)//charge on disbursement
                        {
                            isChargeOnSanction = 2;
                        }
                        else
                        {
                            isChargeOnSanction = 1;
                        }
                    }

                    decimal totalAmount = 0;
                    decimal disbursableAmount = 0;
                    decimal ShareAmount = 0;
                    if (isChargeOnSanction == 1)
                    {
                        disbursableAmount = loandetails.ADetail.ADrLimit.AppAmt;
                    }
                    else if (isChargeOnSanction == 2)
                    {
                        if (loanDisbursement.RegularLoan != 0)
                            disbursableAmount = loanDisbursement.RegularLoan;
                        else
                            disbursableAmount = loanDisbursement.OtherLoan;
                    }
                    else if (isChargeOnSanction == 0)
                    {
                        disbursableAmount = loanDisbursement.DisbursableAmount;
                    }
                
                    //decimal loandisbursed = loanDisbursement.RegularLoan + loanDisbursement.OtherLoan;
                    if (loanDisbursement.Deposit)
                    {


                        foreach (var item in DisburseDepositList)
                        {

                            totalAmount = totalAmount + Convert.ToDecimal(item.Amount);

                        }
                        
                        
                    }

                    else if (commonService.isDisburseWithCashandBank())
                    {
                        if (loanDisbursement.Cash)
                        {
                            totalAmount = totalAmount + Convert.ToDecimal(loanDisburseCash.Amount);
                        }
                        if (loanDisbursement.Bank)
                        {
                            totalAmount = totalAmount + Convert.ToDecimal(loandisburseCheque.Amount);
                        }

                    }

                    if (AddtionalChargeList.AddtionalChargeList != null)
                    {
                        foreach (var item in AddtionalChargeList.AddtionalChargeList)
                        {
                            totalAmount = totalAmount + Convert.ToDecimal(item.Amount);
                        }
                    }
                    decimal val1 = 0;
                    //decimal val2 = 0;

                    var checkSanction = uow.Repository<DisbursementMain>().FindBy(x => x.IAccno == loanDisbursement.AccountId).FirstOrDefault();
                    if (loanDisbursement.IsChargeAvailable)
                    {
                        if (checkSanction == null)
                        {
                            if (isChargeOnSanction == 1)
                            {
                                foreach (var item in ChargeDetailsList)
                                {
                                    if (item.ChrType == 1)
                                        totalAmount = totalAmount + Convert.ToDecimal(item.CAmount);
                                    else if (item.ChrType == 2)
                                    {

                                        val1 = ((disbursableAmount / Convert.ToDecimal(item.Ratio)));
                                        //val2 = ( Convert.ToDecimal(item.Ratio)));
                                        if (item.Ratio != 0)
                                        {
                                            totalAmount = totalAmount + ((disbursableAmount / Convert.ToDecimal(item.Ratio * 100)));

                                        }
                                    }
                                }
                            }

                        }



                        else if (checkSanction.CheckSanction == false)
                        {
                            foreach (var item in ChargeDetailsList)
                            {
                                if (item.ChrType == 1)
                                    totalAmount = totalAmount + Convert.ToDecimal(item.CAmount);
                                else if (item.ChrType == 2)
                                {

                                    val1 = ((disbursableAmount / Convert.ToDecimal(item.Ratio)));
                                    //val2 = ( Convert.ToDecimal(item.Ratio)));
                                    if (item.Ratio != 0)
                                    {
                                        totalAmount = totalAmount + ((disbursableAmount / Convert.ToDecimal(item.Ratio * 100)));

                                    }
                                }
                            }
                        }

                    }

                    ////for sharelist
                    if (ShareList != null)
                    {
                        foreach (var item in ShareList)
                        {
                            totalAmount = Convert.ToDecimal(item.Amount);
                        }

                    }
                    if (loanDisbursement.IsOtherLoan == false)
                    {
                        if (totalAmount > disbursableAmount)
                        {
                            returnMessage.Msg = "Loan Disbursable Amount Exceeds";
                            returnMessage.Success = false;
                            return returnMessage;
                        }
                    }
                    #endregion
                    #region For Mode Dictionary

                    int mode = 0;
                    if (loanDisbursement.Deposit)
                        mode = 1;
                    else if (loanDisbursement.Bank)
                        mode = 2;
                    else
                        mode = 3;
                    #endregion
                    #region DisburseMain
                    DisbursementMain disbursementMain = new DisbursementMain();
                    disbursementMain.IAccno = loanDisbursement.AccountId;
                    disbursementMain.PostedOn = commonService.GetDate();
                    disbursementMain.PostedBy = Loader.Models.Global.UserId;
                    disbursementMain.Tdate = commonService.GetBranchTransactionDate();
                    disbursementMain.Mode = mode;

                    if (loanDisbursement.ChargeDeductOnSanction == true)
                    {
                        disbursementMain.CheckSanction = true;
                    }
                    else
                    {
                        disbursementMain.CheckSanction = false;
                    }
                    uow.Repository<DisbursementMain>().Add(disbursementMain);

                    DisburseVsPID regular = new DisburseVsPID();

                    if (loanDisbursement.IsOtherLoan == false)
                    {
                        regular.Amount = loanDisbursement.RegularLoan;
                        regular.FID = commonService.GetFidByIAccno(loanDisbursement.AccountId);
                        regular.Remarks = loanDisbursement.Remarks;
                    }
                    else
                    {
                        regular.Amount = loanDisbursement.OtherLoan;
                        regular.FID = commonService.GetFidOtherLoanByPID(loanDisbursement.Product.ProductId);
                        regular.Remarks = loanDisbursement.Remarks;
                    }
                    disbursementMain.DisburseVsPIDs.Add(regular);



                    #endregion

                    #region Disbursing Methods

                    if (loanDisbursement.Deposit)
                    {
                        foreach (var item in DisburseDepositList)
                        {
                                if (item.Amount != 0 && item.Amount != null)
                                {
                                    DisburseDeposit disburseDeposit = new DisburseDeposit();
                                    disburseDeposit.DepositIaccno = item.DepositIaccno;
                                    disburseDeposit.Amount = item.Amount;
                                    disbursementMain.DisburseDeposits.Add(disburseDeposit);
                                }
                        }
                    }


                    if (commonService.isDisburseWithCashandBank())
                    {
                        if (loanDisbursement.Cash)
                        {
                            DisburseCash disburseCash = new DisburseCash();
                            disburseCash.RecieveFrom = loanDisburseCash.RecieveFrom;
                            disburseCash.Amount = loanDisburseCash.Amount;
                            disbursementMain.DisburseCashes.Add(disburseCash);
                        }
                        if (loanDisbursement.Bank)
                        {
                            DisburseCheque disburseCheque = new DisburseCheque();
                            disburseCheque.BankId = loandisburseCheque.BankId;
                            disburseCheque.Amount = loandisburseCheque.Amount;
                            disburseCheque.ChequeNo = loandisburseCheque.ChequeNo;
                            disbursementMain.DisburseCheques.Add(disburseCheque);
                        }


                    }
                    #endregion
                    #region For charges            
                    if (AddtionalChargeList.AddtionalChargeList != null)
                    {
                        foreach (var item in AddtionalChargeList.AddtionalChargeList)
                        {
                            DisburseCharge disbursecharge = new DisburseCharge();
                            disbursecharge.FID = item.FID;
                            disbursecharge.Amount = item.Amount;
                            disbursementMain.DisburseCharges.Add(disbursecharge);
                        }
                    }


                 
                    
                    FinanceParameterService financeParameterService = new FinanceParameterService();
                   
                    financeParameterService.ChargeUnverifiedTransaction(commonService, ChargeDetailsList, loanDisbursement.AccountId, TaskVerificationList, 20);
                    #endregion
                    #region Customised Schedule 
                    //if (loanDisbursement.HasCustomisedSchedule)
                    //{

                    if (scheduleTModel.scheduleList != null)
                    {
                        foreach (var item in scheduleTModel.scheduleList)
                        {
                            //if (!item.PreviousYear)
                            //{
                            if (item.InterestInstall != 0)
                            {
                                TempALSch aschedule = new TempALSch();
                                aschedule.schDate = item.DateAd;
                                aschedule.schPrin = item.PrincipalInstall;
                                aschedule.schInt = item.InterestInstall;
                                aschedule.balPrin = item.Balance;
                                aschedule.IAccno = loanDisbursement.AccountId;
                                //   disbursementMain.ADetail.TempALSches.Add(aschedule);
                                uow.Repository<TempALSch>().Add(aschedule);
                            }
                            //}

                        }
                    }



                    //}

                    //ALoan aloan = new ALoan();
                    //aloan = uow.Repository<ALoan>().FindBy(x => x.IAccno == loanDisbursement.AccountId).FirstOrDefault();
                    //if (isChargeOnSanction == 1)
                    //{
                    //    aloan.IsChargeOnSancation = true;
                    //}
                    //else
                    //    aloan.IsChargeOnSancation = false;

                    #endregion
                    uow.Commit();
                    var latestDisburseid = uow.Repository<string>().SqlQuery("select top 1 CAST (disburseid  as varchar(100))  from [fin].[DisbursementMain] order by disburseid desc").SingleOrDefault();
                    //for share
                    ShrSPurchase objSharePurchaseRow = new ShrSPurchase();

                    if (ShareList != null)
                    {
                        foreach (var item in ShareList)
                        {
                            if (item.Amount != null)
                            {
                                var transactionNumber = commonService.GetUtno();
                                //objSharePurchaseRow.Note = ShareList.Note;
                                objSharePurchaseRow.Brchid = Convert.ToByte(commonService.GetBranchIdByEmployeeUserId());
                                objSharePurchaseRow.Pdate = commonService.GetBranchTransactionDate();
                                objSharePurchaseRow.PostedBy = Global.UserId;
                                objSharePurchaseRow.Regno = item.RegNo;
                                objSharePurchaseRow.SQty = item.SQty;
                                objSharePurchaseRow.SType = 1;
                                objSharePurchaseRow.Tno = transactionNumber;
                                objSharePurchaseRow.ttype = 5;
                                objSharePurchaseRow.Amt = Convert.ToDecimal(item.Amount);
                                objSharePurchaseRow.ReferenceId = Convert.ToString(latestDisburseid);
                                uow.Repository<ShrSPurchase>().Add(objSharePurchaseRow);
                                uow.Commit();
                            }
                            
                        }
                    }
                    taskUow.SaveTaskNotification(TaskVerificationList, disbursementMain.DisburseId, 21);
                    transaction.Commit();
                    
                    returnMessage.Msg = "Successfully Saved";

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
        public List<LoanDisbursement> AllLoanDisburseVerifiedCustomer()
        {

            List<LoanDisbursement> list = uow.Repository<LoanDisbursement>().SqlQuery("Select * from Fin.FGetLoanDisburseDetails()where ApprovedBy is null").ToList();
            return list;
        }
        public DisbursementMain GetDisbursementMainDetailsBy(int disburseId, int taskId = 0)
        {
            DisbursementMain disbursedetails = uow.Repository<DisbursementMain>().FindBy(x => x.DisburseId == disburseId).FirstOrDefault();
            if (disbursedetails != null)
            {
                disbursedetails.DisburseVsPIDs.OrderBy(x => x.FID);
            }
            else
            {
                disbursedetails = new DisbursementMain();
            }
            //var disbursementString = Convert.ToString(disburseId);
            //ShrSPurchase shrSPurchase = new ShrSPurchase();
            //shrSPurchase = uow.Repository<ShrSPurchase>().FindBy(x => x.ReferenceId == disbursementString).SingleOrDefault();
            //disbursedetails.add

            return disbursedetails;
        }
        public List<LoanDisbursement> AllLoanDisburseVerifiedCustomer(int accState, int pageNumber, int pageSize)
        {
            string query = "SELECT COUNT(*) OVER () AS TotalCount,DisburseId, IAccno,Mode,isnull(Amount,0) as RegularLoan,AccountNumber,AddtionalCharge,Charges,cast(IsOtherLoan as bit)IsOtherLoan from Fin.FGetLoanDisburseDetails() where ApprovedBy is null";
            query += @" ORDER BY  IAccno
                       OFFSET ((" + pageNumber + @" - 1) * " + pageSize + @") ROWS
                       FETCH NEXT " + pageSize + " ROWS ONLY";
            //var account = uow.Repository<ADetail>().SqlQuery(query).ToList();
            var disburseDetails = uow.Repository<LoanDisbursement>().SqlQuery(query).ToList();
            return disburseDetails;
        }
        public ReturnBaseMessageModel LoanDisburseVerifyConfirm(int disburseId)
        {

            using (var transaction = uow.GetContext().Database.BeginTransaction())
            {


                {
                    try
                    {
                        FinanceParameterService financeParameterService = new FinanceParameterService();
                        var disbursementmain = uow.Repository<DisbursementMain>().FindBy(x => x.DisburseId == disburseId).FirstOrDefault();
                        var cst = financeParameterService.ChargeTranxNoByEventIdAndEventValue(20, disbursementmain.IAccno);

                        foreach (var item in cst)
                        {
                            financeParameterService.VerifyChargeTransaction(item.TrnxNo);
                        }
                        VerifyDisburse(disburseId);

                      
                        //int taskid = uow.Repository<ChannakyaBase.DAL.DatabaseModel.Task>().FindBy(x => x.EventId == 21 && x.EventValue == disburseId).Select(c => c.Task1Id).FirstOrDefault();
                        //if (taskid != 0)
                        //{
                        //    taskUow.VerifiedOn(taskid);
                        //}
                        transaction.Commit();
                        returnMessage.Success = true;
                        return returnMessage;
                    }
                    catch (Exception ex)
                    {
                        transaction.Dispose();
                        returnMessage.Success = false;
                        returnMessage.Msg = "Not saved.!!" + ex.Message;
                        return returnMessage;
                    }
                }
            }
        }
        public ReturnBaseMessageModel VerifyDisburse(int disburseId)
        {

            try
            {
                var cashDisburse = uow.Repository<DisburseCash>().FindBy(x => x.DisburseId == disburseId).FirstOrDefault();
                int UserId = Loader.Models.Global.UserId;
                string disburseString = Convert.ToString(disburseId);
                int isSuccess = uow.ExecWithStoreProcedure("[Trans].[PSetLoanDisburseVerify] @disburseId,@ApprovedBy", new SqlParameter("@disburseId", disburseId), new SqlParameter("@ApprovedBy", UserId));
                //int isSuccessForDisbursement= uow.ExecWithStoreProcedure("[fin].[Share_Purchase_Disburse] @disburseId,@ApprovedBy", new SqlParameter("@disburseId", disburseString), new SqlParameter("@ApprovedBy", UserId));
                if (cashDisburse != null)
                {
                    TaskVerificationList taskverifyList = new TaskVerificationList();
                    taskverifyList.IsSelected = true;
                    taskverifyList.Message = "Loan disbursement through Cash with amount of " + cashDisburse.Amount;
                    taskverifyList.UserId = (int)cashDisburse.RecieveFrom;
                    taskverifyList.VerifierList.Add(taskverifyList);
                    taskUow.SaveTaskNotification(taskverifyList, disburseId, 28);
                }


                returnMessage.Msg = "Successfully Saved";
                returnMessage.Success = true;
                return returnMessage;
            }
            catch (Exception ex)
            {

                throw ex;

            }

        }
        public ReturnBaseMessageModel DisburseCashVerifyByTeller(DisbursementMain disburseMain, DenoInOutViewModel denoInOutModel)
        {
            {
                using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions()
                {
                    IsolationLevel = IsolationLevel.ReadUncommitted
                }
                ))
                {
                    try
                    {
                        int taskid = 0;
                        taskid = uow.Repository<ChannakyaBase.DAL.DatabaseModel.Task>().FindBy(x => x.EventId == 28 && x.EventValue == disburseMain.DisburseId).Select(c => c.Task1Id).FirstOrDefault();
                        BaseTaskVerificationService verificationService = new BaseTaskVerificationService();
                        if (verificationService.VerifiedBy(taskid) != "")
                        {
                            returnMessage.Msg = "Payment already made by teller!. ";
                            returnMessage.Success = false;
                            return returnMessage;
                        }
                        Int64 transactionNumber = 0;
                        decimal disburseAmount = 0;
                        var DisburseCashes = uow.Repository<DisburseCash>().FindBy(x => x.DisburseId == disburseMain.DisburseId).FirstOrDefault();
                        if (DisburseCashes != null)
                        {
                            transactionNumber = (long)DisburseCashes.TNo;
                            disburseAmount = (decimal)DisburseCashes.Amount;
                        }
                        returnMessage = TellerUtilityService.CheckUserActivateOrNot();
                        if (!returnMessage.Success)
                        {
                            return returnMessage;
                        }
                        bool IsTrnsWithDeno = commonService.IsTransactionWithDeno();

                        decimal userCurrentBalance = TellerUtilityService.UserCurrentBalance(commonService.DefultCurrency());

                        if (userCurrentBalance < disburseAmount)
                        {
                            returnMessage.Msg = "Request amount exceeds available Cash!. ";
                            returnMessage.Success = false;
                            return returnMessage;
                        }

                        if (IsTrnsWithDeno)
                        {
                            if (!TellerUtilityService.BalanceWithDenoAmount(denoInOutModel, disburseAmount))
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
                        commonService.SaveUpdateMyBalance(Global.UserId, commonService.DefultCurrency(), disburseAmount, 0);
                        if (IsTrnsWithDeno)
                        {
                            commonService.DenoInOutTransaction(denoInOutModel, transactionNumber);
                        }
                        AMTrn amtrans = new AMTrn();
                        amtrans = uow.Repository<AMTrn>().FindBy(x => x.tno == transactionNumber).FirstOrDefault();
                        if (amtrans != null)
                        {
                            amtrans.ttype = 42;
                            uow.Repository<AMTrn>().Edit(amtrans);
                        }


                        if (taskid != 0)
                        {
                            taskUow.VerifiedOn(taskid);
                        }
                        uow.Commit();
                        transaction.Complete();
                        returnMessage.Success = true;
                        returnMessage.Msg = "Teller Disburse Cash save successfully with Transaction number #" + transactionNumber;
                        return returnMessage;


                    }
                    catch (Exception ex)
                    {
                        transaction.Dispose();
                        returnMessage.Success = false;
                        returnMessage.Msg = "Teller Disburse Cash not Verified.!!" + ex.Message;
                        return returnMessage;
                    }
                }
            }
        }
        public ReturnBaseMessageModel DeleteUnverifiedDisburse(int disburseId)
        {
            try
            {
                int UserId = Loader.Models.Global.UserId;
                int isSuccess = uow.ExecWithStoreProcedure(" [Trans].[PDelLoanDisburseVerify] @disburseId", new SqlParameter("@disburseId", disburseId));
                returnMessage.Msg = "Successfully Deleted";
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
        public ScheduleModel ScheduleParameters(int iaccno)
            {
            var scheduleParams = uow.Repository<ScheduleModel>().SqlQuery("select * from Fin.GetLoanScheduleParameter(" + iaccno + ")").LastOrDefault();
            return scheduleParams;
        }


        public ReturnBaseMessageModel CheckExistingLoans(int AccountId)
        {

            var count = uow.Repository<DisbursementMain>().FindByMany(x => x.IAccno == AccountId && x.VNo == null).ToList().Count();
            if (count == 0)
            {

                returnMessage.Success = false;
                return returnMessage;
            }
            else
                returnMessage.Success = true;
            returnMessage.Msg = "Please verify the previous disbursement. !!!";
            return returnMessage;
        }
        #endregion


        public ScheduleTrialModel ScheduleDataByIaccno(int AccountId)
        {
            ScheduleTrialModel scheduleParams = new ScheduleTrialModel();
            scheduleParams = uow.Repository<ScheduleTrialModel>().SqlQuery("select * from Fin.FGetPreviousScheduleData(" + AccountId + ")").FirstOrDefault();
            if (scheduleParams != null)
            {
                scheduleParams.DateAd = commonService.GetBranchTransactionDate();
                scheduleParams.NepaliDate = commonService.GetNepaliDate(scheduleParams.DateAd);
            }
            return scheduleParams;
        }

        public ScheduleTrialModel ScheduleDataRenewByIaccno(int AccountId)
        {
            ScheduleTrialModel scheduleParams = new ScheduleTrialModel();
            scheduleParams = uow.Repository<ScheduleTrialModel>().SqlQuery("select * from Fin.FGetPreviousScheduleDataRenew(" + AccountId + ")").FirstOrDefault();
            if (scheduleParams != null)
            {
                scheduleParams.DateAd = commonService.GetBranchTransactionDate();
                scheduleParams.NepaliDate = commonService.GetNepaliDate(scheduleParams.DateAd);
            }
            return scheduleParams;
        }
        public ReturnBaseMessageModel LoanReSanctionSave(ReSenctionModel reSenctionModel)
        {
            try
            {
                ADrLimit adrLimit = uow.Repository<ADrLimit>().FindByMany(x => x.IAccno == reSenctionModel.IAccno).FirstOrDefault();
                ADrlimitLog adrlimitLog = new ADrlimitLog();
                adrlimitLog.IAccno = reSenctionModel.IAccno;
                adrlimitLog.Amount = adrLimit.AppAmt;
                adrlimitLog.PostedBy = Global.UserId;
                adrlimitLog.PostedOn = commonService.GetDate();

                uow.Repository<ADrlimitLog>().Add(adrlimitLog);
                if (adrLimit.QuotAmt < reSenctionModel.NewSenctionAmount)
                {
                    adrLimit.QuotAmt = reSenctionModel.NewSenctionAmount;
                }
                adrLimit.AppAmt = reSenctionModel.NewSenctionAmount;
                uow.Repository<ADrLimit>().Edit(adrLimit);

                ALRegistration aLRegistration = uow.Repository<ALRegistration>().FindBy(x => x.iAccno == reSenctionModel.IAccno).FirstOrDefault();
                aLRegistration.SAmt = reSenctionModel.NewSenctionAmount;
                uow.Repository<ALRegistration>().Edit(aLRegistration);
                uow.Commit();
                returnMessage.Success = true;
                returnMessage.Msg = "Loan Re-senction save successfully.!!";
                return returnMessage;
            }
            catch (Exception ex)
            {

                returnMessage.Success = false;
                returnMessage.Msg = "Not Save.!!" + ex.Message;
                return returnMessage;
            }
        }

        public bool checkSchemeMappedProduct(int schemeId)
        {
            int schemeMappedProduct = uow.Repository<ProductDetail>().FindBy(x => x.SDID == schemeId).Count();
            if (schemeMappedProduct > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public ChannakyaBase.DAL.DatabaseModel.DisbursementMain GetSanctionStatus(int accountNumber)
        {


            ChannakyaBase.DAL.DatabaseModel.DisbursementMain checkSanctinStatus = new DisbursementMain();
            checkSanctinStatus = uow.Repository<ChannakyaBase.DAL.DatabaseModel.DisbursementMain>().FindBy(x => x.IAccno == accountNumber).FirstOrDefault();
            return checkSanctinStatus;


        }

        public bool? GetSanctionStatusforRemaingAmount(int accountNumber)
        {
            var checkSanctinStatus = uow.Repository<ChannakyaBase.DAL.DatabaseModel.DisbursementMain>().FindBy(x => x.IAccno == accountNumber).Select(x => x.CheckSanction).SingleOrDefault();
            var count = uow.Repository<DisbursementMain>().FindByMany(x => x.IAccno == accountNumber && x.VNo == null).ToList().Count();
            if (count == 0)
            {
                checkSanctinStatus = false;
            }
            return checkSanctinStatus;
        }

        public bool? GetSanctionStatusforRemaingAmountonChange(int accountNumber)
        {
            bool? checkSanctinStatus = true;
            //scheduleParams = uow.Repository<ScheduleTrialModel>().SqlQuery("select * from Fin.FGetPreviousScheduleData(" + AccountId + ")").FirstOrDefault();
            var disbursementbyIAccno = uow.Repository<ChannakyaBase.DAL.DatabaseModel.DisbursementMain>().FindBy(x => x.IAccno == accountNumber).FirstOrDefault();

            if (disbursementbyIAccno != null)
            {
                checkSanctinStatus = disbursementbyIAccno.CheckSanction;
                var count = uow.Repository<DisbursementMain>().FindByMany(x => x.IAccno == accountNumber).ToList().Count();
                if (count == 0)
                {
                    checkSanctinStatus = false;
                }

            }
            else
            {
                checkSanctinStatus = false;
            }

            return checkSanctinStatus;
        }


        //GetCollateralListforPledgeInternalfromGarantor

        public List<Guarantor> GetCollateralListforPledgeInternalfromGarantor(int iAccno)
        {
            var gurantorList = uow.Repository<ChannakyaBase.DAL.DatabaseModel.Guarantor>().FindBy(x => x.LIaccno == iAccno).ToList();

            return gurantorList;
        }

        public SchmDetail GetSchemeDetailFromSchemeId(int schemeId)
        {
            var scheme = uow.Repository<SchmDetail>().FindByMany(x => x.SDID == schemeId).FirstOrDefault();

            return scheme;
        }
    }


}



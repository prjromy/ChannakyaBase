using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannakyaBase.Model.ViewModel
{
    public class GuarantorModel
    {
        public int GId { get; set; }
        [DisplayName("Account Number")]
        public int LIaccno { get; set; }
        [DisplayName("Guarantor Account Number")]
        public int GIaccno { get; set; }

        // [Required(ErrorMessage ="Amount is Required")]
        [DisplayName("Block Amount or Percent")]
        public Nullable<decimal> BlockedAmt { get; set; }
        [DisplayName("Display In Withdraw")]
        public bool DisplayMsg { get; set; }

        [DisplayName("Is Percent")]
        public bool IsPercent { get; set; }
        public bool Status { get; set; }
        public int PostedBy { get; set; }
        public System.DateTime PostedOn { get; set; }
        public string AccountNumber { get; set; }
        public bool IsLoanee { get; set; }
        public List<GuarantorModel> GuarantorList { get; set; }
        public Nullable<int> Sno { get; set; }
    }
    public class ScheduleTrialModel
    {


        [DisplayName("Custome Day")]
        public int Day { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yy}", ApplyFormatInEditMode = true)]
        public DateTime DateAd { get; set; }

        public string NepaliDate { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal PrincipalInstall { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal InterestInstall { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal Balance { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal TotalInstall { get; set; }
        public byte GraceOption { get; set; }
        public bool HasInterest { get; set; }
        public bool HasPrincipal { get; set; }
        public DateTime StartDate { get; set; }
        public int ButtonTap { get; set; }
        public decimal Percent { get; set; }
        public decimal Remprecent { get; set; }
        public bool IsFlat { get; set; }
        public decimal Amount { get; set; }
        public decimal Rate { get; set; }
        public bool IsChecked { get; set; }
        public bool IsRestructure { get; set; }
        public bool IsReschedule { get; set; }
        public decimal RemainingBalance { get; set; }
        public List<ScheduleTrialModel> scheduleList { get; set; }
        [DisplayName("Principal")]
        public int Principal { get; set; }

        [DisplayName("Interest")]
        public int Interest { get; set; }

        [DisplayName("Principal Date")]
        public DateTime PrincipalDate { get; set; }

        [DisplayName("Interest Date")]
        public DateTime InterestDate { get; set; }
        //   public ScheduleModel ScheduleModel { get; set; } 
        [DisplayName("Principal Frequency")]
        public int PrincipalFrequency { get; set; }

        [DisplayName("Interest Frequency")]
        public int InterestFrequency { get; set; }
        public string RequestFrom { get; set; }
        public int IAccno { get; set; }
        public bool PreviousYear { get; set; }
        public int Flag { get; set; }


    }
    public class ScheduleModel
    {
        [DisplayName("Value Date")]
        public DateTime ValueDate { get; set; }

        [DisplayName("Schedule Type")]
        public byte ScheduleType { get; set; }

        [DisplayName("Custome Day")]
        public int Day { get; set; }

        public decimal Amount { get; set; }

        public decimal Duration { get; set; }

        public decimal Rate { get; set; }

        [DisplayName("Payment Mode")]
        public byte PaymentMode { get; set; }

        [DisplayName("Principal Frequency")]
        public int PrincipalFrequency { get; set; }

        [DisplayName("Interest Frequency")]
        public int InterestFrequency { get; set; }

        [DisplayName("Grace Option")]
        public byte GraceOption { get; set; }

        [DisplayName("Principal")]
        public int Principal { get; set; }

        [DisplayName("Interest")]
        public int Interest { get; set; }

        [DisplayName("Principal Date")]
        public DateTime PrincipalDate { get; set; }

        [DisplayName("Interest Date")]
        public DateTime InterestDate { get; set; }
        public bool Revolving { get; set; }
        public bool Flat { get; set; }

        public string RequestFrom { get; set; }
        public int IAccno { get; set; }
        public bool IsReschedule { get; set; }
        public bool IsRestructure { get; set; }
        public DateTime? OldMatureDate { get; set; }
        public int LoanAccountId { get; set; }
    }
    public class ALCollModel
    {
        public int Sno { get; set; }

        [Required]
        [DisplayName("Account Number")]
        public int IAccno { get; set; }

        [Required]
        [DisplayName("Collateral Name")]
        public int NCID { get; set; }

        [Required]
        [DisplayName("Acceptable Value")]
        public decimal CValue { get; set; }

        public Nullable<double> Weightage { get; set; }

        [Required]
        [DisplayName("Owner Name")]
        public string OName { get; set; }

        [Required]
        [DisplayName("Owner Address")]
        public string OAdd { get; set; }

        [Required]
        [DisplayName("Contact No")]
        [Range(0, 99999999999, ErrorMessage = "Please enter your valid Contact Number")]
        public string contactNo { get; set; }

        [Required]
        [DisplayName("Citizenship No")]
        public string citizenshipNo { get; set; }

        [Required]
        [DisplayName("Registration No.")]
        public string RegNo { get; set; }
        [Required]
        [DisplayName("Registration Date")]
        public DateTime? RegDate { get; set; }
        [DisplayName("Remarks")]
        public string Remarks { get; set; }
        public bool IsAc { get; set; }
        public bool IsActive { get; set; }
        public int PostedBy { get; set; }
        public System.DateTime PostedOn { get; set; }
        public bool IsInternal { get; set; }

        public string AccountNumber { get; set; }
        public string AccountName { get; set; }

        public string CollateralName { get; set; }

        public int LandId { get; set; }
        public int VehicleId { get; set; }
        public int fixedDepoId { get; set; }
        public IPagedList<ALCollModel> CollList { get; set; }
        public int TotalCount { get; set; }
        public List<GuarantorModel> guarantorModel { get; set; }
    }
    public class ALCollLandModel
    {
        public int AcolLandId { get; set; }
        public int Sno { get; set; }
        [Required]
        [DisplayName("Father's Name")]
        public string Fname { get; set; }
        [Required]
        [DisplayName("Grandfrather's Name")]
        public string Gname { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]

        public string Area { get; set; }
        [Required]
        public string Direction { get; set; }
        [Required]
        [DisplayName("Seat No")]
        public string Seatno { get; set; }
        [Required]
        [DisplayName("Kitta No")]
        public string Kittano { get; set; }
        [Required]
        [DisplayName("Building Area")]
        public string BArea { get; set; }
        public int PostedBy { get; set; }
        [Required]
        [DisplayName("Acceptable Value")]
        public decimal Amount { get; set; }
        public System.DateTime PostedOn { get; set; }
    }
    public class ALCollVehicleModel
    {
        public int ColVechicleId { get; set; }
        public int Sno { get; set; }
        [DisplayName("License  No")]
        [Required]
        public string LicNo { get; set; }
        [DisplayName("License  Right")]
        public string LicRight { get; set; }
        [DisplayName("Issue  Office")]
        [Required]
        public string IssueOff { get; set; }
        [DisplayName("Model No")]
        [Required]
        public string ModNo { get; set; }
        [DisplayName("Chasis No")]
        public string ChassisNo { get; set; }
        [DisplayName("Engine No")]
        public string EngNo { get; set; }
        [Required]
        public string Color { get; set; }
        [DisplayName("Cubic Capacity")]

        public string CC { get; set; }
        [DisplayName("MDF Company")]
        public string MC { get; set; }
        [DisplayName("MDF Date")]
        public string MD { get; set; }
        [DisplayName("Vehicle Type")]
        [Required]
        public byte type { get; set; }
        [DisplayName("Description")]
        public string description { get; set; }
        [Required]
        [DisplayName("Acceptable Value")]
        public decimal Amount { get; set; }
        public int PostedBy { get; set; }
        public System.DateTime PostedOn { get; set; }
        [Required]
        [DisplayName("Vehicle No")]
        public string VehicleNo { get; set; }
    }
    public class ALFixedDepositModel
    {
        public int AlFixedId { get; set; }
        public int Sno { get; set; }
        public bool IsInternalAccount { get; set; }
        [Required]
        [DisplayName("Guarantee A/C")]
        public string fdAccno { get; set; }
        [Required]
        [DisplayName("Quation Percent")]
        public decimal QuationPer { get; set; }
        [Required]
        [DisplayName("Equivalent Amount")]
        public Nullable<decimal> amount { get; set; }
        [Required]
        [DisplayName("Account Open Date")]
        public Nullable<System.DateTime> openDt { get; set; }
        [Required]
        [DisplayName("Maturation  Date")]
        public DateTime? matDt { get; set; }
        [Required]
        [DisplayName("Bank  Name")]
        public string bank { get; set; }
        [Required]
        [DisplayName("Branch")]
        public string brnh { get; set; }
        public int PostedBy { get; set; }
        [Required]
        public decimal? Balance { get; set; }
        public System.DateTime PostedOn { get; set; }
        public string AccountNumber { get; set; }
        public LoanRegAccOpenViewModel a { get; set; }
        public List<GuarantorModel> guarantorModel { get; set; }

    }
    public class LoanRegAccOpenViewModel
    {
        public LoanRegAccOpenViewModel()
        {
            GuarantorModel = new GuarantorModel();
        }
        public int RegId { get; set; }
        [DisplayName("Product")]
        public Int32 PID { get; set; }
        [DisplayName("Quotation Amount")]

        public decimal LAmt { get; set; }
        public int Duration { get; set; }

        [Required]
        public int GrantedDuration { get; set; }
        public byte Status { get; set; }
        public System.DateTime RegistrationDate { get; set; }

        [Required]
        [DisplayName("Sanction Amount")]

        public Nullable<decimal> SAmt { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> iAccno { get; set; }
        [Required]
        [DisplayName("Payment Type")]
        public byte PAYID { get; set; }
        [Required]
        [DisplayName("Schedule Type")]
        public byte PAYSID { get; set; }
        [Required]
        [DisplayName("Principle Frequency")]
        public int PFreq { get; set; }
        [Required]
        [DisplayName("Interest Frequency")]
        public int IFreq { get; set; }
        public Nullable<byte> cDay { get; set; }

        public Nullable<decimal> OthrBal { get; set; }
        public Nullable<decimal> IonPA { get; set; }
        public Nullable<decimal> IonPR { get; set; }
        public Nullable<decimal> IonCA { get; set; }
        public Nullable<decimal> IonCR { get; set; }
        public Nullable<decimal> PonPrA { get; set; }
        public Nullable<decimal> PonPrR { get; set; }
        public Nullable<decimal> PonIA { get; set; }
        public Nullable<decimal> PonIR { get; set; }
        public Nullable<decimal> IonIR { get; set; }
        public Nullable<decimal> IonIA { get; set; }
        public bool IsInsured { get; set; }
        public bool AutoPayment { get; set; }
        public bool overPay { get; set; }
        public Nullable<short> penGDys { get; set; }
        public Nullable<bool> Revolving { get; set; }
        public Nullable<bool> deprived { get; set; }
        public int PostedBy { get; set; }
        public Nullable<int> ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedOn { get; set; }
        [Required]
        [Display(Name = "Customer")]
        public IEnumerable<int> CustomerId { get; set; }
        [Required(ErrorMessage = "Account Name is required ")]
        [Display(Name = "Account Name")]
        public string Aname { get; set; }
        public int isAccepted { get; set; }
        public int LinkAccountNumber { get; set; }
        public int InCalRuleId { get; set; }

        [Required]
        public decimal InterestRate { get; set; }

        [Required]
        public decimal PrincipalPenaltyRate { get; set; }

        [Required]
        public decimal InterestPenaltyRate { get; set; }
        public byte PenaltyCalculation { get; set; }
        public AccountDetailsViewModel AccountDetailsViewModel { get; set; }
        public AccountsViewModel AccountsViewModel { get; set; }
        public LoanLinkedAccounts LinkedAccounts { get; set; }
        public List<LoanLinkedAccounts> LinkedAccountsList { get; set; }
        public ScheduleTrialModel ScheduleTrialModel { get; set; }
        [Display(Name = "Maturation Date")]
        public DateTime MaturationDate { get; set; }
        public bool DateType { get; set; }
        public int isAfterRegistration { get; set; }
        public LoanProductDetails LoanProductDetails { get; set; }
        public SchemeModel SchemeModel { get; set; }
        public bool IsChargeAvailable { get; set; }

        public GuarantorModel GuarantorModel { get; set; }
        [Display(Name = "Fixed Deposit Account No.")]
        public int FDLoanAcId { get; set; }

        //    [Required]
        public string FDLoanAcc { get; set; }
        public bool flag { get; set; }
        public decimal MinValue { get; set; }


    }
    public class LoanLinkedAccounts
    {
        public int LinkIaccno { get; set; }
        public int Priority { get; set; }
        public string LAccountName { get; set; }
        public string LAccountNumber { get; set; }
        public bool isDetailView { get; set; }

    }
    public class LoanAttributes
    {
        public bool IsInsured { get; set; }
        public bool AutoPayment { get; set; }
        [Display(Name = "Allow Pre-Payment")]
        public bool overPay { get; set; }
        public bool Revolving { get; set; }
        [Display(Name = "Deprived")]
        public bool deprived { get; set; }
        public bool HasCheque { get; set; }
        public bool HasCard { get; set; }
        public bool HasIndLimit { get; set; }
        public bool HasIndRate { get; set; }
        public bool HasSMS { get; set; }
        public int Durtype { get; set; }
    }
    public class InterestCalculation
    {
        public byte InCalRuleId { get; set; }

        public string InterestCalRule { get; set; }

    }
    public class InterestCapitalisation
    {
        public byte PAYID { get; set; }
        public string PRule { get; set; }
        public bool active { get; set; }

    }
    public class PenaltyCalculation
    {
        public byte PCID { get; set; }

        public string PCNAME { get; set; }

    }
    public class LoanProductDetails
    {
        public byte SDID { get; set; }
        public int PID { get; set; }
        public int FID { get; set; }
        public string PName { get; set; }
        public Nullable<float> Duration { get; set; }
        public bool HasOverdraw { get; set; }
        public bool HasCheque { get; set; }
        public bool HasCard { get; set; }
        public bool HasCertificate { get; set; }
        public bool HasIndivLimit { get; set; }
        public bool HasIndDuration { get; set; }
        public Nullable<byte> DormantPeriod { get; set; }
        public bool HasIndivRate { get; set; }
        public Nullable<float> IRate { get; set; }
        public Nullable<float> OIRate { get; set; }
        public Nullable<float> PPRate { get; set; }
        public Nullable<float> PIRate { get; set; }
        public byte MID { get; set; }
        public Nullable<byte> RNWID { get; set; }
        public decimal LAmt { get; set; }
        public Nullable<float> PGRACE { get; set; }
        public Nullable<float> ODuration { get; set; }
        public bool Nomianable { get; set; }
        public bool enabled { get; set; }
        public Nullable<bool> MultiDep { get; set; }
        public Nullable<bool> Withdrawal { get; set; }
        public Nullable<bool> IntraBrnhTrnx { get; set; }
        public string Apfix { get; set; }
        public Nullable<decimal> aCtr { get; set; }
        public bool IsInsured { get; set; }
        public Nullable<int> NSId { get; set; }
        public Nullable<bool> Schedule { get; set; }
        public Nullable<short> penGDys { get; set; }
        public Nullable<byte> durState { get; set; }
        public Nullable<bool> Revolving { get; set; }
        public Nullable<bool> HASSMS { get; set; }
        public List<InterestCapitalisation> InterestCapitalisation { get; set; }
        public List<PenaltyCalculation> PenaltyCalculationList { get; set; }
        public bool IsChargeAvailable { get; set; }
        public bool IsFDLoan { get; set; }
    }

    public class LoanDisbursement
    {
        public int TotalCount { get; set; }
        [DisplayName("Account Number")]
        public int AccountId { get; set; }
        public int DisburseId { get; set; }
        public int Mode { get; set; }
        public Nullable<int> VNo { get; set; }
        public System.DateTime PostedOn { get; set; }
        public Nullable<System.DateTime> PostedBy { get; set; }
        public Nullable<System.DateTime> ApprovedOn { get; set; }
        public string Remarks { get; set; }
        public bool Cash { get; set; }
        public bool Bank { get; set; }
        public bool Deposit { get; set; }
        public decimal ApprovedAmount { get; set; }
        [Required]
        public decimal RegularLoan { get; set; }
        [Required]
        public decimal OtherLoan { get; set; }
        public decimal DisbursableAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public string AccountNumber { get; set; }
        public Nullable<Decimal> AddtionalCharge { get; set; }
        public Nullable<Decimal> Charges { get; set; }
        public int FID { get; set; }
        public DisburseCashDepositViewModel DisburseCash { get; set; }
        public DisburseChargeDepositViewModel DisburseCharge { get; set; }

        public DisburseDepositViewModel DisburseDeposit { get; set; }
        public DisburseChequeDepositViewModel DisburseCheque { get; set; }
        public ProductViewModel Product { get; set; }
        public bool IsChargeAvailable { get; set; }
        public bool IsOtherLoan { get; set; }
        public int PrevChargeDeductMode { get; set; }
        public bool ChargeDeductOnSanction { get; set; }
        public bool ChargeDeductOnDisburse { get; set; }
        public bool HasCustomisedSchedule { get; set; }
        public bool IsRevolving { get; set; }
        public IPagedList<LoanDisbursement> LoanDisbursementList { get; set; }
    }

    public class DisburseCashDepositViewModel
    {
        public int DisCashId { get; set; }
        public int DisburseId { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<int> RecieveFrom { get; set; }

        public Nullable<long> TNo { get; set; }
    }
    public class DisburseChargeDepositViewModel
    {
        public int DisChargeId { get; set; }
        public Nullable<int> DisburseId { get; set; }
        public Nullable<int> FID { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<int> IAccno { get; set; }
        public Nullable<int> Vno { get; set; }
        public string Remarks { get; set; }
        public List<DisburseChargeDepositViewModel> AddtionalChargeList { get; set; }

    }
    public class DisburseChequeDepositViewModel
    {
        public int DisChequeId { get; set; }
        public int DisburseId { get; set; }
        public string ChequeNo { get; set; }
        public int BankId { get; set; }
        public string BankName { get; set; }
        public Nullable<decimal> Amount { get; set; }
    }
    public class DisburseDepositViewModel
    {
        public int DisDepositId { get; set; }
        public int DisburseId { get; set; }
        public int DepositIaccno { get; set; }
        public string DepositAccounNumber { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string AccName { get; set; }
        public decimal GoodsBalance { get; set; }
        public List<DisburseDepositViewModel> DisburseDepositList { get; set; }
    }
    public class DisburseShareViewModel
    {

        public int DisburseId { get; set; }
        public int RegNo { get; set; }
        public decimal SQty { get; set; }
        public decimal Rate { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string Name { get; set; }
        public decimal ShareBalance { get; set; }

        public List<DisburseShareViewModel> DisburseShareList { get; set; }
    }

    public class LoanPaymentInformationModel
    {

        public DateTime? DueDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal Installment { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal Mature { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal PerDayInterest { get; set; }

        public DateTime MetureDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal Other { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal CurrentInterest { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal PrincipalAmount { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal TotalInstallMentAmount { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal ToPay { get; set; }

        public DateTime? MatureDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal OutstatdingPrincipal { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal Rebate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal Balance { get; set; }
        public List<LoanPaymentInformationModel> LoanPaymentList { get; set; }
    }

    public class LoanPaymentModel
    {
        public int IAccno { get; set; }
        [DisplayName("Pre Payment Mode")]
        public int PrePaymentMode { get; set; }

        [DisplayName("Extra Interest")]
        public decimal ExtraInterest { get; set; }

        [DisplayName("Payment Interest")]
        public decimal PaymentInterest { get; set; }
        [DisplayName("Notes")]
        public string Notes { get; set; }

        [DisplayName("Rebate")]
        public decimal Rebate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        [DisplayName("Payment")]
        [Required(ErrorMessage = "Required Feild")]
        public decimal Payment { get; set; }
        public DateTime Date { get; set; }
        public bool ReadyToClose { get; set; }
        [DisplayName("Manual Distribute")]
        public bool ManualDistribute { get; set; }

        //first column opther
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public Nullable<decimal> IonCA { get; set; }//Interest
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public Nullable<decimal> IonCR { get; set; }//Income Interest
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public Nullable<decimal> OthrBal { get; set; }//Os
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal OtherIonCARebate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal OtherIonCRRebate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal OtherOthrBalRebate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal OtherIonCAPayment { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal OtherIonCRPayment { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal OtherOthrBalPayment { get; set; }

        //penalty ANBR
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public Nullable<decimal> PonIR { get; set; }//Interest
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public Nullable<decimal> PonPrR { get; set; }//Principal
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public Nullable<decimal> IonIR { get; set; }//Interest on interest
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal PenaltyAnbrPonIRRebate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal PenaltyAnbrPonPrRRebate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal PenaltyAnbrIonIRRebate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal PenaltyAnbrPonIRPayment { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal PenaltyAnbrPonPrRPayment { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal PenaltyAnbrIonIRPayment { get; set; }

        //penalty Income
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public Nullable<decimal> PonIA { get; set; }//Income Interest
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public Nullable<decimal> PonPrA { get; set; }//Income Principal
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public Nullable<decimal> IonIA { get; set; }//Interest On Interest

        public decimal PenaltyIncomePonIARebate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal PenaltyIncomePonPrARebate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal PenaltyIncomeIonIARebate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal PenaltyIncomePonIAPayment { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal PenaltyIncomePonPrAPayment { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal PenaltyIncomeIonIAPayment { get; set; }
        //Interest
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public Nullable<decimal> IonPA { get; set; }//ANRB  Principal
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public Nullable<decimal> IonPR { get; set; }//Income Principal
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal InterestIonPARebate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal InterestIonPRRebate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal InterestIonPAPayment { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal InterestIonPRPayment { get; set; }

        //Principal
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal MaturePA { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal CurentPA { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal PrincipalMaturRebate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal PrincipalMaturPayment { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal PrincipalCurrentRebate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal PrincipalCurrentPayment { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal PrincipalPrePayment { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal InterestPrePayment { get; set; }
        public decimal RemainingIonCA { get; set; }
        public decimal RemainingIonCR { get; set; }
        public decimal RemainingOthrBal { get; set; }
        public decimal RemainingPonIR { get; set; }
        public decimal RemainingPonPrR { get; set; }
        public decimal RemainingIonIR { get; set; }
        public decimal RemainingPonIA { get; set; }
        public decimal RemainingPonPrA { get; set; }
        public decimal RemainingIonIA { get; set; }
        public decimal RemainingIonPA { get; set; }
        public decimal RemainingIonPR { get; set; }
        public decimal RemainingMaturePA { get; set; }
        public decimal RemainingCurentPA { get; set; }

        public decimal TotalBalance { get; set; }

        public bool IsMature { get; set; }
    }

    public class MaturePrincipalInterestModel
    {
        public double MaturePrincipal { get; set; }
        public double CurrentPrincipal { get; set; }
        public DateTime CurrentDate { get; set; }
        public double FatureInterest { get; set; }
        public double CurrentInterest { get; set; }
    }
    public class NonCashLoanPayment
    {
        public int ShareIaccno { get; set; }
        public int DepositIaccno { get; set; }
        public int OtherIaccno { get; set; } 

        public decimal ShareAmount { get; set; }
        public decimal DepositAmount { get; set; }
        public decimal OtherAmount { get; set; }
    }
    public class LoanRebateModel
    {
        public int IAccno { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public Nullable<decimal> InterestOnOther { get; set; }//Interest on Other


        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public Nullable<decimal> IntrestOnInterest { get; set; }//Interest on Interest


        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public Nullable<decimal> PenaltyOnPrincipal { get; set; }//Penalty on Principal

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public Nullable<decimal> PenaltyOnInterest { get; set; }//Penalty on Interest


        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public Nullable<decimal> InterestOnPrincipal { get; set; }//Interest on Principal

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public Nullable<decimal> RebateInterestOnOther { get; set; }//Rebate Interest on Other


        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public Nullable<decimal> RebateIntrestOnInterest { get; set; }//Rebate Interest on Interest


        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public Nullable<decimal> RebatePenaltyOnPrincipal { get; set; }//Rebate Penalty on Principal

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public Nullable<decimal> RebatePenaltyOnInterest { get; set; }//Rebate Penalty on Interest


        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public Nullable<decimal> RebateInterestOnPrincipal { get; set; }//Rebate Interest on Principal

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public Nullable<decimal> OldPrincipalOut { get; set; }//Old Principal Out

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public Nullable<decimal> NewPrincpalOut { get; set; }//New Principal Out


        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public Nullable<decimal> OldOtherBalance { get; set; }//Old Principal Out

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public Nullable<decimal> NewOtherBalance { get; set; }//Old Principal Out


        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public Nullable<decimal> OldLinkBalance { get; set; }//Old Principal Out

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public Nullable<decimal> NewLinkBalance { get; set; }//Old Principal Out

        public int OldLinkAccount { get; set; }
    }
}
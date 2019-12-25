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
    public class DenoInViewModel
    {
        public decimal Tno { get; set; }
        public System.DateTime vdate { get; set; }
        public int DenoID { get; set; }
        public short CurrID { get; set; }

        [DisplayName("Figure")]
        public double Deno { get; set; }
        public Nullable<int> DenoBalId { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> Piece { get; set; }
        public Nullable<int> UserLevel { get; set; }
        public long Id { get; set; }
        public int DenoNumber { get; set; }
        public decimal DenoNumberTotal { get; set; }

        
        public string Currency { get; set; }

        public List<DenoInViewModel> DenoInList { get; set; }
    }
    public class DenoOutViewModel
    {
        public decimal Tno { get; set; }
        public System.DateTime vdate { get; set; }
        public int DenoIDOut { get; set; }
        public short CurrIDOut { get; set; }

        public double DenoOut { get; set; }

        public int? DenoBalIdOut { get; set; }
        public int? UserIdOut { get; set; }

        public int? PieceOut { get; set; }
        public int? UserLevelOut { get; set; }

        public int DenoNumberOut { get; set; }
        public decimal DenoNumberTotalOut { get; set; }
        public List<DenoOutViewModel> DenoOutList { get; set; }
    }

    public class DenoInOutViewModel
    {
        public List<DenoOutViewModel> DenoOutList { get; set; }
        public List<DenoInViewModel> DenoInList { get; set; }

        public decimal BalanceIn { get; set; }
        public decimal BalanceOut { get; set; }
        public decimal Balance { get; set; }
        public Guid TrancactionId { get; set; }
        public decimal TransactionAmount { get; set; }

        public bool IsTransactionWithDeno { get; set; }

        public string DenoInOut { get; set; }
    }

    public class TempTransViewMOdel
    {
        public int DenoTransId { get; set; }
        public string TransactionId { get; set; }
        public int? DenoIn { get; set; }
        public int? DenoOut { get; set; }
        public int UserId { get; set; }
        public int UserLevel { get; set; }
        public int DenoID { get; set; }

        public string DenoInOut { get; set; }
    }

    public class DepositViewModel
    {
        [DisplayName("Account Number")]
        [Required]
        public int AccountId { get; set; }
        [DisplayName("Receipt No")]
        [Required]
        public int ReceiptNo { get; set; }
        [DisplayName("Deposit By")]
        [Required]
        public string DepositBy { get; set; }
        [DisplayName("Currency")]
        [Required]
        public int AmountCurrency { get; set; }

        [DisplayName("Amount")]
        [Required]
        public decimal Amount { get; set; }
        [DisplayName("ExchangeRate")]
        public string ExchangeRate { get; set; }
        [DisplayName("Rate")]

        public string Rate { get; set; }
        public byte TType { get; set; }
    }
    public class WithdrawViewModel
    {
        [DisplayName("Account Number")]
        [Required]
        public int AccountId { get; set; }
        [Required]
        public int SlipNo { get; set; }
        [DisplayName("Payee")]
        [Required]
        public string Payee { get; set; }
        [DisplayName("Currency")]

        public int AmountCurrency { get; set; }
        [Required]
        [DisplayName("Amount")]
        public decimal Amount { get; set; }
        [DisplayName("ExchangeRate")]
        public string ExchangeRate { get; set; }

        [DisplayName("Cheque No.")]

        [Required]
        public int ChequeNumber { get; set; }

        [DisplayName("Rate")]
        public string Rate { get; set; }
        public string Remarks { get; set; }
        [Required]
        public string ChqBounceReason { get; set; }
        [Required]
        public int BounceChequeNumber { get; set; }
        public string withdraw { get; set; }

        public bool IsActiveWithdraw { get; set; }
        public string BranchName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public decimal Tno { get; set; }
        public DateTime TransDate { get; set; }
        public bool IsBounce { get; set; }
        public byte TType { get; set; }
        public DateTime tdate { get; set; }
        public bool IsSlp { get; set; }
        public bool HasCustomisedSchedule { get; set; }
        public bool UserTellerLimit { get; set; }
        public List<WithdrawViewModel> WithdrawNoPageList { get; set; }
        public IPagedList<WithdrawViewModel> WithdrawList { get; set; }
    }
    public class InterestPayableViewModel
    {
        [DisplayName("Amount")]
        [Required]
        public decimal InterestAmount { get; set; }
        [Required]
        [DisplayName("Payee")]
        public string InterestPayee { get; set; }
        [Required]
        [DisplayName("Slip No")]
        public int InterestSlipNo { get; set; }

        public DateTime TransactionDate { get; set; }
        public Int64 Tno { get; set; }

        public string AccountName { get; set; }

        public string AccountNumber { get; set; }
        public bool IsActiveInterest { get; set; }
    }

    public class AccountDetailsShowViewModel
    {
        public int IAccno { get; set; }
        public int CTId { get; set; }
        public int BrchID { get; set; }
        public System.DateTime RDate { get; set; }
        public string Prefix { get; set; }
        public int PID { get; set; }
        public string AccountTitle { get; set; }
        public string AccountState { get; set; }
        public string ProductName { get; set; }
        public string SchemeName { get; set; }
        public Nullable<bool> AllowABBS { get; set; }
        public bool IsOD { get; set; }
        public string IntCalSchm { get; set; }
        public decimal IRate { get; set; }
        public bool Nomianable { get; set; }
        public Nullable<decimal> BalToMan { get; set; }
        public string IntCalschmDr { get; set; }
        public string BranchName { get; set; }
        public byte Subsidiary { get; set; }
        public string AccounNumber { get; set; } //changed here int to string ---- by dorje
        public string ShowType { get; set; }
        public decimal Balance { get; set; }    //after function alter in db
        public DateTime? LastTransactionDate { get; set; }  //after function alter in db
        public List<CustomerAccountsViewModel> customerAddress { get; set; }
        public List<AccountBalanceViewModel> AccountBalance { get; set; }
        public LoanAccountDetailsShowModel LoanAccountDetails { get; set; }

    }
    public class AccountBalanceViewModel
    {
        public string FlagName { get; set; }
        public decimal Amount { get; set; }

        public int FlagId { get; set; }
    }

    public class CurrencyRateViewModel
    {
        public bool IsTransWithDeno { get; set; }
        public decimal RatePerUnit { get; set; }
        public decimal BuyingRate { get; set; }
        public decimal Srate { get; set; }


    }
    public class UttnoModel
    {
        public Int64 uttno { get; set; }
        public Int64? Tno { get; set; }
    }
    public class ASTrnViewModel
    {
        public decimal tno { get; set; }
        public string accountnumber { get; set; }
        public System.DateTime tdate { get; set; }
        public string notes { get; set; }
        public Nullable<int> slpno { get; set; }
        public decimal dramt { get; set; }
        public decimal cramt { get; set; }
        public string ttype { get; set; }
        public string postedby { get; set; }
        public Nullable<decimal> VNO { get; set; }
        public Nullable<bool> IsSlp { get; set; }
        public int IAccno { get; set; }
    

        public string currency { get; set; }

        public byte AccountType { get; set; }
        public int branchId { get; set; }
        public string ProductName { get; set; }
        public byte PID { get; set; }
        public string AccountName { get; set; }
        public string flag { get;set; }
        public IPagedList<ASTrnViewModel> transactionList { get; set; }
        public List<ASTrnViewModel> AstransactionList { get; set; }
        public List<ASTransLoanModel> ASTransLoanList { get; set; }
        public int TotalCount { get; set; }
    }
    public  class ASTransLoanModel
    {
        public decimal AstransLoanId { get; set; }
        public decimal tno { get; set; }
        public byte PId { get; set; }
        public decimal Amt { get; set; }
        public Nullable<decimal> vNO { get; set; }
    }
    public class CheckChequeNo
    {
        public int IAccno { get; set; }
        public int ChqNo { get; set; }

    }

    public class InterestPayableModel
    {
        public bool IsNomee { get; set; }
        public decimal Amount { get; set; }
    }

    public class AWtdQueueModel
    {
        public int Rno { get; set; }
        public int IAccno { get; set; }
        public System.DateTime tdate { get; set; }
        public decimal chqno { get; set; }
        public string notes { get; set; }
        public decimal amount { get; set; }
        public Nullable<int> ttype { get; set; }
        public int postedby { get; set; }
        public Nullable<int> Approvedby { get; set; }
        public int BrnhID { get; set; }
        public Nullable<int> currid { get; set; }
        public bool isslip { get; set; }
        public Nullable<decimal> tno { get; set; }
        public byte Qstate { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }

        public int EventId { get; set; }
        public int TaskId { get; set; }

        public string VerifyBy { get; set; }

        public string Type { get; set; }
        public IPagedList<AWtdQueueModel> TellerExceedList { get; set; }

    }
    public class LoanAccountDetailsShowModel
    {

        public int IAccno { get; set; }
        public Nullable<decimal> Days { get; set; }
        public Nullable<int> Month { get; set; }
        public int DurType { get; set; }
        public decimal SancationAmount { get; set; }
        public decimal PrincipleLoanOut { get; set; }
        public Nullable<bool> Revolving { get; set; }
        public int InterestFrequency { get; set; }
        public int PrincipalFrequency { get; set; }
        public Nullable<System.DateTime> MaturityDate { get; set; }
        public string PayScheme { get; set; }
        public string PayRule { get; set; }
    }
    public class ChangeProductInterestViewModel
    {
        public int PID { get; set; }
        public string ProductType { get; set; }
        public string ProductName { get; set; }
        public DateTime InterestChangeDate { get; set; }
        public int Duration { get; set; }
        public decimal Contribution { get; set; }
        public int Basic { get; set; }
        public decimal CurrentRate { get; set; }
        public float NewRate { get; set; }
        public int InterestCapitalization { get; set; }
        public float PenaltyInterestRate { get; set; }
        public float OIRate { get; set; }
        public float PrinciplePenaltyRate { get; set; }
        public CheckProductTypeModel ChkProductType { get; set; }
        //public AccountDetailsViewModel accountDetailsViewModel { get; set; }
    }

}
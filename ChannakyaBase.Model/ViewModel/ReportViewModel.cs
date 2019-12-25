using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ChannakyaBase.Model.ViewModel
{
    public class AccountReportViewModel
    {
        public int PID { get; set; }
        [DisplayName("Account Number")]
        public int AccountId { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        [DisplayName("Product Name")]
        public string ProductName { get; set; }
        [DisplayName("Register Date")]
        public Nullable<System.DateTime> RegisterDate { get; set; }
        [DisplayName("Closed on")]
        public Nullable<System.DateTime> ClosingDate { get; set; }
        public Nullable<int> BranchId { get; set; }
        public short AccountStatus { get; set; }
        [DisplayName("Account State")]
        public string AccountState { get; set; }
        public byte AccountType { get; set; }
        [DisplayName("User")]
        public string AccountOpenBy { get; set; }
        [DisplayName("Account Operating")]
        public string AccountOperating { get; set; }
        public string AccountClosedBy { get; set; }
        [DisplayName("Currency Symbol")]
        public string CurrencySymbol { get; set; }
        public int TotalCount { get; set; }
        [Required]
        public DateTime FromDate { get; set; }
        [Required]
        public DateTime ToDate { get; set; }


        public DateTime? ChangeOn { get; set; }

        public DateTime MatureDate { get; set; }
        public decimal Balance { get; set; }

        public Byte SType { get; set; }
        public string ViewForm { get; set; }

        public List<AccountReportViewModel> AccountDetailsList { get; set; }
        public IPagedList<AccountReportViewModel> AccountReportList { get; set; }

    }

    public class MatureDurationWiseAccountExportToExcelViewModel
    {
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string ProductName { get; set; }
        public Nullable<System.DateTime> RegisterDate { get; set; }
        public DateTime MatureDate { get; set; }
        public decimal Balance { get; set; }
    }
    public class AccountReportExportToExcelViewModel
    {

        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string ProductName { get; set; }
        public Nullable<System.DateTime> RegisterDate { get; set; }
        public string AccountStatus { get; set; }
        public DateTime? ChangeOn { get; set; }
        public string AccountClosedBy { get; set; }
    }

    public class DenoTransactionViewModel
    {
        public int Denoid { get; set; }
        public string CurrencyName { get; set; }
        public int UserId { get; set; }
        public string EmployeeName { get; set; }
        public string UserName { get; set; }
        public double Deno { get; set; }
        public int Pics { get; set; }
        public decimal Amount { get; set; }
        public int CurrID { get; set; }
        public long TransactionNumber { get; set; }
        public DateTime Date { get; set; }
        public List<DenoTransactionViewModel> DenoReportList { get; set; }
    }

    public class DenoTransactionByUserExportToExcelViewModel
    {
        public double Deno { get; set; }
        public int Pics { get; set; }
        public decimal Amount { get; set; }
    }
    public class UserReportViewModel
    {
        public Nullable<decimal> Brhid { get; set; }
        public Nullable<decimal> ReportUserId { get; set; }
        public Nullable<int> UserId { get; set; }
        public decimal Tno { get; set; }
        public System.DateTime TDate { get; set; }
        public string Tdesc { get; set; }
        public Nullable<int> TType { get; set; }
        public int Currid { get; set; }
        public decimal Dramt { get; set; }
        public Nullable<decimal> Cramt { get; set; }
        public Nullable<int> Iaccno { get; set; }
        public decimal Amount { get; set; }
        public int FUserid { get; set; }
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public Nullable<byte> Status { get; set; }
        public Nullable<decimal> Vno { get; set; }
        public bool UserState { get; set; }
        public Nullable<decimal> CurrentBalance { get; set; }
        public int DesignationId { get; set; }
        public string DGName { get; set; }
        public string EmployeeName { get; set; }
        public int EmployeeId { get; set; }
        public int DepartmentId { get; set; }
        public int PId { get; set; }

        public SelectList UserList { get; set; }
        public int operationType { get; set; }
        public string ActionType { get; set; }

    }

    public class UserReportExportToExcel
    {
        public string AccountNo { get; set; }
        public string EmployeeName { get; set; }
        public decimal Dramt { get; set; }
        public Nullable<decimal> Cramt { get; set; }
        public string Tdesc { get; set; }
        public decimal Tno { get; set; }
    }
    public class DepositStatementViewModel
    {
        public int Iaccno { get; set; }
        public string Accno { get; set; }
        public string OldAccno { get; set; }
        public string Aname { get; set; }
        public string ProductName { get; set; }
        public string SchemeName { get; set; }
        public Nullable<float> InterestRate { get; set; }
        public System.DateTime RegisteredDate { get; set; }
        public Nullable<System.DateTime> MaturedDate { get; set; }
        public decimal IonBal { get; set; }
        public decimal ClosingBalance { get; set; }
        public decimal AccuredInterest { get; set; }
        public int AccountId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public Nullable<decimal> ReportUserId { get; set; }
        public Nullable<int> UserId { get; set; }
        public string Tdesc { get; set; }
        public Nullable<int> Tno { get; set; }
        public Nullable<System.DateTime> Tdate { get; set; }
        public Nullable<System.DateTime> Vdate { get; set; }
        public string Description { get; set; }
        public Nullable<decimal> Debit { get; set; }
        public Nullable<decimal> Credit { get; set; }
        public Nullable<decimal> Balance { get; set; }
        public string DC { get; set; }
        public int TotalDebitCount { get; set; }
        public int TotalCreditCount { get; set; }
        public decimal TotalDebited { get; set; }
        public decimal TotalCredited { get; set; }
        public bool IsMarked { get; set; }
        public List<DepositStatementViewModel> DepositStatementViewModelList { get; set; }
        public SelectList MarkDateList { get; set; }
        public string MarkDateSelect { get; set; }

    }

    public class DepositStatementexportToExcelViewModel
    {
        //public string Accno { get; set; }
        //public System.DateTime RegisteredDate { get; set; }
        //public Nullable<float> InterestRate { get; set; }
        //public decimal IonBal { get; set; }
        //public string Aname { get; set; }
        //public decimal AccuredInterest { get; set; }
        //public string ProductName { get; set; }
        //public string SchemeName { get; set; }
        public Nullable<System.DateTime> Tdate { get; set; }
        public Nullable<System.DateTime> Vdate { get; set; }
        public string Description { get; set; }
        public string Debit { get; set; }
        public string Credit { get; set; }
        public Nullable<decimal> Balance { get; set; }
        public string DC { get; set; }
    }

    public class CashFlowViewModel
    {
        public int UserId1 { get; set; }
        public int UserId2 { get; set; }
        public long Tno { get; set; }
        public DateTime TDate { get; set; }
        public string Tdesc { get; set; }
        public decimal Dramt { get; set; }
        public decimal Cramt { get; set; }
        public string Remarks { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string ProductName { get; set; }
        public decimal? Vno { get; set; }
        public Int16 CurrID { get; set; }
        public int branchId { get; set; }
        public List<CashFlowViewModel> CashFlowList { get; set; }
    }

    public class ProductSummaryExportToExcel
    {
        public string ProductName { get; set; }
        public decimal Dramt { get; set; }
        public decimal Cramt { get; set; }
        public decimal Balance { get; set; }
    }

    public class CashFlowExportToExcelViewModel
    {
        public string UserId1 { get; set; }
        public string UserId2 { get; set; }
        public string Tdesc { get; set; }
        public DateTime TDate { get; set; }
        public long Tno { get; set; }
        public decimal Dramt { get; set; }
        public decimal Cramt { get; set; }
        public string Remarks { get; set; }
    }
    public class ProductTransactionSummeryModel
    {
        public int PID { get; set; }
        public string ProductName { get; set; }
        public decimal CashInFlow { get; set; }
        public decimal NonCashInFlow { get; set; }
        public decimal CashOutFlow { get; set; }
        public decimal NonCashOutFlow { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int branchId { get; set; }

        public List<ProductTransactionSummeryModel> ProductSummeryList { get; set; }
    }
    public class ProductTransactionSummeryModelExportToExcel
    {
        public string ProductName { get; set; }
        public decimal CashInFlow { get; set; }
        public decimal NonCashInFlow { get; set; }
        public decimal CashOutFlow { get; set; }
        public decimal NonCashOutFlow { get; set; }
    }
    public class ChequeWithdewModel
    {
        public DateTime TransactionDate { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }

        public decimal Amount { get; set; }
        public string Notes { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int branchId { get; set; }
        public int ChequeNo { get; set; }
        public List<ChequeWithdewModel> ChequeWithdrawList { get; set; }
    }

    public class ChequeWithdrawViewModelExportToExcel
    {
        public string AccountNumber { get; set; }
        public DateTime TransactionDate { get; set; }
        public string AccountName { get; set; }
        public int ChequeNo { get; set; }
        public decimal Amount { get; set; }
        public string Notes { get; set; }
    }

    public class TransactionPayableModel
    {
        public byte PID { get; set; }
        public string Accno { get; set; }
        public int Iaccno { get; set; }
        public decimal IntAmt { get; set; }
        public Nullable<decimal> Tax { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public float TaxRt { get; set; }
        public System.DateTime Tdate { get; set; }
        public Nullable<decimal> DrAmt { get; set; }
        public string PName { get; set; }
        public string SDName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int branchId { get; set; }
        public decimal Balance { get; set; }
        public string AName { get; set; }
        public List<TransactionPayableModel> InterestList { get; set; }

    }

    public class TransactionPayableModelExportToExcel
    {
        public string PName { get; set; }
        public string Accno { get; set; }
        public string AName { get; set; }
        public System.DateTime Tdate { get; set; }
        public float TaxRt { get; set; }
        public Nullable<decimal> Tax { get; set; }
        public decimal IntAmt { get; set; }
        public Nullable<decimal> Amount { get; set; }
    }
    public class BalancePayableExportToExcel
    {
        public string PName { get; set; }
        public string Accno { get; set; }
        public string AName { get; set; }
        public decimal Balance { get; set; }
    }
    public class LoanTransactionModel
    {
        public int PID { get; set; }
        public decimal tno { get; set; }
        public DateTime vdate { get; set; }
        public decimal PriDr { get; set; }
        public decimal PriCr { get; set; }
        public decimal Interest { get; set; }
        public decimal POnPr { get; set; }
        public decimal POnInt { get; set; }
        public decimal IOnI { get; set; }
        public string AccountName { get; set; }
        public string ProductName { get; set; }
        public string Accno { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int branchId { get; set; }

        public List<LoanTransactionModel> LoanTransList { get; set; }
    }
    public class LoanTransactioExportExcelModel
    {
        public string ProductName { get; set; }
        public string Accno { get; set; }
        public string AccountName { get; set; }
        public DateTime vdate { get; set; }
        public decimal PriDr { get; set; }
        public decimal PriCr { get; set; }
        public decimal Interest { get; set; }
        public decimal POnPr { get; set; }
        public decimal POnInt { get; set; }
        public decimal IOnI { get; set; }

    }

    public class UnVerifiedTransactionModel
    {
        public Nullable<byte> PID { get; set; }
        public string ProductName { get; set; }
        public string accountnumber { get; set; }
        public string AccountName { get; set; }
        public Nullable<int> CurrID { get; set; }
        public System.DateTime tdate { get; set; }
        public string notes { get; set; }
        public decimal cramt { get; set; }
        public decimal dramt { get; set; }
        public int ttype { get; set; }
        public int? branchId { get; set; }
        public decimal? tno { get; set; }
        public string flag { get; set; }
        public int? SType { get; set; }
        public int AccountType { get; set; }
        public List<UnVerifiedTransactionModel> AstransactionList { get; set; }
    }
    public class InternalChequeDepositReportModel
    {
        public System.DateTime Tdate { get; set; }
        public string DrProduct { get; set; }
        public string CrProduct { get; set; }
        public string DrAccountNo { get; set; }
        public string DrName { get; set; }
        public string CrAccountNo { get; set; }
        public string CrName { get; set; }
        public long TrnxNo { get; set; }
        public decimal Amount { get; set; }
        public int Ttype { get; set; }
        public short BrchID { get; set; }

    }
    public class InternalChequeDepositExportExcelModel
    {
        public long TrnxNo { get; set; }
        public System.DateTime Tdate { get; set; }
        public string DrProduct { get; set; }
        public string CrProduct { get; set; }
        public string DrAccountNo { get; set; }
        public string DrName { get; set; }
        public string CrAccountNo { get; set; }
        public string CrName { get; set; }
        public decimal Amount { get; set; }

    }
    public class CommonReportModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int branchId { get; set; }
        public byte VerifyStatus { get; set; }
        public int IAccno { get; set; }
    }
    public class LoanBalanceTillDateModel
    {
        public int IAccNo { get; set; }
        public string Accno { get; set; }
        public string Aname { get; set; }
        public byte PID { get; set; }
        public string SDName { get; set; }
        public string PName { get; set; }
        public Nullable<decimal> PrinOut { get; set; }
        public Nullable<double> IntOut { get; set; }
        public Nullable<double> POnPrOut { get; set; }
        public Nullable<double> POnIOut { get; set; }
        public Nullable<double> IOnIOut { get; set; }
    }
    public class LoanBalanceTillDateExportToExcelViewModel
    {
        public string PName { get; set; }
        public Nullable<decimal> PrinOut { get; set; }
        public Nullable<double> IntOut { get; set; }
        public Nullable<double> POnPrOut { get; set; }
        public Nullable<double> POnIOut { get; set; }
        public Nullable<double> IOnIOut { get; set; }
    }

    public class IndividualLoanBalanceTillDateExportToExcelViewModel
    {
        public string Accno { get; set; }
        public string Aname { get; set; }
        public Nullable<decimal> PrinOut { get; set; }
        public Nullable<double> IntOut { get; set; }
        public Nullable<double> POnPrOut { get; set; }
        public Nullable<double> POnIOut { get; set; }
        public Nullable<double> IOnIOut { get; set; }
    }
    public class LoanAccruedInterestListingModel
    {
        public int IAccNo { get; set; }
        public string AccNo { get; set; }
        public string AName { get; set; }
        public decimal MAccured { get; set; }
        public decimal UMAccured { get; set; }
        public decimal TotalAmount { get; set; }
        public string ProductName { get; set; }

    }
    public class LoanAccruedInterestListingExcelExportModel
    {
        public string ProductName { get; set; }
        public string AccNo { get; set; }
        public string AName { get; set; }
        public decimal MAccured { get; set; }
        public decimal UMAccured { get; set; }
        public decimal TotalAmount { get; set; }

    }
    public class LoanFollowUpModel
    {
        public byte PID { get; set; }
        public string ProductName { get; set; }
        public int IAccno { get; set; }
        public string AccountNumber { get; set; }
        public byte BranchId { get; set; }
        public string AccountName { get; set; }



        public decimal OutStandingBalance { get; set; }
        public decimal MaturePrincipal { get; set; }
        public decimal InterestAccured { get; set; }

        public decimal Penalty { get; set; }
        public decimal Other { get; set; }
        public decimal OtherInterest { get; set; }
        public decimal Rate { get; set; }
        public int NoDays { get; set; }
        public decimal FutureInterest { get; set; }
        public decimal TotalDue { get; set; }
    }
    public class LoanFollowUpExcelExportModel
    {
        public string ProductName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public decimal OutStandingBalance { get; set; }
        public decimal MaturePrincipal { get; set; }
        public decimal InterestAccured { get; set; }
        public decimal Penalty { get; set; }
        public decimal Other { get; set; }
        public decimal OtherInterest { get; set; }
        public decimal Rate { get; set; }
        public int NoDays { get; set; }
        public decimal FutureInterest { get; set; }
        public decimal TotalDue { get; set; }
    }
    public class CheqqueIssueBounceWithdrawExportExcelModel
    {

        public System.DateTime tdate { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public int cfrom { get; set; }
        public int cto { get; set; }
        public string ChequeStatus { get; set; }

    }
    public class ChequeGoodsForPayementExportExcelModel
    {
        public decimal Tno { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public DateTime EngDate { get; set; }
        public int ChequeNumber { get; set; }
        public decimal Amount { get; set; }
        public string Payee { get; set; }
        public string notes { get; set; }

    }
    public class DepositBalanceTillDateAmountWiseModel
    {
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public Nullable<decimal> IntToExp { get; set; }
        public Nullable<decimal> IntToCap { get; set; }
        public Nullable<decimal> Balance { get; set; }

        public int BranchId { get; set; }
        public DateTime TDate { get; set; }
        public decimal FAmount { get; set; }
        public decimal ToAmount { get; set; }
        public int SortId { get; set; }
    }
    public class DepositBalanceTillDateAmountWiseExcelExportModel
    {
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public Nullable<decimal> IntToExp { get; set; }
        public Nullable<decimal> IntToCap { get; set; }
        public Nullable<decimal> Balance { get; set; }
    }
    public class LoanOutstandingModel
    {
        public string Accno { get; set; }
        public string Aname { get; set; }
        public string PName { get; set; }
        public Nullable<System.DateTime> ValDat { get; set; }
        public Nullable<System.DateTime> MatDat { get; set; }
        public bool Revolving { get; set; }
        public decimal DisbursedAmount { get; set; }
        public decimal OutstandingAmount { get; set; }
        public decimal RemainingToDisbursed { get; set; }
        public decimal AppAmt { get; set; }
        public decimal IRate { get; set; }
        public byte AccState { get; set; }

    }
    public class LoanOutstandingExcelExportModel
    {
        public string Accno { get; set; }
        public string Aname { get; set; }
        public string PName { get; set; }
        public Nullable<System.DateTime> MatDat { get; set; }
        public decimal DisbursedAmount { get; set; }
        public decimal OutstandingAmount { get; set; }
        public decimal RemainingToDisbursed { get; set; }
        public decimal AppAmt { get; set; }
        public decimal IRate { get; set; }

    }
    public class LoanStatementModel
    {
        public LoanStatementModel()
        {
            AccountDetails = new AccountDetailsShowViewModel();
            LoanAccountDetails = new LoanAccountDetailsShowModel();
        }
        public Nullable<decimal> SNO { get; set; }
        public Nullable<int> IAccNo { get; set; }
        public Nullable<System.DateTime> TranscDate { get; set; }
        public string Particulars { get; set; }
        public Nullable<decimal> DrPrincipal { get; set; }
        public Nullable<decimal> OtherDr { get; set; }
        public Nullable<decimal> Rebate { get; set; }
        public Nullable<decimal> CrPrincipal { get; set; }
        public Nullable<decimal> OtherCr { get; set; }
        public Nullable<decimal> CrPrincInt { get; set; }
        public Nullable<decimal> CrOtherInt { get; set; }
        public Nullable<decimal> CrPenalty { get; set; }
        public string Balance { get; set; }
        public string OtherBal { get; set; }
        public decimal TotalPayment { get; set; }

        public decimal AccuredPenalty { get; set; }
        public decimal AccuredInterest { get; set; }
        public IEnumerable<LoanStatementModel> LoanStatementList { get; set; }
        public AccountDetailsShowViewModel AccountDetails { get; set; }
        public LoanAccountDetailsShowModel LoanAccountDetails { get; set; }
        public SelectList MarkDateList { get; set; }
    }
    public class LoanStatementExportExcelModel
    {
        public Nullable<System.DateTime> TranscDate { get; set; }
        public Nullable<decimal> DrPrincipal { get; set; }
        public Nullable<decimal> OtherDr { get; set; }
        public Nullable<decimal> Rebate { get; set; }
        public Nullable<decimal> CrPrincipal { get; set; }
        public Nullable<decimal> OtherCr { get; set; }
        public Nullable<decimal> CrPrincInt { get; set; }
        public Nullable<decimal> CrOtherInt { get; set; }
        public Nullable<decimal> CrPenalty { get; set; }
        public string Balance { get; set; }
        public string OtherBal { get; set; }
        public decimal TotalPayment { get; set; }
        public string Particulars { get; set; }
        public IEnumerable<LoanStatementModel> LoanStatementList { get; set; }
    }
    public class InterestCapitalizationModel
    {
        public string CustName { get; set; }
        public System.DateTime VDate { get; set; }
        public decimal IntAmt { get; set; }
        public Nullable<decimal> Tax { get; set; }
        public float TaxRt { get; set; }
        public Nullable<decimal> CRamt { get; set; }
        public string ToAccNo { get; set; }
        public string FromAccNo { get; set; }
        public Nullable<int> IsSlf { get; set; }
        public int IAccNo { get; set; }
        public byte PId { get; set; }
        public string PName { get; set; }
        public string SDName { get; set; }
    }
    public class InterestCapitalizationModelExcelExport
    {
        public string PName { get; set; }
        public string CustName { get; set; }
        public string FromAccNo { get; set; }
        public System.DateTime VDate { get; set; }

        public float TaxRt { get; set; }
        public Nullable<decimal> Tax { get; set; }
        public decimal IntAmt { get; set; }
        public Nullable<decimal> CRamt { get; set; }
        public string ToAccNo { get; set; }


    }
    public class InterestLogModel
    {
        public System.DateTime TDATE { get; set; }
        public string PNAME { get; set; }
        public string ACCNO { get; set; }
        public decimal BALANCE { get; set; }
        public decimal RATE { get; set; }
        public byte FDAY { get; set; }
        public Nullable<double> INTCAL { get; set; }
        public string REMARK { get; set; }
        public int BRCHID { get; set; }
        public int IACCNO { get; set; }
        public string AccountName { get; set; }

    }
    public class InterestLogExcelExportModel
    {
        public System.DateTime TDATE { get; set; }
        public string PNAME { get; set; }
        public decimal BALANCE { get; set; }
        public decimal RATE { get; set; }
        public Nullable<double> INTCAL { get; set; }
        public string REMARK { get; set; }
        public int IACCNO { get; set; }

    }
    public class OverdraftBalanceModel
    {
        public string PName { get; set; }
        public string Accno { get; set; }
        public string Aname { get; set; }
        public decimal Bal { get; set; }
        public Nullable<double> ODInterest { get; set; }
        public decimal AppAmt { get; set; }
        public Nullable<System.DateTime> ValDat { get; set; }
        public Nullable<int> Month { get; set; }
        public Nullable<float> Days { get; set; }
        public Nullable<System.DateTime> MatDat { get; set; }
        public Nullable<System.DateTime> Transaction_Date { get; set; }
    }
    public class OverdraftBalanceExcelExportModel
    {
        public string Accno { get; set; }
        public string Aname { get; set; }
        public decimal Bal { get; set; }
        public Nullable<double> ODInterest { get; set; }
        public decimal AppAmt { get; set; }
        public Nullable<System.DateTime> ValDat { get; set; }
        public Nullable<int> Month { get; set; }
        public Nullable<System.DateTime> MatDat { get; set; }
    }
    public class OverdraftInterestCapitalizationModel
    {
        public string Accno { get; set; }
        public string Aname { get; set; }
        public System.DateTime Tdate { get; set; }
        public decimal CapAmt { get; set; }
        public string TransferedTo { get; set; }
        public decimal tno { get; set; }
        public int? vno { get; set; }
    }
    public class OverdraftInterestCapitalizationExportExcelModel
    {
        public string Accno { get; set; }
        public string Aname { get; set; }
        public System.DateTime Tdate { get; set; }
        public decimal CapAmt { get; set; }
        public string TransferedTo { get; set; }
        public decimal tno { get; set; }
        public int? vno { get; set; }
    }
    public class RemittanceReportModel
    {
        public decimal Tno { get; set; }
        public DateTime TDate { get; set; }
        public string RemitCompany { get; set; }
        public string Particular { get; set; }
        public decimal Amount { get; set; }
    }
    public class RemittanceReportExportExcelModel
    {
        public decimal Tno { get; set; }
        public DateTime TDate { get; set; }
        public string RemitCompany { get; set; }
        public string Particular { get; set; }
        public decimal Amount { get; set; }
    }
    public class InterestTOCapitalizeAccountModel
    {
        public string Accno { get; set; }
        public string Aname { get; set; }
        public string PName { get; set; }
        public string intCalpSchm { get; set; }
        public decimal IRate { get; set; }
    }
    public class InterestTOCapitalizeAccountExcelExportModel
    {
        public string Accno { get; set; }
        public string Aname { get; set; }
        public string PName { get; set; }
        public string intCalpSchm { get; set; }
        public decimal IRate { get; set; }
    }
    public class BranchDetailsModel
    {
        public int BrnhID { get; set; }
        public string BrnhNam { get; set; }
        public string Addr { get; set; }
        public DateTime TDate { get; set; }
    }
    public class BalanceCertificateModel
    {
        public int BrnchID { get; set; }
        public string BrnchName { get; set; }
        public string Address { get; set; }
        public string SchemeName { get; set; }
        public DateTime TDate { get; set; }
        public string AccountTitle { get; set; }
        public string AccountNumber { get; set; } //here changed
        public decimal Balance { get; set; }
        public string currency { get; set; }
        public string BalanceInWords { get; set; }
        public DateTime BalanceOfDate { get; set; }
    }
    public class TaxCertificateModel
    {
        public int IAccno { get; set; }
        public Int16 FYID { get; set; }
        public string FyName { get; set; }
        public int PanNumber { get; set; }
        public int RefNumber { get; set; }
        public DateTime Date { get; set; }
        public decimal Interest { get; set; }
        public decimal Tax { get; set; }
        public string Vno { get; set; }
        public string Bank { get; set; }
        public decimal Total { get; set; }
    }

    public class GuarantorReportModel
    {
        public string LoanNo { get; set; }
        public string LoaneeName { get; set; }
        public string GuarantorAccNo { get; set; }
        public string GuarantorName { get; set; }
        public decimal BlockedAmt { get; set; }
        public decimal LoanApproved { get; set; }
        public decimal LoanBalance { get; set; }
        public decimal AccBalance { get; set; }
    }
    public class ProductWiseInterestLogModel
    {
        public byte pid { get; set; }
        public string PName { get; set; }
        public double? LnInt { get; set; }
        public double? PenOnPrin { get; set; }
        public double? PenOnInt { get; set; }
        public double? IntOnInt { get; set; }
        public double? IntOnOther { get; set; }
        public double? DInt { get; set; }
    }
    public class ProductWiseInterestLogExcelExportModel
    {

        public string PName { get; set; }
        public double? DInt { get; set; }

    }
    #region Amit
    public class ProductTransactionReport
    {
        //used for deposit/withdraw as well as for payment/disburse
        public Nullable<int> PID { get; set; }
        public string PNAME { get; set; }
        public string ACCNO { get; set; }
        public string ANAME { get; set; }
        public Nullable<int> SCURRID { get; set; }
        public Nullable<System.DateTime> VDATE { get; set; }
        public string NOTES { get; set; }
        public Nullable<decimal> AMT { get; set; }
        public Nullable<int> TTYPE { get; set; }
        public Nullable<int> POSTEDBY { get; set; }
        public Nullable<int> APPROVEDBY { get; set; }
        public Nullable<int> BRNHNO { get; set; }
        public Nullable<decimal> TNO { get; set; }
        public Nullable<decimal> VNO { get; set; }
        public Nullable<bool> ISVERIFY { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int DepositFlag { get; set; }
        public int SearchParamFlag { get; set; }
        public int VerifyFlag { get; set; }
        public int PaymentDisburseFlag { get; set; }
        public Nullable<int> SdId { get; set; }
        public Nullable<decimal> PriDr { get; set; }
        public Nullable<decimal> OthrDr { get; set; }
        public Nullable<bool> Isverfied { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> Interest { get; set; }//for payment type data
        public Nullable<decimal> IonI { get; set; }    //for payment type data
        public Nullable<decimal> IonCA { get; set; }   //for payment type data
        public Nullable<decimal> PonPr { get; set; }   //for payment type data
        public Nullable<decimal> PonInt { get; set; }  //for payment type data
        public Nullable<decimal> Rbt { get; set; }     //for payment type data
        public List<ProductTransactionReport> ProductTransactionReportList { get; set; }
    }

    public class ProductTransactionExportExcelModel
    {
        public Nullable<decimal> TNO { get; set; }
        public DateTime TDate { get; set; }
        public string ACCNO { get; set; }
        public string ANAME { get; set; }
        public Nullable<decimal> AMT { get; set; }
        public string NOTES { get; set; }
        
    }
    public class LoanTransactionDispursePaymentExcelExportModel
    {
        public string PNAME { get; set; }
        public string ACCNO { get; set; }
        public string ANAME { get; set; }
        public Nullable<System.DateTime> VDATE { get; set; }
        public Nullable<decimal> PriDr { get; set; }
        public Nullable<decimal> OthrDr { get; set; }
        public string NOTES { get; set; }

    }
    public class CollectionSheetTransReportViewModel
    {
        public long collSheetId { get; set; }
        public System.DateTime TDate { get; set; }
        public int SheetNo { get; set; }
        public int CollectorId { get; set; } //test
        public int? UserId { get; set; }
        public string EmployeeName { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public Nullable<decimal> LoanAmount { get; set; }
        public Nullable<decimal> DepositAmount { get; set; }
        public Nullable<int> LoanCount { get; set; }
        public Nullable<int> DepositCount { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? totalCount { get; set; }
        public List<CollectionSheetTransReportViewModel> CollectionSheetTransReportList { get; set; }
    }
    public class CollectionSheetTransExcelReportViewModel
    {

        public System.DateTime TDate { get; set; }
        public int SheetNo { get; set; }
        public string EmployeeName { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public Nullable<decimal> LoanAmount { get; set; }
        public Nullable<decimal> DepositAmount { get; set; }
        public Nullable<int> LoanCount { get; set; }
        public Nullable<int> DepositCount { get; set; }

    }
    public class ProductWiseCollectionSheet
    {
        public string EmployeeName { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public int PID { get; set; }
        public string PName { get; set; }
        public int EmployeeId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? totalCount { get; set; }
        public List<ProductWiseCollectionSheet> ProductWiseCollectionSheetList { get; set; }
    }



    public class LoanAutoPayment
    {
        public int IAccNo { get; set; }
        public string AccNo { get; set; }
        public string PName { get; set; }
        public string AName { get; set; }
        public decimal Bal { get; set; }
        public decimal PostP { get; set; }
        public decimal PaidP { get; set; }
        public Nullable<decimal> MatPrin { get; set; }
        public int PMDays { get; set; }
        public decimal PostI { get; set; }
        public decimal PaidI { get; set; }
        public int IMDays { get; set; }
        public decimal MatInt { get; set; }
        public Nullable<decimal> PenAcc { get; set; }
        public Nullable<decimal> Other { get; set; }
        public int ILinkAc { get; set; }
        public string LnkAccNo { get; set; }
        public string LnkPName { get; set; }
        public decimal AvlBal { get; set; }
        public Nullable<bool> Revolving { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public byte PID { get; set; }
        [Required]
        [Display(Name = "Scheme Name")]
        public int SchemeId { get; set; }
        public List<LoanAutoPayment> LoanAutoPaymentList { get; set; }
        public int BranchId { get; set; }
    }
    public class LoanAfterAutoPayment
    {
        public decimal tno { get; set; }
        public System.DateTime tdate { get; set; }
        public int brnhno { get; set; }
        public byte LPid { get; set; }
        public string LPName { get; set; }
        public byte DPid { get; set; }
        public string DPName { get; set; }
        public int LIAccno { get; set; }
        public string LAccno { get; set; }
        public string Name { get; set; }
        public int DIAccno { get; set; }
        public string DAccno { get; set; }
        public decimal Dramt { get; set; }
        public Nullable<decimal> Prin { get; set; }
        public Nullable<decimal> Int { get; set; }
        public Nullable<decimal> Penal { get; set; }
        public Nullable<decimal> Charge { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<LoanAfterAutoPayment> LoanAfterAutoPaymentList { get; set; }
    }
    public class LoanReportAccountDetails
    {

        public int IAccno { get; set; }
        [DisplayName("Account Number")]
        public string Accno { get; set; }
        public string OldAccno { get; set; }
        [DisplayName("Scheme Name")]
        public string SDName { get; set; }
        [DisplayName("Product Name")]
        public string PName { get; set; }
        [DisplayName("Account Name")]
        public string Aname { get; set; }
        [DisplayName("Acc State")]
        public string AccountState { get; set; }
        public int Revolving { get; set; }
        [DisplayName("Registered Date")]
        public System.DateTime Rdate { get; set; }
        [DisplayName("Matured Date")]
        public Nullable<System.DateTime> matdat { get; set; }
        [DisplayName("Value Date")]
        public Nullable<System.DateTime> valdat { get; set; }
        [DisplayName("Duration")]
        public string duration { get; set; }
        public Nullable<int> PFreq { get; set; }
        public Nullable<int> IFreq { get; set; }
        [DisplayName("Payment Rule")]
        public string PRule { get; set; }
        [DisplayName("Payment Type")]
        public string schType { get; set; }
        [DisplayName("Quotation Amount")]
        public Nullable<decimal> QuotAmt { get; set; }
        [DisplayName("Approved Amount")]
        public Nullable<decimal> AppAmt { get; set; }
        [DisplayName("Rate")]
        public decimal IRate { get; set; }
        public Nullable<byte> CDay { get; set; }
        public decimal DisburseAmount { get; set; }
        public string location { get; set; }
        public string contact { get; set; }
        public decimal cid { get; set; }
        public bool overPay { get; set; }
    }
    public class LoanAccountWiseSchedule
    {
        public int Iaccno { get; set; }
        public Nullable<System.DateTime> Schdate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public Nullable<decimal> InstallmentAmount { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public Nullable<decimal> Principal { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public Nullable<decimal> Interest { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public Nullable<decimal> Balance { get; set; }
        public string NepDate { get; set; }
    }
    public class LoanAccountWiseScheduleExcelExport
    {
        public Nullable<System.DateTime> Schdate { get; set; }
        public Nullable<decimal> InstallmentAmount { get; set; }
        public Nullable<decimal> Principal { get; set; }
        public Nullable<decimal> Interest { get; set; }
        public Nullable<decimal> Balance { get; set; }
        public string NepDate { get; set; }
    }
 
    public class LoanMatured
    {

        public string PName { get; set; }
        public int IAccNo { get; set; }
        public string AccNo { get; set; }
        public string Name { get; set; }
        public decimal ApprovedAmt { get; set; }
        public Nullable<decimal> DisbursedAmt { get; set; }
        public Nullable<decimal> Balance { get; set; }
        public Nullable<System.DateTime> MDate { get; set; }
        public Nullable<int> MDays { get; set; }
        public Nullable<decimal> PrincipalPybl { get; set; }
        public Nullable<decimal> PrincipalPaid { get; set; }
        public Nullable<decimal> MaturedPrincipal { get; set; }
        public Nullable<decimal> InterestPybl { get; set; }
        public Nullable<decimal> InterestPaid { get; set; }
        public Nullable<decimal> MaturedInterest { get; set; }
        public Nullable<decimal> MaturedPrin { get; set; }
    }
    public class LoanMaturedExcelExport
    {
        public string AccNo { get; set; }
        public string Name { get; set; }
        public string PName { get; set; }
        public Nullable<System.DateTime> MDate { get; set; }
        public Nullable<int> MDays { get; set; }
        public decimal ApprovedAmt { get; set; }
        public Nullable<decimal> DisbursedAmt { get; set; }
        public Nullable<decimal> Balance { get; set; }
        public Nullable<decimal> Total { get; set; }

        public Nullable<decimal> PrincipalPaid { get; set; }
        public Nullable<decimal> PrincipalPybl { get; set; }
        public Nullable<decimal> PrincipalTotal { get; set; }

        public Nullable<decimal> InterestPaid { get; set; }
        public Nullable<decimal> InterestPybl { get; set; }
        public Nullable<decimal> InterestTotalDue { get; set; }
    }
    #endregion


    #region Romy
    public class AccountWiseCollectionModel
    {
        public int collectorId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int BranchId { get; set; }
        public string Accountnumber { get; set; }
        public string Aname { get; set; }
        public System.DateTime TDate { get; set; }
        //public int? SType { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public string PName { get; set; }
        public int iaccno { get; set; }
        public int totalCount { get; set; }
        public List<AccountWiseCollectionModel> AccountWiseCollectionList { get; set; }

    }
    public class AccountWiseCollectionExportExcelModel
    {
        public string Accountnumber { get; set; }
        public string Aname { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public string PName { get; set; }

    }
    public class AccountWiseCollectionModelSummary
    {
        public string Accountnumber { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int BranchId { get; set; }
        public string Collector { get; set; }
        public int iaccno { get; set; }
        public System.DateTime TDate { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public string note { get; set; }
        public List<AccountWiseCollectionModelSummary> AccountWiseCollectionListSummary { get; set; }
    }


    #endregion
    #region ShareReports
    public class ShareTransaction
    {
        public string Pur { get; set; }
        public string Name { get; set; }
        public System.DateTime SDate { get; set; }
        public decimal FSno { get; set; }
        public decimal TSno { get; set; }
        public decimal SQty { get; set; }
        public Nullable<decimal> Amt { get; set; }
        //public byte SType { get; set; }
        public string ShareType { get; set; }
    }

    public class ShareHolderList
    {
        public byte SType { get; set; }
        public decimal RegNo { get; set; }
        public string Name { get; set; }
        public Nullable<decimal> SQtyP { get; set; }
        public Nullable<decimal> AmtP { get; set; }
        public Nullable<decimal> SQtyR { get; set; }
        public Nullable<decimal> AmtR { get; set; }
        public Nullable<decimal> SQty { get; set; }
        public Nullable<decimal> Balance { get; set; }
    }
    public class ShareHolderListExportExcel
    {
        public decimal RegNo { get; set; }
        public string Name { get; set; }
        public Nullable<decimal> SQtyP { get; set; }
        public Nullable<decimal> AmtP { get; set; }
        public Nullable<decimal> SQtyR { get; set; }
        public Nullable<decimal> AmtR { get; set; }
        public Nullable<decimal> SQty { get; set; }
        public Nullable<decimal> Balance { get; set; }
    }
    public class SharePurchaseDetails
    {
        public decimal RegNo { get; set; }
        public string Name { get; set; }
        public System.DateTime PDate { get; set; }
        public string PDateNep { get; set; }
        public decimal SCrtNo { get; set; }
        public decimal FSNo { get; set; }
        public decimal TSNo { get; set; }
        public decimal SQty { get; set; }
        public decimal Amt { get; set; }
        public byte SType { get; set; }
        public int Status { get; set; }
        public string Note { get; set; }
        public int ttype { get; set; }
    }

    public class SharePurchaseExcelDetails
    {
        public decimal RegNo { get; set; }
        public string Name { get; set; }
        public System.DateTime PDate { get; set; }

        public decimal SCrtNo { get; set; }
        public decimal FSNo { get; set; }
        public decimal TSNo { get; set; }
        public decimal SQty { get; set; }
        public decimal Amt { get; set; }
        public string SType { get; set; }

        public string Note { get; set; }
        public string ttype { get; set; }
    }
    public class ShareReturnDetails
    {
        public string Name { get; set; }
        public decimal RegNo { get; set; }
        public decimal SCrtNo { get; set; }
        public decimal FSno { get; set; }
        public decimal TSno { get; set; }
        public decimal SQty { get; set; }
        public Nullable<decimal> Amt { get; set; }
        public byte SType { get; set; }
        public int ttype { get; set; }
        public Nullable<decimal> SoldTo { get; set; }
        public System.DateTime Sdate { get; set; }
        public string SDateNep { get; set; }
        public string Note { get; set; }
    }
    public class ShareReturnDetailsExportExcel
    {
        public string Name { get; set; }
        public decimal RegNo { get; set; }
        public decimal SCrtNo { get; set; }
        public decimal FSno { get; set; }
        public decimal TSno { get; set; }
        public decimal SQty { get; set; }
        public Nullable<decimal> Amt { get; set; }
        public string SType { get; set; }
        public string ttype { get; set; }
        public Nullable<decimal> SoldTo { get; set; }
        public System.DateTime Sdate { get; set; }

        public string Note { get; set; }
    }
    public class ShareStatement
    {
        public int Pur { get; set; }
        public decimal CId { get; set; }
        public decimal RegNo { get; set; }
        public string Name { get; set; }
        public decimal SCrtno { get; set; }
        public System.DateTime PDate { get; set; }
        public decimal FSno { get; set; }
        public decimal TSno { get; set; }
        public decimal PurQty { get; set; }
        public decimal RetQty { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public byte SType { get; set; }
        public string location { get; set; }
        public string GFatherName { get; set; }
        public string FatherName { get; set; }
        public decimal Tno { get; set; }
    }
    public class ShareStatementExportToExcel
    {
        public string PurchaseAndReturn { get; set; }
        public decimal RegNo { get; set; }
        public string Name { get; set; }
        public decimal SCrtno { get; set; }
        public System.DateTime PDate { get; set; }
        public decimal FSno { get; set; }
        public decimal TSno { get; set; }
        public decimal PurQty { get; set; }
        public decimal RetQty { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string GFatherName { get; set; }
        public string FatherName { get; set; }
        public decimal Tno { get; set; }
    }
    #endregion
    #region mausham
    public class AccOpenByCollectorViewModel
    {
        public AccOpenByCollectorViewModel(){
            AccOpenByCollectorViewModelList = new List<AccOpenByCollectorViewModel>();
        }
        public int BrchID { get; set; }
        public System.DateTime FromDate { get; set; }
        public System.DateTime ToDate { get; set; }
        public int Status { get; set; }
        public string Name { get; set; }
        public string PName { get; set; }
        public int BroughtBy { get; set; }
        public decimal NoOfCustomers { get; set; }
        public decimal PCount { get; set; }
        public List<AccOpenByCollectorViewModel> AccOpenByCollectorViewModelList { get; set; } //change list
    }

    public class AccOpenByCollectorExportToExcel
    {
        public string Name { get; set; }
      
        public decimal PCount { get; set; }
    }

    public class CollectorDetailViewModel
    {
        public string Accno { get; set; }
        public string AccountName { get; set; }
        public DateTime RDate { get; set; }
        public decimal Balance { get; set; }
        public string EmployeeName { get; set; }
        public string PName { get; set; }
        public string SDName { get; set; }
        public string CollectorName { get; set; }
        public int totalCount { get; set; }
        public List<CollectorDetailViewModel> collectorDetailViewModel { get; set; }
    }
    public class CollectorDetailExportExcelViewModel
    {
        public string Accno { get; set; }
        public string AccountName { get; set; }
        public DateTime RDate { get; set; }
        public decimal Balance { get; set; }
        public string EmployeeName { get; set; }
        public string PName { get; set; }
        public string SDName { get; set; }
        //public List<CollectorDetailViewModel> collectorDetailViewModel { get; set; }
    }
    #endregion

}


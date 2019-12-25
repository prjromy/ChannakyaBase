using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannakyaBase.BLL.CustomHelper
{
    public enum ECustomerSearchType
    {
        [Description("Limited")]
        Limited = 1,
        [Description("Unlimited")]
        Unlimited = 2,
        [Description("Single")]
        SingleType = 3,
        [Description("AccountOpen")]
        AccountOpen = 4,
        [Description("CompanyOnly")]
        CompanyOnly = 5,
        [Description("CustomerOnly")]
        CustomerOnly = 6,
        
        [Description("VerifiedRegisteredLoanList")]
        VerifiedRegisteredLoanList = 7,
        [Description("All")]
        All = 8,

    }

    public enum TypeOfCustomer
    {
        [Description("Customer")]
        Customer = 1,
        [Description("ShareCustomer")]
        ShareCustomer = 2,
    }

    public enum EEmployeeOrShare
    {
        [Description("Employee")]
        Employee = 1,
        [Description("ShareHolder")]
        ShareHolder = 2,
    }
    public enum EDeno
    {
        [Description("DenoIn")]
        DenoIn = 1,
        [Description("DenoOut")]
        DenoOut = 2,
    }
    public enum EAccountDetailsShow
    {
        [Description("WithAccount")]
        WithAccount = 1,
        [Description("WithOutAccount")]
        WithOutAccount = 2,
        [Description("NoDisplay")]
        NoDisplay = 3,
        [Description("WithdrawWithIntPay")]
        WithdrawWithIntPay = 4,
        [Description("ChargableAccounts")]
        ChargableAccounts = 5,
        [Description("ChequeTransHistory")]
        ChequeTransHistory = 6,
        [Description("CollateralDisplay")]
        CollateralDisplay = 7,
        [Description("LoanWithCollateral")]
        LoanWithCollateral = 8,
        [Description("LoanDetails")]
        LoanDetails = 9,
        [Description("LoanPayment")]
        LoanPayment = 10,
        [Description("Deposit")]
        Deposit = 11,
        [Description("ImageEvent")]
        ImageEvent = 12,

        [Description("Report")]
        Report = 13,
        [Description("IsFDLoan")]
        IsFDLoan = 14,
        [Description("AccountClose")]
        AccountClose = 15,
   
        [Description("Adjustment")]
        Adjustment = 16,
        

        [Description("WithOutAccountSearch")]
        WithOutAccountSearch = 17,
        [Description("DepositList")]
        DepositList = 18,
        [Description("GuarantorDetail")]
        GuarantorDetail = 19,

        [Description("LoanRebate")]
        LoanRebate = 20,
        //[Description("RegisteredLoanAccount")]
        //RegisteredLoanAccount = 18,

    }
    public enum EAccountFilter
    {
        [Description("DepositAccept")]
        DepositAccept = 1,
        [Description("ReadyToClose")]
        ReadyToClose = 2,

        [Description("LoanAccept")]
        LoanAccept = 3,
        [Description("WithdrawAccept")]
        WithdrawAccept = 4,
        [Description("Nominee")]
        Nominee = 5,
        [Description("AllowCheque")]
        AllowCheque = 6,
        [Description("FixedAccountOnly")]
        FixedAccountOnly = 7,
        [Description("WithdrawWithRevolving")]
        WithdrawWithRevolving = 8,
        [Description("ImageAccept")]
        ImageAccept = 9,
       

        [Description("AdjustmentAccept")]
        AdjustmentAccept = 10,
        [Description("AccountClose")]
        AccountClose = 11,
        [Description("DepositAcceptList")]
        DepositAcceptList = 12,

        [Description("UnverifiedAccountList")]
        UnverifiedAccountList=13,
        [Description("WithoutFixedDeposit")]
        WithoutFixedDeposit = 14,
        [Description("WithGuarantor")]
        WithGuarantor = 15,
    }
    public enum EAccountType
    {
        [Description("Loan")]
        Loan = 1,
        [Description("Normal")]
        Normal = 2,
        [Description("Other")]
        Other = 3,

    }
    public enum EScheduleType : int
    {

        DefaultEnglishDate = 1,

        DefaultNepaliDate = 2,

        EndOfEnglishMonth = 3,

        EndOfNepaliMonth = 4,

        CustomEnglishDay = 5,

        CustomNepaliDay = 6,

        DefaultWeekly = 8,

        EndOfWeekEnd = 9,

        Daily = 10,
    }
    public enum DisbOf : byte
    {
        Both = 0,
        Principal = 1,
        Interest = 2
    }
    public enum EEmployeeChange
    {
        [Description("UserChangeTeller")]
        UserChangeTeller = 1,
        [Description("PurchaseShare")]
        PurchaseShare = 2,
        [Description("ReturnShare")]
        ReturnShare = 3,
        [Description("ImageChange")]
        ImageChange = 4,
        [Description("UserCashLimit")]
        UserCashLimit = 5,
    }

    public static class EnumHelper
    {
        public static string GetDescription(this Enum value)
        {
            System.Reflection.FieldInfo fi = value.GetType().GetField(value.ToString());
            System.ComponentModel.DescriptionAttribute[] attributes =
                  (System.ComponentModel.DescriptionAttribute[])fi.GetCustomAttributes(
                  typeof(System.ComponentModel.DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }
    }
}

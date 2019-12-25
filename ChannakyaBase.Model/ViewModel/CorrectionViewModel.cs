using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannakyaBase.Model.ViewModel
{
    public class CorrectionViewModel
    {

        public int AAdjustmentID { get; set; }

        [Display(Name ="Account No.")]
        public int Iaccno { get; set; }

        //[Display(Name = "Product")]
        //public byte PID { get; set; }
 

        [Display(Name = "Adjustment Type")]
        public string AdjustmentType { get; set; }

        [Display(Name ="Old Balance")]
        public decimal OldBalance { get; set; }

        [Display(Name ="Adjustment Amount")]
        [Required(ErrorMessage ="Adjustment Amount is required")]
        public decimal AdjustmentAmount { get; set; }

        [Display(Name = "New Balance")]
         public decimal Amt { get; set; }

        public Nullable<decimal> TNo { get; set; }
        public int UserId { get; set; }
        public System.DateTime TDate { get; set; }
        [Display(Name ="Notes")]
        [Required(ErrorMessage = "Notes is required")]
        public string note { get; set; }
        public int valued { get; set; }
        public string radiovalue { get; set; }

        public List<CorrectionViewModel> correctionList { get; set; }
       
      
    }

    public class TransactionEditViewModel
    {
        //public int AccountId { get; set; }

        public int EditId { get; set; }

        [Display(Name="Enter Transaction No.")]
        public Nullable<decimal> Tno { get; set; }

        [Display(Name = "Transaction Date :")]
        public System.DateTime tdate { get; set; }

        [Display(Name = "Account No :")]
        public int Iaccno { get; set; }

        public string Accno { get; set; }

        [Display(Name = "Name :")]
        public string Aname { get; set; }

        [Display(Name = "Branch :")]
        public string BranchName { get; set; }

        [Display(Name = "Posted By :")]
        public string Username { get; set; }

        [Display(Name = "Posted By :")]
        public int PostedBy { get; set; }

        [Display(Name = "Slip No :")]
        public int slpno { get; set; }

        [Display(Name="Amount :")]
       
        public decimal Amt { get; set; }

        [Display(Name="Currency :")]
        public string CurrencyName { get; set; }

        [Display(Name = "Enter New  Amount:")]
        [Required(ErrorMessage = "Amount is required")]
        public decimal newAmount { get; set; }

        public bool IsDep { get; set; }
        
        [Display(Name ="New Account No:")]
        public int  newAccountNo { get; set; }

        //public string Remarks { get; set; }
        public  Nullable<Int16> Status { get; set; }

       public DateTime PostedOn { get; set; }
        public List<TransactionEditViewModel> transactionList { get; set; }

        public int transactioneditCount { get; set; }
      public bool? IsDeleted { get; set; }
     public int IsDeposit { get; set; }
    }


}

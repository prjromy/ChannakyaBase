using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannakyaBase.Model.ViewModel
{
    public class ChequeClearenceViewModel
    {

        public int rno { get; set; }
        [Required(ErrorMessage ="Deposit account number is required!!")]
        public int IAccno { get; set; }
        [Required(ErrorMessage ="BankName is Required!!")]
        [RegularExpression("^[a-zA-z ]*$", ErrorMessage = "BankName must be String!!")]
        public string Bankname { get; set; }
        [RegularExpression("^[a-zA-z ]*$", ErrorMessage = "BranchName must be String!!")]
        [Required(ErrorMessage = "BranchName is Required!!")]
        public string Brnhname { get; set; }
        [Required(ErrorMessage = "Cheque Number is Required")]
       // [RegularExpression("^[0-9]*$", ErrorMessage = "Only digits please!!")]
        public string chqno { get; set; }
        [Required(ErrorMessage = "Payee Name is Required!!")]
        [RegularExpression("^[a-zA-z ]*$",ErrorMessage ="Name must be String!!")]
        public string payee { get; set; }
        public System.DateTime tdate { get; set; }
        [Required(ErrorMessage = "Please Enter Amount")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Only digits please!!")]
        public decimal camount { get; set; }
        [Required(ErrorMessage ="Enter Remarks!!!")]
        public string remarks { get; set; }
        public int postedby { get; set; }
        [Required(ErrorMessage = "Enter Account Number!!")]
        public string accno { get; set; }
        public int BranchID { get; set;}
        public System.DateTime VDate { get; set; }
        public System.DateTime Postedon { get; set; }
        public Nullable<bool> chqstate { get; set; }
        public int TotalCount {get;set;}
        public List<ChequeClearenceViewModel> ChequeClearenceList { get; set; }
        public IPagedList<ChequeClearenceViewModel> ChequeClearenceWithIpageList {get;set;}

        public int calledFromVerify { get; set; }


    }
    public class AMClearenceViewModel
    {
        public int rno { get; set; }
        public int IAccno { get; set; }
        public string bankname { get; set; }
        public string brnhname { get; set; }
        public string chqno { get; set; }
        public string payee { get; set; }
        public System.DateTime tdate { get; set; }
        public decimal camount { get; set; }
        public string remarks { get; set; }
        public int postedby { get; set; }
        public string accno { get; set; }
        public int BranchID { get; set; }
        public int VNo { get; set;}
        public int TotalCount { get; set; }
        public System.DateTime VDate { get; set; }
        public System.DateTime postedon { get; set; }
        public int verifiedby { get; set; }
        public System.DateTime verifiedon { get; set; }
        public System.DateTime cdate { get; set; }
        public List<AMClearenceViewModel> chequeClearedList { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set;}
        public System.DateTime fromDate { get; set; }
        public System.DateTime toDate { get; set; }
        public IPagedList<AMClearenceViewModel> AmclearenceWithIPageList { get; set; }
        public List<AMClearenceViewModel> AnclearenceList { get; set; }
    }

}

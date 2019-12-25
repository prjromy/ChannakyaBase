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
    public class ChqRqstModel
    {
        [Required]
        [DisplayName("Account No.")]
        public int? Iaccno { get; set; }
        [Required]
        [DisplayName("No. Of Pieces")]
        [Range(1, byte.MaxValue, ErrorMessage = "Number of cheque cann't be greater than 255 at one time.!!")]
        public int Pics { get; set; }
        public System.DateTime Tdate { get; set; }
        public int Rno { get; set; }
        public int PostedBy { get; set; }
        public string AccountNo { get; set; }
        public string PostedUser { get; set; }

        public IPagedList<ChqRqstModel> ChequeRequestList { get; set;}
    }

    public  class AChqHModel
    {
        public int AchqHId { get; set; }
        [Required]
        [DisplayName("Account No.")]
        public int IAccno { get; set; }
        [Required]
        [DisplayName("Action")]
        public byte cstate { get; set; }  
        public int Rno { get; set; }
        public int ChkNo { get; set; }
        public bool IsSelectet { get; set; }
        public Nullable<long> Tno { get; set; }
        public string ShowWith { get; set; }

        public int[] SelectCheque { get; set; }
        public List<AChqHModel> AchqHList { get; set; }
        public AChqModel AChqModel { get; set; }
    }
    public  class AchqHHModel
    {
        public int AchqHHId { get; set; }
        public int AchqHId { get; set; }
        public byte Cstate { get; set; }
        public System.DateTime Tdate { get; set; }
        public int Postedby { get; set; }
        public System.DateTime PostedOn { get; set; }
        public string Remarks { get; set; }
    }
    public class ChqAttributeViewModel
    {
        public int Id { get; set; }
        public string AccountNamePostion { get; set; }
        public string AccountNumberPosition { get; set; }
        public string AccountTypePosition { get; set; }
        public string ChequeNumberPosition { get; set; }
        public string Cheque2NumberPosition { get; set; }
        public string BranchNamePosition { get; set; }
        public string BranchPhoneNumberPosition { get; set; }
        public string BranchAddressPosition { get; set; }
    }
    public class AChqModel
    {
        public int rno { get; set; }
        [Required]
        [DisplayName("Account No.")]
        public int IAccno { get; set; }
        public int cfrom { get; set; }
        public int cto { get; set; }
        [Required]
        [DisplayName("Action")]
        public byte cstate { get; set; }
        public int postedby { get; set; }
        public Nullable<int> approvedby { get; set; }
        public System.DateTime tdate { get; set; }
        public Nullable<bool> IsPrinted { get; set; }
        [Required]
        public byte ApplyStatus { get; set; }
        public bool IsSelectet { get; set; }
        public string ShowWith { get; set; }

        public int[] SelectCheque { get; set; }
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public IPagedList<AChqModel> AchqPageList { get; set; }
        public List<AChqModel> AchqList { get; set; }
        public List<AChqHModel> AchqHDetailsList { get; set; }
        public AChqHModel AchqHistory { get; set; }
        [Required]       
        public string Remarks { get; set; }
        public string AccountName { get; set; }
        public int ChequeNo { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int branchId { get; set; }
        public string branchName { get; set; }
        public string branchAddress { get; set; }
        public string branchPhoneNumber { get; set; }
        public string ChequeStatus { get; set; }
        public bool isChecked { get; set; } //to check in chequeprint
    }
    public class InternalChequeDepositModel
    {
        [Required]
        [DisplayName("Account No.")]
        public int FIaccno { get; set; }
        [Required]
        [DisplayName("Receipient Ac No.")]
        public int TIAccno { get; set; }
        public System.DateTime Tdate { get; set; }
        public Nullable<System.DateTime> Vdate { get; set; }
        public long Tno { get; set; }
        [Required]
        [DisplayName("Amount")]
        public decimal Amt { get; set; }
        public int Ttype { get; set; }
        [Required]
        [DisplayName("Cheque No.")]
        public int SlpNo { get; set; }
        public short postedby { get; set; }
        public Nullable<short> verifiedby { get; set; }
        public Nullable<int> Vno { get; set; }
        public Nullable<decimal> AVno { get; set; }
        public Nullable<bool> IBChq { get; set; }
        public short BrchID { get; set; }
        public bool IsBounce { get; set; }
        [Required]
        [DisplayName("Cheque No")]
        public int BounceChequeNumber { get; set; }
        [Required]
        [DisplayName("Cheque Bounce Reason")]
        public string ChqBounceReason { get; set; }
        [DisplayName("Receipient's Name")]
        public string AccountName { get; set; }
        public string FromAccountNumber { get; set; }
        public string ToAccountNumber { get; set; }
        public string ToAccountName { get; set; }
    }
}

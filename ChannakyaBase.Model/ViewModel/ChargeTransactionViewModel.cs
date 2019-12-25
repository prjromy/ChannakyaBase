using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannakyaBase.Model.ViewModel
{
    public class ChargeTransactionViewModel
    {
        public System.DateTime Tdate { get; set; }
       
        public decimal Amt { get; set; }
        public long TrnxNo { get; set; }
        public string Remarks { get; set; }
        public short Postedby { get; set; }
        public Nullable<short> Approvedby { get; set; }
        public Nullable<int> Iaccno { get; set; }
        public Nullable<short> Vno { get; set; }
        public Nullable<byte> ttype { get; set; }
        [Required]
        public Nullable<decimal> slpno { get; set; }
        public Nullable<decimal> brhid { get; set; }
        public int accountStatus { get; set; }
        [Required]
        public int ChgID { get; set; }
        public string ChgName { get; set; }
        public int FID { get; set; }
        [Required]
        public byte Modules { get; set; }
        public Nullable<int> Product { get; set; }
        public Nullable<int> ModEveID { get; set; }
        public byte Triggertype { get; set; }
        public byte ChrType { get; set; }
        public Nullable<float> Ratio { get; set; }
        public Nullable<decimal> CAmount { get; set; }
        public Nullable<int> ChrTempID { get; set; }
        public bool Chngble { get; set; }
        public bool ChrDirect { get; set; }
        public bool Status { get; set; }
       public bool ChargeRelatedType { get; set; }
        public int CashRelated { get; set; }
        public string AccountName { get; set; }
        public decimal AmountCharge { get; set; }
        [Display(Name = "Register Name")]
        public int Regno { get; set; }
        public Nullable<int> LIaccno { get; set; }
        public Nullable<int> SIaccno { get; set; }
    }
}

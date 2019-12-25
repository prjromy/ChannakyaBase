using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannakyaBase.Model.ViewModel
{
    public class ShareRegisterViewModel
    {
        [Display(Name = "Customer")]
        public int[] CId { get; set; }
        public decimal RegNo { get; set; }
        [Display(Name = "Register Date")]
        public Nullable<System.DateTime> Rdate { get; set; }
        public int PostedBy { get; set; }
        public int ApprovedBy { get; set; }
        [Required]
        [Display(Name = "ReferredBy")]
        public int[] ReferredBy { get; set; }
        [Required]
        [Display(Name = "Collector")]
        public int BroughtBy { get; set; }
        public ANomineeViewModel AccountNominee { get; set; }
        public ICollection<ANomineeViewModel> ANominees { get; set; }
        public string Name { get; set; }
        public string RegistrationCode { get; set; }
        public IPagedList<ShareRegisterViewModel> ShareRegistrationList { get; set; }
    }

    public class ShareNomineeModel
    {
        public ShareNomineeModel()
        {
            CustomerDetails = new CustInformationViewModel();
        }
        public int SnomineeId { get; set; }
        public decimal RegNo { get; set; }
        public string NomNam { get; set; }
        public string NomRel { get; set; }
        public string CCertID { get; set; }
        public string CCertno { get; set; }
        public Nullable<float> Share { get; set; }
        public string ContactAddress { get; set; }
        public string ContactNo { get; set; }

        public CustInformationViewModel CustomerDetails { get; set; }
        public List<ShareNomineeModel> ShareNomineeList { get; set; }
    }
    public class ShrSPurchaseModel
    {
        public decimal SharePurchaseId { get; set; }
        
        [Display(Name = "Register A/c")]
        public int Regno { get; set; }

        public decimal RegistrationNo { get; set; }
        public decimal Tno { get; set; }
        public System.DateTime Pdate { get; set; }
        [Required]
        [Display(Name = "Share Quantity")]
        public decimal SQty { get; set; }
        [Required]
        [Display(Name = "Amount")]
        public decimal Amt { get; set; }
        [Display(Name = "Note")]
        public string Note { get; set; }
        public int PostedBy { get; set; }

        public int ttype { get; set; }
        [Required]
        [Display(Name = "Share type")]
        public Nullable<int> SType { get; set; }
        public Nullable<int> Vno { get; set; }
        public Nullable<byte> Brchid { get; set; }
        [Required]
        [Display(Name = "Share Rate")]
        public decimal Rate { get; set; }
        public string Name { get; set; }
        public List<ShareReturnViewModel> ShareReturnCustomerList { get; set; }
        public IPagedList<ShrSPurchaseModel> SharePurchaseList { get; set; }
    }

    public class ShareReturnViewModel
    {
        [Display(Name = "Customer")]
        public int[] CId { get; set; }
        public decimal RegNo { get; set; }
        [Display(Name = "Register Date")]
        public System.DateTime Rdate { get; set; }
        public decimal Scrtno { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public int Qty { get; set; }
        public string Note { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public int SType { get; set; }
        public decimal Sid { get; set; }

    }
    public class ShareCustomerDetailsViewModel
    {
        [Display(Name = "Customer")]
        public int[] CId { get; set; }
        public decimal RegNo { get; set; }
        [Display(Name = "Register Date")]
        public DateTime Rdate { get; set; }
        public decimal Scrtno { get; set; }
        public string Name { get; set; }
        public decimal From { get; set; }
        public decimal To { get; set; }
        public decimal Qty { get; set; }
   //     public string Stype { get; set; }
        public byte Stype { get; set; }
        public decimal Balance { get; set; }
        public bool IsChecked { get; set; }
        public int ShareType { get; set; }
        public int Tno { get; set; }
        public int TotalCount { get; set; }
        public IPagedList<ShareCustomerDetailsViewModel> ShareCustomerDetailsIPageList { get; set; }
        public List<ShareCustomerDetailsViewModel> ShareCustomerDetailsList { get; set; }
        public byte AccState { get; set; }
        public System.DateTime Sdate { get; set; }
        public string Note { get; set; }
        public int PostedBy  { get; set; }
        public decimal Sid { get; set; }
        public decimal FSno { get; set; }
        public decimal TSno { get; set; }
        public decimal SQty { get; set; }
    }
    public class ShareReportByDateViewModel
    {
        public System.DateTime FromDate { get; set; }
        public System.DateTime ToDate { get; set; }
        public List<ShareReportByDateViewModel> shareReportByDateViewModel { get; set; }
    }

}

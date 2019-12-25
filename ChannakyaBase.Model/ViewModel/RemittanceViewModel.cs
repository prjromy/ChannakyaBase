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
    public class RemitDepositModel
    {
        public int RemitDepositId { get; set; }
        public decimal Tno { get; set; }
        [Required]
        [DisplayName("Sender Name")]
        public string SenderName { get; set; }

        [Required(ErrorMessage = "Your must provide a Contact number")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        [DisplayName("Sender Contact")]
        public string SenderContact { get; set; }

        [Required]
        [DisplayName("Company Name")]
        public int RemitCompanyId { get; set; }

        

        [Required]
        [DisplayName("Amount")]
        public decimal Amount { get; set; }


        [DisplayName("Remarks")]
        public string Remarks { get; set; }

        public int PostedBy { get; set; }
        public System.DateTime PostedOn { get; set; }
        public Nullable<int> ApproveBy { get; set; }
        public Nullable<System.DateTime> ApproveOn { get; set; }
        public int BranchId { get; set; }
        public Nullable<decimal> Vno { get; set; }

        [Required]
        [DisplayName("Receiver Name")]
        public string ReceiverName { get; set; }

        [Required]
        [DisplayName("Receiver Address")]
        public string ReceiverAddress { get; set; }

        [Required]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        [DisplayName("Receiver Contact")]
        public string ReceiverContact { get; set; }

        public string BranchName { get; set; }
        public string CompanyName { get; set; }
        public IPagedList<RemitDepositModel> RemitDepositList { get; set; }
    }
    public class RemitPaymentModel
    {
        public int RemitPaymentId { get; set; }
        public decimal Tno { get; set; }

        [Required]
        [DisplayName("Company Name")]
        public int RemitCompanyId { get; set; }

        [Required]
        [DisplayName("Receiver Name")]
        public string ReceiverName { get; set; }

        [Required]
        [DisplayName("Receiver Identity")]
        public string ReceiverIdNumber { get; set; }

        [Required]
        [DisplayName("Receiver Address")]
        public string ReceiverAddress { get; set; }

        [Required]
        [DisplayName("Amount")]
        public decimal Amount { get; set; }

        public int BranchId { get; set; }
        public Nullable<decimal> Vno { get; set; }


        [DisplayName("Remarks")]
        public string Remarks { get; set; }
        [Required]
        [DisplayName("Remittance Code")]
        public string RemittanceCode { get; set; }
        public int PostedBy { get; set; }
        public System.DateTime PostedOn { get; set; }
        public Nullable<int> ApproveBy { get; set; }
        public Nullable<System.DateTime> ApproveOn { get; set; }

        public string BranchName { get; set; }

        public string CompanyName { get; set; }
        public IPagedList<RemitPaymentModel> RemitPaymentList { get; set; }
        public bool UserTellerLimit { get; set; }
    }
}

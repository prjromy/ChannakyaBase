using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannakyaBase.Model.ViewModel
{
    public class ShareSignatureViewModel
    {
        [Display(Name = "Import Signature")]


        public int ShareID { get; set; }
        public string Signature { get; set; }
        public DateTime UploadedOn { get; set; }
        public int UploadedBy { get; set; }
        public bool Status { get; set; }

        [Display(Name = "Register A/c")]
        public int Regno { get; set; }
        public CustomerPhotoViewModel customerPhotoViewModel { get; set; }
        public AccountSignatureViewModel accountSignatureViewModel { get; set; }

        public List<ShareSignatureViewModel> shareSignatureViewModel { get; set; }

    }

    public class CustomerPhotoViewModel
    {
        public int PID { get; set; }
        [Display(Name = "Import Image")]
        public string Image { get; set; }
        public DateTime UploadedOn { get; set; }
        public int UploadedBy { get; set; }
        public bool Status { get; set; }

        [Display(Name = "Customer Name")]
        public int CustomerName { get; set; }
        public List<CustomerPhotoViewModel> customerPhotoViewModel { get; set; }

    }

    public class AccountSignatureViewModel
    {
        public int SignatureID { get; set; }
        [Display(Name = "Import Signature")]
        public string Signature { get; set; }
        public DateTime UploadedOn { get; set; }
        public int UploadedBy { get; set; }
        public bool Status { get; set; }
        public string signature1 { get; set; }
        [Display(Name = "Account Owner")]
        public int AccountOwner { get; set; }

        public List<AccountSignatureViewModel> accountSignatureViewModel { get; set; }

    }
}

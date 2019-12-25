using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannakyaBase.Model.ViewModel
{
    public class CashLimitModel
    {
        public int cashLimitId { get; set; }
        [DisplayName("Employee Name")]
        [Required(ErrorMessage ="This field is required")]
        public int EmployeeId { get; set; }
        [DisplayName("UserName")]
        [Required]
        public int stfid { get; set; }
        [DisplayName("Dr Amount")]
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal dramt { get; set; }
        [DisplayName("Cr Amount")]
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N2}")]
        public decimal cramt { get; set; }

        public string EmployeeName { get; set; }
        public string UserName { get; set; }
        public DateTime StartFrom { get; set; }
        public string DGName { get; set; }

        public List<CashLimitModel> CashLimitList { get; set; }
    }

    public class ChequeInventorySetupModel
    {
        public int ChequeInventorySetupId { get; set; }


        [DisplayName("To Cheque No.")]
        //[Range(0, 10, ErrorMessage = "Number of cheque cann't be greater than 10 at one time.!!")]

        public decimal toCheckNo { get; set; }

        [DisplayName("From Cheque No.")]
        //[Range(0, 18, ErrorMessage = "Number of cheque cann't be greater than 18 at one time.!!")]
        public decimal fromCheckNo { get; set; }

        [DisplayName("Branch Name")]
        public int BranchId { get; set; }

        [Display(Name ="Current No")]
        public decimal Lastindx { get; set; }

        public bool ISfinish { get; set; }

        public int PostedBy { get; set; }
        public System.DateTime PostedOn { get; set; }
        public Nullable<decimal> Balance { get; set; }

        [DisplayName("Branch Name")]
        public string BranchName { get; set; }
        public List<ChequeInventorySetupModel> ChequeInventorySetupList { get; set; }

    }

}
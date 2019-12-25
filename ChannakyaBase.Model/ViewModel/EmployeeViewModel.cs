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
    public class EmployeeViewModel
    {
        public int EmployeeId { get; set; }

        [DisplayName("Code")]
        public string EmployeeNo { get; set; }

        [DisplayName("Full Name")]
        public string EmployeeName { get; set; }

        [DisplayName("Departmet")]
        public string DeptName { get; set; }

        [DisplayName("Designation")]
        public string DGName { get; set; }

        public string SearchEmployee { get; set; }
        public string EmployeeOption { get; set; }
        public int BranchId { get; set; }
        public string BrnhNam { get; set; }

        public DateTime StartFrom { get; set; }

        public int MapId { get; set; }
        public string RoleName { get; set; }
        public int UserId { get; set; }

        public int TotalCount { get; set; }
        public string UserName { get; set; }
        public string LoadType { get; set; }
        public string SearchType { get; set; }
        public IPagedList<EmployeeViewModel> EmployeeList { get; set; }
        public List<EmployeeViewModel> Employee { get; set; }
    }

    public class EmployeeBranchMapModel
    {
        public int MapId { get; set; }
        [Required]
        public int EmpBranchId { get; set; }
        [Required]
        [Display(Name = "EmployeeName")]
        public int EmpId { get; set; }
        [Required]
        [Display(Name = "Start From")]
        public System.DateTime StartFrom { get; set; }
        public int PostedBy { get; set; }
        public System.DateTime PostedOn { get; set; }
        [Display(Name = "Current Role")]
        public int CurrentRole { get; set; }
        public bool IsCurrentRole { get; set; }
        public string EmployeeName { get; set; }
        public List<EmployeeBranchMapModel> EmployeeBranchList { get; set; }

    }
    public class AllDesignationViewModel
    {
        public int UserId { get; set; }
        public int DesignationId { get; set; }


        [DisplayName("Departmet")]
        public string DeptName { get; set; }
        public int DepartmentId { get; set; }
        [DisplayName("Designation")]
        public string DGName { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int BranchId { get; set; }
        public string UserName { get; set; }
        public int DegOrder { get; set; }
        public string FullName { get; set; }
        public List<string> ConcatName { get; set; }
    }

}

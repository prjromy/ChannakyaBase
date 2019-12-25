using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ChannakyaBase.Model.ViewModel
{

    public class CustInformationViewModel
    {
        public decimal CID { get; set; }
        public string Name { get; set; }

        public byte isind { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string ContactPerson { get; set; }
        public string SearchOption { get; set; }
        public string SearchParameter { get; set; }
        public IEnumerable<int> CIDs { get; set; }
        public int TotalCount { get; set; }

        public byte IsIndividual { get; set; }
        public bool Isselect { get; set; }

        public string Mode { get; set; }
        public string CustomerType { get; set; }
        public int RegNo { get; set; }
        public string Contact { get; set; }
        public string Ctype { get; set; }
        public byte CtypeID { get; set; }
        public string PANNo { get; set; }
        public string location { get; set; }
        public int IAccno { get; set; }
        public List<CustInformationViewModel> SelectedCustInfoList { get; set; }
        public IPagedList<CustInformationViewModel> CustomerInfoList { get; set; }
    }

    public class CustomerCompanyViewModel
    {
        public short CCGroupID { get; set; }
        [Required(ErrorMessage = "Company Name cannot be empty.")]
        [Remote("CheckCustomerCompanyGroup", "Customer", AdditionalFields = "CCGroupID", ErrorMessage = "Duplicate Customer Company !!")]
        [DisplayName("Company Group Name")]
        public string CCGroupName { get; set; }

        public List<CustomerCompanyViewModel> CustomerCompanyList { get; set; }
    }
}
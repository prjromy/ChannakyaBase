using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ChannakyaBase.Model.Models;


namespace ChannakyaBase.Model.ViewModel
{
    public class CustSectCertContViewModel
    {

        public byte CtypeID { get; set; }
        [Display(Name = "Customer Type")]
        [Required(ErrorMessage = "Required Feild")]
        [Remote("CheckCustType", "CustomerType", AdditionalFields = "CtypeID", ErrorMessage = "Duplicate Customer Type!!!")]
       
        public string Ctype { get; set; }
        [Display(Name = "Tax Type")]
        [Required(ErrorMessage = "Required Feild")]
        public byte TaxID { get; set; }
        [Display(Name = "Is Individual")]
        public bool isind { get; set; }
        public virtual SectorDefViewModel SectType { get; set; }
        public virtual ContactDefViewModel ContactDef { get; set; }
        public virtual CertificateDefViewModel CertificateDef { get; set; }
        public virtual List<ContactDefViewModel> ContactDefList { get; set; }
        public virtual List<CertificateDefViewModel> CertifiacteDefList { get; set; }
        public virtual List<SectorDefViewModel> SectorDefList { get; set; }

    }
}

public class ContactDefViewModel
{
    public int CTCntID { get; set; }
    public byte CNotype { get; set; }
    public string CNodesc { get; set; }
    public string CNoabb { get; set; }
    public byte ? ContStatus { get; set; }
    // public virtual ICollection<ContactDef> ContactDefListView { get; set; }
}
public class CertificateDefViewModel
{
    public int CTCID { get; set; }
    public byte CCCertID { get; set; }
    public string CCCert { get; set; }
    public byte ? CertStatus { get; set; }


}
public class SectorDefViewModel
{
    public int CTSID { get; set; }
    public short CDepSector { get; set; }
    public string CDepSectorNam { get; set; }
    public bool SectorChecked { get; set; }

}






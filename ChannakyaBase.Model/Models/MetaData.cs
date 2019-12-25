using ChannakyaAccounting.Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace ChannakyaBase.Model.Models
{
    public class CertificateDefMetaData
    {
        public byte CCCertID { get; set; }
        [Required(ErrorMessage = "Required Certificate Field")]
        
        [Display(Name = "Certificate Name")]
        [Remote("CheckCertificate", "CertificateDef", AdditionalFields = "CCCertID", ErrorMessage = "Duplicate Certificate Name !!")]

        public string CCCert { get; set; }
    }
    public class ContactDefMetaData
    {
        public byte CNotype { get; set; }
      
        [Display(Name = "Contact Type")]
        [Editable(true)]
        [Required(ErrorMessage = "Required Contact Type")]
        [Remote("CheckContactTypeDes", "ContactDef", AdditionalFields = "CNotype", ErrorMessage = "Duplicate Contact Type Name !!")]
        public string CNodesc { get; set; }
        
        [Display(Name = "Abbreviation")]
        [Required(ErrorMessage ="Required Contact Abbreviation")]
        [Remote("CheckContactTypeAb", "ContactDef", AdditionalFields = "CNotype", ErrorMessage = "Duplicate Contact Type Abbreviation !!")]
        [StringLength(3, ErrorMessage = "Abbreviation cannot exceed 3 characters.")]
        //[Remote("CheckContactType", "ContactDef", ErrorMessage = "duplicate", AdditionalFields = "CNodesc")]
        public string CNoabb { get; set; }

    }


    public class OccupationDefMetaData
    {
        public short Occpn { get; set; }
        [Display(Name = "Occupation Title")]
        [Required(ErrorMessage = "Required Occupation Feild")]
        [Remote("CheckOccupation", "OccupationDef", AdditionalFields = "Occpn", ErrorMessage = "Duplicate Occupation Type !!")]
        public string occupation { get; set; }
    }
    public class SectorDefMetaData
    {
        public short CDepSector { get; set; }
       
        [Required(ErrorMessage = "Required Sector Field")]
        [Remote("CheckSector", "SectorDef", AdditionalFields = "CDepSector", ErrorMessage = "Duplicate Sector Type !!")]
        [Display(Name = "Sector Title")]
        public string CDepSectorNam { get; set; }


    }
    public class TaxsetupDefMetadata
    {
        public byte TaxID { get; set; }
        [Display(Name = "Tax Title")]
        [Required(ErrorMessage = "Required Feild")]
        [Remote("CheckTaxTypeDef", "TaxsetupDef", AdditionalFields = "TaxID", ErrorMessage = "Duplicate TaxType Type !!")]
        public string TaxName { get; set; }
        public float TaxRate { get; set; }

    }
    public class CustTypeMetadata
    {
        [DisplayName("Customer Type")]
        public byte CtypeID { get; set; }
        [Display(Name = "Customer Type")]
        public string Ctype { get; set; }
        [Display(Name = "Tax Type")]
        [Required(ErrorMessage = "Required field")]
        public byte TaxID { get; set; }
        [Display(Name = "Is Individual")]
        public byte isind { get; set; }

        // public IEnumerable<SelectListItem> TaxsetupDef { get; set; }
    }

    public class CustInfoMetadata
    {
        public decimal CID { get; set; }

        [DisplayName("CustomerType")]
        public byte CtypeID { get; set; }
        [Display(Name = "Sector")]
        [Required(ErrorMessage = "Please choose the sector needed.")]
        public short CDepSector { get; set; }


        [Display(Name = "Reg. Date")]
        public System.DateTime CRegdate { get; set; }
        [Display(Name = "Certificate")]
        [Required(ErrorMessage = "Please choose the certificate type needed.")]
        public short CCCertID { get; set; }
        [Display(Name = "Certificate No.")]
        [Required(ErrorMessage = "CertificateNo. cannot be empty.")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Certifiacte Number not in range")]
        public string CCCertno { get; set; }
        public bool IsIncomplete { get; set; }
 
        [Display(Name = "Date Of Birth")]

        public Nullable<System.DateTime> DoB { get; set; }
        public Nullable<int> Postedby { get; set; }
        public Nullable<int> Appby { get; set; }
        [Display(Name = "PAN No.")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Value should be greater than or equal to 1")]
        public string PANNo { get; set; }
        public string Street { get; set; }
        //public bool custTypeDel { get; set; }
        
    }
    public class CustContactMetadata
    {

        public decimal CID { get; set; }
        [Display(Name = "Contact Type")]
        public byte CNotype { get; set; }
        [Display(Name = "Contact Details")]
        [Required(ErrorMessage = "Contact Number is Required.")]
        public string CNo { get; set; }
        [NotMapped]
        [Display(Name = "Is Default")]
        public bool IsDefault { get; set; }
    }
    public class CustIndividualMetadata
    {

        public decimal CID { get; set; }
        [Display(Name = "First Name")]
        //[Required(ErrorMessage = "First Name cannot be empty.")]
        public string Fname { get; set; }
        [Display(Name = "Middle Name")]
        public string Mname { get; set; }
        [Display(Name = "Last Name")]
        // [Required(ErrorMessage = "Last Name cannot be empty.")]
        public string Lname { get; set; }
        public byte Gender { get; set; }
        [Display(Name = "Occupation")]
        public short Occpn { get; set; }
        [Display(Name = "GrandFather")]
        public string GFatherName { get; set; }
        [Display(Name = "Father")]
        public string FatherName { get; set; }
        [Display(Name = "Spouse")]
        public string SpouseName { get; set; }
       
    }
    public class CustCompanyMetadata
    {
        public decimal CID { get; set; }
        [Required(ErrorMessage = "Company Name cannot be empty.")]
        [Display(Name = "Customer Company")]
        [Remote("CheckCustomerCompany", "Customer", AdditionalFields = "CCGroupID,CCGroupName", ErrorMessage = "Duplicate Customer Company !!")]
        public string CCName { get; set; }
        [Display(Name = "Company Group")]
        public string CCGroupID { get; set; }

        //public string CCPerson { get; set; }
        //public string CCNo { get; set; }

    }
    //public class CustCompGroupMetadata
    //{
    //    public short CCGroupID { get; set; }
    //    [Required(ErrorMessage = "Company Name cannot be empty.")]
    //    [Remote("CheckCustomerCompany", "Customer", AdditionalFields = "CCGroupID,CCGroupName", ErrorMessage = "Duplicate Customer Company !!")]
    //    public string CCGroupName { get; set; }
    //}
    
        public class CustCertificateMetadata
    {
        public decimal CID { get; set; }
        public int CCertID { get; set; }
        public byte CCCertID { get; set; }


    }
    public partial class CustTypeCertificateMetadata
    {
        [Required(ErrorMessage = "Please select the certificate.")]
        public bool isSubmitted { get; set; }
    }

    public partial class CustContactPersonMetadata
    {
        public int CPId { get; set; }
        public decimal CID { get; set; }

        [Required(ErrorMessage = "Cotact Person cannot be empty.")]
        public string CPName { get; set; }

        [Required(ErrorMessage = "Contact Number be empty.")]
        public string CPCNo { get; set; }

    }
    public partial class LicenseBranchMetadata
    {
        public short BrnhID { get; set; }

        [Remote("IsBranchNameExist", "FinanceParameter", AdditionalFields = "BrnhID", ErrorMessage = "Branch Name already exists")]
        [Display(Name = "Branch Name")]
        [Required(ErrorMessage = "Branch Name is Required")]
     
        public string BrnhNam { get; set; }

        [Display(Name = "Address")]
        [Required(ErrorMessage = "Address is Required")]
        public string Addr { get; set; }

        [Required(ErrorMessage = "Region is Required")]
        [Display(Name = "Region")]
        public Nullable<short> RegID { get; set; }

        [Required(ErrorMessage = "Phone Number is Required")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Not a valid phone number")]
        public string Phone { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Not a valid fax number")]
        public string Fax { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "IP is Required")]
        [Display(Name = "IP Address")]
        //[RegularExpression(@"^(([1-9]?\d|1\d\d|2[0-5][0-5]|2[0-4]\d)\.){3}([1-9]?\d|1\d\d|2[0-5][0-5]|2[0-4]\d)$", ErrorMessage = "Not a valid IP Address")]
        public string IPAdd { get; set; }

        [Display(Name = "Parent Branch")]
        [Required(ErrorMessage = "Parent Branch is Required")]
        public Nullable<short> PBrnhID { get; set; }

        [Required(ErrorMessage = "Additional User is Required")]
        [Display(Name = "Additional User")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Additional user must be numeric")]
        public Nullable<byte> ExtraUsrNo { get; set; }

        [Required(ErrorMessage = "Prefix is Required")]
        public string Prefix { get; set; }

        public int CalSID { get; set; }

        public int CalCid { get; set; }

        [Required(ErrorMessage = "Transaction Date is Required")]
        public System.DateTime TDate { get; set; }

        public bool UseLimit { get; set; }

        public bool atclosing { get; set; }

        [Required(ErrorMessage = "Migration Date is Required")]
        public Nullable<System.DateTime> MigDate { get; set; }

        [Required(ErrorMessage = "Clearabce Code is Required")]
        [Display(Name = "Clearance Code")]
        public string CleCode { get; set; }

        [Required(ErrorMessage = "InExpMode is Required")]
        public Nullable<byte> inExpMode { get; set; }

        [Required(ErrorMessage = "Floint is Required")]
        [Range(0.01, 99999999999, ErrorMessage = "Floint must be greater than 0.00")]
        public Nullable<decimal> Floint { get; set; }


        public bool IsCalcIOnI { get; set; }

        [Required(ErrorMessage = "State is Required")]
        public string State { get; set; }
    }
    public partial class ProductTIDMetadata
    {
        public int PFIID { get; set; }
        [Display(Name = "Product Name")]
        [Required(ErrorMessage = "Required Feild")]
        public byte PID { get; set; }
        [Remote("CheckProductInterest", "FinanceParameter", AdditionalFields = "PID,TID", ErrorMessage = "Already Mapped !!!")]
        [Display(Name = "Template Name")]
        public byte TID { get; set; }

       
    }
    public partial class RemittanceCustomerMetadata
    {
        public int RID { get; set; }
        public decimal CID { get; set; }
        [Remote("CheckRemittanceMap", "FinanceParameter", AdditionalFields = "CID", ErrorMessage = "Already Mapped !!!")]
        public int FID { get; set; }
    }
    public partial class ChargeDetailMetadata
    {
        public int ChgID { get; set; }
        [Display(Name = "Charge Name")]
        [Required(ErrorMessage = "Required Feild")]
        [Remote("ChargeNameExist", "FinanceParameter", AdditionalFields = "ChgID", ErrorMessage = "Charged Name already exist")]
        public string ChgName { get; set; }
        [Display(Name = "Charge Ledger")]
        public int FID { get; set; }
        [Display(Name = "Module")]
        public byte Modules { get; set; }
        public Nullable<byte> Product { get; set; }
        public Nullable<byte> ModEveID { get; set; }
        [Display(Name = "Trigger Type")]
        public byte Triggertype { get; set; }
        [Display(Name = "Charge Type")]
        public byte ChrType { get; set; }
        //[Required(ErrorMessage ="This field is required")]
        [Display(Name = "Ratio (%)")]
        public Nullable<float> Ratio { get; set; }
       // [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Charge Amount")]
        public Nullable<decimal> CAmount { get; set; }
        public Nullable<int> ChrTempID { get; set; }
        [Display(Name = "Is Changeable")]
        public bool Chngble { get; set; }
        [Display(Name = "Is Direct")]

        public bool ChrDirect { get; set; }
       // public decimal AmountCharged { get; set; }

        

    }

    public partial class PolicyIntCalcMetadata
    {
        public byte PSID { get; set; }
        [Required(ErrorMessage = "Required Field")]
       

        [Remote("CheckPolicyName","TransactionSetup",AdditionalFields = "PSID",ErrorMessage ="Penalty Policy Already Exists!")]
        public string PSName { get; set; }
        [Required(ErrorMessage = "Required Field")]
        public byte RID { get; set; }
        [Required(ErrorMessage = "Required Field")]
        public byte BALID { get; set; }
        [Required(ErrorMessage = "Required Field")]
        public byte DURID { get; set; }
    }

    public partial class PolicyPenCalcMetadata
    {

        public byte PCID { get; set; }
        [Required(ErrorMessage = "Required Field")]
       
        [Remote("CheckPenaltyPolicyName","TransactionSetup", AdditionalFields = "PCID", ErrorMessage ="Penalty Policy Name Already Exists")]
        public string PCNAME { get; set; }

        [Required(ErrorMessage = "Required Field")]
        public byte RID { get; set; }
        [Required(ErrorMessage = "Required Field")]
        public byte PBALID { get; set; }
        [Required(ErrorMessage = "Required Field")]
        public byte DURID { get; set; }
    }
    public partial class TempIntRateMetadata
    {
        public byte TID { get; set; }

        [Remote("CheckTemplateName", "TransactionSetup", AdditionalFields = "TID", ErrorMessage = "Template name already exists !!")]
        [Display(Name = "Template Name")]
        public string Tname { get; set; }
    }

    public partial class TempIntRateValMetadata
    {
      
        [Required(ErrorMessage = "Required Field")]
        [Display(Name = "Template Name")]
        public byte TID { get; set; }
     
        [Required(ErrorMessage = "Required Field")]
        [Display(Name = "Interest Rate")]
        public float IRate { get; set; }
        [Display(Name = "Upper Limit Amount")]
       
        [Required(ErrorMessage = "Required Field")]
        public decimal ULAmt { get; set; }
    }

    public partial class LocationMetadata
    {
        

        public int LId { get; set; }

      
        [Required(ErrorMessage = "Required Location Field")]
        [Display(Name = "Location Name")]
        //[Remote("CheckLocation", "Location", AdditionalFields = "LId", ErrorMessage = "Duplicate Location Name !!")]
        public string LocationName { get; set; }
        public Nullable<int> PLId { get; set; }
        public byte Lvl { get; set; }
        public bool IsGroup { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustAddress> CustAddresses { get; set; }
  //      public virtual LocationMaster LocationMaster { get; set; }
    }
}
using ChannakyaBase.Model.Models;
using ChannakyaBase.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChannakyaBase.DAL.DatabaseModel
{
    [MetadataType(typeof(CertificateDefMetaData))]
    public partial class CertificateDef
    {
    }
    [MetadataType(typeof(ContactDefMetaData))]
    public partial class ContactDef
    {
    }
    [MetadataType(typeof(OccupationDefMetaData))]
    public partial class OccupationDef
    {

    }
    [MetadataType(typeof(SectorDefMetaData))]
    public partial class SectorDef
    {

    }
    [MetadataType(typeof(TaxsetupDefMetadata))]
    public partial class TaxsetupDef
    {

    }
    [MetadataType(typeof(ProductTIDMetadata))]
    public partial class Productsetup
    {

    }
    [MetadataType(typeof(CustTypeMetadata))]
    public partial class CustType
    {

    }
    [MetadataType(typeof(CustInfoMetadata))]
    public partial class CustInfo
    {
        public List<CustInfo> CustomerInfo { get; set; }
        public List<CustTypeCertificate> CustTypeCertificate { get; set; }
        public List<CustomerAccountsViewModel> CustomerAccounts { get; set; }
    }
    [MetadataType(typeof(CustContactMetadata))]
    public partial class CustContact
    {
        [NotMapped]
        public bool IsDefault { get; set; }
        public byte CtypeId { get; set; }
        public bool IsDeleted { get; set; }
        public List<CustContact> CustContList { get; set; }
    }
    [MetadataType(typeof(CustIndividualMetadata))]
    public partial class CustIndividual
    {

    }


    [MetadataType(typeof(FinAcc))]
    public partial class FinAcc
    {

    }
    [MetadataType(typeof(CustCompanyMetadata))]
    public partial class CustCompany
    {

    }
    //[MetadataType(typeof(CustCompGroupMetadata))]
    //public partial class CustCompGroup
    //{

    //}


    public partial class CustAddress
    {
        public int AddressTypeId { get; set; }
        public string LType { get; set; }
        public List<LocationTypeDef> LocationDefinition { get; set; }
    }
    [MetadataType(typeof(CustTypeCertificate))]
    public partial class CustTypeCertificate
    {
        public int CCertID { get; set; }
        public bool isSubmitted { get; set; }
    }
    [MetadataType(typeof(CustContactPersonMetadata))]
    public partial class CustContactPerson
    {
        public bool CPDeleted { get; set; }
    }
    #region Account Open
    public partial class ADetail
    {
        public List<CustomerAccountsViewModel> AccountsWiseCustomer { get; set; }
        public List<StatusChangeLogModel> StatusLogList { get; set; }
    }

    [MetadataType(typeof(ANomineeMetaData))]
    public partial class ANominee
    {
        public string CertificateName { get; set; }
    }

    public partial class ANomAcc
    {

    }
    [MetadataType(typeof(AOfCustMetaData))]
    public partial class AOfCust
    {

    }
    [MetadataType(typeof(ReferenceInfoMetaData))]
    public partial class ReferenceInfo
    {
    }
    #endregion
    [MetadataType(typeof(LicenseBranchMetadata))]
    public partial class LicenseBranch
    {

    }
    [MetadataType(typeof(ProductTIDMetadata))]
    public partial class ProductTID
    {

    }
    [MetadataType(typeof(RemittanceCustomerMetadata))]
    public partial class RemittanceCustomer
    {

    }
    [MetadataType(typeof(ChargeDetailMetadata))]
    public partial class ChargeDetail
    {
        public decimal AmountChargedd { get; set; }
  //      public decimal TotalCharge { get; set; }
    }
    //public class PolicyIntCalcMetadata
    //{
    //    public byte PSID { get; set; }
    //    [Required(ErrorMessage = "Required Feild")]
    //    public string PSName { get; set; }
    //    [Required(ErrorMessage = "Required Feild")]
    //    public byte RID { get; set; }
    //    [Required(ErrorMessage = "Required Feild")]
    //    public byte BALID { get; set; }
    //    [Required(ErrorMessage = "Required Feild")]
    //    public byte DURID { get; set; }
    //}

    [MetadataType(typeof(PolicyIntCalcMetadata))]
    public partial class PolicyIntCalc
    {

    }
    [MetadataType(typeof(PolicyPenCalcMetadata))]
    public partial class PolicyPenCalc
    {

    }
    [MetadataType(typeof(TempIntRateMetadata))]
    public partial class TempIntRate
    {

    }
    [MetadataType(typeof(TempIntRateValMetadata))]
    public partial class TempIntRateVal
    {

    }

    [MetadataType(typeof(LocationMetadata))]
    public partial class Location
    {

    }
}

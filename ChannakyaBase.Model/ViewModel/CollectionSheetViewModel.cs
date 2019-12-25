
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ChannakyaBase.Model.ViewModel
{
    public class CollectionSheetViewModel
    {
        public int Id { get; set; }

        [Remote("CheckSheetNo", "Teller", AdditionalFields = "RetId", ErrorMessage = "Duplicate Sheet Number In Current Fiscal Year!!")]
        public int SheetNo { get; set; }

        [Required(ErrorMessage = "Please Choose Collector")]
        public Int64 CollectorId { get; set; }
        public string COllectorName { get; set; }
        public System.DateTime TDate { get; set; }
        public System.DateTime VDate { get; set; }
        public int PostedBy { get; set; }
        public Nullable<int> ApprovedBy { get; set; }
        public int BrchId { get; set; }
        [Required(ErrorMessage = "Please fill Collection Amount")]
        public decimal TotalAmount { get; set; }
        public string note { get; set; }

        public int IAccNo { get; set; }
        public int TNo { get; set; }
        public int SType { get; set; }
        [Required(ErrorMessage = "Please fill Amount")]
       // [Range(0, float.MaxValue, ErrorMessage = "Please enter valid float Number")]
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public decimal TempTotal { get; set; }
        public int RetId { get; set; }
        public long CollSheetId { get; set; }
        public List<CollectedAccountSheet> CollectedAccountSheet { get; set; }
        public List<CollectedAccountSheetSummary> CollectedAccountSheetSummary { get; set; }
        public Nullable<int> RejectedBy { get; set; }
        public bool Status { get; set; }
        public string RejectedRemarks { get; set; }
        public int TaskId { get; set; }

    }

    public class SmallCollectionSheetViewModel
    {
        public int Id { get; set; }

        [Remote("CheckSheetNo", "Teller", AdditionalFields = "RetId", ErrorMessage = "Duplicate Sheet Number In Current Fiscal Year!!")]
        public int SheetNo { get; set; }

        [Required(ErrorMessage = "Please Choose Collector")]
        public int CollectorId { get; set; }
        public string COllectorName { get; set; }
        public System.DateTime TDate { get; set; }
        public System.DateTime VDate { get; set; }
        public int PostedBy { get; set; }
        public Nullable<int> ApprovedBy { get; set; }
        public int BrchId { get; set; }
        [Required(ErrorMessage = "Please fill Collection Amount")]
        public decimal TotalAmount { get; set; }
        public string note { get; set; }

        public int IAccNo { get; set; }
        public int TNo { get; set; }
        public int SType { get; set; }
        [Required(ErrorMessage = "Please fill Amount")]
        // [Range(0, float.MaxValue, ErrorMessage = "Please enter valid float Number")]
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public decimal TempTotal { get; set; }
        public int RetId { get; set; }
        public long CollSheetId { get; set; }
        public List<CollectedAccountSheet> CollectedAccountSheet { get; set; }
        public List<CollectedAccountSheetSummary> CollectedAccountSheetSummary { get; set; }
        public Nullable<int> RejectedBy { get; set; }
        public bool Status { get; set; }
        public string RejectedRemarks { get; set; }
        public int TaskId { get; set; }

    }

    public class CollectedAccountSheet
    {
        public int Id { get; set; }
        public int IAccNo { get; set; }
        public Int64 TNo { get; set; }
        public int SType { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string AccountNumber { get; set; }
        public string Name { get; set; }
        public List<CollectedAccountSheet> CollectedAccountSheetList { get; set; }
        public int CollectorId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TempTotal { get; set; }
        public int RetId { get; set; }
        public bool IsSelected { get; set; }
        public Int64 CollSheetId { get; set; }
        public System.DateTime TDate { get; set; }
        public System.DateTime VDate { get; set; }
        public string note { get; set; }
        public int SheetNo { get; set; }
    }
    public class CollectedAccountSheetSummary
    {
        public int NoOfAccount { get; set; }
        public decimal Amount { get; set; }
        public string PName { get; set; }
        public int RetId { get; set; }
        public List<CollectedAccountSheetSummary> CollectedAccountSheetSummaryList { get; set; }
    }
    public class CollectSheetNumber
    {
        public int SheetNo { get; set; }
    }
}

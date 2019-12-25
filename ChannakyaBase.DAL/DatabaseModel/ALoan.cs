//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ChannakyaBase.DAL.DatabaseModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class ALoan
    {
        public int IAccno { get; set; }
        public byte PAYID { get; set; }
        public byte PAYSID { get; set; }
        public int PFreq { get; set; }
        public int IFreq { get; set; }
        public Nullable<byte> cDay { get; set; }
        public bool overPay { get; set; }
        public Nullable<decimal> OthrBal { get; set; }
        public Nullable<decimal> IonPA { get; set; }
        public Nullable<decimal> IonPR { get; set; }
        public Nullable<decimal> IonCA { get; set; }
        public Nullable<decimal> IonCR { get; set; }
        public Nullable<decimal> PonPrA { get; set; }
        public Nullable<decimal> PonPrR { get; set; }
        public Nullable<decimal> PonIA { get; set; }
        public Nullable<decimal> PonIR { get; set; }
        public Nullable<decimal> IonIR { get; set; }
        public Nullable<decimal> IonIA { get; set; }
        public bool IsInsured { get; set; }
        public bool AutoPayment { get; set; }
        public Nullable<short> penGDys { get; set; }
        public Nullable<bool> Revolving { get; set; }
        public Nullable<bool> deprived { get; set; }
        public int PostedBy { get; set; }
        public System.DateTime PostedOn { get; set; }
        public Nullable<int> ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedOn { get; set; }
        public Nullable<decimal> PrincipleLoanOut { get; set; }
        public Nullable<decimal> OtherLoanOut { get; set; }
        public Nullable<bool> IsChargeOnSancation { get; set; }
        public Nullable<System.DateTime> LastTotalInterestPaidDate { get; set; }
    
        public virtual ADetail ADetail { get; set; }
    }
}

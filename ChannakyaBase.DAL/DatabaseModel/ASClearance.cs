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
    
    public partial class ASClearance
    {
        public int rno { get; set; }
        public int IAccno { get; set; }
        public string Bankname { get; set; }
        public string Brnhname { get; set; }
        public string chqno { get; set; }
        public string payee { get; set; }
        public System.DateTime tdate { get; set; }
        public decimal camount { get; set; }
        public string remarks { get; set; }
        public int postedby { get; set; }
        public string accno { get; set; }
        public System.DateTime VDate { get; set; }
        public System.DateTime Postedon { get; set; }
        public Nullable<bool> chqstate { get; set; }
        public Nullable<int> BrchId { get; set; }
        public string RejectRemarks { get; set; }
    }
}

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
    
    public partial class ShrReturn
    {
        public decimal Regno { get; set; }
        public decimal SCrtno { get; set; }
        public Nullable<decimal> SoldTo { get; set; }
        public decimal Tno { get; set; }
        public System.DateTime Sdate { get; set; }
        public decimal SQty { get; set; }
        public Nullable<decimal> Amt { get; set; }
        public string Note { get; set; }
        public Nullable<int> PostedBy { get; set; }
        public int ApprovedBy { get; set; }
        public int ttype { get; set; }
        public byte brchid { get; set; }
        public Nullable<int> Vno { get; set; }
    }
}

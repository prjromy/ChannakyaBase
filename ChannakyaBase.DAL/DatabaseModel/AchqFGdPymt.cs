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
    
    public partial class AchqFGdPymt
    {
        public decimal tno { get; set; }
        public int IAccno { get; set; }
        public int brnhid { get; set; }
        public int Chqno { get; set; }
        public string notes { get; set; }
        public decimal amount { get; set; }
        public System.DateTime tdate { get; set; }
        public int postedby { get; set; }
        public Nullable<int> approvedby { get; set; }
        public byte tstate { get; set; }
        public string payee { get; set; }
        public Nullable<bool> IsRejected { get; set; }
    
        public virtual ADetail ADetail { get; set; }
        public virtual Company Company { get; set; }
    }
}
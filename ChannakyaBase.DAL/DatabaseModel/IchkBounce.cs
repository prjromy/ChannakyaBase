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
    
    public partial class IchkBounce
    {
        public int IAccno { get; set; }
        public decimal Chkno { get; set; }
        public string Rmks { get; set; }
        public System.DateTime TDate { get; set; }
        public int PostedBy { get; set; }
        public System.DateTime Postedon { get; set; }
    
        public virtual ADetail ADetail { get; set; }
    }
}

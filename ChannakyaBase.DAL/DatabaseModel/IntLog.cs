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
    
    public partial class IntLog
    {
        public System.DateTime Tdate { get; set; }
        public int IAccno { get; set; }
        public byte Fday { get; set; }
        public decimal Balance { get; set; }
        public float Rate { get; set; }
        public Nullable<double> Intcal { get; set; }
        public byte Valued { get; set; }
    
        public virtual ADetail ADetail { get; set; }
    }
}

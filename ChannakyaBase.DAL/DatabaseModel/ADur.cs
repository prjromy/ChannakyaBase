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
    
    public partial class ADur
    {
        public int IAccno { get; set; }
        public Nullable<float> Days { get; set; }
        public Nullable<int> Month { get; set; }
        public Nullable<System.DateTime> ValDat { get; set; }
        public bool IsOD { get; set; }
        public Nullable<System.DateTime> MatDat { get; set; }
        public Nullable<int> DurType { get; set; }
        public Nullable<int> DurationId { get; set; }
    
        public virtual ADetail ADetail { get; set; }
    }
}
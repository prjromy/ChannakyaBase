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
    
    public partial class ALFixedDeposit
    {
        public int AlFixedId { get; set; }
        public int Sno { get; set; }
        public bool IsInternal { get; set; }
        public string fdAccno { get; set; }
        public Nullable<decimal> amount { get; set; }
        public Nullable<System.DateTime> openDt { get; set; }
        public Nullable<System.DateTime> matDt { get; set; }
        public string bank { get; set; }
        public string brnh { get; set; }
    
        public virtual ALColl ALColl { get; set; }
    }
}
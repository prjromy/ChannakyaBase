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
    
    public partial class ARate
    {
        public int IAccno { get; set; }
        public float IRate { get; set; }
        public int ARMID { get; set; }
        public int IRateId { get; set; }
    
        public virtual ADetail ADetail { get; set; }
        public virtual ARateMaster ARateMaster { get; set; }
    }
}

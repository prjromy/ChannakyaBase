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
    
    public partial class ANomAcc
    {
        public int NAId { get; set; }
        public int IAccno { get; set; }
        public int NIAccno { get; set; }
        public Nullable<bool> TransOnMat { get; set; }
    
        public virtual ADetail ADetail { get; set; }
    }
}
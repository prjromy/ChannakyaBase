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
    
    public partial class AIntExpens
    {
        public int Iaccno { get; set; }
        public System.DateTime Tdate { get; set; }
        public decimal IntAmt { get; set; }
        public byte valued { get; set; }
        public Nullable<int> VNo { get; set; }
    
        public virtual ADetail ADetail { get; set; }
    }
}

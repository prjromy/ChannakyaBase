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
    
    public partial class TempIntRateVal
    {
        public int TIRID { get; set; }
        public byte TID { get; set; }
        public float IRate { get; set; }
        public decimal ULAmt { get; set; }
    
        public virtual TempIntRate TempIntRate { get; set; }
    }
}

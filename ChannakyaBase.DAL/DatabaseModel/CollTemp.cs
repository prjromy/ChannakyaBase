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
    
    public partial class CollTemp
    {
        public int Id { get; set; }
        public int RetId { get; set; }
        public int IAccNo { get; set; }
        public int FId { get; set; }
        public int SType { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
    
        public virtual CollMainTemp CollMainTemp { get; set; }
    }
}

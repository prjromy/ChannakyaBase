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
    
    public partial class ALRegCust
    {
        public int ALRegID { get; set; }
        public int RegId { get; set; }
        public Nullable<int> CID { get; set; }
        public Nullable<int> SNo { get; set; }
    
        public virtual ALRegistration ALRegistration { get; set; }
    }
}

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
    
    public partial class ChqRqst
    {
        public int Rno { get; set; }
        public int Iaccno { get; set; }
        public int Pics { get; set; }
        public System.DateTime Tdate { get; set; }
        public int PostedBy { get; set; }
    
        public virtual ADetail ADetail { get; set; }
    }
}
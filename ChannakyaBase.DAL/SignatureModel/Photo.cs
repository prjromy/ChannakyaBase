//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ChannakyaBase.DAL.SignatureModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class Photo
    {
        public int PID { get; set; }
        public Nullable<int> Cid { get; set; }
        public byte[] Image { get; set; }
        public Nullable<System.DateTime> UploadedOn { get; set; }
        public Nullable<int> UploadedBy { get; set; }
        public Nullable<bool> Status { get; set; }
    }
}
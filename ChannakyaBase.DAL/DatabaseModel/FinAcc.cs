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
    
    public partial class FinAcc
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FinAcc()
        {
            this.ProductDetails = new HashSet<ProductDetail>();
            this.RemittanceCustomers = new HashSet<RemittanceCustomer>();
            this.SchmDetails = new HashSet<SchmDetail>();
            this.ProductVfins = new HashSet<ProductVfin>();
            this.SchemeVFins = new HashSet<SchemeVFin>();
        }
    
        public int Fid { get; set; }
        public string Fname { get; set; }
        public Nullable<int> Pid { get; set; }
        public int F2Type { get; set; }
        public byte[] Content { get; set; }
        public Nullable<bool> DebitRestriction { get; set; }
        public Nullable<bool> CreditRestriction { get; set; }
        public string Code { get; set; }
        public string Alias { get; set; }
    
        public virtual FinSys2 FinSys2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductDetail> ProductDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RemittanceCustomer> RemittanceCustomers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SchmDetail> SchmDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductVfin> ProductVfins { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SchemeVFin> SchemeVFins { get; set; }
    }
}

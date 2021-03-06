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
    
    public partial class DisbursementMain
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DisbursementMain()
        {
            this.DisburseCashes = new HashSet<DisburseCash>();
            this.DisburseCharges = new HashSet<DisburseCharge>();
            this.DisburseCheques = new HashSet<DisburseCheque>();
            this.DisburseDeposits = new HashSet<DisburseDeposit>();
            this.DisburseVsPIDs = new HashSet<DisburseVsPID>();
        }
    
        public int DisburseId { get; set; }
        public int IAccno { get; set; }
        public int Mode { get; set; }
        public Nullable<int> VNo { get; set; }
        public Nullable<System.DateTime> Tdate { get; set; }
        public System.DateTime PostedOn { get; set; }
        public Nullable<int> PostedBy { get; set; }
        public Nullable<System.DateTime> ApprovedOn { get; set; }
        public Nullable<int> ApprovedBy { get; set; }
        public Nullable<bool> CheckSanction { get; set; }
    
        public virtual ADetail ADetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DisburseCash> DisburseCashes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DisburseCharge> DisburseCharges { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DisburseCheque> DisburseCheques { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DisburseDeposit> DisburseDeposits { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DisburseVsPID> DisburseVsPIDs { get; set; }
    }
}

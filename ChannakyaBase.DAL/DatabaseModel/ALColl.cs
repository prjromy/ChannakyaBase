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
    
    public partial class ALColl
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ALColl()
        {
            this.ALCollLands = new HashSet<ALCollLand>();
            this.ALCollVehicles = new HashSet<ALCollVehicle>();
            this.ALFixedDeposits = new HashSet<ALFixedDeposit>();
        }
    
        public int Sno { get; set; }
        public int IAccno { get; set; }
        public int NCID { get; set; }
        public decimal CValue { get; set; }
        public Nullable<double> Weightage { get; set; }
        public string OName { get; set; }
        public string OAdd { get; set; }
        public string contactNo { get; set; }
        public string citizenshipNo { get; set; }
        public string RegNo { get; set; }
        public Nullable<System.DateTime> RegDate { get; set; }
        public string Remarks { get; set; }
        public bool IsAc { get; set; }
        public bool IsActive { get; set; }
        public int PostedBy { get; set; }
        public System.DateTime PostedOn { get; set; }
    
        public virtual ADetail ADetail { get; set; }
        public virtual NCollateralDetail NCollateralDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ALCollLand> ALCollLands { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ALCollVehicle> ALCollVehicles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ALFixedDeposit> ALFixedDeposits { get; set; }
    }
}

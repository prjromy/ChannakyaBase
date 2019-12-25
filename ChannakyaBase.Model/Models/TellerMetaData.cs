using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannakyaBase.Model.Models
{
  
    public class ANomineeMetaData
    {
        public int NID { get; set; }
        public int IAccno { get; set; }
        [Required]
        [Display(Name = "Nominee Name")]
        public string NomNam { get; set; }
        [Required]
        [Display(Name = "Nominee Relation")]
        public string NomRel { get; set; }
        [Required]
        [Display(Name = "Certificate")]
        public string CCertID { get; set; }
        [Display(Name = "CertificateNo")]
        public string CCertno { get; set; }
        [Required]
        [Display(Name = "Share")]
        public float Share { get; set; }

        [Required]
        [Display(Name = "Contact No")]
        public string ContactNo { get; set; }
       
        [Display(Name = "ContactAddress")]
        public string ContactAddress { get; set; }
    }

    public class AOfCustMetaData
    {
      
       
    }
    public  class ReferenceInfoMetaData
    {
        
    }
}

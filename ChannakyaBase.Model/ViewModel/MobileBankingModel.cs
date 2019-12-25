using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannakyaBase.Model.ViewModel
{
   public class MobileBankingModel
    {
        public long Tno { get; set; }
        public System.DateTime TDate { get; set; }
        public int IAccNO { get; set; }
        public int TType { get; set; }
        public string MobileNo { get; set; }
        public decimal Amount { get; set; }
        public System.DateTime PostedOn { get; set; }
        public string Remarks { get; set; }
        public Nullable<System.DateTime> SuccessOrFailureOn { get; set; }
        public Nullable<int> Vno { get; set; }
    }
}

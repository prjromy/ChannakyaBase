using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannakyaBase.Model.ViewModel
{
    public class CustomerAccountsViewModel
    {
        public decimal CID { get; set; }
        public string Accno { get; set; }
        public int IAccno { get; set; }
        public string PName { get; set; }
        public string AName { get; set; }
        public string AccountState { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public string location { get; set; }
        public string Ctype { get; set; }
        public byte CtypeID { get; set; }
        public string PANNo { get; set; }



    }


    public class CustomerViewModel
    {
        public decimal CID { get; set; }
        public byte CCCertID { get; set; }

        public bool isSubmitted { get; set; }
        public byte CTypeID { get; set; }

        public byte CCState { get; set; }
    }


    }

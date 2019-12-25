using ChannakyaBase.BLL.Repository;
using ChannakyaBase.DAL.DatabaseModel;
using ChannakyaBase.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ChannakyaBase.BLL.Service
{
    public static class ShareUtilityService
    {
        public static SelectList ShareType()
        {
            List<SelectListItem> objshareOption = new List<SelectListItem>();

            objshareOption.Add(new SelectListItem { Text = "Ordinary Share", Value = "1" });
            objshareOption.Add(new SelectListItem { Text = "Promoter share", Value = "2" });

            return new SelectList(objshareOption, "Value", "Text");
        }

        public static string GetShareRegNumber()
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                string regNo = "";
                var regDetails = uow.Repository<ReturnSingleValueModdel>().SqlQuery("select cast(isnull(max(RegNo),0)as int) as IdValue from fin.ShrReg  ").FirstOrDefault(); ;

                string regNumber = uow.Repository<ShrReg>().FindBy(x => x.RegNo == regDetails.IdValue).Select(x => x.RegistrationCode).FirstOrDefault();
                if (regNumber == "" || regNumber == null)
                {
                    regNo = "001";
                }
                else
                {
                    //string[] splitRegNumber = regNumber.Split('-');
                    //int givenRegNo = Convert.ToInt32(splitRegNumber[1]);
                    int finalnumber = Convert.ToInt32(regNumber) + 1;
                    if (finalnumber <= 9)
                    {
                        regNo = "00" + finalnumber;
                    }
                    else if (finalnumber >= 10 && finalnumber < 100)
                    {
                        regNo = "0" + finalnumber;
                    }
                    else
                    {
                        regNo = finalnumber.ToString();
                    }
                }
                return regNo;
            }
        }
    }
}

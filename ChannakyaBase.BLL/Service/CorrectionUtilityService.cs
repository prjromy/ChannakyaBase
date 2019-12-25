using ChannakyaBase.DAL.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ChannakyaBase.BLL.Service
{
    public class CorrectionUtilityService
    {
        public static CorrectionService correctionService = null;
        static ChannakyaBaseEntities _context = null;
        static CorrectionUtilityService()
        {
            correctionService = new CorrectionService();
            _context = new ChannakyaBaseEntities();

        }
        //public static SelectList SelectAllAdjustmentTypeById(int IAccNo)
        //{
        //    return new SelectList(CorrectionService.SelectAllAdjustmentTypeByID(IAccNo), "SelectedIndex", "Text ");
        //}

    }
}

using ChannakyaBase.BLL.Repository;
using ChannakyaBase.DAL.DatabaseModel;
using ChannakyaBase.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using static System.Drawing.Printing.PrinterSettings;

namespace ChannakyaBase.BLL.Service
{
    public static class InformationUtilityService
    {
        static CommonService commonService = null;
        static InformationUtilityService()
        {
            commonService = new CommonService();
        }
        public static decimal AvailableChequeInStock()
        {
            using (ChannakyaBaseEntities _context = new ChannakyaBaseEntities())
            {
                int userId = commonService.GetBranchIdByEmployeeUserId();
                decimal availableCheque = _context.Database.SqlQuery<decimal>("SELECT top 100 percent Tochqno - Lastindx AS Balance FROM fin.ChqInventory WHERE (Brnhid = " + userId + ") AND (Lastindx <> Tochqno) AND (ISfinish = 0) ORDER BY Lastindx").FirstOrDefault();
                return availableCheque;
            }
        }

        public static bool IsAllowChequeNumber(int productId)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var fixDeposit = (from s in uow.Repository<SchmDetail>().GetAll()
                                  join p in uow.Repository<ProductDetail>().FindBy(x => x.PID == productId) on s.SDID equals p.SDID
                                  select new SchemeModel()
                                  {
                                      IsCheque = p.HasCheque

                                  }).FirstOrDefault();

                if (fixDeposit.IsCheque == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static IEnumerable<SelectListItem> GetChequeStatus()
        {
            IList<SelectListItem> items = new List<SelectListItem>
            {
                new SelectListItem{Text = "Un Used", Value = "2"},
                new SelectListItem{Text = "Cheque Book Blocked", Value = "3"},
                new SelectListItem{Text = "Cheque Piece Blocked", Value = "4"},
                new SelectListItem{Text = "Deactive Cheque Book", Value = "5"},
            };
            return items;
        }

        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetDefaultPrinter(string Name);

        public static IEnumerable<SelectListItem> GetPrinterList()
        {
            PrinterSettings printerSettings = new PrinterSettings();
            StringCollection installedPrinters = PrinterSettings.InstalledPrinters;
            List<SelectListItem> get = new List<SelectListItem>();

            List<string> ips = new List<string>();
            foreach (string item in installedPrinters)
            {
                ips.Add(item);
            }

            for (int i = 0; i < installedPrinters.Count; i++)
            {
                get.Add(new SelectListItem() { Text = ips[i], Value = i.ToString() });
            }

            return get;

        }
        public static IEnumerable<SelectListItem> GetPrinterPaperSize()
        {
            PrinterSettings printerSettings = new PrinterSettings();
            InformationService informationService = new InformationService();
            var printerDB = informationService.GetPrinter();
            printerSettings.PrinterName = printerDB.DefaultPrinterName;
            var paperSizes = printerSettings.PaperSizes;

            List<SelectListItem> get = new List<SelectListItem>();

            for (int i = 0; i < paperSizes.Count; i++)
            {
                get.Add(new SelectListItem() { Text = paperSizes[i].PaperName, Value = i.ToString() });
            }

            return get;

        }
        public static IEnumerable<SelectListItem> GetPrinterPaperSize(string printerName)
        {
            PrinterSettings printerSettings = new PrinterSettings();
            //InformationService informationService = new InformationService();
            //var printerDB = informationService.GetPrinter();
            printerSettings.PrinterName = printerName;
            var paperSizes = printerSettings.PaperSizes;

            List<SelectListItem> get = new List<SelectListItem>();

            for (int i = 0; i < paperSizes.Count; i++)
            {
                get.Add(new SelectListItem() { Text = paperSizes[i].PaperName, Value = i.ToString() });
            }

            return get;

        }
        
        public static IEnumerable<SelectListItem> GetChequeApplyStatus()
        {
            IList<SelectListItem> items = new List<SelectListItem>
            {
               new SelectListItem{Text = "Cheque Book Blocked", Value = "3"},
                new SelectListItem{Text = "Cheque Piece Blocked", Value = "4"},
               new SelectListItem{Text = "Deactive Cheque Book", Value = "5"},
            };
            return items;
        }
        public static IEnumerable<SelectListItem> GetChequeActiveApplyStatus()
        {
            IList<SelectListItem> items = new List<SelectListItem>
            {
                 new SelectListItem{Text = "Un Block", Value = "2"},


            };
            return items;
        }

        public static string StatusCheckBook(int Sid)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                return uow.Repository<ChequeBookStatu>().FindBy(x => x.Id == Sid).Select(x => x.ChequeStatus).FirstOrDefault();

            }
        }

        public static bool IsChequeBounce(int iaccNo, int chqNo)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var cheqBounceInfo = uow.Repository<IchkBounce>().FindBy(x => x.Chkno == chqNo && x.IAccno == iaccNo).FirstOrDefault();

                if (cheqBounceInfo != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }
        public static string BounceReason(int iaccNo, int chqNo)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                string cheqBounceInfo = uow.Repository<IchkBounce>().FindBy(x => x.Chkno == chqNo && x.IAccno == iaccNo).Select(x => x.Rmks).FirstOrDefault();

                return cheqBounceInfo;
            }

        }
    }
}

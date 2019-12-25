using ChannakyaBase.BLL.Repository;
using ChannakyaBase.DAL.DatabaseModel;
using ChannakyaBase.Model.ViewModel;
using MoreLinq.Extensions;
//using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ChannakyaBase.BLL.Service
{
    public class FinanceParameterUtilityService
    {
        private static GenericUnitOfWork uow = null;
        static FinanceParameterUtilityService()
        {
            uow = new GenericUnitOfWork();
        }
        public static SelectList Region()
        {
            List<LicenseRegion> licenseRegion = uow.Repository<LicenseRegion>().GetAll().ToList();
            return new SelectList(licenseRegion, "RegID", "RegionNam");
        }
        public static string GetRegionByRegionId(int regID)
        {
            LicenseRegion licenseRegion = uow.Repository<LicenseRegion>().GetAll().Where(x => x.RegID == regID).FirstOrDefault(); ;
            return licenseRegion.RegionNam;
        }
        public static SelectList ParentBranch()
        {

            List<LicenseBranch> licensebranch = uow.Repository<LicenseBranch>().GetAll().ToList();
            return new SelectList(licensebranch, "BrnhID", "BrnhNam");

        }
        public static SelectList FloatingInterestList()
        {

            List<TempIntRateVal> tempIntRateValList = uow.Repository<TempIntRateVal>().GetAll().ToList();
            List<TempIntRate> tempIntRateList = uow.Repository<TempIntRate>().GetAll().ToList();

            var tempIntRateNameList = (from tir in tempIntRateList
                                       join tirval in tempIntRateValList on tir.TID equals tirval.TID

                                       select new TempIntRate
                                       {
                                           TID=tir.TID,
                                           Tname=tir.Tname
                                       }
                                     ).DistinctBy(x=>x.Tname).ToList();
            return new SelectList(tempIntRateNameList, "TID", "Tname");



        }
        public static SelectList ProductList()
        {
            var productDetail = uow.Repository<ProductDetail>().GetAll().Where(x=>x.enabled==true).ToList();
            var schmeDetail = uow.Repository<SchmDetail>().FindBy(x => x.SType == 0).ToList();
            var productlist = (from p in productDetail
                               join
                               s in schmeDetail on p.SDID equals s.SDID
                               select new ProductDetail()
                               {
                                   PID = p.PID,
                                   PName = p.PName
                               }).OrderBy(x=>x.PName).Distinct().ToList();
            return new SelectList(productlist, "PID", "Pname");
        }
        public static List<SelectListItem> ChargeMode(int ChrType = 0)
        {
            List<SelectListItem> options = new List<SelectListItem>();

            options.Add(new SelectListItem { Text = "Fixed", Value = "1", Selected = ChrType == 1 ? true : false });
            options.Add(new SelectListItem { Text = "Ratio of Charge", Value = "2", Selected = ChrType == 2 ? true : false });
            return options;
        }


        public static List<SelectListItem> TriggerType(int trigType = 0)
        {

            List<SelectListItem> options = new List<SelectListItem>();
            options.Add(new SelectListItem { Text = "Automatic", Value = "1", Selected = trigType == 1 ? true : false });
            options.Add(new SelectListItem { Text = "Manual", Value = "2", Selected = trigType == 2 ? true : false });
            return options;
        }

        public static SelectList ChargeLedger()
        {
            List<FinAcc> chargelegder = (from f in uow.Repository<FinAcc>().GetAll()
                                         join
                                         fin2 in uow.Repository<FinSys2>().FindBy(x => x.F1id == 237) on f.F2Type equals fin2.F2id
                                         select new FinAcc()
                                         {
                                             Fid = f.Fid,
                                             Fname = f.Fname
                                         }).ToList();
            return new SelectList(chargelegder, "Fid", "Fname");
        }
        public static string LedgerNameByFid(int fid)
        {
            return uow.Repository<FinAcc>().FindBy(x => x.Fid == fid).Select(x => x.Fname).FirstOrDefault().ToString();
        }
        public static string GetStateByBranchId(int BranchId)
        {
            if(BranchId == 0){
                return "";
            }
            else
            return uow.Repository<ChannakyaBase.DAL.DatabaseModel.Company>().FindBy(x => x.CompanyId == BranchId).Select(x => x.State).FirstOrDefault().ToString();
        }
        public static SelectList Modules()
        {
            List<Module> chargelegder = uow.Repository<Module>().GetAll().ToList();
            return new SelectList(chargelegder, "Modid", "ModName");
        }
        public static SelectList CustCompany()
        {
            List<CustCompany> chargelegder = uow.Repository<CustCompany>().GetAll().ToList();
            return new SelectList(chargelegder, "CID", "CCName");
        }

        public static SelectList RemittanceCompany()
        {
            List<RemittanceCustomer> remittanceCustomer = uow.Repository<RemittanceCustomer>().GetAll().ToList();
            List<CustCompany> custCompany = uow.Repository<CustCompany>().GetAll().ToList();

            List<SelectListItem> options = new List<SelectListItem>();

            //int val = 0;
            //foreach (var item in remittanceCompany)
            //{
            //    val++;
            //    var custName = uow.Repository<CustCompany>().FindBy(x=>x.CID ==item.CID).FirstOrDefault();
            //    options.Add(new SelectListItem { Text = custName.CCName, Value = val.ToString() });
            //}
            //return options;
            var remittanceCompany = (from u in remittanceCustomer
                                     join b in custCompany on u.CID equals b.CID
                                     
                                     select new RemitDepositModel
                                     {
                                         RemitCompanyId=u.RID,
                                         CompanyName=b.CCName
                                        
                                          
                                     }).DistinctBy(x=>x.CompanyName).ToList();

            return new SelectList(remittanceCompany, "RemitCompanyId", "CompanyName");

        }
        public static SelectList RemitanceLegderList()
        {
            List<FinAcc> chargelegder = (from f in uow.Repository<FinAcc>().GetAll()
                                         join
                                         fin2 in uow.Repository<FinSys2>().FindBy(x => x.F1id == 239) on f.F2Type equals fin2.F2id
                                         select new FinAcc()
                                         {
                                             Fid = f.Fid,
                                             Fname = f.Fname
                                         }).ToList();
            return new SelectList(chargelegder, "Fid", "Fname");
        }
     
        public static SelectList ModulesWiseEvents(int? moduleId)
        {

            List<Event> modulesWiseEvents = uow.Repository<Event>().FindBy(x => x.ModId == moduleId && x.EventType == 2).ToList();
            return new SelectList(modulesWiseEvents, "EventId", "EventName");
        }

        public static SelectList ModulesWiseProducts(byte stype)
        {
            List<SelectListItem> ProductList = new List<SelectListItem>();
            List<ProductViewModel> productList = new List<ProductViewModel>();
            if (stype == 1)
                stype = 0;
            else if (stype == 2)
                stype = 1;
            productList = uow.Repository<ProductViewModel>().SqlQuery(@"select cast(a.PID as int) as ProductId, PName as ProductName from fin.productdetail a inner join  fin.schmdetails b on a.sdid = b.sdid where b.stype =" + stype).ToList();
            return new SelectList(productList, "ProductId", "ProductName");
        }
        //public static SelectList ModulesWiseProducts(byte stype)
        //{
        //    List<SelectListItem> ProductList = new List<SelectListItem>();
        //    List<ProductViewModel> productList = new List<ProductViewModel>();
        //    productList.Add(new ProductViewModel() { ProductName = "ALL", ProductId = 0 });
        //    if (stype == 0 || stype == 3)
        //    {
        //        ProductList.Add(new SelectListItem { Text = "", Value = "0" });
        //        return new SelectList(ProductList, "Value", "Text");
        //    }
        //    if (stype == 1)
        //        stype = 0;
        //    else if (stype == 2)
        //        stype = 1;
        //    productList.AddRange(uow.Repository<ProductViewModel>().SqlQuery(@"select cast(a.PID as int) as ProductId, PName as ProductName from fin.productdetail a inner join  fin.schmdetails b on a.sdid = b.sdid where b.stype =" + stype).ToList());
        //    return new SelectList(productList, "ProductId", "ProductName");
        //}
        public static Int64? ChargeTranxNoByEventIdAndEventValue(Int64 eventId, Int64 eventValue)
        {
            var chargeList = uow.Repository<ChannakyaBase.Model.ViewModel.ChargeTransactionViewModel>().SqlQuery(@"select TrnxNo from Trans.ChgSTrnx a  inner join fin.chargedetail b on a.ChgId=b.ChgID where Iaccno=" + eventValue + " and Triggertype =" + 1 + " and ModEveID =" + eventId).FirstOrDefault();
            if (chargeList != null)
                return chargeList.TrnxNo;
            else return 0;
        }
        public static SelectList chargesByModules(int? moduleId)
        {
            List<ChargeDetail> modulesWiseEvents = uow.Repository<ChargeDetail>().FindBy(x => x.Modules == moduleId && x.Triggertype == 2 && x.Status == true).ToList();
            return new SelectList(modulesWiseEvents, "ChgID", "ChgName");
        }

        public static SelectList GetCashLimitUserList(int employeeId)
        {
            FinanceParameterService fncService = new FinanceParameterService();
            var cashLimitService = fncService.GetUserRoleByEmployeeId(employeeId);
            return new SelectList(cashLimitService, "UserId", "UserName");
        }


        public static SelectList DenoSetupList()
        {


            //foreach (var item in denoDetail)
            //{
            //    item.CurrencyName = item.CurrencyName + "(" + item.Country + ")";
            //}



            return new SelectList((from s in uow.Repository<CurrencyType>().GetAll()
                                   select new
                                   {
                                       CurrencyName = s.CurrencyName + "(" + s.Country + ")",

                                       CTId = s.CTId
                                   }), "CTId", "CurrencyName");
        }

    }
}

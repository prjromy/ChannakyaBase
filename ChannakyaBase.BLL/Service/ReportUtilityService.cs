using ChannakyaBase.BLL.Repository;
using ChannakyaBase.DAL.DatabaseModel;
using ChannakyaBase.Model.ViewModel;
using Loader.Models;
using Loader.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ChannakyaBase.BLL.Service
{
    public static class ReportUtilityService
    {
        static CommonService commonService = null;

        static ReportUtilityService()
        {
            commonService = new CommonService();
        }
        public static SelectList AccountType()
        {
            List<SelectListItem> objAccountType = new List<SelectListItem>();
            if (AllowBoth())
            {

                objAccountType.Add(new SelectListItem { Text = "All", Value = "0" });
                objAccountType.Add(new SelectListItem { Text = "Deposit", Value = "2" });
                objAccountType.Add(new SelectListItem { Text = "Loan", Value = "3" });
            }
            else if (AllowDeposit())
            {
                objAccountType.Add(new SelectListItem { Text = "Deposit", Value = "2" });
            }
            else if (AllowLoan())
            {
                objAccountType.Add(new SelectListItem { Text = "Loan", Value = "3" });
            }



            return new SelectList(objAccountType, "Value", "Text");
        }



        public static SelectList GetBranchList(int branchId = 0)
        {
            if (branchId == 0)
            {
                BranchSetupService branchService = new BranchSetupService();
                var branchList = branchService.GetAll().OrderBy(x => x.BranchName).ToList();
                //branchList.Add(new Loader.Models.Company() { CompanyId = 0, BranchName = "All" });
                branchList.Add(new Loader.Models.Company() { CompanyId = 0, BranchName = "All" });
                return new SelectList(branchList, "CompanyId", "BranchName");
            }
            else
            {

                BranchSetupService branchService = new BranchSetupService();
                branchId = commonService.GetBranchIdByEmployeeUserId();
                var branchList = branchService.GetAll().Where(x => x.CompanyId == branchId).ToList();
                return new SelectList(branchList, "CompanyId", "BranchName", branchId);
            }


        }
        public static SelectList GetBranchWithOutAllList(int branchId = 0)
        {
            if (branchId == 0)
            {
                BranchSetupService branchService = new BranchSetupService();
                var branchList = branchService.GetAll().ToList();
                return new SelectList(branchList, "CompanyId", "BranchName");
            }
            else
            {

                BranchSetupService branchService = new BranchSetupService();
                branchId = commonService.GetBranchIdByEmployeeUserId();
                var branchList = branchService.GetAll().Where(x => x.CompanyId == branchId).ToList();
                return new SelectList(branchList, "CompanyId", "BranchName", branchId);
            }


        }
        public static IEnumerable<SelectListItem> GetChequeStatus()
        {
            IList<SelectListItem> items = new List<SelectListItem>
            {
                new SelectListItem{Text = "Cheque Issue", Value = "1"},
                new SelectListItem{Text = "Cheque  Blocked", Value = "2"},
                new SelectListItem{Text = "Cheque Bounce", Value = "3"},
            };
            return items;
        }
        public static APBookChked GetCheckedTillDate(int iaccNo, DateTime checkDate)
        {
            using (GenericUnitOfWork _uow = new GenericUnitOfWork())
            {
                var checkTill = _uow.Repository<APBookChked>().FindBy(x => x.IAccNo == iaccNo && x.ChkedTill >= checkDate).FirstOrDefault();
                return checkTill;
            }
        }
        public static IEnumerable<SelectListItem> LoanRevolving()
        {
            IList<SelectListItem> items = new List<SelectListItem>
            {
                new SelectListItem{Text = "All", Value = "1"},
                new SelectListItem{Text = "Revolving", Value = "2"},
                new SelectListItem{Text = "Non Revolving", Value = "3"},
            };
            return items;
        }
        public static SelectList ProductList()
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var productList = uow.Repository<ProductDetail>().GetAll().ToList();
                productList.Add(new ProductDetail() { PID = 0, PName = "All" });
                //return new SelectList(productList, "PID", "PName", 0);
                return new SelectList(productList, "PID", "PName");
            }
        }
        public static bool IsAllowAllBranch()
        {

            var AllowAllBranch = commonService.GetUserAssignMenu(9031, Global.UserId);
            if (AllowAllBranch != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static int GetBranchId()
        {
            int branchID = 0;
            if (!IsAllowAllBranch())
            {
                branchID = commonService.GetBranchIdByEmployeeUserId();
            }
            return branchID;
        }
        public static bool IsAllowAllTransactionUser()
        {

            var AllowAlltransUser = commonService.GetUserAssignMenu(0, Global.UserId);
            if (AllowAlltransUser != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static SelectList VaultTellerAndCashierUser(int userId = 0)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                //if(userId==0)
                //{
                var reportService = new ReportService();
                var allReciever = reportService.GetAllUserReportList();
                return new SelectList(allReciever, "UserId", "EmployeeName", Global.UserId);
                //}
                //else
                //{
                //}

            }
        }
        public static SelectList HierarchyWiseUser(int userId = 0)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var reportService = new ReportService();
                var user = reportService.VaultTellerAndCashierUser();
                //var order = user.Where(x => x.UserId == Global.UserId).FirstOrDefault();
                //int hierarchyOrder = 3;
                //if(order!=null)
                //{
                //    hierarchyOrder = order.DegOrder;
                //}
                //var hierarchyWise = user.Where(x => x.DegOrder > hierarchyOrder).ToList();
                //if (order != null)
                //{
                //    hierarchyWise.Add(order);
                //}               
                return new SelectList(user, "UserId", "EmployeeName");

            }
        }
        public static SelectList AccountStateList()
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                //var productList = uow.Repository<ProductDetail>().GetAll().ToList();
                //productList.Add(new ProductDetail() { PID = 0, PName = "All" });
                ////return new SelectList(productList, "PID", "PName", 0);
                //return new SelectList(productList, "PID", "PName");
                //productList.Add(new ProductDetail() { PID = 0, PName = "All" });
                var accstate = uow.Repository<AccountStatu>().GetAll().ToList();
                //accstate.Add(new SelectList {  "0",  "All" });
                accstate.Add(new AccountStatu() { AsId = 0, AccountState = "All" });
                //return new SelectList(accstate, "AsId", "AccountState", 0);
                return new SelectList(accstate, "AsId", "AccountState");
            }
        }
        public static bool AllowLoan()
        {
            var AllowLoan = commonService.GetUserAssignMenu(9058, Global.UserId);
            if (AllowLoan != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool AllowDeposit()
        {
            var AllowDeposit = commonService.GetUserAssignMenu(9059, Global.UserId);
            if (AllowDeposit != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool AllowBoth()
        {
            if (AllowDeposit() == true && AllowLoan() == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static SelectList DurationWiseProduct(short stype)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var productList = (from d in uow.Repository<ProductDetail>().GetAll()
                                   join s in uow.Repository<SchmDetail>().FindBy(x => x.HasDuration == true && x.SType == stype)
                                   on d.SDID equals s.SDID
                                   select new ProductViewModel()
                                   {
                                       ProductId = d.PID,
                                       ProductName = d.PName
                                   }

                                  ).OrderBy(x=>x.ProductName).ToList();
                productList.Add(new ProductViewModel() { ProductId = 0, ProductName = "All" });
                return new SelectList(productList, "ProductId", "ProductName", 0);
            }


        }
        public static SelectList DepositWithdraw()
        {
            List<SelectListItem> DepositWithdraw = new List<SelectListItem>();

            DepositWithdraw.Add(new SelectListItem { Text = "Deposit", Value = "1" });
            DepositWithdraw.Add(new SelectListItem { Text = "WithDraw", Value = "0" });
            return new SelectList(DepositWithdraw, "Value", "Text");
        }
        public static SelectList ShortType()
        {
            List<SelectListItem> shortType = new List<SelectListItem>();

            shortType.Add(new SelectListItem { Text = "Between", Value = "1" });
            shortType.Add(new SelectListItem { Text = "Greater Than/Equal To", Value = "2" });
            shortType.Add(new SelectListItem { Text = "Less Than/Equal To", Value = "3" });
            return new SelectList(shortType, "Value", "Text");
        }
        public static SelectList SearchParams()
        {
            List<SelectListItem> SearchParams = new List<SelectListItem>();

            SearchParams.Add(new SelectListItem { Text = "All Report", Value = "4" });
            SearchParams.Add(new SelectListItem { Text = "Cash Report", Value = "1" });
            SearchParams.Add(new SelectListItem { Text = "Non-Cash Report", Value = "3" });
            SearchParams.Add(new SelectListItem { Text = "ABBS Report", Value = "2" });
            return new SelectList(SearchParams, "Value", "Text");
        }
        public static SelectList LoanSearchParams()
        {
            List<SelectListItem> SearchParams = new List<SelectListItem>();

            SearchParams.Add(new SelectListItem { Text = "All Report", Value = "3" });
            SearchParams.Add(new SelectListItem { Text = "Cash Report", Value = "2" });
            SearchParams.Add(new SelectListItem { Text = "Non-Cash Report", Value = "1" });
            return new SelectList(SearchParams, "Value", "Text");
        }
        public static SelectList VerifyStatus()
        {
            List<SelectListItem> VerifyStatus = new List<SelectListItem>();
            VerifyStatus.Add(new SelectListItem { Text = "All", Value = "2" });
            VerifyStatus.Add(new SelectListItem { Text = "Verified", Value = "1" });
            VerifyStatus.Add(new SelectListItem { Text = "UnVerified", Value = "0" });
            return new SelectList(VerifyStatus, "Value", "Text", 2);
        }
        public static SelectList VerifyStatusGoodForPayment()
        {
            List<SelectListItem> VerifyStatus = new List<SelectListItem>();
            VerifyStatus.Add(new SelectListItem { Text = "All", Value = "0" });
            VerifyStatus.Add(new SelectListItem { Text = "Verified", Value = "4" });
            VerifyStatus.Add(new SelectListItem { Text = "UnVerified", Value = "1" });
            VerifyStatus.Add(new SelectListItem { Text = "Cash withdraw", Value = "5" });
            return new SelectList(VerifyStatus, "Value", "Text");
        }
        public static SelectList DisbursePayment()
        {
            List<SelectListItem> DepositWithdraw = new List<SelectListItem>();
            DepositWithdraw.Add(new SelectListItem { Text = "Payment", Value = "0" });
            DepositWithdraw.Add(new SelectListItem { Text = "Disburse", Value = "1" });
            return new SelectList(DepositWithdraw, "Value", "Text");
        }
        public static string ContactDetails(int iaccNo)
        {
            using (GenericUnitOfWork _uow = new GenericUnitOfWork())
            {
                string contact = _uow.Repository<string>().SqlQuery("select Contact from cust.FGetCustContact() where CID in (select CID from fin.AOfCust where IAccno=" + iaccNo + ")").FirstOrDefault();
                return contact;
            }
        }
        public static SelectList OrdinaryPromoter()
        {
            List<SelectListItem> Ordinary = new List<SelectListItem>();
            Ordinary.Add(new SelectListItem { Text = "All", Value = "0" });
            Ordinary.Add(new SelectListItem { Text = "Ordinary", Value = "1" });
            Ordinary.Add(new SelectListItem { Text = "Promoter", Value = "2" });
            return new SelectList(Ordinary, "Value", "Text");
        }

        public static SelectList AccountStatus()
        {
            List<SelectListItem> accountStatus = new List<SelectListItem>();
            accountStatus.Add(new SelectListItem { Text = "All", Value = "0" });
            accountStatus.Add(new SelectListItem { Text = "Active", Value = "1" });
            accountStatus.Add(new SelectListItem { Text = "Blocked", Value = "2" });
            return new SelectList(accountStatus, "Value", "Text");
        }

        #region convertNumericToWords
        public static String ones(String Number)
        {
            int _Number = Convert.ToInt32(Number);
            String name = "";
            switch (_Number)
            {

                case 1:
                    name = "One";
                    break;
                case 2:
                    name = "Two";
                    break;
                case 3:
                    name = "Three";
                    break;
                case 4:
                    name = "Four";
                    break;
                case 5:
                    name = "Five";
                    break;
                case 6:
                    name = "Six";
                    break;
                case 7:
                    name = "Seven";
                    break;
                case 8:
                    name = "Eight";
                    break;
                case 9:
                    name = "Nine";
                    break;
            }
            return name;
        }
        public static String tens(String Number)
        {
            int _Number = Convert.ToInt32(Number);
            String name = null;
            switch (_Number)
            {
                case 10:
                    name = "Ten";
                    break;
                case 11:
                    name = "Eleven";
                    break;
                case 12:
                    name = "Twelve";
                    break;
                case 13:
                    name = "Thirteen";
                    break;
                case 14:
                    name = "Fourteen";
                    break;
                case 15:
                    name = "Fifteen";
                    break;
                case 16:
                    name = "Sixteen";
                    break;
                case 17:
                    name = "Seventeen";
                    break;
                case 18:
                    name = "Eighteen";
                    break;
                case 19:
                    name = "Nineteen";
                    break;
                case 20:
                    name = "Twenty";
                    break;
                case 30:
                    name = "Thirty";
                    break;
                case 40:
                    name = "Fourty";
                    break;
                case 50:
                    name = "Fifty";
                    break;
                case 60:
                    name = "Sixty";
                    break;
                case 70:
                    name = "Seventy";
                    break;
                case 80:
                    name = "Eighty";
                    break;
                case 90:
                    name = "Ninety";
                    break;
                default:
                    if (_Number > 0)
                    {
                        name = tens(Number.Substring(0, 1) + "0") + " " + ones(Number.Substring(1));
                    }
                    break;
            }
            return name;
        }
        public static String ConvertWholeNumber(String Number)
        {
            string word = "";
            try
            {
                bool beginsZero = false;//tests for 0XX   
                bool isDone = false;//test if already translated   
                double dblAmt = (Convert.ToDouble(Number));
                //if ((dblAmt > 0) && number.StartsWith("0"))   
                if (dblAmt > 0)
                {//test for zero or digit zero in a nuemric   
                    beginsZero = Number.StartsWith("0");

                    int numDigits = Number.Length;
                    int pos = 0;//store digit grouping   
                    String place = "";//digit grouping name:hundres,thousand,etc...   
                    switch (numDigits)
                    {
                        case 1://ones' range   
                            word = ones(Number);
                            isDone = true;
                            break;
                        case 2://tens' range   
                            word = tens(Number);
                            isDone = true;
                            break;
                        case 3://hundreds' range   
                            pos = (numDigits % 3) + 1;
                            place = " Hundred ";
                            break;
                        case 4://thousands' range   
                        case 5:
                            pos = (numDigits % 4) + 1;
                            place = " Thousand ";
                            break;
                        case 6://lakhs' range
                        case 7:
                            pos = (numDigits % 6) + 1;
                            place = " Lakh ";
                            break;
                        case 8://crores' range
                        case 9:
                            pos = (numDigits % 8) + 1;
                            place = " Crore ";
                            break;
                        case 10://Arbas' range 
                        case 11:
                            pos = (numDigits % 10) + 1;
                            place = " Arba ";
                            break;
                        case 12:

                        //add extra case options for anything above Billion...   
                        default:
                            isDone = true;
                            break;
                    }
                    if (!isDone)
                    {//if transalation is not done, continue...(Recursion comes in now!!)   
                        if (Number.Substring(0, pos) != "0" && Number.Substring(pos) != "0")
                        {
                            try
                            {
                                word = ConvertWholeNumber(Number.Substring(0, pos)) + place + ConvertWholeNumber(Number.Substring(pos));
                            }
                            catch { }
                        }
                        else
                        {
                            word = ConvertWholeNumber(Number.Substring(0, pos)) + ConvertWholeNumber(Number.Substring(pos));
                        }


                    }
                    //ignore digit grouping names   
                    if (word.Trim().Equals(place.Trim())) word = "";
                }
            }
            catch { }
            return word.Trim();
        }
        public static String ConvertDecimals(String number)
        {
            String cd = "", digit = "", engOne = "";
            for (int i = 0; i < number.Length; i++)
            {
                digit = number[i].ToString();
                if (digit.Equals("0"))
                {
                    engOne = "Zero";
                }
                else
                {
                    engOne = ones(digit);
                }
                cd += " " + engOne;
            }
            return cd;
        }
        public static String ConvertToWords(String numb)
        {
            String val = "", wholeNo = numb, points = "", andStr = "", pointStr = "";
            String endStr = "Only";
            try
            {
                int decimalPlace = numb.IndexOf(".");
                if (decimalPlace > 0)
                {
                    wholeNo = numb.Substring(0, decimalPlace);
                    points = numb.Substring(decimalPlace + 1);
                    if (Convert.ToInt32(points) > 0)
                    {
                        andStr = "and";// just to separate whole numbers from points/cents   
                        endStr = "Paisa " + endStr;//Cents   
                        pointStr = ConvertDecimals(points);
                    }
                }
                val = String.Format("{0} {1}{2} {3}", ConvertWholeNumber(wholeNo).Trim(), andStr, pointStr, endStr);
            }
            catch { }
            return val;
        }
        #endregion

        public static SelectList GetFiscalYearByFYIDAndCurrentDate(int FYid)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var contact = uow.Repository<TaxCertificateModel>().SqlQuery("select * from lg.FiscalYears where FYID >="+Convert.ToInt16(FYid)+" and FYID<=(select FYID from lg.FiscalYears where GETDATE() between StartDt and EndDt)").ToList();
                var TaxCertificateList = contact.Select(x => new TaxCertificateModel()
                {
                    FYID = x.FYID,
                    FyName = x.FyName
                }).ToList();
                return new SelectList( TaxCertificateList, "FYID", "FyName");
            }
        }
    }
}

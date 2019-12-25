using ChannakyaBase.BLL.Repository;
using ChannakyaBase.DAL.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChannakyaBase.Model.ViewModel;
using Loader.Models;
using ChannakyaBase.BLL.CustomHelper;
using ChannakyaBase.Model.Models;
using PagedList;
using System.Data.Entity.Validation;

namespace ChannakyaBase.BLL.Service
{
    public class CustomerService
    {

        ReturnBaseMessageModel returnMessage = null;

        private GenericUnitOfWork uow = null;
        private Loader.Repository.GenericUnitOfWork loaderUOW = null;
        public CustomerService()
        {
            returnMessage = new ReturnBaseMessageModel();
            uow = new GenericUnitOfWork();
            loaderUOW = new Loader.Repository.GenericUnitOfWork();
        }
        public List<CustInfo> GetAllCustomer()
        {
            var customer = uow.Repository<CustInfo>().GetAll().ToList();

            return customer;
        }
        public List<CustInformationViewModel> CustomerList(string Name, string Address, string contact, int? cType, int pageNo, int pageSize)
        {
            string query = "";
            query = "select COUNT(*) OVER () AS TotalCount, * from [cust].[FGetCustList]()  where  Name like'%"+ Name.Trim() +"%'";

            //if (Name != "")
            //{
            //    query += " and Name like'" + Name + "%'";
            //}
            if (contact != "")
            {
                query += " and contact like'%" + contact.Trim() + "%'";
            }
            if (Address != "")
            {
                query += " and location like'%" + Address.Trim() + "%'";
            }

            if (cType != 0 && cType != null)
            {
                query += " and CtypeID =" + cType;
            }
            query += @" ORDER BY  Name
                       OFFSET ((" + pageNo + @" - 1) * " + pageSize + @") ROWS
                       FETCH NEXT " + pageSize + " ROWS ONLY";
            var customerList = uow.Repository<CustInformationViewModel>().SqlQuery(query).ToList();
            return customerList;
        }

        public CustInfo GetSingleCustomer(int? cId)
        {

            CustInfo custInfo = uow.Repository<ChannakyaBase.DAL.DatabaseModel.CustInfo>().FindBy(x => x.CID == cId).First();
            //CustIndividual cind = uow.Repository<CustIndividual>().FindBy(x => x.CID == cId).First();
            //List<CustContact> CustContactList = uow.Repository<CustContact>().FindBy(x => x.CID == cId).ToList();
            //CustCompany ccmp = uow.Repository<CustCompany>().FindBy(x => x.CID == cId).First();

            //custInfo.CustCompany = ccmp;
            //// List<CustTypeCertificate> CustCertList = uow.Repository<CustTypeCertificate>().FindBy(x => x.CID == cId).ToList();
            //List<CustAddress> CustAddressList = uow.Repository<CustAddress>().FindBy(x => x.CID == cId).ToList();
            //List<DAL.DatabaseModel.CustContactPerson> CustContactPerson = uow.Repository<CustContactPerson>().FindBy(x => x.CID == cId).ToList();
            return custInfo;

        }

        public CustInfo GetCustomerForEdit(int? cId)
        {
            CustInfo custInfo = uow.Repository<ChannakyaBase.DAL.DatabaseModel.CustInfo>().FindBy(x => x.CID == cId).FirstOrDefault();
            CustIndividual cind = uow.Repository<CustIndividual>().FindBy(x => x.CID == cId).FirstOrDefault();
            custInfo.CustIndividual = cind;
            List<CustContact> CustContactList = uow.Repository<CustContact>().FindBy(x => x.CID == cId).ToList();
            custInfo.CustContacts = CustContactList;
            CustCompany ccmp = uow.Repository<CustCompany>().FindBy(x => x.CID == cId).FirstOrDefault();
            custInfo.CustCompany = ccmp;
            // List<CustTypeCertificate> CustCertList = uow.Repository<CustTypeCertificate>().FindBy(x => x.CID == cId).ToList();

            List<CustAddress> CustAddressList = uow.Repository<CustAddress>().FindBy(x => x.CID == cId).ToList();
            custInfo.CustAddresses = CustAddressList;
            List<DAL.DatabaseModel.CustContactPerson> CustContactPerson = uow.Repository<CustContactPerson>().FindBy(x => x.CID == cId).ToList();
            custInfo.CustContactPersons = CustContactPerson;
            return custInfo;
        }

        //public List<CustContact> GetContactByCtype(byte ctypeID)
        //{
        //    CustContact cont = new CustContact();
        //    List<CustContact> contList = new List<CustContact>();
        //    //var contlist = uow.Repository<CustContact>().FindBy(x => x.CID == CID).ToList();
        //    //List<CustContact> contlist = new List<CustContact>();
        //    var conttypelist = uow.Repository<CustTypeContact>().FindBy(x => x.CTypeID == ctypeID).ToList();
        //    // var combineCustType = conttypelist.Zip(contlist, (ct, cc) => new { CustTypeContact = ct ,CustContact = cc });
        //    foreach (var item in conttypelist)
        //    {
        //        if (item.CNoState == 2)
        //        {


        //            cont.IsDeleted = false;
        //            // itemModel.IsDeleted = true;


        //        }
        //        else
        //        {

        //            cont.IsDeleted = true;
        //        }
                
        //    }
        //    //}
        //    contList.Add(cont);
        //    return contList;
        //}

        public List<CustTypeContact> GetCustTypeContacts(int? ctypeID)
        {
            var custtypelist= uow.Repository<CustTypeContact>().FindBy(x => x.CTypeID == ctypeID).ToList();
            return custtypelist;
        }

        public List<LocationTypeDef> CustAddressType()
        {
            List<LocationTypeDef> companyGroupList = uow.Repository<LocationTypeDef>().GetAll().ToList();
            return companyGroupList;


        }
        public List<CustTypeContact> CustCompulSoryContactType(byte? CtypeId)
        {
            var custContactList = uow.Repository<CustTypeContact>().FindByMany(x => x.CNoState == 2 && x.CTypeID == CtypeId);
            return custContactList.ToList();
        }
        public CustType GetCustomerType(int cTypeId)
        {
            var result =  uow.Repository<CustType>().GetSingle(x => x.CtypeID == cTypeId);
            return result;

        }
        public ReturnBaseMessageModel Save(CustInfo cinfo, CustIndividual cind, List<CustContact> CustContactList, CustCompany ccmp, List<CustTypeCertificate> CustCertList, List<CustAddress> CustAddressList, List<DAL.DatabaseModel.CustContactPerson> CustContactPerson)

        {
            try
            {
                if (cinfo.CID == 0)
                {
                    if (cind.Fname != null)
                    {
                        cind.Fname = new CultureInfo("en-US").TextInfo.ToTitleCase(cind.Fname.ToString().Trim());//ToTitleCase() case is used for making first letter of name
                        if (cind.Mname != null)
                        {
                            cind.Mname = new CultureInfo("en-US").TextInfo.ToTitleCase(cind.Mname.ToString().Trim());
                        }
                        cind.Lname = new CultureInfo("en-US").TextInfo.ToTitleCase(cind.Lname.ToString().Trim());
                        cind.FatherName = new CultureInfo("en-US").TextInfo.ToTitleCase(cind.FatherName.ToString().Trim());
                        cind.GFatherName = new CultureInfo("en-US").TextInfo.ToTitleCase(cind.GFatherName.ToString().Trim());
                        if (cind.SpouseName != null)
                            cind.SpouseName = new CultureInfo("en-US").TextInfo.ToTitleCase(cind.SpouseName.ToString().Trim());
                        cinfo.CustIndividual = cind;
                    }
                    if (ccmp.CCName != null)
                    {
                        if (ccmp.CCName != null)
                            ccmp.CCName = new CultureInfo("en-US").TextInfo.ToTitleCase(ccmp.CCName.ToString().Trim());
                        cinfo.CustCompany = ccmp;
                        // cinfo.CustContactPersons = ccmp.CCPerson;

                    }

                    foreach (var item in CustContactList)
                    {
                        if (item.IsDeleted == false)
                        {
                            cinfo.CustContacts.Add(item);
                            if (item.IsDefault == true)
                            {
                                CustIRegContact custIReg = new CustIRegContact();
                                item.CustIRegContacts.Add(custIReg);
                            }
                        }
                    }
                    foreach (var item in CustAddressList)
                    {
                        item.ATypeId = item.AddressTypeId;
                        cinfo.CustAddresses.Add(item);

                    }
                    foreach (var item in CustContactPerson)
                    {
                        cinfo.CustContactPersons.Add(item);
                        ccmp.CCPerson = item.CPName;
                        ccmp.CCno = item.CPCNo;
                    }

                    
                    foreach (var item in CustCertList)
                    {

                        if (item.isSubmitted == true)
                        { 
                            
                            CustCertificate custcert = new CustCertificate();
                            
                            custcert.CCCertID = item.CCCertID;
                            cinfo.CustCertificates.Add(custcert);

                        }


                    }

                    if (ccmp.CCName != null)
                    {
                        uow.Repository<CustCompany>().Add(ccmp);
                    }

                    cinfo.Postedby = Global.UserId;
                    CommonService commonservice = new CommonService();
                    cinfo.CRegdate = commonservice.GetBranchTransactionDate();
                    uow.Repository<CustInfo>().Add(cinfo);
                    uow.Commit();
                }
                else
                {
                    //var custinfo = uow.Repository<CustInfo>().FindBy(x => x.CID == cinfo.CID).FirstOrDefault();
                    //cinfo.CtypeID = custinfo.CtypeID;
                    List<CustContact> custcontact = new List<CustContact>();
                    foreach (var item in CustContactList)
                    {
                        CustIRegContact cIreg = uow.Repository<CustIRegContact>().GetSingle(x => x.CCID == item.CCID);
                        if (item.IsDeleted == true && item.CCID != 0)
                        {
                            if (cIreg != null)
                            {
                                uow.Repository<CustIRegContact>().Delete(cIreg);
                            }
                            CustContact ccn = uow.Repository<CustContact>().GetSingle(x => x.CCID == item.CCID);
                            uow.Repository<CustContact>().Delete(ccn);
                        }

                        else if (cIreg == null && item.IsDefault == true)
                        {
                            cIreg = new CustIRegContact();
                            cIreg.CID = cinfo.CID;
                            cIreg.CCID = item.CCID;
                            uow.Repository<CustIRegContact>().Add(cIreg);
                        }
                        else if (cIreg != null && item.IsDefault == false)
                        {
                            uow.Repository<CustIRegContact>().Delete(cIreg);
                        }

                        else if (item.CCID == 0)
                        {

                            if (item.IsDeleted == false)
                            {

                                item.CID = cinfo.CID;
                                uow.Repository<CustContact>().Add(item);
                                if (item.IsDefault == true)
                                {
                                    CustIRegContact custIReg = new CustIRegContact();
                                    custIReg.CID = cinfo.CID;
                                    cinfo.CustIRegContacts.Add(custIReg);
                                }
                            }

                        }

                    }
                    foreach (var item in CustContactPerson)
                    {
                        DAL.DatabaseModel.CustContactPerson custContactPerson = uow.Repository<DAL.DatabaseModel.CustContactPerson>().GetSingle(x => x.CPId == item.CPId);
                        if (custContactPerson == null)
                        {
                            custContactPerson = new DAL.DatabaseModel.CustContactPerson();
                        }

                        custContactPerson.CPName = item.CPName;
                        custContactPerson.CPCNo = item.CPCNo;
                        custContactPerson.CID = cinfo.CID;
                        if (item.CPDeleted == false && item.CPId == 0)
                        {

                            uow.Repository<DAL.DatabaseModel.CustContactPerson>().Add(custContactPerson);
                        }

                        else if (item.CPDeleted == true && custContactPerson != null)
                        {
                            uow.Repository<DAL.DatabaseModel.CustContactPerson>().Delete(custContactPerson);
                        }
                        else if (item.CPDeleted == false && custContactPerson != null)
                        {
                            uow.Repository<DAL.DatabaseModel.CustContactPerson>().Edit(custContactPerson);
                        }
                    }

                    foreach (var item in CustAddressList)
                    {
                        item.CID = cinfo.CID;
                        item.ATypeId = item.AddressTypeId;
                        uow.Repository<CustAddress>().Edit(item);

                    }
                    foreach (var item1 in CustContactList)
                        if (item1.IsDeleted == false && item1.CCID != 0)
                        {
                            item1.CID = cinfo.CID;
                            uow.Repository<CustContact>().Edit(item1);

                        }
                    var getcertificate = uow.Repository<CustCertificate>().FindBy(x => x.CID == cinfo.CID).ToList();
                    foreach (var item in getcertificate)
                    {
                        uow.Repository<CustCertificate>().Delete(item);
                    }

                    foreach (var item in CustCertList)
                    {

                        //if (item.isSubmitted == true && item.CCertID == 0)
                        //{


                        //    CustCertificate custcert = new CustCertificate();
                        //    custcert.CID = cinfo.CID;
                        //    custcert.CCCertID = item.CCCertID;
                        //    uow.Repository<CustCertificate>().Add(custcert);
                        //}
                        if (item.isSubmitted == true)
                        {


                            CustCertificate custcert = new CustCertificate();
                            custcert.CID = cinfo.CID;
                            custcert.CCCertID = item.CCCertID;
                            custcert.CCCertID = item.CCCertID;
                            uow.Repository<CustCertificate>().Add(custcert);
                        }
                        else if (item.isSubmitted == false && item.CCertID != 0)
                        {
                            CustCertificate custCertificate = uow.Repository<CustCertificate>().GetSingle(x => x.CCCertID == item.CCCertID);
                            if (custCertificate != null)
                            {
                                uow.Repository<CustCertificate>().Delete(custCertificate);
                            }
                        }

                    }

                    if (ccmp.CCName != null)
                    {

                        uow.Repository<CustCompany>().Edit(ccmp);

                    }
                    if (cind.Fname != null)
                    {
                        uow.Repository<CustIndividual>().Edit(cind);
                    }
                    //custinfo = cinfo;
                    uow.Repository<CustInfo>().Edit(cinfo);


                }

                uow.Commit();
                returnMessage.Msg = "successfully Saved";
                returnMessage.Success = true;
                return returnMessage;
            }
            catch (DbEntityValidationException ex)
            {
                //returnMessage.Msg = "Not Saved" + ex.Message;
                //return returnMessage;
                foreach (var eve in ex.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;

            }
        }

        public List<CustTypeCertificate> GetCertificateTypeByCtype(int ctypeid)
        {

            var certlist = uow.Repository<CustTypeCertificate>().FindBy(x => x.CTypeID == ctypeid).ToList();
            //var certlist = (from ct in uow.Repository<CustTypeCertificate>().GetAll()
            //                join cust in uow.Repository<CustInfo>().GetAll()
            //                on ct.CTypeID equals cust.CtypeID
            //                select new CustomerViewModel() {
            //                    CID = cust.CID,
            //                    CCCertID=ct.CCCertID,
                                      
                                
            //                }).ToList();

                          




            //foreach(var item in certlist)
            //{
            //    if (item.CCState == 1)
            //    {
            //        item.isSubmitted = false;
            //    }
            //    else
            //    {
            //        item.isSubmitted = true;
            //    }
               
            //}
            return certlist;
        }


        public List<CustTypeCertificate> GetEditCertificateTypeByCtype(int ctypeid, decimal cId)
        {
            var certlist = uow.Repository<CustTypeCertificate>().FindBy(x => x.CTypeID == ctypeid).ToList();
            foreach (var item in certlist)
            {
                var countcert = uow.Repository<CustCertificate>().FindBy(x => x.CCCertID == item.CCCertID && x.CID == cId).Count();
                if (countcert == 1)
                {
                    item.isSubmitted = true;
                }
                else
                {
                    item.isSubmitted = false;
                }
            }
            return certlist;
        }
        public List<SelectListItem> GetCustomerListFromMaster()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            var list = uow.Repository<CustType>().GetAll().OrderBy(x=>x.Ctype);
            foreach (var item in list)
            {
                lst.Add(new SelectListItem { Text = item.Ctype, Value = item.CtypeID.ToString() });
            }
            return lst;
        }

        public List<SelectListItem> GetOccupationList()
        {
            List<SelectListItem> customerlist = new List<SelectListItem>();
            var list = uow.Repository<OccupationDef>().GetAll().OrderBy(x=>x.occupation);
            foreach (var item in list)
            {
                customerlist.Add(new SelectListItem { Text = item.occupation, Value = item.Occpn.ToString() });
            }
            return customerlist;
        }



        public List<CustInformationViewModel> CustomerInfoList(string searchParameter, string searchOption, string mode, string type, int pageNo, int pageSize)
        {
            string query = "";
            query = "select  * from cust.FGetCustListForSearch()";
            if (searchParameter != "")
            {
                if (searchOption == "Name")
                {
                    query += " where Name like'%" + searchParameter + "%'";
                }
                else if (searchOption == "Mobile")
                {
                    query += " where Mobile like'%" + searchParameter + "%'";
                }
                else if (searchOption == "ContactPerson")
                {
                    query += " where ContactPerson like'%" + searchParameter + "%'";
                }
                else if(searchOption == "AccountNumber")
                {
                    query += "where AccountNumber like'%" + searchParameter + "%";
                }
            }
            if (mode == ECustomerSearchType.CompanyOnly.GetDescription())
            {
                query += " where IsIndividual=0";
            }
            else if (mode == ECustomerSearchType.CustomerOnly.GetDescription())
            {
                query += " where IsIndividual=1";
            }
            else if (mode == ECustomerSearchType.All.GetDescription())
            {

            }
            string FinalQuery = @";WITH Customer_Table AS(
                                     " + query + @"
                                                          )
                                          , Count_CTE AS (
                           SELECT COUNT(*) AS[TotalCount]
                         FROM Customer_Table
                           )";
            FinalQuery += @"SELECT *
                        FROM Customer_Table, Count_CTE
                       ORDER BY Customer_Table.Name
                       OFFSET ((" + pageNo + @" - 1) * " + pageSize + @") ROWS
                       FETCH NEXT " + pageSize + " ROWS ONLY";

            var customerInfoList = uow.Repository<CustInformationViewModel>().SqlQuery(FinalQuery).ToList();
            return customerInfoList;
        }



        public CustInformationViewModel GetSelectedCustomer(int customerID, string custType)
        {
            CustInformationViewModel customerInfoList = new CustInformationViewModel();
            if (TypeOfCustomer.ShareCustomer.GetDescription() == custType)
            {
                //here customerID is regNo
                customerInfoList = uow.Repository<CustInformationViewModel>().SqlQuery(@"select  sg.RegNo as CId ,Name from cust.FGetCustListForSearch() cl 
                          join fin.ShrReg sg on sg.CId = cl.CID where sg.RegNo=" + customerID).FirstOrDefault();
            }
            else
            {
                customerInfoList = uow.Repository<CustInformationViewModel>().SqlQuery(@"select a.CID,Name,ct.isind from (SELECT    CID  , Cast(( Fname + ' ' + ISNULL(Mname, '') +' '+ Lname) as varchar(200)) Name
                                                                                         FROM cust.CustIndividual
                                                                                         UNION
                                                                                         SELECT  CID, CCName AS Name
                                                                                         FROM cust.CustCompany) a
																						 join cust.CustInfo ci on ci.CID=a.CID
																						 join cust.CustType ct on ct.CtypeID=ci.CtypeID  where a.cid={0} ", customerID).FirstOrDefault();
            }

            return customerInfoList;
        }
        public EmployeeViewModel GetShareHolderDetails(int regId)
        {
            EmployeeViewModel empModel = new EmployeeViewModel();
            var customerInfoList = uow.Repository<CustInformationViewModel>().SqlQuery(@"select  sg.RegNo as CId ,Name from cust.FGetCustListForSearch() cl 
                          join fin.ShrReg sg on sg.CId = cl.CID where sg.RegNo=" + regId).FirstOrDefault();
            empModel.EmployeeId = Convert.ToInt32(customerInfoList.CID);
            empModel.EmployeeName = customerInfoList.Name;
            return empModel;

        }
        public List<CustInformationViewModel> GetSelectedMultipleCustomer(int[] listBox)
        {
            string customer = "";
            foreach (var item in listBox)
            {
                if (customer.Length > 0)
                {
                    customer += ", ";
                }
                customer += item;
            }
            var customerInfoList = uow.Repository<CustInformationViewModel>().SqlQuery(@"select a.CID,Name,ct.isind from (SELECT    CID  , Cast(( Fname + ' ' + ISNULL(Mname, '') +' '+ Lname) as varchar(200)) Name
                                                                                         FROM cust.CustIndividual
                                                                                         UNION
                                                                                         SELECT  CID, CCName AS Name
                                                                                         FROM cust.CustCompany) a
																						 join cust.CustInfo ci on ci.CID=a.CID
																						 join cust.CustType ct on ct.CtypeID=ci.CtypeID  where a.cid in (" + customer + ")").ToList();
            return customerInfoList;
        }



        public List<EmployeeViewModel> GetEmployeeDetails(int branchId, string searchParam, string searchOption, string loadType, string searchType, int pageNo, int pageSize)
        {

            string query = "";
            if (loadType == EEmployeeOrShare.Employee.GetDescription())
            {

                if (EEmployeeChange.UserChangeTeller.GetDescription() == searchType || EEmployeeChange.UserCashLimit.GetDescription() == searchType)
                {
                    //query = "select COUNT(*) OVER() AS TotalCount, * from[fin].[FGetAllUsersWithDesignation]() where DegOrder< 7 and DegOrder> 2 and DegOrder!= 4  AND DegOrder!=3";
                    query = "select COUNT(*) OVER() AS TotalCount, * from[fin].[FGetUserForCashLimit]("+Global.BranchId+") ";
                }
                else
                {
                    CommonService commonService = new CommonService();
                    branchId = commonService.GetBranchIdByEmployeeUserId();


              
                    query = "select COUNT(*) OVER () AS TotalCount,* from [fin].[FGetUserByCollectorDesignation](2009," + branchId + ") ";
                }

            }
            else
            {
                if (EEmployeeChange.PurchaseShare.GetDescription() == searchType || EEmployeeChange.ImageChange.GetDescription() == searchType)
                {
                    query = @"select COUNT(*) OVER () AS TotalCount, CONVERT(int,sg.RegNo) as EmployeeId ,Name as EmployeeName , sg.RegistrationCode as EmployeeNo from cust.FGetCustListForSearch() cl 
                          join fin.ShrReg sg on sg.CId = cl.CID where sg.ApprovedBy!=0 ";
                }
                else
                {
                    query = @"SELECT Count(*) 
                            OVER () AS TotalCount,* 
                        FROM   (SELECT DISTINCT CONVERT(INT, sg.regno) AS EmployeeId, 
                        NAME                   AS EmployeeName, 
                        sg.registrationcode    AS EmployeeNo 
                        FROM   cust.Fgetcustlistforsearch() cl 
                        JOIN fin.shrreg sg 
                        ON sg.cid = cl.cid 
                        JOIN fin.shrpurchase sp 
                         ON sp.regno = sg.regno) c "

                  ;
                }
                //here RegNo id is employeeId

            }


            if ((searchParam != "") && (searchParam != null))
            {
                if (searchOption == "")
                {
                    if (loadType == EEmployeeChange.PurchaseShare.GetDescription() || EEmployeeChange.ImageChange.GetDescription() == searchType)
                    {
                        query += " where Name like'%" + searchParam + "%'";  

                    }
                    else
                    {
                        query += " where EmployeeName like'%" + searchParam + "%'";
                    }
                }
                if (searchOption == "Name")
                {
                    if (searchType == EEmployeeChange.PurchaseShare.GetDescription() || EEmployeeChange.ImageChange.GetDescription() == searchType)
                    {
                        query += " and Name like'%" + searchParam + "%'"; //changed from where to and 
                    }
                    else
                    {
                        query += " where EmployeeName like'%" + searchParam + "%'";

                    }
                }
                else if (searchOption == "Code")
                {
                    if (searchType == EEmployeeChange.PurchaseShare.GetDescription() || EEmployeeChange.ImageChange.GetDescription() == searchType)
                    {
                        query += " and RegistrationCode like'%" + searchParam + "%'";
                    }
                    else
                    {
                        query += " where EmployeeNo like'%" + searchParam + "%'";

                    }

                }

            }

            //if (branchId != 0)
            //{
            //    if (searchParam != "")
            //    {
            //        query += " and BranchId=" + branchId;
            //    }
            //    else
            //    {
            //        query += "  where BranchId=" + branchId;
            //    }
            //}
            query += @" ORDER BY  EmployeeName
                       OFFSET ((" + pageNo + @" - 1) * " + pageSize + @") ROWS
                       FETCH NEXT " + pageSize + " ROWS ONLY";
            var EmplaoyeeList = uow.Repository<EmployeeViewModel>().SqlQuery(query).ToList();
            return EmplaoyeeList;
        }


        public List<Loader.Models.Employee> GetEmployeeByText(string text, string loadType, string searchType)
        {
            List<Loader.Models.Employee> empList = new List<Loader.Models.Employee>();
            if (loadType == EEmployeeOrShare.Employee.GetDescription())
            {
                empList = loaderUOW.Repository<Loader.Models.Employee>().FindBy(x => x.EmployeeName.Contains(text)).ToList();
            }
            else
            {
                List<CustInformationViewModel> customerInfoList = new List<CustInformationViewModel>();
                if (EEmployeeChange.ReturnShare.GetDescription() == searchType)
                {
                    customerInfoList = uow.Repository<CustInformationViewModel>().SqlQuery(@"SELECT DISTINCT CONVERT(INT, sg.regno) AS RegNo,Name                   

                        FROM   cust.Fgetcustlistforsearch() cl 
                        JOIN fin.shrreg sg 
                        ON sg.cid = cl.cid 
                        JOIN fin.shrpurchase sp 
                         ON sp.regno = sg.regno where sg.ApprovedBy!=0 and Name like'%" + text + "%'").ToList();
                }
                else
                {
                    customerInfoList = uow.Repository<CustInformationViewModel>().SqlQuery(@"select  CONVERT(int,sg.RegNo) as RegNo ,Name from cust.FGetCustListForSearch() cl 
                          join fin.ShrReg sg on sg.CId = cl.CID where sg.ApprovedBy!=0 and Name like'%" + text + "%'").ToList();
                }

                empList = customerInfoList.Select(x => new Loader.Models.Employee()
                {
                    EmployeeId = x.RegNo,
                    EmployeeName = x.Name
                }).ToList();

            }

            return empList;
        }

        public bool CheckForDeletionWithRemitCompany(int CCGroupID)
        {
            int count = uow.Repository<CustCompany>().FindBy(x => x.CCGroupID == CCGroupID).Count();

            if (count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<CustomerAccountsViewModel> GetCustomerAccounts(decimal cid)
        {
            var customerAccounts = uow.Repository<CustomerAccountsViewModel>().SqlQuery(@"select CID, a.IAccno,Accno,PName,Aname,AccountState from fin.AOfCust a
                                                                                         inner join fin.ADetail b on a.IAccno=b.IAccno inner join fin.ProductDetail p on b.pid= p.pid
                                                                                         inner join fin.accountstatus acs on b.AccState= acs.asid where a.cid = (" + cid + ")").ToList();

            return customerAccounts;
        }

        public bool CheckExistsGroup(string cCGroupName, int cCGroupID)
        {

            int count = uow.Repository<CustCompGroup>().GetAll().Where(x => x.CCGroupName.ToLower().Trim() == cCGroupName.ToLower().Trim()).Where(x => x.CCGroupID != cCGroupID).Count();

            if (count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetCustomerName(int CID)
        {
            var customer = uow.Repository<CustInformationViewModel>().SqlQuery("select Name From [cust].[FGetCustName]() where cid=" + CID).FirstOrDefault();
            if (customer != null)
                return customer.Name;
            else
                return "-";
        }
        public string GetCustomerNameByAccno(int accno)
        {
            List<AOfCust> objO = uow.Repository<AOfCust>().SqlQuery(@"select * from fin.AOfCust where IAccno=" + accno + "").ToList();
            // var customerInfoList = uow.Repository<MessageInfoViewModel>().SqlQuery(@"select [Id],[EventValue],[Idtype],[Eventid],[Fdate],[Tdate],[Mdesc] from fin.MessInfo where EventValue in (" + customersids + ") and Eventid=" + eventid + "").ToList();// where a.cid in (" + customer + ")"
            var customer = uow.Repository<CustInformationViewModel>().SqlQuery("select Name From [cust].[FGetCustName]() where cid=" + objO[0].CID).FirstOrDefault();
            if (customer != null)
                return customer.Name;
            else
                return "-";                                                                                                                                                                                                                                      // var customerInfoList = uow.Repository<MessageInfoViewModel>().SqlQuery(@"select [Id],[EventValue],[Idtype],[Eventid],[Fdate],[Tdate],[Mdesc] from fin.MessInfo where EventValue in (" + custid + ") and Eventid=" + eid + "").ToList();// where a.cid in (" + customer + ")"

        }
        #region Event message
        public ReturnBaseMessageModel MessageCreate(MessageInfoViewModel message)
        {
            if (message.Eventid == 0)
            {
                try
                {
                    var eventitem = BaseTaskUtilityService.GetAllEventList();
                    foreach (var item in eventitem)
                    {
                        var id = item.Value;
                        MessInfo EventMessage = new MessInfo();
                        EventMessage.Eventid = Convert.ToByte(id);
                        EventMessage.EventValue = message.EventValue;
                        EventMessage.Fdate = message.Fdate;
                        EventMessage.Tdate = message.Tdate;
                        EventMessage.Mdesc = message.Mdesc;
                        EventMessage.Idtype = Convert.ToByte(message.Idtype);
                        uow.Repository<MessInfo>().Add(EventMessage);
                        uow.Commit();
                    }
                    returnMessage.Success = true;
                    returnMessage.Msg = "Sucessfully Saved";
                    return returnMessage;
                }
                catch (Exception ex)
                {

                    returnMessage.Success = false;
                    returnMessage.Msg = "Message Not Saved" + ex.Message;
                    return returnMessage;
                }

            }
            try
            {
                MessInfo EventMessage = new MessInfo();
                EventMessage.Eventid = message.Eventid;
                EventMessage.EventValue = message.EventValue;
                EventMessage.Fdate = message.Fdate;
                EventMessage.Tdate = message.Tdate;
                EventMessage.Mdesc = message.Mdesc;
                EventMessage.Idtype = Convert.ToByte(message.Idtype);
                uow.Repository<MessInfo>().Add(EventMessage);
                uow.Commit();
                returnMessage.Success = true;
                returnMessage.Msg = "Sucessfully Saved";
                return returnMessage;
            }
            catch (Exception ex)
            {

                returnMessage.Success = false;
                returnMessage.Msg = "Message Not Saved" + ex.Message;
                return returnMessage;
            }

        }
        public List<MessageInfoViewModel> EventMessageList(int? EventValue)
        {
            var MessageList = uow.Repository<MessInfo>().FindByMany(x => x.EventValue == EventValue).OrderByDescending(x => x.Id).Select(x => new MessageInfoViewModel()
            {
                EventValue = x.EventValue,
                Eventid = x.Eventid,
                Fdate = x.Fdate,
                Tdate = x.Tdate,
                Mdesc = x.Mdesc,
                Idtype = x.Idtype

            }).ToList();

            return MessageList;
        }
        public List<MessageInfoViewModel> EventMessageListbycustomerids(int[] listbox, int eventid)
        {
            string customer = "";
            var customerInfoList = new List<MessageInfoViewModel>();
            foreach (var item in listbox)
            {
                if (customer.Length > 0)
                {
                    customer += ", ";
                }
                customer += item;
            }
            if (eventid == 0)
            {
                customerInfoList = uow.Repository<MessageInfoViewModel>().SqlQuery(@"select [Id],[EventValue],[Idtype],[Eventid],[Fdate],[Tdate],[Mdesc] from fin.MessInfo where EventValue in (" + customer + ")").ToList();// where a.cid in (" + customer + ")"
                //customerInfoList = uow.Repository<MessInfo>().FindBy(x=> listbox.Contains(x.EventValue))
                //    .Select(x => new MessageInfoViewModel() {
                //    EventValue = x.EventValue,
                //    Eventid = x.Eventid,
                //    Fdate = x.Fdate,
                //    Tdate = x.Tdate,
                //    Mdesc = x.Mdesc,
                //    Idtype = x.Idtype
                //}).ToList(); 
                // customerInfoList=uow.Repository<MessInfo>().GetAll  .GetAll().Select(x=>x.EventValue).ToList().Contains(Convert.ToInt32(customer)).Where(x=>x.EventValue).FindBy(x => x.eve.Contains(customer))
            }
            else
            {
                customerInfoList = uow.Repository<MessageInfoViewModel>().SqlQuery(@"select [Id],[EventValue],[Idtype],[Eventid],[Fdate],[Tdate],[Mdesc] from fin.MessInfo where EventValue in (" + customer + ") and Eventid=" + eventid + "").ToList();// where a.cid in (" + customer + ")"
            }

            return customerInfoList;
        }
        public List<MessageInfoViewModel> GetCustomerIdByaccountno(int accountno, int eventid)
        {

            List<AOfCust> objO = uow.Repository<AOfCust>().SqlQuery(@"select * from fin.AOfCust where IAccno=" + accountno + "").ToList();


            string customersids = "";
            foreach (var item in objO)
            {
                if (customersids != "")
                {
                    customersids += ", ";
                }
                customersids += item.CID;
            }
            //var custid = 3;
            // var eid = 1;
            //to specify that it reterive data from account number
            var idtype = 2;
            var idtypecustomer = 1;

            var eventListFromAccount = uow.Repository<MessageInfoViewModel>().SqlQuery(@"select [Id],[EventValue],[Idtype],[Eventid],[Fdate],[Tdate],[Mdesc] from fin.MessInfo where EventValue in (" + accountno + ") and Eventid=" + eventid + " and Idtype=" + idtype + "").ToList();
            var eventListFromCid = uow.Repository<MessageInfoViewModel>().SqlQuery(@"select [Id],[EventValue],[Idtype],[Eventid],[Fdate],[Tdate],[Mdesc] from fin.MessInfo where EventValue in (" + customersids + ") and Eventid=" + eventid + "and Idtype=" + idtypecustomer + "").ToList();// where a.cid in (" + customer + ")"
                                                                                                                                                                                                                                                                                              // var customerInfoList = uow.Repository<MessageInfoViewModel>().SqlQuery(@"select [Id],[EventValue],[Idtype],[Eventid],[Fdate],[Tdate],[Mdesc] from fin.MessInfo where EventValue in (" + custid + ") and Eventid=" + eid + "").ToList();// where a.cid in (" + customer + ")"
            var customerEventList = new List<MessageInfoViewModel>();
            foreach (var item in eventListFromAccount)
            {
                customerEventList.Add(item);
            }
            foreach (var item in eventListFromCid)
            {
                customerEventList.Add(item);
            }
            return customerEventList;
        }

        #endregion

        #region CustomerCompanyGroup


        //public CustCompGroup GetSingleCustomerCompanyGroupForDelete(int CCGroupID)
        //{
        //    //return uow.Repository<CustCompGroup>().FindBy(x => x.CCGroupID == CCGroupID).Select(x => new CustomerCompanyViewModel
        //    //{
        //    //    CCGroupID = x.CCGroupID,
        //    //    CCGroupName = x.CCGroupName
        //    //}).FirstOrDefault();
        //    return uow.Repository<CustCompGroup>().FindBy(x => x.CCGroupID == CCGroupID).SingleOrDefault();

        //}
        public CustomerCompanyViewModel GetSingleCustomerCompanyGroup(int CCGroupID)
        {
            return uow.Repository<CustCompGroup>().FindBy(x => x.CCGroupID == CCGroupID).Select(x => new CustomerCompanyViewModel
            {
                CCGroupID = x.CCGroupID,
                CCGroupName = x.CCGroupName
            }).FirstOrDefault();

        }

        public ReturnBaseMessageModel InsertUpdateCompanyGroup(CustomerCompanyViewModel cCGroup)
        {
            try
            {
                var singleCompanyGroup = uow.Repository<CustCompGroup>().FindBy(x => x.CCGroupID == cCGroup.CCGroupID).FirstOrDefault();

                if (singleCompanyGroup == null)
                {
                    singleCompanyGroup = new CustCompGroup();
                }
                singleCompanyGroup.CCGroupID = cCGroup.CCGroupID;
                singleCompanyGroup.CCGroupName = cCGroup.CCGroupName;

                if (cCGroup.CCGroupID == 0)
                {
                    uow.Repository<CustCompGroup>().Add(singleCompanyGroup);
                    returnMessage.Msg = "Customer Company Saved Successfully";
                    returnMessage.Success = true;
                }
                else
                {
                    uow.Repository<CustCompGroup>().Edit(singleCompanyGroup);
                    returnMessage.Msg = "Customer Company edited Successfully";
                    returnMessage.Success = true;
                }
                uow.Commit();
                //returnMessage.Success = true;
                //returnMessage.Msg = "Customer Company Saved Successfully";
                return returnMessage;
            }

            catch (Exception ex)
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Failed To Save" + ex.Message;
                return returnMessage;
            }

        }




        public List<CustomerCompanyViewModel> GetAllCustomerCompany()
        {
            return uow.Repository<CustCompGroup>().GetAll().Select(x => new CustomerCompanyViewModel
            {
                CCGroupID = x.CCGroupID,
                CCGroupName = x.CCGroupName
            }).ToList();
        }


        public CustomerCompanyViewModel CustomerCompanyInfoId(int CCGroupID)
        {
            var customerCompanyGroup = uow.Repository<CustCompGroup>().FindBy(x => x.CCGroupID == CCGroupID).Select(x => new CustomerCompanyViewModel
            {

                CCGroupID = x.CCGroupID,
                CCGroupName = x.CCGroupName

            }).FirstOrDefault();
            return customerCompanyGroup;
        }



        public void CustomerCompanyDelete(CustomerCompanyViewModel customerCompanyGroup)
        {
            var singleCompanyGroup = uow.Repository<CustCompGroup>().FindBy(x => x.CCGroupID == customerCompanyGroup.CCGroupID).FirstOrDefault();
            uow.Repository<CustCompGroup>().Delete(singleCompanyGroup);
            uow.Commit();
        }

        public bool CheckExists(string CCGroupName, int CCGroupID = 0)
        {
            int count = uow.Repository<CustCompany>().GetAll().Where(x => x.CCName.ToLower().Trim() == CCGroupName.ToLower().Trim()).Where(x => x.CCGroupID != CCGroupID).Count();

            if (count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
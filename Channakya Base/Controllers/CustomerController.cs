using ChannakyaBase.BLL.Service;
using ChannakyaBase.DAL.DatabaseModel;
using ChannakyaBase.Model.ViewModel;
using PagedList;
using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChannakyaBase.BLL.CustomHelper;
using Loader.Service;
using ChannakyaBase.Model.Models;
using ChannakyaBase.BLL;
using Loader;
using System.Data.Entity.Validation;

namespace ChannakyaBase.Web.Controllers
{

    [MyAuthorize]
    public class CustomerController : Controller
    {
        ReturnBaseMessageModel returnMessage = null;
        private CustomerService customerService = null;
        private EmployeeService employeeService = null;


        public CustomerController()
        {
            returnMessage = new ReturnBaseMessageModel();
            employeeService = new EmployeeService();
            customerService = new CustomerService();


        }
        // GET: Customer
        public ActionResult CustomerListIndex()
        {
            CustInformationViewModel customerInfo = new CustInformationViewModel();
            var customerInfoList = customerService.CustomerList("", "", "", null, 1, 10);
            customerInfo.CustomerInfoList = new StaticPagedList<CustInformationViewModel>(customerInfoList, 1, 10, (customerInfoList.Count == 0) ? 0 : customerInfoList.FirstOrDefault().TotalCount); ;
            return PartialView(customerInfo);
            
        }
        #region CustomerCreate

        public ActionResult _CustomerList(string name, string address, string contact, int? cType,string accountnumber, int pageNo = 1, int pageSize = 10)
        {
            CustInformationViewModel customerInfo = new CustInformationViewModel();
            var customerInfoList = customerService.CustomerList(name, address, contact, cType, pageNo, pageSize);
            customerInfo.CustomerInfoList = new StaticPagedList<CustInformationViewModel>(customerInfoList, pageNo, pageSize, (customerInfoList.Count == 0) ? 0 : customerInfoList.FirstOrDefault().TotalCount); ;
            return PartialView(customerInfo);
        }
        [HttpGet]
        public ActionResult _CustomerCreate(decimal cId = 0)
        { 
            var UserId = Loader.Models.Global.UserId;
            CustInfo cust = new CustInfo();
            if (cId != 0)
            {
                var custInfo2 = customerService.GetCustomerForEdit(Convert.ToInt32(cId));
                cust = custInfo2;
                var custType = customerService.GetCustomerType(cust.CtypeID).isind;
                ViewBag.cType = custType;
                ViewBag.CustomerList = customerService.GetCustomerListFromMaster();

            }
            else
            {
                ViewBag.CustomerList = customerService.GetCustomerListFromMaster();
                // ViewBag.OccupationList = customerService.GetOccupationList();
                //cust = customerService.GetSingleCustomer(Convert.ToInt32(cId));
                cust.CRegdate = new CommonService().GetBranchTransactionDate();
                var custAdd = customerService.CustAddressType();


                foreach (var item in custAdd)
                {
                    CustAddress custAD = new CustAddress();
                    custAD.AddressTypeId = item.ATypeId;
                    custAD.LType = item.LType;
                    cust.CustAddresses.Add(custAD);
                }

            }
            CustType c1 = new CustType();

            cust.CustTypeCertificate = customerService.GetEditCertificateTypeByCtype(cust.CtypeID, cId).ToList();
            c1.CustTypeContacts = customerService.GetCustTypeContacts(cust.CtypeID).ToList();
            cust.CustType = c1;
            //cust.CustContacts = customerService.GetContactByCtype(cust.CtypeID).ToList();
            return PartialView(cust);
        }
        [HttpPost]
        public ActionResult _CustomerCreate(CustInfo cinfo, CustIndividual cind, List<CustContact> CustContactList, CustCompany ccmp, List<CustTypeCertificate> CustCertList, List<CustAddress> CustAddressList, List<DAL.DatabaseModel.CustContactPerson> CustContactPerson)
        {


            try
            {
                returnMessage = customerService.Save(cinfo, cind, CustContactList, ccmp, CustCertList, CustAddressList, CustContactPerson);
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
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
                throw ex;

                  }

             }
        public ActionResult CheckCheckedBox(List<CustTypeCertificate> CustCertList)
        {
            CustIndividual custInd = new CustIndividual();

            return PartialView(custInd);
        }
        public ActionResult _CustomerIndividualCreate()
        {
            CustIndividual custInd = new CustIndividual();

            return PartialView(custInd);
        }
        public ActionResult _CustomerCompanyCreate()
        {
            CustCompany custComp = new CustCompany();
            return PartialView(custComp);
        }
        public ActionResult _AttachedCertificate(int cTypeId)
        {
            var certificateType = customerService.GetCertificateTypeByCtype(cTypeId).ToList();
            //return Json(certificateType, JsonRequestBehavior.AllowGet);
            return PartialView("_AttachedCertificateCreate", certificateType);
        }
        public ActionResult GetCascadeCert(int? id)
        {
            SelectList custcert = CustomerUtilityService.CustTypeCertificateList(Convert.ToByte(id));
            return Json(custcert, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCompCert(int? id)
        {
            SelectList custcert = CustomerUtilityService.CustTypeCompCertificateList(Convert.ToByte(id));
            return Json(custcert, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCascadeSect(int? id)
        {
            SelectList custsect = CustomerUtilityService.CustTypeSectorList(Convert.ToByte(id));
            return Json(custsect, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCustypeByCTypeId(int ctypeId = 0)
        {
            var custType = customerService.GetCustomerType(ctypeId).isind;
            if (custType == 0)
                return PartialView("_CustomerCompanyCreate");
            else
                return PartialView("_CustomerIndividualCreate");
        }

        public ActionResult GetCustypeByIsIndividual(int id = 0)
        {
            var custType = customerService.GetCustomerType(id).isind;
            return Json(custType,JsonRequestBehavior.AllowGet);
        }
        public ActionResult _CustContactCreate(byte id = 0)
        {
            CustContact custContact = new CustContact();
            custContact.CtypeId = id;
            return PartialView(custContact);
        }

        [HttpGet]
        public ActionResult _CustContactPerson()
        {
            DAL.DatabaseModel.CustContactPerson custContactPerson = new DAL.DatabaseModel.CustContactPerson();
            return PartialView(custContactPerson);

        }
        public ActionResult _CustContactCompulsory(byte id = 0)
        {
            List<CustContact> custContact = new List<CustContact>();

            CustContact cModel = new CustContact();
            
            var cntlst = customerService.CustCompulSoryContactType(id);
            foreach (var item in cntlst)
            {
                CustContact custCompContact = new CustContact();
                custCompContact.CNotype = item.CNoType;
                custCompContact.CtypeId = id;
                custContact.Add(custCompContact);
            }
            cModel.CustContList = custContact;
           // cModel.ContactDef =
            return PartialView(cModel);
        }
        public ActionResult _CustAddress(byte id = 0)
        {
            var custad = customerService.CustAddressType();
            return PartialView(custad);
        }
        public ActionResult GetCascadeContactType(int? id)
        {
            SelectList custcontact = CustomerUtilityService.CustContactType(Convert.ToByte(id));
            return Json(custcontact, JsonRequestBehavior.AllowGet);
        }



        #endregion

        #region Customer Details


        [HttpGet]
        public ActionResult _CustomerDetails(decimal cId = 0, string modelFrom = "")
        {
            CustInfo cust = new CustInfo();
            cust = customerService.GetSingleCustomer(Convert.ToInt32(cId));
            var custType = customerService.GetCustomerType(cust.CtypeID).isind;
            ViewBag.cType = custType;
            ViewBag.ModelFrom = modelFrom;
            cust.CustomerAccounts = customerService.GetCustomerAccounts(cust.CID);
            cust.CustTypeCertificate = customerService.GetCertificateTypeByCtype(cust.CtypeID).ToList();
            return PartialView(cust);
        }

        public ActionResult _CustomerIndividualDetails()
        {
            CustIndividual custInd = new CustIndividual();
            return PartialView(custInd);
        }
        public ActionResult _CustomerCompanyDetails()
        {
            CustCompany custComp = new CustCompany();
            return PartialView(custComp);
        }
        public ActionResult _AttachedCertificateDetails(int cTypeId)
        {
            var certificateType = customerService.GetCertificateTypeByCtype(cTypeId).ToList();
            //foreach (var item in certificateType)
            //{
            //    ViewBag.CState = item.CCState;
            //}
           
            return PartialView("_AttachedCertificateCreate", certificateType);
        }


        public ActionResult GetCustypeByCTypeIdDetails(int ctypeId = 0)
        {
            var custType = customerService.GetCustomerType(ctypeId).isind;
            if (custType == 0)
                return PartialView("_CustomerCompanyCreate");
            else
                return PartialView("_CustomerIndividualCreate");
        }
        public ActionResult _CustContactDetails(byte id = 0)
        {
            CustContact custContact = new CustContact();
            custContact.CtypeId = id;
            return PartialView(custContact);
        }

        [HttpGet]
        public ActionResult _CustContactPersonDetails()
        {
            DAL.DatabaseModel.CustContactPerson custContactPerson = new DAL.DatabaseModel.CustContactPerson();
            return PartialView(custContactPerson);

        }
        public ActionResult _CustContactCompulsoryDetails(byte id = 0)
        {
            List<CustContact> custContact = new List<CustContact>();

            CustContact cModel = new CustContact();
            var cntlst = customerService.CustCompulSoryContactType(id);
            foreach (var item in cntlst)
            {
                CustContact custCompContact = new CustContact();
                custCompContact.CNotype = item.CNoType;
                custCompContact.CtypeId = id;
                custContact.Add(custCompContact);
            }
            cModel.CustContList = custContact;
            return PartialView(cModel);
        }
        public ActionResult _CustAddressDetails(byte id = 0)
        {
            var custad = customerService.CustAddressType();
            return PartialView(custad);
        }


        #endregion


        #region CustomerSearch
        public ActionResult CustomerInfoList(int[] listBox, string mode, string custType, int pageNo = 1, int pageSize = 10)
        {
            CustInformationViewModel custInfoModel = new CustInformationViewModel();
            var custtomerList = customerService.CustomerInfoList("", "", "", custType, pageNo, pageSize);
            custInfoModel.CustomerInfoList = new StaticPagedList<CustInformationViewModel>(custtomerList, pageNo, pageSize, (custtomerList.Count == 0) ? 0 : custtomerList.FirstOrDefault().TotalCount);
            List<CustInformationViewModel> selectMultipleList = new List<CustInformationViewModel>();
            if (mode != ECustomerSearchType.SingleType.GetDescription())
            {
                selectMultipleList = customerService.GetSelectedMultipleCustomer(listBox);
                custInfoModel.CIDs = listBox;
            }
            custInfoModel.Mode = mode;
            custInfoModel.CustomerType = custType;
            custInfoModel.SelectedCustInfoList = selectMultipleList;
            return PartialView("CustomerInfoList", custInfoModel);
        }

        public ActionResult _CustomerInfoList(string searchParam, string searchOption, int[] listBox, string mode, string custType, int pageNo = 1, int pageSize = 10)
        {
            CustInformationViewModel custInfoModel = new CustInformationViewModel();
            List<CustInformationViewModel> custList = new List<CustInformationViewModel>();
            var custtomerList = customerService.CustomerInfoList(searchParam, searchOption, mode, custType, pageNo, pageSize);
            custInfoModel.CustomerInfoList = new StaticPagedList<CustInformationViewModel>(custtomerList, pageNo, pageSize, (custtomerList.Count == 0) ? 0 : custtomerList.FirstOrDefault().TotalCount);
            if (listBox != null)
            {
                foreach (var item in listBox)
                {
                    CustInformationViewModel selectCustInfoModel = new CustInformationViewModel();
                    selectCustInfoModel.CID = item;
                    custList.Add(selectCustInfoModel);
                }
            }

            custInfoModel.SelectedCustInfoList = custList;
            return PartialView("_CustomerInfoList", custInfoModel);
        }

        public ActionResult GetSelectedCustomer(int customerId, int[] listBox, string mode, string custType)
        {
            var singleCustomer = customerService.GetSelectedCustomer(customerId, custType);

            if (mode == ECustomerSearchType.Limited.GetDescription() || mode == ECustomerSearchType.AccountOpen.GetDescription())
            {

                if (singleCustomer.isind == 0 && listBox.Count() > 0)
                {
                    if (listBox.Where(x => x == 0).Count() == 1)
                        singleCustomer.Isselect = true;
                    else
                        singleCustomer.Isselect = false;
                }
                else
                {
                    var isCheck = customerService.GetSelectedCustomer(listBox[0], custType);
                    if (isCheck != null)
                    {
                        if (isCheck.isind == 0)
                            singleCustomer.Isselect = false;
                        else
                            singleCustomer.Isselect = true;
                    }
                    else
                    {
                        singleCustomer.Isselect = true;
                    }
                }
            }
            else
            {
                singleCustomer.Isselect = true;
            }

            return Json(singleCustomer, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetMultipleSelectedCustomer(int[] listBox)
        {
            var multipleCustomer = customerService.GetSelectedMultipleCustomer(listBox);
            return Json(multipleCustomer, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region EmployeeSearch
        public ActionResult EmployeeList(string text, int branchId = 0, string loadType = "", string searchType = "", int pageNo = 1, int pageSize = 5)
        {
            ChannakyaBase.Model.ViewModel.EmployeeViewModel empModel = new ChannakyaBase.Model.ViewModel.EmployeeViewModel();

            empModel.BranchId = branchId;
            empModel.LoadType = loadType;
            empModel.SearchType = searchType;
            var employeeList = customerService.GetEmployeeDetails(branchId, text, "", loadType, searchType, pageNo, pageSize);
            empModel.EmployeeList = new StaticPagedList<ChannakyaBase.Model.ViewModel.EmployeeViewModel>(employeeList, pageNo, pageSize, (employeeList.Count == 0) ? 0 : employeeList.Select(x => x.TotalCount).FirstOrDefault());
            return PartialView("EmployeeList", empModel);
        }

        public ActionResult _EmployeeDetails(string searchParam, string searchOption, int branchId = 0, string loadType = "", string searchType = "", int pageNo = 1, int pageSize = 5)
        {

            ChannakyaBase.Model.ViewModel.EmployeeViewModel empModel = new ChannakyaBase.Model.ViewModel.EmployeeViewModel();
            empModel.BranchId = branchId;
            empModel.LoadType = loadType;
            empModel.SearchType = searchType;
            var employeeList = customerService.GetEmployeeDetails(branchId, searchParam, searchOption, loadType, searchType, pageNo, pageSize);
            empModel.EmployeeList = new StaticPagedList<ChannakyaBase.Model.ViewModel.EmployeeViewModel>(employeeList, pageNo, pageSize, (employeeList.Count == 0) ? 0 : employeeList.Select(x => x.TotalCount).FirstOrDefault());
            return PartialView("_EmployeeDetails", empModel);
        }

        public ActionResult GetSelectedEmploye(int employeeId, string loadType)
        {
            ChannakyaBase.Model.ViewModel.EmployeeViewModel empModel = new ChannakyaBase.Model.ViewModel.EmployeeViewModel();
            if (loadType == EEmployeeOrShare.Employee.GetDescription())
            {
                var singleEmployeeById = employeeService.GetSingle(employeeId);
                empModel.EmployeeId = singleEmployeeById.EmployeeId;
                empModel.EmployeeName = singleEmployeeById.EmployeeName;
            }
            else
            {
                empModel = customerService.GetShareHolderDetails(employeeId);
            }

            return Json(empModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEmployeeName(string text, string loadType, string searchType)
        {

            var empList = customerService.GetEmployeeByText(text, loadType, searchType);
            return Json(empList, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region display Event message
        public ActionResult EventMessage(int idtype, int id = 0, int? formEventId = 0)
        {
            //ViewBag.CustomerId = id;
            CommonService cs = new CommonService();
            MessageInfoViewModel messageInfoViewModel = new MessageInfoViewModel();
            messageInfoViewModel.Fdate = cs.GetBranchTransactionDate();
            messageInfoViewModel.Tdate = cs.GetBranchTransactionDate();
            messageInfoViewModel.CustomerId = id;
            messageInfoViewModel.OpenFormEventid = Convert.ToInt32(formEventId);
            messageInfoViewModel.Eventid = Convert.ToByte(formEventId);
            if (idtype == 2)
            {
                string CustomerName = customerService.GetCustomerNameByAccno(id);
                messageInfoViewModel.CustomerName = CustomerName;
                messageInfoViewModel.Idtype = 2;
            }
            else
            {
                string CustomerName = customerService.GetCustomerName(id);
                messageInfoViewModel.CustomerName = CustomerName;
                messageInfoViewModel.Idtype = 1;
            }

            return PartialView(messageInfoViewModel);
        }
        [HttpPost]
        public ActionResult EventMessage(MessageInfoViewModel Msg)
        {
            Msg.EventValue = Msg.CustomerId;
            // Msg.Tdate = Convert.ToDateTime("10/08/2018");
            try
            {
                returnMessage = customerService.MessageCreate(Msg);
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns> filtered data in descending order by tdate</returns>
        public ActionResult GetEventListByCustomerId(int? id)
        {
            var MessageList = customerService.EventMessageList(id);
            List<MessageInfoViewModel> ValidDate = new List<MessageInfoViewModel>();
            List<MessageInfoViewModel> InvalidDate = new List<MessageInfoViewModel>();
            List<MessageInfoViewModel> MessagelistForDisplay = new List<MessageInfoViewModel>();
            //IEnumerable<MessageInfoViewModel> orderingvaliddate = new MessageInfoViewModel();
            CommonService transitiondate = new CommonService();
            DateTime compare = transitiondate.GetBranchTransactionDate();
            foreach (var item in MessageList)
            {
                int tdatecheck = item.Tdate.CompareTo(compare);
                int fdatecheck = item.Fdate.CompareTo(compare);

                if (tdatecheck >= 0 && fdatecheck <= 0)
                {
                    ValidDate.Add(item);
                }
                else
                {
                    InvalidDate.Add(item);
                }
            }
            IEnumerable<MessageInfoViewModel> ordervaliddate = ValidDate.OrderByDescending(x => x.Tdate);
            foreach (var item in ordervaliddate)
            {
                MessagelistForDisplay.Add(item);
            }
            foreach (var item in InvalidDate)
            {
                MessagelistForDisplay.Add(item);
            }
            return PartialView(MessagelistForDisplay);

        }

        /// <summary>
        /// retrive  Eventlist of multiple customer
        /// </summary>
        /// <param name="customerid"></param>
        /// <param name="EventId"></param>
        ///     //MessageList[0].Tdate = Convert.ToDateTime("08/10/2018");
        /// <returns> display EventMessage View</returns>
        public ActionResult GetEventListByCustomerIds(int[] customerid, int? EventId = 0)
        {
            var MessageList = customerService.EventMessageListbycustomerids(customerid, Convert.ToInt32(EventId));
            List<MessageInfoViewModel> MessagelistForDisplay = new List<MessageInfoViewModel>();
            CommonService transitiondate = new CommonService();
            DateTime compare = transitiondate.GetBranchTransactionDate();
            foreach (var item in MessageList)
            {
                int tdatecheck = item.Tdate.CompareTo(compare);
                int fdatecheck = item.Fdate.CompareTo(compare);

                if (tdatecheck >= 0 && fdatecheck <= 0)
                {
                    MessagelistForDisplay.Add(item);
                }
            }
            IEnumerable<MessageInfoViewModel> orderedvaliddateeventlist = MessagelistForDisplay.OrderByDescending(x => x.Tdate);
            return PartialView(orderedvaliddateeventlist);
        }
        public ActionResult GetCustomerEventListByAccountId(int accountid, int eventid)
        {
            var MessageList = customerService.GetCustomerIdByaccountno(accountid, eventid);
            List<MessageInfoViewModel> MessagelistForDisplay = new List<MessageInfoViewModel>();
            CommonService transitiondate = new CommonService();
            DateTime compare = transitiondate.GetBranchTransactionDate();
            foreach (var item in MessageList)
            {
                int tdatecheck = item.Tdate.CompareTo(compare);
                int fdatecheck = item.Fdate.CompareTo(compare);

                if (tdatecheck >= 0 && fdatecheck <= 0)
                {
                    MessagelistForDisplay.Add(item);
                }
            }
            IEnumerable<MessageInfoViewModel> orderedvaliddateeventlist = MessagelistForDisplay.OrderByDescending(x => x.Tdate);
            return PartialView("GetEventListByCustomerIds", orderedvaliddateeventlist);
        }
        #endregion

        #region CustomerCompanyGroup
        public ActionResult CustomerCompanyIndex()
        {
            return PartialView();
        }

        public ActionResult InsertUpdateCustomerCompanyGroup(int CCGroupID = 0)
        {
            CustomerCompanyViewModel customerCompanyGroup = new CustomerCompanyViewModel();
            if (CCGroupID != 0)
            {
                customerCompanyGroup = customerService.GetSingleCustomerCompanyGroup(CCGroupID);
            }

            return View(customerCompanyGroup);
        }

        [HttpPost]

        public ActionResult InsertUpdateCustomerCompanyGroup(CustomerCompanyViewModel cCGroup)
        {
            if (ModelState.IsValid)
            {
                returnMessage = customerService.InsertUpdateCompanyGroup(cCGroup);
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
            else
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Please fill the required field";
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }

        }


        public ActionResult CustomerCompanyList()
        {

            CustomerCompanyViewModel customerCompanyGroup = new CustomerCompanyViewModel();
            var customerCompanyGroupList = customerService.GetAllCustomerCompany();
            customerCompanyGroup.CustomerCompanyList = customerCompanyGroupList;
            return PartialView(customerCompanyGroup);
        }

        public JsonResult _CustomerCompanyDelete(int CCGroupID  = 0)
        {
            //CustCompGroup customerCompanyGroup = customerService.GetSingleCustomerCompanyGroupForDelete(CCGroupID);
            bool result = customerService.CheckForDeletionWithRemitCompany(CCGroupID);
            
            return Json(result, JsonRequestBehavior.AllowGet);

        }



        [HttpPost]
        public ActionResult _CustomerCompanyDeleteConfirm(int CCGroupID)
        {
            CustomerCompanyViewModel customerCompanyGroup = customerService.GetSingleCustomerCompanyGroup(CCGroupID);
            customerService.CustomerCompanyDelete(customerCompanyGroup);
            return RedirectToAction("CustomerCompanyList");
        }

      

        public JsonResult CheckCustomerCompany(string CCGroupName, int CCGroupID = 0)
        {

            bool ifExists = customerService.CheckExists(CCGroupName, CCGroupID);
            return Json(ifExists, JsonRequestBehavior.AllowGet);
        }
        #endregion
        public JsonResult CheckCustomerCompanyGroup(string CCGroupName, int CCGroupID = 0)
        {

            bool ifExists = customerService.CheckExistsGroup(CCGroupName, CCGroupID);
            return Json(ifExists, JsonRequestBehavior.AllowGet);
        }
    }

}
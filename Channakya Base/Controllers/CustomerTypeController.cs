using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ChannakyaBase.BLL.Service;
using ChannakyaBase.Model.ViewModel;
using ChannakyaBase.DAL.DatabaseModel;
using Loader;
using ChannakyaBase.Model.Models;

namespace ChannakyaBase.Web.Controllers
{
    [MyAuthorize]
    public class CustomerTypeController : Controller
    {
        private CustomerTypeService cts = null;
        private ReturnBaseMessageModel returnMessage = null;

        public CustomerTypeController()
        {
            cts = new CustomerTypeService();
            returnMessage = new ReturnBaseMessageModel();
        }
        // GET: CustomerType
        public ActionResult Index()
        {
            return View();
            //CustSectCertContViewModel objCustTypeContact = new CustSectCertContViewModel();
            //return View(objCustTypeContact);

        }
        public ActionResult _List()
        {
            return PartialView(cts.GetAll().ToList());
        }
        public JsonResult CheckCustType(string Ctype, int CtypeID = 0)
        {

            bool ifExists = cts.CheckExists(Ctype, CtypeID);
            return Json(ifExists, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult _CustomerTypeCreate(int CTypeID = 0)
        {

            ViewBag.myId = CTypeID;

            ViewBag.GetTaxOption = cts.GetTaxOption();
            ViewBag.CertificateList = cts.CertificateOptionList();
            ViewBag.ContactOption = cts.ContactOptionList();
            CustSectCertContViewModel objCustType = new CustSectCertContViewModel();
            objCustType.ContactDefList = cts.ContactAssigned(CTypeID).ToList();
            objCustType.CertifiacteDefList = cts.CertifiacteAssigned(CTypeID).ToList();
            objCustType.SectorDefList = cts.SectorAssigned(CTypeID).ToList();
            if (CTypeID != 0)
            {
                var custs = cts.GetSingle(CTypeID);

                objCustType.Ctype = custs.Ctype;
                objCustType.TaxID = custs.TaxID;
                objCustType.isind = Convert.ToBoolean(custs.isind);
            }

            return PartialView(objCustType);
        }

        [HttpPost]
        public ActionResult _CustomerTypeCreate(CustSectCertContViewModel ct)
        {
            try
            {

                //if (ct.CertifiacteDefList.Where(x => x.CertStatus == 2).Count() >= 1&& ct.ContactDefList.Where(x => x.ContStatus == 2).Count() >= 1)

                //bool ifExists = false;
                //ifExists = cts.CheckExists(ct.Ctype, ct.CtypeID);
                //if (!ifExists)
                //{
                //    returnMessage.Msg = "Customer Type Duplication";
                //    return Json(returnMessage, JsonRequestBehavior.AllowGet);
                //}


                if (ct.CertifiacteDefList.Where(x => x.CertStatus == 3).Count() >= 1)
                {
                    returnMessage = cts.CustomerTypeCreate(ct);
                    return Json(returnMessage, JsonRequestBehavior.AllowGet);
                    //return RedirectToAction("_List");
                }

               

                returnMessage.Success = false;
                returnMessage.Msg = "Please Select any id type certificate or contact.";

                //return JavaScript("OnFailure('Please Select any id type certificate.')");

                //if (ct.ContactDefList.Where(x => x.ContStatus == 3).Count() >= 1)
                //{
                //    returnMessage = cts.CustomerTypeCreate(ct);
                //    return Json(returnMessage, JsonRequestBehavior.AllowGet);
                //    //return RedirectToAction("_List");
                //}
                //else
                //{
                //    returnMessage.Success = false;
                //    returnMessage.Msg = "Please Select any type contact.";
                //}



                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { status = false, ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }


        public ActionResult _GetCertificateType()
        {
            return PartialView(cts.CertifcateTypeList());
        }
        public ActionResult _GetContactType()
        {
            return PartialView(cts.ContactTypeList());
        }
        public ActionResult _GetSectorType()
        {
            return PartialView(cts.SectorTypeList());
        }


        [HttpGet]
        public JsonResult Delete(int id)
        {
            bool result = true;
            if (cts.CheckExisitinCustomerRegistration(id) > 0)
            {
                result = false;
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DeleteConfirm(int id)
        {

            var certtype = cts.FindCertificatelistByCTypeId(id).ToList();
            foreach (var item in certtype)
            {
                CustTypeCertificate cert = cts.GetSingleCertificate(item.CTCID);
                cts.DeleteCertificate(cert);
            }
            var cncttype = cts.FindContListByCTypeId(id).ToList();
            foreach (var item in cncttype)
            {
                CustTypeContact cont = cts.GetSingleContact(item.CTCntID);
                cts.DeleteContactType(cont);
            }
            var custcert = cts.FindSectorListByCTypeId(id);

            foreach (var item in custcert)
            {
                CustTypeSector sect = cts.GetSingleSector(item.CTSID);
                cts.DeleteSector(sect);
            }

            CustType cust = cts.GetSingle(id);
            //int i = cts.DeleteCustomer(cust);
            int i = cts.DeleteCustomer(cust);
            //bool result = true;
            if (i == 0)
            {
                //result = false;
                return JavaScript("OnFailure('Cannot be deleted,used in customer.')");
            }

            //bool result = true;
            //return Json(result, JsonRequestBehavior.AllowGet);
            return JavaScript("OnSuccess('Successfully deleted')");
        }


    }
}

//CustType obj = cts.GetSingle(ct.CtypeID);
//if (obj == null)
//{
//    obj = new CustType();
//}

//obj.Ctype = ct.Ctype;
//obj.TaxID = ct.TaxID;
//obj.CtypeID = ct.CtypeID;
//obj.isind = Convert.ToByte(ct.isind);

//foreach (var item in ct.CertifiacteDefList)
//{
//    CustTypeCertificate certificate = new CustTypeCertificate();
//    if (item.CertStatus == 0 && item.CTCID != 0)
//    {
//        certificate = cts.GetSingleCertificate(item.CTCID);
//        if (certificate != null)
//        {
//            cts.DeleteCertificate(certificate);
//        }
//    }
//    else if (item.CertStatus != 0 && item.CTCID == 0)
//    {
//        certificate.CCState = item.CertStatus;
//        certificate.CCCertID = item.CCCertID;
//        obj.CustTypeCertificates.Add(certificate);
//    }
//    else if (item.CertStatus != 0 && item.CTCID != 0)
//    {
//        certificate.CCState = item.CertStatus;
//        certificate.CCCertID = item.CCCertID;
//        certificate.CTCID = item.CTCID;
//        certificate.CTypeID = ct.CtypeID;
//        cts.EditCertificateType(certificate);
//    }
//  }
//foreach (var item in ct.ContactDefList)
//{
//    CustTypeContact contact1 = new CustTypeContact();

//    if (item.ContStatus != 0 && item.CTCntID == 0)
//    {

//        contact1.CNoState = item.ContStatus;
//        contact1.CNoType = item.CNotype;
//        obj.CustTypeContacts.Add(contact1);
//    }

//    else if (item.ContStatus != 0 && item.CTCntID != 0)
//    {
//        contact1.CNoState = item.ContStatus;
//        contact1.CNoType = item.CNotype;
//        contact1.CTCntID = item.CTCntID;
//        contact1.CTypeID = ct.CtypeID;
//        cts.EditContactType(contact1);
//    }
//    else if (item.ContStatus == 0 && item.CTCntID != 0)
//    {
//        contact1 = cts.GetSingleContact(item.CTCntID);
//        if (contact1 != null)
//        {
//            cts.DeleteContactType(contact1);
//        }
//    }
//}
//foreach (var item in ct.SectorDefList)
//{
//    CustTypeSector custsector = new CustTypeSector();
//    if (item.SectorChecked == false && item.CTSID != 0)
//    {
//        custsector = cts.GetSingleSector(item.CTSID);
//        if (custsector != null)
//        {

//            cts.DeleteSector(custsector);
//        }
//    }

//    else if (item.SectorChecked == true && item.CTSID == 0)
//    {
//        {
//            custsector.CDepSector = item.CDepSector;
//            obj.CustTypeSectors.Add(custsector);
//        }
//    }
//}
//cts.Save(obj);
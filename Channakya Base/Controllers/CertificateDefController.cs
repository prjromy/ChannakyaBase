using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using ChannakyaBase.BLL.Service;
using ChannakyaBase.DAL.DatabaseModel;
using Loader;
using ChannakyaBase.Model.Models;

namespace ChannakyaBase.Web.Controllers
{
    [MyAuthorize]
    public class CertificateDefController : Controller
    {
        private CertificateDefService cs = null;
        ReturnBaseMessageModel returnMessage = null;
        public CertificateDefController()
        {
            cs = new CertificateDefService();
            returnMessage = new ReturnBaseMessageModel();
        }
        // GET: CertificateDef
      
        public ActionResult Index()
        {

            return PartialView();
        }

        public ActionResult _List(string search,int pageNo = 1, int pageSize = 5)
        {

            var list = cs.GetAll();
            ViewBag.searchedVal = search;
            var filteredlist = list.Where(x => x.CCCert.ToLower().Contains(search.ToLower()));
            if (search == null)
            {
                return PartialView(cs.GetAll().ToList());
            }
            return PartialView(filteredlist.ToList());
        }
        [HttpGet]
        public ActionResult _Create()
        {
            CertificateDef cert = new CertificateDef();
            return PartialView(cert);
        }
        [HttpPost]
        public ActionResult _Create(CertificateDef certificateDef)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result=cs.Save(certificateDef);
                    return Json(result,JsonRequestBehavior.AllowGet);
                }
                //return RedirectToAction("Index");
                else
                {
                    returnMessage.Success = false;
                    returnMessage.Msg = "Please fill out form please!!";
                    return Json(returnMessage, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {
                return JavaScript(ex.Message);
            }

        }

        public JsonResult CheckCertificate(string CCCert, int CCCertID = 0)
         {
           
            bool ifExists = cs.CheckExists(CCCert, CCCertID);
            return Json(ifExists, JsonRequestBehavior.AllowGet);
        }


        //for edit check certificate exist
        public JsonResult CheckCertificateAfterSave(string CCCert, int CCCertID = 0)
        {

            bool ifExists = cs.CheckCertificateAfterSave(CCCert, CCCertID);
            return Json(ifExists, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult _Edit(int? id)
        {
            CertificateDef cert = cs.GetSingle(id);
            if (cert == null)
            {
                return HttpNotFound();
            }
            return PartialView(cert);

        }
        [HttpPost]
        public ActionResult _Edit(CertificateDef cert)
        {

             var result=cs.Save(cert);
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
       
        public ActionResult Delete(int id)
        {

            bool result = false;
            if (cs.CheckCertMapped(id))
            {
                //return JavaScript("OnFailure('certificate type is already used.')");
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else {
                result = true;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
       
        public ActionResult DeleteConfirm(int id)
        {
            CertificateDef cert = cs.GetSingle(id);
            if (cert == null)
            {
                return HttpNotFound();
            }
            cs.Delete(cert);
            //bool result = true;
            //return Json(result, JsonRequestBehavior.AllowGet);
            return RedirectToAction("_List");
         
        }

    }
}
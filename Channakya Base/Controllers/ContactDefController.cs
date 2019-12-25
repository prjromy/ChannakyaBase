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
    public class ContactDefController : Controller
    {
        // GET: ContactDef
        ReturnBaseMessageModel returnMessage=null;
        private ContactDefService cnts = null;
        public ContactDefController()
        {
            cnts = new ContactDefService();

        }

        public ActionResult Index()
        {
            return View(cnts.GetAll().ToList());
        }
        public ActionResult _List(string search,int pageNo=1, int pageSize=5)
        {
            var list = cnts.GetAll();
            ViewBag.searchedVal = search;
            var filteredlist = list.Where(x => x.CNodesc.ToLower().Contains(search.ToLower()) || x.CNoabb.ToLower().Contains(search.ToLower()));
            if (search == null)
            {
                return PartialView(cnts.GetAll().ToList());
            }
            return PartialView(filteredlist.ToList());
            
        }
        public ActionResult _Create()
        {
            ContactDef contactDef = new ContactDef();
            return PartialView(contactDef);
        }
        [HttpPost]
        public ActionResult _Create(ContactDef contactDef)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result=cnts.Save(contactDef);
                    return Json(result, JsonRequestBehavior.AllowGet);                
                }
                else
                {
                    returnMessage.Success = false;
                    returnMessage.Msg = "Please fill out the form please!!!";
                    return Json(returnMessage, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return JavaScript(ex.Message);
            }          
        }



        //public JsonResult CheckContactType(string CNodesc, string CNoabb)
        //{
        //    var ifExists = true;
        //    int cert = cs.CheckContactDef(CNodesc, CNoabb);
        //    if (cert > 0)
        //    {
        //        ifExists = false;
        //    }
        //    return Json(ifExists, JsonRequestBehavior.AllowGet);
        //}
        public JsonResult CheckContactTypeDes(string CNodesc="",int CNotype = 0)
        {
           
            bool ifExists = cnts.CheckContactTypeDes(CNodesc, CNotype);
            return Json(ifExists, JsonRequestBehavior.AllowGet);
           
        }
        public JsonResult CheckContactTypeAb(string CNoabb = "", int CNotype = 0)
        {

            bool ifExists = cnts.CheckContactTypeAbb(CNoabb, CNotype);
            return Json(ifExists, JsonRequestBehavior.AllowGet);

        }

        public ActionResult _Edit(int? id)
        {
            ContactDef contactDef = cnts.GetSingle(id);
            if (contactDef == null)
            {
                return HttpNotFound();
            }
            return PartialView(contactDef);

        }
        [HttpPost]
        public ActionResult _Edit(ContactDef contactDef)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = cnts.Save(contactDef);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    returnMessage.Success = false;
                    returnMessage.Msg = "Pleasse fill out form please!!";
                    return Json(returnMessage, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return JavaScript(ex.Message);
            }
         
        }
         
        [HttpGet]
        public ActionResult Delete(int id)
        {
            bool result = true;
            if (cnts.CheckContMapped(id) ==false)
            {
                result = false;
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            else
            {
                result = true;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
                
            
        }
        [HttpPost]
        public ActionResult DeleteConfirm(int id)
        {
            ContactDef contactDef = cnts.GetSingle(id);
            if (contactDef == null)
            {
                return HttpNotFound();
            }
            cnts.Delete(contactDef);
            return RedirectToAction("_List");

        }
    }
}
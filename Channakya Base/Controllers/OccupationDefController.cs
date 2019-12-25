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
    public class OccupationDefController : Controller
    {
        // GET: OccupationDef
        private OccupationDefService os = null;       
        ReturnBaseMessageModel returnMessage = null;

        public OccupationDefController()
        {
            os = new OccupationDefService();
        }
        // GET: OccupationDef
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult _List(string search, int pageNo = 1, int pageSize = 5)
        {
            var list = os.GetAll();
            ViewBag.searchedVal = search;
            var filteredlist = list.Where(x=>x.occupation.ToLower().Contains(search.ToLower()));
            if (search == null)
            {
                return PartialView(os.GetAll().ToList());
            }
            return PartialView(filteredlist.ToList());


        }

        public ActionResult _Create()
        {
            OccupationDef occupationDef = new OccupationDef();
            return PartialView(occupationDef);
        }
        [HttpPost]
        public ActionResult _Create(OccupationDef occupationDef)
        {
            try
            {
                if (ModelState.IsValid)
                {                  
                   
                    var result= os.Save(occupationDef);
                    return Json(result, JsonRequestBehavior.AllowGet);                   
                }
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
        public JsonResult CheckOccupation(string occupation,int Occpn = 0)
        {
            bool ifExists = os.CheckOccupation(occupation, Occpn);
            return Json(ifExists, JsonRequestBehavior.AllowGet);

        }
        public ActionResult _Edit(int? id)
        {
            OccupationDef occupationDef = os.GetSingle(id);
            if (occupationDef == null)
            {
                return HttpNotFound();
            }
            return PartialView(occupationDef);

        }
        [HttpPost]
        public ActionResult _Edit(OccupationDef occupationDef)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = os.Save(occupationDef);
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
            if (os.CheckOccMapped(id) >= 1)
            {
                result = false;
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            else {
               
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult DeleteConfirm(int id)
        {
            OccupationDef occupationDef = os.GetSingle(id);
            if (occupationDef == null)
            {
                return HttpNotFound();
            }
            os.Delete(occupationDef);
            return RedirectToAction("_List");

        }
    }
}
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
    public class SectorDefController : Controller
    {
        private SectorDefService sds = null;
        ReturnBaseMessageModel returnMessage = null;
        public SectorDefController()
        {
            sds = new SectorDefService();

        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _List(int pageNo = 1, int pageSize = 5)
        {      
            
                return PartialView(sds.GetAll().ToList());

        }
        [HttpGet]
        public ActionResult _Create()
        {
            SectorDef SectorDef = new SectorDef();
            return PartialView(SectorDef);
        }
        [HttpPost]
        public ActionResult _Create(SectorDef SectorDef)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var result = sds.Save(SectorDef);
                    return Json(result, JsonRequestBehavior.AllowGet);
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

        public JsonResult CheckSector(string CDepSectorNam, int CDepSector = 0)
            {

            bool ifExists = sds.CheckExists(CDepSectorNam, CDepSector);
            return Json(ifExists, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult _Edit(int? id)
        {
            SectorDef sect = sds.GetSingle(id);
            if (sect == null)
            {
                return HttpNotFound();
            }
            return PartialView(sect);

        }
        [HttpPost]
        public ActionResult _Edit(SectorDef SectorDef)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var result = sds.Save(SectorDef);
                    return Json(result, JsonRequestBehavior.AllowGet);
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

        [HttpGet]
        public ActionResult Delete(int id)
        {


            bool result = true;
            if (sds.CheckSectorMapped(id))
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
            SectorDef sect = sds.GetSingle(id);
            sds.Delete(sect);
            //bool result = true;
            //return Json(result, JsonRequestBehavior.AllowGet);
            return RedirectToAction("_List");

        }

    }
}
    

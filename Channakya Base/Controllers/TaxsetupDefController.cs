using ChannakyaBase.BLL.Service;
using ChannakyaBase.DAL.DatabaseModel;
using ChannakyaBase.Model.Models;
using Loader;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChannakyaBase.Web.Controllers
{
    [MyAuthorize]
    public class TaxsetupDefController : Controller
    {

        private TaxSetupDefService tsds = null;
        ReturnBaseMessageModel returnMessage = null;
        public TaxsetupDefController()
        {
            tsds = new TaxSetupDefService();
            returnMessage = new ReturnBaseMessageModel();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _List(string search, int pageNo = 1, int pageSize = 5)
        {
            var list = tsds.GetAll();
            ViewBag.searchedVal = search;
            var filteredlist = list.Where(x => x.TaxName.ToLower().Contains(search.ToLower()));
            if (search == null)
            {
                return PartialView(tsds.GetAll().ToList());
            }
            return PartialView(filteredlist.ToList());
        }

        [HttpGet]
        public ActionResult _Create()
        {
            TaxsetupDef contactDef = new TaxsetupDef();
            return PartialView(contactDef);
        }
        //[HttpPost]
        //public ActionResult _Create(TaxsetupDef taxSetupDef)
        //{

        //    if (ModelState.IsValid)
        //    {

        //        tsds.Save(taxSetupDef);
        //        return RedirectToAction("Index");
        //    }
        //    return PartialView();

        //}
        [HttpPost]
        public ActionResult _Create(TaxsetupDef taxSetupDef)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = tsds.Save(taxSetupDef);
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

        public JsonResult CheckTaxTypeDef(string TaxName = "", int TaxID = 0)
        {

            bool ifExists = tsds.CheckTaxTypeDef(TaxName, TaxID);
            return Json(ifExists, JsonRequestBehavior.AllowGet);

        }


        public ActionResult _Edit(int? id)
        {
            TaxsetupDef taxsetupDef = tsds.GetSingle(id);
            if (taxsetupDef == null)
            {
                return HttpNotFound();
            }
            return PartialView(taxsetupDef);

        }
        [HttpPost]
        public ActionResult _Edit(TaxsetupDef taxsetupDef)
        {
            var result = tsds.Save(taxsetupDef);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (tsds.CheckTaxMapped(id) >= 1)
            {
                return JavaScript("OnFailure('certificate type is already used.')");

            }
            else
            {
                bool result = true;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult DeleteConfirm(int id)
        {
            TaxsetupDef taxsetupDef = tsds.GetSingle(id);
            if (taxsetupDef == null)
            {
                return HttpNotFound();
            }
            tsds.Delete(taxsetupDef);
            return RedirectToAction("_List");

        }
    }
}

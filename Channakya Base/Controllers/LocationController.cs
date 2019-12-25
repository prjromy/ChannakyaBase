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
using static ChannakyaBase.BLL.CustomHelper.CustomHelper;

namespace ChannakyBase.Controllers
{
    [MyAuthorize]
    public class LocationController : Controller
    {

        private LocationService ls = null;
        private ReturnBaseMessageModel returnBaseMessage = null;

        public LocationController()
        {
            ls = new LocationService();
            returnBaseMessage = new ReturnBaseMessageModel();
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult _GetLocationTree(bool withCheckBox, bool withImageIcon, int selectedNode, bool allowSelectGroupNode, List<int> withOutMe, int rootNode)
        {  
            var tree = ls._GetLocationTree( withCheckBox,  withImageIcon,  selectedNode,  allowSelectGroupNode, withOutMe,  rootNode);
            tree.Title = "Location";
            ViewData["withImage"] = true;
            return PartialView("~/Views/Tree/LocationTree.cshtml", tree);
        }
        public ActionResult _GetLocationMoveTree(List<int> selectedNodeId)
        {

            int? currentId = ls._GetLocationMoveTree(selectedNodeId);
            

            return Json(currentId, JsonRequestBehavior.AllowGet);
        }
        public ActionResult _ListChildLocation(int? parentNodeId = 0)
        {
            string address = "";
            int maxLevel = 0;
            int currentLevel = 0;
            int level = 0;

            var chieldLocationList = (from m in ls.GetAllLocation().Where(m => m.PLId == parentNodeId) select m).ToList();
            ViewBag.childList = chieldLocationList;
            ////<--------- Back Button --------->////

            int? PLId = parentNodeId;

            if (parentNodeId != 0)
            {

                ////<--------- Address --------->////

                while (PLId > 0)
                {
                    var locations = ls.GetAllLocation().Where(m => m.LId == PLId).Select(m => new { m.LocationName, m.PLId }).SingleOrDefault();
                    if (locations != null)
                    {
                        address = " => " + locations.LocationName + address;
                        PLId = Convert.ToInt32(locations.PLId);
                    }
                }

                address = "Location" + address;

                ////<--------- Create button within level --------->////

                level = (from m in ls.GetAllLocation().Where(m => m.LId == parentNodeId) select m.Lvl).SingleOrDefault();
                currentLevel = level;
                int? parentNode = parentNodeId;
                int? currentNodeId = 0;

                while (currentLevel > 1)
                {
                    currentNodeId = (from m in ls.GetAllLocation().Where(m => m.LId == parentNode) select m.PLId).SingleOrDefault();
                    parentNode = currentNodeId;
                    currentLevel = (from m in ls.GetAllLocation().Where(m => m.LId == currentNodeId) select m.Lvl).SingleOrDefault();
                }

                maxLevel = (from m in ls.GetAllLocationMaster().Where(m => m.LId == parentNode) select m.MaxLvl).SingleOrDefault();
            }

            else
            {
                address = "Root";
                currentLevel = 0;
                maxLevel = 1;
                level = 0;
            }


            ////<--------- Location List --------->////

            var location = (from m in ls.GetAllLocation().Where(m => m.PLId == parentNodeId)
                            select m);


            ViewBag.ParentNodeId = parentNodeId;
            ViewBag.Address = address;
            ViewBag.MaxLevel = maxLevel;
            ViewBag.CurrentLevel = level;
            ViewBag.ChieldLevel = chieldLocationList.Select(x => x.Lvl).FirstOrDefault();
            return PartialView(location.ToList());
        }
        public ActionResult _GetParentId(int nodeId)
        {
            int? parentNode = 0;
            if (nodeId != 0)
            {
                parentNode = (from m in ls.GetAllLocation().Where(m => m.LId == nodeId) select m.PLId).SingleOrDefault();
            }
            return Json(parentNode, JsonRequestBehavior.AllowGet);
        }
        public ActionResult _ConfirmDelete(int parentNodeId)
        {
            ViewBag.ParentNodeId = parentNodeId;
            return PartialView();
        }
        public ActionResult _DeleteLocation(int parentNodeId)
        {
            string message = ls._DeleteLocation(parentNodeId);
            return Json(new { message = message }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult _AddLocation(int parentNodeId)
        {
            int currentLevel = 0;
            int level = 0;
            ViewBag.ParentNodeId = parentNodeId;


            if (parentNodeId != 0)
            {
                ViewBag.ParentLocation = (from m in ls.GetAllLocation().Where(m => m.LId == parentNodeId) select m.LocationName).SingleOrDefault();
                ViewBag.ParentLevel = (from m in ls.GetAllLocation().Where(m => m.LId == parentNodeId) select m.Lvl).SingleOrDefault();
                level = (from m in ls.GetAllLocation().Where(m => m.LId == parentNodeId) select m.Lvl).SingleOrDefault();
                currentLevel = level;
                int? parentNode = parentNodeId;
                int? currentNodeId = 0;

                while (currentLevel > 1)
                {
                    currentNodeId = (from m in ls.GetAllLocation().Where(m => m.LId == parentNode) select m.PLId).Single();
                    parentNode = currentNodeId;
                    currentLevel = (from m in ls.GetAllLocation().Where(m => m.LId == currentNodeId) select m.Lvl).Single();
                }

                ViewBag.MaxLevel = (from m in ls.GetAllLocationMaster().Where(m => m.LId == parentNode) select m.MaxLvl).SingleOrDefault();
            }
            else
            {
                ViewBag.ParentLocation = "Root";
                ViewBag.ParentLevel = 0;
            }

            return PartialView();
        }
        public ActionResult _EditLocation(int? parentNodeId)
        {
            int level = 0;
            int? parentNode = parentNodeId;

            level = (from m in ls.GetAllLocation().Where(m => m.LId == parentNodeId) select m.Lvl).SingleOrDefault();
            if (level == 1)
            {
                ViewBag.maxLevel = (from m in ls.GetAllLocationMaster().Where(m => m.LId == parentNodeId) select m.MaxLvl).SingleOrDefault();
            }
            var dummy = 0;
            //if ((from m in db.CustCompany.Where(m => m.LocationId == parentNodeId) select m.CustId).Count() != 0 ||
            //    (from m in db.CustIndividual.Where(m => m.LocationId == parentNodeId) select m.CustId).Count() != 0)
            //{
            //    ViewBag.result = 0;
            //}
            //else
            //{
            //    ViewBag.result = 1;
            //}
            if (dummy == 0)
            {
                ViewBag.result = 1;
            }
            else
            {
                ViewBag.result = 0;
            }

            ViewBag.currentLevel = level;
            ViewBag.currentId = parentNodeId;
            ViewBag.currentLocationName = (from m in ls.GetAllLocation().Where(m => m.LId == parentNodeId) select m.LocationName).SingleOrDefault();
            ViewBag.parentId = (from m in ls.GetAllLocation().Where(m => m.LId == parentNodeId) select m.PLId).SingleOrDefault();

            return PartialView();
        }
        public ActionResult _LocationDetail(int? parentNodeId)
        {
            int level = 0;
            int maxLevel = 0;
            int? PLId = 0;
            string locationName = "";
            string locationParent = "";
            bool IsGroup = (from m in ls.GetAllLocation().Where(m => m.LId == parentNodeId) select m.IsGroup).SingleOrDefault();

            PLId = (from m in ls.GetAllLocation().Where(m => m.LId == parentNodeId) select m.PLId).Single();
            locationName = (from m in ls.GetAllLocation().Where(m => m.LId == parentNodeId) select m.LocationName).SingleOrDefault();
            if (PLId == 0)
            {
                locationParent = "Root";
                ViewBag.parentLocation = locationParent;
            }
            else
            {
                locationParent = (from m in ls.GetAllLocation().Where(m => m.LId == PLId) select m.LocationName).SingleOrDefault();
                ViewBag.parentLocation = locationParent;
            }
            locationParent = (from m in ls.GetAllLocation().Where(m => m.LId == PLId) select m.LocationName).SingleOrDefault();
            level = (from m in ls.GetAllLocation().Where(m => m.LId == parentNodeId) select m.Lvl).SingleOrDefault();
            if (IsGroup == true)
            {
                ViewBag.locationGroup = "Yes";
            }
            else
            {
                ViewBag.locationGroup = "No";
            }
            if (level == 1)
            {
                maxLevel = (from m in ls.GetAllLocationMaster().Where(m => m.LId == parentNodeId) select m.MaxLvl).SingleOrDefault();
                ViewBag.locationMaxLevel = maxLevel;
            }

            ViewBag.locationLevel = level;
            ViewBag.locationName = locationName;


            return PartialView();
        }
        public ActionResult _DeleteLevel(int parentNodeId, int newMaxLevel)
        {
            ViewBag.newMaxLevel = newMaxLevel;
            ViewBag.maxLevel = (from m in ls.GetAllLocationMaster().Where(m => m.LId == parentNodeId) select m.MaxLvl).SingleOrDefault();
            return PartialView();
        }
        public ActionResult _DeleteSelectedLevel(byte level, int rootNode)
        {
            ls._DeleteSelectedLevel(level, rootNode);
            var maxLevel = from m in ls.GetAllLocationMaster().Where(m => m.LId == rootNode) select m.MaxLvl;
            return Json(maxLevel, JsonRequestBehavior.AllowGet);
        }
        public ActionResult _CreateSelectedLevel(int rootNode, byte level, string locationName)
        {
            ls._CreateSelectedLevel(rootNode, level, locationName);
            var maxLevel = from m in ls.GetAllLocationMaster().Where(m => m.LId == rootNode) select m.MaxLvl;
            return Json(maxLevel, JsonRequestBehavior.AllowGet);
        }
        public ActionResult _MoveLocation(List<int> selectedNodeId, int parentNodeId)
        {
            
            List<string> message = new List<string>();
            message = ls._MoveLocation(selectedNodeId, parentNodeId);

     
            return Json(new { message = message, rootNode = parentNodeId }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult _AddLocation([Bind(Prefix = "Item1")] Location location, [Bind(Prefix = "Item2")] LocationMaster locationMaster) 
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {
                   returnBaseMessage = ls._AddLocation(location, locationMaster);
                    //message = "Location " + location.LocationName + " created successfully!";
                    //return Json(messageModel, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    returnBaseMessage.Msg = "Model Not Valid";
                    returnBaseMessage.Success = false;
                    //return Json(returnBaseMessage, JsonRequestBehavior.AllowGet);
                }
               
            }
            return Json(returnBaseMessage, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        //public ActionResult _EditLocation([Bind(Prefix = "Item1", Include = "LId, LocationName")] Location location)
        //{
        //    var locationName = (from m in ls.GetAllLocation().Where(m => m.LId == location.LId) select m.LocationName).SingleOrDefault();
        //    var message = "";
        //    if (locationName != location.LocationName)
        //    {
        //        ls._EditLocation(location);
        //        message = "Location " + locationName + " edited successfully!";
        //    }
        //    return Json(new { message = message, parentId = location.PLId }, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult _EditLocation([Bind(Prefix = "Item1")] Location location, [Bind(Prefix = "Item2")] LocationMaster locationMaster)
        {
            var locationName = (from m in ls.GetAllLocation().Where(m => m.LId == location.LId) select m.LocationName).SingleOrDefault();
            
            var message = "";
            if (locationName != location.LocationName)
            {
                ls._EditLocation(location, locationMaster);
                message = "Location " + locationName + " edited successfully!";
            }
            return Json(new { message = message, parentId = location.PLId }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetLocation(string locationName)
        {
            var locationList = ls.LocationList().AsQueryable().Where(x => x.LocationName.StartsWith(locationName)).ToList();//from x in db.Designation select x;          

            if (locationList.Count() == 1)
            {
                return Json(new { LocationName = locationList[0].LocationName, LId = locationList[0].LId }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { LocationName = "", LId = 0 }, JsonRequestBehavior.AllowGet);
            }

        }


        public ActionResult _GetLocationList(string search, string sortOrder, int pageNo = 1, int pageSize = 5)
        {

            var filterList = new List<SelectListItem>();
            filterList = helperFunction.getSelectedListItem("Location", sortOrder);
            var locationList = new List<FGetLocationTB_Result>();
            locationList = ls.LocationList().AsQueryable().ToList();
            if (!String.IsNullOrEmpty(search))
            {
                locationList = ls.LocationList().Where(x =>Convert.ToString(x.LocationName).ToUpper().StartsWith(search.ToUpper())).AsQueryable().OrderByDescending(x => x.LocationName).ToList();
            }
            switch (sortOrder)
            {
                case "Location":
                    if (!String.IsNullOrEmpty(search))
                    {

                        locationList = ls.LocationList().OrderByDescending(x => x.LocationName).Where(x => Convert.ToString(x.LocationName).ToUpper().StartsWith(search.ToUpper())).ToList(); ;//from x in db.Designation select x;          
                    }

                    else
                    {
                        locationList = ls.LocationList().OrderByDescending(x => x.LocationName).ToList();
                    }
                    break;
            }
            var searchFilterList = locationList.ToPagedList(pageNo, pageSize);
            //filterList = helperFunction.getSelectedListItem("CustomerName,Industry", sortOrder);
            ViewBag.sortFilter = filterList;

            return PartialView("_LocationSearch", searchFilterList);

        }

        public JsonResult CheckLocation(string LocationName, int LId = 0)
        {

            bool ifExists = ls.CheckExists(LocationName, LId);
            return Json(ifExists, JsonRequestBehavior.AllowGet);
          
        }
    }
}

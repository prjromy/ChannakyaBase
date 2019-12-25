
using ChannakyaBase.BLL.Repository;
using ChannakyaBase.DAL.DatabaseModel;
using ChannakyaBase.Model.Models;
using ChannakyaBase.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ChannakyaBase.BLL.Service
{
    public class LocationService
    {
        private GenericUnitOfWork uow = null;
        private ReturnBaseMessageModel returnMessageModel = null;
        public LocationService()
        {
            uow = new GenericUnitOfWork();
            returnMessageModel = new ReturnBaseMessageModel();
        }
        public LocationTreeView GenerateLocationTree(List<Location> list, int? id)
        {
            var parent = list.Where(x => x.PLId == id);
            //List<ViewModel.TreeView> tree = new List<ViewModel.TreeView>();
            LocationTreeView tree = new LocationTreeView();

            foreach (var item in parent)
            {
                tree.TreeData.Add(new LocationTreeDTO
                {
                    Id = item.LId,
                    PId = item.PLId,
                    Text = item.LocationName,
                    IsGroup = Convert.ToBoolean(item.IsGroup)
                });
            }

            foreach (var item in tree.TreeData)
            {
                item.Children = GenerateLocationTree(list, item.Id).TreeData.ToList();
            }
            return tree;
        }
        //public List<FGetLocationTB_Result> LocationList()
        //{
        //    var locList = uow.GetLocationList().ToList();
        //    List<FGetLocationTB_Result> location = new List<FGetLocationTB_Result>();
        //    foreach (var item in locList)
        //    {
        //        var locationname = uow.Repository<FGetLocationTB_Result>().FindBy(x => x.PLId == item.PLId).SingleOrDefault();
        //        location.Add(locationname);
        //    }


        //    return location;
        //}

        public List<FGetLocationTB_Result> LocationList()
        {
            var locList = uow.GetLocationList().ToList();
            return locList;
        }
        public ICollection<Location> GetAllLocation()
        {
            return uow.Repository<Location>().GetAll().ToList();
        }
        public ICollection<LocationMaster> GetAllLocationMaster()
        {
            return uow.Repository<LocationMaster>().GetAll().ToList();
        }
        public LocationTreeView _GetLocationTree(bool withCheckBox, bool withImageIcon, int selectedNode, bool allowSelectGroupNode, List<int> withOutMe, int rootNode)
        {

            var treeList = uow.Repository<Location>().GetAll().ToList();
            treeList.Add(new Location
            {
                LId = 0,
                LocationName = "Location",
                PLId = -1,
                IsGroup = true,
                Lvl = 0

            });
            var tree = GenerateLocationTree(treeList, -1);
            return tree;
        }
        public int? _GetLocationMoveTree(List<int> selectedNodeId)
        {
            int level = 0;
            //int rootLevel = 0;
            int? currentId = selectedNodeId.First();
            int? nodeId = 0;
            //int maxLevel = 0;
            level = (from m in uow.Repository<Location>().GetAll().Where(m => m.LId == currentId) select m.Lvl).Single();
            while (level > 1)
            {
                nodeId = (from m in uow.Repository<Location>().GetAll().Where(m => m.LId == currentId) select m.PLId).Single();
                currentId = nodeId;
                level = (from m in uow.Repository<Location>().GetAll().Where(m => m.LId == nodeId) select m.Lvl).Single();
            }

            return currentId;
        }
        public string _DeleteLocation(int parentNodeId)
        {
            List<Location> childList = (from m in GetAllLocation().Where(m => m.PLId == parentNodeId) select m).ToList();

            var parentId = (from m in GetAllLocation().Where(m => m.LId == parentNodeId) select m.PLId).SingleOrDefault();
            var locationName = (from m in GetAllLocation().Where(m => m.LId == parentNodeId) select m.LocationName).SingleOrDefault();
            var message = "";
            if (childList.Count() == 0)
            {
                var parentRow = (from m in GetAllLocation().Where(m => m.LId == parentNodeId) select m).SingleOrDefault();
                var parentLocationMasterRow = (from m in GetAllLocationMaster().Where(m => m.LId == parentNodeId) select m).SingleOrDefault();
                if (parentLocationMasterRow == null)
                {
                    uow.Repository<Location>().Delete(parentRow);

                }
                else
                {
                    uow.Repository<Location>().Delete(parentRow);

                    uow.Repository<LocationMaster>().Delete(parentLocationMasterRow);
                }

                uow.Commit();
                message = "Location " + locationName + " deleted successfully!";
            }
            else
            {
                message = "Cannot delete location " + locationName + " because it has child elements.";
            }
            return message;
        }
        public void _DeleteSelectedLevel(byte level, int rootNode)
        {
            uow.ExecWithStoreProcedure("[Loc].[PSetRemoveLocatioByLvl] @CurrentRoot,@LvlToRemove",
                                                       new SqlParameter("@CurrentRoot", rootNode),
                                                       new SqlParameter("@LvlToRemove", level));

        }
        public void _CreateSelectedLevel(int rootNode, byte level, string locationName)
        {
            uow.ExecWithStoreProcedure("[Loc].[PSetRemoveLocatioByLvl] @CurrentRoot,@LvlToCreate,@LocationName",
                                                      new SqlParameter("@CurrentRoot", rootNode),
                                                      new SqlParameter("@LvlToCreate", level),
                                                      new SqlParameter("@LocationName", locationName));

        }
        public List<string> _MoveLocation(List<int> selectedNodeId, int parentNodeId)
        {
            int level = 0;
            int rootLevel = 0;
            int? rootId = parentNodeId;
            int? nodeId = 0;
            int maxLevel = 0;
            var locationName = "";
            List<string> message = new List<string>();

            level = (from m in GetAllLocation().Where(m => m.LId == rootId) select m.Lvl).SingleOrDefault();
            while (level > 1)
            {
                nodeId = (from m in GetAllLocation().Where(m => m.LId == rootId) select m.PLId).SingleOrDefault();
                rootId = nodeId;
                level = (from m in GetAllLocation().Where(m => m.LId == nodeId) select m.Lvl).SingleOrDefault();
            }
            rootLevel = (from m in GetAllLocation().Where(m => m.LId == rootId) select m.Lvl).Single();
            maxLevel = (from m in GetAllLocationMaster().Where(m => m.LId == rootId) select m.MaxLvl).Single();

            foreach (var item in selectedNodeId)
            {
                try
                {
                    uow.ExecWithStoreProcedure("[loc].[PSetRemoveLocationByLvl] @CurrentRoot,@LvlToRemove",
                                                     new SqlParameter("@CurrentRoot", rootId),

                                                     new SqlParameter("@LvlToRemove", parentNodeId));

                    locationName = (from m in GetAllLocation().Where(m => m.LId == item) select m.LocationName).SingleOrDefault();
                    message.Add(locationName + " Moved Successfully!");
                }
                catch (Exception ex)
                {
                    message.Add(ex.InnerException.Message);
                }

            }
            return message;
        }
        public ReturnBaseMessageModel _AddLocation(Location location, LocationMaster locationMaster)
        {

            try
            {
                uow.Repository<Location>().Add(location);

                if (location.PLId == 0)
                {
                    locationMaster.LId = location.LId;
                }
                if (locationMaster != null)
                {
                    uow.Repository<LocationMaster>().Add(locationMaster);
                }
                returnMessageModel.Msg = "Location Added Successfully";
                returnMessageModel.Success = true;
                uow.Commit();
                return returnMessageModel;
            }
            catch (Exception e)
            {
                returnMessageModel.Msg = "Not Saved";
                returnMessageModel.Success = false;
                return returnMessageModel;
            }

        }
        public void _EditLocation(Location location, LocationMaster locationMaster)
        {


            //location.PLId = (from m in GetAllLocation().Where(m => m.LId == location.LId) select m.PLId).Single();
            //location.Lvl = (from m in GetAllLocation().Where(m => m.LId == location.LId) select m.Lvl).Single();
            //location.IsGroup = (from m in GetAllLocation().Where(m => m.LId == location.LId) select m.IsGroup).Single();
            //locationMaster.MaxLvl = (from m in GetAllLocationMaster().Where(m => m.LId == locationMaster.LId) select m.MaxLvl).Single();

            var locationSingleOrDefault = uow.Repository<Location>().FindBy(x => x.LId == location.LId).SingleOrDefault();
            var locationMasterSingleOrDefault = uow.Repository<LocationMaster>().FindBy(x => x.LId == locationMaster.LId).SingleOrDefault();

            locationSingleOrDefault.Lvl = location.Lvl;
            locationSingleOrDefault.PLId = location.PLId;
            locationSingleOrDefault.IsGroup = location.IsGroup;
            locationSingleOrDefault.LocationName = location.LocationName;

            if (location.Lvl == 1)
            {
                locationMasterSingleOrDefault.MaxLvl = locationMaster.MaxLvl;

                uow.Repository<Location>().Edit(locationSingleOrDefault);
                uow.Repository<LocationMaster>().Edit(locationMasterSingleOrDefault);
            }
            else
            {
                uow.Repository<Location>().Edit(locationSingleOrDefault);
            }
            uow.Commit();


        }
        public string GetAddressOfCustomerByLID(int lid)
        {

            string query = " select loc.FGetDetailLocByLID(LId) as Location  from loc.Location where lid =" + lid;
            return uow.Repository<string>().SqlQuery(query).FirstOrDefault();

        }

        public bool CheckExists(string LocationName, int LId = 0)
        {
            int count = uow.Repository<Location>().GetAll().Where(x => x.LocationName.ToLower().Trim() == LocationName.ToLower().Trim()).Where(x => x.LId != LId).Count();

            if (count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
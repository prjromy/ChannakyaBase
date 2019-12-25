using ChannakyaBase.BLL.Repository;
using ChannakyaBase.DAL.DatabaseModel;
using ChannakyaBase.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChannakyaBase.BLL.Service
{
    public class OccupationDefService
    {
        private GenericUnitOfWork uow = null;
        private ReturnBaseMessageModel retunmessage = null;
        public OccupationDefService()
        {
            uow = new GenericUnitOfWork();
            retunmessage = new ReturnBaseMessageModel();
        }
        public List<OccupationDef> GetAll()
        {
            return uow.Repository<OccupationDef>().GetAll().OrderByDescending(x=>x.Occpn).ToList();

        }
        public List<OccupationDef> GetAllByName(string search="")
        {
            return uow.Repository<OccupationDef>().GetAll().OrderByDescending(x => x.Occpn).Where(x=>x.occupation==search).ToList();

        }
        public OccupationDef GetSingle(int? Occpn)
        {
            return uow.Repository<OccupationDef>().GetSingle(x => x.Occpn == Occpn);

        }
        public ReturnBaseMessageModel Save(OccupationDef occupationDef)
        {
            try
            {
                var checkExists = uow.Repository<OccupationDef>().FindBy(x => x.occupation.Equals(occupationDef.occupation)).Count();
                if (checkExists > 0)
                {
                    throw new Exception("Duplicate occupation. Occupation Caption not valid");
                }
                else
                {
            if (occupationDef.Occpn == 0)
            {
                uow.Repository<OccupationDef>().Add(occupationDef);
                        retunmessage.Msg = "Occupation Saved Successfully";
                retunmessage.Success = true;
            }
            else
            {
                uow.Repository<OccupationDef>().Edit(occupationDef);
                        retunmessage.Msg = "Occupation Updated Successfully";
                retunmessage.Success = true;
            }
                }
            uow.Commit();
            return retunmessage;
        }
            catch (Exception ex)
            {
                retunmessage.Success = false;
                retunmessage.Msg = "Not Saved" + ex.Message;
                return retunmessage;
            }
        }

         
        public void Delete(OccupationDef occupationDef)
        {
            uow.Repository<OccupationDef>().Delete(occupationDef);
            uow.Commit();

        }
        public bool CheckOccupation(string occupation = "", int Occpn = 0)
        {
            int count = uow.Repository<OccupationDef>().GetAll().Where(x => x.occupation.ToLower().Trim()== occupation.ToLower().Trim()).Where(x => x.Occpn != Occpn).Count();
            if (count == 0)
            {
                return true;
            }
            else
            {
          
               return false;
            }
        }
        public int CheckOccMapped(int? Occpn)
        {
            int count = uow.Repository<CustIndividual>().FindBy(x => x.Occpn == Occpn).Count();
            return count;

        }
    }
}
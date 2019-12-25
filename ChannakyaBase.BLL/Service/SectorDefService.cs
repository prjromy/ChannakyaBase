
using ChannakyaBase.BLL.Repository;
using ChannakyaBase.DAL.DatabaseModel;
using ChannakyaBase.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChannakyaBase.BLL.Service
{
    public class SectorDefService
    {
        private GenericUnitOfWork uow = null;
        private ReturnBaseMessageModel returnMessage = null;
        public SectorDefService()
        {
            uow = new GenericUnitOfWork();
            returnMessage = new ReturnBaseMessageModel();
        }
        public List<SectorDef> GetAll()
        {

            return uow.Repository<SectorDef>().GetAll().OrderByDescending(x => x.CDepSector).ToList();

        }

        public SectorDef GetSingle(int? CDepSector)
        {
            return uow.Repository<SectorDef>().GetSingle(x => x.CDepSector == CDepSector);

        }
        public ReturnBaseMessageModel Save(SectorDef sectorDef)
        {
            try
            {
                var checkExist = uow.Repository<SectorDef>().FindBy(x => x.CDepSectorNam.Equals(sectorDef.CDepSectorNam) && x.CDepSector!=sectorDef.CDepSector).Count();
                if (checkExist > 0)
                {
                    throw new Exception("Duplicate Department Found. Department Caption Not Valid");
                }
                else
                {
                    if (sectorDef.CDepSector == 0)
                    {
                        uow.Repository<SectorDef>().Add(sectorDef);
                        returnMessage.Msg = "Sector Saved Successfully";
                        returnMessage.Success = true;
                    }
                    else
                    {
                        uow.Repository<SectorDef>().Edit(sectorDef);
                        returnMessage.Msg = "Sector Edited Successfully";
                        returnMessage.Success = true;
                    }
                }

                uow.Commit();

                return returnMessage;
            }
            catch (Exception ex)
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Not Save" + ex.Message;
                return returnMessage;
            }

        }
        public void Delete(SectorDef sectorDef)
        {
            uow.Repository<SectorDef>().Delete(sectorDef);
            uow.Commit();

        }

      
        public bool CheckExists(string cDepSectorName, int CDepSector = 0)
        {
            //int count = uow.Repository<SectorDef>().GetAll().Where(x => x.CDepSectorNam == cDepSectorName).Where(x => x.CDepSector != myId).Count();
            int count = uow.Repository<SectorDef>().GetAll().Where(x => x.CDepSectorNam.ToLower().Trim() == cDepSectorName.ToLower().Trim()).Where(x => x.CDepSector != CDepSector).Count();
            if (count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool CheckSectorMapped(int? CDepSector)
        {
            int count = uow.Repository<CustTypeSector>().FindBy(x => x.CDepSector == CDepSector).Count();
            int countsectorinproduct = uow.Repository<ProductDetail>().FindBy(x => x.NSId == CDepSector).Count();
            if(count>=1 || countsectorinproduct >= 1)
            {
                return false;
            }

            else
            {
                return true;
            }
        }

    }
}
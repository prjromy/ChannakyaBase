using ChannakyaBase.BLL.Repository;
using ChannakyaBase.DAL.DatabaseModel;
using ChannakyaBase.Model.Models;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ChannakyaBase.BLL.Service
{
    public class ContactDefService
    {
        private GenericUnitOfWork uow = null;
        private ReturnBaseMessageModel returnMessage=null;
        public ContactDefService()
        {
            uow = new GenericUnitOfWork();
            returnMessage = new ReturnBaseMessageModel();
        }
        public List<ContactDef> GetAll()
        {
            return uow.Repository<ContactDef>().GetAll().OrderByDescending(x => x.CNotype).ToList();

        }

        public ContactDef GetSingle(int? CNotype)
        {
            return uow.Repository<ContactDef>().GetSingle(x => x.CNotype == CNotype);

        }
        public int FindBy(string name)
        {

            int count = uow.Repository<ContactDef>().GetAll().Where(x => x.CNodesc == name).ToList().Count();
            return count;
        }

        public ReturnBaseMessageModel Save(ContactDef contactDef)
        {
            if (contactDef.CNotype == 0)
            {
                contactDef.CNodesc = new CultureInfo("en-US").TextInfo.ToTitleCase(contactDef.CNodesc.ToString().Trim());
                uow.Repository<ContactDef>().Add(contactDef);
                returnMessage.Msg = "Contact Added Successfully";
                returnMessage.Success = true;
            }
            else
            {
                contactDef.CNodesc = new CultureInfo("en-US").TextInfo.ToTitleCase(contactDef.CNodesc.ToString().Trim());
                uow.Repository<ContactDef>().Edit(contactDef);
                returnMessage.Msg = "Contact Edited Successfully";
                returnMessage.Success = true;
            }
            uow.Commit();
            return returnMessage;
        }
        public void Delete(ContactDef contactDef)
        {
            uow.Repository<ContactDef>().Delete(contactDef);
            uow.Commit();

        }
        public bool CheckContactTypeDes(string CNodesc = "",int CNotype = 0)
        {
             
            int count = uow.Repository<ContactDef>().GetAll().Where(x => x.CNodesc.ToLower().Trim() == CNodesc.ToLower().Trim()).Where(x => x.CNotype != CNotype).Count();
           

            if (count == 0 )
            {
                return true;
            }
            else
            {
                return false;
            }
             
        }
        public bool CheckContMapped(int? CTCntID)
        {
            int custtypeCount = uow.Repository<CustTypeContact>().FindBy(x => x.CNoType == CTCntID).Count();
            int custcontactCount = uow.Repository<CustContact>().FindBy(x => x.CNotype == CTCntID).Count();
            if(custtypeCount >= 1|| custcontactCount >= 1)
            {
                return false;
            }
            return true;

        }
        public bool CheckContactTypeAbb(string CNoabb = "", int CNotype = 0)
       {

            int count = uow.Repository<ContactDef>().FindBy(x => x.CNoabb.ToLower().Trim() == CNoabb.ToLower().Trim() &&  x.CNotype != CNotype).Count();


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
    
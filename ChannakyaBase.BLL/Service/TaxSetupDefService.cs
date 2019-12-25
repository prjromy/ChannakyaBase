using ChannakyaBase.BLL.Repository;
using ChannakyaBase.DAL.DatabaseModel;
using ChannakyaBase.Model.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ChannakyaBase.BLL.Service
{
    public class TaxSetupDefService
    {
        private GenericUnitOfWork uow = null;
        private ReturnBaseMessageModel returnMessage = null;

        public TaxSetupDefService()
        {
            uow = new GenericUnitOfWork();
             returnMessage = new ReturnBaseMessageModel();
        }
        public List<TaxsetupDef> GetAll()
        {
            return uow.Repository<TaxsetupDef>().GetAll().ToList();

        }
        public TaxsetupDef GetSingle(int? TaxID)
        {
            return uow.Repository<TaxsetupDef>().GetSingle(x => x.TaxID == TaxID);
        }

        public ReturnBaseMessageModel Save(TaxsetupDef taxSetupDef)
        {
            try
            {
                var checkExist = uow.Repository<TaxsetupDef>().FindBy(x => x.TaxName.ToLower().Equals(taxSetupDef.TaxName.ToLower()) && x.TaxID != taxSetupDef.TaxID).Count();

                if (checkExist > 0)
                {

                    throw new Exception("Duplicate Tax Found. Tax Caption Not Valid");

                }
                else
                {
                    if (taxSetupDef.TaxID == 0)
                    {
                        //certificateDef.CCCert = new CultureInfo("en-US").TextInfo.ToTitleCase(certificateDef.CCCert.ToString().Trim());
                        uow.Repository<TaxsetupDef>().Add(taxSetupDef);
                        returnMessage.Msg = "Tax Saved Successfully";
                        returnMessage.Success = true;
                    }
                    else
                    {
                        //TaxsetupDef.TaxName = new CultureInfo("en-US").TextInfo.ToTitleCase(taxSetupDef.Taxname.ToString().Trim());

                        uow.Repository<TaxsetupDef>().Edit(taxSetupDef);
                        returnMessage.Msg = "Tax Edited Successfully";
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

        public int FindBy(string name)
        {
            int count = uow.Repository<TaxsetupDef>().GetAll().Where(x => x.TaxName == name).ToList().Count();
            return count;
        }
        //public void Save(TaxsetupDef taxsetupDef)
        //{
        //    if (taxsetupDef.TaxID == 0)
        //    {
        //        taxsetupDef.TaxName = new CultureInfo("en-US").TextInfo.ToTitleCase(taxsetupDef.TaxName.ToString().Trim());
        //        uow.Repository<TaxsetupDef>().Add(taxsetupDef);
        //    }
        //    else
        //    {
        //        taxsetupDef.TaxName = new CultureInfo("en-US").TextInfo.ToTitleCase(taxsetupDef.TaxName.ToString().Trim());
        //        uow.Repository<TaxsetupDef>().Edit(taxsetupDef);
        //    }
        //    uow.Commit();
        //}
        public void Delete(TaxsetupDef taxsetupDef)
        {
            uow.Repository<TaxsetupDef>().Delete(taxsetupDef);
            uow.Commit();

        }
        public bool CheckTaxTypeDef(string TaxName, int TaxID = 0)
        {
            int count = uow.Repository<TaxsetupDef>().GetAll().Where(x => x.TaxName.Trim().ToLower() == TaxName.Trim().ToLower()).Where(x => x.TaxID != TaxID).Count();

            if (count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public int CheckTaxMapped(int? TaxID)
        {
            int count = uow.Repository<CustType>().FindBy(x => x.TaxID == TaxID).Count();
            return count;

        }
    } 
}
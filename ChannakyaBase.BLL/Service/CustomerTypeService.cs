using ChannakyaBase.BLL.Repository;
using ChannakyaBase.DAL.DatabaseModel;
using ChannakyaBase.Model.Models;
using ChannakyaBase.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChannakyaBase.BLL.Service
{
    public class CustomerTypeService
    {
        private GenericUnitOfWork uow = null;
        private ReturnBaseMessageModel returnMessage = null;
        public CustomerTypeService()
        {
            uow = new GenericUnitOfWork();
            returnMessage = new ReturnBaseMessageModel();
        }
        public List<CustType> GetAll()
        {
            var customer = (from cus in uow.Repository<CustType>().GetAll() join tax in uow.Repository<TaxsetupDef>().GetAll() on cus.TaxID equals tax.TaxID select cus).ToList();
            return customer;
        }
        public CustType GetSingle(int? ctypeID)
        {
            //return uow.Repository<CustType>().GetSingle(x => x.CtypeID == ctypeID);
            return uow.Repository<CustType>().FindBy(x => x.CtypeID == ctypeID).SingleOrDefault();
        }
        public CustTypeSector GetSingleSector(int? ctSid)
        {
            return uow.Repository<CustTypeSector>().GetSingle(x => x.CTSID == ctSid);

        }
        public CustTypeCertificate GetSingleCertificate(int? ctcId)
        {
            return uow.Repository<CustTypeCertificate>().GetSingle(x => x.CTCID == ctcId);

        }
        public CustTypeContact GetSingleContact(int? cTCntID)
        {

            return uow.Repository<CustTypeContact>().GetSingle(x => x.CTCntID == cTCntID);

        }
        public CustTypeSector GetSectorByCtypeIDSectorId(int? ctypeID, int sectorId)
        {
            return uow.Repository<CustTypeSector>().GetSingle(x => x.CDepSector == sectorId && x.CTypeID == ctypeID);

        }

        public List<SelectListItem> GetTaxOption()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            var list = uow.Repository<TaxsetupDef>().GetAll().ToList();
            foreach (var item in list)
            {
                lst.Add(new SelectListItem { Text = item.TaxName, Value = item.TaxID.ToString() });
            }
            return lst;
        }
        public ICollection<CertificateDefViewModel> CertifcateTypeList()
        {

            return uow.Repository<CertificateDef>().GetAll().Select(x => new CertificateDefViewModel()
            {
                CCCertID = x.CCCertID,
                CCCert = x.CCCert
            }).ToList();
        }
        public List<CustTypeCertificate> CustTypeCertificate()
        {
            return uow.Repository<CustTypeCertificate>().GetAll().ToList();


        }
        public List<CustTypeCertificate> FindCertificatelistByCTypeId(int id)
        {
            return uow.Repository<CustTypeCertificate>().FindBy(x => x.CTypeID == id).ToList();
        }
        public List<CustTypeContact> FindContListByCTypeId(int id)
        {
            return uow.Repository<CustTypeContact>().FindBy(x => x.CTypeID == id).ToList();
        }
        public List<CustTypeSector> FindSectorListByCTypeId(int id)
        {
            return uow.Repository<CustTypeSector>().FindBy(x => x.CTypeID == id).ToList();
        }
        public ICollection<ContactDefViewModel> ContactTypeList()
        {
            return uow.Repository<ContactDef>().GetAll().Select(x => new ContactDefViewModel()
            {
                CNoabb = x.CNoabb,
                CNodesc = x.CNodesc,
                CNotype = x.CNotype
            }).ToList();
        }
        public ICollection<SectorDefViewModel> SectorTypeList()
        {
            return uow.Repository<SectorDef>().GetAll().Select(x => new SectorDefViewModel()
            {
                CDepSector = x.CDepSector,
                CDepSectorNam = x.CDepSectorNam

            }).ToList();
        }

        public int CheckExisitinCustomerRegistration(int id)
        {
            int count = uow.Repository<CustInfo>().FindBy(x => x.CtypeID == id).Count();
            return count;
        }

        public void EditCertificateType(CustTypeCertificate certificateType)
        {

            {
                uow.Repository<CustTypeCertificate>().Edit(certificateType);
            }

        }
        public void EditContactType(CustTypeContact custTypeContact)
        {

            {
                uow.Repository<CustTypeContact>().Edit(custTypeContact);
            }

        }
        public ReturnBaseMessageModel Save(CustType custType)
        {
            if (custType.CtypeID == 0)
            {
                //certificateDef.CCCert = new CultureInfo("en-US").TextInfo.ToTitleCase(certificateDef.CCCert.ToString().Trim());
                uow.Repository<CustType>().Add(custType);
                
            }
            else
            {

                uow.Repository<CustType>().Edit(custType);
                
            }
            uow.Commit();
            returnMessage.Msg = "Customer Type Saved Successfully";
            returnMessage.Success = true;
            return returnMessage;


        }
        public int DeleteSector(CustTypeSector custtypesector)
        {
            CustInfo custInfo = uow.Repository<CustInfo>().GetSingle(x => x.CDepSector == custtypesector.CDepSector && x.CtypeID==custtypesector.CTypeID);
            
            if (custInfo == null)
            {
                uow.Repository<CustTypeSector>().Delete(custtypesector);
                return 1;
            }
            else
                return 0;
        }
        public int DeleteCertificate(CustTypeCertificate custtypecertificate)
        {
            //CustTypeCertificate custtypecertificates = uow.Repository<CustTypeCertificate>().GetSingle(x => x.CCCertID == custtypecertificate.CCCertID);
            //if (custtypecertificates == null)
            //{
                uow.Repository<CustTypeCertificate>().Delete(custtypecertificate);
                return 1;
            //}
            //else
            //    return 0;



        }
        public int DeleteContactType(CustTypeContact custTypeContact)
        {
           // var custContact = uow.Repository<CustContact>().GetSingle(x => x.CNotype == custTypeContact.CNoType && x.CtypeId== custTypeContact.CTypeID);
            //if (custContact == null)
            //{
                uow.Repository<CustTypeContact>().Delete(custTypeContact);
                return 1;
            //}
            //else
            //{
            //    return 0;
            //}

        }
        public List<SelectListItem> CertificateOptionList()
        {
            List<SelectListItem> options = new List<SelectListItem>();

            options.Add(new SelectListItem { Text = "Optional", Value = "1" });
            options.Add(new SelectListItem { Text = "Compulsory", Value = "2" });
            options.Add(new SelectListItem { Text = "Id", Value = "3" });
            return options;
        }

        public List<SelectListItem> ContactOptionList()
        {
            List<SelectListItem> options = new List<SelectListItem>();

            options.Add(new SelectListItem { Text = "Optional", Value = "1" });
            options.Add(new SelectListItem { Text = "Compulsory", Value = "2" });
            return options;
        }
        public IEnumerable<CertificateDefViewModel> CertifiacteAssigned(int CTypeID)
        {
            var b = uow.Repository<CustTypeCertificate>().GetAll().Where(m => m.CTypeID == CTypeID);
            var q =
              (from c in CertifcateTypeList()
               join p in b on c.CCCertID equals p.CCCertID
               into ps
               from r in ps.DefaultIfEmpty(new CustTypeCertificate { CCState = 0 })

               select new CertificateDefViewModel()
               {
                   CertStatus = r.CCState,
                   CCCertID = c.CCCertID,
                   CCCert = c.CCCert,
                   CTCID = r.CTCID
               }).ToList();
            return q;
        }
        public IEnumerable<ContactDefViewModel> ContactAssigned(int CTypeID)
        {
            var ctc = uow.Repository<CustTypeContact>().GetAll().Where(m => m.CTypeID == CTypeID);
            var ctl =
              (from c in ContactTypeList()
               join p in ctc on c.CNotype equals p.CNoType
               into ps
               from r in ps.DefaultIfEmpty(new CustTypeContact { CNoState = 0 })
               select new ContactDefViewModel()
               {
                   ContStatus = r.CNoState,
                   CNotype = c.CNotype,
                   CNodesc = c.CNodesc,
                   CNoabb = c.CNoabb,
                   CTCntID = r.CTCntID
               }).ToList();
            return ctl;
        }
        public IEnumerable<SectorDefViewModel> SectorAssigned(int CTypeID)
        {
            var sec = uow.Repository<CustTypeSector>().GetAll().Where(m => m.CTypeID == CTypeID);
            var secList =
              (from sector in SectorTypeList()
               join totsec in sec on sector.CDepSector equals totsec.CDepSector
               into leftjoin
               from tot in leftjoin.DefaultIfEmpty(new CustTypeSector { CDepSector = 0 })

               select new SectorDefViewModel()
               {
                   SectorChecked = tot.CDepSector != 0 ? true : false,
                   CDepSector = sector.CDepSector,
                   CDepSectorNam = sector.CDepSectorNam,
                   CTSID = tot.CTSID
               }).ToList();
            return secList.ToList();
        }


        public ReturnBaseMessageModel CustomerTypeCreate(CustSectCertContViewModel ct)
        {
            CustType obj = GetSingle(ct.CtypeID);
            var customerType = uow.Repository<CustTypeContact>().GetSingle(x => x.CTypeID == ct.CtypeID);
        

            if (obj == null)
            {
                obj = new CustType();
            }

            obj.Ctype = ct.Ctype;
            obj.TaxID = ct.TaxID;
            obj.CtypeID = ct.CtypeID;
            obj.isind = Convert.ToByte(ct.isind);
            foreach (var item in ct.CertifiacteDefList)
            {
                CustTypeCertificate certificate = new CustTypeCertificate();
                if (item.CertStatus == null && item.CTCID != 0)
                {
                    certificate = GetSingleCertificate(item.CTCID);
                    if (certificate != null)
                    {
                        DeleteCertificate(certificate);
                    }
                }
                else if (item.CertStatus != null && item.CTCID == 0)
                {
                    certificate.CCState = Convert.ToByte(item.CertStatus);
                    certificate.CCCertID = item.CCCertID;
                    certificate.CTypeID = ct.CtypeID;
                    uow.Repository<CustTypeCertificate>().Add(certificate);
                    //obj.CustTypeCertificates.Add(certificate);
                }
                else if (item.CertStatus != null && item.CTCID != 0)
                {
                    certificate.CCState = Convert.ToByte(item.CertStatus);
                    certificate.CCCertID = item.CCCertID;
                    certificate.CTCID = item.CTCID;
                    certificate.CTypeID = ct.CtypeID;
                    EditCertificateType(certificate);

                }

            }
            foreach (var item in ct.ContactDefList)
            {
               CustTypeContact contact1 = new CustTypeContact();

                if (item.ContStatus != null && item.CTCntID == 0)
                {
                    contact1.CNoState = Convert.ToByte(item.ContStatus);
                    contact1.CNoType = item.CNotype;
                    obj.CustTypeContacts.Add(contact1);
                }
                else if (item.ContStatus != null && item.CTCntID != 0)
                {
                    customerType.CNoState = Convert.ToByte(item.ContStatus);
                    customerType.CNoType = item.CNotype;
                    //customerType.CTCntID = item.CTCntID;
                    customerType.CTypeID = ct.CtypeID;
                    EditContactType(customerType);
                }
                else if (item.ContStatus == null && item.CTCntID != 0)
                {
                    contact1 = GetSingleContact(item.CTCntID);
                    if (contact1 != null)
                    {
                        DeleteContactType(contact1);
                    }
                }
            }
            foreach (var item in ct.SectorDefList)
            {
                
                CustTypeSector custsector = new CustTypeSector();
          //      if (item.SectorChecked == false && item.CTSID != 0)
                    if (item.SectorChecked == false)
                    {
                    custsector = GetSingleSector(item.CTSID);
                    if (custsector != null)
                    {

                        DeleteSector(custsector);
                    }
                }

                else if (item.SectorChecked == true && item.CTSID == 0)
                {
                    {
                        custsector.CDepSector = item.CDepSector;
                        obj.CustTypeSectors.Add(custsector);
                    }
                }
            }
            var message= Save(obj);
            return message;
            
        }




        public int DeleteCustomer(CustType ct)
        {
            CustInfo cInfo = uow.Repository<CustInfo>().GetSingle(x => x.CtypeID == ct.CtypeID);
            if (cInfo == null)
            {
                uow.Repository<CustType>().Delete(ct);
                uow.Commit();
                return 1;
            }
            else return 0;
        }


        public bool CheckExists(string custtype, int myId = 0)
        {
            int count = uow.Repository<CustType>().GetAll().Where(x => x.Ctype.Trim().ToLower() == custtype.Trim().ToLower()).Where(x => x.CtypeID != myId).Count();

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
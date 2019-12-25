using ChannakyaBase.BLL.Repository;
using ChannakyaBase.DAL.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChannakyaBase.BLL.Service
{
    public static class CustomerUtilityService
    {
        private static GenericUnitOfWork uow = null;
        static CustomerUtilityService()
        {
            uow = new GenericUnitOfWork();
        }
        public static SelectList CustTypeList()
        {

            List<CustType> custTypeList = uow.Repository<CustType>().GetAll().ToList();
            return new SelectList(custTypeList, "CtypeID", "Ctype");

        }
        public static SelectList CustTypeCertificateList(byte? CtypeId)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var certificateList = (from custType in uow.Repository<CustTypeCertificate>().GetAll()
                                       join
                                       certtype in uow.Repository<CertificateDef>().GetAll() on custType.CCCertID equals certtype.CCCertID
                                       where custType.CTypeID == CtypeId && Convert.ToInt32(custType.CCState) == 3
                                       select new CertificateDef()
                                       {
                                           CCCertID = certtype.CCCertID,
                                           CCCert = certtype.CCCert
                                       }
                                    ).OrderBy(x=>x.CCCert).ToList();
                return new SelectList(certificateList, "CCCertID", "CCCert");
            }
        }

        public static SelectList CustTypeSectorList(byte? CtypeId)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var custSectorList = (from custType in uow.Repository<CustTypeSector>().GetAll()
                                      join
                                      secttype in uow.Repository<SectorDef>().GetAll().OrderBy(x => x.CDepSectorNam) on custType.CDepSector equals secttype.CDepSector
                                      where custType.CTypeID == CtypeId
                                      select new SectorDef()
                                      {
                                          CDepSector = secttype.CDepSector,
                                          CDepSectorNam = secttype.CDepSectorNam
                                      }
                                    ).ToList();
                return new SelectList(custSectorList, "CDepSector", "CDepSectorNam");
            }
        }
        public static SelectList CustOccupList()
        {
            //List<OccupationDef> occupationList = uow.Repository<OccupationDef>().GetAll().ToList();
            //return new SelectList(occupationList, "Occpn", "occupation");
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var custOccupationList = (from u in uow.Repository<OccupationDef>().GetAll() select u).OrderBy(x=>x.occupation).ToList();
                return new SelectList(custOccupationList, "occpn", "occupation");
                

            }

        }
        //for company
        public static SelectList CompanyGroupList()
        {
            List<CustCompGroup> companyGroupList = uow.Repository<CustCompGroup>().GetAll().OrderBy(x=>x.CCGroupName).ToList();
            return new SelectList(companyGroupList, "CCGroupID", "CCGroupName");

        }
        public static SelectList CustAddressType()
        {
            List<LocationTypeDef> companyGroupList = uow.Repository<LocationTypeDef>().GetAll().ToList();
            return new SelectList(companyGroupList, "ATypeId", "LType");

        }
        public static SelectList GenderList()
        {
            List<SelectListItem> genderOptions = new List<SelectListItem>();

            genderOptions.Add(new SelectListItem { Text = "Male", Value = "1" });
            genderOptions.Add(new SelectListItem { Text = "Female", Value = "2" });
            genderOptions.Add(new SelectListItem { Text = "Other", Value = "3" });
            return new SelectList(genderOptions, "Value", "Text");
        }
        public static SelectList CustContactType(byte? CtypeId)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var custContactList = (from custType in uow.Repository<CustTypeContact>().GetAll()
                                       join
                                       conttype in uow.Repository<ContactDef>().GetAll() on custType.CNoType equals conttype.CNotype
                                       where custType.CTypeID == CtypeId
                                       select new ContactDef()
                                       {
                                           CNotype = conttype.CNotype,
                                           CNodesc = conttype.CNodesc,
                                       }
                                    ).ToList();
                return new SelectList(custContactList, "CNotype", "CNodesc");
            }
        }
        public static SelectList CustCompulSoryContactType(byte? CNotype,byte? CtypeId)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                //var custContactList = (from custType in uow.Repository<CustTypeContact>().FindBy(x => /*x.CNoState == 2 &&*/ x.CTypeID==CtypeId)
                //                       join
                //                       conttype in uow.Repository<ContactDef>().GetAll() on custType.CNoType equals conttype.CNotype
                //                       where custType.CNoType == CNotype
                //                       select new ContactDef()
                //                       {
                //                           CNotype = conttype.CNotype,
                //                           CNodesc = conttype.CNodesc,
                //                       }
                //                    ).ToList();

                var custContactList = (from custType in uow.Repository<CustTypeContact>().GetAll()
                                       join
                                       conttype in uow.Repository<ContactDef>().GetAll() on custType.CNoType equals conttype.CNotype
                                       where custType.CTypeID == CtypeId
                                       select new ContactDef()
                                       {
                                           CNotype = conttype.CNotype,
                                           CNodesc = conttype.CNodesc,
                                       }
                                    ).ToList();
                return new SelectList(custContactList, "CNotype", "CNodesc");
            }
        }

        public static SelectList CustTypeCompCertificateList(byte CtypeId)
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                var certificateList = (from custType in uow.Repository<CustTypeCertificate>().GetAll()
                                       join
                                       certtype in uow.Repository<CertificateDef>().GetAll() on custType.CCCertID equals certtype.CCCertID
                                       where custType.CTypeID == CtypeId && Convert.ToInt32(custType.CCState) == 2
                                       select new CertificateDef()
                                       {
                                           CCCertID = certtype.CCCertID,
                                           CCCert = certtype.CCCert
                                       }
                                    ).ToList();
                return new SelectList(certificateList, "CCCertID", "CCCert");
            }
        }

        public static SelectList CustomerSearchOption()
        {
            List<SelectListItem> objCustomerSrchOption = new List<SelectListItem>();

            objCustomerSrchOption.Add(new SelectListItem { Text = "Name", Value = "Name" });
            objCustomerSrchOption.Add(new SelectListItem { Text = "Mobile Number", Value = "Mobile" });
            objCustomerSrchOption.Add(new SelectListItem { Text = "Contact Person", Value = "ContactPerson" });
            return new SelectList(objCustomerSrchOption, "Value", "Text");
        }

        public static SelectList EmployeeSearchOption()
        {
            List<SelectListItem> objEmployeeSearch = new List<SelectListItem>();

            objEmployeeSearch.Add(new SelectListItem { Text = "Name", Value = "Name" });
            objEmployeeSearch.Add(new SelectListItem { Text = "Code", Value = "Code" });
           
            return new SelectList(objEmployeeSearch, "Value", "Text");
        }
        public static byte CNoStateByCID(byte? CNoType,decimal CID)
        {
            var Ctype = uow.Repository<CustInfo>().GetSingle(x => x.CID == CID).CtypeID;
            var custContact = uow.Repository<CustTypeContact>().GetSingle(x => x.CTypeID == Ctype && x.CNoType == CNoType).CNoState;
            return custContact;
        }


    }
}
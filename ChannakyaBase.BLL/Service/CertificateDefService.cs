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
    public class CertificateDefService
    {
        private GenericUnitOfWork uow = null;
        private ReturnBaseMessageModel returnMessage = null;
        public CertificateDefService()
        {
            uow = new GenericUnitOfWork();
            returnMessage = new ReturnBaseMessageModel();
        }
        public List<CertificateDef> GetAll()
        {
         
            return uow.Repository<CertificateDef>().GetAll().OrderByDescending(x=>x.CCCertID).ToList();

        }

        public CertificateDef GetSingle(int? CCCertID)
        {
            return uow.Repository<CertificateDef>().GetSingle(x => x.CCCertID == CCCertID);

        }


        public ReturnBaseMessageModel Save(CertificateDef certificateDef)
        {
            try
            {
                var checkExist = uow.Repository<CertificateDef>().FindBy(x => x.CCCert.ToLower().Trim().Equals(certificateDef.CCCert.ToLower().Trim()) && x.CCCertID!=certificateDef.CCCertID).Count();

                if (checkExist > 0)
                {

                    returnMessage.Msg = "Certificate Already  Exist";
                    returnMessage.Success = false;

                }
                else
                {
                    if (certificateDef.CCCertID == 0)
                    {
                        //certificateDef.CCCert = new CultureInfo("en-US").TextInfo.ToTitleCase(certificateDef.CCCert.ToString().Trim());
                        uow.Repository<CertificateDef>().Add(certificateDef);
                        returnMessage.Msg = "Certificate Saved Successfully";
                        returnMessage.Success = true;
                    }
                    else
                    {
                        //certificateDef.CCCert = new CultureInfo("en-US").TextInfo.ToTitleCase(certificateDef.CCCert.ToString().Trim());

                        uow.Repository<CertificateDef>().Edit(certificateDef);
                        returnMessage.Msg = "Certificate Edited Successfully";
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

        public bool CheckCertificateAfterSave(string cCCert, int cCCertID)
        {
            int count = uow.Repository<CertificateDef>().GetAll().Where(x => x.CCCert.ToLower().Trim() == cCCert.ToLower().Trim()).Where(x => x.CCCertID != cCCertID).Count();

            if (count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckCertMapped(int? CCCertID)
        {
            int countCustTypeCertificate = uow.Repository<CustTypeCertificate>().FindBy(x => x.CCCertID == CCCertID).Count() ;
            int countAnominee = uow.Repository<ANominee>().FindBy(x => x.CCertID == CCCertID).Count();
            int countCustInfo = uow.Repository<CustInfo>().FindBy(x => x.CCCertID == CCCertID).Count();
            if(countCustTypeCertificate>=1 || countAnominee>=1 || countCustInfo>=1)
            {
                return true;
            }
            return false;

        }


        public void Delete(CertificateDef certificateDef)
        {
            uow.Repository< CertificateDef>().Delete(certificateDef);
            uow.Commit();
          

        }

        //public bool CheckExists(string certificateName, string prename)
        //{
        //    //myId = uow.Repository<CertificateDef>().GetSingle(x => x.CCCert == certificateName).CCCertID;
        //    //int count = uow.Repository<CertificateDef>().GetAll().Where(x => x.CCCert == certificateName).Where(x=>x.CCCertID != myId).Count();
        //    int count = uow.Repository<CertificateDef>().GetAll().Where(x => x.CCCert == certificateName).Count();
        //    if (certificateName == prename|| count == 0)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        public bool CheckExists(string certificateName, int CCCertID = 0)
        {
            int count = uow.Repository<CertificateDef>().GetAll().Where(x => x.CCCert.ToLower().Trim() == certificateName.ToLower().Trim()).Where(x => x.CCCertID != CCCertID).Count();

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




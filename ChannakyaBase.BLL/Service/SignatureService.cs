using ChannakyaBase.BLL.Repository;
using ChannakyaBase.DAL.SignatureModel;
using ChannakyaBase.Model.Models;
using ChannakyaBase.Model.ViewModel;
using Loader.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ChannakyaBase.BLL.Service
{
    public class SignatureService
    {
        ReturnBaseMessageModel returnMessage = null;
        private SignatureEntities _context = null;
        private GenericUnitOfWork uow = null;
        SignatureGenericUnitOfWork suow = null;
        public SignatureService()
        {


            uow = new GenericUnitOfWork();
            returnMessage = new ReturnBaseMessageModel();
            _context = new SignatureEntities();
            suow = new SignatureGenericUnitOfWork();
        }


        public ReturnBaseMessageModel UploadImageInDataBase(byte[] imageBytes, AccountSignatureViewModel accountSignatureViewModel)

        {



            try
            {

                var Signature = suow.Repository<Signature>().GetSingle(x => x.SignatureId == accountSignatureViewModel.SignatureID);
                if (Signature == null)
                {
                    Signature = new Signature();
                }

                Signature.SignatureId = accountSignatureViewModel.SignatureID;
                Signature.IACCNo = accountSignatureViewModel.AccountOwner;


                // signatureViewModel.Signature = ; ;
                Signature.Signature1 = imageBytes;
                Signature.UploadedOn =new  CommonService().GetDate();
                Signature.UploadedBy = Global.UserId;

                int accountid = accountSignatureViewModel.AccountOwner;

                //byte[] imageBytes = ConvertToBytes(file);

                var customerAlreadyExists = suow.Repository<Signature>().FindBy(x => x.IACCNo == accountid).ToList();


                if (customerAlreadyExists.Count() > 0)
                {

                    customerAlreadyExists.ForEach(x => x.Status = false);

                }
                if (accountSignatureViewModel.SignatureID == 0)
                {
                    Signature.Status = true;
                    suow.Repository<Signature>().Add(Signature);

                }

                suow.Commit();
                returnMessage.Success = true;
                returnMessage.Msg = "AccountSignature Saved successfully";
                return returnMessage;
            }
            catch (Exception ex)
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Faild to save!!";
                return returnMessage;
            }




        }

        

        public byte[] ConvertToBytes(HttpPostedFileBase image)

        {

            byte[] imageBytes = null;

            BinaryReader reader = new BinaryReader(image.InputStream);

            imageBytes = reader.ReadBytes((int)image.ContentLength);

            return imageBytes;

        }

        public ReturnBaseMessageModel UploadCustomerPhotoInDatabase(byte[] imageBytes, CustomerPhotoViewModel customerPhotoViewModel)
        {

            try
            {


                var photo = suow.Repository<Photo>().GetSingle(x => x.PID == customerPhotoViewModel.PID);
                if (photo == null)
                {
                    photo = new Photo();
                }

                photo.PID = customerPhotoViewModel.PID;
                photo.Cid = customerPhotoViewModel.CustomerName;

                photo.Image = imageBytes;
                photo.UploadedOn =new  CommonService().GetDate();
                photo.UploadedBy = Global.UserId;
                int custId = customerPhotoViewModel.CustomerName;

                //byte[] imageBytes = ConvertToBytes(file);

                var customerAlreadyExists = suow.Repository<Photo>().FindBy(x => x.Cid == custId /*&& x.Image == imageBytes*/).ToList();


                if (customerAlreadyExists.Count() > 0)
                {

                    customerAlreadyExists.ForEach(x => x.Status = false);

                }


                if (customerPhotoViewModel.PID == 0)
                {
                    //var photoAlreadyExists = suow.Repository<Photo>().FindBy(x => x.Image == imageBytes).ToList();
                    //if (photoAlreadyExists.Count() > 0)
                    //{
                    //    returnMessage.Msg = "Photo Already Exists";
                    //    return returnMessage;
                    //}
                    //else
                    //{
                        photo.Status = true;

                        suow.Repository<Photo>().Add(photo);
                    //}
                }

                suow.Commit();
                returnMessage.Success = true;
                returnMessage.Msg = "Photo Saved successfully";
                return returnMessage;
            }
            catch (Exception ex)
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Faild to save!!";
                return returnMessage;
            }

        }

       
        public ReturnBaseMessageModel UploadSignatureShare(byte[] imageBytes, ShareSignatureViewModel signatureViewModel)
        {
            try
            {

                var Share = suow.Repository<Share>().GetSingle(x => x.ShareID == signatureViewModel.ShareID);
                if (Share == null)
                {
                    Share = new Share();
                }

                Share.ShareID = signatureViewModel.ShareID;
                Share.RegID = signatureViewModel.Regno;


                // signatureViewModel.Signature = ; ;
                Share.Signature = imageBytes;
                Share.UploadedOn = new CommonService().GetDate() ;
                Share.UploadedBy = Global.UserId;
                int custId = signatureViewModel.Regno;

                var customerAlreadyExists = suow.Repository<Share>().FindBy(x => x.RegID == signatureViewModel.Regno).ToList();


                if (customerAlreadyExists.Count() > 0)
                {

                    customerAlreadyExists.ForEach(x => x.Status = false);

                }
                

                if (Share.ShareID == 0)
                {
                    Share.Status = true;
                    suow.Repository<Share>().Add(Share);

                }

                suow.Commit();
                returnMessage.Success = true;
                returnMessage.Msg = "Share Signature Saved successfully";
                return returnMessage;
            }
            catch (Exception ex)
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Faild to save!!";
                return returnMessage;
            }
        }

        public byte[] GetMemberPhoto(int id)
        {
            try
            {
                var Photo = suow.Repository<Photo>().FindBy(x => x.Cid == id).LastOrDefault();

                byte[] cover = Photo.Image;
                return cover;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public byte[] GetShareSignatureFromDataBase(int id)
        {
            try
            {
                var Photo = suow.Repository<Share>().FindBy(x => x.RegID == id).LastOrDefault();

                byte[] cover = Photo.Signature;
                return cover;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public byte[] GetAccountSignatureFromDataBase(int id)
        {
            
           try
            {
                var signatureAll = suow.Repository<Signature>().FindBy(x => x.IACCNo == id).LastOrDefault();

                byte[] cover = signatureAll.Signature1;
                return cover;

            }
            catch (Exception ex)
            {
                return null;
            }
        }






        public List<AccountSignatureViewModel> GetAccountSignatureHistory(int id)
        {
         
            try
            {
                
                var signature = suow.Repository<Signature>().FindBy(x => x.IACCNo == id && x.Status == false).Select(x => new AccountSignatureViewModel()
                {
                    signature1 = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(x.Signature1))

                }).ToList();

                return signature;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<ShareSignatureViewModel> GetShareSignatureHistory(int id)
        {

            try
            {
                //var accountall = suow.repository<signature>().findby(x => x.iaccno == accountdetail.iaccno).firstordefault();
                //var accountAll = suow.Repository<Signature>().FindBy(x=>x.IACCNo==accountDetail.IAccno).ToList();
                var signature = suow.Repository<Share>().FindBy(x => x.RegID == id && x.Status == false).Select(x => new ShareSignatureViewModel()
                {
                    Signature = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(x.Signature))
                }).ToList();

                return signature;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<CustomerPhotoViewModel> GetCustomerPhotoHistory(int id)
        {

            try
            {
                
                var photo = suow.Repository<Photo>().FindBy(x => x.Cid == id && x.Status == false).Select(x => new CustomerPhotoViewModel()
                {
                    Image = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(x.Image))

                }).ToList();

                return photo;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}

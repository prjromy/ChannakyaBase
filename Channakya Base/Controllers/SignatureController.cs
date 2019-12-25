using ChannakyaBase.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using ChannakyaBase.Model.ViewModel;

using System.Web.Mvc;
using ChannakyaBase.BLL.Service;
using System.Web;
using System.IO;
using System.Drawing;
using System.Web.Script.Serialization;
using Loader;

namespace ChannakyaBase.Web.Controllers
{
    [MyAuthorize]
    public class SignatureController : Controller
    {
        ReturnBaseMessageModel returnMessage = null;
        SignatureService signatureService = null;

        public SignatureController()
        {
            returnMessage = new ReturnBaseMessageModel();
            signatureService = new SignatureService();
        }

        public ActionResult SignatureIndex()
        {
            return View();
        }

        public ActionResult ShareSignatureViewModel()
        {
            ShareSignatureViewModel signatureViewModel = new ShareSignatureViewModel();
            return PartialView(signatureViewModel);
        }

        //[Route("SignatureIndex")]
        [HttpPost]
        public ActionResult ShareSignatureViewModel(ShareSignatureViewModel signatureViewModel)
        {
            if (ModelState.IsValid)
            {

                string x = signatureViewModel.Signature.Split(',')[1];
                byte[] bytes = System.Convert.FromBase64String(x);
                var getVal = signatureService.UploadSignatureShare(bytes, signatureViewModel);
                return Json(getVal, JsonRequestBehavior.AllowGet);

            }
            else
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Please fill out form please!!";
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }

        }




        public ActionResult CustomerPhoto()
        {
            CustomerPhotoViewModel customerPhotoViewModel = new CustomerPhotoViewModel();
            return PartialView(customerPhotoViewModel);
        }

        //[Route("CustomerPhoto")]

        [HttpPost]
        public ActionResult CustomerPhoto(CustomerPhotoViewModel customerPhotoViewModel)
        {

            if (ModelState.IsValid)
            {


                string x = customerPhotoViewModel.Image.Split(',')[1];
                byte[] bytes = System.Convert.FromBase64String(x);
                var getVal = signatureService.UploadCustomerPhotoInDatabase(bytes, customerPhotoViewModel);
                return Json(getVal, JsonRequestBehavior.AllowGet);
            }



            else
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Please fill out form please!!";
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }

        }


        public ActionResult AccountSignature()
        {
            AccountSignatureViewModel signatureViewModel = new AccountSignatureViewModel();
            return PartialView(signatureViewModel);
        }

        [Route("AccountSignature")]
        [HttpPost]
        public ActionResult AccountSignature(AccountSignatureViewModel accountSignatureViewModel)
        {
            if (ModelState.IsValid)
            {
                string x = accountSignatureViewModel.Signature.Split(',')[1];
                byte[] bytes = System.Convert.FromBase64String(x);
                var getVal = signatureService.UploadImageInDataBase(bytes, accountSignatureViewModel);
                return Json(getVal, JsonRequestBehavior.AllowGet);

            }
            else
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Please fill out form please!!";
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }

        }



        //public ActionResult DisplayAccountSignature()
        //{
        //    AccountSignatureViewModel signatureViewModel = new AccountSignatureViewModel();
        //    return PartialView(signatureViewModel);
        //}

        //public JsonResult RetrieveImage(string accountNumber)

        //{

        //    byte[] cover = signatureService.GetImageFromDataBase(accountNumber);

        //    try
        //    {
        //        string stringImage = Convert.ToBase64String(cover);
        //        string imgDataURL = string.Format("data:image/png;base64,{0}", stringImage);

        //        return Json(imgDataURL, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception)
        //    {
        //        returnMessage.Success = false;
        //        returnMessage.Msg = "account number is incorrect";
        //        return Json(returnMessage, JsonRequestBehavior.AllowGet);


        //    }
        //}

        public ActionResult RetrieveHistoryImage(int id, int flag)
        {
            if (flag == 1)
            {
                try
                {
                    var imageList = signatureService.GetAccountSignatureHistory(id);
                    if (imageList != null)
                    {


                        ViewData["multipleimage"] = imageList;
                        ViewBag.flag = flag;
                    }
                    else
                    {
                        ViewData["multipleimage"] = "";
                        ViewBag.flag = flag;
                    }
                    return PartialView("RetrieveHistoryImage");
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            else if (flag == 3)
            {
                try
                {
                    var imageList = signatureService.GetShareSignatureHistory(id);
                    if (imageList != null)
                    {

                        ViewData["multipleimage"] = imageList;
                        ViewBag.flag = flag;
                    }
                    else
                    {
                        ViewData["multipleimage"] = "";
                        ViewBag.flag = flag;
                    }
                    return PartialView("RetrieveShareSignature");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else if (flag == 2)
            {
                try
                {
                    var imageList = signatureService.GetCustomerPhotoHistory(id);
                    if (imageList != null)
                    {
                        ViewData["multipleimage"] = imageList;
                        ViewBag.flag = flag;
                    }
                    else
                    {
                        ViewData["multipleimage"] = "";
                        ViewBag.flag = flag;
                    }
                    return PartialView("RetrieveCustomerPhoto");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                return View();
            }
        }

        public ActionResult DisplayShareSignaturePartial()
        {
            ShareSignatureViewModel signatureViewModel = new ShareSignatureViewModel();
            return PartialView(signatureViewModel);
        }

        public ActionResult DisplayCustomerPhoto()
        {
            CustomerPhotoViewModel customerPhotoViewModel = new CustomerPhotoViewModel();
            return PartialView(customerPhotoViewModel);
        }

        public ActionResult DisplayAccountSignaturePartial()
        {
            AccountSignatureViewModel signatureViewModel = new AccountSignatureViewModel();
            return PartialView(signatureViewModel);
        }

        public ActionResult DisplayAll()
        {

            return View();
        }

        public ActionResult GetAll(int id, int flag)
        {
            //flag 1=Signature Deposit/loan 2=Member Photo 3= Share Signature


            if (flag == 1)
            {
                byte[] cover = signatureService.GetAccountSignatureFromDataBase(id);
                try
                {
                    if (cover != null)
                    {
                        string stringImage = Convert.ToBase64String(cover);
                        ViewData["ImgPath"] = string.Format("data:image/png;base64,{0}", stringImage);
                    }
                    else
                    {
                        ViewData["ImgPath"] = "";
                    }
                    return PartialView("PartialImage");

                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            else if (flag == 3)
            {
                byte[] cover = signatureService.GetShareSignatureFromDataBase(id);
                try
                {
                    if (cover != null)
                    {
                        string stringImage = Convert.ToBase64String(cover);
                        ViewData["ImgPath"] = string.Format("data:image/png;base64,{0}", stringImage);

                    }
                    else
                    {
                        ViewData["ImgPath"] = "";
                    }
                    return PartialView("PartialImage");

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else if (flag == 2)
            {

                byte[] cover = signatureService.GetMemberPhoto(id);
                try
                {
                    if (cover != null)
                    {
                        string stringImage = Convert.ToBase64String(cover);
                        ViewData["ImgPath"] = string.Format("data:image/png;base64,{0}", stringImage);
                    }
                    else
                    {
                        ViewData["ImgPath"] = "";
                    }

                    return PartialView("PartialImage");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                return View();
            }


        }
        public ActionResult ImageModal(int id, int flag)
        {
            //flag 1=Signature Deposit/loan 2=Member Photo 3= Share Signature


            if (flag == 1)
            {
                byte[] cover = signatureService.GetAccountSignatureFromDataBase(id);
                try
                {
                    if (cover != null)
                    {
                        string stringImage = Convert.ToBase64String(cover);
                        ViewData["ImgPath"] = string.Format("data:image/png;base64,{0}", stringImage);
                    }
                    else
                    {
                        ViewData["ImgPath"] = "";
                    }
                    return PartialView("SignatureModal");

                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            else if (flag == 3)
            {
                byte[] cover = signatureService.GetShareSignatureFromDataBase(id);
                try
                {
                    if (cover != null)
                    {
                        string stringImage = Convert.ToBase64String(cover);
                        ViewData["ImgPath"] = string.Format("data:image/png;base64,{0}", stringImage);

                    }
                    else
                    {
                        ViewData["ImgPath"] = "";
                    }
                    return PartialView("SignatureModal");

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else if (flag == 2)
            {

                byte[] cover = signatureService.GetMemberPhoto(id);
                try
                {
                    if (cover != null)
                    {
                        string stringImage = Convert.ToBase64String(cover);
                        ViewData["ImgPath"] = string.Format("data:image/png;base64,{0}", stringImage);
                    }
                    else
                    {
                        ViewData["ImgPath"] = "";
                    }

                    return PartialView("SignatureModal");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                return View();
            }


        }


    }


}


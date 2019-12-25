using ChannakyaBase.BLL.Service;
using ChannakyaBase.DAL.DatabaseModel;
using ChannakyaBase.Model.Models;
using ChannakyaBase.Model.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Loader;
using System.Data.Entity.Validation;

namespace ChannakyaBase.Web.Controllers
{
    [MyAuthorize]
    [System.Runtime.InteropServices.Guid("4ABAF00D-2162-4A92-ACA5-F862E458625E")]
    public class FinanceParameterController : Controller
    {
        private FinanceParameterService financeParameterService = null;

        ReturnBaseMessageModel returnMessage = null;
        public FinanceParameterController()
        {
            financeParameterService = new FinanceParameterService();
            returnMessage = new ReturnBaseMessageModel();
        }
        // GET: FinanceParameter
        public ActionResult BranchIndex()
        {
            return View();
        }
        public ActionResult _BranchList()
        {

            return View(financeParameterService.GetAllBranch());
        }
        [HttpGet]
        public ActionResult _BranchCreate(int BrnhID = 0)
        {
            ViewBag.ID = BrnhID;
            if (BrnhID == 0)
            {
                LicenseBranch licenseBranch = new LicenseBranch();
                return PartialView(licenseBranch);
            }
            else
            {
                return PartialView(financeParameterService.GetSingleBranchById(BrnhID));
            }

        }
        public JsonResult IsBranchNameExist(string BrnhNam, int? BrnhID)
        {
            var validateName = financeParameterService.CheckBranchNameExists(BrnhNam, BrnhID);
            if (validateName==false)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult _BranchCreate(LicenseBranch licenseBranchModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var message = financeParameterService.SaveBranch(licenseBranchModel);

                    return Json(message, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            else
            {
                returnMessage.Msg = "License Branch Not Saved";
                returnMessage.Success = false;
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult _SetupList()
        {
            ChequeInventorySetupModel chequeAllInventoryList = new ChequeInventorySetupModel();
            var chequeInventoryList = financeParameterService.GetAllChequeInventory();
            chequeAllInventoryList.ChequeInventorySetupList = chequeInventoryList;
            return PartialView(chequeAllInventoryList);
        }

        [HttpGet]
        public ActionResult BranchDelete(int BrnhID)
        {
            bool result = true;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult BranchDeleteConfirm(int BrnhID)
        {
            LicenseBranch productTID = financeParameterService.GetSingleBranchById(BrnhID);
            financeParameterService.BranchDelete(productTID);
            return RedirectToAction("_BranchList");

        }

        //Product floating mapping part
        public ActionResult ProductFloatingInterestIndex()
        {
            return View();
        }
        //public ActionResult _ProductFloatingInterestList()
        //{

        //    return PartialView(financeParameterService.GetAllProductFloatingInterest().ToList());
        //}

        public ActionResult _ProductFloatingInterestList(int PID = 0)
        {

            return PartialView(financeParameterService.GetAllProductFloatingInterestSelect(PID).ToList());
        }
        [HttpGet]
        public ActionResult _ProductFloatingInterestCreate(int PFIID = 0)
        {
            if (PFIID == 0)
            {
                ProductTID productFloatingInterest = new ProductTID();
                return PartialView(productFloatingInterest);
            }
            else

                return PartialView(financeParameterService.GetSingleProductFloatingInterestById(PFIID));
        }
        [HttpPost]
        //public ActionResult _ProductFloatingInterestCreate(ProductTID productFloatingInterest)
        //{

        //   var message= financeParameterService.SaveProductFloatingInterest(productFloatingInterest);
        //    return Json(message,JsonRequestBehavior.AllowGet);
        //}
        public ActionResult _ProductFloatingInterestCreate(ProductTID productFloatingInterest)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var result = financeParameterService.SaveProductFloatingInterest(productFloatingInterest);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                //return RedirectToAction("Index");
                else
                {
                    returnMessage.Success = false;
                    returnMessage.Msg = "Please fill out form please!!";
                    return Json(returnMessage, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {
                return JavaScript(ex.Message);
            }

            //var message= financeParameterService.SaveProductFloatingInterest(productFloatingInterest);
            // return Json(message,JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckProductInterest(int PID, int TID, int PFIID = 0)
        {

            bool ifExists = financeParameterService.CheckExists(PID, TID, PFIID);
            return Json(ifExists, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ProductFloatingInterestDelete(int PFIID)
        {
            bool result = true;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ProductFloatingInterestDeleteConfirm(int PFIID)
        {
            ProductTID productTID = financeParameterService.GetSingleProductFloatingInterestById(PFIID);
            financeParameterService.ProductFloatingInterestDelete(productTID);
            return RedirectToAction("_ProductFloatingInterestList");

        }

        //charge setup
        public ActionResult ChargeSetupIndex()
        {
            return View();
        }
        public ActionResult _ChargeSetupList()
        {

            //var chargelist = financeParameterService.GetAllChargeSetup(ChgID).ToList();
            //return PartialView(chargelist);
            return PartialView(financeParameterService.GetAllChargeSetup());
        }
        [HttpGet]
        public ActionResult _ChargeSetupCreate(int ChgID = 0)
        {
            if (ChgID == 0)
            {
                ChargeDetail chargeDetail = new ChargeDetail();
                return PartialView(chargeDetail);

            }
            else
            {
                return PartialView(financeParameterService.GetSingleChargeSetupById(ChgID));
            }

        }

        [HttpPost]
        public ActionResult _ChargeSetupCreate(ChargeDetail chargeDetail, List<ProductViewModel> ProductViewModelList)
        {
            //    try
            //    {
            //        if (ModelState.IsValid == true)
            //        {
            //            financeParameterService.SaveChargeSetup(chargeDetail, ProductViewModelList);
            //            return RedirectToAction("ChargeSetupIndex");
            //        }
            //        else return View();

            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            try

            {
                if (ModelState.IsValid)
                {
                    var message = financeParameterService.SaveChargeSetup(chargeDetail, ProductViewModelList);
                    return Json(message, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    returnMessage.Success = false;
                    returnMessage.Msg = "Please fill the form !!!";
                    return Json(returnMessage, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return JavaScript(ex.Message);
            }

        }

        public JsonResult ChargeNameExist(string ChgName, int? ChgID)
        {
            var ifExists = financeParameterService.ChargeNameExist(ChgName, ChgID);

            return Json(ifExists, JsonRequestBehavior.AllowGet);
            //if (validateName)
            //{
            //    return Json(false, JsonRequestBehavior.AllowGet);
            //}
            //else
            //{
            //    return Json(true, JsonRequestBehavior.AllowGet);
            //}
        }

        public ActionResult GetCascadedEvents(int? id)
        {
            SelectList eventlist = FinanceParameterUtilityService.ModulesWiseEvents(Convert.ToByte(id));
            return Json(eventlist, JsonRequestBehavior.AllowGet);
        }
        public ActionResult _ProductListByModules(int? id)
        {
            SelectList productList = FinanceParameterUtilityService.ModulesWiseProducts(Convert.ToByte(id));
            List<ProductViewModel> productsList = new List<ProductViewModel>();
            foreach (var item in productList)
            {
                ProductViewModel products = new ProductViewModel();
                products.ProductName = item.Text;
                products.ProductId = Convert.ToInt16(item.Value);
                productsList.Add(products);
            }
            return PartialView(productsList);
            //return Json(productList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult _ProductListByModulesEdit(int ChgID)
        {
            var checkedProducts = financeParameterService.GetSingleChargeSetupById(ChgID);
            string cp = checkedProducts.Product;
            List<string> CP = cp.Split(',').ToList<string>();
            var id = checkedProducts.Module.Modid;
            SelectList productList = FinanceParameterUtilityService.ModulesWiseProducts(Convert.ToByte(id));
            List<ProductViewModel> productsList = new List<ProductViewModel>();

            foreach (var item in productList)
            {
                ProductViewModel products = new ProductViewModel();
                products.ProductName = item.Text;
                products.ProductId = Convert.ToInt16(item.Value);
                foreach (var itemCP in CP)
                {
                    if (products.ProductId == Convert.ToInt32(itemCP))
                    {
                        products.HasDuration = true;
                    }
                }
                productsList.Add(products);
            }
            return PartialView(productsList);
        }

        [HttpGet]
        public ActionResult _ChargeSetupDelete(int ChgID)
        {
            var result = financeParameterService.CheckMap(ChgID);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult _ChargeSetupDeleteConfirm(int ChgID)
        {
            ChargeDetail chargeDetail = financeParameterService.GetSingleChargeSetupById(ChgID);
            financeParameterService.ChargeSetupDelete(chargeDetail);
            return RedirectToAction("_ChargeSetupList");

        }
        [HttpGet]
        public ActionResult _ChargeAutoTriggered(int productId = 0, int modevent = 0,decimal AmountCharge = 0,bool checkSanction=false,decimal RegularLoanCharge=0,int accountNumber=0)
        {
            var chargeDetailsforTranxAutoTrig = financeParameterService.GetChargeDetailsAndAmountChargeByProductId(productId, modevent,AmountCharge,checkSanction,RegularLoanCharge, accountNumber);

            ViewBag.AccountNumber = accountNumber;
            return PartialView(chargeDetailsforTranxAutoTrig);
        }



        [HttpGet]
        public ActionResult _ChargeAutoTriggeredForAccountOpen(int productId = 0, int modevent = 0)
        {
            var chargeDetailsforTranxAutoTrig = financeParameterService.GetChargeDetailsByProductIdForAccountOpen(productId, modevent);

           // ViewBag.AccountNumber = accountNumber;
           return PartialView(chargeDetailsforTranxAutoTrig);
        }



        [HttpGet]
        public ActionResult _UnverifiedChargeDetails(Int64 utno = 0)
        {
            var singleUnverifiedCharge = financeParameterService.GetSingleUnverifiedChargeTransaction(utno);

            return PartialView(singleUnverifiedCharge);
        }

        public ActionResult RemitCustomerIndex()
        {
            return View();
        }
        public ActionResult _RemitCustomerList(int CID = 0)
        {

            return PartialView(financeParameterService.GetAllRemittanceCustomer(CID).ToList());
        }
        [HttpGet]
        public ActionResult _RemitCustomerCreate(int RID = 0)
        {
            if (RID == 0)
            {
                RemittanceCustomer remittanceCustomer = new RemittanceCustomer();
                return PartialView(remittanceCustomer);

            }
            else
            {
                return PartialView(financeParameterService.GetSingleRemittanceCustomerById(RID));
            }

        }
        [HttpPost]
        public ActionResult _RemitCustomerCreate(RemittanceCustomer remittanceCustomer)
        {
            //var result = financeParameterService.SaveRemittanceCustomer(remittanceCustomer);
            //    return Json(result, JsonRequestBehavior.AllowGet);
            //try
            //{
            //    if (ModelState.IsValid == true)
            //    {
            //        financeParameterService.SaveRemittanceCustomer(remittanceCustomer);
            //        return RedirectToAction("RemitCustomerIndex");
            //    }
            //    else return View();

            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            try
            {
                if (ModelState.IsValid)
                {
                    var result = financeParameterService.SaveRemittanceCustomer(remittanceCustomer);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                else
                {
                    returnMessage.Success = false;
                    returnMessage.Msg = "Please fill out form please!!";
                    return Json(returnMessage, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {
                return JavaScript(ex.Message);
            }
        }

        public JsonResult CheckRemittanceMap(int CID, int FID)
        {

            bool ifExists = financeParameterService.CheckMap(CID, FID);
            return Json(ifExists, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public ActionResult _RemitCustomerDelete(int RID)
        {
            //if (ts.CheckTempIntRate(TID) >= 1)
            //{
            //    return JavaScript("OnFailure('type is already used.')");

            //}
            //else {

            // bool result = true;
            //RemittanceCustomer remittanceCustomer = financeParameterService.GetSingleRemittanceCustomerById(RID);
            var result = financeParameterService.RemittanceCustomerDelete(RID);
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult _RemitCustomerDeleteConfirm(int RID)
        {
            RemittanceCustomer remittanceCustomer = financeParameterService.GetSingleRemittanceCustomerById(RID);
            financeParameterService.RemittanceCustomerDeleteConfirm(remittanceCustomer);

            return RedirectToAction("_RemitCustomerList");
        }

        #region EmployeeBranchMapping
        public ActionResult BranchEmployeeMap(int mapId = 0)
        {
            EmployeeBranchMapModel bEModel = new EmployeeBranchMapModel();
            if (mapId != 0)
            {
                bEModel = financeParameterService.GetSingleBranchEmployee(mapId);
            }

            return PartialView(bEModel);
        }

        public ActionResult _AddBranchEmployeeMore()
        {
            EmployeeBranchMapModel bEModel = new EmployeeBranchMapModel();
            return PartialView(bEModel);
        }

        public ActionResult BranchEmployeeList()
        {
            ChannakyaBase.Model.ViewModel.EmployeeViewModel empModel = new ChannakyaBase.Model.ViewModel.EmployeeViewModel();
            var employeeList = financeParameterService.BranchEmployeeList("", 0, 1, 10);
            empModel.EmployeeList = new StaticPagedList<ChannakyaBase.Model.ViewModel.EmployeeViewModel>(employeeList, 1, 10, (employeeList.Count == 0) ? 0 : employeeList.Select(x => x.TotalCount).FirstOrDefault());
            return PartialView(empModel);
        }
        public ActionResult _BranchEmployeeList(string employeeName = "", int branchId = 0, int pageNo = 1, int pageSize = 10)
        {
            ChannakyaBase.Model.ViewModel.EmployeeViewModel empModel = new ChannakyaBase.Model.ViewModel.EmployeeViewModel();
            var employeeList = financeParameterService.BranchEmployeeList(employeeName, branchId, pageNo, pageSize);
            empModel.EmployeeList = new StaticPagedList<ChannakyaBase.Model.ViewModel.EmployeeViewModel>(employeeList, pageNo, pageSize, (employeeList.Count == 0) ? 0 : employeeList.Select(x => x.TotalCount).FirstOrDefault());
            return PartialView(empModel);
        }
        [HttpPost]
        public ActionResult BranchEmployeeMap(EmployeeBranchMapModel beModel)
        {
            try
            {

                returnMessage = financeParameterService.InsertUpdateBranchEmployeeMap(beModel);
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                returnMessage.Success = false;
                returnMessage.Msg = "error";
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion


        #region Teller cash Limit

        public ActionResult CashLimitIndex()
        {

            return PartialView();
        }
        public ActionResult TellerCashLimitList(int employeeId = 0)
        {
            CashLimitModel cashModel = new CashLimitModel();
            var tellerCashlimit = financeParameterService.GetUserTellerCashLimitList(employeeId);
            cashModel.CashLimitList = tellerCashlimit;
            return PartialView(cashModel);
        }

        public ActionResult GetSingleCashLimit(int UserId = 0)

        {
            CashLimitModel cashModel = new CashLimitModel();
            cashModel.CashLimitList = financeParameterService.getSingleCashLimitData(UserId);
            return PartialView("_TellerCashLimitList", cashModel);
        }
        public ActionResult InsertUpdateCashLimit(int cashLimitId = 0)
        {
            CashLimitModel cashLModel = new CashLimitModel();
            if (cashLimitId != 0)
            {
                cashLModel = financeParameterService.GetSingleCashLimit(cashLimitId);
            }
            return View(cashLModel);
        }
        [HttpPost]
        public ActionResult InsertUpdateCashLimit(CashLimitModel cashLimitModel)
        {

            if (ModelState.IsValid)
            {
                returnMessage = financeParameterService.InsertUpdateCashLimit(cashLimitModel);
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
            else
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Please fill all required field!!";
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetUserRoleByEmployeeId(int employeeId)
        {
            var userNameByEmployeeId = financeParameterService.GetUserRoleByEmployeeId(employeeId);
            return Json(userNameByEmployeeId, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region DenoSetup

        public ActionResult DenoSetupIndex()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult _GetDenolistbyid(int CurrID = 0)
        {
            return PartialView(financeParameterService.GetAllCurrval(CurrID).ToList());
        }


        public ActionResult InsertUpdateDenoSetup(int DenoID = 0)
        {
            DenoInViewModel denosetup = new DenoInViewModel();
            if (DenoID != 0)
            {
                denosetup = financeParameterService.GetSingleDenoSetupById(DenoID);
            }

            return View(denosetup);
        }


        public ActionResult DenoAlreadyExist(short currID, double Deno)
        {

            bool value = true;
            if (financeParameterService.DenoAlreadyExistCheck(currID, Deno) == false)
            {
                value = false;
                return Json(value, JsonRequestBehavior.AllowGet);
            }
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DenoInDenoTrnx(int DenoID)
        {

            bool value = true;
            if (financeParameterService.DenoInDenoTrnxCheck(DenoID) == false)
            {
                value = false;
                return Json(value, JsonRequestBehavior.AllowGet);
            }
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult _DenosetupList(int CurrID = 0)

        {
            DenoInViewModel denoSetup = new DenoInViewModel();
            var denolist = financeParameterService.GetAllDenoSetup(CurrID);
            denoSetup.DenoInList = denolist.ToList();
            return PartialView(denoSetup);
        }


        [HttpPost]
        public ActionResult InsertUpdateDenoSetup(DenoInViewModel denosetup)
        {
            if (ModelState.IsValid)
            {
                var getVal = financeParameterService.SaveDenoSetup(denosetup);
                return Json(getVal, JsonRequestBehavior.AllowGet);
            }
            else
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Please fill out form please!!";
                return Json(returnMessage, JsonRequestBehavior.AllowGet);

            }
        }

        public ActionResult _DenoSetupList(int CurrID)
        {
            DenoInViewModel denoSetup = new DenoInViewModel();
            var denolist = financeParameterService.GetAllDenoSetup(CurrID);
            denoSetup.DenoInList = denolist;
            return PartialView(denoSetup);
        }


        public ActionResult GetDenoListById(int DenoID)
        {
            var demoListById = financeParameterService.GetSingleDenoSetupById(DenoID);
            return Json(demoListById, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region ChequeInventorySetup

        public ActionResult ChequeInventorySetupIndex()
        {
            return PartialView();
        }

        public ActionResult InsertUpdateChequeInventorySetup(int ChckID = 0)
        {
            ChequeInventorySetupModel chequeInventorySetup = new ChequeInventorySetupModel();

            return View(chequeInventorySetup);
        }
        public decimal GetFromChequeByBranch(int BranchID)
        {

            decimal formCheckNumber = Convert.ToDecimal(financeParameterService.GetFromChequeByBranch(BranchID));
            return formCheckNumber;
        }
        [HttpPost]
        public ActionResult InsertUpdateChequeInventorySetup(ChequeInventorySetupModel chequeInventorySetup)
        {

            if (ModelState.IsValid)
            {
                var getVal = financeParameterService.saveChequeInventorySetup(chequeInventorySetup);
                return Json(getVal, JsonRequestBehavior.AllowGet);
            }
            else
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Please fill out form please!!";
                return Json(returnMessage, JsonRequestBehavior.AllowGet);

            }

        }

        public ActionResult _ChequeInventorySetupList()
        {
            ChequeInventorySetupModel chequeAllInventoryList = new ChequeInventorySetupModel();
            var chequeInventoryList = financeParameterService.GetAllChequeInventory();
            chequeAllInventoryList.ChequeInventorySetupList = chequeInventoryList;
            return PartialView(chequeAllInventoryList);
        }

        #endregion
    }
}

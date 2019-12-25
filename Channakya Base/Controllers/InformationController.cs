using ChannakyaBase.BLL;
using ChannakyaBase.BLL.Service;
using ChannakyaBase.Model.Models;
using ChannakyaBase.Model.ViewModel;
using Loader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing.Printing;
using System.Text;
using System.IO;
using Loader.Models;
using System.Net;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic.PowerPacks.Printing;
using System.Text.RegularExpressions;

namespace ChannakyaBase.Web.Controllers
{
    [MyAuthorize]
    public class InformationController : Controller
    {
        // GET: Cheque
        InformationService informationService = null;
        ReturnBaseMessageModel returnMessage = null;
        ReportService reportService = null;
        public InformationController()
        {
            informationService = new InformationService();
            returnMessage = new ReturnBaseMessageModel();
            reportService = new ReportService();
        }
        public ActionResult NewChequeIssue()
        {

            return PartialView();
        }

        public ActionResult NewChequeIssueIndex(int accountNumber = 0, int pageNo = 1, int pageSize = 10)
        {
            ChqRqstModel chqModel = new ChqRqstModel();
            chqModel.ChequeRequestList = informationService.GetAllRegisterUnverifiedChequeList(accountNumber, pageNo, pageSize);
            return PartialView(chqModel);
        }
        public ActionResult PaginationChequeIssueIndex(int accountNumber = 0, int pageNo = 1, int pageSize = 10)
        {
            ChqRqstModel chqModel = new ChqRqstModel();
            chqModel.ChequeRequestList = informationService.GetAllRegisterUnverifiedChequeList(accountNumber, pageNo, pageSize);
            return PartialView(chqModel);
        }

        public ActionResult _NewChequeIssueIndex(int? accountNumber, int pageNo = 1, int pageSize = 10)

        {
            ChqRqstModel chqModel = new ChqRqstModel();
            chqModel.ChequeRequestList = informationService.GetAllRegisterChequeList(accountNumber, pageNo, pageSize);
            return PartialView(chqModel);
        }

        public ActionResult ChequeRegistration()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult ChequeRegistration(ChqRqstModel chqRqstModel, Model.ViewModel.TaskVerificationList TaskVerifierList)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    returnMessage = informationService.InsertChqRqst(chqRqstModel, TaskVerifierList);
                    return Json(returnMessage, JsonRequestBehavior.AllowGet);
                }
                catch (Exception exc)
                {
                    return Json(returnMessage, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                returnMessage.Success = false;
                returnMessage.Msg = "please fill all input field.";
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult ChequeVerification()
        {
            return View();
        }

        public ActionResult ChequeNumberVerification()
        {
            ChqRqstModel chqReqModel = new ChqRqstModel();
            var chequeVerificationList = informationService.ChequeVerificationList(1, 10);
            chqReqModel.ChequeRequestList = chequeVerificationList;
            return PartialView(chqReqModel);
        }
        public ActionResult _ChequeNumberVerification(int pageNo = 1, int pageSize = 10)
        {
            ChqRqstModel chqReqModel = new ChqRqstModel();
            var chequeVerificationList = informationService.ChequeVerificationList(pageNo, pageSize);
            chqReqModel.ChequeRequestList = chequeVerificationList;
            return PartialView(chqReqModel);
        }
        public ActionResult VerifiyRegisterCheque(int rNo = 0)
        {
            var unverifiedCheque = informationService.UnVerifiedChequeRegistration(rNo);
            return PartialView(unverifiedCheque);
        }

        public ActionResult ChequeBlockUnBlockDeactivateIndex()
        {
            AChqModel aChqModel = new AChqModel();
            aChqModel.AchqPageList = informationService.GetAChqList("", 1, 2);
            return View(aChqModel);
        }
        public ActionResult _ChequeBlockUnBlockDeactivateIndex(string accNo = "", int pageNo = 1, int pageSize = 2)
        {
            AChqModel aChqModel = new AChqModel();
            aChqModel.AchqPageList = informationService.GetAChqList(accNo, pageNo, pageSize);
            return PartialView(aChqModel);
        }

        public ActionResult ChequeChangeStatus()
        {
            return PartialView();
        }

        public ActionResult StatusChequeList(int iAccNo)
        {
            AChqModel aChqModel = new AChqModel();
            aChqModel.AchqList = informationService.GetChqListByIAccno(iAccNo);
            return PartialView(aChqModel);
        }
        public ActionResult _ChequeNumberDetails(int rno = 0, bool isSelect = false, string showWith = "", int applyStatus = 0)
        {
            AChqModel aChqModel = new AChqModel();
            aChqModel.AchqHDetailsList = informationService.GetAchqHList(rno, applyStatus);
            aChqModel.IsSelectet = isSelect;
            aChqModel.ShowWith = showWith;
            return PartialView(aChqModel);
        }
        public ActionResult _ChequeNumberDetailsList(int rno, int[] selectChqList, string showWith = "", int applyStatus = 0)
        {
            AChqModel aChqModel = new AChqModel();
            aChqModel.AchqHDetailsList = informationService.GetAchqHList(rno, applyStatus);
            aChqModel.SelectCheque = selectChqList;
            aChqModel.ShowWith = showWith;
            return PartialView("_ChequeNumberDetails", aChqModel);
        }

        public ActionResult ChequelistByStatusAndAccountId(byte statusId = 0, int iAccNo = 0)
        {
            AChqModel aChqModel = new AChqModel();

            aChqModel.cstate = statusId;
            aChqModel.AchqList = informationService.GetChequeStatusByIAccNoAndStatuId(statusId, iAccNo);
            return PartialView("ChequeByChequeStatus", aChqModel);
        }
        public ActionResult CheckByChequeStatusTable(byte statusId, int iAccNo)
        {
            AChqModel aChqModel = new AChqModel();

            aChqModel.cstate = statusId;
            aChqModel.AchqList = informationService.GetChequeStatusByIAccNoAndStatuId(statusId, iAccNo);
            return PartialView("CheckByChequeStatusTable", aChqModel);
        }
        [HttpPost]
        public ActionResult ChequeChangeStatus(AChqModel achqModel)
        {
            //if (ModelState.IsValid)
            //{
            returnMessage = informationService.UpdateChequeBlockUnBlockDeactivateChequeStatus(achqModel);
            return Json(returnMessage, JsonRequestBehavior.AllowGet);
            //}
            //else
            //{
            //    returnMessage.Success = false;
            //    returnMessage.Msg = "Please fill all required field.!!";
            //    return Json(returnMessage, JsonRequestBehavior.AllowGet);
            //}
        }
        public ActionResult ChequeGoodForPayment()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult ChequeGoodForPayment(ChannakyaBase.Model.ViewModel.WithdrawViewModel withdrawModel, Model.ViewModel.TaskVerificationList taskVerifier)
        {
            try
            {
                returnMessage = informationService.ChequeGoodForPayment(withdrawModel, taskVerifier);
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ChequeGoodForPaymentVerification()
        {
            ChannakyaBase.Model.ViewModel.WithdrawViewModel chequeVerification = new ChannakyaBase.Model.ViewModel.WithdrawViewModel();
            var goodForpayment = informationService.GetChequeGoodForPaymentList(1, 10);
            chequeVerification.WithdrawList = goodForpayment;
            return PartialView(chequeVerification);
        }
        public ActionResult _ChequeGoodForPaymentVerification(int pageNo = 1, int pageSize = 2)
        {
            ChannakyaBase.Model.ViewModel.WithdrawViewModel chequeVerification = new ChannakyaBase.Model.ViewModel.WithdrawViewModel();
            var goodForpayment = informationService.GetChequeGoodForPaymentList(1, 10);
            chequeVerification.WithdrawList = goodForpayment;
            return PartialView(chequeVerification);
        }
        public ActionResult ChequeGoodForPaymentDetails(Int64 tno)
        {
            var goodForpayment = informationService.GetChequeGoodForPaymentDetails(tno);
            return PartialView(goodForpayment);

        }
        public ActionResult CheckCheque(int chequeNumber = 0, int accId = 0)
        {
            bool IsExiestscheque = TellerUtilityService.CheckChequeNumber(chequeNumber, accId);
            if (IsExiestscheque)
            {

                if (TellerUtilityService.IsDeactiveChequeBook(accId, chequeNumber))
                {
                    returnMessage.Msg = "Cheque book is finished.!!";
                    returnMessage.Success = false;

                }
                else if (TellerUtilityService.IsDuplicateChequeNo(accId, chequeNumber))
                {
                    if (InformationUtilityService.IsChequeBounce(accId, chequeNumber))
                    {
                        returnMessage.Success = false;
                        returnMessage.Msg = "Cheque is bounce for a reason :- " + InformationUtilityService.BounceReason(accId, chequeNumber) + "!!";
                    }
                    else
                    {
                        returnMessage.Success = false;
                        returnMessage.Msg = "Cheque number is already used.!!";
                    }
                }
                else
                {
                    returnMessage = TellerUtilityService.CheckChequeState(accId, chequeNumber);
                }
            }
            else
            {
                returnMessage.Msg = "This cheque number doesn't belongs to account holder.";
                returnMessage.Success = false;
            }
            return Json(returnMessage, JsonRequestBehavior.AllowGet);
        }
        public ActionResult InsertInternalChequeDeposit()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult InsertInternalChequeDeposit(InternalChequeDepositModel internalChqModel, Model.ViewModel.TaskVerificationList taskVerifier)
        {
            try
            {
                returnMessage = informationService.InsertInternalChequeDeposit(internalChqModel, taskVerifier);
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(returnMessage, JsonRequestBehavior.AllowGet);

            }
        }

        public ActionResult InternalChequeDepositDetails(long tno)
        {
            var internalDepositDeails = informationService.InternalChequeDepositDetails(tno);
            return PartialView(internalDepositDeails);
        }

        public ActionResult UnverifiedChequeDepositList()
        {
            InternalChequeDepositModel internalChequeDepositModel = new InternalChequeDepositModel();

            var internalDepositDeails = informationService.InternalChequeDepositList(1, 10);
            return PartialView(internalDepositDeails);
        }
        public ActionResult _UnverifiedChequeDepositList(int pageNo = 1, int pageSize = 10)
        {
            var internalDepositDeails = informationService.InternalChequeDepositList(pageNo, pageSize);
            return PartialView(internalDepositDeails);
        }
        public ActionResult ChequePrint()
        {
            List<AChqModel> aChqModel = new List<AChqModel>();
            aChqModel = informationService.GetChequeRegisteredNotPrinted();
            //ViewBag.ChequeFlag = informationService.GetChequePrintType();
            //ViewBag.ChequeFlag = 1;

            return PartialView(aChqModel);
        }
        public ActionResult _ChequePrint(int chequeRecordNo)
        {
            AChqModel aChqModel = new AChqModel();
            aChqModel = informationService.GetChequeDetailsFromRno(chequeRecordNo);
            return PartialView(aChqModel);
        }
        public ActionResult ChequePreview()
        {
            return View();

        }

        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetDefaultPrinter(string Name);

        public void SetDefaultPrinter()
        {
            PrinterSettings printerSettings = new PrinterSettings();
            var check = printerSettings.IsDefaultPrinter;

            var printerName = printerSettings.PrinterName;
            var printerNameDB = informationService.GetPrinter();
            SetDefaultPrinter(printerNameDB.DefaultPrinterName);

            //PrintDocument recordDoc = new PrintDocument();
            //recordDoc.PrinterSettings = printerSettings;

            //IEnumerable<PaperSize> paperSizes = printerSettings.PaperSizes.Cast<PaperSize>();
            //var s = printerSettings.PaperSizes;
            //PaperSize sizeContinuous = paperSizes.First<PaperSize>(size => size.PaperName == "Continous"); // setting paper size to A4 size
            //recordDoc.DefaultPageSettings.PaperSize = sizeContinuous;
            //recordDoc.
            //recordDoc.Print();
        }

        [HttpGet]
        public ActionResult SelectPrinter()
        {
            //PrinterSettings printerSettings = new PrinterSettings();
            //var installedPrinters = PrinterSettings.InstalledPrinters;

            return View();
        }

        public ActionResult _SelectPrinter(string printerName)
        {
            informationService.SetPrinter(printerName);
            return View();
        }
        public ActionResult GetPaperSizeByPrinterName(string printerName)
        {
            var paperSizes = InformationUtilityService.GetPrinterPaperSize(printerName);
            return Json(paperSizes, JsonRequestBehavior.AllowGet);
        }

        //[ValidateInput(false)]
        //public ActionResult _PrintChequePrint(string htmlf)
        //{
        //    //*****Render any HTML fragment or document to HTML*****
        //    var Renderer = new IronPdf.HtmlToPdf();
        //    htmlf = "<html><head><title>Print cheque</title></head><body>" + htmlf + "</body></html>";
        //    DateTime dt = DateTime.Now;
        //    var OutputPath = AppDomain.CurrentDomain.BaseDirectory + "Content/ChequePrint/Print" + dt.ToString("yyyyMMddHHmmss") + ".pdf";

        //    //*****changing the pagesetting during rendering*****
        //    //Renderer.PrintOptions.PaperOrientation = PdfPrintOptions.PdfPaperOrientation.Landscape;
        //    //Renderer.PrintOptions.MarginTop = 0;
        //    //Renderer.PrintOptions.MarginBottom = 0;
        //    //Renderer.PrintOptions.MarginLeft = 0;
        //    //Renderer.PrintOptions.MarginRight = 0;
        //    //Renderer.PrintOptions.Zoom = 20;
        //    var PDF = Renderer.RenderHtmlAsPdf(htmlf);

        //    //*****setting the default printer settings*****
        //    PrintDocument printDocument = PDF.GetPrintDocument();
        //    //PdfDocument doc = new PdfDocument(OutputPath);
        //    var printerDetails = informationService.GetPrinter(); //getting the default printers that is set in database
        //    PrinterSettings printerSettings = new PrinterSettings();
        //    var paperSizes = printerSettings.PaperSizes;
        //    var paperSizesDB = printerDetails.PaperSize;

        //    //*****papersize default from database*****
        //    IEnumerable<PaperSize> defaultPaperSizes = printerSettings.PaperSizes.Cast<PaperSize>();
        //    PaperSize paperSizeDB = defaultPaperSizes.First<PaperSize>(size => size.PaperName == printerDetails.PaperSize);
        //    printDocument.DefaultPageSettings.PaperSize = paperSizeDB;
        //    printDocument.PrinterSettings.DefaultPageSettings.PaperSize = paperSizeDB;

        //    //*****for margins and orientations*****
        //    Margins margin = new Margins(0, 0, 0, 0);
        //    printDocument.DefaultPageSettings.Margins = margin;
        //    printDocument.PrinterSettings.DefaultPageSettings.Margins = margin;
        //    printDocument.PrinterSettings.DefaultPageSettings.Landscape = false;
        //    printDocument.DefaultPageSettings.Landscape = false;
        //    printDocument.OriginAtMargins = false;
        //    //PdfPrintOptions che = new PdfPrintOptions();
        //    //che.PaperOrientation = PdfPrintOptions.PdfPaperOrientation.Landscape;
        //    //che.MarginTop = 0;
        //    //che.MarginBottom = 0;
        //    //che.MarginLeft = 0;
        //    //che.MarginRight = 0;
        //    //che.Zoom = 20;
        //    printDocument.Print();
        //    PDF.SaveAs(OutputPath);
        //    //doc.Print();

        //    //*****opens our PDF file so we can see the result in our default PDF viewer****
        //    //System.Diagnostics.Process.Start(OutputPath);

        //    //*****parameters neededto go to the view ChequePrint*****
        //    List<AChqModel> aChqModel = new List<AChqModel>();
        //    aChqModel = informationService.GetChequeRegisteredNotPrinted();
        //    ViewBag.ChequeFlag = informationService.GetChequePrintType();

        //    return View("ChequePrint", aChqModel);
        //}

        //[ValidateInput(false)]
        public ActionResult _PDFChequeWithCoordinates(int[] checkedId)
        {
            SetDefaultPrinter();
            ChannakyaBase.DAL.DatabaseModel.ChqAttribute chqattribute = new ChannakyaBase.DAL.DatabaseModel.ChqAttribute();
            chqattribute = informationService.GetChequeAttribute();
            ChannakyaBase.DAL.DatabaseModel.ChqSize chqSize = new ChannakyaBase.DAL.DatabaseModel.ChqSize();
            chqSize = informationService.GetChequeSize();

            //multiply by 15 to match coordinates to convert pixels into twips
            string[] chequeNumberPosition = Regex.Split(chqattribute.ChequeNumberPosition, @"\D+");
            int chequeNumberPositionX = int.Parse(chequeNumberPosition[1]) * 15;
            int chequeNumberPositionY = int.Parse(chequeNumberPosition[2]) * 15;

            string[] cheque2NumberPosition = Regex.Split(chqattribute.Cheque2NumberPosition, @"\D+");
            int cheque2NumberPositionX = int.Parse(cheque2NumberPosition[1]) * 15;
            int cheque2NumberPositionY = int.Parse(cheque2NumberPosition[2]) * 15;

            string[] AccountNumberPosition = Regex.Split(chqattribute.AccountNumberPosition, @"\D+");
            int AccountNumberPositionX = int.Parse(AccountNumberPosition[1]) * 15;
            int AccountNumberPositionY = int.Parse(AccountNumberPosition[2]) * 15;

            string[] AccountNamePostion = Regex.Split(chqattribute.AccountNamePostion, @"\D+");
            int AccountNamePostionX = int.Parse(AccountNamePostion[1]) * 15;
            int AccountNamePostionY = int.Parse(AccountNamePostion[2]) * 15;

            string[] AccountTypePosition = Regex.Split(chqattribute.AccountTypePosition, @"\D+");
            int AccountTypePositionX = int.Parse(AccountTypePosition[1]) * 15;
            int AccountTypePositionY = int.Parse(AccountTypePosition[2]) * 15;

            string[] BranchNamePosition = Regex.Split(chqattribute.BranchNamePosition, @"\D+");
            int BranchNamePositionX = int.Parse(BranchNamePosition[1]) * 15;
            int BranchNamePositionY = int.Parse(BranchNamePosition[2]) * 15;

            string[] BranchPhoneNumberPosition = Regex.Split(chqattribute.BranchPhoneNumberPosition, @"\D+");
            int BranchPhoneNumberPositionX = int.Parse(BranchPhoneNumberPosition[1]) * 15;
            int BranchPhoneNumberPositionY = int.Parse(BranchPhoneNumberPosition[2]) * 15;

            string[] BranchAddressPosition = Regex.Split(chqattribute.BranchAddressPosition, @"\D+");
            int BranchAddressPositionX = int.Parse(BranchAddressPosition[1]) * 15;
            int BranchAddressPositionY = int.Parse(BranchAddressPosition[2]) * 15;

            string[] chqHeight = Regex.Split(chqSize.Height, @"\D+");
            int chqHeightX = int.Parse(chqHeight[0]) * 15;

            string[] chqWidth = Regex.Split(chqSize.Width, @"\D+");
            int chqWidthX = int.Parse(chqWidth[0]) * 15;

            if (checkedId != null)
            {
                var company = reportService.CompanyDetails(Global.BranchId);
                List<AChqModel> aChqModel = new List<AChqModel>();
                for (int i = 0; i < checkedId.Length; i++)
                {
                    aChqModel.Add(informationService.GetChequeDetailsFromRno(checkedId[i]));
                }

                Microsoft.VisualBasic.PowerPacks.Printing.Compatibility.VB6.Printer printer = new Microsoft.VisualBasic.PowerPacks.Printing.Compatibility.VB6.Printer();
                //int counter = 0;
                //float addHeight = 0;
                printer.FontSize = 10;
                int paperheight = printer.Height;
                int paperwidth = printer.Width;

                printer.Height = chqHeightX; //try this in print
                printer.Width = chqWidthX;

                foreach (var item in aChqModel)
                {
                    for (int i = item.cfrom; i <= item.cto; i++)
                    {
                        printer.CurrentX = chequeNumberPositionY;
                        printer.CurrentY = chequeNumberPositionX;
                        printer.Print(item.cfrom);

                        printer.CurrentX = cheque2NumberPositionY;
                        printer.CurrentY = cheque2NumberPositionX;
                        printer.Print(item.cfrom);

                        printer.CurrentX = AccountNamePostionY;
                        printer.CurrentY = AccountNamePostionX;
                        printer.Print(item.AccountName);

                        printer.CurrentX = AccountNumberPositionY;
                        printer.CurrentY = AccountNumberPositionX;
                        printer.Print(item.AccountNumber);

                        printer.CurrentX = AccountTypePositionY;
                        printer.CurrentY = AccountTypePositionX;
                        printer.Print(item.AccountType);

                        printer.CurrentX = BranchNamePositionY;
                        printer.CurrentY = BranchNamePositionX;
                        printer.Print(item.branchName);

                        printer.CurrentX = BranchAddressPositionY;
                        printer.CurrentY = BranchAddressPositionX;
                        printer.Print(item.branchAddress);

                        printer.CurrentX = BranchPhoneNumberPositionY;
                        printer.CurrentY = BranchPhoneNumberPositionX;
                        printer.Print(item.branchPhoneNumber);

                        //counter++;
                        //addHeight = counter * chqHeightX;
                        printer.NewPage();
                    }
                }
                printer.EndDoc();
                //changing in databases
                foreach (var item in aChqModel)
                {
                    item.IsPrinted = true;
                }
                informationService.UpdatePrintedCheques(aChqModel);
                //return PartialView(aChqModel);
                List<AChqModel> aChqModel2 = new List<AChqModel>();
                aChqModel2 = informationService.GetChequeRegisteredNotPrinted();
                ViewBag.ChequeFlag = informationService.GetChequePrintType();
                return PartialView("ChequePrint", aChqModel2);
            }
            else
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Check the account you want to print";
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult _ChequePrintDetails(int[] checkedId)
        {
            SetDefaultPrinter();
            if (checkedId != null)
            {
                var company = reportService.CompanyDetails(Global.BranchId);
                List<AChqModel> aChqModel = new List<AChqModel>();
                for (int i = 0; i < checkedId.Length; i++)
                {
                    aChqModel.Add(informationService.GetChequeDetailsFromRno(checkedId[i]));
                }

                ////changing in databases
                //foreach (var item in aChqModel)
                //{
                //    item.IsPrinted = true;
                //}
                ////IronPdf.AspxToPdf.RenderThisPageAsPdf(FileBehavior.Attachment, "MyPdf.pdf", new PdfPrintOptions() { DPI = 300 });
                //informationService.UpdatePrintedCheques(aChqModel);
                return PartialView(aChqModel);
            }
            else
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Check the account you want to print";
                return Json(returnMessage, JsonRequestBehavior.AllowGet);
            }
            //return RedirectToAction("_ChequePrintRedirect", aChqModel);
            //return PartialView(aChqModel);
        }

        public ActionResult _ChequePrintDetailsNotepad(int[] checkedId)
        {
            SetDefaultPrinter();
            var company = reportService.CompanyDetails(Global.BranchId);
            List<AChqModel> aChqModel = new List<AChqModel>();
            for (int i = 0; i < checkedId.Length; i++)
            {
                aChqModel.Add(informationService.GetChequeDetailsFromRno(checkedId[i]));
            }

            using (FileStream fileStream = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "Content/ChequePrint/ChequePrint" + aChqModel[0].rno + ".txt", FileMode.Create))
            using (TextWriter writer = new StreamWriter(fileStream))
            {
                int chequeCounter = 0;
                foreach (var item in aChqModel)
                {
                    int chqNo = item.cfrom;
                    for (int i = item.cfrom; i <= item.cto; i++)
                    {
                        chequeCounter++;
                        writer.WriteLine(chqNo + "\t\t\t\t\t\t\t\t\t\t\t" + chqNo);
                        writer.WriteLine("\n");
                        writer.WriteLine("\n");
                        writer.WriteLine("\n");
                        writer.WriteLine("\n");
                        writer.WriteLine("\n");
                        writer.WriteLine("\n");

                        writer.WriteLine("\t\t\t  " + item.AccountName + "\n");
                        writer.WriteLine("\t\t\t  " + item.AccountNumber + "\n");
                        writer.WriteLine("\t\t\t  " + item.AccountType + "\n");
                        if (chequeCounter != 3)
                        {
                            writer.WriteLine("\n");
                        }
                        else
                        {
                            chequeCounter = 0;
                        }

                        chqNo = chqNo + 1;
                    }
                    item.IsPrinted = true;    //this is for later
                }
            }
            //changing in databases
            informationService.UpdatePrintedCheques(aChqModel);

            TempData["file"] = AppDomain.CurrentDomain.BaseDirectory + "Content/ChequePrint/ChequePrint" + aChqModel[0].rno + ".txt";
            TempData["filename"] = "ChequePrint" + aChqModel[0].rno + ".txt";
            return RedirectToAction("_ChequePrintNotepadDownload");
        }
        public FileResult _ChequePrintNotepadDownload()
        {
            var file = Convert.ToString(TempData["file"]);
            var filename = Convert.ToString(TempData["filename"]);
            TempData.Keep();
            byte[] filebytes = System.IO.File.ReadAllBytes(file);
            return File(filebytes, "text/plain", filename);
        }
        public ActionResult _ChequePrintRedirect(AChqModel aChqModel)
        {
            return PartialView(aChqModel);
        }
        public int AddChequeAttributes(string custstyle, string accnostyle, string savingtypestyle, string chknostyle, string chk2nostyle, string branchnamestyle, string branchaddressstyle, string branchphonenumberstyle)
        {
            try
            {
                ChqAttributeViewModel chqattribute = new ChqAttributeViewModel();
                chqattribute.AccountNamePostion = custstyle;
                chqattribute.AccountNumberPosition = accnostyle;
                chqattribute.AccountTypePosition = savingtypestyle;
                chqattribute.ChequeNumberPosition = chknostyle;
                chqattribute.Cheque2NumberPosition = chk2nostyle;
                chqattribute.BranchNamePosition = branchnamestyle;
                chqattribute.BranchAddressPosition = branchaddressstyle;
                chqattribute.BranchPhoneNumberPosition = branchphonenumberstyle;
                informationService.AddChequeAttribute(chqattribute);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public int AddChequeSize(string height, string width)
        {
            try
            {
                informationService.AddChequeSize(height, width);
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
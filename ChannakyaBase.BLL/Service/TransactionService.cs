using ChannakyaBase.BLL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChannakyaBase.DAL.DatabaseModel;
using ChannakyaBase.Model.ViewModel;
using System.Globalization;
using ChannakyaBase.Model.Models;
using System.Transactions;
using System.Web.Mvc;

namespace ChannakyaBase.BLL.Service
{
    public class TransactionService
    {
        TellerService tellerService = new TellerService();
        private GenericUnitOfWork uow = null;
        private ReturnBaseMessageModel returnMessage = null;
        CommonService commonServic = null;

        public TransactionService()
        {
            uow = new GenericUnitOfWork();
            commonServic = new CommonService();
            returnMessage = new ReturnBaseMessageModel();
        }
        public List<TempIntRate> GetAllTempIntRate()
        {
            return uow.Repository<TempIntRate>().GetAll().ToList();
        }
        public TempIntRate GetSingleTempIntRate(int? TID)
        {
            return uow.Repository<TempIntRate>().GetSingle(x => x.TID == TID);
        }
        public ReturnBaseMessageModel SaveTempIntRate(TempIntRate tempIntRate)
        {
            try
            {
                if (tempIntRate.TID == 0)
                {
                    uow.Repository<TempIntRate>().Add(tempIntRate);
                    returnMessage.Msg = "Template Interest Rate Saved successfully";
                }
                else
                {
                    uow.Repository<TempIntRate>().Edit(tempIntRate);
                    returnMessage.Msg = "Template Interest Rate Edited successfully";
                }
                uow.Commit();
                returnMessage.Success = true;
                return returnMessage;

            }
            catch (Exception ex)
            {
                returnMessage.Success = false;
                returnMessage.Msg = "Not Save" + ex.Message;
                return returnMessage;
            }
        }

        public bool CheckTempIntRate(int? TID)
        {
            int count = uow.Repository<ProductTID>().FindBy(x => x.TID == TID).Count();
            int countTemplateIntRate = uow.Repository<TempIntRateVal>().FindBy(x => x.TID == TID).Count();
            if (count >= 1 || countTemplateIntRate >= 1)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        public void TempIntRateDelete(TempIntRate tempIntRate)
        {
            uow.Repository<TempIntRate>().Delete(tempIntRate);
            uow.Commit();
        }
        public List<TempIntRateVal> GetAllTempIntRateVal(int TID)
        {
            if (TID == 0)
            {
                return uow.Repository<TempIntRateVal>().GetAll().ToList();
            }
            else
                return uow.Repository<TempIntRateVal>().GetAll().Where(x => x.TID == TID).ToList();
        }
        public TempIntRateVal GetSingleTempIntRateVal(int? TIRID)
        {
            return uow.Repository<TempIntRateVal>().GetSingle(x => x.TIRID == TIRID);
        }
        public ReturnBaseMessageModel SaveTempIntRateVal(TempIntRateVal tempIntRateVal)
        {
            try
            {
                if (tempIntRateVal.TIRID == 0)
                {
                    uow.Repository<TempIntRateVal>().Add(tempIntRateVal);
                    returnMessage.Msg = "Template Interest Rate  Saved successfully";
                    returnMessage.Success = true;
                }
                else
                {
                    uow.Repository<TempIntRateVal>().Edit(tempIntRateVal);
                    returnMessage.Msg = "Template Interest Rate  Saved successfully";
                    returnMessage.Success = true;
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
        public int CheckTempIntValRate(int? TIRID)
        {
            int count = uow.Repository<TempIntRateVal>().FindBy(x => x.TIRID == TIRID).Count();
            return count;
        }
        public void TempIntRateValDelete(TempIntRateVal tempIntRateval)
        {
            uow.Repository<TempIntRateVal>().Delete(tempIntRateval);
            uow.Commit();
        }
        public List<PolicyIntCalc> GetAllInterestPolicy()
        {
            return uow.Repository<PolicyIntCalc>().GetAll().ToList();
        }
        public PolicyIntCalc GetSingleInterestPolicy(int? PSID)
        {
            return uow.Repository<PolicyIntCalc>().GetSingle(x => x.PSID == PSID);
        }
        public ReturnBaseMessageModel SaveInterestPolicy(PolicyIntCalc policyIntCalc)
        {
            if (policyIntCalc.PSID == 0)
            {
                uow.Repository<PolicyIntCalc>().Add(policyIntCalc);
                returnMessage.Msg = "Interest Policy  Added Successfully";
                returnMessage.Success = true;
            }
            else
            {
                uow.Repository<PolicyIntCalc>().Edit(policyIntCalc);
                returnMessage.Msg = "Interest Policy  Updated Successfully";
                returnMessage.Success = true;

            }
            uow.Commit();
            return returnMessage;
        }
        public void InterestPolicyDelete(PolicyIntCalc policyIntCalc)
        {
            uow.Repository<PolicyIntCalc>().Delete(policyIntCalc);
            uow.Commit();
        }
        public List<PolicyPenCalc> GetAllPenaltyPolicy()
        {
            return uow.Repository<PolicyPenCalc>().GetAll().ToList();
        }
        public bool CheckTempIntRateInFloatingPointInterest(int TID)
        {
            var count = uow.Repository<TempIntRateVal>().FindBy(x => x.TID == TID).Count();
            var countProductTID = uow.Repository<ProductTID>().FindBy(x => x.TID == TID).Count();
            if (count > 1)
            {
                return true;
            }

            else
            {
                if (countProductTID < 1)
                {
                    return true;
                }

                return false;
            }

        }
        public PolicyPenCalc GetSinglePenaltyPolicy(int? PCID)
        {
            return uow.Repository<PolicyPenCalc>().GetSingle(x => x.PCID == PCID);
        }


        public ReturnBaseMessageModel SavePenaltyPolicy(PolicyPenCalc policyPenCalc)
        {
            try
            {
                if (policyPenCalc.PCID == 0)
                {
                    uow.Repository<PolicyPenCalc>().Add(policyPenCalc);
                    returnMessage.Msg = "Penalty Policy Saved Successfully";
                    returnMessage.Success = true;

                }
                else
                {
                    uow.Repository<PolicyPenCalc>().Edit(policyPenCalc);
                    returnMessage.Msg = "Penalty Policy Edited Successfully";
                    returnMessage.Success = true;

                }
                uow.Commit();
                return returnMessage;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public void SaveProductDetail(ProductDetail productDetail)
        {
            if (productDetail.PID == 0)
            {
                uow.Repository<ProductDetail>().Add(productDetail);
            }
            else
            {
                uow.Repository<ProductDetail>().Edit(productDetail);
            }
            uow.Commit();
        }
        public void InterestPenaltyDelete(PolicyPenCalc policyPenCalc)
        {
            uow.Repository<PolicyPenCalc>().Delete(policyPenCalc);
            uow.Commit();
        }

        public bool CheckInterestPolicy(int pSID)
        {
            int countInterestPolicy = uow.Repository<APolicyInterest>().FindBy(x => x.PSID == pSID).Count();
            int countProductPSID = uow.Repository<ProductPSID>().FindBy(x => x.PSID == pSID).Count();
            if (countInterestPolicy >= 1 || countProductPSID >= 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void SaveProductDurInt(ProductDurationInt productDurationInt)
        {
            uow.Repository<ProductDurationInt>().Edit(productDurationInt);
            uow.Commit();
        }
        public void SaveProductICBDur(ProductICBDur productICBDur)
        {
            uow.Repository<ProductICBDur>().Edit(productICBDur);
            uow.Commit();
        }
        public void SaveLogs(InterestLog interestlog)
        {
            uow.Repository<InterestLog>().Add(interestlog);
            uow.Commit();
        }

        public ReturnBaseMessageModel ChangeProductInterest(ChangeProductInterestViewModel changeProductInterest, List<AccountDetailsViewModel> accountInterestList, bool flag)
        {
            using (TransactionScope transaction = new TransactionScope(
              // a new transaction will always be created
              TransactionScopeOption.Required,
              // we will allow volatile data to be read during transaction
              new TransactionOptions()
              {
                  IsolationLevel = IsolationLevel.ReadCommitted,
                  Timeout = TimeSpan.MaxValue

              }
          ))
            {
                try
                {
                    InterestLog interestLog = new InterestLog();
                    ARateMaster aRateMaster = new ARateMaster();
                    ProductDurationInt productDurationInt = new ProductDurationInt();
                    ProductICBDur productICBDur = new ProductICBDur();
                    ProductDetail productDetail = new ProductDetail();

                    productDetail = tellerService.GetProductDetailsByProductId(changeProductInterest.PID);

                    AccountDetailsViewModel accountDetailsForStype = new AccountDetailsViewModel();
                    accountDetailsForStype = GetProductType(changeProductInterest.PID);

                    bool hasIndividalRate = TellerUtilityService.HasIndiviualInterestRate(changeProductInterest.PID);

                    aRateMaster.EffectiveDate = changeProductInterest.InterestChangeDate;
                    aRateMaster.PostedDate = DateTime.Now;
                    aRateMaster.PostedBy = Loader.Models.Global.UserId;

                    interestLog.EffectiveFrom = changeProductInterest.InterestChangeDate;
                    interestLog.PostedBy = Loader.Models.Global.UserId;
                    interestLog.PostedOn = DateTime.Now;
                    interestLog.isApplied = false;
                    #region add logs
                    if (TellerUtilityService.IsFixedAccount(changeProductInterest.PID))
                    {

                        interestLog.InterestRate = Convert.ToDecimal(changeProductInterest.NewRate);
                        interestLog.PId = changeProductInterest.PID;
                        interestLog.DVId = changeProductInterest.Duration;
                        interestLog.ICBId = changeProductInterest.InterestCapitalization;
                        if (accountDetailsForStype.SType == 1)
                        {
                            interestLog.PIRate = changeProductInterest.PenaltyInterestRate;
                            interestLog.PPRate = changeProductInterest.PrinciplePenaltyRate;
                            interestLog.OIRate = changeProductInterest.OIRate;
                        }
                        //interestLog.ProductInterestLogChilds.Add(productInterestLogChild);

                    }
                    else if (TellerUtilityService.IsRecurringDeposit(changeProductInterest.PID) || TellerUtilityService.IsOtherTypeOfRecurringDeposit(changeProductInterest.PID))
                    {
                        interestLog.InterestRate = Convert.ToDecimal(changeProductInterest.NewRate);
                        interestLog.PId = changeProductInterest.PID;
                        interestLog.DBId = changeProductInterest.Basic;
                        interestLog.DVId = changeProductInterest.Duration;
                        interestLog.ICBId = changeProductInterest.InterestCapitalization;
                        interestLog.Value = Convert.ToDouble(changeProductInterest.Contribution);
                        if (accountDetailsForStype.SType == 1)
                        {
                            interestLog.PIRate = changeProductInterest.PenaltyInterestRate;
                            interestLog.PPRate = changeProductInterest.PrinciplePenaltyRate;
                            interestLog.OIRate = changeProductInterest.OIRate;

                        }
                        //interestLog.ProductInterestLogChilds.Add(productInterestLogChild);

                    }
                    else
                    {
                        interestLog.InterestRate = Convert.ToDecimal(changeProductInterest.NewRate);
                        interestLog.PId = changeProductInterest.PID;
                        interestLog.ICBId = changeProductInterest.InterestCapitalization;
                        if (accountDetailsForStype.SType == 1)
                        {
                            interestLog.PIRate = changeProductInterest.PenaltyInterestRate;
                            interestLog.PPRate = changeProductInterest.PrinciplePenaltyRate;
                            interestLog.OIRate = changeProductInterest.OIRate;

                        }
                        //interestLog.ProductInterestLogChilds.Add(productInterestLogChild);
                    }
                    #endregion

                    // if today
                    if (changeProductInterest.InterestChangeDate == commonServic.GetBranchTransactionDate())
                    {
                        #region Apply To Products Only
                        if (flag == true)
                        {
                            if (TellerUtilityService.IsFixedAccount(changeProductInterest.PID))
                            {
                                if (accountDetailsForStype.SType == 1)
                                {
                                    productDetail.IRate = changeProductInterest.NewRate;
                                    productDetail.OIRate = changeProductInterest.OIRate;
                                    productDetail.PIRate = changeProductInterest.PenaltyInterestRate;
                                    productDetail.PPRate = changeProductInterest.PrinciplePenaltyRate;
                                    SaveProductDetail(productDetail);
                                }
                                else
                                {
                                    productDurationInt = uow.Repository<ProductDurationInt>().FindBy(x => x.ICBId == changeProductInterest.InterestCapitalization && x.PId == changeProductInterest.PID && x.DVId == changeProductInterest.Duration && x.DBId == null).FirstOrDefault();
                                    productDurationInt.InterestRate = Convert.ToDecimal(changeProductInterest.NewRate);
                                    SaveProductDurInt(productDurationInt);
                                }
                            }
                            else if (TellerUtilityService.IsRecurringDeposit(changeProductInterest.PID) || TellerUtilityService.IsOtherTypeOfRecurringDeposit(changeProductInterest.PID))
                            {

                                if (accountDetailsForStype.SType == 1)
                                {
                                    productDetail.IRate = changeProductInterest.NewRate;
                                    productDetail.OIRate = changeProductInterest.OIRate;
                                    productDetail.PIRate = changeProductInterest.PenaltyInterestRate;
                                    productDetail.PPRate = changeProductInterest.PrinciplePenaltyRate;
                                    SaveProductDetail(productDetail);
                                }
                                else
                                {
                                    productDurationInt = uow.Repository<ProductDurationInt>().FindBy(x => x.ICBId == changeProductInterest.InterestCapitalization && x.PId == changeProductInterest.PID && x.DVId == changeProductInterest.Duration && x.Value == (double)changeProductInterest.Contribution && x.DBId == changeProductInterest.Basic).FirstOrDefault();
                                    productDurationInt.InterestRate = Convert.ToDecimal(changeProductInterest.NewRate);
                                    SaveProductDurInt(productDurationInt);
                                }
                            }
                            else if (TellerUtilityService.IsOtherTypeOfRemainingProducts(changeProductInterest.PID))
                            {
                                if (accountDetailsForStype.SType == 1)
                                {
                                    productDetail.IRate = changeProductInterest.NewRate;
                                    productDetail.OIRate = changeProductInterest.OIRate;
                                    productDetail.PIRate = changeProductInterest.PenaltyInterestRate;
                                    productDetail.PPRate = changeProductInterest.PrinciplePenaltyRate;
                                    SaveProductDetail(productDetail);
                                }
                                else
                                {
                                    productDurationInt = uow.Repository<ProductDurationInt>().FindBy(x => x.ICBId == changeProductInterest.InterestCapitalization && x.PId == changeProductInterest.PID && x.DVId == changeProductInterest.Duration).FirstOrDefault();
                                    productDurationInt.InterestRate = Convert.ToDecimal(changeProductInterest.NewRate);
                                    SaveProductDurInt(productDurationInt);
                                }
                            }
                            else
                            {
                                if (accountDetailsForStype.SType == 1)
                                {
                                    productDetail.IRate = changeProductInterest.NewRate;
                                    productDetail.OIRate = changeProductInterest.OIRate;
                                    productDetail.PIRate = changeProductInterest.PenaltyInterestRate;
                                    productDetail.PPRate = changeProductInterest.PrinciplePenaltyRate;
                                    SaveProductDetail(productDetail);
                                }
                                else
                                {
                                    productICBDur = uow.Repository<ProductICBDur>().FindBy(x => x.PICBDurID == changeProductInterest.InterestCapitalization && x.PID == changeProductInterest.PID).FirstOrDefault();
                                    productICBDur.IRate = changeProductInterest.NewRate;
                                    SaveProductICBDur(productICBDur);
                                }
                            }
                        }
                        #endregion

                        #region Apply To Accounts
                        //Accounts update
                        ARate aRate = new ARate();

                        if (flag == false) //not for product update
                        {
                            foreach (var items in accountInterestList)
                            {
                                ADetail adetail = uow.Repository<ADetail>().GetSingle(x => x.IAccno == items.IAccno);

                                //if (changeProductInterest.CurrentRate != items.IRate)
                                //{
                                if (hasIndividalRate == true)
                                {
                                    if (flag == false)
                                    {
                                        if (items.isSingleChecked)
                                        {
                                            aRate.IRate = items.NewInterestRate;
                                            aRate.IAccno = items.IAccno;
                                            adetail.IRate = Convert.ToDecimal(items.NewInterestRate);

                                            if (accountDetailsForStype.SType == 1)
                                            {
                                                APRate apRate = uow.Repository<APRate>().GetSingle(x => x.IAccno == adetail.IAccno);
                                                apRate.IAccno = adetail.IAccno;

                                                apRate.PIRate = changeProductInterest.PenaltyInterestRate;
                                                apRate.PPRate = changeProductInterest.PrinciplePenaltyRate;

                                                apRate.ADetail = adetail;
                                                //adetail.APRate = apRate;
                                                uow.Repository<APRate>().Edit(apRate);


                                            }
                                            //aRate.ADetail = adetail; //checkk
                                            //aRateMaster.ARates.Add(aRate);
                                            //uow.Repository<ADetail>().Edit(adetail);
                                        }
                                    }
                                    else
                                    {
                                        if (items.isSingleChecked)
                                        {

                                            aRate.IRate = changeProductInterest.NewRate;
                                            aRate.IAccno = items.IAccno;
                                            adetail.IRate = Convert.ToDecimal(changeProductInterest.NewRate);
                                            if (accountDetailsForStype.SType == 1)
                                            {
                                                APRate apRate = uow.Repository<APRate>().GetSingle(x => x.IAccno == adetail.IAccno);
                                                apRate.IAccno = adetail.IAccno;

                                                apRate.PIRate = changeProductInterest.PenaltyInterestRate;
                                                apRate.PPRate = changeProductInterest.PrinciplePenaltyRate;

                                                apRate.ADetail = adetail;
                                                //adetail.APRate = apRate;
                                                uow.Repository<APRate>().Edit(apRate);

                                            }
                                        }
                                        //aRate.IRate = changeProductInterest.NewRate;
                                        //aRate.IAccno = items.IAccno;
                                        //adetail.IRate = Convert.ToDecimal(changeProductInterest.NewRate);
                                    }
                                }
                                else
                                {
                                    if (items.isSingleChecked)
                                    {
                                        aRate.IRate = items.NewInterestRate;
                                        aRate.IAccno = items.IAccno;
                                        adetail.IRate = Convert.ToDecimal(items.NewInterestRate);
                                        if (accountDetailsForStype.SType == 1)
                                        {
                                            APRate apRate = uow.Repository<APRate>().GetSingle(x => x.IAccno == adetail.IAccno);

                                            apRate.PIRate = changeProductInterest.PenaltyInterestRate;
                                            apRate.PPRate = changeProductInterest.PrinciplePenaltyRate;
                                            apRate.IAccno = adetail.IAccno;

                                            apRate.ADetail = adetail;
                                            //adetail.APRate = apRate;
                                            uow.Repository<APRate>().Edit(apRate);
                                        }
                                    }
                                }


                                aRate.ADetail = adetail; //checkk
                                aRateMaster.ARates.Add(aRate);
                                uow.Repository<ADetail>().Edit(adetail);
                                //}
                            }//loop end
                        }//flag end
                        #endregion

                        interestLog.isApplied = true;
                        uow.Repository<ARateMaster>().Add(aRateMaster);
                    }
                    SaveLogs(interestLog);
                    uow.Commit();

                    transaction.Complete();
                    returnMessage.Msg = "Accounts and Logs Successfully Updated";
                    returnMessage.Success = true;

                }
                catch (Exception ex)
                {
                    returnMessage.Success = false;
                    returnMessage.Msg = "Not saved " + ex.Message;
                    transaction.Dispose();
                }
            }
            return returnMessage;

        }

        public bool CheckPenaltyPolicy(int pCID)
        {

            int countPenaltyPolicy = uow.Repository<APolPen>().FindBy(x => x.PCID == pCID).Count();
            int countPenaltyPSID = uow.Repository<ProductPCID>().FindBy(x => x.PCID == pCID).Count();
            if (countPenaltyPolicy >= 1 || countPenaltyPSID >= 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool CheckExists(string PSName, int PSID)
        {
            //int count = uow.Repository<PolicyPenCalc>().GetAll().Where(x => x.PCNAME == pCNAME.Trim()).Where(x => x.PCID!= mYId).Count();

            int count = uow.Repository<PolicyIntCalc>().GetAll().Where(x => x.PSName.ToLower().Trim() == PSName.ToLower().Trim()).Where(x => x.PSID != PSID).Count();


            if (count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CheckExistsPenaltyPolicy(string PCName, int PCID)
        {
            int count = uow.Repository<PolicyPenCalc>().GetAll().Where(x => x.PCNAME.Trim().ToLower() == PCName.Trim().ToLower()).Where(x => x.PCID != PCID).Count();

            //int count = uow.Repository<PolicyPenCalc>().FindBy(x => x.PCNAME.Trim().ToLower() == PCName.Trim().ToLower() && x.PCID != PCID).Count();

            if (count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public AccountDetailsViewModel GetProductType(int pid = 0)
        {
            var stype = uow.Repository<AccountDetailsViewModel>().SqlQuery("select a.stype,b.PID from fin.SchmDetails a inner join fin.ProductDetail b on a.SDID = b.SDID where pid = " + pid + "  ").FirstOrDefault();
            return stype;
        }

        public static List<SelectListItem> GetTemplateListForFloatingInterest()
        {

            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {

                List<SelectListItem> lst = new List<SelectListItem>();
                var list = uow.Repository<TempIntRate>().GetAll().OrderBy(x => x.Tname);
                foreach (var item in list)
                {
                    lst.Add(new SelectListItem { Text = item.Tname, Value = item.TID.ToString() });
                }
                return lst;

            }



        }
    }
}

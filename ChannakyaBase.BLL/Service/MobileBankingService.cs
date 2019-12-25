using ChannakyaBase.BLL.Repository;
using ChannakyaBase.DAL.DatabaseModel;
using ChannakyaBase.Model.Models;
using ChannakyaBase.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ChannakyaBase.BLL.Service
{
    public class MobileBankingService
    {
        private GenericUnitOfWork uow = null;
        ReturnBaseMessageModel returnMessage = null;
        private CommonService commonService = null;
        public MobileBankingService()
        {
            uow = new GenericUnitOfWork();
            returnMessage = new ReturnBaseMessageModel();
            commonService = new CommonService();
        }

        public ReturnBaseMessageModel InitialRequest(MobileBankingModel mobileBanking)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions()
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            }
           ))
            {
                try
                {
                    MobileBanking mobileBankingRow = new MobileBanking();
                    mobileBankingRow.Tno = commonService.GetUtno();
                    mobileBankingRow.TDate = commonService.GetBranchTransactionDate();
                    mobileBankingRow.IAccNO = mobileBanking.IAccNO;
                    mobileBankingRow.TType = 100;
                    mobileBankingRow.MobileNo = mobileBanking.MobileNo;
                    mobileBankingRow.Amount = mobileBanking.Amount;
                    uow.Repository<MobileBanking>().Add(mobileBankingRow);
                    commonService.InsertAvailableBalance(5, mobileBanking.IAccNO, mobileBankingRow.Amount);
                    uow.Commit();
                    transaction.Complete();
                    returnMessage.Success = true;
                    returnMessage.Msg = "Request has been sent successfully.";
                    return returnMessage;
                }
                catch (Exception)
                {

                    returnMessage.Msg = "OOps somthing wrong.!!Request faild.!!";
                    returnMessage.Success = false;
                    transaction.Dispose();
                    return returnMessage;
                }

            }

        }
        public ReturnBaseMessageModel ClientResponse(Int64 tno, string successMessage, int SuccessCode)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions()
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            }
           ))
            {
                try
                {
                    MobileBanking mobileBankingRow = uow.Repository<MobileBanking>().GetSingle(x => x.Tno == tno);
                    mobileBankingRow.Remarks = successMessage;
                    commonService.InsertAvailableBalance(5, mobileBankingRow.IAccNO, -mobileBankingRow.Amount);
                    if (SuccessCode == 1)
                    {
                        mobileBankingRow.TType = 101;

                        commonService.InsertAvailableBalance(3, mobileBankingRow.IAccNO, -mobileBankingRow.Amount);
                        var accountRow = uow.Repository<ADetail>().GetSingle(x => x.IAccno == mobileBankingRow.IAccNO);
                        accountRow.Bal = accountRow.Bal - mobileBankingRow.Amount;
                        uow.Repository<ADetail>().Edit(accountRow);
                    }
                    else
                    {
                        mobileBankingRow.TType = 102;
                    }
                    uow.Repository<MobileBanking>().Edit(mobileBankingRow);

                    uow.Commit();
                    transaction.Complete();
                    returnMessage.Success = true;
                    returnMessage.Msg = "Request has been sent successfully.";
                    return returnMessage;
                }
                catch (Exception)
                {

                    returnMessage.Msg = "OOps somthing wrong.!!Request faild.!!";
                    returnMessage.Success = false;
                    transaction.Dispose();
                    return returnMessage;
                }

            }
        }
    }
}

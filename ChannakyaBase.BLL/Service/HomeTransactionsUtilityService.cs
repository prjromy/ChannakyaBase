using ChannakyaBase.BLL.Repository;
using ChannakyaBase.DAL.DatabaseModel;
using ChannakyaBase.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ChannakyaBase.BLL.Service
{
    public class HomeTransactionsUtilityService
    {
        private static GenericUnitOfWork uow = null;
        static HomeTransactionsUtilityService()
        {
            uow = new GenericUnitOfWork();
        }
        public static SelectList GetCashierList()
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                CommonService cs = new CommonService();
                
                var cashierList = cs.GetAllCashier();
                return new SelectList(cashierList, "UserId", "EmployeeName");
            }
        }
        public static SelectList GetCurrencyList()
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                CommonService cs = new CommonService();
                var cashierList = cs.GetMyBalanceWithCurrency();
                return new SelectList(cashierList, "CTId", "CurrencyName");
            }
        }
    }
}

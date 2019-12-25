using ChannakyaBase.BLL.Repository;
using ChannakyaBase.DAL.DatabaseModel;
using ChannakyaBase.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChannakyaBase.BLL.Service
{
    public static class TransactionUtilityService
    {
        private static GenericUnitOfWork uow = null;
        static TransactionUtilityService()
        {
            uow = new GenericUnitOfWork();
        }
        public static SelectList TempInterestRate()
        {

            List<TempIntRate> TempInttRate = uow.Repository<TempIntRate>().GetAll().ToList();
            return new SelectList(TempInttRate, "TID", "Tname");

        }
        public static SelectList InterestRate()
        {

            List<RuleRate> ruleRate = uow.Repository<RuleRate>().GetAll().Where(x => x.Enable == true).ToList();
            return new SelectList(ruleRate, "RID", "RateRule");

        }
        public static SelectList InterestOnBal()
        {

            List<RuleBalance> ruleBalance = uow.Repository<RuleBalance>().GetAll().Where(x => x.Enable == true).ToList();
            return new SelectList(ruleBalance, "BALID", "BalRule");
    
        }
        public static SelectList InterestIn()
        {

            List<RuleDuration> TempInttRate = uow.Repository<RuleDuration>().GetAll().Where(x => x.Enable == true).ToList();
            return new SelectList(TempInttRate, "DURID", "DurRule");

        }
        public static SelectList PenaltyOnBal()
        {

            List<RulePenBalance> rulePenBalance = uow.Repository<RulePenBalance>().GetAll().Where(x => x.Enable == true).ToList();
            return new SelectList(rulePenBalance, "PBALID", "PBalance");

        }

        public static SelectList GetAllProductDetails()
        {
            using (ChannakyaBaseEntities _context = new ChannakyaBaseEntities())
            {
                var Product = (from x in _context.ProductDetails
                               select new ProductViewModel()
                               {
                                   ProductId = x.PID,
                                   ProductName = x.PName

                               }).ToList();

                return new SelectList(Product, "ProductId", "ProductName");
            }
        }


        public static SelectList GetProductDetails(int stype)
        {
            using (ChannakyaBaseEntities _context = new ChannakyaBaseEntities())
            {
                var Product = (from x in _context.ProductDetails
                               join s in _context.SchmDetails on x.SDID equals s.SDID
                               where s.SType == stype
                               select new ProductViewModel()
                               {
                                   ProductId = x.PID,
                                   ProductName = x.PName,
                                   enabled = x.enabled
                               }).ToList();
                Product = Product.Where(x => x.enabled == true).OrderBy(x=>x.ProductName).ToList();
                return new SelectList(Product, "ProductId", "ProductName");
            }
        }

        public static SelectList AccountType()
        {
            List<SelectListItem> objAccountType = new List<SelectListItem>();


            objAccountType.Add(new SelectListItem { Text = "Deposit", Value = "0" });
            objAccountType.Add(new SelectListItem { Text = "Loan", Value = "1" });

            return new SelectList(objAccountType, "Value", "Text");
        }
    }
}

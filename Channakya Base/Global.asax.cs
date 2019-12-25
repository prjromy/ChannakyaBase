using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ChannakyaBase.BLL.Repository;
using ChannakyaBase.DAL.DatabaseModel;

namespace Channakya_Base
{
    public class MvcApplication : System.Web.HttpApplication
    {
        //private static DateTime whenTaskLastRan;

        protected void Application_Start()
        {
            //whenTaskLastRan = DateTime.Now;

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        //public void Session_Start(object sender, EventArgs e)
        //{
        //    ChangeScheduledProductInterest();
        //}

        //public static void ChangeScheduledProductInterest()
        //{
        //    GenericUnitOfWork uow = new GenericUnitOfWork();
        //    var unupdatedInterestsList = uow.Repository<InterestLog>().GetAll().Where(x => x.EffectiveFrom >= DateTime.Today && x.isApplied == false);

        //    foreach (var item in unupdatedInterestsList)
        //    {
                
        //    }
        //}
    }
}

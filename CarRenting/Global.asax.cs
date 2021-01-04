using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CarRenting
{
    public class MvcApplication : System.Web.HttpApplication
    {

        protected void Application_Error(Object sender, EventArgs e)
        {

            var exception = Server.GetLastError();
            if (exception != null)
            {
                Server.TransferRequest("~/Error?Message=" + exception.Message);
            }

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}

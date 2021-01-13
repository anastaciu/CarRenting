using System.Web.Configuration;
using System.Web.Mvc;

namespace CarRenting.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                if (User.IsInRole(WebConfigurationManager.AppSettings["Cn"]))
                {
                    return RedirectToAction("Index", "CompanyAdminArea");
                }

                if (User.IsInRole(WebConfigurationManager.AppSettings["An"]))
                {
                    return RedirectToAction("Index", "AdminArea");
                }

                if (User.IsInRole(WebConfigurationManager.AppSettings["Cr"]))
                {
                    return RedirectToAction("Index", "CompanyUserArea");
                }

                if (User.IsInRole(WebConfigurationManager.AppSettings["Ur"]))
                {
                    return RedirectToAction("Index", "ClientArea");
                }
            }
            return View();
        }
    }
}
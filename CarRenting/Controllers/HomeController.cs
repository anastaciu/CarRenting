using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace CarRenting.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.IsInRole(WebConfigurationManager.AppSettings["Cn"]))
            {
                return RedirectToAction("Index", "CompanyArea");
            }
            return View();
        }
    }
}
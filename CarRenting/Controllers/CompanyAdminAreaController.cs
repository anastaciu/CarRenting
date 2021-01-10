using CarRenting.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarRenting.Controllers
{
    public class CompanyAdminAreaController : Controller
    {
        // GET: CompanyArea
        public ActionResult Index()
        {
            return View();
        }

    }

}
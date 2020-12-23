using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarRenting.Models;

namespace CarRenting.Controllers
{
    public class CompanyAreaController : Controller
    {
        // GET: CompanyArea
        public ActionResult Index()
        {

            CompanyViewModel companyViewModel = (CompanyViewModel)TempData["companyViewModel"];
            return View(companyViewModel);
        }
    }


}
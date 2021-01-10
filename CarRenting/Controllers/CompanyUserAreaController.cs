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
    public class CompanyUserAreaController : Controller
    {
        // GET: CompanyUserArea
        public ActionResult Index(bool? isDelivered)
        {
            ViewBag.IsDelivered = isDelivered != null;
            return View();
        }
    }
}
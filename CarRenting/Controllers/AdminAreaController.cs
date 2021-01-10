using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarRenting.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace CarRenting.Controllers
{
    public class AdminAreaController : Controller
    {
        // GET: AdminArea
        public ActionResult Index()
        {
            return View();
        }
    }
}
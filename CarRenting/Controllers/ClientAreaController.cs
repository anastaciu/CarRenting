using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarRenting.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace CarRenting.Controllers
{
    [Authorize(Roles = "Utilizador Registado")]
    public class ClientAreaController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}
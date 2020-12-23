using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarRenting.Controllers
{
    public class ClientAreaController : Controller
    {
        // GET: ClientArea
        public ActionResult Index()
        {
            return View();
        }
    }
}
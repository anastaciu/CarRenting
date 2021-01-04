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
            using (var db = new ApplicationDbContext())
            {
                var thisUser = db.Users.Find(User.Identity.GetUserId());
                if (thisUser != null)
                {
                    var role = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().GetRoles(thisUser.Id).SingleOrDefault();

                    var userViewModel = new DashViewModel()
                        { UserName = thisUser.Name, Role = role, CompanyName = null};

                    Session["UserInfo"] = userViewModel;
                    return View();
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
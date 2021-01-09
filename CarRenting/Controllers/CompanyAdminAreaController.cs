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
            using (var db = new ApplicationDbContext())
            {
                var thisUser = db.Users.Find(User.Identity.GetUserId());
                if (thisUser != null)
                {
                    try
                    {
                        var company = db.Companies.SingleOrDefault(c => c.Employees.Any(e => e.ApplicationUserId == thisUser.Id));
                        var role = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().GetRoles(thisUser.Id).SingleOrDefault();

                        var companyViewModel = new DashViewModel{ CompanyName = company?.CompanyName, UserName = thisUser.Name, Role = role };
                        Session["UserInfo"] = companyViewModel;
                        return View();

                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }

    }

}
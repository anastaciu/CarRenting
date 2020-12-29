using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarRenting.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;

namespace CarRenting.Controllers
{
    public class CompanyAreaController : Controller
    {
        // GET: CompanyArea
        public ActionResult Index()
        {
            using (var db = new ApplicationDbContext())
            {
                var thisUser = db.Users.Find(User.Identity.GetUserId());
                if (thisUser != null)
                {
                    var employee = db.Employees.SingleOrDefault(e => e.ApplicationUserId == thisUser.Id);
                    var company = db.Companies.SingleOrDefault(c => c.Id == employee.CompanyId);
                    var companyViewModel = new CompanyViewModel
                    { Company = company, ApplicationUser = thisUser, Employee = employee };
                    return View(companyViewModel);
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }


}
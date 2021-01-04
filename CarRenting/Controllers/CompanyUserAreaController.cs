﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarRenting.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace CarRenting.Controllers
{
    public class CompanyUserAreaController : Controller
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
                    var role = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>().GetRoles(thisUser.Id).SingleOrDefault();

                    var companyViewModel = new DashViewModel
                        { CompanyName = company?.CompanyName, UserName = thisUser.Name, Role = role };
                    return View(companyViewModel);
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
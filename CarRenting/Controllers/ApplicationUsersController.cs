using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using CarRenting.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace CarRenting.Controllers
{
    public class ApplicationUsersController : Controller
    {
        private ApplicationDbContext dbContext = new ApplicationDbContext();
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;


        public ApplicationUsersController()
        {
        }

        public ApplicationUsersController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }

        public ApplicationRoleManager RoleManager
        {
            get => _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            private set => _roleManager = value;
        }

        public async Task<ActionResult> Index()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(await dbContext.Users.ToListAsync());
            }
        }


        // GET: ApplicationUsers
        public ActionResult GetCompanyEmployees()
        {
            var userId = User.Identity.GetUserId();
            if (!IsCompanyAdmin(userId))
            {
                return RedirectToAction("Index", "Home");
            }
            var employee = dbContext.Employees.SingleOrDefault(e => e.ApplicationUserId == userId);
            var company = dbContext.Companies.SingleOrDefault(c => c.Id == employee.CompanyId);
            var employees = dbContext.Employees.Where(e => e.CompanyId == company.Id);
            var users = dbContext.Users.ToList();

            ICollection<EmployeeViewModel> empList = new List<EmployeeViewModel>();

            foreach (var applicationUser in users)
            {
                foreach (var dbEmployee in employees)
                {
                    if (applicationUser.Id == dbEmployee.ApplicationUserId)
                    {
                        var role = UserManager.GetRoles(applicationUser.Id).SingleOrDefault();
                        empList.Add(new EmployeeViewModel { ApplicationUser = applicationUser, Role = role });
                    }
                }
            }
            return View(empList);
        }




        // GET: ApplicationUsers/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = await Task.FromResult(dbContext.Users.Find(id));
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // GET: ApplicationUsers/Create
        public ActionResult Create()
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: ApplicationUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                dbContext.Users.Add(applicationUser);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(applicationUser);
        }

        // GET: ApplicationUsers/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ApplicationUser applicationUser = await Task.FromResult(dbContext.Users.Find(id));
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: ApplicationUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                dbContext.Entry(applicationUser).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(applicationUser);
        }

        // GET: ApplicationUsers/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = await Task.FromResult(dbContext.Users.Find(id));
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: ApplicationUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (!IsAdmin() && !IsCompanyAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            ApplicationUser applicationUser = await Task.FromResult(dbContext.Users.Find(id));
            dbContext.Users.Remove(applicationUser);
            await dbContext.SaveChangesAsync();
            if (IsAdmin())
            {
                return RedirectToAction("Index");
            }
            else if (IsCompanyAdmin())
            {
                return RedirectToAction("GetCompanyEmployees");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        private bool IsCompanyAdmin()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var role = UserManager.GetRoles(userId).SingleOrDefault();
                if (userId == null || role != WebConfigurationManager.AppSettings["Cn"])
                {
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Debug: " + e.Message);
                return false;
            }
        }

        private bool IsCompanyAdmin(string userId)
        {
            try
            {
                var role = UserManager.GetRoles(userId).SingleOrDefault();
                if (userId == null || role != WebConfigurationManager.AppSettings["Cn"])
                {
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Debug: " + e.Message);
                return false;
            }
        }

        private bool IsAdmin()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                var role = UserManager.GetRoles(userId).SingleOrDefault();
                if (userId == null || role != WebConfigurationManager.AppSettings["An"])
                {
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine("Debug: " + e.Message);
                return false;
            }
            
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

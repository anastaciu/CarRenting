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
        public ActionResult CompanyEmployees()
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
                        empList.Add(new EmployeeViewModel { ApplicationUser = applicationUser, RoleName = role });
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

        // GET: ApplicationUsers/Edit/5
        [Authorize(Roles = "Administrador da Empresa, Administrador do Site")]
        public ActionResult Edit(string id)
        {
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = dbContext.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }

            var roles = new SelectList(dbContext.Roles.Where(r => r.Name.Contains("Empresa")), "Name", "Name");
            ViewBag.Roles = roles;
            UserEditViewModel userEditViewModel = new UserEditViewModel
            {
                Id = applicationUser.Id,
                Name = applicationUser.Name,
                PhoneNumber = applicationUser.PhoneNumber,
                Role = null,
                Email = applicationUser.Email
            };
            return View(userEditViewModel);
        }

        // POST: ApplicationUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Administrador da Empresa, Administrador do Site")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Email,PhoneNumber,Role")] UserEditViewModel applicationUser)
        {
            var user = dbContext.Users.Find(applicationUser.Id);
            SelectList roles;
            if (user != null)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        var originalRole = user.Roles.ElementAt(0);
                        var role = RoleManager.Roles.SingleOrDefault(r => r.Id == originalRole.RoleId);

                        if (role?.Name != applicationUser.Role)
                        {
                            user.Roles.Remove(originalRole);
                            var result = UserManager.AddToRoles(applicationUser.Id, applicationUser.Role);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                        roles = new SelectList(dbContext.Roles.Where(r => r.Name.Contains("Empresa")), "Name",
                            "Name");
                        ViewBag.Roles = roles;
                        return View(applicationUser);
                    }

                    user.Id = applicationUser.Id;
                    user.PhoneNumber = applicationUser.PhoneNumber;
                    user.UserName = applicationUser.Email;
                    user.Name = applicationUser.Name;
                    user.Email = applicationUser.Email;
                    dbContext.Entry(user).State = EntityState.Modified;
                    dbContext.SaveChanges();
              
                    return RedirectToAction("CompanyEmployees");
                }
            }
            roles = new SelectList(dbContext.Roles.Where(r => r.Name.Contains("Empresa")), "Name", "Name");
            ViewBag.Roles = roles;
            return View(applicationUser);
        }

        // GET: ApplicationUsers/Delete/5
        [Authorize(Roles = "Administrador da Empresa, Administrador do Site")]
        public async Task<ActionResult> Delete(string id)
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

        // POST: ApplicationUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrador da Empresa, Administrador do Site")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {

            ApplicationUser applicationUser = await Task.FromResult(dbContext.Users.Find(id));
            dbContext.Users.Remove(applicationUser);
            await dbContext.SaveChangesAsync();
            if (IsAdmin())
            {
                return RedirectToAction("Index");
            }
            else if (IsCompanyAdmin())
            {
                return RedirectToAction("CompanyEmployees");
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

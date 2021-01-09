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
    [Authorize(Roles = "Administrador da Empresa, Administrador do Site")]
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
        [Authorize(Roles = "Administrador da Empresa")]
        public ActionResult CompanyEmployees(bool? isCreated)
        {
            var userId = User.Identity.GetUserId();
            var employees = dbContext.Companies.Include(c=>c.Employees).SingleOrDefault(c => c.Employees.Any(e => e.ApplicationUserId == userId))?.Employees; 
            var users = dbContext.Users.ToList();

            ICollection<UserViewModel> empList = new List<UserViewModel>();

            if (employees != null)
            {
                foreach (var employee in employees)
                {
                    {
                        var role = UserManager.GetRoles(employee.ApplicationUser.Id).SingleOrDefault() ?? string.Empty;
                        empList.Add(new UserViewModel
                        {
                            Name = employee.ApplicationUser.Name, Id = employee.ApplicationUserId,
                            Email = employee.ApplicationUser.Email, PhoneNumber = employee.ApplicationUser.PhoneNumber,
                            Role = role
                        });
                    }
                }
            }
            ViewBag.IsCreated = isCreated != null;
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
            UserViewModel userViewModel = new UserViewModel
            {
                Id = applicationUser.Id,
                Name = applicationUser.Name,
                PhoneNumber = applicationUser.PhoneNumber,
                Role = null,
                Email = applicationUser.Email
            };
            return View(userViewModel);
        }

        // POST: ApplicationUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Email,PhoneNumber,Role")] UserViewModel applicationUser)
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

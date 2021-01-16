using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly ApplicationDbContext _dbContext = new ApplicationDbContext();
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

        private ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            set => _userManager = value;
        }

        private ApplicationRoleManager RoleManager
        {
            get => _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            set => _roleManager = value;
        }

        [Authorize(Roles = "Administrador do Site")]
        public async Task<ActionResult> Index()
        {
            var users = await _dbContext.Users.Include(u=>u.Roles).ToListAsync();
            var userList = new List<UserViewModel>();
            foreach (var user in users)
            {
                var roleId =  user.Roles.SingleOrDefault()?.RoleId;
                var role = await RoleManager.FindByIdAsync(roleId);
                
                userList.Add(new UserViewModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = role.Name
                });
            }
            return View(userList);
        }

        // GET: ApplicationUsers
        [Authorize(Roles = "Administrador da Empresa")]
        public ActionResult CompanyEmployees()
        {
            var userId = User.Identity.GetUserId();
            var employees = _dbContext.Companies.Include(c => c.Employees).SingleOrDefault(c => c.Employees.Any(e => e.ApplicationUserId == userId))?.Employees;
            var users = _dbContext.Users.ToList();
            if (users == null)
            {
                throw new HttpException(404, NoUserFound());
            }

            ICollection<UserViewModel> empList = new List<UserViewModel>();

            if (employees != null)
            {
                foreach (var employee in employees)
                {
                    {
                        var role = UserManager.GetRoles(employee.ApplicationUser.Id).SingleOrDefault() ?? string.Empty;
                        empList.Add(new UserViewModel
                        {
                            Name = employee.ApplicationUser.Name,
                            Id = employee.ApplicationUserId,
                            Email = employee.ApplicationUser.Email,
                            PhoneNumber = employee.ApplicationUser.PhoneNumber,
                            Role = role
                        });
                    }
                }
            }
            return View(empList);
        }

        [Authorize(Roles = "Administrador da Empresa, Administrador do Site")]
        // GET: ApplicationUsers/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException(NoUser(), nameof(id));
            
            ApplicationUser applicationUser =_dbContext.Users.Find(id);

            if (applicationUser == null)
            {
                throw new HttpException(404, NoUserFound());
            }

            var roleId = applicationUser.Roles.SingleOrDefault()?.RoleId;
            var company = await _dbContext.Companies.SingleOrDefaultAsync(c => c.Employees.Any(e => e.ApplicationUserId == applicationUser.Id));
            var role = await RoleManager.FindByIdAsync(roleId);
            var userViewModel = new UserViewModel
            {
                Id = applicationUser.Id,
                Email = applicationUser.Email,
                Name = applicationUser.Name,
                Role = role.Name,
                PhoneNumber = applicationUser.PhoneNumber,
                CompanyName = company?.CompanyName
                
            };

            return View(userViewModel);
        }

        [Authorize(Roles = "Administrador da Empresa, Administrador do Site, Utilizador Registado, Utilizador da Empresa")]
        // GET: ApplicationUsers/Details/5
        public async Task<ActionResult> ManageUserDetails()
        {
            var userId = User.Identity.GetUserId();
            ApplicationUser applicationUser = _dbContext.Users.Find(userId);

            if (applicationUser == null)
            {
                throw new HttpException(404, NoUserFound());
            }

            var roleId = applicationUser.Roles.SingleOrDefault()?.RoleId;
            var company = await _dbContext.Companies.SingleOrDefaultAsync(c => c.Employees.Any(e => e.ApplicationUserId == applicationUser.Id));
            var role = await RoleManager.FindByIdAsync(roleId);
            var userViewModel = new UserViewModel
            {
                Id = applicationUser.Id,
                Email = applicationUser.Email,
                Name = applicationUser.Name,
                Role = role.Name,
                PhoneNumber = applicationUser.PhoneNumber,
                CompanyName = company?.CompanyName

            };

            return View(userViewModel);
        }


        // GET: ApplicationUsers/Edit/5
        [Authorize(Roles = "Administrador da Empresa, Administrador do Site")]
        public async Task<ActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException(NoUser(), nameof(id));
            
            ApplicationUser applicationUser = _dbContext.Users.Find(id);

            if (applicationUser == null)
            {
                throw new HttpException(404, NoUserFound());
            }
            ViewBag.Roles = new SelectList(_dbContext.Roles.Where(r => r.Name.Contains("Empresa")), "Name", "Name");
            string currentRoleName = null;
            try
            {
                var currentRole = await RoleManager.FindByIdAsync(applicationUser.Roles.SingleOrDefault()?.RoleId);
                currentRoleName = currentRole.Name;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            UserViewModel userViewModel = new UserViewModel
            {
                Id = applicationUser.Id,
                Name = applicationUser.Name,
                PhoneNumber = applicationUser.PhoneNumber,
                Role = currentRoleName,
                Email = applicationUser.Email
            };
            return View(await Task.FromResult(userViewModel));
        }

        // POST: ApplicationUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrador da Empresa, Administrador do Site")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Email,PhoneNumber,Role")] UserViewModel applicationUser)
        {
            var user = _dbContext.Users.Find(applicationUser.Id);
            SelectList roles;
            if (user != null)
            {
                if (ModelState.IsValid)
                {
                    if (_dbContext.Users.Any(u => u.Email == applicationUser.Email))
                    {
                        ModelState.AddModelError(nameof(applicationUser.Email), @"O email já está registado");
                        roles = new SelectList(_dbContext.Roles.Where(r => r.Name.Contains("Empresa")), "Name", "Name");
                        ViewBag.Roles = roles;
                        return View(applicationUser);
                    }
                    try
                    {
                        var originalRole = user.Roles.SingleOrDefault();
                        var role = await RoleManager.FindByIdAsync(originalRole?.RoleId);

                        if (role.Name != applicationUser.Role)
                        {
                            user.Roles.Remove(originalRole);
                            UserManager.AddToRoles(applicationUser.Id, applicationUser.Role);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                        roles = new SelectList(_dbContext.Roles.Where(r => r.Name.Contains("Empresa")), "Name",
                            "Name");
                        ViewBag.Roles = roles;
                        return View(applicationUser);
                    }

                    user.Id = applicationUser.Id;
                    user.PhoneNumber = applicationUser.PhoneNumber;
                    user.Email = applicationUser.Email;
                    user.UserName = applicationUser.Email;
                    user.Name = applicationUser.Name;
                    _dbContext.Entry(user).State = EntityState.Modified;
                    await _dbContext.SaveChangesAsync();
                    TempData["isChanged"] = true;
                    if (User.IsInRole(WebConfigurationManager.AppSettings["Cn"]))
                    {
                        return RedirectToAction("CompanyEmployees");
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            roles = new SelectList(_dbContext.Roles.Where(r => r.Name.Contains("Empresa")), "Name", "Name");
            ViewBag.Roles = roles;
            return View(applicationUser);
        }

        // GET: ApplicationUsers/Delete/5
        [Authorize(Roles = "Administrador da Empresa")]
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                throw new HttpException(400, NoUser());
            }
            ApplicationUser applicationUser = await Task.FromResult(_dbContext.Users.Find(id));
            if (applicationUser == null)
            {
                throw new HttpException(404, NoUserFound());
            }
            return View(applicationUser);
        }

        // POST: ApplicationUsers/Delete/5
        [Authorize(Roles = "Administrador da Empresa")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {

            ApplicationUser applicationUser = await Task.FromResult(_dbContext.Users.Find(id));

            _dbContext.Users.Remove(applicationUser);
            await _dbContext.SaveChangesAsync();
            TempData["deleted"] = true;
            return RedirectToAction("CompanyEmployees");

        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
            base.Dispose(disposing);
        }

        private string NoUserFound()
        {
            return @"O utilizador não existe ou foi encontrado";
        }

        private string NoUser()
        {
            return @"É necessário indicar um utilizador";
        }
    }
}

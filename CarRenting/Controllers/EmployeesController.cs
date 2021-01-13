using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CarRenting.Models;
using Microsoft.AspNet.Identity;

namespace CarRenting.Controllers
{
    public class EmployeesController : Controller
    {
        private ApplicationDbContext dbContext = new ApplicationDbContext();

        // GET: Employees
        public async Task<ActionResult> Index()
        {
            var employeeId = User.Identity.GetUserId();
            var employee = dbContext.Employees.SingleOrDefault(e => e.ApplicationUserId == employeeId);
            var company = dbContext.Companies.SingleOrDefault(e => e.Id == employee.CompanyId);
            var list = await Task.FromResult(company?.Employees);
            return View(list);
        }

        // GET: Employees/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                throw new HttpException(400, NoUser());

            }
            Employee employee = await dbContext.Employees.FindAsync(id);
            if (employee == null)
            {
                throw new HttpException(404, NoUserFound());

            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id")] Employee employee)
        {

            if (ModelState.IsValid)
            {
                dbContext.Employees.Add(employee);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                throw new HttpException(400, NoUser());

            }
            Employee employee = await dbContext.Employees.FindAsync(id);
            if (employee == null)
            {
                throw new HttpException(404, NoUserFound());

            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                dbContext.Entry(employee).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                throw new HttpException(400, NoUser());

            }
            Employee employee = await dbContext.Employees.FindAsync(id);
            if (employee == null)
            {
                throw new HttpException(404, NoUserFound());

            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                throw new HttpException(400, NoUser());
            }
            Employee employee = await dbContext.Employees.FindAsync(id);
            if (employee == null)
            {
                throw new HttpException(404, NoUserFound());
            }
            dbContext.Employees.Remove(employee);
            await dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbContext.Dispose();
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

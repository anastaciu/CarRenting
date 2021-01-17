using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CarRenting.Models;

namespace CarRenting.Controllers
{
    [Authorize(Roles = "Administrador do Site")]
    public class CompaniesController : Controller
    {
        private readonly ApplicationDbContext  _dbContext = new ApplicationDbContext();

        // GET: Companies
        public async Task<ActionResult> Index()
        {
            return View(await _dbContext.Companies.ToListAsync());
        }

        // GET: Companies/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                throw new HttpException(400, NoCompany());

            }
            Company company = await _dbContext.Companies.FindAsync(id);
            if (company == null)
            {
                throw new HttpException(404, NoCompanyFound());

            }
            return View(company);
        }

        // GET: Companies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,CompanyName,CompanyPhoneNumber,CompanyCellNumber")] Company company)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Companies.Add(company);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(company);
        }

        // GET: Companies/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                throw new HttpException(400, NoCompany());

            }
            Company company = await _dbContext.Companies.FindAsync(id);
            if (company == null)
            {
                throw new HttpException(404, NoCompanyFound());

            }
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Nif,Email,CompanyName,CompanyPhoneNumber,CompanyCellNumber")] Company company)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Entry(company).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                TempData["companyEdited"] = true;
                return RedirectToAction("Index");
            }
            return View(company);
        }

        // GET: Companies/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                throw new HttpException(400, NoCompany());

            }
            Company company = await _dbContext.Companies.FindAsync(id);
            if (company == null)
            {
                throw new HttpException(404, NoCompanyFound());

            }
            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var company = await _dbContext.Companies.FindAsync(id);
            var employees = _dbContext.Employees.Include(e=>e.ApplicationUser).Where(e => e.CompanyId == id);
            foreach (var employee in employees)
            {
                _dbContext.Users.Remove(employee.ApplicationUser);
            }
            _dbContext.Companies.Remove(company ?? throw new HttpException(400, NoCompanyFound()));

            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
            base.Dispose(disposing);
        }

        private string NoCompanyFound()
        {
            return @"A empresa não existe ou foi encontrada";
        }

        private string NoCompany()
        {
            return @"É necessário indicar uma empresa";
        }
    }
}

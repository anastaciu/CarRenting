using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarRenting.Models;
using Microsoft.AspNet.Identity;

namespace CarRenting.Controllers
{
    public class ChecksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Checks
        public async Task<ActionResult> Index()
        {
            return View(await db.Checks.Include(c => c.CarTypes).ToListAsync());
        }

        // GET: Checks/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Check check = await db.Checks.FindAsync(id);
            if (check == null)
            {
                return HttpNotFound();
            }
            return View(check);
        }

        // GET: Checks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Checks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Description")] Check check)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var company = db.Companies.SingleOrDefault(c => c.Employees.Any(e => e.ApplicationUserId == userId));
                check.Company = company;
                db.Checks.Add(check);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(check);
        }


        public async Task<ActionResult> AssociateCarType(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var carTypes = db.CarTypes.ToList();
            Check check = await db.Checks.FindAsync(id);
            if (check == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<CheckBoxes> checkBoxes= new List<CheckBoxes>();
            foreach (var carType in carTypes)
            {
                checkBoxes.Add(new CheckBoxes
                {
                    Id = carType.Id,
                    Type = carType.Type
                });
            }

            var associateViewModel = new AssociateViewModel
            {
                Id = check.Id,
                Description = check.Description,
                CarTypes = checkBoxes
            };

            return View(associateViewModel);
        }

        // POST: Checks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AssociateCarType(AssociateViewModel associateViewModel)
        {
            if (ModelState.IsValid)
            {
                var check = await db.Checks.FindAsync(associateViewModel.Id);
                if (check == null)
                {
                    return HttpNotFound();
                }
                foreach (var carType in associateViewModel.CarTypes)
                {
                    if (carType.Add)
                    {
                        var type = await db.CarTypes.FindAsync(carType.Id);
                        check.CarTypes.Add(type);
                        Debug.WriteLine("Erro" + associateViewModel.Id);
                    }
                    else
                    {
                        var type = await db.CarTypes.FindAsync(carType.Id);
                        check.CarTypes.Remove(type);
                    }
                }
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(associateViewModel);
        }

        // GET: Checks/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Check check = await db.Checks.FindAsync(id);
            if (check == null)
            {
                return HttpNotFound();
            }
            return View(check);
        }

        // POST: Checks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Description")] Check check)
        {
            if (ModelState.IsValid)
            {
                db.Entry(check).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(check);
        }

        // GET: Checks/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Check check = await db.Checks.FindAsync(id);
            if (check == null)
            {
                return HttpNotFound();
            }
            return View(check);
        }

        // POST: Checks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Check check = await db.Checks.FindAsync(id);
            db.Checks.Remove(check);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

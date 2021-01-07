using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarRenting.Models;
using Microsoft.AspNet.Identity;

namespace CarRenting.Controllers
{
    public class CarsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Cars
        public async Task<ActionResult> Index()
        {
            if (Request.IsAuthenticated)
            {
                RedirectToAction("Index", "Home");
            }

            var cars = db.Cars.Include(c => c.Company).Include(c => c.Type);
            return View(await cars.ToListAsync());
        }

        [Authorize(Roles = "Administrador da Empresa")]
        public async Task<ActionResult> CompanyCars()
        {
            var userId = User.Identity.GetUserId();
            var employee = db.Employees.Include(e=>e.Company).SingleOrDefault(e => e.ApplicationUserId == userId);
            var carsWithType = db.Cars.Where(c=>c.CompanyId == employee.CompanyId).Include(c=>c.Type);
            return View(await carsWithType.ToListAsync());
        }
        // GET: Cars/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }
            Car car = await db.Cars.Include(c=>c.Type).Include(c=>c.Company).SingleOrDefaultAsync(c=>c.Id == id);

            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        [Authorize(Roles = "Administrador da Empresa")]
        public ActionResult Create()
        {
            List<SelectListItem> fuelList = new List<SelectListItem>
            {
                new SelectListItem { Value = "Vazio", Text = "Vazio" },
                new SelectListItem { Value = "Meio", Text = "Meio" },
                new SelectListItem { Value = "Cheio", Text = "Cheio" }
            };
            ViewBag.FuelLevels = fuelList;
            ViewBag.TypeId = new SelectList(db.CarTypes, "Id", "Type");
            
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador da Empresa")]
        public async Task<ActionResult> Create([Bind(Include = "Id,License,Brand,Model,Fuel,FuelLevel,Seats,TypeId,Price,Kms")] Car car)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var employee = db.Employees.Include(e=>e.Company).SingleOrDefault(e => e.ApplicationUserId == userId);
                if (employee != null)
                {
                    car.Company = employee.Company;
                    db.Cars.Add(car);
                    await db.SaveChangesAsync();
                    return RedirectToAction("CompanyCars");
                }
            }
            var fuelList = new List<SelectListItem>
            {
                new SelectListItem() { Value = "Vazio", Text = "Vazio" },
                new SelectListItem() { Value = "Meio", Text = "Meio" },
                new SelectListItem() { Value = "Cheio", Text = "Cheio" }
            };
            ViewBag.FuelLevels = fuelList;
            ViewBag.TypeId = new SelectList(db.CarTypes, "Id", "Type", car.TypeId);
            return View(car);
        }

        // GET: Cars/Edit/5
        [Authorize(Roles = "Administrador da Empresa")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = await db.Cars.FindAsync(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            var fuelList = new List<SelectListItem>
            {
                new SelectListItem() { Value = "Vazio", Text = "Vazio" },
                new SelectListItem() { Value = "Meio", Text = "Meio" },
                new SelectListItem() { Value = "Cheio", Text = "Cheio" }
            };
            ViewBag.FuelLevels = fuelList;
            ViewBag.TypeId = new SelectList(db.CarTypes, "Id", "Type", car.TypeId);

            return View(car);
        }
        
        // POST: Cars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Administrador da Empresa")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Brand,Model,Fuel,FuelLevel,Seats,TypeId,Price,Kms,CompanyId")] Car car)
        {
            if (ModelState.IsValid)
            {
                db.Entry(car).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CompanyId = new SelectList(db.Companies, "Id", "CompanyName", car.CompanyId);
            ViewBag.TypeId = new SelectList(db.CarTypes, "Id", "Type", car.TypeId);
            return View(car);
        }


        // GET: Cars/Delete/5
        [Authorize(Roles = "Administrador da Empresa")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = await db.Cars.FindAsync(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrador da Empresa")]
        [ValidateAntiForgeryToken]
        
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Car car = await db.Cars.FindAsync(id);
            db.Cars.Remove(car);
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

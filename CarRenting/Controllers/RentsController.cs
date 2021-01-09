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
    public class RentsController : Controller
    {
        private readonly ApplicationDbContext _dbContext = new ApplicationDbContext();

        // GET: Rents
        [Authorize(Roles = "Empregado da Empresa")]
        public async Task<ActionResult> Index(bool? isConfirmed)
        {
            var userId = User.Identity.GetUserId();
            var companyRents = _dbContext.Rents.Where(r => r.Car.Company.Employees.Any(e => e.ApplicationUserId == userId)).Include(r=>r.Car).Include(r=>r.ApplicationUser);
            ViewBag.IsConfirmed = isConfirmed != null;
            return View(await companyRents.ToListAsync());
        }

        // GET: Rents
        [Authorize(Roles = "Utilizador Registado")]
        public async Task<ActionResult> UserRents()
        {
            var userId = User.Identity.GetUserId();
            var rents = _dbContext.Rents.Include(r => r.ApplicationUser).Include(r => r.Car).Where(r => r.ApplicationUserId == userId);
            return View(await rents.ToListAsync());
        }

        [Authorize(Roles = "Utilizador Registado")]
        public async Task<ActionResult> RentVehicle(int carId)
        {
            var userId = User.Identity.GetUserId();
            Rent rent = new Rent {CarId = carId, ApplicationUserId = userId};
            return View(await Task.FromResult(rent));
        }

        // POST: Rents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Utilizador Registado")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RentVehicle([Bind(Include = "Id,Begin,End,ApplicationUserId,CarId")] Rent rent)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Rents.Add(rent);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ApplicationUserId = new SelectList(_dbContext.Users, "Id", "Name", rent.ApplicationUserId);
            ViewBag.CarId = new SelectList(_dbContext.Cars, "Id", "License", rent.CarId);
            return View(rent);
        }

        [Authorize(Roles = "Empregado da Empresa")]
        public ActionResult ConfirmRent(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var rent = _dbContext.Rents.Find(id);
            if (rent == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                rent.IsConfirmed = true;
                _dbContext.Entry(rent).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
            return RedirectToAction("Index", new {isConfirmed = true});
        }


        //[Authorize(Roles = "Empregado da Empresa")]
        //public async Task<ActionResult> DeliverVehicle(int rentId)
        //{

        //}

        //[Authorize(Roles = "Empregado da Empresa")]
        //public async Task<ActionResult> ReceiveVehicle(int rentId)
        //{

        //}

        // GET: Rents/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rent rent = await _dbContext.Rents.FindAsync(id);
            if (rent == null)
            {
                return HttpNotFound();
            }

            return View(rent);
        }

        // GET: Rents/Create
        public ActionResult Create()
        {
            ViewBag.ApplicationUserId = new SelectList(_dbContext.Users, "Id", "Name");
            ViewBag.CarId = new SelectList(_dbContext.Cars, "Id", "License");
            return View();
        }

        // POST: Rents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Begin,End,ApplicationUserId,CarId,IsConfirmed,IsDelivered,IsReceived,IsChecked,DeliveryFaults,KmsIn,KmsOut,IsDamaged")] Rent rent)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Rents.Add(rent);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ApplicationUserId = new SelectList(_dbContext.Users, "Id", "Name", rent.ApplicationUserId);
            ViewBag.CarId = new SelectList(_dbContext.Cars, "Id", "License", rent.CarId);
            return View(rent);
        }

        // GET: Rents/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rent rent = await _dbContext.Rents.FindAsync(id);
            if (rent == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApplicationUserId = new SelectList(_dbContext.Users, "Id", "Name", rent.ApplicationUserId);
            ViewBag.CarId = new SelectList(_dbContext.Cars, "Id", "License", rent.CarId);
            return View(rent);
        }

        // POST: Rents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Begin,End,ApplicationUserId,CarId,IsConfirmed,IsDelivered,IsReceived,IsChecked,DeliveryFaults,KmsIn,KmsOut,IsDamaged")] Rent rent)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Entry(rent).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ApplicationUserId = new SelectList(_dbContext.Users, "Id", "Name", rent.ApplicationUserId);
            ViewBag.CarId = new SelectList(_dbContext.Cars, "Id", "License", rent.CarId);
            return View(rent);
        }

        // GET: Rents/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rent rent = await _dbContext.Rents.FindAsync(id);
            if (rent == null)
            {
                return HttpNotFound();
            }
            return View(rent);
        }

        // POST: Rents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Rent rent = await _dbContext.Rents.FindAsync(id);
            _dbContext.Rents.Remove(rent);
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
    }
}

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
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Rents
        [Authorize(Roles = "Utilizador Registado")]
        public async Task<ActionResult> Index()
        {
            var userId = User.Identity.GetUserId();
            var rents = db.Rents.Include(r => r.ApplicationUser).Include(r => r.Car).Where(r=>r.ApplicationUserId == userId);
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
                db.Rents.Add(rent);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Name", rent.ApplicationUserId);
            ViewBag.CarId = new SelectList(db.Cars, "Id", "License", rent.CarId);
            return View(rent);
        }


        // GET: Rents/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rent rent = await db.Rents.FindAsync(id);
            if (rent == null)
            {
                return HttpNotFound();
            }
            return View(rent);
        }

        // GET: Rents/Create
        public ActionResult Create()
        {
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Name");
            ViewBag.CarId = new SelectList(db.Cars, "Id", "License");
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
                db.Rents.Add(rent);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Name", rent.ApplicationUserId);
            ViewBag.CarId = new SelectList(db.Cars, "Id", "License", rent.CarId);
            return View(rent);
        }

        // GET: Rents/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rent rent = await db.Rents.FindAsync(id);
            if (rent == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Name", rent.ApplicationUserId);
            ViewBag.CarId = new SelectList(db.Cars, "Id", "License", rent.CarId);
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
                db.Entry(rent).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Name", rent.ApplicationUserId);
            ViewBag.CarId = new SelectList(db.Cars, "Id", "License", rent.CarId);
            return View(rent);
        }

        // GET: Rents/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rent rent = await db.Rents.FindAsync(id);
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
            Rent rent = await db.Rents.FindAsync(id);
            db.Rents.Remove(rent);
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

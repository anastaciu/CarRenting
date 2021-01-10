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
        public async Task<ActionResult> ConfirmRent(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var rent = await _dbContext.Rents.FindAsync(id);
            if (rent == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                rent.IsConfirmed = true;
                _dbContext.Entry(rent).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction("Index", new {isConfirmed = true});
        }


        // GET: Rents
        [Authorize(Roles = "Empregado da Empresa")]
        public async Task<ActionResult> ListForDelivery(bool? isDelivered)
        {
            var userId = User.Identity.GetUserId();
            var companyRents = _dbContext.Rents.Where(r => r.Car.Company.Employees.Any(e => e.ApplicationUserId == userId)).Include(r => r.Car).Include(r => r.ApplicationUser);
            ViewBag.IsConfirmed = isDelivered != null;
            return View(await companyRents.ToListAsync());
        }

        [Authorize(Roles = "Empregado da Empresa")]
        public async Task<ActionResult> DeliverVehicle(int? id)
        {
            var rent = await _dbContext.Rents.FindAsync(id);
            if (rent == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<SelectListItem> fuelList = new List<SelectListItem>
            {
                new SelectListItem { Value = "Vazio", Text = "Vazio" },
                new SelectListItem { Value = "Meio", Text = "Meio Vazio"},
                new SelectListItem { Value = "Cheio", Text = "Meio" },
                new SelectListItem { Value = "Meio", Text = "Meio Cheio"},
                new SelectListItem { Value = "Cheio", Text = "Cheio" }
            };
            ViewBag.FuelLevels = fuelList;
            var deliveryModel = new DeliveryViewModel
            {
                Id = rent.Id,
                DeliveryFaults = rent.DeliveryFaults,
                KmsOut = rent.KmsOut
            };

            return View(deliveryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Empregado da Empresa")]
        public async Task<ActionResult> DeliverVehicle([Bind(Include = "Id,DeliveryFaults,KmsOut,FuelLevel")] DeliveryViewModel deliveryModel)
        {
            if (ModelState.IsValid)
            {
                var rent = await _dbContext.Rents.FindAsync(deliveryModel.Id);
                if (rent == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                rent.DeliveryFaults = deliveryModel.DeliveryFaults;
                rent.KmsOut = deliveryModel.KmsOut;
                rent.IsDelivered = true;
                _dbContext.Entry(rent).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                return RedirectToAction("Index", "CompanyUserArea", new {isDelivered = true});
            }

            return View();
        }

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

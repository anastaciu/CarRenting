using System;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CarRenting.Models;
using CarRenting.Utils;
using Microsoft.AspNet.Identity;


namespace CarRenting.Controllers
{
    public class RentsController : Controller
    {
        private readonly ApplicationDbContext _dbContext = new ApplicationDbContext();

        // GET: Rents
        [Authorize(Roles = "Utilizador da Empresa")]
        public async Task<ActionResult> Index()
        {
            var userId = User.Identity.GetUserId();
            var companyRents = _dbContext.Rents.Where(r => r.Car.Company.Employees.Any(e => e.ApplicationUserId == userId)).Include(r => r.Car).Include(r => r.ApplicationUser);
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
        public async Task<ActionResult> RentVehicle(int? carId)
        {
            if (carId == null)
            {
                throw new HttpException(400, NoRent());
            }
            var userId = User.Identity.GetUserId();
            Rent rent = new Rent { CarId = (int)carId, ApplicationUserId = userId };
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
                if (rent.End.Day < rent.Begin.Day)
                {
                    ModelState.AddModelError(nameof(rent.End), @"Data de fim não pode ser inferior à data de início");
                    return View(rent);
                }

                var car = _dbContext.Cars.Include(c => c.Rents).SingleOrDefault(c => c.Id == rent.CarId);
                if (car == null)
                {
                    throw new HttpException(404, @"O veículo não existe ou não foi encontrado");
                }
                var dateAvailable = car.Rents.Any(r => rent.Begin >= r.Begin && rent.Begin <= r.End);

                if (!dateAvailable)
                {
                    _dbContext.Rents.Add(rent);
                    await _dbContext.SaveChangesAsync();
                    TempData["rentSuccess"] = true;
                    return RedirectToAction("UserRents");
                }

                var unavailableCars = _dbContext.Cars.Where(c =>
                      c.Rents.Any(r => !(rent.End < r.Begin || rent.Begin > r.End)));
                var carList = _dbContext.Cars.Where(c=>c.TypeId == car.TypeId).Include(c=>c.Type).Include(c=>c.CarImages).Include(c=>c.Company).Except(unavailableCars);
                TempData["newCarList"] = true;
                TempData["begin"] = rent.Begin;
                TempData["end"] = rent.End;
                return View("~/Views/Cars/Index.cshtml", carList);

            }
            return View(rent);
        }


        public async Task<ActionResult> RentValidDate([Bind(Include = "Id,Begin,End,ApplicationUserId,CarId")] Rent rent)
        {
            if (ModelState.IsValid)
            {
                var car = _dbContext.Cars.Include(c => c.Rents).SingleOrDefault(c => c.Id == rent.CarId);
                if (car == null)
                {
                    throw new HttpException(404, @"O veículo não existe ou não foi encontrado");
                }
                
                _dbContext.Rents.Add(rent);
                await _dbContext.SaveChangesAsync();
                TempData["rentSuccess"] = true;
                return RedirectToAction("UserRents");
            }
            return RedirectToAction("UserRents");
        }

        [Authorize(Roles = "Utilizador da Empresa")]
        public async Task<ActionResult> ConfirmRent(int? id)
        {
            if (id == null)
            {
                throw new HttpException(400, NoRent());
            }
            var rent = await _dbContext.Rents.Include(r=>r.Car).Include(r=>r.ApplicationUser).Include(r=>r.Car.Type).SingleOrDefaultAsync(r=>r.Id == id);
            if (rent == null)
            {
                throw new HttpException(404, NoRentFound());
            }
            return View(rent);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Utilizador da Empresa")]
        public async Task<ActionResult> ConfirmRent([Bind(Include = "Id,Begin,End,ApplicationUserId,CarId,IsConfirmed,IsDelivered,IsReceived,IsChecked,DeliveryFaults,KmsIn,KmsOut,IsDamaged")] Rent rent)
        {

            if (ModelState.IsValid)
            {
                rent.IsConfirmed = true;
                _dbContext.Entry(rent).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                TempData["isConfirmed"] = true;
                return RedirectToAction("Index");
            }
            TempData["isConfirmed"] = false;
            return RedirectToAction("Index");
        }



        // GET: Rents
        [Authorize(Roles = "Utilizador da Empresa")]
        public async Task<ActionResult> ListForDelivery()
        {
            var userId = User.Identity.GetUserId();
            var companyRents = _dbContext.Rents.Where(r => r.Car.Company.Employees.Any(e => e.ApplicationUserId == userId)).Include(r => r.Car).Include(r => r.ApplicationUser);
            return View(await companyRents.ToListAsync());
        }

        [Authorize(Roles = "Utilizador da Empresa")]
        public async Task<ActionResult> DeliverVehicle(int? id)
        {
            if (id == null)
            {
                throw new HttpException(400, NoRent());
            }
            var rent = await _dbContext.Rents.Include(r => r.Car).SingleOrDefaultAsync(r => r.Id == id);
            if (rent == null)
            {
                throw new HttpException(404, NoRentFound());
            }
            var fuelList = SelectLists.FuelLevelList();
            ViewBag.FuelLevels = fuelList;
            var deliveryModel = new DeliveryViewModel
            {
                Id = rent.Id,
                DeliveryFaults = rent.DeliveryFaults,
                KmsOut = rent.Car.Kms,
                FuelLevel = rent.Car.FuelLevel
            };

            return View(deliveryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Utilizador da Empresa")]
        public async Task<ActionResult> DeliverVehicle(DeliveryViewModel deliveryModel)
        {
            if (ModelState.IsValid)
            {
                var rent = await _dbContext.Rents.Include(r => r.Car).SingleOrDefaultAsync(r => r.Id == deliveryModel.Id);
                if (rent == null)
                {
                    throw new HttpException(404, NoRentFound());
                }
                rent.DeliveryFaults = deliveryModel.DeliveryFaults;
                rent.KmsOut = deliveryModel.KmsOut;
                rent.IsDelivered = true;
                _dbContext.Entry(rent).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                TempData["isDelivered"] = true;
                return RedirectToAction("Index", "CompanyUserArea");
            }
            var fuelList = SelectLists.FuelLevelList();
            ViewBag.FuelLevels = fuelList;
            return View(deliveryModel);
        }

        //public async Task<ActionResult> SaveVehicleDetails(int? id)
        //{

        //}


        [Authorize(Roles = "Utilizador da Empresa")]
        public async Task<ActionResult> ReceiveVehicle(int? id)
        {
            if (id == null)
            {
                throw new HttpException(400, NoRent());
            }
            var rent = await _dbContext.Rents.Include(r => r.Car.Type.Checks).SingleOrDefaultAsync(r => r.Id == id);
            if (rent == null)
            {
                throw new HttpException(404, NoRentFound());
            }
            var checks = rent.Car.Type.Checks.Where(c => c.CompanyId == rent.Car.CompanyId);
            var fuelList = SelectLists.FuelLevelList();
            ViewBag.FuelLevels = fuelList;
            
            var receptionViewModel = new ReceptionViewModel
            {
                Id = rent.Id,
                KmsIn = rent.Car.Kms,
                CarBrand = rent.Car.Brand,
                CarLicense = rent.Car.License,
                CarModel = rent.Car.Model,
                Checks = checks.ToList()
                

            };
            return View(receptionViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Utilizador da Empresa")]
        public async Task<ActionResult> ReceiveVehicle(ReceptionViewModel receptionViewModel)
        {
            if (ModelState.IsValid)
            {
                var rent = await _dbContext.Rents.FindAsync(receptionViewModel.Id);
                if (rent == null)
                {
                    throw new HttpException(404, NoRentFound());
                }
                rent.IsChecked = receptionViewModel.IsChecked;
                rent.KmsIn = receptionViewModel.KmsIn;
                rent.IsReceived = true;
                rent.IsDamaged = receptionViewModel.IsDamaged;

                foreach (HttpPostedFileBase file in receptionViewModel.Files)
                {
                    if (file != null)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var relativePath = "~/DamageImagesUpload/" + fileName;
                        var path = Path.Combine(Server.MapPath("~/DamageImagesUpload/") + fileName);
                        file.SaveAs(path);
                        if (!_dbContext.DamageImages.Any(d => d.ImagePath == fileName))
                        {
                            _dbContext.DamageImages.Add(new DamageImage
                            { ImagePath = relativePath, RentId = receptionViewModel.Id });
                        }
                    }
                }

                _dbContext.Entry(rent).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                TempData["isReceived"] = true;
                return RedirectToAction("Index", "CompanyUserArea");
            }

            var fuelList = SelectLists.FuelLevelList();
            ViewBag.FuelLevels = fuelList;
            return View(receptionViewModel);
        }
        [Authorize(Roles = "Utilizador Registado")]
        public async Task<ActionResult> RentVehicleValidDate(int carId, DateTime begin, DateTime end)
        {
            if (ModelState.IsValid)
            {
                var car = _dbContext.Cars.Include(c => c.Rents).SingleOrDefault(c => c.Id ==  carId);
                if (car == null)
                {
                    throw new HttpException(404, @"O veículo não existe ou não foi encontrado");
                }

                var rent = new Rent
                {
                    CarId = carId,
                    Begin = begin,
                    End =  end,
                    ApplicationUserId = User.Identity.GetUserId()
                };
                _dbContext.Rents.Add(rent);
                await _dbContext.SaveChangesAsync();
                TempData["rentSuccess"] = true;
                return RedirectToAction("UserRents");
            }
            return RedirectToAction("Index", "Cars");
        }

        // GET: Rents/Details/5
        [Authorize(Roles = "Utilizador Registado")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                throw new HttpException(400, NoRent());
            }
            Rent rent = await _dbContext.Rents.Include(r => r.Car.Type).Include(r => r.ApplicationUser).FirstOrDefaultAsync(r => r.Id == id);
            if (rent == null)
            {
                throw new HttpException(404, NoRentFound());
            }
            return View(rent);
        }

        [Authorize(Roles = "Utilizador da Empresa")]
        public async Task<ActionResult> DetailsAdmin(int? id)
        {
            if (id == null)
            {
                throw new HttpException(400, NoRent());
            }
            Rent rent = await _dbContext.Rents.Include(r => r.Car.Type).Include(r => r.ApplicationUser).Include(r=>r.DamageImages).FirstOrDefaultAsync(r => r.Id == id);
            if (rent == null)
            {
                throw new HttpException(404, NoRentFound());
            }
            return View(rent);
        }

        [Authorize(Roles = "Utilizador Registado")]
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
        [Authorize(Roles = "Utilizador Registado")]
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


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
            base.Dispose(disposing);
        }

        private string NoRent()
        {
            return @"É necessário indicar uma reserva";
        }
        private string NoRentFound()
        {
            return @"A Reserva não existe ou não foi encontrada";
        }

    }
}

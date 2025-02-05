﻿using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using CarRenting.Models;
using CarRenting.Utils;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;

namespace CarRenting.Controllers
{
    public class CarsController : Controller
    {
        private readonly ApplicationDbContext _dbContext = new ApplicationDbContext();

        // GET: Cars
        public async Task<ActionResult> Index()
        {
            var cars = _dbContext.Cars.Include(c => c.Company).Include(c => c.Type).Include(c=>c.CarImages);
            return View(await cars.ToListAsync());
        }

        // GET: Cars
        public async Task<ActionResult> Search(string term)
        {
            if (!term.IsEmpty())
            {
                var carsBrands = _dbContext.Cars.Include(c => c.Company).Include(c => c.CarImages).Include(c => c.Type).Where(c=> c.Brand.Contains(term));
                var carModels = _dbContext.Cars.Include(c => c.Company).Include(c => c.CarImages).Include(c => c.Type).Where(c => c.Model.Contains(term));
                var carTypes = _dbContext.Cars.Include(c => c.Company).Include(c => c.CarImages).Include(c => c.Type).Where(c => c.Type.Type.Contains(term));
                var finalList = carModels.Union(carTypes).Union(carsBrands);
                if (finalList.Any())
                {
                    return View("Index", await finalList.ToListAsync());
                }
            }
            TempData["termNorFound"] = true;
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrador da Empresa, Administrador do Site")]
        public async Task<ActionResult> CompanyCars()
        {
            var userId = User.Identity.GetUserId();
            var employee = _dbContext.Employees.Include(e=>e.Company).SingleOrDefault(e => e.ApplicationUserId == userId);
            var cars = _dbContext.Cars.Include(c => c.CarImages).Include(c => c.Type).Where(c=>c.CompanyId == employee.CompanyId);
            return View(await cars.ToListAsync());
        }




        [Authorize(Roles = "Administrador do Site")]
        // GET: Cars/Details/5
        public async Task<ActionResult> Catalog(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var cars = _dbContext.Cars.Where(c => c.CompanyId == id);

            if (cars == null)
            {
                throw new HttpException(404, NoCarFound());
            }
            
            return View("CompanyCars", await _dbContext.Cars.Include(c=>c.Company).Include(c => c.CarImages).Include(c=>c.Type).Where(c => c.CompanyId == id).ToListAsync());
        }

        // GET: Cars/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                throw new HttpException(400, NoCar());
            }
            Car car = await _dbContext.Cars.Include(c=>c.CarImages).Include(c=>c.Type).Include(c=>c.Company).SingleOrDefaultAsync(c=>c.Id == id);

            if (car == null)
            {
                throw new HttpException(404, NoCarFound());
            }
            return View(car);
        }

        [Authorize(Roles = "Administrador da Empresa")]
        public ActionResult Create()
        {
            ViewBag.FuelLevels = SelectLists.FuelLevelList();
            ViewBag.TypeId = new SelectList(_dbContext.CarTypes, "Id", "Type");
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador da Empresa")]
        public async Task<ActionResult> Create([Bind(Include = "Id,License,Brand,Model,Fuel,FuelLevel,Seats,TypeId,Price,Kms,Files")] Car car)
        {
            if (ModelState.IsValid)
            {

                var repeat = _dbContext.Cars.Any(c => c.License == car.License);
                if(repeat)
                {
                    ModelState.AddModelError("License", @"Matrícula já esta registada");
                    ViewBag.FuelLevels = SelectLists.FuelLevelList();
                    ViewBag.TypeId = new SelectList(_dbContext.CarTypes, "Id", "Type", car.TypeId);
                    return View(car);
                }
                
                var userId = User.Identity.GetUserId();
                var employee = _dbContext.Employees.Include(e=>e.Company).SingleOrDefault(e => e.ApplicationUserId == userId);
                if (employee != null)
                {
                    car.Company = employee.Company;
                    _dbContext.Cars.Add(car);

                    foreach (HttpPostedFileBase file in car.Files)
                    {
                        if (file != null)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            var relativePath = "~/CarImagesUpload/" + fileName;
                            var path = Path.Combine(Server.MapPath("~/CarImagesUpload/") + fileName);
                            file.SaveAs(path);
                            if (!_dbContext.CarImages.Any(d => d.ImagePath == fileName))
                            {
                                _dbContext.CarImages.Add(new CarImage
                                    { ImagePath = relativePath, CarId = car.Id });
                            }
                        }
                    }

                    await _dbContext.SaveChangesAsync();


                    TempData["carCreated"] = true;
                    return RedirectToAction("CompanyCars");

                }
            }
            ViewBag.FuelLevels = SelectLists.FuelLevelList();
            ViewBag.TypeId = new SelectList(_dbContext.CarTypes, "Id", "Type", car.TypeId);
            return View(car);
        }

        // GET: Cars/Edit/5
        [Authorize(Roles = "Administrador da Empresa")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                throw new HttpException(400, NoCar());
            }
            Car car = await _dbContext.Cars.FindAsync(id);
            if (car == null)
            {
                throw new HttpException(404, NoCarFound());
            }
            ViewBag.FuelLevels = SelectLists.FuelLevelList();
            ViewBag.TypeId = new SelectList(_dbContext.CarTypes, "Id", "Type", car.TypeId);

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
                _dbContext.Entry(car).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                TempData["edited"] = true;
                return RedirectToAction("CompanyCars");
            }
            ViewBag.FuelLevels = SelectLists.FuelLevelList();
            ViewBag.CompanyId = new SelectList(_dbContext.Companies, "Id", "CompanyName", car.CompanyId);
            ViewBag.TypeId = new SelectList(_dbContext.CarTypes, "Id", "Type", car.TypeId);
            TempData["edited"] = false;
            return RedirectToAction("CompanyCars");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
            base.Dispose(disposing);
        }

        private string NoCar()
        {
            return @"É necessário indicar uma viatura";
        }

        private string NoCarFound()
        {
            return @"A viatura não existe ou foi encontrada";
        }


    }
}

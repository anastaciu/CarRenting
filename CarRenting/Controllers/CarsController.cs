﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using CarRenting.Models;
using CarRenting.Utils;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace CarRenting.Controllers
{
    public class CarsController : Controller
    {
        private readonly ApplicationDbContext _dbContext = new ApplicationDbContext();

        // GET: Cars
        public async Task<ActionResult> Index()
        {
            if (Request.IsAuthenticated)
            {
                RedirectToAction("Index", "Home");
            }
            var cars = _dbContext.Cars.Include(c => c.Company).Include(c => c.Type);
            return View(await cars.ToListAsync());
        }

        [Authorize(Roles = "Administrador da Empresa")]
        public async Task<ActionResult> CompanyCars()
        {
            var userId = User.Identity.GetUserId();
            var employee = _dbContext.Employees.Include(e=>e.Company).SingleOrDefault(e => e.ApplicationUserId == userId);
            var cars = _dbContext.Cars.Where(c=>c.CompanyId == employee.CompanyId).Include(c=>c.Type);
            return View(await cars.ToListAsync());
        }

        // GET: Cars/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Error");
            }
            Car car = await _dbContext.Cars.Include(c=>c.Type).Include(c=>c.Company).SingleOrDefaultAsync(c=>c.Id == id);

            if (car == null)
            {
                return HttpNotFound();
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
        public async Task<ActionResult> Create([Bind(Include = "Id,License,Brand,Model,Fuel,FuelLevel,Seats,TypeId,Price,Kms")] Car car)
        {
            if (ModelState.IsValid)
            {

                var repeat = _dbContext.Cars.Any(c => c.License == car.License);
                {
                    ModelState.AddModelError("License", @"Matrícula já esta registada");
                }
                var cars = _dbContext.Cars.ToList();
                

                var userId = User.Identity.GetUserId();
                var employee = _dbContext.Employees.Include(e=>e.Company).SingleOrDefault(e => e.ApplicationUserId == userId);
                if (employee != null)
                {
                    car.Company = employee.Company;
                    _dbContext.Cars.Add(car);
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
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = await _dbContext.Cars.FindAsync(id);
            if (car == null)
            {
                return HttpNotFound();
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
                return RedirectToAction("Index");
            }
            ViewBag.FuelLevels = SelectLists.FuelLevelList();
            ViewBag.CompanyId = new SelectList(_dbContext.Companies, "Id", "CompanyName", car.CompanyId);
            ViewBag.TypeId = new SelectList(_dbContext.CarTypes, "Id", "Type", car.TypeId);

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
            Car car = await _dbContext.Cars.FindAsync(id);
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
            Car car = await _dbContext.Cars.FindAsync(id);
            _dbContext.Cars.Remove(car);
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

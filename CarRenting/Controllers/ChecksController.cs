﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CarRenting.Models;
using Microsoft.AspNet.Identity;

namespace CarRenting.Controllers
{
    [Authorize(Roles = "Administrador da Empresa")]
    public class ChecksController : Controller
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();

        // GET: Checks
        public async Task<ActionResult> Index()
        {
            var userId = User.Identity.GetUserId();
            var company = _dbContext.Companies.SingleOrDefault(c => c.Employees.Any(e => e.ApplicationUserId == userId));
            return View(await _dbContext.Checks.Include(c => c.CarTypes).Where(c=>c.CompanyId == company.Id).ToListAsync());
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
                var company = _dbContext.Companies.SingleOrDefault(c => c.Employees.Any(e => e.ApplicationUserId == userId));
                check.Company = company;
                _dbContext.Checks.Add(check);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(check);
        }


        public async Task<ActionResult> AssociateCarType(int? id)
        {
            if (id == null)
            {
                throw new HttpException(400, NoCheck());
            }
            var carTypes = _dbContext.CarTypes.ToList();
            Check check = await _dbContext.Checks.FindAsync(id);
            if (check == null)
            {
                 throw new HttpException(404, NoCheckFound());
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
                var check = await _dbContext.Checks.FindAsync(associateViewModel.Id);
                if (check == null)
                {
                    throw new HttpException(404, NoCheckFound());

                }
                foreach (var carType in associateViewModel.CarTypes)
                {
                    if (carType.Add)
                    {
                        var type = await _dbContext.CarTypes.FindAsync(carType.Id);
                        check.CarTypes.Add(type);
                        Debug.WriteLine("Erro" + associateViewModel.Id);
                    }
                    else
                    {
                        var type = await _dbContext.CarTypes.FindAsync(carType.Id);
                        check.CarTypes.Remove(type);
                    }
                }
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(associateViewModel);
        }

        // GET: Checks/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                throw new HttpException(400, NoCheck());

            }
            Check check = await _dbContext.Checks.FindAsync(id);
            if (check == null)
            {
                throw new HttpException(404, NoCheckFound());

            }
            return View(check);
        }

        // POST: Checks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                throw new HttpException(400, NoCheck());
            }
            Check check = await _dbContext.Checks.FindAsync(id);
            if (check == null)
            {
                throw new HttpException(404, NoCheckFound());
            }
            _dbContext.Checks.Remove(check);
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

        private string NoCheck()
        {
            return @"É necessário indicar uma verificação";
        }
        private string NoCheckFound()
        {
            return @"A verificação não existe ou foi encontrada";
        }
    }
}

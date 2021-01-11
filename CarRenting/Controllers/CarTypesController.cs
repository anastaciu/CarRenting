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
using Microsoft.Ajax.Utilities;

namespace CarRenting.Controllers
{
    [Authorize(Roles = "Administrador do Site")]
    public class CarTypesController : Controller
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();

        // GET: CarTypes
    
        public async Task<ActionResult> Index()
        {
            return View(await _dbContext.CarTypes.ToListAsync());
        }

        // GET: CarTypes/Details/5
 
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarType carType = await _dbContext.CarTypes.FindAsync(id);
            if (carType == null)
            {
                return HttpNotFound();
            }
            return View(carType);
        }

        // GET: CarTypes/Create
      
        public ActionResult Create()
        {
            return View();
        }

        // POST: CarTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
     
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Type")] CarType carType)
        {
            if (ModelState.IsValid)
            {
                var type = _dbContext.CarTypes.SingleOrDefault(t => t.Type == carType.Type);
                if (type == null)
                {
                    _dbContext.CarTypes.Add(carType);
                    await _dbContext.SaveChangesAsync();
                    TempData["isCreated"] = true;
                    return RedirectToAction("Index");
                }
            }
            return View(carType);
        }

        // GET: CarTypes/Edit/5
    
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarType carType = await _dbContext.CarTypes.FindAsync(id);
            if (carType == null)
            {
                return HttpNotFound();
            }
            return View(carType);
        }

        // POST: CarTypes/Edit/5
       
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
   
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Type")] CarType carType)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Entry(carType).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                TempData["isModified"] = true;
                return RedirectToAction("Index");
            }
            TempData["isModified"] = false;
            return RedirectToAction("Index");
        }

        // GET: CarTypes/Delete/5
  
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarType carType = await _dbContext.CarTypes.FindAsync(id);
            if (carType == null)
            {
                return HttpNotFound();
            }
            return View(carType);
        }

        // POST: CarTypes/Delete/5
       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CarType carType = await _dbContext.CarTypes.FindAsync(id);
            _dbContext.CarTypes.Remove(carType);
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

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
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CarTypes
    
        public async Task<ActionResult> Index()
        {
            return View(await db.CarTypes.ToListAsync());
        }

        // GET: CarTypes/Details/5
 
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarType carType = await db.CarTypes.FindAsync(id);
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
                var type = db.CarTypes.SingleOrDefault(t => t.Type == carType.Type);
                if (type == null)
                {
                    db.CarTypes.Add(carType);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            ModelState.AddModelError("carTypeError", "Já existe uma categoria com esse nome");
            return View(carType);
        }

        // GET: CarTypes/Edit/5
    
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarType carType = await db.CarTypes.FindAsync(id);
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
                db.Entry(carType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(carType);
        }

        // GET: CarTypes/Delete/5
  
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarType carType = await db.CarTypes.FindAsync(id);
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
            CarType carType = await db.CarTypes.FindAsync(id);
            db.CarTypes.Remove(carType);
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

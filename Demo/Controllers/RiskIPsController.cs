using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Demo.Models;

namespace Demo.Controllers
{
    public class RiskIPsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: RiskIPs
        public async Task<ActionResult> Index()
        {
            return View(await db.RiskIPs.ToListAsync());
        }

        // GET: RiskIPs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RiskIP riskIP = await db.RiskIPs.FindAsync(id);
            if (riskIP == null)
            {
                return HttpNotFound();
            }
            return View(riskIP);
        }

        // GET: RiskIPs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RiskIPs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,HighRiskIP")] RiskIP riskIP)
        {
            if (ModelState.IsValid)
            {
                db.RiskIPs.Add(riskIP);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(riskIP);
        }

        // GET: RiskIPs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RiskIP riskIP = await db.RiskIPs.FindAsync(id);
            if (riskIP == null)
            {
                return HttpNotFound();
            }
            return View(riskIP);
        }

        // POST: RiskIPs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,HighRiskIP")] RiskIP riskIP)
        {
            if (ModelState.IsValid)
            {
                db.Entry(riskIP).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(riskIP);
        }

        // GET: RiskIPs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RiskIP riskIP = await db.RiskIPs.FindAsync(id);
            if (riskIP == null)
            {
                return HttpNotFound();
            }
            return View(riskIP);
        }

        // POST: RiskIPs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            RiskIP riskIP = await db.RiskIPs.FindAsync(id);
            db.RiskIPs.Remove(riskIP);
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

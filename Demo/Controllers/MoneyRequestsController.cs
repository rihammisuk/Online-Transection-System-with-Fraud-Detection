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
using Microsoft.AspNet.Identity;

namespace Demo.Controllers
{
    
    public class MoneyRequestsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: MoneyRequests
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> Index()
        {
            // If user is admin
            var moneyRequest = await db.MoneyRequests.ToListAsync(); 
            if (Request.IsAuthenticated) {
                if (User.IsInRole("User"))
                {  
                    //With this we can get Login users Money Request(If User is not ADMIN)
                    var usersID = User.Identity.GetUserId();
                    var userAccID = db.Users.Where(m => m.Id == usersID).Select(m => m.AccountID).FirstOrDefault();
                    moneyRequest = await db.MoneyRequests.Where(m => m.RequestID == userAccID).ToListAsync();
                }
                
            }

            return View(moneyRequest);

        }

        // GET: MoneyRequests/Details/5
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MoneyRequest moneyRequest = await db.MoneyRequests.FindAsync(id);
            if (moneyRequest == null)
            {
                return HttpNotFound();
            }
            return View(moneyRequest);
        }

        // GET: MoneyRequests/Create
        [Authorize(Roles = "Admin,User")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: MoneyRequests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create([Bind(Include = "ID,UserID,UserName,RequestID,RequestBalance,RequestDate")] MoneyRequest moneyRequest)
        public async Task<ActionResult> Create(MoneyRequestViewModel model)
        {
            var usersID = User.Identity.GetUserId();
            var usersName = User.Identity.GetUserName();
            var moneyRequest = new MoneyRequest
            {
                ID = model.ID,
                UserID = usersID,
                UserName = usersName,
                RequestID = model.RequestID,
                RequestBalance = model.RequestBalance,
                RequestDate = DateTime.Now.ToString("MM/dd/yyyy")
            };
            if (ModelState.IsValid)
            {
                db.MoneyRequests.Add(moneyRequest);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(moneyRequest);
        }

        // GET: MoneyRequests/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MoneyRequest moneyRequest = await db.MoneyRequests.FindAsync(id);
            if (moneyRequest == null)
            {
                return HttpNotFound();
            }
            return View(moneyRequest);
        }

        // POST: MoneyRequests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,UserID,UserName,RequestID,RequestBalance,RequestDate")] MoneyRequest moneyRequest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(moneyRequest).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(moneyRequest);
        }

        // GET: MoneyRequests/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MoneyRequest moneyRequest = await db.MoneyRequests.FindAsync(id);
            if (moneyRequest == null)
            {
                return HttpNotFound();
            }
            return View(moneyRequest);
        }

        // POST: MoneyRequests/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            MoneyRequest moneyRequest = await db.MoneyRequests.FindAsync(id);
            db.MoneyRequests.Remove(moneyRequest);
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

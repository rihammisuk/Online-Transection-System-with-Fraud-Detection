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
    
    public class UsersAccountsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: UsersAccounts
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> Index()
        {
            // If user is admin
            var usersAccounts = await db.UsersAccounts.OrderByDescending(s=>s.ID).ToListAsync();
            if (Request.IsAuthenticated)
            {
                if (User.IsInRole("User"))
                {
                    //With this we can get Login users Money Request(If User is not ADMIN)
                    var usersID = User.Identity.GetUserId();
                    //var userAccID = db.Users.Where(m => m.Id == usersID).Select(m => m.AccountID).FirstOrDefault();
                    usersAccounts = await db.UsersAccounts.Where(m => m.UserID == usersID).OrderByDescending(s => s.ID).ToListAsync();

                }

            }

            return View(usersAccounts);
          
        }

        // GET: UsersAccounts/Details/5
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> Details(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsersAccount usersAccount = await db.UsersAccounts.FindAsync(id);
            if (usersAccount == null)
            {
                return HttpNotFound();
            }
            return View(usersAccount);
        }

        // GET: UsersAccounts/Create
        [Authorize(Roles = "Admin,User")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsersAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create([Bind(Include = "ID,UserID,Transection_Money,TransectionType,F_P_AccNo,IsApproved_old,IsApproved_new,IsFraud_old,IsFraud_new,CreditRatingForOne")] UsersAccount usersAccount)
        public async Task<ActionResult> Create(UserAccountViewModel model)
        {
            var usersID = User.Identity.GetUserId();
            if (usersID == null)
            {
                usersID = "0";
            }

            //IP Address
            var ip = Request.UserHostAddress;
           
            //Is any high risk ip address as same as user IP Address(For User Sight)
            bool isUserHighRisk = db.RiskIPs.Any(s => s.HighRiskIP == ip);


            //Is any high risk ip address as same as user IP Address(For  Sight)
            // We can't select all because if receiver once use with high risk ip then he affected everytime for this so that's why we use only last IP for check
            string accountIP = db.UsersAccounts.Where(s => s.F_P_AccNo == model.F_P_AccNo).OrderByDescending(s=>s.ID).Select(s => s.UserIPAddress).FirstOrDefault();
            bool isReceiverHighRisk = db.RiskIPs.Any(s => s.HighRiskIP == accountIP);

            //Got Account Type
            var accountType = db.Users.Where(s => s.Id == usersID).Select(s => s.AccountType).FirstOrDefault();
            // Credit Avg on other method
            var creditAvg = CreditRatio(accountType);

            var usersAccount = new UsersAccount();

            if(isUserHighRisk == true)
            {
                ViewBag.Message = "Your IP address is high risk. you can't transtion any money";
               
                
            }
            else
            {
                if (isReceiverHighRisk == true)
                {
                    ViewBag.Message = "Your Receiver IP address is high risk. you can't transtion any money";
                }
                else
                {
                    if (creditAvg > Convert.ToDouble(model.Transection_Money))
                    {
                        var isFraud = Algorithm(accountType, model.TransectionType, "Normal");

                        if (isFraud == true)
                        {
                            model.IsApproved_old = false;
                        }
                        else
                        {
                            model.IsApproved_old = true;
                        }
                        usersAccount = new UsersAccount
                        {
                            ID = model.ID,
                            UserID = usersID,
                            Transection_Money = model.Transection_Money,
                            TransectionType = model.TransectionType,
                            F_P_AccNo = model.F_P_AccNo,
                            IsApproved_old = model.IsApproved_old,
                            IsApproved_new = model.IsApproved_old,
                            IsFraud_old = isFraud,
                            IsFraud_new = isFraud,
                            CreditRatingForOne = "Normal",
                            AccountTypeForOne = accountType,
                            TransectionDate = DateTime.Now.ToString("MM/dd/yyyy"),
                            UserIPAddress = ip

                        };

                        if (ModelState.IsValid)
                        {
                            if(isFraud ==  true)
                            {
                                ViewBag.Message = "You transection has been blocked. Please contact with admin.";
                            }

                            db.UsersAccounts.Add(usersAccount);
                            await db.SaveChangesAsync();
                            
                            //return RedirectToAction("Create");
                            
                        }

                    }
                    else
                    {
                        var isFraud = Algorithm(accountType, model.TransectionType, "Extra");

                        if (isFraud == true)
                        {
                            model.IsApproved_old = false;
                        }
                        else
                        {
                            model.IsApproved_old = true;
                        }
                        usersAccount = new UsersAccount
                        {

                            ID = model.ID,
                            UserID = usersID,
                            Transection_Money = model.Transection_Money,
                            TransectionType = model.TransectionType,
                            F_P_AccNo = model.F_P_AccNo,
                            IsApproved_old = model.IsApproved_old,
                            IsApproved_new = model.IsApproved_old,
                            IsFraud_old = isFraud,
                            IsFraud_new = isFraud,
                            CreditRatingForOne = "Extra",
                            AccountTypeForOne = accountType,
                            TransectionDate = DateTime.Now.ToString("MM/dd/yyyy"),
                            UserIPAddress = ip

                        };

                        if (ModelState.IsValid)
                        {
                            db.UsersAccounts.Add(usersAccount);
                            await db.SaveChangesAsync();
                            if (isFraud == true)
                            {
                                ViewBag.Message = "You transection has been blocked. Please contact with admin.";
                            }
                            //return RedirectToAction("Create");
                        }

                    }
                }
               
            }
          
            return View(usersAccount);
        }



        public double CreditRatio(string accountType)
        {
            var usersID = User.Identity.GetUserId();
            //var accountType = db.Users.Where(s => s.Id == usersID).Select(s=>s.AccountType).FirstOrDefault();
            var transectionMoney = db.UsersAccounts.Where(s => s.AccountTypeForOne == accountType).Sum(s => s.Transection_Money);
            var transectionCount = db.UsersAccounts.Where(s => s.AccountTypeForOne == accountType).Select(s => s.ID).ToList().Count();

            double creditAvg = Convert.ToDouble(transectionMoney) / Convert.ToDouble(transectionCount);
            return creditAvg;

        }


        public bool Algorithm(string accountType,string transectionType,string creditRatio)
        {
            //Fraud Count P(yes/No)
            double fraudCount = db.UsersAccounts.Select(s => s.ID).ToList().Count();
            double fraudYes = db.UsersAccounts.Where(s => s.IsFraud_old == true).Select(s => s.ID).ToList().Count();
            double fraudNo = db.UsersAccounts.Where(s => s.IsFraud_old == false).Select(s => s.ID).ToList().Count();

            //Account Type Count P(yes/No)
            double accTypeYes = db.UsersAccounts.Where(s => s.AccountTypeForOne == accountType && s.IsFraud_old == true).Select(s => s.ID).ToList().Count();
            double accYes = accTypeYes / fraudYes;
            double accTypeNo = db.UsersAccounts.Where(s => s.AccountTypeForOne == accountType && s.IsFraud_old == false).Select(s => s.ID).ToList().Count();
            double accNo = accTypeNo / fraudNo;

            //Transection Type Count P(yes/No)
            double traTypeYes = db.UsersAccounts.Where(s => s.TransectionType == transectionType && s.IsFraud_old == true).Select(s => s.ID).ToList().Count();
            double traYes = traTypeYes / fraudYes;
            double traTypeNo = db.UsersAccounts.Where(s => s.TransectionType == transectionType && s.IsFraud_old == false).Select(s => s.ID).ToList().Count();
            double traNo = traTypeNo / fraudNo;

            //Credit Ratio Count P(yes/No)
            double creditRatioYes = db.UsersAccounts.Where(s => s.CreditRatingForOne == creditRatio && s.IsFraud_old == true).Select(s => s.ID).ToList().Count();
            double creditYes = creditRatioYes / fraudYes;
            double creditRatioNo = db.UsersAccounts.Where(s => s.CreditRatingForOne == creditRatio && s.IsFraud_old == false).Select(s => s.ID).ToList().Count();
            double creditNo = creditRatioNo / fraudNo;

            //Proved Fraud or Not
            double calculateYes = accYes * traYes * creditYes;
            double calculateNo = accNo * creditNo * creditNo;

            bool isFraud = false;
            if (calculateYes > calculateNo)
            {
                isFraud = true;
            }

            return isFraud;

        }

        // GET: UsersAccounts/Edit/5
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsersAccount usersAccount = await db.UsersAccounts.FindAsync(id);
            if (usersAccount == null)
            {
                return HttpNotFound();
            }
            return View(usersAccount);
        }

        // POST: UsersAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserAccountViewModel model)
        {

            var usersID = User.Identity.GetUserId();
            if (usersID == null)
            {
                usersID = "0";
            }

            var usersAccount = new UsersAccount
            {

                ID = model.ID,
                UserID = usersID,
                Transection_Money = model.Transection_Money,
                TransectionType = model.TransectionType,
                F_P_AccNo = model.F_P_AccNo,
                IsApproved_old = model.IsApproved_old,
                IsApproved_new = model.IsApproved_new,
                IsFraud_old = model.IsFraud_old,
                IsFraud_new = model.IsFraud_new,
                CreditRatingForOne = model.CreditRatingForOne,
                AccountTypeForOne = model.AccountTypeForOne,
                TransectionDate = model.TransectionDate,
                UserIPAddress = model.UserIPAddress

            };

            if (ModelState.IsValid)
            {
                db.Entry(usersAccount).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(usersAccount);
        }

        // GET: UsersAccounts/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsersAccount usersAccount = await db.UsersAccounts.FindAsync(id);
            if (usersAccount == null)
            {
                return HttpNotFound();
            }
            return View(usersAccount);
        }

        // POST: UsersAccounts/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            UsersAccount usersAccount = await db.UsersAccounts.FindAsync(id);
            db.UsersAccounts.Remove(usersAccount);
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

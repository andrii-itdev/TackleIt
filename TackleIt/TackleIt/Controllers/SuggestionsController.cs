using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ExciteMyLife.EF.Models;
using ExciteMyLife.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ExciteMyLife.Controllers
{
    public class SuggestionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager userManager;
        public SuggestionsController()
        {
            var store = new UserStore<ApplicationUser>(db);
            userManager = new ApplicationUserManager(store);
        }

        private async Task<bool> IsNotAuthenticated()
        {
            if (Request.IsAuthenticated)
            {
                string uname = this.User.Identity.Name;
                var user = await userManager.FindByEmailAsync(uname);
                if (user?.IsAdmin == true)
                    return false;
            }
            return true;
        }
        private ActionResult GetErrorPage()
        {
            ViewBag.ErrorText = "To access this page you have to sign in as administrator.";
            return View("Error");
        }

        // GET: Suggestions
        public async Task<ActionResult> Index()
        {
            if (await IsNotAuthenticated())
                return GetErrorPage();

            return View(db.Suggestions.ToList());
        }

        // GET: Suggestions/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (await IsNotAuthenticated())
                return GetErrorPage();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Suggestion suggestion = db.Suggestions.Find(id);
            if (suggestion == null)
            {
                return HttpNotFound();
            }
            return View(suggestion);
        }

        // GET: Suggestions/Create
        public async Task<ActionResult> Create()
        {
            if (await IsNotAuthenticated())
                return GetErrorPage();

            return View();
        }

        // POST: Suggestions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Text,Description")] Suggestion suggestion)
        {
            if (await IsNotAuthenticated())
                return GetErrorPage();

            if (ModelState.IsValid)
            {
                db.Suggestions.Add(suggestion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(suggestion);
        }

        // GET: Suggestions/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (await IsNotAuthenticated())
                return GetErrorPage();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Suggestion suggestion = db.Suggestions.Find(id);
            if (suggestion == null)
            {
                return HttpNotFound();
            }
            return View(suggestion);
        }

        // POST: Suggestions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Text,Description")] Suggestion suggestion)
        {
            if (await IsNotAuthenticated())
                return GetErrorPage();

            if (ModelState.IsValid)
            {
                db.Entry(suggestion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(suggestion);
        }

        // GET: Suggestions/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (await IsNotAuthenticated())
                return GetErrorPage();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Suggestion suggestion = db.Suggestions.Find(id);
            if (suggestion == null)
            {
                return HttpNotFound();
            }
            return View(suggestion);
        }

        // POST: Suggestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if (await IsNotAuthenticated())
                return GetErrorPage();

            Suggestion suggestion = db.Suggestions.Find(id);
            db.Suggestions.Remove(suggestion);
            db.SaveChanges();
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

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
    public class UserSuggestionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager userManager;

        public UserSuggestionsController()
        {
            var store = new UserStore<ApplicationUser>(db);
            userManager = new ApplicationUserManager(store);
        }

        public async Task<ApplicationUser> GetUser()
        {
            if (Request.IsAuthenticated)
            {
                string uname = this.User.Identity.Name;
                var user = await userManager.FindByEmailAsync(uname);
                return user;
            }
            return null;
        }

        // GET: UserSuggestions
        public async Task<ActionResult> Index()
        {
            var user = await GetUser();
            var userSuggestions = db.UserSuggestions.Where(us => us.UserId == user.Id).Include(u => u.Suggestion);
            return View(userSuggestions.ToList());
        }

        // GET: UserSuggestions/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await GetUser();
            UserSuggestion userSuggestion = db.UserSuggestions.Find(user.Id, id);
            if (userSuggestion == null)
            {
                return HttpNotFound();
            }
            return View(userSuggestion);
        }

        // GET: UserSuggestions/Create
        public ActionResult Create()
        {
            ViewBag.SuggestionId = new SelectList(db.Suggestions, "Id", "Text");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email");
            return View();
        }

        // POST: UserSuggestions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "UserId,SuggestionId,Status")] UserSuggestion userSuggestion)
        {
            if (ModelState.IsValid)
            {
                var user = await GetUser();
                userSuggestion.UserId = user.Id;
                db.UserSuggestions.Add(userSuggestion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SuggestionId = new SelectList(db.Suggestions, "Id", "Text", userSuggestion.SuggestionId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", userSuggestion.UserId);
            return View(userSuggestion);
        }

        // GET: UserSuggestions/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await GetUser();
            UserSuggestion userSuggestion = db.UserSuggestions.Find(user.Id, id);
            if (userSuggestion == null)
            {
                return HttpNotFound();
            }
            ViewBag.SuggestionId = new SelectList(db.Suggestions, "Id", "Text", userSuggestion.SuggestionId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", userSuggestion.UserId);
            return View(userSuggestion);
        }

        // POST: UserSuggestions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,SuggestionId,Status")] UserSuggestion userSuggestion)
        {
            if (ModelState.IsValid)
            {
                var entry = db.Entry(userSuggestion);
                if (userSuggestion.Status == SuggestionStatus.NotToday && entry.Entity.Status != SuggestionStatus.NotToday)
                    userSuggestion.DTAdded = DateTime.Now;
                entry.State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SuggestionId = new SelectList(db.Suggestions, "Id", "Text", userSuggestion.SuggestionId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", userSuggestion.UserId);
            return View(userSuggestion);
        }

        // GET: UserSuggestions/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await GetUser();
            UserSuggestion userSuggestion = db.UserSuggestions.Find(user.Id, id);
            if (userSuggestion == null)
            {
                return HttpNotFound();
            }
            return View(userSuggestion);
        }

        // POST: UserSuggestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int? id)
        {
            var user = await GetUser();
            UserSuggestion userSuggestion = db.UserSuggestions.Find(user.Id, id);
            db.UserSuggestions.Remove(userSuggestion);
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

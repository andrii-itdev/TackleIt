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
    public class QuestionSuggestionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager userManager;
        public QuestionSuggestionsController()
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

        // GET: QuestionSuggestions
        public async Task<ActionResult> Index()
        {
            if (await IsNotAuthenticated())
                return GetErrorPage();

            var questionSuggestions = db.QuestionSuggestions.Include(q => q.Question).Include(q => q.Suggestion);
            return View(questionSuggestions.ToList());
        }

        // GET: QuestionSuggestions/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (await IsNotAuthenticated())
                return GetErrorPage();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuestionSuggestion questionSuggestion = db.QuestionSuggestions.Find(id);
            if (questionSuggestion == null)
            {
                return HttpNotFound();
            }
            return View(questionSuggestion);
        }

        // GET: QuestionSuggestions/Create
        public async Task<ActionResult> Create()
        {
            if (await IsNotAuthenticated())
                return GetErrorPage();

            ViewBag.QuestionId = new SelectList(db.Questions, "Id", "Text");
            ViewBag.SuggestionId = new SelectList(db.Suggestions, "Id", "Text");
            return View();
        }

        // POST: QuestionSuggestions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "QuestionId,SuggestionId")] QuestionSuggestion questionSuggestion)
        {
            if (await IsNotAuthenticated())
                return GetErrorPage();

            if (ModelState.IsValid)
            {
                db.QuestionSuggestions.Add(questionSuggestion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.QuestionId = new SelectList(db.Questions, "Id", "Text", questionSuggestion.QuestionId);
            ViewBag.SuggestionId = new SelectList(db.Suggestions, "Id", "Text", questionSuggestion.SuggestionId);
            return View(questionSuggestion);
        }

        // GET: QuestionSuggestions/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (await IsNotAuthenticated())
                return GetErrorPage();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuestionSuggestion questionSuggestion = db.QuestionSuggestions.Find(id);
            if (questionSuggestion == null)
            {
                return HttpNotFound();
            }
            ViewBag.QuestionId = new SelectList(db.Questions, "Id", "Text", questionSuggestion.QuestionId);
            ViewBag.SuggestionId = new SelectList(db.Suggestions, "Id", "Text", questionSuggestion.SuggestionId);
            return View(questionSuggestion);
        }

        // POST: QuestionSuggestions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "QuestionId,SuggestionId")] QuestionSuggestion questionSuggestion)
        {
            if (await IsNotAuthenticated())
                return GetErrorPage();

            if (ModelState.IsValid)
            {
                db.Entry(questionSuggestion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.QuestionId = new SelectList(db.Questions, "Id", "Text", questionSuggestion.QuestionId);
            ViewBag.SuggestionId = new SelectList(db.Suggestions, "Id", "Text", questionSuggestion.SuggestionId);
            return View(questionSuggestion);
        }

        // GET: QuestionSuggestions/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (await IsNotAuthenticated())
                return GetErrorPage();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuestionSuggestion questionSuggestion = db.QuestionSuggestions.Find(id);
            if (questionSuggestion == null)
            {
                return HttpNotFound();
            }
            return View(questionSuggestion);
        }

        // POST: QuestionSuggestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if (await IsNotAuthenticated())
                return GetErrorPage();

            QuestionSuggestion questionSuggestion = db.QuestionSuggestions.Find(id);
            db.QuestionSuggestions.Remove(questionSuggestion);
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

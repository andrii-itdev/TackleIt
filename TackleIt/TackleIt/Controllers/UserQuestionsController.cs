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
    public class UserQuestionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager userManager;

        public UserQuestionsController()
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

        // GET: UserQuestions
        public async Task<ActionResult> Index()
        {
            var user = await GetUser();
            var userQuestions = db.UserQuestions.Where(uq => uq.UserId == user.Id).Include(u => u.Question);
            return View(userQuestions.ToList());
        }

        // GET: UserQuestions/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await GetUser();
            UserQuestion userQuestion = db.UserQuestions.Find(user.Id, id);
            if (userQuestion == null)
            {
                return HttpNotFound();
            }
            return View(userQuestion);
        }

        // GET: UserQuestions/Create
        public ActionResult Create()
        {
            ViewBag.QuestionId = new SelectList(db.Questions, "Id", "Text");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email");
            return View();
        }

        // POST: UserQuestions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "UserId,QuestionId,Response")] UserQuestion userQuestion)
        {
            if (ModelState.IsValid)
            {
                var user = await GetUser();
                userQuestion.UserId = user.Id;
                db.UserQuestions.Add(userQuestion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.QuestionId = new SelectList(db.Questions, "Id", "Text", userQuestion.QuestionId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", userQuestion.UserId);
            return View(userQuestion);
        }

        // GET: UserQuestions/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await GetUser();
            UserQuestion userQuestion = db.UserQuestions.Find(user.Id, id);
            if (userQuestion == null)
            {
                return HttpNotFound();
            }
            ViewBag.QuestionId = new SelectList(db.Questions, "Id", "Text", userQuestion.QuestionId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", userQuestion.UserId);
            return View(userQuestion);
        }

        // POST: UserQuestions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,QuestionId,Response")] UserQuestion userQuestion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userQuestion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.QuestionId = new SelectList(db.Questions, "Id", "Text", userQuestion.QuestionId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", userQuestion.UserId);
            return View(userQuestion);
        }

        // GET: UserQuestions/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await GetUser();
            UserQuestion userQuestion = db.UserQuestions.Find(user.Id, id);
            if (userQuestion == null)
            {
                return HttpNotFound();
            }
            return View(userQuestion);
        }

        // POST: UserQuestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int? id)
        {
            var user = await GetUser();
            UserQuestion userQuestion = db.UserQuestions.Find(user.Id, id);
            db.UserQuestions.Remove(userQuestion);
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

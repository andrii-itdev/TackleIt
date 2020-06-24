using ExciteMyLife.EF.Models;
using ExciteMyLife.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.SqlServer;
using System.ComponentModel.DataAnnotations;

namespace ExciteMyLife.Controllers
{
    public class PropositionController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager userManager;

        public PropositionController()
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

        private ActionResult GetErrorPage(string msg = "Error finding suggestion.")
        {
            ViewBag.ErrorText = "Error finding suggestion.";
            return View("Error");
        }

        private Suggestion GetSuggestion(ApplicationUser user)
        {
            var x =
                (
                    RetrieveSuggestions(user)
                    .OrderBy(sv => -sv.Value)
                    .Take(3)
                    .OrderBy(i => Guid.NewGuid())
                    .Take(1)
                )
                .FirstOrDefault();
            ;
            return x?.Suggestion;
        }

        private IQueryable<SuggestionValue> RetrieveSuggestions(ApplicationUser user)
        {
            return (
                        from sg in db.Suggestions
                        join us in db.UserSuggestions.Include("Status")
                        on sg.Id equals us.SuggestionId into us_sg
                        from j in us_sg.DefaultIfEmpty()
                        where
                        (
                            (
                                   j.Status != SuggestionStatus.Completed
                                && j.Status != SuggestionStatus.NotToday
                                && j.Status != SuggestionStatus.NeverAsk
                            )
                            ||
                            (
                                    j.Status == SuggestionStatus.NotToday
                                &&  SqlFunctions.DateDiff("day", j.DTAdded, SqlFunctions.GetDate()) > 1
                            )
                            &&
                            j.UserId == user.Id
                        )
                        select new SuggestionValue
                        {
                            Suggestion = sg,
                            Value = SqlFunctions.Exp(
                                (
                                    (
                                        from q in
                                        (
                                            sg.Suggestions.Select(q => q.Question)
                                        )
                                        join uq in db.UserQuestions on q.Id equals uq.QuestionId
                                        where uq.UserId == user.Id
                                        select uq.Response
                                    )
                                    .DefaultIfEmpty()
                                )
                                .Select(i => SqlFunctions.Log(i + 0.000001))
                                .DefaultIfEmpty()
                                .Sum()
                            ),

                            Status = j.Status
                        }

                   );
        }

        [HttpGet]
        public async Task<ActionResult> ListMany()
        {
            var user = await GetUser();
            var suggestions = RetrieveSuggestions(user).OrderBy(sv => -sv.Value );
            return View(suggestions.ToList());
        }

        [HttpGet]
        public async Task<ActionResult> One()
        {
            var state = Session["SuggestionState"] as SuggestionState?;
            if (state == SuggestionState.QuestionAnswered)
            {

                var user = await GetUser();
                var suggestion = GetSuggestion(user);
                if (suggestion == null)
                    return GetErrorPage();
                var model = new SuggestionResponse()
                {
                    Id = suggestion.Id,
                    Text = suggestion.Text,
                    Description = suggestion.Description
                };
                return View(model);
            }
            else return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> One(SuggestionResponse model)
        {
            if (ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                {
                    var user = await GetUser();
                    var val = db.UserSuggestions.Find(user.Id, model.Id);
                    if (val != null && val.Status == SuggestionStatus.NotToday)
                    {
                        val.Status = SuggestionStatus.Completed;
                        db.Entry(val).State = System.Data.Entity.EntityState.Modified;
                    }
                    else
                    {
                        db.UserSuggestions.Add(new UserSuggestion()
                        {
                            Status = model.Status,
                            UserId = user.Id,
                            SuggestionId = model.Id
                        }
                        );
                    }
                    db.SaveChanges();

                    Session["SuggestionState"] = SuggestionState.Start;

                    //if (model.Status == SuggestionStatus.Completed)
                    //    Session["SuggestionState"] = SuggestionState.SuggestionCompleted;
                }
            }

            if (model.Status == SuggestionStatus.NeverAsk || model.Status == SuggestionStatus.NotToday)
                return RedirectToAction("One", "Proposition");

            else return RedirectToAction("Ask", "Inquiry");
        }

        [HttpPost]
        public ActionResult PostOne(SuggestionResponse model)
        {
            Session["SuggestionState"] = SuggestionState.SuggestionSelected;
            Session["SuggestionId"] = model.Id;
            return RedirectToAction("SuggestionConfirmation");
        }

        [HttpGet]
        public ActionResult SuggestionConfirmation()
        {
            var state = Session["SuggestionState"] as SuggestionState?;
            if(state == SuggestionState.SuggestionSelected)
            {
                var suggestionId = Session["SuggestionId"] as int?;
                if(suggestionId != null)
                {
                    SuggestionResponse suggestion = new SuggestionResponse(db.Suggestions.Find(suggestionId));
                    return View(suggestion);
                }
            }
            else return RedirectToAction("Index", "Home");

            return GetErrorPage();
        }

        public ActionResult AnotherOne()
        {
            Session["SuggestionState"] = SuggestionState.QuestionAnswered;
            return RedirectToAction("One");
        }
    }
}
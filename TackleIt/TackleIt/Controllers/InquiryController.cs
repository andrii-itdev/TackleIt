using ExciteMyLife.EF.Models;
using ExciteMyLife.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ExciteMyLife.Controllers
{
    public class InquiryController : Controller
    {
        public ApplicationDbContext DB { get; protected set; } = new ApplicationDbContext();
        private ApplicationUserManager userManager;

        public InquiryController()
        {
            var store = new UserStore<ApplicationUser>(DB);
            userManager = new ApplicationUserManager(store);
        }

        private async Task<ApplicationUser> GetUser()
        {
            if (Request.IsAuthenticated)
            {
                string uname = this.User.Identity.Name;
                var user = await userManager.FindByEmailAsync(uname);
                return user;
            }
            return null;
        }

        private async Task<EF.Models.Question> GetRandom()
        {
            var user = await GetUser();
            var question =
                DB.Questions.Where( q =>
                        (
                            DB.UserQuestions.Where(
                                    uq => 
                                    uq.UserId == user.Id
                                    && uq.QuestionId == q.Id
                                )
                        )
                        .Count() == 0
                    )
                    .OrderBy(ex => Guid.NewGuid())
                    .Take(1)
                    .FirstOrDefault()
                ;
            return question;
        }

        // GET: Inquiry
        public async Task<ActionResult> Ask()
        {
            var Q = await GetRandom();
            if (Q == null)
            {
                Session["SuggestionState"] = SuggestionState.QuestionAnswered;
                return RedirectToAction("One", "Proposition");
            }
            else
            {
                InquiryModel I = new InquiryModel
                {
                    QuestionId = Q.Id,
                    Text = Q.Text,
                    Description = Q.Description,
                    Type = Q.Type
                };
                return View(I);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Ask(/*[Bind(Include = "QuestionId,SuggestionId")]*/ InquiryModel inquiry)
        {
            if(User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    DB.UserQuestions.Add(new UserQuestion() { 
                        QuestionId = inquiry.QuestionId,
                        UserId = (await GetUser()).Id ,
                        Response = inquiry.Answer}
                    );
                    DB.SaveChanges();

                    Session["SuggestionState"] = SuggestionState.QuestionAnswered;
                    return RedirectToAction("One", "Proposition");
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
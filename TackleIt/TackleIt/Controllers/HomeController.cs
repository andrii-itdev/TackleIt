using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExciteMyLife.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var state = Session["SuggestionState"] as SuggestionState?;
                if(state == null)
                {
                    state = SuggestionState.Start;
                    Session["SuggestionState"] = state;
                }
                
                if(state == SuggestionState.Start)
                {
                    return RedirectToAction("Ask", "Inquiry");
                }
                else if(state == SuggestionState.QuestionAnswered)
                {
                    return RedirectToAction("One", "Proposition");
                }
                else if(state == SuggestionState.SuggestionSelected)
                {
                    return RedirectToAction("SuggestionConfirmation", "Proposition");
                }
                else if(state == SuggestionState.SuggestionCompleted)
                {
                    Session["SuggestionState"] = SuggestionState.Start;
                    return RedirectToAction("Ask", "Inquiry");
                }
                else
                {
                    return RedirectToAction("Ask", "Inquiry");
                }
            }
            else
            {
                return View();
            }
            
        }

        public ActionResult About()
        {
            ViewBag.Message = "This is an learing materials recommendation service. Answer questions and get some studying suggestions.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "";

            return View();
        }
    }
}
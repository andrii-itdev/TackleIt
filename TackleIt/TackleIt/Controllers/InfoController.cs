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
    public class InfoController : Controller
    {
        public ApplicationDbContext DB { get; protected set; } 
        private ApplicationUserManager userManager;

        public InfoController() : this(new ApplicationDbContext())
        {
        }
        public InfoController(ApplicationDbContext db)
        {
            this.DB = db;
            var store = new UserStore<ApplicationUser>(db);
            userManager = new ApplicationUserManager(store);
        }

        public bool IsAuthenticated() => Request.IsAuthenticated;

        public async Task<bool> IsAdmin() => !(await IsNotAdmin());

        public async Task<bool> IsNotAdmin()
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

        public async Task<string> UserId() => (await userManager.FindByEmailAsync(this.User?.Identity?.Name ?? ""))?.Id;

        public ActionResult GetErrorPage()
        {
            ViewBag.ErrorText = "To access this page you have to sign in as administrator.";
            return View("Error");
        }

        // GET: Info
        public ActionResult Index()
        {
            return View();
        }
    }
}
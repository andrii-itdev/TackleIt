using ExciteMyLife.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ExciteMyLife
{
    public class DataInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        public override void InitializeDatabase(ApplicationDbContext context)
        {
            base.InitializeDatabase(context);
        }

        private async Task CreateUser(ApplicationDbContext context)
        {
            string email = "me@mail.com";
            string password = "Qwerty1!";
            var masterUser = new ApplicationUser { UserName = email, Email = email };

            var store = new UserStore<ApplicationUser>(context);
            ApplicationUserManager UserManager = new ApplicationUserManager(store);
            var result = await UserManager.CreateAsync(masterUser, password);
            if (result.Succeeded)
            {
                context.SaveChanges();
            }
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //base.Seed(context);

            //CreateUser(context);

        }
    }
}
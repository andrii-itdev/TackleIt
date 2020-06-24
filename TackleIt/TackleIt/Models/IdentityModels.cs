using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using ExciteMyLife.EF.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ExciteMyLife.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        [Required]
        public bool IsAdmin { get; set; }

        public virtual ICollection<UserQuestion> UserQuestions { get; set; }
            = new HashSet<UserQuestion>();

        public virtual ICollection<UserSuggestion> UserSuggestions{ get; set; }
            = new HashSet<UserSuggestion>();
    }
     
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("ApplicationConnection", throwIfV1Schema: false)
        {
            //Database.SetInitializer(new DataInitializer());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Unit>()
            //    .HasMany(i => i.Has)
            //    .WithMany(i => i.HasReverse)
            //    .Map(i =>
            //    {
            //        i.ToTable("HasRelations");
            //        i.MapLeftKey("UnitId");
            //        i.MapRightKey("TargetId");
            //    }
            //    );
        }
        public virtual IDbSet<Question> Questions { get; set; }
        public virtual IDbSet<Suggestion> Suggestions { get; set; }

        public virtual IDbSet<UserQuestion> UserQuestions { get; set; }
        public virtual DbSet<UserSuggestion> UserSuggestions { get; set; }

        public virtual IDbSet<QuestionSuggestion> QuestionSuggestions { get; set; }

        //public System.Data.Entity.DbSet<ExciteMyLife.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
}
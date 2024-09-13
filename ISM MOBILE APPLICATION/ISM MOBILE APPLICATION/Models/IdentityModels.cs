using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ISM_MOBILE_APPLICATION.Models.IdentityReplace;

namespace ISM_MOBILE_APPLICATION.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
           }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
       
        public ApplicationDbContext()
            : base("AppConnectionDB", throwIfV1Schema: false)
        {
        }
       
        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUser>().ToTable("Users").Property(p => p.Id).HasColumnName("UserId");
            modelBuilder.Entity<ApplicationUser>().ToTable("Users").Property(p => p.Id).HasColumnName("UserId");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
           
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }


    }
}

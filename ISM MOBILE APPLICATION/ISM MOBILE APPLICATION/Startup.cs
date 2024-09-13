using ISM_MOBILE_APPLICATION.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using ISM_MOBILE_APPLICATION.Data;
using System.Data.SqlClient;
using System.Configuration;

[assembly: OwinStartupAttribute(typeof(ISM_MOBILE_APPLICATION.Startup))]
namespace ISM_MOBILE_APPLICATION
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRolesandUsers(); //Add by fahrul for Create roles Admin, Spinning, Weaving, Dyeing, Management(ALL DEPT except register user)
            CreateViewRoles();
        }

        private void CreateViewRoles()
        {
            SqlConnection conn = null;
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["AppConnectionDB"].ConnectionString);
            conn.Open();
            string strSQLCommand = "IF NOT EXISTS (SELECT 1 FROM sys.views WHERE OBJECT_ID = OBJECT_ID('dbo.[v_Roles]')) " + 
                                    "BEGIN " +
                                    "    EXEC dbo.sp_executesql @statement = N' CREATE VIEW [dbo].[v_Roles] as " + 
                                    "    SELECT " +
                                    "        Users.UserId, Users.Email, Users.UserName, ISNULL(UserRoles.RoleId, '') as [RoleID], ISNULL(Roles.Name, '') AS[RolesName] " +
                                    "    FROM " +
                                    "        Users " +
                                    "    LEFT JOIN " +
                                    "        UserRoles " + 
                                    "    ON " + 
                                    "        Users.UserId = UserRoles.UserId " + 
                                    "    LEFT JOIN " + 
                                    "        Roles " + 
                                    "    ON " + 
                                    "        UserRoles.RoleId = Roles.Id ' " +
                                    "END" ;

            SqlCommand command = new SqlCommand(strSQLCommand, conn);
            string returnvalue = (string)command.ExecuteScalar();
            conn.Close();
        }
        private void CreateRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            // In Startup iam creating first Admin Role and creating a default Admin User
            if (!roleManager.RoleExists("Admin"))
            {
                // first we create Admin rool
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);
                //Here we create a Admin super user who will maintain the website
                var user = new ApplicationUser();
                user.UserName = "Administrator";
                user.Email = "fahrul.ferdiandama.m3@mail.toray";
                string userPWD = "@Admin123";
                var chkUser = UserManager.Create(user, userPWD);
                //Add default User to Role Admin
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");
                }
            }

            // creating Creating Spinning role
            if (!roleManager.RoleExists("Management"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Management";
                roleManager.Create(role);
            }


            // creating Creating Spinning role
            if (!roleManager.RoleExists("Spinning"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Spinning";
                roleManager.Create(role);
            }

            // creating Creating Weaving role
            if (!roleManager.RoleExists("Weaving"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Weaving";
                roleManager.Create(role);
            }

            // creating Creating Dyeing role
            if (!roleManager.RoleExists("Dyeing"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Dyeing";
                roleManager.Create(role);
            }
        }
    }
}

using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace RestHomes.Infrastructure
{
    public class AppIdentityDbContext : IdentityDbContext<IdentityUser>
    {
        public AppIdentityDbContext() : base("IdentityDb") { }
        static AppIdentityDbContext()
        {
            Database.SetInitializer<AppIdentityDbContext>(new IdentityDbInit());
        }
        public static AppIdentityDbContext Create()
        {
            return new AppIdentityDbContext();
        }
    }
    public class IdentityDbInit : DropCreateDatabaseIfModelChanges<AppIdentityDbContext>
    {
        protected override void Seed(AppIdentityDbContext context)
        {
            PerformInitialSetup(context);
            base.Seed(context);
        }
        public void PerformInitialSetup(AppIdentityDbContext context)
        {
            AppUserManager userMgr = new AppUserManager(new UserStore<IdentityUser>(context));
            AppRoleManager roleMgr = new AppRoleManager(new RoleStore<IdentityRole>(context));
            {
                string roleName = "Users";
                string userName = "User";
                string password = "MySecret";
                string email = " user@example.com ";
                if (!roleMgr.RoleExists(roleName))
                {
                    roleMgr.Create(new IdentityRole(roleName));
                }
                IdentityUser user = userMgr.FindByName(userName);
                if (user == null)
                {
                    userMgr.Create(new IdentityUser { UserName = userName, Email = email },
                    password);
                    user = userMgr.FindByName(userName);
                }
                if (!userMgr.IsInRole(user.Id, roleName))
                {
                    userMgr.AddToRole(user.Id, roleName);
                }
            }
            {
                string roleName = "Administrators";
                string userName = "Admin";
                string password = "MySecret";
                string email = " admin@example.com ";
                if (!roleMgr.RoleExists(roleName))
                {
                    roleMgr.Create(new IdentityRole(roleName));
                }
                IdentityUser user = userMgr.FindByName(userName);
                if (user == null)
                {
                    userMgr.Create(new IdentityUser { UserName = userName, Email = email },
                    password);
                    user = userMgr.FindByName(userName);
                }
                if (!userMgr.IsInRole(user.Id, roleName))
                {
                    userMgr.AddToRole(user.Id, roleName);
                }
            }

        }
    }
}
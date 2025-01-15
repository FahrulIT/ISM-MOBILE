using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ISM_REPAIR_MAINTENANCE.Startup))]
namespace ISM_REPAIR_MAINTENANCE
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

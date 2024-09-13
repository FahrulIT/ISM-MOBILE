using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ISM_MOBILE.Startup))]

namespace ISM_MOBILE
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

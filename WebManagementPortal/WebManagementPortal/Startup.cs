using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebManagementPortal.Startup))]
namespace WebManagementPortal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

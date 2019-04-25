using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(KIOSK_Management.Startup))]
namespace KIOSK_Management
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

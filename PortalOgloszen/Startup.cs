using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PortalOgloszen.Startup))]
namespace PortalOgloszen
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

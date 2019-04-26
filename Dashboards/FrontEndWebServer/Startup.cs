using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Deg.FrontEndWebServer.Startup))]
namespace Deg.FrontEndWebServer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

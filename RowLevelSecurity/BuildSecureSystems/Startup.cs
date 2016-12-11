using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BuildSecureSystems.Startup))]
namespace BuildSecureSystems
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

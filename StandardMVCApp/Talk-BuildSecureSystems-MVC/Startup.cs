using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Talk_BuildSecureSystems_MVC.Startup))]
namespace Talk_BuildSecureSystems_MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

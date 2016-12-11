using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Talk_BuildSecureSystems_RowLevelSecurity.Startup))]
namespace Talk_BuildSecureSystems_RowLevelSecurity
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SecureFrameworkDemo.Startup))]
namespace SecureFrameworkDemo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

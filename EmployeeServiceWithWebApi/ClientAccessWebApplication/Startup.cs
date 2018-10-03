using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ClientAccessWebApplication.Startup))]
namespace ClientAccessWebApplication
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}

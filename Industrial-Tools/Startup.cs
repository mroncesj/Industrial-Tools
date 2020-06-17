using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Industrial_Tools.Startup))]
namespace Industrial_Tools
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

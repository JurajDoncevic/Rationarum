using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Rationarum_v3.Startup))]
namespace Rationarum_v3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

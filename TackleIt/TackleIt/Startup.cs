using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ExciteMyLife.Startup))]
namespace ExciteMyLife
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

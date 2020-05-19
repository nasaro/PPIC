using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PPIC.Startup))]
namespace PPIC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LOLAWebsite.Startup))]
namespace LOLAWebsite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

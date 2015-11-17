using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Car4U.Startup))]
namespace Car4U
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

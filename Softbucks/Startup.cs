using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Softbucks.Startup))]
namespace Softbucks
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

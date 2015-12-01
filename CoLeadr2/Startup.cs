using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CoLeadr2.Startup))]
namespace CoLeadr2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

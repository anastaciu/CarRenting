using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CarRenting.Startup))]
namespace CarRenting
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

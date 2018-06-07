using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ManageImage.Startup))]
namespace ManageImage
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OurLibrary.Startup))]
namespace OurLibrary
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}

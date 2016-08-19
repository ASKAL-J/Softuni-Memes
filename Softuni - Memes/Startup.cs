using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Softuni___Memes.Startup))]
namespace Softuni___Memes
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
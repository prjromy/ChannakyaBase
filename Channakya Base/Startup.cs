using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Channakya_Base.Startup))]
namespace Channakya_Base
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

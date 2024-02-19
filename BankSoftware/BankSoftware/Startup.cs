using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BankSoftware.Startup))]
namespace BankSoftware
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyContosoUniversity.Startup))]
namespace MyContosoUniversity
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

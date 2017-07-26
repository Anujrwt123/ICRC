using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IC_RC.Startup))]
namespace IC_RC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}

using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(ChatApp.App_Start.Startup))]

namespace ChatApp.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}

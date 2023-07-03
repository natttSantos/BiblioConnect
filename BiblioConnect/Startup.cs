using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(BiblioConnect.Startup))]
namespace BiblioConnect
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
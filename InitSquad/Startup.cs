using Microsoft.Owin;
using Owin;
using System.Diagnostics;

[assembly: OwinStartupAttribute(typeof(ClanWeb.Web.Startup))]
namespace ClanWeb.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }


    // This is code to be used all around the profject

    public static class Helper
    {
        public static bool IsDebug()
        {
#if DEBUG
            return true;
            

#else
            return false;
#endif
        }
    }



}


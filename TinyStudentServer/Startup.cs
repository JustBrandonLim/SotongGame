using Owin;
using System.Web.Http;

namespace TinyStudentServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            HttpConfiguration config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();

            appBuilder.UseWebApi(config);
        }
    }
}
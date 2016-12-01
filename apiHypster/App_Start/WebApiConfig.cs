using System.Web.Http;

namespace apiHypster
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "ControllersApi",
                routeTemplate: "{controller}/{action}/{username}",
                defaults: new { controller = "User", username = RouteParameter.Optional }
            );
        }
    }
}

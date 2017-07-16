using System.Web.Http;

namespace drs_backend_phase1
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
                routeTemplate: "{controller}/{action}/{id}",
                defaults: new {id = RouteParameter.Optional});
            //config
            //    .EnableSwagger(c => c.SingleApiVersion("v1", "A title for your API"))
            //    .EnableSwaggerUi();

        }
    }
}

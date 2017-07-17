using System.Web.Http;
using System.Web.Http.Cors;

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
                "DefaultApi",
                "{controller}/{action}/{id}",
                new {id = RouteParameter.Optional});

            var cors = new EnableCorsAttribute("http://localhost:53901, http://localhost:4200,http://ash-int-iis01:85,http://ash-int-iis01:91",
                "*", "*") {SupportsCredentials = true};
            config.EnableCors(cors);
            //config
            //    .EnableSwagger(c => c.SingleApiVersion("v1", "A title for your API"))
            //    .EnableSwaggerUi();
        }
    }
}
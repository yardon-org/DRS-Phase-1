using System.Web.Http;
using Swashbuckle.Application;

namespace drs_backend_phase1
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();
            //config
            //    .EnableSwagger(c => c.SingleApiVersion("v1", "A title for your API"))
            //    .EnableSwaggerUi();

        }
    }
}

using System.Web.Http;
using System.Web.Http.OData;

namespace drs_backend_phase1
{
    /// <summary>
    /// 
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Registers the specified configuration.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public static void Register(HttpConfiguration config)
        {
            // Allow OData Queries on all methods that return IQueryable
            System.Web.Http.OData.Extensions
                .HttpConfigurationExtensions.AddODataQueryFilter(config);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                "DefaultApi",
                "{controller}/{action}/{id}",
                new {id = RouteParameter.Optional});
        }
    }
}
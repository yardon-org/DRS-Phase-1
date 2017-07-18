using System.Web.Http;

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
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                "DefaultApi",
                "{controller}/{action}/{id}",
                new {id = RouteParameter.Optional});
        }
    }
}
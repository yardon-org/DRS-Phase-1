using System.Web.Http;
using drs_backend_phase1.Filter;
using drs_backend_phase1.Handlers;
using FluentValidation.WebApi;

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
            //config.Filters.Add(new ValidateModelStateFilter());
            //config.MessageHandlers.Add(new ResponseWrappingHandler());

            //// Add validation
            //FluentValidationModelValidatorProvider.Configure(config);

            // Allow OData Queries on all methods that return IQueryable
            System.Web.Http.OData.Extensions.HttpConfigurationExtensions.AddODataQueryFilter(config);
            HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();
            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                "DefaultApi",
                "{controller}/{action}/{id}",
                new {id = RouteParameter.Optional});
        }
    }
}
using System;
using System.Configuration;
using System.Web.Http;
using drs_backend_phase1;
using drs_backend_phase1.Provider;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;

[assembly: OwinStartupAttribute(typeof(Startup))]
namespace drs_backend_phase1
{
    /// <summary>
    /// Startup Class
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configurations the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            ConfigureOAuth(app);

            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(config);
        }

        /// <summary>
        /// Configures the o authentication.
        /// </summary>
        /// <param name="app">The application.</param>
        public void ConfigureOAuth(IAppBuilder app)
        {
            var oAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                // AllowInsecureHttp needs to be set to false for live environment
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/stats/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(Convert.ToDouble(ConfigurationManager.AppSettings["tokenDuration"])),
                Provider = new SimpleAuthorizationServerProvider()
            };

            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions { });
        }
    }
}
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using drs_backend_phase1.Models;
using drs_backend_phase1.Repository;
using drs_backend_phase1.Services;
using log4net;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;

namespace drs_backend_phase1.Provider
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomOAuthProvider : OAuthAuthorizationServerProvider
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Called to validate that the origin of the request is a registered "client_id", and that the correct credentials for that client are
        /// present on the request. If the web application accepts Basic authentication credentials,
        /// context.TryGetBasicCredentials(out clientId, out clientSecret) may be called to acquire those values if present in the request header. If the web
        /// application accepts "client_id" and "client_secret" as form encoded POST parameters,
        /// context.TryGetFormCredentials(out clientId, out clientSecret) may be called to acquire those values if present in the request body.
        /// If context.Validated is not called the request will not proceed further.
        /// </summary>
        /// <param name="context">The context of the event carries information in and results out.</param>
        /// <returns>
        /// Task to enable asynchronous execution
        /// </returns>
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context) => context.Validated();

        /// <summary>
        /// Called when a request to the Token endpoint arrives with a "grant_type" of "password". This occurs when the user has provided name and password
        /// credentials directly into the client application's user interface, and the client application is using those to acquire an "access_token" and
        /// optional "refresh_token". If the web application supports the
        /// resource owner credentials grant type it must validate the context.Username and context.Password as appropriate. To issue an
        /// access token the context.Validated must be called with a new ticket containing the claims about the resource owner which should be associated
        /// with the access token. The application should take appropriate measures to ensure that the endpoint isn’t abused by malicious callers.
        /// The default behavior is to reject this grant type.
        /// See also http://tools.ietf.org/html/rfc6749#section-4.3.2
        /// </summary>
        /// <param name="context">The context of the event carries information in and results out.</param>
        /// <returns>
        /// Task to enable asynchronous execution
        /// </returns>
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            Log.DebugFormat("Attempting to log in via the internal service...\n");

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            var repo = new AuthRepository();
            var user = await repo.FindUserAsync(context.UserName, context.Password);

            if (user == null)
            {
                Log.DebugFormat("The username or password is incorrect.\n");
                context.SetError("invalid_grant", "The username or password is incorrect");
                context.Rejected();
            }

            // Fetch the Role for the User
            SecurityRole role;
            using (var securityLayer = new SecurityService())
            {
                role = securityLayer.FindSecurityRoleByActiveDirectoryEmailAddress(user);
            }

            if (role == null)
            {
                context.SetError("invalid_grant", "The user has no associated roles.");
                return;
            }

            try
            {
                var identity = new ClaimsIdentity(context.Options.AuthenticationType, "id", "role");
                identity.AddClaim(new Claim("sub", context.UserName));
                identity.AddClaim(new Claim("username", context.UserName));
                identity.AddClaim(new Claim("role", role.roleName));

                // TODO: Add Group-based claims
                //GroupPrincipal liveExceptioningExecGroup = GroupPrincipal.FindByIdentity(ctx, ConfigurationManager.AppSettings["LiveExceptioningExecutiveGroup"]);

                if (user != null)
                {
                    var secretKeyBytes = Encoding.UTF8.GetBytes(user.ActiveDirectorySid);

                    var standardProps = new AuthenticationProperties(new Dictionary<string, string>
                    {
                        //{"clientId", user.ActiveDirectoryGuid},
                        //{"clientSecret", Convert.ToBase64String(secretKeyBytes)}
                        //{"as:AudienceId","IC24" },
                        //{"as:AudienceSecret","qMCdFDQuF23RV1Y-1Gq9L3cF3VmuFwVbam4fMTdAfpo" }

                    });

                    // Add role in plain text on header
                    context.OwinContext.Response.Headers.Add("Role", new[] { role.roleName });

                    var ticket = new AuthenticationTicket(identity, standardProps);
                    context.Validated(ticket);
                }


                Log.DebugFormat("Successfully retrieved active directory credentials for: " + context.UserName);

            }
            catch (Exception ex)
            {
                Log.DebugFormat("An error has occurred, login was unsuccessful for: " + context.UserName + ". The reason is as follows: " + ex.Message, ex.InnerException, ex + "\n");
                context.SetError("An error has occurred, login was unsuccessful for: " + context.UserName + ". The reason is as follows: " + ex.Message, ex + "\n");
            }

        }

        /// <summary>
        /// Called at the final stage of a successful Token endpoint request. An application may implement this call in order to do any final
        /// modification of the claims being used to issue access or refresh tokens. This call may also be used in order to add additional
        /// response parameters to the Token endpoint's json response body.
        /// </summary>
        /// <param name="context">The context of the event carries information in and results out.</param>
        /// <returns>
        /// Task to enable asynchronous execution
        /// </returns>
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (var property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

    }
}
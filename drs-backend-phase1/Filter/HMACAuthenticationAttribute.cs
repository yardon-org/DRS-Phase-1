using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.Caching;
using System.Security.Cryptography;
using System.Security.Principal;
using System.ServiceModel.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Results;
using log4net;

namespace drs_backend_phase1.Filter
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Attribute" />
    /// <seealso cref="System.Web.Http.Filters.IAuthenticationFilter" />
    public class HMACAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        /// <summary>
        /// The log
        /// </summary>
        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// The authentication scheme
        /// </summary>
        private readonly string authenticationScheme = "amx";
        /// <summary>
        /// The request maximum age in seconds
        /// </summary>
        private readonly ulong requestMaxAgeInSeconds = 300;

        /// <summary>
        /// The internal service host name
        /// </summary>
        private readonly string _internalServiceHostName = ConfigurationManager.AppSettings["internalServiceHostName"];
        /// <summary>
        /// The proxy service host name
        /// </summary>
        private readonly string _proxyServiceHostName = ConfigurationManager.AppSettings["proxyServiceHostName"];

        // The following code implements the core authentication logic of validating the incoming signature in the request
        /// <summary>
        /// Authenticates the request.
        /// </summary>
        /// <param name="context">The authentication context.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>
        /// A Task that will perform authentication.
        /// </returns>
        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var request = context.Request;

            IEnumerable<string> customHeaderValue;
            var headerName = string.Empty;

            if (request.Headers.TryGetValues("X-Authorization", out customHeaderValue))
            {
                headerName = customHeaderValue.FirstOrDefault();
            }
            _log.DebugFormat("\n \nHMAC Authentication - Header Name: {0}", headerName);


            // Check to see if header name is not empty or null.
            if (!string.IsNullOrEmpty(headerName))
            {
                // Seperate the scheme and the parameters
                var customHeader = headerName;

                var headerScheme = customHeader.Split(' ');

                // Splits on the space between amx and the header values.
                var customAuthorization = new AuthenticationHeaderValue(headerScheme[0], headerScheme[1]);

                if (authenticationScheme.Equals(customAuthorization.Scheme, StringComparison.OrdinalIgnoreCase))
                {
                    var rawAuthHeader = customAuthorization.Parameter;
                    var authHeaderArray = GetAuthHeaderValues(rawAuthHeader);

                    // Display each of the header parameters as individual objects.
                    if (authHeaderArray != null)
                    {
                        var clientId = authHeaderArray[0];
                        var clientSecret = authHeaderArray[1];
                        var nonce = authHeaderArray[2];
                        var requestTimeStamp = authHeaderArray[3];
                        _log.DebugFormat("Received Header Values: {0}, {1}, {2}, {3}", clientId, clientSecret, nonce, requestTimeStamp);

                        // Reconstruct the signature and compare against incoming signature
                        var isValid = IsValidRequest(clientId, request, clientSecret, nonce, requestTimeStamp);

                        try
                        {

                            if (isValid.Result)
                            {
                                _log.DebugFormat("Request is valid");
                                IPrincipal currentPrincipal = new GenericPrincipal(
                                    new GenericIdentity(clientId), null);
                                Thread.CurrentPrincipal = currentPrincipal;
                                HttpContext.Current.User = currentPrincipal;
                                context.Principal = currentPrincipal;
                            }
                            else
                            {
                                _log.DebugFormat("Request is invalid\n");
                                context.ErrorResult = new UnauthorizedResult(new AuthenticationHeaderValue[0], context.Request);
                            }
                        }
                        catch (Exception ex)
                        {
                            _log.DebugFormat("Error: " + ex.Message, ex.StackTrace, ex.InnerException, ex + "\n");
                        }
                    }
                    else
                    {
                        _log.DebugFormat("Header is null or empty" + context.ErrorResult);
                        context.ErrorResult = new UnauthorizedResult(new AuthenticationHeaderValue[0], context.Request);
                    }
                }
            }
            else
            {
                context.ErrorResult = new UnauthorizedResult(new AuthenticationHeaderValue[0], context.Request);
            }
            return Task.FromResult(0);
        }

        /// <summary>
        /// Challenges the asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            context.Result = new ResultWithChallenge(context.Result);
            return Task.FromResult(0);
        }

        /// <summary>
        /// Gets or sets a value indicating whether more than one instance of the indicated attribute can be specified for a single program element.
        /// </summary>
        public bool AllowMultiple => false;

        // Ensure that the parts of the HMAC are equal to 4 (clientId, clientSecret, nonce, timestamp)
        /// <summary>
        /// Gets the authentication header values.
        /// </summary>
        /// <param name="rawAuthHeader">The raw authentication header.</param>
        /// <returns></returns>
        private string[] GetAuthHeaderValues(string rawAuthHeader)
        {
            var credentialArray = rawAuthHeader.Split(':');

            if (credentialArray.Length == 4)
            {
                return credentialArray;
            }

            return null;
        }

        // Core implementation of reconstructing the request parameters and generating the signature on the server
        /// <summary>
        /// Determines whether [is valid request] [the specified client identifier].
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="request">The request.</param>
        /// <param name="clientSecret">The client secret.</param>
        /// <param name="nonce">The nonce.</param>
        /// <param name="requestTimeStamp">The request time stamp.</param>
        /// <returns></returns>
        private async Task<bool> IsValidRequest(string clientId, HttpRequestMessage request, string clientSecret, string nonce, string requestTimeStamp)
        {
            try
            {
                _log.Debug("Check if request is valid");

                var requestContentBase64String = "";
                var requestUriRaw = request.RequestUri.AbsoluteUri.ToLower();
                requestUriRaw = requestUriRaw.Replace(_internalServiceHostName, _proxyServiceHostName);

                _log.DebugFormat($"Raw Request Uri: {requestUriRaw}\n");

                var requestUri = HttpUtility.UrlEncode(requestUriRaw);
                _log.DebugFormat($"Request Uri: {requestUri}\n");

                var requestHttpMethod = request.Method.Method;
                var sharedKey = VerifyUser(clientId);

                if (string.IsNullOrEmpty(sharedKey))
                {
                    return false;
                }

                _log.DebugFormat($"Checking if replay request: [{nonce}], [{requestTimeStamp}]\n");
                if (IsReplayRequest(nonce, requestTimeStamp))
                {
                    return false;
                }

                var hash = await ComputeHash(request.Content);
                if (hash != null)
                {
                    requestContentBase64String = Convert.ToBase64String(hash);
                }

                var data = $"{clientId}{requestHttpMethod}{requestUri}{nonce}{requestTimeStamp}{requestContentBase64String}";
                _log.DebugFormat($"Request Content Base 64 String (Hash): {requestContentBase64String}");
                _log.DebugFormat("Data: {0}", data);

                var secretKeyBytes = Convert.FromBase64String(sharedKey);

                byte[] signature = Encoding.UTF8.GetBytes(data);

                //var signature = Encoding.UTF8.GetBytes(data);

                using (HMACSHA256 hmac = new HMACSHA256(secretKeyBytes))
                {
                    //var signatureBytes = hmac.ComputeHash(signature);

                    byte[] signatureBytes = hmac.ComputeHash(signature);

                    _log.DebugFormat("Signature: {0}", clientSecret);
                    _log.DebugFormat("Post HMAC String: {0}", Convert.ToBase64String(signatureBytes));

                    //return clientSecret.Equals(Convert.ToBase64String(signatureBytes), StringComparison.Ordinal);

                    return clientSecret.Equals(Convert.ToBase64String(signatureBytes), StringComparison.Ordinal);

                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex);
                return false;
            }
        }

        /// <summary>
        /// Determines whether [is replay request] [the specified nonce].
        /// </summary>
        /// <param name="nonce">The nonce.</param>
        /// <param name="requestTimeStamp">The request time stamp.</param>
        /// <returns>
        ///   <c>true</c> if [is replay request] [the specified nonce]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsReplayRequest(string nonce, string requestTimeStamp)
        {
            if (MemoryCache.Default.Contains(nonce))
            {
                return true;
            }

            var epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
            var currentTimeSpan = DateTime.UtcNow - epochStart;

            var serverTotalSeconds = Convert.ToUInt64(currentTimeSpan.TotalSeconds);
            var requestTotalSeconds = Convert.ToUInt64(requestTimeStamp);

            if (serverTotalSeconds - requestTotalSeconds > requestMaxAgeInSeconds)
            {
                return true;
            }

            MemoryCache.Default.Add(nonce, requestTimeStamp, DateTimeOffset.UtcNow.AddSeconds(requestMaxAgeInSeconds));
            return false;

        }

        // POST only: hash the content being sent
        /// <summary>
        /// Computes the hash.
        /// </summary>
        /// <param name="httpContent">Content of the HTTP.</param>
        /// <returns></returns>
        private static async Task<byte[]> ComputeHash(HttpContent httpContent)
        {
            using (var md5 = MD5.Create())
            {
                byte[] hash = null;
                var content = await httpContent.ReadAsByteArrayAsync();
                if (content.Length != 0)
                {
                    hash = md5.ComputeHash(content);
                }
                return hash;
            }
        }

        /// <summary>
        /// Verifies the user.
        /// </summary>
        /// <param name="guidValue">The unique identifier value.</param>
        /// <returns></returns>
        /// <exception cref="SecurityAccessDeniedException">No valid active directory accounts had the GUID value</exception>
        /// <exception cref="Exception"></exception>
        private string VerifyUser(string guidValue)
        {
            // Establish domain credentials
            using (var context = new PrincipalContext(
                ContextType.Domain,
                @"ash-dc02.sehnp.nhs.uk:3268",
                @"DC=sehnp,DC=nhs,DC=uk",
                @"SEHNP\RemoteDeployService",
                @"RDS31@Essex#"))
            {
                UserPrincipal personPrincipal = null;
                try
                {
                    _log.DebugFormat("Searching for user with GUID: {0}", guidValue);
                    // Find the Active Directory GUID
                    personPrincipal = UserPrincipal.FindByIdentity(context, IdentityType.Guid, guidValue);

                    // Is the person in the Active Directory? If not they will suffer a painful death OR throw an exception  
                    if (personPrincipal == null)
                    {
                        throw new SecurityAccessDeniedException("No valid active directory accounts had the GUID value");
                    }

                    _log.DebugFormat("Found user SID {0}", personPrincipal.Sid);
                    _log.DebugFormat("Active Directory Account for: {0}", personPrincipal.DistinguishedName);

                    // Return the SID as base 64 encoded value
                    return Convert.ToBase64String(Encoding.UTF8.GetBytes(personPrincipal.Sid.ToString()));
                    //return personPrincipal.Sid.ToString();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    personPrincipal?.Dispose();
                }
            }
        }

        // Authentication challenge to unauthorized responses
        /// <summary>
        /// 
        /// </summary>
        /// <seealso cref="System.Web.Http.IHttpActionResult" />
        public class ResultWithChallenge : IHttpActionResult
        {
            /// <summary>
            /// The authentication scheme
            /// </summary>
            private readonly string authenticationScheme = "amx";
            /// <summary>
            /// The next
            /// </summary>
            private readonly IHttpActionResult _next;

            /// <summary>
            /// Initializes a new instance of the <see cref="ResultWithChallenge"/> class.
            /// </summary>
            /// <param name="next">The next.</param>
            public ResultWithChallenge(IHttpActionResult next)
            {
                _next = next;
            }

            /// <summary>
            /// Creates an <see cref="T:System.Net.Http.HttpResponseMessage" /> asynchronously.
            /// </summary>
            /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
            /// <returns>
            /// A task that, when completed, contains the <see cref="T:System.Net.Http.HttpResponseMessage" />.
            /// </returns>
            public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                var response = await _next.ExecuteAsync(cancellationToken);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue(authenticationScheme));
                }

                return response;
            }
        }
    }
}
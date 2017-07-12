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
    public class HMACAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        private readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string authenticationScheme = "amx";
        private readonly ulong requestMaxAgeInSeconds = 300;

        private readonly string _internalServiceHostName = ConfigurationManager.AppSettings["internalServiceHostName"];
        private readonly string _proxyServiceHostName = ConfigurationManager.AppSettings["proxyServiceHostName"];

        // The following code implements the core authentication logic of validating the incoming signature in the request
        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var request = context.Request;

            IEnumerable<string> customHeaderValue;
            var headerName = string.Empty;

            if (request.Headers.TryGetValues("X-Authorization", out customHeaderValue))
            {
                headerName = customHeaderValue.FirstOrDefault();
            }
            Log.DebugFormat("\n \nHMAC Authentication - Header Name: {0}", headerName);


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
                        Log.DebugFormat("Received Header Values: {0}, {1}, {2}, {3}", clientId, clientSecret, nonce, requestTimeStamp);

                        // Reconstruct the signature and compare against incoming signature
                        var isValid = IsValidRequest(clientId, request, clientSecret, nonce, requestTimeStamp);

                        try
                        {

                            if (isValid.Result)
                            {
                                Log.DebugFormat("Request is valid");
                                IPrincipal currentPrincipal = new GenericPrincipal(
                                    new GenericIdentity(clientId), null);
                                Thread.CurrentPrincipal = currentPrincipal;
                                HttpContext.Current.User = currentPrincipal;
                                context.Principal = currentPrincipal;
                            }
                            else
                            {
                                Log.DebugFormat("Request is invalid\n");
                                context.ErrorResult = new UnauthorizedResult(new AuthenticationHeaderValue[0], context.Request);
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.DebugFormat("Error: " + ex.Message, ex.StackTrace, ex.InnerException, ex + "\n");
                        }
                    }
                    else
                    {
                        Log.DebugFormat("Header is null or empty" + context.ErrorResult);
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

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            context.Result = new ResultWithChallenge(context.Result);
            return Task.FromResult(0);
        }

        public bool AllowMultiple => false;

        // Ensure that the parts of the HMAC are equal to 4 (clientId, clientSecret, nonce, timestamp)
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
        private async Task<bool> IsValidRequest(string clientId, HttpRequestMessage request, string clientSecret, string nonce, string requestTimeStamp)
        {
            try
            {
                Log.Debug("Check if request is valid");

                var requestContentBase64String = "";
                var requestUriRaw = request.RequestUri.AbsoluteUri.ToLower();
                requestUriRaw = requestUriRaw.Replace(_internalServiceHostName, _proxyServiceHostName);

                Log.DebugFormat($"Raw Request Uri: {requestUriRaw}\n");

                var requestUri = HttpUtility.UrlEncode(requestUriRaw);
                Log.DebugFormat($"Request Uri: {requestUri}\n");

                var requestHttpMethod = request.Method.Method;
                var sharedKey = VerifyUser(clientId);

                if (string.IsNullOrEmpty(sharedKey))
                {
                    return false;
                }

                Log.DebugFormat($"Checking if replay request: [{nonce}], [{requestTimeStamp}]\n");
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
                Log.DebugFormat($"Request Content Base 64 String (Hash): {requestContentBase64String}");
                Log.DebugFormat("Data: {0}", data);

                var secretKeyBytes = Convert.FromBase64String(sharedKey);

                byte[] signature = Encoding.UTF8.GetBytes(data);

                //var signature = Encoding.UTF8.GetBytes(data);

                using (HMACSHA256 hmac = new HMACSHA256(secretKeyBytes))
                {
                    //var signatureBytes = hmac.ComputeHash(signature);

                    byte[] signatureBytes = hmac.ComputeHash(signature);

                    Log.DebugFormat("Signature: {0}", clientSecret);
                    Log.DebugFormat("Post HMAC String: {0}", Convert.ToBase64String(signatureBytes));

                    //return clientSecret.Equals(Convert.ToBase64String(signatureBytes), StringComparison.Ordinal);

                    return clientSecret.Equals(Convert.ToBase64String(signatureBytes), StringComparison.Ordinal);

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                return false;
            }
        }

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
                    Log.DebugFormat("Searching for user with GUID: {0}", guidValue);
                    // Find the Active Directory GUID
                    personPrincipal = UserPrincipal.FindByIdentity(context, IdentityType.Guid, guidValue);

                    // Is the person in the Active Directory? If not they will suffer a painful death OR throw an exception  
                    if (personPrincipal == null)
                    {
                        throw new SecurityAccessDeniedException("No valid active directory accounts had the GUID value");
                    }

                    Log.DebugFormat("Found user SID {0}", personPrincipal.Sid);
                    Log.DebugFormat("Active Directory Account for: {0}", personPrincipal.DistinguishedName);

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
        public class ResultWithChallenge : IHttpActionResult
        {
            private readonly string authenticationScheme = "amx";
            private readonly IHttpActionResult _next;

            public ResultWithChallenge(IHttpActionResult next)
            {
                _next = next;
            }

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
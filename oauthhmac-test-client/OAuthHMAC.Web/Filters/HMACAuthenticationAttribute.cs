using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Results;

namespace OAuthHMAC.Web.Filters
{
    public class HmacAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        // Allowed apps simulates the database that contains the clientID and the clientSecret
        private static Dictionary<string, string> allowedApps = new Dictionary<string, string>();
        private readonly UInt64 requestMaxAgeInSeconds = 300;
        private readonly string authenticationScheme = "amx";


        public HmacAuthenticationAttribute()
        {
            // Pull values from the active directory of current user
            var clientId = UserPrincipal.Current.Guid.ToString();
            var clientSecret = Convert.ToBase64String(Encoding.UTF8.GetBytes(UserPrincipal.Current.Sid.ToString()));

            if (allowedApps.Count == 0)
            {
                allowedApps.Add(clientId, clientSecret);
            }
        }

        // Implements the core authentication logic of validating the incoming signature in the request
        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var req = context.Request;

            IEnumerable<string> customHeaderValue;
            var headerName = string.Empty;

            if (req.Headers.TryGetValues("X-Authorization", out customHeaderValue))
            {
                headerName = customHeaderValue.FirstOrDefault();
            }

            

            // Check header name is not empty or null
            if (!string.IsNullOrEmpty(headerName))
            {
                // Seperate scheme and parameter
                var customHeader = headerName;

                var headerScheme = customHeader.Split(' ');

                var customAuthorization = new AuthenticationHeaderValue(headerScheme[0], headerScheme[1]);

                if (authenticationScheme.Equals(customAuthorization.Scheme, StringComparison.OrdinalIgnoreCase))

                {
                    var rawAuthHeader = customAuthorization.Parameter;

                    var authHeaderArray = GetAuthHeaderValues(rawAuthHeader);

                    if (authHeaderArray != null)
                    {
                        var clientId = authHeaderArray[0];
                        var clientSecret = authHeaderArray[1];
                        var nonce = authHeaderArray[2];
                        var requestTimeStamp = authHeaderArray[3];

                        // Reconstructs the signature and compares it with the incoming signature
                        var isValid = isValidRequest(req, clientId, clientSecret, nonce, requestTimeStamp);

                        if (isValid.Result)
                        {
                            var currentPrinciple = new GenericPrincipal(new GenericIdentity(clientId), null);
                            context.Principal = currentPrinciple;
                        }
                        else
                        {
                            context.ErrorResult = new UnauthorizedResult(new AuthenticationHeaderValue[0],
                                context.Request);
                        }
                    }
                    else
                    {
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

        public bool AllowMultiple
        {
            get { return false; }
        }

        private string[] GetAuthHeaderValues(string rawAuthHeader)
        {
            var credArray = rawAuthHeader.Split(':');

            if (credArray.Length == 4)
            {
                return credArray;
            }

            return null;
        }


        // Authentication challenge to unauthorized responses
        public class ResultWithChallenge : IHttpActionResult
        {
            private readonly string authenticationScheme = "amx";
            private readonly IHttpActionResult next;

            public ResultWithChallenge(IHttpActionResult next)
            {
                this.next = next;
            }

            public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
            {
                var response = await next.ExecuteAsync(cancellationToken);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue(authenticationScheme));
                }

                return response;
            }
        }

        // Core implementation of reconstructing the request parameters and generating the signature on the server happens here
        private async Task<bool> isValidRequest(HttpRequestMessage req, string clientId, string clientSecret, string nonce, string requestTimeStamp)
        {
            string requestContentBase64String = "";
            string requestUri = HttpUtility.UrlEncode(req.RequestUri.AbsoluteUri.ToLower());
            string requestHttpMethod = req.Method.Method;

            if (!allowedApps.ContainsKey(clientId))
            {
                return false;
            }

            var sharedKey = allowedApps[clientId];

            if (isReplayRequest(nonce, requestTimeStamp))
            {
                return false;
            }

            byte[] hash = await ComputeHash(req.Content);

            if (hash != null)
            {
                requestContentBase64String = Convert.ToBase64String(hash);
            }

            string data = String.Format("{0}{1}{2}{3}{4}{5}", clientId, requestHttpMethod, requestUri, requestTimeStamp, nonce, requestContentBase64String);

            var secretKeyBytes = Convert.FromBase64String(sharedKey);

            byte[] signature = Encoding.UTF8.GetBytes(data);

            using (HMACSHA256 hmac = new HMACSHA256(secretKeyBytes))
            {
                byte[] signatureBytes = hmac.ComputeHash(signature);

                return clientSecret.Equals(Convert.ToBase64String(signatureBytes), StringComparison.Ordinal);
            }
        }

        private bool isReplayRequest(string nonce, string requestTimeStamp)
        {
            if (System.Runtime.Caching.MemoryCache.Default.Contains(nonce))
            {
                return true;
            }

            DateTime epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan currentTs = DateTime.UtcNow - epochStart;

            var serverTotalSeconds = Convert.ToUInt64(currentTs.TotalSeconds);
            var requestTotalSeconds = Convert.ToUInt64(requestTimeStamp);

            if ((serverTotalSeconds - requestTotalSeconds) > requestMaxAgeInSeconds)
            {
                return true;
            }

            System.Runtime.Caching.MemoryCache.Default.Add(nonce, requestTimeStamp, DateTimeOffset.UtcNow.AddSeconds(requestMaxAgeInSeconds));

            return false;
        }

        private static async Task<byte[]> ComputeHash(HttpContent httpContent)
        {
            using (MD5 md5 = MD5.Create())
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
    }
}
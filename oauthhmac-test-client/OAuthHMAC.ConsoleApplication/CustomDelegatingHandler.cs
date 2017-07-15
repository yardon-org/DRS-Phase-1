using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace OAuthHMAC.ConsoleApplication
{
    internal partial class Program
    {
        public class CustomDelegatingHandler : DelegatingHandler
        {
            private readonly string _clientId;
            private readonly string _clientSecret;
            private readonly string _token;


            public CustomDelegatingHandler(string clientId, string clientSecret, string token)
            {
                _clientId = clientId;
                _clientSecret = clientSecret;
                _token = token;
            }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                CancellationToken cancellationToken)
            {
                // Set Url Parameters
                var requestUri = HttpUtility.UrlEncode(request.RequestUri.AbsoluteUri.ToLower());
                var requestHttpMethod = request.Method.Method;

                HttpResponseMessage response = null;
                var requestContentBase64String = string.Empty;

                // Set requestTimeStamp parameters
                var epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
                var timeSpan = DateTime.UtcNow - epochStart;
                var requestTimeStamp = Convert.ToUInt64(timeSpan.TotalSeconds).ToString();

                // Create random nonce for each request
                var nonce = Guid.NewGuid().ToString("N");

                // Checking if the request contains body, usually will be null with HTTP GET and DELETE
                if (request.Content != null)
                {
                    var content = await request.Content.ReadAsByteArrayAsync();
                    var md5 = MD5.Create();

                    // Hashing the request body, any change in request body will result in different hash, we'll incure message integrity
                    var requestContentHash = md5.ComputeHash(content);
                    requestContentBase64String = Convert.ToBase64String(requestContentHash);
                    Console.WriteLine($"Request Content Base 64 String (Hash): {requestContentBase64String}");
                }

                // Creating the raw signature string
                var signatureRawData =
                    $"{_clientId}{requestHttpMethod}{requestUri}{nonce}{requestTimeStamp}{requestContentBase64String}";
                //Console.WriteLine($"Data: {signatureRawData}");

                var secretKeyByteArray = Convert.FromBase64String(_clientSecret);

                var signature = Encoding.UTF8.GetBytes(signatureRawData);


                // Something within this 
                using (var hmac = new HMACSHA256(secretKeyByteArray))
                {
                    var signatureBytes = hmac.ComputeHash(signature);

                    var requestSignatureBase64String = Convert.ToBase64String(signatureBytes);

                    // Setting the values in the authorization header using custom scheme (amx)
                    request.Headers.Add("X-Authorization",
                        $"amx {_clientId}:{requestSignatureBase64String}:{nonce}:{requestTimeStamp}");

                    var test = $"amx {_clientId}:{requestSignatureBase64String}:{nonce}:{requestTimeStamp}";

                    Console.WriteLine($"Header Name: {test}");
                }

                request.Headers.Authorization = new AuthenticationHeaderValue("bearer", _token);
                response = await base.SendAsync(request, cancellationToken);

                return response;
            }
        }
    }
}
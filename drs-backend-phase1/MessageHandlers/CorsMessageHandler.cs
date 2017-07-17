using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace drs_backend_phase1.MessageHandlers
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Net.Http.DelegatingHandler" />
    public class CorsMessageHandler : DelegatingHandler
    {
        /// <summary>
        /// The origin
        /// </summary>
        private const string Origin = "Origin";
        /// <summary>
        /// The access control request method
        /// </summary>
        private const string AccessControlRequestMethod = "Access-Control-Request-Method";
        /// <summary>
        /// The access control request headers
        /// </summary>
        private const string AccessControlRequestHeaders = "Access-Control-Request-Headers";
        /// <summary>
        /// The access control allow origin
        /// </summary>
        private const string AccessControlAllowOrigin = "Access-Control-Allow-Origin";
        /// <summary>
        /// The access control allow methods
        /// </summary>
        private const string AccessControlAllowMethods = "Access-Control-Allow-Methods";
        /// <summary>
        /// The access control allow headers
        /// </summary>
        private const string AccessControlAllowHeaders = "Access-Control-Allow-Headers";

        /// <summary>
        /// Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.
        /// </summary>
        /// <param name="request">The HTTP request message to send to the server.</param>
        /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
        /// <returns>
        /// Returns <see cref="T:System.Threading.Tasks.Task`1" />. The task object representing the asynchronous operation.
        /// </returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return request.Headers.Contains(Origin)
                ? ProcessCorsRequest(request, ref cancellationToken)
                : base.SendAsync(request, cancellationToken);
        }

        /// <summary>
        /// Processes the cors request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        private Task<HttpResponseMessage> ProcessCorsRequest(HttpRequestMessage request,
            ref CancellationToken cancellationToken)
        {
            if (request.Method == HttpMethod.Options)
                return Task.Factory.StartNew(() =>
                {
                    var response = new HttpResponseMessage(HttpStatusCode.OK);
                    AddCorsResponseHeaders(request, response);
                    return response;
                }, cancellationToken);
            return base.SendAsync(request, cancellationToken).ContinueWith(task =>
            {
                var resp = task.Result;
                resp.Headers.Add(AccessControlAllowOrigin, request.Headers.GetValues(Origin).First());
                return resp;
            }, cancellationToken);
        }

        /// <summary>
        /// Adds the cors response headers.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        private static void AddCorsResponseHeaders(HttpRequestMessage request, HttpResponseMessage response)
        {
            response.Headers.Add(AccessControlAllowOrigin, request.Headers.GetValues(Origin).First());

            var accessControlRequestMethod = request.Headers.GetValues(AccessControlRequestMethod).FirstOrDefault();
            if (accessControlRequestMethod != null)
                response.Headers.Add(AccessControlAllowMethods, accessControlRequestMethod);

            var requestedHeaders = string.Join(", ", request.Headers.GetValues(AccessControlRequestHeaders));
            if (!string.IsNullOrEmpty(requestedHeaders))
                response.Headers.Add(AccessControlAllowHeaders, requestedHeaders);
        }
    }
}
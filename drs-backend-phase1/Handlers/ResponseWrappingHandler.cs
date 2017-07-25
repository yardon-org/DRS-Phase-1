using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;

namespace drs_backend_phase1.Handlers
{
    /// <summary>
    /// Validation response handler
    /// </summary>
    /// <seealso cref="System.Net.Http.DelegatingHandler" />
    public class ResponseWrappingHandler : DelegatingHandler
    {
        /// <summary>
        /// Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.
        /// </summary>
        /// <param name="request">The HTTP request message to send to the server.</param>
        /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
        /// <returns>
        /// Returns <see cref="T:System.Threading.Tasks.Task`1" />. The task object representing the asynchronous operation.
        /// </returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //Step 1: Wait for the Response
            var response = await base.SendAsync(request, cancellationToken);

            return BuildApiResponse(request, response);
        }

        /// <summary>
        /// Builds the API response.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="response">The response.</param>
        /// <returns></returns>
        private HttpResponseMessage BuildApiResponse(HttpRequestMessage request, HttpResponseMessage response)
        {
            object content;
            List<string> modelStateErrors = new List<string>();

            //Step 2: Get the Response Content
            if (response.TryGetContentValue(out content) && !response.IsSuccessStatusCode)
            {
                HttpError error = content as HttpError;
                if (error != null)
                {
                    //Step 3: If content is an error, return nothing for the Result.
                    content = null; //We have errors, so don't return any content
                    //Step 4: Insert the ModelState errors              
                    if (error.ModelState != null)
                    {
                        //Read as string
                        var httpErrorObject = response.Content.ReadAsStringAsync().Result;

                        //Convert to anonymous object
                        var anonymousErrorObject = new { message = "", ModelState = new Dictionary<string, string[]>() };

                        // Deserialize anonymous object
                        var deserializedErrorObject = JsonConvert.DeserializeAnonymousType(httpErrorObject, anonymousErrorObject);

                        // Get error messages from ModelState object
                        var modelStateValues = deserializedErrorObject.ModelState.Select(kvp => string.Join(". ", kvp.Value));

                        for (int i = 0; i < modelStateValues.Count(); i++)
                        {
                            modelStateErrors.Add(modelStateValues.ElementAt(i));
                        }
                    }
                }
            }

            //Step 5: Create a new response
            var newResponse = request.CreateResponse(response.StatusCode, new ResponsePackage(content, modelStateErrors));

            //Step 6: Add Back the Response Headers
            foreach (var header in response.Headers) //Add back the response headers
            {
                newResponse.Headers.Add(header.Key, header.Value);
            }

            return newResponse;
        }
    }
}

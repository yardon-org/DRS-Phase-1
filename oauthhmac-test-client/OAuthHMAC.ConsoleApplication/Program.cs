using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace OAuthHMAC.ConsoleApplication
{
    internal partial class Program
    {
        private static void Main(string[] args)
        {
            RunGetAsync().Wait();
        }

        private static async Task RunGetAsync()
        {
            Console.WriteLine("Calling API");

            var apiBaseAddress = "http://localhost:53901"; // <---- you will need to change this!!

            string clientId;
            string clientSecret;
            string token;

            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(apiBaseAddress + "/api/stats/token"),
                    Method = HttpMethod.Post
                };

                // Ensure that the header will accept json values
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // The body that is being sent is using x-www-form-urlencoded and uses the following values
                request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"username", "xxxxxxxx"}, // <------ replace with your AD username
                    {"password", "xxxxxx"}, // <------ replace with your AD password
                    {"grant_type", "password"}
                });

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                // Get the authorisation headers
                //authFields = request.Headers.Authorization;

                // Strip each part of the response down so that it can be handled invidually
                var payload = JObject.Parse(await response.Content.ReadAsStringAsync());
                token = payload.Value<string>("access_token");
                var tokentype = payload.Value<string>("token_type");
                clientId = payload.Value<string>("clientId");
                clientSecret = payload.Value<string>("clientSecret");

                Console.WriteLine(
                    "\nAccess Token: {0},\n \nToken Type: {1},\n \nClientId: {2},\n \nClientSecret: {3}\n", token,
                    tokentype, clientId, clientSecret);
            }

            try
            {
                var customDelegatingHandler = new CustomDelegatingHandler(clientId, clientSecret, token);
                var client = HttpClientFactory.Create(customDelegatingHandler);

                var response = await client.GetAsync(apiBaseAddress + "/api/event-log");

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    Console.Clear();
                    Console.WriteLine(responseString);
                    Console.WriteLine("HTTP Status: {0}, Reason {1}. Press ENTER to exit", response.StatusCode,
                        response.ReasonPhrase);
                }
                else
                {
                    Console.WriteLine("Failed to call the API. HTTP Status {0}, Reason {1}", response.StatusCode,
                        response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.ReadLine();
        }

        private static EncryptedData ExtractEncryptedData(HttpContent content)
        {
            // Pass in the result retrieved as a string
            var quotedText = content.ReadAsStringAsync().Result;

            // Encrypt the result
            var encryptString = EncryptionManager.Encrypt(quotedText);

            return new EncryptedData
            {
                Data = encryptString
            };
        }
    }
}
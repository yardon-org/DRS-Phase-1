using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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

            //var apiBaseAddress = "http://ash-int-iis01:85"; // <---- you will need to change this!!
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
                    {"username", "pfoster1"}, // <------ replace with your AD username
                    {"password", "Piglet72!!"}, // <------ replace with your AD password
                    {"grant_type", "password"}
                });

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

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

                //var response = await client.GetAsync(apiBaseAddress + "/api/profile/fetchProfiles");
                //var responseString1 = await response.Content.ReadAsStringAsync();

                string serilized = "{\"smsEnabled\":true,\"ProfileProfessional\":{},\"ProfileFinance\":{},\"ProfileDocuments\":[],\"specialNoteArray\":[],\"firstName\":\"Jon\",\"lastName\":\"Snow\",\"dateOfBirth\":\"1980-01-01T00:00:00.000Z\",\"postcode\":\"N11 1AN\",\"address2\":\"LONDON\",\"address1\":\"352A BOWES ROAD\",\"address3\":null,\"address4\":null,\"address5\":null,\"mobilePhone\":\"3453452\",\"homeEmail\":\"test@test.com\",\"nhsEmail\":\"test@nhs.net\"}";
                var inputMessage = new HttpRequestMessage
                {
                    Content = new StringContent(serilized, Encoding.UTF8, "application/json")
                };
                inputMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //HttpResponseMessage message = client.PutAsync(apiBaseAddress + "/api/profile", inputMessage.Content).Result;
                //var httpContent = new StringContent(responseString1, Encoding.UTF8, "application/json");
                //var response = client.PutAsync(apiBaseAddress + "/api/profile", inputMessage.Content).Result;
                var response = client.PostAsync(apiBaseAddress + "/api/profile", inputMessage.Content).Result;
                var responseString = await response.Content.ReadAsStringAsync();
                response.EnsureSuccessStatusCode();


                //if (response.IsSuccessStatusCode)
                //{
                //    //var responseString = await response.Content.ReadAsStringAsync();
                //    Console.Clear();
                //    //Console.WriteLine(responseString);
                //    Console.WriteLine("HTTP Status: {0}, Reason {1}. Press ENTER to exit", response.StatusCode,
                //        response.ReasonPhrase);
                //}
                //else
                //{
                //    Console.WriteLine("Failed to call the API. HTTP Status {0}, Reason {1}", response.StatusCode,
                //        response.ReasonPhrase);
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

                Console.ReadLine();
        }
    }
}
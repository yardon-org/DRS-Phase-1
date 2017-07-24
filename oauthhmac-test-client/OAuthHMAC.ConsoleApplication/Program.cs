using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
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

                var response = await client.GetAsync(apiBaseAddress + "/api/profile/fetchProfiles");

                string serilized = "{\"$id\":\"1\",\"address1\":\"21 SIR BERNARD PAGET AVENUE\",\"address2\":\"ASHFORD\",\"address3\":null,\"address4\":null,\"address5\":null,\"adEmailAddress\":\"Facund.pons@ic24.nhs.uk\",\"dateOfBirth\":\"1995-06-08T23:00:00\",\"firstName\":\"test\",\"homeEmail\":\"444@test.test\",\"homePhone\":\"423523\",\"id\":5,\"isComplete\":false,\"isDeleted\":false,\"isInactive\":false,\"lastName\":\"PP\",\"middleNames\":\"test\",\"mobilePhone\":\"4r4\",\"nhsEmail\":\"test@nhs.net\",\"postcode\":\"TN23 3RT\",\"ProfileDocuments\":[{\"$id\":\"2\",\"dateExpires\":\"2016-07-23T00:00:00\",\"dateObtained\":null,\"DocumentType\":{\"$id\":\"3\",\"id\":1,\"isDeleted\":false,\"name\":\"Document Type One\"},\"documentTypeId\":1,\"id\":13,\"isDeleted\":false,\"mimeType\":\"application/pdf\",\"multerFileName\":\"file-1498565718804\",\"originalFileName\":\"E--Sites-Webportal-live-user-Certificate.pdf\",\"profileId\":5},{\"$id\":\"4\",\"dateExpires\":\"2020-11-26T00:00:00\",\"dateObtained\":null,\"DocumentType\":{\"$id\":\"5\",\"id\":3,\"isDeleted\":false,\"name\":\"Document Type Three\"},\"documentTypeId\":3,\"id\":14,\"isDeleted\":false,\"mimeType\":\"application/msword\",\"multerFileName\":\"file-1498565727679\",\"originalFileName\":\"Learning and Development Funding Request Form - Natalie b.doc\",\"profileId\":5},{\"$id\":\"6\",\"dateExpires\":\"2017-06-27T00:00:00\",\"dateObtained\":null,\"DocumentType\":{\"$id\":\"7\",\"id\":2,\"isDeleted\":false,\"name\":\"Document Type Two\"},\"documentTypeId\":2,\"id\":15,\"isDeleted\":false,\"mimeType\":\"application/pdf\",\"multerFileName\":\"file-1498575529566\",\"originalFileName\":\"E--Sites-Webportal-live-user-Certificate.pdf\",\"profileId\":5}],\"ProfileFinance\":{\"$id\":\"8\",\"Bank\":{\"$id\":\"9\",\"id\":2,\"isDeleted\":false,\"name\":\"Bank Two\"},\"bankAccountNumber\":\"3\",\"bankId\":2,\"bankSortCode\":\"3\",\"buildingSocietyRollNumber\":null,\"id\":5,\"isDeleted\":false,\"isIc24Staff\":false,\"nationalInsuranceNumber\":\"3\",\"payrollNumber\":\"3\"},\"profileFinanceId\":5,\"ProfileProfessional\":{\"$id\":\"10\",\"Agency\":{\"$id\":\"11\",\"id\":3,\"isDeleted\":false,\"name\":\"Agency Three\"},\"agencyId\":3,\"Base_\":{\"$id\":\"12\",\"id\":2,\"isDeleted\":false,\"name\":\"Base One\",\"teamId\":1},\"baseId\":2,\"CCG\":{\"$id\":\"13\",\"id\":3,\"isDeleted\":false,\"name\":\"CCG Three\"},\"ccgId\":3,\"gmcNumber\":null,\"hcpcNumber\":null,\"id\":5,\"indemnityExpiryDate\":\"2017-06-27T00:00:00\",\"indemnityNumber\":\"3\",\"IndemnityProvider\":{\"$id\":\"14\",\"id\":3,\"isDeleted\":false,\"name\":\"Indemnity Provider Three\"},\"indemnityProviderId\":3,\"isDeleted\":false,\"isPremium\":false,\"isRegistrarGreen\":false,\"isTrainer\":false,\"JobType\":{\"$id\":\"15\",\"id\":11,\"isClinical\":true,\"isDeleted\":null,\"isGmcRequired\":false,\"isHcpcRequired\":false,\"isNmcRequired\":false,\"name\":\"PP\"},\"jobTypeId\":11,\"nmcNumber\":null,\"performersList\":\"3\",\"performersListChecked\":false,\"performersListCheckedBy\":\"Natalie Brenchley\",\"performersListCheckedDate\":\"2017-06-27T12:14:52\",\"ProfilePaymentCategories\":[{\"$id\":\"16\",\"id\":14,\"isDefault\":false,\"isDeleted\":false,\"paymentCategoryId\":3,\"profileProfessionalId\":5}],\"ProfileShiftTypes\":[],\"RegisteredSurgery\":{\"$id\":\"17\",\"id\":3,\"isDeleted\":false,\"name\":\"Registered Surgery Three\"},\"registeredSurgeryId\":3,\"RegistrarLevel\":{\"$id\":\"18\",\"id\":1,\"isDeleted\":false,\"name\":\"Registrar Level One\"},\"registrarLevelId\":1,\"registrarTrainer\":null,\"registrationExpiryDate\":null,\"SubType\":{\"$id\":\"19\",\"id\":3,\"isAgency\":false,\"isDeleted\":false,\"isRegistrar\":false,\"name\":\"Sub Type Three\"},\"subTypeId\":3,\"teamId\":3},\"profileProfessionalId\":5,\"roleID\":1,\"SecurityRole\":{\"$id\":\"20\",\"createdProfileID\":null,\"isDeleted\":null,\"modifiedProfileID\":null,\"roleId\":1,\"roleName\":\"PERSONNEL\",\"validFrom\":\"2017-07-12T12:27:30.9922098\",\"validTo\":\"9999-12-31T23:59:59.9999999\"},\"smsEnabled\":true,\"SpecialNotes\":[],\"profileDocumentArray\":[{\"documentTypeId\":3,\"documentType\":{\"$id\":\"5\",\"id\":3,\"isDeleted\":false,\"name\":\"Document Type Three\"}}]";
                //var inputMessage = new HttpRequestMessage
                //{
                //    Content = new StringContent(serilized, Encoding.UTF8, "application/json")
                //};
                //inputMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //HttpResponseMessage message = client.PutAsJsonAsync(apiBaseAddress + "/api/lookup/saveTeam", inputMessage.Content).Result;
                var httpContent = new StringContent(serilized, Encoding.UTF8, "application/json");
                var message = await client.PutAsync(apiBaseAddress + "/api/profile", httpContent);
                response.EnsureSuccessStatusCode();


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
    }
}
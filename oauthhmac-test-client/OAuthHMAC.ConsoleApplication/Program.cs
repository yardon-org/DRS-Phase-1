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

                var response = await client.GetAsync(apiBaseAddress + "/api/profile/1");
                var responseString1 = await response.Content.ReadAsStringAsync();

                string serilized = "{\"$id\":\"1\",\"firstName\":\"Test\",\"middleNames\":\"GP\",\"lastName\":\"Doctor\",\"dateOfBirth\":\"1984-06-13T23:00:00\",\"address1\":\"INTEGRATED CARE 24 LTD\",\"address2\":\"KINGSTON HOUSE\",\"address3\":\"THE LONG BARROW\",\"address4\":\"ORBITAL PARK\",\"address5\":\"ASHFORD\",\"postcode\":\"TN24 0GP\",\"homePhone\":\"\",\"mobilePhone\":\"77777777777777777777\",\"homeEmail\":\"doctor@test.test\",\"nhsEmail\":\"doctor@nhs.net\",\"smsEnabled\":true,\"isInactive\":true,\"isComplete\":false,\"id\":1,\"isDeleted\":false,\"profileProfessionalId\":1,\"profileFinanceId\":1,\"adEmailAddress\":\"Peter.Foster1@ic24.nhs.uk\",\"roleID\":1,\"rowVersion\":\"AAAAAAAAC4I=\",\"SpecialNotes\":[],\"ProfileProfessional\":{\"$id\":\"2\",\"teamId\":1,\"jobTypeId\":7,\"gmcNumber\":\"1111111\",\"nmcNumber\":null,\"hcpcNumber\":null,\"registrationExpiryDate\":\"2011-11-11T00:00:00\",\"subTypeId\":1,\"registrarLevelId\":1,\"registrarTrainer\":null,\"isRegistrarGreen\":false,\"agencyId\":1,\"registeredSurgeryId\":3,\"ccgId\":3,\"indemnityProviderId\":3,\"performersList\":\"wwwww\",\"performersListChecked\":false,\"performersListCheckedDate\":\"2017-06-27T10:40:58\",\"performersListCheckedBy\":\"Natalie Brenchley\",\"indemnityNumber\":\"ee\",\"indemnityExpiryDate\":\"2016-07-21T00:00:00\",\"isPremium\":false,\"isTrainer\":false,\"id\":1,\"isDeleted\":false,\"baseId\":null,\"rowVersion\":\"AAAAAAAAC0g=\",\"Agency\":{\"$id\":\"3\",\"id\":1,\"name\":\"Agency One\",\"isDeleted\":false,\"rowVersion\":\"AAAAAAAADB4=\"},\"Base_\":null,\"CCG\":{\"$id\":\"4\",\"id\":3,\"name\":\"CCG Three\",\"isDeleted\":false,\"rowVersion\":\"AAAAAAAACMw=\"},\"IndemnityProvider\":{\"$id\":\"5\",\"id\":3,\"name\":\"Indemnity Provider Three\",\"isDeleted\":false,\"rowVersion\":\"AAAAAAAACNg=\"},\"JobType\":{\"$id\":\"6\",\"name\":\"Doctor\",\"isClinical\":true,\"isGmcRequired\":true,\"isNmcRequired\":null,\"isHcpcRequired\":null,\"id\":7,\"isDeleted\":null,\"rowVersion\":\"AAAAAAAACso=\"},\"ProfilePaymentCategories\":[{\"$id\":\"7\",\"paymentCategoryId\":5,\"profileProfessionalId\":1,\"isDefault\":true,\"id\":3,\"isDeleted\":false,\"rowVersion\":\"AAAAAAAACxA=\",\"PaymentCategory\":{\"$id\":\"8\",\"id\":5,\"name\":\"Brightdoc\",\"isDeleted\":false,\"rowVersion\":\"AAAAAAAACNE=\"}},{\"$id\":\"9\",\"paymentCategoryId\":2,\"profileProfessionalId\":1,\"isDefault\":false,\"id\":4,\"isDeleted\":false,\"rowVersion\":\"AAAAAAAACxE=\",\"PaymentCategory\":{\"$id\":\"10\",\"id\":2,\"name\":\"Sessional\",\"isDeleted\":false,\"rowVersion\":\"AAAAAAAACM4=\"}}],\"SubType\":{\"$id\":\"11\",\"name\":\"Sub Type One\",\"isAgency\":false,\"isRegistrar\":false,\"id\":1,\"isDeleted\":false,\"rowVersion\":\"AAAAAAAACwc=\"},\"ProfileShiftTypes\":[{\"$id\":\"12\",\"shiftTypeId\":1,\"profileProfessionalId\":1,\"isUnderReview\":false,\"id\":8,\"isDeleted\":false,\"rowVersion\":\"AAAAAAAACzE=\",\"ShiftType\":{\"$id\":\"13\",\"id\":1,\"name\":\"Base\",\"isDeleted\":false,\"rowVersion\":\"AAAAAAAACO8=\"}}],\"RegisteredSurgery\":{\"$id\":\"14\",\"id\":3,\"name\":\"Registered Surgery Three\",\"isDeleted\":false,\"rowVersion\":\"AAAAAAAACNs=\"},\"RegistrarLevel\":{\"$id\":\"15\",\"id\":1,\"name\":\"Registrar Level One\",\"isDeleted\":false,\"rowVersion\":\"AAAAAAAACOE=\"}},\"ProfileDocuments\":[{\"$id\":\"16\",\"documentTypeId\":1,\"originalFileName\":\"E--Sites-Webportal-live-user-Certificate.pdf\",\"multerFileName\":\"file-1498560216101\",\"mimeType\":\"application/pdf\",\"dateObtained\":null,\"dateExpires\":\"2015-07-02T00:00:00\",\"profileId\":1,\"id\":1,\"isDeleted\":false,\"rowVersion\":\"AAAAAAAAC50=\",\"DocumentType\":{\"$id\":\"17\",\"id\":1,\"name\":\"Document Type One\",\"isDeleted\":false,\"rowVersion\":\"AAAAAAAACOk=\"}},{\"$id\":\"18\",\"documentTypeId\":3,\"originalFileName\":\"Learning and Development Funding Request Form - Natalie b.doc\",\"multerFileName\":\"file-1498560231835\",\"mimeType\":\"application/msword\",\"dateObtained\":\"2017-02-08T00:00:00\",\"dateExpires\":\"2018-07-18T00:00:00\",\"profileId\":1,\"id\":2,\"isDeleted\":false,\"rowVersion\":\"AAAAAAAAC54=\",\"DocumentType\":{\"$id\":\"19\",\"id\":3,\"name\":\"Document Type Three\",\"isDeleted\":false,\"rowVersion\":\"AAAAAAAACOs=\"}},{\"$id\":\"20\",\"documentTypeId\":2,\"originalFileName\":\"E--Sites-Webportal-live-user-Certificate.pdf\",\"multerFileName\":\"file-1498560216101\",\"mimeType\":\"application/pdf\",\"dateObtained\":\"2014-02-27T00:00:00\",\"dateExpires\":\"2015-07-02T00:00:00\",\"profileId\":1,\"id\":3,\"isDeleted\":false,\"rowVersion\":\"AAAAAAAAC58=\",\"DocumentType\":{\"$id\":\"21\",\"id\":2,\"name\":\"Document Type Two\",\"isDeleted\":false,\"rowVersion\":\"AAAAAAAACOo=\"}}],\"ProfileFinance\":{\"$id\":\"22\",\"payrollNumber\":\"ee\",\"isIc24Staff\":false,\"nationalInsuranceNumber\":\"ee\",\"bankId\":3,\"bankAccountNumber\":\"e\",\"bankSortCode\":\"e\",\"buildingSocietyRollNumber\":null,\"id\":1,\"isDeleted\":false,\"rowVersion\":\"AAAAAAAAC2M=\",\"Bank\":{\"$id\":\"23\",\"id\":3,\"name\":\"Bank Three\",\"isDeleted\":false,\"rowVersion\":\"AAAAAAAADCU=\"}},\"SecurityRole\":{\"$id\":\"24\",\"roleId\":1,\"roleName\":\"PERSONNEL\",\"validFrom\":\"2017-07-12T12:27:30.9922098\",\"validTo\":\"9999-12-31T23:59:59.9999999\",\"createdProfileID\":null,\"modifiedProfileID\":null,\"isDeleted\":null,\"rowVersion\":\"AAAAAAAAC/s=\",\"SecurityPermissions\":[{\"$id\":\"25\",\"permissionId\":1,\"groupId\":1,\"roleId\":1,\"rightId\":1,\"validFrom\":\"2017-07-24T13:02:44.3563213\",\"validTo\":\"9999-12-31T23:59:59.9999999\",\"createdProfileId\":null,\"modifiedProfileId\":null,\"isDeleted\":false,\"rowVersion\":\"AAAAAAAADAU=\",\"SecurityGroup\":{\"$id\":\"26\",\"groupId\":1,\"groupName\":\"SG - DRS Administrator\",\"validFrom\":\"2017-07-12T12:27:30.9922098\",\"validTo\":\"9999-12-31T23:59:59.9999999\",\"rowVersion\":\"AAAAAAAAC/Y=\"},\"SecurityRight\":{\"$id\":\"27\",\"rightID\":1,\"rightName\":\"VIEW\",\"validFrom\":\"2017-07-12T12:27:30.9922098\",\"validTo\":\"9999-12-31T23:59:59.9999999\",\"createdProfileId\":null,\"modifiedProfileId\":null,\"isDeleted\":null,\"rowVersion\":\"AAAAAAAADAE=\"}},{\"$id\":\"28\",\"permissionId\":6,\"groupId\":1,\"roleId\":1,\"rightId\":2,\"validFrom\":\"2017-07-24T13:02:44.3563213\",\"validTo\":\"9999-12-31T23:59:59.9999999\",\"createdProfileId\":null,\"modifiedProfileId\":null,\"isDeleted\":false,\"rowVersion\":\"AAAAAAAADAo=\",\"SecurityGroup\":{\"$id\":\"29\",\"groupId\":1,\"groupName\":\"SG - DRS Administrator\",\"validFrom\":\"2017-07-12T12:27:30.9922098\",\"validTo\":\"9999-12-31T23:59:59.9999999\",\"rowVersion\":\"AAAAAAAAC/Y=\"},\"SecurityRight\":{\"$id\":\"30\",\"rightID\":2,\"rightName\":\"CREATE\",\"validFrom\":\"2017-07-12T12:27:30.9922098\",\"validTo\":\"9999-12-31T23:59:59.9999999\",\"createdProfileId\":null,\"modifiedProfileId\":null,\"isDeleted\":null,\"rowVersion\":\"AAAAAAAADAI=\"}},{\"$id\":\"31\",\"permissionId\":11,\"groupId\":1,\"roleId\":1,\"rightId\":3,\"validFrom\":\"2017-07-24T13:02:44.3563213\",\"validTo\":\"9999-12-31T23:59:59.9999999\",\"createdProfileId\":null,\"modifiedProfileId\":null,\"isDeleted\":false,\"rowVersion\":\"AAAAAAAADA8=\",\"SecurityGroup\":{\"$id\":\"32\",\"groupId\":1,\"groupName\":\"SG - DRS Administrator\",\"validFrom\":\"2017-07-12T12:27:30.9922098\",\"validTo\":\"9999-12-31T23:59:59.9999999\",\"rowVersion\":\"AAAAAAAAC/Y=\"},\"SecurityRight\":{\"$id\":\"33\",\"rightID\":3,\"rightName\":\"EDIT\",\"validFrom\":\"2017-07-12T12:27:30.9922098\",\"validTo\":\"9999-12-31T23:59:59.9999999\",\"createdProfileId\":null,\"modifiedProfileId\":null,\"isDeleted\":null,\"rowVersion\":\"AAAAAAAADAM=\"}},{\"$id\":\"34\",\"permissionId\":16,\"groupId\":1,\"roleId\":1,\"rightId\":4,\"validFrom\":\"2017-07-24T13:02:44.3563213\",\"validTo\":\"9999-12-31T23:59:59.9999999\",\"createdProfileId\":null,\"modifiedProfileId\":null,\"isDeleted\":false,\"rowVersion\":\"AAAAAAAADBQ=\",\"SecurityGroup\":{\"$id\":\"35\",\"groupId\":1,\"groupName\":\"SG - DRS Administrator\",\"validFrom\":\"2017-07-12T12:27:30.9922098\",\"validTo\":\"9999-12-31T23:59:59.9999999\",\"rowVersion\":\"AAAAAAAAC/Y=\"},\"SecurityRight\":{\"$id\":\"36\",\"rightID\":4,\"rightName\":\"DELETE\",\"validFrom\":\"2017-07-12T12:27:30.9922098\",\"validTo\":\"9999-12-31T23:59:59.9999999\",\"createdProfileId\":null,\"modifiedProfileId\":null,\"isDeleted\":null,\"rowVersion\":\"AAAAAAAADAQ=\"}},{\"$id\":\"37\",\"permissionId\":21,\"groupId\":2,\"roleId\":1,\"rightId\":1,\"validFrom\":\"2017-07-24T13:02:44.3563213\",\"validTo\":\"9999-12-31T23:59:59.9999999\",\"createdProfileId\":null,\"modifiedProfileId\":null,\"isDeleted\":false,\"rowVersion\":\"AAAAAAAADBk=\",\"SecurityGroup\":{\"$id\":\"38\",\"groupId\":2,\"groupName\":\"SG - DRS User\",\"validFrom\":\"2017-07-12T12:27:30.9922098\",\"validTo\":\"9999-12-31T23:59:59.9999999\",\"rowVersion\":\"AAAAAAAAC/c=\"},\"SecurityRight\":{\"$id\":\"39\",\"rightID\":1,\"rightName\":\"VIEW\",\"validFrom\":\"2017-07-12T12:27:30.9922098\",\"validTo\":\"9999-12-31T23:59:59.9999999\",\"createdProfileId\":null,\"modifiedProfileId\":null,\"isDeleted\":null,\"rowVersion\":\"AAAAAAAADAE=\"}}]}}";
                //var inputMessage = new HttpRequestMessage
                //{
                //    Content = new StringContent(serilized, Encoding.UTF8, "application/json")
                //};
                //inputMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //HttpResponseMessage message = client.PutAsJsonAsync(apiBaseAddress + "/api/lookup/saveTeam", inputMessage.Content).Result;
                var httpContent = new StringContent(responseString1, Encoding.UTF8, "application/json");
                response = client.PutAsync(apiBaseAddress + "/api/profile", httpContent).Result;
                var responseString = await response.Content.ReadAsStringAsync();
                response.EnsureSuccessStatusCode();


                if (response.IsSuccessStatusCode)
                {
                    //var responseString = await response.Content.ReadAsStringAsync();
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
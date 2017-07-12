using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using drs_backend_phase1.Repository;
using drs_backend_phase1.Services;
using log4net;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;

namespace drs_backend_phase1.Provider
{
    /// <summary>
    /// 
    /// </summary>
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private static readonly string _privateKey = ConfigurationManager.AppSettings["privateKey"];
        private static readonly string _initializeVector = ConfigurationManager.AppSettings["initializeVector"];
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            Log.DebugFormat("Attempting to log in via the internal service...\n");

            // USE THIS TO GET THE ROLE OBJECT FOR THE USER
            // ********************************************
           

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            var _repo = new AuthRepository();

            var decrypted = Decrypt(context.Password);
            var user = await _repo.FindUserAsync(context.UserName, decrypted);

            if (user == null)
            {
                Log.DebugFormat("The username or password is incorrect.\n");
                context.SetError("invalid_grant", "The username or password is incorrect");
                context.Rejected();
            }

            // Fetch the Role for the User
            var securityLayer = new SecurityService();
            var role = securityLayer.FindSecurityRoleByActiveDirectoryEmailAddress(user);

            try
            {
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim("sub", context.UserName));

                PrincipalContext ctx = new PrincipalContext(ContextType.Domain);
                UserPrincipal currentUser = UserPrincipal.FindByIdentity(ctx, context.UserName);

             


                GroupPrincipal liveExceptioningExecGroup = GroupPrincipal.FindByIdentity(ctx, ConfigurationManager.AppSettings["LiveExceptioningExecutiveGroup"]);

                if (user != null)
                {
                    var secretKeyBytes = Encoding.UTF8.GetBytes(user.ActiveDirectorySid);

                    if (currentUser.IsMemberOf(liveExceptioningExecGroup))
                    {
                        var liveExceptioningProps = new AuthenticationProperties(new Dictionary<string, string>
                        {
                            {"clientId", user.ActiveDirectoryGuid},
                            {"clientSecret", Convert.ToBase64String(secretKeyBytes)},
                            {"Forename", currentUser.GivenName},
                            {"LE", "1"}
                        });
                        var ticket = new AuthenticationTicket(identity, liveExceptioningProps);
                        context.Validated(ticket);
                    }

                    if (!currentUser.IsMemberOf(liveExceptioningExecGroup))
                    {
                        var standardProps = new AuthenticationProperties(new Dictionary<string, string>
                        {
                            {"clientId", user.ActiveDirectoryGuid},
                            {"clientSecret", Convert.ToBase64String(secretKeyBytes)},
                            {"Forename", currentUser.GivenName },
                            {"LE", "0" }
                        });

                        var ticket = new AuthenticationTicket(identity, standardProps);
                        context.Validated(ticket);
                    }
                }

           

                Log.DebugFormat("Successfully retrieved active directory credentials for: " + context.UserName);

            }
            catch (Exception ex)
            {
                Log.DebugFormat("An error has occurred, login was unsuccessful for: " + context.UserName + ". The reason is as follows: " + ex.Message, ex.InnerException, ex + "\n");
                context.SetError("An error has occurred, login was unsuccessful for: " + context.UserName + ". The reason is as follows: " + ex.Message, ex + "\n");
            }

        }

        public static AuthenticationProperties CreateProperties(string clientId, string clientSecret)
        {
            IDictionary<string, string> props = new Dictionary<string, string>
            {
                {"clientId", clientId},
                {"clientSecret", clientSecret}
            };

            return new AuthenticationProperties(props);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (var property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public static string Decrypt(string encrypted)
        {
            // Convert the encrypted bytes from the value provided.
            byte[] encryptedBytes = Convert.FromBase64String(encrypted);

            AesCryptoServiceProvider aesServiceProvider = new AesCryptoServiceProvider
            {
                // Set the AES block size to be 128 bits.
                BlockSize = 128,

                // Set the AES key size to be 256 bits.
                KeySize = 256,

                // Ensure that the key provided is looking at the Key variable.
                Key = Encoding.UTF8.GetBytes(_privateKey),
                //Key = Encoding.ASCII.GetBytes("qwertyuiopasdfghjklzxcvbnmqwerty"),

                // Set the AES Initialization Vector to be the IV variable.
                IV = Encoding.UTF8.GetBytes(_initializeVector),
                //IV = Encoding.ASCII.GetBytes("poiuytrewqlkjhgf"),

                // Set the AES Padding to be that of PKCS7 (the value of each added byte is the number of bytes that are added. E.g:
                // 01
                // 02 02
                // 03 03 03
                // 04 04 04 04
                // Etc.
                Padding = PaddingMode.PKCS7,

                // Set the AES Cipher Mode to CBC. In CBC mode, each block of plain text is XORed with the previous cipher block before being encrypted.
                // See the following link for a more detailed explanation (https://en.wikipedia.org/wiki/Block_cipher_mode_of_operation#Cipher_Block_Chaining_.28CBC.29).
                Mode = CipherMode.CBC
            };

            // When decrypting, pass in the AES Key and IV.
            ICryptoTransform cryptoTransform = aesServiceProvider.CreateDecryptor(aesServiceProvider.Key, aesServiceProvider.IV);

            // Transforms the specified region of the input byte array and copies the resulting transform to the specified region of the output byte array.
            byte[] secret = cryptoTransform.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);


            // Chuck the cryptoTransform in the bin, no longer wanted or needed here on out.
            cryptoTransform.Dispose();


            // Return the decrypted value as plain text.
            return Encoding.UTF8.GetString(secret);

        }

    }
}
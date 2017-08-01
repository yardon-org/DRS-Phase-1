using System;
using System.Configuration;
using System.IdentityModel.Tokens;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Thinktecture.IdentityModel.Tokens;

namespace drs_backend_phase1.Provider
{
    /// <summary>
    /// Formats the retuned JWT token
    /// </summary>
    /// <seealso cref="Microsoft.Owin.Security.ISecureDataFormat{Microsoft.Owin.Security.AuthenticationTicket}" />
    public class CustomJwtFormat : ISecureDataFormat<AuthenticationTicket>
    {
        /// <summary>
        /// The issuer
        /// </summary>
        private readonly string _issuer;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomJwtFormat" /> class.
        /// </summary>
        /// <param name="issuer">The issuer.</param>
        public CustomJwtFormat(string issuer)
        {
            _issuer = issuer;
        }

        /// <summary>
        /// Protects the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">data</exception>
        /// <exception cref="System.InvalidOperationException">AuthenticationTicket.Properties does not include audience</exception>
        public string Protect(AuthenticationTicket data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            var audienceId = ConfigurationManager.AppSettings["as:AudienceId"];
            var keyByteArray = TextEncodings.Base64Url.Decode(ConfigurationManager.AppSettings["as:AudienceSecret"]);

            if (string.IsNullOrWhiteSpace(audienceId) || keyByteArray==null) throw new InvalidOperationException("AuthenticationTicket.Properties does not include audience and/or keybyte");
            
            Array.Resize(ref keyByteArray, 48);

            var signingKey = new HmacSigningCredentials(keyByteArray);

            var issued = data.Properties.IssuedUtc;
            var expires = data.Properties.ExpiresUtc;

            var token = new JwtSecurityToken(_issuer, audienceId, data.Identity.Claims, issued.Value.UtcDateTime, expires.Value.UtcDateTime, signingKey);

            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.WriteToken(token);

            return jwt;
        }

        /// <summary>
        /// Unprotects the specified protected text.
        /// </summary>
        /// <param name="protectedText">The protected text.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public AuthenticationTicket Unprotect(string protectedText)
        {
            throw new NotImplementedException();
        }
    }
}
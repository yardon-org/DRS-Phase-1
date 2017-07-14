using System.Data.Entity;

namespace OAuthHMAC.Data.Context
{
    public class AuthContext : DbContext
    {
        public AuthContext() : base("AuthContext")
        {
        }
    }
}
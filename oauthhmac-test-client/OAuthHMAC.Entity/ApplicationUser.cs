using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthHMAC.Entity
{
    public class ApplicationUser
    {
        // Will be used as the clientId
        public string ActiveDirectoryGuid { get; set; }

        // Will be used as the clientSecret
        public string ActiveDirectorySid { get; set; }
    }
}

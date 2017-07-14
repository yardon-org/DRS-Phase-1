using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthHMAC.Data
{
    public class Configuration
    {
        public string ActiveDirectoryServer { get; set; }

        public string ActiveDirectoryRoot { get; set; }

        public string ActiveDirectoryUser { get; set; }

        public string ActiveDirectoryPassword { get; set; }
    }
}

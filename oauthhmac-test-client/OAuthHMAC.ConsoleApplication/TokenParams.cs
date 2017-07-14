using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthHMAC.ConsoleApplication
{
    public class TokenParams
    {
        public string ContentType { get; set; }

        public string AcceptType { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}

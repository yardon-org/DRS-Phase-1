using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthHMAC.ConsoleApplication
{
    public class OrderModel
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public string County { get; set; }
        public Boolean DeliveredPackage { get; set; }
    }
}

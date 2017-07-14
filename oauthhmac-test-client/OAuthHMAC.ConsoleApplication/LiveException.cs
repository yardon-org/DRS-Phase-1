using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OAuthHMAC.ConsoleApplication
{
    public class LiveException
    {
        public DateTimeOffset ExceptionStartDateTime { get; set; }
        public DateTimeOffset? ExceptionEndDateTime { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedDateTime { get; set; }
        public string ReasonText { get; set; }

        public int LiveException111ServiceId { get; set; }
        public int LiveException111KpiId { get; set; }
        public int LiveException111ReasonId { get; set; }
        public int LiveExceptionStatus { get; set; }
    }
}

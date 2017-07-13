using System;
using System.Reflection;
using System.Web.Http;
using drs_backend_phase1.Models;
using log4net;
using System.Linq;
using System.Web.Security;
using drs_backend_phase1.Filter;

namespace drs_backend_phase1.Controllers
{
    /// <summary>
    /// EventLog Controller
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [HMACAuthentication]
    //[Authorize]
    [RoutePrefix("api/event-log")]
   
    public class EventLogController : ApiController
    {
        private readonly DRSEntities _db;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Initializes a new instance of the <see cref="EventLogController"/> class.
        /// </summary>
        public EventLogController()
        {
            _db = new DRSEntities();
        }

        /// <summary>
        /// Reads all event logs.
        /// </summary>
        /// <returns>List of EventLogs</returns>
        [Authorize(Roles = "PERSONNEL")]
        [HttpGet]
        [Route("")]
        public IHttpActionResult ReadAllEventLogs()
        {
            Log.DebugFormat("EventLogController (ReadAllEventLogs)\n");

            try
            {
                var listOfEvents = _db.EventLogs
                .Select(x => new
                    {
                     x.typeKey,
                     x.configId
                    })
                    .ToList();

                Log.DebugFormat("Retrieval of EventLogs was successful.\n");

            

                return Ok(listOfEvents);
            }
            catch (Exception ex)
            {
                Log.DebugFormat($"Error retrieving EventLogs. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving EventLogs. The reason is as follows: {ex.Message}");
            }
        }

        /// <summary>
        /// Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

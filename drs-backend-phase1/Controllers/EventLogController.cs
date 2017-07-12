using System;
using System.Reflection;
using System.Web.Http;
using drs_backend_phase1.Models;
using log4net;
using System.Data.Entity;
using System.Linq;
using drs_backend_phase1.Filter;

namespace drs_backend_phase1.Controllers
{
    /// <summary>
    /// EventLog Controller
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [RoutePrefix("api/event-log")]
    [Authorize]
    [HMACAuthentication]
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
        [HttpGet]
        [Route("")]
        public IHttpActionResult ReadAllEventLogs()
        {
            Log.DebugFormat("EventLogController (ReadAllEventLogs)\n");

            try
            {
                var listOfEvents = _db.EventLogs
                    .Include(b => b.EventLogConfig)
                    .Include(b => b.EventLogType)
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
    }
}

using System;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using drs_backend_phase1.Models;
using log4net;

namespace drs_backend_phase1.Controllers
{
    [RoutePrefix("api/event-log")]
    public class EventLogController : ApiController
    {
        private readonly DRSEntities _db;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        public EventLogController()
        {
            _db = new DRSEntities();
        }

        [HttpGet]
        public IHttpActionResult ReadAllEventLogs()
        {
            Log.DebugFormat("EventLogController (ReadAllEventLogs)\n");

            try
            {
                var listOfEvents = _db.EventLogs.ToList();
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

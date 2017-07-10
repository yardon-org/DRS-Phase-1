using System;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using drs_backend_phase1.Models;
using log4net;

namespace drs_backend_phase1.Controllers
{
    [RoutePrefix("api/paf")]
    public class PafController : ApiController
    {
        private readonly PAFEntities _db;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        public PafController()
        {
            _db = new PAFEntities();
        }

        [HttpGet]
        [Route("findaddresses")]
        public IHttpActionResult QueryPostcode(string postcode)
        {
            Log.DebugFormat("PafController (QueryPostcode)\n");

            try
            {
                var addresses = _db.Get_AddressesForPostCodeSimple(postcode).ToList();
                Log.DebugFormat("Retrieval of Addresses was successful.\n");
                return Ok(addresses);
            }
            catch (Exception ex)
            {
                Log.DebugFormat($"Error retrieving Addresses. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving Addresses. The reason is as follows: {ex.Message}");
            }
        }
    }
}

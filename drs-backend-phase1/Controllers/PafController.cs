using System;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using drs_backend_phase1.Filter;
using drs_backend_phase1.Models;
using log4net;

namespace drs_backend_phase1.Controllers
{
    /// <summary>
    /// PAF Controller
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [RoutePrefix("api/paf")]
    [HMACAuthentication]
    public class PafController : ApiController
    {
        /// <summary>
        /// The database
        /// </summary>
        private readonly PAFEntities _db;
        /// <summary>
        /// The log
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Initializes a new instance of the <see cref="PafController"/> class.
        /// </summary>
        public PafController()
        {
            _db = new PAFEntities();
        }

        /// <summary>
        /// Queries the PAF server for addresses.
        /// </summary>
        /// <param name="postcode">The postcode to query.</param>
        /// <returns>List of Addresses</returns>
       [Authorize(Roles = "PERSONNEL")]
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

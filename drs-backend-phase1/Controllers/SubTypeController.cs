using drs_backend_phase1.Extensions;
using drs_backend_phase1.Models;
using log4net;
using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.OData;

namespace drs_backend_phase1.Controllers
{
    /// <summary>
    /// SubType Controller
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [RoutePrefix("api/sub-type")]
    //[HMACAuthentication]
    public class SubTypeController : ApiController
    {
        /// <summary>
        /// The database
        /// </summary>
        private readonly DRSEntities _db;

        /// <summary>
        /// The log
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the <see cref="SubTypeController"/> class.
        /// </summary>
        public SubTypeController()
        {
            _db = new DRSEntities();
        }

        /// <summary>
        /// Gets the job types o data.
        /// </summary>
        /// <param name="includeDeleted">if set to <c>true</c> [include deleted].</param>
        /// <returns></returns>
        [Authorize(Roles = "PERSONNEL")]
        [EnableQuery(PageSize = 200)]
        [Route("odata")]
        public IQueryable<object> GetSubTypesOData(bool includeDeleted = false)
        {
            Log.DebugFormat("SubTypeController (GetSubTypesOData)\n");

            try
            {
                IQueryable<object> query = _db.SubTypes
                    .Select(
                        p =>
                            new
                            {
                                p.id,
                                p.name,
                                p.isAgency,
                                p.isRegistrar
                            });

                return query.AsQueryable();
            }
            catch (Exception ex)
            {
                Log.DebugFormat($"Error retrieving GetSubTypesOData. The reason is as follows: {ex.Message} {ex.StackTrace}");
            }

            return null;
        }

        /// <summary>
        /// Creates a new SubType.
        /// </summary>
        /// <param name="newSubType">New type of the sub.</param>
        /// <returns>HttpActionResult</returns>
        [Authorize(Roles = "PERSONNEL")]
        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateSubType([FromBody]SubType newSubType)
        {
            if (newSubType != null)
            {
                try
                {
                    _db.SubTypes.Add(newSubType);
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Log.DebugFormat(
                        $"Error creating new SubType. The reason is as follows: {ex.Message} {ex.StackTrace}\n");
                    return BadRequest($"Error creating new SubType. The reason is as follows: {ex.Message}");
                }

                Log.DebugFormat("The new SubType record has been created successfully.\n");
                return Ok(true);
            }

            Log.DebugFormat(
                $"Error creating new SubType. SubType cannot be null\n");
            return BadRequest($"Error creating new SubType. SubType cannot be null");
        }

        /// <summary>
        /// Fetches all SubTypes.
        /// </summary>
        /// <returns>A list of SubTypes</returns>
        [Authorize(Roles = "PERSONNEL")]
        [HttpGet]
        [Route("")]
        public object FetchAllSubTypes(int page = 0, int pageSize = 10)
        {
            Log.DebugFormat("SubTypeController (ReadAllSubTypes)\n");

            try
            {
                var query = _db.SubTypes
                    .Select(
                        p =>
                            new
                            {
                                p.id,
                                p.name,
                                p.isAgency,
                                p.isRegistrar
                            }).OrderBy(x => x.name);
                Log.DebugFormat("Retrieval of SubTypes was successful.\n");
                return query.DoPaging(page, pageSize);
            }
            catch (Exception ex)
            {
                Log.DebugFormat($"Error retrieving SubTypes. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving SubTypes. The reason is as follows: {ex.Message}");
            }
        }

        /// <summary>
        /// Fetches a SubType by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A SubType object</returns>
        [Authorize(Roles = "PERSONNEL")]
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult FetchSubTypeById(int id)
        {
            Log.DebugFormat("SubTypeController (ReadAllSubTypeById)\n");

            try
            {
                var subType = _db.SubTypes.SingleOrDefault(x => x.id == id);
                Log.DebugFormat("Retrieval of ReadAllSubTypeById was successful.\n");
                return Ok(subType);
            }
            catch (Exception ex)
            {
                Log.DebugFormat($"Error retrieving ReadAllSubTypeById. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving ReadAllSubTypeById. The reason is as follows: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates a SubType.
        /// </summary>
        /// <param name="subTypeToUpdate">The sub type to update.</param>
        /// <returns>HttpActionResult</returns>
        [Authorize(Roles = "PERSONNEL")]
        [HttpPut]
        [Route("")]
        public IHttpActionResult UpdateSubType(SubType subTypeToUpdate)
        {
            Log.DebugFormat("SubTypeController (UpdateSubType)\n");

            if (subTypeToUpdate != null)
            {
                try
                {
                    _db.SubTypes.AddOrUpdate(subTypeToUpdate);
                    _db.SaveChanges();

                    Log.DebugFormat("Retrieval of UpdateSubType was successful.\n");
                    return Ok(true);
                }
                catch (Exception ex)
                {
                    Log.DebugFormat(
                        $"Error retrieving UpdateSubType. The reason is as follows: {ex.Message} {ex.StackTrace}");
                    return BadRequest($"Error retrieving UpdateSubType. The reason is as follows: {ex.Message}");
                }
            }

            Log.DebugFormat(
                $"Error updating SubType. SubType cannot be null\n");
            return BadRequest($"Error creating new SubType. SubType cannot be null");
        }

        /// <summary>
        /// Deletes a SubType by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>HttpActionResult</returns>
        [Authorize(Roles = "PERSONNEL")]
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteSubTypeById(int id)
        {
            Log.DebugFormat("SubTypeController (DeleteSubTypeById)\n");

            try
            {
                var subType = _db.SubTypes.SingleOrDefault(x => x.id == id);

                if (subType != null)
                {
                    _db.SubTypes.Remove(subType);
                    _db.SaveChanges();
                }

                Log.DebugFormat("Retrieval of DeleteSubTypeById was successful.\n");
                return Ok(true);
            }
            catch (Exception ex)
            {
                Log.DebugFormat($"Error retrieving DeleteSubTypeById. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving DeleteSubTypeById. The reason is as follows: {ex.Message}");
            }
        }
    }
}
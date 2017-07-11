using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using drs_backend_phase1.Models;
using log4net;

namespace drs_backend_phase1.Controllers
{
    /// <summary>
    /// LookupType Controller
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [RoutePrefix("api/lookuptype")]
    public class LookupTypeController : ApiController
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
        /// Initializes a new instance of the <see cref="LookupTypeController"/> class.
        /// </summary>
        public LookupTypeController()
        {
            _db = new DRSEntities();
        }

        /// <summary>
        /// Creates a new LookupType.
        /// </summary>
        /// <param name="newLookupType">New type of the lookup.</param>
        /// <returns>HttpActionResult</returns>
        [HttpPost]
        public IHttpActionResult CreateLookupType([FromBody]LookupType newLookupType)
        {
            if (newLookupType != null)
            {
                try
                {
                    _db.LookupTypes.Add(newLookupType);
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Log.DebugFormat(
                        $"Error creating new LookupType. The reason is as follows: {ex.Message} {ex.StackTrace}\n");
                    return BadRequest($"Error creating new LookupType. The reason is as follows: {ex.Message}");

                }

                Log.DebugFormat("The new LookupType record has been created successfully.\n");
                return Ok(true);

            }

            Log.DebugFormat(
                $"Error creating new LookupType. LookupType cannot be null\n");
            return BadRequest($"Error creating new LookupType. LookupType cannot be null");
        }

        /// <summary>
        /// Fetches all lookup types.
        /// </summary>
        /// <returns>List of LookupTypes</returns>
        [HttpGet]
        [Route("")]
        public IHttpActionResult FetchAllLookupTypes()
        {   
            Log.DebugFormat("LookupTypeController (ReadAllLookupTypes)\n");

            try
            {
                var listOfLookupTypes = _db.LookupTypes.OrderBy(x=>x.name).ToList();
                Log.DebugFormat("Retrieval of LookupTypes was successful.\n");
                return Ok(listOfLookupTypes);
            }
            catch (Exception ex)
            {
                Log.DebugFormat($"Error retrieving LookupTypes. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving LookupTypes. The reason is as follows: {ex.Message}");
            }
        }

        /// <summary>
        /// Fetches a LookupType by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A LookupType object</returns>
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult FetchLookupTypeById(int id)
        {
            Log.DebugFormat("LookupTypeController (FetchLookupTypeById)\n");

            try
            {
                var lookupType = _db.LookupTypes.SingleOrDefault(x => x.id==id);
                Log.DebugFormat("Retrieval of FetchLookupTypeById was successful.\n");
                return Ok(lookupType);
            }
            catch (Exception ex)
            {
                Log.DebugFormat($"Error retrieving FetchLookupTypeById. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving FetchLookupTypeById. The reason is as follows: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates a LookupType object.
        /// </summary>
        /// <param name="lookupTypeToUpdate">The lookup type to update.</param>
        /// <returns>HttpActionResult</returns>
        [HttpPut]
        public IHttpActionResult UpdateLookupType(LookupType lookupTypeToUpdate)
        {
            Log.DebugFormat("LookupTypeController (UpdateLookupType)\n");

            if (lookupTypeToUpdate != null)
            {
                try
                {
                    _db.LookupTypes.AddOrUpdate(lookupTypeToUpdate);
                    _db.SaveChanges();

                    Log.DebugFormat("Retrieval of UpdateLookupType was successful.\n");
                    return Ok(true);
                }
                catch (Exception ex)
                {
                    Log.DebugFormat(
                        $"Error retrieving UpdateLookupType. The reason is as follows: {ex.Message} {ex.StackTrace}");
                    return BadRequest($"Error retrieving UpdateLookupType. The reason is as follows: {ex.Message}");
                }
            }

            Log.DebugFormat(
                $"Error updating LookupType. LookupType cannot be null\n");
            return BadRequest($"Error creating new LookupType. LookupType cannot be null");

        }

        /// <summary>
        /// Deletes a LookupType by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>HttpActionResult</returns>
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteLookupTypeById(int id)
        {
            Log.DebugFormat("LookupTypeController (DeleteLookupTypeById)\n");

            try
            {
                var lookupType = _db.LookupTypes.SingleOrDefault(x => x.id == id);

                if (lookupType != null)
                {
                    _db.LookupTypes.Remove(lookupType);
                    _db.SaveChanges();
                }

                Log.DebugFormat("Retrieval of DeleteLookupTypeById was successful.\n");
                return Ok(true);
            }
            catch (Exception ex)
            {
                Log.DebugFormat($"Error retrieving DeleteLookupTypeById. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving DeleteLookupTypeById. The reason is as follows: {ex.Message}");
            }
        }
    }
}

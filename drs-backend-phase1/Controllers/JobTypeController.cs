using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using drs_backend_phase1.Filter;
using drs_backend_phase1.Models;
using log4net;

namespace drs_backend_phase1.Controllers
{
    /// <summary>
    /// JobType Controller
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [RoutePrefix("api/job-type")]
    [HMACAuthentication]
    public class JobTypeController : ApiController
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
        /// Initializes a new instance of the <see cref="JobTypeController"/> class.
        /// </summary>
        public JobTypeController()
        {
            _db = new DRSEntities();
        }

        /// <summary>
        /// Adds a new JobType to the database.
        /// </summary>
        /// <param name="newJobType">New type of the job.</param>
       [Authorize(Roles = "PERSONNEL")]
        [HttpPost]
        [Route("")]
        public void CreateJobType([FromBody]JobType newJobType)
        {
            if (newJobType != null)
            {
                try
                {
                    _db.JobTypes.Add(newJobType);
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Log.DebugFormat(
                        $"Error creating new JobType. The reason is as follows: {ex.Message} {ex.StackTrace}\n");
                }

                Log.DebugFormat("The new JobType record has been created successfully.\n");
            }

            Log.DebugFormat(
                $"Error creating new JobType. JobType cannot be null\n");
        }

        /// <summary>
        /// Fetches all job types.
        /// </summary>
        /// <returns>List of all JobTypes</returns>
       [Authorize(Roles = "PERSONNEL")]
        [HttpGet]
        [Route("")]
        public IHttpActionResult FetchAllJobTypes()
        {   
            Log.DebugFormat("JobTypeController (ReadAllJobTypes)\n");

            try
            {
                var listOfJobTypes = _db.JobTypes.OrderBy(x=>x.name).ToList();
                Log.DebugFormat("Retrieval of JobTypes was successful.\n");
                return Ok(listOfJobTypes);
            }
            catch (Exception ex)
            {
                Log.DebugFormat($"Error retrieving JobTypes. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving JobTypes. The reason is as follows: {ex.Message}");
            }
        }

        /// <summary>
        /// Fetches the job type by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A JobType</returns>
       [Authorize(Roles = "PERSONNEL")]
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult FetchJobTypeById(int id)
        {
            Log.DebugFormat("JobTypeController (ReadAllJobTypeById)\n");

            try
            {
                var jobType = _db.JobTypes.SingleOrDefault(x => x.id==id);
                Log.DebugFormat("Retrieval of ReadAllJobTypeById was successful.\n");
                return Ok(jobType);
            }
            catch (Exception ex)
            {
                Log.DebugFormat($"Error retrieving ReadAllJobTypeById. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving ReadAllJobTypeById. The reason is as follows: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the JobType.
        /// </summary>
        /// <param name="jobTypeToUpdate">The job type to update.</param>
        /// <returns>HttpActionResult</returns>
       [Authorize(Roles = "PERSONNEL")]
        [HttpPut]
        [Route("")]
        public IHttpActionResult UpdateJobType(JobType jobTypeToUpdate)
        {
            Log.DebugFormat("JobTypeController (UpdateJobType)\n");

            if (jobTypeToUpdate != null)
            {
                try
                {
                    _db.JobTypes.AddOrUpdate(jobTypeToUpdate);
                    _db.SaveChanges();

                    Log.DebugFormat("Retrieval of UpdateJobType was successful.\n");
                    return Ok(true);
                }
                catch (Exception ex)
                {
                    Log.DebugFormat(
                        $"Error retrieving UpdateJobType. The reason is as follows: {ex.Message} {ex.StackTrace}");
                    return BadRequest($"Error retrieving UpdateJobType. The reason is as follows: {ex.Message}");
                }
            }

            Log.DebugFormat(
                $"Error updating JobType. JobType cannot be null\n");
            return BadRequest($"Error creating new JobType. JobType cannot be null");

        }

        /// <summary>
        /// Deletes the job type by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>HttpActionResult</returns>
       [Authorize(Roles = "PERSONNEL")]
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteJobTypeById(int id)
        {
            Log.DebugFormat("JobTypeController (DeleteJobTypeById)\n");

            try
            {
                var jobType = _db.JobTypes.SingleOrDefault(x => x.id == id);

                if (jobType != null)
                {
                    _db.JobTypes.Remove(jobType);
                    _db.SaveChanges();
                }

                Log.DebugFormat("Retrieval of DeleteJobTypeById was successful.\n");
                return Ok(true);
            }
            catch (Exception ex)
            {
                Log.DebugFormat($"Error retrieving DeleteJobTypeById. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving DeleteJobTypeById. The reason is as follows: {ex.Message}");
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

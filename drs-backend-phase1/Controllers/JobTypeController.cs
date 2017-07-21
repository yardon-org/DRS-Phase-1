using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.OData;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using drs_backend_phase1.Extensions;
using drs_backend_phase1.Models;
using drs_backend_phase1.Models.DTOs;
using log4net;

namespace drs_backend_phase1.Controllers
{
    /// <summary>
    ///     JobType Controller
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [RoutePrefix("api/job-type")]
    //[HMACAuthentication]
    public class JobTypeController : ApiController
    {
        /// <summary>
        ///     The log
        /// </summary>
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///     The database
        /// </summary>
        private readonly DRSEntities _db;


        /// <summary>
        ///     Initializes a new instance of the <see cref="JobTypeController" /> class.
        /// </summary>
        public JobTypeController()
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
        public IQueryable<object> GetJobTypesOData(bool includeDeleted = false)
        {
            Log.DebugFormat("JobTypeController (GetJobTypesOData)\n");

            try
            {
                IQueryable<object> query = _db.EventLogs
                    .Select(x => new
                    {
                        x.typeKey,
                        x.configId
                    });

                return query.AsQueryable();
            }
            catch (Exception ex)
            {
                Log.DebugFormat($"Error retrieving GetJobTypesOData. The reason is as follows: {ex.Message} {ex.StackTrace}");
            }

            return null;
        }

        /// <summary>
        ///     Adds a new JobType to the database.
        /// </summary>
        /// <param name="newJobType">New type of the job.</param>
      [Authorize(Roles = "PERSONNEL")]
        [HttpPost]
        [Route("")]
        public void CreateJobType(JobTypeDTO newJobType)
        {
            if (newJobType != null)
            {
                try
                {
                    var localJobTypeToUpdate = Mapper.Map<JobType>(newJobType);
                    _db.JobTypes.Add(localJobTypeToUpdate);
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
        ///     Fetches all job types.
        /// </summary>
        /// <returns>List of all JobTypes</returns>
      [Authorize(Roles = "PERSONNEL")]
        [HttpGet]
        [Route("")]
        public IHttpActionResult FetchAllJobTypes(int page = 1, int pageSize = 10)
        {
            try
            {
                Log.DebugFormat("JobTypeController (ReadAllJobTypes)\n");

                var jobtypes = _db.JobTypes.OrderBy(x=>x.id).ToPagedList(page, pageSize).ToMappedPagedList<JobType, JobTypeDTO>();
                return Ok(new { metaData = jobtypes.GetMetaData(), items = jobtypes });
            }
            catch (Exception ex)
            {
                Log.DebugFormat($"Error retrieving JobTypes. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving JobTypes. The reason is as follows: {ex.Message}");
            }
        }

        /// <summary>
        ///     Fetches the job type by identifier.
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
                var jobType = _db.JobTypes.OrderBy(x => x.id).Where(p => p.id == id).ProjectTo<JobTypeDTO>().SingleOrDefault();
                JobTypeDTO dto = Mapper.Map<JobTypeDTO>(jobType);
                Log.DebugFormat("Retrieval of ReadAllJobTypeById was successful.\n");
                return Ok(dto);
            }
            catch (Exception ex)
            {
                Log.DebugFormat(
                    $"Error retrieving ReadAllJobTypeById. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving ReadAllJobTypeById. The reason is as follows: {ex.Message}");
            }
        }

        /// <summary>
        ///     Updates the JobType.
        /// </summary>
        /// <param name="jobTypeToUpdate">The job type to update.</param>
        /// <returns>HttpActionResult</returns>
      [Authorize(Roles = "PERSONNEL")]
        [HttpPut]
        [Route("")]
        public IHttpActionResult UpdateJobType(JobTypeDTO jobTypeToUpdate)
        {
            Log.DebugFormat("JobTypeController (UpdateJobType)\n");

            if (jobTypeToUpdate != null)
                try
                {
                    var localJobTypeToUpdate = Mapper.Map<JobType>(jobTypeToUpdate);
                    _db.JobTypes.AddOrUpdate(localJobTypeToUpdate);
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

            Log.DebugFormat(
                $"Error updating JobType. JobType cannot be null\n");
            return BadRequest($"Error creating new JobType. JobType cannot be null");
        }

        /// <summary>
        ///     Deletes the job type by identifier.
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
                Log.DebugFormat(
                    $"Error retrieving DeleteJobTypeById. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving DeleteJobTypeById. The reason is as follows: {ex.Message}");
            }
        }

        /// <summary>
        ///     Releases the unmanaged resources that are used by the object and, optionally, releases the managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     true to release both managed and unmanaged resources; false to release only unmanaged
        ///     resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _db.Dispose();
            base.Dispose(disposing);
        }
    }
}
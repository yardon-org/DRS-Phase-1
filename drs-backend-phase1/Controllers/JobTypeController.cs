using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using drs_backend_phase1.Models;
using log4net;

namespace drs_backend_phase1.Controllers
{
    [RoutePrefix("api/job-type")]
    public class JobTypeController : ApiController
    {
        private readonly DRSEntities _db;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    
        public JobTypeController()
        {
            _db = new DRSEntities();
        }

        [HttpPost]
        public IHttpActionResult CreateJobType(JobType newJobType)
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
                    return BadRequest($"Error creating new JobType. The reason is as follows: {ex.Message}");

                }

                Log.DebugFormat("The new JobType record has been created successfully.\n");
                return Ok();

            }

            Log.DebugFormat(
                $"Error creating new JobType. JobType cannot be null\n");
            return BadRequest($"Error creating new JobType. JobType cannot be null");
        }

        [HttpGet]
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

        [HttpPut]
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
                    return Ok(jobTypeToUpdate);
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
                return Ok(jobType);
            }
            catch (Exception ex)
            {
                Log.DebugFormat($"Error retrieving DeleteJobTypeById. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving DeleteJobTypeById. The reason is as follows: {ex.Message}");
            }
        }
    }
}

using System;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using drs_backend_phase1.Models;
using log4net;
using Microsoft.VisualBasic.Logging;

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
        public IHttpActionResult ReadAllJobTypes()
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
        public IHttpActionResult ReadAllJobTypeById(int id)
        {
            Log.DebugFormat("JobTypeController (ReadAllJobTypeById)\n");

            try
            {
                var jobType = _db.JobTypes.Where(x=>x.id==id);
                Log.DebugFormat("Retrieval of ReadAllJobTypeById was successful.\n");
                return Ok(jobType);
            }
            catch (Exception ex)
            {
                Log.DebugFormat($"Error retrieving ReadAllJobTypeById. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving ReadAllJobTypeById. The reason is as follows: {ex.Message}");
            }
        }
    }
}

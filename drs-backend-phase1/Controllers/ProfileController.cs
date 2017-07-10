using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using drs_backend_phase1.Models;
using log4net;

namespace drs_backend_phase1.Controllers
{
    [RoutePrefix("api/profile")]
    public class ProfileController : ApiController
    {
        private readonly DRSEntities _db;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    
        public ProfileController()
        {
            _db = new DRSEntities();
        }

        [HttpGet]
        [Route("fetchProfiles")]
        public IHttpActionResult FetchAllProfiles(bool includeDeleted)
        {   
            Log.DebugFormat("ProfileController (ReadAllProfiles)\n");

            try
            {
                var listOfProfiles = _db.Profiles
                    .Where(x=>(x.isDeleted==null || x.isDeleted==false) || includeDeleted && x.isDeleted==true)
                    .Include(b => b.ProfileProfessional)
                    .Include(b => b.ProfileFinance)
                    .Include(b => b.SpecialNotes)
                    .Include(b => b.ProfileProfessional.JobType)
                    .Include(b => b.ProfileProfessional.SubType)
                    .Include(b => b.ProfileDocuments)
                    .ToList();

                Log.DebugFormat("Retrieval of Profiles was successful.\n");
                return Ok(listOfProfiles);
            }
            catch (Exception ex)
            {
                Log.DebugFormat($"Error retrieving Profiles. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving Profiles. The reason is as follows: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult FetchProfileById(int id)
        {
            Log.DebugFormat("ProfileController (ReadAllProfileById)\n");

            try
            {
                var Profile = _db.Profiles.SingleOrDefault(x => x.id==id);
                Log.DebugFormat("Retrieval of ReadAllProfileById was successful.\n");
                return Ok(Profile);
            }
            catch (Exception ex)
            {
                Log.DebugFormat($"Error retrieving ReadAllProfileById. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving ReadAllProfileById. The reason is as follows: {ex.Message}");
            }
        }

        [HttpPut]
        public IHttpActionResult UpdateProfile(Profile profileToUpdate)
        {
            Log.DebugFormat("ProfileController (UpdateProfile)\n");

            if (profileToUpdate != null)
            {
                try
                {
                    _db.Profiles.AddOrUpdate(profileToUpdate);
                    _db.SaveChanges();

                    Log.DebugFormat("Retrieval of UpdateProfile was successful.\n");
                    return Ok(profileToUpdate);
                }
                catch (Exception ex)
                {
                    Log.DebugFormat(
                        $"Error retrieving UpdateProfile. The reason is as follows: {ex.Message} {ex.StackTrace}");
                    return BadRequest($"Error retrieving UpdateProfile. The reason is as follows: {ex.Message}");
                }
            }

            Log.DebugFormat(
                $"Error updating Profile. Profile cannot be null\n");
            return BadRequest($"Error creating new Profile. Profile cannot be null");

        }

        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteProfileById(int id)
        {
            Log.DebugFormat("ProfileController (DeleteProfileById)\n");

            try
            {
                var Profile = _db.Profiles.SingleOrDefault(x => x.id == id);

                if (Profile != null)
                {
                    _db.Profiles.Remove(Profile);
                    _db.SaveChanges();
                }

                Log.DebugFormat("Retrieval of DeleteProfileById was successful.\n");
                return Ok(Profile);
            }
            catch (Exception ex)
            {
                Log.DebugFormat($"Error retrieving DeleteProfileById. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving DeleteProfileById. The reason is as follows: {ex.Message}");
            }
        }
    }
}

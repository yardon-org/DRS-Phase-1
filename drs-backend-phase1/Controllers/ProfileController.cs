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
    ///     Profile Controller
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [RoutePrefix("api/profile")]
    [Authorize]
    [HMACAuthentication]
    public class ProfileController : ApiController
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
        ///     Initializes a new instance of the <see cref="ProfileController" /> class.
        /// </summary>
        public ProfileController()
        {
            _db = new DRSEntities();
        }

        /// <summary>
        ///     Fetches all profiles.
        /// </summary>
        /// <param name="includeDeleted">if set to <c>true</c> [include deleted].</param>
        /// <returns>List of Profiles</returns>
        [HttpGet]
        [Route("fetchProfiles")]
        public IHttpActionResult FetchAllProfiles(bool includeDeleted)
        {
            Log.DebugFormat("ProfileController (ReadAllProfiles)\n");

            try
            {
                var listOfProfiles = _db.Profiles
                    .Select(x => new
                    {
                        x.SpecialNotes,
                        x.ProfileProfessional.JobType,
                        x.ProfileProfessional.SubType,
                        x.ProfileDocuments
                    }).ToList();

                Log.DebugFormat("Retrieval of Profiles was successful.\n");
                return Ok(listOfProfiles);
            }
            catch (Exception ex)
            {
                Log.DebugFormat($"Error retrieving Profiles. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving Profiles. The reason is as follows: {ex.Message}");
            }
        }

        /// <summary>
        ///     Fetches a Profile by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A Profile object</returns>
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult FetchProfileById(int id)
        {
            Log.DebugFormat("ProfileController (ReadAllProfileById)\n");

            try
            {
                var profile = _db.Profiles.SingleOrDefault(x => x.id == id);
                Log.DebugFormat("Retrieval of ReadAllProfileById was successful.\n");
                return Ok(profile);
            }
            catch (Exception ex)
            {
                Log.DebugFormat(
                    $"Error retrieving ReadAllProfileById. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving ReadAllProfileById. The reason is as follows: {ex.Message}");
            }
        }

        /// <summary>
        ///     Updates a Profile.
        /// </summary>
        /// <param name="profileToUpdate">The Profile to update.</param>
        /// <returns>HttpActionResult</returns>
        [HttpPut]
        [Route("")]
        public IHttpActionResult UpdateProfile(Profile profileToUpdate)
        {
            Log.DebugFormat("ProfileController (UpdateProfile)\n");

            if (profileToUpdate != null)
                try
                {
                    _db.Profiles.AddOrUpdate(profileToUpdate);
                    _db.SaveChanges();

                    Log.DebugFormat("Retrieval of UpdateProfile was successful.\n");
                    return Ok(true);
                }
                catch (Exception ex)
                {
                    Log.DebugFormat(
                        $"Error retrieving UpdateProfile. The reason is as follows: {ex.Message} {ex.StackTrace}");
                    return BadRequest($"Error retrieving UpdateProfile. The reason is as follows: {ex.Message}");
                }

            Log.DebugFormat(
                $"Error updating Profile. Profile cannot be null\n");
            return BadRequest($"Error creating new Profile. Profile cannot be null");
        }


        /// <summary>
        /// Checks the performers list.
        /// </summary>
        /// <param name="profileToUpdate">The profile to update.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("performers")]
        public IHttpActionResult CheckPerformersList(Profile profileToUpdate)
        {
            Log.DebugFormat("ProfileController (CheckPerformersList)\n");

            if (profileToUpdate != null)
                try
                {
                    profileToUpdate.ProfileProfessional.performersListCheckedDate = DateTime.Now;
                    profileToUpdate.ProfileProfessional.performersListCheckedBy = User.Identity.Name;
                    _db.Profiles.AddOrUpdate(profileToUpdate);
                    _db.SaveChanges();

                    // TODO: Add to eventlog here

                    Log.DebugFormat("Retrieval of CheckPerformersList was successful.\n");
                    return Ok(true);
                }
                catch (Exception ex)
                {
                    Log.DebugFormat(
                        $"Error retrieving CheckPerformersList. The reason is as follows: {ex.Message} {ex.StackTrace}");
                    return BadRequest($"Error retrieving CheckPerformersList. The reason is as follows: {ex.Message}");
                }

            Log.DebugFormat(
                $"Error updating Profile. Profile cannot be null\n");
            return BadRequest($"Error creating new Profile. Profile cannot be null");
        }

        /// <summary>
        /// Fetches the last name of the many by first or.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="includeDeleted">if set to <c>true</c> [include deleted].</param>
        /// <returns></returns>
        [HttpGet]
        [Route("search/{searchTerm}/{includeDeleted}")]
        public IHttpActionResult FetchManyByFirstOrLastName(string searchTerm, bool includeDeleted = false)
        {
            Log.DebugFormat("ProfileController (FetchManyByFirstOrLastName)\n");

            try
            {
                var listOfProfiles = _db.Profiles
                    .Where(x => (x.firstName.ToLower().Contains(searchTerm.ToLower()) ||
                                 x.lastName.ToLower().Contains(searchTerm.ToLower())) &&
                                (x.isDeleted == null || x.isDeleted == false) || includeDeleted && x.isDeleted == true)
                    .Select(x => new
                    {
                        x.ProfileProfessional.JobType,
                        x.ProfileProfessional.SubType,
                        x.ProfileDocuments,
                        x.SpecialNotes,
                        x.ProfileProfessional.teamId,
                        x.ProfileProfessional.registrarLevelId,
                        x.ProfileProfessional.agencyId,
                        x.ProfileProfessional.registeredSurgeryId,
                        x.ProfileProfessional.ccgId,
                        x.ProfileProfessional.indemnityProviderId,
                        x.ProfileFinance.bankId
                    })
                    .ToList();


                Log.DebugFormat("Retrieval of FetchManyByFirstOrLastName was successful.\n");
                return Ok(listOfProfiles);
            }
            catch (Exception ex)
            {
                Log.DebugFormat(
                    $"Error retrieving FetchManyByFirstOrLastName. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest(
                    $"Error retrieving FetchManyByFirstOrLastName. The reason is as follows: {ex.Message}");
            }
        }

        /// <summary>
        /// Fetches the many by team identifier.
        /// </summary>
        /// <param name="teamId">The team identifier.</param>
        /// <param name="includeDeleted">if set to <c>true</c> [include deleted].</param>
        /// <returns></returns>
        [HttpGet]
        [Route("filter/{teamId}/{includeDeleted}")]
        public IHttpActionResult FetchManyByTeamId(int teamId, bool includeDeleted = false)
        {
            Log.DebugFormat("ProfileController (FetchManyByTeamId)\n");

            try
            {
                var listOfProfiles = _db.Profiles
                    .Where(x => x.ProfileProfessional.teamId == teamId &&
                                (x.isDeleted == null || x.isDeleted == false) || includeDeleted && x.isDeleted == true)
                    .Select(x => new
                     {
                         x.ProfileProfessional.JobType,
                         x.ProfileProfessional.SubType,
                         x.ProfileDocuments,
                         x.SpecialNotes,
                         x.ProfileProfessional.teamId,
                         x.ProfileProfessional.registrarLevelId,
                         x.ProfileProfessional.agencyId,
                         x.ProfileProfessional.registeredSurgeryId,
                         x.ProfileProfessional.ccgId,
                         x.ProfileProfessional.indemnityProviderId,
                         x.ProfileFinance.bankId
                     })
                    .ToList();


                Log.DebugFormat("Retrieval of FetchManyByTeamId was successful.\n");
                return Ok(listOfProfiles);
            }
            catch (Exception ex)
            {
                Log.DebugFormat(
                    $"Error retrieving FetchManyByTeamId. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest(
                    $"Error retrieving FetchManyByTeamId. The reason is as follows: {ex.Message}");
            }
        }



        /// <summary>
        ///     Deletes a Profile by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>HttpActionResult</returns>
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteProfileById(int id)
        {
            Log.DebugFormat("ProfileController (DeleteProfileById)\n");

            try
            {
                var profile = _db.Profiles.SingleOrDefault(x => x.id == id);

                if (profile != null)
                {
                    _db.Profiles.Remove(profile);
                    _db.SaveChanges();
                }

                Log.DebugFormat("Retrieval of DeleteProfileById was successful.\n");
                return Ok(true);
            }
            catch (Exception ex)
            {
                Log.DebugFormat(
                    $"Error retrieving DeleteProfileById. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving DeleteProfileById. The reason is as follows: {ex.Message}");
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
using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.OData;
using AutoMapper;
using drs_backend_phase1.Extensions;
using drs_backend_phase1.Models;
using drs_backend_phase1.Models.DTOs;
using log4net;
using Profile = drs_backend_phase1.Models.Profile;

namespace drs_backend_phase1.Controllers
{
    /// <summary>
    ///     Profile Controller
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [RoutePrefix("api/profile")]
    //[HMACAuthentication]
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
        ///     Checks the performers list.
        /// </summary>
        /// <param name="profileToUpdate">The profile to update.</param>
        /// <returns></returns>
        [Authorize(Roles = "PERSONNEL")]
        [HttpPut]
        [Route("performers")]
        public IHttpActionResult CheckPerformersList(ProfileDTO incomingProfileDTO)
        {
            Log.DebugFormat("ProfileController (CheckPerformersList)\n");


            var fetchedProfile = _db.Profiles.SingleOrDefault(x => x.id == incomingProfileDTO.id);
            var profileToUpdate = Mapper.Map(incomingProfileDTO, fetchedProfile);

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
        ///     Deletes a Profile by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>HttpActionResult</returns>
        [Authorize(Roles = "PERSONNEL")]
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
        ///     Fetches all profiles.
        /// </summary>
        /// <param name="includeDeleted">if set to <c>true</c> [include deleted].</param>
        /// <param name="page">No. of Pages</param>
        /// <param name="pageSize">No. of Items per Page</param>
        /// <returns>List of Profiles</returns>
        [Authorize(Roles = "PERSONNEL")]
        [HttpGet]
        [Route("fetchProfiles")]
        public IHttpActionResult FetchAllProfiles(bool includeDeleted = false, int page = 1, int pageSize = 10)
        {
            Log.DebugFormat("ProfileController (ReadAllProfiles)\n");

            try
            {
                var profs = _db.Profiles
                    .Where(p => p.isDeleted == false || includeDeleted && p.isDeleted)
                    .OrderBy(x => x.id)
                    .ToPagedList(page, pageSize).ToMappedPagedList<Profile, ProfileDTO>();

                return Ok(new { metaData = profs.GetMetaData(), items = profs });
            }
            catch (Exception ex)
            {
                Log.DebugFormat($"Error retrieving Profiles. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving Profiles. The reason is as follows: {ex.Message}");
            }
        }

        /// <summary>
        ///     Fetches the many by team identifier.
        /// </summary>
        /// <param name="teamId">The team identifier.</param>
        /// <param name="includeDeleted">if set to <c>true</c> [include deleted].</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        [Authorize(Roles = "PERSONNEL")]
        [HttpGet]
        [Route("filter/{teamId}/{includeDeleted}")]
        public IHttpActionResult FetchManyByTeamId(int teamId, bool includeDeleted = false, int page = 1,
            int pageSize = 10)
        {
            Log.DebugFormat("ProfileController (FetchManyByTeamId)\n");

            try
            {
                var profs = _db.Profiles
                    .Where(x => x.ProfileProfessional.teamId == teamId &&
                                x.isDeleted == false || includeDeleted && x.isDeleted)
                    .OrderBy(x => x.id)
                    .ToPagedList(page, pageSize).ToMappedPagedList<Profile, ProfileDTO>();

                return Ok(new { metaData = profs.GetMetaData(), items = profs });
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
        ///     Fetches a Profile by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A Profile object</returns>
        [Authorize(Roles = "PERSONNEL")]
        [HttpGet]
        [Route("{id}")]
        public IHttpActionResult FetchProfileById(int id)
        {
            Log.DebugFormat("ProfileController (ReadAllProfileById)\n");

            try
            {
                var profile = _db.Profiles
                    .Where(p => p.id == id).OrderBy(x => x.id).SingleOrDefault();

                var dto = Mapper.Map<ProfileDTO>(profile);
                Log.DebugFormat("Retrieval of ReadAllProfileById was successful.\n");

                return Ok(dto);
            }
            catch (Exception ex)
            {
                Log.DebugFormat(
                    $"Error retrieving ReadAllProfileById. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving ReadAllProfileById. The reason is as follows: {ex.Message}");
            }
        }

        /// <summary>
        ///     Gets the profiles.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "PERSONNEL")]
        [EnableQuery(PageSize = 200)]
        [Route("odata")]
        public IQueryable<object> GetProfilesOData(bool includeDeleted = false)
        {
            Log.DebugFormat("ProfileController (ReadAllProfiles)\n");

            try
            {
                IQueryable<object> query = _db.Profiles
                    .Where(p => p.isDeleted == false || includeDeleted && p.isDeleted)
                    .Select(
                        p =>
                            new
                            {
                                p.firstName,
                                p.middleNames,
                                p.lastName,
                                p.dateOfBirth,
                                p.address1,
                                p.address2,
                                p.address3,
                                p.address4,
                                p.address5,
                                p.postcode,
                                p.homePhone,
                                p.mobilePhone,
                                p.homeEmail,
                                p.nhsEmail,
                                p.smsEnabled,
                                p.isInactive,
                                p.isComplete,
                                p.id,
                                p.dateCreated,
                                p.dateModified,
                                p.isDeleted,
                                p.profileProfessionalId,
                                p.profileFinanceId,
                                p.adEmailAddress,
                                role = new
                                {
                                    p.SecurityRole.roleId,
                                    p.SecurityRole.roleName,
                                    p.SecurityRole.isDeleted
                                },
                                finance = new
                                {
                                    p.ProfileFinance.id,
                                    p.ProfileFinance.payrollNumber,
                                    p.ProfileFinance.isIc24Staff,
                                    p.ProfileFinance.bankId,
                                    p.ProfileFinance.bankSortCode,
                                    p.ProfileFinance.bankAccountNumber,
                                    p.ProfileFinance.buildingSocietyRollNumber,
                                    p.ProfileFinance.nationalInsuranceNumber,
                                    p.ProfileFinance.isDeleted
                                },
                                professional = new
                                {
                                    p.ProfileProfessional.id,
                                    p.ProfileProfessional.gmcNumber,
                                    p.ProfileProfessional.hcpcNumber,
                                    p.ProfileProfessional.indemnityExpiryDate,
                                    p.ProfileProfessional.isPremium,
                                    p.ProfileProfessional.isTrainer,
                                    p.ProfileProfessional.nmcNumber,
                                    p.ProfileProfessional.performersListChecked,
                                    p.ProfileProfessional.registrarTrainer,
                                    ccg = new
                                    {
                                        p.ProfileProfessional.CCG.id,
                                        p.ProfileProfessional.CCG.name,
                                        p.ProfileProfessional.CCG.isDeleted
                                    },
                                    registrarLevel = new
                                    {
                                        p.ProfileProfessional.RegistrarLevel.id,
                                        p.ProfileProfessional.RegistrarLevel.name,
                                        p.ProfileProfessional.RegistrarLevel.isDeleted
                                    },
                                    @base = new
                                    {
                                        p.ProfileProfessional.Base.id,
                                        p.ProfileProfessional.Base.name,
                                        p.ProfileProfessional.Base.isDeleted
                                    },
                                    indemnityProvider = new
                                    {
                                        p.ProfileProfessional.IndemnityProvider.id,
                                        p.ProfileProfessional.IndemnityProvider.name,
                                        p.ProfileProfessional.IndemnityProvider.isDeleted
                                    },
                                    registeredSurgery = new
                                    {
                                        p.ProfileProfessional.RegisteredSurgery.id,
                                        p.ProfileProfessional.RegisteredSurgery.name,
                                        p.ProfileProfessional.RegisteredSurgery.isDeleted
                                    },
                                    agency = new
                                    {
                                        p.ProfileProfessional.Agency.id,
                                        p.ProfileProfessional.Agency.name,
                                        p.ProfileProfessional.Agency.isDeleted
                                    },
                                    team = new
                                    {
                                        p.ProfileProfessional.Base.Team.id,
                                        p.ProfileProfessional.Base.Team.name,
                                        p.ProfileProfessional.Base.Team.isDeleted
                                    },
                                    jobRole = new
                                    {
                                        p.ProfileProfessional.JobType.id,
                                        p.ProfileProfessional.JobType.name,
                                        p.ProfileProfessional.JobType.isGmcRequired,
                                        p.ProfileProfessional.JobType.isHcpcRequired,
                                        p.ProfileProfessional.JobType.isNmcRequired,
                                        p.ProfileProfessional.JobType.isDeleted
                                    }
                                }
                            });

                return query.AsQueryable();
            }
            catch (Exception ex)
            {
                Log.DebugFormat($"Error retrieving Profiles. The reason is as follows: {ex.Message} {ex.StackTrace}");
            }

            return null;
        }
        /// <summary>
        ///     Searches  profiles firstName/middlename/lastName by searchTerm.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="includeDeleted">if set to <c>true</c> [include deleted].</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>Array object</returns>
        [Authorize(Roles = "PERSONNEL")]
        [HttpGet]
        [Route("searchProfiles")]
        public IHttpActionResult SearchProfiles(string searchTerm, bool includeDeleted = false, int page = 1,
            int pageSize = 10)
        {
            Log.DebugFormat("ProfileController (SearchProfiles)\n");

            try
            {
                var profs = _db.Profiles
                    .Where(p =>
                        (p.isDeleted == false || includeDeleted && p.isDeleted)
                        && (p.lastName.ToLower().Contains(searchTerm.ToLower())
                            || p.firstName.ToLower().Contains(searchTerm.ToLower())
                            || p.middleNames.ToLower().Contains(searchTerm.ToLower())
                        ))
                    .OrderBy(x => x.id)
                    .ToPagedList(page, pageSize).ToMappedPagedList<Profile, ProfileDTO>();

                return Ok(new {metaData = profs.GetMetaData(), items = profs});
            }
            catch (Exception ex)
            {
                Log.DebugFormat(
                    $"Error retrieving SearchProfiles. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving SearchProfiles. The reason is as follows: {ex.Message}");
            }
        }
        /// <summary>
        ///     Updates a Profile.
        /// </summary>
        /// <param name="incomingProfileDTO">The Profile to update.</param>
        /// <returns>HttpActionResult</returns>
        [Authorize(Roles = "PERSONNEL")]
        [HttpPut]
        [Route("")]
        public IHttpActionResult UpdateProfile(ProfileDTO incomingProfileDTO)
        {
            Log.DebugFormat("ProfileController (UpdateProfile)\n");

            if (incomingProfileDTO != null)
                try
                {
                    // TODO: Test this
                    var fetchedProfile = _db.Profiles.SingleOrDefault(x => x.id == incomingProfileDTO.id);

                    if (fetchedProfile == null)
                    {
                        return BadRequest($"Error retrieving UpdateProfile. The object to update is null");
                    }

                    var profileToUpdate= Mapper.Map(incomingProfileDTO, fetchedProfile);

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
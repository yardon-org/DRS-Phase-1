using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using drs_backend_phase1.Extensions;
using drs_backend_phase1.Models;
using log4net;

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
        public object FetchAllProfiles(bool includeDeleted = false, int page = 0, int pageSize = 10)
        {
            Log.DebugFormat("ProfileController (ReadAllProfiles)\n");

            try
            {
                IOrderedQueryable<object> query = _db.Profiles
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
                                    p.SecurityRole.RoleID,
                                    p.SecurityRole.RoleName
                                },
                                finance = new
                                {
                                    p.ProfileFinance.id,
                                    p.ProfileFinance.payrollNumber,
                                    p.ProfileFinance.isIc24Staff,
                                    p.ProfileFinance.bankId,
                                    p.ProfileFinance.bankSortCode,
                                    p.ProfileFinance.bankAccountNumber,
                                    p.ProfileFinance.buildingSocietyRollNumber
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
                                    jobRole = new
                                    {
                                        p.ProfileProfessional.JobType.id,
                                        p.ProfileProfessional.JobType.name,
                                        p.ProfileProfessional.JobType.isGmcRequired,
                                        p.ProfileProfessional.JobType.isHcpcRequired,
                                        p.ProfileProfessional.JobType.isNmcRequired
                                    }
                                }
                            })
                    .OrderBy(x => x.id);

                return query.DoPaging(page, pageSize);
            }
            catch (Exception ex)
            {
                Log.DebugFormat($"Error retrieving Profiles. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving Profiles. The reason is as follows: {ex.Message}");
            }
        }

        /// <summary>
        /// Searches  profiles firstname/middlename/lastname by keyword.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <param name="includeDeleted">if set to <c>true</c> [include deleted].</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>Array object</returns>
        [Authorize(Roles = "PERSONNEL")]
        [HttpGet]
        [Route("searchProfiles")]
        public object SearchProfiles(string searchTerm, bool includeDeleted = false, int page = 0, int pageSize = 10)
        {
            Log.DebugFormat("ProfileController (SearchProfiles)\n");

            try
            {
                IOrderedQueryable<object> query = _db.Profiles
                    .Where(p => 
                    (p.isDeleted == false || includeDeleted && p.isDeleted) 
                        && (p.lastName.ToLower().Contains(searchTerm.ToLower()) 
                        || p.firstName.ToLower().Contains(searchTerm.ToLower())
                        || p.middleNames.ToLower().Contains(searchTerm.ToLower())
                        ))
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
                                    p.SecurityRole.RoleID,
                                    p.SecurityRole.RoleName
                                },
                                finance = new
                                {
                                    p.ProfileFinance.id,
                                    p.ProfileFinance.payrollNumber,
                                    p.ProfileFinance.isIc24Staff,
                                    p.ProfileFinance.bankId,
                                    p.ProfileFinance.bankSortCode,
                                    p.ProfileFinance.bankAccountNumber,
                                    p.ProfileFinance.buildingSocietyRollNumber
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
                                    jobRole = new
                                    {
                                        p.ProfileProfessional.JobType.id,
                                        p.ProfileProfessional.JobType.name,
                                        p.ProfileProfessional.JobType.isGmcRequired,
                                        p.ProfileProfessional.JobType.isHcpcRequired,
                                        p.ProfileProfessional.JobType.isNmcRequired
                                    }
                                }
                            })
                    .OrderBy(x => x.id);

                return query.DoPaging(page, pageSize);
            }
            catch (Exception ex)
            {
                Log.DebugFormat($"Error retrieving SearchProfiles. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving SearchProfiles. The reason is as follows: {ex.Message}");
            }
        }


        ///// <summary>
        /////     Fetches the last name of the many by first or.
        ///// </summary>
        ///// <param name="searchTerm">The search term.</param>
        ///// <param name="includeDeleted">if set to <c>true</c> [include deleted].</param>
        ///// <param name="page">No.Pages</param>
        ///// <param name="pageSize">No. of Items per Page</param>
        ///// <returns></returns>
        //[Authorize(Roles = "PERSONNEL")]
        //[HttpGet]
        //[Route("search/{searchTerm}/{includeDeleted}")]
        //public object FetchManyByFirstOrLastName(string searchTerm, bool includeDeleted = false, int page = 0, int pageSize = 10)
        //{
        //    Log.DebugFormat("ProfileController (FetchManyByFirstOrLastName)\n");

        //    try
        //    {
        //        IOrderedQueryable<object> query = _db.Profiles
        //            .Where(x => (x.firstName.ToLower().Contains(searchTerm.ToLower()) ||
        //                         x.lastName.ToLower().Contains(searchTerm.ToLower())) &&
        //                        (x.isDeleted == false) || includeDeleted && x.isDeleted)
        //            .Select(x => new
        //            {
        //                //x.ProfileDocuments,
        //                //x.SpecialNotes,
        //                x.id,
        //                x.ProfileProfessional.teamId,
        //                x.ProfileProfessional.registrarLevelId,
        //                x.ProfileProfessional.agencyId,
        //                x.ProfileProfessional.registeredSurgeryId,
        //                x.ProfileProfessional.ccgId,
        //                x.ProfileProfessional.indemnityProviderId,
        //                x.ProfileFinance.bankId,
        //                jobTypeName = x.ProfileProfessional.JobType.name,
        //                subTypeName = x.ProfileProfessional.SubType.name
        //            }).OrderBy(x => x.id);


        //        return query.DoPaging(page, pageSize);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.DebugFormat(
        //            $"Error retrieving FetchManyByFirstOrLastName. The reason is as follows: {ex.Message} {ex.StackTrace}");
        //        return BadRequest(
        //            $"Error retrieving FetchManyByFirstOrLastName. The reason is as follows: {ex.Message}");
        //    }
        //}

        /// <summary>
        /// Fetches the many by team identifier.
        /// </summary>
        /// <param name="teamId">The team identifier.</param>
        /// <param name="includeDeleted">if set to <c>true</c> [include deleted].</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        [Authorize(Roles = "PERSONNEL")]
        [HttpGet]
        [Route("filter/{teamId}/{includeDeleted}")]
        public object FetchManyByTeamId(int teamId, bool includeDeleted = false, int page = 0, int pageSize = 10)
        {
            Log.DebugFormat("ProfileController (FetchManyByTeamId)\n");

            try
            {
                var query = _db.Profiles
                    .Where(x => x.ProfileProfessional.teamId == teamId &&
                                (x.isDeleted == false) || includeDeleted && x.isDeleted)
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
                                    p.SecurityRole.RoleID,
                                    p.SecurityRole.RoleName
                                },
                                finance = new
                                {
                                    p.ProfileFinance.id,
                                    p.ProfileFinance.payrollNumber,
                                    p.ProfileFinance.isIc24Staff,
                                    p.ProfileFinance.bankId,
                                    p.ProfileFinance.bankSortCode,
                                    p.ProfileFinance.bankAccountNumber,
                                    p.ProfileFinance.buildingSocietyRollNumber
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
                                    jobRole = new
                                    {
                                        p.ProfileProfessional.JobType.id,
                                        p.ProfileProfessional.JobType.name,
                                        p.ProfileProfessional.JobType.isGmcRequired,
                                        p.ProfileProfessional.JobType.isHcpcRequired,
                                        p.ProfileProfessional.JobType.isNmcRequired
                                    }
                                }
                            })
                    .OrderBy(x => x.id);

                return query.DoPaging(page, pageSize);
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
                    .Where(p => p.id == id)
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
                                    p.SecurityRole.RoleID,
                                    p.SecurityRole.RoleName
                                },
                                finance = new
                                {
                                    p.ProfileFinance.id,
                                    p.ProfileFinance.payrollNumber,
                                    p.ProfileFinance.isIc24Staff,
                                    p.ProfileFinance.bankId,
                                    p.ProfileFinance.bankSortCode,
                                    p.ProfileFinance.bankAccountNumber,
                                    p.ProfileFinance.buildingSocietyRollNumber
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
                                }
                            }
                    )
                    .SingleOrDefault();

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
        [Authorize(Roles = "PERSONNEL")]
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
using System;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.OData;
using AutoMapper;
using drs_backend_phase1.Extensions;
using drs_backend_phase1.Models;
using drs_backend_phase1.Models.DTOs;
using drs_backend_phase1.Models.DTOs.SuperSlim;
using log4net;
using RefactorThis.GraphDiff;
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
        ///     The database context
        /// </summary>
        private readonly DRSEntities _db;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProfileController" /> class.
        /// </summary>
        public ProfileController()
        {
            _db = new DRSEntities();
        }

        #region Delete_Endpoints

        /// <summary>
        ///     Deletes a Profile by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>HttpActionResult</returns>
        [Authorize(Roles = "PERSONNEL")]
        [HttpDelete]
        [Route("{id}")]
        public virtual IHttpActionResult DeleteProfileById(int id)
        {
            Log.DebugFormat("ProfileController (DeleteProfileById)\n");

            try
            {
                var profile = _db.Profiles.SingleOrDefault(x => x.id == id);

                if (profile != null)
                {
                    profile.isDeleted=true;

                    try
                    {
                        _db.SaveChanges();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        var myError = new Error
                        {
                            Code = "400",
                            Message = "The entity being updated has already been updated by another user...",
                            Data = null
                        };
                        return new ErrorResult(myError, Request);
                    }
                }

                Log.DebugFormat("Retrieval of DeleteProfileById was successful.\n");
                return Ok(true);
            }
            catch (Exception ex)
            {
                Log.DebugFormat(
                    $"Error retrieving DeleteProfileById. The reason is as follows: {ex.Message} {ex.StackTrace}");

                var myError = new Error
                {
                    Code = "400",
                    Message = "Error retrieving DeleteProfileById",
                    Data = new object[] {ex.Message, ex.StackTrace}
                };
                return new ErrorResult(myError, Request);
            }
        }

        #endregion

        #region OData_Endpoints

        /// <summary>
        ///     OData endpoit.
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
                    .Where(p => p.isDeleted == false || includeDeleted && p.isDeleted);

                return query.AsQueryable();
            }
            catch (Exception ex)
            {
                Log.DebugFormat($"Error retrieving Profiles. The reason is as follows: {ex.Message} {ex.StackTrace}");
            }

            return null;
        }

        #endregion

        #region Post_Endpoints

        /// <summary>
        /// Posts the profile.
        /// </summary>
        /// <param name="profileDTO">The profile dto.</param>
        /// <returns></returns>
        [Authorize(Roles = "PERSONNEL")]
        [HttpPost]
        [Route("")]
        public IHttpActionResult PostProfile([FromBody] ProfileDTO profileDTO)
        {
            var profileToAdd = Mapper.Map<Profile>(profileDTO);

            try
            {
                _db.Profiles.Add(profileToAdd);
                _db.SaveChanges();

                using (var responseConn = new DRSEntities())
                {
                    var refreshedEntity = responseConn.Profiles.SingleOrDefault(x => x.id == profileToAdd.id);
                    if (refreshedEntity != null)
                    {
                        Log.DebugFormat("Profile creation was successful.\n");
                        return Ok(Mapper.Map<ProfileDTO>(refreshedEntity));
                    }
                }

            }
            catch (DbEntityValidationException ee)
            {
                return BadRequest(ee.DbEntityValidationResultToString());
            }
            catch (Exception ex)
            {
                Log.DebugFormat(
                    $"Error running CheckPerformersList. The reason is as follows: {ex.Message} {ex.StackTrace}");
                var myError = new Error
                {
                    Code = "400",
                    Message = "Error running CheckPerformersList",
                    Data = new object[] { ex.Message }
                };
                return new ErrorResult(myError, Request);
            }

            Log.DebugFormat("incomingProfileDTO cannot be null");
            var myError2 = new Error
            {
                Code = "400",
                Message = "incomingProfileDTO cannot be null",
                Data = null
            };
            return new ErrorResult(myError2, Request);

        }

        #endregion

        #region GraphDiff_Configuration

        /// <summary>
        ///     Configures the graph dif.
        /// </summary>
        /// <param name="profileToUpdate">The profile to update.</param>
        private void ConfigureGraphDiff(Profile profileToUpdate)
        {
            // TODO: Add more entities to this

            _db.UpdateGraph(profileToUpdate,
                map => map.OwnedEntity(
                        p => p.ProfileProfessional,
                        with =>
                            with.AssociatedEntity(p => p.Agency)
                                .AssociatedEntity(p => p.Base)
                                .AssociatedEntity(p => p.CCG)
                                .AssociatedEntity(p => p.IndemnityProvider)
                                .AssociatedEntity(p => p.JobType)
                                .OwnedCollection(p => p.ProfilePaymentCategories, x => x.AssociatedEntity(p => p.PaymentCategory))
                                .OwnedCollection(p => p.ProfileShiftTypes, x => x.AssociatedEntity(p => p.ShiftType))
                                .AssociatedEntity(p => p.RegisteredSurgery)
                                .AssociatedEntity(p => p.RegistrarLevel)
                    )
                    .OwnedCollection(p => p.ProfileDocuments)
                    .OwnedCollection(p => p.SpecialNotes)
                    .AssociatedEntity(p => p.SecurityRole)
                    .OwnedEntity(p => p.ProfileFinance, x => x.AssociatedEntity(p => p.Bank))
            );
        }

        #endregion

        #region Disposing

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

        #endregion

        #region Put_Endpoints

        /// <summary>
        ///     Checks the performers list.
        /// </summary>
        /// <param name="incomingProfileDTO">The incoming profile dto.</param>
        /// <returns></returns>
        [Authorize(Roles = "PERSONNEL")]
        [HttpPut]
        [Route("performers")]
        public IHttpActionResult CheckPerformersList(ProfileDTO incomingProfileDTO)
        {
            Log.DebugFormat("ProfileController (CheckPerformersList)\n");

            if (incomingProfileDTO != null)
            {
                var profileToUpdate = Mapper.Map<Profile>(incomingProfileDTO);

                profileToUpdate.ProfileProfessional.performersListCheckedDate = DateTime.Now;
                profileToUpdate.ProfileProfessional.performersListCheckedBy = User.Identity.Name;


                try
                {
                    // Create a graph of the Profile entity
                    ConfigureGraphDiff(profileToUpdate);
                    _db.SaveChanges();

                    // Now fetch the updated object to send back
                    using (var responseConn = new DRSEntities())
                    {
                        var refreshedEntity = responseConn.Profiles.SingleOrDefault(x => x.id == profileToUpdate.id);
                        if (refreshedEntity != null)
                        {
                            Log.DebugFormat("Updating of CheckPerformersList was successful.\n");
                            return Ok(Mapper.Map<ProfileDTO>(refreshedEntity));
                        }
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    var myError = new Error
                    {
                        Code = "400",
                        Message = "The entity being updated has already been updated by another user...",
                        Data = null
                    };
                    return new ErrorResult(myError, Request);
                }

                catch (Exception ex)
                {
                    Log.DebugFormat(
                        $"Error running CheckPerformersList. The reason is as follows: {ex.Message} {ex.StackTrace}");
                    var myError = new Error
                    {
                        Code = "400",
                        Message = "Error running CheckPerformersList",
                        Data = new object[] {ex.Message}
                    };
                    return new ErrorResult(myError, Request);
                }

                Log.DebugFormat("Updating of CheckPerformersList was successful.\n");
                return Ok(Mapper.Map<ProfileDTO>(profileToUpdate));
            }

            Log.DebugFormat("incomingProfileDTO cannot be null");
            var myError2 = new Error
            {
                Code = "400",
                Message = "incomingProfileDTO cannot be null",
                Data = null
            };
            return new ErrorResult(myError2, Request);
        }

        /// <summary>
        ///     Updates a Profile.
        /// </summary>
        /// <param name="incomingProfileDTO">The Profile to update.</param>
        /// <returns>HttpActionResult</returns>
        [Authorize(Roles = "PERSONNEL")]
        [HttpPut]
        [Route("")]
        public IHttpActionResult UpdateProfile([FromBody] ProfileDTO incomingProfileDTO)
        {
            Log.DebugFormat("ProfileController (UpdateProfile)\n");

            if (incomingProfileDTO != null)
            {
                var profileToUpdate = Mapper.Map<Profile>(incomingProfileDTO);

                try
                {
                    ConfigureGraphDiff(profileToUpdate);
                    _db.SaveChanges();

                    // Now fetch the updated object to send back
                    using (var responseConn = new DRSEntities())
                    {
                        var refreshedEntity = responseConn.Profiles.SingleOrDefault(x => x.id == profileToUpdate.id);
                        if (refreshedEntity != null)
                        {
                            Log.DebugFormat("Updating of UpdateProfile was successful.\n");
                            return Ok(Mapper.Map<ProfileDTO>(refreshedEntity));
                        }
                    }
                }

                catch (DbUpdateConcurrencyException)
                {
                    var myError = new Error
                    {
                        Code = "400",
                        Message = "The entity being updated has already been updated by another user...",
                        Data = null
                    };
                    return new ErrorResult(myError, Request);
                }

                catch (Exception ex)
                {
                    Log.DebugFormat(
                        $"Error running UpdateProfile. The reason is as follows: {ex.Message} {ex.StackTrace}");
                    var myError = new Error
                    {
                        Code = "400",
                        Message = "Error running UpdateProfile",
                        Data = new object[] {ex.Message, ex.StackTrace}
                    };
                    return new ErrorResult(myError, Request);
                }
            }

            Log.DebugFormat("incomingProfileDTO cannot be null");
            var myError2 = new Error
            {
                Code = "400",
                Message = "incomingProfileDTO cannot be null",
                Data = null
            };
            return new ErrorResult(myError2, Request);
        }

        #endregion

        #region Get_Endpoints

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
                    .ToPagedList(page, pageSize).ToMappedPagedList<Profile, SlimProfileDTO>();

                return Ok(new {metaData = profs.GetMetaData(), items = profs});
                
            }
            catch (Exception ex)
            {
                Log.DebugFormat($"Error retrieving Profiles. The reason is as follows: {ex.Message} {ex.StackTrace}");

                var myError = new Error
                {
                    Code = "400",
                    Message = "Error retrieving Profiles",
                    Data = new object[] {ex.Message, ex.StackTrace}
                };
                return new ErrorResult(myError, Request);
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
                    .ToPagedList(page, pageSize).ToMappedPagedList<Profile, SlimProfileDTO>();

                return Ok(new {metaData = profs.GetMetaData(), items = profs});
            }
            catch (Exception ex)
            {
                Log.DebugFormat(
                    $"Error retrieving FetchManyByTeamId. The reason is as follows: {ex.Message} {ex.StackTrace}");

                var myError = new Error
                {
                    Code = "400",
                    Message = "Error retrieving FetchManyByTeamId",
                    Data = new object[] {ex.Message, ex.StackTrace}
                };
                return new ErrorResult(myError, Request);
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

                var myError = new Error
                {
                    Code = "400",
                    Message = "Error retrieving ReadAllProfileById",
                    Data = new object[] {ex.Message, ex.StackTrace}
                };
                return new ErrorResult(myError, Request);
            }
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
        public IHttpActionResult SearchProfiles(string searchTerm, bool includeDeleted = false, int page = 1, int pageSize = 10)
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
                    .ToPagedList(page, pageSize).ToMappedPagedList<Profile, SlimProfileDTO>();

                return Ok(new {metaData = profs.GetMetaData(), items = profs});
            }
            catch (Exception ex)
            {
                Log.DebugFormat(
                    $"Error retrieving SearchProfiles. The reason is as follows: {ex.Message} {ex.StackTrace}");
                var myError = new Error
                {
                    Code = "400",
                    Message = "Error retrieving SearchProfiles",
                    Data = new object[] {ex.Message, ex.StackTrace}
                };
                return new ErrorResult(myError, Request);
            }
        }

        #endregion
    }
}
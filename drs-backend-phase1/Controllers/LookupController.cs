using System;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using drs_backend_phase1.Extensions;
using drs_backend_phase1.Models;
using drs_backend_phase1.Models.DTOs;
using log4net;

namespace drs_backend_phase1.Controllers
{
    /// <summary>
    /// Lookup Controller - handles small table lookups
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [RoutePrefix("api/lookup")]
    //[HMACAuthentication]
    public class LookupController : ApiController
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
        ///     Initializes a new instance of the <see cref="LookupController" /> class.
        /// </summary>
        public LookupController()
        {
            _db = new DRSEntities();
        }


        /// <summary>
        ///     Fetches all agencies.
        /// </summary>
        /// <param name="isDeleted">if set to <c>true</c> [is deleted].</param>
        /// <param name="page">No. of Pages</param>
        /// <param name="pageSize">No. of Items per Page</param>
        /// <returns>
        ///     List of Agencies
        /// </returns>
        [Authorize(Roles = "PERSONNEL")]
        [HttpGet]
        [Route("fetchAllAgencies")]
        public IHttpActionResult FetchAllAgencies(bool isDeleted = false, int page = 1, int pageSize = 10)
        {
            Log.DebugFormat("LookupController (FetchAllAgencies)\n");

            try
            {
                var agencies = _db.Agencies
                    .Where(x => x.isDeleted == isDeleted)
                    .OrderBy(x => x.id).ToPagedList(page, pageSize).ToMappedPagedList<Agency, AgencyDTO>();

                return Ok(agencies);
            }
            catch (Exception ex)
            {
                Log.DebugFormat($"Error retrieving Agencies. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving Agencies. The reason is as follows: {ex.Message}");
            }
        }

        /// <summary>
        ///     Fetches all banks.
        /// </summary>
        /// <param name="isDeleted">if set to <c>true</c> [is deleted].</param>
        /// <param name="page">No. of Pages</param>
        /// <param name="pageSize">No. of Items per Page</param>
        /// <returns>
        ///     List of Banks
        /// </returns>
        [Authorize(Roles = "PERSONNEL")]
        [HttpGet]
        [Route("fetchAllBanks")]
        public IHttpActionResult FetchAllBanks(bool isDeleted = false, int page = 1, int pageSize = 10)
        {
            Log.DebugFormat("LookupController (FetchAllBanks)\n");

            try
            {
                var banks = _db.Banks
                    .Where(x => x.isDeleted == isDeleted)
                    .OrderBy(x => x.id).ToPagedList(page, pageSize).ToMappedPagedList<Bank, BankDTO>();

                return Ok(banks);
            }
            catch (Exception ex)
            {
                Log.DebugFormat(
                    $"Error retrieving FetchAllBanks. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving FetchAllBanks. The reason is as follows: {ex.Message}");
            }
        }

        /// <summary>
        ///     Fetches all banks.
        /// </summary>
        /// <param name="isDeleted">if set to <c>true</c> [is deleted].</param>
        /// <param name="page">No. of Pages</param>
        /// <param name="pageSize">No. of Items per Page</param>
        /// <returns>
        ///     List of Banks
        /// </returns>
        [Authorize(Roles = "PERSONNEL")]
        [HttpGet]
        [Route("fetchAllBases")]
        public IHttpActionResult FetchAllBases(bool isDeleted = false, int page = 1, int pageSize = 10)
        {
            Log.DebugFormat("LookupController (FetchAllBases)\n");

            try
            {
                var bases = _db.Bases
                    .Where(x => x.isDeleted == isDeleted)
                    .OrderBy(x => x.id).ToPagedList(page, pageSize).ToMappedPagedList<Base, BaseDTO>();

                return Ok(bases);
            }
            catch (Exception ex)
            {
                Log.DebugFormat(
                    $"Error retrieving FetchAllBases. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving FetchAllBases. The reason is as follows: {ex.Message}");
            }
        }

        /// <summary>
        ///     Fetches all cc gs.
        /// </summary>
        /// <param name="isDeleted">if set to <c>true</c> [is deleted].</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        [Authorize(Roles = "PERSONNEL")]
        [HttpGet]
        [Route("fetchAllCCGs")]
        public IHttpActionResult FetchAllCCGs(bool isDeleted = false, int page = 1, int pageSize = 10)
        {
            Log.DebugFormat("LookupController (FetchAllCCGs)\n");

            try
            {
                var ccgs = _db.CCGs
                    .Where(x => x.isDeleted == isDeleted)
                    .OrderBy(x => x.id).ToPagedList(page, pageSize).ToMappedPagedList<CCG, CCGDTO>();

                return Ok(ccgs);
            }
            catch (Exception ex)
            {
                Log.DebugFormat(
                    $"Error retrieving FetchAllCCGs. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving FetchAllCCGs. The reason is as follows: {ex.Message}");
            }
        }

        /// <summary>
        ///     Fetches all document types.
        /// </summary>
        /// <param name="isDeleted">if set to <c>true</c> [is deleted].</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        [Authorize(Roles = "PERSONNEL")]
        [HttpGet]
        [Route("fetchAllDocumentTypes")]
        public IHttpActionResult FetchAllDocumentTypes(bool isDeleted = false, int page = 1, int pageSize = 10)
        {
            Log.DebugFormat("LookupController (FetchAllDocumentTypes)\n");

            try
            {
                var docTypes = _db.DocumentTypes
                    .Where(x => x.isDeleted == isDeleted)
                    .OrderBy(x => x.id).ToPagedList(page, pageSize).ToMappedPagedList<DocumentType, DocumentTypeDTO>();

                return Ok(docTypes);
            }
            catch (Exception ex)
            {
                Log.DebugFormat(
                    $"Error retrieving FetchAllDocumentTypes. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving FetchAllDocumentTypes. The reason is as follows: {ex.Message}");
            }
        }

        /// <summary>
        /// Fetches all indemnity providers.
        /// </summary>
        /// <param name="isDeleted">if set to <c>true</c> [is deleted].</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>List of Indemnity Providers</returns>
        [Authorize(Roles = "PERSONNEL")]
        [HttpGet]
        [Route("fetchAllIndemnityProviders")]
        public IHttpActionResult FetchAllIndemnityProviders(bool isDeleted = false, int page = 1, int pageSize = 10)
        {
            Log.DebugFormat("LookupController (FetchAllIndemnityProviders)\n");

            try
            {
                var indProviders = _db.IndemnityProviders
                    .Where(x => x.isDeleted == isDeleted)
                    .OrderBy(x => x.id).ToPagedList(page, pageSize).ToMappedPagedList<IndemnityProvider, IndemnityProviderDTO>();

                return Ok(indProviders);
            }
            catch (Exception ex)
            {
                Log.DebugFormat(
                    $"Error retrieving FetchAllIndemnityProviders. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest(
                    $"Error retrieving FetchAllIndemnityProviders. The reason is as follows: {ex.Message}");
            }
        }

        /// <summary>
        ///     Fetches all payment categories.
        /// </summary>
        /// <param name="isDeleted">if set to <c>true</c> [is deleted].</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>List of PaymentCategories</returns>
        [Authorize(Roles = "PERSONNEL")]
        [HttpGet]
        [Route("fetchAllPaymentCategories")]
        public IHttpActionResult FetchAllPaymentCategories(bool isDeleted = false, int page = 1, int pageSize = 10)
        {
            Log.DebugFormat("LookupController (FetchAllPaymentCategories)\n");

            try
            {
                var payCats = _db.PaymentCategories
                    .Where(x => x.isDeleted == isDeleted)
                    .OrderBy(x => x.id).ToPagedList(page, pageSize).ToMappedPagedList<PaymentCategory, PaymentCategoryDTO>();

                return Ok(payCats);
            }
            catch (Exception ex)
            {
                Log.DebugFormat(
                    $"Error retrieving FetchAllPaymentCategories. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest(
                    $"Error retrieving FetchAllPaymentCategories. The reason is as follows: {ex.Message}");
            }
        }

        /// <summary>
        ///     Fetches all registered surgeries.
        /// </summary>
        /// <param name="isDeleted">if set to <c>true</c> [is deleted].</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        [Authorize(Roles = "PERSONNEL")]
        [HttpGet]
        [Route("fetchAllRegisteredSurgeries")]
        public IHttpActionResult FetchAllRegisteredSurgeries(bool isDeleted = false, int page = 1, int pageSize = 10)
        {
            Log.DebugFormat("LookupController (FetchAllRegisteredSurgeries)\n");

            try
            {
                var regSurgs = _db.RegisteredSurgeries
                    .Where(x => x.isDeleted == isDeleted)
                    .OrderBy(x => x.id).ToPagedList(page, pageSize).ToMappedPagedList<RegisteredSurgery, RegisteredSurgeryDTO>();

                return Ok(regSurgs);
            }
            catch (Exception ex)
            {
                Log.DebugFormat(
                    $"Error retrieving FetchAllRegisteredSurgeries. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest(
                    $"Error retrieving FetchAllRegisteredSurgeries. The reason is as follows: {ex.Message}");
            }
        }

        /// <summary>
        ///     Fetches all registrar levels.
        /// </summary>
        /// <param name="isDeleted">if set to <c>true</c> [is deleted].</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        [Authorize(Roles = "PERSONNEL")]
        [HttpGet]
        [Route("fetchAllRegistrarLevels")]
        public IHttpActionResult FetchAllRegistrarLevels(bool isDeleted = false, int page = 1, int pageSize = 10)
        {
            Log.DebugFormat("LookupController (FetchAllRegistrarLevels)\n");

            try
            {
                var regLevs = _db.RegistrarLevels
                    .Where(x => x.isDeleted == isDeleted)
                    .OrderBy(x => x.id).ToPagedList(page, pageSize).ToMappedPagedList<RegistrarLevel, RegistrarLevelDTO>();

                return Ok(regLevs);
            }
            catch (Exception ex)
            {
                Log.DebugFormat(
                    $"Error retrieving FetchAllRegistrarLevels. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving FetchAllRegistrarLevels. The reason is as follows: {ex.Message}");
            }
        }

        /// <summary>
        ///     Fetches all shift types.
        /// </summary>
        /// <param name="isDeleted">if set to <c>true</c> [is deleted].</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        [Authorize(Roles = "PERSONNEL")]
        [HttpGet]
        [Route("fetchAllShiftTypes")]
        public IHttpActionResult FetchAllShiftTypes(bool isDeleted = false, int page = 1, int pageSize = 10)
        {
            Log.DebugFormat("LookupController (FetchAllShiftTypes)\n");

            try
            {
                var shiftTypes = _db.ShiftTypes
                    .Where(x => x.isDeleted == isDeleted)
                    .OrderBy(x => x.id).ToPagedList(page, pageSize).ToMappedPagedList<ShiftType, ShiftTypeDTO>();

                return Ok(shiftTypes);
            }
            catch (Exception ex)
            {
                Log.DebugFormat(
                    $"Error retrieving FetchAllShiftTypes. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving FetchAllShiftTypes. The reason is as follows: {ex.Message}");
            }
        }

        /// <summary>
        ///     Fetches all teams.
        /// </summary>
        /// <param name="isDeleted">if set to <c>true</c> [is deleted].</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        [Authorize(Roles = "PERSONNEL")]
        [HttpGet]
        [Route("fetchAllTeams")]
        public IHttpActionResult FetchAllTeams(bool isDeleted = false, int page = 1, int pageSize = 10)
        {
            Log.DebugFormat("LookupController (FetchAllTeams)\n");

            try
            {
                var teams = _db.Teams
                    .Where(x => x.isDeleted == isDeleted)
                    .OrderBy(x => x.id).ToPagedList(page, pageSize).ToMappedPagedList<Team, TeamDTO>();

                return Ok(teams);
            }
            catch (Exception ex)
            {
                Log.DebugFormat(
                    $"Error retrieving FetchAllTeams. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving FetchAllTeams. The reason is as follows: {ex.Message}");
            }
        }

        /// <summary>
        /// Searches for agencies by searchTerm.
        /// </summary>
        /// <param name="searchTerm">The searchTerm.</param>
        /// <param name="isDeleted">if set to <c>true</c> [is deleted].</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>A list of Agencies</returns>
        [Authorize(Roles = "PERSONNEL")]
        [HttpGet]
        [Route("searchAgencies")]
        public IHttpActionResult SearchAgencies(string searchTerm, bool isDeleted = false, int page = 1, int pageSize = 10)
        {
            Log.DebugFormat("LookupController (SearchAgencies)\n");

            try
            {
                var agenciesuery = _db.Agencies
                    .Where(x => x.isDeleted == isDeleted
                                && x.name.ToLower().Contains(searchTerm.ToLower()))
                    .OrderBy(x => x.id).ToPagedList(page, pageSize).ToMappedPagedList<Agency, AgencyDTO>();

                return Ok(agenciesuery);
            }
            catch (Exception ex)
            {
                Log.DebugFormat($"Error searching for Agencies. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error searching for Agencies. The reason is as follows: {ex.Message}");
            }
        }
        /// <summary>
        ///     Searches for registered surgeries by name.
        /// </summary>
        /// <param name="searchTerm">The searchTerm.</param>
        /// <param name="isDeleted">if set to <c>true</c> [is deleted].</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns>Array of Surgeries</returns>
        [Authorize(Roles = "PERSONNEL")]
        [HttpGet]
        [Route("searchRegisteredSurgeries")]
        public IHttpActionResult SearchRegisteredSurgeries(string searchTerm, bool isDeleted = false, int page = 1, int pageSize = 10)
        {
            Log.DebugFormat("LookupController (SearchRegisteredSurgeries)\n");

            try
            {
                var regSurgs = _db.RegisteredSurgeries
                    .Where(x => x.isDeleted == isDeleted
                                &&
                                x.name.ToLower().Contains(searchTerm.ToLower())
                    )
                    .OrderBy(x => x.id).ToPagedList(page, pageSize).ToMappedPagedList<RegisteredSurgery, RegisteredSurgeryDTO>();

                return Ok(regSurgs);
            }
            catch (Exception ex)
            {
                Log.DebugFormat(
                    $"Error retrieving SearchRegisteredSurgeries. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest(
                    $"Error retrieving SearchRegisteredSurgeries. The reason is as follows: {ex.Message}");
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
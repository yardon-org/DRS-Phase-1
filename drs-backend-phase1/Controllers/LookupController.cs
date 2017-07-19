using System;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using drs_backend_phase1.Extensions;
using drs_backend_phase1.Models;
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
        public object FetchAllAgencies(bool isDeleted = false, int page = 0, int pageSize = 10)
        {
            Log.DebugFormat("LookupController (FetchAllAgencies)\n");

            try
            {
                IOrderedQueryable<object> query = _db.Agencies
                    .Where(x => x.isDeleted == isDeleted)
                    .Select(
                        p =>
                            new
                            {
                                p.Id,
                                p.Name
                            })
                    .OrderBy(x => x.Id);

                return query.DoPaging(page, pageSize);
            }
            catch (Exception ex)
            {
                Log.DebugFormat($"Error retrieving Agencies. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving Agencies. The reason is as follows: {ex.Message}");
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
        public object SearchAgencies(string searchTerm, bool isDeleted = false, int page = 0, int pageSize = 10)
        {
            Log.DebugFormat("LookupController (SearchAgencies)\n");

            try
            {
                IOrderedQueryable<object> query = _db.Agencies
                    .Where(x => x.isDeleted == isDeleted
                                && x.Name.ToLower().Contains(searchTerm.ToLower()))
                    .Select(
                        p =>
                            new
                            {
                                p.Id,
                                p.Name
                            })
                    .OrderBy(x => x.Id);

                return query.DoPaging(page, pageSize);
            }
            catch (Exception ex)
            {
                Log.DebugFormat($"Error searching for Agencies. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error searching for Agencies. The reason is as follows: {ex.Message}");
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
        public object FetchAllBanks(bool isDeleted = false, int page = 0, int pageSize = 10)
        {
            Log.DebugFormat("LookupController (FetchAllBanks)\n");

            try
            {
                IOrderedQueryable<object> query = _db.Banks
                    .Where(x => x.isDeleted == isDeleted)
                    .Select(
                        p =>
                            new
                            {
                                p.Id,
                                p.Name
                            })
                    .OrderBy(x => x.Id);

                return query.DoPaging(page, pageSize);
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
        public object FetchAllBases(bool isDeleted = false, int page = 0, int pageSize = 10)
        {
            Log.DebugFormat("LookupController (FetchAllBases)\n");

            try
            {
                IOrderedQueryable<object> query = _db.Bases
                    .Where(x => x.isDeleted == isDeleted)
                    .Select(
                        p =>
                            new
                            {
                                p.Id,
                                p.Name
                            })
                    .OrderBy(x => x.Id);

                return query.DoPaging(page, pageSize);
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
        public object FetchAllCCGs(bool isDeleted = false, int page = 0, int pageSize = 10)
        {
            Log.DebugFormat("LookupController (FetchAllCCGs)\n");

            try
            {
                IOrderedQueryable<object> query = _db.CCGs
                    .Where(x => x.isDeleted == isDeleted)
                    .Select(
                        p =>
                            new
                            {
                                p.Id,
                                p.Name
                            })
                    .OrderBy(x => x.Id);

                return query.DoPaging(page, pageSize);
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
        public object FetchAllDocumentTypes(bool isDeleted = false, int page = 0, int pageSize = 10)
        {
            Log.DebugFormat("LookupController (FetchAllDocumentTypes)\n");

            try
            {
                IOrderedQueryable<object> query = _db.DocumentTypes
                    .Where(x => x.isDeleted == isDeleted)
                    .Select(
                        p =>
                            new
                            {
                                p.Id,
                                p.Name
                            })
                    .OrderBy(x => x.Id);

                return query.DoPaging(page, pageSize);
            }
            catch (Exception ex)
            {
                Log.DebugFormat(
                    $"Error retrieving FetchAllDocumentTypes. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving FetchAllDocumentTypes. The reason is as follows: {ex.Message}");
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
        public object FetchAllPaymentCategories(bool isDeleted = false, int page = 0, int pageSize = 10)
        {
            Log.DebugFormat("LookupController (FetchAllPaymentCategories)\n");

            try
            {
                IOrderedQueryable<object> query = _db.PaymentCategories
                    .Where(x => x.isDeleted == isDeleted)
                    .Select(
                        p =>
                            new
                            {
                                p.Id,
                                p.Name
                            })
                    .OrderBy(x => x.Id);

                return query.DoPaging(page, pageSize);
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
        public object FetchAllRegisteredSurgeries(bool isDeleted = false, int page = 0, int pageSize = 10)
        {
            Log.DebugFormat("LookupController (FetchAllRegisteredSurgeries)\n");

            try
            {
                IOrderedQueryable<object> query = _db.RegisteredSurgeries
                    .Where(x => x.isDeleted == isDeleted)
                    .Select(
                        p =>
                            new
                            {
                                p.Id,
                                p.Name
                            })
                    .OrderBy(x => x.Id);

                return query.DoPaging(page, pageSize);
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
        public object SearchRegisteredSurgeries(string searchTerm, bool isDeleted = false, int page = 0, int pageSize = 10)
        {
            Log.DebugFormat("LookupController (SearchRegisteredSurgeries)\n");

            try
            {
                IOrderedQueryable<object> query = _db.RegisteredSurgeries
                    .Where(x => x.isDeleted == isDeleted
                                &&
                                x.Name.ToLower().Contains(searchTerm.ToLower())
                    )
                    .Select(
                        p =>
                            new
                            {
                                p.Id,
                                p.Name
                            })
                    .OrderBy(x => x.Id);

                return query.DoPaging(page, pageSize);
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
        ///     Fetches all registrar levels.
        /// </summary>
        /// <param name="isDeleted">if set to <c>true</c> [is deleted].</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        [Authorize(Roles = "PERSONNEL")]
        [HttpGet]
        [Route("fetchAllRegistrarLevels")]
        public object FetchAllRegistrarLevels(bool isDeleted = false, int page = 0, int pageSize = 10)
        {
            Log.DebugFormat("LookupController (FetchAllRegistrarLevels)\n");

            try
            {
                IOrderedQueryable<object> query = _db.RegistrarLevels
                    .Where(x => x.isDeleted == isDeleted)
                    .Select(
                        p =>
                            new
                            {
                                p.Id,
                                p.Name
                            })
                    .OrderBy(x => x.Id);

                return query.DoPaging(page, pageSize);
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
        public object FetchAllShiftTypes(bool isDeleted = false, int page = 0, int pageSize = 10)
        {
            Log.DebugFormat("LookupController (FetchAllShiftTypes)\n");

            try
            {
                IOrderedQueryable<object> query = _db.ShiftTypes
                    .Where(x => x.isDeleted == isDeleted)
                    .Select(
                        p =>
                            new
                            {
                                p.Id,
                                p.Name
                            })
                    .OrderBy(x => x.Id);

                return query.DoPaging(page, pageSize);
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
        public object FetchAllTeams(bool isDeleted = false, int page = 0, int pageSize = 10)
        {
            Log.DebugFormat("LookupController (FetchAllTeams)\n");

            try
            {
                IOrderedQueryable<object> query = _db.Teams
                    .Where(x => x.isDeleted == isDeleted)
                    .Select(
                        p =>
                            new
                            {
                                p.Id,
                                p.Name
                            })
                    .OrderBy(x => x.Id);

                return query.DoPaging(page, pageSize);
            }
            catch (Exception ex)
            {
                Log.DebugFormat(
                    $"Error retrieving FetchAllTeams. The reason is as follows: {ex.Message} {ex.StackTrace}");
                return BadRequest($"Error retrieving FetchAllTeams. The reason is as follows: {ex.Message}");
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
using System;
using System.Linq;
using System.Reflection;
using drs_backend_phase1.Entities;
using drs_backend_phase1.Models;
using drs_backend_phase1.Services.Interfaces;
using log4net;
using Microsoft.VisualBasic.Logging;

namespace drs_backend_phase1.Services
{
    /// <summary>
    /// Security Service
    /// </summary>
    public class SecurityService : ISecurityService, IDisposable
    {
        private readonly DRSEntities _db = new DRSEntities();
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Finds the name of the security role by role.
        /// </summary>
        /// <param name="applicationUser">The application user.</param>
        /// <returns></returns>
        public SecurityRole FindSecurityRoleByActiveDirectoryEmailAddress(ApplicationUser applicationUser)
        {
            Profile profile = null;
            try
            {
                profile = _db.Profiles.SingleOrDefault(x => x.adEmailAddress == applicationUser.ActiveDirectoryEmailAddress);
            }
            catch (Exception ex)
            {
                Log.DebugFormat("An error has occurred: " + ex.Message, ex.InnerException, ex + "\n");
            }


            if (profile != null && profile.roleID!=0)
            {
                return _db.SecurityRoles.SingleOrDefault(x => x.RoleID == profile.roleID);
            }

            return null;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _db?.Dispose();
        }
    }
}
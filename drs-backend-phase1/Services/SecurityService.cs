using System;
using System.Linq;
using drs_backend_phase1.Entities;
using drs_backend_phase1.Models;
using drs_backend_phase1.Services.Interfaces;

namespace drs_backend_phase1.Services
{
    /// <summary>
    /// Security Service
    /// </summary>
    public class SecurityService : ISecurityService, IDisposable
    {
        private readonly DRSEntities _db = new DRSEntities();

        /// <summary>
        /// Finds the name of the security role by role.
        /// </summary>
        /// <param name="applicationUser">The application user.</param>
        /// <returns></returns>
        public SecurityRole FindSecurityRoleByActiveDirectoryEmailAddress(ApplicationUser applicationUser)
        {

            var profile =
                _db.Profiles.SingleOrDefault(x => x.adEmailAddress == applicationUser.ActiveDirectoryEmailAddress);


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
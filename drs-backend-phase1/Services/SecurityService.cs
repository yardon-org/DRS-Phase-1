using System.Linq;
using drs_backend_phase1.Models;
using drs_backend_phase1.Services.Interfaces;

namespace drs_backend_phase1.Services
{
    /// <summary>
    /// Security Service
    /// </summary>
    public class SecurityService : ISecurityService
    {
        private readonly DRSEntities _db = new DRSEntities();


        /// <summary>
        ///     Finds the name of the security role by role.
        /// </summary>
        /// <param name="rolename">The rolename.</param>
        /// <returns></returns>
        public SecurityRole FindSecurityRoleByRoleName(string rolename)
        {
            var role = _db.SecurityRoles.SingleOrDefault(x => x.RoleName == rolename.ToUpper());

            return role;
        }
    }
}
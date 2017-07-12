using drs_backend_phase1.Entities;
using drs_backend_phase1.Models;

namespace drs_backend_phase1.Services.Interfaces
{
    /// <summary>
    /// </summary>
    public interface ISecurityService
    {
        /// <summary>
        /// Finds the name of the security role by role.
        /// </summary>
        /// <param name="applicationUser">The application user.</param>
        /// <returns></returns>
        SecurityRole FindSecurityRoleByActiveDirectoryEmailAddress(ApplicationUser applicationUser);
    }
}
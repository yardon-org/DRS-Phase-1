using drs_backend_phase1.Models;

namespace drs_backend_phase1.Services.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISecurityService
    {
        /// <summary>
        /// Finds the name of the security role by role.
        /// </summary>
        /// <param name="rolename">The rolename.</param>
        /// <returns></returns>
        SecurityRole FindSecurityRoleByRoleName(string rolename);
    }
}
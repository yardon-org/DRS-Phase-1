using System;
using System.DirectoryServices.AccountManagement;
using System.ServiceModel.Security;
using System.Threading.Tasks;
using drs_backend_phase1.Entities;

namespace drs_backend_phase1.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthRepository
    {
        /// <summary>
        /// Finds the user asynchronous.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        /// <exception cref="SecurityAccessDeniedException">
        /// There were no valid active directory accounts with the username of: " + username
        /// or
        /// Incorrect username or password
        /// </exception>
        /// <exception cref="Exception"></exception>
        public async Task<ApplicationUser> FindUserAsync(string username, string password)
        {
            // Find the user within the active directory

            using (var context = new PrincipalContext(
                ContextType.Domain,
                @"ash-dc02.sehnp.nhs.uk:3268",
                @"DC=sehnp,DC=nhs,DC=uk",
                @"SEHNP\RemoteDeployService",
                @"RDS31@Essex#"))
            {
                UserPrincipal personPrincipal = null;
                try
                {
                    // Find the user in the active directory by using their username
                    personPrincipal =
                        await
                            Task.Run(() => UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, username));

                    // Throw an exception if an account couldn't be found
                    if (personPrincipal == null)
                    {
                        throw new SecurityAccessDeniedException(
                            "There were no valid active directory accounts with the username of: " + username);
                    }

                    // Validate the password using the UPN as the username because the UPN must be unique across the forest
                    // CF-138: NS: 16-06-2016
                    // Changing this to use sAMAccountName as the UserPrincipalName is not mandatory and can be NULL
                    // see: https://social.technet.microsoft.com/Forums/windowsserver/en-US/0479b81d-07ba-4167-b770-e9db87b2a32b/sam-account-name-vs-upn?forum=winserverDS
                    if (!context.ValidateCredentials(personPrincipal.SamAccountName, password))
                    {
                        throw new SecurityAccessDeniedException("Incorrect username or password");
                    }

                    // Return active directory details
                    return new ApplicationUser
                    {
                        ActiveDirectoryGuid = personPrincipal.Guid.ToString(),
                        ActiveDirectorySid = personPrincipal.Sid.ToString()
                    };
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                finally
                {
                    personPrincipal?.Dispose();
                }
            }
        }
    }
}

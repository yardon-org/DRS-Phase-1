using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.ServiceModel.Security;
using System.Threading.Tasks;
using System.Web;
using OAuthHMAC.Entity;

namespace OAuthHMAC.Web.Repository
{
    public class AuthRepository
    {
        public async Task<ApplicationUser> FindUserAsync(string username, string password)
        {
            // Find user within the active directory

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
                    // find the person in active directory using their username
                    personPrincipal = await Task.Run((() => UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, username)));
                    // Throw exception if an account couldn't be found
                    if (personPrincipal == null)
                    {
                        throw new SecurityAccessDeniedException("The linked active directory account is invalid.");
                    }

                    // Validate the password using the UPN as the username because the UPN must be unique across the forest
                    // CF-138: NS: 16-06-2016
                    // Changing this to use sAMAccountName as the UserPrincipalName is not mandatory and can be NULL
                    // see: https://social.technet.microsoft.com/Forums/windowsserver/en-US/0479b81d-07ba-4167-b770-e9db87b2a32b/sam-account-name-vs-upn?forum=winserverDS
                    if (!context.ValidateCredentials(personPrincipal.SamAccountName, password))
                    {
                        throw new SecurityAccessDeniedException("Incorrect username or password.");
                    }
                    // return model instead of bool
                    return new ApplicationUser
                    {
                        ActiveDirectoryGuid = UserPrincipal.Current.Guid.ToString(),
                        ActiveDirectorySid = UserPrincipal.Current.Sid.ToString()
                    };
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    if (personPrincipal != null)
                    {
                        personPrincipal.Dispose();
                    }
                }
            }
        }
    }
}
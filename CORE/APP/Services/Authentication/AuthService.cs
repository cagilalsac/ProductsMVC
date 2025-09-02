using System.Security.Claims;

namespace CORE.APP.Services.Authentication
{
    /// <summary>
    /// Base abstract class that can be used in all authentication sub (child) concrete classes 
    /// which defines authentication-related operation for generating user claims.
    /// Inherits from <see cref="ServiceBase"/> to utilize culture-specific settings with basic success and error command response operations.
    /// </summary>
    public abstract class AuthService : ServiceBase
    {
        /// <summary>
        /// Generates a list of claims for a user based on their ID, username, and assigned role names.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="userName">The username of the user.</param>
        /// <param name="userRoleNames">An array of role names assigned to the user.</param>
        /// <returns>A list of <see cref="Claim"/> objects representing the user's identity and roles.</returns>
        protected List<Claim> GetClaims(int userId, string userName, string[] userRoleNames)
        {
            // Create claims for user ID and username, then add claims for each user role.
            var claims = new List<Claim>()
            {
                new Claim("Id", userId.ToString()), // custom claim with key Id and value user ID
                new Claim(ClaimTypes.Name, userName)
            };
            foreach (var userRoleName in userRoleNames)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRoleName));
            }
            return claims;
        }
    }
}

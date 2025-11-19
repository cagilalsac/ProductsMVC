using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CORE.APP.Services.Authentication.MVC
{
    /// <summary>
    /// Provides cookie-based authentication services for signing in and signing out users.
    /// </summary>
    public class CookieAuthService : ICookieAuthService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="CookieAuthService"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">Provides access to the current HTTP context.</param>
        public CookieAuthService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Signs in a user by creating an authentication cookie with the specified claims and properties.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="userName">The username of the user.</param>
        /// <param name="userRoleNames">An array of role names assigned to the user.</param>
        /// <param name="expiration">Optional expiration date and time for the authentication cookie. If not specified, a default value (null) is used.</param>
        /// <param name="isPersistent">Indicates whether the authentication cookie should persist across browser sessions.</param>
        /// <returns>A task representing the asynchronous sign-in operation.</returns>
        public async Task SignIn(int userId, string userName, string[] userRoleNames, DateTime? expiration = default, bool isPersistent = true)
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

            // Create a ClaimsIdentity with the generated claims and specify the authentication scheme.
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Create a ClaimsPrincipal from the identity.
            var principal = new ClaimsPrincipal(identity);

            // Set authentication properties, including persistence and expiration.
            var authenticationProperties = new AuthenticationProperties
            {
                IsPersistent = isPersistent,
                ExpiresUtc = expiration.HasValue ? DateTime.SpecifyKind(expiration.Value, DateTimeKind.Utc) : null
            };

            // Sign in the user by issuing the authentication cookie.
            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authenticationProperties);
        }

        /// <summary>
        /// Signs out the current user by removing the authentication cookie.
        /// </summary>
        /// <returns>A task representing the asynchronous sign-out operation.</returns>
        public async Task SignOut()
        {
            // Sign out the user by removing the authentication cookie from the current HTTP context.
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}

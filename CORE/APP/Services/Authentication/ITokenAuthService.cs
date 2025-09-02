using CORE.APP.Models.Authentication;
using System.Security.Claims;

namespace CORE.APP.Services.Authentication
{
    /// <summary>
    /// Provides token-based authentication operations, including access token (JWT) and refresh token generation and claim extraction.
    /// </summary>
    public interface ITokenAuthService
    {
        /// <summary>
        /// Returns a token response including JWT (access token) and refresh token.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="userName">The username of the user.</param>
        /// <param name="userRoleNames">An array of role names assigned to the user.</param>
        /// <param name="expiration">The expiration date and time for the JWT.</param>
        /// <param name="securityKey">The security key used to sign the JWT.</param>
        /// <param name="issuer">The issuer of the JWT, generally the server API application's domain.</param>
        /// <param name="audience">The intended audience for the JWT, generally the client application's domain.</param>
        /// <param name="refreshToken">Refresh token to be included in the returned <see cref="TokenResponse"/> object.</param>
        /// <returns>A <see cref="TokenResponse"/> object containing the created JWT and provided refresh token.</returns>
        public TokenResponse GetTokenResponse(int userId, string userName, string[] userRoleNames, DateTime expiration, 
            string securityKey, string issuer, string audience, string refreshToken); // public may not be written

        /// <summary>
        /// Returns a new generated refresh token, which is a secure, random string used to obtain new JWT without re-authenticating the user.
        /// </summary>
        /// <returns>
        /// A string representing the newly generated refresh token.
        /// </returns>
        public string GetRefreshToken();

        /// <summary>
        /// Extracts and returns a collection of claims from the specified access token (JWT) using the provided security key.
        /// </summary>
        /// <param name="token">The JWT containing encoded claims.</param>
        /// <param name="securityKey">The security key used to validate and decode the JWT.</param>
        /// <returns>
        /// An <see cref="IEnumerable{Claim}"/> containing the claims extracted from the JWT.
        /// </returns>
        public IEnumerable<Claim> GetClaims(string token, string securityKey); 
    }
}

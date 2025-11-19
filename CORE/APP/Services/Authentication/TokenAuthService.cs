using CORE.APP.Models.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CORE.APP.Services.Authentication
{
    /// <summary>
    /// Provides concrete implementations for token-based authentication operations, including
    /// generating token response with JWT (access token) and refresh token, generating refresh token, and extracting claims from access token (JWT).
    /// This service is responsible for securely creating and validating JWT used in authentication flows.
    /// </summary>
    public class TokenAuthService : ITokenAuthService
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
            string securityKey, string issuer, string audience, string refreshToken)
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

            // Create signing credentials using the provided security key.
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            // Build the JWT with claims, issuer, audience, and expiration.
            var jwtSecurityToken = new JwtSecurityToken(issuer, audience, claims, DateTime.Now, expiration, signingCredentials);
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            // Serialize the JWT to a string.
            var jwt = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);

            // Return the token response with the serialized JWT value and the refresh token parameter value.
            return new TokenResponse()
            {
                Token = $"{JwtBearerDefaults.AuthenticationScheme} {jwt}", // JwtBearerDefaults.AuthenticationScheme: "Bearer"
                RefreshToken = refreshToken
            };
        }

        /// <summary>
        /// Generates a new refresh token, which is a secure, random string used to obtain new JWT.
        /// </summary>
        /// <returns>A string representing the newly generated refresh token.</returns>
        public string GetRefreshToken()
        {
            // Generate a cryptographically secure random 32-byte refresh token.
            var bytes = new byte[32];
            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                randomNumberGenerator.GetBytes(bytes);
            }
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Extracts and returns a collection of claims from the specified access token (JWT) using the provided security key.
        /// </summary>
        /// <param name="token">The JWT containing encoded claims.</param>
        /// <param name="securityKey">The security key used to validate and decode the JWT.</param>
        /// <returns>An <see cref="IEnumerable{Claim}"/> containing the claims extracted from the JWT.</returns>
        public IEnumerable<Claim> GetClaims(string token, string securityKey)
        {
            // IEnumerable is an interface that the List class implements. LINQ methods can also be used with IEnumerable.
            // An IEnumerable collection can be converted to a List collection by invoking ToList method when needed.

            // Remove the "Bearer" prefix if exists in the JWT.
            token = token.StartsWith(JwtBearerDefaults.AuthenticationScheme) ? 
                token.Remove(0, JwtBearerDefaults.AuthenticationScheme.Length + 1) : token;

            // Prepare the signing key and validation parameters.
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey
            };

            // Validate the JWT and extract claims.
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            return securityToken is null ? null : principal.Claims;
        }
    }
}

namespace CORE.APP.Models.Authentication
{
    /// <summary>
    /// Represents the response to a <see cref="TokenRequestBase"/> or <see cref="RefreshTokenRequestBase"/>, 
    /// including the JWT (access token) and refresh token.
    /// </summary>
    public class TokenResponse
    {
        /// <summary>
        /// The generated JWT.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the refresh token assigned to the user.
        /// This token is used to request a new JWT without requiring re-authentication, typically after the original JWT has expired.
        /// </summary>
        public string RefreshToken { get; set; }
    }
}

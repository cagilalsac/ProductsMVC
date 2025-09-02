using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CORE.APP.Models.Authentication
{
    /// <summary>
    /// Represents a request to generate a token response including JWT (access token) containing user credentials and refresh token.
    /// </summary>
    public class TokenRequestBase
    {
        /// <summary>
        /// The username of the user requesting the token response.
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// The password of the user requesting the token response.
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the security key used for signing or validating JWT. This property is ignored during JSON serialization and deserialization.
        /// The [JsonIgnore] attribute ensures that the security key is not exposed in JSON payloads sent to or received from clients.
        /// Typically, this value is assigned from application's configuration through IConfiguration instance injection 
        /// and used for cryptographic operations in JWT handling.
        /// </summary>
        [JsonIgnore]
        public string SecurityKey { get; set; }

        /// <summary>
        /// Gets or sets the issuer (iss) claim for the JWT. This value identifies the principal (generally API server application) that issued the JWT.
        /// The [JsonIgnore] attribute ensures that the issuer is not exposed in JSON payloads sent to or received from clients.
        /// Typically, this value is assigned from application's configuration through IConfiguration instance injection 
        /// and used during JWT generation and validation.
        /// </summary>
        [JsonIgnore]
        public string Issuer { get; set; }

        /// <summary>
        /// Gets or sets the audience (aud) claim for the JWT. This value identifies the recipients (generally client applications) that the JWT is intended for.
        /// The [JsonIgnore] attribute ensures that the audience is not exposed in JSON payloads sent to or received from clients.
        /// Typically, this value is assigned from application's configuration through IConfiguration instance injection 
        /// and used during JWT generation and validation.
        /// </summary>
        [JsonIgnore]
        public string Audience { get; set; }
    }
}

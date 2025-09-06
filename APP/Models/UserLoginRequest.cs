using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APP.Models
{
    /// <summary>
    /// Represents the data required for a user login request.
    /// Used for binding and validating login form input in Razor Views.
    /// </summary>
    public class UserLoginRequest
    {
        /// <summary>
        /// Gets or sets the user name for login.
        /// Must be between 3 and 30 characters.
        /// </summary>
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "{0} must be minimum {2} maximum {1} characters!")]
        [DisplayName("User Name")]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password for login.
        /// Must be between 4 and 15 characters.
        /// </summary>
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(15, MinimumLength = 4, ErrorMessage = "{0} must be minimum {2} maximum {1} characters!")]
        public string Password { get; set; }
    }
}

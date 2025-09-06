using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APP.Models
{
    /// <summary>
    /// Represents the data required for a user register request.
    /// Used for binding and validating register form input in Razor Views.
    /// </summary>
    public class UserRegisterRequest
    {
        /// <summary>
        /// Gets or sets the user name for register.
        /// Must be between 3 and 30 characters.
        /// </summary>
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "{0} must be minimum {2} maximum {1} characters!")]
        [DisplayName("User Name")]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password for register.
        /// Must be between 4 and 15 characters.
        /// </summary>
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(15, MinimumLength = 4, ErrorMessage = "{0} must be minimum {2} maximum {1} characters!")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the confirm password for register.
        /// Must be between 4 and 15 characters.
        /// Must have the equal value with Password.
        /// </summary>
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(15, MinimumLength = 4, ErrorMessage = "{0} must be minimum {2} maximum {1} characters!")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password must be the same!")]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}

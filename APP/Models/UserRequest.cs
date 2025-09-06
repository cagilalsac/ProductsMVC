using APP.Domain;
using CORE.APP.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APP.Models
{
    public class UserRequest : Request
    {
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "{0} must be minimum {2} maximum {1} characters!")]
        [DisplayName("User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(15, MinimumLength = 4, ErrorMessage = "{0} must be minimum {2} maximum {1} characters!")]
        public string Password { get; set; }

        [StringLength(50, ErrorMessage = "{0} must be maximum {1} characters!")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [StringLength(50, ErrorMessage = "{0} must be maximum {1} characters!")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        public Genders Gender { get; set; }

        [DisplayName("Birth Date")]
        public DateTime? BirthDate { get; set; }

        // We don't need to get the RegistrationDate from the client since it will be assigned automatically in the service.

        [Range(0, 5, ErrorMessage = "{0} must be between {1} and {2}!")] // minimum value can be 0, maximum value can be 5
        [Required(ErrorMessage = "{0} is required!")]
        public decimal? Score { get; set; } // changed from decimal to decimal? and added [Required] to show validation error message

        [DisplayName("Active")]
        public bool IsActive { get; set; }

        public string Address { get; set; }

        //[Required(ErrorMessage = "{0} is required!")] // can be defined if each user must belong to a group
        [DisplayName("Group")]
        public int? GroupId { get; set; }

        //[Required(ErrorMessage = "{0} is required!")] // can be defined if each user must have at least one role
        [DisplayName("Roles")]
        public List<int> RoleIds { get; set; } = new List<int>();

        //[Required(ErrorMessage = "{0} is required!")] // can be defined if each user must belong to a country
        [DisplayName("Country")]
        public int? CountryId { get; set; }

        //[Required(ErrorMessage = "{0} is required!")] // can be defined if each user must belong to a city
        [DisplayName("City")]
        public int? CityId { get; set; }
    }
}

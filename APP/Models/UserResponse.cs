using APP.Domain;
using CORE.APP.Models;
using System.ComponentModel;

namespace APP.Models
{
    public class UserResponse : Response
    {
        [DisplayName("User Name")]
        public string UserName { get; set; }

        public string Password { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        public Genders Gender { get; set; }

        public DateTime? BirthDate { get; set; }

        public DateTime RegistrationDate { get; set; }

        public decimal Score { get; set; }

        public bool IsActive { get; set; }

        public string Address { get; set; }

        public int? GroupId { get; set; }

        public List<int> RoleIds { get; set; }

        public int? CountryId { get; set; }

        public int? CityId { get; set; }



        [DisplayName("Full Name")]
        public string FullName { get; set; }

        [DisplayName("Gender")]
        public string GenderF { get; set; }

        [DisplayName("Birth Date")]
        public string BirthDateF { get; set; }

        [DisplayName("Registration Date")]
        public string RegistrationDateF { get; set; }

        [DisplayName("Score")]
        public string ScoreF { get; set; }

        [DisplayName("Status")]
        public string IsActiveF { get; set; }
        
        public string Group { get; set; }

        public List<string> Roles { get; set; }

        public string Country { get; set; }

        public string City { get; set; }
    }
}

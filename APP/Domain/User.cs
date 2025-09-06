using CORE.APP.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APP.Domain
{
    public class User : Entity
    {
        [Required, StringLength(30)]
        public string UserName { get; set; }

        [Required, StringLength(15)]
        public string Password { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        public Genders Gender { get; set; }

        public DateTime? BirthDate { get; set; }

        public DateTime RegistrationDate { get; set; }

        public decimal Score { get; set; }

        public bool IsActive { get; set; }

        public string Address { get; set; }

        public int? CountryId { get; set; }

        public Country Country { get; set; }

        public int? CityId { get; set; }

        public City City { get; set; }

        public int? GroupId { get; set; } 

        public Group Group { get; set; } 

        public List<UserRole> UserRoles { get; set; } = new List<UserRole>(); 

        [NotMapped]
        public List<int> RoleIds
        {
            get => UserRoles.Select(userRoleEntity => userRoleEntity.RoleId).ToList();
            set => UserRoles = value.Select(roleId => new UserRole() { RoleId = roleId }).ToList();
        }
    }
}

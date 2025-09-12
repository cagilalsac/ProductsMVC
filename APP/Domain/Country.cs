using CORE.APP.Domain;
using System.ComponentModel.DataAnnotations;

namespace APP.Domain
{
    public class Country : Entity
    {
        [Required, StringLength(125)]
        public string CountryName { get; set; }

        public List<City> Cities { get; set; } = new List<City>();

        public List<User> Users { get; set; } = new List<User>();
    }
}

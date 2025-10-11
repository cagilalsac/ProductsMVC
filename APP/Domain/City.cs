using CORE.APP.Domain;
using CORE.APP.Domain.Files;
using System.ComponentModel.DataAnnotations;

namespace APP.Domain
{
    public class City : Entity, IFileEntity
    {
        [Required, StringLength(175)]
        public string CityName { get; set; }

        public int CountryId { get; set; }

        public Country Country { get; set; }

        public List<User> Users { get; set; } = new List<User>();

        public string FilePath { get; set; } // from IFileEntity
    }
}

using CORE.APP.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APP.Models
{
    public class CityRequest : Request
    {
        [Required, StringLength(175)]
        [DisplayName("Name")]
        public string CityName { get; set; }

        [Required]
        [DisplayName("Country")]
        public int? CountryId { get; set; }
    }
}

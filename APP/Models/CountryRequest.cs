using CORE.APP.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APP.Models
{
    public class CountryRequest : Request
    {
        [Required, StringLength(125)]
        [DisplayName("Name")]
        public string CountryName { get; set; }
    }
}

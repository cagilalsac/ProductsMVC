using CORE.APP.Models;
using System.ComponentModel;

namespace APP.Models
{
    public class CityResponse : Response
    {
        [DisplayName("City Name")]
        public string CityName { get; set; }

        public CountryResponse Country { get; set; }
    }
}

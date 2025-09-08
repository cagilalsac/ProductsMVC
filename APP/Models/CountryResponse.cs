using CORE.APP.Models;
using System.ComponentModel;

namespace APP.Models
{
    public class CountryResponse : Response
    {
        [DisplayName("Country Name")]
        public string CountryName { get; set; }

        public List<CityResponse> Cities { get; set; }
    }
}

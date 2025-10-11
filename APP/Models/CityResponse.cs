using CORE.APP.Models;
using CORE.APP.Models.Files;
using System.ComponentModel;

namespace APP.Models
{
    public class CityResponse : Response, IFileResponse
    {
        [DisplayName("City Name")]
        public string CityName { get; set; }

        public CountryResponse Country { get; set; }

        [DisplayName("Image")]
        public string FilePath { get; set; } // from IFileResponse
    }
}

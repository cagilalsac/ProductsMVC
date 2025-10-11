using CORE.APP.Models;
using CORE.APP.Models.Files;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APP.Models
{
    public class CityRequest : Request, IFileRequest
    {
        [Required, StringLength(175)]
        [DisplayName("Name")]
        public string CityName { get; set; }

        [Required]
        [DisplayName("Country")]
        public int? CountryId { get; set; }

        [DisplayName("Image")]
        public IFormFile FormFile { get; set; } // from IFileRequest
    }
}

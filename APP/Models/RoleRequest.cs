using CORE.APP.Models;
using System.ComponentModel.DataAnnotations;

namespace APP.Models
{
    public class RoleRequest : Request
    {
        [Required(ErrorMessage = "{0} is required!")]
        [StringLength(25, ErrorMessage = "{0} must be maximum {1} characters!")]
        public string Name { get; set; }
    }
}

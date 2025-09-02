using CORE.APP.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APP.Models
{
    // request properties are created according to the data that will be retrieved from APIs or UIs
    public class StoreRequest : Request
    {
        // copy all the non navigation properties from Store entity
        [Required, StringLength(200)]
        public string Name { get; set; }

        [DisplayName("Virtual")]
        public bool IsVirtual { get; set; }

        // since we won't create or update the relational product data (ProductStores) through this request,
        // we don't need to include the ProductIds property here
    }
}

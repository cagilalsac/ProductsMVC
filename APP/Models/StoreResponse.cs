using CORE.APP.Models;
using System.ComponentModel;

namespace APP.Models
{
    // response properties are created according to the data to be presented in API responses or UIs
    public class StoreResponse : Response
    {
        // copy all the non navigation properties from Store entity
        public string Name { get; set; }
        public bool IsVirtual { get; set; }



        // add the new properties, some ending with F for the properties with the same name, for custom or formatted string values
        [DisplayName("Status")]
        public string IsVirtualF { get; set; }

        [DisplayName("Product Count")]
        public int ProductCount { get; set; }

        public string Products { get; set; }
    }
}

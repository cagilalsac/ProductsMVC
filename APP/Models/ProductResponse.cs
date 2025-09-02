using CORE.APP.Models;
using System.ComponentModel;

namespace APP.Models
{
    // response properties are created according to the data to be presented in API responses or UIs
    public class ProductResponse : Response
    {
        // copy all the non navigation properties from Product entity
        public string Name { get; set; }
        
        public decimal UnitPrice { get; set; }

        public int? StockAmount { get; set; }
        
        public DateTime? ExpirationDate { get; set; }

        public int CategoryId { get; set; }

        public List<int> StoreIds { get; set; }




        // add the new properties, some ending with F for the properties with the same name, for custom or formatted string values
        [DisplayName("Unit Price")]
        public string UnitPriceF { get; set; }

        [DisplayName("Stock Amount")]
        public string StockAmountF { get; set; }

        [DisplayName("Expiration Date")]
        public string ExpirationDateF { get; set; }

        public string Category { get; set; }

        public List<string> Stores { get; set; }
    }
}

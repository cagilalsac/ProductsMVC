using CORE.APP.Models;
using System.ComponentModel;

namespace APP.Models
{
    public class ProductQueryRequest : Request
    {
        // Properties used for filtering Product entity query through the request:
        // All value type properties are defined as nullable because if they have values, their values will be applied for filtering.
        // Generally range filtering, including start and end values, is applied for DateTime and numeric properties.

        public string Name { get; set; }

        [DisplayName("Unit Price")]
        public decimal? UnitPriceStart { get; set; }

        public decimal? UnitPriceEnd { get; set; }

        [DisplayName("Stock Amount")]
        public int? StockAmountStart { get; set; }
        public int? StockAmountEnd { get; set; }

        [DisplayName("Expiration Date")]
        public DateTime? ExpirationDateStart { get; set; }
        public DateTime? ExpirationDateEnd { get; set; }

        [DisplayName("Category")]
        public int? CategoryId { get; set; }

        [DisplayName("Stores")]
        public List<int> StoreIds { get; set; } = new List<int>();
    }
}

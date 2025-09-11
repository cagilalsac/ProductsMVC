using CORE.APP.Models.Ordering;
using CORE.APP.Models.Paging;
using System.ComponentModel;

namespace APP.Models
{
    /// <summary>
    /// Represents a request for inner or left outer join queries between countries and cities, 
    /// including filtering, ordering, and paging options.
    /// </summary>
    public class LocationQueryRequest : IPageRequest, IOrderRequest // Interface Segregation Principle (I of SOLID) is applied
    {
        /// <summary>
        /// Gets or sets the country name filter for the query.
        /// </summary>
        [DisplayName("Country Name")]
        public string CountryName { get; set; }

        /// <summary>
        /// Gets or sets the city name filter for the query.
        /// </summary>
        [DisplayName("City Name")]
        public string CityName { get; set; }

        /// <summary>
        /// Gets or sets the current page number for paging (1-based, default 1).
        /// </summary>
        [DisplayName("Page Number")]
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Gets or sets the number of records to return per page for paging.
        /// </summary>
        [DisplayName("Record Count per Page")]
        public int CountPerPage { get; set; }

        /// <summary>
        /// Gets or sets the total number of records available for paging (for informational purposes and page number list creation).
        /// </summary>
        public int TotalCountForPaging { get; set; }

        /// <summary>
        /// Gets or sets the name of the entity property for ordering by, default CountryName (e.g., "CountryName" or "CityName").
        /// </summary>
        [DisplayName("Order Expression")]
        public string OrderEntityPropertyName { get; set; } = "CountryName";

        /// <summary>
        /// Gets or sets a value indicating whether the direction is ascending or descending for ordering.
        /// </summary>
        [DisplayName("Order Descending")]
        public bool IsOrderDescending { get; set; }
    }
}

using System.ComponentModel;

namespace APP.Models
{
    /// <summary>
    /// Represents the response object for inner and left outer join queries between countries and cities.
    /// </summary>
    public class LocationQueryResponse
    {
        /// <summary>
        /// Gets or sets the ID of the country.
        /// </summary>
        public int CountryId { get; set; }

        /// <summary>
        /// Gets or sets the name of the country.
        /// </summary>
        [DisplayName("Country Name")]
        public string CountryName { get; set; }

        /// <summary>
        /// Gets or sets the ID of the city.
        /// Defined as nullable int to support left outer join query where a city may not have an associated country.
        /// Can be defined as non-nullable int for inner join query since each city will have an associated country.
        /// </summary>
        public int? CityId { get; set; }

        /// <summary>
        /// Gets or sets the name of the city.
        /// </summary>
        [DisplayName("City Name")]
        public string CityName { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc.Rendering;

namespace APP.Models
{
    /// <summary>
    /// ViewModel for displaying a paginated, filterable, and orderable list of locations (countries and cities).
    /// Provides data and select lists for view elements such as paging, count per page, and ordering options.
    /// </summary>
    public class LocationsIndexViewModel
    {
        /// <summary>
        /// Gets or sets the list of location query responses representing countries and cities.
        /// </summary>
        public List<LocationQueryResponse> List { get; set; }

        /// <summary>
        /// Gets or sets the query request containing filtering, paging, and ordering parameters.
        /// </summary>
        public LocationQueryRequest QueryRequest { get; set; } = new LocationQueryRequest(); // to prevent Null Reference Exception



        /// <summary>
        /// Gets a select list of available page numbers for paging in the view.
        /// The list is dynamically generated based on the total record count and records per page count in <see cref="LocationQueryRequest"/>.
        /// </summary>
        public SelectList PageNumberSelectList
        {
            get
            {
                // Initialize the list to hold page number select list items.
                var pageNumberSelectListItems = new List<SelectListItem>();

                // Only generate page numbers if there are any records and a count per page greater than 0 (count per page not selected "All").
                if (QueryRequest is not null && QueryRequest.TotalCountForPaging > 0 && QueryRequest.CountPerPage > 0)
                {
                    // Calculate the total number of pages required.
                    var numberOfPages = Convert.ToInt32(Math.Ceiling(QueryRequest.TotalCountForPaging / Convert.ToDecimal(QueryRequest.CountPerPage)));

                    // Add a SelectListItem for each page number.
                    for (int pageNumber = 1; pageNumber <= numberOfPages; pageNumber++)
                    {
                        pageNumberSelectListItems.Add(new SelectListItem(pageNumber.ToString(), pageNumber.ToString()));
                    }
                }
                else
                {
                    // If no records or count per page selected "All", add 1 page number.
                    pageNumberSelectListItems.Add(new SelectListItem("1", "1"));
                }

                // Return the select list for use in the view.
                return new SelectList(pageNumberSelectListItems, "Value", "Text");
            }
        }

        /// <summary>
        /// Gets a select list of available record counts per page for paging in the view.
        /// Provides fixed options including "All" and several common page sizes.
        /// </summary>
        public SelectList CountPerPageSelectList
        {
            get
            {
                // Define the available options for records per page count.
                var countPerPageSelectListItems = new List<SelectListItem>
                {
                    new SelectListItem { Value = "0", Text = "All" },
                    new SelectListItem { Value = "5", Text = "5" },
                    new SelectListItem { Value = "10", Text = "10" },
                    new SelectListItem { Value = "25", Text = "25" },
                    new SelectListItem { Value = "50", Text = "50" },
                    new SelectListItem { Value = "100", Text = "100" }
                };

                // Return the select list for use in the view.
                return new SelectList(countPerPageSelectListItems, "Value", "Text");
            }
        }

        /// <summary>
        /// Gets a select list of available ordering options for ordering in the view.
        /// Provides fixed options to order by country name or city name.
        /// </summary>
        public SelectList OrderSelectList
        {
            get
            {
                // Define the available ordering options. Value property corresponds to entity property names.
                var orderSelectListItems = new List<SelectListItem>
                {
                    new SelectListItem { Value = "CountryName", Text = "Country Name" },
                    new SelectListItem { Value = "CityName", Text = "City Name" }
                };

                // Return the select list for use in the view.
                return new SelectList(orderSelectListItems, "Value", "Text");
            }
        }
    }
}

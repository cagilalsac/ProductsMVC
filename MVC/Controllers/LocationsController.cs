using APP.Models;
using APP.Services;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    /// <summary>
    /// Controller for handling location-related actions, including displaying lists of countries and cities
    /// using inner and left outer join queries.
    /// </summary>
    public class LocationsController : Controller
    {
        private readonly ILocationQueryService _locationQueryService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationsController"/> class.
        /// </summary>
        /// <param name="locationQueryService">Service for querying location data.</param>
        public LocationsController(ILocationQueryService locationQueryService)
        {
            _locationQueryService = locationQueryService;
        }

        /// <summary>
        /// Displays a list of locations (countries and cities) using an inner join query.
        /// </summary>
        /// <param name="viewModel">The view model containing query parameters, the result list, 
        /// page number list, record count per page list and order expression list.
        /// </param>
        /// <returns>The view displaying the inner join results.</returns>
        public IActionResult InnerJoinIndex(LocationsIndexViewModel viewModel)
        {
            viewModel.List = _locationQueryService.InnerJoinList(viewModel.QueryRequest);
            return View(viewModel);
        }

        /// <summary>
        /// Displays a list of locations (countries and cities) using a left outer join query.
        /// </summary>
        /// <param name="viewModel">The view model containing query parameters, the result list, 
        /// page number list, record count per page list and order expression list.
        /// </param>
        /// <returns>The view displaying the left outer join results.</returns>
        public IActionResult LeftJoinIndex(LocationsIndexViewModel viewModel)
        {
            viewModel.List = _locationQueryService.LeftJoinList(viewModel.QueryRequest);
            return View(viewModel);
        }
    }
}

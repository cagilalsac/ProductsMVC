#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CORE.APP.Services;
using APP.Models;
using Microsoft.AspNetCore.Authorization;

// Generated from Custom MVC Template.

namespace MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CountriesController : Controller
    {
        // Service injections:
        private readonly IService<CountryRequest, CountryResponse> _countryService;

        /* Can be uncommented and used for many to many relationships, "entity" may be replaced with the related entity name in the controller and views. */
        //private readonly IService<EntityRequest, EntityResponse> _EntityService;

        public CountriesController(
			IService<CountryRequest, CountryResponse> countryService

            /* Can be uncommented and used for many to many relationships, "entity" may be replaced with the related entity name in the controller and views. */
            //, IService<EntityRequest, EntityResponse> EntityService
        )
        {
            _countryService = countryService;

            /* Can be uncommented and used for many to many relationships, "entity" may be replaced with the related entity name in the controller and views. */
            //_EntityService = EntityService;
        }

        private void SetViewData()
        {
            /* 
            ViewBag and ViewData are the same collection (dictionary).
            They carry extra data other than the model from a controller action to its view, or between views.
            */

            // Related items service logic to set ViewData (Id and Name parameters may need to be changed in the SelectList constructor according to the model):
            
            /* Can be uncommented and used for many to many relationships, "entity" may be replaced with the related entity name in the controller and views. */
            //ViewBag.EntityIds = new MultiSelectList(_EntityService.List(), "Id", "Name");
        }

        private void SetTempData(string message, string key = "Message")
        {
            /*
            TempData is used to carry extra data to the redirected controller action's view.
            */

            TempData[key] = message;
        }

        // GET: Countries
        public IActionResult Index()
        {
            // Get collection service logic:
            var list = _countryService.List();
            return View(list); // return response collection as model to the Index view
        }

        // GET: Countries/Details/5
        public IActionResult Details(int id)
        {
            // Get item service logic:
            var item = _countryService.Item(id);
            return View(item); // return response item as model to the Details view
        }

        // GET: Countries/Create
        public IActionResult Create()
        {
            SetViewData(); // set ViewData dictionary to carry extra data other than the model to the view
            return View(); // return Create view with no model
        }

        // POST: Countries/Create
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(CountryRequest country)
        {
            if (ModelState.IsValid) // check data annotation validation errors in the request
            {
                // Insert item service logic:
                var response = _countryService.Create(country);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message); // set TempData dictionary to carry the message to the redirected action's view
                    return RedirectToAction(nameof(Details), new { id = response.Id }); // redirect to Details action with id parameter as response.Id route value
                }
                ModelState.AddModelError("", response.Message); // to display service error message in the validation summary of the view
            }
            SetViewData(); // set ViewData dictionary to carry extra data other than the model to the view
            return View(country); // return request as model to the Create view
        }

        // GET: Countries/Edit/5
        public IActionResult Edit(int id)
        {
            // Get item to edit service logic:
            var item = _countryService.Edit(id);
            SetViewData(); // set ViewData dictionary to carry extra data other than the model to the view
            return View(item); // return request as model to the Edit view
        }

        // POST: Countries/Edit
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(CountryRequest country)
        {
            if (ModelState.IsValid) // check data annotation validation errors in the request
            {
                // Update item service logic:
                var response = _countryService.Update(country);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message); // set TempData dictionary to carry the message to the redirected action's view
                    return RedirectToAction(nameof(Details), new { id = response.Id }); // redirect to Details action with id parameter as response.Id route value
                }
                ModelState.AddModelError("", response.Message); // to display service error message in the validation summary of the view
            }
            SetViewData(); // set ViewData dictionary to carry extra data other than the model to the view
            return View(country); // return request as model to the Edit view
        }

        // GET: Countries/Delete/5
        public IActionResult Delete(int id)
        {
            // Get item to delete service logic:
            var item = _countryService.Item(id);
            return View(item); // return response item as model to the Delete view
        }

        // POST: Countries/Delete
        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            // Delete item service logic:
            var response = _countryService.Delete(id);
            SetTempData(response.Message); // set TempData dictionary to carry the message to the redirected action's view
            return RedirectToAction(nameof(Index)); // redirect to the Index action
        }
	}
}

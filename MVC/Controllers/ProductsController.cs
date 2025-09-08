#nullable disable
using APP.Models;
using APP.Services;
using CORE.APP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

// Generated from Custom MVC Template.

namespace MVC.Controllers
{
    [Authorize] // Only authenticated users can execute the actions of the controller.
    public class ProductsController : Controller
    {
        // Service injections:
        private readonly IService<ProductRequest, ProductResponse> _productService;
        private readonly IService<CategoryRequest, CategoryResponse> _categoryService;

        /* Can be uncommented and used for many to many relationships, "entity" may be replaced with the related entity name in the controller and views. */
        private readonly IService<StoreRequest, StoreResponse> _StoreService;

        public ProductsController(
			IService<ProductRequest, ProductResponse> productService
            , IService<CategoryRequest, CategoryResponse> categoryService

            /* Can be uncommented and used for many to many relationships, "entity" may be replaced with the related entity name in the controller and views. */
            , IService<StoreRequest, StoreResponse> StoreService
        )
        {
            _productService = productService;
            _categoryService = categoryService;

            /* Can be uncommented and used for many to many relationships, "entity" may be replaced with the related entity name in the controller and views. */
            _StoreService = StoreService;
        }

        private void SetViewData()
        {
            /* 
            ViewBag and ViewData are the same collection (dictionary).
            They carry extra data other than the model from a controller action to its view, or between views.
            */

            // Related items service logic to set ViewData (Id and Name parameters may need to be changed in the SelectList constructor according to the model):
            ViewData["CategoryId"] = new SelectList(_categoryService.List(), "Id", "Title");

            /* Can be uncommented and used for many to many relationships, "entity" may be replaced with the related entity name in the controller and views. */
            ViewBag.StoreIds = new MultiSelectList(_StoreService.List(), "Id", "Name");
        }

        private void SetTempData(string message, string key = "Message")
        {
            /*
            TempData is used to carry extra data to the redirected controller action's view.
            */

            TempData[key] = message;
        }

        // GET: Products
        public IActionResult Index()
        {
            // Get collection service logic:
            var list = _productService.List();
            return View(list); // return response collection as model to the Index view
        }

        // GET: Products/Details/5
        public IActionResult Details(int id)
        {
            // Get item service logic:
            var item = _productService.Item(id);
            return View(item); // return response item as model to the Details view
        }

        // GET: Products/Create
        [Authorize(Roles = "Admin")] // Only authenticated users with role Admin can execute this action.
                                     // Overrides the Authorize defined for the controller.
        public IActionResult Create()
        {
            SetViewData(); // set ViewData dictionary to carry extra data other than the model to the view
            return View(); // return Create view with no model
        }

        // POST: Products/Create
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")] // Only authenticated users with role Admin can execute this action.
                                     // Overrides the Authorize defined for the controller.
        public IActionResult Create(ProductRequest product)
        {
            if (ModelState.IsValid) // check data annotation validation errors in the request
            {
                // Insert item service logic:
                var response = _productService.Create(product);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message); // set TempData dictionary to carry the message to the redirected action's view
                    return RedirectToAction(nameof(Details), new { id = response.Id }); // redirect to Details action with id parameter as response.Id route value
                }
                ModelState.AddModelError("", response.Message); // to display service error message in the validation summary of the view
            }
            SetViewData(); // set ViewData dictionary to carry extra data other than the model to the view
            return View(product); // return request as model to the Create view
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Admin")] // Only authenticated users with role Admin can execute this action.
                                     // Overrides the Authorize defined for the controller.
        public IActionResult Edit(int id)
        {
            // Get item to edit service logic:
            var item = _productService.Edit(id);
            SetViewData(); // set ViewData dictionary to carry extra data other than the model to the view
            return View(item); // return request as model to the Edit view
        }

        // POST: Products/Edit
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")] // Only authenticated users with role Admin can execute this action.
                                     // Overrides the Authorize defined for the controller.
        public IActionResult Edit(ProductRequest product)
        {
            if (ModelState.IsValid) // check data annotation validation errors in the request
            {
                // Update item service logic:
                var response = _productService.Update(product);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message); // set TempData dictionary to carry the message to the redirected action's view
                    return RedirectToAction(nameof(Details), new { id = response.Id }); // redirect to Details action with id parameter as response.Id route value
                }
                ModelState.AddModelError("", response.Message); // to display service error message in the validation summary of the view
            }
            SetViewData(); // set ViewData dictionary to carry extra data other than the model to the view
            return View(product); // return request as model to the Edit view
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Admin")] // Only authenticated users with role Admin can execute this action.
                                     // Overrides the Authorize defined for the controller.
        public IActionResult Delete(int id)
        {
            // Get item to delete service logic:
            var item = _productService.Item(id);
            return View(item); // return response item as model to the Delete view
        }

        // POST: Products/Delete
        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        [Authorize(Roles = "Admin")] // Only authenticated users with role Admin can execute this action.
                                     // Overrides the Authorize defined for the controller.
        public IActionResult DeleteConfirmed(int id)
        {
            // Delete item service logic:
            var response = _productService.Delete(id);
            SetTempData(response.Message); // set TempData dictionary to carry the message to the redirected action's view
            return RedirectToAction(nameof(Index)); // redirect to the Index action
        }



        // Filtering:
        // GET: Products/List
        public IActionResult List()
        {
            // get all products
            var list = _productService.List();

            // get categories for the categories drop down list (select HTML tag without multiple attribute)
            var categorySelectList = new SelectList(_categoryService.List(), "Id", "Title");

            // get stores for the stores list box (select HTML tag with multiple attribute)
            var storeMultiSelectList = new MultiSelectList(_StoreService.List(), "Id", "Name");

            // create the view model to be sent to the view
            var viewModel = new ProductsIndexViewModel
            {
                List = list,
                Filter = new ProductQueryRequest(), // filter object with no filter criteria
                CategorySelectList = categorySelectList,
                StoreMultiSelectList = storeMultiSelectList
            };

            // send the view model to the view as the model
            return View(viewModel);
        }

        // POST: Products/List
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult List(ProductsIndexViewModel viewModel)
        {
            // cast to the concrete service type to access the overloaded List method with filter query request parameter
            var list = (_productService as ProductService).List(viewModel.Filter); // get filtered products by the view model's filter request

            // get categories for the categories drop down list as request's CategoryId value selected (select HTML tag without multiple attribute)
            var categorySelectList = new SelectList(_categoryService.List(), "Id", "Title", viewModel.Filter.CategoryId);

            // get stores for the stores list box as request's StoreIds values selected (select HTML tag with multiple attribute)
            var storeMultiSelectList = new MultiSelectList(_StoreService.List(), "Id", "Name", viewModel.Filter.StoreIds);

            // update the view model to be sent to the view, no need to update the Filter property because it is received from the view with data
            viewModel.List = list;
            viewModel.CategorySelectList = categorySelectList;
            viewModel.StoreMultiSelectList = storeMultiSelectList;

            // send the view model to the view as the model
            return View(viewModel);
        }
	}
}

using APP.Models;
using APP.Services;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    /// <summary>
    /// Controller responsible for handling category-related HTTP requests.
    /// Provides actions for listing, viewing details, creating, editing, updating and deleting categories.
    /// </summary>
    [Obsolete("Use CategoriesController class instead!")]
    public class CategoriesObsoleteController : Controller
    {
        /// <summary>
        /// The service that provides business logic of CRUD operations for categories.
        /// </summary>
        private readonly CategoryObsoleteService _categoryService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoriesObsoleteController"/> class.
        /// Injects the <see cref="CategoryObsoleteService"/> dependency via constructor injection.
        /// Concrete type object injection is not suitable for SOLID Principles (D: Dependency Inversion) and Unit Testing.
        /// </summary>
        /// <param name="categoryService">The injected <see cref="CategoryObsoleteService"/> instance used to perform category operations.
        /// This service is typically registered in the dependency injection container (IoC) in Program.cs and provided by ASP.NET Core at runtime.
        /// </param>
        public CategoriesObsoleteController(CategoryObsoleteService categoryService)
        {
            // Store the injected CategoryService instance for use in controller actions.
            _categoryService = categoryService;
        }

        // GET: Categories
        /// <summary>
        /// Displays a list of all categories.
        /// </summary>
        /// <returns>The view with the list of categories of type <see cref="CategoryResponse"/> and a record count message.</returns>
        public IActionResult Index()
        {
            // Retrieve all categories as a list from the service.
            var list = _categoryService.Query().ToList();

            // Set a message value in ViewBag for the Count key based on the number of categories found.
            ViewBag.Count = list.Count == 0 ? "No categories found!" : list.Count == 1 ? "1 category found." : $"{list.Count} categories found.";

            // Return the view with the list of categories.
            return View(list);
        }

        // GET: Categories/Details/13
        /// <summary>
        /// Displays the details of a specific category.
        /// </summary>
        /// <param name="id">The unique identifier of the category to retrieve.</param>
        /// <returns>The view with the category details of type <see cref="CategoryResponse"/>, or a not found message if the category does not exist.</returns>
        public IActionResult Details(int id)
        {
            // Retrieve the category with the specified ID.
            var item = _categoryService.Query().SingleOrDefault(categoryResponse => categoryResponse.Id == id);

            // If not found, set a not found message.
            if (item is null)
                ViewBag.Message = "Category not found!";

            // Return the view with the category details (or null if not found).
            return View(item);
        }

        // GET: Categories/Create
        /// <summary>
        /// Displays the form in the view for creating a new category.
        /// </summary>
        /// <returns>The create view.</returns>
        [HttpGet] // this action method (attribute) specifies that this action only handles HTTP GET requests, no need to write since it's the default
        public IActionResult Create()
        {
            // Return the empty create view.
            return View();
        }

        // POST: Categories/Create
        /// <summary>
        /// Handles the submission of a new category.
        /// </summary>
        /// <param name="request">The category data submitted from the create view's form.</param>
        /// <returns>
        /// The view with a success message if creation is successful, or if fails the view with the request model that may include validation or service errors.
        /// </returns>
        [HttpPost] // this action method (attribute) specifies that this action only handles HTTP POST requests, default is [HttpGet] if not written
        public IActionResult Create(CategoryRequest request)
        {
            // Check if the model state is valid through the data annotations of the request before attempting to create.
            if (ModelState.IsValid)
            {
                // Insert the category using the service.
                var response = _categoryService.Create(request);

                // If creation was successful, redirect to the Index action to show the updated category list with operation result message.
                if (response.IsSuccessful)
                {
                    // Set the operation result message in TempData dictionary.
                    TempData["Message"] = response.Message;

                    // Redirect to the Index action to show the updated category list.
                    return RedirectToAction(nameof(Index));
                }

                // If creation fails, set the error result message in ViewBag.
                ViewBag.Message = response.Message;
            }

            // If model state is invalid or creation failed, return the view with the request model data.
            return View(request);
        }

        // GET: Categories/Edit/13
        /// <summary>
        /// Displays the form in the view for editing an existing category.
        /// </summary>
        /// <param name="id">The unique identifier of the category to edit.</param>
        /// <returns>The edit view with the category request model data.</returns>
        public IActionResult Edit(int id)
        {
            // Retrieve the category data by ID for editing.
            var request = _categoryService.Edit(id);

            // If not found, set a not found message.
            if (request is null)
                ViewBag.Message = "Category not found!";

            // Return the edit view with the category request model data.
            return View(request);
        }

        // POST: Categories/Edit
        /// <summary>
        /// Handles the submission of a category update.
        /// </summary>
        /// <param name="request">The updated category data submitted from the edit view's form.</param>
        /// <returns>
        /// The view with a success message if update is successful, or if fails the view with the request model that may include validation or service errors.
        /// </returns>
        [HttpPost] // this action method (attribute) specifies that this action only handles HTTP POST requests, default is [HttpGet] if not written
        public IActionResult Edit(CategoryRequest request)
        {
            // Check if the model state is valid through the data annotations of the request before attempting to update.
            if (ModelState.IsValid)
            {
                // Update the category using the service.
                var response = _categoryService.Update(request);

                // If update was successful, redirect to the details action using the id parameter set as response.Id value.
                if (response.IsSuccessful)
                    return RedirectToAction(nameof(Details), new { id = response.Id });

                // If update fails, set the error result message in ViewBag.
                ViewBag.Message = response.Message;
            }

            // If model state is invalid or update fails, return the view with the request model data.
            return View(request);
        }

        // GET: Categories/Delete/13
        /// <summary>
        /// Deletes a category and redirects to show the updated catergory list.
        /// </summary>
        /// <param name="id">The unique identifier of the category to delete.</param>
        /// <returns>Redirects to the Index action with a operation result message.</returns>
        public IActionResult Delete(int id)
        {
            // Delete the category with the specified ID.
            var response = _categoryService.Delete(id);

            // Store the result message in TempData to display after redirect.
            TempData["Message"] = response.Message;

            // Redirect to the Index action to show the updated category list.
            return RedirectToAction(nameof(Index));
        }
    }
}

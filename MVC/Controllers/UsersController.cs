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
    public class UsersController : Controller
    {
        // Service injections:
        private readonly IService<UserRequest, UserResponse> _userService;
        private readonly IService<GroupRequest, GroupResponse> _groupService;

        // TODO
        //private readonly IService<CityRequest, CityResponse> _cityService;
        //private readonly IService<CountryRequest, CountryResponse> _countryService;

        /* Can be uncommented and used for many to many relationships, "entity" may be replaced with the related entity name in the controller and views. */
        private readonly IService<RoleRequest, RoleResponse> _RoleService;

        public UsersController(
			IService<UserRequest, UserResponse> userService
            , IService<GroupRequest, GroupResponse> groupService

        // TODO
        //, IService<CityRequest, CityResponse> cityService
        //, IService<CountryRequest, CountryResponse> countryService

        /* Can be uncommented and used for many to many relationships, "entity" may be replaced with the related entity name in the controller and views. */
        , IService<RoleRequest, RoleResponse> RoleService
        )
        {
            _userService = userService;
            _groupService = groupService;

            // TODO
            //_cityService = cityService;
            //_countryService = countryService;

            /* Can be uncommented and used for many to many relationships, "entity" may be replaced with the related entity name in the controller and views. */
            _RoleService = RoleService;
        }

        private void SetViewData()
        {
            /* 
            ViewBag and ViewData are the same collection (dictionary).
            They carry extra data other than the model from a controller action to its view, or between views.
            */

            // Related items service logic to set ViewData (Id and Name parameters may need to be changed in the SelectList constructor according to the model):
            ViewData["GroupId"] = new SelectList(_groupService.List(), "Id", "Title");

            // TODO
            //ViewData["CityId"] = new SelectList(_cityService.List(), "Id", "CityName");
            //ViewData["CountryId"] = new SelectList(_countryService.List(), "Id", "CountryName");

            /* Can be uncommented and used for many to many relationships, "entity" may be replaced with the related entity name in the controller and views. */
            ViewBag.RoleIds = new MultiSelectList(_RoleService.List(), "Id", "Name");
        }

        private void SetTempData(string message, string key = "Message")
        {
            /*
            TempData is used to carry extra data to the redirected controller action's view.
            */

            TempData[key] = message;
        }

        // GET: Users
        // Way 1:
        //[Authorize(Roles = "Admin,User")] // Only authenticated users with role Admin or User can execute this action.
        // Way 2: since we have only 2 roles Admin and User, we can use Authorize to check auhenticated users without defining roles.
        [Authorize] // Only authenticated users can execute this action.
        public IActionResult Index()
        {
            // Get collection service logic:
            var list = _userService.List();
            return View(list); // return response collection as model to the Index view
        }

        // GET: Users/Details/5
        [Authorize(Roles = "Admin")] // Only authenticated users with role Admin can execute this action.
        public IActionResult Details(int id)
        {
            // Get item service logic:
            var item = _userService.Item(id);
            return View(item); // return response item as model to the Details view
        }

        // GET: Users/Create
        [Authorize(Roles = "Admin")] // Only authenticated users with role Admin can execute this action.
        public IActionResult Create()
        {
            SetViewData(); // set ViewData dictionary to carry extra data other than the model to the view
            return View(); // return Create view with no model
        }

        // POST: Users/Create
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")] // Only authenticated users with role Admin can execute this action.
        public IActionResult Create(UserRequest user)
        {
            if (ModelState.IsValid) // check data annotation validation errors in the request
            {
                // Insert item service logic:
                var response = _userService.Create(user);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message); // set TempData dictionary to carry the message to the redirected action's view
                    return RedirectToAction(nameof(Details), new { id = response.Id }); // redirect to Details action with id parameter as response.Id route value
                }
                ModelState.AddModelError("", response.Message); // to display service error message in the validation summary of the view
            }
            SetViewData(); // set ViewData dictionary to carry extra data other than the model to the view
            return View(user); // return request as model to the Create view
        }

        // GET: Users/Edit/5
        [Authorize(Roles = "Admin")] // Only authenticated users with role Admin can execute this action.
        public IActionResult Edit(int id)
        {
            // Get item to edit service logic:
            var item = _userService.Edit(id);
            SetViewData(); // set ViewData dictionary to carry extra data other than the model to the view
            return View(item); // return request as model to the Edit view
        }

        // POST: Users/Edit
        [HttpPost, ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")] // Only authenticated users with role Admin can execute this action.
        public IActionResult Edit(UserRequest user)
        {
            if (ModelState.IsValid) // check data annotation validation errors in the request
            {
                // Update item service logic:
                var response = _userService.Update(user);
                if (response.IsSuccessful)
                {
                    SetTempData(response.Message); // set TempData dictionary to carry the message to the redirected action's view
                    return RedirectToAction(nameof(Details), new { id = response.Id }); // redirect to Details action with id parameter as response.Id route value
                }
                ModelState.AddModelError("", response.Message); // to display service error message in the validation summary of the view
            }
            SetViewData(); // set ViewData dictionary to carry extra data other than the model to the view
            return View(user); // return request as model to the Edit view
        }

        // GET: Users/Delete/5
        [Authorize(Roles = "Admin")] // Only authenticated users with role Admin can execute this action.
        public IActionResult Delete(int id)
        {
            // Get item to delete service logic:
            var item = _userService.Item(id);
            return View(item); // return response item as model to the Delete view
        }

        // POST: Users/Delete
        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        [Authorize(Roles = "Admin")] // Only authenticated users with role Admin can execute this action.
        public IActionResult DeleteConfirmed(int id)
        {
            // Delete item service logic:
            var response = _userService.Delete(id);
            SetTempData(response.Message); // set TempData dictionary to carry the message to the redirected action's view
            return RedirectToAction(nameof(Index)); // redirect to the Index action
        }



        /// <summary>
        /// Displays the login view for user authentication.
        /// Route changed from GET: /Users/Login to GET: /Login for easier access.
        /// </summary>
        [Route("~/[action]")]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Handles user login form submission.
        /// POST: /Login
        /// </summary>
        /// <param name="request">The login request containing user credentials (UserName and Password).</param>
        /// <returns>
        /// Redirects to Home/Index on successful login; otherwise, redisplays the login view with 
        /// validation errors or authentication failure message.
        /// </returns>
        [HttpPost, ValidateAntiForgeryToken]
        [Route("~/[action]")]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            if (ModelState.IsValid) // Validate input model (checks required fields and string lengths through data annotations)
            {
                var userService = _userService as UserService; // Cast to concrete UserService to access Login method
                                                               // since injected IService instance does not have Login method definition
                var response = await userService.Login(request); // Attempt to authenticate user
                if (response.IsSuccessful)
                    return RedirectToAction("Index", "Home"); // On success, redirect to home page
                ModelState.AddModelError("", response.Message); // On failure, show error message in the validation summary of the view
            }
            return View(); // Redisplay login view with validation errors or authentication failure message
        }

        /// <summary>
        /// Logs out the current user and redirects to Login.
        /// Route changed from GET: /Users/Logout to GET: /Logout for easier access.
        /// </summary>
        /// <remarks>This method terminates the user's authentication cookie.</remarks>
        /// <returns>Redirects to Login.</returns>
        [Route("~/[action]")]
        public async Task<IActionResult> Logout()
        {
            var userService = _userService as UserService; // Cast to concrete UserService to access Logout method
                                                           // since injected IService instance does not have Logout method definition
            await userService.Logout(); // Sign out the user
            return RedirectToAction(nameof(Login)); // Redirect to the Login view through the Login action
        }

        /// <summary>
        /// Displays the register view for user registration.
        /// Route changed from GET: /Users/Register to GET: /Register for easier access.
        /// </summary>
        [Route("~/[action]")]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Handles user register form submission.
        /// POST: /Register
        /// </summary>
        /// <param name="request">The register request containing UserName, Password and ConfirmPassword.</param>
        /// <returns>
        /// Redirects to Login on successful registration; otherwise, redisplays the register view with validation errors 
        /// or registration failure message.
        /// </returns>
        [HttpPost, ValidateAntiForgeryToken, Route("~/[action]")]
        public IActionResult Register(UserRegisterRequest request)
        {
            if (ModelState.IsValid) // Validate input model through data annotations
            {
                var userService = _userService as UserService; // Cast to concrete UserService to access Register method
                                                               // since injected IService instance does not have Register method definition
                var response = userService.Register(request); // Attempt to register user
                if (response.IsSuccessful)
                    return RedirectToAction(nameof(Login)); // On success, redirect to Login view through the Login action
                ModelState.AddModelError("", response.Message); // On failure, show error message in the validation summary of the view
            }
            return View(request); // Redisplay register view with the model and validation errors or registration failure message
        }
    }
}

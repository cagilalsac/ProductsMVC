using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using System.Diagnostics;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        // Declares an optional logger for this controller and assigns it via dependency injection in the constructor.
        // Provides logging information, warnings, errors, and other diagnostic messages within this controller.
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // This action can be executed in the browser's address bar to return the Index view as entering the URLs
        // "https://localhost:port/Home/Index" or "http://localhost:port/Home/Index".
        // The route of the action is /Home/Index according to the default MVC routing pattern defined in Program.cs file as
        // "{controller=Home}/{action=Index}/{id?}".
        // The default action is defined as Index therefore the Index action of a controller will be executed when action name
        // is not provided such as the route: "/Home" ("https://localhost:port/Home").
        // The default controller is Home therefore the Index action of the Home controller will be executed when both action
        // and controller names are not provided such as the route: "/" ("https://localhost:port").
        // ? means id value is optional. So if provided as the action's parameter as Index(int id), the route will be: "/Home/Index/7"
        // ("https://localhost:port/Home/Index/7") where id parameter value will be 7.
        public IActionResult Index() // Index action without returning a model to the Index view
        {
            return View(); // Right-click and then Go to View to see the Index.cshtml view under Views/Home folder

            // The returned view (Views/Home/Index.cshtml) is first searched under Views/Home folder, then Views/Shared folder,
            // and the view is returned if found.
            // Views in the Shared folder can be returned from any controller's any action.
        }



        // Some action result examples:

        /* 
        ActionResult inheritance:
        IActionResult : general return type of actions in a controller
        |
        ActionResult: base class that implements IActionResult
        |
        ViewResult (returned by View method) - ContentResult (returned by Content method) - NotFoundResult (returned by NotFound method) - 
        RedirectResults - HttpStatusCodeResults - JsonResult - etc. 
        */

        // The route of the action is /Home/View1

        // Way 1:
        //public ViewResult View1()
        // Way 2: since ViewResult inherits ActionResult and ActionResult implements IActionResult, IActionResult can be used as return type
        public IActionResult View1() // Right-click then Add View to create an Emtpy view named View1.cshtml under Views/Home folder
        {
            // Way 1:
            //string model = "View1 view returned with this string Model object from View1 action.";
            // Way 2:
            object model = "View1 view returned with this string Model object from View1 action."; // all classes in C# inherit from the Object class

            // Carrying extra data other than the Model to the view
            // Way 1:
            //ViewData["Text"] = "Extra string text other than the Model object is carried via ViewData dictionary from View1 action."; 
            // Way 2:
            ViewBag.Text = "Extra string text other than the Model object is carried via ViewData dictionary from View1 action."; 
            // objects can also be assigned

            return View(model); // Model object types can be string, int, etc. also generally complex types such as class
        }

        // The route of the action is /Home/View2
        public IActionResult View2()
        {
            // Way 1:
            //string model = "View1 view returned with this string Model object from View2 action.";
            // Way 2: var can be used instead of types if there is an assignment
            var model = "View1 view returned with this string Model object from View2 action.";
            return View("View1", model);
        }

        // The route of the action is /Home/Content1
        public IActionResult Content1()
        {
            return Content("Plain string text content returned from Content1 action."); // optionally text/plain and Encoding.UTF8 parameters
                                                                                        // (for Turkish character problems) can be provided
        }

        // The route of the action is /Home/Content2
        public IActionResult Content2()
        {
            return Content("<b>String HTML content returned from Content2 action.</b>", "text/html"); // optionally Encoding.UTF8 parameter
                                                                                                      // (for Turkish character problems) can be provided
        }

        // The route of the action is /Home/Redirect1
        public IActionResult Redirect1()
        {
            // If a view is returned from an action, extra data may be sent to the view via ViewData (ViewBag) dictionary.
            // If a redirection to another action occurs in the action, data may be sent to the redirected action,
            // therefore the redirected action's view via TempData dictionary.
            TempData["Message"] = "Message string is carried from Redirect1 action to the View1 action, therefore View1 action's " +
                "returned view via TempData dictionary.";

            // Way 1:
            //return RedirectToAction("View1", "Home");
            // Way 2:
            //return RedirectToAction("View1");
            // Way 3:
            return RedirectToAction(nameof(View1)); // nameof returns the string representation of the View1 method
                                                    // and can also be used for classes, class properties, etc.
        }

        // The route of the action is /Home/NotFound1
        public IActionResult NotFound1()
        {
            // Logs a warning message to the Kestrel Console or Visual Studio Output window.
            // LogError, LogInformation, LogDebug, LogCritical and LogTrace methods can also be used.
            // Logging is optional, therefore ILogger instance doesn't need to be injected in every controller.
            _logger.LogWarning("Home controller NotFound1 action is executed."); 

            return NotFound("Message string returned from the NotFound1 action."); // returns HTTP 404 Not Found status code, parameter is optional
        }



        // The route of the action is /Home/Privacy
        public IActionResult Privacy()
        {
            return View(); // Will return the Privacy.cshtml view under Views/Home folder
        }

        // This action is used for displaying error information of the operations.
        // ResponseCache is an attribute used for disabling response caching for this action so that every request gets a fresh result.
        // Attributes gain new features to the methods, properties, classes, etc. they are applied to.
        // The route of the action is /Home/Error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() // Will return the Error.cshtml view under Views/Shared folder sending the Model object as type ErrorViewModel class
        {
            return View(new ErrorViewModel
            {
                // Set RequestId to the current activity's ID if available, otherwise uses the HTTP request's unique trace identifier.
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}

using APP.Domain;
using APP.Models;
using APP.Services;
using APP.Services.Carts;
using CORE.APP.Services;
using CORE.APP.Services.Authentication.MVC;
using CORE.APP.Services.Session.MVC;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

// Create a builder for the web application and initialize configuration, logging, etc.
var builder = WebApplication.CreateBuilder(args);



// -----------------------------------------------------------------------------------
// Add services to the IoC (Inversion of Control) container for Dependency Injections.
// -----------------------------------------------------------------------------------
// Register the application's DbContext dependency injection by sending Db instances to the injected service class constructors'
// DbContext or Db parameter, using SQLite with the connection string from appsettings.json.
builder.Services.AddDbContext<DbContext, Db>(options => options.UseSqlite(builder.Configuration.GetConnectionString("Db")));

// Register the CategoryObsoleteService dependency injection by sending CategoryObsoleteService instance to the injected controller class
// constructor's CategoryObsoleteService parameter. Concrete type object injection is not suitable for SOLID Principles.
// Therefore, this injection is a temporary example and will be replaced with the suitable injection in the future.
// This is why the service name is marked with "Obsolete".
builder.Services.AddScoped<CategoryObsoleteService>();

// Register the CategoryService dependency injection by sending IService<CategoryRequest, CategoryResponse> instance reference to the
// injected controller class constructor's IService<CategoryRequest, CategoryResponse> parameter.
// Injection through abstract class or interface is suitable for SOLID Principles.
builder.Services.AddScoped<IService<CategoryRequest, CategoryResponse>, CategoryService>();

// Register the StoreService dependency injection by sending IService<StoreRequest, StoreResponse> instance reference to the
// injected controller class constructor's IService<StoreRequest, StoreResponse> parameter.
// Injection through abstract class or interface is suitable for SOLID Principles.
builder.Services.AddScoped<IService<StoreRequest, StoreResponse>, StoreService>();

// Register the ProductService dependency injection by sending IService<ProductRequest, ProductResponse> instance reference to the
// injected controller class constructor's IService<ProductRequest, ProductResponse> parameter.
// Injection through abstract class or interface is suitable for SOLID Principles.
builder.Services.AddScoped<IService<ProductRequest, ProductResponse>, ProductService>();

builder.Services.AddScoped<IService<GroupRequest, GroupResponse>, GroupService>();
builder.Services.AddScoped<IService<RoleRequest, RoleResponse>, RoleService>();
builder.Services.AddScoped<IService<UserRequest, UserResponse>, UserService>();

// Register IHttpContextAccessor as a singleton service.
// Enables access to the current HttpContext from classes other than controller classes such as services.
// Required for services like CookieAuthService that need to read or modify HTTP context (e.g., authentication, user info).
// Allows constructor injection of IHttpContextAccessor throughout the services of the application.
builder.Services.AddHttpContextAccessor();

// Register CookieAuthService as a scoped dependency for ICookieAuthService.
// Scoped lifetime ensures a new instance per HTTP request, which is required for services accessing HttpContext.
// CookieAuthService handles user authentication using cookies, supporting login and logout operations.
// This service registration enables constructor injection of ICookieAuthService to the controllers or services throughout the application.
builder.Services.AddScoped<ICookieAuthService, CookieAuthService>();



// -------
// Session
// -------
// Register session services and configure session options.
// Sets the session idle timeout to 30 minutes (default 20 minutes); if no activity occurs within this period, the session expires.
// Enables storing user-specific data (e.g., shopping cart) across multiple requests during the session lifetime.
// Required for features that rely on session state, such as CartService and SessionService.
builder.Services.AddSession(config =>
{
    config.IdleTimeout = TimeSpan.FromMinutes(30);
});



// Register SessionService as a scoped dependency for SessionServiceBase.
// Scoped lifetime ensures a new instance per HTTP request, which is required for services accessing HttpContext.
// SessionService handles session management.
// This service registration enables constructor injection of SessionServiceBase to the controllers or services throughout the application.
builder.Services.AddScoped<SessionServiceBase, SessionService>();

// Register CartService as a scoped dependency for ICartService.
// Scoped lifetime ensures a new instance per HTTP request.
// CartService handles shopping cart management.
// This service registration enables constructor injection of ICartService to the controllers or services throughout the application.
builder.Services.AddScoped<ICartService, CartService>();

/* 
 SOLID Principles:
 1.	Single Responsibility Principle (SRP)
    A class should have only one reason to change, meaning it should have only one job or responsibility.
 2.	Open/Closed Principle (OCP)
    Software entities (classes, modules, functions) should be open for extension but closed for modification. 
    You should be able to add new functionality without changing existing code.
 3. Liskov Substitution Principle (LSP)
    Subtypes must be substitutable for their base types. Derived classes should extend base classes without changing their behavior.
 4.	Interface Segregation Principle (ISP)
    No client should be forced to depend on methods it does not use. Prefer small, specific interfaces over large, general-purpose ones.
 5. Dependency Inversion Principle (DIP)
    High-level modules should not depend on low-level modules; both should depend on abstractions (e.g., interfaces). 
    This is commonly implemented in ASP.NET Core using dependency injection, as seen in Program.cs.
/*

 * Service Lifetimes in ASP.NET Core Dependency Injection:
 *
 * 1. AddScoped:
 *    - Lifetime: Scoped to a single HTTP request (or scope).
 *    - Behavior: Creates one instance of the service per HTTP request.
 *    - Use case: Use when you want to maintain state or dependencies that last only during a single request.
 *    - Example: DbContext, which should be shared across operations within a request, generally added with AddDbContext method.
 *
 * 2. AddSingleton:
 *    - Lifetime: Singleton for the entire application lifetime.
 *    - Behavior: Creates only one instance of the service for the whole app lifecycle.
 *    - Use case: Use for stateless services or global shared data/services.
 *    - Example: Caching services, configuration providers, logging services.
 *
 * 3. AddTransient:
 *    - Lifetime: Transient (short-lived).
 *    - Behavior: Creates a new instance every time the service is requested.
 *    - Use case: Use for lightweight, stateless services that are cheap to create.
 *    - Example: Utility/helper classes without state.
 *
 * Notes:
 * - Injecting a Scoped service into a Singleton can cause issues due to lifetime mismatch.
 * - ASP.NET Core DI container will warn about such mismatches.
 *
 * Summary:
 * | Method        | Lifetime                | Instance Created             | Typical Use Case                  |
 * |---------------|-------------------------|------------------------------|-----------------------------------|
 * | AddScoped     | Per HTTP request        | One instance per request     | DbContext, per-request services   |
 * | AddSingleton  | Application-wide        | One instance for app lifetime| Caching, config, logging          |
 * | AddTransient  | Every time requested    | New instance each time       | Lightweight stateless helpers     |
 */



// --------------
// Authentication
// --------------
// Configure authentication services using cookie-based authentication.
// - Sets the default authentication scheme to Cookies.
// - Specifies options for cookie authentication:
//   - LoginPath: Path to redirect unauthenticated users (when authentication is required).
//   - AccessDeniedPath: Path to redirect users who are authenticated but lack required permissions.
//   - ExpireTimeSpan: Duration before the authentication cookie expires (here, 1 hour).
//   - SlidingExpiration: Resets the expiration time on each request to keep active sessions alive.
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login"; // changed from /Users/Login to /Login since route was changed for the action
        options.AccessDeniedPath = "/Login"; // changed from /Users/Login to /Login since route was changed for the action
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.SlidingExpiration = true;
    });



// Add support for controllers and views (MVC pattern).
builder.Services.AddControllersWithViews();

// Build the application.
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // Use a custom error handler in non-development environments redirecting to Home controller's Error action.
    app.UseExceptionHandler("/Home/Error");
    // Enable HTTP Strict Transport Security (HSTS).
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// ASP.NET Core Environments:
// The environment is development when the MVC application is run from Visual Studio choosing a development profile from the
// launchSettings.json file in the Properties folder of the MVC Project.
// When the MVC application is run on a server or from Visual Studio choosing a production profile from the launchSettings.json
// file, the environment is production.
// In launchSettings.json, http profile was defined as Development through ASPNETCORE_ENVIRONMENT section, https profile's
// ASPNETCORE_ENVIRONMENT value was changed to Production from Development for using the production environment.
// The environments can be changed from the drop down list (selected as https) near the run button under the Visual Studio top menu
// when running the application. Before, MVC must be set as startup project from the drop down list at left of the run button.
// If https (production environment) is selected, sections in appsettings.json will be used for configuration,
// if http (development environment) is selected, sections in appsettings.Development.json will be used for configuration.
// Therefore, same sections with some different values (such as connection string) must present in both files.
// Environment configurations and usage is not a must for the development of applications.

// Redirect HTTP requests to HTTPS.
app.UseHttpsRedirection();

// Enable serving static files (e.g., CSS, JS, images).
app.UseStaticFiles();

// Enable routing capabilities.
app.UseRouting();



// --------------
// Authentication
// --------------
// Enable authentication middleware so that [Authorize] works.
app.UseAuthentication();



// Enable authorization middleware.
app.UseAuthorization();



// -------
// Session
// -------
// Enable session middleware in the HTTP request pipeline.
app.UseSession();



// Configure the default route for controllers for sending requests to controllers' actions as controller/action/id where id is optional.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Start the application and listen for incoming HTTP requests.
app.Run();

# Project Development Roadmap

Note: The development of Users and Locations Modules will not be explained. You need to develop Users Module as homework 
      while learning from the Products Module. Locations Module is optional.  
      You also need to develop your own MVC Project including Users Module for a different domain such as Movies, Books, etc.  
      Users and optional Locations diagrams with some project example diagrams can be found at:  
      https://need4code.com/DotNet?path=.NET%5C00_Files%5CProjects%5CDiagrams.jpg

Note: A simplified version of the N-Layered Architecture and Repository Pattern that include the basic concepts and structures 
      are applied to the project. More information about N-Layered Architecture and other architectures can be found at:  
      https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures

Note: Domain is for data access from a database, services are for business logic and MVC is for presentation. 
      CQRS (Command Query Response Segregation) Pattern is also applied to this project.

## 1. Environment and Tools

1. Visual Studio Community installation for Windows:  
   https://need4code.com/DotNet/Home/Index?path=.NET%5C00_Files%5CVisual%20Studio%20Community%5CInstallation.pdf

2. Rider for MAC:  
   https://www.jetbrains.com/rider

3. SQLite Database:  
   https://www.sqlite.org

## 2. Solution Setup

4. Create the ASP.NET Core Web App (Model-View-Controller) project.

5. Give the Project name MVC. You may change the solution folder in Location. Give the Solution name your project name. Place solution and project in the 
   same directory sould not be checked.

6. Select ".NET 8.0" as the "Framework", choose "None" for "Authentication type", check "Configure for HTTPS", do not check "Enable container support", do not check 
   "Do not use top-levet statements" and do not check "Enlist in .NET Aspire orchestration".

## 3. MVC Project: General topics explained in details in this file and solution's projects' files.

7. Controller classes have action methods that handle the incoming HTTP get or post requests, optionally interact with entity data in the database through 
   service classes using model (DTO: Data Transfer Object) classes and generally return views with or without model class data.

  - ActionResult inheritance:  
    IActionResult : general return type of actions in a controller  
    |  
    ActionResult: base class that implements IActionResult  
    |  
    ViewResult (returned by View method) - ContentResult (returned by Content method) - NotFoundResult (returned by NotFound method) -  
    RedirectResults - HttpStatusCodeResults - JsonResult - etc.

  - Check the HomeController in the Controllers folder of the MVC Project for the example usages of actions and views:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Controllers/HomeController.cs

  - You can right-click an action to go to its view with the same name such as Index action to go to the Index view (Index.cshtml) in 
    Views/Home folder and Privacy action to go to the Privacy view (Privacy.cshtml) in Views/Home folder. You should change the HTML content 
    in these views according to your project.

  - Razor syntax provides the usage of HTML and C# together in views:  
    ```
    In order to switch to C# in HTML, @ is used.  
   
    C# code blocks can be written in @{ ... }, for example  
    @{  
        var name = "Cagil Alsac";  
    }  
    and the value of the name variable can be printed in HTML like  
    <h1>@name</h1>  
  
    Other C# code blocks such as if, foreach, etc. can be written like  
    @if (true)  
    {  
        <label>Success.</label>  
    }  
    else  
    {  
        <label>Error!</label>  
    }  
  
    @foreach (string item in list)  
    {  
        <p>@item</p>  
    }
    ```
  
  - The folder names in the Views folder correspond to the controller names and the view file names correspond to the action names 
    if view name is not changed in the returned View method. If view name is changed in the returned View method, the view file name 
    must also be changed accordingly.

    The returned view of an action is first searched under Views/Home folder, then Views/Shared folder, and the view is returned if found. 
    Therefore, views in the Shared folder can be returned from any controller's any action.

  - ViewData and ViewBag are the same collection (dictionary). They carry extra data other than the model object from a controller action 
    to its view, or between views.

    TempData carries data from an action to the redirected action, therefore to the redirected action's view.

  - Layout view in Views/Shared folder is like a master page. It contains the common HTML elements such as head, body, Bootstrap navbar 
    (top menu), footer, etc. The RenderBody C# method in the body tag prints the content of the returned views. The links for directing 
    to the created contollers' actions can be added in the Bootstrap navbar.

  - The layout of a view may be changed at the top code block with one of the below assignments:  
    ```
    @{  
        Layout = "_Layout"; // _Layout.cshtml in Views/Shared folder, no need to write because defined in _ViewStart.cshtml under Views folder  
        Layout = "~/Views/Shared/_Layout.cshtml"; // can also be written  
        Layout = null; // for no layout  
        Layout = "_CustomLayout"; // if _CustomLayout.cshtml in Views/Shared folder is created to be used with the view  
    }
    ```

  - Tag Helpers enable server-side code to participate in creating and rendering HTML elements in Razor views. They look like standard 
    HTML tags but add special attributes such as asp-controller, asp-action, asp-area, asp-route, asp-for, etc. that are processed 
    on the server to generate dynamic content.

  - The default MVC (Model-View-Controller) route to execute controllers' actions is defined in Program.cs file of the MVC Project as
    "{controller=Home}/{action=Index}/{id?}" where id value is optional, default action value is Index and default controller value is Home.
  
## 4. CORE Project

8. Right-click the solution in Solution Explorer, then Add -> New Project to create a project named CORE as a Class Library with .NET 8.0.

9. Set Nullable to Disable for all class library projects (via project properties or XML).

10. Create the folders and classes under the CORE project as below:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/CORE/APP/Domain/Entity.cs  
    https://github.com/cagilalsac/ProductsMVC/tree/master/CORE/APP/Models/Request.cs  
    https://github.com/cagilalsac/ProductsMVC/tree/master/CORE/APP/Models/Response.cs  
    https://github.com/cagilalsac/ProductsMVC/tree/master/CORE/APP/Models/CommandResponse.cs  
    https://github.com/cagilalsac/ProductsMVC/tree/master/CORE/APP/Services/ServiceBase.cs

## 5. Category Entity - APP Project

11. Create a new project under your solution as Class Library (.NET 8) and name it APP.

12. Set Nullable to Disable for APP (via project properties or XML).

13. Right-click APP Project then click Add -> Project Reference then select the CORE Project to use the classes of the CORE Project 
    in the APP Project.

14. Create the Category entity class under the Domain folder:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/APP/Domain/Category.cs

15. Right-click APP Project then click Manage NuGet Packages then in Browse tab search for System.Data.SQLite.Core latest version 
    and install then search for Microsoft.EntityFrameworkCore.Sqlite latest version starting with 8 and install.

16. Create the Db DbContext class under the Domain folder:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/APP/Domain/Db.cs

## 5. Category Entity - MVC Project

17. If MVC project name doesn't appear bold in Solution Explorer, right-click MVC Project then click Set as Startup Project.

18. Right-click MVC Project then click Manage NuGet Packages then in Browse tab search for Microsoft.EntityFrameworkCore.Tools 
    latest version starting with 8 and install.

19. Right-click MVC Project then click Add -> Project Reference then select the APP Project to use both the classes of the CORE Project 
    and APP Project in the MVC Project.

20. Open appsettings.json and add the ConnectionStrings section:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/appsettings.json

21. Open Program.cs and add builder.Services.AddDbContext...  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Program.cs

22. Create your ProductsDB database using migrations:  
    - Open Package Manager Console from Visual Studio menu -> Tools -> NuGet Package Manager -> Package Manager Console  
    - Set APP as Default Project in Package Manager Console  
    - Run:  
      add-migration v1  
      update-database  
    - For Rider, use the UI as described in JetBrains documentation.  
    - You can see the created ProductsDB database file in MVC Project.  
    - Optionally in Visual Studio, you can install the SQLite and SQL Server Compact Toolbox extension 
      from Visual Studio menu -> Extensions -> Manage Extensions to connect to the created ProductsDB SQLite database.

## 5. Category Entity - APP Project

23. Create a Models folder and inside create the CategoryResponse class which will be the response model for carrying queried 
    and projected entity data through the CategoryService:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/APP/Models/CategoryResponse.cs

24. Create the CategoryRequest class which will be the request model for getting data in the views, then sending the data 
    to the related CategoryService methods for create and update operations by the controller's related actions:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/APP/Models/CategoryRequest.cs
 
    - Attributes gain new features to the fields, properties, methods or classes. When they are used in entities or requests, 
      they are also called data annotations which provide data validations.
    
      Some commonly used data annotation attributes in C#:  
      [Required]           // Ensures the property must have a value.  
    
      [StringLength]       // Sets maximum (and optionally minimum) length for strings.  
    
      [Length]             // Sets maximum and minimum length for strings.  
    
      [MinLength]          // Specifies the minimum length for strings or collections.  
    
      [MaxLength]          // Specifies the maximum length for strings or collections.  
    
      [Range]              // Defines the allowed range for numeric values.  
    
      [RegularExpression]  // Validates the property value against a regex pattern.  
    
      [EmailAddress]       // Validates that the property is a valid email address.  
    
      [Phone]              // Validates that the property is a valid phone number.  
    
      [Url]                // Validates that the property is a valid URL.  
    
      [Compare]            // Compares two properties for equality (e.g., password confirmation).  
    
      [DisplayName]        // Sets a friendly name for the property (used in error messages/UI).  
    
      [DataType]           // Specifies the data type (e.g., DateTime) for formatting/UI hints.  
    
      ErrorMessage parameter can be set in all data annotations to show custom validation error messages:  
      Example 1: [Required(ErrorMessage = "{0} is required!")]  
      where {0} is the DisplayName (used in MVC) if defined otherwise property name.  
    
      Example 2: [StringLength(100, 3, ErrorMessage = "{0} must be minimum {2} maximum {1} characters!")]  
      where {0} is the DisplayName (used in MVC) if defined otherwise property name, {1} is the first parameter which is 100 and 
      {2} is the second parameter which is 3.

25. Create a Services folder and inside create the CategoryObsoleteService class which will manage the business logic for database CRUD 
    (Create, Read, Update, Delete) operations:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/APP/Services/CategoryObsoleteService.cs
  
    - This service will be replaced with the CategoryService class implementing the generic service interface in the future. 
      This is why the name is given "Obsolete".

    - Service classes are business logic classes that first get entity data from the database through the database class (Db), 
      which inherits Entity Framework Core's DbContext class for data access, convert the data to the response model object 
      and return the response model object to the controller action for presentation (Query method).  
      Secondly, service classes get request model data from the controller action, convert the data to the entity object for create 
      and update operations, or use unique identifier (ID) for delete operation, in the database. Then they return a response model object 
      to the controller action to present the result of the operation (Create, Update and Delete methods).

    - Request and response model classes are also called Data Transfer Object (DTO) classes.

    - Synchronous methods execute tasks one after another. Each operation must complete before the next one starts. The calling thread 
      waits (or "blocks") until the method finishes.  
      Asynchronous methods allow tasks to run in the background. The calling thread does not wait for the operation to finish and 
      can continue executing other code. In C#, asynchronous methods often use the async and await keywords, enabling non-blocking operations 
      (such as I/O or database calls) and improving application responsiveness.
    
    - Some LINQ (Language Integrated Query) methods for querying data (async versions already exists):  
      Find: Finds an entity with the given primary key value. Returns null if not found.  
      Uses the database context's cache before querying the database.  
      Example: var group = _db.Groups.Find(5);  
    
      Single: Returns the only element that matches the specified condition(s).  
      Throws an exception if no element or more than one element is found.  
      Example: var group = _db.Groups.Single(groupEntity => groupEntity.Id == 5);  
    
      SingleOrDefault: Returns the only element that matches the specified condition(s), or null if no such element exists.  
      Throws an exception if more than one element is found.  
      Example: var group = _db.Groups.SingleOrDefault(groupEntity => groupEntity.Id == 5);  
    
      First: Returns the first element that matches the specified condition(s).  
      Throws an exception if no element is found.  
      Example: var group = _db.Groups.First();  
      Example: var group = _db.Groups.First(groupEntity => groupEntity.Id > 5 && groupEntity.Title.StartsWith("Jun");  
    
      FirstOrDefault: Returns the first element that matches the specified condition(s), or null if no such element exists.  
      Example: var group = _db.Groups.FirstOrDefault();  
      Example: var group = _db.Groups.FirstOrDefault(groupEntity => groupEntity.Id < 5 || groupEntity.Title == "Senior");  
    
      Last: Returns the last element that matches the specified condition(s).  
      Throws an exception if no element is found. Usually requires an OrderBy or OrderByDescending clause.  
      Example: var group = _db.Groups.OrderByDescending(groupEntity => groupEntity.Id).Last();  
               gets the first group from the groups descending ordered by Id.  
      Example: var group = _db.Groups.OrderBy(groupEntity => groupEntity.Id).Last();  
               gets the last group from the groups ordered by Id.  
   
      LastOrDefault: Returns the last element that matches the specified condition(s), or null if no such element exists.  
      Usually requires an OrderBy or OrderByDescending clause.  
      Example: var group = _db.Groups.OrderBy(groupEntity => groupEntity.Id).LastOrDefault();  
      Example: var group = _db.Groups.OrderBy(groupEntity => groupEntity.Id).LastOrDefault(groupEntity.Title.Contains("io"));  
   
      Where: Returns the filtered query that matches the specified condition(s). Tolist, SingleOrDefault or FirstOrDefault 
      methods are invoked to get the filtered data.  
      Example: var groups = _db.Groups.Where(groupEntity => groupEntity.Id > 5).ToList();  
    
      Note: SingleOrDefault is generally preferred to get single data.  
      Note: These LINQ methods can also be used with collections such as lists and arrays.

## 5. Category Entity - MVC Project
    
26. Add builder.Services.AddScoped... for type CategoryService in the IoC Container of the Program.cs:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Program.cs

    - Concrete type object injection is not suitable for SOLID Principles (D: Dependency Inversion) and Unit Testing.

      SOLID Principles:  
      1. Single Responsibility Principle (SRP)  
         A class should have only one reason to change, meaning it should have only one job or responsibility.  
      2. Open/Closed Principle (OCP)  
         Software entities (classes, modules, functions) should be open for extension but closed for modification.  
         You should be able to add new functionality without changing existing code.  
      3. Liskov Substitution Principle (LSP)  
         Subtypes must be substitutable for their base types. Derived classes should extend base classes without changing their behavior.  
      4. Interface Segregation Principle (ISP)  
         No client should be forced to depend on methods it does not use. Prefer small, specific interfaces over large, general-purpose ones.  
      5. Dependency Inversion Principle (DIP)  
         High-level modules should not depend on low-level modules; both should depend on abstractions (e.g., interfaces).  
         This is commonly implemented in ASP.NET Core using dependency injection, as seen in Program.cs.

    - Service Lifetimes in ASP.NET Core Dependency Injection:  
      I) AddScoped:  
      Lifetime: Scoped to a single HTTP request (or scope).  
      Behavior: Creates one instance of the service per HTTP request.  
      Use case: Use when you want to maintain state or dependencies that last only during a single request.  
      Example: DbContext, which should be shared across operations within a request, generally added with AddDbContext method.
      
      II) AddSingleton:  
      Lifetime: Singleton for the entire application lifetime.  
      Behavior: Creates only one instance of the service for the whole app lifecycle.  
      Use case: Use for stateless services or global shared data/services.  
      Example: Caching services, configuration providers, logging services.
      
      III) AddTransient:  
      Lifetime: Transient (short-lived).  
      Behavior: Creates a new instance every time the service is requested.  
      Use case: Use for lightweight, stateless services that are cheap to create.  
      Example: Utility/helper classes without state.
      
      Notes:  
      Injecting a Scoped service into a Singleton can cause issues due to lifetime mismatch. ASP.NET Core DI container will warn about such mismatches.

27. Right-click MVC Controllers folder then Add -> Controller -> Common -> MVC -> MVC Controller - Empty to create the CategoriesObsoleteController, 
    implement the CategoryObsoleteService injection with Index, Details, Create, Edit and Delete actions:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Controllers/CategoriesObsoleteController.cs

    - This controller will be replaced with the CategoriesController which will inject the generic service interface in the future. 
      This is why the name is given "Obsolete".

28. Right-click on each controller action to add their Razor empty views with names Index, Details, Create and Edit. No need to add the Delete view 
    since the delete operation is performed in the get action. Don't forget the implement ViewData or TempData dictionaries set in actions 
    to show messages in the views.  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Views/CategoriesObsolete/Index.cshtml  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Views/CategoriesObsolete/Details.cshtml  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Views/CategoriesObsolete/Create.cshtml  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Views/CategoriesObsolete/Edit.cshtml

    - Some commonly used HTML Helpers in ASP.NET Core MVC:  
      We will use mostly Tag Helpers other than the HTML Helpers DisplayNameFor and DisplayFor.  
      These helpers use lambda expressions to bind directly to model properties, providing compile-time safety and IntelliSense.
    
      @Html.LabelFor(model => model.Property)  
      Generates a <label> for the specified property.
    
      @Html.DisplayNameFor(model => model.Property)  
      Renders the display name of the property, using the [DisplayName] attribute if set, otherwise the property name.
    
      @Html.TextBoxFor(model => model.Property)  
      Generates a <input type="text"> for the property.
    
      @Html.TextAreaFor(model => model.Property)  
      Generates a <textarea> for the property.
    
      @Html.EditorFor(model => model.Property)  
      Generates the most appropriate input element based on the property type and data annotations.
    
      @Html.DisplayFor(model => model.Property)  
      Renders a display-only view of the property (e.g., as plain text).
    
      @Html.CheckBoxFor(model => model.Property)  
      Generates a checkbox for boolean properties.
    
      @Html.DropDownListFor(model => model.Property, selectList)  
      Generates a dropdown list for the property.
    
      @Html.ListBoxFor(model => model.Property, multiSelectList)  
      Generates a multi-select list box for the property.
    
      @Html.HiddenFor(model => model.Property)  
      Generates a hidden input field for the property.
    
      @Html.PasswordFor(model => model.Property)  
      Generates a password input field for the property.
    
      @Html.ValidationMessageFor(model => model.Property)  
      Displays validation error messages for the property.
    
      @Html.Raw(string)  
      Renders raw HTML markup from a string (only use with trusted content to avoid XSS vulnerabilities).

## 6. CORE Project - Generic Service

29. Right-click CORE Project then click Manage NuGet Packages then in Browse tab search for Microsoft.EntityFrameworkCore 
    latest version starting with 8 and install.

30. Implement a base abstract generic service class for entity CRUD (Create, Read, Update, Delete) operations in CORE/APP/Services:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/CORE/APP/Services/Service.cs

31. Implement a base generic service interface for request and response method definitions in CORE/APP/Services/MVC:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/CORE/APP/Services/MVC/IService.cs

## 7. Store and Product Entities - APP Project

Note: The entities and DbContext class should be implemented first. Second, request, response and service classes should be implemented. 
      Finally, controllers and views should be implemented.

32. Create the Store entity class under the Domain folder:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/APP/Domain/Store.cs

33. Create the Product entity class under the Domain folder:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/APP/Domain/Product.cs

34. Create the ProductStore entity class under the Domain folder (for products-stores many to many relationship):  
    https://github.com/cagilalsac/ProductsMVC/tree/master/APP/Domain/ProductStore.cs

35. Add the Products property in the Category entity class (for category-products one to many relationship):  
    https://github.com/cagilalsac/ProductsMVC/tree/master/APP/Domain/Category.cs

36. Create the Stores, Products and ProductStores DbSets in Db:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/APP/Domain/Db.cs

37. Update your ProductsDB database for Stores, Products and ProductStores tables using migrations:  
    - Open Package Manager Console from Visual Studio menu -> Tools -> NuGet Package Manager -> Package Manager Console  
    - Right-click MVC Project and click Set as Startup Project  
    - Set APP as Default Project in Package Manager Console  
    - Run:  
      add-migration v2  
      update-database  
    - For Rider, use the UI as described in JetBrains documentation.

38. Under Models folder of the APP Project, create the ProductRequest class:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/APP/Models/ProductRequest.cs

39. Under Models folder of the APP Project, create the ProductResponse class:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/APP/Models/ProductResponse.cs

40. Under Services folder of the APP Project, create the ProductService class inheriting from the 
    base abstract generic entity Service class and implementing base generic request and response IService interface:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/APP/Services/ProductService.cs

    - Relational entity data can be included to the query by using the Include method (Entity Framework Core Eager Loading). 
      If the included relational entity data has a relation with other entity data, ThenInclude method is used. 
      If you want to automatically include all relational data without using Include / ThenInclude methods (Entity Framework Core Lazy Loading), 
      you need to make the necessary configuration in the class inheriting from DbContext class (Db) to enable Lazy Loading (not recommended).

41. Under Models folder of the APP Project, create the StoreRequest class:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/APP/Models/StoreRequest.cs

42. Under Models folder of the APP Project, create the StoreResponse class:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/APP/Models/StoreResponse.cs

43. Under Services folder of the APP Project, create the StoreService class inheriting from the 
    base abstract generic entity Service class and implementing base generic request and response IService interface:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/APP/Services/StoreService.cs

44. Under Services folder of the APP Project, create the CategoryService class inheriting from the 
    base abstract generic entity Service class and implementing base generic request and response IService interface:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/APP/Services/CategoryService.cs

    Note: Since Category entity has relational Product entities (category-products one to many relationship), 
          we should check if the category to be deleted has any relational products in the Delete method. 
          If any, we shouldn't delete the category.

## 7. Store and Product Entities - MVC Project

45. Add builder.Services.AddScoped... for types IService<CategoryRequest, CategoryResponse>, CategoryService,  
    Add builder.Services.AddScoped... for types IService<StoreRequest, StoreResponse>, StoreService,  
    Add builder.Services.AddScoped... for types IService<ProductRequest, ProductResponse>, ProductService  
    in the IoC Container of the Program.cs:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Program.cs

46. Download the Scaffolding Templates that will generate the code for the controllers and views automatically from:  
    https://need4code.com/DotNet/Home/Index?path=.NET%5C00_Files%5CScaffolding%20Templates%5CTemplates.7z  
    Extract the Templates folder in the compressed file to your MVC Project. 
    Right-click the Templates folder then click Exclude From Project for the template codes not being compiled and published. 
    If you want to see the excluded folders or files of your project, you can click the fifth icon from left in 
    top of the Solution Explorer with description Show All Files. 
    The excluded files or folders are seen as dashed points in Solution Explorer. 
    If you want to include a folder or file in your project, right-click the file or folder with dashed points 
    then click Include In Project, therefore the codes will be compiled and published. 
    You don't have to use these templates, however if you choose not to, you need to write or modify the dependency injections 
    and actions with views.

    Note: If you get any exceptions during scaffolding, create the DbFactory class in APP/Domain folder:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/APP/Domain/DbFactory.cs

47. Right-click the Controllers folder then Add -> Controller -> Common -> MVC -> MVC Controller with views, using Entity Framework 
    and select Store entity as Model class, select Db as DbContext class, check Generate views, do not check Reference script libraries, 
    check Use a layout page (leave the text box below empty), and give the name StoresController as Controller name. 
    Then modify the controller or views if necessary:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Controllers/StoresController.cs
    
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Views/Stores/Index.cshtml  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Views/Stores/Details.cshtml  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Views/Stores/Create.cshtml  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Views/Stores/Edit.cshtml  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Views/Stores/Delete.cshtml

    Note: Client-side validation through jQuery instead of Server-side validation is enabled at the bottom in Create and Edit views.

    Note: Html.AntiForgeryToken() in Create, Edit and Delete views generates a hidden form field containing a unique anti-forgery token 
    to prevent Cross-Site Request Forgery (CSRF) attacks by ensuring that form submissions originate from the web application's views.  
    [ValidateAntiForgeryToken] attribute defined for Create, Edit and Delete post actions in the controller rejects requests with 
    missing or invalid anti-forgery tokens, thereby enhancing the security of the web application.

48. Add Stores link in the nav bar (top menu) of the layout view:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Views/Shared/_Layout.cshtml

49. Right-click the Controllers folder then Add -> Controller -> Common -> MVC -> MVC Controller with views, using Entity Framework 
    and select Product entity as Model class, select Db as DbContext class, check Generate views, do not check Reference script libraries, 
    check Use a layout page (leave the text box below empty), and give the name ProductsController as Controller name. 
    Then modify the controller or views if necessary:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Controllers/ProductsController.cs
    
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Views/Products/Index.cshtml  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Views/Products/Details.cshtml  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Views/Products/Create.cshtml  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Views/Products/Edit.cshtml  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Views/Products/Delete.cshtml

    Note: In order to use the calendar in Create and Edit views, right-click the lib folder in the wwwroot folder, then click
    Add -> Client-Side Library, select "cdnjs" as the Provider, type "jquery-datetimepicker" in the Library text box, 
    select the latest version, check Include all library files, check "wwwroot/lib/" as Target location and click Install.

    Note: Optionally you can also install the "select2" library to provide better usage for drop down lists and list boxes (select tags):  
    https://select2.org

    You need to add the following code which will convert all select tags into select2 in the Scripts section:  
    ```
    <link href="~/lib/select2/css/select2.min.css" rel="stylesheet" />
    <script src="~/lib/select2/js/select2.min.js"></script>
    <script>
        $(function() {
            $("select").select2();
        });
    </script>
    ```

50. Add Products link in the nav bar (top menu) of the layout view:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Views/Shared/_Layout.cshtml

51. Right-click the Controllers folder then Add -> Controller -> Common -> MVC -> MVC Controller with views, using Entity Framework 
    and select Category entity as Model class, select Db as DbContext class, check Generate views, do not check Reference script libraries, 
    check Use a layout page (leave the text box below empty), and give the name CategoriesController as Controller name. 
    Then modify the controller or views if necessary:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Controllers/CategoriesController.cs
    
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Views/Categories/Index.cshtml  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Views/Categories/Details.cshtml  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Views/Categories/Create.cshtml  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Views/Categories/Edit.cshtml  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Views/Categories/Delete.cshtml

52. Add Categories link in the nav bar (top menu) of the layout view:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Views/Shared/_Layout.cshtml

53. Add Title and App sections in appsettings.json:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/appsettings.json

54. IConfiguration injected instance can be used in controllers or views to read configuration values from appsettings.json:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Views/Shared/_Layout.cshtml  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Views/Home/Index.cshtml

## 8. User, Role, Group, Country and City Entities - APP Project: Source code shared in APP and MVC Projects.

Note: Homework includes:  

APP Domain: User, UserRole, Role, Group, Country and City entities with Genders enum  
            Users, UserRoles, Roles, Groups, Countries and Cities DbSets in the Db class  

APP Models: UserRequest, UserResponse, RoleRequest, RoleResponse, GroupRequest and GroupResponse classes  
            CountryRequest, CountryResponse, CityRequest and CityResponse classes are optional, you don't need to do as homework  

APP Services: UserService, RoleService and GroupService classes  
              CountryService and CityService classes are optional, you don't need to do as homework  

MVC Controllers: UsersController, RolesController and GroupsController classes with views  
                 CountriesController and CitiesController classes with views are optional, you don't need to do as homework

Note: If you prefer not to implement City and Country requests, responses and services, you need to remove 
the related code in UsersController and its views.

Note: Optionally a DatabaseController with a Seed action can be created to seed the database with initial data:  
https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Controllers/DatabaseController.cs

IWebHostEnvironment injected instance can be used to get the web application's running environment information such as 
development environment or not by using IsDevelopment method.

The get route changed from /Database/Seed to /SeedDb for easy execution by using the Route attribute.
            
Note: Check and add missing injection configurations in the Program.cs for:  
builder.Services.AddScoped<IService<GroupRequest, GroupResponse>, GroupService>();  
builder.Services.AddScoped<IService<RoleRequest, RoleResponse>, RoleService>();  
builder.Services.AddScoped<IService<UserRequest, UserResponse>, UserService>();  
https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Program.cs

Note: If you prefer to implement Country and City requests, responses and services, you also need to manage injections in Program.cs.

Note: Check and add missing links in the layout view:  
https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Views/Shared/_Layout.cshtml

## 9. Cookie Authentication and Authorization - CORE Project

55. Create the base authentication service class named AuthServiceBase in CORE Project's APP/Services/Authentication folder.  
    https://github.com/cagilalsac/ProductsMVC/tree/master/CORE/APP/Services/Authentication/AuthServiceBase.cs

56. Create the cookie authentication service interface named ICookieAuthService in CORE Project's APP/Services/Authentication/MVC folder.  
    https://github.com/cagilalsac/ProductsMVC/tree/master/CORE/APP/Services/Authentication/MVC/ICookieAuthService.cs

57. Create the cookie authentication service concrete class named CookieAuthService in CORE Project's APP/Services/Authentication/MVC folder.  
    https://github.com/cagilalsac/ProductsMVC/tree/master/CORE/APP/Services/Authentication/MVC/CookieAuthService.cs

## 9. Cookie Authentication and Authorization - APP Project

58. Under Models, create UserLoginRequest model:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/APP/Models/UserLoginRequest.cs

59. Under Models, create UserRegisterRequest model:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/APP/Models/UserRegisterRequest.cs

60. Add ICookieAuthService injection in the constructor with Login, Logout and Register methods at the bottom in the UserService:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/APP/Services/UserService.cs

## 9. Cookie Authentication and Authorization - MVC Project

61. Add  
    builder.Services.AddHttpContextAccessor...  
    builder.Services.AddScoped... for types ICookieAuthService and CookieAuthService  
    builder.Services.AddAuthentication...  
    app.UseAuthentication...  
    in the Program.cs:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Program.cs

    Note: In a controller, the HTTP Context can be directly accessed by the HttpContext property inherited from Controller base class.
 
62. Add Login get and post actions with Login view, Logout get action, and Register get and post actions with Register view 
    at the bottom of the UsersController:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Controllers/UsersController.cs  
    
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Views/Users/Login.cshtml  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Views/Users/Register.cshtml

63. Add Login, Register and Logout links in the nav bar (top menu) of the layout view:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Views/Shared/_Layout.cshtml
    
    - Authentication cookie existance can be checked by User.Identity.IsAuthenticated in controller actions and views 
      returning the result of whether the user is signed in or not.

    - The auhenticated user's user name can be reached by User.Identity.Name.

    - The authenticated user's role claim value can be checked by User.IsInRole method which gets the role name string as parameter, 
      or the role claim can be accessed by User.Claims.SingleOrDefault method which gets the predicate checking claim type 
      equals role claim type as parameter.  
    
    - Examples:  
    
      if (User.IsInRole("Admin")) { ... }  
    
      Claim roleClaim = User.Claims.SingleOrDefault(claim => claim.Type == ClaimTypes.Role);  
      if (roleClaim is not null && roleClaim.Value == "User") { ... }  

      Custom claim types' values such as Id can be accessed by:  
      int id = Convert.ToInt32(User.Claims?.Single(claim => claim.Type == "Id").Value);

64. Add the [Authorize(Roles = "Admin")] attribute for Admin role on top of the RolesController class so that all of the actions can be executed 
    by only authenticated users with role Admin:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Controllers/RolesController.cs
 
    - The [Authorize] attribute in ASP.NET Core is used to control access to a controller's actions by requiring that the user is authenticated 
      and, optionally, meets certain authorization requirements.  
    
      When you decorate a controller or action with [Authorize], only authenticated users who provides the Authentication Cookie or 
      Json Web Token (used in Web APIs) can access it. Unauthenticated requests will receive a 401 Unauthorized response. In MVC, 
      they will be redirected to the LoginPath defined in the authentication configuration in Program.cs.
    
      You can specify roles or policies to further restrict access. For example, [Authorize(Roles = "Admin")] allows only authenticated users 
      in the "Admin" role.  
    
      You can apply [Authorize] at the controller level (to protect all actions) or at the action level.  
    
      Example usages:  
      [Authorize]: Any authenticated user can access.  
    
      [Authorize(Roles = "User")]: Only authenticated users with role "User" can access.  

      [Authorize(Roles = "Admin,User")]: Only authenticated users with the "Admin" or "User" role can access.  
    
      [AllowAnonymous]: Can be used to override [Authorize] at the action level and allows public access, therefore gives permission to everyone 
      for executing specific actions.

65. Add the [Authorize] attribute on top of the GroupsController class. For example, if the Index action is wanted to be executed by 
    authorized and unauthorized users (eveyone), [AllowAnonymous] attribute can be defined. The Details action can be executed by only 
    authenticated users since Authorize is defined at controller level. Add [Authorize(Roles = "Admin")] attribute on top of the 
    Create, Edit and Delete get and post actions so that only authenticated users with role Admin can execute these actions:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Controllers/GroupsController.cs

66. Add the [Authorize(Roles = "Admin,User")] attribute, or [Authorize] attribute since we have only 2 roles, on top of the Index action 
    of the UsersController class so that authenticated users with all roles can execute the Index action. Then add 
    [Authorize(Roles = "Admin")] attribute on top of the Details, Create, Edit and Delete get and post actions so that 
    authenticated users with role Admin can execute these actions. The Login, Logout and Register actions can be executed by everyone 
    since no Authorize attribute is defined. However, if Authorize attribute was defined at the controller level, we should have used 
    AllowAnonymous for Login, Logout and Register actions:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Controllers/UsersController.cs

67. We shouldn't show the links directing to unauthorized controller actions in the views. Therefore, we need to add if the user is in
    Admin role in the layout view for the Roles link. We also need to check if the user is in Admin or User role 
    (checking if user is authenticated is better since we have only 2 roles) for the Groups and Users links. We can show the user name and 
    Logout link if the user is authenticated, Register and Login links if the user is not authenticated:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Views/Shared/_Layout.cshtml

68. We also should check if the user is authenticated in the Index view of the Groups since only authenticated users can execute the 
    Details action. We should also show the Create, Edit and Delete links to the users with role Admin. In Details view, 
    we should show the Edit and Delete links for the Admin role, and in Edit view we should show the Delete link for the Admin role:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Views/Groups/Index.cshtml  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Views/Groups/Details.cshtml  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Views/Groups/Edit.cshtml

69. We should check if the user is in role Admin for the Create, Details, Edit and Delete links in the Index view of the Users. 
    We should also show the Edit and Delete links in the Details view, and Delete link in the Edit view for the Admin role:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Views/Users/Index.cshtml  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Views/Users/Details.cshtml  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Views/Users/Edit.cshtml

70. The Authorize attribute is also applied in the Products, Stores and Categories controllers, and authentication is checked in the 
    layout view with the Index views of Products, Stores and Categories for showing the links. This is enough for the homeworks and projects. 
    Modifying the links in Details and Edit views is optional:  
  
    - Only users in Admin role can perform category operations.  
    - Only authenticated users can see the product list and details, users in Admin role can perform create, edit and delete operations.  
    - Everyone can see the store list, only users in Admin role can see store details and can perform create, edit and delete operations.

    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Controllers/ProductsController.cs  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Controllers/StoresController.cs  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Controllers/CategoriesController.cs  

    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Views/Shared/_Layout.cshtml  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Views/Products/Index.cshtml  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Views/Stores/Index.cshtml

71. Optionally you can use the Sneat Web Template for your web application to look better:  
    https://need4code.com/DotNet/Home/Index?path=.NET%5C00_Files%5CWeb%20Templates%5CSneat.7z  
    Extract the folders in the compressed file to your MVC Project.  
    Then modify the _SneatLayout.cshtml according to your project.  
    Finally, change the Layout assignment in the _ViewStart.cshtml to _SneatLayout for your views to use.

## 10. Session - CORE Project

72. Create SessionServiceBase and SessionService classes in CORE Project's APP/Services/Session/MVC folder:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/CORE/APP/Services/Session/MVC/SessionServiceBase.cs  
    https://github.com/cagilalsac/ProductsMVC/tree/master/CORE/APP/Services/Session/MVC/SessionService.cs

## 10. Session - APP Project

73. Create CartItem model in APP/Models/Carts folder:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/APP/Models/Carts/CartItem.cs

74. Create CartItemGroupedBy model in APP/Models/Carts folder:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/APP/Models/Carts/CartItemGroupedBy.cs

75. Create ICartService interface in APP/Services/Carts folder:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/APP/Services/Carts/ICartService.cs

76. Create CartService class in APP/Services/Carts folder:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/APP/Services/Carts/CartService.cs

## 10. Session - MVC Project

77. Add  
    builder.Services.AddSession...  
    builder.Services.AddScoped... for types SessionServiceBase and SessionService  
    builder.Services.AddScoped... for types ICartService and CartService  
    app.UseSession...  
    in Program.cs:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Program.cs

78. Create CartsController in the Controllers/Carts folder:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Controllers/Carts/CartsController.cs

79. Create Index view in the Views/Carts folder:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Views/Carts/Index.cshtml

80. Add Cart link in the nav bar of the layout view:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Views/Shared/_Layout.cshtml  
    or  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Views/Shared/_SneatLayout.cshtml

81. In Views/Products/Index.cshtml view, create the link for adding the product to the cart:  
    https://github.com/cagilalsac/ProductsMVC/tree/master/MVC/Views/Products/Index.cshtml
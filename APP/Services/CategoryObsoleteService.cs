using APP.Domain;
using APP.Models;
using CORE.APP.Models;
using CORE.APP.Services;

namespace APP.Services
{
    /// <summary>
    /// Provides category-related CRUD operations business logic for the application.
    /// Inherits from <see cref="ServiceBase"/> to utilize culture with success and error response helper methods.
    /// Temporary DbContext operations demonstration class, will be replaced with CategoryService in the future.
    /// </summary>
    [Obsolete("Use CategoryService class instead!")]
    public class CategoryObsoleteService : ServiceBase
    {
        private readonly Db _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryObsoleteService"/> class with the specified database context.
        /// </summary>
        /// <param name="db">The database context used for accessing category data.</param>
        public CategoryObsoleteService(Db db)
        {
            _db = db;
        }

        /// <summary>
        /// Returns a queryable collection (query) of <see cref="CategoryResponse"/> model (DTO) objects representing categories.
        /// </summary>
        /// <returns>
        /// An <see cref="IQueryable{T}"/> of <see cref="CategoryResponse"/> for further filtering or enumeration.
        /// </returns>
        public IQueryable<CategoryResponse> Query()
        {
            // Project each Category entity from the Categories table in the database into a CategoryResponse response model (DTO).
            // Here, projection means mapping the values of the entity properties to the corresponding properties of the response model.

            // Way 1: types can be used with variables for declarations
            //IQueryable<CategoryResponse> query = _db.Categories.Select(categoryEntity => new CategoryResponse()
            // Way 2: var can also be used therefore the type of the variable (IQueryable<CategoryResponse>) will be known dynamically
            // if an assignment is provided, if no assignment, types must be used
            var query = _db.Categories.Select(categoryEntity => new CategoryResponse()
            {
                Id = categoryEntity.Id,                     // Map the unique integer identifier.
                Guid = categoryEntity.Guid,                 // Map the unique string identifier.
                Title = categoryEntity.Title,               // Map the category title.
                Description = categoryEntity.Description    // Map the category description.
            });

            // Return the queryable result for filtering (e.g. getting a single item) or enumeration (e.g. getting a list of items).
            return query;
        }

        /// <summary>
        /// Retrieves the category entity with the specified unique identifier from the database
        /// and maps its data to a <see cref="CategoryRequest"/> model (DTO) for editing.
        /// If the category is not found, returns <c>null</c>.
        /// </summary>
        /// <param name="id">The unique identifier of the category to edit.</param>
        /// <returns>
        /// A <see cref="CategoryRequest"/> containing the category's data if found; otherwise, <c>null</c>.
        /// </returns>
        public CategoryRequest Edit(int id)
        {
            // Attempt to find the Category entity by its ID in the database.
            var entity = _db.Categories.Find(id);

            // If the entity is not found, return null to indicate the category does not exist.
            if (entity is null)
                return null;

            // Map the found entity's properties to a new CategoryRequest model.
            var request = new CategoryRequest()
            {
                Id = entity.Id,                  // Set the category's unique identifier.
                Title = entity.Title,            // Set the category's title.
                Description = entity.Description // Set the category's description.
            };

            // Return the populated CategoryRequest model for editing.
            return request;
        }

        /// <summary>
        /// Creates a new category in the database using the provided <see cref="CategoryRequest"/> model (DTO).
        /// Validates that no existing category has the same title (case-sensitive, trimmed).
        /// If a duplicate title exists, returns an error <see cref="CommandResponse"/>.
        /// Otherwise, adds the new category, saves changes, and returns a success <see cref="CommandResponse"/> containing the created category's ID.
        /// </summary>
        /// <param name="request">The category data to create.</param>
        /// <returns>
        /// A <see cref="CommandResponse"/> indicating the result of the operation: success with the new category's ID if created, 
        /// or error if a duplicate title exists.
        /// </returns>
        public CommandResponse Create(CategoryRequest request)
        {
            // Check if any category already exists with the same title (case-sensitive, trimmed) preventing duplicate category titles in the database.
            // Way 1:
            //var existingEntity = _db.Categories.SingleOrDefault(categoryEntity => categoryEntity.Title == request.Title.Trim());
            //if (existingEntity is not null) // if (existingEntity != null) can alo be written
            //    return Error("Category with the same title exists!");
            // Way 2:
            if (_db.Categories.Any(categoryEntity => categoryEntity.Title == request.Title.Trim()))
                return Error("Category with the same title exists!");

            // Creates a new Category entity with the provided title (trimmed for consistency).
            var entity = new Category()
            {
                Guid = Guid.NewGuid().ToString(), // generates a new unique string identifier for the category
                Title = request.Title.Trim(), // since request.Title has required data annotation and can't be null,
                                              // assign request.Title's trimmed value (?. won't have any effect but can be used)
                Description = request.Description?.Trim() // since request.Description is optional and can be null,
                                                          // use null-conditional operator (?.) to avoid Null Reference Exception
            };

            // Adds the new Category entity to the database context.
            _db.Categories.Add(entity); // _db.Add(entity); can also be written

            // Saves changes to the database by using Unit of Work (all changes made to the DbSets will be commited to the database once).
            _db.SaveChanges();

            // Returns a success response indicating the category was created with the created category entity's Id value.
            return Success("Category created successfully.", entity.Id);

            // There are also asynchronous versions of methods such as SingleOrDefaultAsync, AnyAsync and SaveChangesAsync
            // that can be used with await in async methods.

            /* Some LINQ (Language Integrated Query) methods for querying data (async versions already exists):
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
            */
        }

        /// <summary>
        /// Updates an existing category in the database using the provided <see cref="CategoryRequest"/> model (DTO).
        /// Validates that no existing category has the same title other than the current updated category (case-sensitive, trimmed).
        /// If a duplicate title exists, returns an error <see cref="CommandResponse"/>.
        /// Otherwise, updates the category, saves changes, and returns a success <see cref="CommandResponse"/> containing the updated category's ID.
        /// </summary>
        /// <param name="request">The category data to update.</param>
        /// <returns>
        /// A <see cref="CommandResponse"/> indicating the result of the operation: success with the existing category's ID if updated, 
        /// or error if a duplicate title exists.
        /// </returns>
        public CommandResponse Update(CategoryRequest request)
        {
            // If any other category (excluding the current one) already has the same title (case-sensitive, trimmed), don't update.
            if (_db.Categories.Any(categoryEntity => categoryEntity.Id != request.Id && categoryEntity.Title == request.Title.Trim()))
                return Error("Category with the same title exists!");

            // Attempt to find the Category entity by its ID, if not found return error command response with message.
            var entity = _db.Categories.Find(request.Id);
            if (entity is null)
                return Error("Category not found!");

            // Update the Category entity's title and description with the new values (trimmed for consistency).
            entity.Title = request.Title?.Trim(); // since request.Title has required data annotation and can't be null,
                                                  // assign request.Title's trimmed value (?. won't have any effect but can be used)
            entity.Description = request.Description?.Trim(); // since request.Description is optional and can be null,
                                                              // use null-conditional operator (?.) to avoid Null Reference Exception

            // Mark the entity as modified in the context.
            _db.Categories.Update(entity); // _db.Update(entity); can also be written

            // Persist the changes to the database by using Unit of Work (all changes made to the DbSets will be commited to the database once).
            _db.SaveChanges();

            // Return a success response indicating the category was updated, including the entity's ID.
            return Success("Category updated successfully.", entity.Id);
        }

        /// <summary>
        /// Deletes the category with the specified unique identifier from the database.
        /// If the category is not found, returns an error <see cref="CommandResponse"/>.
        /// Otherwise, removes the category, saves changes, and returns a success <see cref="CommandResponse"/>
        /// containing the deleted category's ID.
        /// </summary>
        /// <param name="id">The unique identifier of the category to delete.</param>
        /// <returns>
        /// A <see cref="CommandResponse"/> indicating the result of the operation:
        /// success with the deleted category's ID if found and deleted, or error if the category is not found.
        /// </returns>
        public CommandResponse Delete(int id)
        {
            var entity = _db.Categories.Find(id);

            // If the category does not exist, return an error command response.
            if (entity is null)
                return Error("Category not found!");

            // Remove the Category entity from the database context.
            _db.Categories.Remove(entity); // _db.Remove(entity); can also be written

            // Persist the changes to the database by using Unit of Work (all changes made to the DbSets will be commited to the database once).
            _db.SaveChanges();

            // Return a success response indicating the category was deleted with the deleted catergory entity's ID value.
            return Success("Category deleted successfully.", entity.Id);
        }
    }
}

using APP.Domain;
using APP.Models;
using CORE.APP.Models;
using CORE.APP.Services;
using Microsoft.EntityFrameworkCore;

namespace APP.Services
{
    // Inherit from the generic entity service class therefore DbContext injected constructor can be automatically created
    // and entity CRUD (create, read, update, delete) methods in the base class can be invoked.
    public class ProductService : Service<Product>, IService<ProductRequest, ProductResponse>
    {
        public ProductService(DbContext db) : base(db)
        {
            // if the culture of the application is needed to be changed for this service, below assignment can be made:
            //CultureInfo = new CultureInfo("tr-TR"); default culture is defined as "en-US" in the base service class
        }

        // base virtual Query method is overriden therefore the overriden query can be used in all other methods
        protected override IQueryable<Product> Query(bool isNoTracking = true)
        {
            // p: Product entity delegate, ps: ProductStore entity delegate
            return base.Query(isNoTracking) // will return Products DbSet
                .Include(p => p.Category) // will include the relational Category data
                .Include(p => p.ProductStores).ThenInclude(ps => ps.Store) // will first include the relational ProductStores then Store data
                .OrderByDescending(p => p.ExpirationDate) // query will be ordered descending by ExpirationDate values
                .ThenByDescending(p => p.StockAmount) // after ExpirationDate ordering, query will be ordered descending by StockAmount values
                .ThenBy(p => p.Name); // after StockAmount ordering, query will be ordered ascending by Name values

            // Include, ThenInclude, OrderBy, OrderByDescending, ThenBy and ThenByDescending methods can also be used with DbSets.

            /*
            Relational entity data (Store, ProductStores) can be included to the query by using the Include method (Entity Framework Core Eager Loading).
            If the included relational entity data (ProductStores) has a relation with other entity data (Store), ThenInclude method is used.
            If you want to automatically include all relational data without using Include / ThenInclude methods (Entity Framework Core Lazy Loading), 
            you need to make the necessary configuration in the class inheriting from DbContext class (Db) to enable Lazy Loading (not recommended).
            */
        }

        public List<ProductResponse> List()
        {
            // get the query of all Product entities then project each entity to ProductResponse object and return the list of ProductResponse objects
            return Query().Select(p => new ProductResponse // () after the class name may not be used
            {
                // assigning entity properties to the response
                Id = p.Id,
                Guid = p.Guid,
                Name = p.Name,
                UnitPrice = p.UnitPrice,
                StockAmount = p.StockAmount,
                ExpirationDate = p.ExpirationDate,
                CategoryId = p.CategoryId,
                StoreIds = p.StoreIds,

                // assigning custom or formatted properties to the response
                UnitPriceF = p.UnitPrice.ToString("C2"), // C: currency format, N: number format, 2: 2 decimal places

                // If Product entity's ExpirationDate value is not null, convert and assign the value with month/day/year format, otherwise assign "".
                // No need to give the second CultureInfo parameter (e.g. new CultureInfo("tr-TR")) to the ToString method since
                // CultureInfo property was assigned in the constructor of the base or this class.
                // Instead of ToString method, ToShortDateString (e.g. 08/30/2025) or ToLongDateString (e.g. Saturday, August 30, 2025) methods can be used.
                // For time ToShortTimeString (13:21) or ToLongTimeString (13:21:57) can be used.
                // Again CultureInfo parameter is not needed for these methods.
                ExpirationDateF = p.ExpirationDate.HasValue ? p.ExpirationDate.Value.ToString("MM/dd/yyyy") : string.Empty,

                // Way 1: Ternary Operator
                //StockAmountF = (p.StockAmount.HasValue ? p.StockAmount.Value : 0).ToString(),
                // Way 2:
                StockAmountF = (p.StockAmount ?? 0).ToString(), // If p.StockAmount value is null use 0 otherwise use p.StockAmount value.

                Category = p.Category.Title, // Assign the relational Category's Title value, if Category was optional meaning a product may not have a category,
                                             // p.Category != null ? p.Category.Title : string.Empty must be written

                Stores = p.ProductStores.Select(ps => ps.Store.Name).ToList() // Get store name values from the relational Store of each ProductStore (ps)
                                                                              // and convert them to a list of string.
            }).ToList();
        }

        // get a single Product entity by Id then project the entity to ProductResponse object and return the ProductResponse object
        public ProductResponse Item(int id)
        {
            var entity = Query().SingleOrDefault(p => p.Id == id);
            if (entity is null)
                return null;

            return new ProductResponse
            {
                // assigning entity properties to the response
                Id = entity.Id,
                Guid = entity.Guid,
                Name = entity.Name,
                UnitPrice = entity.UnitPrice,
                StockAmount = entity.StockAmount,
                ExpirationDate = entity.ExpirationDate,
                CategoryId = entity.CategoryId,
                StoreIds = entity.StoreIds,

                // assigning custom or formatted properties to the response
                UnitPriceF = entity.UnitPrice.ToString("C2"),
                ExpirationDateF = entity.ExpirationDate.HasValue ? entity.ExpirationDate.Value.ToShortDateString() : string.Empty,
                StockAmountF = (entity.StockAmount ?? 0).ToString(),
                Category = entity.Category.Title,
                Stores = entity.ProductStores.Select(ps => ps.Store.Name).ToList()
            };
        }

        // get a single Product entity by Id then project the entity to ProductRequest object and return the ProductRequest object
        public ProductRequest Edit(int id)
        {
            var entity = Query().SingleOrDefault(p => p.Id == id);
            if (entity is null)
                return null;

            return new ProductRequest
            {
                // assigning entity properties to the request
                Id = entity.Id,
                Name = entity.Name,
                UnitPrice = entity.UnitPrice,
                StockAmount = entity.StockAmount,
                ExpirationDate = entity.ExpirationDate,
                CategoryId = entity.CategoryId,
                StoreIds = entity.StoreIds
            };
        }

        // create a new Product entity from the ProductRequest object and save it to the database if a product with the same name does not exist
        public CommandResponse Create(ProductRequest request)
        {
            // p: Product entity delegate. Check if a product with the same name exists
            // (case-sensitive, request.Name without white space characters in the beginning and at the end).
            if (Query().Any(p => p.Name == request.Name.Trim()))
                return Error("Product with the same name exists!");

            var entity = new Product
            {
                Name = request.Name.Trim(), // remove white space characters in the beginning and at the end
                UnitPrice = request.UnitPrice,
                StockAmount = request.StockAmount,
                ExpirationDate = request.ExpirationDate,
                CategoryId = request.CategoryId ?? 0,
                StoreIds = request.StoreIds
            };

            Create(entity); // will add the entity to the Products DbSet and since save default parameter's value is true, will save changes to the database

            return Success("Product created successfully.", entity.Id);
        }

        // get the Product entity by Id then update the entity from the ProductRequest object and save changes to the database
        // if a product other than the current updated product with the same name does not exist
        public CommandResponse Update(ProductRequest request)
        {
            // p: Product entity delegate. Check if a product excluding the current updated product with the same name exists
            // (case-sensitive, request.Name without white space characters in the beginning and at the end).
            if (Query().Any(p => p.Id != request.Id && p.Name == request.Name.Trim()))
                return Error("Product with the same name exists!");

            // get the Product entity by ID from the Products table
            var entity = Query(false).SingleOrDefault(p => p.Id == request.Id); // isNoTracking is false for being tracked by EF Core to update the entity
            if (entity is null)
                return Error("Product not found!");

            // delete the relational ProductStore entities data since the stores of the product is updated from request.StoreIds below
            Delete(entity.ProductStores);

            // update retrieved Product entity's properties with request properties
            entity.Name = request.Name.Trim();
            entity.UnitPrice = request.UnitPrice;
            entity.StockAmount = request.StockAmount;
            entity.ExpirationDate = request.ExpirationDate;
            entity.CategoryId = request.CategoryId ?? 0;
            entity.StoreIds = request.StoreIds;
            
            Update(entity); // will update the entity in the Products DbSet and since save default parameter's value is true, will save changes to the database
            
            return Success("Product updated successfully.", entity.Id);
        }

        // get the Product entity by Id then delete the entity from the database
        public CommandResponse Delete(int id)
        {
            // get the Product entity by ID from the Products table
            var entity = Query(false).SingleOrDefault(p => p.Id == id); // isNoTracking is false for being tracked by EF Core to delete the entity
            if (entity is null)
                return Error("Product not found!");

            // delete the relational ProductStore entities
            Delete(entity.ProductStores);

            // delete the Product entity
            Delete(entity); // will delete the entity from the Products DbSet and since save default parameter's value is true, will save changes to the database

            return Success("Product deleted successfully.", entity.Id);
        }



        // get a filtered list of Product response items filtered by the Product query request properties
        public List<ProductResponse> List(ProductQueryRequest request) 
        {
            // get the query of all Product entities
            var query = Query();

            // apply filtering according to the request properties if they have values, p: Product entity delegate
            // if Name != null and Name.Trim() != ""
            if (!string.IsNullOrWhiteSpace(request.Name))
                // apply name filtering to the query for exact match
                // Way 1:
                //query = query.Where(u => p.Name.Equals(request.Name));
                // Way 2:
                //query = query.Where(p => p.Name == request.Name);
                // Way 3: apply name filtering to the query for partial match
                // (case-sensitive, without white space characters in the beginning and at the end)
                query = query.Where(p => p.Name.Contains(request.Name.Trim()));

            // if UnitPriceStart has a value
            if (request.UnitPriceStart.HasValue)
                // apply unit price start filtering to the query for greater than or equal match
                query = query.Where(p => p.UnitPrice >= request.UnitPriceStart.Value);

            // if UnitPriceEnd has a value
            if (request.UnitPriceEnd.HasValue)
                // apply unit price end filtering to the query for less than or equal match
                query = query.Where(p => p.UnitPrice <= request.UnitPriceEnd.Value);

            // if StockAmountStart has a value
            if (request.StockAmountStart.HasValue)
                // apply stock amount start filtering to the query for greater than or equal match
                query = query.Where(p => (p.StockAmount ?? 0) >= request.StockAmountStart.Value);
                // if p.StockAmount is null use 0 otherwise use p.StockAmount value

            // if StockAmountEnd has a value
            if (request.StockAmountEnd.HasValue)
                // apply stock amount end filtering to the query for less than or equal match
                query = query.Where(p => (p.StockAmount ?? 0) <= request.StockAmountEnd.Value);

            // if ExpirationDateStart has a value
            if (request.ExpirationDateStart.HasValue)
                // apply expiration date start filtering to the query for greater than or equal match
                // Way 1: filtering with date and time value (e.g. 08/22/1990 13:45:57)
                //query = query.Where(u => u.ExpirationDateStart.HasValue && u.ExpirationDateStart.Value >= request.ExpirationDateStart.Value);
                // Way 2: filtering with date value only (e.g. 08/22/1990)
                query = query.Where(p => p.ExpirationDate.HasValue && p.ExpirationDate.Value.Date >= request.ExpirationDateStart.Value.Date);
                // p.ExpirationDate.HasValue is checked because null values cannot be compared

            // if ExpirationDateEnd has a value
            if (request.ExpirationDateEnd.HasValue)
                // apply expiration date end filtering to the query for less than or equal match
                query = query.Where(p => p.ExpirationDate.HasValue && p.ExpirationDate.Value.Date <= request.ExpirationDateEnd.Value.Date);

            // if CategoryId has a value
            if (request.CategoryId.HasValue)
                // apply category ID filtering to the query for exact match
                query = query.Where(p => p.CategoryId == request.CategoryId.Value);

            // if StoreIds has a list with at least one element
            if (request.StoreIds.Count > 0) // Any() method can also be used instead of Count > 0
                                            // apply store IDs filtering to the query for any match
                query = query.Where(p => p.ProductStores.Any(ps => request.StoreIds.Contains(ps.StoreId)));
                // check if any of the ProductStores store ID exist in the request's StoreIds list, ps: ProductStore entity delegate

            // project each entity to ProductResponse object and return the list of ProductResponse objects
            return query.Select(p => new ProductResponse
            {
                // assigning entity properties to the response
                Id = p.Id,
                Guid = p.Guid,
                Name = p.Name,
                UnitPrice = p.UnitPrice,
                StockAmount = p.StockAmount,
                ExpirationDate = p.ExpirationDate,
                CategoryId = p.CategoryId,
                StoreIds = p.StoreIds,

                // assigning custom or formatted properties to the response
                UnitPriceF = p.UnitPrice.ToString("C2"),
                StockAmountF = (p.StockAmount ?? 0).ToString(),
                ExpirationDateF = p.ExpirationDate.HasValue ? p.ExpirationDate.Value.ToShortDateString() : string.Empty,
                Category = p.Category.Title,
                Stores = p.ProductStores.OrderBy(ps => ps.Store.Name).Select(ps => ps.Store.Name).ToList() // ps: ProductStore entity delegate
            }).ToList();
        }
    }
}

using APP.Domain;
using APP.Models;
using CORE.APP.Models;
using CORE.APP.Services;
using Microsoft.EntityFrameworkCore;

namespace APP.Services
{
    // Inherit from the generic entity service class therefore DbContext injected constructor can be automatically created
    // and entity CRUD (create, read, update, delete) methods in the base class can be invoked.
    public class StoreService : Service<Store>, IService<StoreRequest, StoreResponse>
    {
        public StoreService(DbContext db) : base(db)
        {
        }

        // base virtual Query method is overriden therefore the overriden query can be used in all other methods
        protected override IQueryable<Store> Query(bool isNoTracking = true)
        {
            return base.Query(isNoTracking) // returns Stores DbSet
                .Include(s => s.ProductStores).ThenInclude(ps => ps.Product) // first includes relational ProductStore entities,
                                                                             // then includes relational Product entities of the
                                                                             // ProductStore entities
                .OrderBy(s => s.IsVirtual) // orders ascending by IsVirtual (true or false)
                .ThenBy(s => s.Name); // then applies ascending ordering by Name to the ordered ascending by IsVirtual query

            // Include, ThenInclude, OrderBy, OrderByDescending, ThenBy and ThenByDescending methods can also be used with DbSets.
        }

        public CommandResponse Create(StoreRequest request)
        {
            // s: Store entity delegate. Check if a virtual or physical store with the same name exists.
            if (Query().Any(s => s.Name.ToUpper() == request.Name.ToUpper().Trim() && s.IsVirtual == request.IsVirtual))
            {
                // Way 1:
                //return Error((request.IsVirtual ? "Virtual" : "Physical") + " store with the same name exists!");
                // Way 2:
                return Error($"{(request.IsVirtual ? "Virtual" : "Physical")} store with the same name exists!");
            }

            // map the StoreRequest to Store entity
            var entity = new Store
            {
                Name = request.Name.Trim(), // request.Name is required and can't be null
                IsVirtual = request.IsVirtual
            };

            Create(entity); // will add the entity to the Stores DbSet and since save default parameter's value is true, will save changes to the database

            return Success("Store created successfully.", entity.Id);
        }

        public CommandResponse Delete(int id)
        {
            // s: Store entity delegate. Get the Store entity by ID from the Stores table
            var entity = Query().SingleOrDefault(s => s.Id == id);
            if (entity is null)
                return Error("Store not found!");

            // delete the relational ProductStore entities data
            Delete(entity.ProductStores);

            // delete the Store entity data
            Delete(entity);

            return Success("Store deleted successfully.", entity.Id);
        }

        public StoreRequest Edit(int id)
        {
            // get the Store entity by ID from the Stores table
            var entity = Query().SingleOrDefault(s => s.Id == id);
            if (entity is null)
                return null;

            // map the Store entity to StoreRequest and return
            return new StoreRequest
            {
                // assigning entity properties to the response
                Id = entity.Id,
                Name = entity.Name,
                IsVirtual = entity.IsVirtual
            };
        }

        public StoreResponse Item(int id)
        {
            // get the Store entity by ID from the Stores table
            var entity = Query().SingleOrDefault(s => s.Id == id);
            if (entity is null)
                return null;

            // map the Store entity to StoreResponse and return
            return new StoreResponse
            {
                // assigning entity properties to the response
                Id = entity.Id,
                Guid = entity.Guid,
                Name = entity.Name,
                IsVirtual = entity.IsVirtual,

                // assigning custom or formatted properties to the response
                IsVirtualF = entity.IsVirtual ? "Virtual" : "Physical",
                ProductCount = entity.ProductStores.Count,
                Products = string.Join("<br>", entity.ProductStores.Select(ps => ps.Product.Name))
            };
        }

        public List<StoreResponse> List()
        {
            // project the Store entities query to StoreResponse query and return the list
            return Query().Select(entity => new StoreResponse
            {
                // assigning entity properties to the response
                Id = entity.Id,
                Guid = entity.Guid,
                Name = entity.Name,
                IsVirtual = entity.IsVirtual,

                // assigning custom or formatted properties to the response
                IsVirtualF = entity.IsVirtual ? "Virtual" : "Physical",
                ProductCount = entity.ProductStores.Count,
                Products = string.Join("<br>", entity.ProductStores.Select(ps => ps.Product.Name))
            }).ToList();
        }

        public CommandResponse Update(StoreRequest request)
        {
            // s: Store entity delegate. Check if a physical or virtual store excluding the current updated store with the same name exists.
            if (Query().Any(s => s.Id != request.Id && s.Name.ToUpper() == request.Name.ToUpper().Trim() && s.IsVirtual == request.IsVirtual))
                return Error($"{(request.IsVirtual ? "Virtual" : "Physical")} store with the same name exists!");

            // get the Store entity by ID from the Stores table
            var entity = Query().SingleOrDefault(s => s.Id == request.Id);
            if (entity is null)
                return Error("Store not found!");

            // update retrieved Store entity's properties with request properties
            entity.Name = request.Name.Trim(); // request.Name is required and can't be null
            entity.IsVirtual = request.IsVirtual;

            Update(entity); // will update the entity in the Stores DbSet and since save default parameter's value is true, will save changes to the database

            return Success("Store updated successfully.", entity.Id);
        }
    }
}

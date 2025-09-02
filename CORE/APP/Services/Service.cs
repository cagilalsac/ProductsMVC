using CORE.APP.Domain;
using Microsoft.EntityFrameworkCore;

namespace CORE.APP.Services
{
    /// <summary>
    /// Provides a generic base repository service for handling CRUD operations on entities implementing Repository Pattern.
    /// This abstract class defines common database actions such as query, create, update, and delete,
    /// and is designed to work with a specific <typeparamref name="TEntity"/> type.
    /// Inherits from <see cref="ServiceBase"/> to utilize culture-specific settings with basic success and error command response operations.
    /// Instead of a base Service class, a concrete Repository class implementing an IRepository interface with the same implementations 
    /// can also be created, and can be injected to the Services or Handlers for database operations. 
    /// If you prefer to do so, remember to inject the Repository class in the IoC Container of the Program.cs as
    /// builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    /// </summary>
    /// <typeparam name="TEntity">
    /// The entity type the service operates on. Must inherit from <see cref="Entity"/> and have a parameterless constructor.
    /// </typeparam>
    public abstract class Service<TEntity> : ServiceBase, IDisposable where TEntity : Entity, new()
    {
        /// <summary>
        /// The database context used for data access operations.
        /// </summary>
        private readonly DbContext _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="Service{TEntity}"/> class.
        /// </summary>
        /// <param name="db">The database context to be used for entity operations.</param>
        protected Service(DbContext db)
        {
            _db = db;
        }



        /*
        Synchronous methods execute tasks one after another. Each operation must complete before the next one starts. The calling thread 
        waits (or "blocks") until the method finishes.
        Asynchronous methods allow tasks to run in the background. The calling thread does not wait for the operation to finish and 
        can continue executing other code. In C#, asynchronous methods often use the async and await keywords, enabling non-blocking operations 
        (such as I/O or database calls) and improving application responsiveness.
        */

        // *** Synchronous Repository Operations ***

        /// <summary>
        /// Provides a queryable interface to the <typeparamref name="TEntity"/> dataset.
        /// </summary>
        /// <param name="isNoTracking">
        /// If true, disables entity tracking for read-only scenarios, improving performance.
        /// If false, enables tracking for updates.
        /// </param>
        /// <returns>An <see cref="IQueryable{TEntity}"/> representing the dataset.</returns>
        protected virtual IQueryable<TEntity> Query(bool isNoTracking = true)
        {
            return isNoTracking ? _db.Set<TEntity>().AsNoTracking() : _db.Set<TEntity>();
        }

        /// <summary>
        /// Persists changes to the database.
        /// </summary>
        /// <returns>The number of state entries written to the database.</returns>
        // Way 1:
        //protected virtual int Save()
        //{
        //    return _db.SaveChanges();
        //}
        // Way 2:
        protected virtual int Save() => _db.SaveChanges();

        /// <summary>
        /// Adds a new entity to the database.
        /// </summary>
        /// <param name="entity">The entity to be added.</param>
        /// <param name="save">If true, saves changes immediately; otherwise, defers saving.</param>
        protected void Create(TEntity entity, bool save = true)
        {
            entity.Guid = Guid.NewGuid().ToString(); // generate a new guid for the entity that will be inserted
            _db.Set<TEntity>().Add(entity);
            if (save)
                Save();
        }

        /// <summary>
        /// Updates an existing entity in the database.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <param name="save">If true, saves changes immediately; otherwise, defers saving.</param>
        protected void Update(TEntity entity, bool save = true)
        {
            _db.Set<TEntity>().Update(entity);
            if (save)
                Save();
        }

        /// <summary>
        /// Removes an entity from the database.
        /// </summary>
        /// <param name="entity">The entity to remove.</param>
        /// <param name="save">If true, saves changes immediately; otherwise, defers saving.</param>
        protected void Delete(TEntity entity, bool save = true)
        {
            _db.Set<TEntity>().Remove(entity);
            if (save)
                Save();
        }

        // *** ***



        // *** Asynchronous Repository Operations ***

        /// <summary>
        /// Asynchronously saves all pending changes in the current <see cref="DbContext"/> to the database.
        /// </summary>
        /// <param name="cancellationToken">
        /// A token to monitor for cancellation requests, allowing the operation to be cancelled early.
        /// </param>
        /// <returns>The number of state entries written to the database.</returns>
        protected virtual async Task<int> Save(CancellationToken cancellationToken) => await _db.SaveChangesAsync(cancellationToken);

        /// <summary>
        /// Asynchronously adds a new entity to the database context and optionally saves the change.
        /// </summary>
        /// <param name="entity">The entity to be added.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <param name="save">
        /// If <c>true</c>, the method immediately calls <see cref="Save"/> to persist changes.
        /// If <c>false</c>, saving is deferred for batch operations.
        /// </param>
        protected async Task Create(TEntity entity, CancellationToken cancellationToken, bool save = true)
        {
            entity.Guid = Guid.NewGuid().ToString(); // generate a new guid for the entity that will be inserted
            _db.Set<TEntity>().Add(entity);
            if (save)
                await Save(cancellationToken);
        }

        /// <summary>
        /// Asynchronously updates an existing entity in the database context and optionally saves the change.
        /// </summary>
        /// <param name="entity">The entity to be updated.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <param name="save">
        /// If <c>true</c>, the method immediately calls <see cref="Save"/> to persist changes.
        /// If <c>false</c>, saving is deferred for batch operations.
        /// </param>
        protected async Task Update(TEntity entity, CancellationToken cancellationToken, bool save = true)
        {
            _db.Set<TEntity>().Update(entity);
            if (save)
                await Save(cancellationToken);
        }

        /// <summary>
        /// Asynchronously removes an entity from the database context and optionally saves the change.
        /// </summary>
        /// <param name="entity">The entity to be removed.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <param name="save">
        /// If <c>true</c>, the method immediately calls <see cref="Save"/> to persist changes.
        /// If <c>false</c>, saving is deferred for batch operations.
        /// </param>
        protected async Task Delete(TEntity entity, CancellationToken cancellationToken, bool save = true)
        {
            _db.Set<TEntity>().Remove(entity);
            if (save)
                await Save(cancellationToken);
        }

        // *** ***



        // *** Relational Data Operations ***
        /// <summary>
        /// Provides a queryable, read-only interface to the specified related entity type in the database context.
        /// This method is especially useful for performing join operations between the main entity of the service and other related entities.
        /// The returned <see cref="IQueryable{TRelationalEntity}"/> can be used in LINQ queries for join operations
        /// from multiple tables, enabling complex queries such as inner joins and left joins.
        /// </summary>
        /// <typeparam name="TRelationalEntity">
        /// The type of the related entity to query. Must inherit from <see cref="Entity"/> and have a parameterless constructor.
        /// </typeparam>
        /// <returns>
        /// An <see cref="IQueryable{TRelationalEntity}"/> for the specified entity type, with change tracking is disabled for improved read performance.
        /// This can be composed in LINQ queries for join operations with other entity sets retrieved from this method.
        /// </returns>
        public IQueryable<TRelationalEntity> Query<TRelationalEntity>() where TRelationalEntity : Entity, new()
        {
            return _db.Set<TRelationalEntity>().AsNoTracking();
        }

        /// <summary>
        /// Deletes a list of related entities of type <typeparamref name="TRelationalEntity"/> from the database context.
        /// </summary>
        /// <typeparam name="TRelationalEntity">
        /// The type of the related entity to delete. Must inherit from <see cref="Entity"/> and have a parameterless constructor.
        /// </typeparam>
        /// <param name="relationalEntities">The list of related entities to be removed.</param>
        /// <remarks>
        /// This method does not call <c>SaveChanges()</c>, so you must explicitly save the context after calling this method
        /// if you want changes to persist in the database.
        /// </remarks>
        protected void Delete<TRelationalEntity>(List<TRelationalEntity> relationalEntities) where TRelationalEntity : Entity, new()
        {
            _db.Set<TRelationalEntity>().RemoveRange(relationalEntities);
        }

        // *** ***



        /// <summary>
        /// Releases the database context and any unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _db.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}

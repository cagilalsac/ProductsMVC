using Microsoft.EntityFrameworkCore;

namespace APP.Domain
{
    /// <summary>
    /// Represents the Entity Framework Core database context for the Products application domain.
    /// Manages the entity sets and provides configuration for database access.
    /// </summary>
    public class Db : DbContext
    {
        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> representing categories in the database.
        /// Each <see cref="Category"/> entity corresponds to a record in the Categories table.
        /// </summary>
        public DbSet<Category> Categories { get; set; }



        public DbSet<Store> Stores { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductStore> ProductStores { get; set; }



        public DbSet<Group> Groups { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }



        /// <summary>
        /// Initializes a new instance of the <see cref="Db"/> class using the specified options.
        /// </summary>
        /// <param name="options">
        /// The options to be used by the <see cref="DbContext"/>, such as the database provider and connection string (from appsettings.json).
        /// </param>
        public Db(DbContextOptions options) : base(options)
        {
        }
    }
}

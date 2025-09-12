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



        // Overriding OnModelCreating method is optional.
        /// <summary>
        /// Configures the entity relationships and database schema rules for the application domain.
        /// This method defines how entities are related, sets up foreign key constraints, and customizes
        /// the delete behavior for each relationship to prevent cascading deletes.
        /// </summary>
        /// <param name="modelBuilder">
        /// The <see cref="ModelBuilder"/> used to configure entity mappings and relationships.
        /// </param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuration should start with the entities that have the foreign keys.
            modelBuilder.Entity<City>()
                .HasOne(cityEntity => cityEntity.Country) // each City entity has one related Country entity
                .WithMany(countryEntity => countryEntity.Cities) // each Country entity has many related City entities
                .HasForeignKey(cityEntity => cityEntity.CountryId) // the foreign key property in the City entity that
                                                                   // references the primary key of the related Country entity
                .OnDelete(DeleteBehavior.NoAction); // prevents deletion of a Country entity if there are related City entities

            modelBuilder.Entity<UserRole>()
                .HasOne(userRoleEntity => userRoleEntity.User) // each UserRole entity has one related User entity
                .WithMany(userEntity => userEntity.UserRoles) // each User entity has many related UserRole entities
                .HasForeignKey(userRoleEntity => userRoleEntity.UserId) // the foreign key property in the UserRole entity that
                                                                        // references the primary key of the related User entity
                .OnDelete(DeleteBehavior.NoAction); // prevents deletion of a User entity if there are related UserRole entities

            modelBuilder.Entity<UserRole>()
                .HasOne(userRoleEntity => userRoleEntity.Role) // each UserRole entity has one related Role entity
                .WithMany(roleEntity => roleEntity.UserRoles) // each Role entity has many related UserRole entities
                .HasForeignKey(userRoleEntity => userRoleEntity.RoleId) // the foreign key property in the UserRole entity that
                                                                        // references the primary key of the related Role entity
                .OnDelete(DeleteBehavior.NoAction); // prevents deletion of a Role entity if there are related UserRole entities

            modelBuilder.Entity<User>()
                .HasOne(userEntity => userEntity.Country) // each User entity has one related Country entity
                .WithMany(countryEntity => countryEntity.Users) // each Country entity has many related User entities
                .HasForeignKey(userEntity => userEntity.CountryId) // the foreign key property in the User entity that
                                                                   // references the primary key of the related Country entity
                .OnDelete(DeleteBehavior.NoAction); // prevents deletion of a Country entity if there are related User entities

            modelBuilder.Entity<User>()
                .HasOne(userEntity => userEntity.City) // each User entity has one related City entity
                .WithMany(cityEntity => cityEntity.Users) // each City entity has many related User entities
                .HasForeignKey(userEntity => userEntity.CityId) // the foreign key property in the User entity that
                                                                // references the primary key of the related City entity
                .OnDelete(DeleteBehavior.NoAction); // prevents deletion of a City entity if there are related User entities

            modelBuilder.Entity<User>()
                .HasOne(userEntity => userEntity.Group) // each User entity has one related Group entity
                .WithMany(groupEntity => groupEntity.Users) // each Group entity has many related User entities
                .HasForeignKey(userEntity => userEntity.GroupId) // the foreign key property in the User entity that
                                                                 // references the primary key of the related Group entity
                .OnDelete(DeleteBehavior.NoAction); // prevents deletion of a Group entity if there are related User entities

            modelBuilder.Entity<ProductStore>()
                .HasOne(productStoreEntity => productStoreEntity.Product) // each ProductStore entity has one related Product entity
                .WithMany(productEntity => productEntity.ProductStores) // each Product entity has many related ProductStore entities
                .HasForeignKey(productStoreEntity => productStoreEntity.ProductId) // the foreign key property in the ProductStore entity that
                                                                                   // references the primary key of the related Product entity
                .OnDelete(DeleteBehavior.NoAction); // prevents deletion of a Product entity if there are related ProductStore entities

            modelBuilder.Entity<ProductStore>()
                .HasOne(productStoreEntity => productStoreEntity.Store) // each ProductStore entity has one related Store entity
                .WithMany(storeEntity => storeEntity.ProductStores) // each Store entity has many related ProductStore entities
                .HasForeignKey(productStoreEntity => productStoreEntity.StoreId) // the foreign key property in the ProductStore entity that
                                                                                 // references the primary key of the related Store entity
                .OnDelete(DeleteBehavior.NoAction); // prevents deletion of a Store entity if there are related ProductStore entities

            modelBuilder.Entity<Product>()
                .HasOne(productEntity => productEntity.Category) // each Product entity has one related Category entity
                .WithMany(categoryEntity => categoryEntity.Products) // each Category entity has many related Product entities
                .HasForeignKey(productEntity => productEntity.CategoryId) // the foreign key property in the Product entity that
                                                                          // references the primary key of the related Category entity
                .OnDelete(DeleteBehavior.NoAction); // prevents deletion of a Category entity if there are related Product entities
        }
    }
}

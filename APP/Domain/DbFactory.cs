using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace APP.Domain
{
    /// <summary>
    /// Provides a factory for creating <see cref="Db"/> instances at design time.
    /// This is used by Entity Framework Core tools (such as migrations) to construct the database context
    /// when the application is not running.
    /// This class should be created if there are any exceptions during scaffolding.
    /// </summary>
    public class DbFactory : IDesignTimeDbContextFactory<Db>
    {
        /// <summary>
        /// The connection string in the configuration file (appsettings.json).
        /// </summary>
        const string CONNECTIONSTRING = "data source=ProductsDB";

        /// <summary>
        /// Creates a new instance of the <see cref="Db"/> context using the connection string.
        /// This method is called by EF Core tooling at design time.
        /// </summary>
        /// <param name="args">Command-line arguments (not used).</param>
        /// <returns>A configured <see cref="Db"/> instance.</returns>
        public Db CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<Db>();
            optionsBuilder.UseSqlite(CONNECTIONSTRING);
            return new Db(optionsBuilder.Options);
        }
    }
}

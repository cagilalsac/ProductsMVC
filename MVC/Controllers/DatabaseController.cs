using APP.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;

namespace MVC.Controllers
{
    public class DatabaseController : Controller
    {
        // Field to reference the injected DbContext instance.
        private readonly Db _db;

        // Field to reference the injected IWebHostEnvironment instance which can be development, staging or production.
        private readonly IWebHostEnvironment _environment;

        // Constructor that accepts DbContext and IWebHostEnvironment instances via dependency injection.
        public DatabaseController(Db db, IWebHostEnvironment environment)
        {
            _db = db;
            _environment = environment;
        }

        // Action method to seed the database with initial data.
        [Route("SeedDb")] // will change the route of the action to /SeedDb instead of /Database/Seed
        public IActionResult Seed()
        {
            // If the running application's environment is not development, prevent seeding initial data to the database
            // and return error HTML content.
            if (!_environment.IsDevelopment())
                return Content("<label style='color:red;'>The seed operation can only be performed in development environment!</label>",
                                "text/html", Encoding.UTF8);

            // Remove existing data
            var productStores = _db.ProductStores.ToList();
            _db.ProductStores.RemoveRange(productStores);

            var stores = _db.Stores.ToList();
            _db.Stores.RemoveRange(stores);

            var products = _db.Products.ToList();
            _db.Products.RemoveRange(products);

            var categories = _db.Categories.ToList();
            _db.Categories.RemoveRange(categories);

            // Reset identity columns (for SQLite), IDs will start from 1
            _db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='ProductStores'");
            _db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='Stores'");
            _db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='Products'");
            _db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='Categories'");

            // Add stores as a list
            var storeList = new List<Store>
            {
                new Store { Guid = Guid.NewGuid().ToString(), Name = "Migros", IsVirtual = false },
                new Store { Guid = Guid.NewGuid().ToString(), Name = "Trendyol", IsVirtual = true },
                new Store { Guid = Guid.NewGuid().ToString(), Name = "MediaMarkt", IsVirtual = false },
                new Store { Guid = Guid.NewGuid().ToString(), Name = "LC Waikiki", IsVirtual = false },
                new Store { Guid = Guid.NewGuid().ToString(), Name = "Ilac Sepeti", IsVirtual = false },
                new Store { Guid = Guid.NewGuid().ToString(), Name = "Hepsiburada", IsVirtual = true }
            };
            _db.Stores.AddRange(storeList);

            _db.SaveChanges();

            // Add categories and products
            _db.Categories.Add(
                new Category
                {
                    Guid = Guid.NewGuid().ToString(),
                    Title = "Electronics",
                    Description = "Latest technology products and gadgets.",
                    Products = new List<Product>
                    {
                        new Product
                        {
                            Guid = Guid.NewGuid().ToString(),
                            Name = "Laptop",
                            UnitPrice = 15000m,
                            StockAmount = 25,
                            ProductStores = new List<ProductStore>
                            {
                                new ProductStore { StoreId = _db.Stores.Single(s => s.Name == "MediaMarkt").Id },
                                new ProductStore { StoreId = _db.Stores.Single(s => s.Name == "Hepsiburada").Id }
                            }
                        },
                        new Product
                        {
                            Guid = Guid.NewGuid().ToString(),
                            Name = "Smartphone",
                            UnitPrice = 8000m,
                            StockAmount = 50,
                            ProductStores = new List<ProductStore>
                            {
                                new ProductStore { StoreId = _db.Stores.Single(s => s.Name == "Trendyol").Id },
                                new ProductStore { StoreId = _db.Stores.Single(s => s.Name == "MediaMarkt").Id },
                                new ProductStore { StoreId = _db.Stores.Single(s => s.Name == "Hepsiburada").Id }
                            }
                        },
                        new Product
                        {
                            Guid = Guid.NewGuid().ToString(),
                            Name = "Headphones",
                            UnitPrice = 1200m,
                            StockAmount = 100,
                            ProductStores = new List<ProductStore>
                            {
                                new ProductStore { StoreId = _db.Stores.Single(s => s.Name == "MediaMarkt").Id }
                            }
                        }
                    }
                });
            _db.Categories.Add(
                new Category
                {
                    Guid = Guid.NewGuid().ToString(),
                    Title = "Home Appliances",
                    Description = "Kitchen and household appliances.",
                    Products = new List<Product>
                    {
                        new Product
                        {
                            Guid = Guid.NewGuid().ToString(),
                            Name = "Microwave Oven",
                            UnitPrice = 3500m
                        },
                        new Product
                        {
                            Guid = Guid.NewGuid().ToString(),
                            Name = "Refrigerator",
                            UnitPrice = 12000m,
                            StockAmount = 15,
                            ProductStores = new List<ProductStore>
                            {
                                new ProductStore { StoreId = _db.Stores.Single(s => s.Name == "Migros").Id }
                            }
                        },
                        new Product
                        {
                            Guid = Guid.NewGuid().ToString(),
                            Name = "Washing Machine",
                            UnitPrice = 9500m,
                            StockAmount = 20,
                            ProductStores = new List<ProductStore>
                            {
                                new ProductStore { StoreId = _db.Stores.Single(s => s.Name == "Migros").Id },
                                new ProductStore { StoreId = _db.Stores.Single(s => s.Name == "Hepsiburada").Id }
                            }
                        }
                    }
                });
            _db.Categories.Add(
                new Category
                {
                    Guid = Guid.NewGuid().ToString(),
                    Title = "Clothing",
                    Description = "Men's, women's, and children's clothing.",
                    Products = new List<Product>
                    {
                        new Product
                        {
                            Guid = Guid.NewGuid().ToString(),
                            Name = "T-shirt",
                            UnitPrice = 150m,
                            StockAmount = 200,
                            ProductStores = new List<ProductStore>
                            {
                                new ProductStore { StoreId = _db.Stores.Single(s => s.Name == "LC Waikiki").Id }
                            }
                        },
                        new Product
                        {
                            Guid = Guid.NewGuid().ToString(),
                            Name = "Jeans",
                            UnitPrice = 350m,
                            StockAmount = 150,
                            ProductStores = new List<ProductStore>
                            {
                                new ProductStore { StoreId = _db.Stores.Single(s => s.Name == "LC Waikiki").Id },
                                new ProductStore { StoreId = _db.Stores.Single(s => s.Name == "Trendyol").Id }
                            }
                        },
                        new Product
                        {
                            Guid = Guid.NewGuid().ToString(),
                            Name = "Winter Jacket",
                            UnitPrice = 750m,
                            StockAmount = 80,
                            ProductStores = new List<ProductStore>
                            {
                                new ProductStore { StoreId = _db.Stores.Single(s => s.Name == "LC Waikiki").Id },
                                new ProductStore { StoreId = _db.Stores.Single(s => s.Name == "Hepsiburada").Id },
                                new ProductStore { StoreId = _db.Stores.Single(s => s.Name == "Trendyol").Id }
                            }
                        }
                    }
                });
            _db.Categories.Add(
                new Category
                {
                    Guid = Guid.NewGuid().ToString(),
                    Title = "Medicine",
                    Description = "Pharmaceutical products and medication.",
                    Products = new List<Product>
                    {
                        new Product
                        {
                            Guid = Guid.NewGuid().ToString(),
                            Name = "Pain Killer",
                            UnitPrice = 35m,
                            StockAmount = 300,
                            ExpirationDate = DateTime.Parse("09/19/2035 00:00:00", new CultureInfo("en-US")), // 00:00:00 may not be written
                            ProductStores = new List<ProductStore>
                            {
                                new ProductStore { StoreId = _db.Stores.Single(s => s.Name == "Ilac Sepeti").Id }
                            }
                        },
                        new Product
                        {
                            Guid = Guid.NewGuid().ToString(),
                            Name = "Vitamin",
                            UnitPrice = 85m,
                            StockAmount = 200,
                            ExpirationDate = new DateTime(2033, 10, 29), // hours, minutes and seconds parameters may also be added
                            ProductStores = new List<ProductStore>
                            {
                                new ProductStore { StoreId = _db.Stores.Single(s => s.Name == "Ilac Sepeti").Id },
                                new ProductStore { StoreId = _db.Stores.Single(s => s.Name == "Migros").Id }
                            }
                        }
                    }
                });

            _db.SaveChanges();

            // Return a success message as HTML content
            return Content("<label style='color:green;'>Database seed successful.</label>", "text/html", Encoding.UTF8);
        }
    }
}

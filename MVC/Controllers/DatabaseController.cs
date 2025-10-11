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
            // Can be uncommented to check if the running application's environment is not development, prevent seeding initial data to the database
            // and return error HTML content.
            //if (!_environment.IsDevelopment())
            //    return Content("<label style='color:red;'>The seed operation can only be performed in development environment!</label>",
            //                    "text/html", Encoding.UTF8);

            // Remove existing data
            var productStores = _db.ProductStores.ToList();
            _db.ProductStores.RemoveRange(productStores);

            var stores = _db.Stores.ToList();
            _db.Stores.RemoveRange(stores);

            var products = _db.Products.ToList();
            _db.Products.RemoveRange(products);

            var categories = _db.Categories.ToList();
            _db.Categories.RemoveRange(categories);

            var userRoles = _db.UserRoles.ToList();
            _db.UserRoles.RemoveRange(userRoles);

            var roles = _db.Roles.ToList();
            _db.Roles.RemoveRange(roles);

            var users = _db.Users.ToList();
            _db.Users.RemoveRange(users);

            var groups = _db.Groups.ToList();
            _db.Groups.RemoveRange(groups);

            var cities = _db.Cities.ToList();
            _db.Cities.RemoveRange(cities);

            var countries = _db.Countries.ToList();
            _db.Countries.RemoveRange(countries);

            // Reset identity columns (for SQLite), IDs will start from 1
            _db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='ProductStores'");
            _db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='Stores'");
            _db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='Products'");
            _db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='Categories'");
            _db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='UserRoles'");
            _db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='Roles'");
            _db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='Users'");
            _db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='Groups'");
            _db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='Cities'");
            _db.Database.ExecuteSqlRaw("UPDATE SQLITE_SEQUENCE SET SEQ=0 WHERE NAME='Countries'");

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

            // Add default roles
            _db.Roles.Add(new Role()
            {
                Name = "Admin"
            });
            _db.Roles.Add(new Role()
            {
                Name = "User"
            });

            _db.SaveChanges();

            // Add a default group with two users: an admin and a regular user
            _db.Groups.Add(new Group()
            {
                Title = "General",
                Users = new List<User>()
                {
                    new User()
                    {
                        Address = "Çankaya",
                        BirthDate = new DateTime(1980, 8, 21),
                        CityId = 6,
                        CountryId = 1,
                        FirstName = "Çağıl",
                        Gender = Genders.Man,
                        Guid = Guid.NewGuid().ToString(),
                        IsActive = true,
                        LastName = "Alsaç",
                        Password = "admin",
                        RegistrationDate = DateTime.UtcNow,
                        Score = 3.8M,
                        UserName = "admin",
                        UserRoles = new List<UserRole>()
                        {
                            // Assign Admin role to this user
                            new UserRole() { RoleId = _db.Roles.SingleOrDefault(r => r.Name == "Admin").Id }
                        }
                    },
                    new User()
                    {
                        BirthDate = DateTime.Parse("09/13/2004", new CultureInfo("en-US")),
                        CityId = 82,
                        CountryId = 2,
                        FirstName = "Luna",
                        Gender = Genders.Woman,
                        Guid = Guid.NewGuid().ToString(),
                        IsActive = true,
                        LastName = "Leo",
                        Password = "user",
                        RegistrationDate = DateTime.UtcNow,
                        Score = 4.7m,
                        UserName = "user",
                        UserRoles = new List<UserRole>()
                        {
                            // Assign User role to this user
                            new UserRole() { RoleId = _db.Roles.SingleOrDefault(r => r.Name == "User").Id }
                        }
                    },
                }
            });

            // Add a new Country entity for Türkiye, including all 81 cities as child entities.
            _db.Countries.Add(new Country
            {
                Guid = Guid.NewGuid().ToString(),
                CountryName = "Türkiye",
                Cities = new List<City>
                {
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Adana" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Adıyaman" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Afyonkarahisar" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Ağrı" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Amasya" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Ankara" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Antalya" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Artvin" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Aydın" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Balıkesir" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Bilecik" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Bingöl" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Bitlis" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Bolu" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Burdur" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Bursa" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Çanakkale" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Çankırı" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Çorum" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Denizli" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Diyarbakır" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Edirne" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Elazığ" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Erzincan" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Erzurum" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Eskişehir" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Gaziantep" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Giresun" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Gümüşhane" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Hakkari" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Hatay" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Isparta" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Mersin" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "İstanbul" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "İzmir" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Kars" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Kastamonu" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Kayseri" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Kırklareli" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Kırşehir" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Kocaeli" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Konya" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Kütahya" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Malatya" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Manisa" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Kahramanmaraş" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Mardin" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Muğla" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Muş" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Nevşehir" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Niğde" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Ordu" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Rize" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Sakarya" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Samsun" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Siirt" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Sinop" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Sivas" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Tekirdağ" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Tokat" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Trabzon" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Tunceli" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Şanlıurfa" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Uşak" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Van" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Yozgat" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Zonguldak" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Aksaray" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Bayburt" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Karaman" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Kırıkkale" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Batman" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Şırnak" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Bartın" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Ardahan" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Iğdır" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Yalova" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Karabük" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Kilis" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Osmaniye" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Düzce" }
                }
            });

            // Add a new Country entity for the United States of America, including a representative list of major cities.
            _db.Countries.Add(new Country
            {
                Guid = Guid.NewGuid().ToString(),
                CountryName = "United States of America",
                Cities = new List<City>
                {
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "New York" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Los Angeles" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Chicago" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Houston" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Phoenix" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Philadelphia" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "San Antonio" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "San Diego" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Dallas" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "San Jose" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Austin" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Jacksonville" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Fort Worth" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Columbus" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Charlotte" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "San Francisco" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Indianapolis" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Seattle" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Denver" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Washington" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Boston" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "El Paso" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Nashville" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Detroit" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Oklahoma City" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Portland" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Las Vegas" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Memphis" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Louisville" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Baltimore" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Milwaukee" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Albuquerque" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Tucson" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Fresno" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Mesa" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Sacramento" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Atlanta" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Kansas City" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Colorado Springs" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Miami" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Raleigh" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Omaha" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Long Beach" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Virginia Beach" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Oakland" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Minneapolis" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Tulsa" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "Arlington" },
                    new City { Guid = Guid.NewGuid().ToString(), CityName = "New Orleans" }
                }
            });

            // Add a new Country entity for China with no cities.
            _db.Countries.Add(new Country
            {
                Guid = Guid.NewGuid().ToString(),
                CountryName = "China",
            });

            _db.SaveChanges();

            // Clean up wwwroot/files directory by deleting all files in it.
            var files = Directory.GetFiles(Path.Combine("wwwroot", "files"));
            foreach (var file in files)
            {
                System.IO.File.Delete(file);
            }

            // Return a success message as HTML content
            return Content("<label style='color:green;'>Database seed successful.</label>", "text/html", Encoding.UTF8);
        }
    }
}
